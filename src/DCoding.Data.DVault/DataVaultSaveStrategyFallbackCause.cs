using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable cause explaining provider-specific save-strategy fallback.
/// </summary>
public sealed record DataVaultSaveStrategyFallbackCause(
    DataVaultSaveStrategyFallbackCauseKind Kind,
    string Message);
