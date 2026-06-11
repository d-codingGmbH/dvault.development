using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Bounded performance-profile recommendation derived from request-bound provider diagnostics.
/// </summary>
public sealed record DataVaultProviderTuningRecommendation(
    DataVaultPerformanceProfileCategory Category,
    string ProfileName,
    string Message);
