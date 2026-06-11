using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies how a provider profile should handle index include columns when the provider has no native include support.
/// </summary>
public enum DataVaultUnsupportedIncludedIndexColumnMode {
  /// <summary>
  /// Drops include columns for providers that cannot persist them as native include columns.
  /// </summary>
  Ignore,

  /// <summary>
  /// Appends include columns to the index key for providers that cannot persist native include columns.
  /// </summary>
  AppendToKey,
}
