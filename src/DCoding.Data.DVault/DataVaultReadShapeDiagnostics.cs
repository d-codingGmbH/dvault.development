using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable request-bound Data Vault read/query-shape diagnostics.
/// </summary>
public sealed record DataVaultReadShapeDiagnostics(
    DataVaultReadShapeKind Kind,
    DataVaultReadShapeProviderDiagnostics Provider,
    DataVaultSatelliteReadShapeDiagnostics? Satellite = null,
    DataVaultPitReadShapeDiagnostics? Pit = null,
    DataVaultBridgeReadShapeDiagnostics? Bridge = null);
