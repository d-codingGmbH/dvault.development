using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultModelDriftPreflightReporterTests {
  [Fact]
  public void CompareMetadataPreflightReturnsNoDifferencesForMatchingRuntimeAndSnapshotModels() {
    var metadataModel = CreateCustomerContactMetadataModel();
    using var context = CreateMetadataContext(metadataModel);
    var snapshotModel = CreateSnapshotModel(metadataModel);

    var report = DataVaultModelDriftPreflightReporter.Compare(metadataModel, context, snapshotModel);

    Assert.False(report.HasBlockingDifferences);
    Assert.Equal(0, report.DifferenceCount);
    Assert.Equal(0, report.BlockingDifferenceCount);
    Assert.Empty(report.MetadataVersusRuntime.Differences);
    Assert.Empty(report.MetadataVersusSnapshotModel.Differences);
    Assert.Empty(report.RuntimeVersusSnapshotModel.Differences);
    Assert.Contains("metadata-versus-runtime:", report.ToDisplayString(), StringComparison.Ordinal);
    Assert.Contains("runtime-versus-snapshot-model:", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void CompareMetadataPreflightSeparatesSnapshotModelDriftFromMatchingRuntime() {
    var metadataModel = CreateCustomerContactMetadataModel();
    var driftedSnapshotMetadata = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        [new DataVaultSatelliteMetadata("Contact", DataVaultMetadataReference.Hub("Customer"), ["PhoneNumber"])]);
    using var context = CreateMetadataContext(metadataModel);
    var snapshotModel = CreateSnapshotModel(driftedSnapshotMetadata);

    var report = DataVaultModelDriftPreflightReporter.Compare(metadataModel, context, snapshotModel);

    Assert.True(report.HasBlockingDifferences);
    Assert.Empty(report.MetadataVersusRuntime.Differences);
    Assert.True(report.MetadataVersusSnapshotModel.HasBlockingDifferences);
    Assert.True(report.RuntimeVersusSnapshotModel.HasBlockingDifferences);
    Assert.Contains(
        report.MetadataVersusSnapshotModel.Differences,
        difference => difference.Code == "missing-property" &&
            difference.LogicalName == "Satellite:Contact.EmailAddress");
    Assert.Contains(
        report.RuntimeVersusSnapshotModel.Differences,
        difference => difference.Code == "missing-property" &&
            difference.LogicalName == "Satellite:Contact.EmailAddress");
    Assert.Contains("DVault model drift preflight: blocked", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void CompareMetadataPreflightSeparatesRuntimeModelDriftFromMatchingSnapshot() {
    var metadataModel = CreateCustomerContactMetadataModel();
    var driftedRuntimeMetadata = new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        [new DataVaultSatelliteMetadata("Contact", DataVaultMetadataReference.Hub("Customer"), ["PhoneNumber"])]);
    using var context = CreateMetadataContext(driftedRuntimeMetadata);
    var snapshotModel = CreateSnapshotModel(metadataModel);

    var report = DataVaultModelDriftPreflightReporter.Compare(metadataModel, context, snapshotModel);

    Assert.True(report.HasBlockingDifferences);
    Assert.True(report.MetadataVersusRuntime.HasBlockingDifferences);
    Assert.Empty(report.MetadataVersusSnapshotModel.Differences);
    Assert.True(report.RuntimeVersusSnapshotModel.HasBlockingDifferences);
    Assert.Contains(
        report.MetadataVersusRuntime.Differences,
        difference => difference.Code == "missing-property" &&
            difference.LogicalName == "Satellite:Contact.EmailAddress");
    Assert.Contains(
        report.RuntimeVersusSnapshotModel.Differences,
        difference => difference.Code == "missing-property" &&
            difference.LogicalName == "Satellite:Contact.PhoneNumber");
  }

  [Fact]
  public void CompareModelFirstPreflightSupportsImportResultAuthority() {
    var importResult = DataVaultModelArtifactImporter.ImportJson(ModelFirstArtifactJson, "models/order-vault.json");
    Assert.True(importResult.IsValid, DataVaultModelImportResult.FormatDiagnostics(importResult.Diagnostics));

    using var context = CreateImportContext(importResult);
    var snapshotModel = CreateSnapshotModel(importResult);

    var report = DataVaultModelDriftPreflightReporter.Compare(importResult, context, snapshotModel);

    Assert.False(report.HasBlockingDifferences);
    Assert.Equal(0, report.DifferenceCount);
    Assert.Empty(report.MetadataVersusRuntime.Differences);
    Assert.Empty(report.MetadataVersusSnapshotModel.Differences);
    Assert.Empty(report.RuntimeVersusSnapshotModel.Differences);
  }

  private static MetadataPreflightContext CreateMetadataContext(DataVaultMetadataModel metadataModel) {
    var options = new DbContextOptionsBuilder<MetadataPreflightContext>()
        .UseSqlite("Data Source=:memory:")
        .ReplaceService<IModelCacheKeyFactory, MetadataPreflightModelCacheKeyFactory>()
        .Options;

    return new MetadataPreflightContext(options, metadataModel);
  }

  private static ImportPreflightContext CreateImportContext(DataVaultModelImportResult importResult) {
    var options = new DbContextOptionsBuilder<ImportPreflightContext>()
        .UseSqlite("Data Source=:memory:")
        .ReplaceService<IModelCacheKeyFactory, ImportPreflightModelCacheKeyFactory>()
        .Options;

    return new ImportPreflightContext(options, importResult);
  }

  private static IReadOnlyModel CreateSnapshotModel(DataVaultMetadataModel metadataModel) {
    var modelBuilder = new ModelBuilder(new ConventionSet());
    modelBuilder.ApplyDataVaultMetadata(metadataModel);

    return modelBuilder.Model;
  }

  private static IReadOnlyModel CreateSnapshotModel(DataVaultModelImportResult importResult) {
    var modelBuilder = new ModelBuilder(new ConventionSet());
    modelBuilder.ApplyDataVaultMetadata(importResult);

    return modelBuilder.Model;
  }

  private static DataVaultMetadataModel CreateCustomerContactMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["CustomerId"])],
        [],
        [new DataVaultSatelliteMetadata("Contact", DataVaultMetadataReference.Hub("Customer"), ["EmailAddress"])]);
  }

  private sealed class MetadataPreflightContext(
      DbContextOptions<MetadataPreflightContext> options,
      DataVaultMetadataModel metadataModel) : DbContext(options) {
    public DataVaultMetadataModel MetadataModel { get; } = metadataModel;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(MetadataModel);
    }
  }

  private sealed class MetadataPreflightModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is MetadataPreflightContext preflightContext
          ? (context.GetType(), preflightContext.MetadataModel, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private sealed class ImportPreflightContext(
      DbContextOptions<ImportPreflightContext> options,
      DataVaultModelImportResult importResult) : DbContext(options) {
    public DataVaultModelImportResult ImportResult { get; } = importResult;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ImportResult);
    }
  }

  private sealed class ImportPreflightModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is ImportPreflightContext preflightContext
          ? (context.GetType(), preflightContext.ImportResult, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private const string ModelFirstArtifactJson =
      """
      {
        "schemaVersion": "dvault.model.v1",
        "hubs": [
          {
            "name": "Order",
            "businessKeys": ["OrderId"]
          }
        ],
        "satellites": [
          {
            "name": "OrderFulfillment",
            "parent": {
              "kind": "hub",
              "name": "Order"
            },
            "payload": ["StatusCode"]
          }
        ]
      }
      """;
}
