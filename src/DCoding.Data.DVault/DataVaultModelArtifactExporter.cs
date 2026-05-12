using System.Text;
using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Exports already-materialized Data Vault metadata to provider-neutral strict JSON <c>dvault.model.v1</c> artifacts.
/// </summary>
/// <remarks>
/// The exporter accepts metadata models and registries only. It does not accept raw Code-First fluent declarations,
/// Entity Framework <c>ModelBuilder</c> state, or runtime save/read state. Legacy <c>PointInTimeTables</c> metadata is
/// not part of the <c>dvault.model.v1</c> artifact contract and is rejected deterministically.
/// </remarks>
public static class DataVaultModelArtifactExporter {
  private const string SchemaVersion = "dvault.model.v1";
  private const string NamingPolicy = "default";
  private const string ProviderDefaultLoadTimestampStorage = "provider-default";
  private const string Iso8601LoadTimestampStorage = "iso-8601-utc-text";
  private const string UtcTicksLoadTimestampStorage = "utc-ticks";
  private const string Iso8601ProfileSuffix = "-loadts-iso8601";
  private const string UtcTicksProfileSuffix = "-loadts-utc-ticks";

  private static readonly JsonWriterOptions WriterOptions = new() {
    Indented = true,
  };

  private static readonly HashSet<string> BuiltInProviderDefaultProfileNames = new(StringComparer.Ordinal) {
      DataVaultProviderCapabilityProfiles.Sqlite.ProfileName,
      DataVaultProviderCapabilityProfiles.Oracle.ProfileName,
      DataVaultProviderCapabilityProfiles.Postgres.ProfileName,
      DataVaultProviderCapabilityProfiles.SqlServer.ProfileName,
      DataVaultProviderCapabilityProfiles.MySql.ProfileName,
  };

  /// <summary>
  /// Exports an existing metadata registry to deterministic strict JSON matching the <c>dvault.model.v1</c> contract.
  /// </summary>
  /// <param name="metadataRegistry">The already-materialized metadata registry to export.</param>
  /// <returns>The deterministic JSON artifact.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="metadataRegistry" /> is <see langword="null" />.</exception>
  /// <exception cref="NotSupportedException">
  /// The registry contains legacy <c>PointInTimeTables</c> metadata or another metadata shape that cannot be represented
  /// by <c>dvault.model.v1</c>.
  /// </exception>
  public static string ExportJson(DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    ValidateNoLegacyPointInTimeTables(metadataRegistry.PointInTimeTables);
    ValidateLinkParticipants(metadataRegistry.Links);
    ValidateBridgeEndpoints(metadataRegistry.Links, metadataRegistry.Bridges);

    return ExportJson(
        metadataRegistry.Hubs,
        metadataRegistry.Links,
        metadataRegistry.Satellites,
        metadataRegistry.Pits,
        metadataRegistry.Bridges,
        InferLoadTimestampStorage(metadataRegistry.ProviderCapabilityProfiles));
  }

  /// <summary>
  /// Exports an existing metadata model to deterministic strict JSON matching the <c>dvault.model.v1</c> contract.
  /// </summary>
  /// <param name="metadataModel">The already-materialized metadata model to export.</param>
  /// <returns>The deterministic JSON artifact.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="metadataModel" /> is <see langword="null" />.</exception>
  /// <exception cref="NotSupportedException">
  /// The model contains legacy <c>PointInTimeTables</c> metadata or another metadata shape that cannot be represented
  /// by <c>dvault.model.v1</c>.
  /// </exception>
  public static string ExportJson(DataVaultMetadataModel metadataModel) {
    ArgumentNullException.ThrowIfNull(metadataModel);

    ValidateNoLegacyPointInTimeTables(metadataModel.PointInTimeTables);

    var registry = DataVaultMetadataRegistry.Create(metadataModel);

    ValidateLinkParticipants(registry.Links);
    ValidateBridgeEndpoints(registry.Links, registry.Bridges);

    return ExportJson(
        registry.Hubs,
        registry.Links,
        registry.Satellites,
        registry.Pits,
        registry.Bridges,
        DataVaultLoadTimestampStorage.ProviderDefault);
  }

