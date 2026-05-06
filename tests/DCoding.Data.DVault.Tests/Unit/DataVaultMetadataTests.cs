using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultMetadataTests {
  [Fact]
  public void HubMetadataRetainsIdentifyingProperties() {
    var hub = new DataVaultHubMetadata("Customer", ["CustomerId", "SourceSystem"]);

    Assert.Equal("Customer", hub.Name);
    Assert.Equal(["CustomerId", "SourceSystem"], hub.BusinessKeyNames);
    Assert.Equal(["CustomerId", "SourceSystem"], hub.BusinessKeyColumns.Select(column => column.ColumnName));
    Assert.Equal(TechnicalMetadataColumnRole.HashKey, hub.HashKeyMetadata.Role);
    Assert.Equal(TechnicalMetadataColumnRole.LoadTimestamp, hub.LoadTimestampMetadata.Role);
    Assert.Equal(TechnicalMetadataColumnRole.RecordSource, hub.RecordSourceMetadata.Role);
    AssertRequiredRoles(
        hub.TechnicalMetadataColumns,
        TechnicalMetadataColumnRole.HashKey,
        TechnicalMetadataColumnRole.LoadTimestamp,
        TechnicalMetadataColumnRole.RecordSource);

    var reference = hub.ToReference();
    Assert.Equal(DataVaultMetadataReferenceKind.Hub, reference.Kind);
    Assert.Equal("Customer", reference.Name);
  }

  [Fact]
  public void LinkMetadataRetainsAtLeastTwoHubEndpoints() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);

    var link = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);

    Assert.Equal("CustomerOrder", link.Name);
    Assert.Equal(2, link.Endpoints.Count);
    Assert.All(link.Endpoints, endpoint => Assert.Equal(DataVaultMetadataReferenceKind.Hub, endpoint.Kind));
    Assert.Equal("Customer", link.Endpoints[0].Name);
    Assert.Equal("Order", link.Endpoints[1].Name);
    Assert.Equal(2, link.Participants.Count);
    Assert.Equal("Customer", link.Participants[0].HubReference.Name);
    Assert.Equal("Order", link.Participants[1].HubReference.Name);
    Assert.All(link.Participants, participant => {
      Assert.Equal(DataVaultMetadataReferenceKind.Hub, participant.HubReference.Kind);
      Assert.Equal(TechnicalMetadataColumnRole.HashKey, participant.HashKeyMetadata.Role);
    });
    Assert.Equal(TechnicalMetadataColumnRole.HashKey, link.HashKeyMetadata.Role);
    Assert.Equal(TechnicalMetadataColumnRole.LoadTimestamp, link.LoadTimestampMetadata.Role);
    Assert.Equal(TechnicalMetadataColumnRole.RecordSource, link.RecordSourceMetadata.Role);
    AssertRequiredRoles(
        link.TechnicalMetadataColumns,
        TechnicalMetadataColumnRole.HashKey,
        TechnicalMetadataColumnRole.LoadTimestamp,
        TechnicalMetadataColumnRole.RecordSource);

    var reference = link.ToReference();
    Assert.Equal(DataVaultMetadataReferenceKind.Link, reference.Kind);
    Assert.Equal("CustomerOrder", reference.Name);
  }

  [Fact]
  public void BridgeMetadataRetainsManyToManyTraversalSelectors() {
    var customer = DataVaultMetadataReference.Hub("Customer");
    var product = DataVaultMetadataReference.Hub("Product");
    var customerProduct = DataVaultMetadataReference.Link("CustomerProduct");

    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerProductBridge",
        customer,
        customerProduct,
        product,
        sourceParticipantOrdinal: 0,
        targetParticipantOrdinal: 1);

    Assert.Equal("CustomerProductBridge", bridge.Name);
    Assert.Equal(DataVaultBridgeKind.ManyToMany, bridge.Kind);
    Assert.Equal(DataVaultMetadataReferenceKind.Hub, bridge.SourceHubReference.Kind);
    Assert.Equal("Customer", bridge.SourceHubReference.Name);
    Assert.Equal(DataVaultMetadataReferenceKind.Link, bridge.LinkReference.Kind);
    Assert.Equal("CustomerProduct", bridge.LinkReference.Name);
    Assert.Equal(DataVaultMetadataReferenceKind.Hub, bridge.TargetHubReference.Kind);
    Assert.Equal("Product", bridge.TargetHubReference.Name);
    Assert.Equal(0, bridge.SourceParticipantOrdinal);
    Assert.Equal(1, bridge.TargetParticipantOrdinal);
  }

  [Fact]
  public void BridgeMetadataRetainsHierarchyTraversalSelectors() {
    var employee = DataVaultMetadataReference.Hub("Employee");
    var employeeReportsTo = DataVaultMetadataReference.Link("EmployeeReportsTo");

    var bridge = DataVaultBridgeMetadata.Hierarchy(
        "EmployeeHierarchy",
        employee,
        employeeReportsTo,
        employee,
        ancestorParticipantOrdinal: 0,
        descendantParticipantOrdinal: 1);

    Assert.Equal("EmployeeHierarchy", bridge.Name);
    Assert.Equal(DataVaultBridgeKind.Hierarchy, bridge.Kind);
    Assert.Equal("Employee", bridge.SourceHubReference.Name);
    Assert.Equal("EmployeeReportsTo", bridge.LinkReference.Name);
    Assert.Equal("Employee", bridge.TargetHubReference.Name);
    Assert.Equal(0, bridge.SourceParticipantOrdinal);
    Assert.Equal(1, bridge.TargetParticipantOrdinal);
  }

  [Fact]
  public void SatelliteMetadataRetainsHubParentAndDescriptiveAttributes() {
    var parent = DataVaultMetadataReference.Hub("Customer");

    var satellite = new DataVaultSatelliteMetadata(
        "CustomerContact",
        parent,
        ["EmailAddress", "PhoneNumber"]);

    Assert.Equal("CustomerContact", satellite.Name);
    Assert.Equal(DataVaultMetadataReferenceKind.Hub, satellite.Parent.Kind);
    Assert.Equal("Customer", satellite.Parent.Name);
    Assert.Equal(["EmailAddress", "PhoneNumber"], satellite.DescriptiveAttributeNames);
    Assert.Equal(["EmailAddress", "PhoneNumber"], satellite.PayloadColumns.Select(column => column.ColumnName));
    Assert.Equal(TechnicalMetadataColumnRole.HashDiff, satellite.HashDiffMetadata.Role);
    Assert.Equal(TechnicalMetadataColumnRole.LoadTimestamp, satellite.LoadTimestampMetadata.Role);
    Assert.Equal(TechnicalMetadataColumnRole.RecordSource, satellite.RecordSourceMetadata.Role);
    AssertRequiredRoles(
        satellite.TechnicalMetadataColumns,
        TechnicalMetadataColumnRole.HashDiff,
        TechnicalMetadataColumnRole.LoadTimestamp,
        TechnicalMetadataColumnRole.RecordSource);
  }

  [Fact]
  public void SatelliteMetadataRetainsLinkParent() {
    var parent = DataVaultMetadataReference.Link("CustomerOrder");

    var satellite = new DataVaultSatelliteMetadata("OrderStatus", parent, ["Status"]);

    Assert.Equal(DataVaultMetadataReferenceKind.Link, satellite.Parent.Kind);
    Assert.Equal("CustomerOrder", satellite.Parent.Name);
    Assert.Equal(["Status"], satellite.DescriptiveAttributeNames);
  }

  [Fact]
  public void PointInTimeMetadataRetainsHubAndSatelliteReferences() {
    var pointInTime = new DataVaultPointInTimeMetadata(
        "CustomerHistory",
        DataVaultMetadataReference.Hub("Customer"),
        [
            DataVaultMetadataReference.Satellite("Contact"),
            DataVaultMetadataReference.Satellite("Preferences"),
        ]);

    Assert.Equal("CustomerHistory", pointInTime.Name);
    Assert.Equal(DataVaultMetadataReferenceKind.Hub, pointInTime.HubReference.Kind);
    Assert.Equal("Customer", pointInTime.HubReference.Name);
    Assert.Equal(
        [DataVaultMetadataReferenceKind.Satellite, DataVaultMetadataReferenceKind.Satellite],
        pointInTime.SatelliteReferences.Select(reference => reference.Kind));
    Assert.Equal(["Contact", "Preferences"], pointInTime.SatelliteReferences.Select(reference => reference.Name));
  }

  [Fact]
  public void MetadataModelValidatesPointInTimeReferencesAgainstDeclaredHubsAndSatellites() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var contact = new DataVaultSatelliteMetadata("Contact", customer.ToReference(), ["EmailAddress"]);
    var preferences = new DataVaultSatelliteMetadata("Preferences", customer.ToReference(), ["LanguageCode"]);
    var orderStatus = new DataVaultSatelliteMetadata("OrderStatus", order.ToReference(), ["StatusCode"]);

    var model = new DataVaultMetadataModel(
        [customer, order],
        [],
        [contact, preferences, orderStatus],
        [
            new DataVaultPointInTimeMetadata(
                "CustomerHistory",
                customer.ToReference(),
                [
                    DataVaultMetadataReference.Satellite("Contact"),
                    DataVaultMetadataReference.Satellite("Preferences"),
                ]),
        ]);

    var pointInTime = Assert.Single(model.PointInTimeTables);

    Assert.Equal("CustomerHistory", pointInTime.Name);
    Assert.Equal("Customer", pointInTime.HubReference.Name);
    Assert.Equal(["Contact", "Preferences"], pointInTime.SatelliteReferences.Select(reference => reference.Name));
  }

  [Fact]
  public void MetadataModelRejectsInvalidPointInTimeReferences() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var contact = new DataVaultSatelliteMetadata("Contact", customer.ToReference(), ["EmailAddress"]);
    var orderStatus = new DataVaultSatelliteMetadata("OrderStatus", order.ToReference(), ["StatusCode"]);

    AssertPointInTimeValidation(
        "missing hub",
        [],
        [contact],
        new DataVaultPointInTimeMetadata(
            "CustomerHistory",
            customer.ToReference(),
            [DataVaultMetadataReference.Satellite("Contact")]));
    AssertPointInTimeValidation(
        "at least one satellite",
        [customer],
        [contact],
        new DataVaultPointInTimeMetadata("CustomerHistory", customer.ToReference(), []));
    AssertPointInTimeValidation(
        "missing satellite",
        [customer],
        [contact],
        new DataVaultPointInTimeMetadata(
            "CustomerHistory",
            customer.ToReference(),
            [DataVaultMetadataReference.Satellite("Preferences")]));
    AssertPointInTimeValidation(
        "does not belong to hub",
        [customer, order],
        [contact, orderStatus],
        new DataVaultPointInTimeMetadata(
            "CustomerHistory",
            customer.ToReference(),
            [DataVaultMetadataReference.Satellite("OrderStatus")]));
    AssertPointInTimeValidation(
        "more than once",
        [customer],
        [contact],
        new DataVaultPointInTimeMetadata(
            "CustomerHistory",
            customer.ToReference(),
            [
                DataVaultMetadataReference.Satellite("Contact"),
                DataVaultMetadataReference.Satellite("Contact"),
            ]));
  }

  [Fact]
  public void PitMetadataRetainsParentAndSatelliteDeclarationOrder() {
    var pit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [
            new DataVaultPitSatelliteReferenceMetadata("Profile"),
            new DataVaultPitSatelliteReferenceMetadata("Status", isMultiActive: true),
        ]);

    Assert.Equal("CustomerProfileStatus", pit.Name);
    Assert.Equal(DataVaultMetadataReferenceKind.Hub, pit.Parent.Kind);
    Assert.Equal("Customer", pit.Parent.Name);
    Assert.Equal(["Profile", "Status"], pit.Satellites.Select(satellite => satellite.SatelliteName));
    Assert.False(pit.Satellites[0].IsMultiActive);
    Assert.True(pit.Satellites[1].IsMultiActive);
    Assert.Equal(TechnicalMetadataColumnRole.HashKey, pit.HashKeyMetadata.Role);
    Assert.Equal(TechnicalMetadataColumnRole.LoadTimestamp, pit.LoadTimestampMetadata.Role);
    AssertRequiredRoles(
        pit.TechnicalMetadataColumns,
        TechnicalMetadataColumnRole.HashKey,
        TechnicalMetadataColumnRole.LoadTimestamp);
  }

  [Fact]
  public void BridgeMetadataRetainsSourceEndpointBindingsAndProjectionFeatures() {
    var bridge = new DataVaultBridgeMetadata(
        "SalesRegionHierarchy",
        DataVaultBridgeKind.Hierarchy,
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Ancestor,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ParentRegion"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Descendant,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ChildRegion"),
        ],
        DataVaultBridgeProjectionFeatures.EffectivityWindow);

    Assert.Equal("SalesRegionHierarchy", bridge.Name);
    Assert.Equal(DataVaultBridgeKind.Hierarchy, bridge.Kind);
    Assert.Equal(DataVaultMetadataReferenceKind.Link, bridge.Source.Kind);
    Assert.Equal("SalesRegionParentChild", bridge.Source.Name);
    Assert.Equal(DataVaultBridgeProjectionFeatures.EffectivityWindow, bridge.ProjectionFeatures);
    Assert.Equal(
        [DataVaultBridgeEndpointRole.Ancestor, DataVaultBridgeEndpointRole.Descendant],
        bridge.Endpoints.Select(endpoint => endpoint.Role));
    Assert.Equal(["SalesRegion", "SalesRegion"], bridge.Endpoints.Select(endpoint => endpoint.HubReference.Name));
    Assert.Equal(["ParentRegion", "ChildRegion"], bridge.Endpoints.Select(endpoint => endpoint.SourceEndpointName));
    Assert.All(bridge.Endpoints, endpoint => Assert.Equal(TechnicalMetadataColumnRole.HashKey, endpoint.HashKeyMetadata.Role));
  }

  [Fact]
  public void BridgeMetadataRequiresEndpointRolesForBridgeKind() {
    ThrowsArgumentException(() => new DataVaultBridgeMetadata(
        "CustomerOrder",
        DataVaultBridgeKind.ManyToMany,
        DataVaultMetadataReference.Link("CustomerOrder"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.From,
                DataVaultMetadataReference.Hub("Customer"),
                "Customer"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Ancestor,
                DataVaultMetadataReference.Hub("Order"),
                "Order"),
        ]));
    ThrowsArgumentException(() => new DataVaultBridgeMetadata(
        "CustomerOrder",
        DataVaultBridgeKind.ManyToMany,
        DataVaultMetadataReference.Link("CustomerOrder"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.From,
                DataVaultMetadataReference.Hub("Customer"),
                "Customer"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.To,
                DataVaultMetadataReference.Hub("Order"),
                "Order"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.To,
                DataVaultMetadataReference.Hub("Invoice"),
                "Invoice"),
        ]));
    ThrowsArgumentException(() => new DataVaultBridgeMetadata(
        "SalesRegionHierarchy",
        DataVaultBridgeKind.Hierarchy,
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.From,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ParentRegion"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.To,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ChildRegion"),
        ]));
    ThrowsArgumentException(() => new DataVaultBridgeMetadata(
        "SalesRegionHierarchy",
        DataVaultBridgeKind.Hierarchy,
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Ancestor,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ParentRegion"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Ancestor,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ChildRegion"),
        ]));
  }

  [Fact]
  public void MetadataAbstractionsUseProviderNeutralClrContracts() {
    var metadataTypes = new[]
    {
        typeof(DataVaultBusinessKeyMetadata),
        typeof(DataVaultBridgeEndpointMetadata),
        typeof(DataVaultBridgeMetadata),
        typeof(DataVaultHubMetadata),
        typeof(DataVaultLinkMetadata),
        typeof(DataVaultLinkParticipantMetadata),
        typeof(DataVaultPointInTimeMetadata),
        typeof(DataVaultPitMetadata),
        typeof(DataVaultPitSatelliteReferenceMetadata),
        typeof(DataVaultSatelliteMetadata),
        typeof(DataVaultSatellitePayloadMetadata),
    };
    var providerTokens = new[] { "Sqlite", "Postgres", "Npgsql", "Migration", "Sequence", "Trigger" };

    foreach (var metadataType in metadataTypes) {
      Assert.DoesNotContain(providerTokens, token => metadataType.FullName!.Contains(token, StringComparison.OrdinalIgnoreCase));
      Assert.DoesNotContain(metadataType.GetProperties(), property =>
          providerTokens.Any(token => property.PropertyType.FullName!.Contains(token, StringComparison.OrdinalIgnoreCase)));
    }
  }

  [Fact]
  public void RequiredMetadataNamesRejectNullEmptyAndWhitespace() {
    foreach (var invalidName in new string?[] { null, "", " " }) {
      ThrowsArgumentException(() => new DataVaultHubMetadata(invalidName!, ["CustomerId"]));
      ThrowsArgumentException(() => new DataVaultHubMetadata("Customer", [invalidName!]));
      ThrowsArgumentException(() => DataVaultMetadataReference.Hub(invalidName!));
      ThrowsArgumentException(() => DataVaultMetadataReference.Link(invalidName!));
      ThrowsArgumentException(() => DataVaultMetadataReference.Satellite(invalidName!));
      ThrowsArgumentException(() => DataVaultBridgeMetadata.ManyToMany(
          invalidName!,
          DataVaultMetadataReference.Hub("Customer"),
          DataVaultMetadataReference.Link("CustomerProduct"),
          DataVaultMetadataReference.Hub("Product")));
      ThrowsArgumentException(() => new DataVaultLinkMetadata(
          invalidName!,
          [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]));
      ThrowsArgumentException(() => new DataVaultBridgeMetadata(
          invalidName!,
          DataVaultBridgeKind.ManyToMany,
          DataVaultMetadataReference.Link("CustomerOrder"),
          [
              new DataVaultBridgeEndpointMetadata(
                  DataVaultBridgeEndpointRole.From,
                  DataVaultMetadataReference.Hub("Customer"),
                  "Customer"),
          ]));
      ThrowsArgumentException(() => new DataVaultBridgeEndpointMetadata(
          DataVaultBridgeEndpointRole.From,
          DataVaultMetadataReference.Hub("Customer"),
          invalidName!));
      ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
          invalidName!,
          DataVaultMetadataReference.Hub("Customer"),
          ["EmailAddress"]));
      ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
          "CustomerContact",
          DataVaultMetadataReference.Hub("Customer"),
          [invalidName!]));
      ThrowsArgumentException(() => new DataVaultPointInTimeMetadata(
          invalidName!,
          DataVaultMetadataReference.Hub("Customer"),
          [DataVaultMetadataReference.Satellite("Contact")]));
      ThrowsArgumentException(() => new DataVaultBusinessKeyMetadata(invalidName!));
      ThrowsArgumentException(() => new DataVaultPitMetadata(
          DataVaultMetadataReference.Hub(invalidName!),
          ["Profile"]));
      ThrowsArgumentException(() => new DataVaultPitMetadata(
          DataVaultMetadataReference.Hub("Customer"),
          [invalidName!]));
      ThrowsArgumentException(() => new DataVaultPitSatelliteReferenceMetadata(invalidName!));
      ThrowsArgumentException(() => new DataVaultSatellitePayloadMetadata(invalidName!));
    }
  }

  [Fact]
  public void RequiredMetadataCollectionsRejectNullAndEmpty() {
    ThrowsArgumentException(() => new DataVaultHubMetadata("Customer", null!));
    ThrowsArgumentException(() => new DataVaultHubMetadata("Customer", []));
    ThrowsArgumentException(() => new DataVaultLinkMetadata("CustomerOrder", null!));
    ThrowsArgumentException(() => new DataVaultLinkMetadata("CustomerOrder", []));
    ThrowsArgumentException(() => new DataVaultBridgeMetadata(
        "CustomerOrder",
        DataVaultBridgeKind.ManyToMany,
        DataVaultMetadataReference.Link("CustomerOrder"),
        null!));
    ThrowsArgumentException(() => new DataVaultBridgeMetadata(
        "CustomerOrder",
        DataVaultBridgeKind.ManyToMany,
        DataVaultMetadataReference.Link("CustomerOrder"),
        []));
    ThrowsArgumentException(() => new DataVaultBridgeMetadata(
        "CustomerOrder",
        DataVaultBridgeKind.ManyToMany,
        DataVaultMetadataReference.Link("CustomerOrder"),
        [null!]));
    ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
        "CustomerContact",
        DataVaultMetadataReference.Hub("Customer"),
        null!));
    ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
        "CustomerContact",
        DataVaultMetadataReference.Hub("Customer"),
        []));
    Assert.Throws<ArgumentNullException>(() => new DataVaultPointInTimeMetadata(
        "CustomerHistory",
        DataVaultMetadataReference.Hub("Customer"),
        null!));
    ThrowsArgumentException(() => new DataVaultPitMetadata(
        null!,
        ["Profile"]));
    ThrowsArgumentException(() => new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        (IEnumerable<string>)null!));
    ThrowsArgumentException(() => new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        (IEnumerable<DataVaultPitSatelliteReferenceMetadata>)null!));
  }

  [Fact]
  public void LinkMetadataRequiresAtLeastTwoHubEndpoints() {
    ThrowsArgumentException(() => new DataVaultLinkMetadata(
        "CustomerOrder",
        [DataVaultMetadataReference.Hub("Customer")]));
    ThrowsArgumentException(() => new DataVaultLinkMetadata(
        "CustomerOrder",
        [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Link("OrderPayment")]));
    ThrowsArgumentException(() => new DataVaultLinkParticipantMetadata(DataVaultMetadataReference.Link("OrderPayment")));
    ThrowsArgumentException(() => new DataVaultBridgeMetadata(
        "CustomerOrder",
        DataVaultBridgeKind.ManyToMany,
        DataVaultMetadataReference.Hub("Customer"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.From,
                DataVaultMetadataReference.Hub("Customer"),
                "Customer"),
        ]));
    ThrowsArgumentException(() => new DataVaultBridgeEndpointMetadata(
        DataVaultBridgeEndpointRole.From,
        DataVaultMetadataReference.Link("OrderPayment"),
        "Customer"));
  }

  [Fact]
  public void BridgeMetadataRequiresValidTraversalReferencesAndOrdinals() {
    ThrowsArgumentException(() => DataVaultBridgeMetadata.ManyToMany(
        "CustomerProductBridge",
        DataVaultMetadataReference.Link("CustomerProduct"),
        DataVaultMetadataReference.Link("CustomerProduct"),
        DataVaultMetadataReference.Hub("Product")));
    ThrowsArgumentException(() => DataVaultBridgeMetadata.ManyToMany(
        "CustomerProductBridge",
        DataVaultMetadataReference.Hub("Customer"),
        DataVaultMetadataReference.Hub("Customer"),
        DataVaultMetadataReference.Hub("Product")));
    ThrowsArgumentException(() => DataVaultBridgeMetadata.ManyToMany(
        "CustomerProductBridge",
        DataVaultMetadataReference.Hub("Customer"),
        DataVaultMetadataReference.Link("CustomerProduct"),
        DataVaultMetadataReference.Link("CustomerProduct")));
    ThrowsArgumentException(() => DataVaultBridgeMetadata.ManyToMany(
        "CustomerProductBridge",
        DataVaultMetadataReference.Hub("Customer"),
        DataVaultMetadataReference.Link("CustomerProduct"),
        DataVaultMetadataReference.Hub("Product"),
        sourceParticipantOrdinal: -1,
        targetParticipantOrdinal: 1));
    ThrowsArgumentException(() => new DataVaultBridgeMetadata(
        "CustomerProductBridge",
        (DataVaultBridgeKind)42,
        DataVaultMetadataReference.Hub("Customer"),
        DataVaultMetadataReference.Link("CustomerProduct"),
        DataVaultMetadataReference.Hub("Product")));
  }

  [Fact]
  public void SatelliteMetadataRequiresParentRelationship() {
    ThrowsArgumentException(() => new DataVaultSatelliteMetadata(
        "CustomerContact",
        null!,
        ["EmailAddress"]));
  }

  [Fact]
  public void PointInTimeMetadataRequiresHubAndSatelliteReferences() {
    ThrowsArgumentException(() => new DataVaultPointInTimeMetadata(
        "CustomerHistory",
        DataVaultMetadataReference.Link("CustomerOrder"),
        [DataVaultMetadataReference.Satellite("Contact")]));
    ThrowsArgumentException(() => new DataVaultPointInTimeMetadata(
        "CustomerHistory",
        DataVaultMetadataReference.Hub("Customer"),
        [DataVaultMetadataReference.Hub("Customer")]));
  }

  [Fact]
  public void MetadataModelRetainsOptionalBridgeDeclarations() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var product = new DataVaultHubMetadata("Product", ["ProductId"]);
    var customerProduct = new DataVaultLinkMetadata(
        "CustomerProduct",
        [customer.ToReference(), product.ToReference()]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerProductBridge",
        customer.ToReference(),
        customerProduct.ToReference(),
        product.ToReference(),
        sourceParticipantOrdinal: 0,
        targetParticipantOrdinal: 1);

    var modelWithoutBridges = new DataVaultMetadataModel([customer, product], [customerProduct], []);
    var modelWithBridges = new DataVaultMetadataModel([customer, product], [customerProduct], [], [bridge]);

    Assert.Empty(modelWithoutBridges.Bridges);
    Assert.Equal([bridge], modelWithBridges.Bridges);
  }

  [Fact]
  public void MetadataModelAcceptsSingleLinkHierarchyBridgeDeclaration() {
    var employee = new DataVaultHubMetadata("Employee", ["EmployeeId"]);
    var employeeReportsTo = new DataVaultLinkMetadata(
        "EmployeeReportsTo",
        [employee.ToReference(), employee.ToReference()]);
    var bridge = DataVaultBridgeMetadata.Hierarchy(
        "EmployeeHierarchy",
        employee.ToReference(),
        employeeReportsTo.ToReference(),
        employee.ToReference(),
        ancestorParticipantOrdinal: 0,
        descendantParticipantOrdinal: 1);

    var model = new DataVaultMetadataModel([employee], [employeeReportsTo], [], [bridge]);

    Assert.Equal([bridge], model.Bridges);
  }

  [Fact]
  public void MetadataModelRejectsBridgeWithUnknownHubReference() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var product = new DataVaultHubMetadata("Product", ["ProductId"]);
    var customerProduct = new DataVaultLinkMetadata(
        "CustomerProduct",
        [customer.ToReference(), product.ToReference()]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerProductBridge",
        customer.ToReference(),
        customerProduct.ToReference(),
        product.ToReference(),
        sourceParticipantOrdinal: 0,
        targetParticipantOrdinal: 1);

    ThrowsArgumentException(() => new DataVaultMetadataModel([customer], [customerProduct], [], [bridge]));
  }

  [Fact]
  public void MetadataModelRejectsBridgeWithUnknownLinkReference() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var product = new DataVaultHubMetadata("Product", ["ProductId"]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerProductBridge",
        customer.ToReference(),
        DataVaultMetadataReference.Link("CustomerProduct"),
        product.ToReference(),
        sourceParticipantOrdinal: 0,
        targetParticipantOrdinal: 1);

    ThrowsArgumentException(() => new DataVaultMetadataModel([customer, product], [], [], [bridge]));
  }

  [Fact]
  public void MetadataModelRejectsBridgeParticipantSelectorOutsideReferencedLink() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var product = new DataVaultHubMetadata("Product", ["ProductId"]);
    var customerProduct = new DataVaultLinkMetadata(
        "CustomerProduct",
        [customer.ToReference(), product.ToReference()]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerProductBridge",
        customer.ToReference(),
        customerProduct.ToReference(),
        product.ToReference(),
        sourceParticipantOrdinal: 1,
        targetParticipantOrdinal: 0);

    ThrowsArgumentException(() => new DataVaultMetadataModel([customer, product], [customerProduct], [], [bridge]));
  }

  [Fact]
  public void MetadataModelRejectsAmbiguousBridgeEndpointSelection() {
    var employee = new DataVaultHubMetadata("Employee", ["EmployeeId"]);
    var employeeReportsTo = new DataVaultLinkMetadata(
        "EmployeeReportsTo",
        [employee.ToReference(), employee.ToReference()]);
    var bridge = new DataVaultBridgeMetadata(
        "EmployeeHierarchy",
        DataVaultBridgeKind.Hierarchy,
        employee.ToReference(),
        employeeReportsTo.ToReference(),
        employee.ToReference());

    ThrowsArgumentException(() => new DataVaultMetadataModel([employee], [employeeReportsTo], [], [bridge]));
  }

  [Theory]
  [InlineData(0)]
  [InlineData(1)]
  public void MetadataModelRejectsHierarchyBridgeSelfCycle(int participantOrdinal) {
    var employee = new DataVaultHubMetadata("Employee", ["EmployeeId"]);
    var employeeReportsTo = new DataVaultLinkMetadata(
        "EmployeeReportsTo",
        [employee.ToReference(), employee.ToReference()]);
    var bridge = DataVaultBridgeMetadata.Hierarchy(
        "EmployeeHierarchy",
        employee.ToReference(),
        employeeReportsTo.ToReference(),
        employee.ToReference(),
        ancestorParticipantOrdinal: participantOrdinal,
        descendantParticipantOrdinal: participantOrdinal);

    ThrowsArgumentException(() => new DataVaultMetadataModel([employee], [employeeReportsTo], [], [bridge]));
  }

  private static void ThrowsArgumentException(Action action) {
    var exception = Record.Exception(action);

    Assert.IsAssignableFrom<ArgumentException>(exception);
  }

  private static void AssertPointInTimeValidation(
      string expectedMessage,
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      DataVaultPointInTimeMetadata pointInTimeTable) {
    var exception = Assert.Throws<ArgumentException>(() => new DataVaultMetadataModel(hubs, [], satellites, [pointInTimeTable]));

    Assert.Equal("pointInTimeTables", exception.ParamName);
    Assert.Contains(expectedMessage, exception.Message, StringComparison.Ordinal);
  }

  private static void AssertRequiredRoles(
      IReadOnlyList<TechnicalMetadataColumnContract> contracts,
      params TechnicalMetadataColumnRole[] expectedRoles) {
    Assert.Equal(expectedRoles, contracts.Select(contract => contract.Role));
    Assert.All(contracts, contract => {
      Assert.Equal(TechnicalMetadataColumnRequiredness.RequiredWhenDeclared, contract.RequirednessExpectation);
      Assert.False(string.IsNullOrWhiteSpace(contract.DefaultEffectiveColumnName));
      Assert.Equal(contract.DefaultEffectiveColumnName, contract.EffectiveColumnName);
    });
  }
}
