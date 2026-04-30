namespace DCoding.Data.DVault;

/// <summary>
/// Computes deterministic digests for canonical stable-hash text.
/// </summary>
public interface IStableHashService {
  /// <summary>
  /// Gets the stable identifier for the digest algorithm and compatibility version.
  /// </summary>
  string AlgorithmId { get; }

  /// <summary>
  /// Computes a stable digest for already-normalized canonical text.
  /// </summary>
  /// <param name="normalizedInput">The exact canonical text payload to hash.</param>
  /// <returns>The algorithm identifier and digest value produced for the input text.</returns>
  StableHashDigest ComputeHash(string normalizedInput);
}
