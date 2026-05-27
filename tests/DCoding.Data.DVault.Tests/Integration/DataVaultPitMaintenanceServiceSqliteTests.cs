using System.Globalization;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultPitMaintenanceServiceSqliteTests {
  [Theory]
  [InlineData(DataVaultLoadTimestampStorage.ProviderDefault)]
  [InlineData(DataVaultLoadTimestampStorage.Iso8601UtcText)]
  [InlineData(DataVaultLoadTimestampStorage.UtcTicks)]
  public async Task PitMaintenanceRebuildsDeterministicRowsAndMissingSnapshotsThroughSqlite(
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    var metadata = CreateMetadata();
    var importTimestamp = Utc(2026, 5, 11, 8, 0);
    var statusTimestamp = Utc(2026, 5, 11, 9, 0);
    var profileTimestamp = Utc(2026, 5, 11, 10, 0);
    var secondStatusTimestamp = Utc(2026, 5, 11, 11, 0);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database.DatabasePath, loadTimestampStorage);
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    string customerHashKey;
    DataVaultPitMaintenanceResult maintenanceResult;

    await using (var context = new PitMaintenanceContext(options, loadTimestampStorage)) {
      await context.Database.EnsureCreatedAsync();
      customerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-100", importTimestamp);
      await SaveStatusAsync(saveService, context, metadata, customerHashKey, statusTimestamp, "Active", "status-1");
      await SaveProfileAsync(saveService, context, metadata, customerHashKey, profileTimestamp, "Alice Adams", "Gold", "profile-1");
      await SaveStatusAsync(saveService, context, metadata, customerHashKey, secondStatusTimestamp, "Preferred", "status-2");

      context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(CreatePitRow(
          loadTimestampStorage,
          customerHashKey,
          Utc(2026, 5, 11, 8, 30),
          profileSnapshotTimestamp: null,
          statusSnapshotTimestamp: null));
      await context.SaveChangesAsync();

      maintenanceResult = await maintenanceService.RebuildAsync(
          context,
          new DataVaultPitRebuildRequest(metadata.Pit));
    }

    await using (var context = new PitMaintenanceContext(options, loadTimestampStorage)) {
      var pitRows = await ReadPitRowsAsync(context);
      var readRecords = await readService.ReadPitRowsAsync(
          context,
          new DataVaultPitAsOfReadRequest(metadata.Pit, [customerHashKey], Utc(2026, 5, 11, 10, 30)));

      Assert.Equal("PitCustomerProfileStatus", maintenanceResult.TableName);
      Assert.Equal(1, maintenanceResult.ParentHashKeyCount);
      Assert.Equal(1, maintenanceResult.RowsDeleted);
      Assert.Equal(3, maintenanceResult.RowsWritten);
      Assert.False(maintenanceResult.IsNoOp);
      Assert.Collection(
          pitRows,
          row => AssertPitRow(row, customerHashKey, statusTimestamp, null, statusTimestamp),
          row => AssertPitRow(row, customerHashKey, profileTimestamp, profileTimestamp, statusTimestamp),
          row => AssertPitRow(row, customerHashKey, secondStatusTimestamp, profileTimestamp, secondStatusTimestamp));

      var record = Assert.Single(readRecords);
      Assert.Equal(profileTimestamp, record.LoadTimestamp);
      Assert.Equal(profileTimestamp, RequiredSnapshot(record, "Profile").SnapshotLoadTimestamp);
      Assert.Equal(statusTimestamp, RequiredSnapshot(record, "Status").SnapshotLoadTimestamp);
    }
  }

  [Fact]
  public async Task PitMaintenanceRebuildsAndReadsMultiActiveTupleRowsThroughSqliteFallback() {
    var metadata = CreateMultiActiveMetadata();
    var importTimestamp = Utc(2026, 5, 11, 7, 0);
    var profileBeforeTuple = Utc(2026, 5, 11, 8, 0);
    var billingContact = Utc(2026, 5, 11, 9, 0);
    var shippingContact = Utc(2026, 5, 11, 10, 0);
    var profileAfterTuple = Utc(2026, 5, 11, 11, 0);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<MultiActivePitMaintenanceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    string customerHashKey;
    DataVaultPitMaintenanceResult maintenanceResult;

    await using (var context = new MultiActivePitMaintenanceContext(options)) {
      await context.Database.EnsureCreatedAsync();
      customerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-600", importTimestamp);
      await SaveProfileAsync(saveService, context, metadata, customerHashKey, profileBeforeTuple, "Frank First", "Silver", "profile-before");
      await SaveContactAsync(saveService, context, metadata, customerHashKey, billingContact, "billing", "billing@example.test", "contact-billing");
      await SaveContactAsync(saveService, context, metadata, customerHashKey, shippingContact, "shipping", "shipping@example.test", "contact-shipping");
      await SaveProfileAsync(saveService, context, metadata, customerHashKey, profileAfterTuple, "Frank Final", "Gold", "profile-after");

      maintenanceResult = await maintenanceService.RebuildAsync(
          context,
          new DataVaultPitRebuildRequest(metadata.Pit));
    }

    await using (var context = new MultiActivePitMaintenanceContext(options)) {
      var request = new DataVaultPitAsOfReadRequest(metadata.Pit, [customerHashKey], Utc(2026, 5, 11, 11, 30));
      var diagnostics = readDiagnostics.Analyze(context, request);
      var records = await readService.ReadPitRowsAsync(context, request);
      var projectedRows = await readService.ReadPitAsync(
          context,
          request,
          row => new ContactSnapshotRead(
              row.RequiredString("ParentHashKey"),
              row.RequiredString("Contact Type"),
              row.RequiredSatellite("Contact").RequiredString("Email Address"),
              row.RequiredSatellite("Profile").RequiredString("Customer Name")));

      Assert.Equal("PitCustomerContactProfile", maintenanceResult.TableName);
      Assert.Equal(1, maintenanceResult.ParentHashKeyCount);
      Assert.Equal(0, maintenanceResult.RowsDeleted);
      Assert.Equal(4, maintenanceResult.RowsWritten);
      Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, diagnostics.ReadStrategy.Status);
      Assert.NotNull(diagnostics.ReadShape);
      Assert.Equal(["CustomerHashKey", "ContactType", "LoadTimestamp"], diagnostics.ReadShape!.Pit!.RowIdentityColumns.Single().ColumnNames);
      Assert.Equal(
          ["billing", "shipping"],
          records.Select(record => record.DrivingKeyValues["Contact Type"]).ToArray());
      Assert.All(records, record => {
        Assert.Equal(profileAfterTuple, record.LoadTimestamp);
        Assert.Equal(profileAfterTuple, RequiredSnapshot(record, "Profile").SnapshotLoadTimestamp);
      });
      Assert.Collection(
          projectedRows.OrderBy(row => row.ContactType, StringComparer.Ordinal),
          row => {
            Assert.Equal(customerHashKey, row.ParentHashKey);
            Assert.Equal("billing", row.ContactType);
            Assert.Equal("billing@example.test", row.EmailAddress);
            Assert.Equal("Frank Final", row.CustomerName);
          },
          row => {
            Assert.Equal(customerHashKey, row.ParentHashKey);
            Assert.Equal("shipping", row.ContactType);
            Assert.Equal("shipping@example.test", row.EmailAddress);
            Assert.Equal("Frank Final", row.CustomerName);
          });
    }
  }

  [Fact]
  public async Task PitMaintenanceMaintainsRequestedMultiActiveParentTupleHistoryThroughSqliteFallback() {
    var metadata = CreateMultiActiveMetadata();
    var importTimestamp = Utc(2026, 5, 11, 7, 0);
    var profileBeforeTuple = Utc(2026, 5, 11, 8, 0);
    var billingContact = Utc(2026, 5, 11, 9, 0);
    var lateBillingContact = Utc(2026, 5, 11, 9, 30);
    var shippingContact = Utc(2026, 5, 11, 10, 0);
    var profileAfterTuple = Utc(2026, 5, 11, 11, 0);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<MultiActivePitMaintenanceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    string firstCustomerHashKey;
    string secondCustomerHashKey;

    await using (var context = new MultiActivePitMaintenanceContext(options)) {
      await context.Database.EnsureCreatedAsync();
      firstCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-700", importTimestamp);
      secondCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-800", importTimestamp);
      await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, profileBeforeTuple, "Gina Green", "Silver", "profile-c700-before");
      await SaveContactAsync(saveService, context, metadata, firstCustomerHashKey, billingContact, "billing", "billing-c700@example.test", "contact-c700-billing");
      await SaveContactAsync(saveService, context, metadata, firstCustomerHashKey, shippingContact, "shipping", "shipping-c700@example.test", "contact-c700-shipping");
      await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, profileAfterTuple, "Gina Gold", "Gold", "profile-c700-after");
      await SaveProfileAsync(saveService, context, metadata, secondCustomerHashKey, profileBeforeTuple, "Hank Hazel", "Silver", "profile-c800-before");
      await SaveContactAsync(saveService, context, metadata, secondCustomerHashKey, billingContact, "billing", "billing-c800@example.test", "contact-c800-billing");
      await SaveContactAsync(saveService, context, metadata, secondCustomerHashKey, shippingContact, "shipping", "shipping-c800@example.test", "contact-c800-shipping");
      await SaveProfileAsync(saveService, context, metadata, secondCustomerHashKey, profileAfterTuple, "Hank Gold", "Gold", "profile-c800-after");
      await maintenanceService.RebuildAsync(context, new DataVaultPitRebuildRequest(metadata.Pit));
    }

    DataVaultPitMaintenanceResult maintenanceResult;
    await using (var context = new MultiActivePitMaintenanceContext(options)) {
      await SaveContactAsync(
          saveService,
          context,
          metadata,
          firstCustomerHashKey,
          lateBillingContact,
          "billing",
          "billing-c700-updated@example.test",
          "contact-c700-billing-late");
      maintenanceResult = await maintenanceService.MaintainParentsAsync(
          context,
          new DataVaultPitParentMaintenanceRequest(metadata.Pit, [firstCustomerHashKey]));
    }

    await using (var context = new MultiActivePitMaintenanceContext(options)) {
      var pitRows = await ReadMultiActivePitRowsAsync(context);
      var firstRows = pitRows.Where(row => row.ParentHashKey == firstCustomerHashKey).ToArray();
      var secondRows = pitRows.Where(row => row.ParentHashKey == secondCustomerHashKey).ToArray();
      var readRecords = await readService.ReadPitRowsAsync(
          context,
          new DataVaultPitAsOfReadRequest(metadata.Pit, [firstCustomerHashKey], Utc(2026, 5, 11, 9, 45)));

      Assert.Equal(1, maintenanceResult.ParentHashKeyCount);
      Assert.Equal(4, maintenanceResult.RowsDeleted);
      Assert.Equal(5, maintenanceResult.RowsWritten);
      Assert.Collection(
          firstRows,
          row => AssertMultiActivePitRow(row, firstCustomerHashKey, "billing", billingContact, billingContact, profileBeforeTuple),
          row => AssertMultiActivePitRow(row, firstCustomerHashKey, "billing", lateBillingContact, lateBillingContact, profileBeforeTuple),
          row => AssertMultiActivePitRow(row, firstCustomerHashKey, "billing", profileAfterTuple, lateBillingContact, profileAfterTuple),
          row => AssertMultiActivePitRow(row, firstCustomerHashKey, "shipping", shippingContact, shippingContact, profileBeforeTuple),
          row => AssertMultiActivePitRow(row, firstCustomerHashKey, "shipping", profileAfterTuple, shippingContact, profileAfterTuple));
      Assert.Collection(
          secondRows,
          row => AssertMultiActivePitRow(row, secondCustomerHashKey, "billing", billingContact, billingContact, profileBeforeTuple),
          row => AssertMultiActivePitRow(row, secondCustomerHashKey, "billing", profileAfterTuple, billingContact, profileAfterTuple),
          row => AssertMultiActivePitRow(row, secondCustomerHashKey, "shipping", shippingContact, shippingContact, profileBeforeTuple),
          row => AssertMultiActivePitRow(row, secondCustomerHashKey, "shipping", profileAfterTuple, shippingContact, profileAfterTuple));

      var record = Assert.Single(readRecords);
      Assert.Equal(lateBillingContact, record.LoadTimestamp);
      Assert.Equal("billing", record.DrivingKeyValues["Contact Type"]);
      Assert.Equal(lateBillingContact, RequiredSnapshot(record, "Contact").SnapshotLoadTimestamp);
      Assert.Equal(profileBeforeTuple, RequiredSnapshot(record, "Profile").SnapshotLoadTimestamp);
      Assert.Equal("billing-c700-updated@example.test", RequiredSnapshot(record, "Contact").PayloadValues["Email Address"]);
    }
  }

  [Fact]
  public async Task PitMaintenanceRejectsIncompatibleMultiActiveDrivingKeyFamiliesThroughSqlite() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<IncompatibleMultiActivePitMaintenanceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;

    await using var context = new IncompatibleMultiActivePitMaintenanceContext(options);

    var exception = await Assert.ThrowsAsync<NotSupportedException>(() => context.Database.EnsureCreatedAsync());

    Assert.Contains("do not match multi-active satellite 'Contact' driving-key names", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task PitMaintenanceMaintainsOnlyRequestedParentsAndCorrectsLateArrivingSatelliteHistoryThroughSqlite() {
    var metadata = CreateMetadata();
    var importTimestamp = Utc(2026, 5, 11, 8, 0);
    var statusTimestamp = Utc(2026, 5, 11, 9, 0);
    var profileTimestamp = Utc(2026, 5, 11, 10, 0);
    var lateProfileTimestamp = Utc(2026, 5, 11, 8, 30);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database.DatabasePath, DataVaultLoadTimestampStorage.ProviderDefault);
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    string firstCustomerHashKey;
    string secondCustomerHashKey;

    await using (var context = new PitMaintenanceContext(options, DataVaultLoadTimestampStorage.ProviderDefault)) {
      await context.Database.EnsureCreatedAsync();
      firstCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-100", importTimestamp);
      secondCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-200", importTimestamp);
      await SaveStatusAsync(saveService, context, metadata, firstCustomerHashKey, statusTimestamp, "Active", "status-c100");
      await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, profileTimestamp, "Alice Adams", "Gold", "profile-c100");
      await SaveStatusAsync(saveService, context, metadata, secondCustomerHashKey, statusTimestamp, "Prospect", "status-c200");
      await SaveProfileAsync(saveService, context, metadata, secondCustomerHashKey, profileTimestamp, "Bob Brown", "Silver", "profile-c200");
      await maintenanceService.RebuildAsync(context, new DataVaultPitRebuildRequest(metadata.Pit));
    }

    DataVaultPitMaintenanceResult maintenanceResult;
    await using (var context = new PitMaintenanceContext(options, DataVaultLoadTimestampStorage.ProviderDefault)) {
      await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, lateProfileTimestamp, "Alice A.", "Bronze", "profile-c100-late");
      maintenanceResult = await maintenanceService.MaintainParentsAsync(
          context,
          new DataVaultPitParentMaintenanceRequest(metadata.Pit, [firstCustomerHashKey]));
    }

    await using (var context = new PitMaintenanceContext(options, DataVaultLoadTimestampStorage.ProviderDefault)) {
      var pitRows = await ReadPitRowsAsync(context);
      var firstRows = pitRows.Where(row => row.ParentHashKey == firstCustomerHashKey).ToArray();
      var secondRows = pitRows.Where(row => row.ParentHashKey == secondCustomerHashKey).ToArray();
      var readRecords = await readService.ReadPitRowsAsync(
          context,
          new DataVaultPitAsOfReadRequest(metadata.Pit, [firstCustomerHashKey], Utc(2026, 5, 11, 9, 15)));

      Assert.Equal(1, maintenanceResult.ParentHashKeyCount);
      Assert.Equal(2, maintenanceResult.RowsDeleted);
      Assert.Equal(3, maintenanceResult.RowsWritten);
      Assert.Collection(
          firstRows,
          row => AssertPitRow(row, firstCustomerHashKey, lateProfileTimestamp, lateProfileTimestamp, null),
          row => AssertPitRow(row, firstCustomerHashKey, statusTimestamp, lateProfileTimestamp, statusTimestamp),
          row => AssertPitRow(row, firstCustomerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));
      Assert.Collection(
          secondRows,
          row => AssertPitRow(row, secondCustomerHashKey, statusTimestamp, null, statusTimestamp),
          row => AssertPitRow(row, secondCustomerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));

      var record = Assert.Single(readRecords);
      Assert.Equal(statusTimestamp, record.LoadTimestamp);
      Assert.Equal(lateProfileTimestamp, RequiredSnapshot(record, "Profile").SnapshotLoadTimestamp);
      Assert.Equal(statusTimestamp, RequiredSnapshot(record, "Status").SnapshotLoadTimestamp);
    }
  }

  [Fact]
  public async Task LinkParentPitMaintenanceRebuildsAndReadsRowsThroughProviderNeutralFallback() {
    var metadata = CreateLinkParentMetadata();
    var importTimestamp = Utc(2026, 5, 11, 8, 0);
    var stateTimestamp = Utc(2026, 5, 11, 9, 0);
    var fulfillmentTimestamp = Utc(2026, 5, 11, 10, 0);
    var secondStateTimestamp = Utc(2026, 5, 11, 11, 0);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateLinkParentOptions(database.DatabasePath);
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    string linkHashKey;
    DataVaultPitMaintenanceResult maintenanceResult;

    await using (var context = new LinkParentPitMaintenanceContext(options)) {
      await context.Database.EnsureCreatedAsync();
      linkHashKey = await SaveCustomerOrderLinkAsync(saveService, context, metadata, importTimestamp);
      await SaveLinkStateAsync(saveService, context, metadata, linkHashKey, stateTimestamp, "Packed", "state-1");
      await SaveLinkFulfillmentAsync(saveService, context, metadata, linkHashKey, fulfillmentTimestamp, "Dock 12", "fulfillment-1");
      await SaveLinkStateAsync(saveService, context, metadata, linkHashKey, secondStateTimestamp, "Shipped", "state-2");

      context.Set<Dictionary<string, object>>("PitCustomerOrderStateFulfillment").Add(CreateLinkParentPitRow(
          linkHashKey,
          Utc(2026, 5, 11, 8, 30),
          stateSnapshotTimestamp: null,
          fulfillmentSnapshotTimestamp: null));
      await context.SaveChangesAsync();

      maintenanceResult = await maintenanceService.RebuildAsync(
          context,
          new DataVaultPitRebuildRequest(metadata.Pit));
    }

    await using (var context = new LinkParentPitMaintenanceContext(options)) {
      var request = new DataVaultPitAsOfReadRequest(metadata.Pit, [linkHashKey], Utc(2026, 5, 11, 10, 30));
      var diagnostics = readDiagnostics.Analyze(context, request);
      var pitRows = await ReadLinkParentPitRowsAsync(context);
      var readRecords = await readService.ReadPitRowsAsync(context, request);
      var projectedRows = await readService.ReadPitAsync(
          context,
          request,
          row => new LinkParentSnapshotRead(
              row.RequiredString("ParentHashKey"),
              row.RequiredDateTimeOffset("LoadTimestamp"),
              row.RequiredSatellite("State").RequiredString("State Code"),
              row.RequiredSatellite("Fulfillment").RequiredString("Fulfillment Location")));

      Assert.Equal("PitCustomerOrderStateFulfillment", maintenanceResult.TableName);
      Assert.Equal(1, maintenanceResult.ParentHashKeyCount);
      Assert.Equal(1, maintenanceResult.RowsDeleted);
      Assert.Equal(3, maintenanceResult.RowsWritten);
      Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, diagnostics.ReadStrategy.Status);
      Assert.Contains(
          diagnostics.ReadStrategy.FallbackCauses,
          cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape);
      Assert.Collection(
          pitRows,
          row => AssertLinkParentPitRow(row, linkHashKey, stateTimestamp, stateTimestamp, null),
          row => AssertLinkParentPitRow(row, linkHashKey, fulfillmentTimestamp, stateTimestamp, fulfillmentTimestamp),
          row => AssertLinkParentPitRow(row, linkHashKey, secondStateTimestamp, secondStateTimestamp, fulfillmentTimestamp));

      var record = Assert.Single(readRecords);
      Assert.Equal(linkHashKey, record.ParentHashKey);
      Assert.Equal(fulfillmentTimestamp, record.LoadTimestamp);
      Assert.Equal(stateTimestamp, RequiredSnapshot(record, "State").SnapshotLoadTimestamp);
      Assert.Equal(fulfillmentTimestamp, RequiredSnapshot(record, "Fulfillment").SnapshotLoadTimestamp);
      var projectedRow = Assert.Single(projectedRows);
      Assert.Equal(linkHashKey, projectedRow.ParentHashKey);
      Assert.Equal(fulfillmentTimestamp, projectedRow.LoadTimestamp);
      Assert.Equal("Packed", projectedRow.StateCode);
      Assert.Equal("Dock 12", projectedRow.FulfillmentLocation);
    }
  }

  [Fact]
  public async Task RegistryBackedPitMaintenanceRebuildsByNameThroughSqlite() {
    var metadata = CreateMetadata();
    var importTimestamp = Utc(2026, 5, 11, 8, 0);
    var statusTimestamp = Utc(2026, 5, 11, 9, 0);
    var profileTimestamp = Utc(2026, 5, 11, 10, 0);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    using var provider = CreateRegistryProvider(database.DatabasePath);
    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RegistryPitMaintenanceContext>();
    var saveService = scope.ServiceProvider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = scope.ServiceProvider.GetRequiredService<IDataVaultPitMaintenanceService>();
    await context.Database.EnsureCreatedAsync();

    var customerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-300", importTimestamp);
    await SaveStatusAsync(saveService, context, metadata, customerHashKey, statusTimestamp, "Active", "status-c300");
    await SaveProfileAsync(saveService, context, metadata, customerHashKey, profileTimestamp, "Carol Clark", "Gold", "profile-c300");
    context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(CreatePitRow(
        DataVaultLoadTimestampStorage.ProviderDefault,
        customerHashKey,
        Utc(2026, 5, 11, 8, 30),
        profileSnapshotTimestamp: null,
        statusSnapshotTimestamp: null));
    await context.SaveChangesAsync();

    var result = await maintenanceService.RebuildAsync(
        context,
        new DataVaultRegistryPitRebuildRequest(metadata.Pit.Name));
    var pitRows = await ReadPitRowsAsync(context);

    Assert.Equal("PitCustomerProfileStatus", result.TableName);
    Assert.Equal(1, result.RowsDeleted);
    Assert.Equal(2, result.RowsWritten);
    Assert.Collection(
        pitRows,
        row => AssertPitRow(row, customerHashKey, statusTimestamp, null, statusTimestamp),
        row => AssertPitRow(row, customerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));
  }

  [Fact]
  public async Task RegistryBackedPitMaintenanceMaintainsParentsByClrMappingThroughSqlite() {
    var metadata = CreateMetadata();
    var importTimestamp = Utc(2026, 5, 11, 8, 0);
    var statusTimestamp = Utc(2026, 5, 11, 9, 0);
    var profileTimestamp = Utc(2026, 5, 11, 10, 0);
    var lateProfileTimestamp = Utc(2026, 5, 11, 8, 30);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    using var provider = CreateRegistryProvider(database.DatabasePath);
    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RegistryPitMaintenanceContext>();
    var saveService = scope.ServiceProvider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = scope.ServiceProvider.GetRequiredService<IDataVaultPitMaintenanceService>();
    await context.Database.EnsureCreatedAsync();

    var firstCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-400", importTimestamp);
    var secondCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-500", importTimestamp);
    await SaveStatusAsync(saveService, context, metadata, firstCustomerHashKey, statusTimestamp, "Active", "status-c400");
    await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, profileTimestamp, "Drew Davis", "Gold", "profile-c400");
    await SaveStatusAsync(saveService, context, metadata, secondCustomerHashKey, statusTimestamp, "Prospect", "status-c500");
    await SaveProfileAsync(saveService, context, metadata, secondCustomerHashKey, profileTimestamp, "Evan Evans", "Silver", "profile-c500");
    await maintenanceService.RebuildAsync(context, new DataVaultRegistryPitRebuildRequest(metadata.Pit.Name));
    await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, lateProfileTimestamp, "Drew D.", "Bronze", "profile-c400-late");

    var result = await maintenanceService.MaintainParentsAsync(
        context,
        new DataVaultRegistryPitParentMaintenanceRequest(
            typeof(CustomerProfileStatusPitMapping),
            [firstCustomerHashKey]));
    var pitRows = await ReadPitRowsAsync(context);
    var firstRows = pitRows.Where(row => row.ParentHashKey == firstCustomerHashKey).ToArray();
    var secondRows = pitRows.Where(row => row.ParentHashKey == secondCustomerHashKey).ToArray();

    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(2, result.RowsDeleted);
    Assert.Equal(3, result.RowsWritten);
    Assert.Collection(
        firstRows,
        row => AssertPitRow(row, firstCustomerHashKey, lateProfileTimestamp, lateProfileTimestamp, null),
        row => AssertPitRow(row, firstCustomerHashKey, statusTimestamp, lateProfileTimestamp, statusTimestamp),
        row => AssertPitRow(row, firstCustomerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));
    Assert.Collection(
        secondRows,
        row => AssertPitRow(row, secondCustomerHashKey, statusTimestamp, null, statusTimestamp),
        row => AssertPitRow(row, secondCustomerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));
  }

  private static DbContextOptions<PitMaintenanceContext> CreateOptions(
      object? databasePath,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    return new DbContextOptionsBuilder<PitMaintenanceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(databasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, PitMaintenanceModelCacheKeyFactory>()
        .Options;
  }

  private static DbContextOptions<LinkParentPitMaintenanceContext> CreateLinkParentOptions(object? databasePath) {
    return new DbContextOptionsBuilder<LinkParentPitMaintenanceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(databasePath) + ";Pooling=False")
        .Options;
  }

  private static async Task<string> SaveCustomerAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      PitMaintenanceMetadata metadata,
      string customerId,
      DateTimeOffset loadTimestamp) {
    return await SaveCustomerAsync(saveService, context, metadata.Customer, customerId, loadTimestamp);
  }

  private static async Task<string> SaveCustomerAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      MultiActivePitMaintenanceMetadata metadata,
      string customerId,
      DateTimeOffset loadTimestamp) {
    return await SaveCustomerAsync(saveService, context, metadata.Customer, customerId, loadTimestamp);
  }

  private static async Task<string> SaveCustomerAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      DataVaultHubMetadata customer,
      string customerId,
      DateTimeOffset loadTimestamp) {
    var result = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-import",
            [new(customer, [new("Customer Id", customerId)])],
            []));

    return GetHashKey(result, DataVaultTableKind.Hub, "Customer");
  }

  private static Task SaveProfileAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      PitMaintenanceMetadata metadata,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      string customerName,
      string customerTier,
      string hashDiff) {
    return SaveProfileAsync(saveService, context, metadata.Profile, customerHashKey, loadTimestamp, customerName, customerTier, hashDiff);
  }

  private static Task SaveProfileAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      MultiActivePitMaintenanceMetadata metadata,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      string customerName,
      string customerTier,
      string hashDiff) {
    return SaveProfileAsync(saveService, context, metadata.Profile, customerHashKey, loadTimestamp, customerName, customerTier, hashDiff);
  }

  private static Task SaveProfileAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      DataVaultSatelliteMetadata profile,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      string customerName,
      string customerTier,
      string hashDiff) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-profile",
            [],
            [],
            [
                new(
                    profile,
                    customerHashKey,
                    [new("Customer Name", customerName), new("Customer Tier", customerTier)],
                    hashDiff),
            ]));
  }

  private static Task SaveStatusAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      PitMaintenanceMetadata metadata,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      string statusCode,
      string hashDiff) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-status",
            [],
            [],
            [
                new(
                    metadata.Status,
                    customerHashKey,
                    [new("Status Code", statusCode)],
                    hashDiff),
            ]));
  }

  private static Task SaveContactAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      MultiActivePitMaintenanceMetadata metadata,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      string contactType,
      string emailAddress,
      string hashDiff) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-contact",
            [],
            [],
            [
                new DataVaultSatelliteSaveOperation(
                    metadata.Contact,
                    customerHashKey,
                    [new("Contact Type", contactType)],
                    [new("Email Address", emailAddress)],
                    hashDiff),
            ]));
  }

  private static async Task<string> SaveCustomerOrderLinkAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      LinkParentPitMetadata metadata,
      DateTimeOffset loadTimestamp) {
    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "order-import",
            [
                new(metadata.Customer, [new("Customer Id", "C-100")]),
                new(metadata.Order, [new("Order Id", "O-100")]),
            ],
            []));
    var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
    var orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");
    var linkResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "order-link",
            [],
            [
                new DataVaultLinkSaveOperation(
                    metadata.CustomerOrder,
                    [
                        new KeyValuePair<string, string>("Customer", customerHashKey),
                        new KeyValuePair<string, string>("Order", orderHashKey),
                    ]),
            ]));

    return GetHashKey(linkResult, DataVaultTableKind.Link, "CustomerOrder");
  }

  private static Task SaveLinkStateAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      LinkParentPitMetadata metadata,
      string linkHashKey,
      DateTimeOffset loadTimestamp,
      string stateCode,
      string hashDiff) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "order-state",
            [],
            [],
            [
                new(
                    metadata.State,
                    linkHashKey,
                    [new("State Code", stateCode)],
                    hashDiff),
            ]));
  }

  private static Task SaveLinkFulfillmentAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      LinkParentPitMetadata metadata,
      string linkHashKey,
      DateTimeOffset loadTimestamp,
      string location,
      string hashDiff) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "order-fulfillment",
            [],
            [],
            [
                new(
                    metadata.Fulfillment,
                    linkHashKey,
                    [new("Fulfillment Location", location)],
                    hashDiff),
            ]));
  }

  private static Dictionary<string, object> CreatePitRow(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      string parentHashKey,
      DateTimeOffset pitLoadTimestamp,
      DateTimeOffset? profileSnapshotTimestamp,
      DateTimeOffset? statusSnapshotTimestamp) {
    return new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["LoadTimestamp"] = ToStoredTimestamp(loadTimestampStorage, pitLoadTimestamp),
      ["ProfileLoadTimestamp"] = profileSnapshotTimestamp.HasValue
          ? ToStoredTimestamp(loadTimestampStorage, profileSnapshotTimestamp.Value)
          : null!,
      ["StatusLoadTimestamp"] = statusSnapshotTimestamp.HasValue
          ? ToStoredTimestamp(loadTimestampStorage, statusSnapshotTimestamp.Value)
          : null!,
    };
  }

  private static Dictionary<string, object> CreateLinkParentPitRow(
      string parentHashKey,
      DateTimeOffset pitLoadTimestamp,
      DateTimeOffset? stateSnapshotTimestamp,
      DateTimeOffset? fulfillmentSnapshotTimestamp) {
    return new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerOrderHashKey"] = parentHashKey,
      ["LoadTimestamp"] = pitLoadTimestamp,
      ["StateLoadTimestamp"] = stateSnapshotTimestamp.HasValue
          ? stateSnapshotTimestamp.Value
          : null!,
      ["FulfillmentLoadTimestamp"] = fulfillmentSnapshotTimestamp.HasValue
          ? fulfillmentSnapshotTimestamp.Value
          : null!,
    };
  }

  private static object ToStoredTimestamp(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      DateTimeOffset timestamp) {
    var utcTimestamp = timestamp.ToUniversalTime();
    return loadTimestampStorage switch {
      DataVaultLoadTimestampStorage.Iso8601UtcText => utcTimestamp.ToString("O", CultureInfo.InvariantCulture),
      DataVaultLoadTimestampStorage.UtcTicks => utcTimestamp.UtcDateTime.Ticks,
      _ => utcTimestamp,
    };
  }

  private static async Task<IReadOnlyList<PitRow>> ReadPitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerProfileStatus")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new PitRow(
            Assert.IsType<string>(row["CustomerHashKey"]),
            DataVaultLoadTimestampValueConverter.ReadProviderValue(row["LoadTimestamp"]),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp"),
            ReadOptionalTimestamp(row, "StatusLoadTimestamp")))
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static async Task<IReadOnlyList<MultiActivePitRow>> ReadMultiActivePitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerContactProfile")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new MultiActivePitRow(
            Assert.IsType<string>(row["CustomerHashKey"]),
            Assert.IsType<string>(row["ContactType"]),
            DataVaultLoadTimestampValueConverter.ReadProviderValue(row["LoadTimestamp"]),
            ReadOptionalTimestamp(row, "ContactLoadTimestamp"),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp")))
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => row.ContactType, StringComparer.Ordinal)
        .ThenBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static async Task<IReadOnlyList<LinkParentPitRow>> ReadLinkParentPitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerOrderStateFulfillment")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new LinkParentPitRow(
            Assert.IsType<string>(row["CustomerOrderHashKey"]),
            DataVaultLoadTimestampValueConverter.ReadProviderValue(row["LoadTimestamp"]),
            ReadOptionalTimestamp(row, "StateLoadTimestamp"),
            ReadOptionalTimestamp(row, "FulfillmentLoadTimestamp")))
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static DateTimeOffset? ReadOptionalTimestamp(
      IReadOnlyDictionary<string, object> row,
      string columnName) {
    return row.TryGetValue(columnName, out var value) && value is not null
        ? DataVaultLoadTimestampValueConverter.ReadProviderValue(value)
        : null;
  }

  private static DataVaultPitSatelliteSnapshot RequiredSnapshot(
      DataVaultPitReadRecord record,
      string satelliteName) {
    Assert.True(record.SatelliteSnapshotsByName.TryGetValue(satelliteName, out var snapshot));
    return snapshot!;
  }

  private static void AssertPitRow(
      PitRow row,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? profileSnapshotTimestamp,
      DateTimeOffset? statusSnapshotTimestamp) {
    Assert.Equal(parentHashKey, row.ParentHashKey);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(profileSnapshotTimestamp, row.ProfileSnapshotTimestamp);
    Assert.Equal(statusSnapshotTimestamp, row.StatusSnapshotTimestamp);
  }

  private static void AssertMultiActivePitRow(
      MultiActivePitRow row,
      string parentHashKey,
      string contactType,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? contactSnapshotTimestamp,
      DateTimeOffset? profileSnapshotTimestamp) {
    Assert.Equal(parentHashKey, row.ParentHashKey);
    Assert.Equal(contactType, row.ContactType);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(contactSnapshotTimestamp, row.ContactSnapshotTimestamp);
    Assert.Equal(profileSnapshotTimestamp, row.ProfileSnapshotTimestamp);
  }

  private static void AssertLinkParentPitRow(
      LinkParentPitRow row,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? stateSnapshotTimestamp,
      DateTimeOffset? fulfillmentSnapshotTimestamp) {
    Assert.Equal(parentHashKey, row.ParentHashKey);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(stateSnapshotTimestamp, row.StateSnapshotTimestamp);
    Assert.Equal(fulfillmentSnapshotTimestamp, row.FulfillmentSnapshotTimestamp);
  }

  private static PitMaintenanceMetadata CreateMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name", "Customer Tier"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var model = new DataVaultMetadataModel([customer], [], [profile, status], [pit]);

    return new PitMaintenanceMetadata(customer, profile, status, pit, model);
  }

  private static MultiActivePitMaintenanceMetadata CreateMultiActiveMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name", "Customer Tier"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);
    var pit = new DataVaultPitMetadata(
        customer.ToReference(),
        [
            new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
            new DataVaultPitSatelliteReferenceMetadata("Profile"),
        ]);
    var model = new DataVaultMetadataModel([customer], [], [profile, contact], [pit]);

    return new MultiActivePitMaintenanceMetadata(customer, profile, contact, pit, model);
  }

  private static DataVaultMetadataModel CreateIncompatibleMultiActiveMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);
    var preference = new DataVaultSatelliteMetadata(
        "Preference",
        customer.ToReference(),
        ["Preference Value"],
        ["Preference Channel"]);
    var pit = new DataVaultPitMetadata(
        customer.ToReference(),
        [
            new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
            new DataVaultPitSatelliteReferenceMetadata("Preference", isMultiActive: true),
        ]);

    return new DataVaultMetadataModel([customer], [], [contact, preference], [pit]);
  }

  private static LinkParentPitMetadata CreateLinkParentMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var state = new DataVaultSatelliteMetadata(
        "State",
        customerOrder.ToReference(),
        ["State Code"]);
    var fulfillment = new DataVaultSatelliteMetadata(
        "Fulfillment",
        customerOrder.ToReference(),
        ["Fulfillment Location"]);
    var pit = new DataVaultPitMetadata(customerOrder.ToReference(), ["State", "Fulfillment"]);
    var model = new DataVaultMetadataModel([customer, order], [customerOrder], [state, fulfillment], [pit]);

    return new LinkParentPitMetadata(customer, order, customerOrder, state, fulfillment, pit, model);
  }

  private static ServiceProvider CreateRegistryProvider(object? databasePath) {
    var registry = DataVaultMetadataRegistry.Create(
        CreateMetadata().Model,
        [],
        [DataVaultMetadataClrMapping.Pit<CustomerProfileStatusPitMapping>("CustomerProfileStatus")]);
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataRegistry(registry));
    services.AddDVaultSqlite();
    services.AddDbContext<RegistryPitMaintenanceContext>(options => options
        .UseSqlite("Data Source=" + Assert.IsType<string>(databasePath) + ";Pooling=False")
        .UseDataVaultMetadata());

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static string GetHashKey(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
  }

  private static DateTimeOffset Utc(int year, int month, int day, int hour, int minute) {
    return new DateTimeOffset(year, month, day, hour, minute, 0, TimeSpan.Zero);
  }

  private sealed class PitMaintenanceContext(
      DbContextOptions<PitMaintenanceContext> options,
      DataVaultLoadTimestampStorage loadTimestampStorage) : DbContext(options) {
    public DataVaultLoadTimestampStorage LoadTimestampStorage { get; } = loadTimestampStorage;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateMetadata().Model,
          DataVaultProviderCapabilityProfiles.Sqlite,
          LoadTimestampStorage);
    }
  }

  private sealed class PitMaintenanceModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is PitMaintenanceContext pitMaintenanceContext
          ? (context.GetType(), pitMaintenanceContext.LoadTimestampStorage, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private sealed class RegistryPitMaintenanceContext(DbContextOptions<RegistryPitMaintenanceContext> options) : DbContext(options) {
  }

  private sealed class MultiActivePitMaintenanceContext(DbContextOptions<MultiActivePitMaintenanceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateMultiActiveMetadata().Model,
          DataVaultProviderCapabilityProfiles.Sqlite);
    }
  }

  private sealed class IncompatibleMultiActivePitMaintenanceContext(
      DbContextOptions<IncompatibleMultiActivePitMaintenanceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateIncompatibleMultiActiveMetadata(),
          DataVaultProviderCapabilityProfiles.Sqlite);
    }
  }

  private sealed class LinkParentPitMaintenanceContext(DbContextOptions<LinkParentPitMaintenanceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateLinkParentMetadata().Model,
          DataVaultProviderCapabilityProfiles.Sqlite);
    }
  }

  private sealed class CustomerProfileStatusPitMapping {
  }

  private sealed record PitMaintenanceMetadata(
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile,
      DataVaultSatelliteMetadata Status,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record MultiActivePitMaintenanceMetadata(
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile,
      DataVaultSatelliteMetadata Contact,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record LinkParentPitMetadata(
      DataVaultHubMetadata Customer,
      DataVaultHubMetadata Order,
      DataVaultLinkMetadata CustomerOrder,
      DataVaultSatelliteMetadata State,
      DataVaultSatelliteMetadata Fulfillment,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record PitRow(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ProfileSnapshotTimestamp,
      DateTimeOffset? StatusSnapshotTimestamp);

  private sealed record MultiActivePitRow(
      string ParentHashKey,
      string ContactType,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ContactSnapshotTimestamp,
      DateTimeOffset? ProfileSnapshotTimestamp);

  private sealed record ContactSnapshotRead(
      string ParentHashKey,
      string ContactType,
      string EmailAddress,
      string CustomerName);

  private sealed record LinkParentPitRow(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? StateSnapshotTimestamp,
      DateTimeOffset? FulfillmentSnapshotTimestamp);

  private sealed record LinkParentSnapshotRead(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      string StateCode,
      string FulfillmentLocation);
}
