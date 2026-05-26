using System.Globalization;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal static class ExternalProviderBulkSaveAssertions {
  private const int PairCount = 20;
  private const int ExpectedRowsWritten = PairCount * 3 + 3;
  private const int ExpectedSavedRecordCount = PairCount * 3 + 4;

  private static readonly DateTimeOffset HubLoadTimestamp = new(2026, 5, 18, 10, 0, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset LinkLoadTimestamp = new(2026, 5, 18, 10, 5, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset FirstSatelliteLoadTimestamp = new(2026, 5, 18, 10, 10, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset UnchangedSatelliteLoadTimestamp = new(2026, 5, 18, 10, 15, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset ChangedSatelliteLoadTimestamp = new(2026, 5, 18, 10, 20, 0, TimeSpan.Zero);

  public static async Task AssertProviderBulkSaveAsync(
      Func<Task<ExternalProviderLiveSchemaFixture>> createFixtureAsync,
      Action<IServiceCollection> configureProviderServices,
      string expectedStrategyName,
      Action<DataVaultBulkSaveRequest, DataVaultDiagnosticsResult>? assertProviderBoundary = null) {
    ArgumentNullException.ThrowIfNull(createFixtureAsync);
    ArgumentNullException.ThrowIfNull(configureProviderServices);
    ArgumentException.ThrowIfNullOrWhiteSpace(expectedStrategyName);

    await using var fixture = await createFixtureAsync().ConfigureAwait(false);
    var services = new ServiceCollection();
    configureProviderServices(services);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var scenario = CreateBulkScenario(provider);

    await using var context = fixture.CreateContext();
    var diagnosticResult = diagnostics.Analyze(context, scenario.Request);

    AssertProviderStrategySelected(diagnosticResult, expectedStrategyName);
    assertProviderBoundary?.Invoke(scenario.Request, diagnosticResult);

    var result = await saveService.SaveAsync(context, scenario.Request).ConfigureAwait(false);

    Assert.Equal(ExpectedRowsWritten, result.RowsWritten);
    Assert.Equal(ExpectedSavedRecordCount, result.SavedRecords.Count);
    AssertSavedRecordOrder(result, scenario);
    Assert.Empty(context.ChangeTracker.Entries());

    await AssertPersistedRowsAsync(context, scenario).ConfigureAwait(false);
  }

  public static async Task AssertProviderBulkSaveFailureRollsBackAsync(
      Func<Task<ExternalProviderLiveSchemaFixture>> createFixtureAsync,
      Action<IServiceCollection> configureProviderServices,
      string expectedStrategyName,
      Action<DataVaultBulkSaveRequest, DataVaultDiagnosticsResult>? assertProviderBoundary = null,
      Func<DbContext, Task>? assertAfterFailureAsync = null) {
    ArgumentNullException.ThrowIfNull(createFixtureAsync);
    ArgumentNullException.ThrowIfNull(configureProviderServices);
    ArgumentException.ThrowIfNullOrWhiteSpace(expectedStrategyName);

    await using var fixture = await createFixtureAsync().ConfigureAwait(false);
    var services = new ServiceCollection();
    configureProviderServices(services);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var request = CreateRollbackFailureScenario(provider);

    await using var context = fixture.CreateContext();
    var diagnosticResult = diagnostics.Analyze(context, request);

    AssertProviderStrategySelected(diagnosticResult, expectedStrategyName);
    assertProviderBoundary?.Invoke(request, diagnosticResult);

    await Assert.ThrowsAnyAsync<Exception>(() => saveService.SaveAsync(context, request)).ConfigureAwait(false);
    Assert.Empty(context.ChangeTracker.Entries());
    if (assertAfterFailureAsync is not null) {
      await assertAfterFailureAsync(context).ConfigureAwait(false);
    }

    await using var verificationContext = fixture.CreateContext();
    Assert.Equal(
        0,
        await verificationContext.Set<Dictionary<string, object>>("HubCustomer")
            .AsNoTracking()
            .CountAsync()
            .ConfigureAwait(false));
    Assert.Equal(
        0,
        await verificationContext.Set<Dictionary<string, object>>("SatCustomerContact")
            .AsNoTracking()
            .CountAsync()
            .ConfigureAwait(false));
  }

  private static BulkScenario CreateBulkScenario(IServiceProvider provider) {
    var metadataModel = LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel();
    var customer = metadataModel.Hubs.Single(hub => hub.Name == LiveSchemaReaderContractFixture.CustomerHubName);
    var order = metadataModel.Hubs.Single(hub => hub.Name == LiveSchemaReaderContractFixture.OrderHubName);
    var customerOrder = metadataModel.Links.Single(link => link.Name == LiveSchemaReaderContractFixture.CustomerOrderLinkName);
    var contact = metadataModel.Satellites.Single(satellite => satellite.Name == LiveSchemaReaderContractFixture.ContactSatelliteName);
    var state = metadataModel.Satellites.Single(satellite => satellite.Name == LiveSchemaReaderContractFixture.StateSatelliteName);
    var customerIds = Enumerable.Range(0, PairCount)
        .Select(index => "C-BULK-" + index.ToString("000", CultureInfo.InvariantCulture))
        .ToArray();
    var orderIds = Enumerable.Range(0, PairCount)
        .Select(index => "O-BULK-" + index.ToString("000", CultureInfo.InvariantCulture))
        .ToArray();
    var customerHashKeys = customerIds
        .Select(customerId => ComputeHash(provider, [new("Customer Id", customerId)]))
        .ToArray();
    var orderHashKeys = orderIds
        .Select(orderId => ComputeHash(provider, [new("Order Id", orderId)]))
        .ToArray();
    var linkHashKeys = Enumerable.Range(0, PairCount)
        .Select(index => ComputeHash(
            provider,
            [
                new("Customer", customerHashKeys[index]),
                new("Order", orderHashKeys[index]),
            ]))
        .ToArray();
    var hubRequest = new DataVaultSaveRequest(
        HubLoadTimestamp,
        "bulk-hubs",
        customerIds
            .Select(customerId => new DataVaultHubSaveOperation(customer, [new("Customer Id", customerId)]))
            .Concat(orderIds.Select(orderId => new DataVaultHubSaveOperation(order, [new("Order Id", orderId)])))
            .ToArray(),
        []);
    var linkRequest = new DataVaultSaveRequest(
        LinkLoadTimestamp,
        "bulk-links",
        [],
        Enumerable.Range(0, PairCount)
            .Select(index => new DataVaultLinkSaveOperation(
                customerOrder,
                [
                    new("Customer", customerHashKeys[index]),
                    new("Order", orderHashKeys[index]),
                ]))
            .ToArray());
    var firstSatelliteRequest = new DataVaultSaveRequest(
        FirstSatelliteLoadTimestamp,
        "bulk-satellite-first",
        [],
        [],
        [
            new DataVaultSatelliteSaveOperation(
                contact,
                customerHashKeys[0],
                [new("Email Address", "bulk-first@example.test")],
                "contact-hash-1"),
            new DataVaultSatelliteSaveOperation(
                state,
                linkHashKeys[0],
                [new("State Code", "PLACED")],
                "state-hash-1"),
        ]);
    var unchangedSatelliteRequest = new DataVaultSaveRequest(
        UnchangedSatelliteLoadTimestamp,
        "bulk-satellite-replay",
        [],
        [],
        [
            new DataVaultSatelliteSaveOperation(
                contact,
                customerHashKeys[0],
                [new("Email Address", "bulk-replay@example.test")],
                "contact-hash-1"),
        ]);
    var changedSatelliteRequest = new DataVaultSaveRequest(
        ChangedSatelliteLoadTimestamp,
        "bulk-satellite-change",
        [],
        [],
        [
            new DataVaultSatelliteSaveOperation(
                contact,
                customerHashKeys[0],
                [new("Email Address", "bulk-changed@example.test")],
                "contact-hash-2"),
        ]);

    return new BulkScenario(
        new DataVaultBulkSaveRequest(
            [
                hubRequest,
                linkRequest,
                firstSatelliteRequest,
                unchangedSatelliteRequest,
                changedSatelliteRequest,
            ]),
        customerIds,
        orderIds,
        customerHashKeys,
        orderHashKeys,
        linkHashKeys);
  }

  private static DataVaultBulkSaveRequest CreateRollbackFailureScenario(IServiceProvider provider) {
    var metadataModel = LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel();
    var customer = metadataModel.Hubs.Single(hub => hub.Name == LiveSchemaReaderContractFixture.CustomerHubName);
    var contact = metadataModel.Satellites.Single(satellite => satellite.Name == LiveSchemaReaderContractFixture.ContactSatelliteName);
    var customerIds = Enumerable.Range(0, 60)
        .Select(index => "C-ROLLBACK-" + index.ToString("000", CultureInfo.InvariantCulture))
        .ToArray();
    var customerHashKeys = customerIds
        .Select(customerId => ComputeHash(provider, [new("Customer Id", customerId)]))
        .ToArray();
    var hubRequest = new DataVaultSaveRequest(
        HubLoadTimestamp,
        "bulk-rollback-hubs",
        customerIds
            .Select(customerId => new DataVaultHubSaveOperation(customer, [new("Customer Id", customerId)]))
            .ToArray(),
        []);
    var duplicateSatelliteRequest = new DataVaultSaveRequest(
        FirstSatelliteLoadTimestamp,
        "bulk-rollback-duplicate-satellite",
        [],
        [],
        [
            new DataVaultSatelliteSaveOperation(
                contact,
                customerHashKeys[0],
                [new("Email Address", "rollback-first@example.test")],
                "rollback-contact-hash-1"),
            new DataVaultSatelliteSaveOperation(
                contact,
                customerHashKeys[0],
                [new("Email Address", "rollback-second@example.test")],
                "rollback-contact-hash-2"),
        ]);

    return new DataVaultBulkSaveRequest([hubRequest, duplicateSatelliteRequest]);
  }

  private static void AssertProviderStrategySelected(
      DataVaultDiagnosticsResult diagnostics,
      string expectedStrategyName) {
    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected, diagnostics.SaveStrategy.Status);
    Assert.Equal(expectedStrategyName, diagnostics.SaveStrategy.SelectedStrategyName);
    Assert.Empty(diagnostics.SaveStrategy.FallbackCauses);
    Assert.Contains(
        diagnostics.SaveStrategy.Candidates,
        candidate => string.Equals(candidate.StrategyName, expectedStrategyName, StringComparison.Ordinal) &&
            candidate.CanSave);
  }

  private static async Task AssertPersistedRowsAsync(
      ExternalProviderLiveSchemaContext context,
      BulkScenario scenario) {
    var customerRows = await context.Set<Dictionary<string, object>>("HubCustomer")
        .AsNoTracking()
        .ToListAsync()
        .ConfigureAwait(false);
    var orderRows = await context.Set<Dictionary<string, object>>("HubOrder")
        .AsNoTracking()
        .ToListAsync()
        .ConfigureAwait(false);
    var linkRows = await context.Set<Dictionary<string, object>>("LinkCustomerOrder")
        .AsNoTracking()
        .ToListAsync()
        .ConfigureAwait(false);
    var contactRows = await context.Set<Dictionary<string, object>>("SatCustomerContact")
        .AsNoTracking()
        .ToListAsync()
        .ConfigureAwait(false);
    var stateRows = await context.Set<Dictionary<string, object>>("SatCustomerOrderState")
        .AsNoTracking()
        .ToListAsync()
        .ConfigureAwait(false);

    Assert.Equal(PairCount, customerRows.Count);
    Assert.Equal(PairCount, orderRows.Count);
    Assert.Equal(PairCount, linkRows.Count);
    Assert.Equal(2, contactRows.Count);
    Assert.Single(stateRows);

    var firstCustomerRow = SingleRow(customerRows, "CustomerHashKey", scenario.CustomerHashKeys[0]);
    var firstOrderRow = SingleRow(orderRows, "OrderHashKey", scenario.OrderHashKeys[0]);
    var firstLinkRow = SingleRow(linkRows, "CustomerOrderHashKey", scenario.LinkHashKeys[0]);

    Assert.Equal(scenario.CustomerIds[0], ReadString(firstCustomerRow, "CustomerId"));
    Assert.Equal(scenario.OrderIds[0], ReadString(firstOrderRow, "OrderId"));
    Assert.Equal("bulk-hubs", ReadString(firstCustomerRow, "RecordSource"));
    Assert.Equal("bulk-hubs", ReadString(firstOrderRow, "RecordSource"));
    Assert.Equal(HubLoadTimestamp, ReadLoadTimestamp(firstCustomerRow));
    Assert.Equal(HubLoadTimestamp, ReadLoadTimestamp(firstOrderRow));
    Assert.Equal(scenario.CustomerHashKeys[0], ReadString(firstLinkRow, "CustomerHashKey"));
    Assert.Equal(scenario.OrderHashKeys[0], ReadString(firstLinkRow, "OrderHashKey"));
    Assert.Equal("bulk-links", ReadString(firstLinkRow, "RecordSource"));
    Assert.Equal(LinkLoadTimestamp, ReadLoadTimestamp(firstLinkRow));

    var orderedContactRows = contactRows
        .OrderBy(ReadLoadTimestamp)
        .ToArray();

    AssertSatelliteRow(
        orderedContactRows[0],
        "CustomerHashKey",
        scenario.CustomerHashKeys[0],
        "EmailAddress",
        "bulk-first@example.test",
        "contact-hash-1",
        FirstSatelliteLoadTimestamp,
        "bulk-satellite-first");
    AssertSatelliteRow(
        orderedContactRows[1],
        "CustomerHashKey",
        scenario.CustomerHashKeys[0],
        "EmailAddress",
        "bulk-changed@example.test",
        "contact-hash-2",
        ChangedSatelliteLoadTimestamp,
        "bulk-satellite-change");
    Assert.DoesNotContain(
        contactRows,
        row => string.Equals(ReadString(row, "EmailAddress"), "bulk-replay@example.test", StringComparison.Ordinal));

    AssertSatelliteRow(
        Assert.Single(stateRows),
        "CustomerOrderHashKey",
        scenario.LinkHashKeys[0],
        "StateCode",
        "PLACED",
        "state-hash-1",
        FirstSatelliteLoadTimestamp,
        "bulk-satellite-first");
  }

  private static void AssertSavedRecordOrder(DataVaultSaveResult result, BulkScenario scenario) {
    Assert.Equal(
        Enumerable.Repeat(DataVaultTableKind.Hub, PairCount * 2)
            .Concat(Enumerable.Repeat(DataVaultTableKind.Link, PairCount))
            .Concat(Enumerable.Repeat(DataVaultTableKind.Satellite, 4))
            .ToArray(),
        result.SavedRecords.Select(record => record.Kind).ToArray());
    AssertSavedRecord(
        result.SavedRecords[0],
        DataVaultTableKind.Hub,
        LiveSchemaReaderContractFixture.CustomerHubName,
        "HubCustomer",
        scenario.CustomerHashKeys[0]);
    AssertSavedRecord(
        result.SavedRecords[PairCount],
        DataVaultTableKind.Hub,
        LiveSchemaReaderContractFixture.OrderHubName,
        "HubOrder",
        scenario.OrderHashKeys[0]);
    AssertSavedRecord(
        result.SavedRecords[PairCount * 2],
        DataVaultTableKind.Link,
        LiveSchemaReaderContractFixture.CustomerOrderLinkName,
        "LinkCustomerOrder",
        scenario.LinkHashKeys[0]);

    var satelliteRecords = result.SavedRecords.Skip(PairCount * 3).ToArray();
    Assert.Collection(
        satelliteRecords,
        record => AssertSavedRecord(
            record,
            DataVaultTableKind.Satellite,
            LiveSchemaReaderContractFixture.ContactSatelliteName,
            "SatCustomerContact",
            scenario.CustomerHashKeys[0]),
        record => AssertSavedRecord(
            record,
            DataVaultTableKind.Satellite,
            LiveSchemaReaderContractFixture.StateSatelliteName,
            "SatCustomerOrderState",
            scenario.LinkHashKeys[0]),
        record => AssertSavedRecord(
            record,
            DataVaultTableKind.Satellite,
            LiveSchemaReaderContractFixture.ContactSatelliteName,
            "SatCustomerContact",
            scenario.CustomerHashKeys[0]),
        record => AssertSavedRecord(
            record,
            DataVaultTableKind.Satellite,
            LiveSchemaReaderContractFixture.ContactSatelliteName,
            "SatCustomerContact",
            scenario.CustomerHashKeys[0]));
  }

  private static void AssertSavedRecord(
      DataVaultSavedRecord record,
      DataVaultTableKind kind,
      string metadataName,
      string tableName,
      string hashKey) {
    Assert.Equal(kind, record.Kind);
    Assert.Equal(metadataName, record.MetadataName);
    Assert.Equal(tableName, record.TableName);
    Assert.Equal(hashKey, record.HashKey);
  }

  private static void AssertSatelliteRow(
      Dictionary<string, object> row,
      string parentHashKeyColumnName,
      string parentHashKey,
      string payloadColumnName,
      string payloadValue,
      string hashDiff,
      DateTimeOffset loadTimestamp,
      string recordSource) {
    Assert.Equal(parentHashKey, ReadString(row, parentHashKeyColumnName));
    Assert.Equal(payloadValue, ReadString(row, payloadColumnName));
    Assert.Equal(hashDiff, ReadString(row, "HashDiff"));
    Assert.Equal(loadTimestamp, ReadLoadTimestamp(row));
    Assert.Equal(recordSource, ReadString(row, "RecordSource"));
  }

  private static Dictionary<string, object> SingleRow(
      IEnumerable<Dictionary<string, object>> rows,
      string columnName,
      string value) {
    return Assert.Single(rows.Where(row => string.Equals(ReadString(row, columnName), value, StringComparison.Ordinal)));
  }

  private static string ReadString(Dictionary<string, object> row, string columnName) {
    return Convert.ToString(row[columnName], CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("Expected column '" + columnName + "' to contain a non-null value.");
  }

  private static DateTimeOffset ReadLoadTimestamp(Dictionary<string, object> row) {
    return DataVaultLoadTimestampValueConverter.ReadProviderValue(row["LoadTimestamp"]);
  }

  private static string ComputeHash(
      IServiceProvider provider,
      IEnumerable<KeyValuePair<string, string>> fields) {
    var normalizer = provider.GetRequiredService<IStableHashNormalizer>();
    var hashService = provider.GetRequiredService<IStableHashService>();
    var normalized = normalizer.NormalizeFields(fields.Select(field => new KeyValuePair<string, object?>(field.Key, field.Value)));

    return hashService.ComputeHash(normalized).Value;
  }

  private sealed record BulkScenario(
      DataVaultBulkSaveRequest Request,
      IReadOnlyList<string> CustomerIds,
      IReadOnlyList<string> OrderIds,
      IReadOnlyList<string> CustomerHashKeys,
      IReadOnlyList<string> OrderHashKeys,
      IReadOnlyList<string> LinkHashKeys);
}
