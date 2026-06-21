using System.Globalization;
using System.Text;
using System.Text.Json;

namespace DCoding.Data.DVault.Benchmarks;

internal static class AllocationHotspotArtifacts {
  public const string MarkdownFileName = "allocation-hotspots.md";
  public const string CsvFileName = "allocation-hotspots.csv";
  public const string JsonFileName = "allocation-hotspots.json";
  public const string TicketId = "06FE4R1XJVQZTQ8S9WN2YE3ZKW";

  private static readonly JsonSerializerOptions SerializerOptions = new() {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
  };

  public static async Task<AllocationHotspotArtifactPaths> WriteAsync(
      string outputDirectory,
      BenchmarkRunContext context,
      IReadOnlyList<AllocationHotspotWorkloadSummary> workloads,
      IReadOnlyList<AllocationHotspotReportRow> rankedHotspots,
      CancellationToken cancellationToken) {
    ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
    ArgumentNullException.ThrowIfNull(context);
    ArgumentNullException.ThrowIfNull(workloads);
    ArgumentNullException.ThrowIfNull(rankedHotspots);

    var fullOutputDirectory = Path.GetFullPath(outputDirectory);
    Directory.CreateDirectory(fullOutputDirectory);

    var markdownPath = Path.Combine(fullOutputDirectory, MarkdownFileName);
    var csvPath = Path.Combine(fullOutputDirectory, CsvFileName);
    var jsonPath = Path.Combine(fullOutputDirectory, JsonFileName);
    var optimizationOrder = CreateOptimizationOrder(rankedHotspots);

    await File.WriteAllTextAsync(
            markdownPath,
            CreateMarkdown(context, workloads, rankedHotspots, optimizationOrder),
            cancellationToken)
        .ConfigureAwait(false);
    await File.WriteAllTextAsync(csvPath, CreateCsv(rankedHotspots), cancellationToken)
        .ConfigureAwait(false);
    await File.WriteAllTextAsync(
            jsonPath,
            CreateJson(context, workloads, rankedHotspots, optimizationOrder),
            cancellationToken)
        .ConfigureAwait(false);

    return new AllocationHotspotArtifactPaths(markdownPath, csvPath, jsonPath);
  }

  public static IReadOnlyList<AllocationHotspotReportRow> CreateRankedHotspots(
      IReadOnlyList<AllocationHotspotWorkloadResult> results) {
    ArgumentNullException.ThrowIfNull(results);

    var rows = results
        .SelectMany(result => result.Samples, (result, sample) => new {
          result.Workload,
          Sample = sample,
        })
        .Where(value => !string.Equals(value.Sample.Surface, "database write boundary", StringComparison.Ordinal))
        .GroupBy(
            value => new {
              value.Sample.Surface,
              value.Sample.StepName,
              value.Sample.WorkloadName,
            })
        .Select(group => {
          var workload = group.First().Workload;
          var iterationCount = workload.IterationCount;
          var allocatedByIteration = Enumerable.Range(0, iterationCount)
              .Select(iteration => group
                  .Where(value => value.Sample.Iteration == iteration)
                  .Sum(value => value.Sample.AllocatedBytes))
              .ToArray();
          var millisecondsByIteration = Enumerable.Range(0, iterationCount)
              .Select(iteration => group
                  .Where(value => value.Sample.Iteration == iteration)
                  .Sum(value => value.Sample.Elapsed.TotalMilliseconds))
              .ToArray();
          var callCountByIteration = Enumerable.Range(0, iterationCount)
              .Select(iteration => group.Count(value => value.Sample.Iteration == iteration))
              .ToArray();

          return new AllocationHotspotReportRow(
              Rank: 0,
              group.Key.Surface,
              group.Key.StepName,
              group.Key.WorkloadName,
              workload.DatasetSize,
              workload.ChangeRatio,
              iterationCount,
              allocatedByIteration.Average(),
              allocatedByIteration.Min(),
              allocatedByIteration.Max(),
              millisecondsByIteration.Average(),
              callCountByIteration.Average(),
              "measured",
              Recommend(group.Key.Surface, group.Key.StepName));
        })
        .OrderByDescending(row => row.MeanAllocatedBytes)
        .ThenBy(row => row.Surface, StringComparer.Ordinal)
        .ThenBy(row => row.StepName, StringComparer.Ordinal)
        .ThenBy(row => row.WorkloadName, StringComparer.Ordinal)
        .ToArray();

    return rows
        .Select((row, index) => row with { Rank = index + 1 })
        .ToArray();
  }

