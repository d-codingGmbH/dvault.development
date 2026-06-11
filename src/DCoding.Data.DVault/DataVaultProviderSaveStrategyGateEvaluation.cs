using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

internal sealed record DataVaultProviderSaveStrategyGateEvaluation(
    bool CanSave,
    IReadOnlyList<DataVaultSaveStrategyFallbackCause> FallbackCauses);
