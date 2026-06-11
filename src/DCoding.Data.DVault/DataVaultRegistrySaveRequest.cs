using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Groups registry-backed DVault save operations that share one load timestamp and record source.
/// </summary>
public sealed class DataVaultRegistrySaveRequest {
  /// <summary>
  /// Initializes a new registry-backed save request.
  /// </summary>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="hubOperations">The hub rows whose metadata should be resolved by logical hub name.</param>
  /// <param name="linkOperations">The link rows whose metadata should be resolved by logical link name.</param>
  public DataVaultRegistrySaveRequest(
      DateTimeOffset loadTimestamp,
      string recordSource,
      IEnumerable<DataVaultRegistryHubSaveOperation> hubOperations,
      IEnumerable<DataVaultRegistryLinkSaveOperation> linkOperations)
      : this(loadTimestamp, recordSource, hubOperations, linkOperations, []) {
  }

  /// <summary>
  /// Initializes a new registry-backed save request.
  /// </summary>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="hubOperations">The hub rows whose metadata should be resolved by logical hub name.</param>
  /// <param name="linkOperations">The link rows whose metadata should be resolved by logical link name.</param>
  /// <param name="satelliteOperations">The satellite rows whose metadata should be resolved by parent and logical satellite name.</param>
  public DataVaultRegistrySaveRequest(
      DateTimeOffset loadTimestamp,
      string recordSource,
      IEnumerable<DataVaultRegistryHubSaveOperation> hubOperations,
      IEnumerable<DataVaultRegistryLinkSaveOperation> linkOperations,
      IEnumerable<DataVaultRegistrySatelliteSaveOperation> satelliteOperations) {
    ArgumentException.ThrowIfNullOrWhiteSpace(recordSource);
    ArgumentNullException.ThrowIfNull(hubOperations);
    ArgumentNullException.ThrowIfNull(linkOperations);
    ArgumentNullException.ThrowIfNull(satelliteOperations);

    LoadTimestamp = loadTimestamp.ToUniversalTime();
    RecordSource = recordSource;
    HubOperations = RequireOperations(hubOperations, nameof(hubOperations));
    LinkOperations = RequireOperations(linkOperations, nameof(linkOperations));
    SatelliteOperations = RequireOperations(satelliteOperations, nameof(satelliteOperations));
  }

  /// <summary>
  /// Gets the caller-supplied load timestamp normalized to a UTC instant.
  /// </summary>
  public DateTimeOffset LoadTimestamp { get; }

  /// <summary>
  /// Gets the caller-supplied record source used for every operation in the request.
  /// </summary>
  public string RecordSource { get; }

  /// <summary>
  /// Gets the hub rows whose metadata should be resolved by logical hub name before the explicit save pipeline runs.
  /// </summary>
  public IReadOnlyList<DataVaultRegistryHubSaveOperation> HubOperations { get; }

  /// <summary>
  /// Gets the link rows whose metadata should be resolved by logical link name before the explicit save pipeline runs.
  /// </summary>
  public IReadOnlyList<DataVaultRegistryLinkSaveOperation> LinkOperations { get; }

  /// <summary>
  /// Gets the satellite rows whose metadata should be resolved by parent and logical satellite name before the explicit save pipeline runs.
  /// </summary>
  public IReadOnlyList<DataVaultRegistrySatelliteSaveOperation> SatelliteOperations { get; }

  private static IReadOnlyList<T> RequireOperations<T>(IEnumerable<T> operations, string parameterName)
      where T : class {
    var values = operations.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault registry save operation collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}
