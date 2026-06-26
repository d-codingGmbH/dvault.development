using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable explanation of one translated Data Vault entity/table.
/// </summary>
public sealed record DataVaultEntityExplain(
    string TableName,
    DataVaultTableKind TableKind,
    string MetadataName,
    DataVaultParentReferenceExplain? ParentReference,
    IReadOnlyList<DataVaultPropertyExplain> Properties,
    DataVaultKeyExplain PrimaryKey,
    IReadOnlyList<DataVaultIndexExplain> Indexes,
    IReadOnlyList<DataVaultConstraintExplain> Constraints) {
  /// <summary>
  /// Gets the provider-neutral produced table name preserved on the EF metadata item.
  /// </summary>
  public string ProducedName { get; init; } = string.Empty;

  /// <summary>
  /// Gets ordered link participant facts when this explain entry describes a link entity.
  /// </summary>
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public IReadOnlyList<DataVaultLinkParticipantExplain>? LinkParticipants { get; init; }
}
