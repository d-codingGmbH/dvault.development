using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one hub row whose metadata should be resolved from the authoritative registry by logical hub name.
/// </summary>
public sealed class DataVaultRegistryHubSaveOperation {
  /// <summary>
  /// Initializes a new registry-backed hub save operation.
  /// </summary>
  /// <param name="hubName">The exact logical hub metadata name to resolve from the authoritative registry.</param>
  /// <param name="businessKeyValues">Business-key values keyed by the resolved hub metadata business-key names.</param>
  public DataVaultRegistryHubSaveOperation(
      string hubName,
      IEnumerable<KeyValuePair<string, string>> businessKeyValues) {
    HubName = DataVaultMetadataValidation.RequireName(hubName, nameof(hubName));
    BusinessKeyValues = DataVaultHubSaveOperation.RequireValues(businessKeyValues, nameof(businessKeyValues));
  }

  /// <summary>
  /// Gets the exact logical hub metadata name to resolve from the authoritative registry.
  /// </summary>
  public string HubName { get; }

  /// <summary>
  /// Gets business-key values keyed by the resolved hub metadata business-key names.
  /// </summary>
  public IReadOnlyDictionary<string, string> BusinessKeyValues { get; }
}
