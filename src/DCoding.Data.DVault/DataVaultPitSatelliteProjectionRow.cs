namespace DCoding.Data.DVault;

/// <summary>
/// Provides exact-name access to one materialized PIT satellite segment inside a typed projection delegate.
/// </summary>
/// <remarks>
/// The exact-name space contains <c>SnapshotLoadTimestamp</c>, <c>HashDiff</c>, <c>RecordSource</c>, and the
/// satellite's declared payload names. Names are matched with <see cref="StringComparer.Ordinal" />.
/// </remarks>
public sealed class DataVaultPitSatelliteProjectionRow {
  internal const string SnapshotLoadTimestampName = "SnapshotLoadTimestamp";
  internal const string HashDiffName = "HashDiff";
  internal const string RecordSourceName = "RecordSource";

  private readonly IReadOnlyDictionary<string, DataVaultPitProjectionValue> _values;

  internal DataVaultPitSatelliteProjectionRow(
      string metadataName,
      string satelliteName,
      IReadOnlyDictionary<string, DataVaultPitProjectionValue> values) {
    MetadataName = metadataName;
    SatelliteName = satelliteName;
    _values = values;
  }

  internal string MetadataName { get; }

  /// <summary>
  /// Gets the declared satellite metadata name.
  /// </summary>
  public string SatelliteName { get; }

  /// <summary>
  /// Reads a required satellite segment string value by exact mapped name.
  /// </summary>
  /// <param name="name">The exact satellite technical or payload name to read.</param>
  /// <returns>The non-null string value for the mapped name.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the mapped name is missing, null, or not a string.
  /// </exception>
  public string RequiredString(string name) {
    var value = GetRequiredValue(name);
    if (value.Value is null) {
      throw CreateFailure(
          DataVaultPitProjectionFailures.NullValue,
          name,
          "has a null provider value");
    }

    if (value.Value is string text) {
      return text;
    }

    throw CreateFailure(
        DataVaultPitProjectionFailures.InvalidValue,
        name,
        "does not contain a string provider value");
  }

  /// <summary>
  /// Reads a nullable satellite segment string value by exact mapped name.
  /// </summary>
  /// <param name="name">The exact satellite technical or payload name to read.</param>
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

    throw CreateFailure(
        DataVaultPitProjectionFailures.InvalidValue,
        name,
        "does not contain a string provider value");
  }

  /// <summary>
  /// Reads a required normalized UTC timestamp value by exact mapped name.
  /// </summary>
  /// <param name="name">The exact satellite technical name to read, normally <c>SnapshotLoadTimestamp</c>.</param>
  /// <returns>The non-null timestamp normalized to UTC.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the mapped name is missing, null, or not a normalized timestamp.
  /// </exception>
  public DateTimeOffset RequiredDateTimeOffset(string name) {
    var value = GetRequiredValue(name);
    if (value.Value is null) {
      throw CreateFailure(
          DataVaultPitProjectionFailures.NullValue,
          name,
          "has a null provider value");
    }

    if (value.Value is DateTimeOffset timestamp) {
      return timestamp.ToUniversalTime();
    }

    throw CreateFailure(
        DataVaultPitProjectionFailures.InvalidValue,
        name,
        "does not contain a valid load timestamp provider value");
  }

  internal static DataVaultPitSatelliteProjectionRow FromSnapshot(
      string metadataName,
      DataVaultPitSatelliteSnapshot snapshot) {
    var values = new Dictionary<string, DataVaultPitProjectionValue>(StringComparer.Ordinal) {
      [SnapshotLoadTimestampName] = DataVaultPitProjectionValue.Present(snapshot.SnapshotLoadTimestamp),
      [HashDiffName] = DataVaultPitProjectionValue.Present(snapshot.HashDiff),
      [RecordSourceName] = DataVaultPitProjectionValue.Present(snapshot.RecordSource),
    };

    foreach (var payloadValue in snapshot.PayloadValues) {
      values[payloadValue.Key] = DataVaultPitProjectionValue.Present(payloadValue.Value);
    }

    return new DataVaultPitSatelliteProjectionRow(metadataName, snapshot.SatelliteName, values);
  }

  private DataVaultPitProjectionValue GetRequiredValue(string name) {
    ArgumentNullException.ThrowIfNull(name);

    if (!_values.TryGetValue(name, out var value) || value.IsMissing) {
      throw CreateFailure(
          DataVaultPitProjectionFailures.MissingName,
          name,
          "is not present in the row projection");
    }

    return value;
  }

  private InvalidOperationException CreateFailure(
      string failureKind,
      string mappedName,
      string detail) {
    return DataVaultPitProjectionFailures.Create(
        failureKind,
        MetadataName,
        SatelliteName + "." + mappedName,
        detail);
  }
}
