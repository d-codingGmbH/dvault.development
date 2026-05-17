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
  public void CompileTimeMappingDeclarationAttributesExposeExactNamesAndOrder() {
    var hub = new DataVaultHubMappingAttribute("Customer");
    var businessKey = new DataVaultBusinessKeyBindingAttribute(1, "Region Code", nameof(CustomerSource.RegionCode));
    var link = new DataVaultLinkMappingAttribute("CustomerOrder");
    var participant = new DataVaultLinkParticipantBindingAttribute(0, "Customer", nameof(CustomerOrderSource.CustomerHashKey));
    var satellite = new DataVaultHubSatelliteMappingAttribute("Customer", "Profile");
    var parentHashKey = new DataVaultSatelliteParentHashKeyBindingAttribute(nameof(CustomerProfileSource.CustomerHashKey));
    var drivingKey = new DataVaultSatelliteDrivingKeyBindingAttribute(0, "Contact Type", nameof(CustomerContactSource.ContactType));
    var payload = new DataVaultSatellitePayloadBindingAttribute(0, "customer_name", nameof(CustomerProfileSource.CustomerName));
    var hashDiff = new DataVaultSatelliteHashDiffBindingAttribute(nameof(CustomerProfileSource.HashDiff));

    Assert.Equal("Customer", hub.HubName);
    Assert.Equal(1, businessKey.Order);
    Assert.Equal("Region Code", businessKey.BusinessKeyName);
    Assert.Equal(nameof(CustomerSource.RegionCode), businessKey.SourceMemberName);
    Assert.Equal("CustomerOrder", link.LinkName);
    Assert.Equal(0, participant.Order);
    Assert.Equal("Customer", participant.ParticipantHubName);
    Assert.Equal(nameof(CustomerOrderSource.CustomerHashKey), participant.SourceMemberName);
    Assert.Equal("Customer", satellite.ParentHubName);
    Assert.Equal("Profile", satellite.SatelliteName);
    Assert.Equal(nameof(CustomerProfileSource.CustomerHashKey), parentHashKey.SourceMemberName);
    Assert.Equal("Contact Type", drivingKey.DrivingKeyName);
    Assert.Equal(nameof(CustomerContactSource.ContactType), drivingKey.SourceMemberName);
    Assert.Equal("customer_name", payload.PayloadName);
    Assert.Equal(nameof(CustomerProfileSource.CustomerName), payload.SourceMemberName);
    Assert.Equal(nameof(CustomerProfileSource.HashDiff), hashDiff.SourceMemberName);
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

  [Fact]
  public void TypedSaveHelpersAssembleHubLinkAndOrdinarySatelliteRegistryRequests() {
    var loadTimestamp = new DateTimeOffset(2026, 5, 10, 10, 0, 0, TimeSpan.Zero);

    var hubRequest = DataVaultSaveServiceTypedExtensions.CreateHubRegistrySaveRequest(
        new CustomerSource("C-100", "DE"),
        new CustomerHubMapper(),
        loadTimestamp,
        "typed-import");

    Assert.Equal(loadTimestamp, hubRequest.LoadTimestamp);
    Assert.Equal("typed-import", hubRequest.RecordSource);
    var hubOperation = Assert.Single(hubRequest.HubOperations);
    Assert.Equal("Customer", hubOperation.HubName);
    Assert.Equal("C-100", hubOperation.BusinessKeyValues["Customer Id"]);
    Assert.Equal("DE", hubOperation.BusinessKeyValues["Region Code"]);
    Assert.Empty(hubRequest.LinkOperations);
    Assert.Empty(hubRequest.SatelliteOperations);

    var linkRequest = DataVaultSaveServiceTypedExtensions.CreateLinkRegistrySaveRequest(
        new CustomerOrderSource("customer-hash", "order-hash"),
        new CustomerOrderLinkMapper(),
        loadTimestamp.AddMinutes(1),
        "typed-import");

    var linkOperation = Assert.Single(linkRequest.LinkOperations);
    Assert.Equal("CustomerOrder", linkOperation.LinkName);
    Assert.Equal("customer-hash", linkOperation.ParticipantHashKeyValues["Customer"]);
    Assert.Equal("order-hash", linkOperation.ParticipantHashKeyValues["Order"]);
    Assert.Empty(linkRequest.HubOperations);
    Assert.Empty(linkRequest.SatelliteOperations);

    var satelliteRequest = DataVaultSaveServiceTypedExtensions.CreateOrdinaryHubSatelliteRegistrySaveRequest(
        new CustomerProfileSource("customer-hash", "Alice Adams", "active", "profile-hash"),
        new CustomerProfileSatelliteMapper(),
        loadTimestamp.AddMinutes(2),
        "typed-import");

    var satelliteOperation = Assert.Single(satelliteRequest.SatelliteOperations);
    Assert.Equal("Profile", satelliteOperation.SatelliteName);
    Assert.Equal(DataVaultMetadataReferenceKind.Hub, satelliteOperation.Parent.Kind);
    Assert.Equal("Customer", satelliteOperation.Parent.Name);
    Assert.Equal("customer-hash", satelliteOperation.ParentHashKey);
    Assert.Equal("Alice Adams", satelliteOperation.PayloadValues["customer_name"]);
    Assert.Equal("active", satelliteOperation.PayloadValues["customer_status"]);
    Assert.Equal("profile-hash", satelliteOperation.HashDiff);
    Assert.Empty(satelliteOperation.DrivingKeyValues);
    Assert.Empty(satelliteRequest.HubOperations);
    Assert.Empty(satelliteRequest.LinkOperations);
  }

  [Fact]
  public void TypedBulkSaveHelpersPreserveCallerOrder() {
    var loadTimestamp = new DateTimeOffset(2026, 5, 10, 10, 0, 0, TimeSpan.Zero);

    var bulkRequest = DataVaultSaveServiceTypedExtensions.CreateHubRegistryBulkSaveRequest(
        [
            new CustomerSource("C-100", "DE"),
            new CustomerSource("C-200", "US"),
            new CustomerSource("C-300", "FR"),
        ],
        new CustomerHubMapper(),
        loadTimestamp,
        "bulk-import");

    Assert.Equal(
        ["C-100", "C-200", "C-300"],
        bulkRequest.Requests
            .Select(request => Assert.Single(request.HubOperations).BusinessKeyValues["Customer Id"])
            .ToArray());
    Assert.All(
        bulkRequest.Requests,
        request => {
          Assert.Equal(loadTimestamp, request.LoadTimestamp);
          Assert.Equal("bulk-import", request.RecordSource);
          Assert.Empty(request.LinkOperations);
          Assert.Empty(request.SatelliteOperations);
        });
  }

  [Fact]
  public void TypedSaveHelperDiagnosticsWrapMapperFailuresWithStableSourceContext() {
    var loadTimestamp = new DateTimeOffset(2026, 5, 10, 10, 0, 0, TimeSpan.Zero);

    var exception = Assert.Throws<InvalidOperationException>(() => DataVaultSaveServiceTypedExtensions.CreateHubRegistryBulkSaveRequest(
        [
            new CustomerSource("C-100", "DE"),
            new CustomerSource("C-200", "US"),
        ],
        new FailingSecondCustomerHubMapper(),
        loadTimestamp,
        "bulk-import"));

    Assert.Contains("hub", exception.Message, StringComparison.Ordinal);
    Assert.Contains(typeof(CustomerSource).FullName!, exception.Message, StringComparison.Ordinal);
    Assert.Contains("batch index 1", exception.Message, StringComparison.Ordinal);
    Assert.Contains("mapped failure reason", exception.Message, StringComparison.Ordinal);
    Assert.IsType<ArgumentException>(exception.InnerException);

    var linkException = Assert.Throws<InvalidOperationException>(() => DataVaultSaveServiceTypedExtensions.CreateLinkRegistrySaveRequest(
        new EmployeeReportsToSource("manager-hash", "employee-hash"),
        new EmployeeReportsToLinkMapper(),
        loadTimestamp,
        "typed-import"));

    Assert.Contains("link", linkException.Message, StringComparison.Ordinal);
    Assert.Contains(typeof(EmployeeReportsToSource).FullName!, linkException.Message, StringComparison.Ordinal);
    Assert.Contains("duplicate names", linkException.Message, StringComparison.Ordinal);
    Assert.IsType<ArgumentException>(linkException.InnerException);
  }

  [Fact]
  public void HubSatelliteHelpersRejectOutOfScopeSatelliteShapesWithDiagnostics() {
    var loadTimestamp = new DateTimeOffset(2026, 5, 10, 10, 0, 0, TimeSpan.Zero);

    var linkParentException = Assert.Throws<InvalidOperationException>(() => DataVaultSaveServiceTypedExtensions.CreateOrdinaryHubSatelliteRegistrySaveRequest(
        new CustomerOrderStateSource("customer-order-hash", "submitted", "state-hash"),
        new CustomerOrderStateSatelliteMapper(),
        loadTimestamp,
        "typed-import"));

    Assert.Contains("satellite 'CustomerOrder.State'", linkParentException.Message, StringComparison.Ordinal);
    Assert.Contains("hub-parent", linkParentException.Message, StringComparison.Ordinal);
    Assert.Contains("link parent 'CustomerOrder'", linkParentException.Message, StringComparison.Ordinal);
    Assert.IsType<ArgumentException>(linkParentException.InnerException);

    var multiActiveException = Assert.Throws<InvalidOperationException>(() => DataVaultSaveServiceTypedExtensions.CreateOrdinaryHubSatelliteRegistrySaveRequest(
        new CustomerContactSource(
            "customer-hash",
            "billing",
            "DE",
            "billing@example.test",
            "contact-hash"),
        new CustomerContactSatelliteMapper(),
        loadTimestamp,
        "typed-import"));

    Assert.Contains("satellite 'Customer.ContactChannel'", multiActiveException.Message, StringComparison.Ordinal);
    Assert.Contains("hub-parent", multiActiveException.Message, StringComparison.Ordinal);
    Assert.Contains("driving-key values", multiActiveException.Message, StringComparison.Ordinal);
    Assert.IsType<ArgumentException>(multiActiveException.InnerException);
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

  private sealed class FailingSecondCustomerHubMapper : IDataVaultHubMapper<CustomerSource> {
    public DataVaultRegistryHubSaveOperation Map(CustomerSource source) {
      ArgumentNullException.ThrowIfNull(source);

      if (source.CustomerId == "C-200") {
        throw new ArgumentException("mapped failure reason", nameof(source));
      }

      return new CustomerHubMapper().Map(source);
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
