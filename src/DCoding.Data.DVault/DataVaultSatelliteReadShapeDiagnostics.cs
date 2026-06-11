using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable diagnostics for latest/current/as-of satellite read shape.
/// </summary>
public sealed record DataVaultSatelliteReadShapeDiagnostics(
    DataVaultSatelliteReadSemantics Semantics,
    DataVaultReadShapeEntity Satellite,
    DataVaultParentReferenceExplain ParentReference,
    IReadOnlyList<DataVaultReadShapeColumnSet> FilterColumns,
    string SeriesSelectionRule,
    string CutoffRule,
    IReadOnlyList<DataVaultReadShapeColumnSet> DeterministicOrdering,
    IReadOnlyList<DataVaultReadShapeIndexBaseline> ExpectedIndexBaseline) {
  /// <summary>
  /// Gets deterministic projected-column groups for the translated satellite read.
  /// </summary>
  public IReadOnlyList<DataVaultReadShapeColumnSet> ProjectedColumns { get; init; } =
      Array.Empty<DataVaultReadShapeColumnSet>();
}
