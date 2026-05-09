using System.Security.Cryptography;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    foreach (var entity in entities) {
      ApplyEntity(modelBuilder, entity, providerCapabilities);
    }
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
        .Select(participant => participant.HubReference.Name)
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
    var hub = ResolvePitHub(pit, metadataModel.Hubs);
    var satellites = ResolvePitSatellites(pit, metadataModel.Satellites, hub);
    var tableName = GetPitTableName(hub.Name, satellites);
    var parentHashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, hub.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, pit.Name, tableName));
    var snapshotColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellites.Select(satellite => satellite.Name + " Load Timestamp"),
        [parentHashKeyColumnName, loadTimestampColumnName]);
    var properties = new List<PropertyProjection>
    {
        TechnicalProperty(parentHashKeyColumnName, TechnicalMetadataColumnRole.HashKey, hub.Name),
        TechnicalProperty(
            loadTimestampColumnName,
            TechnicalMetadataColumnRole.LoadTimestamp,
            pit.LoadTimestampMetadata.EffectiveColumnName),
    };

    for (var index = 0; index < snapshotColumnNames.Count; index++) {
      properties.Add(new PropertyProjection(
          snapshotColumnNames[index],
          DataVaultPropertyRole.SnapshotReference,
          TechnicalMetadataColumnRole.LoadTimestamp,
          satellites[index].Name));
    }

    var primaryKey = new KeyProjection(
        NamingPolicy.GetConstraintName(
            new DataVaultConstraintNameContext(
                DataVaultConstraintKind.PrimaryKey,
                tableName,
                [parentHashKeyColumnName, loadTimestampColumnName])),
        [parentHashKeyColumnName, loadTimestampColumnName]);

    return new EntityProjection(
        tableName,
        DataVaultTableKind.Pit,
        pit.Name,
        pit.Parent,
        properties,
        primaryKey,
        []);
  }

  private static DataVaultHubMetadata ResolvePitHub(
      DataVaultPitMetadata pit,
      IEnumerable<DataVaultHubMetadata> hubs) {
    if (pit.Parent.Kind != DataVaultMetadataReferenceKind.Hub) {
      throw PitTranslationFailure(
          "PIT metadata '" + pit.Name + "' declares parent '" + pit.Parent.Name + "' as " + pit.Parent.Kind +
          "; link-based PIT tables are outside the supported baseline.");
    }

    var matches = hubs
        .Where(hub => string.Equals(hub.Name, pit.Parent.Name, StringComparison.Ordinal))
        .ToArray();

    return matches.Length switch {
      1 => matches[0],
      0 => throw PitTranslationFailure(
          "PIT metadata '" + pit.Name + "' references hub '" + pit.Parent.Name + "' that is not declared."),
      _ => throw PitTranslationFailure(
          "PIT metadata '" + pit.Name + "' references hub '" + pit.Parent.Name + "' more than once."),
    };
  }

  private static IReadOnlyList<DataVaultSatelliteMetadata> ResolvePitSatellites(
      DataVaultPitMetadata pit,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      DataVaultHubMetadata hub) {
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

      if (satelliteReference.IsMultiActive) {
        throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references multi-active satellite '" +
            satelliteReference.SatelliteName + "', which is outside the supported baseline.");
      }

      var matches = availableSatellites
          .Where(satellite => string.Equals(satellite.Name, satelliteReference.SatelliteName, StringComparison.Ordinal))
          .ToArray();
      var satellite = matches.Length switch {
        1 => matches[0],
        0 => throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references satellite '" +
            satelliteReference.SatelliteName + "' that is not declared."),
        _ => throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references satellite '" +
            satelliteReference.SatelliteName + "' more than once."),
      };

      if (satellite.DrivingKeyNames.Count > 0) {
        throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references multi-active satellite '" +
            satelliteReference.SatelliteName + "', which is outside the supported baseline.");
      }

      if (satellite.Parent.Kind != DataVaultMetadataReferenceKind.Hub) {
        throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references satellite '" + satellite.Name +
            "' attached to " + satellite.Parent.Kind + " '" + satellite.Parent.Name +
            "'; link-based PIT tables are outside the supported baseline.");
      }

      if (!string.Equals(satellite.Parent.Name, hub.Name, StringComparison.Ordinal)) {
        throw PitTranslationFailure(
            "PIT metadata '" + pit.Name + "' references satellite '" + satellite.Name +
            "' that is attached to hub '" + satellite.Parent.Name +
            "' instead of declared hub '" + hub.Name + "'.");
      }

      resolvedSatellites.Add(satellite);
    }

    return resolvedSatellites;
  }

  private static string GetPitTableName(
      string hubName,
      IEnumerable<DataVaultSatelliteMetadata> satellites) {
    var namingPolicy = DefaultNamingPolicy.Instance;

    return "Pit" + namingPolicy.NormalizeProducedIdentifier(hubName) +
        string.Concat(satellites.Select(satellite => namingPolicy.NormalizeProducedIdentifier(satellite.Name)));
  }

  private static NotSupportedException PitTranslationFailure(string message) {
    return new NotSupportedException(
        "PIT metadata translation supports only one hub plus attached non-multi-active satellites. " + message);
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
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var defaultSchema = modelBuilder.Model.GetDefaultSchema();

    modelBuilder.SharedTypeEntity<Dictionary<string, object>>(entity.Name, entityBuilder => {
      if (defaultSchema is null) {
        entityBuilder.ToTable(entity.Name);
      }
      else {
        entityBuilder.ToTable(entity.Name, defaultSchema);
      }

      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, entity.Name);
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, entity.Kind);
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, entity.MetadataName);

      if (entity.ParentReference is not null) {
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceKind, entity.ParentReference.Kind);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceName, entity.ParentReference.Name);
      }

      for (var ordinal = 0; ordinal < entity.Properties.Count; ordinal++) {
        ApplyProperty(entityBuilder, entity.Properties[ordinal], ordinal, providerCapabilities);
      }

      var keyBuilder = entityBuilder.HasKey(entity.PrimaryKey.PropertyNames.ToArray());
      keyBuilder.HasName(GetPhysicalIdentifierName(entity.PrimaryKey.Name, providerCapabilities));
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
          ApplyIncludedIndexProperties(indexBuilder, index.IncludedPropertyNames, providerCapabilities);
        }

        indexBuilder.HasDatabaseName(GetPhysicalIdentifierName(index.Name, providerCapabilities));
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

  private static string GetPhysicalIdentifierName(
      string producedName,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    if (providerCapabilities.MaximumIdentifierLength is not { } maximumIdentifierLength ||
        producedName.Length <= maximumIdentifierLength) {
      return producedName;
    }

    const int hashLength = 8;
    var prefixLength = Math.Max(1, maximumIdentifierLength - hashLength - 1);
    var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(producedName)))
        .ToLowerInvariant()[..hashLength];

    return producedName[..prefixLength] + "_" + hash;
  }

  private static void ApplyProperty(
      EntityTypeBuilder<Dictionary<string, object>> entityBuilder,
      PropertyProjection property,
      int ordinal,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var logicalPropertyKind = GetLogicalPropertyKind(property);
    var typeMapping = providerCapabilities.GetRequiredTypeMapping(logicalPropertyKind);
    var propertyBuilder = CreateIndexerProperty(entityBuilder, property, providerCapabilities, typeMapping);

    propertyBuilder.HasColumnName(property.Name);
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
      return entityBuilder.IndexerProperty<DateTimeOffset>(property.Name);
    }

    if (typeMapping.ModelClrType == typeof(string)) {
      return entityBuilder.IndexerProperty<string>(property.Name);
    }

    if (typeMapping.ModelClrType == typeof(int)) {
      return entityBuilder.IndexerProperty<int>(property.Name);
    }

    if (typeMapping.ModelClrType == typeof(long)) {
      return entityBuilder.IndexerProperty<long>(property.Name);
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
}
