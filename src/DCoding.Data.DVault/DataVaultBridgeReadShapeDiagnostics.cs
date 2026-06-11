using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable diagnostics for bridge read shape.
/// </summary>
public sealed record DataVaultBridgeReadShapeDiagnostics(
    DataVaultBridgeKind BridgeKind,
    DataVaultReadShapeEntity Bridge,
    IReadOnlyList<DataVaultBridgeEndpointReadShapeDiagnostics> Endpoints,
    DataVaultBridgeTraversalEndpoint FilterEndpoint,
    DataVaultReadShapeColumnSet EndpointFilter,
    DataVaultReadShapeColumnSet? DepthPredicate,
    IReadOnlyList<DataVaultReadShapeColumnSet> DeterministicOrdering,
    IReadOnlyList<string> SupportedEndpointRules,
    IReadOnlyList<DataVaultReadShapeIndexBaseline> ExpectedTraversalIndexBaseline) {
  /// <summary>
  /// Gets deterministic projected-column groups for the translated bridge read.
  /// </summary>
  public IReadOnlyList<DataVaultReadShapeColumnSet> ProjectedColumns { get; init; } =
      Array.Empty<DataVaultReadShapeColumnSet>();
}
