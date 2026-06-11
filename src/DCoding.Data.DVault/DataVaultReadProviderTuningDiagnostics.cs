using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Request-bound provider tuning diagnostics for read dispatch.
/// </summary>
public sealed record DataVaultReadProviderTuningDiagnostics(
    DataVaultProviderTuningRecommendation? Recommendation = null);
