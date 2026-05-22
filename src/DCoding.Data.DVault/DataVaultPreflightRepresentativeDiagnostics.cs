namespace DCoding.Data.DVault;

/// <summary>
/// Carries one caller-owned representative request diagnostics result inside an aggregate preflight report.
/// </summary>
public sealed class DataVaultPreflightRepresentativeDiagnostics {
  /// <summary>
  /// Initializes a new representative diagnostics result.
  /// </summary>
  /// <param name="name">The stable caller-owned name for the representative request or diagnostics payload.</param>
  /// <param name="diagnostics">The existing Data Vault diagnostics result to preserve in the aggregate report.</param>
  public DataVaultPreflightRepresentativeDiagnostics(
      string name,
      DataVaultDiagnosticsResult diagnostics) {
    ArgumentException.ThrowIfNullOrWhiteSpace(name);
    ArgumentNullException.ThrowIfNull(diagnostics);

    Name = name;
    Diagnostics = diagnostics;
  }

  /// <summary>
  /// Gets the stable caller-owned name for the representative request or diagnostics payload.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the existing Data Vault diagnostics result preserved in the aggregate report.
  /// </summary>
  public DataVaultDiagnosticsResult Diagnostics { get; }

  /// <summary>
  /// Gets a value indicating whether the diagnostics result contains blocking validation issues.
  /// </summary>
  public bool IsBlocked => !Diagnostics.Validation.IsValid;
}
