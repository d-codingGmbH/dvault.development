using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DCoding.Data.DVault;

internal static class DataVaultEfMetadataTranslator {
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public static void Apply(ModelBuilder modelBuilder, DataVaultMetadataModel metadataModel) {
    foreach (var entity in CreateEntities(metadataModel)) {
      ApplyEntity(modelBuilder, entity);
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

    var payloadColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellite.PayloadColumns.Select(column => column.ColumnName),
        [parentHashKeyColumnName, hashDiffColumnName, loadTimestampColumnName, recordSourceColumnName]);
    var properties = new List<PropertyProjection>
    {
        TechnicalProperty(parentHashKeyColumnName, TechnicalMetadataColumnRole.HashKey, satellite.Parent.Name),
        TechnicalProperty(hashDiffColumnName, TechnicalMetadataColumnRole.HashDiff, satellite.HashDiffMetadata.EffectiveColumnName),
        TechnicalProperty(
            loadTimestampColumnName,
            TechnicalMetadataColumnRole.LoadTimestamp,
            satellite.LoadTimestampMetadata.EffectiveColumnName),
        TechnicalProperty(
            recordSourceColumnName,
            TechnicalMetadataColumnRole.RecordSource,
            satellite.RecordSourceMetadata.EffectiveColumnName),
    };

    for (var index = 0; index < payloadColumnNames.Count; index++) {
      properties.Add(new PropertyProjection(
          payloadColumnNames[index],
          DataVaultPropertyRole.Payload,
          TechnicalRole: null,
          satellite.PayloadColumns[index].ColumnName));
    }

    var indexes = new[]
    {
        new IndexProjection(
            NamingPolicy.GetIndexName(new DataVaultIndexNameContext(
                DataVaultIndexKind.SatelliteParent,
                tableName,
                [parentHashKeyColumnName],
                IsUnique: false)),
            [parentHashKeyColumnName],
            IsUnique: false),
    };
    var primaryKey = new KeyProjection(
        NamingPolicy.GetConstraintName(
            new DataVaultConstraintNameContext(
                DataVaultConstraintKind.PrimaryKey,
                tableName,
                [parentHashKeyColumnName, loadTimestampColumnName])),
        [parentHashKeyColumnName, loadTimestampColumnName]);

    return new EntityProjection(
        tableName,
        DataVaultTableKind.Satellite,
        satellite.Name,
        satellite.Parent,
        properties,
        primaryKey,
        indexes);
  }

  private static PropertyProjection TechnicalProperty(
      string name,
      TechnicalMetadataColumnRole technicalRole,
      string metadataName) {
    return new PropertyProjection(name, DataVaultPropertyRole.Technical, technicalRole, metadataName);
  }

  private static void ApplyEntity(ModelBuilder modelBuilder, EntityProjection entity) {
    modelBuilder.SharedTypeEntity<Dictionary<string, object>>(entity.Name, entityBuilder => {
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, entity.Name);
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, entity.Kind);
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, entity.MetadataName);

      if (entity.ParentReference is not null) {
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceKind, entity.ParentReference.Kind);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceName, entity.ParentReference.Name);
      }

      for (var ordinal = 0; ordinal < entity.Properties.Count; ordinal++) {
        ApplyProperty(entityBuilder, entity.Properties[ordinal], ordinal);
      }

      var keyBuilder = entityBuilder.HasKey(entity.PrimaryKey.PropertyNames.ToArray());
      keyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, entity.PrimaryKey.Name);

      for (var ordinal = 0; ordinal < entity.Indexes.Count; ordinal++) {
        var index = entity.Indexes[ordinal];
        var indexBuilder = entityBuilder.HasIndex(index.PropertyNames.ToArray());
        indexBuilder.IsUnique(index.IsUnique);
        indexBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, index.Name);
        indexBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, ordinal);
      }
    });
  }

  private static void ApplyProperty(
      EntityTypeBuilder<Dictionary<string, object>> entityBuilder,
      PropertyProjection property,
      int ordinal) {
    PropertyBuilder propertyBuilder = property.TechnicalRole == TechnicalMetadataColumnRole.LoadTimestamp
        ? entityBuilder.IndexerProperty<DateTimeOffset>(property.Name)
        : entityBuilder.IndexerProperty<string>(property.Name);

    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, property.Name);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, property.Role);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, property.MetadataName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, ordinal);

    if (property.TechnicalRole is not null) {
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, property.TechnicalRole);
    }
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

  private sealed record IndexProjection(string Name, IReadOnlyList<string> PropertyNames, bool IsUnique);
}
