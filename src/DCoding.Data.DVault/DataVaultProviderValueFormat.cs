using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the value format used by a provider type mapping.
/// </summary>
public enum DataVaultProviderValueFormat {
  /// <summary>
  /// Values are persisted as provider text without a provider-specific transformation.
  /// </summary>
  Text = 0,

  /// <summary>
  /// Timestamp values are persisted as ISO 8601 UTC text.
  /// </summary>
  Iso8601UtcText = 1,

  /// <summary>
  /// Timestamp values are persisted through the provider's native <see cref="DateTimeOffset" /> mapping.
  /// </summary>
  NativeDateTimeOffset = 2,

  /// <summary>
  /// Integer values are persisted through the provider's native integer mapping.
  /// </summary>
  NativeInteger = 3,

  /// <summary>
  /// Timestamp values are persisted as UTC <see cref="DateTime" /> ticks in a native 64-bit integer column.
  /// </summary>
  UtcTicks = 4,

  /// <summary>
  /// Hash digest values are persisted as canonical lowercase hexadecimal text.
  /// </summary>
  LowercaseHexText = 5,

  /// <summary>
  /// Hash digest values are persisted as bytes with a lowercase hexadecimal string conversion at the EF model boundary.
  /// </summary>
  LowercaseHexBinary = 6,
}
