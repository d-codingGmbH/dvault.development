using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultSaveChangesMetadataInterceptorRegistrationTests {
  [Fact]
  public void AddDVaultDefaultPathDoesNotRegisterSaveChangesInterceptor() {
    var services = new ServiceCollection();

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.Empty(provider.GetServices<ISaveChangesInterceptor>());
  }

  [Fact]
  public void DbContextOptionsBuilderRegistersInterceptorOnlyOnExplicitOptIn() {
    var defaultOptions = new DbContextOptionsBuilder<RegistrationContext>().Options;

    Assert.Empty(GetInterceptors(defaultOptions));

    var optionsBuilder = new DbContextOptionsBuilder<RegistrationContext>();
    var result = optionsBuilder.UseDataVaultSaveChangesMetadataInterceptor(options => options
        .UseLoadTimestamp(new DateTimeOffset(2026, 5, 14, 12, 0, 0, TimeSpan.Zero))
        .UseRecordSource("registration-test"));

    var interceptor = Assert.Single(GetInterceptors(optionsBuilder.Options));

    Assert.Same(optionsBuilder, result);
    Assert.IsAssignableFrom<ISaveChangesInterceptor>(interceptor);
    Assert.Equal("DataVaultSaveChangesMetadataInterceptor", interceptor.GetType().Name);
  }

  [Fact]
  public void InterceptorOptionsValidateRequiredRecordSourceValue() {
    var options = new DataVaultSaveChangesMetadataInterceptorOptions();

    Assert.Throws<ArgumentException>(() => options.UseRecordSource(" "));
  }

  private static IReadOnlyList<IInterceptor> GetInterceptors(DbContextOptions options) {
    var coreOptions = ((IDbContextOptions)options).FindExtension<CoreOptionsExtension>();

    return coreOptions?.Interceptors?.ToArray() ?? [];
  }

  private sealed class RegistrationContext(DbContextOptions<RegistrationContext> options) : DbContext(options) {
  }
}
