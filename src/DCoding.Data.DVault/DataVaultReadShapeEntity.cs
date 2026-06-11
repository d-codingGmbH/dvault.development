using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable translated table identity for one read-shape diagnostics target.
/// </summary>
public sealed record DataVaultReadShapeEntity(
    string MetadataName,
    DataVaultTableKind TableKind,
    string TableName);
