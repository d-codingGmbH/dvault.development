using System.Globalization;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.Db2Provider)]
public sealed class Db2DataVaultSmokeTests {
  private const string Db2ProviderName = "IBM.EntityFrameworkCore";
  private const string CustomerId = "C-DB2-100";
  private const string OrderId = "O-DB2-200";
  private const string SaveRecordSource = "db2-smoke";
  private const string ReadRecordSource = "db2-read-smoke";
  private const string MissingCustomerHashKey = "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff";
  private static readonly DateTimeOffset HubLoadTimestamp = new(2026, 6, 1, 9, 0, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset LinkLoadTimestamp = new(2026, 6, 1, 9, 5, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset SatelliteLoadTimestamp = new(2026, 6, 1, 9, 10, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset FirstReadTimestamp = new(2026, 6, 1, 10, 0, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset SecondReadTimestamp = new(2026, 6, 1, 11, 0, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset OlderPitTimestamp = new(2026, 6, 1, 10, 30, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset SelectedPitTimestamp = new(2026, 6, 1, 11, 30, 0, TimeSpan.Zero);

  [Fact]
  public async Task AddDVaultDb2PersistsRepresentativeHubLinkAndSatelliteRowsWhenConfigured() {
    await using var database = await Db2SmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var metadata = Db2SmokeMetadata.Value;

    await using var context = database.CreateContext();

    Assert.Equal(Db2ProviderName, context.Database.ProviderName);

    var hubRequest = new DataVaultSaveRequest(
        HubLoadTimestamp,
        SaveRecordSource,
        [
            new(metadata.Customer, [new("Customer Id", CustomerId)]),
            new(metadata.Order, [new("Order Id", OrderId)]),
        ],
        []);

    AssertDb2SaveStrategyDiagnostics(diagnostics.Analyze(context, hubRequest));

    var hubResult = await saveService.SaveAsync(context, hubRequest);

    Assert.Equal(2, hubResult.RowsWritten);
    var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
    var orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");

    var linkRequest = new DataVaultSaveRequest(
        LinkLoadTimestamp,
        SaveRecordSource,
        [],
        [
            new(metadata.CustomerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)]),
        ]);

    AssertDb2SaveStrategyDiagnostics(diagnostics.Analyze(context, linkRequest));

    var linkResult = await saveService.SaveAsync(context, linkRequest);

    Assert.Equal(1, linkResult.RowsWritten);
    var customerOrderHashKey = GetHashKey(linkResult, DataVaultTableKind.Link, "CustomerOrder");

    var satelliteRequest = new DataVaultSaveRequest(
        SatelliteLoadTimestamp,
        SaveRecordSource,
        [],
        [],
        [
            new(
                metadata.Contact,
                customerHashKey,
                [new("Email Address", "db2-customer@example.test")],
                "db2-contact-hash-1"),
            new(
                metadata.State,
                customerOrderHashKey,
                [new("State Code", "PLACED")],
                "db2-state-hash-1"),
        ]);

    AssertDb2SaveStrategyDiagnostics(diagnostics.Analyze(context, satelliteRequest));

    var satelliteResult = await saveService.SaveAsync(context, satelliteRequest);

    Assert.Equal(2, satelliteResult.RowsWritten);

    context.ChangeTracker.Clear();

    var customerRow = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().SingleAsync();
    var orderRow = await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().SingleAsync();
    var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync();
    var contactRow = await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().SingleAsync();
    var stateRow = await context.Set<Dictionary<string, object>>("SatCustomerOrderState").AsNoTracking().SingleAsync();

    Assert.Equal(CustomerId, ReadString(customerRow, "CustomerId"));
    Assert.Equal(OrderId, ReadString(orderRow, "OrderId"));
    Assert.Equal(customerHashKey, ReadString(customerRow, "CustomerHashKey"));
    Assert.Equal(orderHashKey, ReadString(orderRow, "OrderHashKey"));
    Assert.Equal(customerOrderHashKey, ReadString(linkRow, "CustomerOrderHashKey"));
    Assert.Equal(SaveRecordSource, ReadString(customerRow, "RecordSource"));
    Assert.Equal(SaveRecordSource, ReadString(orderRow, "RecordSource"));
    Assert.Equal(SaveRecordSource, ReadString(linkRow, "RecordSource"));
    Assert.Equal(HubLoadTimestamp, ReadLoadTimestamp(customerRow));
    Assert.Equal(HubLoadTimestamp, ReadLoadTimestamp(orderRow));
    Assert.Equal(LinkLoadTimestamp, ReadLoadTimestamp(linkRow));
    Assert.Equal(customerHashKey, ReadString(linkRow, "CustomerHashKey"));
    Assert.Equal(orderHashKey, ReadString(linkRow, "OrderHashKey"));
    Assert.Equal(customerHashKey, ReadString(contactRow, "CustomerHashKey"));
    Assert.Equal("db2-customer@example.test", ReadString(contactRow, "EmailAddress"));
    Assert.Equal("db2-contact-hash-1", ReadString(contactRow, "HashDiff"));
    Assert.Equal(SatelliteLoadTimestamp, ReadLoadTimestamp(contactRow));
    Assert.Equal(SaveRecordSource, ReadString(contactRow, "RecordSource"));
    Assert.Equal(customerOrderHashKey, ReadString(stateRow, "CustomerOrderHashKey"));
    Assert.Equal("PLACED", ReadString(stateRow, "StateCode"));
    Assert.Equal("db2-state-hash-1", ReadString(stateRow, "HashDiff"));
    Assert.Equal(SatelliteLoadTimestamp, ReadLoadTimestamp(stateRow));
    Assert.Equal(SaveRecordSource, ReadString(stateRow, "RecordSource"));
  }

  [Fact]
  public async Task AddDVaultDb2ReadsLatestPitAndBridgeThroughProviderStrategiesWhenConfigured() {
    await using var database = await Db2SmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    var metadata = Db2SmokeMetadata.Value;

    await using var context = database.CreateContext();

    Assert.Equal(Db2ProviderName, context.Database.ProviderName);

    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            FirstReadTimestamp,
            ReadRecordSource,
            [
                new(metadata.Customer, [new("Customer Id", "C-DB2-READ")]),
                new(metadata.Order, [new("Order Id", "O-DB2-READ")]),
            ],
            []));
    var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
    var orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");
    var linkResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            FirstReadTimestamp,
            ReadRecordSource,
            [],
            [
                new(metadata.CustomerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)]),
            ]));
    var customerOrderHashKey = GetHashKey(linkResult, DataVaultTableKind.Link, "CustomerOrder");

    await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            FirstReadTimestamp,
            ReadRecordSource,
            [],
            [],
            [
                new(
                    metadata.Contact,
                    customerHashKey,
                    [new("Email Address", "first-db2@example.test")],
                    "db2-contact-read-hash-1"),
                new(
                    metadata.State,
                    customerOrderHashKey,
                    [new("State Code", "PLACED")],
                    "db2-state-read-hash-1"),
            ]));
    await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            SecondReadTimestamp,
            ReadRecordSource,
            [],
            [],
            [
                new(
                    metadata.Contact,
                    customerHashKey,
                    [new("Email Address", "second-db2@example.test")],
                    "db2-contact-read-hash-2"),
            ]));

    context.Set<Dictionary<string, object>>("PitCustomerContact").Add(
        CreatePitRow(customerHashKey, OlderPitTimestamp, FirstReadTimestamp));
    context.Set<Dictionary<string, object>>("PitCustomerContact").Add(
        CreatePitRow(customerHashKey, SelectedPitTimestamp, SecondReadTimestamp));
    context.Set<Dictionary<string, object>>("BridgeCustomerOrder").Add(
        new Dictionary<string, object>(StringComparer.Ordinal) {
          ["CustomerHashKey"] = customerHashKey,
          ["OrderHashKey"] = orderHashKey,
        });
    await context.SaveChangesAsync();
    context.ChangeTracker.Clear();

    var latestRequest = new DataVaultLatestSatelliteReadRequest(metadata.Contact, [customerHashKey]);
    var asOfRequest = new DataVaultLatestSatelliteReadRequest(metadata.Contact, [customerHashKey], FirstReadTimestamp);
    var pitRequest = new DataVaultPitAsOfReadRequest(
        metadata.Pit,
        [customerHashKey, MissingCustomerHashKey],
        SelectedPitTimestamp.AddMinutes(30));
    var bridgeRequest = new DataVaultBridgeReadRequest(
        metadata.Bridge,
        DataVaultBridgeTraversalEndpoint.From,
        [customerHashKey]);

    AssertDb2ReadStrategyDiagnostics(
        readDiagnostics.Analyze(context, latestRequest),
        DataVaultReadShapeKind.LatestSatellite);
    AssertDb2ReadStrategyDiagnostics(
        readDiagnostics.Analyze(context, asOfRequest),
        DataVaultReadShapeKind.LatestSatellite);
    AssertDb2ReadStrategyDiagnostics(
        readDiagnostics.Analyze(context, pitRequest),
        DataVaultReadShapeKind.PitAsOf);
    AssertDb2ReadStrategyDiagnostics(
        readDiagnostics.Analyze(context, bridgeRequest),
        DataVaultReadShapeKind.Bridge);

    var latestRows = await readService.ReadLatestSatelliteAsync(context, latestRequest, ProjectContact);
    var currentRows = await readService.ReadCurrentSatelliteAsync(
        context,
        metadata.Contact,
        [customerHashKey],
        ProjectContact);
    var asOfRows = await readService.ReadLatestSatelliteAsync(context, asOfRequest, ProjectContact);
    var currentAsOfRows = await readService.ReadAsOfSatelliteAsync(
        context,
        metadata.Contact,
        [customerHashKey],
        FirstReadTimestamp,
        ProjectContact);
    var pitRows = await readService.ReadPitAsync(
        context,
        pitRequest,
        row => {
          var contact = row.RequiredSatellite("Contact");

          return new ContactPitRead(
              row.RequiredString("ParentHashKey"),
              row.RequiredDateTimeOffset("LoadTimestamp"),
              contact.RequiredString("Email Address"),
              contact.RequiredString("HashDiff"));
        });
    var bridgeRows = await readService.ReadBridgeAsync(
        context,
        bridgeRequest,
        row => row.RequiredString("OrderHashKey"));

    var latestRow = Assert.Single(latestRows);
    var currentRow = Assert.Single(currentRows);
    var asOfRow = Assert.Single(asOfRows);
    var currentAsOfRow = Assert.Single(currentAsOfRows);
    var pitRow = Assert.Single(pitRows);

    Assert.Equal(customerHashKey, latestRow.ParentHashKey);
    Assert.Equal("db2-contact-read-hash-2", latestRow.HashDiff);
    Assert.Equal(SecondReadTimestamp, latestRow.LoadTimestamp);
    Assert.Equal(ReadRecordSource, latestRow.RecordSource);
    Assert.Equal("second-db2@example.test", latestRow.EmailAddress);
    Assert.Equal(latestRow, currentRow);
    Assert.Equal(customerHashKey, asOfRow.ParentHashKey);
    Assert.Equal("db2-contact-read-hash-1", asOfRow.HashDiff);
    Assert.Equal(FirstReadTimestamp, asOfRow.LoadTimestamp);
    Assert.Equal("first-db2@example.test", asOfRow.EmailAddress);
    Assert.Equal(asOfRow, currentAsOfRow);
    Assert.Equal(customerHashKey, pitRow.ParentHashKey);
    Assert.Equal(SelectedPitTimestamp, pitRow.LoadTimestamp);
    Assert.Equal("second-db2@example.test", pitRow.EmailAddress);
    Assert.Equal("db2-contact-read-hash-2", pitRow.HashDiff);
    Assert.Equal([orderHashKey], bridgeRows);
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    Db2ProviderReflection.AddDVaultDb2(services);

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static Dictionary<string, object> CreatePitRow(
      string customerHashKey,
      DateTimeOffset pitLoadTimestamp,
      DateTimeOffset contactLoadTimestamp) {
    return new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = customerHashKey,
      ["LoadTimestamp"] = DataVaultLoadTimestampValueConverter.FormatIso8601UtcText(pitLoadTimestamp),
      ["ContactLoadTimestamp"] = DataVaultLoadTimestampValueConverter.FormatIso8601UtcText(contactLoadTimestamp),
    };
  }

  private static void AssertDb2SaveStrategyDiagnostics(DataVaultDiagnosticsResult diagnostics) {
    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected, diagnostics.SaveStrategy.Status);
    Assert.Equal(Db2ProviderName, diagnostics.SaveStrategy.ProviderName);
    Assert.Equal("Db2DataVaultSaveStrategy", diagnostics.SaveStrategy.SelectedStrategyName);
    Assert.Contains(
        diagnostics.SaveStrategy.Candidates,
        candidate => string.Equals(candidate.StrategyName, "Db2DataVaultSaveStrategy", StringComparison.Ordinal) &&
            candidate.CanSave);
    Assert.Empty(diagnostics.SaveStrategy.FallbackCauses);
  }

  private static void AssertDb2ReadStrategyDiagnostics(
      DataVaultDiagnosticsResult diagnostics,
      DataVaultReadShapeKind expectedShapeKind) {
    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, diagnostics.ReadStrategy.Status);
    Assert.Equal(Db2ProviderName, diagnostics.ReadStrategy.ProviderName);
    Assert.Equal("Db2DataVaultReadStrategy", diagnostics.ReadStrategy.SelectedStrategyName);
    Assert.Contains(
        diagnostics.ReadStrategy.Candidates,
        candidate => string.Equals(candidate.StrategyName, "Db2DataVaultReadStrategy", StringComparison.Ordinal) &&
            candidate.CanRead);
    Assert.Empty(diagnostics.ReadStrategy.FallbackCauses);
    Assert.NotNull(diagnostics.ReadShape);
    Assert.Equal(expectedShapeKind, diagnostics.ReadShape!.Kind);
    Assert.Equal(
        DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
        diagnostics.ReadShape.Provider.ReadStrategyStatus);
    Assert.Equal("Db2DataVaultReadStrategy", diagnostics.ReadShape.Provider.SelectedStrategyName);
  }

  private static ContactRead ProjectContact(DataVaultSatelliteProjectionRow row) {
    return new ContactRead(
        row.RequiredString("ParentHashKey"),
        row.RequiredString("HashDiff"),
        row.RequiredDateTimeOffset("LoadTimestamp"),
        row.RequiredString("RecordSource"),
        row.RequiredString("Email Address"));
  }

  private static string GetHashKey(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
  }

  private static string ReadString(Dictionary<string, object> row, string columnName) {
    return Convert.ToString(row[columnName], CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("Expected column '" + columnName + "' to contain a non-null value.");
  }

  private static DateTimeOffset ReadLoadTimestamp(Dictionary<string, object> row) {
    return DataVaultLoadTimestampValueConverter.ReadProviderValue(row["LoadTimestamp"]);
  }

  private sealed class Db2SmokeDatabase : IAsyncDisposable {
    private static readonly string[] ProducedTableNames =
    [
        "BridgeCustomerOrder",
        "PitCustomerContact",
        "SatCustomerOrderState",
        "SatCustomerContact",
        "LinkCustomerOrder",
        "HubOrder",
        "HubCustomer",
    ];

    private readonly DbContextOptions<Db2SmokeContext> _options;
    private readonly string _tableNamePrefix;

    private Db2SmokeDatabase(DbContextOptions<Db2SmokeContext> options, string tableNamePrefix) {
      _options = options;
      _tableNamePrefix = tableNamePrefix;
    }

    public Db2SmokeContext CreateContext() {
      return new Db2SmokeContext(_options, _tableNamePrefix);
    }

    public static async Task<Db2SmokeDatabase> CreateAsync() {
      var configuration = Db2IntegrationTestConfiguration.FromEnvironment();
      if (!configuration.IsConfigured) {
        Assert.Skip(Db2IntegrationTestConfiguration.MissingConfigurationSkipMessage);
      }

      var tableNamePrefix = "DVB" + Guid.NewGuid().ToString("N")[..10].ToUpperInvariant() + "_";
      var optionsBuilder = new DbContextOptionsBuilder<Db2SmokeContext>();
      optionsBuilder.ReplaceService<IModelCacheKeyFactory, Db2SmokeModelCacheKeyFactory>();
      Db2ProviderReflection.UseDb2(optionsBuilder, configuration.ConnectionString!);

      var database = new Db2SmokeDatabase(optionsBuilder.Options, tableNamePrefix);

      try {
        await using var context = database.CreateContext();
        await DropTablesAsync(context, tableNamePrefix).ConfigureAwait(false);
        await context.GetService<IRelationalDatabaseCreator>().CreateTablesAsync().ConfigureAwait(false);

        return database;
      }
      catch {
        await database.DisposeAsync().ConfigureAwait(false);
        throw;
      }
    }

    public async ValueTask DisposeAsync() {
      await using var context = CreateContext();
      await DropTablesAsync(context, _tableNamePrefix).ConfigureAwait(false);
    }

    private static async Task DropTablesAsync(DbContext context, string tableNamePrefix) {
      foreach (var producedTableName in ProducedTableNames) {
        await DropTableIfExistsAsync(context, tableNamePrefix + producedTableName).ConfigureAwait(false);
      }
    }

    private static async Task DropTableIfExistsAsync(DbContext context, string tableName) {
      try {
        await context.Database
            .ExecuteSqlRawAsync("DROP TABLE " + QuoteDb2Identifier(tableName))
            .ConfigureAwait(false);
      }
      catch (Exception exception) when (IsUndefinedDb2Object(exception)) {
      }
    }

    private static bool IsUndefinedDb2Object(Exception exception) {
      var message = exception.ToString();

      return message.Contains("SQLSTATE=42704", StringComparison.OrdinalIgnoreCase) ||
          message.Contains("SQL0204N", StringComparison.OrdinalIgnoreCase) ||
          message.Contains("undefined name", StringComparison.OrdinalIgnoreCase);
    }
  }

  private sealed class Db2SmokeContext(
      DbContextOptions<Db2SmokeContext> options,
      string tableNamePrefix) : DbContext(options) {
    public string TableNamePrefix { get; } = tableNamePrefix;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          Db2SmokeMetadata.Value.Model,
          DataVaultProviderCapabilityProfiles.Db2);
      ConfigureDataVaultTables(modelBuilder);
    }

    private void ConfigureDataVaultTables(ModelBuilder modelBuilder) {
      ConfigureProducedTable(
          modelBuilder,
          "HubCustomer",
          ["CustomerHashKey"],
          "PkHubCustomerCustomerHashKey",
          [new(["CustomerId"], "IxHubCustomerBusinessKeyCustomerId", IsUnique: true)]);
      ConfigureProducedTable(
          modelBuilder,
          "HubOrder",
          ["OrderHashKey"],
          "PkHubOrderOrderHashKey",
          [new(["OrderId"], "IxHubOrderBusinessKeyOrderId", IsUnique: true)]);
      ConfigureProducedTable(
          modelBuilder,
          "LinkCustomerOrder",
          ["CustomerOrderHashKey"],
          "PkLinkCustomerOrderCustomerOrderHashKey",
          [new(["CustomerHashKey", "OrderHashKey"], "IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey")]);
      ConfigureProducedTable(
          modelBuilder,
          "SatCustomerContact",
          ["CustomerHashKey", "LoadTimestamp"],
          "PkSatCustomerContactCustomerHashKeyLoadTimestamp",
          [new(
              ["CustomerHashKey", "LoadTimestamp", "HashDiff"],
              "IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp")]);
      ConfigureProducedTable(
          modelBuilder,
          "SatCustomerOrderState",
          ["CustomerOrderHashKey", "LoadTimestamp"],
          "PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp",
          [new(
              ["CustomerOrderHashKey", "LoadTimestamp", "HashDiff"],
              "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp")]);
      ConfigureProducedTable(
          modelBuilder,
          "PitCustomerContact",
          ["CustomerHashKey", "LoadTimestamp"],
          "PkPitCustomerContactCustomerHashKeyLoadTimestamp",
          []);
      ConfigureProducedTable(
          modelBuilder,
          "BridgeCustomerOrder",
          ["CustomerHashKey", "OrderHashKey"],
          "PkBridgeCustomerOrderCustomerHashKeyOrderHashKey",
          [new(["OrderHashKey", "CustomerHashKey"], "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey")]);
    }

    private void ConfigureProducedTable(
        ModelBuilder modelBuilder,
        string producedTableName,
        IReadOnlyList<string> primaryKeyColumnNames,
        string producedPrimaryKeyName,
        IReadOnlyList<IndexOverride> indexes) {
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>(producedTableName, entity => {
        entity.ToTable(GetPhysicalName(producedTableName));
        entity
            .HasKey(primaryKeyColumnNames.ToArray())
            .HasName(GetPhysicalName(producedPrimaryKeyName));

        foreach (var index in indexes) {
          entity
              .HasIndex(index.ColumnNames.ToArray())
              .IsUnique(index.IsUnique)
              .HasDatabaseName(GetPhysicalName(index.ProducedIndexName));
        }
      });
    }

    private string GetPhysicalName(string producedName) {
      return TableNamePrefix + producedName;
    }
  }

  private sealed class Db2SmokeModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is Db2SmokeContext db2Context
          ? (context.GetType(), db2Context.TableNamePrefix, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private sealed record IndexOverride(
      IReadOnlyList<string> ColumnNames,
      string ProducedIndexName,
      bool IsUnique = false);

  private sealed record Db2SmokeMetadata(
      DataVaultMetadataModel Model,
      DataVaultHubMetadata Customer,
      DataVaultHubMetadata Order,
      DataVaultLinkMetadata CustomerOrder,
      DataVaultSatelliteMetadata Contact,
      DataVaultSatelliteMetadata State,
      DataVaultPitMetadata Pit,
      DataVaultBridgeMetadata Bridge) {
    public static Db2SmokeMetadata Value { get; } = Create();

    private static Db2SmokeMetadata Create() {
      var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
      var order = new DataVaultHubMetadata("Order", ["Order Id"]);
      var customerOrder = new DataVaultLinkMetadata(
          "CustomerOrder",
          [customer.ToReference(), order.ToReference()]);
      var contact = new DataVaultSatelliteMetadata(
          "Contact",
          customer.ToReference(),
          ["Email Address"]);
      var state = new DataVaultSatelliteMetadata(
          "State",
          customerOrder.ToReference(),
          ["State Code"]);
      var pit = new DataVaultPitMetadata(customer.ToReference(), ["Contact"]);
      var bridge = DataVaultBridgeMetadata.ManyToMany(
          "CustomerOrder",
          customer.ToReference(),
          customerOrder.ToReference(),
          order.ToReference());
      var model = new DataVaultMetadataModel(
          [customer, order],
          [customerOrder],
          [contact, state],
          Array.Empty<DataVaultPointInTimeMetadata>(),
          [bridge],
          [pit]);

      return new Db2SmokeMetadata(model, customer, order, customerOrder, contact, state, pit, bridge);
    }
  }

  private sealed record ContactRead(
      string ParentHashKey,
      string HashDiff,
      DateTimeOffset LoadTimestamp,
      string RecordSource,
      string EmailAddress);

  private sealed record ContactPitRead(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      string EmailAddress,
      string HashDiff);

  private static string QuoteDb2Identifier(string value) {
    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }
}
