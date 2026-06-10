using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for DB2-specific DVault services.
/// </summary>
public static class DVaultDb2ServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults plus explicit DB2 provider capability and behavior registration.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultDb2(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    DataVaultProviderCapabilityProfileSelection.Register(
        Db2DataVaultProviderBehavior.ProviderName,
        DataVaultProviderCapabilityProfiles.Db2);
    services.AddDVault();
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBehavior, Db2DataVaultProviderBehavior>());

    return services;
  }
}
