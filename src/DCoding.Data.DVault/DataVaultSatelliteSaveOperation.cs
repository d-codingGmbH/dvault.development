using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one satellite row to persist through the explicit DVault save service.
/// </summary>
public sealed class DataVaultSatelliteSaveOperation {
  /// <summary>
  /// Initializes a new satellite save operation.
  /// </summary>
  /// <param name="metadata">The satellite metadata declaration that owns the target table and payload shape.</param>
  /// <param name="parentHashKey">The explicit parent hub or link hash key associated with this satellite row.</param>
  /// <param name="payloadValues">Payload values keyed by the satellite metadata payload names.</param>
  /// <param name="hashDiff">The caller-supplied deterministic hash diff for this payload state.</param>
  public DataVaultSatelliteSaveOperation(
      DataVaultSatelliteMetadata metadata,
      string parentHashKey,
      IEnumerable<KeyValuePair<string, string>> payloadValues,
      string hashDiff)
      : this(metadata, parentHashKey, [], payloadValues, hashDiff) {
  }

  /// <summary>
  /// Initializes a new multi-active satellite save operation.
  /// </summary>
  /// <param name="metadata">The satellite metadata declaration that owns the target table and payload shape.</param>
  /// <param name="parentHashKey">The explicit parent hub or link hash key associated with this satellite row.</param>
  /// <param name="drivingKeyValues">Driving-key values keyed by the satellite metadata driving-key names.</param>
  /// <param name="payloadValues">Payload values keyed by the satellite metadata payload names.</param>
  /// <param name="hashDiff">The caller-supplied deterministic hash diff for this payload state.</param>
  public DataVaultSatelliteSaveOperation(
      DataVaultSatelliteMetadata metadata,
      string parentHashKey,
      IEnumerable<KeyValuePair<string, string>> drivingKeyValues,
      IEnumerable<KeyValuePair<string, string>> payloadValues,
      string hashDiff) {
    ArgumentNullException.ThrowIfNull(metadata);
    ArgumentException.ThrowIfNullOrWhiteSpace(parentHashKey);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashDiff);

    Metadata = metadata;
    ParentHashKey = parentHashKey;
    DrivingKeyValues = RequireDrivingKeyValues(metadata, drivingKeyValues);
    PayloadValues = DataVaultHubSaveOperation.RequireValues(payloadValues, nameof(payloadValues));
    HashDiff = hashDiff;
  }

  /// <summary>
  /// Gets the satellite metadata declaration that owns the target table and payload shape.
  /// </summary>
  public DataVaultSatelliteMetadata Metadata { get; }

  /// <summary>
  /// Gets the explicit parent hub or link hash key associated with this satellite row.
  /// </summary>
  public string ParentHashKey { get; }

  /// <summary>
  /// Gets driving-key values keyed by the satellite metadata driving-key names.
  /// </summary>
  public IReadOnlyDictionary<string, string> DrivingKeyValues { get; }

  /// <summary>
  /// Gets payload values keyed by the satellite metadata payload names.
  /// </summary>
  public IReadOnlyDictionary<string, string> PayloadValues { get; }

  /// <summary>
  /// Gets the caller-supplied deterministic hash diff for this payload state.
  /// </summary>
  public string HashDiff { get; }

  private static IReadOnlyDictionary<string, string> RequireDrivingKeyValues(
      DataVaultSatelliteMetadata metadata,
      IEnumerable<KeyValuePair<string, string>> drivingKeyValues) {
    var values = DataVaultHubSaveOperation.RequireValues(drivingKeyValues, nameof(drivingKeyValues));
    var declaredNames = metadata.DrivingKeyNames.ToHashSet(StringComparer.Ordinal);

    foreach (var drivingKeyName in metadata.DrivingKeyNames) {
      if (!values.ContainsKey(drivingKeyName)) {
        throw new ArgumentException(
            "The Data Vault satellite save operation is missing required driving-key value '" + drivingKeyName + "'.",
            nameof(drivingKeyValues));
      }
    }

    foreach (var suppliedName in values.Keys) {
      if (!declaredNames.Contains(suppliedName)) {
        throw new ArgumentException(
            "The Data Vault satellite save operation contains unexpected driving-key value '" + suppliedName + "'.",
            nameof(drivingKeyValues));
      }
    }

    return values;
  }
}
