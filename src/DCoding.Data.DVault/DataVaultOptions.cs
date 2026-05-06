using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

/// <summary>
/// Configures optional advanced DVault services while keeping the default startup path convention-first.
/// </summary>
public sealed class DataVaultOptions {
  private ServiceDescriptor? _loadTimestampResolverDescriptor;
  private ServiceDescriptor? _recordSourceResolverDescriptor;

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

  internal void Apply(IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    ReplaceDescriptor(services, _loadTimestampResolverDescriptor);
    ReplaceDescriptor(services, _recordSourceResolverDescriptor);
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
