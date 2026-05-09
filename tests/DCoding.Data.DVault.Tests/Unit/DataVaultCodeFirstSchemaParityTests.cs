using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultCodeFirstSchemaParityTests {
  [Fact]
  public void ApplyDataVaultMetadataCodeFirstMatchesMetadataFirstRelationalShapeForBuiltInProviderProfiles() {
    foreach (var profile in BuiltInProfiles()) {
      var metadataFirstModel = TranslateMetadata(CreateCoveredMetadataModel(), profile);
      var codeFirstModel = TranslateCodeFirst(ConfigureCoveredCodeFirstModel, profile);

      Assert.Equal(RelationalProviderShape(metadataFirstModel), RelationalProviderShape(codeFirstModel));
    }
  }

  [Fact]
  public void ApplyDataVaultMetadataCodeFirstKeepsCoveredBaselineOrderingAndCollisionShapeExplicit() {
    var model = TranslateCodeFirst(ConfigureCoveredCodeFirstModel, DataVaultProviderCapabilityProfiles.Sqlite);
    var hub = FindEntity(model, "HubCustomer");
    var ordinarySatellite = FindEntity(model, "SatCustomerContact");
    var multiActiveSatellite = FindEntity(model, "SatCustomerContactChannel");
    var link = FindEntity(model, "LinkCustomerOrder");

    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKeyValue", "CustomerId"],
        PropertyNamesInOrdinalOrder(hub));
    Assert.Equal(
        ["CustomerHashKeyValue", "CustomerId"],
        Assert.Single(hub.GetIndexes()).Properties.Select(property => property.Name));
    Assert.Equal(
        ["CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "LoadTimestampValue", "EmailAddress"],
        PropertyNamesInOrdinalOrder(ordinarySatellite));
    Assert.Equal(
        ["CustomerHashKey", "ContactType", "RegionCode", "HashDiff", "LoadTimestamp", "RecordSource", "HashDiffValue", "RecordSourceValue"],
        PropertyNamesInOrdinalOrder(multiActiveSatellite));
    Assert.Equal(
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp"],
        multiActiveSatellite.FindPrimaryKey()!.Properties.Select(property => property.Name));
    Assert.Equal(
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp", "HashDiff"],
        Assert.Single(multiActiveSatellite.GetIndexes()).Properties.Select(property => property.Name));
    Assert.Equal(
        ["CustomerOrderHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKey", "OrderHashKey"],
        PropertyNamesInOrdinalOrder(link));
    Assert.Equal(
        ["CustomerHashKey", "OrderHashKey"],
        Assert.Single(link.GetIndexes()).Properties.Select(property => property.Name));
  }

  [Fact]
  public void ApplyDataVaultMetadataCodeFirstProviderMatrixKeepsStorageAndIndexDifferencesVisible() {
    Assert.Equal(
        [
            "sqlite-v1|load=DateTimeOffset:TEXT:Iso8601UtcText|payload=String:TEXT:Text|driving=String:TEXT:Text|multi-index=CustomerHashKey,ContactType,RegionCode,LoadTimestamp,HashDiff",
            "oracle-v1|load=String:VARCHAR2(33 CHAR):Iso8601UtcText|payload=String:CLOB:Text|driving=String:VARCHAR2(255 CHAR):Text|multi-index=CustomerHashKey,ContactType,RegionCode,LoadTimestamp,HashDiff",
            "postgres-v1|load=DateTimeOffset:timestamp with time zone:NativeDateTimeOffset|payload=String:text:Text|driving=String:varchar(255):Text|multi-index=CustomerHashKey,ContactType,RegionCode,LoadTimestamp",
            "sqlserver-v1|load=DateTimeOffset:datetimeoffset:NativeDateTimeOffset|payload=String:nvarchar(max):Text|driving=String:nvarchar(255):Text|multi-index=CustomerHashKey,ContactType,RegionCode,LoadTimestamp",
            "mysql-pomelo-v1|load=DateTimeOffset:varchar(33):Iso8601UtcText|payload=String:longtext:Text|driving=String:varchar(255):Text|multi-index=CustomerHashKey,ContactType,RegionCode,LoadTimestamp",
        ],
        BuiltInProfiles().Select(profile => ProviderProfileSummary(TranslateCodeFirst(ConfigureCoveredCodeFirstModel, profile))));
  }

  [Fact]
  public void ApplyDataVaultMetadataCodeFirstMatchesMetadataFirstWhenMySqlTruncatesLongIdentifiers() {
    var metadataFirstModel = TranslateMetadata(CreateLongIdentifierMetadataModel(), DataVaultProviderCapabilityProfiles.MySql);
    var codeFirstModel = TranslateCodeFirst(ConfigureLongIdentifierCodeFirstModel, DataVaultProviderCapabilityProfiles.MySql);
    var identifierPairs = PhysicalIdentifierPairs(codeFirstModel).ToArray();

    Assert.Equal(RelationalProviderShape(metadataFirstModel), RelationalProviderShape(codeFirstModel));
    Assert.All(identifierPairs, pair => Assert.True(
        pair.PhysicalName.Length <= 64,
        pair.PhysicalName + " exceeds the MySQL identifier limit."));
    Assert.Contains(
        identifierPairs,
        pair => pair.ProducedName.Length > 64 &&
            pair.PhysicalName.Length == 64 &&
            !string.Equals(pair.ProducedName, pair.PhysicalName, StringComparison.Ordinal));
  }

  private static DataVaultProviderCapabilityProfile[] BuiltInProfiles() {
    return
    [
        DataVaultProviderCapabilityProfiles.Sqlite,
        DataVaultProviderCapabilityProfiles.Oracle,
        DataVaultProviderCapabilityProfiles.Postgres,
        DataVaultProviderCapabilityProfiles.SqlServer,
        DataVaultProviderCapabilityProfiles.MySql,
    ];
  }

  private static DataVaultMetadataModel CreateCoveredMetadataModel() {
    var customer = new DataVaultHubMetadata(
        nameof(Customer),
        [nameof(Customer.CustomerHashKey), nameof(Customer.CustomerId)]);
    var order = new DataVaultHubMetadata(nameof(Order), [nameof(Order.OrderId)]);

    return new DataVaultMetadataModel(
        [customer, order],
        [new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()])],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                customer.ToReference(),
                [nameof(Customer.LoadTimestamp), nameof(Customer.EmailAddress)]),
            new DataVaultSatelliteMetadata(
                "ContactChannel",
                customer.ToReference(),
                [nameof(Customer.HashDiff), nameof(Customer.RecordSource)],
                [nameof(Customer.ContactType), nameof(Customer.RegionCode)]),
        ]);
  }

  private static void ConfigureCoveredCodeFirstModel(DataVaultCodeFirstModelBuilder vault) {
    vault.Hub<Customer>(hub => {
      hub.BusinessKey(customer => customer.CustomerHashKey);
      hub.BusinessKey(customer => customer.CustomerId);
      hub.Satellite("Contact", satellite => {
        satellite.Payload(customer => customer.LoadTimestamp);
        satellite.Payload(customer => customer.EmailAddress);
      });
      hub.Satellite("ContactChannel", satellite => {
        satellite.DrivingKey(customer => customer.ContactType);
        satellite.DrivingKey(customer => customer.RegionCode);
        satellite.Payload(customer => customer.HashDiff);
        satellite.Payload(customer => customer.RecordSource);
      });
    });
    vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));
    vault.Link("CustomerOrder", link => {
      link.Participant<Customer>();
      link.Participant<Order>();
    });
  }

  private static DataVaultMetadataModel CreateLongIdentifierMetadataModel() {
    var customer = new DataVaultHubMetadata(
        nameof(CustomerIdentityAggregateWithExceptionallyVerboseBusinessBoundaryForMySqlIdentifierTruncation),
        [nameof(CustomerIdentityAggregateWithExceptionallyVerboseBusinessBoundaryForMySqlIdentifierTruncation.CustomerId)]);
    var order = new DataVaultHubMetadata(
        nameof(OrderIdentityAggregateWithExceptionallyVerboseFulfillmentBoundaryForMySqlIdentifierTruncation),
        [nameof(OrderIdentityAggregateWithExceptionallyVerboseFulfillmentBoundaryForMySqlIdentifierTruncation.OrderId)]);

    return new DataVaultMetadataModel(
        [customer, order],
        [
            new DataVaultLinkMetadata(
                "CustomerOrderFulfillmentRelationshipWithExceptionallyVerboseContextForMySqlIdentifierTruncation",
                [customer.ToReference(), order.ToReference()]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "CustomerProfileAttributeHistoryWithExceptionallyVerboseContextForMySqlIdentifierTruncation",
                customer.ToReference(),
                [nameof(CustomerIdentityAggregateWithExceptionallyVerboseBusinessBoundaryForMySqlIdentifierTruncation.EmailAddress)]),
        ]);
  }

  private static void ConfigureLongIdentifierCodeFirstModel(DataVaultCodeFirstModelBuilder vault) {
    vault.Hub<CustomerIdentityAggregateWithExceptionallyVerboseBusinessBoundaryForMySqlIdentifierTruncation>(hub => {
      hub.BusinessKey(customer => customer.CustomerId);
      hub.Satellite(
          "CustomerProfileAttributeHistoryWithExceptionallyVerboseContextForMySqlIdentifierTruncation",
          satellite => satellite.Payload(customer => customer.EmailAddress));
    });
    vault.Hub<OrderIdentityAggregateWithExceptionallyVerboseFulfillmentBoundaryForMySqlIdentifierTruncation>(
        hub => hub.BusinessKey(order => order.OrderId));
    vault.Link(
        "CustomerOrderFulfillmentRelationshipWithExceptionallyVerboseContextForMySqlIdentifierTruncation",
        link => {
          link.Participant<CustomerIdentityAggregateWithExceptionallyVerboseBusinessBoundaryForMySqlIdentifierTruncation>();
          link.Participant<OrderIdentityAggregateWithExceptionallyVerboseFulfillmentBoundaryForMySqlIdentifierTruncation>();
        });
  }

  private static IMutableModel TranslateMetadata(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilityProfile) {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(metadataModel, providerCapabilityProfile);

    return modelBuilder.Model;
  }

  private static IMutableModel TranslateCodeFirst(
      Action<DataVaultCodeFirstModelBuilder> configureModel,
      DataVaultProviderCapabilityProfile providerCapabilityProfile) {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(configureModel, providerCapabilityProfile);

    return modelBuilder.Model;
  }

  private static ModelBuilder CreateModelBuilder() {
    return new ModelBuilder(new ConventionSet());
  }

  private static string ProviderProfileSummary(IMutableModel model) {
    var profileName = AnnotationValue<string>(model, DataVaultAnnotationNames.ProviderProfile);
    var hub = FindEntity(model, "HubCustomer");
    var ordinarySatellite = FindEntity(model, "SatCustomerContact");
    var multiActiveSatellite = FindEntity(model, "SatCustomerContactChannel");
    var loadTimestamp = hub.FindProperty("LoadTimestamp");
    var payload = ordinarySatellite.FindProperty("LoadTimestampValue");
    var drivingKey = multiActiveSatellite.FindProperty("ContactType");
    var multiActiveIndex = Assert.Single(multiActiveSatellite.GetIndexes());

    Assert.NotNull(loadTimestamp);
    Assert.NotNull(payload);
    Assert.NotNull(drivingKey);

    return string.Join(
        "|",
        profileName,
        "load=" + StorageShape(loadTimestamp!),
        "payload=" + StorageShape(payload!),
        "driving=" + StorageShape(drivingKey!),
        "multi-index=" + string.Join(",", multiActiveIndex.Properties.Select(property => property.Name)));
  }

  private static string StorageShape(IMutableProperty property) {
    return string.Join(
        ":",
        property.ClrType.Name,
        property.GetColumnType(),
        AnnotationValue<DataVaultProviderValueFormat>(property, DataVaultAnnotationNames.ProviderValueFormat));
  }

  private static string[] RelationalProviderShape(IMutableModel model) {
    var lines = new List<string>
    {
        "model provider " + AnnotationValue<string>(model, DataVaultAnnotationNames.ProviderProfile),
    };

    foreach (var entityType in model.GetEntityTypes().OrderBy(ProducedName, StringComparer.Ordinal)) {
      var table = StoreObjectIdentifier.Table(entityType.GetTableName()!, entityType.GetSchema());
      lines.Add(
          "entity " +
          ProducedName(entityType) +
          " table=" +
          entityType.GetTableName() +
          " kind=" +
          AnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind) +
          " metadata=" +
          AnnotationValue<string>(entityType, DataVaultAnnotationNames.MetadataName) +
          " parent-kind=" +
          AnnotationValueOrEmpty(entityType, DataVaultAnnotationNames.ParentReferenceKind) +
          " parent-name=" +
          AnnotationValueOrEmpty(entityType, DataVaultAnnotationNames.ParentReferenceName));

      foreach (var property in entityType.GetProperties().OrderBy(Ordinal)) {
        lines.Add(
            "  property " +
            property.Name +
            " column=" +
            property.GetColumnName(table) +
            " order=" +
            property.GetColumnOrder() +
            " clr=" +
            property.ClrType.FullName +
            " store=" +
            property.GetColumnType() +
            " role=" +
            AnnotationValue<DataVaultPropertyRole>(property, DataVaultAnnotationNames.PropertyRole) +
            " technical=" +
            AnnotationValueOrEmpty(property, DataVaultAnnotationNames.TechnicalColumnRole) +
            " metadata=" +
            AnnotationValue<string>(property, DataVaultAnnotationNames.MetadataName) +
            " logical=" +
            AnnotationValue<DataVaultLogicalPropertyKind>(property, DataVaultAnnotationNames.ProviderLogicalPropertyKind) +
            " format=" +
            AnnotationValue<DataVaultProviderValueFormat>(property, DataVaultAnnotationNames.ProviderValueFormat));
      }

      var primaryKey = entityType.FindPrimaryKey();
      Assert.NotNull(primaryKey);
      lines.Add(
          "  primary-key produced=" +
          AnnotationValue<string>(primaryKey!, DataVaultAnnotationNames.ProducedName) +
          " database=" +
          primaryKey!.GetName() +
          " columns=" +
          string.Join(",", primaryKey.Properties.Select(property => property.GetColumnName(table))));

      foreach (var index in entityType.GetIndexes().OrderBy(Ordinal)) {
        lines.Add(
            "  index produced=" +
            AnnotationValue<string>(index, DataVaultAnnotationNames.ProducedName) +
            " database=" +
            index.GetDatabaseName() +
            " unique=" +
            index.IsUnique +
            " columns=" +
            string.Join(",", index.Properties.Select(property => property.GetColumnName(table))));
      }
    }

    return lines.ToArray();
  }

  private static IEnumerable<IdentifierPair> PhysicalIdentifierPairs(IMutableModel model) {
    foreach (var entityType in model.GetEntityTypes()) {
      var primaryKey = entityType.FindPrimaryKey();
      Assert.NotNull(primaryKey);
      yield return new IdentifierPair(
          AnnotationValue<string>(primaryKey!, DataVaultAnnotationNames.ProducedName),
          primaryKey!.GetName()!);

      foreach (var index in entityType.GetIndexes()) {
        yield return new IdentifierPair(
            AnnotationValue<string>(index, DataVaultAnnotationNames.ProducedName),
            index.GetDatabaseName()!);
      }
    }
  }

  private static IMutableEntityType FindEntity(IMutableModel model, string producedName) {
    var matches = model.GetEntityTypes()
        .Where(entityType => string.Equals(ProducedName(entityType), producedName, StringComparison.Ordinal))
        .ToArray();

    Assert.Single(matches);
    return matches[0];
  }

  private static string[] PropertyNamesInOrdinalOrder(IMutableEntityType entityType) {
    return entityType.GetProperties()
        .OrderBy(Ordinal)
        .Select(property => property.Name)
        .ToArray();
  }

  private static string ProducedName(IMutableEntityType entityType) {
    return AnnotationValue<string>(entityType, DataVaultAnnotationNames.ProducedName);
  }

  private static int Ordinal(IReadOnlyAnnotatable annotatable) {
    return AnnotationValue<int>(annotatable, DataVaultAnnotationNames.Ordinal);
  }

  private static T AnnotationValue<T>(IReadOnlyAnnotatable annotatable, string name) {
    return Assert.IsType<T>(RequiredAnnotation(annotatable.FindAnnotation(name)).Value);
  }

  private static string AnnotationValueOrEmpty(IReadOnlyAnnotatable annotatable, string name) {
    return annotatable.FindAnnotation(name)?.Value?.ToString() ?? "";
  }

  private static IAnnotation RequiredAnnotation(IAnnotation? annotation) {
    Assert.NotNull(annotation);

    return annotation!;
  }

  private sealed record IdentifierPair(string ProducedName, string PhysicalName);

  private sealed class Customer {
    public string ContactType { get; init; } = string.Empty;

    public string CustomerHashKey { get; init; } = string.Empty;

    public string CustomerId { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string HashDiff { get; init; } = string.Empty;

    public string LoadTimestamp { get; init; } = string.Empty;

    public string RecordSource { get; init; } = string.Empty;

    public string RegionCode { get; init; } = string.Empty;
  }

  private sealed class Order {
    public string OrderId { get; init; } = string.Empty;
  }

  private sealed class CustomerIdentityAggregateWithExceptionallyVerboseBusinessBoundaryForMySqlIdentifierTruncation {
    public string CustomerId { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;
  }

  private sealed class OrderIdentityAggregateWithExceptionallyVerboseFulfillmentBoundaryForMySqlIdentifierTruncation {
    public string OrderId { get; init; } = string.Empty;
  }
}
