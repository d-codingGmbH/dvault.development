using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Request-bound provider tuning diagnostics derived from save/read strategy diagnostics.
/// </summary>
public sealed record DataVaultProviderTuningDiagnostics(
    DataVaultSaveProviderTuningDiagnostics? Save = null,
    DataVaultReadProviderTuningDiagnostics? Read = null);
