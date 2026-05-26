using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for MySQL-specific DVault services.
/// </summary>
public static class DVaultMySqlServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults plus the MySQL optimized save strategy for supported EF Core MySQL providers.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultMySql(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    DataVaultProviderCapabilityProfileSelection.Register(
        MySqlDataVaultSaveStrategy.PomeloProviderName,
        DataVaultProviderCapabilityProfiles.MySql);
    DataVaultProviderCapabilityProfileSelection.Register(
        MySqlDataVaultSaveStrategy.OracleProviderName,
        DataVaultProviderCapabilityProfiles.MySql);
    services.AddDVault();
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBehavior, MySqlDataVaultProviderBehavior>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderSaveStrategy, MySqlStagedDataVaultSaveStrategy>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderSaveStrategy, MySqlDataVaultSaveStrategy>());

    return services;
  }
}
