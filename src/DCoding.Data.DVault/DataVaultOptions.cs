using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault;

/// <summary>
/// Configures optional advanced DVault services while keeping the default startup path convention-first.
/// </summary>
public sealed class DataVaultOptions {
  private ServiceDescriptor? _loadTimestampResolverDescriptor;
  private ServiceDescriptor? _recordSourceResolverDescriptor;
  private ServiceDescriptor? _metadataRegistryDescriptor;
  private ServiceDescriptor? _stableHashServiceDescriptor;
  private ServiceDescriptor? _conventionsDescriptor;
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
  /// Selects one of DVault's built-in stable hash algorithms for model and key hashing.
  /// </summary>
  /// <param name="algorithmId">The exact built-in algorithm id to register.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseStableHashAlgorithm(string algorithmId) {
    var stableHashService = BuiltInStableHashService.Create(algorithmId);

    _stableHashServiceDescriptor = ServiceDescriptor.Singleton(stableHashService);
    _conventionsDescriptor = ServiceDescriptor.Singleton(
        DataVaultConventions.CreateWithStableHashAlgorithm(
            stableHashService.AlgorithmId,
            stableHashService.ComputeHash(string.Empty).DigestByteLength));
    return this;
  }

  /// <summary>
  /// Configures the app-level default Data Vault metadata model used by opted-in DbContext instances.
  /// </summary>
  /// <param name="metadataModel">The provider-neutral metadata model to convert once into the app-level registry.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseMetadataModel(DataVaultMetadataModel metadataModel) {
    ArgumentNullException.ThrowIfNull(metadataModel);

    return UseMetadataRegistry(DataVaultMetadataRegistry.Create(metadataModel));
  }

  /// <summary>
  /// Configures a successful model-first import result as the app-level default Data Vault metadata registry.
  /// </summary>
  /// <param name="importResult">The successful model-first import result to expose as the app-level default source.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseMetadataModel(DataVaultModelImportResult importResult) {
    ArgumentNullException.ThrowIfNull(importResult);

    return UseMetadataRegistry(importResult.RequireMetadataRegistry());
  }

  /// <summary>
  /// Configures the app-level default Data Vault metadata registry used by opted-in DbContext instances.
  /// </summary>
  /// <param name="metadataRegistry">The immutable metadata registry to expose as the app-level default source.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultOptions UseMetadataRegistry(DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    _metadataRegistryDescriptor = ServiceDescriptor.Singleton(metadataRegistry);
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
    ReplaceDescriptor(services, _metadataRegistryDescriptor);
    ReplaceDescriptor(services, _stableHashServiceDescriptor);
    ReplaceDescriptor(services, _conventionsDescriptor);
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
