namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Declares an optional exact CLR type association for one Data Vault metadata declaration.
/// </summary>
public sealed class DataVaultMetadataClrMapping {
  private DataVaultMetadataClrMapping(
      DataVaultMetadataRegistryKind kind,
      string name,
      DataVaultMetadataReference? parent,
      Type clrType) {
    ArgumentNullException.ThrowIfNull(clrType);

    Kind = kind;
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    Parent = parent;
    ClrType = clrType;
  }

  /// <summary>
  /// Gets the metadata kind addressed by the mapping.
  /// </summary>
  public DataVaultMetadataRegistryKind Kind { get; }

  /// <summary>
  /// Gets the logical metadata name addressed by the mapping.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the parent metadata reference for parent-scoped metadata, when required.
  /// </summary>
  public DataVaultMetadataReference? Parent { get; }

  /// <summary>
  /// Gets the exact CLR type associated with the metadata declaration.
  /// </summary>
  public Type ClrType { get; }

  /// <summary>
  /// Creates a hub CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping Hub(string name, Type clrType) {
    return new DataVaultMetadataClrMapping(DataVaultMetadataRegistryKind.Hub, name, parent: null, clrType);
  }

  /// <summary>
  /// Creates a hub CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping Hub<T>(string name) {
    return Hub(name, typeof(T));
  }

  /// <summary>
  /// Creates a link CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping Link(string name, Type clrType) {
    return new DataVaultMetadataClrMapping(DataVaultMetadataRegistryKind.Link, name, parent: null, clrType);
  }

  /// <summary>
  /// Creates a link CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping Link<T>(string name) {
    return Link(name, typeof(T));
  }

  /// <summary>
  /// Creates a satellite CLR mapping scoped by its hub or link parent.
  /// </summary>
  public static DataVaultMetadataClrMapping Satellite(
      DataVaultMetadataReference parent,
      string name,
      Type clrType) {
    return new DataVaultMetadataClrMapping(
        DataVaultMetadataRegistryKind.Satellite,
        name,
        RequireSatelliteParent(parent, nameof(parent)),
        clrType);
  }

  /// <summary>
  /// Creates a satellite CLR mapping scoped by its hub or link parent.
  /// </summary>
  public static DataVaultMetadataClrMapping Satellite<T>(
      DataVaultMetadataReference parent,
      string name) {
    return Satellite(parent, name, typeof(T));
  }

  /// <summary>
  /// Creates a legacy point-in-time table CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping PointInTimeTable(string name, Type clrType) {
    return new DataVaultMetadataClrMapping(DataVaultMetadataRegistryKind.PointInTimeTable, name, parent: null, clrType);
  }

  /// <summary>
  /// Creates a legacy point-in-time table CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping PointInTimeTable<T>(string name) {
    return PointInTimeTable(name, typeof(T));
  }

  /// <summary>
  /// Creates a bridge CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping Bridge(string name, Type clrType) {
    return new DataVaultMetadataClrMapping(DataVaultMetadataRegistryKind.Bridge, name, parent: null, clrType);
  }

  /// <summary>
  /// Creates a bridge CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping Bridge<T>(string name) {
    return Bridge(name, typeof(T));
  }

  /// <summary>
  /// Creates a PIT CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping Pit(string name, Type clrType) {
    return new DataVaultMetadataClrMapping(DataVaultMetadataRegistryKind.Pit, name, parent: null, clrType);
  }

  /// <summary>
  /// Creates a PIT CLR mapping.
  /// </summary>
  public static DataVaultMetadataClrMapping Pit<T>(string name) {
    return Pit(name, typeof(T));
  }

  private static DataVaultMetadataReference RequireSatelliteParent(
      DataVaultMetadataReference parent,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(parent, parameterName);

    if (parent.Kind is not DataVaultMetadataReferenceKind.Hub and not DataVaultMetadataReferenceKind.Link) {
      throw new ArgumentException("A satellite CLR mapping parent must reference a hub or link.", parameterName);
    }

    return parent;
  }
}
