using System.Reflection;
using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultEfMetadataTranslationTests {
  [Fact]
  public void ApplyDataVaultMetadataIsExplicitRootNamespaceTranslationExtension() {
    var method = typeof(DCoding.Data.DVault.DataVaultModelBuilderExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(methodInfo =>
            methodInfo.Name == "ApplyDataVaultMetadata" &&
            methodInfo.GetParameters().Length == 2);
    var parameters = method.GetParameters();

    Assert.Equal("DCoding.Data.DVault", method.DeclaringType?.Namespace);
    Assert.Equal(typeof(ModelBuilder), parameters[0].ParameterType);
    Assert.Equal(typeof(DataVaultMetadataModel), parameters[1].ParameterType);
    Assert.Equal(typeof(ModelBuilder), method.ReturnType);
    Assert.True(method.IsDefined(typeof(ExtensionAttribute), inherit: false));
  }

  [Fact]
  public void ApplyDataVaultMetadataWithProviderProfileIsExplicitRootNamespaceTranslationExtension() {
    var method = typeof(DCoding.Data.DVault.DataVaultModelBuilderExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(methodInfo =>
            methodInfo.Name == "ApplyDataVaultMetadata" &&
            methodInfo.GetParameters().Length == 3);
    var parameters = method.GetParameters();

    Assert.Equal("DCoding.Data.DVault", method.DeclaringType?.Namespace);
    Assert.Equal(typeof(ModelBuilder), parameters[0].ParameterType);
    Assert.Equal(typeof(DataVaultMetadataModel), parameters[1].ParameterType);
    Assert.Equal(typeof(DataVaultProviderCapabilityProfile), parameters[2].ParameterType);
    Assert.Equal(typeof(ModelBuilder), method.ReturnType);
    Assert.True(method.IsDefined(typeof(ExtensionAttribute), inherit: false));
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsNullArguments() {
    var metadataModel = CreateMetadataModel();
    ModelBuilder? modelBuilder = null;

    var modelBuilderException = Assert.Throws<ArgumentNullException>(() => modelBuilder!.ApplyDataVaultMetadata(metadataModel));
    var metadataException = Assert.Throws<ArgumentNullException>(() => CreateModelBuilder().ApplyDataVaultMetadata(null!));
    var profileException = Assert.Throws<ArgumentNullException>(() =>
        CreateModelBuilder().ApplyDataVaultMetadata(metadataModel, null!));

    Assert.Equal("modelBuilder", modelBuilderException.ParamName);
    Assert.Equal("metadataModel", metadataException.ParamName);
    Assert.Equal("providerCapabilities", profileException.ParamName);
  }

  [Fact]
  public void ApplyDataVaultMetadataCreatesProviderNeutralHubLinkAndSatelliteMetadata() {
    var model = CreateTranslatedModel();

    Assert.Equal(3, model.GetEntityTypes().Count());

    AssertHub(FindEntity(model, "HubCustomer"));
    AssertLink(FindEntity(model, "LinkCustomerOrder"));
    AssertSatellite(FindEntity(model, "SatCustomerContact"));
  }

  [Fact]
  public void ApplyDataVaultMetadataMapsProducedNamesToRelationalMetadata() {
    var model = CreateTranslatedModel();

    AssertRelationalEntity(
        FindEntity(model, "HubCustomer"),
        "HubCustomer",
        ["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId"],
        "PkHubCustomerCustomerHashKey",
        "IxHubCustomerBusinessKeyCustomerId");
    AssertRelationalEntity(
        FindEntity(model, "LinkCustomerOrder"),
        "LinkCustomerOrder",
        ["CustomerOrderHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKey", "OrderHashKey"],
        "PkLinkCustomerOrderCustomerOrderHashKey",
        "IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey");
    AssertRelationalEntity(
        FindEntity(model, "SatCustomerContact"),
        "SatCustomerContact",
        ["CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "EmailAddress"],
        "PkSatCustomerContactCustomerHashKeyLoadTimestamp",
        "IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp");
  }

  [Fact]
  public void ApplyDataVaultMetadataHonorsRelationalDefaultSchema() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.HasDefaultSchema("dvault_test_schema");
    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());

    Assert.All(
        modelBuilder.Model.GetEntityTypes(),
        entityType => Assert.Equal("dvault_test_schema", entityType.GetSchema()));
  }

  [Fact]
  public void ApplyDataVaultMetadataKeepsEquivalentInputDeterministic() {
    var first = CreateTranslatedModel();
    var second = CreateTranslatedModel();

    Assert.Equal(ModelShape(first), ModelShape(second));
  }

  [Fact]
  public void ApplyDataVaultMetadataPreservesDeclaredBusinessKeyOrder() {
    var modelBuilder = CreateModelBuilder();
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id", "Source System"])],
        [],
        []);

    modelBuilder.ApplyDataVaultMetadata(metadataModel);

    var hub = FindEntity(modelBuilder.Model, "HubCustomer");

    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId", "SourceSystem"],
        PropertyNamesInOrdinalOrder(hub));
    AssertPrimaryKey(hub, "PkHubCustomerCustomerHashKey", ["CustomerHashKey"]);
    AssertIndex(hub, "IxHubCustomerBusinessKeyCustomerIdSourceSystem", ["CustomerId", "SourceSystem"], isUnique: true);
  }

  [Fact]
  public void ApplyDataVaultMetadataWithOracleProfileProjectsOracleStorageAnnotations() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel(), DataVaultProviderCapabilityProfiles.Oracle);

    Assert.Equal(
        "oracle-v1",
        Assert.IsType<string>(modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.ProviderProfile)?.Value));

    var hub = FindEntity(modelBuilder.Model, "HubCustomer");
    var link = FindEntity(modelBuilder.Model, "LinkCustomerOrder");
    var satellite = FindEntity(modelBuilder.Model, "SatCustomerContact");

    AssertProviderProperty(
        hub,
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.HashKey,
        typeof(string),
        "VARCHAR2(64 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertProviderProperty(
        hub,
        "LoadTimestamp",
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(string),
        "VARCHAR2(33 CHAR)",
        DataVaultProviderValueFormat.Iso8601UtcText);
    AssertProviderProperty(
        hub,
        "RecordSource",
        DataVaultLogicalPropertyKind.RecordSource,
        typeof(string),
        "VARCHAR2(255 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertProviderProperty(
        hub,
        "CustomerId",
        DataVaultLogicalPropertyKind.BusinessKey,
        typeof(string),
        "VARCHAR2(255 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertProviderProperty(
        link,
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference,
        typeof(string),
        "VARCHAR2(64 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertProviderProperty(
        satellite,
        "HashDiff",
        DataVaultLogicalPropertyKind.HashDiff,
        typeof(string),
        "VARCHAR2(64 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertProviderProperty(
        satellite,
        "EmailAddress",
        DataVaultLogicalPropertyKind.PayloadText,
        typeof(string),
        "CLOB",
        DataVaultProviderValueFormat.Text);
    Assert.DoesNotContain(
        satellite.GetIndexes(),
        index => index.Properties.Select(property => property.Name)
            .SequenceEqual(["CustomerHashKey", "LoadTimestamp"], StringComparer.Ordinal));
  }

  [Fact]
  public void ApplyDataVaultMetadataTranslatesLinkParentSatellites() {
    var modelBuilder = CreateModelBuilder();
    var metadataModel = new DataVaultMetadataModel(
        [],
        [],
        [
            new DataVaultSatelliteMetadata(
                "State",
                DataVaultMetadataReference.Link("CustomerOrder"),
                ["State Code"]),
        ]);

    modelBuilder.ApplyDataVaultMetadata(metadataModel);

    var satellite = FindEntity(modelBuilder.Model, "SatCustomerOrderState");

    Assert.Equal(DataVaultTableKind.Satellite, AnnotationValue<DataVaultTableKind>(satellite, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("State", AnnotationValue<string>(satellite, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        DataVaultMetadataReferenceKind.Link,
        AnnotationValue<DataVaultMetadataReferenceKind>(satellite, DataVaultAnnotationNames.ParentReferenceKind));
    Assert.Equal("CustomerOrder", AnnotationValue<string>(satellite, DataVaultAnnotationNames.ParentReferenceName));
    Assert.Equal(
        ["CustomerOrderHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "StateCode"],
        PropertyNamesInOrdinalOrder(satellite));
    AssertPrimaryKey(satellite, "PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp", ["CustomerOrderHashKey", "LoadTimestamp"]);
    AssertIndex(
        satellite,
        "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp",
        ["CustomerOrderHashKey", "LoadTimestamp"],
        isUnique: false);
  }

  [Fact]
  public void ApplyDataVaultMetadataFailsDeterministicallyWhenProviderProfileOmitsRequiredMapping() {
    var providerCapabilities = new DataVaultProviderCapabilityProfile(
        "broken-test-profile",
        DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
        DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
        [
            new(
                DataVaultLogicalPropertyKind.HashKey,
                typeof(string),
                "TEXT",
                DataVaultProviderValueFormat.Text),
        ]);

    var exception = Assert.Throws<TargetInvocationException>(() =>
        InvokeTranslatorApply(CreateModelBuilder(), CreateMetadataModel(), providerCapabilities));
    var notSupportedException = Assert.IsType<NotSupportedException>(exception.InnerException);

    Assert.Contains("broken-test-profile", notSupportedException.Message, StringComparison.Ordinal);
    Assert.Contains("type mapping for LoadTimestamp", notSupportedException.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void DataVaultMetadataModelRejectsNullCollectionsAndItems() {
    Assert.Throws<ArgumentNullException>(() => new DataVaultMetadataModel(null!, [], []));
    Assert.Throws<ArgumentNullException>(() => new DataVaultMetadataModel([], null!, []));
    Assert.Throws<ArgumentNullException>(() => new DataVaultMetadataModel([], [], null!));
    Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel([null!], [], []));
    Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel([], [null!], []));
    Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel([], [], [null!]));
  }

  private static IMutableModel CreateTranslatedModel() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());

    return modelBuilder.Model;
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
        ]);
  }

  private static ModelBuilder CreateModelBuilder() {
    return new ModelBuilder(new ConventionSet());
  }

  private static void AssertHub(IMutableEntityType hub) {
    Assert.Equal(DataVaultTableKind.Hub, AnnotationValue<DataVaultTableKind>(hub, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("Customer", AnnotationValue<string>(hub, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId"], PropertyNamesInOrdinalOrder(hub));
    AssertProperty(hub, "CustomerHashKey", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(hub, "LoadTimestamp", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(hub, "RecordSource", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.RecordSource);
    AssertProperty(hub, "CustomerId", DataVaultPropertyRole.BusinessKey, expectedTechnicalRole: null);
    AssertPrimaryKey(hub, "PkHubCustomerCustomerHashKey", ["CustomerHashKey"]);
    AssertIndex(hub, "IxHubCustomerBusinessKeyCustomerId", ["CustomerId"], isUnique: true);
    AssertNoRelationships(hub);
  }

  private static void AssertLink(IMutableEntityType link) {
    Assert.Equal(DataVaultTableKind.Link, AnnotationValue<DataVaultTableKind>(link, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("CustomerOrder", AnnotationValue<string>(link, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        ["CustomerOrderHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKey", "OrderHashKey"],
        PropertyNamesInOrdinalOrder(link));
    AssertProperty(link, "CustomerOrderHashKey", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(link, "LoadTimestamp", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(link, "RecordSource", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.RecordSource);
    AssertProperty(link, "CustomerHashKey", DataVaultPropertyRole.ParticipantReference, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(link, "OrderHashKey", DataVaultPropertyRole.ParticipantReference, TechnicalMetadataColumnRole.HashKey);
    AssertPrimaryKey(link, "PkLinkCustomerOrderCustomerOrderHashKey", ["CustomerOrderHashKey"]);
    AssertIndex(link, "IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey", ["CustomerHashKey", "OrderHashKey"], isUnique: false);
    AssertNoRelationships(link);
  }

  private static void AssertSatellite(IMutableEntityType satellite) {
    Assert.Equal(DataVaultTableKind.Satellite, AnnotationValue<DataVaultTableKind>(satellite, DataVaultAnnotationNames.EntityKind));
    Assert.Equal("Contact", AnnotationValue<string>(satellite, DataVaultAnnotationNames.MetadataName));
    Assert.Equal(
        DataVaultMetadataReferenceKind.Hub,
        AnnotationValue<DataVaultMetadataReferenceKind>(satellite, DataVaultAnnotationNames.ParentReferenceKind));
    Assert.Equal("Customer", AnnotationValue<string>(satellite, DataVaultAnnotationNames.ParentReferenceName));
    Assert.Equal(
        ["CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "EmailAddress"],
        PropertyNamesInOrdinalOrder(satellite));
    AssertProperty(satellite, "CustomerHashKey", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashKey);
    AssertProperty(satellite, "HashDiff", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.HashDiff);
    AssertProperty(satellite, "LoadTimestamp", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.LoadTimestamp);
    AssertProperty(satellite, "RecordSource", DataVaultPropertyRole.Technical, TechnicalMetadataColumnRole.RecordSource);
    AssertProperty(satellite, "EmailAddress", DataVaultPropertyRole.Payload, expectedTechnicalRole: null);
    AssertPrimaryKey(satellite, "PkSatCustomerContactCustomerHashKeyLoadTimestamp", ["CustomerHashKey", "LoadTimestamp"]);
    AssertIndex(
        satellite,
        "IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp",
        ["CustomerHashKey", "LoadTimestamp"],
        isUnique: false);
    AssertNoRelationships(satellite);
  }

  private static void AssertProperty(
      IMutableEntityType entityType,
      string propertyName,
      DataVaultPropertyRole expectedRole,
      TechnicalMetadataColumnRole? expectedTechnicalRole) {
    var property = entityType.FindProperty(propertyName);
    var expectedLogicalPropertyKind = GetExpectedLogicalPropertyKind(expectedRole, expectedTechnicalRole);

    Assert.NotNull(property);
    Assert.Equal(propertyName, AnnotationValue<string>(property!, DataVaultAnnotationNames.ProducedName));
    Assert.Equal(expectedRole, AnnotationValue<DataVaultPropertyRole>(property!, DataVaultAnnotationNames.PropertyRole));
    Assert.Equal(ExpectedClrType(expectedLogicalPropertyKind), property!.ClrType);
    Assert.Equal("sqlite-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile));
    Assert.Equal(expectedLogicalPropertyKind, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal("TEXT", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(ExpectedValueFormat(expectedLogicalPropertyKind), AnnotationValue<DataVaultProviderValueFormat>(
        property,
        DataVaultAnnotationNames.ProviderValueFormat));

    if (expectedTechnicalRole is null) {
      Assert.Null(property.FindAnnotation(DataVaultAnnotationNames.TechnicalColumnRole));
      return;
    }

    Assert.Equal(expectedTechnicalRole, AnnotationValue<TechnicalMetadataColumnRole>(property, DataVaultAnnotationNames.TechnicalColumnRole));
  }

  private static void AssertProviderProperty(
      IMutableEntityType entityType,
      string propertyName,
      DataVaultLogicalPropertyKind expectedLogicalPropertyKind,
      Type expectedClrType,
      string expectedStorageType,
      DataVaultProviderValueFormat expectedValueFormat) {
    var property = entityType.FindProperty(propertyName);

    Assert.NotNull(property);
    Assert.Equal(expectedClrType, property!.ClrType);
    Assert.Equal("oracle-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile));
    Assert.Equal(expectedLogicalPropertyKind, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal(expectedStorageType, AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(expectedValueFormat, AnnotationValue<DataVaultProviderValueFormat>(
        property,
        DataVaultAnnotationNames.ProviderValueFormat));
  }

  private static void InvokeTranslatorApply(
      ModelBuilder modelBuilder,
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    var translatorType = typeof(DCoding.Data.DVault.DataVaultModelBuilderExtensions).Assembly
        .GetType("DCoding.Data.DVault.DataVaultEfMetadataTranslator");

    Assert.NotNull(translatorType);

    var applyMethod = translatorType!
        .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
        .Single(methodInfo =>
            methodInfo.Name == "Apply" &&
            methodInfo.GetParameters().Length == 3);

    applyMethod.Invoke(null, [modelBuilder, metadataModel, providerCapabilities]);
  }

  private static DataVaultLogicalPropertyKind GetExpectedLogicalPropertyKind(
      DataVaultPropertyRole role,
      TechnicalMetadataColumnRole? technicalRole) {
    return role switch {
      DataVaultPropertyRole.BusinessKey => DataVaultLogicalPropertyKind.BusinessKey,
      DataVaultPropertyRole.ParticipantReference => DataVaultLogicalPropertyKind.ParticipantReference,
      DataVaultPropertyRole.Payload => DataVaultLogicalPropertyKind.PayloadText,
      DataVaultPropertyRole.Technical => technicalRole switch {
        TechnicalMetadataColumnRole.HashKey => DataVaultLogicalPropertyKind.HashKey,
        TechnicalMetadataColumnRole.HashDiff => DataVaultLogicalPropertyKind.HashDiff,
        TechnicalMetadataColumnRole.LoadTimestamp => DataVaultLogicalPropertyKind.LoadTimestamp,
        TechnicalMetadataColumnRole.RecordSource => DataVaultLogicalPropertyKind.RecordSource,
        _ => throw new InvalidOperationException("Expected a technical metadata role."),
      },
      _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported Data Vault property role."),
    };
  }

  private static Type ExpectedClrType(DataVaultLogicalPropertyKind logicalPropertyKind) {
    return logicalPropertyKind == DataVaultLogicalPropertyKind.LoadTimestamp
        ? typeof(DateTimeOffset)
        : typeof(string);
  }

  private static DataVaultProviderValueFormat ExpectedValueFormat(DataVaultLogicalPropertyKind logicalPropertyKind) {
    return logicalPropertyKind == DataVaultLogicalPropertyKind.LoadTimestamp
        ? DataVaultProviderValueFormat.Iso8601UtcText
        : DataVaultProviderValueFormat.Text;
  }

  private static void AssertPrimaryKey(IMutableEntityType entityType, string expectedName, string[] expectedProperties) {
    var primaryKey = entityType.FindPrimaryKey();

    Assert.NotNull(primaryKey);
    Assert.Equal(expectedName, AnnotationValue<string>(primaryKey!, DataVaultAnnotationNames.ProducedName));
    Assert.Equal(expectedProperties, primaryKey!.Properties.Select(property => property.Name));
  }

  private static void AssertIndex(IMutableEntityType entityType, string expectedName, string[] expectedProperties, bool isUnique) {
    var index = Assert.Single(entityType.GetIndexes());

    Assert.Equal(expectedName, AnnotationValue<string>(index, DataVaultAnnotationNames.ProducedName));
    Assert.Equal(expectedProperties, index.Properties.Select(property => property.Name));
    Assert.Equal(isUnique, index.IsUnique);
  }

  private static void AssertRelationalEntity(
      IMutableEntityType entityType,
      string expectedTableName,
      string[] expectedColumnNames,
      string expectedPrimaryKeyName,
      string expectedIndexName) {
    var table = StoreObjectIdentifier.Table(expectedTableName, schema: null);

    Assert.Equal(expectedTableName, entityType.GetTableName());
    Assert.Equal(expectedColumnNames, PropertyNamesInOrdinalOrder(entityType));
    Assert.Equal(
        expectedColumnNames,
        entityType.GetProperties()
            .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
            .Select(property => property.GetColumnName(table)));
    Assert.Equal(
        Enumerable.Range(0, expectedColumnNames.Length).Select(order => (int?)order),
        entityType.GetProperties()
            .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
            .Select(property => property.GetColumnOrder()));
    Assert.Equal(expectedPrimaryKeyName, entityType.FindPrimaryKey()!.GetName());
    Assert.Equal(expectedIndexName, Assert.Single(entityType.GetIndexes()).GetDatabaseName());
  }

  private static void AssertNoRelationships(IMutableEntityType entityType) {
    Assert.Empty(entityType.GetForeignKeys());
    Assert.Empty(entityType.GetNavigations());
    Assert.Empty(entityType.GetSkipNavigations());
  }

  private static IMutableEntityType FindEntity(IMutableModel model, string producedName) {
    var matches = model.GetEntityTypes()
        .Where(entityType => string.Equals(
            entityType.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string,
            producedName,
            StringComparison.Ordinal))
        .ToArray();

    Assert.Single(matches);
    return matches[0];
  }

  private static string[] PropertyNamesInOrdinalOrder(IMutableEntityType entityType) {
    return entityType.GetProperties()
        .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
        .Select(property => property.Name)
        .ToArray();
  }

  private static string[] ModelShape(IMutableModel model) {
    return model.GetEntityTypes()
        .Select(entityType => string.Join(
            "|",
            AnnotationValue<string>(entityType, DataVaultAnnotationNames.ProducedName),
            AnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind).ToString(),
            string.Join(",", PropertyNamesInOrdinalOrder(entityType)),
            string.Join(",", entityType.FindPrimaryKey()!.Properties.Select(property => property.Name)),
            string.Join(
                ";",
                entityType.GetIndexes()
                    .OrderBy(index => AnnotationValue<int>(index, DataVaultAnnotationNames.Ordinal))
                    .Select(index => string.Join(",", index.Properties.Select(property => property.Name)) + ":" + index.IsUnique))))
        .Order(StringComparer.Ordinal)
        .ToArray();
  }

  private static T AnnotationValue<T>(IMutableEntityType entityType, string name) {
    return Assert.IsType<T>(RequiredAnnotation(entityType.FindAnnotation(name)).Value);
  }

  private static T AnnotationValue<T>(IMutableProperty property, string name) {
    return Assert.IsType<T>(RequiredAnnotation(property.FindAnnotation(name)).Value);
  }

  private static T AnnotationValue<T>(IMutableKey key, string name) {
    return Assert.IsType<T>(RequiredAnnotation(key.FindAnnotation(name)).Value);
  }

  private static T AnnotationValue<T>(IMutableIndex index, string name) {
    return Assert.IsType<T>(RequiredAnnotation(index.FindAnnotation(name)).Value);
  }

  private static IAnnotation RequiredAnnotation(IAnnotation? annotation) {
    Assert.NotNull(annotation);

    return annotation!;
  }
}
