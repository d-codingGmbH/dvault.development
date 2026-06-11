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
}