  private static string ExportJson(
      IReadOnlyList<DataVaultHubMetadata> hubs,
      IReadOnlyList<DataVaultLinkMetadata> links,
      IReadOnlyList<DataVaultSatelliteMetadata> satellites,
      IReadOnlyList<DataVaultPitMetadata> pits,
      IReadOnlyList<DataVaultBridgeMetadata> bridges,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    using var stream = new MemoryStream();
    using (var writer = new Utf8JsonWriter(stream, WriterOptions)) {
      writer.WriteStartObject();
      writer.WriteString("schemaVersion", SchemaVersion);
      WriteNaming(writer);
      writer.WriteString("loadTimestampStorage", GetLoadTimestampStorageToken(loadTimestampStorage));
      WriteHubs(writer, hubs);
      WriteLinks(writer, links);
      WriteSatellites(writer, satellites);
      WritePits(writer, pits);
      WriteBridges(writer, bridges);
      writer.WriteEndObject();
    }

    return Encoding.UTF8.GetString(stream.ToArray()) + "\n";
  }

  private static void WriteNaming(Utf8JsonWriter writer) {
    writer.WritePropertyName("naming");
    writer.WriteStartObject();
    writer.WriteString("policy", NamingPolicy);
    writer.WriteEndObject();
  }

  private static void WriteHubs(
      Utf8JsonWriter writer,
      IEnumerable<DataVaultHubMetadata> hubs) {
    writer.WritePropertyName("hubs");
    writer.WriteStartArray();
    foreach (var hub in hubs) {
      writer.WriteStartObject();
      writer.WriteString("name", hub.Name);
      WriteStringArray(writer, "businessKeys", hub.BusinessKeyNames);
      writer.WriteEndObject();
    }

    writer.WriteEndArray();
  }

  private static void WriteLinks(
      Utf8JsonWriter writer,
      IEnumerable<DataVaultLinkMetadata> links) {
    writer.WritePropertyName("links");
    writer.WriteStartArray();
    foreach (var link in links) {
      writer.WriteStartObject();
      writer.WriteString("name", link.Name);
      writer.WritePropertyName("participants");
      writer.WriteStartArray();
      foreach (var participant in link.Participants) {
        writer.WriteStartObject();
        writer.WriteString("hub", participant.HubReference.Name);
        if (!string.Equals(participant.SourceEndpointName, participant.HubReference.Name, StringComparison.Ordinal)) {
          writer.WriteString("role", participant.SourceEndpointName);
        }

        writer.WriteEndObject();
      }

      writer.WriteEndArray();
      writer.WriteEndObject();
    }

    writer.WriteEndArray();
  }

  private static void WriteSatellites(
      Utf8JsonWriter writer,
      IEnumerable<DataVaultSatelliteMetadata> satellites) {
    writer.WritePropertyName("satellites");
    writer.WriteStartArray();
    foreach (var satellite in satellites) {
      writer.WriteStartObject();
      writer.WriteString("name", satellite.Name);
      writer.WritePropertyName("parent");
      writer.WriteStartObject();
      writer.WriteString("kind", GetParentKindToken(satellite.Parent.Kind));
      writer.WriteString("name", satellite.Parent.Name);
      writer.WriteEndObject();
      WriteStringArray(writer, "payload", satellite.DescriptiveAttributeNames);
      WriteStringArray(writer, "drivingKeys", satellite.DrivingKeyNames);
      writer.WriteEndObject();
    }

    writer.WriteEndArray();
  }

  private static void WritePits(
      Utf8JsonWriter writer,
      IEnumerable<DataVaultPitMetadata> pits) {
    writer.WritePropertyName("pits");
    writer.WriteStartArray();
    foreach (var pit in pits) {
      writer.WriteStartObject();
      writer.WriteString("name", pit.Name);
      writer.WriteString("hub", pit.Parent.Name);
      WriteStringArray(writer, "satellites", pit.Satellites.Select(satellite => satellite.SatelliteName));
      writer.WriteEndObject();
    }

    writer.WriteEndArray();
  }

  private static void WriteBridges(
      Utf8JsonWriter writer,
      IEnumerable<DataVaultBridgeMetadata> bridges) {
    writer.WritePropertyName("bridges");
    writer.WriteStartArray();
    foreach (var bridge in bridges) {
      writer.WriteStartObject();
      writer.WriteString("name", bridge.Name);
      writer.WriteString("kind", GetBridgeKindToken(bridge.Kind));
      writer.WriteString("source", bridge.Source.Name);
      writer.WritePropertyName("endpoints");
      writer.WriteStartObject();
      foreach (var endpointRole in GetBridgeEndpointRoles(bridge.Kind)) {
        var endpoint = bridge.Endpoints.Single(current => current.Role == endpointRole);
        writer.WritePropertyName(GetBridgeEndpointName(endpointRole));
        writer.WriteStartObject();
        writer.WriteString("hub", endpoint.HubReference.Name);
        if (!string.Equals(endpoint.SourceEndpointName, endpoint.HubReference.Name, StringComparison.Ordinal)) {
          writer.WriteString("role", endpoint.SourceEndpointName);
        }

        writer.WriteEndObject();
      }

      writer.WriteEndObject();
      writer.WriteEndObject();
    }

    writer.WriteEndArray();
  }