  private static string CreateMarkdown(
      BenchmarkRunContext context,
      IReadOnlyList<AllocationHotspotWorkloadSummary> workloads,
      IReadOnlyList<AllocationHotspotReportRow> rankedHotspots,
      IReadOnlyList<string> optimizationOrder) {
    var builder = new StringBuilder();

    builder.AppendLine("# DVault Allocation Hotspot Report");
    builder.AppendLine();
    builder.AppendLine("## Context");
    builder.AppendLine();
    builder.AppendLine("- Ticket: `" + TicketId + "`");
    builder.AppendLine("- Evidence posture: measured DVault-owned allocation hotspots; database write boundary samples are excluded from the ranked table.");
    builder.AppendLine("- Required provider: " + context.Provider);
    builder.AppendLine("- Provider filter: " + context.ProviderFilter);
    builder.AppendLine("- Iterations: " + context.Iterations.ToString(CultureInfo.InvariantCulture));
    builder.AppendLine("- Warmup iterations: " + context.WarmupIterations.ToString(CultureInfo.InvariantCulture));
    builder.AppendLine("- Load timestamp storage: " + context.LoadTimestampStorage);
    builder.AppendLine("- Hash key variants: " + string.Join(", ", context.HashKeyVariants.Select(variant => variant.Label)));
    builder.AppendLine("- Stable hash baseline: `sha256-v1` with `HexString` hash-key storage.");
    builder.AppendLine("- Runtime: " + context.DotNetRuntimeDescription + " " + context.DotNetRuntimeVersion + " on " + context.OsDescription + ".");
    builder.AppendLine();
    builder.AppendLine("## Workload Shapes");
    builder.AppendLine();
    builder.AppendLine("| Workload | Dataset size | Change ratio | Persisted outcome |");
    builder.AppendLine("| --- | --- | --- | --- |");
    foreach (var workload in workloads) {
      builder
          .Append("| ")
          .Append(EscapeMarkdownCell(workload.WorkloadName))
          .Append(" | ")
          .Append(EscapeMarkdownCell(workload.DatasetSize))
          .Append(" | ")
          .Append(EscapeMarkdownCell(workload.ChangeRatio))
          .Append(" | ")
          .Append(EscapeMarkdownCell(workload.PersistedOutcome))
          .AppendLine(" |");
    }

    builder.AppendLine();
    builder.AppendLine("## Ranked Hotspots");
    builder.AppendLine();
    builder.AppendLine("| Rank | Surface | Step | Workload | Mean allocated bytes | Mean ms | Mean calls | Recommendation |");
    builder.AppendLine("| ---: | --- | --- | --- | ---: | ---: | ---: | --- |");
    foreach (var row in rankedHotspots) {
      builder
          .Append("| ")
          .Append(row.Rank.ToString(CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.Surface))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.StepName))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.WorkloadName))
          .Append(" | ")
          .Append(row.MeanAllocatedBytes.ToString("F0", CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(row.MeanMilliseconds.ToString("F3", CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(row.MeanCallCount.ToString("F1", CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.Recommendation))
          .AppendLine(" |");
    }

    builder.AppendLine();
    builder.AppendLine("## Recommended Optimization Order");
    builder.AppendLine();
    foreach (var item in optimizationOrder) {
      builder.AppendLine("- " + item);
    }

    builder.AppendLine();
    builder.AppendLine("## Measurement Boundary");
    builder.AppendLine();
    builder.AppendLine("- Caller-owned satellite `HashDiff` generation is outside the measured operation; replay requests are created before the profiled save call.");
    builder.AppendLine("- SQLite database setup, seeding, verification, and cleanup run outside the profiled save action.");
    builder.AppendLine("- `DbContext.SaveChangesAsync` is sampled as context but excluded from the ranked DVault-owned hotspot table.");

    return builder.ToString();
  }

  private static string CreateCsv(IEnumerable<AllocationHotspotReportRow> rankedHotspots) {
    var builder = new StringBuilder();
    builder.AppendLine("rank,surface,stepName,workloadName,datasetSize,changeRatio,iterations,meanAllocatedBytes,minAllocatedBytes,maxAllocatedBytes,meanMilliseconds,meanCallCount,evidencePosture,recommendation");

    foreach (var row in rankedHotspots) {
      AppendCsvRow(
          builder,
          row.Rank.ToString(CultureInfo.InvariantCulture),
          row.Surface,
          row.StepName,
          row.WorkloadName,
          row.DatasetSize,
          row.ChangeRatio,
          row.Iterations.ToString(CultureInfo.InvariantCulture),
          row.MeanAllocatedBytes.ToString("F0", CultureInfo.InvariantCulture),
          row.MinAllocatedBytes.ToString(CultureInfo.InvariantCulture),
          row.MaxAllocatedBytes.ToString(CultureInfo.InvariantCulture),
          row.MeanMilliseconds.ToString("F3", CultureInfo.InvariantCulture),
          row.MeanCallCount.ToString("F1", CultureInfo.InvariantCulture),
          row.EvidencePosture,
          row.Recommendation);
    }

    return builder.ToString();
  }

  private static string CreateJson(
      BenchmarkRunContext context,
      IReadOnlyList<AllocationHotspotWorkloadSummary> workloads,
      IReadOnlyList<AllocationHotspotReportRow> rankedHotspots,
      IReadOnlyList<string> optimizationOrder) {
    return JsonSerializer.Serialize(
        new AllocationHotspotReportDocument(
            "dvault.allocation-hotspots.v1",
            TicketId,
            context,
            workloads,
            rankedHotspots,
            optimizationOrder),
        SerializerOptions) + Environment.NewLine;
  }

  private static IReadOnlyList<string> CreateOptimizationOrder(IReadOnlyList<AllocationHotspotReportRow> rankedHotspots) {
    return rankedHotspots
        .GroupBy(row => row.Surface, StringComparer.Ordinal)
        .OrderByDescending(group => group.Max(row => row.MeanAllocatedBytes))
        .Select(group => {
          var topRow = group.OrderByDescending(row => row.MeanAllocatedBytes).First();
          return group.Key + ": start with " + topRow.StepName + " in " + topRow.WorkloadName +
              " (" + topRow.MeanAllocatedBytes.ToString("F0", CultureInfo.InvariantCulture) + " mean allocated bytes).";
        })
        .ToArray();
  }

  private static string Recommend(string surface, string stepName) {
    if (string.Equals(surface, "stable-hash canonicalization", StringComparison.Ordinal)) {
      return "Target normalized-field collection, sorting, and canonical string materialization before changing hash contracts.";
    }

    if (string.Equals(surface, "digest generation", StringComparison.Ordinal)) {
      return "Target UTF-8 byte materialization, digest byte arrays, and lowercase hex materialization while preserving sha256-v1 output.";
    }

    if (string.Equals(surface, "satellite latest-hash-diff replay filtering", StringComparison.Ordinal)) {
      return "Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope.";
    }

    if (stepName.Contains("Unique", StringComparison.Ordinal) ||
        stepName.Contains("Satellite", StringComparison.Ordinal) ||
        stepName.Contains("SavePlan", StringComparison.Ordinal)) {
      return "Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning.";
    }

    return "Keep as measured context unless it remains above the save-plan and stable-hash rows in a follow-up run.";
  }

  private static void AppendCsvRow(StringBuilder builder, params string[] values) {
    for (var index = 0; index < values.Length; index++) {
      if (index > 0) {
        builder.Append(',');
      }

      builder.Append(EscapeCsv(values[index]));
    }

    builder.AppendLine();
  }

  private static string EscapeCsv(string value) {
    if (!value.Contains('"') &&
        !value.Contains(',') &&
        !value.Contains('\r') &&
        !value.Contains('\n')) {
      return value;
    }

    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string EscapeMarkdownCell(string value) {
    return value
        .Replace("\r", " ", StringComparison.Ordinal)
        .Replace("\n", " ", StringComparison.Ordinal)
        .Replace("|", "\\|", StringComparison.Ordinal);
  }
}
