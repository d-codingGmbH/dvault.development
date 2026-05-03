using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for Oracle-specific DVault services.
/// </summary>
public static class DVaultOracleServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults for Oracle. Provider-specific optimized writers can extend this package without changing callers.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultOracle(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    services.AddDVault();

    return services;
  }
}
