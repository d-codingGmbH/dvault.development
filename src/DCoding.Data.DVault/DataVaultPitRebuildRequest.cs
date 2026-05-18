using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes a full rebuild request for one generated PIT table.
/// </summary>
public sealed class DataVaultPitRebuildRequest {
  /// <summary>
  /// Initializes a new PIT rebuild request.
  /// </summary>
  /// <param name="pit">The PIT metadata declaration whose generated table should be rebuilt.</param>
  public DataVaultPitRebuildRequest(DataVaultPitMetadata pit) {
    ArgumentNullException.ThrowIfNull(pit);

    Pit = pit;
  }

  /// <summary>
  /// Gets the PIT metadata declaration whose generated table should be rebuilt.
  /// </summary>
  public DataVaultPitMetadata Pit { get; }
}
