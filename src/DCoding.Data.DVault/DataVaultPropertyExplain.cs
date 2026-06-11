using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable explanation of one translated Data Vault property.
/// </summary>
public sealed record DataVaultPropertyExplain(
    string Name,
    DataVaultPropertyRole Role,
    TechnicalMetadataColumnRole? TechnicalRole,
    string MetadataName,
    int Ordinal,
    DataVaultLogicalPropertyKind LogicalPropertyKind,
    string ProviderProfileName,
    string StoreType,
    DataVaultProviderValueFormat ValueFormat) {
  /// <summary>
  /// Gets the EF model CLR type name for this translated property.
  /// </summary>
  public string ClrTypeName { get; init; } = string.Empty;

  /// <summary>
  /// Gets a value indicating whether EF marks this translated property nullable.
  /// </summary>
  public bool IsNullable { get; init; }

  /// <summary>
  /// Gets the provider-neutral produced name preserved on the EF metadata item.
  /// </summary>
  public string ProducedName { get; init; } = string.Empty;

  /// <summary>
  /// Gets the hash-key storage profile, when this property is a Data Vault hash key or hash-key reference.
  /// </summary>
  public DataVaultHashKeyStorageProfile? HashKeyStorageProfile { get; init; }

  /// <summary>
  /// Gets the stable-hash algorithm id that sizes this property, when this property is a hash key or hash-key reference.
  /// </summary>
  public string? StableHashAlgorithmId { get; init; }

  /// <summary>
  /// Gets the digest byte length that sizes this property, when this property is a hash key or hash-key reference.
  /// </summary>
  public int? DigestByteLength { get; init; }

  /// <summary>
  /// Gets the logical digest encoding exposed at the model boundary.
  /// </summary>
  public string? DigestEncoding { get; init; }

  /// <summary>
  /// Gets the EF conversion behavior used by this property.
  /// </summary>
  public string? ConversionBehavior { get; init; }
}
