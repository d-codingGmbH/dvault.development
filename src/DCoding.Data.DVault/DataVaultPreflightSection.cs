namespace DCoding.Data.DVault;

/// <summary>
/// Carries one named Data Vault preflight section and the underlying report object that produced its status.
/// </summary>
/// <typeparam name="TReport">The structured report type preserved by this preflight section.</typeparam>
public sealed class DataVaultPreflightSection<TReport> where TReport : class {
  /// <summary>
  /// Initializes a new preflight section.
  /// </summary>
  /// <param name="name">The stable machine-readable section name.</param>
  /// <param name="status">The deterministic section status.</param>
  /// <param name="report">The underlying structured report object when the section was evaluated.</param>
  /// <param name="skipReason">The deterministic reason the section was skipped.</param>
  public DataVaultPreflightSection(
      string name,
      DataVaultPreflightSectionStatus status,
      TReport? report,
      string? skipReason = null) {
    ArgumentException.ThrowIfNullOrWhiteSpace(name);

    if (status == DataVaultPreflightSectionStatus.Skipped) {
      ArgumentException.ThrowIfNullOrWhiteSpace(skipReason);
      if (report is not null) {
        throw new ArgumentException("Skipped preflight sections cannot carry a report.", nameof(report));
      }
    }
    else {
      ArgumentNullException.ThrowIfNull(report);
      if (!string.IsNullOrWhiteSpace(skipReason)) {
        throw new ArgumentException("Evaluated preflight sections cannot carry a skip reason.", nameof(skipReason));
      }
    }

    Name = name;
    Status = status;
    Report = report;
    SkipReason = skipReason;
  }

  /// <summary>
  /// Gets the stable machine-readable section name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the deterministic section status.
  /// </summary>
  public DataVaultPreflightSectionStatus Status { get; }

  /// <summary>
  /// Gets the underlying structured report object when the section was evaluated.
  /// </summary>
  public TReport? Report { get; }

  /// <summary>
  /// Gets the deterministic reason the section was skipped, or null when the section was evaluated.
  /// </summary>
  public string? SkipReason { get; }

  /// <summary>
  /// Gets a value indicating whether this section reported a blocking condition.
  /// </summary>
  public bool IsBlocked => Status == DataVaultPreflightSectionStatus.Blocked;

  /// <summary>
  /// Gets a value indicating whether this section was not evaluated.
  /// </summary>
  public bool IsSkipped => Status == DataVaultPreflightSectionStatus.Skipped;
}
