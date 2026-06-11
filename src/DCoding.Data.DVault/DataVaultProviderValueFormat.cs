using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the value format used by a provider type mapping.
/// </summary>
public enum DataVaultProviderValueFormat {
  /// <summary>
  /// Values are persisted as provider text without a provider-specific transformation.
  /// </summary>
  Text,

  /// <summary>
  /// Timestamp values are persisted as ISO 8601 UTC text.
  /// </summary>
  Iso8601UtcText,

  /// <summary>
  /// Timestamp values are persisted through the provider's native <see cref="DateTimeOffset" /> mapping.
  /// </summary>
  NativeDateTimeOffset,

  /// <summary>
  /// Integer values are persisted through the provider's native integer mapping.
  /// </summary>
  NativeInteger,

  /// <summary>
  /// Timestamp values are persisted as UTC <see cref="DateTime" /> ticks in a native 64-bit integer column.
  /// </summary>
  UtcTicks,
}
