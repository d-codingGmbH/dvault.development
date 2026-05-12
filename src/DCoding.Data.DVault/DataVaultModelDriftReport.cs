using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault;

/// <summary>
/// Stable structured and human-readable model drift report for Data Vault EF metadata.
/// </summary>
public sealed record DataVaultModelDriftReport(IReadOnlyList<DataVaultModelDriftDifference> Differences) {
  /// <summary>
  /// Gets a value indicating whether the report contains at least one blocking difference.
  /// </summary>
  public bool HasBlockingDifferences => Differences.Any(
      difference => difference.Severity == DataVaultModelDriftSeverity.Blocking);

  /// <summary>
  /// Produces a deterministic human-readable rendering of the structured drift report.
  /// </summary>
  /// <returns>A concise report with stable difference ordering.</returns>
  public string ToDisplayString() {
    if (Differences.Count == 0) {
      return "DVault model drift: no differences.";
    }

    var blockingCount = Differences.Count(difference => difference.Severity == DataVaultModelDriftSeverity.Blocking);
    var builder = new StringBuilder();
    builder.Append("DVault model drift: ");
    builder.Append(Differences.Count.ToString(CultureInfo.InvariantCulture));
    builder.Append(" difference");
    if (Differences.Count != 1) {
      builder.Append('s');
    }

    builder.Append(", ");
    builder.Append(blockingCount.ToString(CultureInfo.InvariantCulture));
    builder.Append(" blocking.");

    foreach (var difference in Differences) {
      builder.AppendLine();
      builder.Append("- ");
      builder.Append(difference.Severity);
      builder.Append(' ');
      builder.Append(difference.Code);
      builder.Append(' ');
      builder.Append(difference.ElementKind);
      builder.Append(' ');
      builder.Append(difference.LogicalName);
      if (!string.IsNullOrWhiteSpace(difference.ProducedName)) {
        builder.Append(" (");
        builder.Append(difference.ProducedName);
        builder.Append(')');
      }

      builder.Append(" [");
      builder.Append(difference.PropertyPath);
      builder.Append("]: ");
      builder.Append(difference.Message);
      if (difference.ExpectedValue is not null || difference.ActualValue is not null) {
        builder.Append(" Expected=");
        builder.Append(difference.ExpectedValue ?? "<null>");
        builder.Append("; Actual=");
        builder.Append(difference.ActualValue ?? "<null>");
        builder.Append('.');
      }
    }

    return builder.ToString();
  }
}
