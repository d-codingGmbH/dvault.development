using System.Text.Json;
using DCoding.Data.DVault.Modeling;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultModelArtifactExporterTests {
  [Fact]
  public void ExportJsonExportsRegistryDeterministicallyAndRoundTripsThroughImporter() {
    var registry = DataVaultMetadataRegistry.Create(
        CreateAdvancedMetadataModel(),
        BuiltInProfiles(DataVaultLoadTimestampStorage.UtcTicks));

    var json = DataVaultModelArtifactExporter.ExportJson(registry);
    var repeatedJson = DataVaultModelArtifactExporter.ExportJson(registry);

    Assert.Equal(repeatedJson, json);
    Assert.Equal(
        ["schemaVersion", "naming", "loadTimestampStorage", "hubs", "links", "satellites", "pits", "bridges"],
        TopLevelPropertyNames(json));
    Assert.Contains("\n  \"schemaVersion\": \"dvault.model.v1\"", json, StringComparison.Ordinal);
    Assert.DoesNotContain("pointInTimeTables", json, StringComparison.Ordinal);

    var importResult = DataVaultModelArtifactImporter.ImportJson(json);

    AssertValid(importResult);
    Assert.Equal(DataVaultLoadTimestampStorage.UtcTicks, importResult.LoadTimestampStorage);
    Assert.Equal(["Customer", "Order", "SalesRegion"], importResult.MetadataModel!.Hubs.Select(hub => hub.Name));
    Assert.Equal(
        ["CustomerOrder", "SalesRegionParentChild"],
        importResult.MetadataModel.Links.Select(link => link.Name));
    Assert.Equal(["CustomerPit"], importResult.MetadataModel.Pits.Select(pit => pit.Name));
    Assert.Equal(
        ["CustomerOrderBridge", "SalesRegionHierarchyBridge"],
        importResult.MetadataModel.Bridges.Select(bridge => bridge.Name));
  }

  [Fact]
  public void ExportJsonFromMetadataModelUsesProviderDefaultAndSameCanonicalSectionsAsRegistry() {
    var metadataModel = CreateSharedCodeFirstProducedMetadataModel();
    var modelJson = DataVaultModelArtifactExporter.ExportJson(metadataModel);
    var registryJson = DataVaultModelArtifactExporter.ExportJson(DataVaultMetadataRegistry.Create(metadataModel));

    Assert.Equal(registryJson, modelJson);
    Assert.Contains("\"loadTimestampStorage\": \"provider-default\"", modelJson, StringComparison.Ordinal);

    var importResult = DataVaultModelArtifactImporter.ImportJson(modelJson);

    AssertValid(importResult);
    Assert.Equal(["Customer", "Order"], importResult.MetadataModel!.Hubs.Select(hub => hub.Name));
    Assert.Equal(["CustomerOrder"], importResult.MetadataModel.Links.Select(link => link.Name));
    Assert.Equal(["Contact"], importResult.MetadataModel.Satellites.Select(satellite => satellite.Name));
    Assert.Equal(["ContactType"], importResult.MetadataModel.Satellites.Single().DrivingKeyNames);
  }

  [Fact]
  public void ExportJsonRejectsLegacyPointInTimeTablesDeterministically() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var profile = new DataVaultSatelliteMetadata(
        "CustomerProfile",
        customer.ToReference(),
        ["Name"]);
    var legacyPointInTimeTable = new DataVaultPointInTimeMetadata(
        "CustomerPointInTime",
        customer.ToReference(),
        [DataVaultMetadataReference.Satellite("CustomerProfile")]);
    var metadataModel = new DataVaultMetadataModel(
        [customer],
        [],
        [profile],
        [legacyPointInTimeTable]);
    var registry = DataVaultMetadataRegistry.Create(metadataModel);

    var modelException = Assert.Throws<NotSupportedException>(() =>
        DataVaultModelArtifactExporter.ExportJson(metadataModel));
    var registryException = Assert.Throws<NotSupportedException>(() =>
        DataVaultModelArtifactExporter.ExportJson(registry));

    AssertLegacyPointInTimeTablesMessage(modelException);
    AssertLegacyPointInTimeTablesMessage(registryException);
  }

  [Fact]
  public void ExportJsonRejectsAmbiguousRepeatedHubParticipantsWithoutRoles() {
    var employee = new DataVaultHubMetadata("Employee", ["EmployeeId"]);
    var hierarchyLink = new DataVaultLinkMetadata(
        "EmployeeHierarchy",
        [employee.ToReference(), employee.ToReference()]);
    var metadataModel = new DataVaultMetadataModel(
        [employee],
        [hierarchyLink],
        []);

    var exception = Assert.Throws<NotSupportedException>(() =>
        DataVaultModelArtifactExporter.ExportJson(metadataModel));

    Assert.Contains("EmployeeHierarchy", exception.Message, StringComparison.Ordinal);
    Assert.Contains("repeats hub 'Employee'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("dvault.model.v1", exception.Message, StringComparison.Ordinal);
  }

  private static DataVaultMetadataModel CreateSharedCodeFirstProducedMetadataModel() {
    var builder = new DataVaultCodeFirstModelBuilder();
    builder.Hub<Customer>(hub => {
      hub.BusinessKey(customer => customer.CustomerId);
      hub.Satellite("Contact", satellite => {
        satellite.DrivingKey(customer => customer.ContactType);
        satellite.Payload(customer => customer.ContactValue);
      });
    });
    builder.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));
    builder.Link("CustomerOrder", link => {
      link.Participant<Customer>();
      link.Participant<Order>();
    });

    return builder.BuildMetadataModel();
  }

  private static DataVaultMetadataModel CreateAdvancedMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId", "RegionCode"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var salesRegion = new DataVaultHubMetadata("SalesRegion", ["RegionCode"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var salesRegionHierarchy = new DataVaultLinkMetadata(
        "SalesRegionParentChild",
        [
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ParentRegion"),
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ChildRegion"),
        ]);

    return new DataVaultMetadataModel(
        [customer, order, salesRegion],
        [customerOrder, salesRegionHierarchy],
        [
            new DataVaultSatelliteMetadata(
                "CustomerProfile",
                customer.ToReference(),
                ["Name", "EmailAddress"]),
            new DataVaultSatelliteMetadata(
                "CustomerContactByType",
                customer.ToReference(),
                ["ContactValue", "VerifiedAt"],
                ["ContactType"]),
        ],
        [],
        [
            DataVaultBridgeMetadata.ManyToMany(
                "CustomerOrderBridge",
                customer.ToReference(),
                customerOrder.ToReference(),
                order.ToReference()),
            new DataVaultBridgeMetadata(
                "SalesRegionHierarchyBridge",
                DataVaultBridgeKind.Hierarchy,
                salesRegionHierarchy.ToReference(),
                [
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.Ancestor,
                        salesRegion.ToReference(),
                        "ParentRegion"),
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.Descendant,
                        salesRegion.ToReference(),
                        "ChildRegion"),
                ]),
        ],
        [
            new DataVaultPitMetadata(
                "CustomerPit",
                customer.ToReference(),
                ["CustomerProfile", "CustomerContactByType"]),
        ]);
  }

  private static DataVaultProviderCapabilityProfile[] BuiltInProfiles(
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    return
    [
        DataVaultProviderCapabilityProfiles.Sqlite.WithLoadTimestampStorage(loadTimestampStorage),
        DataVaultProviderCapabilityProfiles.Oracle.WithLoadTimestampStorage(loadTimestampStorage),
        DataVaultProviderCapabilityProfiles.Postgres.WithLoadTimestampStorage(loadTimestampStorage),
        DataVaultProviderCapabilityProfiles.SqlServer.WithLoadTimestampStorage(loadTimestampStorage),
        DataVaultProviderCapabilityProfiles.MySql.WithLoadTimestampStorage(loadTimestampStorage),
    ];
  }

  private static string[] TopLevelPropertyNames(string json) {
    using var document = JsonDocument.Parse(json);

    return document.RootElement
        .EnumerateObject()
        .Select(property => property.Name)
        .ToArray();
  }

  private static void AssertLegacyPointInTimeTablesMessage(NotSupportedException exception) {
    Assert.Contains("PointInTimeTables", exception.Message, StringComparison.Ordinal);
    Assert.Contains("dvault.model.v1", exception.Message, StringComparison.Ordinal);
    Assert.Contains("pointInTimeTables", exception.Message, StringComparison.Ordinal);
    Assert.Contains("CustomerPointInTime", exception.Message, StringComparison.Ordinal);
  }

  private static void AssertValid(DataVaultModelImportResult result) {
    Assert.True(result.IsValid, DataVaultModelImportResult.FormatDiagnostics(result.Diagnostics));
    Assert.Empty(result.Diagnostics);
    Assert.NotNull(result.MetadataModel);
    Assert.NotNull(result.MetadataRegistry);
  }

  private sealed class Customer {
    public string CustomerId { get; set; } = string.Empty;

    public string RegionCode { get; set; } = string.Empty;

    public string ContactType { get; set; } = string.Empty;

    public string ContactValue { get; set; } = string.Empty;
  }

  private sealed class Order {
    public string OrderId { get; set; } = string.Empty;
  }
}
