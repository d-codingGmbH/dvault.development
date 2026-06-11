using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the SQL-function capability set exposed by a provider profile.
/// </summary>
public enum DataVaultProviderSqlFunctionSupport {
  /// <summary>
  /// The v1 profile declares no required SQL functions and treats SQL-function requests as unsupported.
  /// </summary>
  NoneInV1Unsupported,
}
