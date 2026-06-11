using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Stable structured Data Vault diagnostics payload.
/// </summary>
public sealed record DataVaultDiagnosticsResult(
    DataVaultValidationDiagnostics Validation,
    DataVaultExplainDiagnostics Explain,
    DataVaultSaveStrategyDiagnostics SaveStrategy,
    IReadOnlyList<DataVaultDiagnosticsIssue> Issues) {
  /// <summary>
  /// Gets request-bound provider-specific read-strategy diagnostics.
  /// </summary>
  public DataVaultReadStrategyDiagnostics ReadStrategy { get; init; } = new(
      DataVaultReadStrategyDiagnosticsStatus.NotEvaluated,
      ProviderName: null,
      SelectedStrategyName: null,
      SelectedStrategyPriority: null,
      Candidates: Array.Empty<DataVaultReadStrategyCandidateDiagnostics>(),
      FallbackCauses: Array.Empty<DataVaultReadStrategyFallbackCause>());

  /// <summary>
  /// Gets request-bound read/query-shape diagnostics for supported Data Vault read requests.
  /// </summary>
  public DataVaultReadShapeDiagnostics? ReadShape { get; init; }

  /// <summary>
  /// Gets request-bound provider tuning diagnostics derived from save/read strategy and read-shape facts.
  /// </summary>
  public DataVaultProviderTuningDiagnostics? ProviderTuning { get; init; }

  /// <summary>
  /// Produces a concise human-readable rendering of the structured diagnostics payload.
  /// </summary>
  public string ToDisplayString() {
    var builder = new StringBuilder();
    builder.Append("DVault diagnostics: ");
    builder.Append(Validation.IsValid ? "valid" : "invalid");
    builder.Append(", capability ");
    builder.Append(Explain.CapabilityProfileName);
    if (Explain.CapabilityProfileDefaulted) {
      builder.Append(" (defaulted)");
    }

    builder.Append(", provider ");
    builder.Append(string.IsNullOrWhiteSpace(Explain.ProviderName) ? "<none>" : Explain.ProviderName);
    builder.Append(", load timestamp ");
    builder.Append(Explain.LoadTimestampValueFormat);
    builder.Append('/');
    builder.Append(Explain.LoadTimestampStoreType);
    builder.Append(", snapshot reference ");
    builder.Append(Explain.SatelliteSnapshotReferenceValueFormat);
    builder.Append('/');
    builder.Append(Explain.SatelliteSnapshotReferenceStoreType);
    builder.Append(", identifier max ");
    builder.Append(Explain.MaximumIdentifierLength.HasValue
        ? Explain.MaximumIdentifierLength.Value.ToString(CultureInfo.InvariantCulture)
        : "<provider-default>");
    builder.Append(", included indexes ");
    builder.Append(Explain.UnsupportedIncludedIndexColumnMode);
    builder.Append(", SQL functions ");
    builder.Append(Explain.SqlFunctionSupport);
    builder.Append(", concurrency ");
    builder.Append(Explain.ConcurrencySupport);
    builder.Append(", provider behavior ");
    builder.Append(Explain.ProviderBehaviorProfileName);
    if (Explain.ProviderBehaviorDefaulted) {
      builder.Append(" (defaulted)");
    }

    builder.Append(", stable hash ");
    builder.Append(Explain.StableHash.AlgorithmId);
    builder.Append('/');
    builder.Append(Explain.StableHash.DigestByteLength.ToString(CultureInfo.InvariantCulture));
    builder.Append(" bytes/");
    builder.Append(Explain.StableHash.DigestEncoding);
    builder.Append(", entities ");
    builder.Append(Explain.Entities.Count.ToString(CultureInfo.InvariantCulture));
    builder.Append(", save strategy ");
    builder.Append(SaveStrategy.Status.ToString());
    if (!string.IsNullOrWhiteSpace(SaveStrategy.SelectedStrategyName)) {
      builder.Append(" (");
      builder.Append(SaveStrategy.SelectedStrategyName);
      builder.Append(')');
    }

    AppendSaveStrategyDisplayDetails(builder, SaveStrategy);
    builder.Append(", read strategy ");
    builder.Append(ReadStrategy.Status.ToString());
    if (!string.IsNullOrWhiteSpace(ReadStrategy.SelectedStrategyName)) {
      builder.Append(" (");
      builder.Append(ReadStrategy.SelectedStrategyName);
      builder.Append(')');
    }

    AppendReadStrategyDisplayDetails(builder, ReadStrategy);
    if (ReadShape is not null) {
      builder.Append(", read shape ");
      builder.Append(ReadShape.Kind);
      AppendReadShapeDisplayDetails(builder, ReadShape);
    }

    AppendProviderTuningDisplayDetails(builder, ProviderTuning);

    if (Issues.Count > 0) {
      builder.AppendLine();
      foreach (var issue in Issues) {
        builder.Append("- ");
        builder.Append(issue.Severity);
        builder.Append(' ');
        builder.Append(issue.Code);
        builder.Append(": ");
        builder.Append(issue.Message);
        builder.AppendLine();
      }
    }

    return builder.ToString().TrimEnd();
  }

  private static void AppendSaveStrategyDisplayDetails(
      StringBuilder builder,
      DataVaultSaveStrategyDiagnostics strategy) {
    if (strategy.SelectedStrategyPriority.HasValue) {
      builder.Append(", save priority ");
      builder.Append(strategy.SelectedStrategyPriority.Value.ToString(CultureInfo.InvariantCulture));
    }

    builder.Append(", save candidates ");
    builder.Append(strategy.Candidates.Count.ToString(CultureInfo.InvariantCulture));
    if (strategy.FallbackCauses.Count > 0) {
      builder.Append(", save fallback causes ");
      builder.Append(string.Join(", ", strategy.FallbackCauses.Select(cause => cause.Kind.ToString())));
    }

    if (strategy.StagedProviderBulk is not null) {
      builder.Append(", staged provider bulk ");
      builder.Append(strategy.StagedProviderBulk.LifecyclePhase);
      builder.Append(", staged provider caveat ");
      builder.Append(strategy.StagedProviderBulk.ProviderCaveatKind);
      builder.Append(", staged operations ");
      builder.Append(strategy.StagedProviderBulk.OperationCount.ToString(CultureInfo.InvariantCulture));
    }
  }

  private static void AppendReadStrategyDisplayDetails(
      StringBuilder builder,
      DataVaultReadStrategyDiagnostics strategy) {
    if (strategy.SelectedStrategyPriority.HasValue) {
      builder.Append(", read priority ");
      builder.Append(strategy.SelectedStrategyPriority.Value.ToString(CultureInfo.InvariantCulture));
    }

    builder.Append(", read candidates ");
    builder.Append(strategy.Candidates.Count.ToString(CultureInfo.InvariantCulture));
    if (strategy.FallbackCauses.Count > 0) {
      builder.Append(", read fallback causes ");
      builder.Append(string.Join(", ", strategy.FallbackCauses.Select(cause => cause.Kind.ToString())));
    }
  }

  private static void AppendReadShapeDisplayDetails(
      StringBuilder builder,
      DataVaultReadShapeDiagnostics readShape) {
    switch (readShape.Kind) {
      case DataVaultReadShapeKind.LatestSatellite when readShape.Satellite is not null:
        builder.Append(" (");
        builder.Append(readShape.Satellite.Satellite.TableName);
        builder.Append(", ");
        builder.Append(readShape.Satellite.Semantics);
        builder.Append(')');
        return;

      case DataVaultReadShapeKind.PitAsOf when readShape.Pit is not null:
        builder.Append(" (");
        builder.Append(readShape.Pit.Pit.TableName);
        builder.Append(')');
        return;

      case DataVaultReadShapeKind.Bridge when readShape.Bridge is not null:
        builder.Append(" (");
        builder.Append(readShape.Bridge.Bridge.TableName);
        builder.Append(", ");
        builder.Append(readShape.Bridge.FilterEndpoint);
        builder.Append(')');
        return;
    }
  }

  private static void AppendProviderTuningDisplayDetails(
      StringBuilder builder,
      DataVaultProviderTuningDiagnostics? providerTuning) {
    if (providerTuning?.Save?.Recommendation is not null) {
      builder.Append(", save recommendation ");
      builder.Append(providerTuning.Save.Recommendation.Category);
    }

    if (providerTuning?.Read?.Recommendation is not null) {
      builder.Append(", read recommendation ");
      builder.Append(providerTuning.Read.Recommendation.Category);
    }
  }
}
