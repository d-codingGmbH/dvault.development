using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable explanation of one provider capability type mapping.
/// </summary>
public sealed record DataVaultProviderTypeMappingExplain(
    DataVaultLogicalPropertyKind LogicalPropertyKind,
    string ModelClrTypeName,
    string StoreType,
    DataVaultProviderValueFormat ValueFormat) {
  /// <summary>
  /// Gets the hash-key storage profile, when this mapping covers a hash key or hash-key reference.
  /// </summary>
  public DataVaultHashKeyStorageProfile? HashKeyStorageProfile { get; init; }

  /// <summary>
  /// Gets the stable-hash algorithm id that sizes this mapping, when this mapping covers a hash key or hash-key reference.
  /// </summary>
  public string? StableHashAlgorithmId { get; init; }

  /// <summary>
  /// Gets the digest byte length that sizes this mapping, when this mapping covers a hash key or hash-key reference.
  /// </summary>
  public int? DigestByteLength { get; init; }

  /// <summary>
  /// Gets the logical digest encoding exposed at the model boundary.
  /// </summary>
  public string? DigestEncoding { get; init; }

  /// <summary>
  /// Gets the EF conversion behavior used by this mapping.
  /// </summary>
  public string? ConversionBehavior { get; init; }
}
