using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Status of request-bound Data Vault read-strategy evaluation.
/// </summary>
public enum DataVaultReadStrategyDiagnosticsStatus {
  /// <summary>
  /// Strategy evaluation was not requested because no read request was supplied.
  /// </summary>
  NotEvaluated,

  /// <summary>
  /// A provider-specific read strategy accepted the supplied context and read request.
  /// </summary>
  ProviderStrategySelected,

  /// <summary>
  /// No provider-specific read strategy accepted the supplied context and read request.
  /// </summary>
  ProviderNeutralFallback,
}
