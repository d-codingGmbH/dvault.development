using System.Collections;
using System.Reflection;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class ExplicitDataVaultSaveServiceTests {
  [Fact]
  public void AddDVaultProvidesDefaultExplicitSaveServiceWithoutSaveChangesInterceptor() {
    var services = new ServiceCollection();

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.NotNull(provider.GetRequiredService<IDataVaultSaveService>());
    Assert.NotNull(provider.GetRequiredService<IDataVaultReadService>());
    Assert.NotNull(provider.GetRequiredService<IDataVaultBridgeMaintenanceService>());
    Assert.Empty(provider.GetServices<ISaveChangesInterceptor>());
  }

  [Fact]
  public void AddDVaultPreservesCallerExplicitSaveServiceOverride() {
    var replacement = new ReplacementDataVaultSaveService();
    var services = new ServiceCollection();
    services.AddSingleton<IDataVaultSaveService>(replacement);

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.Same(replacement, provider.GetRequiredService<IDataVaultSaveService>());
  }

  [Fact]
  public void AddDVaultPreservesCallerBridgeMaintenanceServiceOverride() {
    var replacement = new ReplacementDataVaultBridgeMaintenanceService();
    var services = new ServiceCollection();
    services.AddSingleton<IDataVaultBridgeMaintenanceService>(replacement);

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.Same(replacement, provider.GetRequiredService<IDataVaultBridgeMaintenanceService>());
  }

  [Fact]
  public void AddDVaultProvidesDefaultTimestampAndRecordSourceResolvers() {
    var services = new ServiceCollection();

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var request = CreateCustomerSaveRequest("crm-import");
    var timestampResolver = provider.GetRequiredService<IDataVaultLoadTimestampResolver>();
    var recordSourceResolver = provider.GetRequiredService<IDataVaultRecordSourceResolver>();

    Assert.Equal(
        request.LoadTimestamp,
        timestampResolver.ResolveLoadTimestamp(new DataVaultLoadTimestampResolutionContext(request)));
    Assert.Equal(
        request.RecordSource,
        recordSourceResolver.ResolveRecordSource(new DataVaultRecordSourceResolutionContext(request, request.LoadTimestamp)));
  }

  [Fact]
  public void AddDVaultConfiguresOptionalTimestampAndRecordSourceResolvers() {
    var timestampResolver = new FixedLoadTimestampResolver(new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero));
    var recordSourceResolver = new FixedRecordSourceResolver("hooked-source");
    var services = new ServiceCollection();

    services.AddDVault(options => options
        .UseLoadTimestampResolver(timestampResolver)
        .UseRecordSourceResolver(recordSourceResolver));

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.Same(timestampResolver, provider.GetRequiredService<IDataVaultLoadTimestampResolver>());
    Assert.Same(recordSourceResolver, provider.GetRequiredService<IDataVaultRecordSourceResolver>());
    Assert.NotNull(provider.GetRequiredService<IDataVaultSaveService>());
  }

  [Fact]
  public async Task SaveServiceResolvesHooksOncePerRequestBeforeProviderStrategyExecution() {
    var firstRequest = CreateCustomerSaveRequest("input-a");
    var secondRequest = CreateCustomerSaveRequest("input-b");
    var timestampResolver = new SequenceLoadTimestampResolver(
        new DateTimeOffset(2026, 5, 4, 10, 0, 0, TimeSpan.Zero),
        new DateTimeOffset(2026, 5, 4, 11, 0, 0, TimeSpan.Zero));
    var recordSourceResolver = new SequenceRecordSourceResolver("source-a", "source-b");
    var providerStrategy = new CapturingProviderSaveStrategy();
    var saveService = new DefaultDataVaultSaveService(
        new TestStableHashService(),
        new TestStableHashNormalizer(),
        [timestampResolver],
        [recordSourceResolver],
        [providerStrategy]);

    using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);

    await saveService.SaveAsync(dbContext, new DataVaultBulkSaveRequest([firstRequest, secondRequest]));

    Assert.Equal(2, timestampResolver.CallCount);
    Assert.Equal(2, recordSourceResolver.CallCount);
    Assert.NotNull(providerStrategy.CapturedContext);
    Assert.Collection(
        providerStrategy.CapturedContext.ResolvedRequests,
        request => {
          Assert.Same(firstRequest, request.Request);
          Assert.Equal(new DateTimeOffset(2026, 5, 4, 10, 0, 0, TimeSpan.Zero), request.LoadTimestamp);
          Assert.Equal("source-a", request.RecordSource);
        },
        request => {
          Assert.Same(secondRequest, request.Request);
          Assert.Equal(new DateTimeOffset(2026, 5, 4, 11, 0, 0, TimeSpan.Zero), request.LoadTimestamp);
          Assert.Equal("source-b", request.RecordSource);
        });
  }

  [Fact]
  public async Task SaveServiceRejectsNullLoadTimestampHookOutput() {
    var saveService = CreateHookedSaveService(
        new FixedLoadTimestampResolver(null),
        new FixedRecordSourceResolver("hooked-source"));

    using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        saveService.SaveAsync(dbContext, CreateCustomerSaveRequest("input-source")));

    Assert.Contains("load timestamp resolver returned null", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task SaveServiceRejectsNonUtcLoadTimestampHookOutput() {
    var saveService = CreateHookedSaveService(
        new FixedLoadTimestampResolver(new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.FromHours(2))),
        new FixedRecordSourceResolver("hooked-source"));

    using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        saveService.SaveAsync(dbContext, CreateCustomerSaveRequest("input-source")));

    Assert.Contains("zero offset", exception.Message, StringComparison.Ordinal);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public async Task SaveServiceRejectsEmptyRecordSourceHookOutput(string? recordSource) {
    var saveService = CreateHookedSaveService(
        new FixedLoadTimestampResolver(new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero)),
        new FixedRecordSourceResolver(recordSource));

    using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        saveService.SaveAsync(dbContext, CreateCustomerSaveRequest("input-source")));

    Assert.Contains("record-source resolver must return a non-empty record source", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void SaveServiceRejectsAmbiguousHookResolverConfiguration() {
    var exception = Assert.Throws<InvalidOperationException>(() =>
        new DefaultDataVaultSaveService(
            new TestStableHashService(),
            new TestStableHashNormalizer(),
            [
                new FixedLoadTimestampResolver(new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero)),
                new FixedLoadTimestampResolver(new DateTimeOffset(2026, 5, 4, 13, 0, 0, TimeSpan.Zero)),
            ],
            [new FixedRecordSourceResolver("hooked-source")],
            []));

    Assert.Contains("ambiguous", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.OracleProvider)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.MySqlProvider)]
  public void ProviderPackagesRegisterCoreSaveService() {
    AssertProviderRegistration(services => services.AddDVaultOracle(), expectProviderStrategy: true);
    AssertProviderRegistration(services => services.AddDVaultMySql(), expectProviderStrategy: true);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.PostgresProvider)]
  public void PostgresProviderPackageRegistersOptimizedSaveStrategy() {
    AssertProviderRegistration(services => services.AddDVaultPostgres(), expectProviderStrategy: true);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
  public void SqliteProviderPackageRegistersOptimizedSaveStrategy() {
    AssertProviderRegistration(services => services.AddDVaultSqlite(), expectProviderStrategy: true);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
  public void SqliteProviderPackageRegistersOptimizedReadStrategiesWithoutChangingAddDVaultFallback() {
    var fallbackServices = new ServiceCollection();
    fallbackServices.AddDVault();
    using var fallbackProvider = fallbackServices.BuildServiceProvider(validateScopes: true);

    Assert.Empty(fallbackProvider.GetServices<IDataVaultProviderReadStrategy>());
    Assert.Empty(fallbackProvider.GetServices<IDataVaultProviderPitReadStrategy>());
    Assert.Empty(fallbackProvider.GetServices<IDataVaultProviderBridgeReadStrategy>());

    var sqliteServices = new ServiceCollection();
    sqliteServices.AddDVaultSqlite();
    using var sqliteProvider = sqliteServices.BuildServiceProvider(validateScopes: true);

    Assert.Single(sqliteProvider.GetServices<IDataVaultProviderReadStrategy>());
    Assert.Single(sqliteProvider.GetServices<IDataVaultProviderPitReadStrategy>());
    Assert.Single(sqliteProvider.GetServices<IDataVaultProviderBridgeReadStrategy>());
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerProviderPackageRegistersOptimizedSaveStrategy() {
    AssertProviderRegistration(services => services.AddDVaultSqlServer(), expectProviderStrategy: true);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerProviderSaveStrategyAcceptsOnlyCleanSqlServerContexts() {
    var services = new ServiceCollection();
    services.AddDVaultSqlServer();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    Assert.Single(provider.GetServices<IDataVaultProviderSaveStrategy>());

    Assert.True(InvokeSqlServerCanSaveProvider("Microsoft.EntityFrameworkCore.SqlServer", hasPendingTrackedChanges: false));
    Assert.False(InvokeSqlServerCanSaveProvider("Microsoft.EntityFrameworkCore.SqlServer", hasPendingTrackedChanges: true));
    Assert.False(InvokeSqlServerCanSaveProvider("Microsoft.EntityFrameworkCore.Sqlite", hasPendingTrackedChanges: false));
    Assert.False(InvokeSqlServerCanSaveProvider(null, hasPendingTrackedChanges: false));
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerUniqueInsertSqlUsesSetBasedExistenceDetection() {
    var commandText = InvokeSqlServerCommandTextFactory(
        "CreateSqlServerUniqueInsertCommandText",
        "HubCustomer",
        new[] { "CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId" },
        "CustomerHashKey",
        3);

    Assert.Contains("INSERT INTO [HubCustomer]", commandText, StringComparison.Ordinal);
    Assert.Contains("VALUES (@p0, @p1, @p2, @p3, @p4)", commandText, StringComparison.Ordinal);
    Assert.Contains("(@p10, @p11, @p12, @p13, @p14)", commandText, StringComparison.Ordinal);
    Assert.Contains("ROW_NUMBER() OVER (PARTITION BY [source].[CustomerHashKey] ORDER BY [source].[__dvault_ordinal])", commandText, StringComparison.Ordinal);
    Assert.Contains("NOT EXISTS (SELECT 1 FROM [HubCustomer]", commandText, StringComparison.Ordinal);
    Assert.Equal(1, CountOccurrences(commandText, "NOT EXISTS"));
    Assert.Equal(1, CountOccurrences(commandText, "WITH (UPDLOCK, HOLDLOCK)"));
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerJsonUniqueInsertSqlUsesSinglePayloadParameterAndKeepsDeduplicationOrder() {
    var commandText = InvokeSqlServerCommandTextFactory(
        "CreateSqlServerJsonUniqueInsertCommandText",
        "HubCustomer",
        new[] { "CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId" },
        "CustomerHashKey",
        new Dictionary<string, string>(StringComparer.Ordinal) {
          ["__dvault_ordinal"] = "int",
          ["CustomerHashKey"] = "nvarchar(64)",
          ["LoadTimestamp"] = "datetimeoffset",
          ["RecordSource"] = "nvarchar(255)",
          ["CustomerId"] = "nvarchar(255)",
        });

    Assert.Contains("FROM OPENJSON(@p0) WITH", commandText, StringComparison.Ordinal);
    Assert.Contains("[__dvault_ordinal] int '$.\"__dvault_ordinal\"'", commandText, StringComparison.Ordinal);
    Assert.Contains("[LoadTimestamp] datetimeoffset '$.\"LoadTimestamp\"'", commandText, StringComparison.Ordinal);
    Assert.Contains("ROW_NUMBER() OVER (PARTITION BY [source].[CustomerHashKey] ORDER BY [source].[__dvault_ordinal])", commandText, StringComparison.Ordinal);
    Assert.Contains("NOT EXISTS (SELECT 1 FROM [HubCustomer]", commandText, StringComparison.Ordinal);
    Assert.DoesNotContain("VALUES (@p0", commandText, StringComparison.Ordinal);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerJsonInsertSqlUsesSinglePayloadParameterForLargeSatelliteBatches() {
    var commandText = InvokeSqlServerCommandTextFactory(
        "CreateSqlServerJsonInsertCommandText",
        "SatCustomerContact",
        new[] { "CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "EmailAddress" },
        new Dictionary<string, string>(StringComparer.Ordinal) {
          ["CustomerHashKey"] = "nvarchar(64)",
          ["HashDiff"] = "nvarchar(64)",
          ["LoadTimestamp"] = "datetimeoffset",
          ["RecordSource"] = "nvarchar(255)",
          ["EmailAddress"] = "nvarchar(max)",
        });

    Assert.Contains("INSERT INTO [SatCustomerContact]", commandText, StringComparison.Ordinal);
    Assert.Contains("SELECT [payload].[CustomerHashKey], [payload].[HashDiff]", commandText, StringComparison.Ordinal);
    Assert.Contains("FROM OPENJSON(@p0) WITH", commandText, StringComparison.Ordinal);
    Assert.Contains("[EmailAddress] nvarchar(max) '$.\"EmailAddress\"'", commandText, StringComparison.Ordinal);
    Assert.DoesNotContain("VALUES (@p0", commandText, StringComparison.Ordinal);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerSatelliteLookupSqlRanksLatestHashDiffsByParentBatch() {
    var commandText = InvokeSqlServerCommandTextFactory(
        "CreateSqlServerLatestSatelliteHashDiffCommandText",
        "SatCustomerContact",
        "CustomerHashKey",
        "HashDiff",
        "LoadTimestamp",
        2);

    Assert.Contains("FROM (VALUES (@p0), (@p1))", commandText, StringComparison.Ordinal);
    Assert.Contains("INNER JOIN [requested]", commandText, StringComparison.Ordinal);
    Assert.Contains("ROW_NUMBER() OVER (PARTITION BY [target].[CustomerHashKey] ORDER BY [target].[LoadTimestamp] DESC)", commandText, StringComparison.Ordinal);
    Assert.Contains("WHERE [__dvault_row_number] = 1", commandText, StringComparison.Ordinal);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerSatelliteFilterUsesLatestHashDiffAcrossOrderedBatch() {
    var latestHashDiff = "hash-a";
    var latestLoadTimestamp = new DateTimeOffset(2026, 5, 4, 10, 0, 0, TimeSpan.Zero);
    var candidates = new[] {
        new SatelliteDecisionCandidate("hash-a", new DateTimeOffset(2026, 5, 4, 10, 5, 0, TimeSpan.Zero)),
        new SatelliteDecisionCandidate("hash-b", new DateTimeOffset(2026, 5, 4, 10, 10, 0, TimeSpan.Zero)),
        new SatelliteDecisionCandidate("hash-b", new DateTimeOffset(2026, 5, 4, 10, 20, 0, TimeSpan.Zero)),
        new SatelliteDecisionCandidate("hash-c", new DateTimeOffset(2026, 5, 4, 9, 30, 0, TimeSpan.Zero)),
        new SatelliteDecisionCandidate("hash-b", new DateTimeOffset(2026, 5, 4, 10, 30, 0, TimeSpan.Zero)),
    };

    var rowWrittenDecisions = new List<bool>();
    foreach (var candidate in candidates) {
      var rowWritten = InvokeSqlServerBooleanFactory(
          "ShouldWriteSatelliteHashDiff",
          latestHashDiff,
          candidate.HashDiff);
      rowWrittenDecisions.Add(rowWritten);

      if (rowWritten && InvokeSqlServerBooleanFactory(
          "ShouldAdvanceLatestSatelliteHashDiff",
          latestLoadTimestamp,
          candidate.LoadTimestamp)) {
        latestHashDiff = candidate.HashDiff;
        latestLoadTimestamp = candidate.LoadTimestamp;
      }
    }

    Assert.Equal([false, true, false, true, false], rowWrittenDecisions);
    Assert.Equal(2, rowWrittenDecisions.Count(rowWritten => rowWritten));
    Assert.Equal("hash-b", latestHashDiff);
    Assert.Equal(new DateTimeOffset(2026, 5, 4, 10, 10, 0, TimeSpan.Zero), latestLoadTimestamp);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerSavePlansKeepFallbackSavedRecordOrderingForBulkRequests() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var contact = new DataVaultSatelliteMetadata("Contact", customer.ToReference(), ["Email"]);
    var firstRequest = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 4, 10, 0, 0, TimeSpan.Zero),
        "source-a",
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", "C-100")])],
        [new DataVaultLinkSaveOperation(customerOrder, [new("Customer", "customer-hash"), new("Order", "order-hash")])],
        [new DataVaultSatelliteSaveOperation(contact, "customer-hash", [new("Email", "a@example.test")], "hash-a")]);
    var secondRequest = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 4, 10, 5, 0, TimeSpan.Zero),
        "source-b",
        [new DataVaultHubSaveOperation(order, [new("Order Id", "O-100")])],
        [],
        [new DataVaultSatelliteSaveOperation(contact, "customer-hash", [new("Email", "b@example.test")], "hash-b")]);

    using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);
    var requests = new[] { firstRequest, secondRequest };
    var strategyContext = new DataVaultProviderSaveStrategyContext(
        dbContext,
        requests,
        new TestStableHashService(),
        new TestStableHashNormalizer());
    var savedRecords = InvokeSqlServerSavedRecords("CreateUniqueRowSavePlans", strategyContext)
        .Concat(InvokeSqlServerSavedRecords("CreateSatelliteSavePlans", strategyContext.ResolvedRequests))
        .ToArray();

    Assert.Collection(
        savedRecords,
        record => AssertSavedRecord(record, DataVaultTableKind.Hub, "Customer", "HubCustomer"),
        record => AssertSavedRecord(record, DataVaultTableKind.Link, "CustomerOrder", "LinkCustomerOrder"),
        record => AssertSavedRecord(record, DataVaultTableKind.Hub, "Order", "HubOrder"),
        record => AssertSavedRecord(record, DataVaultTableKind.Satellite, "Contact", "SatCustomerContact"),
        record => AssertSavedRecord(record, DataVaultTableKind.Satellite, "Contact", "SatCustomerContact"));
  }

  [Fact]
  public void SaveRequestKeepsExplicitMetadataBoundaryDeterministic() {
    var suppliedTimestamp = new DateTimeOffset(2026, 4, 29, 12, 15, 0, TimeSpan.FromHours(2));
    var hub = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var request = new DataVaultSaveRequest(
        suppliedTimestamp,
        "crm-import",
        [new DataVaultHubSaveOperation(hub, [new("Customer Id", "C-100")])],
        []);

    Assert.Equal(new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero), request.LoadTimestamp);
    Assert.Equal("crm-import", request.RecordSource);
    Assert.Single(request.HubOperations);
    Assert.Empty(request.LinkOperations);
  }

  [Fact]
  public void BulkSaveRequestKeepsCallerSuppliedOrder() {
    var first = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero),
        "first-source",
        [],
        []);
    var second = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 11, 15, 0, TimeSpan.Zero),
        "second-source",
        [],
        []);
    var bulkRequest = new DataVaultBulkSaveRequest([first, second]);

    Assert.Equal([first, second], bulkRequest.Requests);
  }

  [Fact]
  public void BulkSaveRequestRejectsNullRequests() {
    Assert.Throws<ArgumentNullException>(() => new DataVaultBulkSaveRequest(null!));
    Assert.Throws<ArgumentException>(() => new DataVaultBulkSaveRequest([null!]));
  }

  [Fact]
  public void ChunkedSaveRequestKeepsCallerSuppliedChunkAndRequestOrder() {
    var first = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero),
        "first-source",
        [],
        []);
    var second = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 11, 15, 0, TimeSpan.Zero),
        "second-source",
        [],
        []);
    var firstChunk = new DataVaultSaveChunk([first]);
    var secondChunk = new DataVaultSaveChunk([second]);
    var chunkedRequest = new DataVaultChunkedSaveRequest([firstChunk, secondChunk]);

    Assert.Equal([firstChunk, secondChunk], chunkedRequest.Chunks);
    Assert.Equal([first], firstChunk.Requests);
    Assert.Equal([second], secondChunk.Requests);
  }

  [Fact]
  public void ChunkedSaveRequestRejectsNullChunksAndRequests() {
    Assert.Throws<ArgumentNullException>(() => new DataVaultChunkedSaveRequest(null!));
    Assert.Throws<ArgumentException>(() => new DataVaultChunkedSaveRequest([null!]));
    Assert.Throws<ArgumentNullException>(() => new DataVaultSaveChunk(null!));
    Assert.Throws<ArgumentException>(() => new DataVaultSaveChunk([null!]));
  }

  [Fact]
  public void SaveOperationsRequireNamedValuesWithoutDuplicates() {
    var hub = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    Assert.Throws<ArgumentException>(() => new DataVaultHubSaveOperation(hub, [new("Customer Id", "C-100"), new("Customer Id", "C-101")]));
    Assert.Throws<ArgumentException>(() => new DataVaultHubSaveOperation(hub, [new(" ", "C-100")]));
    Assert.Throws<ArgumentException>(() => new DataVaultHubSaveOperation(hub, [new("Customer Id", null!)]));
  }

  [Fact]
  public void SatelliteSaveOperationValidatesMultiActiveDrivingKeyValuesExactly() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type", "Region Code"]);

    var operation = new DataVaultSatelliteSaveOperation(
        contact,
        "customer-hash",
        [new("Region Code", "DE"), new("Contact Type", "billing")],
        [new("Email Address", "billing@example.test")],
        "contact-hash");

    Assert.Equal(["Contact Type", "Region Code"], contact.DrivingKeyNames);
    Assert.Equal("billing", operation.DrivingKeyValues["Contact Type"]);
    Assert.Equal("DE", operation.DrivingKeyValues["Region Code"]);
    Assert.Throws<ArgumentException>(() => new DataVaultSatelliteSaveOperation(
        contact,
        "customer-hash",
        [new("Contact Type", "billing")],
        [new("Email Address", "billing@example.test")],
        "contact-hash"));
    Assert.Throws<ArgumentException>(() => new DataVaultSatelliteSaveOperation(
        contact,
        "customer-hash",
        [new("Contact Type", "billing"), new("Region Code", "DE"), new("Scope", "extra")],
        [new("Email Address", "billing@example.test")],
        "contact-hash"));
    Assert.Throws<ArgumentException>(() => new DataVaultSatelliteSaveOperation(
        contact,
        "customer-hash",
        [new("Contact Type", "billing"), new("Contact Type", "shipping"), new("Region Code", "DE")],
        [new("Email Address", "billing@example.test")],
        "contact-hash"));
    Assert.Throws<ArgumentException>(() => new DataVaultSatelliteSaveOperation(
        contact,
        "customer-hash",
        [new("Contact Type", null!), new("Region Code", "DE")],
        [new("Email Address", "billing@example.test")],
        "contact-hash"));
    Assert.Throws<ArgumentException>(() => new DataVaultSatelliteSaveOperation(
        contact,
        "customer-hash",
        [new("Email Address", "billing@example.test")],
        "contact-hash"));
  }

  private sealed class ReplacementDataVaultSaveService : IDataVaultSaveService {
    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultBulkSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultChunkedSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }
  }

  private sealed class ReplacementDataVaultBridgeMaintenanceService : IDataVaultBridgeMaintenanceService {
    public Task<DataVaultBridgeMaintenanceResult> RebuildBridgeAsync(
        DbContext dbContext,
        DataVaultBridgeMaintenanceRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public Task<DataVaultBridgeMaintenanceResult> MaintainBridgeAsync(
        DbContext dbContext,
        DataVaultBridgeMaintenanceRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }
  }

  private static DataVaultSaveRequest CreateCustomerSaveRequest(string recordSource) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 4, 9, 0, 0, TimeSpan.Zero),
        recordSource,
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", "C-100")])],
        []);
  }

  private static DefaultDataVaultSaveService CreateHookedSaveService(
      IDataVaultLoadTimestampResolver loadTimestampResolver,
      IDataVaultRecordSourceResolver recordSourceResolver) {
    return new DefaultDataVaultSaveService(
        new TestStableHashService(),
        new TestStableHashNormalizer(),
        [loadTimestampResolver],
        [recordSourceResolver],
        [new CapturingProviderSaveStrategy()]);
  }

  private static string InvokeSqlServerCommandTextFactory(string methodName, params object[] arguments) {
    var strategyType = typeof(DVaultSqlServerServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.SqlServerDataVaultSaveStrategy",
        throwOnError: true);
    var method = GetSqlServerStrategyMethod(strategyType!, methodName, arguments);

    Assert.NotNull(method);

    return Assert.IsType<string>(method.Invoke(null, arguments));
  }

  private static MethodInfo? GetSqlServerStrategyMethod(Type strategyType, string methodName, object?[] arguments) {
    return strategyType
        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .SingleOrDefault(method => {
          if (!string.Equals(method.Name, methodName, StringComparison.Ordinal)) {
            return false;
          }

          var parameters = method.GetParameters();
          return parameters.Length == arguments.Length &&
              parameters
                  .Zip(arguments)
                  .All(pair => pair.Second is null || pair.First.ParameterType.IsInstanceOfType(pair.Second));
        });
  }

  private static int CountOccurrences(string value, string searchValue) {
    var count = 0;
    var searchIndex = 0;

    while (true) {
      var matchIndex = value.IndexOf(searchValue, searchIndex, StringComparison.Ordinal);
      if (matchIndex < 0) {
        return count;
      }

      count++;
      searchIndex = matchIndex + searchValue.Length;
    }
  }

  private static IReadOnlyList<DataVaultSavedRecord> InvokeSqlServerSavedRecords(string methodName, params object[] arguments) {
    var strategyType = typeof(DVaultSqlServerServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.SqlServerDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);

    Assert.NotNull(method);

    var values = Assert.IsAssignableFrom<IEnumerable>(method.Invoke(null, arguments));

    return values
        .Cast<object>()
        .Select(value => {
          var savedRecordProperty = value.GetType().GetProperty("SavedRecord");

          Assert.NotNull(savedRecordProperty);

          return Assert.IsType<DataVaultSavedRecord>(savedRecordProperty.GetValue(value));
        })
        .ToArray();
  }

  private static bool InvokeSqlServerBooleanFactory(string methodName, params object?[] arguments) {
    var strategyType = typeof(DVaultSqlServerServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.SqlServerDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);

    Assert.NotNull(method);

    return Assert.IsType<bool>(method.Invoke(null, arguments));
  }

  private static void AssertSavedRecord(
      DataVaultSavedRecord record,
      DataVaultTableKind kind,
      string metadataName,
      string tableName) {
    Assert.Equal(kind, record.Kind);
    Assert.Equal(metadataName, record.MetadataName);
    Assert.Equal(tableName, record.TableName);
    Assert.False(string.IsNullOrWhiteSpace(record.HashKey));
  }

  private static void AssertProviderRegistration(
      Action<IServiceCollection> configure,
      bool expectProviderStrategy) {
    try {
      var services = new ServiceCollection();

      configure(services);

      using var provider = services.BuildServiceProvider(validateScopes: true);

      Assert.NotNull(provider.GetRequiredService<IDataVaultSaveService>());
      if (expectProviderStrategy) {
        Assert.NotEmpty(provider.GetServices<IDataVaultProviderSaveStrategy>());
      }
      else {
        Assert.Empty(provider.GetServices<IDataVaultProviderSaveStrategy>());
      }
    }
    finally {
      DataVaultProviderCapabilityProfileSelection.Reset();
    }
  }

  private static bool InvokeSqlServerCanSaveProvider(string? providerName, bool hasPendingTrackedChanges) {
    var strategyType = typeof(DVaultSqlServerServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.SqlServerDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!.GetMethod("CanSaveProvider", BindingFlags.Static | BindingFlags.NonPublic);

    Assert.NotNull(method);

    return Assert.IsType<bool>(method.Invoke(null, [providerName, hasPendingTrackedChanges]));
  }

  private sealed record SatelliteDecisionCandidate(string HashDiff, DateTimeOffset LoadTimestamp);

  private sealed class FixedLoadTimestampResolver(DateTimeOffset? loadTimestamp) : IDataVaultLoadTimestampResolver {
    public DateTimeOffset? ResolveLoadTimestamp(DataVaultLoadTimestampResolutionContext context) {
      ArgumentNullException.ThrowIfNull(context);

      return loadTimestamp;
    }
  }

  private sealed class FixedRecordSourceResolver(string? recordSource) : IDataVaultRecordSourceResolver {
    public string? ResolveRecordSource(DataVaultRecordSourceResolutionContext context) {
      ArgumentNullException.ThrowIfNull(context);

      return recordSource;
    }
  }

  private sealed class SequenceLoadTimestampResolver : IDataVaultLoadTimestampResolver {
    private readonly DateTimeOffset[] _loadTimestamps;

    public SequenceLoadTimestampResolver(params DateTimeOffset[] loadTimestamps) {
      _loadTimestamps = loadTimestamps;
    }

    public int CallCount { get; private set; }

    public DateTimeOffset? ResolveLoadTimestamp(DataVaultLoadTimestampResolutionContext context) {
      ArgumentNullException.ThrowIfNull(context);

      return _loadTimestamps[CallCount++];
    }
  }

  private sealed class SequenceRecordSourceResolver : IDataVaultRecordSourceResolver {
    private readonly string[] _recordSources;

    public SequenceRecordSourceResolver(params string[] recordSources) {
      _recordSources = recordSources;
    }

    public int CallCount { get; private set; }

    public string? ResolveRecordSource(DataVaultRecordSourceResolutionContext context) {
      ArgumentNullException.ThrowIfNull(context);

      return _recordSources[CallCount++];
    }
  }

  private sealed class CapturingProviderSaveStrategy : IDataVaultProviderSaveStrategy {
    public int Priority => 1000;

    public DataVaultProviderSaveStrategyContext? CapturedContext { get; private set; }

    public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(requests);

      return true;
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DataVaultProviderSaveStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      CapturedContext = context;
      return Task.FromResult(new DataVaultSaveResult(0, []));
    }
  }

  private sealed class TestStableHashService : IStableHashService {
    public string AlgorithmId => "test-sha256-v1";

    public StableHashDigest ComputeHash(string normalizedInput) {
      ArgumentNullException.ThrowIfNull(normalizedInput);

      return new StableHashDigest(AlgorithmId, new string('a', 64));
    }
  }

  private sealed class TestStableHashNormalizer : IStableHashNormalizer {
    public string NormalizeValue(object? value) {
      return value?.ToString() ?? string.Empty;
    }

    public string NormalizeFields(IEnumerable<KeyValuePair<string, object?>> fields) {
      ArgumentNullException.ThrowIfNull(fields);

      return string.Join(
          "\n",
          fields.Select(field => field.Key + "=" + NormalizeValue(field.Value)));
    }
  }
}
