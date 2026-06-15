namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkOptions(
    int Iterations,
    int WarmupIterations,
    string? ArtifactOutputDirectory = null,
    bool ScaleMatrix = false,
    bool LatestIndexMatrix = false,
    DataVaultLoadTimestampStorage LoadTimestampStorage = DataVaultLoadTimestampStorage.ProviderDefault,
    string ProviderFilter = BenchmarkProviderFilters.All,
    IReadOnlyList<BenchmarkHashKeyVariant>? HashKeyVariants = null) {
  private const int DefaultIterations = 5;
  private const int DefaultWarmupIterations = 1;
  private const string DefaultStableHashAlgorithmId = "sha256-v1";

  public IReadOnlyList<BenchmarkHashKeyVariant> EffectiveHashKeyVariants { get; } =
      HashKeyVariants is null || HashKeyVariants.Count == 0
          ? [BenchmarkHashKeyVariant.Default]
          : HashKeyVariants;

  public static bool IsHelpRequested(IReadOnlyCollection<string> args) {
    return args.Contains("--help", StringComparer.Ordinal) || args.Contains("-h", StringComparer.Ordinal);
  }

  public static BenchmarkOptions Parse(IReadOnlyList<string> args) {
    var iterations = DefaultIterations;
    var warmupIterations = DefaultWarmupIterations;
    string? artifactOutputDirectory = null;
    var scaleMatrix = false;
    var latestIndexMatrix = false;
    var loadTimestampStorage = DataVaultLoadTimestampStorage.ProviderDefault;
    var providerFilter = BenchmarkProviderFilters.All;
    var hashKeyStorageMatrix = false;
    string? stableHashAlgorithmId = null;
    DataVaultHashKeyStorageProfile? hashKeyStorageProfile = null;

    for (var index = 0; index < args.Count; index++) {
      switch (args[index]) {
        case "--scale":
          scaleMatrix = true;
          break;
        case "--latest-indexes":
          latestIndexMatrix = true;
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
        case "--hash-key-storage-matrix":
          hashKeyStorageMatrix = true;
          break;
        case "--stable-hash":
          stableHashAlgorithmId = ReadStableHashAlgorithmId(args, ref index);
          break;
        case "--hash-key-storage":
          hashKeyStorageProfile = ReadHashKeyStorageProfile(args, ref index);
          break;
        default:
          throw new ArgumentException(
              "Unsupported benchmark argument '" + args[index] + "'. Run with --help for usage.");
      }
    }

    if (scaleMatrix && latestIndexMatrix) {
      throw new ArgumentException("--scale and --latest-indexes cannot be combined.");
    }

    if (hashKeyStorageMatrix && (stableHashAlgorithmId is not null || hashKeyStorageProfile is not null)) {
      throw new ArgumentException(
          "--hash-key-storage-matrix cannot be combined with --stable-hash or --hash-key-storage.");
    }

    return new BenchmarkOptions(
        iterations,
        warmupIterations,
        artifactOutputDirectory,
        scaleMatrix,
        latestIndexMatrix,
        loadTimestampStorage,
        providerFilter,
        hashKeyStorageMatrix
            ? BenchmarkHashKeyVariant.BoundedStorageMatrix
            : [
                CreateHashKeyVariant(
                    stableHashAlgorithmId ?? DefaultStableHashAlgorithmId,
                    hashKeyStorageProfile ?? DataVaultHashKeyStorageProfile.HexString),
            ]);
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

    throw new ArgumentException("Value for --provider must be all, sqlite, postgres, sqlserver, mysql, oracle, or db2.");
  }

  private static string ReadStableHashAlgorithmId(IReadOnlyList<string> args, ref int index) {
    var value = ReadNonEmptyString(args, ref index, "--stable-hash").ToLowerInvariant();

    return value switch {
      "sha256-v1" or "sha256-128-v1" => value,
      _ => throw new ArgumentException("Value for --stable-hash must be sha256-v1 or sha256-128-v1."),
    };
  }

  private static DataVaultHashKeyStorageProfile ReadHashKeyStorageProfile(IReadOnlyList<string> args, ref int index) {
    var value = ReadNonEmptyString(args, ref index, "--hash-key-storage").ToLowerInvariant();

    return value switch {
      "hex" or "hex-string" or "hexstring" => DataVaultHashKeyStorageProfile.HexString,
      "binary" => DataVaultHashKeyStorageProfile.Binary,
      _ => throw new ArgumentException("Value for --hash-key-storage must be hex or binary."),
    };
  }

  private static BenchmarkHashKeyVariant CreateHashKeyVariant(
      string stableHashAlgorithmId,
      DataVaultHashKeyStorageProfile storageProfile) {
    var digestByteLength = stableHashAlgorithmId switch {
      "sha256-v1" => 32,
      "sha256-128-v1" => 16,
      _ => throw new ArgumentException("Unsupported stable hash algorithm id '" + stableHashAlgorithmId + "'."),
    };

    var storageLabel = storageProfile == DataVaultHashKeyStorageProfile.Binary
        ? "binary"
        : "hex";
    return stableHashAlgorithmId == BenchmarkHashKeyVariant.Default.StableHashAlgorithmId &&
        storageProfile == BenchmarkHashKeyVariant.Default.StorageProfile
        ? BenchmarkHashKeyVariant.Default
        : new BenchmarkHashKeyVariant(
            stableHashAlgorithmId + "-" + storageLabel,
            stableHashAlgorithmId,
            digestByteLength,
            storageProfile);
  }
}
