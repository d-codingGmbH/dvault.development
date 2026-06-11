using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable expected translated index or key baseline for a read shape.
/// </summary>
public sealed record DataVaultReadShapeIndexBaseline(
    string Name,
    string Kind,
    IReadOnlyList<string> ColumnNames,
    bool IsUnique,
    IReadOnlyList<string> DescendingColumnNames,
    IReadOnlyList<string> IncludedColumnNames);
