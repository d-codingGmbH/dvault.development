using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one hub row to persist through the explicit DVault save service.
/// </summary>
public sealed class DataVaultHubSaveOperation {
  /// <summary>
  /// Initializes a new hub save operation.
  /// </summary>
  /// <param name="metadata">The hub metadata declaration that owns the target table and business-key shape.</param>
  /// <param name="businessKeyValues">Business-key values keyed by the hub metadata business-key names.</param>
  public DataVaultHubSaveOperation(
      DataVaultHubMetadata metadata,
      IEnumerable<KeyValuePair<string, string>> businessKeyValues) {
    ArgumentNullException.ThrowIfNull(metadata);

    Metadata = metadata;
    BusinessKeyValues = RequireValues(businessKeyValues, nameof(businessKeyValues));
  }

  /// <summary>
  /// Gets the hub metadata declaration that owns the target table and business-key shape.
  /// </summary>
  public DataVaultHubMetadata Metadata { get; }

  /// <summary>
  /// Gets business-key values keyed by the hub metadata business-key names.
  /// </summary>
  public IReadOnlyDictionary<string, string> BusinessKeyValues { get; }

  internal static IReadOnlyDictionary<string, string> RequireValues(
      IEnumerable<KeyValuePair<string, string>> values,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(values);

    var valueMap = new Dictionary<string, string>(StringComparer.Ordinal);
    foreach (var value in values) {
      ArgumentException.ThrowIfNullOrWhiteSpace(value.Key, parameterName);
      if (value.Value is null) {
        throw new ArgumentException("Data Vault save values must not contain null values.", parameterName);
      }

      if (!valueMap.TryAdd(value.Key, value.Value)) {
        throw new ArgumentException("Data Vault save values must not contain duplicate names.", parameterName);
      }
    }

    return new ReadOnlyDictionary<string, string>(valueMap);
  }
}
