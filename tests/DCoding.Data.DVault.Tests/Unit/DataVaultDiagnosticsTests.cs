using System.Text.Json;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultDiagnosticsTests {
  [Fact]
  public void AnalyzeMetadataModelReturnsSerializableExplainAndNotEvaluatedStrategy() {
    using var provider = CreateServiceProvider();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();

    var result = diagnostics.Analyze(CreateCustomerMetadataModel());

    Assert.True(result.Validation.IsValid);
    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated, result.SaveStrategy.Status);
    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.NotEvaluated, result.ReadStrategy.Status);
    Assert.Equal("sqlite-v1", result.Explain.CapabilityProfileName);
    Assert.Equal("provider-neutral-v1", result.Explain.ProviderBehaviorProfileName);
    Assert.Equal(
        ["HubCustomer", "SatCustomerProfile"],
        result.Explain.Entities.Select(entity => entity.TableName).ToArray());
    Assert.Equal(
        DataVaultLogicalPropertyKind.LoadTimestamp,
        result.Explain.Entities[0]
            .Properties
            .Single(property => property.TechnicalRole == TechnicalMetadataColumnRole.LoadTimestamp)
            .LogicalPropertyKind);
    Assert.Equal(DataVaultProviderValueFormat.Iso8601UtcText, result.Explain.SatelliteSnapshotReferenceValueFormat);
    Assert.Equal("TEXT", result.Explain.SatelliteSnapshotReferenceStoreType);
    Assert.Contains(
        result.Explain.TypeMappings,
        mapping => mapping.LogicalPropertyKind == DataVaultLogicalPropertyKind.SatelliteSnapshotReference &&
            mapping.StoreType == "TEXT");
    Assert.Equal(DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported, result.Explain.SqlFunctionSupport);
    Assert.Equal(DataVaultProviderConcurrencySupport.NoneInV1Unsupported, result.Explain.ConcurrencySupport);

    var json = JsonSerializer.Serialize(result);
    Assert.Contains("HubCustomer", json, StringComparison.Ordinal);
    Assert.Contains("SatelliteSnapshotReference", json, StringComparison.Ordinal);
    Assert.Contains("not", result.ToDisplayString(), StringComparison.OrdinalIgnoreCase);
  }

  [Fact]
  public void AnalyzeRegistryAndCodeFirstUseTheSameStructuredResultShape() {
    using var provider = CreateServiceProvider();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var metadataModel = CreateCustomerMetadataModel();
    var registry = DataVaultMetadataRegistry.Create(metadataModel);

    var registryResult = diagnostics.Analyze(registry, DataVaultProviderCapabilityProfiles.Postgres);
    var codeFirstResult = diagnostics.Analyze(
        model => model.Hub<Customer>(hub => hub
            .BusinessKey(customer => customer.CustomerNumber)
            .Satellite("Profile", satellite => satellite.Payload(customer => customer.Name))),
        DataVaultProviderCapabilityProfiles.Postgres);

    Assert.True(registryResult.Validation.IsValid);
    Assert.True(codeFirstResult.Validation.IsValid);
    Assert.Equal("postgres-v1", registryResult.Explain.CapabilityProfileName);
    Assert.Equal("postgres-v1", codeFirstResult.Explain.CapabilityProfileName);
    Assert.Equal(
        registryResult.Explain.Entities.Select(entity => entity.TableKind),
        codeFirstResult.Explain.Entities.Select(entity => entity.TableKind));
  }

  [Fact]
  public void ReadDiagnosticsPopulateReadShapeForExplicitRegistryPitAndBridgeRequests() {
    var metadata = CreateReadShapeMetadata();
    var optionsBuilder = new DbContextOptionsBuilder<ReadShapeDiagnosticsContext>()
        .UseSqlite("Data Source=:memory:");
    optionsBuilder.UseDataVaultMetadata(DataVaultMetadataRegistry.Create(metadata.Model));
    var options = optionsBuilder.Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();
    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    var asOf = new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero);

    using var context = new ReadShapeDiagnosticsContext(options);
    var explicitLatest = diagnostics.Analyze(
        context,
        new DataVaultLatestSatelliteReadRequest(metadata.Profile, ["customer-hk"], asOf));
    var registryLatest = diagnostics.Analyze(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            metadata.Customer.ToReference(),
            "Profile",
            ["different-customer-hk"],
            asOf));
    var pit = diagnostics.Analyze(
        context,
        new DataVaultPitAsOfReadRequest(metadata.Pit, ["customer-hk"], asOf));
    var explicitBridge = diagnostics.Analyze(
        context,
        new DataVaultBridgeReadRequest(
            metadata.Bridge,
            DataVaultBridgeTraversalEndpoint.From,
            ["customer-hk"]));
    var hierarchyBridge = diagnostics.Analyze(
        context,
        new DataVaultBridgeReadRequest(
            metadata.HierarchyBridge,
            DataVaultBridgeTraversalEndpoint.Ancestor,
            ["sales-region-hk"],
            maximumDepth: 2));
    var registryBridge = diagnostics.Analyze(
        context,
        new DataVaultRegistryBridgeReadRequest(
            "CustomerOrder",
            DataVaultBridgeTraversalEndpoint.From,
            ["different-customer-hk"]));

    Assert.NotNull(explicitLatest.ReadShape);
    var latestShape = explicitLatest.ReadShape!;
    Assert.Equal(DataVaultReadShapeKind.LatestSatellite, latestShape.Kind);
    Assert.NotNull(latestShape.Satellite);
    var latestSatelliteShape = latestShape.Satellite!;
    Assert.Equal(DataVaultSatelliteReadSemantics.AsOf, latestSatelliteShape.Semantics);
    Assert.Equal("SatCustomerProfile", latestSatelliteShape.Satellite.TableName);
    Assert.Equal(["CustomerHashKey"], latestSatelliteShape.FilterColumns[0].ColumnNames);
    Assert.Equal(["LoadTimestamp"], latestSatelliteShape.FilterColumns[1].ColumnNames);
    AssertColumnSet(
        latestSatelliteShape.ProjectedColumns.Single(columns => columns.Role == "technicalProjection"),
        "technicalProjection",
        ["CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource"]);
    AssertColumnSet(
        latestSatelliteShape.ProjectedColumns.Single(columns => columns.Role == "payloadProjection"),
        "payloadProjection",
        ["CustomerName", "CustomerTier"]);
    Assert.DoesNotContain(latestSatelliteShape.ProjectedColumns, columns => columns.Role == "drivingKeyProjection");
    Assert.Contains(
        latestSatelliteShape.ExpectedIndexBaseline,
        index => index.Kind == "secondary-index" && index.DescendingColumnNames.Contains("LoadTimestamp"));
    Assert.NotNull(registryLatest.ReadShape);
    Assert.NotNull(registryLatest.ReadShape!.Satellite);
    var registrySatelliteShape = registryLatest.ReadShape.Satellite!;
    Assert.Equal(latestSatelliteShape.Semantics, registrySatelliteShape.Semantics);
    Assert.Equal(latestSatelliteShape.Satellite, registrySatelliteShape.Satellite);
    Assert.Equal(
        latestSatelliteShape.FilterColumns.SelectMany(columns => columns.ColumnNames),
        registrySatelliteShape.FilterColumns.SelectMany(columns => columns.ColumnNames));
    Assert.Equal(
        latestSatelliteShape.ProjectedColumns.Select(columns => columns.Role),
        registrySatelliteShape.ProjectedColumns.Select(columns => columns.Role));
    Assert.Equal(
        latestSatelliteShape.ProjectedColumns.SelectMany(columns => columns.ColumnNames),
        registrySatelliteShape.ProjectedColumns.SelectMany(columns => columns.ColumnNames));

    Assert.NotNull(pit.ReadShape);
    var pitShape = pit.ReadShape!;
    Assert.Equal(DataVaultReadShapeKind.PitAsOf, pitShape.Kind);
    Assert.NotNull(pitShape.Pit);
    var pitReadShape = pitShape.Pit!;
    Assert.Equal("PitCustomerProfileStatus", pitReadShape.Pit.TableName);
    Assert.Equal(["Profile", "Status"], pitReadShape.ReferencedSatellites.Select(satellite => satellite.MetadataName).ToArray());
    Assert.Equal(2, pitReadShape.ReferencedSatelliteLookupCount);
    AssertColumnSet(
        pitReadShape.ProjectedColumns.Single(columns => columns.Role == "pitTechnicalProjection"),
        "pitTechnicalProjection",
        ["CustomerHashKey", "LoadTimestamp"]);
    AssertColumnSet(
        pitReadShape.ProjectedColumns.Single(columns => columns.Role == "snapshotReferenceProjection"),
        "snapshotReferenceProjection",
        ["ProfileLoadTimestamp", "StatusLoadTimestamp"]);
    AssertColumnSet(
        pitReadShape.ProjectedColumns.Single(columns => columns.Role == "satellitePayloadProjection"),
        "satellitePayloadProjection",
        ["CustomerName", "CustomerTier", "StatusCode"]);
    Assert.Contains("no latest-satellite fallback", pitReadShape.NoLatestFallbackBehavior, StringComparison.Ordinal);
    Assert.Contains(pitReadShape.ExpectedIndexBaseline, index => index.Kind == "primary-key");

    Assert.NotNull(explicitBridge.ReadShape);
    var bridgeShape = explicitBridge.ReadShape!;
    Assert.Equal(DataVaultReadShapeKind.Bridge, bridgeShape.Kind);
    Assert.NotNull(bridgeShape.Bridge);
    var bridgeReadShape = bridgeShape.Bridge!;
    Assert.Equal(DataVaultBridgeKind.ManyToMany, bridgeReadShape.BridgeKind);
    Assert.Equal("BridgeCustomerOrder", bridgeReadShape.Bridge.TableName);
    Assert.Equal(["CustomerHashKey"], bridgeReadShape.EndpointFilter.ColumnNames);
    AssertColumnSet(
        bridgeReadShape.ProjectedColumns.Single(columns => columns.Role == "endpointProjection"),
        "endpointProjection",
        ["CustomerHashKey", "OrderHashKey"]);
    Assert.DoesNotContain(bridgeReadShape.ProjectedColumns, columns => columns.Role == "depthProjection");
    Assert.Contains(
        bridgeReadShape.ExpectedTraversalIndexBaseline,
        index => index.Kind == "secondary-index" && index.ColumnNames.SequenceEqual(["OrderHashKey", "CustomerHashKey"]));
    Assert.NotNull(hierarchyBridge.ReadShape);
    Assert.NotNull(hierarchyBridge.ReadShape!.Bridge);
    var hierarchyBridgeShape = hierarchyBridge.ReadShape.Bridge!;
    Assert.Equal(DataVaultBridgeKind.Hierarchy, hierarchyBridgeShape.BridgeKind);
    AssertColumnSet(
        hierarchyBridgeShape.ProjectedColumns.Single(columns => columns.Role == "endpointProjection"),
        "endpointProjection",
        ["AncestorSalesRegionHashKey", "DescendantSalesRegionHashKey"]);
    AssertColumnSet(
        hierarchyBridgeShape.ProjectedColumns.Single(columns => columns.Role == "depthProjection"),
        "depthProjection",
        ["TraversalDepth"]);
    Assert.NotNull(registryBridge.ReadShape);
    Assert.NotNull(registryBridge.ReadShape!.Bridge);
    var registryBridgeShape = registryBridge.ReadShape.Bridge!;
    Assert.Equal(bridgeReadShape.Bridge, registryBridgeShape.Bridge);
    Assert.Equal(bridgeReadShape.FilterEndpoint, registryBridgeShape.FilterEndpoint);
    Assert.Equal(bridgeReadShape.EndpointFilter.ColumnNames, registryBridgeShape.EndpointFilter.ColumnNames);
  }

  [Fact]
  public void SupportBundleSerializesReadShapeWithoutRequestValues() {
    var metadata = CreateReadShapeMetadata();
    var optionsBuilder = new DbContextOptionsBuilder<ReadShapeDiagnosticsContext>()
        .UseSqlite("Data Source=:memory:");
    optionsBuilder.UseDataVaultMetadata(DataVaultMetadataRegistry.Create(metadata.Model));
    var options = optionsBuilder.Options;
    var services = new ServiceCollection();
    services.AddDVault();
    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();

    using var context = new ReadShapeDiagnosticsContext(options);
    var result = diagnostics.Analyze(
        context,
        new DataVaultLatestSatelliteReadRequest(metadata.Profile, ["secret-customer-hash-key"]));

    var json = DataVaultSupportBundleExporter.ExportJson(result);

    Assert.Contains("\"readShape\"", json, StringComparison.Ordinal);
    Assert.Contains("\"kind\": \"LatestSatellite\"", json, StringComparison.Ordinal);
    Assert.Contains("\"readStrategyStatus\": \"ProviderNeutralFallback\"", json, StringComparison.Ordinal);
    Assert.Contains("\"parentHashKeyFilter\"", json, StringComparison.Ordinal);
    Assert.DoesNotContain("secret-customer-hash-key", json, StringComparison.Ordinal);
  }

  [Fact]
  public void AnalyzeBuiltInProviderProfilesAndLoadTimestampStorageVariants() {
    using var provider = CreateServiceProvider();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var metadataModel = CreateCustomerMetadataModel();
    var profiles = new[]
    {
        DataVaultProviderCapabilityProfiles.Sqlite,
        DataVaultProviderCapabilityProfiles.Postgres,
        DataVaultProviderCapabilityProfiles.SqlServer,
        DataVaultProviderCapabilityProfiles.Oracle,
        DataVaultProviderCapabilityProfiles.MySql,
    };

    foreach (var profile in profiles) {
      foreach (var storage in new[] {
        DataVaultLoadTimestampStorage.ProviderDefault,
        DataVaultLoadTimestampStorage.Iso8601UtcText,
        DataVaultLoadTimestampStorage.UtcTicks,
      }) {
        var selectedProfile = profile.WithLoadTimestampStorage(storage);
        var result = diagnostics.Analyze(metadataModel, selectedProfile);

        Assert.True(result.Validation.IsValid);
        Assert.Equal(selectedProfile.ProfileName, result.Explain.CapabilityProfileName);
        Assert.NotEmpty(result.Explain.LoadTimestampStoreType);
        Assert.NotEmpty(result.Explain.SatelliteSnapshotReferenceStoreType);
        Assert.NotEmpty(result.Explain.TypeMappings);
        Assert.Equal(selectedProfile.MaximumIdentifierLength, result.Explain.MaximumIdentifierLength);
        Assert.Equal(selectedProfile.AllowsIndexesCoveredByPrimaryKey, result.Explain.AllowsIndexesCoveredByPrimaryKey);
        Assert.Equal(selectedProfile.UnsupportedIncludedIndexColumnMode, result.Explain.UnsupportedIncludedIndexColumnMode);
        Assert.All(
            result.Explain.Entities.SelectMany(entity => entity.Properties),
            property => Assert.Equal(selectedProfile.ProfileName, property.ProviderProfileName));
      }
    }

    var mySqlResult = diagnostics.Analyze(metadataModel, DataVaultProviderCapabilityProfiles.MySql);
    Assert.Equal(64, mySqlResult.Explain.MaximumIdentifierLength);
    Assert.Equal(DataVaultUnsupportedIncludedIndexColumnMode.Ignore, mySqlResult.Explain.UnsupportedIncludedIndexColumnMode);

    var oracleResult = diagnostics.Analyze(metadataModel, DataVaultProviderCapabilityProfiles.Oracle);
    Assert.False(oracleResult.Explain.AllowsIndexesCoveredByPrimaryKey);
  }

  [Fact]
  public void MigrationOperationGuardrailsUseExistingDiagnosticsResultSurface() {
    using var provider = CreateServiceProvider();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var baseline = diagnostics.Analyze(CreateMigrationGuardrailMetadataModel());

    var safeResult = DataVaultMigrationOperationDiagnostics.Analyze(
        baseline,
        [
            new AddColumnOperation {
              Table = "SatCustomerContact",
              Name = "PhoneNumber",
              ClrType = typeof(string),
            },
            new DropColumnOperation {
              Table = "SatCustomerContact",
              Name = "EmailAddress",
            },
            new DropTableOperation {
              Name = "LegacyAuditScratch",
            },
            new RenameColumnOperation {
              Table = "SatCustomerContact",
              Name = "EmailAddress",
              NewName = "StatusCode",
            },
            new CreateIndexOperation {
              Table = "SatCustomerContact",
              Name = "IX_SatCustomerContact_EmailAddress",
              Columns = ["EmailAddress"],
              IsUnique = false,
            },
            new AlterColumnOperation {
              Table = "SatCustomerContact",
              Name = "EmailAddress",
              ClrType = typeof(string),
            },
        ]);

    Assert.True(safeResult.Validation.IsValid);
    Assert.Empty(safeResult.Issues);

    var findingResult = DataVaultMigrationOperationDiagnostics.Analyze(
        baseline,
        [
            new AddColumnOperation {
              Table = "HubCustomer",
              Name = "CustomerStatus",
              ClrType = typeof(string),
            },
            new DropColumnOperation {
              Table = "SatCustomerContact",
              Name = "HashDiff",
            },
            new DropColumnOperation {
              Table = "SatCustomerContact",
              Name = "CustomerHashKey",
            },
            new CreateIndexOperation {
              Table = "HubCustomer",
              Name = "IxHubCustomerBusinessKeyCustomerId",
              Columns = ["RecordSource"],
              IsUnique = false,
            },
            new RenameColumnOperation {
              Table = "HubCustomer",
              Name = "LoadTimestamp",
              NewName = "LoadedAt",
            },
            new AlterColumnOperation {
              Table = "HubCustomer",
              Name = "RecordSource",
              ClrType = typeof(string),
            },
            new AlterColumnOperation {
              Table = "LinkCustomerOrder",
              Name = "OrderHashKey",
              ClrType = typeof(string),
            },
            new DropTableOperation {
              Name = "HubCustomer",
            },
        ]);

    Assert.False(findingResult.Validation.IsValid);
    Assert.Collection(
        findingResult.Issues,
        issue => AssertMigrationIssue(
            issue,
            "DVM2001",
            DataVaultDiagnosticsIssueSeverity.Error,
            "migration/AddColumn/HubCustomer/CustomerStatus",
            "MI-1"),
        issue => AssertMigrationIssue(
            issue,
            "DVM2002",
            DataVaultDiagnosticsIssueSeverity.Error,
            "migration/DropColumn/SatCustomerContact/HashDiff",
            "MI-2"),
        issue => AssertMigrationIssue(
            issue,
            "DVM2003",
            DataVaultDiagnosticsIssueSeverity.Error,
            "migration/DropColumn/SatCustomerContact/CustomerHashKey",
            "MI-3"),
        issue => AssertMigrationIssue(
            issue,
            "DVM2004",
            DataVaultDiagnosticsIssueSeverity.Warning,
            "migration/CreateIndex/HubCustomer/IxHubCustomerBusinessKeyCustomerId",
            "MI-4"),
        issue => AssertMigrationIssue(
            issue,
            "DVM2005",
            DataVaultDiagnosticsIssueSeverity.Warning,
            "migration/RenameColumn/HubCustomer/LoadTimestamp",
            "MI-5"),
        issue => AssertMigrationIssue(
            issue,
            "DVM2002",
            DataVaultDiagnosticsIssueSeverity.Error,
            "migration/AlterColumn/HubCustomer/RecordSource",
            "MI-2"),
        issue => AssertMigrationIssue(
            issue,
            "DVM2003",
            DataVaultDiagnosticsIssueSeverity.Error,
            "migration/AlterColumn/LinkCustomerOrder/OrderHashKey",
            "MI-3"),
        issue => AssertMigrationIssue(
            issue,
            "DVM2006",
            DataVaultDiagnosticsIssueSeverity.Error,
            "migration/DropTable/HubCustomer",
            "MI-5"));
    Assert.Equal(
        ["DVM2001", "DVM2002", "DVM2003", "DVM2002", "DVM2003", "DVM2006"],
        findingResult.Validation.Issues.Select(issue => issue.Code));
  }

  [Fact]
  public void ProviderReadStrategyGateReportsMaterialFallbackCauses() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerNumber"]);
    var supportedSatellite = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Name"]);
    var linkParentSatellite = new DataVaultSatelliteMetadata(
        "Fulfillment",
        DataVaultMetadataReference.Link("OrderProduct"),
        ["State"]);
    var multiActiveSatellite = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["EmailAddress"],
        ["ContactType"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile"]);
    var unsupportedPit = new DataVaultPitMetadata(DataVaultMetadataReference.Link("OrderProduct"), ["Fulfillment"]);
    var order = new DataVaultHubMetadata("Order", ["OrderNumber"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());

    var supported = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(
        KnownProviderNames.Sqlite,
        new DataVaultLatestSatelliteReadRequest(supportedSatellite, ["customer-hk"]));
    var unknownProvider = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(
        "Contoso.UnknownProvider",
        new DataVaultLatestSatelliteReadRequest(supportedSatellite, ["customer-hk"]));
    var unsupportedParent = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(
        KnownProviderNames.Sqlite,
        new DataVaultLatestSatelliteReadRequest(linkParentSatellite, ["link-hk"]));
    var multiActive = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(
        KnownProviderNames.Sqlite,
        new DataVaultLatestSatelliteReadRequest(multiActiveSatellite, ["customer-hk"]));
    var supportedPit = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(
        KnownProviderNames.Sqlite,
        new DataVaultPitAsOfReadRequest(
            pit,
            ["customer-hk"],
            new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero)));
    var unsupportedPitParent = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(
        KnownProviderNames.Sqlite,
        new DataVaultPitAsOfReadRequest(
            unsupportedPit,
            ["link-hk"],
            new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero)));
    var supportedBridge = DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(
        KnownProviderNames.Sqlite,
        new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.From,
            ["customer-hk"]));

    Assert.True(supported.CanRead);
    Assert.Empty(supported.FallbackCauses);
    Assert.True(supportedPit.CanRead);
    Assert.Empty(supportedPit.FallbackCauses);
    Assert.True(supportedBridge.CanRead);
    Assert.Empty(supportedBridge.FallbackCauses);
    Assert.Contains(
        unknownProvider.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        unsupportedParent.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent);
    Assert.Contains(
        multiActive.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
    Assert.Contains(
        unsupportedPitParent.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape);

    var readStrategy = new SqliteDataVaultReadStrategy();
    Assert.Equal([KnownProviderNames.Sqlite], DataVaultProviderReadStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(readStrategy));
    Assert.Contains(
        DataVaultProviderReadStrategyGateEvaluator.GetKnownLatestSatelliteGateRequirements(readStrategy),
        requirement => requirement.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
  }

  [Fact]
  public void ProviderSaveStrategyGateReportsMaterialFallbackCauses() {
    var smallBatch = CreateRequests(totalOperationCount: 1, satelliteOperationCount: 0);
    var sqlServerTooSmall = DataVaultProviderSaveStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        hasPendingTrackedChanges: false,
        smallBatch);
    var mySqlTooSmall = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlPomelo,
        hasPendingTrackedChanges: false,
        smallBatch);
    var mySqlStagedTooSmall = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySqlStaged(
        KnownProviderNames.MySqlPomelo,
        hasPendingTrackedChanges: false,
        CreateRequests(totalOperationCount: 50, satelliteOperationCount: 0));
    var oracleTooSmall = DataVaultProviderSaveStrategyGateEvaluator.EvaluateOracle(
        KnownProviderNames.Oracle,
        hasPendingTrackedChanges: false,
        smallBatch);
    var sqlServerTooManySatellites = DataVaultProviderSaveStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        hasPendingTrackedChanges: false,
        CreateRequests(totalOperationCount: 501, satelliteOperationCount: 501));
    var oracleTooManySatellites = DataVaultProviderSaveStrategyGateEvaluator.EvaluateOracle(
        KnownProviderNames.Oracle,
        hasPendingTrackedChanges: false,
        CreateRequests(totalOperationCount: 10001, satelliteOperationCount: 10001));
    var dirtyContext = DataVaultProviderSaveStrategyGateEvaluator.EvaluateSqlite(
        KnownProviderNames.Sqlite,
        hasPendingTrackedChanges: true,
        smallBatch);
    var multiActive = DataVaultProviderSaveStrategyGateEvaluator.EvaluatePostgres(
        KnownProviderNames.Postgres,
        hasPendingTrackedChanges: false,
        [CreateMultiActiveSatelliteRequest()]);
    var unknownProvider = DataVaultProviderSaveStrategyGateEvaluator.EvaluateSqlite(
        "Contoso.UnknownProvider",
        hasPendingTrackedChanges: false,
        smallBatch);

    Assert.Contains(
        sqlServerTooSmall.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold);
    Assert.Contains(
        mySqlTooSmall.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold);
    Assert.Contains(
        mySqlStagedTooSmall.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold &&
            cause.Message.Contains("MySQL staged bulk", StringComparison.Ordinal));
    Assert.Contains(
        oracleTooSmall.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.OracleMinimumOperationThreshold);
    Assert.Contains(
        sqlServerTooManySatellites.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold);
    Assert.Contains(
        oracleTooManySatellites.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.OracleMaximumSatelliteOperationThreshold);
    Assert.Contains(
        dirtyContext.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext);
    Assert.Contains(
        multiActive.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MultiActiveSatelliteOperations);
    Assert.Contains(
        unknownProvider.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.ProviderNameMismatch);

    var saveStrategy = new SqlServerDataVaultSaveStrategy();
    Assert.Equal([KnownProviderNames.SqlServer], DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(saveStrategy));
    Assert.Contains(
        DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategyGateRequirements(saveStrategy),
        requirement => requirement.Kind == DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold &&
            requirement.MinimumTotalOperationCount == 50);
    Assert.Contains(
        DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategyGateRequirements(saveStrategy),
        requirement => requirement.Kind == DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold &&
            requirement.MaximumSatelliteOperationCount == 500);

    var mySqlStagedSaveStrategy = new MySqlStagedDataVaultSaveStrategy();
    Assert.Equal(
        [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
        DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(mySqlStagedSaveStrategy));
    Assert.Contains(
        DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategyGateRequirements(mySqlStagedSaveStrategy),
        requirement => requirement.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold &&
            requirement.MinimumTotalOperationCount == 60);

    using var oracleProvider = CreateOracleServiceProvider();
    var oracleSaveStrategy = oracleProvider
        .GetRequiredService<IEnumerable<IDataVaultProviderSaveStrategy>>()
        .Single(strategy => string.Equals(strategy.GetType().Name, "OracleDataVaultSaveStrategy", StringComparison.Ordinal));
    Assert.Equal([KnownProviderNames.Oracle], DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategySupportedProviderNames(oracleSaveStrategy));
    Assert.Contains(
        DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategyGateRequirements(oracleSaveStrategy),
        requirement => requirement.Kind == DataVaultSaveStrategyFallbackCauseKind.OracleMinimumOperationThreshold &&
            requirement.MinimumTotalOperationCount == 50);
    Assert.Contains(
        DataVaultProviderSaveStrategyGateEvaluator.GetKnownStrategyGateRequirements(oracleSaveStrategy),
        requirement => requirement.Kind == DataVaultSaveStrategyFallbackCauseKind.OracleMaximumSatelliteOperationThreshold &&
            requirement.MaximumSatelliteOperationCount == 10000);
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVault();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static ServiceProvider CreateOracleServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVaultOracle();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static DataVaultMetadataModel CreateCustomerMetadataModel() {
    var customerHub = new DataVaultHubMetadata("Customer", ["CustomerNumber"]);
    var customerSatellite = new DataVaultSatelliteMetadata(
        "Profile",
        DataVaultMetadataReference.Hub("Customer"),
        ["Name"]);

    return new DataVaultMetadataModel([customerHub], [], [customerSatellite]);
  }

  private static DataVaultMetadataModel CreateMigrationGuardrailMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var channel = new DataVaultSatelliteMetadata(
        "ContactChannel",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);

    return new DataVaultMetadataModel([customer, order], [customerOrder], [contact, channel]);
  }

  private static ReadShapeMetadata CreateReadShapeMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var salesRegion = new DataVaultHubMetadata("SalesRegion", ["Region Code"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var salesRegionParentChild = new DataVaultLinkMetadata(
        "SalesRegionParentChild",
        [
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ParentRegion"),
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ChildRegion"),
        ]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name", "Customer Tier"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());
    var hierarchyBridge = new DataVaultBridgeMetadata(
        "SalesRegionHierarchy",
        DataVaultBridgeKind.Hierarchy,
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Ancestor,
                salesRegion.ToReference(),
                "ParentRegion"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Descendant,
                salesRegion.ToReference(),
                "ChildRegion"),
        ]);
    var model = new DataVaultMetadataModel(
        [customer, order, salesRegion],
        [customerOrder, salesRegionParentChild],
        [profile, status],
        Array.Empty<DataVaultPointInTimeMetadata>(),
        [bridge, hierarchyBridge],
        [pit]);

    return new ReadShapeMetadata(model, customer, profile, pit, bridge, hierarchyBridge);
  }

  private static void AssertColumnSet(
      DataVaultReadShapeColumnSet columnSet,
      string role,
      IReadOnlyList<string> columnNames) {
    Assert.Equal(role, columnSet.Role);
    Assert.Equal(columnNames, columnSet.ColumnNames);
  }

  private static void AssertMigrationIssue(
      DataVaultDiagnosticsIssue issue,
      string code,
      DataVaultDiagnosticsIssueSeverity severity,
      string path,
      string invariant) {
    Assert.Equal(code, issue.Code);
    Assert.Equal(severity, issue.Severity);
    Assert.Equal(path, issue.Path);
    Assert.Contains(invariant, issue.Message, StringComparison.Ordinal);
    Assert.NotEmpty(DataVaultDiagnosticCatalog.GetMigrationOperationDefinition(code).Remediation);
  }

  private static IReadOnlyList<DataVaultSaveRequest> CreateRequests(
      int totalOperationCount,
      int satelliteOperationCount) {
    var hub = new DataVaultHubMetadata("Customer", ["CustomerNumber"]);
    var satellite = new DataVaultSatelliteMetadata(
        "Profile",
        DataVaultMetadataReference.Hub("Customer"),
        ["Name"]);
    var hubOperationCount = Math.Max(0, totalOperationCount - satelliteOperationCount);
    var hubOperations = Enumerable
        .Range(0, hubOperationCount)
        .Select(index => new DataVaultHubSaveOperation(
            hub,
            [new("CustomerNumber", "C-" + index)]))
        .ToArray();
    var satelliteOperations = Enumerable
        .Range(0, satelliteOperationCount)
        .Select(index => new DataVaultSatelliteSaveOperation(
            satellite,
            "hk-" + index,
            [new("Name", "Name " + index)],
            "hd-" + index))
        .ToArray();

    return [new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 10, 0, 0, 0, TimeSpan.Zero),
        "unit-test",
        hubOperations,
        [],
        satelliteOperations)];
  }

  private static DataVaultSaveRequest CreateMultiActiveSatelliteRequest() {
    var satellite = new DataVaultSatelliteMetadata(
        "Phone",
        DataVaultMetadataReference.Hub("Customer"),
        ["PhoneNumber"],
        ["PhoneType"]);

    return new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 10, 0, 0, 0, TimeSpan.Zero),
        "unit-test",
        [],
        [],
        [new DataVaultSatelliteSaveOperation(
            satellite,
            "customer-hk",
            [new("PhoneType", "mobile")],
            [new("PhoneNumber", "123")],
            "phone-hd")]);
  }

  private sealed class Customer {
    public string CustomerNumber { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
  }

  private sealed record ReadShapeMetadata(
      DataVaultMetadataModel Model,
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile,
      DataVaultPitMetadata Pit,
      DataVaultBridgeMetadata Bridge,
      DataVaultBridgeMetadata HierarchyBridge);

  private sealed class ReadShapeDiagnosticsContext(DbContextOptions<ReadShapeDiagnosticsContext> options) : DbContext(options) {
  }

  private sealed class SqlServerDataVaultSaveStrategy : IDataVaultProviderSaveStrategy {
    public int Priority => 100;

    public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
      return false;
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DataVaultProviderSaveStrategyContext context,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException("Probe strategy is not used for persistence.");
    }
  }

  private sealed class SqliteDataVaultReadStrategy : IDataVaultProviderReadStrategy {
    public int Priority => 100;

    public bool CanReadLatestSatelliteRows(
        DbContext dbContext,
        DataVaultLatestSatelliteReadRequest request) {
      return false;
    }

    public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException("Probe strategy is not used for reads.");
    }

    public Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException("Probe strategy is not used for reads.");
    }
  }

}
