using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one link row to persist through the explicit DVault save service.
/// </summary>
public sealed class DataVaultLinkSaveOperation {
  /// <summary>
  /// Initializes a new link save operation.
  /// </summary>
  /// <param name="metadata">The link metadata declaration that owns the target table and participant shape.</param>
  /// <param name="participantHashKeyValues">Participant hash keys keyed by the produced participant names.</param>
  public DataVaultLinkSaveOperation(
      DataVaultLinkMetadata metadata,
      IEnumerable<KeyValuePair<string, string>> participantHashKeyValues) {
    ArgumentNullException.ThrowIfNull(metadata);

    Metadata = metadata;
    ParticipantHashKeyValues = DataVaultHubSaveOperation.RequireValues(
        participantHashKeyValues,
        nameof(participantHashKeyValues));
  }

  /// <summary>
  /// Gets the link metadata declaration that owns the target table and participant shape.
  /// </summary>
  public DataVaultLinkMetadata Metadata { get; }

  /// <summary>
  /// Gets participant hash keys keyed by the produced participant names.
  /// </summary>
  public IReadOnlyDictionary<string, string> ParticipantHashKeyValues { get; }
}
