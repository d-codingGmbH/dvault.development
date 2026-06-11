using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Groups explicit DVault save operations that share one load timestamp and record source.
/// </summary>
public sealed class DataVaultSaveRequest {
  /// <summary>
  /// Initializes a new explicit save request.
  /// </summary>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="hubOperations">The hub rows to persist before link and satellite rows.</param>
  /// <param name="linkOperations">The link rows to persist after hub rows and before satellite rows.</param>
  public DataVaultSaveRequest(
      DateTimeOffset loadTimestamp,
      string recordSource,
      IEnumerable<DataVaultHubSaveOperation> hubOperations,
      IEnumerable<DataVaultLinkSaveOperation> linkOperations)
      : this(loadTimestamp, recordSource, hubOperations, linkOperations, []) {
  }

  /// <summary>
  /// Initializes a new explicit save request.
  /// </summary>
  /// <param name="loadTimestamp">The caller-visible load timestamp to persist as UTC metadata.</param>
  /// <param name="recordSource">The caller-visible record source to persist as lineage metadata.</param>
  /// <param name="hubOperations">The hub rows to persist before link and satellite rows.</param>
  /// <param name="linkOperations">The link rows to persist after hub rows and before satellite rows.</param>
  /// <param name="satelliteOperations">The satellite rows to persist after hub and link rows.</param>
  public DataVaultSaveRequest(
      DateTimeOffset loadTimestamp,
      string recordSource,
      IEnumerable<DataVaultHubSaveOperation> hubOperations,
      IEnumerable<DataVaultLinkSaveOperation> linkOperations,
      IEnumerable<DataVaultSatelliteSaveOperation> satelliteOperations) {
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
  /// Gets the hub rows to persist before link and satellite rows.
  /// </summary>
  public IReadOnlyList<DataVaultHubSaveOperation> HubOperations { get; }

  /// <summary>
  /// Gets the link rows to persist after hub rows and before satellite rows.
  /// </summary>
  public IReadOnlyList<DataVaultLinkSaveOperation> LinkOperations { get; }

  /// <summary>
  /// Gets the satellite rows to persist after hub and link rows.
  /// </summary>
  public IReadOnlyList<DataVaultSatelliteSaveOperation> SatelliteOperations { get; }

  private static IReadOnlyList<T> RequireOperations<T>(IEnumerable<T> operations, string parameterName)
      where T : class {
    var values = operations.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault save operation collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}
