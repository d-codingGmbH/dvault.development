using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultSaveChangesGuardInterceptorDecisionTests {
  [Fact]
  public void EvaluateThrowsBlockingExceptionForUnsafeStateAndMissingStructuralValue() {
    var options = new DataVaultSaveChangesGuardOptions().UseBlockingMode();
    var interceptor = new DataVaultSaveChangesGuardInterceptor(options);
    using var context = CreateContext();

    var hubEntry = context.Set<Dictionary<string, object>>("GuardHub").Add(new Dictionary<string, object> {
      ["HubHash"] = "customer-hash",
      ["BusinessId"] = "C-100",
    });
    hubEntry.State = EntityState.Modified;
    context.Set<Dictionary<string, object>>("GuardLink").Add(new Dictionary<string, object> {
      ["SyntheticRelationshipKey"] = "relationship-hash",
      ["SourceCustomerKey"] = "customer-hash",
    });

    var exception = Assert.Throws<DataVaultSaveChangesGuardException>(() => interceptor.Evaluate(context));
    var explanation = exception.Report.ToDisplayString();

    Assert.Equal(2, exception.Report.Findings.Count);
    Assert.Contains(exception.Report.Findings, finding =>
        finding.EntityKind == DataVaultTableKind.Hub &&
        finding.State == EntityState.Modified &&
        finding.TableName == "GuardHubTable");
    Assert.Contains(exception.Report.Findings, finding =>
        finding.EntityKind == DataVaultTableKind.Link &&
        finding.State == EntityState.Added &&
        finding.TableName == "GuardRelationshipTable");
    Assert.Contains("Hub 'Customer' mapped to 'GuardHubTable' in Modified state", explanation, StringComparison.Ordinal);
    Assert.Contains("Required structural property 'TargetOrderKey' is missing.", explanation, StringComparison.Ordinal);
  }

  [Fact]
  public void EvaluateEmitsWarningReportWithoutMutatingTrackedRows() {
    var reports = new List<DataVaultSaveChangesGuardReport>();
    var options = new DataVaultSaveChangesGuardOptions().UseWarningMode(reports.Add);
    var interceptor = new DataVaultSaveChangesGuardInterceptor(options);
    using var context = CreateContext();
    var row = new Dictionary<string, object> {
      ["SyntheticRelationshipKey"] = "relationship-hash",
      ["SourceCustomerKey"] = "customer-hash",
    };
    var entry = context.Set<Dictionary<string, object>>("GuardLink").Add(row);

    interceptor.Evaluate(context);

    var report = Assert.Single(reports);
    var finding = Assert.Single(report.Findings);
    Assert.Equal(DataVaultTableKind.Link, finding.EntityKind);
    Assert.Equal(EntityState.Added, finding.State);
    Assert.Contains("TargetOrderKey", finding.Reasons[0], StringComparison.Ordinal);
    Assert.False(row.ContainsKey("TargetOrderKey"));
    Assert.Equal(EntityState.Added, entry.State);
  }

  [Fact]
  public void EvaluateUsesDataVaultAnnotationsAndIgnoresNonHubLinkSatelliteRows() {
    var reports = new List<DataVaultSaveChangesGuardReport>();
    var options = new DataVaultSaveChangesGuardOptions().UseWarningMode(reports.Add);
    var interceptor = new DataVaultSaveChangesGuardInterceptor(options);
    using var context = CreateContext();

    context.Set<Dictionary<string, object>>("GuardLink").Add(new Dictionary<string, object> {
      ["SyntheticRelationshipKey"] = "relationship-hash",
      ["SourceCustomerKey"] = "customer-hash",
    });
    context.Set<Dictionary<string, object>>("GuardBridge").Add(new Dictionary<string, object> {
      ["BridgeSnapshotKey"] = "bridge-hash",
      ["Depth"] = 1,
    });

    interceptor.Evaluate(context);

    var report = Assert.Single(reports);
    var finding = Assert.Single(report.Findings);

    Assert.Equal("GuardRelationshipTable", finding.TableName);
    Assert.Equal("CustomRelationship", finding.MetadataName);
    Assert.Equal(DataVaultTableKind.Link, finding.EntityKind);
    Assert.Contains("Required structural property 'TargetOrderKey' is missing.", finding.Reasons);
  }

  private static DirectGuardContext CreateContext() {
    return new DirectGuardContext(
        new DbContextOptionsBuilder<DirectGuardContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);
  }

  private static void ConfigureDataVaultEntity(
      EntityTypeBuilder<Dictionary<string, object>> entityBuilder,
      DataVaultTableKind entityKind,
      string metadataName) {
    entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, entityKind);
    entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, metadataName);
  }

  private static PropertyBuilder AnnotateProperty(
      PropertyBuilder propertyBuilder,
      DataVaultPropertyRole propertyRole,
      TechnicalMetadataColumnRole? technicalRole = null,
      string? producedName = null,
      int? ordinal = null) {
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, propertyRole);
    if (technicalRole is not null) {
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, technicalRole);
    }

    if (producedName is not null) {
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, producedName);
    }

    if (ordinal is not null) {
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, ordinal.Value);
    }

    return propertyBuilder;
  }

  private sealed class DirectGuardContext(DbContextOptions<DirectGuardContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("GuardHub", entityBuilder => {
        entityBuilder.ToTable("GuardHubTable");
        ConfigureDataVaultEntity(entityBuilder, DataVaultTableKind.Hub, "Customer");
        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("HubHash"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.HashKey,
            "CustomerHashKey",
            ordinal: 0);
        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("BusinessId"),
            DataVaultPropertyRole.BusinessKey,
            ordinal: 1);
        entityBuilder.HasKey("HubHash");
      });

      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("GuardLink", entityBuilder => {
        entityBuilder.ToTable("GuardRelationshipTable");
        ConfigureDataVaultEntity(entityBuilder, DataVaultTableKind.Link, "CustomRelationship");
        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("SyntheticRelationshipKey"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.HashKey,
            ordinal: 0);
        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("SourceCustomerKey"),
            DataVaultPropertyRole.ParticipantReference,
            TechnicalMetadataColumnRole.HashKey,
            ordinal: 1);
        var targetOrderKey = entityBuilder.IndexerProperty<string?>("TargetOrderKey");
        targetOrderKey.IsRequired(false);
        AnnotateProperty(
            targetOrderKey,
            DataVaultPropertyRole.ParticipantReference,
            TechnicalMetadataColumnRole.HashKey,
            ordinal: 2);
        AnnotateProperty(
            entityBuilder.IndexerProperty<DateTimeOffset?>("LoadedAtUtc"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.LoadTimestamp,
            ordinal: 3);
        AnnotateProperty(
            entityBuilder.IndexerProperty<string?>("SourceSystem"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.RecordSource,
            ordinal: 4);
        entityBuilder.HasKey("SyntheticRelationshipKey");
      });

      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("GuardBridge", entityBuilder => {
        entityBuilder.ToTable("GuardBridgeTable");
        ConfigureDataVaultEntity(entityBuilder, DataVaultTableKind.Bridge, "RelationshipBridge");
        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("BridgeSnapshotKey"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.HashKey,
            ordinal: 0);
        AnnotateProperty(
            entityBuilder.IndexerProperty<int>("Depth"),
            DataVaultPropertyRole.BridgeDepth,
            ordinal: 1);
        entityBuilder.HasKey("BridgeSnapshotKey");
      });
    }
  }
}
