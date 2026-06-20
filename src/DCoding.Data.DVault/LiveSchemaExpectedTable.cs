using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed record LiveSchemaExpectedTable(
    LiveSchemaTableIdentifier Identifier,
    string? PrimaryKeyName,
    IReadOnlyList<string> ColumnNames,
    IReadOnlyList<string> IndexNames);
