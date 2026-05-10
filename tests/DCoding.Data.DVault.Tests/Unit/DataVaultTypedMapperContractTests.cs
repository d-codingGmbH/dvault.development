using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultTypedMapperContractTests {
  [Fact]
  public void HubMapperMapsOneSourceToRegistryHubOperationByExactNames() {
    var mapper = new CustomerHubMapper();
    var source = new CustomerSource("C-100", "DE");

    var operation = mapper.Map(source);

    Assert.Equal("Customer", operation.HubName);
    Assert.Equal("C-100", operation.BusinessKeyValues["Customer Id"]);
    Assert.Equal("DE", operation.BusinessKeyValues["Region Code"]);
  }

  [Fact]
  public void LinkMapperMapsOneSourceToRegistryLinkOperationByUniqueParticipantNames() {
    var mapper = new CustomerOrderLinkMapper();
    var source = new CustomerOrderSource("customer-hash", "order-hash");

    var operation = mapper.Map(source);

    Assert.Equal("CustomerOrder", operation.LinkName);
    Assert.Equal("customer-hash", operation.ParticipantHashKeyValues["Customer"]);
    Assert.Equal("order-hash", operation.ParticipantHashKeyValues["Order"]);
  }

  [Fact]
  public void LinkMapperV1RejectsRepeatedSameHubParticipantNamesThroughOperationContract() {
    var mapper = new EmployeeReportsToLinkMapper();
    var source = new EmployeeReportsToSource("manager-hash", "employee-hash");

    var exception = Assert.Throws<ArgumentException>(() => mapper.Map(source));

    Assert.Contains("duplicate names", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void SatelliteMappersMapOrdinaryMultiActiveAndLinkParentTargetsByExactNames() {
    var ordinaryMapper = new CustomerProfileSatelliteMapper();
    var multiActiveMapper = new CustomerContactSatelliteMapper();
    var linkParentMapper = new CustomerOrderStateSatelliteMapper();

    var ordinaryOperation = ordinaryMapper.Map(new CustomerProfileSource(
        "customer-hash",
        "Alice Adams",
        "active",
        "profile-hash"));
    var multiActiveOperation = multiActiveMapper.Map(new CustomerContactSource(
        "customer-hash",
        "billing",
        "DE",
        "billing@example.test",
        "contact-hash"));
    var linkParentOperation = linkParentMapper.Map(new CustomerOrderStateSource(
        "customer-order-hash",
        "submitted",
        "state-hash"));

    Assert.Equal(DataVaultMetadataReferenceKind.Hub, ordinaryOperation.Parent.Kind);
    Assert.Equal("Customer", ordinaryOperation.Parent.Name);
    Assert.Equal("Profile", ordinaryOperation.SatelliteName);
    Assert.Equal("Alice Adams", ordinaryOperation.PayloadValues["customer_name"]);
    Assert.Empty(ordinaryOperation.DrivingKeyValues);

    Assert.Equal("ContactChannel", multiActiveOperation.SatelliteName);
    Assert.Equal("billing", multiActiveOperation.DrivingKeyValues["Contact Type"]);
    Assert.Equal("DE", multiActiveOperation.DrivingKeyValues["Region Code"]);
    Assert.Equal("billing@example.test", multiActiveOperation.PayloadValues["Email Address"]);

    Assert.Equal(DataVaultMetadataReferenceKind.Link, linkParentOperation.Parent.Kind);
    Assert.Equal("CustomerOrder", linkParentOperation.Parent.Name);
    Assert.Equal("State", linkParentOperation.SatelliteName);
    Assert.Equal("submitted", linkParentOperation.PayloadValues["State Code"]);
  }

  [Fact]
  public void MapperImplementationsRejectNullSourcesImmediately() {
    Assert.Throws<ArgumentNullException>(() => new CustomerHubMapper().Map(null!));
    Assert.Throws<ArgumentNullException>(() => new CustomerOrderLinkMapper().Map(null!));
    Assert.Throws<ArgumentNullException>(() => new CustomerProfileSatelliteMapper().Map(null!));
  }

  [Fact]
  public void RegistryMapperOutputsRejectNullValuesAndDuplicateNamesAtConstruction() {
    Assert.Throws<ArgumentException>(() => new DataVaultRegistryHubSaveOperation(
        "Customer",
        [new("Customer Id", "C-100"), new("Customer Id", "C-101")]));
    Assert.Throws<ArgumentException>(() => new DataVaultRegistryHubSaveOperation(
        "Customer",
        [new("Customer Id", null!)]));
    Assert.Throws<ArgumentException>(() => new DataVaultRegistrySatelliteSaveOperation(
        DataVaultMetadataReference.Hub("Customer"),
        "Profile",
        "customer-hash",
        [new("customer_name", "Alice"), new("customer_name", "Alicia")],
        "profile-hash"));
  }

  [Fact]
  public void SatelliteSupportingOperationValidatesMultiActiveDrivingKeySetsExactly() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "ContactChannel",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type", "Region Code"]);

    Assert.Throws<ArgumentException>(() => new DataVaultSatelliteSaveOperation(
        contact,
        "customer-hash",
        [new("Contact Type", "billing")],
        [new("Email Address", "billing@example.test")],
        "contact-hash"));
    Assert.Throws<ArgumentException>(() => new DataVaultSatelliteSaveOperation(
        contact,
        "customer-hash",
        [new("Contact Type", "billing"), new("Region Code", "DE"), new("Scope", "extra")],
        [new("Email Address", "billing@example.test")],
        "contact-hash"));
  }

  [Fact]
  public async Task MissingRequiredMappedNamesSurfaceAtSavePlanCreationBoundary() {
    await AssertMissingRequiredValueAtSaveBoundaryAsync(
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 10, 10, 0, 0, TimeSpan.Zero),
            "crm-import",
            [
                new DataVaultHubSaveOperation(
                    new DataVaultHubMetadata("Customer", ["Customer Id", "Region Code"]),
                    [new("Customer Id", "C-100")]),
            ],
            []),
        "Region Code");

    await AssertMissingRequiredValueAtSaveBoundaryAsync(
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 10, 10, 0, 0, TimeSpan.Zero),
            "crm-import",
            [],
            [
                new DataVaultLinkSaveOperation(
                    new DataVaultLinkMetadata(
                        "CustomerOrder",
                        [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
                    [new("Customer", "customer-hash")]),
            ]),
        "Order");

    await AssertMissingRequiredValueAtSaveBoundaryAsync(
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 10, 10, 0, 0, TimeSpan.Zero),
            "crm-import",
            [],
            [],
            [
                new DataVaultSatelliteSaveOperation(
                    new DataVaultSatelliteMetadata(
                        "Profile",
                        DataVaultMetadataReference.Hub("Customer"),
                        ["customer_name", "customer_status"]),
                    "customer-hash",
                    [new("customer_name", "Alice Adams")],
                    "profile-hash"),
            ]),
        "customer_status");
  }

  private static async Task AssertMissingRequiredValueAtSaveBoundaryAsync(
      DataVaultSaveRequest request,
      string expectedMissingName) {
    var saveService = new DefaultDataVaultSaveService(
        DefaultStableHashService.Instance,
        DefaultStableHashNormalizer.Instance);
    await using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);

    var exception = await Assert.ThrowsAsync<ArgumentException>(() => saveService.SaveAsync(dbContext, request));

    Assert.Contains("missing required value '" + expectedMissingName + "'", exception.Message, StringComparison.Ordinal);
  }

  private sealed class CustomerHubMapper : IDataVaultHubMapper<CustomerSource> {
    public DataVaultRegistryHubSaveOperation Map(CustomerSource source) {
      ArgumentNullException.ThrowIfNull(source);

      return new DataVaultRegistryHubSaveOperation(
          "Customer",
          [
              new("Customer Id", source.CustomerId),
              new("Region Code", source.RegionCode),
          ]);
    }
  }

  private sealed class CustomerOrderLinkMapper : IDataVaultLinkMapper<CustomerOrderSource> {
    public DataVaultRegistryLinkSaveOperation Map(CustomerOrderSource source) {
      ArgumentNullException.ThrowIfNull(source);

      return new DataVaultRegistryLinkSaveOperation(
          "CustomerOrder",
          [
              new("Customer", source.CustomerHashKey),
              new("Order", source.OrderHashKey),
          ]);
    }
  }

  private sealed class EmployeeReportsToLinkMapper : IDataVaultLinkMapper<EmployeeReportsToSource> {
    public DataVaultRegistryLinkSaveOperation Map(EmployeeReportsToSource source) {
      ArgumentNullException.ThrowIfNull(source);

      return new DataVaultRegistryLinkSaveOperation(
          "EmployeeReportsTo",
          [
              new("Employee", source.ManagerHashKey),
              new("Employee", source.EmployeeHashKey),
          ]);
    }
  }

  private sealed class CustomerProfileSatelliteMapper : IDataVaultSatelliteMapper<CustomerProfileSource> {
    public DataVaultRegistrySatelliteSaveOperation Map(CustomerProfileSource source) {
      ArgumentNullException.ThrowIfNull(source);

      return new DataVaultRegistrySatelliteSaveOperation(
          DataVaultMetadataReference.Hub("Customer"),
          "Profile",
          source.CustomerHashKey,
          [
              new("customer_name", source.CustomerName),
              new("customer_status", source.CustomerStatus),
          ],
          source.HashDiff);
    }
  }

  private sealed class CustomerContactSatelliteMapper : IDataVaultSatelliteMapper<CustomerContactSource> {
    public DataVaultRegistrySatelliteSaveOperation Map(CustomerContactSource source) {
      ArgumentNullException.ThrowIfNull(source);

      return new DataVaultRegistrySatelliteSaveOperation(
          DataVaultMetadataReference.Hub("Customer"),
          "ContactChannel",
          source.CustomerHashKey,
          [
              new("Contact Type", source.ContactType),
              new("Region Code", source.RegionCode),
          ],
          [new("Email Address", source.EmailAddress)],
          source.HashDiff);
    }
  }

  private sealed class CustomerOrderStateSatelliteMapper : IDataVaultSatelliteMapper<CustomerOrderStateSource> {
    public DataVaultRegistrySatelliteSaveOperation Map(CustomerOrderStateSource source) {
      ArgumentNullException.ThrowIfNull(source);

      return new DataVaultRegistrySatelliteSaveOperation(
          DataVaultMetadataReference.Link("CustomerOrder"),
          "State",
          source.CustomerOrderHashKey,
          [new("State Code", source.StateCode)],
          source.HashDiff);
    }
  }

  private sealed record CustomerSource(string CustomerId, string RegionCode);

  private sealed record CustomerOrderSource(string CustomerHashKey, string OrderHashKey);

  private sealed record EmployeeReportsToSource(string ManagerHashKey, string EmployeeHashKey);

  private sealed record CustomerProfileSource(
      string CustomerHashKey,
      string CustomerName,
      string CustomerStatus,
      string HashDiff);

  private sealed record CustomerContactSource(
      string CustomerHashKey,
      string ContactType,
      string RegionCode,
      string EmailAddress,
      string HashDiff);

  private sealed record CustomerOrderStateSource(string CustomerOrderHashKey, string StateCode, string HashDiff);
}
