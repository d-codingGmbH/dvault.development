namespace DCoding.Data.DVault;

/// <summary>
/// Represents the algorithm identifier and canonical hexadecimal value produced by a stable hash service.
/// </summary>
public sealed record StableHashDigest {
  /// <summary>
  /// Initializes a new stable hash digest.
  /// </summary>
  /// <param name="algorithmId">The stable identifier for the producing hash algorithm and digest shape.</param>
  /// <param name="value">The canonical lowercase hexadecimal digest value.</param>
  public StableHashDigest(string algorithmId, string value) {
    ArgumentException.ThrowIfNullOrWhiteSpace(algorithmId);
    ArgumentException.ThrowIfNullOrWhiteSpace(value);

    if (value.Length % 2 != 0 || !IsLowerHex(value)) {
      throw new ArgumentException(
          "The stable hash digest value must be whole-byte lowercase hexadecimal text.",
          nameof(value));
    }

    var expectedHexLength = GetExpectedHexLength(algorithmId);
    if (expectedHexLength is not null && value.Length != expectedHexLength.Value) {
      throw new ArgumentException(
          "The stable hash digest value length is not valid for algorithm '" +
          algorithmId +
          "'. Expected " +
          expectedHexLength.Value +
          " lowercase hexadecimal characters.",
          nameof(value));
    }

    AlgorithmId = algorithmId;
    Value = value;
  }

  /// <summary>
  /// Gets the stable identifier copied from the hash service that produced this digest.
  /// </summary>
  public string AlgorithmId { get; private init; }

  /// <summary>
  /// Gets the number of digest bytes represented by <see cref="Value" />.
  /// </summary>
  public int DigestByteLength => Value.Length / 2;

  /// <summary>
  /// Gets the canonical lowercase hexadecimal digest text.
  /// </summary>
  public string Value { get; private init; }

  private static int? GetExpectedHexLength(string algorithmId) {
    return algorithmId switch {
      "sha256-v1" => 64,
      "sha1-v1" => 40,
      "sha256-128-v1" => 32,
      "sha256-160-v1" => 40,
      _ => null,
    };
  }

  private static bool IsLowerHex(string value) {
    foreach (var character in value) {
      if ((character < '0' || character > '9') && (character < 'a' || character > 'f')) {
        return false;
      }
    }

    return true;
  }
}
