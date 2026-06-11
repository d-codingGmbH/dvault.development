namespace DCoding.Data.DVault;

/// <summary>
/// Provides exact-name access to one PIT-backed as-of row inside a caller-supplied typed projection delegate.
/// </summary>
/// <remarks>
/// The PIT row exact-name space contains <c>ParentHashKey</c>, <c>LoadTimestamp</c>, and any tuple driving-key names
/// projected by a multi-active PIT row. Satellite values are scoped behind declared satellite names and matched with
/// <see cref="StringComparer.Ordinal" />.
/// </remarks>
public sealed class DataVaultPitProjectionRow {
  internal const string ParentHashKeyName = "ParentHashKey";
  internal const string LoadTimestampName = "LoadTimestamp";

  private readonly IReadOnlyDictionary<string, DataVaultPitProjectionValue> _values;
  private readonly IReadOnlyDictionary<string, DataVaultPitSatelliteProjectionRow> _satellites;

  internal DataVaultPitProjectionRow(
      string metadataName,
      IReadOnlyDictionary<string, DataVaultPitProjectionValue> values,
      IReadOnlyDictionary<string, DataVaultPitSatelliteProjectionRow> satellites) {
    MetadataName = metadataName;
    _values = values;
    _satellites = satellites;
  }

  internal string MetadataName { get; }

  /// <summary>
  /// Reads a required PIT row string value by exact mapped name.
  /// </summary>
  /// <param name="name">The exact PIT technical name to read, normally <c>ParentHashKey</c>.</param>
  /// <returns>The non-null string value for the mapped name.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the mapped name is missing, null, or not a string.
  /// </exception>
  public string RequiredString(string name) {
    var value = GetRequiredValue(name);
    if (value.Value is null) {
      throw DataVaultPitProjectionFailures.Create(
          DataVaultPitProjectionFailures.NullValue,
          MetadataName,
          name,
          "has a null provider value");
    }

    if (value.Value is string text) {
      return text;
    }

    throw DataVaultPitProjectionFailures.Create(
        DataVaultPitProjectionFailures.InvalidValue,
        MetadataName,
        name,
        "does not contain a string provider value");
  }

  /// <summary>
  /// Reads a required normalized UTC timestamp value by exact mapped name.
  /// </summary>
  /// <param name="name">The exact PIT technical name to read, normally <c>LoadTimestamp</c>.</param>
  /// <returns>The non-null timestamp normalized to UTC.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the mapped name is missing, null, or not a normalized timestamp.
  /// </exception>
  public DateTimeOffset RequiredDateTimeOffset(string name) {
    var value = GetRequiredValue(name);
    if (value.Value is null) {
      throw DataVaultPitProjectionFailures.Create(
          DataVaultPitProjectionFailures.NullValue,
          MetadataName,
          name,
          "has a null provider value");
    }

    if (value.Value is DateTimeOffset timestamp) {
      return timestamp.ToUniversalTime();
    }

    throw DataVaultPitProjectionFailures.Create(
        DataVaultPitProjectionFailures.InvalidValue,
        MetadataName,
        name,
        "does not contain a valid load timestamp provider value");
  }

  /// <summary>
  /// Reads a required materialized satellite snapshot by declared satellite name.
  /// </summary>
  /// <param name="satelliteName">The declared satellite name.</param>
  /// <returns>The materialized satellite projection row.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when the satellite name is not declared in the PIT row projection or the satellite segment is absent.
  /// </exception>
  public DataVaultPitSatelliteProjectionRow RequiredSatellite(string satelliteName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(satelliteName);

    if (_satellites.TryGetValue(satelliteName, out var satellite)) {
      return satellite;
    }

    throw DataVaultPitProjectionFailures.Create(
        DataVaultPitProjectionFailures.MissingSatellite,
        MetadataName,
        satelliteName,
        "is not present as a materialized PIT satellite segment");
  }

  /// <summary>
  /// Reads an optional materialized satellite snapshot by declared satellite name.
  /// </summary>
  /// <param name="satelliteName">The declared satellite name.</param>
  /// <returns>The materialized satellite projection row, or null when the segment is absent.</returns>
  public DataVaultPitSatelliteProjectionRow? OptionalSatellite(string satelliteName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(satelliteName);

    return _satellites.TryGetValue(satelliteName, out var satellite) ? satellite : null;
  }

  internal static DataVaultPitProjectionRow FromReadRecord(string metadataName, DataVaultPitReadRecord record) {
    var values = new Dictionary<string, DataVaultPitProjectionValue>(StringComparer.Ordinal) {
      [ParentHashKeyName] = DataVaultPitProjectionValue.Present(record.ParentHashKey),
      [LoadTimestampName] = DataVaultPitProjectionValue.Present(record.LoadTimestamp),
    };
    foreach (var drivingKeyValue in record.DrivingKeyValues) {
      if (!values.TryAdd(drivingKeyValue.Key, DataVaultPitProjectionValue.Present(drivingKeyValue.Value))) {
        throw DataVaultPitProjectionFailures.Create(
            DataVaultPitProjectionFailures.DuplicateName,
            metadataName,
            drivingKeyValue.Key,
            "collides with a reserved PIT row technical name");
      }
    }

    var satellites = record.SatelliteSnapshots
        .Where(snapshot => snapshot.IsPresent)
        .ToDictionary(
            snapshot => snapshot.SatelliteName,
            snapshot => DataVaultPitSatelliteProjectionRow.FromSnapshot(metadataName, snapshot),
            StringComparer.Ordinal);

    return new DataVaultPitProjectionRow(metadataName, values, satellites);
  }

  private DataVaultPitProjectionValue GetRequiredValue(string name) {
    ArgumentNullException.ThrowIfNull(name);

    if (!_values.TryGetValue(name, out var value) || value.IsMissing) {
      throw DataVaultPitProjectionFailures.Create(
          DataVaultPitProjectionFailures.MissingName,
          MetadataName,
          name,
          "is not present in the row projection");
    }

    return value;
  }
}
