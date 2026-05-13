namespace DCoding.Data.DVault;

/// <summary>
/// Classifies the outcome of a requested live database schema read.
/// </summary>
public enum DataVaultLiveSchemaReadStatus {
  /// <summary>
  /// The live schema was read successfully and a snapshot is available.
  /// </summary>
  Succeeded,

  /// <summary>
  /// The current provider does not have a live schema reader implementation in this DVault slice.
  /// </summary>
  UnsupportedProvider,

  /// <summary>
  /// The provider is supported, but the live database or schema metadata was unavailable in the current environment.
  /// </summary>
  Unavailable,
}
