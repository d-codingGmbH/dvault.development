using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault;

/// <summary>
/// Structured Data Vault model drift preflight result across metadata, runtime model, and snapshot-model evidence.
/// </summary>
public sealed record DataVaultModelDriftPreflightReport {
  /// <summary>
  /// Initializes a new model drift preflight report.
  /// </summary>
  /// <param name="metadataVersusRuntime">The metadata authority compared with the configured DbContext runtime model.</param>
  /// <param name="metadataVersusSnapshotModel">The metadata authority compared with the explicit consumer-materialized snapshot model.</param>
  /// <param name="runtimeVersusSnapshotModel">The configured DbContext runtime model compared with the explicit consumer-materialized snapshot model.</param>
  public DataVaultModelDriftPreflightReport(
      DataVaultModelDriftReport metadataVersusRuntime,
      DataVaultModelDriftReport metadataVersusSnapshotModel,
      DataVaultModelDriftReport runtimeVersusSnapshotModel) {
    ArgumentNullException.ThrowIfNull(metadataVersusRuntime);
    ArgumentNullException.ThrowIfNull(metadataVersusSnapshotModel);
    ArgumentNullException.ThrowIfNull(runtimeVersusSnapshotModel);

    MetadataVersusRuntime = metadataVersusRuntime;
    MetadataVersusSnapshotModel = metadataVersusSnapshotModel;
    RuntimeVersusSnapshotModel = runtimeVersusSnapshotModel;
  }

  /// <summary>
  /// Gets the metadata authority compared with the configured DbContext runtime model.
  /// </summary>
  public DataVaultModelDriftReport MetadataVersusRuntime { get; }

  /// <summary>
  /// Gets the metadata authority compared with the explicit consumer-materialized snapshot model.
  /// </summary>
  public DataVaultModelDriftReport MetadataVersusSnapshotModel { get; }

  /// <summary>
  /// Gets the configured DbContext runtime model compared with the explicit consumer-materialized snapshot model.
  /// </summary>
  public DataVaultModelDriftReport RuntimeVersusSnapshotModel { get; }

  /// <summary>
  /// Gets the total number of differences across all preflight comparison sections.
  /// </summary>
  public int DifferenceCount =>
      MetadataVersusRuntime.Differences.Count +
      MetadataVersusSnapshotModel.Differences.Count +
      RuntimeVersusSnapshotModel.Differences.Count;

  /// <summary>
  /// Gets the total number of blocking differences across all preflight comparison sections.
  /// </summary>
  public int BlockingDifferenceCount =>
      CountBlocking(MetadataVersusRuntime) +
      CountBlocking(MetadataVersusSnapshotModel) +
      CountBlocking(RuntimeVersusSnapshotModel);

  /// <summary>
  /// Gets a value indicating whether any preflight comparison section contains a blocking difference.
  /// </summary>
  public bool HasBlockingDifferences => BlockingDifferenceCount > 0;

  /// <summary>
  /// Produces deterministic human-readable model drift preflight output for console, test, or build logs.
  /// </summary>
  /// <returns>A concise deterministic display string with all comparison sections.</returns>
  public string ToDisplayString() {
    var builder = new StringBuilder();
    builder.Append("DVault model drift preflight: ");
    builder.Append(HasBlockingDifferences ? "blocked" : "passed");
    builder.Append(", ");
    builder.Append(DifferenceCount.ToString(CultureInfo.InvariantCulture));
    builder.Append(" difference");
    if (DifferenceCount != 1) {
      builder.Append('s');
    }

    builder.Append(", ");
    builder.Append(BlockingDifferenceCount.ToString(CultureInfo.InvariantCulture));
    builder.Append(" blocking.");

    AppendSection(builder, "metadata-versus-runtime", MetadataVersusRuntime);
    AppendSection(builder, "metadata-versus-snapshot-model", MetadataVersusSnapshotModel);
    AppendSection(builder, "runtime-versus-snapshot-model", RuntimeVersusSnapshotModel);

    return builder.ToString();
  }

  private static int CountBlocking(DataVaultModelDriftReport report) {
    return report.Differences.Count(difference => difference.Severity == DataVaultModelDriftSeverity.Blocking);
  }

  private static void AppendSection(
      StringBuilder builder,
      string sectionName,
      DataVaultModelDriftReport report) {
    builder.AppendLine();
    builder.Append(sectionName);
    builder.AppendLine(":");
    builder.Append(report.ToDisplayString());
  }
}
