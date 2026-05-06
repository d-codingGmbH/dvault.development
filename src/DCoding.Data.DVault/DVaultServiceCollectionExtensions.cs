using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for DVault services and conventions.
/// </summary>
public static class DVaultServiceCollectionExtensions {
  /// <summary>
  /// Adds the provider-neutral DVault defaults used by the optionless v1 startup path.
  /// This convention-first entry point requires no DVault options object; advanced configuration remains optional.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVault(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    TryAddSingleton(services, typeof(DefaultNamingPolicy), DefaultNamingPolicy.Instance);
    TryAddSingleton(services, typeof(DataVaultConventions), DataVaultConventions.Default);
    TryAddSingleton(services, typeof(IStableHashService), DefaultStableHashService.Instance);
    TryAddSingleton(services, typeof(IStableHashNormalizer), DefaultStableHashNormalizer.Instance);
    TryAddSingleton(services, typeof(IDataVaultLoadTimestampResolver), DefaultDataVaultLoadTimestampResolver.Instance);
    TryAddSingleton(services, typeof(IDataVaultRecordSourceResolver), DefaultDataVaultRecordSourceResolver.Instance);
    TryAddSingleton(services, typeof(IDataVaultSaveService), typeof(DefaultDataVaultSaveService));

    return services;
  }

  /// <summary>
  /// Adds the provider-neutral DVault defaults and applies optional advanced configuration.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <param name="configure">The optional advanced configuration callback.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVault(
      this IServiceCollection services,
      Action<DataVaultOptions> configure) {
    ArgumentNullException.ThrowIfNull(services);
    ArgumentNullException.ThrowIfNull(configure);

    var options = new DataVaultOptions();
    configure(options);
    options.Apply(services);

    return services.AddDVault();
  }

  private static void TryAddSingleton(IServiceCollection services, Type serviceType, object implementationInstance) {
    foreach (var descriptor in services) {
      if (descriptor.ServiceType == serviceType) {
        return;
      }
    }

    services.Add(ServiceDescriptor.Singleton(serviceType, implementationInstance));
  }

  private static void TryAddSingleton(IServiceCollection services, Type serviceType, Type implementationType) {
    foreach (var descriptor in services) {
      if (descriptor.ServiceType == serviceType) {
        return;
      }
    }

    services.Add(ServiceDescriptor.Singleton(serviceType, implementationType));
  }
}
