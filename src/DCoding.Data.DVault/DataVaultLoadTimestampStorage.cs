using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the physical load-timestamp storage shape used when provider profiles are projected to EF metadata.
/// </summary>
public enum DataVaultLoadTimestampStorage {
  /// <summary>
  /// Keeps the provider profile's default load-timestamp storage mapping.
  /// </summary>
  ProviderDefault,

  /// <summary>
  /// Persists load timestamps as ISO 8601 UTC text.
  /// </summary>
  Iso8601UtcText,

  /// <summary>
  /// Persists load timestamps as UTC <see cref="DateTime" /> ticks in a native 64-bit integer column.
  /// </summary>
  UtcTicks,
}
