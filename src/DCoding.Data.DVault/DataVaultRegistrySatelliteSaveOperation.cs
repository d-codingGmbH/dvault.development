using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one satellite row whose metadata should be resolved from the authoritative registry by parent and logical satellite name.
/// </summary>
public sealed class DataVaultRegistrySatelliteSaveOperation {
  /// <summary>
  /// Initializes a new registry-backed satellite save operation.
  /// </summary>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKey">The explicit parent hub or link hash key associated with this satellite row.</param>
  /// <param name="payloadValues">Payload values keyed by the resolved satellite metadata payload names.</param>
  /// <param name="hashDiff">The caller-supplied deterministic hash diff for this payload state.</param>
  public DataVaultRegistrySatelliteSaveOperation(
      DataVaultMetadataReference parent,
      string satelliteName,
      string parentHashKey,
      IEnumerable<KeyValuePair<string, string>> payloadValues,
      string hashDiff)
      : this(parent, satelliteName, parentHashKey, [], payloadValues, hashDiff) {
  }

  /// <summary>
  /// Initializes a new registry-backed multi-active satellite save operation.
  /// </summary>
  /// <param name="parent">The exact parent hub or link metadata reference used to resolve the satellite.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKey">The explicit parent hub or link hash key associated with this satellite row.</param>
  /// <param name="drivingKeyValues">Driving-key values keyed by the resolved satellite metadata driving-key names.</param>
  /// <param name="payloadValues">Payload values keyed by the resolved satellite metadata payload names.</param>
  /// <param name="hashDiff">The caller-supplied deterministic hash diff for this payload state.</param>
  public DataVaultRegistrySatelliteSaveOperation(
      DataVaultMetadataReference parent,
      string satelliteName,
      string parentHashKey,
      IEnumerable<KeyValuePair<string, string>> drivingKeyValues,
      IEnumerable<KeyValuePair<string, string>> payloadValues,
      string hashDiff) {
    ArgumentNullException.ThrowIfNull(parent);
    ArgumentException.ThrowIfNullOrWhiteSpace(parentHashKey);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashDiff);

    Parent = parent;
    SatelliteName = DataVaultMetadataValidation.RequireName(satelliteName, nameof(satelliteName));
    ParentHashKey = parentHashKey;
    DrivingKeyValues = DataVaultHubSaveOperation.RequireValues(drivingKeyValues, nameof(drivingKeyValues));
    PayloadValues = DataVaultHubSaveOperation.RequireValues(payloadValues, nameof(payloadValues));
    HashDiff = hashDiff;
  }

  /// <summary>
  /// Gets the exact parent hub or link metadata reference used to resolve the satellite.
  /// </summary>
  public DataVaultMetadataReference Parent { get; }

  /// <summary>
  /// Gets the exact logical satellite metadata name to resolve from the authoritative registry.
  /// </summary>
  public string SatelliteName { get; }

  /// <summary>
  /// Gets the explicit parent hub or link hash key associated with this satellite row.
  /// </summary>
  public string ParentHashKey { get; }

  /// <summary>
  /// Gets driving-key values keyed by the resolved satellite metadata driving-key names.
  /// </summary>
  public IReadOnlyDictionary<string, string> DrivingKeyValues { get; }

  /// <summary>
  /// Gets payload values keyed by the resolved satellite metadata payload names.
  /// </summary>
  public IReadOnlyDictionary<string, string> PayloadValues { get; }

  /// <summary>
  /// Gets the caller-supplied deterministic hash diff for this payload state.
  /// </summary>
  public string HashDiff { get; }
}
