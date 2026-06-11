using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Request-bound provider tuning diagnostics for save dispatch.
/// </summary>
public sealed record DataVaultSaveProviderTuningDiagnostics(
    DataVaultProviderTuningRecommendation? Recommendation = null,
    IReadOnlyList<DataVaultProviderThresholdFact>? ThresholdFacts = null);
