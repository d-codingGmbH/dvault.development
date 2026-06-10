using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultModelArtifactImporterTests {
  private static readonly string[] ApprovedModelArtifactDiagnosticCodes =
  [
      "DMV1001",
      "DMV1002",
      "DMV1101",
      "DMV1102",
      "DMV1103",
      "DMV1201",
      "DMV1202",
      "DMV1203",
      "DMV1301",
      "DMV1302",
      "DMV1303",
      "DMV1401",
      "DMV1501",
      "DMV1502",
      "DMV1601",
      "DMV1602",
      "DMV1701",
      "DMV1801",
  ];

  private static readonly IReadOnlyDictionary<string, (string Severity, string Category)> ApprovedModelArtifactDiagnosticMetadata =
      new Dictionary<string, (string Severity, string Category)>(StringComparer.Ordinal) {
        ["DMV1001"] = ("error", "schema-version"),
        ["DMV1002"] = ("error", "schema-version"),
        ["DMV1101"] = ("error", "shape"),
        ["DMV1102"] = ("error", "shape"),
        ["DMV1103"] = ("error", "shape"),
        ["DMV1201"] = ("error", "duplicate"),
        ["DMV1202"] = ("error", "duplicate"),
        ["DMV1203"] = ("error", "duplicate"),
        ["DMV1301"] = ("error", "reference"),
        ["DMV1302"] = ("error", "reference"),
        ["DMV1303"] = ("error", "reference"),
        ["DMV1401"] = ("error", "naming"),
        ["DMV1501"] = ("error", "capability"),
        ["DMV1502"] = ("error", "provider-choice"),
        ["DMV1601"] = ("error", "recursive-participant-binding"),
        ["DMV1602"] = ("error", "recursive-participant-binding"),
        ["DMV1701"] = ("error", "shape"),
        ["DMV1801"] = ("error", "projection"),
      };

  [Fact]
  public void DiagnosticCatalogExposesApprovedModelArtifactSeedSetInAscendingCodeOrder() {
    var definitions = DataVaultDiagnosticCatalog.ModelArtifactDefinitions;

    Assert.Equal(ApprovedModelArtifactDiagnosticCodes, definitions.Select(definition => definition.Code));
    Assert.Equal(
        ApprovedModelArtifactDiagnosticCodes,
        definitions.Select(definition => definition.Code).OrderBy(code => code, StringComparer.Ordinal));
  }

  [Fact]
  public void DiagnosticCatalogEntriesHaveUniqueCodesAndRequiredDocumentation() {
    var definitions = DataVaultDiagnosticCatalog.ModelArtifactDefinitions;

    Assert.Equal(
        definitions.Count,
        definitions.Select(definition => definition.Code).Distinct(StringComparer.Ordinal).Count());

    foreach (var definition in definitions) {
      Assert.False(string.IsNullOrWhiteSpace(definition.Summary), definition.Code + " summary should be documented.");
      Assert.False(string.IsNullOrWhiteSpace(definition.Explanation), definition.Code + " explanation should be documented.");
      Assert.False(string.IsNullOrWhiteSpace(definition.Remediation), definition.Code + " remediation should be documented.");
    }
  }

  [Fact]
  public void DiagnosticCatalogPreservesApprovedModelArtifactSeverityAndCategoryBaseline() {
    var definitions = DataVaultDiagnosticCatalog.ModelArtifactDefinitions;

    Assert.Equal(
        ApprovedModelArtifactDiagnosticMetadata.Keys.OrderBy(code => code, StringComparer.Ordinal),
        definitions.Select(definition => definition.Code));
    foreach (var definition in definitions) {
      var expected = ApprovedModelArtifactDiagnosticMetadata[definition.Code];

      Assert.Equal(expected.Severity, definition.Severity);
      Assert.Equal(expected.Category, definition.Category);
    }
  }

  [Fact]
  public void ImportJsonReturnsPublicResultWithSourceScopedDiagnosticsAndRegistryProfiles() {
    var result = DataVaultModelArtifactImporter.ImportJson(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "loadTimestampStorage": "utc-ticks",
          "hubs": [
            {
              "name": "Customer",
              "businessKeys": ["CustomerNumber"]
            }
          ]
        }
        """,
        "models/customer.json");

    AssertValid(result);
    Assert.Equal("models/customer.json", result.LogicalSourcePath);
    Assert.Equal(DataVaultLoadTimestampStorage.UtcTicks, result.LoadTimestampStorage);
    Assert.True(result.MetadataRegistry!.TryGetProviderCapabilityProfile("postgres-v1-loadts-utc-ticks", out var profile));
    Assert.True(result.MetadataRegistry.TryGetProviderCapabilityProfile("db2-v1-loadts-utc-ticks", out _));

    var mapping = profile!.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.LoadTimestamp);

    Assert.Equal(typeof(long), mapping.ModelClrType);
    Assert.Equal(DataVaultProviderValueFormat.UtcTicks, mapping.ValueFormat);
  }

  [Fact]
  public void ImportJsonScopesParseDiagnosticsToLogicalSourcePath() {
    var result = DataVaultModelArtifactImporter.ImportJson(
        """
        {
          "schemaVersion": "dvault.model.v2"
        }
        """,
        "models/invalid.json");

    Assert.False(result.IsValid);
    var diagnostic = Assert.Single(result.Diagnostics);

    Assert.Equal("error", diagnostic.Severity);
    Assert.Equal("schema-version", diagnostic.Category);
    Assert.Equal("models/invalid.json", diagnostic.LogicalSourcePath);
    Assert.Equal("/schemaVersion", diagnostic.JsonPointer);
    Assert.Equal("DMV1002", diagnostic.Code);
  }

  [Fact]
  public void FormatDiagnosticsIncludesParseDiagnosticCodeCategoryAndAffectedLocation() {
    var result = DataVaultModelArtifactImporter.ImportJson(
        """
        {
          "schemaVersion": "dvault.model.v2"
        }
        """,
        "models/sales-vault.json");

    var diagnosticText = DataVaultModelImportResult.FormatDiagnostics(result.Diagnostics);

    Assert.Equal(
        "error schema-version DMV1002 models/sales-vault.json/schemaVersion: Unsupported schemaVersion 'dvault.model.v2'. Expected 'dvault.model.v1'.",
        diagnosticText);
  }

  [Fact]
  public void ApplyToProjectsImportedRegistryThroughModelArtifactSourceAndTimestampStorageProfile() {
    var result = DataVaultModelArtifactImporter.ImportJson(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "loadTimestampStorage": "utc-ticks",
          "hubs": [
            {
              "name": "Customer",
              "businessKeys": ["CustomerNumber"]
            }
          ]
        }
        """);
    var modelBuilder = CreateModelBuilder();

    var projection = result.ApplyTo(modelBuilder);

    AssertValid(projection);
    Assert.Equal(
        "model-artifact",
        Assert.IsType<string>(modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.MetadataSourceKind)?.Value));

    var loadTimestamp = FindEntity(modelBuilder.Model, "HubCustomer").FindProperty("LoadTimestamp");

    Assert.NotNull(loadTimestamp);
    Assert.Equal(typeof(long), loadTimestamp!.ClrType);
    Assert.Equal(
        "sqlite-v1-loadts-utc-ticks",
        AnnotationValue<string>(loadTimestamp, DataVaultAnnotationNames.ProviderProfile));
    Assert.Equal(
        DataVaultProviderValueFormat.UtcTicks,
        AnnotationValue<DataVaultProviderValueFormat>(loadTimestamp, DataVaultAnnotationNames.ProviderValueFormat));
  }

  [Fact]
  public void ApplyToReportsPostParseProjectionFailuresAtSourceDeclaration() {
    var result = DataVaultModelArtifactImporter.ImportJson(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            {
              "name": "Customer",
              "businessKeys": ["CustomerNumber"]
            }
          ],
          "satellites": [
          {
            "name": "CustomerContactByType",
            "parent": {
              "kind": "hub",
              "name": "Customer"
            },
            "drivingKeys": ["ContactType"],
            "payload": ["ContactValue"]
          },
          {
            "name": "CustomerContactByRegion",
            "parent": {
              "kind": "hub",
              "name": "Customer"
            },
            "drivingKeys": ["RegionCode"],
            "payload": ["ContactValue"]
          }
        ],
        "pits": [
          {
            "name": "CustomerPit",
            "hub": "Customer",
            "satellites": ["CustomerContactByType", "CustomerContactByRegion"]
          }
        ]
        }
        """,
        "models/customer.json");

    AssertValid(result);

    var projection = result.ApplyTo(CreateModelBuilder());

    Assert.False(projection.IsValid);
    var diagnostic = Assert.Single(projection.Diagnostics);

    Assert.Equal("error", diagnostic.Severity);
    Assert.Equal("projection", diagnostic.Category);
    Assert.Equal("DMV1801", diagnostic.Code);
    Assert.Equal("models/customer.json", diagnostic.LogicalSourcePath);
    Assert.Equal("/pits/0", diagnostic.JsonPointer);
    Assert.Contains("do not match multi-active satellite 'CustomerContactByType' driving-key names", diagnostic.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void FormatDiagnosticsIncludesProjectionDiagnosticCodeCategoryAndAffectedLocation() {
    var result = DataVaultModelArtifactImporter.ImportJson(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            {
              "name": "Customer",
              "businessKeys": ["CustomerNumber"]
            }
          ],
          "satellites": [
          {
            "name": "CustomerContactByType",
            "parent": {
              "kind": "hub",
              "name": "Customer"
            },
            "drivingKeys": ["ContactType"],
            "payload": ["ContactValue"]
          },
          {
            "name": "CustomerContactByRegion",
            "parent": {
              "kind": "hub",
              "name": "Customer"
            },
            "drivingKeys": ["RegionCode"],
            "payload": ["ContactValue"]
          }
        ],
        "pits": [
          {
            "name": "CustomerPit",
            "hub": "Customer",
            "satellites": ["CustomerContactByType", "CustomerContactByRegion"]
          }
        ]
        }
        """,
        "models/sales-vault.json");
    AssertValid(result);

    var projection = result.ApplyTo(CreateModelBuilder());

    Assert.False(projection.IsValid);
    var diagnostic = Assert.Single(projection.Diagnostics);
    var diagnosticText = DataVaultModelImportResult.FormatDiagnostics(projection.Diagnostics);

    Assert.Equal(
        "error projection DMV1801 models/sales-vault.json/pits/0: " + diagnostic.Message,
        diagnosticText);
  }

  [Fact]
  public void ImportedResultFeedsAddDVaultAndDbContextRegistryOptIn() {
    var importResult = DataVaultModelArtifactImporter.ImportJson(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            {
              "name": "Customer",
              "businessKeys": ["CustomerNumber"]
            }
          ]
        }
        """);
    var services = new ServiceCollection();

    services.AddDVault(options => options.UseMetadataModel(importResult));

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var registry = provider.GetRequiredService<DataVaultMetadataRegistry>();

    Assert.True(registry.TryGetHub("Customer", out _));

    var optionsBuilder = new DbContextOptionsBuilder<ImportedMetadataContext>()
        .UseSqlite("Data Source=:memory:");
    optionsBuilder.UseDataVaultMetadata(importResult);

    var options = optionsBuilder.Options;
    using var context = new ImportedMetadataContext(options);

    Assert.NotNull(context.Model.FindEntityType("HubCustomer"));
  }

  [Fact]
  public void ImportedSharedSubsetMatchesMetadataFirstAndCodeFirstProjection() {
    var metadataModel = CreateSharedMetadataModel();
    var importResult = DataVaultModelArtifactImporter.ImportJson(SharedSubsetArtifactJson);

    foreach (var profile in BuiltInProfiles()) {
      var metadataFirst = CreateModelBuilder();
      var codeFirst = CreateModelBuilder();
      var imported = CreateModelBuilder();

      metadataFirst.ApplyDataVaultMetadata(metadataModel, profile);
      codeFirst.ApplyDataVaultMetadata(ConfigureSharedCodeFirstModel, profile);
      imported.ApplyDataVaultMetadata(importResult, profile);

      Assert.Equal(RelationalShape(metadataFirst.Model), RelationalShape(imported.Model));
      Assert.Equal(RelationalShape(metadataFirst.Model), RelationalShape(codeFirst.Model));
    }
  }

  [Fact]
  public void ImportedAdvancedShapesMatchMetadataFirstProjection() {
    var metadataModel = CreateAdvancedMetadataModel();
    var importResult = DataVaultModelArtifactImporter.ImportJson(AdvancedArtifactJson);

    foreach (var profile in BuiltInProfiles()) {
      var metadataFirst = CreateModelBuilder();
      var imported = CreateModelBuilder();

      metadataFirst.ApplyDataVaultMetadata(metadataModel, profile);
      imported.ApplyDataVaultMetadata(importResult, profile);

      Assert.Equal(RelationalShape(metadataFirst.Model), RelationalShape(imported.Model));
    }
  }

  [Fact]
  public void ImportedLoadTimestampStorageMatchesMetadataFirstProviderMatrix() {
    var metadataModel = CreateSharedMetadataModel();

    foreach (var storage in LoadTimestampStorageOptions()) {
      var importResult = DataVaultModelArtifactImporter.ImportJson(
          SharedSubsetArtifactJsonWithLoadTimestampStorage(storage.Token));

      foreach (var profile in BuiltInProfiles()) {
        var metadataFirst = CreateModelBuilder();
        var imported = CreateModelBuilder();

        metadataFirst.ApplyDataVaultMetadata(metadataModel, profile, storage.Storage);
        imported.ApplyDataVaultMetadata(importResult, profile);

        Assert.Equal(RelationalShape(metadataFirst.Model), RelationalShape(imported.Model));
      }
    }
  }

  private const string SharedSubsetArtifactJson =
      """
      {
        "schemaVersion": "dvault.model.v1",
        "hubs": [
          {
            "name": "Customer",
            "businessKeys": ["CustomerId", "RegionCode"]
          },
          {
            "name": "Order",
            "businessKeys": ["OrderId"]
          }
        ],
        "links": [
          {
            "name": "CustomerOrder",
            "participants": [
              { "hub": "Customer" },
              { "hub": "Order" }
            ]
          }
        ],
        "satellites": [
          {
            "name": "Contact",
            "parent": {
              "kind": "hub",
              "name": "Customer"
            },
            "payload": ["EmailAddress"]
          },
          {
            "name": "ContactByType",
            "parent": {
              "kind": "hub",
              "name": "Customer"
            },
            "drivingKeys": ["ContactType"],
            "payload": ["ContactValue"]
          }
        ]
      }
      """;

  private const string AdvancedArtifactJson =
      """
      {
        "schemaVersion": "dvault.model.v1",
        "hubs": [
          {
            "name": "Customer",
            "businessKeys": ["CustomerId"]
          },
          {
            "name": "Order",
            "businessKeys": ["OrderId"]
          },
          {
            "name": "SalesRegion",
            "businessKeys": ["RegionCode"]
          }
        ],
        "links": [
          {
            "name": "CustomerOrder",
            "participants": [
              { "hub": "Customer" },
              { "hub": "Order" }
            ]
          },
          {
            "name": "SalesRegionParentChild",
            "participants": [
              { "hub": "SalesRegion", "role": "ParentRegion" },
              { "hub": "SalesRegion", "role": "ChildRegion" }
            ]
          }
        ],
        "satellites": [
          {
            "name": "OrderState",
            "parent": {
              "kind": "link",
              "name": "CustomerOrder"
            },
            "payload": ["State"]
          },
          {
            "name": "Profile",
            "parent": {
              "kind": "hub",
              "name": "Customer"
            },
            "payload": ["EmailAddress"]
          }
        ],
        "pits": [
          {
            "name": "CustomerProfile",
            "hub": "Customer",
            "satellites": ["Profile"]
          }
        ],
        "bridges": [
          {
            "name": "CustomerOrderBridge",
            "kind": "many-to-many",
            "source": "CustomerOrder",
            "endpoints": {
              "from": { "hub": "Customer" },
              "to": { "hub": "Order" }
            }
          },
          {
            "name": "SalesRegionHierarchyBridge",
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

  private static DataVaultMetadataModel CreateSharedMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId", "RegionCode"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);

    return new DataVaultMetadataModel(
        [customer, order],
        [new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()])],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                customer.ToReference(),
                ["EmailAddress"]),
            new DataVaultSatelliteMetadata(
                "ContactByType",
                customer.ToReference(),
                ["ContactValue"],
                ["ContactType"]),
        ]);
  }

  private static DataVaultMetadataModel CreateAdvancedMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var salesRegion = new DataVaultHubMetadata("SalesRegion", ["RegionCode"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var salesRegionHierarchy = new DataVaultLinkMetadata(
        "SalesRegionParentChild",
        [
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ParentRegion"),
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ChildRegion"),
        ]);
    var profile = new DataVaultSatelliteMetadata("Profile", customer.ToReference(), ["EmailAddress"]);

    return new DataVaultMetadataModel(
        [customer, order, salesRegion],
        [customerOrder, salesRegionHierarchy],
        [
            new DataVaultSatelliteMetadata("OrderState", customerOrder.ToReference(), ["State"]),
            profile,
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
        [new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"])]);
  }

  private static void ConfigureSharedCodeFirstModel(DataVaultCodeFirstModelBuilder vault) {
    vault.Hub<Customer>(hub => {
      hub.BusinessKey(customer => customer.CustomerId);
      hub.BusinessKey(customer => customer.RegionCode);
      hub.Satellite("Contact", satellite => satellite.Payload(customer => customer.EmailAddress));
      hub.Satellite("ContactByType", satellite => {
        satellite.DrivingKey(customer => customer.ContactType);
        satellite.Payload(customer => customer.ContactValue);
      });
    });
    vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));
    vault.Link("CustomerOrder", link => {
      link.Participant<Customer>();
      link.Participant<Order>();
    });
  }

  private static ModelBuilder CreateModelBuilder() {
    return new ModelBuilder(new ConventionSet());
  }

  private static DataVaultProviderCapabilityProfile[] BuiltInProfiles() {
    return
    [
        DataVaultProviderCapabilityProfiles.Sqlite,
        DataVaultProviderCapabilityProfiles.Oracle,
        DataVaultProviderCapabilityProfiles.Postgres,
        DataVaultProviderCapabilityProfiles.SqlServer,
        DataVaultProviderCapabilityProfiles.Db2,
        DataVaultProviderCapabilityProfiles.MySql,
    ];
  }

  private static LoadTimestampStorageOption[] LoadTimestampStorageOptions() {
    return
    [
        new("provider-default", DataVaultLoadTimestampStorage.ProviderDefault),
        new("iso-8601-utc-text", DataVaultLoadTimestampStorage.Iso8601UtcText),
        new("utc-ticks", DataVaultLoadTimestampStorage.UtcTicks),
    ];
  }

  private static string SharedSubsetArtifactJsonWithLoadTimestampStorage(string loadTimestampStorage) {
    return SharedSubsetArtifactJson.Replace(
        "\"schemaVersion\": \"dvault.model.v1\",",
        "\"schemaVersion\": \"dvault.model.v1\"," +
        Environment.NewLine +
        "        \"loadTimestampStorage\": \"" +
        loadTimestampStorage +
        "\",",
        StringComparison.Ordinal);
  }

  private static IMutableEntityType FindEntity(IMutableModel model, string name) {
    var entity = model.FindEntityType(name);

    Assert.NotNull(entity);

    return entity!;
  }

  private static string[] RelationalShape(IMutableModel model) {
    return model.GetEntityTypes()
        .OrderBy(entityType => entityType.Name, StringComparer.Ordinal)
        .SelectMany(EntityShape)
        .ToArray();
  }

  private static IEnumerable<string> EntityShape(IMutableEntityType entityType) {
    var tableName = entityType.GetTableName() ?? entityType.Name;
    var table = StoreObjectIdentifier.Table(tableName, entityType.GetSchema());

    yield return string.Join(
        "|",
        "entity",
        entityType.Name,
        tableName,
        AnnotationShape(entityType.GetAnnotations()));

    foreach (var property in entityType.GetProperties().OrderBy(property => AnnotationValue<int>(property, DataVaultAnnotationNames.Ordinal))) {
      yield return string.Join(
          "|",
          "property",
          entityType.Name,
          property.Name,
          property.GetColumnName(table),
          property.GetColumnType() ?? string.Empty,
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
          index.IsUnique.ToString(System.Globalization.CultureInfo.InvariantCulture),
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
      IFormattable formattable => formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture),
      _ => value.ToString() ?? string.Empty,
    };
  }

  private static T AnnotationValue<T>(IReadOnlyAnnotatable annotatable, string name) {
    var annotation = annotatable.FindAnnotation(name);

    Assert.NotNull(annotation);

    return Assert.IsType<T>(annotation!.Value);
  }

  private static void AssertValid(DataVaultModelImportResult result) {
    Assert.True(result.IsValid, DataVaultModelImportResult.FormatDiagnostics(result.Diagnostics));
    Assert.Empty(result.Diagnostics);
    Assert.NotNull(result.MetadataModel);
    Assert.NotNull(result.MetadataRegistry);
  }

  private sealed class ImportedMetadataContext(DbContextOptions<ImportedMetadataContext> options) : DbContext(options) {
  }

  private sealed class Customer {
    public string CustomerId { get; set; } = string.Empty;

    public string RegionCode { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string ContactType { get; set; } = string.Empty;

    public string ContactValue { get; set; } = string.Empty;
  }

  private sealed class Order {
    public string OrderId { get; set; } = string.Empty;
  }

  private sealed record LoadTimestampStorageOption(
      string Token,
      DataVaultLoadTimestampStorage Storage);
}
