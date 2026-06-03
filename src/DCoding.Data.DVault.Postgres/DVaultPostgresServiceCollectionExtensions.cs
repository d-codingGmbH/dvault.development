using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for PostgreSQL-specific DVault services.
/// </summary>
public static class DVaultPostgresServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults plus the PostgreSQL optimized save and PIT/bridge read strategies.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultPostgres(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    DataVaultProviderCapabilityProfileSelection.Register(
        PostgresDataVaultSaveStrategy.NpgsqlProviderName,
        DataVaultProviderCapabilityProfiles.Postgres);
    services.AddDVault();
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBehavior, PostgresDataVaultProviderBehavior>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderSaveStrategy, PostgresDataVaultSaveStrategy>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderPitReadStrategy, PostgresDataVaultReadStrategy>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBridgeReadStrategy, PostgresDataVaultReadStrategy>());

    return services;
  }
}
