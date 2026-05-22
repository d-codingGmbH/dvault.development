namespace DCoding.Data.DVault;

/// <summary>
/// Represents a blocking Data Vault SaveChanges guard failure.
/// </summary>
public sealed class DataVaultSaveChangesGuardException : InvalidOperationException {
  /// <summary>
  /// Initializes a new instance of the DataVaultSaveChangesGuardException class.
  /// </summary>
  /// <param name="report">The guard report that caused SaveChanges to be blocked.</param>
  public DataVaultSaveChangesGuardException(DataVaultSaveChangesGuardReport report)
      : base(CreateMessage(report)) {
    Report = report;
  }

  /// <summary>
  /// Gets the guard report that caused SaveChanges to be blocked.
  /// </summary>
  public DataVaultSaveChangesGuardReport Report { get; }

  private static string CreateMessage(DataVaultSaveChangesGuardReport report) {
    ArgumentNullException.ThrowIfNull(report);

    return "Data Vault SaveChanges guard blocked unsafe generated-row changes.\n" +
        report.ToDisplayString();
  }
}
