using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Compares an expected Data Vault metadata model with generated/current EF metadata without database access.
/// </summary>
public static class DataVaultModelDriftReporter {
  /// <summary>
  /// Compares expected Data Vault metadata with current EF metadata using the default SQLite capability profile.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="currentModel">The generated/current EF model metadata to compare.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      IReadOnlyModel currentModel) {
    return Compare(expectedMetadataModel, currentModel, DataVaultProviderCapabilityProfiles.Sqlite);
  }

  /// <summary>
  /// Compares expected Data Vault metadata with current EF metadata using an explicit provider capability profile.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="currentModel">The generated/current EF model metadata to compare.</param>
  /// <param name="providerCapabilities">The provider capability profile expected for generated storage metadata.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      IReadOnlyModel currentModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(expectedMetadataModel);
    ArgumentNullException.ThrowIfNull(currentModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var expectedModel = BuildExpectedModel(expectedMetadataModel, providerCapabilities);
    return CompareSnapshots(CreateSnapshot(expectedModel), CreateSnapshot(currentModel));
  }

  /// <summary>
  /// Compares a successful model-first import result with current EF metadata using an explicit provider capability profile.
  /// </summary>
  /// <param name="expectedImport">The expected successful dvault.model.v1 import result.</param>
  /// <param name="currentModel">The generated/current EF model metadata to compare.</param>
  /// <param name="providerCapabilities">The provider capability profile expected for generated storage metadata.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultModelImportResult expectedImport,
      IReadOnlyModel currentModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(expectedImport);
    ArgumentNullException.ThrowIfNull(currentModel);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    expectedImport.ThrowIfInvalid();
    if (expectedImport.MetadataRegistry is null) {
      throw new InvalidOperationException("The Data Vault model import result does not contain an expected metadata registry.");
    }

    var expectedModel = BuildExpectedModel(
        expectedImport,
        providerCapabilities.WithLoadTimestampStorage(expectedImport.LoadTimestampStorage));
    return CompareSnapshots(CreateSnapshot(expectedModel), CreateSnapshot(currentModel));
  }

  /// <summary>
  /// Compares expected Data Vault metadata with the design-time EF model metadata from a DbContext.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="currentContext">The current DbContext whose design-time model metadata should be compared.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DbContext currentContext) {
    ArgumentNullException.ThrowIfNull(currentContext);

    return Compare(
        expectedMetadataModel,
        currentContext.GetService<IDesignTimeModel>().Model,
        DataVaultProviderCapabilityProfileSelection.Select(currentContext.Database.ProviderName));
  }

  /// <summary>
  /// Compares a successful model-first import result with the design-time EF model metadata from a DbContext.
  /// </summary>
  /// <param name="expectedImport">The expected successful dvault.model.v1 import result.</param>
  /// <param name="currentContext">The current DbContext whose design-time model metadata should be compared.</param>
  /// <returns>A deterministic structured and displayable drift report.</returns>
  public static DataVaultModelDriftReport Compare(
      DataVaultModelImportResult expectedImport,
      DbContext currentContext) {
    ArgumentNullException.ThrowIfNull(currentContext);

    return Compare(
        expectedImport,
        currentContext.GetService<IDesignTimeModel>().Model,
        DataVaultProviderCapabilityProfileSelection.Select(currentContext.Database.ProviderName));
  }

  private static IReadOnlyModel BuildExpectedModel(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var modelBuilder = new ModelBuilder(new ConventionSet());
    modelBuilder.ApplyDataVaultMetadata(metadataModel, providerCapabilities);

    return modelBuilder.Model;
  }

  private static IReadOnlyModel BuildExpectedModel(
      DataVaultModelImportResult importResult,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var modelBuilder = new ModelBuilder(new ConventionSet());
    modelBuilder.ApplyDataVaultMetadata(importResult, providerCapabilities);

    return modelBuilder.Model;
  }

  private static DataVaultModelDriftReport CompareSnapshots(
      ModelSnapshot expected,
      ModelSnapshot actual) {
    var differences = new List<DataVaultModelDriftDifference>();

    AddModelAnnotationDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "provider-profile-mismatch",
        "providerProfile",
        expected.ProviderProfile,
        actual.ProviderProfile,
        "The current EF model was generated for a different Data Vault provider capability profile.");
    AddModelAnnotationDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "metadata-source-kind-mismatch",
        "metadataSource.kind",
        expected.MetadataSourceKind,
        actual.MetadataSourceKind,
        "The current EF model carries a different Data Vault metadata source kind.");
    AddModelAnnotationDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "metadata-source-fingerprint-mismatch",
        "metadataSource.fingerprint",
        expected.MetadataSourceFingerprint,
        actual.MetadataSourceFingerprint,
        "The current EF model carries a different Data Vault metadata source fingerprint.");

    var matchedActualEntities = new bool[actual.Entities.Count];
    foreach (var expectedEntity in expected.Entities) {
      var actualEntityIndex = FindEntity(expectedEntity, actual.Entities, matchedActualEntities);
      if (actualEntityIndex is null) {
        AddDifference(
            differences,
            DataVaultModelDriftSeverity.Blocking,
            "missing-entity",
            DataVaultModelDriftElementKind.Entity,
            expectedEntity.LogicalName,
            expectedEntity.ProducedName,
            "entity",
            expectedEntity.Kind.ToString(),
            "<missing>",
            "The expected Data Vault entity is missing from the current EF model.");
        continue;
      }

      matchedActualEntities[actualEntityIndex.Value] = true;
      CompareEntity(differences, expectedEntity, actual.Entities[actualEntityIndex.Value]);
    }

    for (var index = 0; index < actual.Entities.Count; index++) {
      if (matchedActualEntities[index]) {
        continue;
      }

      var actualEntity = actual.Entities[index];
      AddDifference(
          differences,
          DataVaultModelDriftSeverity.Informational,
          "unexpected-entity",
          DataVaultModelDriftElementKind.Entity,
          actualEntity.LogicalName,
          actualEntity.ProducedName,
          "entity",
          "<missing>",
          actualEntity.Kind.ToString(),
          "The current EF model contains an additional Data Vault entity that is not in the expected model.");
    }

    return new DataVaultModelDriftReport(SortDifferences(differences));
  }

  private static void CompareEntity(
      ICollection<DataVaultModelDriftDifference> differences,
      EntitySnapshot expected,
      EntitySnapshot actual) {
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "entity-kind-mismatch",
        DataVaultModelDriftElementKind.Entity,
        expected.LogicalName,
        expected.ProducedName,
        "entity.kind",
        expected.Kind.ToString(),
        actual.Kind.ToString(),
        "The current EF entity has a different Data Vault table kind.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "entity-produced-name-mismatch",
        DataVaultModelDriftElementKind.Entity,
        expected.LogicalName,
        expected.ProducedName,
        "entity.producedName",
        expected.ProducedName,
        actual.ProducedName,
        "The current EF entity has a different produced physical name.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "entity-metadata-name-mismatch",
        DataVaultModelDriftElementKind.Entity,
        expected.LogicalName,
        expected.ProducedName,
        "entity.metadataName",
        expected.MetadataName,
        actual.MetadataName,
        "The current EF entity carries a different logical metadata name annotation.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "entity-parent-reference-mismatch",
        DataVaultModelDriftElementKind.Entity,
        expected.LogicalName,
        expected.ProducedName,
        "entity.parentReference",
        expected.ParentReference,
        actual.ParentReference,
        "The current EF entity has a different Data Vault parent reference.");

    CompareProperties(differences, expected, actual);
    ComparePrimaryKey(differences, expected, actual);
    CompareIndexes(differences, expected, actual);
  }

  private static void CompareProperties(
      ICollection<DataVaultModelDriftDifference> differences,
      EntitySnapshot expectedEntity,
      EntitySnapshot actualEntity) {
    var matchedActualProperties = new bool[actualEntity.Properties.Count];
    foreach (var expectedProperty in expectedEntity.Properties) {
      var actualPropertyIndex = FindProperty(expectedProperty, actualEntity.Properties, matchedActualProperties);
      if (actualPropertyIndex is null) {
        AddDifference(
            differences,
            DataVaultModelDriftSeverity.Blocking,
            "missing-property",
            DataVaultModelDriftElementKind.Property,
            expectedEntity.LogicalName + "." + expectedProperty.MetadataName,
            expectedProperty.ProducedName,
            "properties." + expectedProperty.MetadataName,
            FormatNullable(expectedProperty.Role),
            "<missing>",
            "The expected Data Vault property is missing from the current EF entity.");
        continue;
      }

      matchedActualProperties[actualPropertyIndex.Value] = true;
      CompareProperty(differences, expectedEntity, expectedProperty, actualEntity.Properties[actualPropertyIndex.Value]);
    }

    for (var index = 0; index < actualEntity.Properties.Count; index++) {
      if (matchedActualProperties[index]) {
        continue;
      }

      var actualProperty = actualEntity.Properties[index];
      AddDifference(
          differences,
          DataVaultModelDriftSeverity.Informational,
          "unexpected-property",
          DataVaultModelDriftElementKind.Property,
          actualEntity.LogicalName + "." + actualProperty.MetadataName,
          actualProperty.ProducedName,
          "properties." + actualProperty.MetadataName,
          "<missing>",
          FormatNullable(actualProperty.Role),
          "The current EF entity contains an additional Data Vault property that is not in the expected model.");
    }
  }

  private static void CompareProperty(
      ICollection<DataVaultModelDriftDifference> differences,
      EntitySnapshot expectedEntity,
      PropertySnapshot expected,
      PropertySnapshot actual) {
    var logicalName = expectedEntity.LogicalName + "." + expected.MetadataName;
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "property-produced-name-mismatch",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ProducedName,
        "properties." + expected.MetadataName + ".producedName",
        expected.ProducedName,
        actual.ProducedName,
        "The current EF property has a different produced physical name.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "property-metadata-name-mismatch",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ProducedName,
        "properties." + expected.MetadataName + ".metadataName",
        expected.MetadataName,
        actual.MetadataName,
        "The current EF property carries a different logical metadata name annotation.");
    AddNullableScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "property-role-mismatch",
        "property-role-unsupported-gap",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ProducedName,
        "properties." + expected.MetadataName + ".role",
        expected.Role,
        actual.Role,
        "The current EF property has an incompatible Data Vault property role.",
        "The current EF property does not expose the required Data Vault property role annotation.");
    AddNullableScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "technical-column-role-mismatch",
        "technical-column-role-unsupported-gap",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ProducedName,
        "properties." + expected.MetadataName + ".technicalRole",
        expected.TechnicalRole,
        actual.TechnicalRole,
        "The current EF property has an incompatible Data Vault technical column role.",
        "The current EF property does not expose the required Data Vault technical column role annotation.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "property-ordinal-mismatch",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ProducedName,
        "properties." + expected.MetadataName + ".ordinal",
        expected.Ordinal.ToString(CultureInfo.InvariantCulture),
        actual.Ordinal.ToString(CultureInfo.InvariantCulture),
        "The current EF property has a different declaration ordinal.");
    AddNullableScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "provider-logical-property-kind-mismatch",
        "provider-logical-property-kind-unsupported-gap",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ProducedName,
        "properties." + expected.MetadataName + ".providerLogicalPropertyKind",
        expected.LogicalPropertyKind,
        actual.LogicalPropertyKind,
        "The current EF property has an incompatible provider logical property kind.",
        "The current EF property does not expose the required provider logical property kind annotation.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        GetStorageMismatchCode(expected, actual),
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ProducedName,
        "properties." + expected.MetadataName + ".providerStorageType",
        expected.ProviderStorageType,
        actual.ProviderStorageType,
        "The current EF property has an incompatible provider storage type.");
    AddNullableScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        GetValueFormatMismatchCode(expected, actual),
        GetValueFormatGapCode(expected, actual),
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ProducedName,
        "properties." + expected.MetadataName + ".providerValueFormat",
        expected.ProviderValueFormat,
        actual.ProviderValueFormat,
        "The current EF property has an incompatible provider value format.",
        "The current EF property does not expose the required provider value format annotation.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "property-provider-profile-mismatch",
        DataVaultModelDriftElementKind.Property,
        logicalName,
        expected.ProducedName,
        "properties." + expected.MetadataName + ".providerProfile",
        expected.ProviderProfile,
        actual.ProviderProfile,
        "The current EF property was generated for a different provider capability profile.");
  }

  private static void ComparePrimaryKey(
      ICollection<DataVaultModelDriftDifference> differences,
      EntitySnapshot expectedEntity,
      EntitySnapshot actualEntity) {
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "primary-key-produced-name-mismatch",
        DataVaultModelDriftElementKind.Key,
        expectedEntity.LogicalName,
        expectedEntity.PrimaryKey.ProducedName,
        "primaryKey.producedName",
        expectedEntity.PrimaryKey.ProducedName,
        actualEntity.PrimaryKey.ProducedName,
        "The current EF primary key has a different produced physical name.");
    AddPropertyReferenceDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "primary-key-property-mismatch",
        DataVaultModelDriftElementKind.Key,
        expectedEntity.LogicalName,
        expectedEntity.PrimaryKey.ProducedName,
        "primaryKey.properties",
        expectedEntity.PrimaryKey.Properties,
        actualEntity.PrimaryKey.Properties,
        "The current EF primary key has an incompatible property set or order.");
  }

  private static void CompareIndexes(
      ICollection<DataVaultModelDriftDifference> differences,
      EntitySnapshot expectedEntity,
      EntitySnapshot actualEntity) {
    var matchedActualIndexes = new bool[actualEntity.Indexes.Count];
    foreach (var expectedIndex in expectedEntity.Indexes) {
      var actualIndexIndex = FindIndex(expectedIndex, actualEntity.Indexes, matchedActualIndexes);
      if (actualIndexIndex is null) {
        AddDifference(
            differences,
            DataVaultModelDriftSeverity.Blocking,
            "missing-index",
            DataVaultModelDriftElementKind.Index,
            expectedEntity.LogicalName,
            expectedIndex.ProducedName,
            "indexes." + expectedIndex.ProducedName,
            FormatIndexSignature(expectedIndex),
            "<missing>",
            "The expected Data Vault index is missing from the current EF entity.");
        continue;
      }

      matchedActualIndexes[actualIndexIndex.Value] = true;
      CompareIndex(differences, expectedEntity, expectedIndex, actualEntity.Indexes[actualIndexIndex.Value]);
    }

    for (var index = 0; index < actualEntity.Indexes.Count; index++) {
      if (matchedActualIndexes[index]) {
        continue;
      }

      var actualIndex = actualEntity.Indexes[index];
      AddDifference(
          differences,
          DataVaultModelDriftSeverity.Informational,
          "unexpected-index",
          DataVaultModelDriftElementKind.Index,
          actualEntity.LogicalName,
          actualIndex.ProducedName,
          "indexes." + actualIndex.ProducedName,
          "<missing>",
          FormatIndexSignature(actualIndex),
          "The current EF entity contains an additional Data Vault index that is not in the expected model.");
    }
  }

  private static void CompareIndex(
      ICollection<DataVaultModelDriftDifference> differences,
      EntitySnapshot expectedEntity,
      IndexSnapshot expected,
      IndexSnapshot actual) {
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "index-produced-name-mismatch",
        DataVaultModelDriftElementKind.Index,
        expectedEntity.LogicalName,
        expected.ProducedName,
        "indexes." + expected.ProducedName + ".producedName",
        expected.ProducedName,
        actual.ProducedName,
        "The current EF index has a different produced physical name.");
    AddPropertyReferenceDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "index-property-mismatch",
        DataVaultModelDriftElementKind.Index,
        expectedEntity.LogicalName,
        expected.ProducedName,
        "indexes." + expected.ProducedName + ".properties",
        expected.Properties,
        actual.Properties,
        "The current EF index has an incompatible property set or order.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "index-uniqueness-mismatch",
        DataVaultModelDriftElementKind.Index,
        expectedEntity.LogicalName,
        expected.ProducedName,
        "indexes." + expected.ProducedName + ".isUnique",
        expected.IsUnique.ToString(),
        actual.IsUnique.ToString(),
        "The current EF index has an incompatible uniqueness flag.");
    AddScalarDifference(
        differences,
        DataVaultModelDriftSeverity.Informational,
        "index-ordinal-mismatch",
        DataVaultModelDriftElementKind.Index,
        expectedEntity.LogicalName,
        expected.ProducedName,
        "indexes." + expected.ProducedName + ".ordinal",
        expected.Ordinal.ToString(CultureInfo.InvariantCulture),
        actual.Ordinal.ToString(CultureInfo.InvariantCulture),
        "The current EF index has a different declaration ordinal.");
    AddPropertyReferenceDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "index-descending-property-mismatch",
        DataVaultModelDriftElementKind.Index,
        expectedEntity.LogicalName,
        expected.ProducedName,
        "indexes." + expected.ProducedName + ".descendingProperties",
        expected.DescendingProperties,
        actual.DescendingProperties,
        "The current EF index has an incompatible descending property set.");
    AddPropertyReferenceDifference(
        differences,
        DataVaultModelDriftSeverity.Blocking,
        "index-included-property-mismatch",
        DataVaultModelDriftElementKind.Index,
        expectedEntity.LogicalName,
        expected.ProducedName,
        "indexes." + expected.ProducedName + ".includedProperties",
        expected.IncludedProperties,
        actual.IncludedProperties,
        "The current EF index has an incompatible included property set.");
  }

  private static int? FindEntity(
      EntitySnapshot expected,
      IReadOnlyList<EntitySnapshot> actualEntities,
      IReadOnlyList<bool> matched) {
    var exact = FindSingleIndex(
        actualEntities,
        matched,
        entity => entity.Kind == expected.Kind &&
            string.Equals(entity.MetadataName, expected.MetadataName, StringComparison.Ordinal));
    if (exact is not null) {
      return exact;
    }

    var byMetadataName = FindSingleIndex(
        actualEntities,
        matched,
        entity => string.Equals(entity.MetadataName, expected.MetadataName, StringComparison.Ordinal));
    if (byMetadataName is not null) {
      return byMetadataName;
    }

    return FindSingleIndex(
        actualEntities,
        matched,
        entity => string.Equals(entity.ProducedName, expected.ProducedName, StringComparison.Ordinal));
  }

  private static int? FindProperty(
      PropertySnapshot expected,
      IReadOnlyList<PropertySnapshot> actualProperties,
      IReadOnlyList<bool> matched) {
    var exact = FindSingleIndex(
        actualProperties,
        matched,
        property => property.Role == expected.Role &&
            property.TechnicalRole == expected.TechnicalRole &&
            string.Equals(property.MetadataName, expected.MetadataName, StringComparison.Ordinal));
    if (exact is not null) {
      return exact;
    }

    var byMetadataName = FindSingleIndex(
        actualProperties,
        matched,
        property => string.Equals(property.MetadataName, expected.MetadataName, StringComparison.Ordinal));
    if (byMetadataName is not null) {
      return byMetadataName;
    }

    return FindSingleIndex(
        actualProperties,
        matched,
        property => string.Equals(property.ProducedName, expected.ProducedName, StringComparison.Ordinal));
  }

  private static int? FindIndex(
      IndexSnapshot expected,
      IReadOnlyList<IndexSnapshot> actualIndexes,
      IReadOnlyList<bool> matched) {
    var byProducedName = FindSingleIndex(
        actualIndexes,
        matched,
        index => string.Equals(index.ProducedName, expected.ProducedName, StringComparison.Ordinal));
    if (byProducedName is not null) {
      return byProducedName;
    }

    var byOrdinal = FindSingleIndex(
        actualIndexes,
        matched,
        index => index.Ordinal == expected.Ordinal);
    if (byOrdinal is not null) {
      return byOrdinal;
    }

    return FindSingleIndex(actualIndexes, matched, index => HasSameIndexSignature(expected, index));
  }

  private static int? FindSingleIndex<T>(
      IReadOnlyList<T> values,
      IReadOnlyList<bool> matched,
      Func<T, bool> predicate) {
    int? foundIndex = null;
    for (var index = 0; index < values.Count; index++) {
      if (matched[index] || !predicate(values[index])) {
        continue;
      }

      if (foundIndex is not null) {
        return null;
      }

      foundIndex = index;
    }

    return foundIndex;
  }

  private static ModelSnapshot CreateSnapshot(IReadOnlyModel model) {
    var entities = model
        .GetEntityTypes()
        .Where(IsDataVaultEntity)
        .Select(CreateEntitySnapshot)
        .OrderBy(entity => GetEntityKindSortKey(entity.Kind))
        .ThenBy(entity => entity.MetadataName, StringComparer.Ordinal)
        .ThenBy(entity => entity.ProducedName, StringComparer.Ordinal)
        .ToArray();

    return new ModelSnapshot(
        GetStringAnnotation(model, DataVaultAnnotationNames.ProviderProfile),
        GetStringAnnotation(model, DataVaultAnnotationNames.MetadataSourceKind),
        GetStringAnnotation(model, DataVaultAnnotationNames.MetadataSourceFingerprint),
        entities);
  }

  private static EntitySnapshot CreateEntitySnapshot(IReadOnlyEntityType entityType) {
    var producedName = GetStringAnnotation(entityType, DataVaultAnnotationNames.ProducedName) ??
        entityType.GetTableName() ??
        entityType.Name;
    var kind = GetAnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind);
    var metadataName = GetStringAnnotation(entityType, DataVaultAnnotationNames.MetadataName) ?? producedName;
    var parentKind = GetNullableAnnotationValue<DataVaultMetadataReferenceKind>(
        entityType,
        DataVaultAnnotationNames.ParentReferenceKind);
    var parentName = GetStringAnnotation(entityType, DataVaultAnnotationNames.ParentReferenceName);
    var properties = entityType
        .GetProperties()
        .Select(CreatePropertySnapshot)
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.ProducedName, StringComparer.Ordinal)
        .ToArray();
    var primaryKey = entityType.FindPrimaryKey();
    var primaryKeySnapshot = primaryKey is null
        ? new KeySnapshot("<none>", Array.Empty<PropertyReferenceSnapshot>())
        : new KeySnapshot(
            GetStringAnnotation(primaryKey, DataVaultAnnotationNames.ProducedName) ??
                primaryKey.GetName() ??
                "Pk" + producedName,
            primaryKey.Properties.Select(CreatePropertyReferenceSnapshot).ToArray());
    var indexes = entityType
        .GetIndexes()
        .Select(CreateIndexSnapshot)
        .OrderBy(index => index.ProducedName, StringComparer.Ordinal)
        .ToArray();

    return new EntitySnapshot(
        kind,
        metadataName,
        producedName,
        parentKind.HasValue && parentName is not null ? parentKind.Value + ":" + parentName : null,
        properties,
        primaryKeySnapshot,
        indexes);
  }

  private static PropertySnapshot CreatePropertySnapshot(IReadOnlyProperty property) {
    return new PropertySnapshot(
        GetStringAnnotation(property, DataVaultAnnotationNames.ProducedName) ?? property.Name,
        GetNullableAnnotationValue<DataVaultPropertyRole>(property, DataVaultAnnotationNames.PropertyRole),
        GetNullableAnnotationValue<TechnicalMetadataColumnRole>(property, DataVaultAnnotationNames.TechnicalColumnRole),
        GetStringAnnotation(property, DataVaultAnnotationNames.MetadataName) ?? property.Name,
        GetNullableAnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal) ?? property.GetColumnOrder() ?? 0,
        GetNullableAnnotationValue<DataVaultLogicalPropertyKind>(property, DataVaultAnnotationNames.ProviderLogicalPropertyKind),
        GetStringAnnotation(property, DataVaultAnnotationNames.ProviderProfile) ?? string.Empty,
        GetStringAnnotation(property, DataVaultAnnotationNames.ProviderStorageType) ?? property.GetColumnType() ?? string.Empty,
        GetNullableAnnotationValue<DataVaultProviderValueFormat>(property, DataVaultAnnotationNames.ProviderValueFormat));
  }

  private static IndexSnapshot CreateIndexSnapshot(IReadOnlyIndex index) {
    var properties = index.Properties.Select(CreatePropertyReferenceSnapshot).ToArray();
    return new IndexSnapshot(
        GetStringAnnotation(index, DataVaultAnnotationNames.ProducedName) ??
            index.GetDatabaseName() ??
            string.Join("_", properties.Select(property => property.ProducedName)),
        properties,
        index.IsUnique,
        GetNullableAnnotationValue<int>(index, DataVaultAnnotationNames.Ordinal) ?? 0,
        GetDescendingProperties(index).ToArray(),
        GetIncludedProperties(index));
  }

  private static PropertyReferenceSnapshot CreatePropertyReferenceSnapshot(IReadOnlyProperty property) {
    return new PropertyReferenceSnapshot(
        GetStringAnnotation(property, DataVaultAnnotationNames.MetadataName) ?? property.Name,
        GetStringAnnotation(property, DataVaultAnnotationNames.ProducedName) ?? property.Name,
        GetNullableAnnotationValue<DataVaultPropertyRole>(property, DataVaultAnnotationNames.PropertyRole),
        GetNullableAnnotationValue<TechnicalMetadataColumnRole>(property, DataVaultAnnotationNames.TechnicalColumnRole));
  }

  private static IEnumerable<PropertyReferenceSnapshot> GetDescendingProperties(IReadOnlyIndex index) {
    if (index.IsDescending is null) {
      yield break;
    }

    for (var ordinal = 0; ordinal < index.Properties.Count && ordinal < index.IsDescending.Count; ordinal++) {
      if (index.IsDescending[ordinal]) {
        yield return CreatePropertyReferenceSnapshot(index.Properties[ordinal]);
      }
    }
  }

  private static IReadOnlyList<PropertyReferenceSnapshot> GetIncludedProperties(IReadOnlyIndex index) {
    foreach (var annotationName in new[] { "SqlServer:Include", "Npgsql:IndexInclude" }) {
      var value = index.FindAnnotation(annotationName)?.Value;
      if (value is string[] stringArray) {
        return stringArray.Select(propertyName => CreateIncludedPropertyReference(index, propertyName)).ToArray();
      }

      if (value is IEnumerable<string> stringValues) {
        return stringValues.Select(propertyName => CreateIncludedPropertyReference(index, propertyName)).ToArray();
      }
    }

    return Array.Empty<PropertyReferenceSnapshot>();
  }

  private static PropertyReferenceSnapshot CreateIncludedPropertyReference(IReadOnlyIndex index, string propertyName) {
    var property = index.DeclaringEntityType.FindProperty(propertyName);
    return property is null
        ? new PropertyReferenceSnapshot(propertyName, propertyName, null, null)
        : CreatePropertyReferenceSnapshot(property);
  }

  private static IReadOnlyList<DataVaultModelDriftDifference> SortDifferences(
      IEnumerable<DataVaultModelDriftDifference> differences) {
    return differences
        .OrderBy(difference => difference.ElementKind)
        .ThenBy(difference => difference.LogicalName, StringComparer.Ordinal)
        .ThenBy(difference => difference.ProducedName ?? string.Empty, StringComparer.Ordinal)
        .ThenBy(difference => difference.Code, StringComparer.Ordinal)
        .ThenBy(difference => difference.PropertyPath, StringComparer.Ordinal)
        .ToArray();
  }

  private static void AddModelAnnotationDifference(
      ICollection<DataVaultModelDriftDifference> differences,
      DataVaultModelDriftSeverity severity,
      string code,
      string propertyPath,
      string? expectedValue,
      string? actualValue,
      string message) {
    AddScalarDifference(
        differences,
        severity,
        code,
        DataVaultModelDriftElementKind.Model,
        "<model>",
        producedName: null,
        propertyPath,
        expectedValue,
        actualValue,
        message);
  }

  private static void AddScalarDifference(
      ICollection<DataVaultModelDriftDifference> differences,
      DataVaultModelDriftSeverity severity,
      string code,
      DataVaultModelDriftElementKind elementKind,
      string logicalName,
      string? producedName,
      string propertyPath,
      string? expectedValue,
      string? actualValue,
      string message) {
    if (string.Equals(expectedValue, actualValue, StringComparison.Ordinal)) {
      return;
    }

    AddDifference(
        differences,
        severity,
        code,
        elementKind,
        logicalName,
        producedName,
        propertyPath,
        expectedValue,
        actualValue,
        message);
  }

  private static void AddNullableScalarDifference<T>(
      ICollection<DataVaultModelDriftDifference> differences,
      DataVaultModelDriftSeverity severity,
      string mismatchCode,
      string unsupportedGapCode,
      DataVaultModelDriftElementKind elementKind,
      string logicalName,
      string? producedName,
      string propertyPath,
      T? expectedValue,
      T? actualValue,
      string mismatchMessage,
      string unsupportedGapMessage)
      where T : struct {
    if (EqualityComparer<T?>.Default.Equals(expectedValue, actualValue)) {
      return;
    }

    var isUnsupportedGap = expectedValue.HasValue && !actualValue.HasValue;
    AddDifference(
        differences,
        severity,
        isUnsupportedGap ? unsupportedGapCode : mismatchCode,
        elementKind,
        logicalName,
        producedName,
        propertyPath,
        FormatNullable(expectedValue),
        FormatNullable(actualValue),
        isUnsupportedGap ? unsupportedGapMessage : mismatchMessage);
  }

  private static void AddPropertyReferenceDifference(
      ICollection<DataVaultModelDriftDifference> differences,
      DataVaultModelDriftSeverity severity,
      string code,
      DataVaultModelDriftElementKind elementKind,
      string logicalName,
      string? producedName,
      string propertyPath,
      IReadOnlyList<PropertyReferenceSnapshot> expectedValue,
      IReadOnlyList<PropertyReferenceSnapshot> actualValue,
      string message) {
    if (HasSamePropertyReferenceShape(expectedValue, actualValue)) {
      return;
    }

    AddDifference(
        differences,
        severity,
        code,
        elementKind,
        logicalName,
        producedName,
        propertyPath,
        FormatPropertyReferences(expectedValue),
        FormatPropertyReferences(actualValue),
        message);
  }

  private static void AddDifference(
      ICollection<DataVaultModelDriftDifference> differences,
      DataVaultModelDriftSeverity severity,
      string code,
      DataVaultModelDriftElementKind elementKind,
      string logicalName,
      string? producedName,
      string propertyPath,
      string? expectedValue,
      string? actualValue,
      string message) {
    differences.Add(new DataVaultModelDriftDifference(
        severity,
        code,
        elementKind,
        logicalName,
        producedName,
        propertyPath,
        expectedValue,
        actualValue,
        message));
  }

  private static bool IsDataVaultEntity(IReadOnlyEntityType entityType) {
    return entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value is DataVaultTableKind;
  }

  private static bool HasSameIndexSignature(IndexSnapshot expected, IndexSnapshot actual) {
    return expected.IsUnique == actual.IsUnique &&
        HasSamePropertyReferenceShape(expected.Properties, actual.Properties) &&
        HasSamePropertyReferenceShape(expected.DescendingProperties, actual.DescendingProperties) &&
        HasSamePropertyReferenceShape(expected.IncludedProperties, actual.IncludedProperties);
  }

  private static bool HasSamePropertyReferenceShape(
      IReadOnlyList<PropertyReferenceSnapshot> expected,
      IReadOnlyList<PropertyReferenceSnapshot> actual) {
    if (expected.Count != actual.Count) {
      return false;
    }

    for (var index = 0; index < expected.Count; index++) {
      if (!expected[index].HasSameLogicalIdentity(actual[index])) {
        return false;
      }
    }

    return true;
  }

  private static string GetStorageMismatchCode(PropertySnapshot expected, PropertySnapshot actual) {
    return IsTimestampProperty(expected) || IsTimestampProperty(actual)
        ? "timestamp-storage-mismatch"
        : "property-storage-type-mismatch";
  }

  private static string GetValueFormatMismatchCode(PropertySnapshot expected, PropertySnapshot actual) {
    return IsTimestampProperty(expected) || IsTimestampProperty(actual)
        ? "timestamp-value-format-mismatch"
        : "property-value-format-mismatch";
  }

  private static string GetValueFormatGapCode(PropertySnapshot expected, PropertySnapshot actual) {
    return IsTimestampProperty(expected) || IsTimestampProperty(actual)
        ? "timestamp-value-format-unsupported-gap"
        : "property-value-format-unsupported-gap";
  }

  private static bool IsTimestampProperty(PropertySnapshot property) {
    return property.LogicalPropertyKind is
        DataVaultLogicalPropertyKind.LoadTimestamp or
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference;
  }

  private static string FormatPropertyReferences(IReadOnlyList<PropertyReferenceSnapshot> values) {
    return values.Count == 0
        ? "<none>"
        : string.Join("|", values.Select(value => value.MetadataName + "=>" + value.ProducedName));
  }

  private static string FormatIndexSignature(IndexSnapshot index) {
    return "properties=" +
        FormatPropertyReferences(index.Properties) +
        "; unique=" +
        index.IsUnique.ToString() +
        "; descending=" +
        FormatPropertyReferences(index.DescendingProperties) +
        "; included=" +
        FormatPropertyReferences(index.IncludedProperties);
  }

  private static string FormatNullable<T>(T? value)
      where T : struct {
    return value?.ToString() ?? "<none>";
  }

  private static int GetEntityKindSortKey(DataVaultTableKind tableKind) {
    return tableKind switch {
      DataVaultTableKind.Hub => 0,
      DataVaultTableKind.Link => 1,
      DataVaultTableKind.Satellite => 2,
      DataVaultTableKind.Bridge => 3,
      DataVaultTableKind.Pit => 4,
      DataVaultTableKind.PointInTime => 5,
      _ => 99,
    };
  }

  private static string? GetStringAnnotation(IReadOnlyAnnotatable annotatable, string annotationName) {
    return annotatable.FindAnnotation(annotationName)?.Value as string;
  }

  private static T GetAnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName)
      where T : struct {
    var value = annotatable.FindAnnotation(annotationName)?.Value;
    return value is T typed ? typed : default;
  }

  private static T? GetNullableAnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName)
      where T : struct {
    var value = annotatable.FindAnnotation(annotationName)?.Value;
    return value is T typed ? typed : null;
  }

  private sealed record ModelSnapshot(
      string? ProviderProfile,
      string? MetadataSourceKind,
      string? MetadataSourceFingerprint,
      IReadOnlyList<EntitySnapshot> Entities);

  private sealed record EntitySnapshot(
      DataVaultTableKind Kind,
      string MetadataName,
      string ProducedName,
      string? ParentReference,
      IReadOnlyList<PropertySnapshot> Properties,
      KeySnapshot PrimaryKey,
      IReadOnlyList<IndexSnapshot> Indexes) {
    public string LogicalName => Kind + ":" + MetadataName;
  }

  private sealed record PropertySnapshot(
      string ProducedName,
      DataVaultPropertyRole? Role,
      TechnicalMetadataColumnRole? TechnicalRole,
      string MetadataName,
      int Ordinal,
      DataVaultLogicalPropertyKind? LogicalPropertyKind,
      string ProviderProfile,
      string ProviderStorageType,
      DataVaultProviderValueFormat? ProviderValueFormat);

  private sealed record PropertyReferenceSnapshot(
      string MetadataName,
      string ProducedName,
      DataVaultPropertyRole? Role,
      TechnicalMetadataColumnRole? TechnicalRole) {
    public bool HasSameLogicalIdentity(PropertyReferenceSnapshot other) {
      return string.Equals(MetadataName, other.MetadataName, StringComparison.Ordinal) &&
          Role == other.Role &&
          TechnicalRole == other.TechnicalRole;
    }
  }

  private sealed record KeySnapshot(string ProducedName, IReadOnlyList<PropertyReferenceSnapshot> Properties);

  private sealed record IndexSnapshot(
      string ProducedName,
      IReadOnlyList<PropertyReferenceSnapshot> Properties,
      bool IsUnique,
      int Ordinal,
      IReadOnlyList<PropertyReferenceSnapshot> DescendingProperties,
      IReadOnlyList<PropertyReferenceSnapshot> IncludedProperties);
}
