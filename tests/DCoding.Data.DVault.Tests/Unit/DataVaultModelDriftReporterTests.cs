using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultModelDriftReporterTests {
  [Fact]
  public void CompareMatchingMetadataReturnsNoDifferencesForRepresentativeModel() {
    var metadataModel = CreateRepresentativeMetadataModel();
    var currentModel = CreateModel(metadataModel).Model;

    var report = DataVaultModelDriftReporter.Compare(metadataModel, currentModel);

    Assert.False(report.HasBlockingDifferences);
    Assert.Empty(report.Differences);
    Assert.Equal("DVault model drift: no differences.", report.ToDisplayString());
  }

  [Fact]
  public void CompareClassifiesProducedNameAndMetadataSourceDriftAsInformational() {
    var metadataModel = CreateCustomerContactMetadataModel();
    var modelBuilder = CreateModel(metadataModel);
    var hub = FindEntity(modelBuilder.Model, "HubCustomer");
    hub.SetAnnotation(DataVaultAnnotationNames.ProducedName, "HubCustomerArchive");
    modelBuilder.Model.SetAnnotation(DataVaultAnnotationNames.MetadataSourceFingerprint, "alternate-source");

    var report = DataVaultModelDriftReporter.Compare(metadataModel, modelBuilder.Model);

    Assert.False(report.HasBlockingDifferences);
    Assert.All(report.Differences, difference => Assert.Equal(DataVaultModelDriftSeverity.Informational, difference.Severity));
    Assert.Contains(report.Differences, difference => difference.Code == "entity-produced-name-mismatch");
    Assert.Contains(report.Differences, difference => difference.Code == "metadata-source-fingerprint-mismatch");
  }

  [Fact]
  public void CompareKeepsProducedColumnNameDriftInformationalForKeyAndIndexMembers() {
    var metadataModel = CreateCustomerContactMetadataModel();
    var modelBuilder = CreateModel(metadataModel);
    var satellite = FindEntity(modelBuilder.Model, "SatCustomerContact");
    satellite.FindProperty("CustomerHashKey")!.SetAnnotation(DataVaultAnnotationNames.ProducedName, "CustomerHashKeyArchive");

    var report = DataVaultModelDriftReporter.Compare(metadataModel, modelBuilder.Model);

    Assert.False(report.HasBlockingDifferences);
    Assert.All(report.Differences, difference => Assert.Equal(DataVaultModelDriftSeverity.Informational, difference.Severity));
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "property-produced-name-mismatch" &&
            difference.LogicalName == "Satellite:Contact.Customer" &&
            difference.ExpectedValue == "CustomerHashKey" &&
            difference.ActualValue == "CustomerHashKeyArchive");
    Assert.DoesNotContain(report.Differences, difference => difference.Code == "primary-key-property-mismatch");
    Assert.DoesNotContain(report.Differences, difference => difference.Code == "index-property-mismatch");
  }

  [Fact]
  public void CompareReportsMissingEntityAndMissingPropertyAsBlocking() {
    var expectedModel = CreateCustomerContactMetadataModel();
    var missingEntityModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        []);
    var missingPropertyModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        [new DataVaultSatelliteMetadata("Contact", DataVaultMetadataReference.Hub("Customer"), ["PhoneNumber"])]);

    var missingEntityReport = DataVaultModelDriftReporter.Compare(expectedModel, CreateModel(missingEntityModel).Model);
    var missingPropertyReport = DataVaultModelDriftReporter.Compare(expectedModel, CreateModel(missingPropertyModel).Model);

    Assert.True(missingEntityReport.HasBlockingDifferences);
    Assert.Contains(
        missingEntityReport.Differences,
        difference => difference.Code == "missing-entity" &&
            difference.LogicalName == "Satellite:Contact" &&
            difference.Severity == DataVaultModelDriftSeverity.Blocking);
    Assert.True(missingPropertyReport.HasBlockingDifferences);
    Assert.Contains(
        missingPropertyReport.Differences,
        difference => difference.Code == "missing-property" &&
            difference.LogicalName == "Satellite:Contact.EmailAddress" &&
            difference.Severity == DataVaultModelDriftSeverity.Blocking);
    Assert.Contains(
        missingPropertyReport.Differences,
        difference => difference.Code == "unexpected-property" &&
            difference.LogicalName == "Satellite:Contact.PhoneNumber" &&
            difference.Severity == DataVaultModelDriftSeverity.Informational);
  }

  [Fact]
  public void CompareReportsRoleMismatchAsBlocking() {
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        []);
    var modelBuilder = CreateModel(metadataModel);
    var hub = FindEntity(modelBuilder.Model, "HubCustomer");
    var businessKey = hub.FindProperty("CustomerId")!;
    businessKey.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);

    var report = DataVaultModelDriftReporter.Compare(metadataModel, modelBuilder.Model);

    Assert.True(report.HasBlockingDifferences);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "property-role-mismatch" &&
            difference.LogicalName == "Hub:Customer.CustomerId" &&
            difference.ExpectedValue == DataVaultPropertyRole.BusinessKey.ToString() &&
            difference.ActualValue == DataVaultPropertyRole.Technical.ToString());
  }

  [Fact]
  public void CompareReportsTimestampStorageAndProviderProfileDriftAsBlocking() {
    var metadataModel = CreateCustomerContactMetadataModel();
    var currentModel = CreateModel(
        metadataModel,
        DataVaultProviderCapabilityProfiles.Sqlite.WithLoadTimestampStorage(DataVaultLoadTimestampStorage.UtcTicks)).Model;

    var report = DataVaultModelDriftReporter.Compare(
        metadataModel,
        currentModel,
        DataVaultProviderCapabilityProfiles.Sqlite);

    Assert.True(report.HasBlockingDifferences);
    Assert.Contains(report.Differences, difference => difference.Code == "provider-profile-mismatch");
    Assert.Contains(report.Differences, difference => difference.Code == "property-provider-profile-mismatch");
    Assert.Contains(report.Differences, difference => difference.Code == "timestamp-storage-mismatch");
    Assert.Contains(report.Differences, difference => difference.Code == "timestamp-value-format-mismatch");
  }

  [Fact]
  public void CompareReportsKeyAndIndexShapeDriftAsBlocking() {
    var expectedModel = CreateCustomerContactMetadataModel();
    var currentModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        [new DataVaultSatelliteMetadata(
            "Contact",
            DataVaultMetadataReference.Hub("Customer"),
            ["EmailAddress"],
            ["ContactType"])]);

    var report = DataVaultModelDriftReporter.Compare(expectedModel, CreateModel(currentModel).Model);

    Assert.True(report.HasBlockingDifferences);
    var primaryKeyDifference = Assert.Single(
        report.Differences,
        difference => difference.Code == "primary-key-property-mismatch" &&
            difference.LogicalName == "Satellite:Contact");
    var indexDifference = Assert.Single(
        report.Differences,
        difference => difference.Code == "index-property-mismatch" &&
            difference.LogicalName == "Satellite:Contact");

    Assert.Contains("Customer=>CustomerHashKey", primaryKeyDifference.ExpectedValue!);
    Assert.Contains("ContactType=>ContactType", primaryKeyDifference.ActualValue!);
    Assert.Contains("Customer=>CustomerHashKey", indexDifference.ExpectedValue!);
    Assert.Contains("ContactType=>ContactType", indexDifference.ActualValue!);
  }

  [Fact]
  public void CompareReportsRepresentativeDriftCategoriesWithStableOrdering() {
    var expectedModel = CreateCustomerContactMetadataModel();
    var currentModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"]), new DataVaultHubMetadata("Order", ["OrderId"])],
        [],
        [new DataVaultSatelliteMetadata(
            "Contact",
            DataVaultMetadataReference.Hub("Customer"),
            ["PhoneNumber"],
            ["ContactType"])]);
    var modelBuilder = CreateModel(
        currentModel,
        DataVaultProviderCapabilityProfiles.Sqlite.WithLoadTimestampStorage(DataVaultLoadTimestampStorage.UtcTicks));
    var hub = FindEntity(modelBuilder.Model, "HubCustomer");
    hub.SetAnnotation(DataVaultAnnotationNames.ProducedName, "HubCustomerArchive");
    hub.FindProperty("CustomerId")!.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);

    var report = DataVaultModelDriftReporter.Compare(
        expectedModel,
        modelBuilder.Model,
        DataVaultProviderCapabilityProfiles.Sqlite);
    var repeatedReport = DataVaultModelDriftReporter.Compare(
        expectedModel,
        modelBuilder.Model,
        DataVaultProviderCapabilityProfiles.Sqlite);

    Assert.True(report.HasBlockingDifferences);
    Assert.Equal(
        report.Differences.Select(CreateDifferenceSignature).ToArray(),
        repeatedReport.Differences.Select(CreateDifferenceSignature).ToArray());
    Assert.Equal(
        report.Differences
            .OrderBy(difference => difference.ElementKind)
            .ThenBy(difference => difference.LogicalName, StringComparer.Ordinal)
            .ThenBy(difference => difference.ProducedName ?? string.Empty, StringComparer.Ordinal)
            .ThenBy(difference => difference.Code, StringComparer.Ordinal)
            .ThenBy(difference => difference.PropertyPath, StringComparer.Ordinal)
            .Select(CreateDifferenceSignature)
            .ToArray(),
        report.Differences.Select(CreateDifferenceSignature).ToArray());
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "unexpected-entity" &&
            difference.LogicalName == "Hub:Order" &&
            difference.Severity == DataVaultModelDriftSeverity.Informational);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "missing-property" &&
            difference.LogicalName == "Satellite:Contact.EmailAddress" &&
            difference.Severity == DataVaultModelDriftSeverity.Blocking);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "unexpected-property" &&
            difference.LogicalName == "Satellite:Contact.PhoneNumber" &&
            difference.Severity == DataVaultModelDriftSeverity.Informational);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "entity-produced-name-mismatch" &&
            difference.LogicalName == "Hub:Customer" &&
            difference.ExpectedValue == "HubCustomer" &&
            difference.ActualValue == "HubCustomerArchive");
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "property-role-mismatch" &&
            difference.LogicalName == "Hub:Customer.CustomerId" &&
            difference.ExpectedValue == DataVaultPropertyRole.BusinessKey.ToString() &&
            difference.ActualValue == DataVaultPropertyRole.Technical.ToString());
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "timestamp-storage-mismatch" &&
            difference.LogicalName == "Hub:Customer.LoadTimestamp" &&
            difference.Severity == DataVaultModelDriftSeverity.Blocking);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "primary-key-property-mismatch" &&
            difference.ElementKind == DataVaultModelDriftElementKind.Key &&
            difference.Severity == DataVaultModelDriftSeverity.Blocking);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "index-property-mismatch" &&
            difference.ElementKind == DataVaultModelDriftElementKind.Index &&
            difference.Severity == DataVaultModelDriftSeverity.Blocking);
  }

  [Fact]
  public void CompareReportsPitSnapshotReferenceDriftAsBlocking() {
    var metadataModel = CreateRepresentativeMetadataModel();
    var modelBuilder = CreateModel(metadataModel);
    var pit = FindEntity(modelBuilder.Model, "PitCustomerContact");
    var snapshotReference = pit.FindProperty("ContactLoadTimestamp")!;
    snapshotReference.SetAnnotation(
        DataVaultAnnotationNames.ProviderLogicalPropertyKind,
        DataVaultLogicalPropertyKind.PayloadText);

    var report = DataVaultModelDriftReporter.Compare(metadataModel, modelBuilder.Model);

    Assert.True(report.HasBlockingDifferences);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "provider-logical-property-kind-mismatch" &&
            difference.LogicalName == "Pit:CustomerContact.Contact" &&
            difference.ExpectedValue == DataVaultLogicalPropertyKind.SatelliteSnapshotReference.ToString() &&
            difference.ActualValue == DataVaultLogicalPropertyKind.PayloadText.ToString());
  }

  [Fact]
  public void CompareModelFirstArtifactReturnsNoDifferencesForHierarchyBridgeSnapshot() {
    var importResult = DataVaultModelArtifactImporter.ImportJson(HierarchyBridgeArtifactJson);
    var currentModel = CreateModel(importResult).Model;

    var report = DataVaultModelDriftReporter.Compare(
        importResult,
        currentModel,
        DataVaultProviderCapabilityProfiles.Sqlite);

    Assert.True(importResult.IsValid);
    Assert.False(report.HasBlockingDifferences);
    Assert.Empty(report.Differences);
  }

  [Fact]
  public void CompareModelFirstArtifactReportsHierarchyBridgeTraversalDepthDrift() {
    var importResult = DataVaultModelArtifactImporter.ImportJson(HierarchyBridgeArtifactJson);
    var modelBuilder = CreateModel(importResult);
    var bridge = FindEntity(modelBuilder.Model, "BridgeSalesRegionHierarchy");
    var traversalDepth = bridge.FindProperty("TraversalDepth")!;
    traversalDepth.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Payload);
    traversalDepth.SetAnnotation(
        DataVaultAnnotationNames.ProviderLogicalPropertyKind,
        DataVaultLogicalPropertyKind.PayloadText);

    var report = DataVaultModelDriftReporter.Compare(
        importResult,
        modelBuilder.Model,
        DataVaultProviderCapabilityProfiles.Sqlite);

    Assert.True(importResult.IsValid);
    Assert.True(report.HasBlockingDifferences);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "property-role-mismatch" &&
            difference.LogicalName == "Bridge:SalesRegionHierarchy.TraversalDepth" &&
            difference.ExpectedValue == DataVaultPropertyRole.BridgeDepth.ToString() &&
            difference.ActualValue == DataVaultPropertyRole.Payload.ToString());
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "provider-logical-property-kind-mismatch" &&
            difference.LogicalName == "Bridge:SalesRegionHierarchy.TraversalDepth" &&
            difference.ExpectedValue == DataVaultLogicalPropertyKind.BridgeDepth.ToString() &&
            difference.ActualValue == DataVaultLogicalPropertyKind.PayloadText.ToString());
  }

  [Fact]
  public void CompareReportsUnsupportedGapsWhenRequiredEfAnnotationsAreMissing() {
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        []);
    var modelBuilder = CreateModel(metadataModel);
    var hub = FindEntity(modelBuilder.Model, "HubCustomer");
    var hashKey = hub.FindProperty("CustomerHashKey")!;
    hashKey.SetAnnotation(DataVaultAnnotationNames.PropertyRole, null);
    hashKey.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, null);
    hashKey.SetAnnotation(DataVaultAnnotationNames.ProviderLogicalPropertyKind, null);
    hashKey.SetAnnotation(DataVaultAnnotationNames.ProviderValueFormat, null);

    var report = DataVaultModelDriftReporter.Compare(metadataModel, modelBuilder.Model);

    Assert.True(report.HasBlockingDifferences);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "property-role-unsupported-gap" &&
            difference.LogicalName == "Hub:Customer.HashKey" &&
            difference.ExpectedValue == DataVaultPropertyRole.Technical.ToString() &&
            difference.ActualValue == "<none>");
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "technical-column-role-unsupported-gap" &&
            difference.LogicalName == "Hub:Customer.HashKey" &&
            difference.ExpectedValue == TechnicalMetadataColumnRole.HashKey.ToString() &&
            difference.ActualValue == "<none>");
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "provider-logical-property-kind-unsupported-gap" &&
            difference.LogicalName == "Hub:Customer.HashKey" &&
            difference.ExpectedValue == DataVaultLogicalPropertyKind.HashKey.ToString() &&
            difference.ActualValue == "<none>");
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "property-value-format-unsupported-gap" &&
            difference.LogicalName == "Hub:Customer.HashKey" &&
            difference.ExpectedValue == DataVaultProviderValueFormat.LowercaseHexText.ToString() &&
            difference.ActualValue == "<none>");
  }

  private static ModelBuilder CreateModel(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile? providerCapabilities = null) {
    var modelBuilder = new ModelBuilder(new ConventionSet());
    modelBuilder.ApplyDataVaultMetadata(
        metadataModel,
        providerCapabilities ?? DataVaultProviderCapabilityProfiles.Sqlite);

    return modelBuilder;
  }

  private static ModelBuilder CreateModel(
      DataVaultModelImportResult importResult,
      DataVaultProviderCapabilityProfile? providerCapabilities = null) {
    var modelBuilder = new ModelBuilder(new ConventionSet());
    modelBuilder.ApplyDataVaultMetadata(
        importResult,
        providerCapabilities ?? DataVaultProviderCapabilityProfiles.Sqlite);

    return modelBuilder;
  }

  private static IMutableEntityType FindEntity(IMutableModel model, string entityName) {
    return model.GetEntityTypes().Single(entity => string.Equals(entity.Name, entityName, StringComparison.Ordinal));
  }

  private static string CreateDifferenceSignature(DataVaultModelDriftDifference difference) {
    return string.Join(
        "|",
        difference.ElementKind,
        difference.LogicalName,
        difference.ProducedName ?? string.Empty,
        difference.Code,
        difference.PropertyPath,
        difference.ExpectedValue ?? string.Empty,
        difference.ActualValue ?? string.Empty);
  }

  private static DataVaultMetadataModel CreateCustomerContactMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        [new DataVaultSatelliteMetadata("Contact", DataVaultMetadataReference.Hub("Customer"), ["EmailAddress"])]);
  }

  private static DataVaultMetadataModel CreateRepresentativeMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        DataVaultMetadataReference.Hub("Customer"),
        ["EmailAddress"]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        DataVaultMetadataReference.Hub("Customer"),
        DataVaultMetadataReference.Link("CustomerOrder"),
        DataVaultMetadataReference.Hub("Order"));
    var pit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        ["Contact"]);

    return new DataVaultMetadataModel(
        [customer, order],
        [customerOrder],
        [contact],
        [],
        [bridge],
        [pit]);
  }

  private const string HierarchyBridgeArtifactJson =
      """
      {
        "schemaVersion": "dvault.model.v1",
        "hubs": [
          {
            "name": "SalesRegion",
            "businessKeys": ["RegionId"]
          }
        ],
        "links": [
          {
            "name": "SalesRegionParentChild",
            "participants": [
              { "hub": "SalesRegion", "role": "ParentRegion" },
              { "hub": "SalesRegion", "role": "ChildRegion" }
            ]
          }
        ],
        "bridges": [
          {
            "name": "SalesRegionHierarchy",
            "kind": "hierarchy",
            "source": "SalesRegionParentChild",
            "endpoints": {
              "ancestor": { "hub": "SalesRegion", "role": "ParentRegion" },
              "descendant": { "hub": "SalesRegion", "role": "ChildRegion" }
            }
          }
        ]
      }
      """;

}
