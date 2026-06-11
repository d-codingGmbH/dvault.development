using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Represents a named hub, link, or satellite metadata target.
/// </summary>
public sealed class DataVaultMetadataReference {
  private DataVaultMetadataReference(DataVaultMetadataReferenceKind kind, string name) {
    Kind = kind;
    Name = name;
  }

  /// <summary>
  /// Gets the kind of metadata declaration being referenced.
  /// </summary>
  public DataVaultMetadataReferenceKind Kind { get; }

  /// <summary>
  /// Gets the referenced hub, link, or satellite name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Creates a reference to hub metadata.
  /// </summary>
  public static DataVaultMetadataReference Hub(string name) {
    return new DataVaultMetadataReference(
        DataVaultMetadataReferenceKind.Hub,
        DataVaultMetadataValidation.RequireName(name, nameof(name)));
  }

  /// <summary>
  /// Creates a reference to link metadata.
  /// </summary>
  public static DataVaultMetadataReference Link(string name) {
    return new DataVaultMetadataReference(
        DataVaultMetadataReferenceKind.Link,
        DataVaultMetadataValidation.RequireName(name, nameof(name)));
  }

  /// <summary>
  /// Creates a reference to satellite metadata.
  /// </summary>
  public static DataVaultMetadataReference Satellite(string name) {
    return new DataVaultMetadataReference(
        DataVaultMetadataReferenceKind.Satellite,
        DataVaultMetadataValidation.RequireName(name, nameof(name)));
  }
}
