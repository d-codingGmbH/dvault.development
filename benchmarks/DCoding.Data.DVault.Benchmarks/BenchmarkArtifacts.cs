using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkArtifacts {
  public const string RequiredProviderName = "SQLite local temporary files";

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
    builder.AppendLine("| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Execution status | Skip reason | Iterations | Mean ms | Min ms | Max ms | Persisted outcome |");
    builder.AppendLine("| --- | --- | --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | --- |");

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
          .Append(EscapeMarkdownCell(summary.ExecutionStatus))
          .Append(" | ")
          .Append(EscapeMarkdownCell(summary.SkipReason))
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
        .Append("- Required provider: ")
        .AppendLine(context.Provider);
    builder
        .Append("- Optional PostgreSQL provider: ")
        .AppendLine(context.OptionalPostgresProvider);
    builder
        .Append("- PostgreSQL execution status: ")
        .AppendLine(context.PostgresExecutionStatus);
    if (!string.IsNullOrEmpty(context.PostgresSkipReason)) {
      builder
          .Append("- PostgreSQL skip reason: ")
          .AppendLine(context.PostgresSkipReason);
    }
    if (context.OptionalProviders.Count > 0) {
      builder.AppendLine("- Optional provider status:");
      foreach (var provider in context.OptionalProviders) {
        builder
            .Append("  - ")
            .Append(provider.ProviderName)
            .Append(": ")
            .Append(provider.ExecutionStatus);
        if (!string.IsNullOrEmpty(provider.SkipReason)) {
          builder
              .Append(" - ")
              .Append(provider.SkipReason);
        }

        builder.AppendLine();
      }
    }

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
        .Append("- Load timestamp storage: ")
        .AppendLine(context.LoadTimestampStorage);
    builder
        .Append("- Provider filter: ")
        .AppendLine(context.ProviderFilter);
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
    builder.AppendLine("scenario,provider,baseline,strategyFamily,datasetSize,changeRatio,executionStatus,skipReason,iterations,meanMilliseconds,minMilliseconds,maxMilliseconds,persistedOutcome");

    foreach (var summary in summaries) {
      AppendCsvRow(
          builder,
          summary.ScenarioName,
          summary.Provider,
          summary.BaselineName,
          summary.StrategyFamily,
          summary.DatasetSize,
          summary.ChangeRatio,
          summary.ExecutionStatus,
          summary.SkipReason,
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

  private static string FormatMilliseconds(double? value) {
    return value?.ToString("F3", CultureInfo.InvariantCulture) ?? string.Empty;
  }
}

internal sealed record BenchmarkArtifactPaths(string MarkdownPath, string CsvPath, string JsonPath);

internal sealed record BenchmarkRunContext(
    string Provider,
    string OptionalPostgresProvider,
    string PostgresExecutionStatus,
    string PostgresSkipReason,
    int Iterations,
    int WarmupIterations,
    string LoadTimestampStorage,
    string ProviderFilter,
    string OsDescription,
    string OsArchitecture,
    string ProcessArchitecture,
    int ProcessorCount,
    string DotNetRuntimeDescription,
    string DotNetRuntimeVersion,
    IReadOnlyList<BenchmarkProviderRunContext> OptionalProviders) {
  public static BenchmarkRunContext Create(
      BenchmarkOptions options,
      PostgresBenchmarkAvailability postgresAvailability) {
    return Create(
        options,
        postgresAvailability,
        [BenchmarkProviderAvailability.FromPostgres(postgresAvailability)]);
  }

  public static BenchmarkRunContext Create(
      BenchmarkOptions options,
      PostgresBenchmarkAvailability postgresAvailability,
      IReadOnlyList<BenchmarkProviderAvailability> optionalProviders) {
    ArgumentNullException.ThrowIfNull(options);
    ArgumentNullException.ThrowIfNull(postgresAvailability);
    ArgumentNullException.ThrowIfNull(optionalProviders);

    return new BenchmarkRunContext(
        BenchmarkArtifacts.RequiredProviderName,
        PostgresBenchmarkAvailability.ProviderName,
        postgresAvailability.ExecutionStatus,
        postgresAvailability.SkipReason?.DisplayText ?? string.Empty,
        options.Iterations,
        options.WarmupIterations,
        options.LoadTimestampStorage.ToString(),
        options.ProviderFilter,
        RuntimeInformation.OSDescription,
        RuntimeInformation.OSArchitecture.ToString(),
        RuntimeInformation.ProcessArchitecture.ToString(),
        Environment.ProcessorCount,
        RuntimeInformation.FrameworkDescription,
        Environment.Version.ToString(),
        [.. optionalProviders.Select(BenchmarkProviderRunContext.FromAvailability)]);
  }
}

internal sealed record BenchmarkProviderRunContext(
    string ProviderName,
    string ConnectionStringEnvironmentVariable,
    string ExecutionStatus,
    string SkipReason) {
  public static BenchmarkProviderRunContext FromAvailability(BenchmarkProviderAvailability availability) {
    ArgumentNullException.ThrowIfNull(availability);

    return new BenchmarkProviderRunContext(
        availability.ProviderName,
        availability.Definition.ConnectionStringEnvironmentVariable,
        availability.ExecutionStatus,
        availability.SkipReason?.DisplayText ?? string.Empty);
  }
}

internal sealed record BenchmarkArtifactDocument(
    BenchmarkRunContext Context,
    IReadOnlyList<BenchmarkSummary> Results);
