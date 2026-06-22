using System.Globalization;
using System.Diagnostics;
using System.Data;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
public sealed class SqlServerDataVaultSmokeTests {
  private const int StagedBulkHubOnlyCount = 50;
  private const int StagedBulkPairCount = 25;

  [Fact]
  public async Task AddDVaultSqlServerPersistsRepresentativeHubSaveWhenConfigured() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 4, 9, 15, 0, TimeSpan.Zero);
    var request = new DataVaultSaveRequest(
        loadTimestamp,
        "sqlserver-smoke",
        [new(customer, [new("Customer Id", "C-SQL-100")])],
        []);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = database.CreateContext()) {
      var result = await saveService.SaveAsync(context, request);

      Assert.Equal(1, result.RowsWritten);
      AssertSingleSavedRecord(
          result,
          DataVaultTableKind.Hub,
          "Customer",
          "HubCustomer",
          GetHashKey(result, DataVaultTableKind.Hub, "Customer"));
    }

    await using (var context = database.CreateContext()) {
      var row = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().SingleAsync();

      Assert.Equal("C-SQL-100", row["CustomerId"]);
      Assert.Equal("sqlserver-smoke", row["RecordSource"]);
      Assert.Equal(loadTimestamp, row["LoadTimestamp"]);
      Assert.Matches("^[0-9a-f]{64}$", Assert.IsType<string>(row["CustomerHashKey"]));
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerPersistsRepresentativeLinkSaveWhenConfigured() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 4, 9, 30, 0, TimeSpan.Zero);
    var hubRequest = new DataVaultSaveRequest(
        loadTimestamp,
        "sqlserver-smoke",
        [
            new(customer, [new("Customer Id", "C-SQL-200")]),
            new(order, [new("Order Id", "O-SQL-200")]),
        ],
        []);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string customerHashKey;
    string orderHashKey;
    DataVaultSaveResult linkResult;

    await using (var context = database.CreateContext()) {
      var hubResult = await saveService.SaveAsync(context, hubRequest);

      Assert.Equal(2, hubResult.RowsWritten);
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
      orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");

      var linkRequest = new DataVaultSaveRequest(
          loadTimestamp,
          "sqlserver-smoke",
          [],
          [
              new(customerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)]),
          ]);
      linkResult = await saveService.SaveAsync(context, linkRequest);

      Assert.Equal(1, linkResult.RowsWritten);
      AssertSingleSavedRecord(
          linkResult,
          DataVaultTableKind.Link,
          "CustomerOrder",
          "LinkCustomerOrder",
          GetHashKey(linkResult, DataVaultTableKind.Link, "CustomerOrder"));
    }

    await using (var context = database.CreateContext()) {
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync();

      Assert.Equal(customerHashKey, linkRow["CustomerHashKey"]);
      Assert.Equal(orderHashKey, linkRow["OrderHashKey"]);
      Assert.Equal("sqlserver-smoke", linkRow["RecordSource"]);
      Assert.Equal(loadTimestamp, linkRow["LoadTimestamp"]);
      Assert.Matches("^[0-9a-f]{64}$", Assert.IsType<string>(linkRow["CustomerOrderHashKey"]));
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerPersistsRepresentativeSatelliteSaveWhenConfigured() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var hubLoadTimestamp = new DateTimeOffset(2026, 5, 4, 9, 45, 0, TimeSpan.Zero);
    var satelliteLoadTimestamp = new DateTimeOffset(2026, 5, 4, 10, 0, 0, TimeSpan.Zero);
    var hubRequest = new DataVaultSaveRequest(
        hubLoadTimestamp,
        "sqlserver-smoke",
        [new(customer, [new("Customer Id", "C-SQL-300")])],
        []);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string customerHashKey;
    DataVaultSaveResult satelliteResult;

    await using (var context = database.CreateContext()) {
      var hubResult = await saveService.SaveAsync(context, hubRequest);

      Assert.Equal(1, hubResult.RowsWritten);
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      var satelliteRequest = new DataVaultSaveRequest(
          satelliteLoadTimestamp,
          "sqlserver-smoke",
          [],
          [],
          [
              new(contact, customerHashKey, [new("Email Address", "sqlserver@example.test")], "contact-hash-sqlserver-1"),
          ]);
      satelliteResult = await saveService.SaveAsync(context, satelliteRequest);

      Assert.Equal(1, satelliteResult.RowsWritten);
      AssertSingleSavedRecord(
          satelliteResult,
          DataVaultTableKind.Satellite,
          "Contact",
          "SatCustomerContact",
          customerHashKey);
    }

    await using (var context = database.CreateContext()) {
      var row = await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().SingleAsync();

      Assert.Equal(customerHashKey, row["CustomerHashKey"]);
      Assert.Equal("sqlserver@example.test", row["EmailAddress"]);
      Assert.Equal("contact-hash-sqlserver-1", row["HashDiff"]);
      Assert.Equal(satelliteLoadTimestamp, row["LoadTimestamp"]);
      Assert.Equal("sqlserver-smoke", row["RecordSource"]);
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerReadsCurrentAndAsOfSatelliteRowsWhenConfigured() {
    var metadataModel = CreateMetadataModel();
    var customer = metadataModel.Hubs.Single(hub => hub.Name == "Customer");
    var contact = metadataModel.Satellites.Single(satellite => satellite.Name == "Contact");
    var linkState = metadataModel.Satellites.Single(satellite => satellite.Name == "State");
    var firstLoadTimestamp = new DateTimeOffset(2026, 5, 4, 10, 15, 0, TimeSpan.Zero);
    var secondLoadTimestamp = new DateTimeOffset(2026, 5, 4, 10, 45, 0, TimeSpan.Zero);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    string customerHashKey;

    await using (var context = database.CreateContext()) {
      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "sqlserver-read-smoke",
              [new(customer, [new("Customer Id", "C-SQL-READ")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "sqlserver-read-smoke",
              [],
              [],
              [
                  new(contact, customerHashKey, [new("Email Address", "first-sqlserver@example.test")], "contact-hash-sqlserver-1"),
              ]));
      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              secondLoadTimestamp,
              "sqlserver-read-smoke",
              [],
              [],
              [
                  new(contact, customerHashKey, [new("Email Address", "latest-sqlserver@example.test")], "contact-hash-sqlserver-2"),
              ]));
    }

    await using (var context = database.CreateContext()) {
      var latestRequest = new DataVaultLatestSatelliteReadRequest(contact, [customerHashKey]);
      var asOfRequest = new DataVaultLatestSatelliteReadRequest(contact, [customerHashKey], firstLoadTimestamp);
      var diagnostics = readDiagnostics.Analyze(context, latestRequest);
      var unsupportedShapeDiagnostics = readDiagnostics.Analyze(
          context,
          new DataVaultLatestSatelliteReadRequest(linkState, ["link-hk"]));

      Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, diagnostics.ReadStrategy.Status);
      Assert.Equal(KnownProviderNames.SqlServer, diagnostics.ReadStrategy.ProviderName);
      Assert.Equal("SqlServerDataVaultReadStrategy", diagnostics.ReadStrategy.SelectedStrategyName);
      Assert.Empty(diagnostics.ReadStrategy.FallbackCauses);
      Assert.Contains(
          diagnostics.ReadStrategy.Candidates,
          candidate => candidate.StrategyName == "SqlServerDataVaultReadStrategy" && candidate.CanRead);
      Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, unsupportedShapeDiagnostics.ReadStrategy.Status);
      Assert.Contains(
          unsupportedShapeDiagnostics.ReadStrategy.FallbackCauses,
          cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent);

      var latestRows = await readService.ReadLatestSatelliteRowsAsync(context, latestRequest);
      var currentRows = await readService.ReadCurrentSatelliteRowsAsync(context, contact, [customerHashKey]);
      var asOfRows = await readService.ReadLatestSatelliteRowsAsync(context, asOfRequest);
      var projectedRows = await readService.ReadLatestSatelliteAsync(
          context,
          latestRequest,
          row => row.RequiredString("Email Address"));
      var latestRow = Assert.Single(latestRows);
      var currentRow = Assert.Single(currentRows);
      var asOfRow = Assert.Single(asOfRows);

      Assert.Equal(customerHashKey, latestRow.ParentHashKey);
      Assert.Equal("contact-hash-sqlserver-2", latestRow.HashDiff);
      Assert.Equal(secondLoadTimestamp, latestRow.LoadTimestamp);
      Assert.Equal("latest-sqlserver@example.test", latestRow.PayloadValues["Email Address"]);
      Assert.Equal(latestRow.HashDiff, currentRow.HashDiff);
      Assert.Equal("contact-hash-sqlserver-1", asOfRow.HashDiff);
      Assert.Equal(firstLoadTimestamp, asOfRow.LoadTimestamp);
      Assert.Equal("first-sqlserver@example.test", asOfRow.PayloadValues["Email Address"]);
      Assert.Equal(["latest-sqlserver@example.test"], projectedRows);
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerRebuildsOrdinaryPitViaInsertSelectWhenConfigured() {
    var metadataModel = CreateMetadataModel();
    var customer = metadataModel.Hubs.Single(hub => hub.Name == "Customer");
    var contact = metadataModel.Satellites.Single(satellite => satellite.Name == "Contact");
    var profile = metadataModel.Satellites.Single(satellite => satellite.Name == "Profile");
    var pit = metadataModel.Pits.Single(current => current.Name == "CustomerContactProfile");
    var importTimestamp = new DateTimeOffset(2026, 5, 4, 11, 0, 0, TimeSpan.Zero);
    var contactTimestamp = new DateTimeOffset(2026, 5, 4, 11, 15, 0, TimeSpan.Zero);
    var profileTimestamp = new DateTimeOffset(2026, 5, 4, 11, 30, 0, TimeSpan.Zero);
    var secondContactTimestamp = new DateTimeOffset(2026, 5, 4, 11, 45, 0, TimeSpan.Zero);
    var stalePitTimestamp = new DateTimeOffset(2026, 5, 4, 10, 45, 0, TimeSpan.Zero);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var sqlServerProvider = CreateServiceProvider();
    using var neutralProvider = CreateNeutralServiceProvider();
    var saveService = sqlServerProvider.GetRequiredService<IDataVaultSaveService>();
    var sqlServerMaintenance = sqlServerProvider.GetRequiredService<IDataVaultPitMaintenanceService>();
    var neutralMaintenance = neutralProvider.GetRequiredService<IDataVaultPitMaintenanceService>();
    string customerHashKey;

    await using (var context = database.CreateContext()) {
      customerHashKey = await SavePitCustomerHistoryAsync(
          saveService,
          context,
          customer,
          contact,
          profile,
          importTimestamp,
          contactTimestamp,
          profileTimestamp,
          secondContactTimestamp);
    }

    await using (var context = database.CreateContext()) {
      await SeedStalePitRowAsync(context, customerHashKey, stalePitTimestamp);
      var fallbackResult = await neutralMaintenance.RebuildAsync(context, new DataVaultPitRebuildRequest(pit));
      var fallbackRows = await ReadPitRowsAsync(context);

      Assert.Equal(1, fallbackResult.ParentHashKeyCount);
      Assert.Equal(1, fallbackResult.RowsDeleted);
      Assert.Equal(3, fallbackResult.RowsWritten);
      Assert.Equal("PitCustomerContactProfile", fallbackResult.TableName);

      await ResetPitToStaleRowAsync(context, customerHashKey, stalePitTimestamp);
      using var listener = new DataVaultActivityTestListener();
      var sqlServerResult = await sqlServerMaintenance.RebuildAsync(context, new DataVaultPitRebuildRequest(pit));
      var sqlServerRows = await ReadPitRowsAsync(context);

      Assert.Equal(fallbackResult.ParentHashKeyCount, sqlServerResult.ParentHashKeyCount);
      Assert.Equal(fallbackResult.RowsDeleted, sqlServerResult.RowsDeleted);
      Assert.Equal(fallbackResult.RowsWritten, sqlServerResult.RowsWritten);
      Assert.Equal(fallbackRows, sqlServerRows);

      var activity = Assert.Single(listener.StoppedActivities);
      var tags = GetTags(activity);
      Assert.Equal("dvault.maintenance.pit.rebuild", activity.OperationName);
      Assert.Equal(ActivityStatusCode.Ok, activity.Status);
      Assert.Equal("ProviderStrategySelected", tags["dvault.strategy.status"]);
      Assert.Equal("SqlServerDataVaultPitMaintenanceService", tags["dvault.strategy.type"]);
      Assert.Contains(
          activity.Events,
          current => string.Equals(current.Name, "dvault.strategy.selected", StringComparison.Ordinal));
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerPitRebuildRollsBackWhenInsertSelectFailsWhenConfigured() {
    var metadataModel = CreateMetadataModel();
    var customer = metadataModel.Hubs.Single(hub => hub.Name == "Customer");
    var contact = metadataModel.Satellites.Single(satellite => satellite.Name == "Contact");
    var profile = metadataModel.Satellites.Single(satellite => satellite.Name == "Profile");
    var pit = metadataModel.Pits.Single(current => current.Name == "CustomerContactProfile");
    var importTimestamp = new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero);
    var contactTimestamp = new DateTimeOffset(2026, 5, 4, 12, 15, 0, TimeSpan.Zero);
    var profileTimestamp = new DateTimeOffset(2026, 5, 4, 12, 30, 0, TimeSpan.Zero);
    var secondContactTimestamp = new DateTimeOffset(2026, 5, 4, 12, 45, 0, TimeSpan.Zero);
    var stalePitTimestamp = new DateTimeOffset(2026, 5, 4, 11, 45, 0, TimeSpan.Zero);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    string customerHashKey;

    await using (var context = database.CreateContext()) {
      customerHashKey = await SavePitCustomerHistoryAsync(
          saveService,
          context,
          customer,
          contact,
          profile,
          importTimestamp,
          contactTimestamp,
          profileTimestamp,
          secondContactTimestamp);
      await SeedStalePitRowAsync(context, customerHashKey, stalePitTimestamp);
      await CreatePitInsertFaultTriggerAsync(context);
    }

    await using (var context = database.CreateContext()) {
      await Assert.ThrowsAnyAsync<Exception>(() =>
          maintenanceService.RebuildAsync(context, new DataVaultPitRebuildRequest(pit)));

      var rows = await ReadPitRowsAsync(context);
      Assert.Equal(
          [new PitRowSnapshot(customerHashKey, stalePitTimestamp, ContactLoadTimestamp: null, ProfileLoadTimestamp: null)],
          rows);
      Assert.Equal(0, await CountSqlServerStagingTablesAsync(context));
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerPitRebuildRollsBackWhenCancellationIsObservedBeforeCommitWhenConfigured() {
    var metadataModel = CreateMetadataModel();
    var customer = metadataModel.Hubs.Single(hub => hub.Name == "Customer");
    var contact = metadataModel.Satellites.Single(satellite => satellite.Name == "Contact");
    var profile = metadataModel.Satellites.Single(satellite => satellite.Name == "Profile");
    var pit = metadataModel.Pits.Single(current => current.Name == "CustomerContactProfile");
    var importTimestamp = new DateTimeOffset(2026, 5, 4, 13, 0, 0, TimeSpan.Zero);
    var contactTimestamp = new DateTimeOffset(2026, 5, 4, 13, 15, 0, TimeSpan.Zero);
    var profileTimestamp = new DateTimeOffset(2026, 5, 4, 13, 30, 0, TimeSpan.Zero);
    var secondContactTimestamp = new DateTimeOffset(2026, 5, 4, 13, 45, 0, TimeSpan.Zero);
    var stalePitTimestamp = new DateTimeOffset(2026, 5, 4, 12, 45, 0, TimeSpan.Zero);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    using var cancellation = new CancellationTokenSource();
    string customerHashKey;

    await using (var context = database.CreateContext()) {
      customerHashKey = await SavePitCustomerHistoryAsync(
          saveService,
          context,
          customer,
          contact,
          profile,
          importTimestamp,
          contactTimestamp,
          profileTimestamp,
          secondContactTimestamp);
      await SeedStalePitRowAsync(context, customerHashKey, stalePitTimestamp);
    }

    SqlServerDataVaultPitMaintenanceService.BeforeCommitHookForTestingAsync = _ => {
      cancellation.Cancel();

      return Task.CompletedTask;
    };

    try {
      await using var context = database.CreateContext();
      await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
          maintenanceService.RebuildAsync(context, new DataVaultPitRebuildRequest(pit), cancellation.Token));

      var rows = await ReadPitRowsAsync(context);
      Assert.Equal(
          [new PitRowSnapshot(customerHashKey, stalePitTimestamp, ContactLoadTimestamp: null, ProfileLoadTimestamp: null)],
          rows);
      Assert.Equal(0, await CountSqlServerStagingTablesAsync(context));
    }
    finally {
      SqlServerDataVaultPitMaintenanceService.BeforeCommitHookForTestingAsync = null;
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerBulkStrategyPersistsOrderedHubLinkAndSatelliteBatchWhenConfigured() {
    await ExternalProviderBulkSaveAssertions.AssertProviderBulkSaveAsync(
        ExternalProviderLiveSchemaFixture.CreateSqlServerAsync,
        services => services.AddDVaultSqlServer(),
        "SqlServerDataVaultSaveStrategy");
  }

  [Fact]
  public async Task AddDVaultSqlServerStagedBulkReuseKeepsHubAndLinkIdempotencyWhenConfigured() {
    await using var fixture = await ExternalProviderLiveSchemaFixture.CreateSqlServerAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var request = CreateStagedHubLinkBulkRequest(provider, "REUSE", StagedBulkPairCount);

    await using var context = fixture.CreateContext();
    var firstResult = await saveService.SaveAsync(context, request);
    var replayResult = await saveService.SaveAsync(context, request);

    Assert.Equal(StagedBulkPairCount * 3, firstResult.RowsWritten);
    Assert.Equal(0, replayResult.RowsWritten);
    Assert.Equal(StagedBulkPairCount * 3, replayResult.SavedRecords.Count);
    Assert.Empty(context.ChangeTracker.Entries());
    Assert.Equal(StagedBulkPairCount, await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().CountAsync());
    Assert.Equal(StagedBulkPairCount, await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().CountAsync());
    Assert.Equal(StagedBulkPairCount, await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().CountAsync());
  }

  [Fact]
  public async Task AddDVaultSqlServerStagedBulkSaveParticipatesInCallerTransactionWhenConfigured() {
    await using var fixture = await ExternalProviderLiveSchemaFixture.CreateSqlServerAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var request = CreateStagedCustomerHubBulkRequest("TX", StagedBulkHubOnlyCount);

    await using (var context = fixture.CreateContext()) {
      await using var transaction = await context.Database.BeginTransactionAsync();
      var result = await saveService.SaveAsync(context, request);

      Assert.Equal(StagedBulkHubOnlyCount, result.RowsWritten);
      Assert.Equal(0, await CountSqlServerStagingTablesAsync(context));

      await transaction.RollbackAsync();
    }

    await using (var context = fixture.CreateContext()) {
      Assert.Equal(0, await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().CountAsync());
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerStagedBulkSaveObservesCancellationBeforeWritingWhenConfigured() {
    await using var fixture = await ExternalProviderLiveSchemaFixture.CreateSqlServerAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var request = CreateStagedCustomerHubBulkRequest("CANCEL", StagedBulkHubOnlyCount);
    using var cancellation = new CancellationTokenSource();
    await cancellation.CancelAsync();

    await using var context = fixture.CreateContext();
    await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
        saveService.SaveAsync(context, request, cancellation.Token));

    Assert.Equal(0, await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().CountAsync());
  }

  private static DataVaultBulkSaveRequest CreateStagedCustomerHubBulkRequest(
      string idPrefix,
      int count) {
    var metadataModel = LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel();
    var customer = metadataModel.Hubs.Single(hub => hub.Name == LiveSchemaReaderContractFixture.CustomerHubName);

    return new DataVaultBulkSaveRequest([
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 19, 10, 0, 0, TimeSpan.Zero),
            "sqlserver-staged-" + idPrefix.ToLowerInvariant(),
            Enumerable.Range(0, count)
                .Select(index => new DataVaultHubSaveOperation(
                    customer,
                    [new("Customer Id", idPrefix + "-C-" + index.ToString("000", CultureInfo.InvariantCulture))]))
                .ToArray(),
            []),
    ]);
  }

  private static DataVaultBulkSaveRequest CreateStagedHubLinkBulkRequest(
      IServiceProvider provider,
      string idPrefix,
      int pairCount) {
    var metadataModel = LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel();
    var customer = metadataModel.Hubs.Single(hub => hub.Name == LiveSchemaReaderContractFixture.CustomerHubName);
    var order = metadataModel.Hubs.Single(hub => hub.Name == LiveSchemaReaderContractFixture.OrderHubName);
    var customerOrder = metadataModel.Links.Single(link => link.Name == LiveSchemaReaderContractFixture.CustomerOrderLinkName);
    var customerIds = Enumerable.Range(0, pairCount)
        .Select(index => idPrefix + "-C-" + index.ToString("000", CultureInfo.InvariantCulture))
        .ToArray();
    var orderIds = Enumerable.Range(0, pairCount)
        .Select(index => idPrefix + "-O-" + index.ToString("000", CultureInfo.InvariantCulture))
        .ToArray();
    var customerHashKeys = customerIds
        .Select(customerId => ComputeHash(provider, [new("Customer Id", customerId)]))
        .ToArray();
    var orderHashKeys = orderIds
        .Select(orderId => ComputeHash(provider, [new("Order Id", orderId)]))
        .ToArray();

    return new DataVaultBulkSaveRequest([
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 19, 10, 0, 0, TimeSpan.Zero),
            "sqlserver-staged-hubs",
            customerIds
                .Select(customerId => new DataVaultHubSaveOperation(customer, [new("Customer Id", customerId)]))
                .Concat(orderIds.Select(orderId => new DataVaultHubSaveOperation(order, [new("Order Id", orderId)])))
                .ToArray(),
            []),
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 19, 10, 5, 0, TimeSpan.Zero),
            "sqlserver-staged-links",
            [],
            Enumerable.Range(0, pairCount)
                .Select(index => new DataVaultLinkSaveOperation(
                    customerOrder,
                    [
                        new("Customer", customerHashKeys[index]),
                        new("Order", orderHashKeys[index]),
                    ]))
                .ToArray()),
    ]);
  }

  private static string ComputeHash(
      IServiceProvider provider,
      IEnumerable<KeyValuePair<string, string>> fields) {
    var normalizer = provider.GetRequiredService<IStableHashNormalizer>();
    var hashService = provider.GetRequiredService<IStableHashService>();
    var normalized = normalizer.NormalizeFields(fields.Select(field => new KeyValuePair<string, object?>(field.Key, field.Value)));

    return hashService.ComputeHash(normalized).Value;
  }

  private static async Task<string> SavePitCustomerHistoryAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      DataVaultHubMetadata customer,
      DataVaultSatelliteMetadata contact,
      DataVaultSatelliteMetadata profile,
      DateTimeOffset importTimestamp,
      DateTimeOffset contactTimestamp,
      DateTimeOffset profileTimestamp,
      DateTimeOffset secondContactTimestamp) {
    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            importTimestamp,
            "sqlserver-pit-smoke",
            [new(customer, [new("Customer Id", "C-SQL-PIT")])],
            []));
    var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

    await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            contactTimestamp,
            "sqlserver-pit-smoke",
            [],
            [],
            [
                new(contact, customerHashKey, [new("Email Address", "first-pit@example.test")], "contact-hash-pit-1"),
            ]));
    await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            profileTimestamp,
            "sqlserver-pit-smoke",
            [],
            [],
            [
                new(profile, customerHashKey, [new("Tier", "Gold")], "profile-hash-pit-1"),
            ]));
    await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            secondContactTimestamp,
            "sqlserver-pit-smoke",
            [],
            [],
            [
                new(contact, customerHashKey, [new("Email Address", "latest-pit@example.test")], "contact-hash-pit-2"),
            ]));

    return customerHashKey;
  }

  private static async Task SeedStalePitRowAsync(
      DbContext context,
      string customerHashKey,
      DateTimeOffset loadTimestamp) {
    context.Set<Dictionary<string, object>>("PitCustomerContactProfile").Add(new Dictionary<string, object> {
      ["CustomerHashKey"] = customerHashKey,
      ["LoadTimestamp"] = loadTimestamp,
      ["ContactLoadTimestamp"] = null!,
      ["ProfileLoadTimestamp"] = null!,
    });

    await context.SaveChangesAsync();
  }

  private static async Task ResetPitToStaleRowAsync(
      DbContext context,
      string customerHashKey,
      DateTimeOffset loadTimestamp) {
    await context.Set<Dictionary<string, object>>("PitCustomerContactProfile").ExecuteDeleteAsync();
    await SeedStalePitRowAsync(context, customerHashKey, loadTimestamp);
  }

  private static async Task<IReadOnlyList<PitRowSnapshot>> ReadPitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerContactProfile")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new PitRowSnapshot(
            Assert.IsType<string>(row["CustomerHashKey"]),
            ReadRequiredTimestamp(row, "LoadTimestamp"),
            ReadOptionalTimestamp(row, "ContactLoadTimestamp"),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp")))
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static DateTimeOffset ReadRequiredTimestamp(
      Dictionary<string, object> row,
      string columnName) {
    Assert.True(DataVaultLoadTimestampValueConverter.TryReadProviderValue(row[columnName], out var timestamp));

    return timestamp;
  }

  private static DateTimeOffset? ReadOptionalTimestamp(
      Dictionary<string, object> row,
      string columnName) {
    if (!row.TryGetValue(columnName, out var value) || value is null or DBNull) {
      return null;
    }

    Assert.True(DataVaultLoadTimestampValueConverter.TryReadProviderValue(value, out var timestamp));

    return timestamp;
  }

  private static async Task CreatePitInsertFaultTriggerAsync(SqlServerSmokeContext context) {
    await context.Database.ExecuteSqlRawAsync(
        "CREATE TRIGGER " +
        QuoteIdentifier(context.SchemaName) +
        ".[trg_dvault_pit_rebuild_fault] ON " +
        QuoteIdentifier(context.SchemaName) +
        ".[PitCustomerContactProfile] AFTER INSERT AS BEGIN THROW 51000, 'DVault PIT rebuild fault injection', 1; END");
  }

  private static async Task<int> CountSqlServerStagingTablesAsync(DbContext context) {
    var connection = context.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync();
    }

    try {
      await using var command = connection.CreateCommand();
      command.Transaction = context.Database.CurrentTransaction?.GetDbTransaction();
      command.CommandText = "SELECT COUNT(1) FROM tempdb.sys.tables WHERE [name] LIKE '#dvault[_]stage[_]%'";

      return Convert.ToInt32(await command.ExecuteScalarAsync(), CultureInfo.InvariantCulture);
    }
    finally {
      if (shouldCloseConnection) {
        await connection.CloseAsync();
      }
    }
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVaultSqlServer();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static ServiceProvider CreateNeutralServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVault();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static IReadOnlyDictionary<string, object?> GetTags(Activity activity) {
    return activity.TagObjects.ToDictionary(tag => tag.Key, tag => tag.Value, StringComparer.Ordinal);
  }

  private static string GetHashKey(DataVaultSaveResult result, DataVaultTableKind kind, string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
  }

  private static void AssertSingleSavedRecord(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName,
      string tableName,
      string hashKey) {
    var record = Assert.Single(result.SavedRecords);

    Assert.Equal(kind, record.Kind);
    Assert.Equal(metadataName, record.MetadataName);
    Assert.Equal(tableName, record.TableName);
    Assert.Equal(hashKey, record.HashKey);
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        DataVaultMetadataReference.Hub("Customer"),
        ["Email Address"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        DataVaultMetadataReference.Hub("Customer"),
        ["Tier"]);
    var state = new DataVaultSatelliteMetadata(
        "State",
        DataVaultMetadataReference.Link("CustomerOrder"),
        ["State Code"]);
    var customerContactProfile = new DataVaultPitMetadata(customer.ToReference(), ["Contact", "Profile"]);

    return new DataVaultMetadataModel(
        [
            customer,
            order,
        ],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
        ],
        [
            contact,
            profile,
            state,
        ],
        [customerContactProfile]);
  }

  private static DbContextOptions<SqlServerSmokeContext> CreateSqlServerOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<SqlServerSmokeContext>();

    SqlServerProviderReflection.UseSqlServer(optionsBuilder, connectionString);
    optionsBuilder.ReplaceService<IModelCacheKeyFactory, SqlServerSmokeModelCacheKeyFactory>();

    return optionsBuilder.Options;
  }

  private static string QuoteIdentifier(string value) {
    return "[" + value.Replace("]", "]]", StringComparison.Ordinal) + "]";
  }

  private static string SqlLiteral(string value) {
    return "N'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private sealed record PitRowSnapshot(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ContactLoadTimestamp,
      DateTimeOffset? ProfileLoadTimestamp);

  private sealed class SqlServerSmokeDatabase : IAsyncDisposable {
    private static readonly string[] ProducedTables = [
        "PitCustomerContactProfile",
        "SatCustomerProfile",
        "SatCustomerContact",
        "SatCustomerOrderState",
        "LinkCustomerOrder",
        "HubOrder",
        "HubCustomer",
    ];

    private readonly DbContextOptions<SqlServerSmokeContext> _options;
    private readonly string _schemaName;

    private SqlServerSmokeDatabase(DbContextOptions<SqlServerSmokeContext> options, string schemaName) {
      _options = options;
      _schemaName = schemaName;
    }

    public static async Task<SqlServerSmokeDatabase> CreateAsync() {
      var configuration = SqlServerIntegrationTestConfiguration.FromEnvironment();
      if (!configuration.IsConfigured) {
        Assert.Skip(SqlServerIntegrationTestConfiguration.MissingConfigurationSkipMessage);
      }

      var schemaName = "dvault_test_" + Guid.NewGuid().ToString("N");
      var database = new SqlServerSmokeDatabase(CreateSqlServerOptions(configuration.ConnectionString!), schemaName);

      try {
        await using var context = database.CreateContext();
        await database.CreateSchemaAsync(context);
      }
      catch {
        await database.DisposeAsync();
        throw;
      }

      return database;
    }

    public SqlServerSmokeContext CreateContext() {
      return new SqlServerSmokeContext(_options, _schemaName);
    }

    public async ValueTask DisposeAsync() {
      await using var context = CreateContext();
      await DropSchemaAsync(context).ConfigureAwait(false);
    }

    private async Task CreateSchemaAsync(SqlServerSmokeContext context) {
      await context.Database.ExecuteSqlRawAsync(
          "IF SCHEMA_ID(" + SqlLiteral(_schemaName) + ") IS NULL EXEC(N'CREATE SCHEMA " + QuoteIdentifier(_schemaName) + "');");
      foreach (var batch in SqlServerBatchScript.SplitBatches(context.Database.GenerateCreateScript())) {
        await context.Database.ExecuteSqlRawAsync(batch);
      }
    }

    private async Task DropSchemaAsync(SqlServerSmokeContext context) {
      foreach (var tableName in ProducedTables) {
        await context.Database.ExecuteSqlRawAsync(
            "DROP TABLE IF EXISTS " + QuoteIdentifier(_schemaName) + "." + QuoteIdentifier(tableName) + ";");
      }

      await context.Database.ExecuteSqlRawAsync("DROP SCHEMA IF EXISTS " + QuoteIdentifier(_schemaName) + ";");
    }
  }

  private sealed class SqlServerSmokeContext(
      DbContextOptions<SqlServerSmokeContext> options,
      string schemaName) : DbContext(options) {
    public string SchemaName { get; } = schemaName;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.HasDefaultSchema(SchemaName);
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }

  private sealed class SqlServerSmokeModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      if (context is SqlServerSmokeContext smokeContext) {
        return (context.GetType(), smokeContext.SchemaName, designTime);
      }

      return (context.GetType(), designTime);
    }
  }
}
