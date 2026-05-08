namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkOptions(
    int Iterations,
    int WarmupIterations,
    string? ArtifactOutputDirectory = null,
    bool ScaleMatrix = false,
    DataVaultLoadTimestampStorage LoadTimestampStorage = DataVaultLoadTimestampStorage.ProviderDefault,
    string ProviderFilter = BenchmarkProviderFilters.All) {
  private const int DefaultIterations = 5;
  private const int DefaultWarmupIterations = 1;

  public static bool IsHelpRequested(IReadOnlyCollection<string> args) {
    return args.Contains("--help", StringComparer.Ordinal) || args.Contains("-h", StringComparer.Ordinal);
  }

  public static BenchmarkOptions Parse(IReadOnlyList<string> args) {
    var iterations = DefaultIterations;
    var warmupIterations = DefaultWarmupIterations;
    string? artifactOutputDirectory = null;
    var scaleMatrix = false;
    var loadTimestampStorage = DataVaultLoadTimestampStorage.ProviderDefault;
    var providerFilter = BenchmarkProviderFilters.All;

    for (var index = 0; index < args.Count; index++) {
      switch (args[index]) {
        case "--scale":
          scaleMatrix = true;
          break;
        case "--iterations":
          iterations = ReadPositiveInt(args, ref index, "--iterations");
          break;
        case "--warmup":
          warmupIterations = ReadNonNegativeInt(args, ref index, "--warmup");
          break;
        case "--output":
          artifactOutputDirectory = ReadNonEmptyString(args, ref index, "--output");
          break;
        case "--load-timestamp-storage":
          loadTimestampStorage = ReadLoadTimestampStorage(args, ref index);
          break;
        case "--provider":
          providerFilter = ReadProviderFilter(args, ref index);
          break;
        default:
          throw new ArgumentException(
              "Unsupported benchmark argument '" + args[index] + "'. Run with --help for usage.");
      }
    }

    return new BenchmarkOptions(
        iterations,
        warmupIterations,
        artifactOutputDirectory,
        scaleMatrix,
        loadTimestampStorage,
        providerFilter);
  }

  private static int ReadPositiveInt(IReadOnlyList<string> args, ref int index, string optionName) {
    var value = ReadInt(args, ref index, optionName);
    if (value <= 0) {
      throw new ArgumentOutOfRangeException(optionName, value, optionName + " must be greater than zero.");
    }

    return value;
  }

  private static int ReadNonNegativeInt(IReadOnlyList<string> args, ref int index, string optionName) {
    var value = ReadInt(args, ref index, optionName);
    if (value < 0) {
      throw new ArgumentOutOfRangeException(optionName, value, optionName + " must not be negative.");
    }

    return value;
  }

  private static int ReadInt(IReadOnlyList<string> args, ref int index, string optionName) {
    index++;
    if (index >= args.Count) {
      throw new ArgumentException("Missing value for " + optionName + ".");
    }

    if (!int.TryParse(args[index], out var value)) {
      throw new ArgumentException("Value for " + optionName + " must be an integer.");
    }

    return value;
  }

  private static string ReadNonEmptyString(IReadOnlyList<string> args, ref int index, string optionName) {
    index++;
    if (index >= args.Count) {
      throw new ArgumentException("Missing value for " + optionName + ".");
    }

    if (string.IsNullOrWhiteSpace(args[index])) {
      throw new ArgumentException("Value for " + optionName + " must not be empty.");
    }

    return args[index];
  }

  private static DataVaultLoadTimestampStorage ReadLoadTimestampStorage(IReadOnlyList<string> args, ref int index) {
    var value = ReadNonEmptyString(args, ref index, "--load-timestamp-storage");

    return value.ToLowerInvariant() switch {
      "default" or "provider-default" => DataVaultLoadTimestampStorage.ProviderDefault,
      "iso" or "iso8601" or "iso8601-utc-text" => DataVaultLoadTimestampStorage.Iso8601UtcText,
      "ticks" or "utc-ticks" => DataVaultLoadTimestampStorage.UtcTicks,
      _ => throw new ArgumentException(
          "Value for --load-timestamp-storage must be provider-default, iso8601-utc-text, or utc-ticks."),
    };
  }

  private static string ReadProviderFilter(IReadOnlyList<string> args, ref int index) {
    var value = ReadNonEmptyString(args, ref index, "--provider").ToLowerInvariant();

    if (BenchmarkProviderFilters.AllProviderFilters.Contains(value, StringComparer.Ordinal)) {
      return value;
    }

    throw new ArgumentException("Value for --provider must be all, sqlite, postgres, sqlserver, mysql, or oracle.");
  }
}

internal static class BenchmarkProviderFilters {
  public const string All = "all";
  public const string Sqlite = "sqlite";
  public const string Postgres = "postgres";
  public const string SqlServer = "sqlserver";
  public const string MySql = "mysql";
  public const string Oracle = "oracle";

  public static IReadOnlyList<string> AllProviderFilters { get; } =
  [
      All,
      Sqlite,
      Postgres,
      SqlServer,
      MySql,
      Oracle,
  ];
}
