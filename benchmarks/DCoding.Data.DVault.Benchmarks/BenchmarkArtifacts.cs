using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkArtifacts {
  public const string ProviderName = "SQLite local temporary files";

  private const string MarkdownFileName = "benchmark-summary.md";
  private const string CsvFileName = "benchmark-summary.csv";
  private const string JsonFileName = "benchmark-summary.json";

  private static readonly JsonSerializerOptions SerializerOptions = new() {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
  };

  public static async Task<BenchmarkArtifactPaths> WriteAsync(
      string outputDirectory,
      BenchmarkRunContext context,
      IReadOnlyList<BenchmarkSummary> summaries,
      CancellationToken cancellationToken) {
    ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
    ArgumentNullException.ThrowIfNull(context);
    ArgumentNullException.ThrowIfNull(summaries);

    var fullOutputDirectory = Path.GetFullPath(outputDirectory);
    Directory.CreateDirectory(fullOutputDirectory);

    var markdownPath = Path.Combine(fullOutputDirectory, MarkdownFileName);
    var csvPath = Path.Combine(fullOutputDirectory, CsvFileName);
    var jsonPath = Path.Combine(fullOutputDirectory, JsonFileName);

    await File.WriteAllTextAsync(markdownPath, CreateMarkdown(context, summaries), cancellationToken)
        .ConfigureAwait(false);
    await File.WriteAllTextAsync(csvPath, CreateCsv(summaries), cancellationToken)
        .ConfigureAwait(false);
    await File.WriteAllTextAsync(jsonPath, CreateJson(context, summaries), cancellationToken)
        .ConfigureAwait(false);

    return new BenchmarkArtifactPaths(markdownPath, csvPath, jsonPath);
  }

  public static string CreateMarkdownTable(IEnumerable<BenchmarkSummary> summaries) {
    ArgumentNullException.ThrowIfNull(summaries);

    var builder = new StringBuilder();
    builder.AppendLine("| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Iterations | Mean ms | Min ms | Max ms | Persisted outcome |");
    builder.AppendLine("| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | --- |");

    foreach (var summary in summaries) {
      builder
          .Append("| ")
          .Append(EscapeMarkdownCell(summary.ScenarioName))
          .Append(" | ")
          .Append(EscapeMarkdownCell(summary.Provider))
          .Append(" | ")
          .Append(EscapeMarkdownCell(summary.BaselineName))
          .Append(" | ")
          .Append(EscapeMarkdownCell(summary.StrategyFamily))
          .Append(" | ")
          .Append(EscapeMarkdownCell(summary.DatasetSize))
          .Append(" | ")
          .Append(EscapeMarkdownCell(summary.ChangeRatio))
          .Append(" | ")
          .Append(summary.Iterations.ToString(CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(FormatMilliseconds(summary.MeanMilliseconds))
          .Append(" | ")
          .Append(FormatMilliseconds(summary.MinMilliseconds))
          .Append(" | ")
          .Append(FormatMilliseconds(summary.MaxMilliseconds))
          .Append(" | ")
          .Append(EscapeMarkdownCell(summary.PersistedOutcome))
          .AppendLine(" |");
    }

    return builder.ToString();
  }

  private static string CreateMarkdown(BenchmarkRunContext context, IReadOnlyList<BenchmarkSummary> summaries) {
    var builder = new StringBuilder();
    builder.AppendLine("# DVault Benchmark Summary");
    builder.AppendLine();
    builder.AppendLine("## Summary");
    builder.AppendLine();
    builder
        .Append("- Benchmark baselines: ")
        .AppendLine(summaries.Count.ToString(CultureInfo.InvariantCulture));
    builder
        .Append("- Provider: ")
        .AppendLine(context.Provider);
    builder.AppendLine("- External services: not required");
    builder.AppendLine();
    builder.AppendLine("## Run Context");
    builder.AppendLine();
    builder
        .Append("- Iterations: ")
        .AppendLine(context.Iterations.ToString(CultureInfo.InvariantCulture));
    builder
        .Append("- Warmup iterations: ")
        .AppendLine(context.WarmupIterations.ToString(CultureInfo.InvariantCulture));
    builder
        .Append("- OS description: ")
        .AppendLine(context.OsDescription);
    builder
        .Append("- OS architecture: ")
        .AppendLine(context.OsArchitecture);
    builder
        .Append("- Process architecture: ")
        .AppendLine(context.ProcessArchitecture);
    builder
        .Append("- Processor count: ")
        .AppendLine(context.ProcessorCount.ToString(CultureInfo.InvariantCulture));
    builder
        .Append("- .NET runtime description: ")
        .AppendLine(context.DotNetRuntimeDescription);
    builder
        .Append("- .NET runtime version: ")
        .AppendLine(context.DotNetRuntimeVersion);
    builder.AppendLine();
    builder.AppendLine("## Results");
    builder.AppendLine();
    builder.Append(CreateMarkdownTable(summaries));

    return builder.ToString();
  }

  private static string CreateCsv(IEnumerable<BenchmarkSummary> summaries) {
    var builder = new StringBuilder();
    builder.AppendLine("scenario,provider,baseline,strategyFamily,datasetSize,changeRatio,iterations,meanMilliseconds,minMilliseconds,maxMilliseconds,persistedOutcome");

    foreach (var summary in summaries) {
      AppendCsvRow(
          builder,
          summary.ScenarioName,
          summary.Provider,
          summary.BaselineName,
          summary.StrategyFamily,
          summary.DatasetSize,
          summary.ChangeRatio,
          summary.Iterations.ToString(CultureInfo.InvariantCulture),
          FormatMilliseconds(summary.MeanMilliseconds),
          FormatMilliseconds(summary.MinMilliseconds),
          FormatMilliseconds(summary.MaxMilliseconds),
          summary.PersistedOutcome);
    }

    return builder.ToString();
  }

  private static string CreateJson(BenchmarkRunContext context, IReadOnlyList<BenchmarkSummary> summaries) {
    var document = new BenchmarkArtifactDocument(context, summaries);
    return JsonSerializer.Serialize(document, SerializerOptions) + Environment.NewLine;
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

  private static string FormatMilliseconds(double value) {
    return value.ToString("F3", CultureInfo.InvariantCulture);
  }
}

internal sealed record BenchmarkArtifactPaths(string MarkdownPath, string CsvPath, string JsonPath);

internal sealed record BenchmarkRunContext(
    string Provider,
    int Iterations,
    int WarmupIterations,
    string OsDescription,
    string OsArchitecture,
    string ProcessArchitecture,
    int ProcessorCount,
    string DotNetRuntimeDescription,
    string DotNetRuntimeVersion) {
  public static BenchmarkRunContext Create(BenchmarkOptions options) {
    ArgumentNullException.ThrowIfNull(options);

    return new BenchmarkRunContext(
        BenchmarkArtifacts.ProviderName,
        options.Iterations,
        options.WarmupIterations,
        RuntimeInformation.OSDescription,
        RuntimeInformation.OSArchitecture.ToString(),
        RuntimeInformation.ProcessArchitecture.ToString(),
        Environment.ProcessorCount,
        RuntimeInformation.FrameworkDescription,
        Environment.Version.ToString());
  }
}

internal sealed record BenchmarkArtifactDocument(
    BenchmarkRunContext Context,
    IReadOnlyList<BenchmarkSummary> Results);
