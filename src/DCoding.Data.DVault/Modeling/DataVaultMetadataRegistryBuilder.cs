using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Builds an immutable metadata registry from a metadata model plus optional provider profiles and CLR mappings.
/// </summary>
public sealed class DataVaultMetadataRegistryBuilder {
  private readonly DataVaultMetadataModel _metadataModel;
  private readonly List<DataVaultProviderCapabilityProfile> _providerCapabilityProfiles = [];
  private readonly List<DataVaultMetadataClrMapping> _clrMappings = [];

  /// <summary>
  /// Initializes a new registry builder for an existing metadata model.
  /// </summary>
  public DataVaultMetadataRegistryBuilder(DataVaultMetadataModel metadataModel) {
    ArgumentNullException.ThrowIfNull(metadataModel);

    _metadataModel = metadataModel;
  }

  /// <summary>
  /// Adds provider capability profile metadata to the registry.
  /// </summary>
  public DataVaultMetadataRegistryBuilder AddProviderCapabilityProfile(
      DataVaultProviderCapabilityProfile providerCapabilityProfile) {
    ArgumentNullException.ThrowIfNull(providerCapabilityProfile);

    _providerCapabilityProfiles.Add(providerCapabilityProfile);
    return this;
  }

  /// <summary>
  /// Adds provider capability profile metadata to the registry.
  /// </summary>
  public DataVaultMetadataRegistryBuilder AddProviderCapabilityProfiles(
      IEnumerable<DataVaultProviderCapabilityProfile> providerCapabilityProfiles) {
    ArgumentNullException.ThrowIfNull(providerCapabilityProfiles);

    foreach (var providerCapabilityProfile in providerCapabilityProfiles) {
      AddProviderCapabilityProfile(providerCapabilityProfile);
    }

    return this;
  }

  /// <summary>
  /// Adds an optional exact CLR type mapping to the registry.
  /// </summary>
  public DataVaultMetadataRegistryBuilder AddClrMapping(DataVaultMetadataClrMapping clrMapping) {
    ArgumentNullException.ThrowIfNull(clrMapping);

    _clrMappings.Add(clrMapping);
    return this;
  }

  /// <summary>
  /// Adds optional exact CLR type mappings to the registry.
  /// </summary>
  public DataVaultMetadataRegistryBuilder AddClrMappings(IEnumerable<DataVaultMetadataClrMapping> clrMappings) {
    ArgumentNullException.ThrowIfNull(clrMappings);

    foreach (var clrMapping in clrMappings) {
      AddClrMapping(clrMapping);
    }

    return this;
  }

  /// <summary>
  /// Builds an immutable metadata registry.
  /// </summary>
  public DataVaultMetadataRegistry Build() {
    return DataVaultMetadataRegistry.Create(
        _metadataModel,
        _providerCapabilityProfiles,
        _clrMappings);
  }
}
