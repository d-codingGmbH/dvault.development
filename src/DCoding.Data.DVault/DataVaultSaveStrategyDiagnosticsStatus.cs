using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Status of request-bound Data Vault save-strategy evaluation.
/// </summary>
public enum DataVaultSaveStrategyDiagnosticsStatus {
  /// <summary>
  /// Strategy evaluation was not requested because no explicit save request batch was supplied.
  /// </summary>
  NotEvaluated,

  /// <summary>
  /// A provider-specific save strategy accepted the supplied context and ordered request batch.
  /// </summary>
  ProviderStrategySelected,

  /// <summary>
  /// No provider-specific save strategy accepted the supplied context and ordered request batch.
  /// </summary>
  ProviderNeutralFallback,
}
