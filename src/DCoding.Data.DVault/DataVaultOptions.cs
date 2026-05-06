using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

/// <summary>
/// Configures optional advanced DVault services while keeping the default startup path convention-first.
/// </summary>
public sealed class DataVaultOptions {
  private ServiceDescriptor? _loadTimestampResolverDescriptor;
  private ServiceDescriptor? _recordSourceResolverDescriptor;
  private readonly List<ServiceDescriptor> _providerBehaviorDescriptors = [];

  /// <summary>
  /// Configures the load timestamp resolver instance used by the explicit save service.
  /// </summary>
  /// <param name="resolver">The resolver instance to register.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseLoadTimestampResolver(IDataVaultLoadTimestampResolver resolver) {
    ArgumentNullException.ThrowIfNull(resolver);

    _loadTimestampResolverDescriptor = ServiceDescriptor.Singleton<IDataVaultLoadTimestampResolver>(resolver);
    return this;
  }

  /// <summary>
  /// Configures the load timestamp resolver implementation used by the explicit save service.
  /// </summary>
  /// <typeparam name="TResolver">The resolver implementation type.</typeparam>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseLoadTimestampResolver<TResolver>()
      where TResolver : class, IDataVaultLoadTimestampResolver {
    _loadTimestampResolverDescriptor = ServiceDescriptor.Singleton<IDataVaultLoadTimestampResolver, TResolver>();
    return this;
  }

  /// <summary>
  /// Configures the record-source resolver instance used by the explicit save service.
  /// </summary>
  /// <param name="resolver">The resolver instance to register.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseRecordSourceResolver(IDataVaultRecordSourceResolver resolver) {
    ArgumentNullException.ThrowIfNull(resolver);

    _recordSourceResolverDescriptor = ServiceDescriptor.Singleton<IDataVaultRecordSourceResolver>(resolver);
    return this;
  }

  /// <summary>
  /// Configures the record-source resolver implementation used by the explicit save service.
  /// </summary>
  /// <typeparam name="TResolver">The resolver implementation type.</typeparam>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseRecordSourceResolver<TResolver>()
      where TResolver : class, IDataVaultRecordSourceResolver {
    _recordSourceResolverDescriptor = ServiceDescriptor.Singleton<IDataVaultRecordSourceResolver, TResolver>();
    return this;
  }

  /// <summary>
  /// Adds an explicit provider-behavior override while preserving the provider-neutral baseline when it does not apply.
  /// </summary>
  /// <param name="providerBehavior">The provider-behavior override instance to register.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseProviderBehavior(IDataVaultProviderBehavior providerBehavior) {
    ArgumentNullException.ThrowIfNull(providerBehavior);

    _providerBehaviorDescriptors.Add(ServiceDescriptor.Singleton<IDataVaultProviderBehavior>(providerBehavior));
    return this;
  }

  /// <summary>
  /// Adds an explicit provider-behavior override implementation while preserving the provider-neutral baseline when it does not apply.
  /// </summary>
  /// <typeparam name="TProviderBehavior">The provider-behavior override implementation type.</typeparam>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseProviderBehavior<TProviderBehavior>()
      where TProviderBehavior : class, IDataVaultProviderBehavior {
    _providerBehaviorDescriptors.Add(ServiceDescriptor.Singleton<IDataVaultProviderBehavior, TProviderBehavior>());
    return this;
  }

  internal void Apply(IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    ReplaceDescriptor(services, _loadTimestampResolverDescriptor);
    ReplaceDescriptor(services, _recordSourceResolverDescriptor);
    foreach (var providerBehaviorDescriptor in _providerBehaviorDescriptors) {
      services.Add(providerBehaviorDescriptor);
    }
  }

  private static void ReplaceDescriptor(IServiceCollection services, ServiceDescriptor? descriptor) {
    if (descriptor is null) {
      return;
    }

    for (var index = services.Count - 1; index >= 0; index--) {
      if (services[index].ServiceType == descriptor.ServiceType) {
        services.RemoveAt(index);
      }
    }

    services.Add(descriptor);
  }
}
