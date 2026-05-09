using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultCodeFirstMetadataTranslationTests {
  [Fact]
  public void ApplyDataVaultMetadataProjectsFluentHubAndSatelliteLikeMetadataBaseline() {
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata(nameof(Customer), [nameof(Customer.CustomerId), nameof(Customer.RegionCode)])],
        [],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                DataVaultMetadataReference.Hub(nameof(Customer)),
                [nameof(Customer.EmailAddress), nameof(Customer.PhoneNumber)]),
        ]);
    var baselineModelBuilder = CreateModelBuilder();
    var fluentModelBuilder = CreateModelBuilder();

    baselineModelBuilder.ApplyDataVaultMetadata(metadataModel);
    fluentModelBuilder.ApplyDataVaultMetadata(vault => {
      vault.Hub<Customer>(hub => {
        hub.BusinessKey(customer => customer.CustomerId);
        hub.BusinessKey(customer => customer.RegionCode);
        hub.Satellite("Contact", satellite => {
          satellite.Payload(customer => customer.EmailAddress);
          satellite.Payload(customer => customer.PhoneNumber);
        });
      });
    });

    Assert.Equal(ModelShape(baselineModelBuilder.Model), ModelShape(fluentModelBuilder.Model));
  }

  [Fact]
  public void ApplyDataVaultMetadataProjectsFluentDrivingKeysLikeMetadataBaseline() {
    var metadataModel = new DataVaultMetadataModel(
        [new DataVaultHubMetadata(nameof(Customer), [nameof(Customer.CustomerId)])],
        [],
        [
            new DataVaultSatelliteMetadata(
                "ContactChannel",
                DataVaultMetadataReference.Hub(nameof(Customer)),
                [nameof(Customer.EmailAddress)],
                [nameof(Customer.ContactType), nameof(Customer.RegionCode)]),
        ]);
    var baselineModelBuilder = CreateModelBuilder();
    var fluentModelBuilder = CreateModelBuilder();

    baselineModelBuilder.ApplyDataVaultMetadata(metadataModel);
    fluentModelBuilder.ApplyDataVaultMetadata(vault => {
      vault.Hub<Customer>(hub => {
        hub.BusinessKey(customer => customer.CustomerId);
        hub.Satellite("ContactChannel", satellite => {
          satellite.DrivingKey(customer => customer.ContactType);
          satellite.DrivingKey(customer => customer.RegionCode);
          satellite.Payload(customer => customer.EmailAddress);
        });
      });
    });

    Assert.Equal(ModelShape(baselineModelBuilder.Model), ModelShape(fluentModelBuilder.Model));
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsUnsupportedFluentSelectorsWithActionableMessages() {
    AssertSelectorFailure(
        () => CreateModelBuilder().ApplyDataVaultMetadata(vault => {
          vault.Hub<Customer>(hub => hub.BusinessKey(customer => new { customer.CustomerId, customer.RegionCode }));
        }),
        "BusinessKey");
    AssertSelectorFailure(
        () => CreateModelBuilder().ApplyDataVaultMetadata(vault => {
          vault.Hub<Customer>(hub => {
            hub.BusinessKey(customer => customer.CustomerId);
            hub.Satellite("Contact", satellite => {
              satellite.Payload(customer => customer.EmailAddress + customer.PhoneNumber);
            });
          });
        }),
        "Payload");
    AssertSelectorFailure(
        () => CreateModelBuilder().ApplyDataVaultMetadata(vault => {
          vault.Hub<Customer>(hub => {
            hub.BusinessKey(customer => customer.CustomerId);
            hub.Satellite("ContactChannel", satellite => {
              satellite.DrivingKey(customer => customer.ContactType.ToUpperInvariant());
              satellite.Payload(customer => customer.EmailAddress);
            });
          });
        }),
        "DrivingKey");
    AssertSelectorFailure(
        () => CreateModelBuilder().ApplyDataVaultMetadata(vault => {
          vault.Hub<Customer>(hub => {
            hub.BusinessKey(customer => customer.CustomerId);
            hub.Satellite("Tags", satellite => {
              satellite.Payload(customer => customer.Tags);
            });
          });
        }),
        "Payload");
  }

  [Fact]
  public void ApplyDataVaultMetadataRejectsDuplicateFluentMembersByLogicalName() {
    AssertDuplicateMemberFailure(
        () => CreateModelBuilder().ApplyDataVaultMetadata(vault => {
          vault.Hub<Customer>(hub => {
            hub.BusinessKey(customer => customer.CustomerId);
            hub.BusinessKey(customer => customer.CustomerId);
          });
        }),
        "BusinessKey",
        nameof(Customer.CustomerId));
    AssertDuplicateMemberFailure(
        () => CreateModelBuilder().ApplyDataVaultMetadata(vault => {
          vault.Hub<Customer>(hub => {
            hub.BusinessKey(customer => customer.CustomerId);
            hub.Satellite("Contact", satellite => {
              satellite.Payload(customer => customer.EmailAddress);
              satellite.Payload(customer => customer.EmailAddress);
            });
          });
        }),
        "Payload",
        nameof(Customer.EmailAddress));
    AssertDuplicateMemberFailure(
        () => CreateModelBuilder().ApplyDataVaultMetadata(vault => {
          vault.Hub<Customer>(hub => {
            hub.BusinessKey(customer => customer.CustomerId);
            hub.Satellite("ContactChannel", satellite => {
              satellite.DrivingKey(customer => customer.ContactType);
              satellite.DrivingKey(customer => customer.ContactType);
              satellite.Payload(customer => customer.EmailAddress);
            });
          });
        }),
        "DrivingKey",
        nameof(Customer.ContactType));
  }

  [Fact]
  public void ApplyDataVaultMetadataCodeFirstOverloadRejectsNullArguments() {
    ModelBuilder? modelBuilder = null;
    Action<DataVaultCodeFirstModelBuilder>? configureModel = null;

    var modelBuilderException = Assert.Throws<ArgumentNullException>(() =>
        modelBuilder!.ApplyDataVaultMetadata(vault => vault.Hub<Customer>()));
    var configureException = Assert.Throws<ArgumentNullException>(() =>
        CreateModelBuilder().ApplyDataVaultMetadata(configureModel!));
    var profileException = Assert.Throws<ArgumentNullException>(() =>
        CreateModelBuilder().ApplyDataVaultMetadata(
            vault => vault.Hub<Customer>(),
            null!,
            DataVaultLoadTimestampStorage.ProviderDefault));

    Assert.Equal("modelBuilder", modelBuilderException.ParamName);
    Assert.Equal("configureModel", configureException.ParamName);
    Assert.Equal("providerCapabilities", profileException.ParamName);
  }

  private static ModelBuilder CreateModelBuilder() {
    return new ModelBuilder(new ConventionSet());
  }

  private static void AssertSelectorFailure(Action action, string verb) {
    var exception = Assert.Throws<ArgumentException>(action);

    Assert.Equal("selector", exception.ParamName);
    Assert.Contains(verb + " selector", exception.Message, StringComparison.Ordinal);
    Assert.Contains("direct readable scalar member", exception.Message, StringComparison.Ordinal);
    Assert.Contains("Use repeated " + verb + "(x => x.Member) calls for each scalar member", exception.Message, StringComparison.Ordinal);
  }

  private static void AssertDuplicateMemberFailure(Action action, string verb, string memberName) {
    var exception = Assert.Throws<ArgumentException>(action);

    Assert.Equal("selector", exception.ParamName);
    Assert.Contains(verb + " member '" + memberName + "' is already declared", exception.Message, StringComparison.Ordinal);
    Assert.Contains("Use each logical member name at most once", exception.Message, StringComparison.Ordinal);
  }

  private static string ModelShape(IMutableModel model) {
    var lines = new List<string>();

    foreach (var entityType in model.GetEntityTypes().OrderBy(entity => entity.Name, StringComparer.Ordinal)) {
      lines.Add("entity " + entityType.Name);
      lines.Add("  kind " + AnnotationValue<DataVaultTableKind>(entityType, DataVaultAnnotationNames.EntityKind));
      lines.Add("  metadata " + AnnotationValue<string>(entityType, DataVaultAnnotationNames.MetadataName));
      lines.Add("  parent-kind " + AnnotationValueOrEmpty(entityType, DataVaultAnnotationNames.ParentReferenceKind));
      lines.Add("  parent-name " + AnnotationValueOrEmpty(entityType, DataVaultAnnotationNames.ParentReferenceName));

      foreach (var property in entityType.GetProperties().OrderBy(Ordinal)) {
        lines.Add(
            "  property " +
            property.Name +
            " role=" +
            AnnotationValue<DataVaultPropertyRole>(property, DataVaultAnnotationNames.PropertyRole) +
            " technical=" +
            AnnotationValueOrEmpty(property, DataVaultAnnotationNames.TechnicalColumnRole) +
            " metadata=" +
            AnnotationValue<string>(property, DataVaultAnnotationNames.MetadataName));
      }

      var primaryKey = entityType.FindPrimaryKey();
      Assert.NotNull(primaryKey);
      lines.Add(
          "  primary-key " +
          AnnotationValue<string>(primaryKey!, DataVaultAnnotationNames.ProducedName) +
          " (" +
          string.Join("|", primaryKey!.Properties.Select(property => property.Name)) +
          ")");

      foreach (var index in entityType.GetIndexes().OrderBy(Ordinal)) {
        lines.Add(
            "  index " +
            AnnotationValue<string>(index, DataVaultAnnotationNames.ProducedName) +
            " unique=" +
            index.IsUnique +
            " (" +
            string.Join("|", index.Properties.Select(property => property.Name)) +
            ")");
      }
    }

    return string.Join('\n', lines);
  }

  private static int Ordinal(IReadOnlyAnnotatable annotatable) {
    return AnnotationValue<int>(annotatable, DataVaultAnnotationNames.Ordinal);
  }

  private static T AnnotationValue<T>(IReadOnlyAnnotatable annotatable, string annotationName) {
    return Assert.IsType<T>(annotatable.FindAnnotation(annotationName)?.Value);
  }

  private static string AnnotationValueOrEmpty(IReadOnlyAnnotatable annotatable, string annotationName) {
    return annotatable.FindAnnotation(annotationName)?.Value?.ToString() ?? "";
  }

  private sealed class Customer {
    public string ContactType { get; init; } = string.Empty;

    public string CustomerId { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string RegionCode { get; init; } = string.Empty;

    public IReadOnlyList<string> Tags { get; init; } = [];
  }
}
