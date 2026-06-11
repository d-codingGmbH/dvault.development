using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for SQLite-specific DVault services.
/// </summary>
public static class DVaultSqliteServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults plus the SQLite optimized save and read strategies.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultSqlite(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    DataVaultProviderCapabilityProfileSelection.Register(
        SqliteDataVaultSaveStrategy.ProviderName,
        DataVaultProviderCapabilityProfiles.Sqlite);
    services.AddDVault();
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBehavior, SqliteDataVaultProviderBehavior>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderSaveStrategy, SqliteDataVaultSaveStrategy>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderReadStrategy, SqliteDataVaultReadStrategy>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderPitReadStrategy, SqliteDataVaultReadStrategy>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBridgeReadStrategy, SqliteDataVaultReadStrategy>());

    return services;
  }
}
