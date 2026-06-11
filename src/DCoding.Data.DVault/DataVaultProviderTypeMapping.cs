using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one provider-specific native storage mapping for a Data Vault logical property kind.
/// </summary>
public sealed record DataVaultProviderTypeMapping {
  /// <summary>
  /// Initializes a new provider type mapping.
  /// </summary>
  /// <param name="logicalPropertyKind">The Data Vault logical property kind covered by the mapping.</param>
  /// <param name="modelClrType">The CLR type projected into the Entity Framework model.</param>
  /// <param name="nativeStoreType">The native provider storage type declared by the profile.</param>
  /// <param name="valueFormat">The value format declared for persisted values.</param>
  public DataVaultProviderTypeMapping(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      Type modelClrType,
      string nativeStoreType,
      DataVaultProviderValueFormat valueFormat) {
    ArgumentNullException.ThrowIfNull(modelClrType);
    ArgumentException.ThrowIfNullOrWhiteSpace(nativeStoreType);

    LogicalPropertyKind = logicalPropertyKind;
    ModelClrType = modelClrType;
    NativeStoreType = nativeStoreType;
    ValueFormat = valueFormat;
  }

  /// <summary>
  /// Gets the Data Vault logical property kind covered by the mapping.
  /// </summary>
  public DataVaultLogicalPropertyKind LogicalPropertyKind { get; private init; }

  /// <summary>
  /// Gets the CLR type projected into the Entity Framework model.
  /// </summary>
  public Type ModelClrType { get; private init; }

  /// <summary>
  /// Gets the native provider storage type declared by the profile.
  /// </summary>
  public string NativeStoreType { get; private init; }

  /// <summary>
  /// Gets the value format declared for persisted values.
  /// </summary>
  public DataVaultProviderValueFormat ValueFormat { get; private init; }
}
