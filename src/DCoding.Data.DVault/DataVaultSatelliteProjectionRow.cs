namespace DCoding.Data.DVault;

/// <summary>
/// Provides exact-name access to one latest/as-of satellite row inside a caller-supplied typed projection delegate.
/// </summary>
/// <remarks>
/// The exact-name space contains the technical names <c>ParentHashKey</c>, <c>HashDiff</c>, <c>LoadTimestamp</c>,
/// and <c>RecordSource</c>, plus the satellite's declared driving-key names and payload names. Names are matched with
/// <see cref="StringComparer.Ordinal" />.
/// </remarks>
public sealed class DataVaultSatelliteProjectionRow {
  internal const string ParentHashKeyName = "ParentHashKey";
  internal const string HashDiffName = "HashDiff";
  internal const string LoadTimestampName = "LoadTimestamp";
  internal const string RecordSourceName = "RecordSource";

  private readonly IReadOnlyDictionary<string, DataVaultSatelliteProjectionValue> _values;

  internal DataVaultSatelliteProjectionRow(
      string metadataName,
      IReadOnlyDictionary<string, DataVaultSatelliteProjectionValue> values) {
    MetadataName = metadataName;
    _values = values;
  }

  internal string MetadataName { get; }

  /// <summary>
  /// Reads a required string value by exact mapped name.
  /// </summary>
  /// <param name="name">The exact technical, driving-key, or payload name to read.</param>
  /// <returns>The non-null string value for the mapped name.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the mapped name is missing, the provider value is null, or the provider value is not a string.
  /// </exception>
  public string RequiredString(string name) {
    var value = GetRequiredValue(name);
    if (value.Value is null) {
      throw DataVaultSatelliteProjectionFailures.Create(
          DataVaultSatelliteProjectionFailures.NullValue,
          MetadataName,
          name,
          "has a null provider value");
    }

    if (value.Value is string text) {
      return text;
    }

    throw DataVaultSatelliteProjectionFailures.Create(
        DataVaultSatelliteProjectionFailures.InvalidValue,
        MetadataName,
        name,
        "does not contain a string provider value");
  }

  /// <summary>
  /// Reads a nullable string value by exact mapped name.
  /// </summary>
  /// <param name="name">The exact technical, driving-key, or payload name to read.</param>
  /// <returns>The string value, or null when the mapped name exists and the provider value is null.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the mapped name is missing or the provider value is not a string or null.
  /// </exception>
  public string? NullableString(string name) {
    var value = GetRequiredValue(name);
    if (value.Value is null) {
      return null;
    }

    if (value.Value is string text) {
      return text;
    }

    throw DataVaultSatelliteProjectionFailures.Create(
        DataVaultSatelliteProjectionFailures.InvalidValue,
        MetadataName,
        name,
        "does not contain a string provider value");
  }

  /// <summary>
  /// Reads a required normalized UTC load timestamp value by exact mapped name.
  /// </summary>
  /// <param name="name">The exact technical mapped name to read, normally <c>LoadTimestamp</c>.</param>
  /// <returns>The non-null load timestamp normalized to UTC.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the mapped name is missing, the provider value is null, or the provider value is not a normalized timestamp.
  /// </exception>
  public DateTimeOffset RequiredDateTimeOffset(string name) {
    var value = GetRequiredValue(name);
    if (value.Value is null) {
      throw DataVaultSatelliteProjectionFailures.Create(
          DataVaultSatelliteProjectionFailures.NullValue,
          MetadataName,
          name,
          "has a null provider value");
    }

    if (value.Value is DateTimeOffset timestamp) {
      return timestamp.ToUniversalTime();
    }

    throw DataVaultSatelliteProjectionFailures.Create(
        DataVaultSatelliteProjectionFailures.InvalidValue,
        MetadataName,
        name,
        "does not contain a valid load timestamp provider value");
  }

  private DataVaultSatelliteProjectionValue GetRequiredValue(string name) {
    ArgumentNullException.ThrowIfNull(name);

    if (!_values.TryGetValue(name, out var value) || value.IsMissing) {
      throw DataVaultSatelliteProjectionFailures.Create(
          DataVaultSatelliteProjectionFailures.MissingName,
          MetadataName,
          name,
          "is not present in the row projection");
    }

    return value;
  }
}
