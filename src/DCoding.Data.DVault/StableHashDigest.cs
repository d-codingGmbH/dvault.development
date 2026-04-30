namespace DCoding.Data.DVault;

/// <summary>
/// Represents the algorithm identifier and hexadecimal value produced by a stable hash service.
/// </summary>
public sealed record StableHashDigest {
  /// <summary>
  /// Initializes a new stable hash digest.
  /// </summary>
  /// <param name="algorithmId">The stable identifier for the producing hash algorithm.</param>
  /// <param name="value">The lowercase hexadecimal digest value.</param>
  public StableHashDigest(string algorithmId, string value) {
    ArgumentException.ThrowIfNullOrWhiteSpace(algorithmId);
    ArgumentException.ThrowIfNullOrWhiteSpace(value);

    if (value.Length != 64 || !IsLowerHex(value)) {
      throw new ArgumentException(
          "The stable hash digest value must be 64 lowercase hexadecimal characters.",
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
  /// Gets the lowercase hexadecimal digest text.
  /// </summary>
  public string Value { get; private init; }

  private static bool IsLowerHex(string value) {
    foreach (var character in value) {
      if ((character < '0' || character > '9') && (character < 'a' || character > 'f')) {
        return false;
      }
    }

    return true;
  }
}
