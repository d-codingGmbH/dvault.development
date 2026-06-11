using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable issue emitted by Data Vault diagnostics.
/// </summary>
public sealed record DataVaultDiagnosticsIssue(
    DataVaultDiagnosticsIssueSeverity Severity,
    string Code,
    string Message,
    string? Path = null);
