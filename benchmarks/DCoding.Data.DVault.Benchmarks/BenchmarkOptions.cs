namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkOptions(int Iterations, int WarmupIterations, string? ArtifactOutputDirectory = null) {
  private const int DefaultIterations = 5;
  private const int DefaultWarmupIterations = 1;

  public static bool IsHelpRequested(IReadOnlyCollection<string> args) {
    return args.Contains("--help", StringComparer.Ordinal) || args.Contains("-h", StringComparer.Ordinal);
  }

  public static BenchmarkOptions Parse(IReadOnlyList<string> args) {
    var iterations = DefaultIterations;
    var warmupIterations = DefaultWarmupIterations;
    string? artifactOutputDirectory = null;

    for (var index = 0; index < args.Count; index++) {
      switch (args[index]) {
        case "--iterations":
          iterations = ReadPositiveInt(args, ref index, "--iterations");
          break;
        case "--warmup":
          warmupIterations = ReadNonNegativeInt(args, ref index, "--warmup");
          break;
        case "--output":
          artifactOutputDirectory = ReadNonEmptyString(args, ref index, "--output");
          break;
        default:
          throw new ArgumentException(
              "Unsupported benchmark argument '" + args[index] + "'. Run with --help for usage.");
      }
    }

    return new BenchmarkOptions(iterations, warmupIterations, artifactOutputDirectory);
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
}
