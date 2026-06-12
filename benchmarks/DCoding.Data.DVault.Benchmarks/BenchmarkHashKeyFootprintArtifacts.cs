using System.Globalization;
using System.Text;
using System.Text.Json;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkHashKeyFootprintArtifacts {
  public const string MarkdownFileName = "hash-key-footprint.md";
  public const string CsvFileName = "hash-key-footprint.csv";
  public const string JsonFileName = "hash-key-footprint.json";

  private static readonly JsonSerializerOptions SerializerOptions = new() {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
  };

  public static async Task<BenchmarkHashKeyFootprintArtifactPaths?> WriteAsync(
      string outputDirectory,
      BenchmarkRunContext context,
      IReadOnlyList<BenchmarkSummary> summaries,
      CancellationToken cancellationToken) {
    ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
    ArgumentNullException.ThrowIfNull(context);
    ArgumentNullException.ThrowIfNull(summaries);

    if (context.HashKeyVariants.Count <= 1) {
      return null;
    }

    var rows = CreateRows(context, summaries);
    var markdownPath = Path.Combine(outputDirectory, MarkdownFileName);
    var csvPath = Path.Combine(outputDirectory, CsvFileName);
    var jsonPath = Path.Combine(outputDirectory, JsonFileName);

    await File.WriteAllTextAsync(markdownPath, CreateMarkdown(context, rows), cancellationToken)
        .ConfigureAwait(false);
    await File.WriteAllTextAsync(csvPath, CreateCsv(rows), cancellationToken)
        .ConfigureAwait(false);
    await File.WriteAllTextAsync(jsonPath, CreateJson(context, rows), cancellationToken)
        .ConfigureAwait(false);

    return new BenchmarkHashKeyFootprintArtifactPaths(markdownPath, csvPath, jsonPath);
  }

  public static IReadOnlyList<BenchmarkHashKeyFootprintRow> CreateRows(
      BenchmarkRunContext context,
      IReadOnlyList<BenchmarkSummary> summaries) {
    ArgumentNullException.ThrowIfNull(context);
    ArgumentNullException.ThrowIfNull(summaries);

    return
    [
        .. context.HashKeyVariants.Select(variant => CreateRow(context, summaries, variant)),
    ];
  }

  private static BenchmarkHashKeyFootprintRow CreateRow(
      BenchmarkRunContext context,
      IReadOnlyList<BenchmarkSummary> summaries,
      BenchmarkHashKeyVariantRunContext variant) {
    var providerCapabilities = DataVaultProviderCapabilityProfiles.Sqlite.WithHashKeyStorageProfile(
        ParseStorageProfile(variant.StorageProfile),
        variant.StableHashAlgorithmId,
        variant.DigestByteLength);
    var hashKeyMapping = providerCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.HashKey);
    var participantReferenceMapping = providerCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.ParticipantReference);
    var matchingSummaries = summaries
        .Where(summary =>
            string.Equals(summary.Provider, BenchmarkArtifacts.RequiredProviderName, StringComparison.Ordinal) &&
            summary.ExecutionDetail.Contains("hashKeyVariant=" + variant.Label, StringComparison.Ordinal))
        .ToArray();

    return new BenchmarkHashKeyFootprintRow(
        variant.Label,
        BenchmarkArtifacts.RequiredProviderName,
        variant.StableHashAlgorithmId,
        variant.DigestByteLength,
        variant.HexCharacterLength,
        variant.StorageProfile,
        hashKeyMapping.NativeStoreType,
        participantReferenceMapping.NativeStoreType,
        hashKeyMapping.ValueFormat.ToString(),
        participantReferenceMapping.ValueFormat.ToString(),
        variant.HashKeyPayloadBytes,
        variant.HashKeyPayloadBytes,
        variant.HashKeyPayloadBytes * 2,
        matchingSummaries.Count(summary => summary.ExecutionStatus == BenchmarkExecutionStatus.Completed),
        matchingSummaries.Count(summary => summary.ExecutionStatus == BenchmarkExecutionStatus.Skipped),
        matchingSummaries.Count(summary => summary.ExecutionStatus == BenchmarkExecutionStatus.Failed));
  }

  private static string CreateMarkdown(
      BenchmarkRunContext context,
      IReadOnlyList<BenchmarkHashKeyFootprintRow> rows) {
    var builder = new StringBuilder();
    builder.AppendLine("# DVault Hash-Key Footprint Summary");
    builder.AppendLine();
    builder.AppendLine("## Run Context");
    builder.AppendLine();
    builder
        .Append("- Benchmark artifact triplet: ")
        .AppendLine("benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json");
    builder
        .Append("- Hash key variants: ")
        .AppendLine(string.Join(", ", context.HashKeyVariants.Select(variant => variant.Label)));
    builder
        .Append("- Required provider: ")
        .AppendLine(context.Provider);
    builder.AppendLine();
    builder.AppendLine("## Footprint Rows");
    builder.AppendLine();
    builder.AppendLine("| Variant | Provider | Stable hash algorithm | Digest bytes | Hex characters | Storage profile | Hash key store type | Participant reference store type | Hash key value format | Participant reference value format | Hash key payload bytes | Parent reference payload bytes | Two-column hash-reference index payload bytes | Completed rows | Skipped rows | Failed rows |");
    builder.AppendLine("| --- | --- | --- | ---: | ---: | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |");

    foreach (var row in rows) {
      builder
          .Append("| ")
          .Append(EscapeMarkdownCell(row.Variant))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.Provider))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.StableHashAlgorithmId))
          .Append(" | ")
          .Append(row.DigestByteLength.ToString(CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(row.HexCharacterLength.ToString(CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.StorageProfile))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.HashKeyStoreType))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.ParticipantReferenceStoreType))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.HashKeyValueFormat))
          .Append(" | ")
          .Append(EscapeMarkdownCell(row.ParticipantReferenceValueFormat))
          .Append(" | ")
          .Append(row.HashKeyPayloadBytes.ToString(CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(row.ParentHashReferencePayloadBytes.ToString(CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(row.TwoColumnHashReferenceIndexPayloadBytes.ToString(CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(row.CompletedRows.ToString(CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(row.SkippedRows.ToString(CultureInfo.InvariantCulture))
          .Append(" | ")
          .Append(row.FailedRows.ToString(CultureInfo.InvariantCulture))
          .AppendLine(" |");
    }

    return builder.ToString();
  }

  private static string CreateCsv(IEnumerable<BenchmarkHashKeyFootprintRow> rows) {
    var builder = new StringBuilder();
    builder.AppendLine("variant,provider,stableHashAlgorithmId,digestByteLength,hexCharacterLength,storageProfile,hashKeyStoreType,participantReferenceStoreType,hashKeyValueFormat,participantReferenceValueFormat,hashKeyPayloadBytes,parentHashReferencePayloadBytes,twoColumnHashReferenceIndexPayloadBytes,completedRows,skippedRows,failedRows");

    foreach (var row in rows) {
      AppendCsvRow(
          builder,
          row.Variant,
          row.Provider,
          row.StableHashAlgorithmId,
          row.DigestByteLength.ToString(CultureInfo.InvariantCulture),
          row.HexCharacterLength.ToString(CultureInfo.InvariantCulture),
          row.StorageProfile,
          row.HashKeyStoreType,
          row.ParticipantReferenceStoreType,
          row.HashKeyValueFormat,
          row.ParticipantReferenceValueFormat,
          row.HashKeyPayloadBytes.ToString(CultureInfo.InvariantCulture),
          row.ParentHashReferencePayloadBytes.ToString(CultureInfo.InvariantCulture),
          row.TwoColumnHashReferenceIndexPayloadBytes.ToString(CultureInfo.InvariantCulture),
          row.CompletedRows.ToString(CultureInfo.InvariantCulture),
          row.SkippedRows.ToString(CultureInfo.InvariantCulture),
          row.FailedRows.ToString(CultureInfo.InvariantCulture));
    }

    return builder.ToString();
  }

  private static string CreateJson(
      BenchmarkRunContext context,
      IReadOnlyList<BenchmarkHashKeyFootprintRow> rows) {
    return JsonSerializer.Serialize(new BenchmarkHashKeyFootprintDocument(context, rows), SerializerOptions) +
        Environment.NewLine;
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

  private static DataVaultHashKeyStorageProfile ParseStorageProfile(string storageProfile) {
    return Enum.Parse<DataVaultHashKeyStorageProfile>(storageProfile, ignoreCase: false);
  }
}
