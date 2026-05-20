using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides service registration helpers for opt-in DVault telemetry counters.
/// </summary>
public static class DataVaultTelemetryServiceCollectionExtensions {
  /// <summary>
  /// Adds the built-in meter-backed DVault telemetry observer.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultTelemetry(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    foreach (var descriptor in services) {
      if (descriptor.ServiceType == typeof(IDataVaultTelemetryObserver) &&
          descriptor.ImplementationType == typeof(DataVaultMeterTelemetryObserver)) {
        return services;
      }
    }

    services.AddSingleton<IDataVaultTelemetryObserver, DataVaultMeterTelemetryObserver>();
    return services;
  }
}
