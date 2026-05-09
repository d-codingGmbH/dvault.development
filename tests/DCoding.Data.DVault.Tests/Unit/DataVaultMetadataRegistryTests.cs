using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultMetadataRegistryTests {
  [Fact]
  public void RegistryRetainsDeclarationOrderAcrossMetadataAndProviderProfiles() {
    var model = CreateFullMetadataModel();

    var registry = DataVaultMetadataRegistry.Create(
        model,
        [DataVaultProviderCapabilityProfiles.Sqlite, DataVaultProviderCapabilityProfiles.Postgres]);

    Assert.Equal(["Customer", "Order"], registry.Hubs.Select(hub => hub.Name));
    Assert.Equal(["CustomerOrder"], registry.Links.Select(link => link.Name));
    Assert.Equal(["Contact", "Status"], registry.Satellites.Select(satellite => satellite.Name));
    Assert.Equal(["CustomerHistory"], registry.PointInTimeTables.Select(pointInTimeTable => pointInTimeTable.Name));
    Assert.Equal(["CustomerOrderBridge"], registry.Bridges.Select(bridge => bridge.Name));
    Assert.Equal(["OrderStatus"], registry.Pits.Select(pit => pit.Name));
    Assert.Equal(["StatusType"], registry.Satellites[1].DrivingKeyNames);
    Assert.True(registry.Pits[0].Satellites[0].IsMultiActive);
    Assert.Equal(["sqlite-v1", "postgres-v1"], registry.ProviderCapabilityProfiles.Select(profile => profile.ProfileName));

    Assert.True(registry.TryGetProviderCapabilityProfile("sqlite-v1", out var providerCapabilityProfile));
    Assert.Same(DataVaultProviderCapabilityProfiles.Sqlite, providerCapabilityProfile);
  }

  [Fact]
  public void RegistryCollectionsRemainImmutableAfterBuild() {
    var providerCapabilityProfiles = new List<DataVaultProviderCapabilityProfile>
    {
        DataVaultProviderCapabilityProfiles.Sqlite,
    };
    var builder = new DataVaultMetadataRegistryBuilder(CreateMinimalMetadataModel())
        .AddProviderCapabilityProfiles(providerCapabilityProfiles);

    var registry = builder.Build();

    providerCapabilityProfiles.Add(DataVaultProviderCapabilityProfiles.Postgres);
    builder.AddProviderCapabilityProfile(DataVaultProviderCapabilityProfiles.Oracle);

    Assert.Equal(["sqlite-v1"], registry.ProviderCapabilityProfiles.Select(profile => profile.ProfileName));
    var profilesCollection = Assert.IsAssignableFrom<ICollection<DataVaultProviderCapabilityProfile>>(
        registry.ProviderCapabilityProfiles);
    Assert.True(profilesCollection.IsReadOnly);
    Assert.Throws<NotSupportedException>(() => profilesCollection.Add(DataVaultProviderCapabilityProfiles.Postgres));
  }

  [Fact]
  public void RegistryUsesExactOrdinalNameLookup() {
    var registry = DataVaultMetadataRegistry.Create(CreateFullMetadataModel());

    Assert.True(registry.TryGetHub("Customer", out var hub));
    Assert.Equal("Customer", hub!.Name);
    Assert.False(registry.TryGetHub("customer", out _));

    Assert.True(registry.TryGetLink("CustomerOrder", out var link));
    Assert.Equal("CustomerOrder", link!.Name);
    Assert.False(registry.TryGetLink("customerorder", out _));

    Assert.True(registry.TryGetBridge("CustomerOrderBridge", out var bridge));
    Assert.Equal("CustomerOrderBridge", bridge!.Name);
    Assert.False(registry.TryGetBridge("customerorderbridge", out _));

    Assert.True(registry.TryGetPointInTimeTable("CustomerHistory", out var pointInTimeTable));
    Assert.Equal("CustomerHistory", pointInTimeTable!.Name);
    Assert.False(registry.TryGetPointInTimeTable("customerhistory", out _));

    Assert.True(registry.TryGetPit("OrderStatus", out var pit));
    Assert.Equal("OrderStatus", pit!.Name);
    Assert.False(registry.TryGetPit("orderstatus", out _));
  }

  [Fact]
  public void RegistrySupportsParentScopedSatelliteLookupWithoutGlobalNameUniqueness() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var customerStatus = new DataVaultSatelliteMetadata("Status", customer.ToReference(), ["Tier"]);
    var orderStatus = new DataVaultSatelliteMetadata("Status", order.ToReference(), ["State"]);
    var registry = DataVaultMetadataRegistry.Create(
        new DataVaultMetadataModel([customer, order], [], [customerStatus, orderStatus]));

    Assert.True(registry.TryGetSatellite(customer.ToReference(), "Status", out var resolvedCustomerStatus));
    Assert.True(registry.TryGetSatellite(order.ToReference(), "Status", out var resolvedOrderStatus));
    Assert.Same(customerStatus, resolvedCustomerStatus);
    Assert.Same(orderStatus, resolvedOrderStatus);
    Assert.Equal(["Status"], registry.GetSatellites(customer.ToReference()).Select(satellite => satellite.Name));
    Assert.Empty(registry.GetSatellites(DataVaultMetadataReference.Hub("Unknown")));
  }

  [Fact]
  public void RegistryExposesOptionalClrLookupWithoutInventingMappings() {
    var model = CreateFullMetadataModel();
    var metadataOnlyRegistry = DataVaultMetadataRegistry.Create(model);

    Assert.False(metadataOnlyRegistry.TryGetHub(typeof(CustomerEntity), out _));
    Assert.False(metadataOnlyRegistry.TryGetSatellite(DataVaultMetadataReference.Hub("Customer"), typeof(CustomerContactEntity), out _));

    var registry = DataVaultMetadataRegistry.Create(
        model,
        [],
        [
            DataVaultMetadataClrMapping.Hub<CustomerEntity>("Customer"),
            DataVaultMetadataClrMapping.Link<CustomerOrderEntity>("CustomerOrder"),
            DataVaultMetadataClrMapping.Satellite<CustomerContactEntity>(
                DataVaultMetadataReference.Hub("Customer"),
                "Contact"),
            DataVaultMetadataClrMapping.PointInTimeTable<CustomerHistoryEntity>("CustomerHistory"),
            DataVaultMetadataClrMapping.Bridge<CustomerOrderBridgeEntity>("CustomerOrderBridge"),
            DataVaultMetadataClrMapping.Pit<OrderStatusPitEntity>("OrderStatus"),
        ]);

    Assert.True(registry.TryGetHub(typeof(CustomerEntity), out var hub));
    Assert.Equal("Customer", hub!.Name);
    Assert.True(registry.TryGetLink(typeof(CustomerOrderEntity), out var link));
    Assert.Equal("CustomerOrder", link!.Name);
    Assert.True(registry.TryGetSatellite(DataVaultMetadataReference.Hub("Customer"), typeof(CustomerContactEntity), out var satellite));
    Assert.Equal("Contact", satellite!.Name);
    Assert.True(registry.TryGetPointInTimeTable(typeof(CustomerHistoryEntity), out var pointInTimeTable));
    Assert.Equal("CustomerHistory", pointInTimeTable!.Name);
    Assert.True(registry.TryGetBridge(typeof(CustomerOrderBridgeEntity), out var bridge));
    Assert.Equal("CustomerOrderBridge", bridge!.Name);
    Assert.True(registry.TryGetPit(typeof(OrderStatusPitEntity), out var pit));
    Assert.Equal("OrderStatus", pit!.Name);
  }

  [Fact]
  public void RegistryRejectsDuplicateLogicalNamesInLookupDomains() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var duplicateCustomer = new DataVaultHubMetadata("Customer", ["CustomerNumber"]);

    AssertRegistryValidationFailure(
        () => DataVaultMetadataRegistry.Create(new DataVaultMetadataModel([customer, duplicateCustomer], [], [])),
        "Duplicate hub metadata logical name 'Customer'.");

    AssertRegistryValidationFailure(
        () => DataVaultMetadataRegistry.Create(
            new DataVaultMetadataModel(
                [customer],
                [],
                [
                    new DataVaultSatelliteMetadata("Contact", customer.ToReference(), ["EmailAddress"]),
                    new DataVaultSatelliteMetadata("Contact", customer.ToReference(), ["PhoneNumber"]),
                ])),
        "Duplicate satellite metadata logical name 'Contact' under hub 'Customer'.");

    var exception = Assert.Throws<ArgumentException>(() => DataVaultMetadataRegistry.Create(
        new DataVaultMetadataModel([customer], [], []),
        [DataVaultProviderCapabilityProfiles.Sqlite, DataVaultProviderCapabilityProfiles.Sqlite]));

    Assert.Equal("providerCapabilityProfiles", exception.ParamName);
    Assert.Contains("Duplicate provider capability profile logical name 'sqlite-v1'.", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void RegistryRejectsAmbiguousClrMappings() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);

    var exception = Assert.Throws<ArgumentException>(() => DataVaultMetadataRegistry.Create(
        new DataVaultMetadataModel([customer, order], [], []),
        [],
        [
            DataVaultMetadataClrMapping.Hub<CustomerEntity>("Customer"),
            DataVaultMetadataClrMapping.Hub<CustomerEntity>("Order"),
        ]));

    Assert.Equal("clrMappings", exception.ParamName);
    Assert.Contains("CLR type '" + typeof(CustomerEntity).FullName, exception.Message, StringComparison.Ordinal);
    Assert.Contains("hub metadata declaration", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void RegistryRejectsClrMappingTargetsThatAreNotPresent() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);

    var exception = Assert.Throws<ArgumentException>(() => DataVaultMetadataRegistry.Create(
        new DataVaultMetadataModel([customer], [], []),
        [],
        [DataVaultMetadataClrMapping.Hub<OrderEntity>("Order")]));

    Assert.Equal("clrMappings", exception.ParamName);
    Assert.Contains("hub metadata 'Order'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("not present", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void RegistryRejectsMissingReferencedMetadataDependencies() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var missingHubLink = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), DataVaultMetadataReference.Hub("Order")]);

    AssertRegistryValidationFailure(
        () => DataVaultMetadataRegistry.Create(new DataVaultMetadataModel([customer], [missingHubLink], [])),
        "link metadata 'CustomerOrder' references missing hub metadata 'Order'.");

    AssertRegistryValidationFailure(
        () => DataVaultMetadataRegistry.Create(
            new DataVaultMetadataModel(
                [customer],
                [],
                [new DataVaultSatelliteMetadata("OrderStatus", DataVaultMetadataReference.Link("CustomerOrder"), ["StatusCode"])])),
        "satellite metadata 'OrderStatus' references missing link metadata 'CustomerOrder'.");

    AssertRegistryValidationFailure(
        () => DataVaultMetadataRegistry.Create(
            new DataVaultMetadataModel(
                [customer],
                [],
                [],
                [],
                [],
                [new DataVaultPitMetadata(customer.ToReference(), ["Contact"])])),
        "PIT metadata 'CustomerContact' references missing satellite metadata 'Contact'.");
  }

  private static DataVaultMetadataModel CreateMinimalMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        []);
  }

  private static DataVaultMetadataModel CreateFullMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var contact = new DataVaultSatelliteMetadata("Contact", customer.ToReference(), ["EmailAddress"]);
    var status = new DataVaultSatelliteMetadata("Status", order.ToReference(), ["State"], ["StatusType"]);
    var pointInTimeTable = new DataVaultPointInTimeMetadata(
        "CustomerHistory",
        customer.ToReference(),
        [DataVaultMetadataReference.Satellite("Contact")]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrderBridge",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference(),
        sourceParticipantOrdinal: 0,
        targetParticipantOrdinal: 1);
    var pit = new DataVaultPitMetadata(
        order.ToReference(),
        [new DataVaultPitSatelliteReferenceMetadata("Status", isMultiActive: true)]);

    return new DataVaultMetadataModel(
        [customer, order],
        [customerOrder],
        [contact, status],
        [pointInTimeTable],
        [bridge],
        [pit]);
  }

  private static void AssertRegistryValidationFailure(Action action, string expectedMessage) {
    var exception = Assert.Throws<ArgumentException>(action);

    Assert.Equal("metadataModel", exception.ParamName);
    Assert.Contains(expectedMessage, exception.Message, StringComparison.Ordinal);
  }

  private sealed class CustomerEntity {
  }

  private sealed class OrderEntity {
  }

  private sealed class CustomerOrderEntity {
  }

  private sealed class CustomerContactEntity {
  }

  private sealed class CustomerHistoryEntity {
  }

  private sealed class CustomerOrderBridgeEntity {
  }

  private sealed class OrderStatusPitEntity {
  }
}
