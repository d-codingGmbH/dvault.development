namespace DCoding.Data.DVault;

/// <summary>
/// Configures the optional Data Vault SaveChanges runtime guard interceptor.
/// </summary>
public sealed class DataVaultSaveChangesGuardOptions {
  private Action<DataVaultSaveChangesGuardReport>? _warningReporter;

  /// <summary>
  /// Gets the configured guard mode.
  /// </summary>
  public DataVaultSaveChangesGuardMode Mode { get; private set; } = DataVaultSaveChangesGuardMode.Blocking;

  /// <summary>
  /// Configures the guard to throw when unsafe generated-row changes are detected.
  /// </summary>
  /// <returns>The current options instance.</returns>
  public DataVaultSaveChangesGuardOptions UseBlockingMode() {
    Mode = DataVaultSaveChangesGuardMode.Blocking;
    _warningReporter = null;

    return this;
  }

  /// <summary>
  /// Configures the guard to report unsafe generated-row changes and allow SaveChanges to continue.
  /// </summary>
  /// <param name="reportWarning">The deterministic warning report callback invoked when guard findings exist.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultSaveChangesGuardOptions UseWarningMode(Action<DataVaultSaveChangesGuardReport> reportWarning) {
    ArgumentNullException.ThrowIfNull(reportWarning);

    Mode = DataVaultSaveChangesGuardMode.Warning;
    _warningReporter = reportWarning;

    return this;
  }

  internal void HandleReport(DataVaultSaveChangesGuardReport report) {
    if (!report.HasFindings) {
      return;
    }

    if (Mode == DataVaultSaveChangesGuardMode.Warning) {
      _warningReporter?.Invoke(report);
      return;
    }

    throw new DataVaultSaveChangesGuardException(report);
  }
}
