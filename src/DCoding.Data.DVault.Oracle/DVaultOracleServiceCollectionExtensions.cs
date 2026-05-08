using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for Oracle-specific DVault services.
/// </summary>
public static class DVaultOracleServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults plus the Oracle optimized save-strategy boundary.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultOracle(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    DataVaultProviderCapabilityProfileSelection.Register(
        OracleDataVaultSaveStrategy.OracleProviderName,
        DataVaultProviderCapabilityProfiles.Oracle);
    services.AddDVault();
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBehavior, OracleDataVaultProviderBehavior>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderSaveStrategy, OracleDataVaultSaveStrategy>());

    return services;
  }
}
