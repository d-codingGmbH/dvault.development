using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal static class DataVaultModelArtifactParser {
  private const string ExpectedSchemaVersion = "dvault.model.v1";
  private const string SeverityError = "error";

  private static readonly JsonDocumentOptions DocumentOptions = new() {
    AllowTrailingCommas = false,
    CommentHandling = JsonCommentHandling.Disallow,
    MaxDepth = 64,
  };

  private static readonly HashSet<string> TopLevelProperties = new(StringComparer.Ordinal) {
      "schemaVersion",
      "naming",
      "loadTimestampStorage",
      "hubs",
      "links",
      "satellites",
      "pits",
      "bridges",
  };

  private static readonly HashSet<string> NamingProperties = new(StringComparer.Ordinal) {
      "policy",
  };

  private static readonly HashSet<string> HubProperties = new(StringComparer.Ordinal) {
      "name",
      "businessKeys",
  };

  private static readonly HashSet<string> LinkProperties = new(StringComparer.Ordinal) {
      "name",
      "participants",
  };

  private static readonly HashSet<string> LinkParticipantProperties = new(StringComparer.Ordinal) {
      "hub",
      "role",
  };

  private static readonly HashSet<string> SatelliteProperties = new(StringComparer.Ordinal) {
      "name",
      "parent",
      "payload",
      "drivingKeys",
  };

  private static readonly HashSet<string> ParentProperties = new(StringComparer.Ordinal) {
      "kind",
      "name",
  };

  private static readonly HashSet<string> PitProperties = new(StringComparer.Ordinal) {
      "name",
      "hub",
      "satellites",
  };

  private static readonly HashSet<string> BridgeProperties = new(StringComparer.Ordinal) {
      "name",
      "kind",
      "source",
      "endpoints",
  };

  private static readonly HashSet<string> ManyToManyEndpointProperties = new(StringComparer.Ordinal) {
      "from",
      "to",
  };

  private static readonly HashSet<string> HierarchyEndpointProperties = new(StringComparer.Ordinal) {
      "ancestor",
      "descendant",
  };

  private static readonly HashSet<string> BridgeEndpointProperties = new(StringComparer.Ordinal) {
      "hub",
      "role",
  };

  private static readonly HashSet<string> ProviderSpecificFieldNames = new(StringComparer.Ordinal) {
      "provider",
      "providers",
      "providerOptions",
      "nativeStoreType",
      "storeType",
      "columnType",
      "sql",
  };

  public static DataVaultModelArtifactParseResult Parse(string json) {
    ArgumentNullException.ThrowIfNull(json);

    var diagnostics = new List<DataVaultModelArtifactDiagnostic>();
    JsonDocument document;
    try {
      document = JsonDocument.Parse(json, DocumentOptions);
    }
    catch (JsonException exception) {
      diagnostics.Add(new DataVaultModelArtifactDiagnostic(
          SeverityError,
          "shape",
          "DMV1102",
          "The artifact must be strict JSON: " + exception.Message,
          exception.Path ?? string.Empty));
      return CreateFailure(diagnostics);
    }

    using (document) {
      var root = document.RootElement;
      if (root.ValueKind != JsonValueKind.Object) {
        AddIssue(
            diagnostics,
            "shape",
            "DMV1102",
            "The artifact root must be a JSON object.",
            string.Empty);
        return CreateFailure(diagnostics);
      }

      ValidateKnownProperties(root, TopLevelProperties, string.Empty, diagnostics);

      var schemaVersion = ReadSchemaVersion(root, diagnostics);
      var naming = ReadNaming(root, diagnostics);
      var loadTimestampStorage = ReadLoadTimestampStorage(root, diagnostics);
      var hubs = ReadHubs(root, diagnostics);
      var links = ReadLinks(root, diagnostics);
      var satellites = ReadSatellites(root, diagnostics);
      var pits = ReadPits(root, diagnostics);
      var bridges = ReadBridges(root, diagnostics);

      var artifact = new DataVaultModelArtifact(
          schemaVersion,
          naming,
          loadTimestampStorage,
          hubs,
          links,
          satellites,
          pits,
          bridges);

      ValidateArtifact(artifact, diagnostics);
      if (HasErrors(diagnostics)) {
        return CreateFailure(diagnostics);
      }

      try {
        var metadataModel = CreateMetadataModel(artifact);
        var metadataRegistry = DataVaultMetadataRegistry.Create(metadataModel);
        return new DataVaultModelArtifactParseResult(
            artifact,
            metadataModel,
            metadataRegistry,
            diagnostics.ToArray());
      }
      catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
        AddIssue(
            diagnostics,
            "capability",
            "DMV1501",
            "The artifact could not be mapped to the current Data Vault metadata surface: " + exception.Message,
            string.Empty);
        return CreateFailure(diagnostics);
      }
    }
  }

  private static DataVaultModelArtifactParseResult CreateFailure(
      IReadOnlyList<DataVaultModelArtifactDiagnostic> diagnostics) {
    return new DataVaultModelArtifactParseResult(
        Artifact: null,
        MetadataModel: null,
        MetadataRegistry: null,
        diagnostics.ToArray());
  }

  private static string ReadSchemaVersion(
      JsonElement root,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var path = PropertyPath(string.Empty, "schemaVersion");
    if (!root.TryGetProperty("schemaVersion", out var schemaVersion) ||
        schemaVersion.ValueKind != JsonValueKind.String ||
        string.IsNullOrWhiteSpace(schemaVersion.GetString())) {
      AddIssue(
          diagnostics,
          "schema-version",
          "DMV1001",
          "The artifact requires a non-blank string schemaVersion.",
          path);
      return string.Empty;
    }

    var value = schemaVersion.GetString()!;
    if (!string.Equals(value, ExpectedSchemaVersion, StringComparison.Ordinal)) {
      AddIssue(
          diagnostics,
          "schema-version",
          "DMV1002",
          "Unsupported schemaVersion '" + value + "'. Expected '" + ExpectedSchemaVersion + "'.",
          path);
    }

    return value;
  }

  private static DataVaultModelArtifactNaming ReadNaming(
      JsonElement root,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!root.TryGetProperty("naming", out var naming)) {
      return new DataVaultModelArtifactNaming("default");
    }

    var namingPath = PropertyPath(string.Empty, "naming");
    if (naming.ValueKind != JsonValueKind.Object) {
      AddIssue(diagnostics, "shape", "DMV1102", "The naming value must be an object.", namingPath);
      return new DataVaultModelArtifactNaming("default");
    }

    ValidateKnownProperties(naming, NamingProperties, namingPath, diagnostics);
    if (!naming.TryGetProperty("policy", out var policy)) {
      return new DataVaultModelArtifactNaming("default");
    }

    var policyPath = PropertyPath(namingPath, "policy");
    if (policy.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(policy.GetString())) {
      AddIssue(diagnostics, "shape", "DMV1102", "The naming.policy value must be a non-blank string.", policyPath);
      return new DataVaultModelArtifactNaming("default");
    }

    var policyValue = policy.GetString()!;
    if (!string.Equals(policyValue, "default", StringComparison.Ordinal)) {
      AddIssue(
          diagnostics,
          "provider-choice",
          "DMV1502",
          "Unsupported naming.policy '" + policyValue + "'. The only v1 policy is 'default'.",
          policyPath);
    }

    return new DataVaultModelArtifactNaming(policyValue);
  }

  private static DataVaultLoadTimestampStorage ReadLoadTimestampStorage(
      JsonElement root,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!root.TryGetProperty("loadTimestampStorage", out var storage)) {
      return DataVaultLoadTimestampStorage.ProviderDefault;
    }

    var path = PropertyPath(string.Empty, "loadTimestampStorage");
    if (storage.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(storage.GetString())) {
      AddIssue(
          diagnostics,
          "shape",
          "DMV1102",
          "The loadTimestampStorage value must be a non-blank string.",
          path);
      return DataVaultLoadTimestampStorage.ProviderDefault;
    }

    var value = storage.GetString()!;
    return value switch {
      "provider-default" => DataVaultLoadTimestampStorage.ProviderDefault,
      "iso-8601-utc-text" => DataVaultLoadTimestampStorage.Iso8601UtcText,
      "utc-ticks" => DataVaultLoadTimestampStorage.UtcTicks,
      _ => UnsupportedLoadTimestampStorage(value, path, diagnostics),
    };
  }

  private static DataVaultLoadTimestampStorage UnsupportedLoadTimestampStorage(
      string value,
      string path,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    AddIssue(
        diagnostics,
        "provider-choice",
        "DMV1502",
        "Unsupported loadTimestampStorage '" + value + "'.",
        path);
    return DataVaultLoadTimestampStorage.ProviderDefault;
  }

  private static IReadOnlyList<DataVaultModelHubDeclaration> ReadHubs(
      JsonElement root,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!TryReadOptionalArray(root, "hubs", string.Empty, diagnostics, out var hubs)) {
      return Array.Empty<DataVaultModelHubDeclaration>();
    }

    var values = new List<DataVaultModelHubDeclaration>();
    var index = 0;
    foreach (var hub in hubs.EnumerateArray()) {
      var path = IndexPath(PropertyPath(string.Empty, "hubs"), index++);
      if (hub.ValueKind != JsonValueKind.Object) {
        AddIssue(diagnostics, "shape", "DMV1102", "Each hub declaration must be an object.", path);
        continue;
      }

      ValidateKnownProperties(hub, HubProperties, path, diagnostics);
      values.Add(new DataVaultModelHubDeclaration(
          ReadRequiredString(hub, "name", path, "Hub name", diagnostics),
          ReadRequiredStringArray(hub, "businessKeys", path, "A hub requires at least one business key.", "DMV1202", diagnostics),
          path));
    }

    return values;
  }

  private static IReadOnlyList<DataVaultModelLinkDeclaration> ReadLinks(
      JsonElement root,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!TryReadOptionalArray(root, "links", string.Empty, diagnostics, out var links)) {
      return Array.Empty<DataVaultModelLinkDeclaration>();
    }

    var values = new List<DataVaultModelLinkDeclaration>();
    var index = 0;
    foreach (var link in links.EnumerateArray()) {
      var path = IndexPath(PropertyPath(string.Empty, "links"), index++);
      if (link.ValueKind != JsonValueKind.Object) {
        AddIssue(diagnostics, "shape", "DMV1102", "Each link declaration must be an object.", path);
        continue;
      }

      ValidateKnownProperties(link, LinkProperties, path, diagnostics);
      values.Add(new DataVaultModelLinkDeclaration(
          ReadRequiredString(link, "name", path, "Link name", diagnostics),
          ReadLinkParticipants(link, path, diagnostics),
          path));
    }

    return values;
  }

  private static IReadOnlyList<DataVaultModelLinkParticipantDeclaration> ReadLinkParticipants(
      JsonElement link,
      string linkPath,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var participantsPath = PropertyPath(linkPath, "participants");
    if (!TryReadRequiredArray(link, "participants", linkPath, diagnostics, out var participants)) {
      return Array.Empty<DataVaultModelLinkParticipantDeclaration>();
    }

    if (participants.GetArrayLength() < 2) {
      AddIssue(
          diagnostics,
          "shape",
          "DMV1103",
          "A link requires at least two participants.",
          participantsPath);
    }

    var values = new List<DataVaultModelLinkParticipantDeclaration>();
    var index = 0;
    foreach (var participant in participants.EnumerateArray()) {
      var path = IndexPath(participantsPath, index++);
      if (participant.ValueKind != JsonValueKind.Object) {
        AddIssue(diagnostics, "shape", "DMV1102", "Each link participant must be an object.", path);
        continue;
      }

      ValidateKnownProperties(participant, LinkParticipantProperties, path, diagnostics);
      values.Add(new DataVaultModelLinkParticipantDeclaration(
          ReadRequiredString(participant, "hub", path, "Link participant hub", diagnostics),
          ReadOptionalString(participant, "role", path, diagnostics),
          path));
    }

    return values;
  }

  private static IReadOnlyList<DataVaultModelSatelliteDeclaration> ReadSatellites(
      JsonElement root,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!TryReadOptionalArray(root, "satellites", string.Empty, diagnostics, out var satellites)) {
      return Array.Empty<DataVaultModelSatelliteDeclaration>();
    }

    var values = new List<DataVaultModelSatelliteDeclaration>();
    var index = 0;
    foreach (var satellite in satellites.EnumerateArray()) {
      var path = IndexPath(PropertyPath(string.Empty, "satellites"), index++);
      if (satellite.ValueKind != JsonValueKind.Object) {
        AddIssue(diagnostics, "shape", "DMV1102", "Each satellite declaration must be an object.", path);
        continue;
      }

      ValidateKnownProperties(satellite, SatelliteProperties, path, diagnostics);
      values.Add(new DataVaultModelSatelliteDeclaration(
          ReadRequiredString(satellite, "name", path, "Satellite name", diagnostics),
          ReadParentReference(satellite, path, diagnostics),
          ReadRequiredStringArray(satellite, "payload", path, "A satellite requires at least one payload name.", "DMV1202", diagnostics),
          ReadOptionalStringArray(satellite, "drivingKeys", path, "DMV1202", diagnostics),
          path));
    }

    return values;
  }

  private static DataVaultModelParentReferenceDeclaration ReadParentReference(
      JsonElement satellite,
      string satellitePath,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var parentPath = PropertyPath(satellitePath, "parent");
    if (!satellite.TryGetProperty("parent", out var parent) || parent.ValueKind != JsonValueKind.Object) {
      AddIssue(diagnostics, "shape", "DMV1102", "A satellite requires an object parent reference.", parentPath);
      return new DataVaultModelParentReferenceDeclaration(string.Empty, string.Empty, parentPath);
    }

    ValidateKnownProperties(parent, ParentProperties, parentPath, diagnostics);
    return new DataVaultModelParentReferenceDeclaration(
        ReadRequiredString(parent, "kind", parentPath, "Satellite parent kind", diagnostics),
        ReadRequiredString(parent, "name", parentPath, "Satellite parent name", diagnostics),
        parentPath);
  }

  private static IReadOnlyList<DataVaultModelPitDeclaration> ReadPits(
      JsonElement root,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!TryReadOptionalArray(root, "pits", string.Empty, diagnostics, out var pits)) {
      return Array.Empty<DataVaultModelPitDeclaration>();
    }

    var values = new List<DataVaultModelPitDeclaration>();
    var index = 0;
    foreach (var pit in pits.EnumerateArray()) {
      var path = IndexPath(PropertyPath(string.Empty, "pits"), index++);
      if (pit.ValueKind != JsonValueKind.Object) {
        AddIssue(diagnostics, "shape", "DMV1102", "Each PIT declaration must be an object.", path);
        continue;
      }

      ValidateKnownProperties(pit, PitProperties, path, diagnostics);
      values.Add(new DataVaultModelPitDeclaration(
          ReadRequiredString(pit, "name", path, "PIT name", diagnostics),
          ReadRequiredString(pit, "hub", path, "PIT hub", diagnostics),
          ReadRequiredStringArray(pit, "satellites", path, "A PIT requires at least one satellite reference.", "DMV1203", diagnostics),
          path));
    }

    return values;
  }

  private static IReadOnlyList<DataVaultModelBridgeDeclaration> ReadBridges(
      JsonElement root,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!TryReadOptionalArray(root, "bridges", string.Empty, diagnostics, out var bridges)) {
      return Array.Empty<DataVaultModelBridgeDeclaration>();
    }

    var values = new List<DataVaultModelBridgeDeclaration>();
    var index = 0;
    foreach (var bridge in bridges.EnumerateArray()) {
      var path = IndexPath(PropertyPath(string.Empty, "bridges"), index++);
      if (bridge.ValueKind != JsonValueKind.Object) {
        AddIssue(diagnostics, "shape", "DMV1102", "Each bridge declaration must be an object.", path);
        continue;
      }

      ValidateKnownProperties(bridge, BridgeProperties, path, diagnostics);
      var kind = ReadRequiredString(bridge, "kind", path, "Bridge kind", diagnostics);
      values.Add(new DataVaultModelBridgeDeclaration(
          ReadRequiredString(bridge, "name", path, "Bridge name", diagnostics),
          kind,
          ReadRequiredString(bridge, "source", path, "Bridge source", diagnostics),
          ReadBridgeEndpoints(bridge, kind, path, diagnostics),
          path));
    }

    return values;
  }

  private static IReadOnlyDictionary<string, DataVaultModelBridgeEndpointDeclaration> ReadBridgeEndpoints(
      JsonElement bridge,
      string kind,
      string bridgePath,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var endpointsPath = PropertyPath(bridgePath, "endpoints");
    if (!bridge.TryGetProperty("endpoints", out var endpoints) || endpoints.ValueKind != JsonValueKind.Object) {
      AddIssue(diagnostics, "shape", "DMV1102", "A bridge requires an object endpoints value.", endpointsPath);
      return new Dictionary<string, DataVaultModelBridgeEndpointDeclaration>(StringComparer.Ordinal);
    }

    var requiredProperties = string.Equals(kind, "hierarchy", StringComparison.Ordinal)
        ? HierarchyEndpointProperties
        : ManyToManyEndpointProperties;
    ValidateKnownProperties(endpoints, requiredProperties, endpointsPath, diagnostics);

    var values = new Dictionary<string, DataVaultModelBridgeEndpointDeclaration>(StringComparer.Ordinal);
    foreach (var propertyName in requiredProperties) {
      var endpointPath = PropertyPath(endpointsPath, propertyName);
      if (!endpoints.TryGetProperty(propertyName, out var endpoint) || endpoint.ValueKind != JsonValueKind.Object) {
        AddIssue(diagnostics, "shape", "DMV1102", "Bridge endpoint '" + propertyName + "' must be an object.", endpointPath);
        continue;
      }

      ValidateKnownProperties(endpoint, BridgeEndpointProperties, endpointPath, diagnostics);
      values[propertyName] = new DataVaultModelBridgeEndpointDeclaration(
          ReadRequiredString(endpoint, "hub", endpointPath, "Bridge endpoint hub", diagnostics),
          ReadOptionalString(endpoint, "role", endpointPath, diagnostics),
          endpointPath);
    }

    return values;
  }

  private static void ValidateArtifact(
      DataVaultModelArtifact artifact,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    ValidateDuplicateDeclarations(artifact.Hubs, "hub", declaration => declaration.Name, declaration => declaration.Path, diagnostics);
    ValidateDuplicateDeclarations(artifact.Links, "link", declaration => declaration.Name, declaration => declaration.Path, diagnostics);
    ValidateDuplicateDeclarations(artifact.Satellites, "satellite", declaration => declaration.Name, declaration => declaration.Path, diagnostics);
    ValidateDuplicateDeclarations(artifact.Pits, "PIT", declaration => declaration.Name, declaration => declaration.Path, diagnostics);
    ValidateDuplicateDeclarations(artifact.Bridges, "bridge", declaration => declaration.Name, declaration => declaration.Path, diagnostics);
    ValidateLinkDeclarations(artifact, diagnostics);
    ValidateSatelliteDeclarations(artifact, diagnostics);
    ValidatePitDeclarations(artifact, diagnostics);
    ValidateBridgeDeclarations(artifact, diagnostics);
    ValidateNamingCollisions(artifact, diagnostics);
  }

  private static void ValidateDuplicateDeclarations<T>(
      IEnumerable<T> declarations,
      string kind,
      Func<T, string> getName,
      Func<T, string> getPath,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var names = new HashSet<string>(StringComparer.Ordinal);
    foreach (var declaration in declarations) {
      var name = getName(declaration);
      if (string.IsNullOrWhiteSpace(name)) {
        continue;
      }

      if (!names.Add(name)) {
        AddIssue(
            diagnostics,
            "duplicate",
            "DMV1201",
            "Duplicate " + kind + " declaration name '" + name + "'.",
            PropertyPath(getPath(declaration), "name"));
      }
    }
  }

  private static void ValidateLinkDeclarations(
      DataVaultModelArtifact artifact,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var hubNames = artifact.Hubs.Select(hub => hub.Name).ToHashSet(StringComparer.Ordinal);
    foreach (var link in artifact.Links) {
      var roles = new HashSet<string>(StringComparer.Ordinal);
      foreach (var participant in link.Participants) {
        if (!string.IsNullOrWhiteSpace(participant.Role) && !roles.Add(participant.Role)) {
          AddIssue(
              diagnostics,
              "duplicate",
              "DMV1202",
              "Link '" + link.Name + "' declares participant role '" + participant.Role + "' more than once.",
              PropertyPath(participant.Path, "role"));
        }

        if (!string.IsNullOrWhiteSpace(participant.Hub) && !hubNames.Contains(participant.Hub)) {
          AddReferenceIssue(
              diagnostics,
              artifact,
              "link",
              link.Name,
              "hub",
              participant.Hub,
              PropertyPath(participant.Path, "hub"));
        }
      }

      foreach (var group in link.Participants
          .Where(participant => !string.IsNullOrWhiteSpace(participant.Hub))
          .GroupBy(participant => participant.Hub, StringComparer.Ordinal)) {
        if (group.Count() > 1 && group.Any(participant => string.IsNullOrWhiteSpace(participant.Role))) {
          AddIssue(
              diagnostics,
              "recursive-participant-binding",
              "DMV1602",
              "Link '" + link.Name + "' repeats hub '" + group.Key + "' without roles on every occurrence.",
              PropertyPath(link.Path, "participants"));
        }
      }
    }
  }

  private static void ValidateSatelliteDeclarations(
      DataVaultModelArtifact artifact,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    foreach (var satellite in artifact.Satellites) {
      if (!string.Equals(satellite.Parent.Kind, "hub", StringComparison.Ordinal) &&
          !string.Equals(satellite.Parent.Kind, "link", StringComparison.Ordinal) &&
          !string.IsNullOrWhiteSpace(satellite.Parent.Kind)) {
        AddIssue(
            diagnostics,
            "reference",
            "DMV1302",
            "Satellite '" + satellite.Name + "' declares unsupported parent kind '" + satellite.Parent.Kind + "'.",
            PropertyPath(satellite.Parent.Path, "kind"));
      }
      else if (string.Equals(satellite.Parent.Kind, "hub", StringComparison.Ordinal)) {
        if (!artifact.Hubs.Any(hub => string.Equals(hub.Name, satellite.Parent.Name, StringComparison.Ordinal))) {
          AddReferenceIssue(
              diagnostics,
              artifact,
              "satellite",
              satellite.Name,
              "hub",
              satellite.Parent.Name,
              PropertyPath(satellite.Parent.Path, "name"));
        }
      }
      else if (string.Equals(satellite.Parent.Kind, "link", StringComparison.Ordinal) &&
          !artifact.Links.Any(link => string.Equals(link.Name, satellite.Parent.Name, StringComparison.Ordinal))) {
        AddReferenceIssue(
            diagnostics,
            artifact,
            "satellite",
            satellite.Name,
            "link",
            satellite.Parent.Name,
            PropertyPath(satellite.Parent.Path, "name"));
      }

      var payloadNames = satellite.Payload.ToHashSet(StringComparer.Ordinal);
      foreach (var drivingKey in satellite.DrivingKeys) {
        if (payloadNames.Contains(drivingKey)) {
          AddIssue(
              diagnostics,
              "shape",
              "DMV1701",
              "Satellite '" + satellite.Name + "' driving key '" + drivingKey + "' overlaps payload.",
              PropertyPath(satellite.Path, "drivingKeys"));
        }
      }
    }
  }

  private static void ValidatePitDeclarations(
      DataVaultModelArtifact artifact,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    foreach (var pit in artifact.Pits) {
      if (!artifact.Hubs.Any(hub => string.Equals(hub.Name, pit.Hub, StringComparison.Ordinal))) {
        AddReferenceIssue(diagnostics, artifact, "PIT", pit.Name, "hub", pit.Hub, PropertyPath(pit.Path, "hub"));
      }

      foreach (var satelliteName in pit.Satellites) {
        var satellite = artifact.Satellites.FirstOrDefault(
            current => string.Equals(current.Name, satelliteName, StringComparison.Ordinal));
        if (satellite is null) {
          AddReferenceIssue(
              diagnostics,
              artifact,
              "PIT",
              pit.Name,
              "satellite",
              satelliteName,
              PropertyPath(pit.Path, "satellites"));
          continue;
        }

        if (!string.Equals(satellite.Parent.Kind, "hub", StringComparison.Ordinal) ||
            !string.Equals(satellite.Parent.Name, pit.Hub, StringComparison.Ordinal)) {
          AddIssue(
              diagnostics,
              "reference",
              "DMV1303",
              "PIT '" + pit.Name + "' references satellite '" + satelliteName + "' that does not belong to hub '" + pit.Hub + "'.",
              PropertyPath(pit.Path, "satellites"));
        }
      }
    }
  }

  private static void ValidateBridgeDeclarations(
      DataVaultModelArtifact artifact,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    foreach (var bridge in artifact.Bridges) {
      if (!string.Equals(bridge.Kind, "many-to-many", StringComparison.Ordinal) &&
          !string.Equals(bridge.Kind, "hierarchy", StringComparison.Ordinal) &&
          !string.IsNullOrWhiteSpace(bridge.Kind)) {
        AddIssue(
            diagnostics,
            "capability",
            "DMV1501",
            "Bridge '" + bridge.Name + "' declares unsupported kind '" + bridge.Kind + "'.",
            PropertyPath(bridge.Path, "kind"));
        continue;
      }

      var sourceLink = artifact.Links.FirstOrDefault(link => string.Equals(link.Name, bridge.Source, StringComparison.Ordinal));
      if (sourceLink is null) {
        AddReferenceIssue(diagnostics, artifact, "bridge", bridge.Name, "link", bridge.Source, PropertyPath(bridge.Path, "source"));
        continue;
      }

      if (string.Equals(bridge.Kind, "many-to-many", StringComparison.Ordinal)) {
        ValidateManyToManyBridge(artifact, bridge, sourceLink, diagnostics);
      }
      else if (string.Equals(bridge.Kind, "hierarchy", StringComparison.Ordinal)) {
        ValidateHierarchyBridge(artifact, bridge, sourceLink, diagnostics);
      }
    }
  }

  private static void ValidateManyToManyBridge(
      DataVaultModelArtifact artifact,
      DataVaultModelBridgeDeclaration bridge,
      DataVaultModelLinkDeclaration sourceLink,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var from = bridge.Endpoints.TryGetValue("from", out var fromEndpoint)
        ? ResolveBridgeEndpoint(artifact, bridge, sourceLink, fromEndpoint, diagnostics)
        : null;
    var to = bridge.Endpoints.TryGetValue("to", out var toEndpoint)
        ? ResolveBridgeEndpoint(artifact, bridge, sourceLink, toEndpoint, diagnostics)
        : null;

    if (from.HasValue && to.HasValue && from.Value == to.Value) {
      AddIssue(
          diagnostics,
          "duplicate",
          "DMV1203",
          "Bridge '" + bridge.Name + "' binds both endpoints to the same source-link participant.",
          PropertyPath(bridge.Path, "endpoints"));
    }
  }

  private static void ValidateHierarchyBridge(
      DataVaultModelArtifact artifact,
      DataVaultModelBridgeDeclaration bridge,
      DataVaultModelLinkDeclaration sourceLink,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var participants = sourceLink.Participants;
    if (participants.Count != 2 ||
        participants.Select(participant => participant.Hub).Distinct(StringComparer.Ordinal).Count() != 1) {
      AddIssue(
          diagnostics,
          "recursive-participant-binding",
          "DMV1601",
          "Hierarchy bridge '" + bridge.Name + "' must traverse a two-participant self-link.",
          bridge.Path);
      return;
    }

    if (participants.Any(participant => string.IsNullOrWhiteSpace(participant.Role)) ||
        participants.Select(participant => participant.Role).Distinct(StringComparer.Ordinal).Count() != 2) {
      AddIssue(
          diagnostics,
          "recursive-participant-binding",
          "DMV1601",
          "Hierarchy bridge '" + bridge.Name + "' requires distinct source-link participant roles.",
          PropertyPath(sourceLink.Path, "participants"));
    }

    var ancestor = bridge.Endpoints.TryGetValue("ancestor", out var ancestorEndpoint)
        ? ResolveRequiredRoleBridgeEndpoint(artifact, bridge, sourceLink, ancestorEndpoint, diagnostics)
        : null;
    var descendant = bridge.Endpoints.TryGetValue("descendant", out var descendantEndpoint)
        ? ResolveRequiredRoleBridgeEndpoint(artifact, bridge, sourceLink, descendantEndpoint, diagnostics)
        : null;

    if (ancestor.HasValue && descendant.HasValue && ancestor.Value == descendant.Value) {
      AddIssue(
          diagnostics,
          "recursive-participant-binding",
          "DMV1601",
          "Hierarchy bridge '" + bridge.Name + "' resolves ancestor and descendant to the same source-link participant.",
          PropertyPath(bridge.Path, "endpoints"));
    }
  }

  private static int? ResolveRequiredRoleBridgeEndpoint(
      DataVaultModelArtifact artifact,
      DataVaultModelBridgeDeclaration bridge,
      DataVaultModelLinkDeclaration sourceLink,
      DataVaultModelBridgeEndpointDeclaration endpoint,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (string.IsNullOrWhiteSpace(endpoint.Role)) {
      AddIssue(
          diagnostics,
          "recursive-participant-binding",
          "DMV1601",
          "Bridge endpoint for hub '" + endpoint.Hub + "' requires a role to bind a recursive participant.",
          PropertyPath(endpoint.Path, "role"));
      return null;
    }

    return ResolveBridgeEndpoint(artifact, bridge, sourceLink, endpoint, diagnostics);
  }

  private static int? ResolveBridgeEndpoint(
      DataVaultModelArtifact artifact,
      DataVaultModelBridgeDeclaration bridge,
      DataVaultModelLinkDeclaration sourceLink,
      DataVaultModelBridgeEndpointDeclaration endpoint,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!artifact.Hubs.Any(hub => string.Equals(hub.Name, endpoint.Hub, StringComparison.Ordinal))) {
      AddReferenceIssue(diagnostics, artifact, "bridge", bridge.Name, "hub", endpoint.Hub, PropertyPath(endpoint.Path, "hub"));
      return null;
    }

    var matchingHubParticipants = sourceLink.Participants
        .Select((participant, index) => new BridgeParticipantBinding(participant, index))
        .Where(binding => string.Equals(binding.Participant.Hub, endpoint.Hub, StringComparison.Ordinal))
        .ToArray();
    if (matchingHubParticipants.Length == 0) {
      AddIssue(
          diagnostics,
          "reference",
          "DMV1301",
          "Bridge '" + bridge.Name + "' endpoint hub '" + endpoint.Hub + "' does not resolve to source link '" + sourceLink.Name + "'.",
          PropertyPath(endpoint.Path, "hub"));
      return null;
    }

    if (!string.IsNullOrWhiteSpace(endpoint.Role)) {
      var matchingRoleParticipants = matchingHubParticipants
          .Where(binding => string.Equals(binding.Participant.Role, endpoint.Role, StringComparison.Ordinal))
          .ToArray();
      if (matchingRoleParticipants.Length == 0) {
        AddIssue(
            diagnostics,
            "reference",
            "DMV1301",
            "Bridge '" + bridge.Name + "' endpoint role '" + endpoint.Role + "' does not resolve to source link '" + sourceLink.Name + "'.",
            PropertyPath(endpoint.Path, "role"));
        return null;
      }

      return matchingRoleParticipants[0].Index;
    }

    if (matchingHubParticipants.Length > 1) {
      AddIssue(
          diagnostics,
          "recursive-participant-binding",
          "DMV1601",
          "Bridge '" + bridge.Name + "' endpoint hub '" + endpoint.Hub + "' is ambiguous and requires a role.",
          endpoint.Path);
      return null;
    }

    return matchingHubParticipants[0].Index;
  }

  private static void ValidateNamingCollisions(
      DataVaultModelArtifact artifact,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var tableNames = new Dictionary<string, string>(StringComparer.Ordinal);
    foreach (var hub in artifact.Hubs.Where(hub => !string.IsNullOrWhiteSpace(hub.Name))) {
      TrackProducedName(
          tableNames,
          DefaultNamingPolicy.Instance.GetHubTableName(hub.Name),
          "hub '" + hub.Name + "'",
          PropertyPath(hub.Path, "name"),
          diagnostics);
    }

    foreach (var link in artifact.Links.Where(link => !string.IsNullOrWhiteSpace(link.Name))) {
      TrackProducedName(
          tableNames,
          DefaultNamingPolicy.Instance.GetLinkTableName(link.Name, link.Participants.Select(GetParticipantProducedBaseName)),
          "link '" + link.Name + "'",
          PropertyPath(link.Path, "name"),
          diagnostics);
      ValidateLinkColumnNames(link, diagnostics);
    }

    foreach (var satellite in artifact.Satellites.Where(satellite => !string.IsNullOrWhiteSpace(satellite.Name))) {
      TrackProducedName(
          tableNames,
          DefaultNamingPolicy.Instance.GetSatelliteTableName(satellite.Parent.Name, satellite.Name),
          "satellite '" + satellite.Name + "'",
          PropertyPath(satellite.Path, "name"),
          diagnostics);
    }

    foreach (var pit in artifact.Pits.Where(pit => !string.IsNullOrWhiteSpace(pit.Name))) {
      TrackProducedName(
          tableNames,
          DefaultDataVaultNamingPolicy.Instance.GetPointInTimeTableName(
              new DataVaultPointInTimeNameContext(pit.Name, pit.Hub, pit.Satellites)),
          "PIT '" + pit.Name + "'",
          PropertyPath(pit.Path, "name"),
          diagnostics);
    }

    foreach (var bridge in artifact.Bridges.Where(bridge => !string.IsNullOrWhiteSpace(bridge.Name))) {
      TrackProducedName(
          tableNames,
          "Bridge" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(bridge.Name),
          "bridge '" + bridge.Name + "'",
          PropertyPath(bridge.Path, "name"),
          diagnostics);
    }
  }

  private static void ValidateLinkColumnNames(
      DataVaultModelLinkDeclaration link,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var columnNames = new Dictionary<string, string>(StringComparer.Ordinal);
    var tableName = DefaultNamingPolicy.Instance.GetLinkTableName(link.Name, link.Participants.Select(GetParticipantProducedBaseName));
    TrackProducedName(
        columnNames,
        DefaultNamingPolicy.Instance.GetHashKeyColumnName(link.Name),
        "link hash key",
        PropertyPath(link.Path, "name"),
        diagnostics);
    TrackProducedName(
        columnNames,
        DefaultNamingPolicy.Instance.GetLoadTimestampColumnName(),
        "link load timestamp",
        PropertyPath(link.Path, "name"),
        diagnostics);
    TrackProducedName(
        columnNames,
        DefaultNamingPolicy.Instance.GetRecordSourceColumnName(),
        "link record source",
        PropertyPath(link.Path, "name"),
        diagnostics);

    foreach (var participant in link.Participants) {
      TrackProducedName(
          columnNames,
          DefaultNamingPolicy.Instance.GetHashKeyColumnName(GetParticipantProducedBaseName(participant)),
          "link participant '" + GetParticipantProducedBaseName(participant) + "' on table '" + tableName + "'",
          participant.Path,
          diagnostics);
    }
  }

  private static string GetParticipantProducedBaseName(DataVaultModelLinkParticipantDeclaration participant) {
    return string.IsNullOrWhiteSpace(participant.Role) ? participant.Hub : participant.Role!;
  }

  private static void TrackProducedName(
      IDictionary<string, string> producedNames,
      string producedName,
      string sourceDescription,
      string path,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!producedNames.TryAdd(producedName, sourceDescription)) {
      AddIssue(
          diagnostics,
          "naming",
          "DMV1401",
          "Default naming collision for produced name '" + producedName + "' between " + producedNames[producedName] + " and " + sourceDescription + ".",
          path);
    }
  }

  private static void AddReferenceIssue(
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics,
      DataVaultModelArtifact artifact,
      string sourceKind,
      string sourceName,
      string targetKind,
      string targetName,
      string path) {
    var wrongKind = targetKind switch {
      "hub" => artifact.Links.Any(link => string.Equals(link.Name, targetName, StringComparison.Ordinal)) ||
          artifact.Satellites.Any(satellite => string.Equals(satellite.Name, targetName, StringComparison.Ordinal)),
      "link" => artifact.Hubs.Any(hub => string.Equals(hub.Name, targetName, StringComparison.Ordinal)) ||
          artifact.Satellites.Any(satellite => string.Equals(satellite.Name, targetName, StringComparison.Ordinal)),
      "satellite" => artifact.Hubs.Any(hub => string.Equals(hub.Name, targetName, StringComparison.Ordinal)) ||
          artifact.Links.Any(link => string.Equals(link.Name, targetName, StringComparison.Ordinal)),
      _ => false,
    };

    AddIssue(
        diagnostics,
        "reference",
        wrongKind ? "DMV1302" : "DMV1301",
        sourceKind + " '" + sourceName + "' references " + (wrongKind ? "wrong-kind" : "missing") + " " + targetKind + " '" + targetName + "'.",
        path);
  }

  private static DataVaultMetadataModel CreateMetadataModel(DataVaultModelArtifact artifact) {
    var hubs = artifact.Hubs
        .Select(hub => new DataVaultHubMetadata(hub.Name, hub.BusinessKeys))
        .ToArray();
    var links = artifact.Links
        .Select(link => new DataVaultLinkMetadata(
            link.Name,
            link.Participants.Select(participant => DataVaultMetadataReference.Hub(participant.Hub))))
        .ToArray();
    var satellites = artifact.Satellites
        .Select(CreateSatelliteMetadata)
        .ToArray();
    var pointInTimeTables = artifact.Pits
        .Select(pit => new DataVaultPointInTimeMetadata(
            pit.Name,
            DataVaultMetadataReference.Hub(pit.Hub),
            pit.Satellites.Select(DataVaultMetadataReference.Satellite)))
        .ToArray();
    var bridges = artifact.Bridges
        .Select(CreateBridgeMetadata)
        .ToArray();

    return new DataVaultMetadataModel(hubs, links, satellites, pointInTimeTables, bridges);
  }

  private static DataVaultSatelliteMetadata CreateSatelliteMetadata(DataVaultModelSatelliteDeclaration satellite) {
    var parent = string.Equals(satellite.Parent.Kind, "link", StringComparison.Ordinal)
        ? DataVaultMetadataReference.Link(satellite.Parent.Name)
        : DataVaultMetadataReference.Hub(satellite.Parent.Name);
    return satellite.DrivingKeys.Count == 0
        ? new DataVaultSatelliteMetadata(satellite.Name, parent, satellite.Payload)
        : new DataVaultSatelliteMetadata(satellite.Name, parent, satellite.Payload, satellite.DrivingKeys);
  }

  private static DataVaultBridgeMetadata CreateBridgeMetadata(DataVaultModelBridgeDeclaration bridge) {
    var kind = string.Equals(bridge.Kind, "hierarchy", StringComparison.Ordinal)
        ? DataVaultBridgeKind.Hierarchy
        : DataVaultBridgeKind.ManyToMany;
    var endpoints = bridge.Endpoints
        .Select(endpoint => new DataVaultBridgeEndpointMetadata(
            GetBridgeEndpointRole(endpoint.Key),
            DataVaultMetadataReference.Hub(endpoint.Value.Hub),
            endpoint.Value.Role ?? endpoint.Value.Hub))
        .ToArray();

    return new DataVaultBridgeMetadata(
        bridge.Name,
        kind,
        DataVaultMetadataReference.Link(bridge.Source),
        endpoints);
  }

  private static DataVaultBridgeEndpointRole GetBridgeEndpointRole(string endpointName) {
    return endpointName switch {
      "from" => DataVaultBridgeEndpointRole.From,
      "to" => DataVaultBridgeEndpointRole.To,
      "ancestor" => DataVaultBridgeEndpointRole.Ancestor,
      "descendant" => DataVaultBridgeEndpointRole.Descendant,
      _ => throw new ArgumentOutOfRangeException(nameof(endpointName), endpointName, "Unsupported bridge endpoint."),
    };
  }

  private static bool TryReadOptionalArray(
      JsonElement element,
      string propertyName,
      string elementPath,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics,
      out JsonElement array) {
    array = default;
    if (!element.TryGetProperty(propertyName, out var value)) {
      return false;
    }

    if (value.ValueKind != JsonValueKind.Array) {
      AddIssue(
          diagnostics,
          "shape",
          "DMV1102",
          "The '" + propertyName + "' value must be an array.",
          PropertyPath(elementPath, propertyName));
      return false;
    }

    array = value;
    return true;
  }

  private static bool TryReadRequiredArray(
      JsonElement element,
      string propertyName,
      string elementPath,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics,
      out JsonElement array) {
    array = default;
    if (!element.TryGetProperty(propertyName, out var value) || value.ValueKind != JsonValueKind.Array) {
      AddIssue(
          diagnostics,
          "shape",
          "DMV1102",
          "The '" + propertyName + "' value must be an array.",
          PropertyPath(elementPath, propertyName));
      return false;
    }

    array = value;
    return true;
  }

  private static string ReadRequiredString(
      JsonElement element,
      string propertyName,
      string elementPath,
      string label,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var path = PropertyPath(elementPath, propertyName);
    if (!element.TryGetProperty(propertyName, out var value) ||
        value.ValueKind != JsonValueKind.String ||
        string.IsNullOrWhiteSpace(value.GetString())) {
      AddIssue(diagnostics, "shape", "DMV1102", label + " must be a non-blank string.", path);
      return string.Empty;
    }

    return value.GetString()!;
  }

  private static string? ReadOptionalString(
      JsonElement element,
      string propertyName,
      string elementPath,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!element.TryGetProperty(propertyName, out var value)) {
      return null;
    }

    var path = PropertyPath(elementPath, propertyName);
    if (value.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(value.GetString())) {
      AddIssue(diagnostics, "shape", "DMV1102", "The '" + propertyName + "' value must be a non-blank string.", path);
      return null;
    }

    return value.GetString();
  }

  private static IReadOnlyList<string> ReadRequiredStringArray(
      JsonElement element,
      string propertyName,
      string elementPath,
      string emptyMessage,
      string duplicateCode,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!TryReadRequiredArray(element, propertyName, elementPath, diagnostics, out var array)) {
      return Array.Empty<string>();
    }

    if (array.GetArrayLength() == 0) {
      AddIssue(diagnostics, "shape", "DMV1103", emptyMessage, PropertyPath(elementPath, propertyName));
    }

    return ReadStringArrayValues(array, PropertyPath(elementPath, propertyName), duplicateCode, diagnostics);
  }

  private static IReadOnlyList<string> ReadOptionalStringArray(
      JsonElement element,
      string propertyName,
      string elementPath,
      string duplicateCode,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    if (!TryReadOptionalArray(element, propertyName, elementPath, diagnostics, out var array)) {
      return Array.Empty<string>();
    }

    return ReadStringArrayValues(array, PropertyPath(elementPath, propertyName), duplicateCode, diagnostics);
  }

  private static IReadOnlyList<string> ReadStringArrayValues(
      JsonElement array,
      string arrayPath,
      string duplicateCode,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    var values = new List<string>();
    var seen = new HashSet<string>(StringComparer.Ordinal);
    var index = 0;
    foreach (var item in array.EnumerateArray()) {
      var itemPath = IndexPath(arrayPath, index++);
      if (item.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(item.GetString())) {
        AddIssue(diagnostics, "shape", "DMV1102", "Array values must be non-blank strings.", itemPath);
        continue;
      }

      var value = item.GetString()!;
      if (!seen.Add(value)) {
        AddIssue(
            diagnostics,
            "duplicate",
            duplicateCode,
            "Duplicate name '" + value + "'.",
            itemPath);
      }

      values.Add(value);
    }

    return values;
  }

  private static void ValidateKnownProperties(
      JsonElement element,
      ISet<string> knownProperties,
      string elementPath,
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics) {
    foreach (var property in element.EnumerateObject()) {
      if (knownProperties.Contains(property.Name)) {
        continue;
      }

      var path = PropertyPath(elementPath, property.Name);
      if (ProviderSpecificFieldNames.Contains(property.Name)) {
        AddIssue(
            diagnostics,
            "provider-choice",
            "DMV1502",
            "Provider-specific field '" + property.Name + "' is not supported by dvault.model.v1.",
            path);
        continue;
      }

      AddIssue(
          diagnostics,
          "shape",
          "DMV1101",
          "Unknown field '" + property.Name + "'.",
          path);
    }
  }

  private static bool HasErrors(IEnumerable<DataVaultModelArtifactDiagnostic> diagnostics) {
    return diagnostics.Any(diagnostic => string.Equals(diagnostic.Severity, SeverityError, StringComparison.Ordinal));
  }

  private static void AddIssue(
      ICollection<DataVaultModelArtifactDiagnostic> diagnostics,
      string category,
      string code,
      string message,
      string path) {
    diagnostics.Add(new DataVaultModelArtifactDiagnostic(SeverityError, category, code, message, path));
  }

  private static string PropertyPath(string elementPath, string propertyName) {
    return elementPath.Length == 0
        ? "/" + EscapeJsonPointerToken(propertyName)
        : elementPath + "/" + EscapeJsonPointerToken(propertyName);
  }

  private static string IndexPath(string arrayPath, int index) {
    return arrayPath + "/" + index.ToString(System.Globalization.CultureInfo.InvariantCulture);
  }

  private static string EscapeJsonPointerToken(string value) {
    return value.Replace("~", "~0", StringComparison.Ordinal).Replace("/", "~1", StringComparison.Ordinal);
  }

  private readonly record struct BridgeParticipantBinding(
      DataVaultModelLinkParticipantDeclaration Participant,
      int Index);
}

