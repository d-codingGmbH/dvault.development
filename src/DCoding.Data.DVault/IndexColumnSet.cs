using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed record IndexColumnSet(
    IReadOnlyList<string> ColumnNames,
    IReadOnlyList<string> DescendingColumnNames,
    IReadOnlyList<string> IncludedColumnNames);
