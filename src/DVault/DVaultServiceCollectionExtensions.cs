using DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;

namespace DVault;

/// <summary>
/// Provides startup registration extensions for DVault services and conventions.
/// </summary>
public static class DVaultServiceCollectionExtensions
{
    /// <summary>
    /// Adds the provider-neutral DVault defaults used by the optionless v1 startup path.
    /// </summary>
    /// <param name="services">The service collection used by the application startup pipeline.</param>
    /// <returns>The same service collection so startup configuration can continue fluently.</returns>
    public static IServiceCollection AddDVault(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        TryAddSingleton(services, typeof(DefaultNamingPolicy), DefaultNamingPolicy.Instance);
        TryAddSingleton(services, typeof(DataVaultConventions), DataVaultConventions.Default);

        return services;
    }

    private static void TryAddSingleton(IServiceCollection services, Type serviceType, object implementationInstance)
    {
        foreach (var descriptor in services)
        {
            if (descriptor.ServiceType == serviceType)
            {
                return;
            }
        }

        services.Add(ServiceDescriptor.Singleton(serviceType, implementationInstance));
    }
}