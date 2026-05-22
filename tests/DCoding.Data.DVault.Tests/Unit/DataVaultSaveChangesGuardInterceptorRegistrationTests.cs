using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultSaveChangesGuardInterceptorRegistrationTests {
  [Fact]
  public void AddDVaultDefaultPathDoesNotRegisterGuardInterceptor() {
    var services = new ServiceCollection();

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.Empty(provider.GetServices<ISaveChangesInterceptor>());
  }

  [Fact]
  public void DbContextOptionsBuilderRegistersGuardOnlyOnExplicitOptIn() {
    var defaultOptions = new DbContextOptionsBuilder<RegistrationContext>().Options;

    Assert.Empty(GetInterceptors(defaultOptions));

    var optionsBuilder = new DbContextOptionsBuilder<RegistrationContext>();
    var result = optionsBuilder.UseDataVaultSaveChangesGuardInterceptor(options => options.UseBlockingMode());

    var interceptor = Assert.Single(GetInterceptors(optionsBuilder.Options));

    Assert.Same(optionsBuilder, result);
    Assert.IsAssignableFrom<ISaveChangesInterceptor>(interceptor);
    Assert.Equal("DataVaultSaveChangesGuardInterceptor", interceptor.GetType().Name);
  }

  [Fact]
  public void GuardOptionsConfigureWarningAndBlockingModes() {
    var reports = new List<DataVaultSaveChangesGuardReport>();
    var options = new DataVaultSaveChangesGuardOptions();

    Assert.Equal(DataVaultSaveChangesGuardMode.Blocking, options.Mode);

    Assert.Same(options, options.UseWarningMode(reports.Add));
    Assert.Equal(DataVaultSaveChangesGuardMode.Warning, options.Mode);

    Assert.Same(options, options.UseBlockingMode());
    Assert.Equal(DataVaultSaveChangesGuardMode.Blocking, options.Mode);
  }

  [Fact]
  public void WarningModeRequiresCallerFacingReportCallback() {
    var options = new DataVaultSaveChangesGuardOptions();

    Assert.Throws<ArgumentNullException>(() => options.UseWarningMode(null!));
  }

  [Fact]
  public void GuardReportCreatesDeterministicExplanation() {
    var report = new DataVaultSaveChangesGuardReport([
        new DataVaultSaveChangesGuardFinding(
            "HubCustomer",
            DataVaultTableKind.Hub,
            "Customer",
            EntityState.Modified,
            ["Tracked state 'Modified' is unsafe for generated Data Vault Hub rows."]),
    ]);

    Assert.True(report.HasFindings);
    Assert.Equal(
        "Data Vault SaveChanges guard found 1 unsafe generated row change(s):\n" +
        "- Hub 'Customer' mapped to 'HubCustomer' in Modified state: " +
        "Tracked state 'Modified' is unsafe for generated Data Vault Hub rows.",
        report.ToDisplayString());
  }

  [Fact]
  public void BlockingExceptionExposesStructuredReport() {
    var report = new DataVaultSaveChangesGuardReport([
        new DataVaultSaveChangesGuardFinding(
            "LinkCustomerOrder",
            DataVaultTableKind.Link,
            "CustomerOrder",
            EntityState.Deleted,
            ["Tracked state 'Deleted' is unsafe for generated Data Vault Link rows."]),
    ]);

    var exception = new DataVaultSaveChangesGuardException(report);

    Assert.Same(report, exception.Report);
    Assert.Contains("Data Vault SaveChanges guard blocked unsafe generated-row changes.", exception.Message, StringComparison.Ordinal);
    Assert.Contains("Link 'CustomerOrder' mapped to 'LinkCustomerOrder' in Deleted state", exception.Message, StringComparison.Ordinal);
  }

  private static IReadOnlyList<IInterceptor> GetInterceptors(DbContextOptions options) {
    var coreOptions = ((IDbContextOptions)options).FindExtension<CoreOptionsExtension>();

    return coreOptions?.Interceptors?.ToArray() ?? [];
  }

  private sealed class RegistrationContext(DbContextOptions<RegistrationContext> options) : DbContext(options) {
  }
}
