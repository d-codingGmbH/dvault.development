using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class PostgresProviderCapabilityTests {
  [Fact]
  public void AddDVaultPostgresRegistersOptimizedStrategyStagedDiagnosticsAndProviderProfileSelection() {
    try {
      var services = new ServiceCollection();

      services.AddDVaultPostgres();

      using var provider = services.BuildServiceProvider(validateScopes: true);
      var strategies = provider.GetServices<IDataVaultProviderSaveStrategy>().ToArray();
      var readStrategies = provider.GetServices<IDataVaultProviderReadStrategy>().ToArray();
      var pitMaintenanceStrategies = provider.GetServices<IDataVaultProviderPitMaintenanceStrategy>().ToArray();

      Assert.Contains(strategies, strategy => strategy is PostgresDataVaultSaveStrategy);
      Assert.Contains(strategies, strategy => strategy is IDataVaultProviderStagedBulkSaveDiagnostics);
      Assert.Contains(readStrategies, strategy => strategy is PostgresDataVaultReadStrategy);
      Assert.Contains(pitMaintenanceStrategies, strategy => strategy is PostgresDataVaultPitMaintenanceStrategy);
      Assert.Same(
          DataVaultProviderCapabilityProfiles.Postgres,
          DataVaultProviderCapabilityProfileSelection.Select(PostgresDataVaultSaveStrategy.NpgsqlProviderName));
      Assert.Same(
          DataVaultProviderCapabilityProfiles.Sqlite,
          DataVaultProviderCapabilityProfileSelection.Select((string?)null));
    }
    finally {
      DataVaultProviderCapabilityProfileSelection.Reset();
    }
  }

  [Fact]
  public void PostgresPitMaintenanceGateAcceptsApprovedFullRebuildShapesAndDeclinesFallbackShapes() {
    try {
      DataVaultProviderCapabilityProfileSelection.Register(
          KnownProviderNames.Postgres,
          DataVaultProviderCapabilityProfiles.Postgres);
      var ordinaryHubPit = new DataVaultPitRebuildRequest(new DataVaultPitMetadata(
          DataVaultMetadataReference.Hub("Customer"),
          ["Profile", "Status"]));
      var multiActiveHubPit = new DataVaultPitRebuildRequest(new DataVaultPitMetadata(
          DataVaultMetadataReference.Hub("Customer"),
          [
              new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
              new DataVaultPitSatelliteReferenceMetadata("Profile"),
          ]));
      var linkPit = new DataVaultPitRebuildRequest(new DataVaultPitMetadata(
          DataVaultMetadataReference.Link("CustomerOrder"),
          ["State", "Fulfillment"]));
      var linkMultiActivePit = new DataVaultPitRebuildRequest(new DataVaultPitMetadata(
          DataVaultMetadataReference.Link("CustomerOrder"),
          [new DataVaultPitSatelliteReferenceMetadata("State", isMultiActive: true)]));

      Assert.True(DataVaultProviderPitMaintenanceStrategyGateEvaluator
          .EvaluatePostgres(KnownProviderNames.Postgres, ordinaryHubPit)
          .CanRebuild);
      Assert.True(DataVaultProviderPitMaintenanceStrategyGateEvaluator
          .EvaluatePostgres(KnownProviderNames.Postgres, multiActiveHubPit)
          .CanRebuild);
      Assert.True(DataVaultProviderPitMaintenanceStrategyGateEvaluator
          .EvaluatePostgres(KnownProviderNames.Postgres, linkPit)
          .CanRebuild);

      Assert.Contains(
          DataVaultProviderPitMaintenanceStrategyGateEvaluator
              .EvaluatePostgres(KnownProviderNames.Sqlite, ordinaryHubPit)
              .FallbackCauses,
          cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.ProviderNameMismatch);
      Assert.Contains(
          DataVaultProviderPitMaintenanceStrategyGateEvaluator
              .EvaluatePostgres(KnownProviderNames.Postgres, ordinaryHubPit, hasPendingTrackedChanges: true)
              .FallbackCauses,
          cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.DirtyDbContext);
      Assert.Contains(
          DataVaultProviderPitMaintenanceStrategyGateEvaluator
              .EvaluatePostgres(
                  KnownProviderNames.Postgres,
                  ordinaryHubPit,
                  hasCurrentTransaction: true)
              .FallbackCauses,
          cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.CurrentTransactionSavepointUnavailable);
      Assert.Contains(
          DataVaultProviderPitMaintenanceStrategyGateEvaluator
              .EvaluatePostgres(KnownProviderNames.Postgres, linkMultiActivePit)
              .FallbackCauses,
          cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.UnsupportedPitShape);
      Assert.Contains(
          DataVaultProviderPitMaintenanceStrategyGateEvaluator
              .EvaluatePostgres(
                  KnownProviderNames.Postgres,
                  ordinaryHubPit,
                  hasCompleteMaintenanceShapeEvidence: false)
              .FallbackCauses,
          cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.IncompleteMaintenanceShapeEvidence);
    }
    finally {
      DataVaultProviderCapabilityProfileSelection.Reset();
    }
  }

  [Fact]
  public void PostgresPitMaintenanceStrategyBuildsInsertSelectSqlInsideProviderPackage() {
    var metadata = CreatePitMaintenanceMetadata();
    var options = new DbContextOptionsBuilder<PostgresPitMaintenanceSqlContext>()
        .UseSqlite("Data Source=:memory:")
        .Options;
    using var context = new PostgresPitMaintenanceSqlContext(options);
    var projection = DefaultDataVaultPitMaintenanceService.CreatePitProjection(context, metadata.Pit);

    var commandText = PostgresDataVaultPitMaintenanceStrategy.CreatePostgresPitRebuildInsertCommandText(
        context,
        projection);

    Assert.Contains("WITH \"pit_source\" AS", commandText, StringComparison.Ordinal);
    Assert.Contains("INSERT INTO \"PitCustomerProfileStatus\"", commandText, StringComparison.Ordinal);
    Assert.Contains("SELECT \"source\".\"parent_hash_key\", \"source\".\"load_timestamp\"", commandText, StringComparison.Ordinal);
    Assert.Contains("LEFT JOIN LATERAL", commandText, StringComparison.Ordinal);
    Assert.Contains("ORDER BY \"source\".\"parent_hash_key\", \"source\".\"load_timestamp\"", commandText, StringComparison.Ordinal);
  }

  [Fact]
  public void PostgresStrategyKeepsSetBasedBoundaryBelowStagedBulkBoundary() {
    var setBasedBatch = CreateHubRequest(totalOperationCount: PostgresDataVaultSaveStrategy.MinimumStagedBulkOperationCount - 1);
    var stagedBatch = CreateHubRequest(totalOperationCount: PostgresDataVaultSaveStrategy.MinimumStagedBulkOperationCount);

    var setBasedGate = DataVaultProviderSaveStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.Postgres,
        hasPendingTrackedChanges: false,
        setBasedBatch);
    var stagedGate = DataVaultProviderSaveStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.Postgres,
        hasPendingTrackedChanges: false,
        stagedBatch);

    Assert.True(setBasedGate.CanSave);
    Assert.True(stagedGate.CanSave);
    Assert.False(PostgresDataVaultSaveStrategy.IsStagedBatchShape(setBasedBatch));
    Assert.True(PostgresDataVaultSaveStrategy.IsStagedBatchShape(stagedBatch));
  }

  [Fact]
  public void PostgresStrategyBuildsStagedCopySqlInsideProviderPackage() {
    var createCommandText = PostgresDataVaultSaveStrategy.CreatePostgresCreateStagingTableCommandText(
        "__dvault_stage_1",
        "\"analytics\".\"HubCustomer\"");
    var copyCommandText = PostgresDataVaultSaveStrategy.CreatePostgresCopyCommandText(
        "__dvault_stage_1",
        ["__dvault_ordinal", "CustomerHashKey", "LoadTimestamp"]);
    var uniqueInsertCommandText = PostgresDataVaultSaveStrategy.CreatePostgresStagedUniqueInsertCommandText(
        "\"analytics\".\"HubCustomer\"",
        "__dvault_stage_1",
        ["CustomerHashKey", "LoadTimestamp"],
        "CustomerHashKey");
    var insertCommandText = PostgresDataVaultSaveStrategy.CreatePostgresStagedInsertCommandText(
        "\"analytics\".\"SatCustomerContact\"",
        "__dvault_stage_1",
        ["CustomerHashKey", "HashDiff"]);
    var dropCommandText = PostgresDataVaultSaveStrategy.CreatePostgresDropStagingTableCommandText("__dvault_stage_1");

    Assert.Equal(
        "CREATE TEMPORARY TABLE \"__dvault_stage_1\" " +
        "(\"__dvault_ordinal\" integer NOT NULL, LIKE \"analytics\".\"HubCustomer\" INCLUDING DEFAULTS) ON COMMIT DROP",
        createCommandText);
    Assert.Equal(
        "COPY \"__dvault_stage_1\" (\"__dvault_ordinal\", \"CustomerHashKey\", \"LoadTimestamp\") " +
        "FROM STDIN (FORMAT CSV, NULL '\\N')",
        copyCommandText);
    Assert.Equal(
        "WITH \"deduplicated\" AS (SELECT \"stage\".\"CustomerHashKey\", \"stage\".\"LoadTimestamp\", " +
        "ROW_NUMBER() OVER (PARTITION BY \"stage\".\"CustomerHashKey\" ORDER BY \"stage\".\"__dvault_ordinal\") " +
        "AS \"__dvault_row_number\" FROM \"__dvault_stage_1\" AS \"stage\") INSERT INTO " +
        "\"analytics\".\"HubCustomer\" (\"CustomerHashKey\", \"LoadTimestamp\") SELECT " +
        "\"deduplicated\".\"CustomerHashKey\", \"deduplicated\".\"LoadTimestamp\" FROM \"deduplicated\" " +
        "WHERE \"deduplicated\".\"__dvault_row_number\" = 1 ON CONFLICT (\"CustomerHashKey\") DO NOTHING",
        uniqueInsertCommandText);
    Assert.Equal(
        "INSERT INTO \"analytics\".\"SatCustomerContact\" (\"CustomerHashKey\", \"HashDiff\") SELECT " +
        "\"stage\".\"CustomerHashKey\", \"stage\".\"HashDiff\" FROM \"__dvault_stage_1\" AS \"stage\" " +
        "ORDER BY \"stage\".\"__dvault_ordinal\"",
        insertCommandText);
    Assert.Equal(
        "DROP TABLE IF EXISTS \"__dvault_stage_1\"",
        dropCommandText);
  }

  [Fact]
  public void PostgresStrategyRetainsWindowedLatestSatelliteReadSqlInsideProviderPackage() {
    using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Name"]);
    var projection = DataVaultSatelliteReadPipeline.CreateSatelliteProjection(profile);
    var commandText = new PostgresDataVaultReadStrategy().CreateLatestRowsCommandText(
        context,
        projection,
        [
            "CustomerHashKey",
            "HashDiff",
            "LoadTimestamp",
            "RecordSource",
            "Name",
        ],
        parentHashKeyCount: 2,
        hasAsOf: true);

    Assert.Equal(
        "SELECT \"CustomerHashKey\", \"HashDiff\", \"LoadTimestamp\", \"RecordSource\", \"Name\" " +
        "FROM (SELECT \"CustomerHashKey\", \"HashDiff\", \"LoadTimestamp\", \"RecordSource\", \"Name\", " +
        "ROW_NUMBER() OVER (PARTITION BY \"CustomerHashKey\" ORDER BY \"LoadTimestamp\" DESC) " +
        "AS \"__dvault_row_number\" FROM \"SatCustomerProfile\" " +
        "WHERE \"CustomerHashKey\" IN (@p0, @p1) AND \"LoadTimestamp\" <= @p2) AS \"__dvault_latest\" " +
        "WHERE \"__dvault_row_number\" = 1 ORDER BY \"CustomerHashKey\"",
        commandText);
  }

  [Fact]
  public void PostgresStrategyRecognizesOnlyNpgsqlProviderName() {
    Assert.True(PostgresDataVaultSaveStrategy.IsSupportedProviderName(PostgresDataVaultSaveStrategy.NpgsqlProviderName));
    Assert.False(PostgresDataVaultSaveStrategy.IsSupportedProviderName("Microsoft.EntityFrameworkCore.Sqlite"));
    Assert.False(PostgresDataVaultSaveStrategy.IsSupportedProviderName(null));
  }

  private static IReadOnlyList<DataVaultSaveRequest> CreateHubRequest(int totalOperationCount) {
    var hub = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var request = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 26, 12, 0, 0, TimeSpan.Zero),
        "postgres-provider-tests",
        Enumerable.Range(0, totalOperationCount)
            .Select(index => new DataVaultHubSaveOperation(
                hub,
                [new("Customer Id", "C-" + index.ToString("000", CultureInfo.InvariantCulture))]))
            .ToArray(),
        []);

    return [request];
  }

  private static PitMaintenanceMetadata CreatePitMaintenanceMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var model = new DataVaultMetadataModel([customer], [], [profile, status], [pit]);

    return new PitMaintenanceMetadata(pit, model);
  }

  private sealed class PostgresPitMaintenanceSqlContext(
      DbContextOptions<PostgresPitMaintenanceSqlContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreatePitMaintenanceMetadata().Model);
    }
  }

  private sealed record PitMaintenanceMetadata(
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);
}
