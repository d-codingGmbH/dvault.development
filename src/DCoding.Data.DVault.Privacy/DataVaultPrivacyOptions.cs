using DCoding.Data.DVault;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Configures the optional privacy extension proof without enabling automatic privacy behavior.
/// </summary>
public sealed class DataVaultPrivacyOptions {
  private readonly List<string> encryptedPayloadAliases = [];
  private IDataVaultPrivacyKeyProvider? keyProvider;

  /// <summary>
  /// Gets the provider-neutral encrypted-payload aliases registered for explicit privacy flows.
  /// </summary>
  public IReadOnlyList<string> EncryptedPayloadAliases => encryptedPayloadAliases;

  /// <summary>
  /// Gets the caller-owned key provider registered for explicit privacy flows.
  /// </summary>
  public IDataVaultPrivacyKeyProvider? KeyProvider => keyProvider;

  /// <summary>
  /// Registers an encrypted-payload alias from model personal-data metadata for explicit privacy flows.
  /// </summary>
  /// <param name="encryptedPayloadAlias">The stable provider-neutral encrypted-payload alias.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultPrivacyOptions RegisterEncryptedPayloadAlias(string encryptedPayloadAlias) {
    if (string.IsNullOrWhiteSpace(encryptedPayloadAlias)) {
      throw new ArgumentException("Encrypted payload alias must be non-empty.", nameof(encryptedPayloadAlias));
    }

    if (encryptedPayloadAliases.Contains(encryptedPayloadAlias, StringComparer.Ordinal)) {
      throw new InvalidOperationException("Encrypted payload alias '" + encryptedPayloadAlias + "' has already been registered.");
    }

    encryptedPayloadAliases.Add(encryptedPayloadAlias);
    return this;
  }

  /// <summary>
  /// Registers a caller-owned key provider without giving DVault ownership of key material or key lifecycle.
  /// </summary>
  /// <param name="provider">The caller-owned key-provider marker.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultPrivacyOptions UseCallerOwnedKeyProvider(IDataVaultPrivacyKeyProvider provider) {
    ArgumentNullException.ThrowIfNull(provider);

    keyProvider = provider;
    return this;
  }

  internal void Apply(IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    ReplaceDescriptor(
        services,
        ServiceDescriptor.Singleton<IDataVaultPrivacyConfiguration>(
            new DefaultDataVaultPrivacyConfiguration([.. encryptedPayloadAliases], keyProvider)));
    ReplaceDescriptor(
        services,
        ServiceDescriptor.Singleton<IDataVaultPersonalDataCoverageProof, DataVaultPrivacyPersonalDataCoverageProof>());
    ReplaceDescriptor(
        services,
        ServiceDescriptor.Singleton<IDataVaultPrivacyAliasCoverageProvider, DataVaultPrivacyAliasCoverageProvider>());

    if (keyProvider is not null) {
      ReplaceDescriptor(services, ServiceDescriptor.Singleton(keyProvider));
      if (keyProvider is IDataVaultEncryptedPayloadKeyProvider encryptedPayloadKeyProvider) {
        ReplaceDescriptor(services, ServiceDescriptor.Singleton(encryptedPayloadKeyProvider));
      }
    }
  }

  private static void ReplaceDescriptor(IServiceCollection services, ServiceDescriptor descriptor) {
    for (var index = services.Count - 1; index >= 0; index--) {
      if (services[index].ServiceType == descriptor.ServiceType) {
        services.RemoveAt(index);
      }
    }

    services.Add(descriptor);
  }
}
