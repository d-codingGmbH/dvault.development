namespace DCoding.Data.DVault;

/// <summary>
/// Provides exact-name access to one bridge row inside a caller-supplied typed projection delegate.
/// </summary>
/// <remarks>
/// The exact-name space contains the generated bridge endpoint hash-key column names and, for hierarchy bridges,
/// <c>TraversalDepth</c>. Names are matched with <see cref="StringComparer.Ordinal" />.
/// </remarks>
public sealed class DataVaultBridgeProjectionRow {
  internal const string TraversalDepthName = "TraversalDepth";

  private readonly IReadOnlyDictionary<string, DataVaultBridgeProjectionValue> _values;

  internal DataVaultBridgeProjectionRow(
      string metadataName,
      IReadOnlyDictionary<string, DataVaultBridgeProjectionValue> values) {
    MetadataName = metadataName;
    _values = values;
  }

  internal string MetadataName { get; }

  /// <summary>
  /// Reads a required string value by exact generated bridge column name.
  /// </summary>
  /// <param name="name">The exact generated endpoint hash-key column name to read.</param>
  /// <returns>The non-null string value for the mapped name.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the mapped name is missing, the provider value is null, or the provider value is not a string.
  /// </exception>
  public string RequiredString(string name) {
    var value = GetRequiredValue(name);
    if (value.Value is null) {
      throw DataVaultBridgeProjectionFailures.Create(
          DataVaultBridgeProjectionFailures.NullValue,
          MetadataName,
          name,
          "has a null provider value");
    }

    if (value.Value is string text) {
      return text;
    }

    throw DataVaultBridgeProjectionFailures.Create(
        DataVaultBridgeProjectionFailures.InvalidValue,
        MetadataName,
        name,
        "does not contain a string provider value");
  }

  /// <summary>
  /// Reads a required integer value by exact generated bridge column name.
  /// </summary>
  /// <param name="name">The exact generated hierarchy depth column name to read, normally <c>TraversalDepth</c>.</param>
  /// <returns>The non-null integer value for the mapped name.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the mapped name is missing, the provider value is null, or the provider value is not an integer.
  /// </exception>
  public int RequiredInt32(string name) {
    var value = GetRequiredValue(name);
    if (value.Value is null) {
      throw DataVaultBridgeProjectionFailures.Create(
          DataVaultBridgeProjectionFailures.NullValue,
          MetadataName,
          name,
          "has a null provider value");
    }

    if (value.Value is int number) {
      return number;
    }

    throw DataVaultBridgeProjectionFailures.Create(
        DataVaultBridgeProjectionFailures.InvalidValue,
        MetadataName,
        name,
        "does not contain an integer provider value");
  }

  private DataVaultBridgeProjectionValue GetRequiredValue(string name) {
    ArgumentNullException.ThrowIfNull(name);

    if (!_values.TryGetValue(name, out var value) || value.IsMissing) {
      throw DataVaultBridgeProjectionFailures.Create(
          DataVaultBridgeProjectionFailures.MissingName,
          MetadataName,
          name,
          "is not present in the row projection");
    }

    return value;
  }
}

internal readonly record struct DataVaultBridgeProjectionValue {
  private DataVaultBridgeProjectionValue(bool isMissing, object? value) {
    IsMissing = isMissing;
    Value = value;
  }

  public bool IsMissing { get; }

  public object? Value { get; }

  public static DataVaultBridgeProjectionValue Missing() {
    return new DataVaultBridgeProjectionValue(isMissing: true, value: null);
  }

  public static DataVaultBridgeProjectionValue Present(object? value) {
    return new DataVaultBridgeProjectionValue(isMissing: false, value);
  }
}

internal static class DataVaultBridgeProjectionFailures {
  public const string MissingName = "missing-name";
  public const string NullValue = "null-value";
  public const string InvalidValue = "invalid-value";

  public static InvalidOperationException Create(
      string failureKind,
      string metadataName,
      string mappedName,
      string detail) {
    return new InvalidOperationException(
        "DVault typed bridge projection failed (" +
        failureKind +
        "): bridge metadata '" +
        metadataName +
        "' mapped name '" +
        mappedName +
        "' " +
        detail +
        ".");
  }
}