  private static void WriteStringArray(
      Utf8JsonWriter writer,
      string propertyName,
      IEnumerable<string> values) {
    writer.WritePropertyName(propertyName);
    writer.WriteStartArray();
    foreach (var value in values) {
      writer.WriteStringValue(value);
    }

    writer.WriteEndArray();
  }

  private static DataVaultLoadTimestampStorage InferLoadTimestampStorage(
      IReadOnlyList<DataVaultProviderCapabilityProfile> providerCapabilityProfiles) {
    if (providerCapabilityProfiles.Count == 0 ||
        providerCapabilityProfiles.All(IsBuiltInProviderDefaultProfile)) {
      return DataVaultLoadTimestampStorage.ProviderDefault;
    }

    if (providerCapabilityProfiles.All(profile => profile.ProfileName.EndsWith(UtcTicksProfileSuffix, StringComparison.Ordinal))) {
      return DataVaultLoadTimestampStorage.UtcTicks;
    }

    if (providerCapabilityProfiles.All(profile => profile.ProfileName.EndsWith(Iso8601ProfileSuffix, StringComparison.Ordinal))) {
      return DataVaultLoadTimestampStorage.Iso8601UtcText;
    }

    if (providerCapabilityProfiles.All(IsUtcTicksLoadTimestampProfile)) {
      return DataVaultLoadTimestampStorage.UtcTicks;
    }

    if (providerCapabilityProfiles.All(profile => !IsBuiltInProviderDefaultProfile(profile)) &&
        providerCapabilityProfiles.All(IsIso8601LoadTimestampProfile)) {
      return DataVaultLoadTimestampStorage.Iso8601UtcText;
    }

    throw new NotSupportedException(
        "ProviderCapabilityProfiles do not map to one supported dvault.model.v1 loadTimestampStorage token. " +
        "Use provider-default, iso-8601-utc-text, or utc-ticks compatible profiles before export.");
  }

  private static bool IsBuiltInProviderDefaultProfile(DataVaultProviderCapabilityProfile profile) {
    return BuiltInProviderDefaultProfileNames.Contains(profile.ProfileName);
  }

  private static bool IsUtcTicksLoadTimestampProfile(DataVaultProviderCapabilityProfile profile) {
    return HasLoadTimestampValueFormat(profile, DataVaultProviderValueFormat.UtcTicks);
  }

  private static bool IsIso8601LoadTimestampProfile(DataVaultProviderCapabilityProfile profile) {
    return HasLoadTimestampValueFormat(profile, DataVaultProviderValueFormat.Iso8601UtcText);
  }

  private static bool HasLoadTimestampValueFormat(
      DataVaultProviderCapabilityProfile profile,
      DataVaultProviderValueFormat valueFormat) {
    return TryGetValueFormat(profile, DataVaultLogicalPropertyKind.LoadTimestamp, out var loadTimestampFormat) &&
        TryGetValueFormat(profile, DataVaultLogicalPropertyKind.SatelliteSnapshotReference, out var snapshotFormat) &&
        loadTimestampFormat == valueFormat &&
        snapshotFormat == valueFormat;
  }

  private static bool TryGetValueFormat(
      DataVaultProviderCapabilityProfile profile,
      DataVaultLogicalPropertyKind logicalPropertyKind,
      out DataVaultProviderValueFormat valueFormat) {
    try {
      valueFormat = profile.GetRequiredTypeMapping(logicalPropertyKind).ValueFormat;
      return true;
    }
    catch (NotSupportedException) {
      valueFormat = default;
      return false;
    }
  }

  private static string GetLoadTimestampStorageToken(DataVaultLoadTimestampStorage loadTimestampStorage) {
    return loadTimestampStorage switch {
      DataVaultLoadTimestampStorage.ProviderDefault => ProviderDefaultLoadTimestampStorage,
      DataVaultLoadTimestampStorage.Iso8601UtcText => Iso8601LoadTimestampStorage,
      DataVaultLoadTimestampStorage.UtcTicks => UtcTicksLoadTimestampStorage,
      _ => throw new ArgumentOutOfRangeException(
          nameof(loadTimestampStorage),
          loadTimestampStorage,
          "Unsupported Data Vault load timestamp storage."),
    };
  }

