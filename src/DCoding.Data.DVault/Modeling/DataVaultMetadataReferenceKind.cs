using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies the Data Vault metadata structures that can be referenced by another metadata declaration.
/// </summary>
public enum DataVaultMetadataReferenceKind {
  /// <summary>
  /// References a hub metadata declaration.
  /// </summary>
  Hub,

  /// <summary>
  /// References a link metadata declaration.
  /// </summary>
  Link,

  /// <summary>
  /// References a satellite metadata declaration.
  /// </summary>
  Satellite,
}