internal sealed record DataVaultModelArtifactParseResult(
    DataVaultModelArtifact? Artifact,
    DataVaultMetadataModel? MetadataModel,
    DataVaultMetadataRegistry? MetadataRegistry,
    IReadOnlyList<DataVaultModelArtifactDiagnostic> Diagnostics) {
  public bool IsValid => Diagnostics.All(
      diagnostic => !string.Equals(diagnostic.Severity, "error", StringComparison.Ordinal));

  public DataVaultLoadTimestampStorage LoadTimestampStorage =>
      Artifact?.LoadTimestampStorage ?? DataVaultLoadTimestampStorage.ProviderDefault;
}

internal sealed record DataVaultModelArtifactDiagnostic(
    string Severity,
    string Category,
    string Code,
    string Message,
    string Path);

internal sealed record DataVaultModelArtifact(
    string SchemaVersion,
    DataVaultModelArtifactNaming Naming,
    DataVaultLoadTimestampStorage LoadTimestampStorage,
    IReadOnlyList<DataVaultModelHubDeclaration> Hubs,
    IReadOnlyList<DataVaultModelLinkDeclaration> Links,
    IReadOnlyList<DataVaultModelSatelliteDeclaration> Satellites,
    IReadOnlyList<DataVaultModelPitDeclaration> Pits,
    IReadOnlyList<DataVaultModelBridgeDeclaration> Bridges);

