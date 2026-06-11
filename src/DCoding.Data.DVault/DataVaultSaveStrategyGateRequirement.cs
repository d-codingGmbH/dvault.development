using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable declared gate for provider-specific save-strategy eligibility.
/// </summary>
public sealed record DataVaultSaveStrategyGateRequirement(
    DataVaultSaveStrategyFallbackCauseKind Kind,
    int? MinimumTotalOperationCount = null,
    int? MaximumSatelliteOperationCount = null);
