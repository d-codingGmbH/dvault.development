using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultProviderBehaviorTests {
  private const string SqliteProviderName = "Microsoft.EntityFrameworkCore.Sqlite";
  private const string PomeloProviderName = "Pomelo.EntityFrameworkCore.MySql";
  private const string OracleMySqlProviderName = "MySql.EntityFrameworkCore";
  private const string PostgresProviderName = "Npgsql.EntityFrameworkCore.PostgreSQL";
  private const string SqlServerProviderName = "Microsoft.EntityFrameworkCore.SqlServer";
  private const string OracleProviderName = "Oracle.EntityFrameworkCore";

  [Fact]
  public void AddDVaultRegistersDefaultInheritingProviderBehaviorSelector() {
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var selector = provider.GetRequiredService<IDataVaultProviderBehaviorSelector>();
    var selectedProfile = selector.SelectBehavior(new DataVaultProviderBehaviorContext(SqliteProviderName));

    Assert.Empty(provider.GetServices<IDataVaultProviderBehavior>());
    Assert.Same(DataVaultProviderBehaviorProfiles.ProviderNeutral, selectedProfile);
    Assert.Equal("provider-neutral-v1", selectedProfile.ProfileName);
  }

  [Fact]
  public void IncompatibleProviderBehaviorOverrideKeepsProviderNeutralBaseline() {
    var overrideBehavior = new ProviderNameBehavior(
        "contoso-provider-v1",
        "Contoso.EntityFrameworkCore.Provider",
        priority: 100);
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseProviderBehavior(overrideBehavior));

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var selector = provider.GetRequiredService<IDataVaultProviderBehaviorSelector>();
    var selectedProfile = selector.SelectBehavior(new DataVaultProviderBehaviorContext(SqliteProviderName));

    Assert.Same(DataVaultProviderBehaviorProfiles.ProviderNeutral, selectedProfile);
    Assert.Equal(1, overrideBehavior.CanApplyCallCount);
    Assert.Equal(0, overrideBehavior.CreateProfileCallCount);
  }

  [Fact]
  public void ExplicitProviderBehaviorRegistrationIsTheOnlyPathThatChangesSelection() {
    var baselineServices = new ServiceCollection();
    baselineServices.AddDVault();

    using (var baselineProvider = baselineServices.BuildServiceProvider(validateScopes: true)) {
      var baselineSelector = baselineProvider.GetRequiredService<IDataVaultProviderBehaviorSelector>();
      var baselineProfile = baselineSelector.SelectBehavior(
          new DataVaultProviderBehaviorContext("Contoso.EntityFrameworkCore.Provider"));

      Assert.Same(DataVaultProviderBehaviorProfiles.ProviderNeutral, baselineProfile);
    }

    var services = new ServiceCollection();
    services.AddDVault(options => options.UseProviderBehavior(
        new ProviderNameBehavior(
            "contoso-provider-v1",
            "Contoso.EntityFrameworkCore.Provider",
            priority: 100)));

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var selector = provider.GetRequiredService<IDataVaultProviderBehaviorSelector>();
    var selectedProfile = selector.SelectBehavior(new DataVaultProviderBehaviorContext("Contoso.EntityFrameworkCore.Provider"));
    var missingProviderProfile = selector.SelectBehavior(new DataVaultProviderBehaviorContext(SqliteProviderName));

    Assert.Equal("contoso-provider-v1", selectedProfile.ProfileName);
    Assert.Same(DataVaultProviderBehaviorProfiles.ProviderNeutral, missingProviderProfile);
  }

  [Fact]
  public void ProviderPackagesRegisterExplicitProviderBehaviorOverrides() {
    AssertProviderPackageBehavior(
        services => services.AddDVaultSqlite(),
        SqliteProviderName,
        "sqlite-provider-v1");
    AssertProviderPackageBehavior(
        services => services.AddDVaultMySql(),
        PomeloProviderName,
        "mysql-provider-v1");
    AssertProviderPackageBehavior(
        services => services.AddDVaultMySql(),
        OracleMySqlProviderName,
        "mysql-provider-v1");
    AssertProviderPackageBehavior(
        services => services.AddDVaultPostgres(),
        PostgresProviderName,
        "postgres-provider-v1");
    AssertProviderPackageBehavior(
        services => services.AddDVaultSqlServer(),
        SqlServerProviderName,
        "sqlserver-provider-v1");
    AssertProviderPackageBehavior(
        services => services.AddDVaultOracle(),
        OracleProviderName,
        "oracle-provider-v1");
  }

  private static void AssertProviderPackageBehavior(
      Action<IServiceCollection> configureServices,
      string providerName,
      string expectedProfileName) {
    var services = new ServiceCollection();
    configureServices(services);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var selector = provider.GetRequiredService<IDataVaultProviderBehaviorSelector>();
    var selectedProfile = selector.SelectBehavior(new DataVaultProviderBehaviorContext(providerName));
    var unmatchedProfile = selector.SelectBehavior(new DataVaultProviderBehaviorContext("Contoso.UnknownProvider"));

    Assert.NotEmpty(provider.GetServices<IDataVaultProviderBehavior>());
    Assert.Equal(expectedProfileName, selectedProfile.ProfileName);
    Assert.Same(DataVaultProviderBehaviorProfiles.ProviderNeutral, unmatchedProfile);
  }

  private sealed class ProviderNameBehavior(
      string profileName,
      string providerName,
      int priority) : IDataVaultProviderBehavior {
    private readonly DataVaultProviderBehaviorProfile _profile = new(profileName);

    public int CanApplyCallCount { get; private set; }

    public int CreateProfileCallCount { get; private set; }

    public int Priority { get; } = priority;

    public bool CanApply(DataVaultProviderBehaviorContext context) {
      ArgumentNullException.ThrowIfNull(context);

      CanApplyCallCount++;

      return string.Equals(context.ProviderName, providerName, StringComparison.Ordinal);
    }

    public DataVaultProviderBehaviorProfile CreateProfile(DataVaultProviderBehaviorContext context) {
      ArgumentNullException.ThrowIfNull(context);

      CreateProfileCallCount++;

      return _profile;
    }
  }
}
