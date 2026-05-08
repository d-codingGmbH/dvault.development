using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for SQL Server-specific DVault services.
/// </summary>
public static class DVaultSqlServerServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults plus the SQL Server optimized save strategy.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultSqlServer(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    DataVaultProviderCapabilityProfileSelection.Register(
        SqlServerDataVaultSaveStrategy.SqlServerProviderName,
        DataVaultProviderCapabilityProfiles.SqlServer);
    services.AddDVault();
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBehavior, SqlServerDataVaultProviderBehavior>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderSaveStrategy, SqlServerDataVaultSaveStrategy>());

    return services;
  }
}
