using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultMetadataRegistrationTests {
  [Fact]
  public void AddDVaultRegistersMetadataModelAsDefaultRegistry() {
    var services = new ServiceCollection();
    var metadataModel = CreateCustomerMetadataModel();

    services.AddDVault(options => options.UseMetadataModel(metadataModel));

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var registry = provider.GetRequiredService<DataVaultMetadataRegistry>();

    Assert.Equal(["Customer"], registry.Hubs.Select(hub => hub.Name));
    Assert.True(registry.TryGetHub("Customer", out var hub));
    Assert.Same(registry.Hubs[0], hub);
  }

  [Fact]
  public void AddDVaultRegistersPrebuiltRegistryAsDefaultRegistry() {
    var services = new ServiceCollection();
    var registry = DataVaultMetadataRegistry.Create(CreateCustomerMetadataModel());

    services.AddDVault(options => options.UseMetadataRegistry(registry));

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.Same(registry, provider.GetRequiredService<DataVaultMetadataRegistry>());
  }

  [Fact]
  public void ApplyDataVaultMetadataProjectsRegistryThroughMetadataTranslator() {
    var metadataModel = CreateProjectionParityMetadataModel();
    var baselineModelBuilder = CreateModelBuilder();
    var registryModelBuilder = CreateModelBuilder();

    baselineModelBuilder.ApplyDataVaultMetadata(metadataModel);
    registryModelBuilder.ApplyDataVaultMetadata(DataVaultMetadataRegistry.Create(metadataModel));

    Assert.Equal(ModelShape(baselineModelBuilder.Model), ModelShape(registryModelBuilder.Model));
    Assert.Equal(
        "model-registry",
        Assert.IsType<string>(registryModelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.MetadataSourceKind)?.Value));
    Assert.NotNull(registryModelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.MetadataSourceFingerprint)?.Value);
  }

  private static DataVaultMetadataModel CreateCustomerMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        []);
  }

  private static DataVaultMetadataModel CreateProjectionParityMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);

    return new DataVaultMetadataModel(
        [customer, order],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [customer.ToReference(), order.ToReference()]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                customer.ToReference(),
                ["Email Address"]),
        ]);
  }

  private static ModelBuilder CreateModelBuilder() {
    return new ModelBuilder(new ConventionSet());
  }

  private static string[] ModelShape(IMutableModel model) {
    return
    [
      "model|" + AnnotationShape(model.GetAnnotations()),
      .. model.GetEntityTypes()
        .OrderBy(entityType => entityType.Name, StringComparer.Ordinal)
        .SelectMany(EntityShape),
    ];
  }

  private static IEnumerable<string> EntityShape(IMutableEntityType entityType) {
    var tableName = entityType.GetTableName() ?? entityType.Name;
    var table = StoreObjectIdentifier.Table(tableName, entityType.GetSchema());

    yield return string.Join(
        "|",
        "entity",
        entityType.Name,
        tableName,
        entityType.GetSchema() ?? string.Empty,
        AnnotationShape(entityType.GetAnnotations()));

    foreach (var property in entityType.GetProperties().OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))) {
      yield return string.Join(
          "|",
          "property",
          entityType.Name,
          property.Name,
          property.GetColumnName(table),
          property.GetColumnType() ?? string.Empty,
          property.GetColumnOrder()?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
          property.ClrType.FullName ?? property.ClrType.Name,
          AnnotationShape(property.GetAnnotations()));
    }

    var primaryKey = entityType.FindPrimaryKey();
    if (primaryKey is not null) {
      yield return string.Join(
          "|",
          "primary-key",
          entityType.Name,
          primaryKey.GetName() ?? string.Empty,
          string.Join(",", primaryKey.Properties.Select(property => property.GetColumnName(table))),
          AnnotationShape(primaryKey.GetAnnotations()));
    }

    foreach (var index in entityType.GetIndexes().OrderBy(index => AnnotationValue<int>(index, DataVaultAnnotationNames.Ordinal))) {
      yield return string.Join(
          "|",
          "index",
          entityType.Name,
          index.GetDatabaseName() ?? string.Empty,
          index.IsUnique.ToString(CultureInfo.InvariantCulture),
          string.Join(",", index.Properties.Select(property => property.GetColumnName(table))),
          AnnotationShape(index.GetAnnotations()));
    }
  }

  private static string AnnotationShape(IEnumerable<IAnnotation> annotations) {
    return string.Join(
        ",",
        annotations
            .Where(annotation =>
                !string.Equals(annotation.Name, DataVaultAnnotationNames.MetadataSourceKind, StringComparison.Ordinal) &&
                !string.Equals(annotation.Name, DataVaultAnnotationNames.MetadataSourceFingerprint, StringComparison.Ordinal))
            .OrderBy(annotation => annotation.Name, StringComparer.Ordinal)
            .Select(annotation => annotation.Name + "=" + FormatAnnotationValue(annotation.Value)));
  }

  private static string FormatAnnotationValue(object? value) {
    return value switch {
      null => "<null>",
      Type type => type.FullName ?? type.Name,
      IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
      _ => value.ToString() ?? string.Empty,
    };
  }

  private static T AnnotationValue<T>(IMutableProperty property, string name) {
    var annotation = property.FindAnnotation(name);

    Assert.NotNull(annotation);

    return Assert.IsType<T>(annotation!.Value);
  }

  private static T AnnotationValue<T>(IMutableIndex index, string name) {
    var annotation = index.FindAnnotation(name);

    Assert.NotNull(annotation);

    return Assert.IsType<T>(annotation!.Value);
  }
}