internal sealed record DataVaultModelArtifactNaming(string Policy);

internal sealed record DataVaultModelHubDeclaration(
    string Name,
    IReadOnlyList<string> BusinessKeys,
    string Path);

internal sealed record DataVaultModelLinkDeclaration(
    string Name,
    IReadOnlyList<DataVaultModelLinkParticipantDeclaration> Participants,
    string Path);

internal sealed record DataVaultModelLinkParticipantDeclaration(
    string Hub,
    string? Role,
    string Path);

internal sealed record DataVaultModelSatelliteDeclaration(
    string Name,
    DataVaultModelParentReferenceDeclaration Parent,
    IReadOnlyList<string> Payload,
    IReadOnlyList<string> DrivingKeys,
    string Path);

internal sealed record DataVaultModelParentReferenceDeclaration(
    string Kind,
    string Name,
    string Path);

internal sealed record DataVaultModelPitDeclaration(
    string Name,
    string Hub,
    IReadOnlyList<string> Satellites,
    string Path);

internal sealed record DataVaultModelBridgeDeclaration(
    string Name,
    string Kind,
    string Source,
    IReadOnlyDictionary<string, DataVaultModelBridgeEndpointDeclaration> Endpoints,
    string Path);

internal sealed record DataVaultModelBridgeEndpointDeclaration(
    string Hub,
    string? Role,
    string Path);
