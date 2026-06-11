using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable diagnostics for request-bound provider-specific read-strategy dispatch.
/// </summary>
public sealed record DataVaultReadStrategyDiagnostics(
    DataVaultReadStrategyDiagnosticsStatus Status,
    string? ProviderName,
    string? SelectedStrategyName,
    int? SelectedStrategyPriority,
    IReadOnlyList<DataVaultReadStrategyCandidateDiagnostics> Candidates,
    IReadOnlyList<DataVaultReadStrategyFallbackCause> FallbackCauses);
