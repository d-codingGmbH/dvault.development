using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultProviderReadStrategyTests {
  [Fact]
  public async Task ReadDispatchEvaluatesStrategiesByDescendingPriorityUntilFirstCompatibleStrategy() {
    var evaluationOrder = new List<string>();
    var lowPriorityCompatible = new DispatchProbeReadStrategy(
        "low-priority-compatible",
        priority: 10,
        canRead: true,
        evaluationOrder);
    var selectedCompatible = new DispatchProbeReadStrategy(
        "selected-compatible",
        priority: 100,
        canRead: true,
        evaluationOrder);
    var highPriorityIncompatible = new DispatchProbeReadStrategy(
        "high-priority-incompatible",
        priority: 200,
        canRead: false,
        evaluationOrder);
    var readService = new DefaultDataVaultReadService([
        lowPriorityCompatible,
        selectedCompatible,
        highPriorityIncompatible,
    ]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var rows = await readService.ReadLatestSatelliteRowsAsync(context, CreateReadRequest(["customer-hk"]));

    Assert.Equal(
        ["high-priority-incompatible", "selected-compatible"],
        evaluationOrder);
    Assert.Equal(1, highPriorityIncompatible.CanReadCallCount);
    Assert.Equal(0, highPriorityIncompatible.ReadCallCount);
    Assert.Equal(1, selectedCompatible.CanReadCallCount);
    Assert.Equal(1, selectedCompatible.ReadCallCount);
    Assert.Equal(0, lowPriorityCompatible.CanReadCallCount);
    Assert.Equal(0, lowPriorityCompatible.ReadCallCount);

    var row = Assert.Single(rows);
    Assert.Equal("selected-compatible", row.MetadataName);
    Assert.Equal("StrategyProbe", row.TableName);
  }

  [Fact]
  public async Task ReadDispatchKeepsRegistrationOrderWhenCompatibleStrategiesSharePriority() {
    var evaluationOrder = new List<string>();
    var firstRegistered = new DispatchProbeReadStrategy(
        "first-registered",
        priority: 100,
        canRead: true,
        evaluationOrder);
    var secondRegistered = new DispatchProbeReadStrategy(
        "second-registered",
        priority: 100,
        canRead: true,
        evaluationOrder);
    var readService = new DefaultDataVaultReadService([firstRegistered, secondRegistered]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var rows = await readService.ReadLatestSatelliteRowsAsync(context, CreateReadRequest(["customer-hk"]));

    Assert.Equal(["first-registered"], evaluationOrder);
    Assert.Equal(1, firstRegistered.CanReadCallCount);
    Assert.Equal(1, firstRegistered.ReadCallCount);
    Assert.Equal(0, secondRegistered.CanReadCallCount);
    Assert.Equal(0, secondRegistered.ReadCallCount);
    Assert.Equal("first-registered", Assert.Single(rows).MetadataName);
  }

  [Fact]
  public async Task TypedProjectionReadUsesSelectedProviderStrategy() {
    var strategy = new DispatchProbeReadStrategy(
        "projection-selected",
        priority: 100,
        canRead: true,
        []);
    var readService = new DefaultDataVaultReadService([strategy]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var projections = await readService.ReadLatestSatelliteAsync(
        context,
        CreateReadRequest(["customer-hk"]),
        row => new {
          ParentHashKey = row.RequiredString("ParentHashKey"),
          HashDiff = row.RequiredString("HashDiff"),
          LoadTimestamp = row.RequiredDateTimeOffset("LoadTimestamp"),
          RecordSource = row.RequiredString("RecordSource"),
          Name = row.RequiredString("Name"),
        });

    var projection = Assert.Single(projections);
    Assert.Equal("customer-hk", projection.ParentHashKey);
    Assert.Equal("projection-selected-hash", projection.HashDiff);
    Assert.Equal("projection-selected", projection.RecordSource);
    Assert.Equal("projection-selected name", projection.Name);
    Assert.Equal(1, strategy.ProjectionReadCallCount);
    Assert.Equal(0, strategy.ReadCallCount);
  }

  [Fact]
  public async Task PitReadDispatchUsesSelectedProviderStrategy() {
    var strategy = new DispatchProbePitReadStrategy(priority: 100, canRead: true);
    var readService = new DefaultDataVaultReadService(
        Array.Empty<IDataVaultProviderReadStrategy>(),
        [strategy],
        Array.Empty<IDataVaultProviderBridgeReadStrategy>());

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var rows = await readService.ReadPitRowsAsync(
        context,
        CreatePitReadRequest(["customer-hk"]));

    var row = Assert.Single(rows);
    Assert.Equal("customer-hk", row.ParentHashKey);
    Assert.Equal(1, strategy.CanReadCallCount);
    Assert.Equal(1, strategy.ReadCallCount);
  }

  [Fact]
  public async Task BridgeReadExtensionUsesSelectedProviderStrategy() {
    var strategy = new DispatchProbeBridgeReadStrategy(priority: 100, canRead: true);
    IDataVaultReadService readService = new DefaultDataVaultReadService(
        Array.Empty<IDataVaultProviderReadStrategy>(),
        Array.Empty<IDataVaultProviderPitReadStrategy>(),
        [strategy]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var rows = await readService.ReadBridgeRowsAsync(
        context,
        CreateBridgeReadRequest(["customer-hk"]));

    var row = Assert.Single(rows);
    Assert.Equal("StrategyProbeBridge", row.MetadataName);
    Assert.Equal("customer-hk", Assert.Single(row.EndpointHashKeys).HashKey);
    Assert.Equal(1, strategy.CanReadCallCount);
    Assert.Equal(1, strategy.ReadCallCount);
  }

  [Fact]
  public async Task ReadDispatchFallsBackWhenNoProviderStrategyIsRegistered() {
    var readService = new DefaultDataVaultReadService();

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var rows = await readService.ReadLatestSatelliteRowsAsync(context, CreateReadRequest([]));

    Assert.Empty(rows);
  }

  [Fact]
  public void PostgresLatestSatelliteReadGateAcceptsPublishedHubParentShape() {
    var evaluation = DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.Postgres,
        CreateReadRequest(["customer-hk"]));

    Assert.True(evaluation.CanRead);
    Assert.Empty(evaluation.FallbackCauses);
  }

  [Fact]
  public void PostgresLatestSatelliteReadGateFailsClosedForProviderMismatchUnsupportedParentAndMultiActiveShape() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var linkParentSatellite = new DataVaultSatelliteMetadata(
        "Fulfillment",
        DataVaultMetadataReference.Link("OrderProduct"),
        ["State"]);
    var multiActiveSatellite = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["EmailAddress"],
        ["ContactType"]);

    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluatePostgres(KnownProviderNames.Sqlite, CreateReadRequest(["customer-hk"]))
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluatePostgres(
                KnownProviderNames.Postgres,
                new DataVaultLatestSatelliteReadRequest(linkParentSatellite, ["link-hk"]))
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluatePostgres(
                KnownProviderNames.Postgres,
                new DataVaultLatestSatelliteReadRequest(multiActiveSatellite, ["customer-hk"]))
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
  }

  [Fact]
  public void SqlServerAndOracleLatestSatelliteReadGatesAcceptHubParentOrdinarySatellites() {
    var sqlServerSupported = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        CreateReadRequest(["customer-hk"]));
    var oracleSupported = DataVaultProviderReadStrategyGateEvaluator.EvaluateOracle(
        KnownProviderNames.Oracle,
        CreateReadRequest(["customer-hk"]));

    Assert.True(sqlServerSupported.CanRead);
    Assert.Empty(sqlServerSupported.FallbackCauses);
    Assert.True(oracleSupported.CanRead);
    Assert.Empty(oracleSupported.FallbackCauses);
  }

  [Fact]
  public void SqlServerAndOracleLatestSatelliteReadGatesFailClosedForMismatchedProviderOrUnsupportedShapes() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var linkParentSatellite = new DataVaultSatelliteMetadata(
        "Fulfillment",
        DataVaultMetadataReference.Link("OrderProduct"),
        ["State"]);
    var multiActiveSatellite = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);

    var providerMismatch = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.Sqlite,
        CreateReadRequest(["customer-hk"]));
    var unsupportedParent = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        new DataVaultLatestSatelliteReadRequest(linkParentSatellite, ["link-hk"]));
    var multiActive = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        new DataVaultLatestSatelliteReadRequest(multiActiveSatellite, ["customer-hk"]));
    var oracleProviderMismatch = DataVaultProviderReadStrategyGateEvaluator.EvaluateOracle(
        KnownProviderNames.SqlServer,
        CreateReadRequest(["customer-hk"]));
    var oracleUnsupportedParent = DataVaultProviderReadStrategyGateEvaluator.EvaluateOracle(
        KnownProviderNames.Oracle,
        new DataVaultLatestSatelliteReadRequest(linkParentSatellite, ["link-hk"]));
    var oracleMultiActive = DataVaultProviderReadStrategyGateEvaluator.EvaluateOracle(
        KnownProviderNames.Oracle,
        new DataVaultLatestSatelliteReadRequest(multiActiveSatellite, ["customer-hk"]));

    Assert.False(providerMismatch.CanRead);
    Assert.Contains(
        providerMismatch.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.False(unsupportedParent.CanRead);
    Assert.Contains(
        unsupportedParent.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent);
    Assert.False(multiActive.CanRead);
    Assert.Contains(
        multiActive.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
    Assert.False(oracleProviderMismatch.CanRead);
    Assert.Contains(
        oracleProviderMismatch.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.False(oracleUnsupportedParent.CanRead);
    Assert.Contains(
        oracleUnsupportedParent.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent);
    Assert.False(oracleMultiActive.CanRead);
    Assert.Contains(
        oracleMultiActive.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
  }

  [Fact]
  public void MySqlLatestSatelliteReadGateAcceptsPublishedHubParentShapeForPomeloAndOracleProviders() {
    var request = CreateReadRequest(["customer-hk"]);

    var pomelo = DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlPomelo,
        request);
    var oracle = DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlOracle,
        request);

    Assert.True(pomelo.CanRead);
    Assert.Empty(pomelo.FallbackCauses);
    Assert.True(oracle.CanRead);
    Assert.Empty(oracle.FallbackCauses);
  }

  [Fact]
  public void MySqlLatestSatelliteReadGateFailsClosedForProviderMismatchUnsupportedParentAndMultiActiveShapes() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var linkParentSatellite = new DataVaultSatelliteMetadata(
        "Fulfillment",
        DataVaultMetadataReference.Link("OrderProduct"),
        ["State"]);
    var multiActiveSatellite = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email"],
        ["Contact Type"]);

    var supportedRequest = CreateReadRequest(["customer-hk"]);
    var unsupportedParentRequest = new DataVaultLatestSatelliteReadRequest(linkParentSatellite, ["link-hk"]);
    var multiActiveRequest = new DataVaultLatestSatelliteReadRequest(multiActiveSatellite, ["customer-hk"]);

    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.Sqlite, supportedRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlPomelo, unsupportedParentRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlOracle, multiActiveRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
  }

  [Fact]
  public void Db2LatestSatelliteReadGateAcceptsHubParentOrdinarySatellites() {
    var supported = DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(
        KnownProviderNames.Db2,
        CreateReadRequest(["customer-hk"]));

    Assert.True(supported.CanRead);
    Assert.Empty(supported.FallbackCauses);
  }

  [Fact]
  public void Db2LatestSatelliteReadGateFailsClosedForMismatchedProviderOrUnsupportedShapes() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var linkParentSatellite = new DataVaultSatelliteMetadata(
        "Fulfillment",
        DataVaultMetadataReference.Link("OrderProduct"),
        ["State"]);
    var multiActiveSatellite = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);

    var providerMismatch = DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(
        KnownProviderNames.SqlServer,
        CreateReadRequest(["customer-hk"]));
    var unsupportedParent = DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(
        KnownProviderNames.Db2,
        new DataVaultLatestSatelliteReadRequest(linkParentSatellite, ["link-hk"]));
    var multiActive = DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(
        KnownProviderNames.Db2,
        new DataVaultLatestSatelliteReadRequest(multiActiveSatellite, ["customer-hk"]));

    Assert.False(providerMismatch.CanRead);
    Assert.Contains(
        providerMismatch.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.False(unsupportedParent.CanRead);
    Assert.Contains(
        unsupportedParent.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent);
    Assert.False(multiActive.CanRead);
    Assert.Contains(
        multiActive.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
  }

  [Fact]
  public void PostgresAndSqlServerPitReadGatesAcceptPublishedMaintainedPitShapes() {
    var hubPit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
    var hubMultiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true)]);
    var linkPit = new DataVaultPitMetadata(DataVaultMetadataReference.Link("CustomerOrder"), ["State"]);

    var postgresHub = DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.Postgres,
        CreatePitReadRequest(hubPit, ["customer-hk"]));
    var postgresLink = DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.Postgres,
        CreatePitReadRequest(linkPit, ["customer-order-hk"]));
    var sqlServerMultiActive = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        CreatePitReadRequest(hubMultiActivePit, ["customer-hk"]));

    Assert.True(postgresHub.CanRead);
    Assert.Empty(postgresHub.FallbackCauses);
    Assert.True(postgresLink.CanRead);
    Assert.Empty(postgresLink.FallbackCauses);
    Assert.True(sqlServerMultiActive.CanRead);
    Assert.Empty(sqlServerMultiActive.FallbackCauses);
  }

  [Fact]
  public void MySqlAndOraclePitReadGatesAcceptPublishedMaintainedPitShapes() {
    var hubPit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
    var hubMultiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true)]);
    var linkPit = new DataVaultPitMetadata(DataVaultMetadataReference.Link("CustomerOrder"), ["State"]);

    var mySqlPomeloHub = DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlPomelo,
        CreatePitReadRequest(hubPit, ["customer-hk"]));
    var mySqlOracleLink = DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlOracle,
        CreatePitReadRequest(linkPit, ["customer-order-hk"]));
    var oracleMultiActive = DataVaultProviderReadStrategyGateEvaluator.EvaluateOracle(
        KnownProviderNames.Oracle,
        CreatePitReadRequest(hubMultiActivePit, ["customer-hk"]));

    Assert.True(mySqlPomeloHub.CanRead);
    Assert.Empty(mySqlPomeloHub.FallbackCauses);
    Assert.True(mySqlOracleLink.CanRead);
    Assert.Empty(mySqlOracleLink.FallbackCauses);
    Assert.True(oracleMultiActive.CanRead);
    Assert.Empty(oracleMultiActive.FallbackCauses);
  }

  [Fact]
  public void Db2PitReadGateAcceptsPublishedMaintainedPitShapes() {
    var hubPit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
    var hubMultiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true)]);
    var linkPit = new DataVaultPitMetadata(DataVaultMetadataReference.Link("CustomerOrder"), ["State"]);

    var db2Hub = DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(
        KnownProviderNames.Db2,
        CreatePitReadRequest(hubPit, ["customer-hk"]));
    var db2Link = DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(
        KnownProviderNames.Db2,
        CreatePitReadRequest(linkPit, ["customer-order-hk"]));
    var db2MultiActive = DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(
        KnownProviderNames.Db2,
        CreatePitReadRequest(hubMultiActivePit, ["customer-hk"]));

    Assert.True(db2Hub.CanRead);
    Assert.Empty(db2Hub.FallbackCauses);
    Assert.True(db2Link.CanRead);
    Assert.Empty(db2Link.FallbackCauses);
    Assert.True(db2MultiActive.CanRead);
    Assert.Empty(db2MultiActive.FallbackCauses);
  }

  [Fact]
  public void PostgresAndSqlServerPitReadGatesFailClosedForMismatchedProviderOrUnsupportedLinkMultiActiveShape() {
    var hubPit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
    var linkMultiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Link("CustomerOrder"),
        [new DataVaultPitSatelliteReferenceMetadata("State", isMultiActive: true)]);

    var providerMismatch = DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.SqlServer,
        CreatePitReadRequest(hubPit, ["customer-hk"]));
    var unsupportedShape = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        CreatePitReadRequest(linkMultiActivePit, ["customer-order-hk"]));

    Assert.False(providerMismatch.CanRead);
    Assert.Contains(
        providerMismatch.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.False(unsupportedShape.CanRead);
    Assert.Contains(
        unsupportedShape.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape);
  }

  [Fact]
  public void MySqlAndOraclePitReadGatesFailClosedForProviderShapeEvidenceAndMaintenanceFallbacks() {
    var hubPit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
    var linkMultiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Link("CustomerOrder"),
        [new DataVaultPitSatelliteReferenceMetadata("State", isMultiActive: true)]);
    var supportedRequest = CreatePitReadRequest(hubPit, ["customer-hk"]);
    var unsupportedShapeRequest = CreatePitReadRequest(linkMultiActivePit, ["customer-order-hk"]);

    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.SqlServer, supportedRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateOracle(KnownProviderNames.MySqlPomelo, supportedRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlPomelo, unsupportedShapeRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateOracle(KnownProviderNames.Oracle, unsupportedShapeRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlPomelo, supportedRequest, hasCompleteReadShapeEvidence: false)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateOracle(KnownProviderNames.Oracle, supportedRequest, hasCompleteReadShapeEvidence: false)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(
                KnownProviderNames.MySqlOracle,
                supportedRequest,
                hasCompleteReadShapeEvidence: true,
                hasStaleReadModelMaintenanceSignal: true)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateOracle(
                KnownProviderNames.Oracle,
                supportedRequest,
                hasCompleteReadShapeEvidence: true,
                hasStaleReadModelMaintenanceSignal: true)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance);
  }

  [Fact]
  public void Db2PitReadGateFailsClosedForProviderShapeEvidenceAndMaintenanceFallbacks() {
    var hubPit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
    var linkMultiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Link("CustomerOrder"),
        [new DataVaultPitSatelliteReferenceMetadata("State", isMultiActive: true)]);
    var supportedRequest = CreatePitReadRequest(hubPit, ["customer-hk"]);
    var unsupportedShapeRequest = CreatePitReadRequest(linkMultiActivePit, ["customer-order-hk"]);

    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateDb2(KnownProviderNames.Postgres, supportedRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateDb2(KnownProviderNames.Db2, unsupportedShapeRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateDb2(KnownProviderNames.Db2, supportedRequest, hasCompleteReadShapeEvidence: false)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateDb2(
                KnownProviderNames.Db2,
                supportedRequest,
                hasCompleteReadShapeEvidence: true,
                hasStaleReadModelMaintenanceSignal: true)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance);
  }

  [Fact]
  public void PostgresAndSqlServerBridgeReadGatesAcceptManyToManyAndHierarchyShapes() {
    var manyToManyRequest = CreateBridgeReadRequest(["customer-hk"]);
    var hierarchyRequest = CreateHierarchyBridgeReadRequest(["ancestor-hk"]);

    var postgresManyToMany = DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.Postgres,
        manyToManyRequest);
    var sqlServerHierarchy = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        hierarchyRequest);

    Assert.True(postgresManyToMany.CanRead);
    Assert.Empty(postgresManyToMany.FallbackCauses);
    Assert.True(sqlServerHierarchy.CanRead);
    Assert.Empty(sqlServerHierarchy.FallbackCauses);
  }

  [Fact]
  public void MySqlAndOracleBridgeReadGatesAcceptManyToManyAndHierarchyShapes() {
    var manyToManyRequest = CreateBridgeReadRequest(["customer-hk"]);
    var hierarchyRequest = CreateHierarchyBridgeReadRequest(["ancestor-hk"]);

    var mySqlPomeloManyToMany = DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlPomelo,
        manyToManyRequest);
    var mySqlOracleHierarchy = DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlOracle,
        hierarchyRequest);
    var oracleHierarchy = DataVaultProviderReadStrategyGateEvaluator.EvaluateOracle(
        KnownProviderNames.Oracle,
        hierarchyRequest);

    Assert.True(mySqlPomeloManyToMany.CanRead);
    Assert.Empty(mySqlPomeloManyToMany.FallbackCauses);
    Assert.True(mySqlOracleHierarchy.CanRead);
    Assert.Empty(mySqlOracleHierarchy.FallbackCauses);
    Assert.True(oracleHierarchy.CanRead);
    Assert.Empty(oracleHierarchy.FallbackCauses);
  }

  [Fact]
  public void Db2BridgeReadGateAcceptsManyToManyAndHierarchyShapes() {
    var manyToManyRequest = CreateBridgeReadRequest(["customer-hk"]);
    var hierarchyRequest = CreateHierarchyBridgeReadRequest(["ancestor-hk"]);

    var db2ManyToMany = DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(
        KnownProviderNames.Db2,
        manyToManyRequest);
    var db2Hierarchy = DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(
        KnownProviderNames.Db2,
        hierarchyRequest);

    Assert.True(db2ManyToMany.CanRead);
    Assert.Empty(db2ManyToMany.FallbackCauses);
    Assert.True(db2Hierarchy.CanRead);
    Assert.Empty(db2Hierarchy.FallbackCauses);
  }

  [Fact]
  public void PostgresAndSqlServerPitAndBridgeReadGatesFailClosedForIncompleteReadShapeEvidence() {
    var pitRequest = CreatePitReadRequest(["customer-hk"]);
    var bridgeRequest = CreateBridgeReadRequest(["customer-hk"]);

    var postgresPit = DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.Postgres,
        pitRequest,
        hasCompleteReadShapeEvidence: false);
    var sqlServerBridge = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        bridgeRequest,
        hasCompleteReadShapeEvidence: false);

    Assert.False(postgresPit.CanRead);
    Assert.Contains(
        postgresPit.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence);
    Assert.False(sqlServerBridge.CanRead);
    Assert.Contains(
        sqlServerBridge.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence);
  }

  [Fact]
  public void MySqlAndOracleBridgeReadGatesFailClosedForProviderShapeEvidenceAndMaintenanceFallbacks() {
    var bridgeRequest = CreateBridgeReadRequest(["customer-hk"]);
    var unsupportedFeatureRequest = CreateUnsupportedFeatureBridgeReadRequest(["customer-hk"]);

    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.SqlServer, bridgeRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateOracle(KnownProviderNames.MySqlOracle, bridgeRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlPomelo, unsupportedFeatureRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateOracle(KnownProviderNames.Oracle, unsupportedFeatureRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlPomelo, bridgeRequest, hasCompleteReadShapeEvidence: false)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateOracle(KnownProviderNames.Oracle, bridgeRequest, hasCompleteReadShapeEvidence: false)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateMySql(
                KnownProviderNames.MySqlOracle,
                bridgeRequest,
                hasCompleteReadShapeEvidence: true,
                hasStaleReadModelMaintenanceSignal: true)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateOracle(
                KnownProviderNames.Oracle,
                bridgeRequest,
                hasCompleteReadShapeEvidence: true,
                hasStaleReadModelMaintenanceSignal: true)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance);
  }

  [Fact]
  public void Db2BridgeReadGateFailsClosedForProviderShapeEvidenceAndMaintenanceFallbacks() {
    var bridgeRequest = CreateBridgeReadRequest(["customer-hk"]);
    var unsupportedFeatureRequest = CreateUnsupportedFeatureBridgeReadRequest(["customer-hk"]);

    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateDb2(KnownProviderNames.SqlServer, bridgeRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateDb2(KnownProviderNames.Db2, unsupportedFeatureRequest)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateDb2(KnownProviderNames.Db2, bridgeRequest, hasCompleteReadShapeEvidence: false)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence);
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator
            .EvaluateDb2(
                KnownProviderNames.Db2,
                bridgeRequest,
                hasCompleteReadShapeEvidence: true,
                hasStaleReadModelMaintenanceSignal: true)
            .FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance);
  }

  [Fact]
  public void PostgresAndSqlServerPitAndBridgeReadGatesFailClosedForStaleMaintenanceSignals() {
    var pitRequest = CreatePitReadRequest(["customer-hk"]);
    var bridgeRequest = CreateBridgeReadRequest(["customer-hk"]);

    var postgresPit = DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.Postgres,
        pitRequest,
        hasCompleteReadShapeEvidence: true,
        hasStaleReadModelMaintenanceSignal: true);
    var sqlServerBridge = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        bridgeRequest,
        hasCompleteReadShapeEvidence: true,
        hasStaleReadModelMaintenanceSignal: true);

    Assert.False(postgresPit.CanRead);
    Assert.Contains(
        postgresPit.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance);
    Assert.False(sqlServerBridge.CanRead);
    Assert.Contains(
        sqlServerBridge.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance);
  }

  private static DataVaultLatestSatelliteReadRequest CreateReadRequest(IEnumerable<string> parentHashKeys) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Name"]);

    return new DataVaultLatestSatelliteReadRequest(profile, parentHashKeys);
  }

  private static DataVaultPitAsOfReadRequest CreatePitReadRequest(IEnumerable<string> parentHashKeys) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile"]);

    return CreatePitReadRequest(pit, parentHashKeys);
  }

  private static DataVaultPitAsOfReadRequest CreatePitReadRequest(
      DataVaultPitMetadata pit,
      IEnumerable<string> parentHashKeys) {
    return new DataVaultPitAsOfReadRequest(
        pit,
        parentHashKeys,
        new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero));
  }

  private static DataVaultBridgeReadRequest CreateBridgeReadRequest(IEnumerable<string> endpointHashKeys) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());

    return new DataVaultBridgeReadRequest(
        bridge,
        DataVaultBridgeTraversalEndpoint.From,
        endpointHashKeys);
  }

  private static DataVaultBridgeReadRequest CreateHierarchyBridgeReadRequest(IEnumerable<string> endpointHashKeys) {
    var employee = new DataVaultHubMetadata("Employee", ["Employee Id"]);
    var employeeManager = new DataVaultLinkMetadata("EmployeeManager", [employee.ToReference(), employee.ToReference()]);
    var bridge = DataVaultBridgeMetadata.Hierarchy(
        "EmployeeManager",
        employee.ToReference(),
        employeeManager.ToReference(),
        employee.ToReference(),
        ancestorParticipantOrdinal: 0,
        descendantParticipantOrdinal: 1);

    return new DataVaultBridgeReadRequest(
        bridge,
        DataVaultBridgeTraversalEndpoint.Ancestor,
        endpointHashKeys,
        maximumDepth: 3);
  }

  private static DataVaultBridgeReadRequest CreateUnsupportedFeatureBridgeReadRequest(IEnumerable<string> endpointHashKeys) {
    var bridge = new DataVaultBridgeMetadata(
        "CustomerOrder",
        DataVaultBridgeKind.ManyToMany,
        DataVaultMetadataReference.Link("CustomerOrder"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.From,
                DataVaultMetadataReference.Hub("Customer"),
                "Customer"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.To,
                DataVaultMetadataReference.Hub("Order"),
                "Order"),
        ],
        DataVaultBridgeProjectionFeatures.PathPayload);

    return new DataVaultBridgeReadRequest(
        bridge,
        DataVaultBridgeTraversalEndpoint.From,
        endpointHashKeys);
  }

  private sealed class DispatchProbeReadStrategy(
      string strategyName,
      int priority,
      bool canRead,
      List<string> evaluationOrder) : IDataVaultProviderReadStrategy {
    private static readonly DateTimeOffset LoadTimestamp = new(2026, 5, 11, 12, 0, 0, TimeSpan.Zero);

    public int CanReadCallCount { get; private set; }

    public int ReadCallCount { get; private set; }

    public int ProjectionReadCallCount { get; private set; }

    public int Priority { get; } = priority;

    public bool CanReadLatestSatelliteRows(
        DbContext dbContext,
        DataVaultLatestSatelliteReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      CanReadCallCount++;
      evaluationOrder.Add(strategyName);

      return canRead;
    }

    public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      ReadCallCount++;

      return Task.FromResult<IReadOnlyList<DataVaultSatelliteReadRecord>>([
          new DataVaultSatelliteReadRecord(
              strategyName,
              "StrategyProbe",
              "customer-hk",
              new Dictionary<string, string>(StringComparer.Ordinal),
              strategyName + "-hash",
              LoadTimestamp,
              strategyName,
              new Dictionary<string, string>(StringComparer.Ordinal) {
                ["Name"] = strategyName + " name",
              }),
      ]);
    }

    public Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      ProjectionReadCallCount++;

      return Task.FromResult<IReadOnlyList<DataVaultSatelliteProjectionRow>>([
          new DataVaultSatelliteProjectionRow(
              strategyName,
              new Dictionary<string, DataVaultSatelliteProjectionValue>(StringComparer.Ordinal) {
                ["ParentHashKey"] = DataVaultSatelliteProjectionValue.Present("customer-hk"),
                ["HashDiff"] = DataVaultSatelliteProjectionValue.Present(strategyName + "-hash"),
                ["LoadTimestamp"] = DataVaultSatelliteProjectionValue.Present(LoadTimestamp),
                ["RecordSource"] = DataVaultSatelliteProjectionValue.Present(strategyName),
                ["Name"] = DataVaultSatelliteProjectionValue.Present(strategyName + " name"),
              }),
      ]);
    }
  }

  private sealed class DispatchProbePitReadStrategy(
      int priority,
      bool canRead) : IDataVaultProviderPitReadStrategy {
    private static readonly DateTimeOffset LoadTimestamp = new(2026, 5, 11, 12, 0, 0, TimeSpan.Zero);

    public int CanReadCallCount { get; private set; }

    public int ReadCallCount { get; private set; }

    public int Priority { get; } = priority;

    public bool CanReadPitRows(
        DbContext dbContext,
        DataVaultPitAsOfReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      CanReadCallCount++;

      return canRead;
    }

    public Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
        DataVaultProviderPitReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      ReadCallCount++;

      return Task.FromResult<IReadOnlyList<DataVaultPitReadRecord>>([
          new DataVaultPitReadRecord(
              "customer-hk",
              LoadTimestamp,
              new Dictionary<string, string>(StringComparer.Ordinal),
              [DataVaultPitSatelliteSnapshot.Missing("Profile", 0)]),
      ]);
    }
  }

  private sealed class DispatchProbeBridgeReadStrategy(
      int priority,
      bool canRead) : IDataVaultProviderBridgeReadStrategy {
    public int CanReadCallCount { get; private set; }

    public int ReadCallCount { get; private set; }

    public int ProjectionReadCallCount { get; private set; }

    public int Priority { get; } = priority;

    public bool CanReadBridgeRows(
        DbContext dbContext,
        DataVaultBridgeReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      CanReadCallCount++;

      return canRead;
    }

    public Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
        DataVaultProviderBridgeReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      ReadCallCount++;

      return Task.FromResult<IReadOnlyList<DataVaultBridgeReadRecord>>([
          new DataVaultBridgeReadRecord(
              "StrategyProbeBridge",
              "StrategyProbeBridge",
              [
                  new DataVaultBridgeEndpointReadValue(
                      DataVaultBridgeTraversalEndpoint.From,
                      "Customer",
                      "CustomerHashKey",
                      "customer-hk"),
              ],
              traversalDepth: null),
      ]);
    }

    public Task<IReadOnlyList<DataVaultBridgeProjectionRow>> ReadBridgeProjectionRowsAsync(
        DataVaultProviderBridgeReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      ProjectionReadCallCount++;

      return Task.FromResult<IReadOnlyList<DataVaultBridgeProjectionRow>>([
          new DataVaultBridgeProjectionRow(
              "StrategyProbeBridge",
              new Dictionary<string, DataVaultBridgeProjectionValue>(StringComparer.Ordinal) {
                ["CustomerHashKey"] = DataVaultBridgeProjectionValue.Present("customer-hk"),
              }),
      ]);
    }
  }
}
