using System.Text.Json;
using DCoding.Data.DVault.Modeling;
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

    var json = JsonSerializer.Serialize(result);
    Assert.Contains("HubCustomer", json, StringComparison.Ordinal);
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
        Assert.All(
            result.Explain.Entities.SelectMany(entity => entity.Properties),
            property => Assert.Equal(selectedProfile.ProfileName, property.ProviderProfileName));
      }
    }
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
    var oracleTooSmall = DataVaultProviderSaveStrategyGateEvaluator.EvaluateOracle(
        KnownProviderNames.Oracle,
        hasPendingTrackedChanges: false,
        smallBatch);
    var sqlServerTooManySatellites = DataVaultProviderSaveStrategyGateEvaluator.EvaluateSqlServer(
        KnownProviderNames.SqlServer,
        hasPendingTrackedChanges: false,
        CreateRequests(totalOperationCount: 501, satelliteOperationCount: 501));
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
        oracleTooSmall.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.OracleMinimumOperationThreshold);
    Assert.Contains(
        sqlServerTooManySatellites.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold);
    Assert.Contains(
        dirtyContext.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext);
    Assert.Contains(
        multiActive.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MultiActiveSatelliteOperations);
    Assert.Contains(
        unknownProvider.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.ProviderNameMismatch);
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVault();

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
}
