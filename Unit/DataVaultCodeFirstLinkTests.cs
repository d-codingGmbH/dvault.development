using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit.ExpectedPaths;

public sealed class DataVaultCodeFirstLinkTests {
  [Fact]
  public void CodeFirstLinkParticipantsMatchMetadataFirstRelationshipShapeInDeclarationOrder() {
    var customer = new DataVaultHubMetadata(nameof(Customer), [nameof(Customer.CustomerId)]);
    var order = new DataVaultHubMetadata(nameof(Order), [nameof(Order.OrderId)]);
    var metadataModel = new DataVaultMetadataModel(
        [customer, order],
        [new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()])],
        []);

    var metadataFirstModel = TranslateMetadata(metadataModel);
    var codeFirstModel = TranslateCodeFirst(vault => {
      vault.Hub<Customer>(hub => hub.BusinessKey(customerEntity => customerEntity.CustomerId));
      vault.Hub<Order>(hub => hub.BusinessKey(orderEntity => orderEntity.OrderId));
      vault.Link("CustomerOrder", link => {
        link.Participant<Customer>();
        link.Participant<Order>();
      });
    });

    Assert.Equal(RelationalShape(metadataFirstModel), RelationalShape(codeFirstModel));

    var linkEntity = FindEntity(codeFirstModel, "LinkCustomerOrder");
    var table = StoreObjectIdentifier.Table(linkEntity.GetTableName()!, linkEntity.GetSchema());
    var index = Assert.Single(linkEntity.GetIndexes());

    Assert.Equal(["CustomerHashKey", "OrderHashKey"], index.Properties.Select(property => property.GetColumnName(table)));
  }

  private static IMutableModel TranslateMetadata(DataVaultMetadataModel metadataModel) {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(metadataModel);

    return modelBuilder.Model;
  }

  private static IMutableModel TranslateCodeFirst(Action<DataVaultCodeFirstModelBuilder> configureModel) {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.ApplyDataVaultMetadata(configureModel);

    return modelBuilder.Model;
  }

  private static ModelBuilder CreateModelBuilder() {
    return new ModelBuilder(new ConventionSet());
  }

  private static string[] RelationalShape(IMutableModel model) {
    var lines = new List<string>();

    foreach (var entityType in model.GetEntityTypes().OrderBy(ProducedName, StringComparer.Ordinal)) {
      var table = StoreObjectIdentifier.Table(entityType.GetTableName()!, entityType.GetSchema());
      lines.Add("entity " + ProducedName(entityType) + " table=" + entityType.GetTableName());

      foreach (var property in entityType.GetProperties().OrderBy(Ordinal)) {
        lines.Add(
            "  property " +
            property.Name +
            " column=" +
            property.GetColumnName(table) +
            " role=" +
            AnnotationValue<DataVaultPropertyRole>(property, DataVaultAnnotationNames.PropertyRole) +
            " metadata=" +
            AnnotationValue<string>(property, DataVaultAnnotationNames.MetadataName));
      }

      var primaryKey = entityType.FindPrimaryKey();
      Assert.NotNull(primaryKey);
      lines.Add(
          "  primary-key " +
          AnnotationValue<string>(primaryKey!, DataVaultAnnotationNames.ProducedName) +
          " (" +
          string.Join("|", primaryKey.Properties.Select(property => property.GetColumnName(table))) +
          ")");

      foreach (var index in entityType.GetIndexes().OrderBy(Ordinal)) {
        lines.Add(
            "  index " +
            AnnotationValue<string>(index, DataVaultAnnotationNames.ProducedName) +
            " (" +
            string.Join("|", index.Properties.Select(property => property.GetColumnName(table))) +
            ")");
      }
    }

    return lines.ToArray();
  }

  private static IMutableEntityType FindEntity(IMutableModel model, string producedName) {
    var matches = model.GetEntityTypes()
        .Where(entityType => string.Equals(ProducedName(entityType), producedName, StringComparison.Ordinal))
        .ToArray();

    Assert.Single(matches);
    return matches[0];
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

  private static IAnnotation RequiredAnnotation(IAnnotation? annotation) {
    Assert.NotNull(annotation);

    return annotation!;
  }

  private sealed class Customer {
    public string CustomerId { get; init; } = string.Empty;
  }

  private sealed class Order {
    public string OrderId { get; init; } = string.Empty;
  }
}
