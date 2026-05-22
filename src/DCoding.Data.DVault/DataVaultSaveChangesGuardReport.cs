namespace DCoding.Data.DVault;

/// <summary>
/// Describes the deterministic findings produced by the optional Data Vault SaveChanges guard.
/// </summary>
public sealed class DataVaultSaveChangesGuardReport {
  /// <summary>
  /// Initializes a new instance of the DataVaultSaveChangesGuardReport class.
  /// </summary>
  /// <param name="findings">The guard findings included in the report.</param>
  public DataVaultSaveChangesGuardReport(IEnumerable<DataVaultSaveChangesGuardFinding> findings) {
    ArgumentNullException.ThrowIfNull(findings);

    Findings = findings.ToArray();
  }

  /// <summary>
  /// Gets the guard findings included in the report.
  /// </summary>
  public IReadOnlyList<DataVaultSaveChangesGuardFinding> Findings { get; }

  /// <summary>
  /// Gets a value indicating whether the report contains at least one finding.
  /// </summary>
  public bool HasFindings => Findings.Count > 0;

  /// <summary>
  /// Formats this report as a deterministic multi-line explanation.
  /// </summary>
  /// <returns>A deterministic explanation suitable for exceptions, warnings, tests, and diagnostics.</returns>
  public string ToDisplayString() {
    if (!HasFindings) {
      return "No Data Vault SaveChanges guard findings.";
    }

    return "Data Vault SaveChanges guard found " +
        Findings.Count +
        " unsafe generated row change(s):\n- " +
        string.Join("\n- ", Findings.Select(finding => finding.ToDisplayString()));
  }
}
