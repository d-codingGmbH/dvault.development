using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable PIT satellite reference facts used by PIT read-shape diagnostics.
/// </summary>
public sealed record DataVaultPitReferencedSatelliteReadShapeDiagnostics(
    string MetadataName,
    string TableName,
    string SnapshotReferenceColumnName,
    string ParentHashKeyColumnName,
    string LoadTimestampColumnName,
    IReadOnlyList<string> DrivingKeyColumnNames);
