using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault;

/// <summary>
/// Structured aggregate of caller-owned representative request diagnostics supplied to Data Vault preflight.
/// </summary>
public sealed class DataVaultPreflightRequestDiagnosticsReport {
  /// <summary>
  /// Initializes a new request diagnostics report.
  /// </summary>
  /// <param name="results">The ordered representative diagnostics results to preserve.</param>
  public DataVaultPreflightRequestDiagnosticsReport(
      IEnumerable<DataVaultPreflightRepresentativeDiagnostics> results) {
    ArgumentNullException.ThrowIfNull(results);

    Results = results.Select(result => {
      ArgumentNullException.ThrowIfNull(result);
      return result;
    }).ToArray();
  }

  /// <summary>
  /// Gets the ordered representative diagnostics results preserved by this report.
  /// </summary>
  public IReadOnlyList<DataVaultPreflightRepresentativeDiagnostics> Results { get; }

  /// <summary>
  /// Gets the number of representative diagnostics results that contain blocking validation issues.
  /// </summary>
  public int BlockingResultCount => Results.Count(result => result.IsBlocked);

  /// <summary>
  /// Gets a value indicating whether any representative diagnostics result contains blocking validation issues.
  /// </summary>
  public bool HasBlockingDiagnostics => BlockingResultCount > 0;

  /// <summary>
  /// Produces deterministic human-readable request diagnostics output for console, test, or build logs.
  /// </summary>
  /// <returns>A concise deterministic display string with all supplied representative diagnostics.</returns>
  public string ToDisplayString() {
    var builder = new StringBuilder();
    builder.Append("DVault request-bound diagnostics: ");
    builder.Append(HasBlockingDiagnostics ? "blocked" : "passed");
    builder.Append(", results ");
    builder.Append(Results.Count.ToString(CultureInfo.InvariantCulture));
    builder.Append(", blocking ");
    builder.Append(BlockingResultCount.ToString(CultureInfo.InvariantCulture));
    builder.Append('.');

    foreach (var result in Results) {
      builder.AppendLine();
      builder.Append("- ");
      builder.Append(result.IsBlocked ? "blocked " : "passed ");
      builder.Append(result.Name);
      builder.AppendLine(":");
      builder.Append(result.Diagnostics.ToDisplayString());
    }

    return builder.ToString();
  }
}
