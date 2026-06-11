using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable diagnostics for PIT-backed as-of read shape.
/// </summary>
public sealed record DataVaultPitReadShapeDiagnostics(
    DataVaultReadShapeEntity Pit,
    DataVaultParentReferenceExplain ParentReference,
    IReadOnlyList<DataVaultPitReferencedSatelliteReadShapeDiagnostics> ReferencedSatellites,
    IReadOnlyList<DataVaultReadShapeColumnSet> FilterColumns,
    string PitRowSelectionRule,
    string SnapshotLookupBehavior,
    string NoLatestFallbackBehavior,
    string MaintainedPitPrerequisite,
    IReadOnlyList<DataVaultReadShapeIndexBaseline> ExpectedIndexBaseline) {
  /// <summary>
  /// Gets deterministic projected-column groups for the translated PIT read.
  /// </summary>
  public IReadOnlyList<DataVaultReadShapeColumnSet> ProjectedColumns { get; init; } =
      Array.Empty<DataVaultReadShapeColumnSet>();

  /// <summary>
  /// Gets the PIT row identity column groups used for row selection and result disambiguation.
  /// </summary>
  public IReadOnlyList<DataVaultReadShapeColumnSet> RowIdentityColumns { get; init; } =
      Array.Empty<DataVaultReadShapeColumnSet>();

  /// <summary>
  /// Gets the number of referenced satellite snapshot lookups required by the PIT read.
  /// </summary>
  public int ReferencedSatelliteLookupCount { get; init; }
}
