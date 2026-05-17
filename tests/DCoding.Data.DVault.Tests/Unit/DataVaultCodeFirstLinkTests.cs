using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultCodeFirstLinkTests {
  [Fact]
  public void ApplyDataVaultMetadataProjectsExplicitNameTwoParticipantLinkThroughMetadataTranslator() {
    void Configure(DataVaultCodeFirstModelBuilder vault) {
      vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
      vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));

      vault.Link("CustomerOrder", link => {
        link.Participant<Customer>();
        link.Participant<Order>();
      });
    }

    var codeFirstMetadata = CreateCodeFirstMetadata(Configure);
    var codeFirstModel = TranslateCodeFirst(Configure);
    var metadataFirstMetadata = CreateExplicitTwoParticipantMetadata();

    AssertLinkMetadata(codeFirstMetadata, "CustomerOrder", ["Customer", "Order"]);
    Assert.Equal(ModelShape(Translate(metadataFirstMetadata)), ModelShape(codeFirstModel));

    var link = FindEntity(codeFirstModel, "LinkCustomerOrder");

    AssertRelationalEntity(
        link,
        "LinkCustomerOrder",
        ["CustomerOrderHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKey", "OrderHashKey"],
        "PkLinkCustomerOrderCustomerOrderHashKey",
        "IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey",
        ["CustomerHashKey", "OrderHashKey"]);
  }

  [Fact]
  public void ApplyDataVaultMetadataProjectsRoleBearingSameHubLinkThroughMetadataTranslator() {
    void Configure(DataVaultCodeFirstModelBuilder vault) {
      vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));

      vault.Link("CustomerIdentityMatch", link => {
        link.Participant<Customer>("SourceCustomer");
        link.Participant<Customer>("MatchedCustomer");
      });
    }

    var codeFirstMetadata = CreateCodeFirstMetadata(Configure);
    var codeFirstModel = TranslateCodeFirst(Configure);
    var metadataFirstMetadata = CreateSameHubRoleMetadata();

    AssertLinkMetadata(
        codeFirstMetadata,
        "CustomerIdentityMatch",
        ["Customer", "Customer"],
        ["SourceCustomer", "MatchedCustomer"]);
    Assert.Equal(ModelShape(Translate(metadataFirstMetadata)), ModelShape(codeFirstModel));

    var link = FindEntity(codeFirstModel, "LinkCustomerIdentityMatch");

    AssertRelationalEntity(
        link,
        "LinkCustomerIdentityMatch",
        [
            "CustomerIdentityMatchHashKey",
            "LoadTimestamp",
            "RecordSource",
            "SourceCustomerHashKey",
            "MatchedCustomerHashKey",
        ],
        "PkLinkCustomerIdentityMatchCustomerIdentityMatchHashKey",
        "IxLinkCustomerIdentityMatchRelationshipSourceCustomerHashKeyMatchedCustomerHashKey",
        ["SourceCustomerHashKey", "MatchedCustomerHashKey"]);
  }

  [Fact]
  public void ApplyDataVaultMetadataProjectsDerivedNameMultiParticipantLinkInDeclarationOrder() {
    void Configure(DataVaultCodeFirstModelBuilder vault) {
      vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
      vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));
      vault.Hub<SaleRegion>(hub => {
        hub.BusinessKey(region => region.CountryCode);
        hub.BusinessKey(region => region.RegionCode);
      });

      vault.Link(link => {
        link.Participant<Customer>();
        link.Participant<Order>();
        link.Participant<SaleRegion>();
      });
    }

    var codeFirstMetadata = CreateCodeFirstMetadata(Configure);
    var codeFirstModel = TranslateCodeFirst(Configure);
    var metadataFirstMetadata = CreateDerivedMultiParticipantMetadata();

    AssertLinkMetadata(codeFirstMetadata, "CustomerOrderSaleRegion", ["Customer", "Order", "SaleRegion"]);
    Assert.Equal(ModelShape(Translate(metadataFirstMetadata)), ModelShape(codeFirstModel));

    var link = FindEntity(codeFirstModel, "LinkCustomerOrderSaleRegion");

    AssertRelationalEntity(
        link,
        "LinkCustomerOrderSaleRegion",
        [
            "CustomerOrderSaleRegionHashKey",
            "LoadTimestamp",
            "RecordSource",
            "CustomerHashKey",
            "OrderHashKey",
            "SaleRegionHashKey",
        ],
        "PkLinkCustomerOrderSaleRegionCustomerOrderSaleRegionHashKey",
        "IxLinkCustomerOrderSaleRegionRelationshipCustomerHashKeyOrderHashKeySaleRegionHashKey",
        ["CustomerHashKey", "OrderHashKey", "SaleRegionHashKey"]);
  }

  [Fact]
  public void ApplyDataVaultMetadataProjectsDerivedNameLinkParentSatelliteThroughMetadataTranslator() {
    void Configure(DataVaultCodeFirstModelBuilder vault) {
      vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
      vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));

      vault.Link(link => {
        link.Participant<Customer>();
        link.Participant<Order>();
        link.Satellite<CustomerOrderState>("State", satellite => {
          satellite.DrivingKey(state => state.StateSource);
          satellite.Payload(state => state.StatusCode);
          satellite.Payload(state => state.StateChangedAt);
        });
      });
    }

    var codeFirstMetadata = CreateCodeFirstMetadata(Configure);
    var codeFirstModel = TranslateCodeFirst(Configure);
    var metadataFirstMetadata = CreateDerivedLinkParentSatelliteMetadata();

    AssertLinkMetadata(codeFirstMetadata, "CustomerOrder", ["Customer", "Order"]);

    var metadataSatellite = Assert.Single(codeFirstMetadata.Satellites);

    Assert.Equal("State", metadataSatellite.Name);
    Assert.Equal(DataVaultMetadataReferenceKind.Link, metadataSatellite.Parent.Kind);
    Assert.Equal("CustomerOrder", metadataSatellite.Parent.Name);
    Assert.Equal([nameof(CustomerOrderState.StateSource)], metadataSatellite.DrivingKeyNames);
    Assert.Equal(
        [nameof(CustomerOrderState.StatusCode), nameof(CustomerOrderState.StateChangedAt)],
        metadataSatellite.DescriptiveAttributeNames);
    Assert.Equal(ModelShape(Translate(metadataFirstMetadata)), ModelShape(codeFirstModel));

    var satellite = FindEntity(codeFirstModel, "SatCustomerOrderState");

    Assert.Equal(
        DataVaultMetadataReferenceKind.Link,
        AnnotationValue<DataVaultMetadataReferenceKind>(satellite, DataVaultAnnotationNames.ParentReferenceKind));
    Assert.Equal("CustomerOrder", AnnotationValue<string>(satellite, DataVaultAnnotationNames.ParentReferenceName));
    AssertRelationalEntity(
        satellite,
        "SatCustomerOrderState",
        [
            "CustomerOrderHashKey",
            "StateSource",
            "HashDiff",
            "LoadTimestamp",
            "RecordSource",
            "StatusCode",
            "StateChangedAt",
        ],
        "PkSatCustomerOrderStateCustomerOrderHashKeyStateSourceLoadTimestamp",
        "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyStateSourceLoadTimestamp",
        ["CustomerOrderHashKey", "StateSource", "LoadTimestamp", "HashDiff"]);
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsMissingParticipantHub() {
    var exception = Assert.Throws<ArgumentException>(() =>
        CreateCodeFirstMetadata(vault => {
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
          vault.Link("CustomerOrder", link => {
            link.Participant<Customer>();
            link.Participant<Order>();
          });
        }));

    Assert.Equal("configureModel", exception.ParamName);
    Assert.Contains("CustomerOrder", exception.Message, StringComparison.Ordinal);
    Assert.Contains(typeof(Order).FullName!, exception.Message, StringComparison.Ordinal);
    Assert.Contains("has not been configured as a hub", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsParticipantHubDeclaredAfterLink() {
    var exception = Assert.Throws<ArgumentException>(() =>
        CreateCodeFirstMetadata(vault => {
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
          vault.Link("CustomerOrder", link => {
            link.Participant<Customer>();
            link.Participant<Order>();
          });
          vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));
        }));

    Assert.Equal("configureModel", exception.ParamName);
    Assert.Contains("CustomerOrder", exception.Message, StringComparison.Ordinal);
    Assert.Contains(typeof(Order).FullName!, exception.Message, StringComparison.Ordinal);
    Assert.Contains("before this link declaration", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsAmbiguousParticipantHub() {
    var exception = Assert.Throws<ArgumentException>(() =>
        CreateCodeFirstMetadata(vault => {
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
          vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));
          vault.Link("CustomerOrder", link => {
            link.Participant<Customer>();
            link.Participant<Order>();
          });
        }));

    Assert.Equal("configureModel", exception.ParamName);
    Assert.Contains("CustomerOrder", exception.Message, StringComparison.Ordinal);
    Assert.Contains(typeof(Customer).FullName!, exception.Message, StringComparison.Ordinal);
    Assert.Contains("resolves to more than one configured hub", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsLinkWithTooFewParticipants() {
    var exception = Assert.Throws<ArgumentException>(() =>
        CreateCodeFirstMetadata(vault => {
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
          vault.Link("CustomerOnly", link => link.Participant<Customer>());
        }));

    Assert.Equal("configureModel", exception.ParamName);
    Assert.Contains("CustomerOnly", exception.Message, StringComparison.Ordinal);
    Assert.Contains("requires at least two participant declarations", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsRepeatedSameHubParticipantsWithoutRoles() {
    var exception = Assert.Throws<ArgumentException>(() =>
        CreateCodeFirstMetadata(vault => {
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
          vault.Link("CustomerHierarchy", link => {
            link.Participant<Customer>();
            link.Participant<Customer>();
          });
        }));

    Assert.Equal("configureModel", exception.ParamName);
    Assert.Contains("CustomerHierarchy", exception.Message, StringComparison.Ordinal);
    Assert.Contains("declares hub 'Customer' more than once", exception.Message, StringComparison.Ordinal);
    Assert.Contains("Every repeated same-hub participant must declare a distinct non-blank role", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsDuplicateRepeatedSameHubParticipantRoles() {
    var exception = Assert.Throws<ArgumentException>(() =>
        CreateCodeFirstMetadata(vault => {
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
          vault.Link("CustomerHierarchy", link => {
            link.Participant<Customer>("RelatedCustomer");
            link.Participant<Customer>("RelatedCustomer");
          });
        }));

    Assert.Equal("configureModel", exception.ParamName);
    Assert.Contains("CustomerHierarchy", exception.Message, StringComparison.Ordinal);
    Assert.Contains("duplicate participant role 'RelatedCustomer'", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsDerivedNameRepeatedSameHubParticipantRoles() {
    var exception = Assert.Throws<ArgumentException>(() =>
        CreateCodeFirstMetadata(vault => {
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
          vault.Link(link => {
            link.Participant<Customer>("SourceCustomer");
            link.Participant<Customer>("MatchedCustomer");
          });
        }));

    Assert.Equal("configureModel", exception.ParamName);
    Assert.Contains("derived relationship name", exception.Message, StringComparison.Ordinal);
    Assert.Contains("requires an explicit relationship name", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void BusinessKeyRejectsUnsupportedSelectorShapes() {
    var exception = Assert.Throws<ArgumentException>(() =>
        CreateCodeFirstMetadata(vault => {
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.Contact.EmailAddress));
          vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));
          vault.Link("CustomerOrder", link => {
            link.Participant<Customer>();
            link.Participant<Order>();
          });
        }));

    Assert.Equal("propertySelector", exception.ParamName);
    Assert.Contains("BusinessKey supports only a direct readable scalar member selector", exception.Message, StringComparison.Ordinal);
  }

  private static DataVaultMetadataModel CreateCodeFirstMetadata(Action<DataVaultCodeFirstModelBuilder> configureModel) {
    var builder = new DataVaultCodeFirstModelBuilder();
    configureModel(builder);

    return builder.BuildMetadataModel();
  }

  private static DataVaultMetadataModel CreateExplicitTwoParticipantMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);

    return new DataVaultMetadataModel(
        [customer, order],
        [new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()])],
        []);
  }

  private static DataVaultMetadataModel CreateDerivedMultiParticipantMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var saleRegion = new DataVaultHubMetadata("SaleRegion", ["CountryCode", "RegionCode"]);

    return new DataVaultMetadataModel(
        [customer, order, saleRegion],
        [
            new DataVaultLinkMetadata(
                "CustomerOrderSaleRegion",
                [customer.ToReference(), order.ToReference(), saleRegion.ToReference()]),
        ],
        []);
  }

  private static DataVaultMetadataModel CreateSameHubRoleMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);

    return new DataVaultMetadataModel(
        [customer],
        [
            new DataVaultLinkMetadata(
                "CustomerIdentityMatch",
                [
                    new DataVaultLinkParticipantMetadata(customer.ToReference(), "SourceCustomer"),
                    new DataVaultLinkParticipantMetadata(customer.ToReference(), "MatchedCustomer"),
                ]),
        ],
        []);
  }

  private static DataVaultMetadataModel CreateDerivedLinkParentSatelliteMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);

    return new DataVaultMetadataModel(
        [customer, order],
        [customerOrder],
        [
            new DataVaultSatelliteMetadata(
                "State",
                customerOrder.ToReference(),
                [nameof(CustomerOrderState.StatusCode), nameof(CustomerOrderState.StateChangedAt)],
                [nameof(CustomerOrderState.StateSource)]),
        ]);
  }

  private static IMutableModel Translate(DataVaultMetadataModel metadataModel) {
    var modelBuilder = new ModelBuilder(new ConventionSet());

    modelBuilder.ApplyDataVaultMetadata(metadataModel);

    return modelBuilder.Model;
  }

  private static IMutableModel TranslateCodeFirst(Action<DataVaultCodeFirstModelBuilder> configureModel) {
    var modelBuilder = new ModelBuilder(new ConventionSet());

    var result = modelBuilder.ApplyDataVaultMetadata(configureModel);

    Assert.Same(modelBuilder, result);
    return modelBuilder.Model;
  }

  private static void AssertLinkMetadata(
      DataVaultMetadataModel metadataModel,
      string expectedName,
      string[] expectedParticipantHubNames,
      string[]? expectedProducedParticipantNames = null) {
    var link = Assert.Single(metadataModel.Links);

    Assert.Equal(expectedName, link.Name);
    Assert.Equal(expectedParticipantHubNames, link.Participants.Select(participant => participant.HubReference.Name));
    Assert.Equal(
        expectedProducedParticipantNames ?? expectedParticipantHubNames,
        link.Participants.Select(participant => participant.SourceEndpointName));
  }

  private static void AssertRelationalEntity(
      IMutableEntityType entityType,
      string expectedTableName,
      string[] expectedColumnNames,
      string expectedPrimaryKeyName,
      string expectedIndexName,
      string[] expectedIndexColumnNames) {
    var table = StoreObjectIdentifier.Table(expectedTableName, schema: null);
    var index = Assert.Single(entityType.GetIndexes());

    Assert.Equal(expectedTableName, entityType.GetTableName());
    Assert.Equal(expectedColumnNames, PropertyNamesInOrdinalOrder(entityType));
    Assert.Equal(
        expectedColumnNames,
        entityType.GetProperties()
            .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
            .Select(property => property.GetColumnName(table)));
    Assert.Equal(expectedPrimaryKeyName, entityType.FindPrimaryKey()!.GetName());
    Assert.Equal(expectedIndexName, index.GetDatabaseName());
    Assert.Equal(expectedIndexColumnNames, index.Properties.Select(property => property.GetColumnName(table)));
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

  private static string[] PropertyNamesInOrdinalOrder(IMutableEntityType entityType) {
    return entityType.GetProperties()
        .OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))
        .Select(property => property.Name)
        .ToArray();
  }

  private static T AnnotationValue<T>(IMutableEntityType entityType, string name) {
    return Assert.IsType<T>(RequiredAnnotation(entityType.FindAnnotation(name)).Value);
  }

  private static T AnnotationValue<T>(IMutableProperty property, string name) {
    return Assert.IsType<T>(RequiredAnnotation(property.FindAnnotation(name)).Value);
  }

  private static T AnnotationValue<T>(IMutableIndex index, string name) {
    return Assert.IsType<T>(RequiredAnnotation(index.FindAnnotation(name)).Value);
  }

  private static IAnnotation RequiredAnnotation(IAnnotation? annotation) {
    Assert.NotNull(annotation);

    return annotation!;
  }

  private sealed class Customer {
    public string CustomerId { get; init; } = string.Empty;

    public Contact Contact { get; init; } = new();
  }

  private sealed class Contact {
    public string EmailAddress { get; init; } = string.Empty;
  }

  private sealed class Order {
    public string OrderId { get; init; } = string.Empty;
  }

  private sealed class CustomerOrderState {
    public string StateChangedAt { get; init; } = string.Empty;

    public string StateSource { get; init; } = string.Empty;

    public string StatusCode { get; init; } = string.Empty;
  }

  private sealed class SaleRegion {
    public string CountryCode { get; init; } = string.Empty;

    public string RegionCode { get; init; } = string.Empty;
  }
}
