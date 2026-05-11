using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes a request for PIT-backed as-of rows for explicit parent hub hash keys.
/// </summary>
public sealed class DataVaultPitAsOfReadRequest {
  /// <summary>
  /// Initializes a new PIT-backed as-of read request.
  /// </summary>
  /// <param name="pit">The PIT metadata declaration to read.</param>
  /// <param name="parentHashKeys">The parent hub hash keys to read.</param>
  /// <param name="asOf">The inclusive UTC cutoff for PIT-row visibility.</param>
  public DataVaultPitAsOfReadRequest(
      DataVaultPitMetadata pit,
      IEnumerable<string> parentHashKeys,
      DateTimeOffset asOf) {
    ArgumentNullException.ThrowIfNull(pit);
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    Pit = pit;
    ParentHashKeys = DataVaultLatestSatelliteReadRequest.RequireParentHashKeys(parentHashKeys);
    AsOf = asOf.ToUniversalTime();
  }

  /// <summary>
  /// Gets the PIT metadata declaration to read.
  /// </summary>
  public DataVaultPitMetadata Pit { get; }

  /// <summary>
  /// Gets the parent hub hash keys to read.
  /// </summary>
  public IReadOnlyList<string> ParentHashKeys { get; }

  /// <summary>
  /// Gets the inclusive UTC cutoff for PIT-row visibility.
  /// </summary>
  public DateTimeOffset AsOf { get; }
}
