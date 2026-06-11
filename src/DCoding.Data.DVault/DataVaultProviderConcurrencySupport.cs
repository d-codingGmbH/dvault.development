using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the concurrency capability set exposed by a provider profile.
/// </summary>
public enum DataVaultProviderConcurrencySupport {
  /// <summary>
  /// The v1 profile declares no concurrency tokens or mutable-record conflict signals.
  /// </summary>
  NoneInV1Unsupported,
}
