using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for MySQL-specific DVault services.
/// </summary>
public static class DVaultMySqlServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults for MySQL. Provider-specific optimized writers can extend this package without changing callers.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultMySql(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    services.AddDVault();

    return services;
  }
}
