using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Severity assigned to one Data Vault diagnostics issue.
/// </summary>
public enum DataVaultDiagnosticsIssueSeverity {
  /// <summary>
  /// Informational diagnostic.
  /// </summary>
  Info,

  /// <summary>
  /// Risky but non-blocking diagnostic.
  /// </summary>
  Warning,

  /// <summary>
  /// Blocking validation diagnostic.
  /// </summary>
  Error,
}