  private static string GetParentKindToken(DataVaultMetadataReferenceKind kind) {
    return kind switch {
      DataVaultMetadataReferenceKind.Hub => "hub",
      DataVaultMetadataReferenceKind.Link => "link",
      _ => throw new NotSupportedException(
          "Metadata reference kind '" +
          kind +
          "' is not serializable to dvault.model.v1 satellite parent references."),
    };
  }

  private static string GetBridgeKindToken(DataVaultBridgeKind kind) {
    return kind switch {
      DataVaultBridgeKind.ManyToMany => "many-to-many",
      DataVaultBridgeKind.Hierarchy => "hierarchy",
      _ => throw new NotSupportedException(
          "Bridge kind '" + kind + "' is not serializable to dvault.model.v1 bridges."),
    };
  }

  private static DataVaultBridgeEndpointRole[] GetBridgeEndpointRoles(DataVaultBridgeKind kind) {
    return kind switch {
      DataVaultBridgeKind.ManyToMany =>
      [
          DataVaultBridgeEndpointRole.From,
          DataVaultBridgeEndpointRole.To,
      ],
      DataVaultBridgeKind.Hierarchy =>
      [
          DataVaultBridgeEndpointRole.Ancestor,
          DataVaultBridgeEndpointRole.Descendant,
      ],
      _ => throw new NotSupportedException(
          "Bridge kind '" + kind + "' is not serializable to dvault.model.v1 bridges."),
    };
  }

  private static string GetBridgeEndpointName(DataVaultBridgeEndpointRole role) {
    return role switch {
      DataVaultBridgeEndpointRole.From => "from",
      DataVaultBridgeEndpointRole.To => "to",
      DataVaultBridgeEndpointRole.Ancestor => "ancestor",
      DataVaultBridgeEndpointRole.Descendant => "descendant",
      _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported bridge endpoint role."),
    };
  }

  private static void ValidateNoLegacyPointInTimeTables(
      IReadOnlyList<DataVaultPointInTimeMetadata> pointInTimeTables) {
    if (pointInTimeTables.Count == 0) {
      return;
    }

    throw new NotSupportedException(
        "Legacy PointInTimeTables metadata is not serializable to dvault.model.v1 because the artifact contract " +
        "defines pits instead of a pointInTimeTables surface. Remove or migrate PointInTimeTables before export. " +
        "First unsupported PointInTimeTables entry: '" +
        pointInTimeTables[0].Name +
        "'.");
  }

  private static void ValidateLinkParticipants(IReadOnlyList<DataVaultLinkMetadata> links) {
    foreach (var link in links) {
      foreach (var group in link.Participants.GroupBy(participant => participant.HubReference.Name, StringComparer.Ordinal)) {
        if (group.Count() > 1 && group.Any(participant =>
            string.Equals(participant.SourceEndpointName, participant.HubReference.Name, StringComparison.Ordinal))) {
          throw new NotSupportedException(
              "Link '" +
              link.Name +
              "' repeats hub '" +
              group.Key +
              "' without role-bearing participant metadata. dvault.model.v1 repeated hub participants require roles.");
        }
      }
    }
  }

  private static void ValidateBridgeEndpoints(
      IReadOnlyList<DataVaultLinkMetadata> links,
      IReadOnlyList<DataVaultBridgeMetadata> bridges) {
    var linksByName = links.ToDictionary(link => link.Name, StringComparer.Ordinal);
    foreach (var bridge in bridges) {
      if (!linksByName.TryGetValue(bridge.Source.Name, out var sourceLink)) {
        continue;
      }

      foreach (var endpoint in bridge.Endpoints) {
        var matchingParticipants = sourceLink.Participants
            .Where(participant => string.Equals(
                participant.HubReference.Name,
                endpoint.HubReference.Name,
                StringComparison.Ordinal))
            .ToArray();
        if (matchingParticipants.Length > 1 &&
            string.Equals(endpoint.SourceEndpointName, endpoint.HubReference.Name, StringComparison.Ordinal)) {
          throw new NotSupportedException(
              "Bridge '" +
              bridge.Name +
              "' endpoint for hub '" +
              endpoint.HubReference.Name +
              "' lacks role-bearing participant metadata. dvault.model.v1 bridge endpoints require roles when " +
              "the source link participant hub is ambiguous.");
        }
      }
    }
  }
}
