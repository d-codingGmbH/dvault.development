using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DCoding.Data.DVault;

internal static class DataVaultEfMetadataTranslator {
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public static void Apply(ModelBuilder modelBuilder, DataVaultMetadataModel metadataModel) {
    Apply(modelBuilder, metadataModel, DataVaultProviderCapabilityProfileSelection.Select(modelBuilder));
  }

  internal static void Apply(
      ModelBuilder modelBuilder,
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(metadataModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var entities = CreateEntities(metadataModel).ToArray();
    var providerIdentifiers = CreateProviderIdentifierProjectionSet(entities, providerCapabilities);
    foreach (var entity in entities) {
      ApplyEntity(modelBuilder, entity, providerCapabilities, providerIdentifiers);
    }
  }

  internal static IEnumerable<DataVaultDiagnosticsIssue> ValidateProviderIdentifiers(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities,
      string? providerName = null) {
    ArgumentNullException.ThrowIfNull(metadataModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var entities = CreateEntities(metadataModel).ToArray();
    var preflight = DataVaultProviderIdentifierPreflight.Analyze(
        providerCapabilities,
        CreateIdentifierCandidates(entities));

    return preflight.Issues.Select(issue =>
        DataVaultProviderIdentifierPreflight.CreateDiagnosticIssue(issue, providerName));
  }

  private static IEnumerable<EntityProjection> CreateEntities(DataVaultMetadataModel metadataModel) {
    foreach (var hub in metadataModel.Hubs) {
      yield return CreateHubEntity(hub);
    }

    foreach (var link in metadataModel.Links) {
      yield return CreateLinkEntity(link);
    }

    foreach (var satellite in metadataModel.Satellites) {
      yield return CreateSatelliteEntity(satellite);
    }

    foreach (var bridge in metadataModel.Bridges) {
      yield return CreateBridgeEntity(bridge);
    }

    foreach (var pit in metadataModel.Pits) {
      yield return CreatePitEntity(pit, metadataModel);
    }
  }

  private static DataVaultProviderIdentifierProjectionSet CreateProviderIdentifierProjectionSet(
      IReadOnlyList<EntityProjection> entities,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var preflight = DataVaultProviderIdentifierPreflight.Analyze(
        providerCapabilities,
        CreateIdentifierCandidates(entities));
    if (preflight.Issues.Count == 0) {
      return preflight.ProjectionSet;
    }

    var issue = preflight.Issues[0];
    var diagnostic = DataVaultProviderIdentifierPreflight.CreateDiagnosticIssue(issue);
    throw new InvalidOperationException(diagnostic.Message);
  }

  private static IEnumerable<DataVaultProviderIdentifierCandidate> CreateIdentifierCandidates(
      IReadOnlyList<EntityProjection> entities) {
    foreach (var entity in entities) {
      yield return new DataVaultProviderIdentifierCandidate(
          DataVaultProviderIdentifierKind.Table,
          entity.Name,
          entity.MetadataName,
          "<default-schema>",
          CreateTableIdentifierPath(entity));

      foreach (var property in entity.Properties) {
        yield return new DataVaultProviderIdentifierCandidate(
            DataVaultProviderIdentifierKind.Column,
            property.Name,
            property.MetadataName,
            entity.Name,
            CreateColumnIdentifierPath(entity, property));
      }

      yield return new DataVaultProviderIdentifierCandidate(
          DataVaultProviderIdentifierKind.PrimaryKey,
          entity.PrimaryKey.Name,
          entity.MetadataName,
          entity.Name,
          CreatePrimaryKeyIdentifierPath(entity));

      foreach (var index in entity.Indexes) {
        yield return new DataVaultProviderIdentifierCandidate(
            DataVaultProviderIdentifierKind.Index,
            index.Name,
            entity.MetadataName,
            entity.Name,
            CreateIndexIdentifierPath(entity, index));
      }
    }
  }

  private static string CreateTableIdentifierPath(EntityProjection entity) {
    return CreateEntityIdentifierPath(entity) + "/table";
  }

  private static string CreateColumnIdentifierPath(
      EntityProjection entity,
      PropertyProjection property) {
    return CreateEntityIdentifierPath(entity) + "/columns/" + property.Name;
  }

  private static string CreatePrimaryKeyIdentifierPath(EntityProjection entity) {
    return CreateEntityIdentifierPath(entity) + "/primary-key/" + entity.PrimaryKey.Name;
  }

  private static string CreateIndexIdentifierPath(
      EntityProjection entity,
      IndexProjection index) {
    return CreateEntityIdentifierPath(entity) + "/indexes/" + index.Name;
  }

  private static string CreateEntityIdentifierPath(EntityProjection entity) {
    return "metadata/" + entity.Kind.ToString().ToLowerInvariant() + "/" + entity.Name;
  }

  private static EntityProjection CreateHubEntity(DataVaultHubMetadata hub) {
    var tableName = NamingPolicy.GetHubTableName(new DataVaultHubNameContext(hub.Name));
    var hashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, hub.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, hub.Name, tableName));
    var recordSourceColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, hub.Name, tableName));

    var businessKeyColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        hub.BusinessKeyColumns.Select(column => column.ColumnName),
        [hashKeyColumnName, loadTimestampColumnName, recordSourceColumnName]);
    var properties = new List<PropertyProjection>
    {
        TechnicalProperty(hashKeyColumnName, TechnicalMetadataColumnRole.HashKey, hub.HashKeyMetadata.EffectiveColumnName),
        TechnicalProperty(
            loadTimestampColumnName,
            TechnicalMetadataColumnRole.LoadTimestamp,
            hub.LoadTimestampMetadata.EffectiveColumnName),
        TechnicalProperty(
            recordSourceColumnName,
            TechnicalMetadataColumnRole.RecordSource,
            hub.RecordSourceMetadata.EffectiveColumnName),
    };

    for (var index = 0; index < businessKeyColumnNames.Count; index++) {
      properties.Add(new PropertyProjection(
          businessKeyColumnNames[index],
          DataVaultPropertyRole.BusinessKey,
          TechnicalRole: null,
          hub.BusinessKeyColumns[index].ColumnName));
    }

    var indexes = new[]
    {
        new IndexProjection(
            NamingPolicy.GetIndexName(new DataVaultIndexNameContext(
                DataVaultIndexKind.BusinessKey,
                tableName,
                businessKeyColumnNames,
                IsUnique: true)),
            businessKeyColumnNames,
            IsUnique: true),
    };
    var primaryKey = new KeyProjection(
        NamingPolicy.GetConstraintName(
            new DataVaultConstraintNameContext(DataVaultConstraintKind.PrimaryKey, tableName, [hashKeyColumnName])),
        [hashKeyColumnName]);

    return new EntityProjection(
        tableName,
        DataVaultTableKind.Hub,
        hub.Name,
        ParentReference: null,
        properties,
        primaryKey,
        indexes);
  }

  private static EntityProjection CreateLinkEntity(DataVaultLinkMetadata link) {
    var participantNames = link.Participants
        .Select(participant => participant.SourceEndpointName)
        .ToArray();
    var tableName = NamingPolicy.GetLinkTableName(new DataVaultLinkNameContext(link.Name, participantNames));
    var linkHashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, link.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, link.Name, tableName));
    var recordSourceColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, link.Name, tableName));
    var participantHashKeyColumnNames = participantNames
        .Select(participantName => NamingPolicy.GetTechnicalColumnName(
            new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, participantName, tableName)))
        .ToArray();

    var properties = new List<PropertyProjection>
    {
        TechnicalProperty(linkHashKeyColumnName, TechnicalMetadataColumnRole.HashKey, link.HashKeyMetadata.EffectiveColumnName),
        TechnicalProperty(
            loadTimestampColumnName,
            TechnicalMetadataColumnRole.LoadTimestamp,
            link.LoadTimestampMetadata.EffectiveColumnName),
        TechnicalProperty(
            recordSourceColumnName,
            TechnicalMetadataColumnRole.RecordSource,
            link.RecordSourceMetadata.EffectiveColumnName),
    };

    for (var index = 0; index < participantHashKeyColumnNames.Length; index++) {
      properties.Add(new PropertyProjection(
          participantHashKeyColumnNames[index],
          DataVaultPropertyRole.ParticipantReference,
          TechnicalMetadataColumnRole.HashKey,
          participantNames[index]));
    }

    var indexes = new[]
    {
        new IndexProjection(
            NamingPolicy.GetIndexName(new DataVaultIndexNameContext(
                DataVaultIndexKind.Relationship,
                tableName,
                participantHashKeyColumnNames,
                IsUnique: false)),
            participantHashKeyColumnNames,
            IsUnique: false),
    };
    var primaryKey = new KeyProjection(
        NamingPolicy.GetConstraintName(
            new DataVaultConstraintNameContext(DataVaultConstraintKind.PrimaryKey, tableName, [linkHashKeyColumnName])),
        [linkHashKeyColumnName]);

    return new EntityProjection(
        tableName,
        DataVaultTableKind.Link,
        link.Name,
        ParentReference: null,
        properties,
        primaryKey,
        indexes);
  }

  private static EntityProjection CreateSatelliteEntity(DataVaultSatelliteMetadata satellite) {
    var tableName = NamingPolicy.GetSatelliteTableName(
        new DataVaultSatelliteNameContext(satellite.Parent.Name, satellite.Name));
    var parentHashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, satellite.Parent.Name, tableName));
    var hashDiffColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashDiff, satellite.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, satellite.Name, tableName));
    var recordSourceColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, satellite.Name, tableName));
    var drivingKeyColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellite.DrivingKeyNames,
        [parentHashKeyColumnName, hashDiffColumnName, loadTimestampColumnName, recordSourceColumnName]);

    var payloadColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellite.PayloadColumns.Select(column => column.ColumnName),
        [parentHashKeyColumnName, .. drivingKeyColumnNames, hashDiffColumnName, loadTimestampColumnName, recordSourceColumnName]);
    var properties = new List<PropertyProjection>
    {
        TechnicalProperty(parentHashKeyColumnName, TechnicalMetadataColumnRole.HashKey, satellite.Parent.Name),
    };

    for (var index = 0; index < drivingKeyColumnNames.Count; index++) {
      properties.Add(new PropertyProjection(
          drivingKeyColumnNames[index],
          DataVaultPropertyRole.DrivingKey,
          TechnicalRole: null,
          satellite.DrivingKeyNames[index]));
    }

    properties.AddRange(
        [
            TechnicalProperty(hashDiffColumnName, TechnicalMetadataColumnRole.HashDiff, satellite.HashDiffMetadata.EffectiveColumnName),
            TechnicalProperty(
                loadTimestampColumnName,
                TechnicalMetadataColumnRole.LoadTimestamp,
                satellite.LoadTimestampMetadata.EffectiveColumnName),
            TechnicalProperty(
                recordSourceColumnName,
                TechnicalMetadataColumnRole.RecordSource,
                satellite.RecordSourceMetadata.EffectiveColumnName),
        ]);

    for (var index = 0; index < payloadColumnNames.Count; index++) {
      properties.Add(new PropertyProjection(
          payloadColumnNames[index],
          DataVaultPropertyRole.Payload,
          TechnicalRole: null,
          satellite.PayloadColumns[index].ColumnName));
    }

    var satelliteParentIndexColumnNames = new[]
    {
        parentHashKeyColumnName,
    }
        .Concat(drivingKeyColumnNames)
        .Append(loadTimestampColumnName)
        .ToArray();
    var indexes = new[]
    {
        new IndexProjection(
            NamingPolicy.GetIndexName(new DataVaultIndexNameContext(
                DataVaultIndexKind.SatelliteParent,
                tableName,
                satelliteParentIndexColumnNames,
                IsUnique: false)),
            satelliteParentIndexColumnNames,
            IsUnique: false,
            DescendingPropertyNames: [loadTimestampColumnName],
            IncludedPropertyNames: [hashDiffColumnName]),
    };
    var primaryKey = new KeyProjection(
        NamingPolicy.GetConstraintName(
            new DataVaultConstraintNameContext(
                DataVaultConstraintKind.PrimaryKey,
                tableName,
                satelliteParentIndexColumnNames)),
        satelliteParentIndexColumnNames);

    return new EntityProjection(
        tableName,
        DataVaultTableKind.Satellite,
        satellite.Name,
        satellite.Parent,
        properties,
        primaryKey,
        indexes);
  }

  private static EntityProjection CreateBridgeEntity(DataVaultBridgeMetadata bridge) {
    if (bridge.ProjectionFeatures != DataVaultBridgeProjectionFeatures.None) {
      throw new NotSupportedException(
          "Bridge metadata '" +
          bridge.Name +
          "' requests unsupported provider-neutral projection feature(s) '" +
          bridge.ProjectionFeatures +
          "'. Baseline bridge translation supports only endpoint hash-key columns and hierarchy TraversalDepth.");
    }

    return bridge.Kind switch {
      DataVaultBridgeKind.ManyToMany => CreateManyToManyBridgeEntity(bridge),
      DataVaultBridgeKind.Hierarchy => CreateHierarchyBridgeEntity(bridge),
      _ => throw new NotSupportedException(
          "Bridge metadata '" +
          bridge.Name +
          "' declares unsupported bridge kind '" +
          bridge.Kind +
          "'. Baseline bridge translation supports only many-to-many and hierarchy bridge metadata."),
    };
  }

  private static EntityProjection CreateManyToManyBridgeEntity(DataVaultBridgeMetadata bridge) {
    var tableName = GetBridgeTableName(bridge);
    var participantColumnNames = bridge.Endpoints
        .Select(GetBridgeEndpointHashKeyColumnName)
        .ToArray();
    var properties = bridge.Endpoints
        .Select((endpoint, index) => BridgeParticipantProperty(participantColumnNames[index], endpoint))
        .ToArray();
    var primaryKey = new KeyProjection(
        NamingPolicy.GetConstraintName(
            new DataVaultConstraintNameContext(DataVaultConstraintKind.PrimaryKey, tableName, participantColumnNames)),
        participantColumnNames);
    var traversalColumnNames = participantColumnNames.Reverse().ToArray();
    var indexes = new[]
    {
        new IndexProjection(
            NamingPolicy.GetIndexName(new DataVaultIndexNameContext(
                DataVaultIndexKind.BridgeTraversal,
                tableName,
                traversalColumnNames,
                IsUnique: false)),
            traversalColumnNames,
            IsUnique: false),
    };

    return new EntityProjection(
        tableName,
        DataVaultTableKind.Bridge,
        bridge.Name,
        ParentReference: null,
        properties,
        primaryKey,
        indexes);
  }

  private static EntityProjection CreateHierarchyBridgeEntity(DataVaultBridgeMetadata bridge) {
    var tableName = GetBridgeTableName(bridge);
    var participantColumnNames = bridge.Endpoints
        .Select(GetBridgeEndpointHashKeyColumnName)
        .ToArray();
    var endpointColumns = bridge.Endpoints
        .Zip(participantColumnNames, (endpoint, columnName) => new EndpointColumn(endpoint.Role, columnName))
        .ToArray();
    // DataVaultBridgeMetadata validates hierarchy endpoint roles before translation.
    var ancestorColumnName = endpointColumns.First(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.Ancestor).ColumnName;
    var descendantColumnName = endpointColumns.First(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.Descendant).ColumnName;
    var traversalDepthColumnName = "TraversalDepth";
    var properties = bridge.Endpoints
        .Select((endpoint, index) => BridgeParticipantProperty(participantColumnNames[index], endpoint))
        .Concat([new PropertyProjection(
            traversalDepthColumnName,
            DataVaultPropertyRole.BridgeDepth,
            TechnicalRole: null,
            traversalDepthColumnName)])
        .ToArray();
    var primaryKey = new KeyProjection(
        NamingPolicy.GetConstraintName(
            new DataVaultConstraintNameContext(
                DataVaultConstraintKind.PrimaryKey,
                tableName,
                [ancestorColumnName, descendantColumnName])),
        [ancestorColumnName, descendantColumnName]);
    var indexes = new[]
    {
        new IndexProjection(
            NamingPolicy.GetIndexName(new DataVaultIndexNameContext(
                DataVaultIndexKind.BridgeTraversal,
                tableName,
                [ancestorColumnName, traversalDepthColumnName],
                IsUnique: false)),
            [ancestorColumnName, traversalDepthColumnName],
            IsUnique: false),
        new IndexProjection(
            NamingPolicy.GetIndexName(new DataVaultIndexNameContext(
                DataVaultIndexKind.BridgeTraversal,
                tableName,
                [descendantColumnName, ancestorColumnName],
                IsUnique: false)),
            [descendantColumnName, ancestorColumnName],
            IsUnique: false),
    };

    return new EntityProjection(
        tableName,
        DataVaultTableKind.Bridge,
        bridge.Name,
        ParentReference: null,
        properties,
        primaryKey,
        indexes);
  }

  private static string GetBridgeTableName(DataVaultBridgeMetadata bridge) {
    return "Bridge" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(bridge.Name);
  }

  private static string GetBridgeEndpointHashKeyColumnName(DataVaultBridgeEndpointMetadata endpoint) {
    var baseName = endpoint.Role switch {
      DataVaultBridgeEndpointRole.Ancestor => "Ancestor" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(endpoint.HubReference.Name),
      DataVaultBridgeEndpointRole.Descendant => "Descendant" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(endpoint.HubReference.Name),
      _ => endpoint.HubReference.Name,
    };

    return DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(baseName) + "HashKey";
  }

  private static PropertyProjection BridgeParticipantProperty(
      string columnName,
      DataVaultBridgeEndpointMetadata endpoint) {
    return new PropertyProjection(
        columnName,
        DataVaultPropertyRole.ParticipantReference,
        TechnicalMetadataColumnRole.HashKey,
        endpoint.SourceEndpointName);
  }

  private static EntityProjection CreatePitEntity(
      DataVaultPitMetadata pit,
      DataVaultMetadataModel metadataModel) {
    var parent = ResolvePitParent(pit, metadataModel.Hubs, metadataModel.Links);
    var satellites = ResolvePitSatellites(pit, metadataModel.Satellites, parent);
    var tableName = GetPitTableName(pit.Name);
    var parentHashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, parent.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, pit.Name, tableName));
    var drivingKeyNames = GetPitDrivingKeyNames(pit, satellites);
    var drivingKeyColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        drivingKeyNames,
        [parentHashKeyColumnName, loadTimestampColumnName]);
    var snapshotColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellites.Select(satellite => satellite.Name + " Load Timestamp"),
        [parentHashKeyColumnName, .. drivingKeyColumnNames, loadTimestampColumnName]);
    var properties = new List<PropertyProjection>
    {
        TechnicalProperty(parentHashKeyColumnName, TechnicalMetadataColumnRole.HashKey, parent.Name),
    };

    for (var index = 0; index < drivingKeyColumnNames.Count; index++) {
      properties.Add(new PropertyProjection(
          drivingKeyColumnNames[index],
          DataVaultPropertyRole.DrivingKey,
          TechnicalRole: null,
          drivingKeyNames[index]));
    }

    properties.Add(
        TechnicalProperty(
            loadTimestampColumnName,
            TechnicalMetadataColumnRole.LoadTimestamp,
            pit.LoadTimestampMetadata.EffectiveColumnName));

    for (var index = 0; index < snapshotColumnNames.Count; index++) {
      properties.Add(new PropertyProjection(
          snapshotColumnNames[index],
          DataVaultPropertyRole.SnapshotReference,
          TechnicalMetadataColumnRole.LoadTimestamp,
          satellites[index].Name));
    }

    var rowIdentityColumnNames = new[]
    {
        parentHashKeyColumnName,
    }
        .Concat(drivingKeyColumnNames)
        .Append(loadTimestampColumnName)
        .ToArray();
    var primaryKey = new KeyProjection(
        NamingPolicy.GetConstraintName(
            new DataVaultConstraintNameContext(
                DataVaultConstraintKind.PrimaryKey,
                tableName,
                rowIdentityColumnNames)),
        rowIdentityColumnNames);
    var indexes = new[]
    {
        new IndexProjection(
            NamingPolicy.GetIndexName(new DataVaultIndexNameContext(
                DataVaultIndexKind.PitTraversal,
                tableName,
                rowIdentityColumnNames,
                IsUnique: false)),
            rowIdentityColumnNames,
            IsUnique: false),
    };

    return new EntityProjection(
        tableName,
        DataVaultTableKind.Pit,
        pit.Name,
        pit.Parent,
        properties,
        primaryKey,
        indexes);
  }

  private static DataVaultMetadataReference ResolvePitParent(
      DataVaultPitMetadata pit,
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links) {
    if (pit.Parent.Kind == DataVaultMetadataReferenceKind.Hub) {
      var matches = hubs
          .Where(hub => string.Equals(hub.Name, pit.Parent.Name, StringComparison.Ordinal))
          .ToArray();

      return matches.Length switch {
        1 => matches[0].ToReference(),
        0 => throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references hub '" + pit.Parent.Name + "' that is not declared."),
        _ => throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references hub '" + pit.Parent.Name + "' more than once."),
      };
    }

    if (pit.Parent.Kind == DataVaultMetadataReferenceKind.Link) {
      var matches = links
          .Where(link => string.Equals(link.Name, pit.Parent.Name, StringComparison.Ordinal))
          .ToArray();

      return matches.Length switch {
        1 => matches[0].ToReference(),
        0 => throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references link '" + pit.Parent.Name + "' that is not declared."),
        _ => throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references link '" + pit.Parent.Name + "' more than once."),
      };
    }

    throw PitTranslationFailure(
        "PIT metadata '" + pit.Name + "' declares parent '" + pit.Parent.Name + "' as " + pit.Parent.Kind +
        "; supported PIT tables require a declared hub or link parent.");
  }

  private static IReadOnlyList<DataVaultSatelliteMetadata> ResolvePitSatellites(
      DataVaultPitMetadata pit,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      DataVaultMetadataReference parent) {
    if (pit.Satellites.Count == 0) {
      throw PitTranslationFailure(
          "PIT metadata '" + pit.Name + "' must declare at least one attached satellite.");
    }

    var availableSatellites = satellites.ToArray();
    var satelliteNames = new HashSet<string>(StringComparer.Ordinal);
    var resolvedSatellites = new List<DataVaultSatelliteMetadata>();

    foreach (var satelliteReference in pit.Satellites) {
      if (!satelliteNames.Add(satelliteReference.SatelliteName)) {
        throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' declares duplicate satellite reference '" +
            satelliteReference.SatelliteName + "'.");
      }

      var namedSatellites = availableSatellites
          .Where(satellite => string.Equals(satellite.Name, satelliteReference.SatelliteName, StringComparison.Ordinal))
          .ToArray();
      if (namedSatellites.Length == 0) {
        throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references satellite '" +
            satelliteReference.SatelliteName + "' that is not declared.");
      }

      var parentMatches = namedSatellites
          .Where(satellite => satellite.Parent.Kind == parent.Kind &&
              string.Equals(satellite.Parent.Name, parent.Name, StringComparison.Ordinal))
          .ToArray();
      var satellite = parentMatches.Length switch {
        1 => parentMatches[0],
        0 => throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references satellite '" + namedSatellites[0].Name +
            "' attached to " + namedSatellites[0].Parent.Kind + " '" + namedSatellites[0].Parent.Name +
            "' instead of declared " + parent.Kind + " '" + parent.Name + "'."),
        _ => throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references satellite '" +
            satelliteReference.SatelliteName + "' more than once under declared " + parent.Kind + " '" + parent.Name + "'."),
      };

      ValidatePitSatelliteMultiActiveReference(pit, satelliteReference, satellite);

      if (parent.Kind == DataVaultMetadataReferenceKind.Link &&
          (satelliteReference.IsMultiActive || satellite.DrivingKeyNames.Count > 0)) {
        throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references multi-active satellite '" +
            satelliteReference.SatelliteName + "' for link parent '" + parent.Name +
            "', which is outside the supported baseline.");
      }
      resolvedSatellites.Add(satellite);
    }

    ValidateSharedPitDrivingKeySet(pit, resolvedSatellites);

    return resolvedSatellites;
  }

  private static IReadOnlyList<string> GetPitDrivingKeyNames(
      DataVaultPitMetadata pit,
      IReadOnlyList<DataVaultSatelliteMetadata> satellites) {
    var drivingKeyNames = satellites
        .Where(satellite => satellite.DrivingKeyNames.Count > 0)
        .Select(satellite => satellite.DrivingKeyNames)
        .FirstOrDefault();

    if (drivingKeyNames is null) {
      return Array.Empty<string>();
    }

    ValidatePitDrivingKeyNames(pit, drivingKeyNames);
    return drivingKeyNames;
  }

  private static void ValidatePitSatelliteMultiActiveReference(
      DataVaultPitMetadata pit,
      DataVaultPitSatelliteReferenceMetadata satelliteReference,
      DataVaultSatelliteMetadata satellite) {
    var satelliteIsMultiActive = satellite.DrivingKeyNames.Count > 0;
    if (satelliteReference.IsMultiActive == satelliteIsMultiActive) {
      return;
    }

    throw PitTranslationFailure(
        "PIT metadata '" +
        pit.Name +
        "' reference metadata for satellite '" +
        satelliteReference.SatelliteName +
        "' declares IsMultiActive=" +
        satelliteReference.IsMultiActive +
        ", but resolved satellite metadata declares " +
        (satelliteIsMultiActive ? "multi-active driving keys" : "no driving keys") +
        ".");
  }

  private static void ValidateSharedPitDrivingKeySet(
      DataVaultPitMetadata pit,
      IReadOnlyList<DataVaultSatelliteMetadata> satellites) {
    IReadOnlyList<string>? drivingKeyNames = null;
    string? drivingKeySatelliteName = null;

    foreach (var satellite in satellites.Where(satellite => satellite.DrivingKeyNames.Count > 0)) {
      if (drivingKeyNames is null) {
        drivingKeyNames = satellite.DrivingKeyNames;
        drivingKeySatelliteName = satellite.Name;
        continue;
      }

      if (!drivingKeyNames.SequenceEqual(satellite.DrivingKeyNames, StringComparer.Ordinal)) {
        throw PitTranslationFailure(
            "PIT metadata '" +
            pit.Name +
            "' references multi-active satellite '" +
            satellite.Name +
            "' with driving-key names [" +
            string.Join(", ", satellite.DrivingKeyNames) +
            "] that do not match multi-active satellite '" +
            drivingKeySatelliteName +
            "' driving-key names [" +
            string.Join(", ", drivingKeyNames) +
            "] in canonical order.");
      }
    }
  }

  private static void ValidatePitDrivingKeyNames(
      DataVaultPitMetadata pit,
      IReadOnlyList<string> drivingKeyNames) {
    foreach (var drivingKeyName in drivingKeyNames) {
      if (string.Equals(drivingKeyName, DataVaultPitProjectionRow.ParentHashKeyName, StringComparison.Ordinal) ||
          string.Equals(drivingKeyName, DataVaultPitProjectionRow.LoadTimestampName, StringComparison.Ordinal)) {
        throw PitTranslationFailure(
            "PIT metadata '" +
            pit.Name +
            "' declares driving-key name '" +
            drivingKeyName +
            "' that collides with a reserved PIT row technical name.");
      }
    }
  }

  private static string GetPitTableName(string pitName) {
    return "Pit" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(pitName);
  }

  private static NotSupportedException PitTranslationFailure(string message) {
    return new NotSupportedException(
        "PIT metadata translation supports one declared hub with attached ordinary satellites or one shared multi-active driving-key family, or one declared link with attached non-multi-active satellites. " + message);
  }

  private static PropertyProjection TechnicalProperty(
      string name,
      TechnicalMetadataColumnRole technicalRole,
      string metadataName) {
    return new PropertyProjection(name, DataVaultPropertyRole.Technical, technicalRole, metadataName);
  }

  private static void ApplyEntity(
      ModelBuilder modelBuilder,
      EntityProjection entity,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultProviderIdentifierProjectionSet providerIdentifiers) {
    var defaultSchema = modelBuilder.Model.GetDefaultSchema();
    var physicalTableName = providerIdentifiers.GetPhysicalName(CreateTableIdentifierPath(entity));

    modelBuilder.SharedTypeEntity<Dictionary<string, object>>(entity.Name, entityBuilder => {
      if (defaultSchema is null) {
        entityBuilder.ToTable(physicalTableName);
      }
      else {
        entityBuilder.ToTable(physicalTableName, defaultSchema);
      }

      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, entity.Name);
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, entity.Kind);
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, entity.MetadataName);

      if (entity.ParentReference is not null) {
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceKind, entity.ParentReference.Kind);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceName, entity.ParentReference.Name);
      }

      for (var ordinal = 0; ordinal < entity.Properties.Count; ordinal++) {
        ApplyProperty(entityBuilder, entity, entity.Properties[ordinal], ordinal, providerCapabilities, providerIdentifiers);
      }

      var keyBuilder = entityBuilder.HasKey(entity.PrimaryKey.PropertyNames.ToArray());
      keyBuilder.HasName(providerIdentifiers.GetPhysicalName(CreatePrimaryKeyIdentifierPath(entity)));
      keyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, entity.PrimaryKey.Name);

      for (var ordinal = 0; ordinal < entity.Indexes.Count; ordinal++) {
        var index = entity.Indexes[ordinal];
        if (IsIndexCoveredByPrimaryKey(index, entity.PrimaryKey, providerCapabilities)) {
          continue;
        }

        var indexPropertyNames = GetEffectiveIndexPropertyNames(index, providerCapabilities);
        var indexBuilder = entityBuilder.HasIndex(indexPropertyNames.ToArray());
        indexBuilder.IsUnique(index.IsUnique);
        if (index.DescendingPropertyNames.Count > 0) {
          indexBuilder.IsDescending(indexPropertyNames
              .Select(propertyName => index.DescendingPropertyNames.Contains(propertyName, StringComparer.Ordinal))
              .ToArray());
        }

        if (SupportsIncludedIndexProperties(providerCapabilities) && index.IncludedPropertyNames.Count > 0) {
          indexBuilder.Metadata.SetAnnotation(
              DataVaultInternalAnnotationNames.ProviderIncludedIndexPropertyNames,
              index.IncludedPropertyNames.ToArray());
          ApplyIncludedIndexProperties(indexBuilder, index.IncludedPropertyNames, providerCapabilities);
        }

        indexBuilder.HasDatabaseName(providerIdentifiers.GetPhysicalName(CreateIndexIdentifierPath(entity, index)));
        indexBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, index.Name);
        indexBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, ordinal);
      }
    });
  }

  private static bool IsIndexCoveredByPrimaryKey(
      IndexProjection index,
      KeyProjection primaryKey,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var effectiveIndexPropertyNames = GetEffectiveIndexPropertyNames(index, providerCapabilities);
    var hasEffectiveIncludedProperties =
        index.IncludedPropertyNames.Count > 0 &&
        (SupportsIncludedIndexProperties(providerCapabilities) ||
            providerCapabilities.UnsupportedIncludedIndexColumnMode == DataVaultUnsupportedIncludedIndexColumnMode.AppendToKey);

    return !providerCapabilities.AllowsIndexesCoveredByPrimaryKey &&
        index.DescendingPropertyNames.Count == 0 &&
        !hasEffectiveIncludedProperties &&
        effectiveIndexPropertyNames.SequenceEqual(primaryKey.PropertyNames, StringComparer.Ordinal);
  }

  private static IReadOnlyList<string> GetEffectiveIndexPropertyNames(
      IndexProjection index,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    if (SupportsIncludedIndexProperties(providerCapabilities) || index.IncludedPropertyNames.Count == 0) {
      return index.PropertyNames;
    }

    if (providerCapabilities.UnsupportedIncludedIndexColumnMode == DataVaultUnsupportedIncludedIndexColumnMode.Ignore) {
      return index.PropertyNames;
    }

    return index.PropertyNames
        .Concat(index.IncludedPropertyNames)
        .Distinct(StringComparer.Ordinal)
        .ToArray();
  }

  private static bool SupportsIncludedIndexProperties(DataVaultProviderCapabilityProfile providerCapabilities) {
    return providerCapabilities.ProfileName.StartsWith("sqlserver-", StringComparison.Ordinal) ||
        providerCapabilities.ProfileName.StartsWith("postgres-", StringComparison.Ordinal);
  }

  private static void ApplyIncludedIndexProperties(
      IndexBuilder indexBuilder,
      IReadOnlyList<string> includedPropertyNames,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    if (providerCapabilities.ProfileName.StartsWith("sqlserver-", StringComparison.Ordinal)) {
      InvokeProviderIndexExtension(
          "Microsoft.EntityFrameworkCore.SqlServerIndexBuilderExtensions, Microsoft.EntityFrameworkCore.SqlServer",
          indexBuilder,
          includedPropertyNames);
      return;
    }

    if (providerCapabilities.ProfileName.StartsWith("postgres-", StringComparison.Ordinal)) {
      InvokeProviderIndexExtension(
          "Microsoft.EntityFrameworkCore.NpgsqlIndexBuilderExtensions, Npgsql.EntityFrameworkCore.PostgreSQL",
          indexBuilder,
          includedPropertyNames);
    }
  }

  private static void InvokeProviderIndexExtension(
      string extensionTypeName,
      IndexBuilder indexBuilder,
      IReadOnlyList<string> includedPropertyNames) {
    var extensionType = Type.GetType(extensionTypeName, throwOnError: false);
    var includePropertiesMethod = extensionType?
        .GetMethods()
        .Where(method => string.Equals(method.Name, "IncludeProperties", StringComparison.Ordinal))
        .Select(method => new {
          Method = method,
          Parameters = method.GetParameters(),
        })
        .Where(candidate =>
            candidate.Parameters.Length == 2 &&
            candidate.Parameters[0].ParameterType.IsAssignableFrom(typeof(IndexBuilder)) &&
            candidate.Parameters[1].ParameterType == typeof(string[]))
        .Select(candidate => candidate.Method)
        .FirstOrDefault();

    if (includePropertiesMethod is null) {
      return;
    }

    includePropertiesMethod.Invoke(null, [indexBuilder, includedPropertyNames.ToArray()]);
  }

  private static void ApplyProperty(
      EntityTypeBuilder<Dictionary<string, object>> entityBuilder,
      EntityProjection entity,
      PropertyProjection property,
      int ordinal,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultProviderIdentifierProjectionSet providerIdentifiers) {
    var logicalPropertyKind = GetLogicalPropertyKind(property);
    var typeMapping = providerCapabilities.GetRequiredTypeMapping(logicalPropertyKind);
    var propertyBuilder = CreateIndexerProperty(entityBuilder, property, providerCapabilities, typeMapping);

    if (property.Role == DataVaultPropertyRole.SnapshotReference) {
      propertyBuilder.IsRequired(false);
    }

    propertyBuilder.HasColumnName(providerIdentifiers.GetPhysicalName(CreateColumnIdentifierPath(entity, property)));
    propertyBuilder.HasColumnType(typeMapping.NativeStoreType);
    propertyBuilder.HasColumnOrder(ordinal);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, property.Name);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, property.Role);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, property.MetadataName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, ordinal);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderProfile, providerCapabilities.ProfileName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderLogicalPropertyKind, logicalPropertyKind);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderStorageType, typeMapping.NativeStoreType);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderValueFormat, typeMapping.ValueFormat);
    if (typeMapping.HashKeyStorageProfile is not null) {
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.HashKeyStorageProfile, typeMapping.HashKeyStorageProfile.Value);
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.StableHashAlgorithmId, typeMapping.StableHashAlgorithmId);
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.StableHashDigestByteLength, typeMapping.DigestByteLength);
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.StableHashDigestEncoding, typeMapping.DigestEncoding);
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.HashKeyConversionBehavior, typeMapping.ConversionBehavior);
    }

    if (property.TechnicalRole is not null) {
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, property.TechnicalRole);
    }
  }

  private static PropertyBuilder CreateIndexerProperty(
      EntityTypeBuilder<Dictionary<string, object>> entityBuilder,
      PropertyProjection property,
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultProviderTypeMapping typeMapping) {
    if (typeMapping.ModelClrType == typeof(DateTimeOffset)) {
      return property.Role == DataVaultPropertyRole.SnapshotReference
          ? entityBuilder.IndexerProperty<DateTimeOffset?>(property.Name)
          : entityBuilder.IndexerProperty<DateTimeOffset>(property.Name);
    }

    if (typeMapping.ModelClrType == typeof(string)) {
      var propertyBuilder = entityBuilder.IndexerProperty<string>(property.Name);
      if (typeMapping.ValueFormat == DataVaultProviderValueFormat.LowercaseHexBinary) {
        propertyBuilder.HasConversion(new LowercaseHexStringToBytesConverter(
            typeMapping.DigestByteLength ??
            throw new InvalidOperationException(
                "Binary hash-key conversion requires a declared stable-hash digest byte length.")));
      }

      return propertyBuilder;
    }

    if (typeMapping.ModelClrType == typeof(int)) {
      return property.Role == DataVaultPropertyRole.SnapshotReference
          ? entityBuilder.IndexerProperty<int?>(property.Name)
          : entityBuilder.IndexerProperty<int>(property.Name);
    }

    if (typeMapping.ModelClrType == typeof(long)) {
      return property.Role == DataVaultPropertyRole.SnapshotReference
          ? entityBuilder.IndexerProperty<long?>(property.Name)
          : entityBuilder.IndexerProperty<long>(property.Name);
    }

    throw new NotSupportedException(
        "Provider capability profile '" +
        providerCapabilities.ProfileName +
        "' declares unsupported CLR type '" +
        typeMapping.ModelClrType.FullName +
        "' for required capability 'type mapping for " +
        typeMapping.LogicalPropertyKind +
        "'.");
  }

  private static DataVaultLogicalPropertyKind GetLogicalPropertyKind(PropertyProjection property) {
    return property.Role switch {
      DataVaultPropertyRole.BusinessKey => DataVaultLogicalPropertyKind.BusinessKey,
      DataVaultPropertyRole.ParticipantReference => DataVaultLogicalPropertyKind.ParticipantReference,
      DataVaultPropertyRole.DrivingKey => DataVaultLogicalPropertyKind.DrivingKey,
      DataVaultPropertyRole.Payload => DataVaultLogicalPropertyKind.PayloadText,
      DataVaultPropertyRole.SnapshotReference => DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
      DataVaultPropertyRole.BridgeDepth => DataVaultLogicalPropertyKind.BridgeDepth,
      DataVaultPropertyRole.Technical => GetTechnicalLogicalPropertyKind(property),
      _ => throw new ArgumentOutOfRangeException(nameof(property), property.Role, "Unsupported Data Vault property role."),
    };
  }

  private static DataVaultLogicalPropertyKind GetTechnicalLogicalPropertyKind(PropertyProjection property) {
    if (property.TechnicalRole is null) {
      throw new InvalidOperationException("Technical Data Vault properties must declare a technical metadata role.");
    }

    return property.TechnicalRole.Value switch {
      TechnicalMetadataColumnRole.HashKey => DataVaultLogicalPropertyKind.HashKey,
      TechnicalMetadataColumnRole.HashDiff => DataVaultLogicalPropertyKind.HashDiff,
      TechnicalMetadataColumnRole.LoadTimestamp => DataVaultLogicalPropertyKind.LoadTimestamp,
      TechnicalMetadataColumnRole.RecordSource => DataVaultLogicalPropertyKind.RecordSource,
      _ => throw new ArgumentOutOfRangeException(nameof(property), property.TechnicalRole, "Unsupported technical metadata role."),
    };
  }

  private sealed record EntityProjection(
      string Name,
      DataVaultTableKind Kind,
      string MetadataName,
      DataVaultMetadataReference? ParentReference,
      IReadOnlyList<PropertyProjection> Properties,
      KeyProjection PrimaryKey,
      IReadOnlyList<IndexProjection> Indexes);

  private sealed record PropertyProjection(
      string Name,
      DataVaultPropertyRole Role,
      TechnicalMetadataColumnRole? TechnicalRole,
      string MetadataName);

  private sealed record KeyProjection(string Name, IReadOnlyList<string> PropertyNames);

  private sealed record IndexProjection(
      string Name,
      IReadOnlyList<string> PropertyNames,
      bool IsUnique,
      IReadOnlyList<string>? DescendingPropertyNames = null,
      IReadOnlyList<string>? IncludedPropertyNames = null) {
    public IReadOnlyList<string> DescendingPropertyNames { get; } = DescendingPropertyNames ?? [];

    public IReadOnlyList<string> IncludedPropertyNames { get; } = IncludedPropertyNames ?? [];
  }

  private readonly record struct EndpointColumn(DataVaultBridgeEndpointRole Role, string ColumnName);

  private sealed class LowercaseHexStringToBytesConverter : ValueConverter<string, byte[]> {
    public LowercaseHexStringToBytesConverter(int digestByteLength)
        : base(
            value => DataVaultHashKeyProviderValueConverter.ConvertCanonicalHexToBytes(value, digestByteLength),
            value => DataVaultHashKeyProviderValueConverter.ConvertBytesToCanonicalHex(value, digestByteLength)) {
      if (digestByteLength <= 0) {
        throw new ArgumentOutOfRangeException(nameof(digestByteLength));
      }
    }
  }
}
