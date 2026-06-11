using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable explanation section of a Data Vault diagnostics result.
/// </summary>
public sealed record DataVaultExplainDiagnostics(
    string MetadataSourceKind,
    string? MetadataSourceFingerprint,
    string? ProviderName,
    string CapabilityProfileName,
    bool CapabilityProfileDefaulted,
    DataVaultProviderValueFormat LoadTimestampValueFormat,
    string LoadTimestampStoreType,
    string ProviderBehaviorProfileName,
    bool ProviderBehaviorDefaulted,
    IReadOnlyList<DataVaultEntityExplain> Entities) {
  /// <summary>
  /// Gets the value format used when PIT rows persist satellite snapshot load-timestamp references.
  /// </summary>
  public DataVaultProviderValueFormat SatelliteSnapshotReferenceValueFormat { get; init; } =
      DataVaultProviderValueFormat.Text;

  /// <summary>
  /// Gets the provider store type used when PIT rows persist satellite snapshot load-timestamp references.
  /// </summary>
  public string SatelliteSnapshotReferenceStoreType { get; init; } = string.Empty;

  /// <summary>
  /// Gets the deterministic provider type-mapping facts declared by the selected capability profile.
  /// </summary>
  public IReadOnlyList<DataVaultProviderTypeMappingExplain> TypeMappings { get; init; } =
      Array.Empty<DataVaultProviderTypeMappingExplain>();

  /// <summary>
  /// Gets the provider-specific maximum physical identifier length, if the selected capability profile declares one.
  /// </summary>
  public int? MaximumIdentifierLength { get; init; }

  /// <summary>
  /// Gets a value indicating whether the selected capability profile accepts secondary indexes covered by primary keys.
  /// </summary>
  public bool AllowsIndexesCoveredByPrimaryKey { get; init; } = true;

  /// <summary>
  /// Gets how the selected capability profile projects index include columns without native include-column support.
  /// </summary>
  public DataVaultUnsupportedIncludedIndexColumnMode UnsupportedIncludedIndexColumnMode { get; init; } =
      DataVaultUnsupportedIncludedIndexColumnMode.AppendToKey;

  /// <summary>
  /// Gets the SQL-function posture declared by the selected capability profile.
  /// </summary>
  public DataVaultProviderSqlFunctionSupport SqlFunctionSupport { get; init; } =
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported;

  /// <summary>
  /// Gets the concurrency posture declared by the selected capability profile.
  /// </summary>
  public DataVaultProviderConcurrencySupport ConcurrencySupport { get; init; } =
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported;

  /// <summary>
  /// Gets the active stable-hash algorithm metadata used for Data Vault hash-key compatibility decisions.
  /// </summary>
  public DataVaultStableHashExplain StableHash { get; init; } =
      new("sha256-v1", 32, "lowercase-hex-no-prefix");
}
