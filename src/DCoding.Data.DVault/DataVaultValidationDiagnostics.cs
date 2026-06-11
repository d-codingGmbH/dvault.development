using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Validation section of a Data Vault diagnostics result.
/// </summary>
public sealed record DataVaultValidationDiagnostics(
    bool IsValid,
    IReadOnlyList<DataVaultDiagnosticsIssue> Issues);
