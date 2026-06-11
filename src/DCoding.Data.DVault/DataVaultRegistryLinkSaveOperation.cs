using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one link row whose metadata should be resolved from the authoritative registry by logical link name.
/// </summary>
public sealed class DataVaultRegistryLinkSaveOperation {
  /// <summary>
  /// Initializes a new registry-backed link save operation.
  /// </summary>
  /// <param name="linkName">The exact logical link metadata name to resolve from the authoritative registry.</param>
  /// <param name="participantHashKeyValues">Participant hash keys keyed by the resolved link produced participant names.</param>
  public DataVaultRegistryLinkSaveOperation(
      string linkName,
      IEnumerable<KeyValuePair<string, string>> participantHashKeyValues) {
    LinkName = DataVaultMetadataValidation.RequireName(linkName, nameof(linkName));
    ParticipantHashKeyValues = DataVaultHubSaveOperation.RequireValues(
        participantHashKeyValues,
        nameof(participantHashKeyValues));
  }

  /// <summary>
  /// Gets the exact logical link metadata name to resolve from the authoritative registry.
  /// </summary>
  public string LinkName { get; }

  /// <summary>
  /// Gets participant hash keys keyed by the resolved link produced participant names.
  /// </summary>
  public IReadOnlyDictionary<string, string> ParticipantHashKeyValues { get; }
}
