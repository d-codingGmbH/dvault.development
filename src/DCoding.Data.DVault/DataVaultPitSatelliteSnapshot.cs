using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one satellite segment materialized from a matched PIT row.
/// </summary>
public sealed class DataVaultPitSatelliteSnapshot {
  internal DataVaultPitSatelliteSnapshot(
      string satelliteName,
      int ordinal,
      bool isPresent,
      DateTimeOffset? snapshotLoadTimestamp,
      string? hashDiff,
      string? recordSource,
      IReadOnlyDictionary<string, string?> payloadValues) {
    SatelliteName = satelliteName;
    Ordinal = ordinal;
    IsPresent = isPresent;
    SnapshotLoadTimestamp = snapshotLoadTimestamp?.ToUniversalTime();
    HashDiff = hashDiff;
    RecordSource = recordSource;
    PayloadValues = new ReadOnlyDictionary<string, string?>(
        payloadValues as IDictionary<string, string?> ??
        payloadValues.ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.Ordinal));
  }

  /// <summary>
  /// Gets the declared satellite metadata name.
  /// </summary>
  public string SatelliteName { get; }

  /// <summary>
  /// Gets the zero-based PIT satellite declaration ordinal.
  /// </summary>
  public int Ordinal { get; }

  /// <summary>
  /// Gets a value indicating whether a satellite row was materialized for the PIT snapshot reference.
  /// </summary>
  public bool IsPresent { get; }

  /// <summary>
  /// Gets the satellite load timestamp referenced by the matched PIT row, normalized to UTC.
  /// </summary>
  public DateTimeOffset? SnapshotLoadTimestamp { get; }

  /// <summary>
  /// Gets the materialized satellite hash diff, or null when the segment is absent.
  /// </summary>
  public string? HashDiff { get; }

  /// <summary>
  /// Gets the materialized satellite record source, or null when the segment is absent.
  /// </summary>
  public string? RecordSource { get; }

  /// <summary>
  /// Gets materialized payload values keyed by declared satellite payload name.
  /// </summary>
  public IReadOnlyDictionary<string, string?> PayloadValues { get; }

  internal static DataVaultPitSatelliteSnapshot Missing(string satelliteName, int ordinal) {
    return new DataVaultPitSatelliteSnapshot(
        satelliteName,
        ordinal,
        isPresent: false,
        snapshotLoadTimestamp: null,
        hashDiff: null,
        recordSource: null,
        new Dictionary<string, string?>(StringComparer.Ordinal));
  }
}
