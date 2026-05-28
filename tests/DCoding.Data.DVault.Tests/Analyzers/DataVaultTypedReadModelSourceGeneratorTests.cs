using System.Collections.Immutable;
using DCoding.Data.DVault.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace DCoding.Data.DVault.Tests.Analyzers;

public sealed class DataVaultTypedReadModelSourceGeneratorTests {
  [Fact]
  public void GeneratesSatelliteReadModelsForProjectedHubLinkAndMultiActiveShapes() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerProfile",
                    "Profile",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [],
                    [
                        Payload("EmailAddress", "EmailAddress", true),
                        Payload("StatusCode", "StatusCode", false),
                    ]),
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerContactByType",
                    "ContactByType",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [
                        DrivingKey("ContactType", "ContactType"),
                        DrivingKey("RegionCode", "RegionCode"),
                    ],
                    [
                        Payload("EmailAddress", "EmailAddress", true),
                    ]),
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerOrderState",
                    "State",
                    "Link",
                    "CustomerOrder",
                    "CustomerOrderHashKey",
                    "CustomerOrder",
                    [],
                    [
                        Payload("StatusCode", "StatusCode", false),
                        Payload("Note", "Note", true),
                    ]))),
        ]);

    Assert.Empty(result.CompilationErrors);
    Assert.Empty(result.GeneratorDiagnostics);

    var profileSource = AssertGeneratedSource(result, "DVault.GeneratedReadModels.SatCustomerProfile.g.cs");
    Assert.Contains("namespace ConsumerApp.DVault.GeneratedReadModels;", profileSource, StringComparison.Ordinal);
    Assert.Contains("public sealed record SatCustomerProfileReadModel(", profileSource, StringComparison.Ordinal);
    Assert.Contains("string CustomerHashKey", profileSource, StringComparison.Ordinal);
    Assert.Contains("string? EmailAddress", profileSource, StringComparison.Ordinal);
    Assert.Contains("string StatusCode", profileSource, StringComparison.Ordinal);
    Assert.Contains("public const string ProducedTableName = \"SatCustomerProfile\";", profileSource, StringComparison.Ordinal);
    Assert.Contains("public const string CustomerHashKeyProducedColumnName = \"CustomerHashKey\";", profileSource, StringComparison.Ordinal);
    Assert.Contains("ReadSatCustomerProfileCurrentAsync", profileSource, StringComparison.Ordinal);
    Assert.Contains("ReadSatCustomerProfileLatestAsync", profileSource, StringComparison.Ordinal);
    Assert.Contains("ReadSatCustomerProfileAsOfAsync", profileSource, StringComparison.Ordinal);
    Assert.Contains("row.NullableString(\"EmailAddress\")", profileSource, StringComparison.Ordinal);
    Assert.Contains("row.RequiredString(\"StatusCode\")", profileSource, StringComparison.Ordinal);

    var multiActiveSource = AssertGeneratedSource(result, "DVault.GeneratedReadModels.SatCustomerContactByType.g.cs");
    Assert.Contains("string ContactType", multiActiveSource, StringComparison.Ordinal);
    Assert.Contains("string RegionCode", multiActiveSource, StringComparison.Ordinal);
    Assert.Contains("new string[] {\"ContactType\", \"RegionCode\"}", multiActiveSource, StringComparison.Ordinal);

    var linkParentSource = AssertGeneratedSource(result, "DVault.GeneratedReadModels.SatCustomerOrderState.g.cs");
    Assert.Contains("DataVaultMetadataReference.Link(\"CustomerOrder\")", linkParentSource, StringComparison.Ordinal);
    Assert.Contains("string CustomerOrderHashKey", linkParentSource, StringComparison.Ordinal);
  }

  [Fact]
  public void GeneratesSatelliteReadModelsFromAuthoritativeSupportBundleExplain() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerProfileRuntime",
                    "Profile",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [
                        DrivingKey("ContactTypeCode", "ContactType"),
                    ],
                    [
                        Payload("EmailAddressValue", "EmailAddress", true),
                    ]))),
        ]);

    Assert.Empty(result.CompilationErrors);
    Assert.Empty(result.GeneratorDiagnostics);

    var source = AssertGeneratedSource(result, "DVault.GeneratedReadModels.SatCustomerProfileRuntime.g.cs");
    Assert.Contains("public sealed record SatCustomerProfileRuntimeReadModel(", source, StringComparison.Ordinal);
    Assert.Contains("public const string ProducedTableName = \"SatCustomerProfileRuntime\";", source, StringComparison.Ordinal);
    Assert.Contains("public const string MetadataSourceKind = \"model-metadata\";", source, StringComparison.Ordinal);
    Assert.Contains("public const string MetadataSourceFingerprint = \"fingerprint-1\";", source, StringComparison.Ordinal);
    Assert.Contains("string CustomerHashKey", source, StringComparison.Ordinal);
    Assert.Contains("string ContactTypeCode", source, StringComparison.Ordinal);
    Assert.Contains("string? EmailAddressValue", source, StringComparison.Ordinal);
    Assert.Contains("public const string ContactTypeCodeProducedColumnName = \"ContactTypeCode\";", source, StringComparison.Ordinal);
    Assert.Contains("public const string EmailAddressValueProducedColumnName = \"EmailAddressValue\";", source, StringComparison.Ordinal);
    Assert.Contains("new string[] {\"EmailAddress\"}", source, StringComparison.Ordinal);
    Assert.Contains("new string[] {\"ContactType\"}", source, StringComparison.Ordinal);
    Assert.Contains("row.NullableString(\"EmailAddress\")", source, StringComparison.Ordinal);
  }

  [Fact]
  public void KeepsFixedTechnicalPublicNamesWhenProducedTechnicalColumnsAreCustomized() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerProfileCustomTechnicalNames",
                    "Profile",
                    "Hub",
                    "Customer",
                    "custom_col_HashKey_Customer",
                    "Customer",
                    [],
                    [
                        Payload("StatusCode", "StatusCode", false),
                    ],
                    hashDiffColumnName: "custom_col_HashDiff",
                    loadTimestampColumnName: "custom_col_LoadTimestamp",
                    recordSourceColumnName: "custom_col_RecordSource"))),
        ]);

    Assert.Empty(result.CompilationErrors);
    Assert.Empty(result.GeneratorDiagnostics);

    var source = AssertGeneratedSource(result, "DVault.GeneratedReadModels.SatCustomerProfileCustomTechnicalNames.g.cs");
    Assert.Contains("string CustomColHashKeyCustomer", source, StringComparison.Ordinal);
    Assert.Contains("string HashDiff", source, StringComparison.Ordinal);
    Assert.Contains("global::System.DateTimeOffset LoadTimestamp", source, StringComparison.Ordinal);
    Assert.Contains("string RecordSource", source, StringComparison.Ordinal);
    Assert.DoesNotContain("string CustomColHashDiff", source, StringComparison.Ordinal);
    Assert.DoesNotContain("global::System.DateTimeOffset CustomColLoadTimestamp", source, StringComparison.Ordinal);
    Assert.DoesNotContain("string CustomColRecordSource", source, StringComparison.Ordinal);
    Assert.Contains("public const string HashDiffProducedColumnName = \"custom_col_HashDiff\";", source, StringComparison.Ordinal);
    Assert.Contains("public const string LoadTimestampProducedColumnName = \"custom_col_LoadTimestamp\";", source, StringComparison.Ordinal);
    Assert.Contains("public const string RecordSourceProducedColumnName = \"custom_col_RecordSource\";", source, StringComparison.Ordinal);
    Assert.Contains("public const string HashDiffMappedName = \"HashDiff\";", source, StringComparison.Ordinal);
    Assert.Contains("public const string LoadTimestampMappedName = \"LoadTimestamp\";", source, StringComparison.Ordinal);
    Assert.Contains("public const string RecordSourceMappedName = \"RecordSource\";", source, StringComparison.Ordinal);
    Assert.Contains("row.RequiredString(\"HashDiff\")", source, StringComparison.Ordinal);
    Assert.Contains("row.RequiredDateTimeOffset(\"LoadTimestamp\")", source, StringComparison.Ordinal);
    Assert.Contains("row.RequiredString(\"RecordSource\")", source, StringComparison.Ordinal);
    Assert.DoesNotContain("row.RequiredString(\"custom_col_HashDiff\")", source, StringComparison.Ordinal);
    Assert.DoesNotContain("row.RequiredDateTimeOffset(\"custom_col_LoadTimestamp\")", source, StringComparison.Ordinal);
    Assert.DoesNotContain("row.RequiredString(\"custom_col_RecordSource\")", source, StringComparison.Ordinal);
  }

  [Fact]
  public void ReportsUnsupportedSupportBundlePayloadOutsideStringBoundary() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerRisk",
                    "Risk",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [],
                    [
                        Payload("RiskScore", "RiskScore", false, clrTypeName: "System.Int32"),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1962", diagnostic.Id);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsUnsupportedSupportBundlePayloadReservedProjectionNameCollision() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerReservedPayload",
                    "ReservedPayload",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [],
                    [
                        Payload("BusinessValue", "ParentHashKey", false),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1962", diagnostic.Id);
    Assert.Contains("collides with a technical projection name", diagnostic.GetMessage(), StringComparison.Ordinal);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsNullabilityFallbackForUnprovenProjectedPayloadNullability() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerProfile",
                    "Profile",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [],
                    [
                        Payload("EmailAddress", "EmailAddress", isNullable: null),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1966", diagnostic.Id);

    var source = AssertGeneratedSource(result, "DVault.GeneratedReadModels.SatCustomerProfile.g.cs");
    Assert.Contains("string? EmailAddress", source, StringComparison.Ordinal);
  }

  [Fact]
  public void ReportsUnavailableSourceForRawModelFirstAdditionalFiles() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.model.json", """
                {
                  "schemaVersion": "dvault.model.v1",
                  "satellites": [
                    {
                      "name": "CustomerOrderState",
                      "parent": {
                        "kind": "link",
                        "name": "CustomerOrder"
                      },
                      "payload": ["StatusCode"],
                      "drivingKeys": ["StateSource"]
                    }
                  ]
                }
                """),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1960", diagnostic.Id);
    Assert.Contains("dvault.support-bundle.v1", diagnostic.GetMessage(), StringComparison.Ordinal);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsStaleConfiguredFingerprintAndSkipsGeneration() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerProfile",
                    "Profile",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [],
                    [
                        Payload("EmailAddress", "EmailAddress", false),
                    ]))),
        ],
        options: new Dictionary<string, string>(StringComparer.Ordinal) {
          ["build_property.DVaultTypedReadModelMetadataSourceFingerprint"] = "stale-fingerprint",
        });

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1961", diagnostic.Id);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsUnavailableSourceWhenNoProjectedSupportBundleIsPresent() {
    var result = RunGenerator(RuntimeStubs);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1960", diagnostic.Id);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsDeterministicGeneratedTypeNameCollisions() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerProfile",
                    "Profile",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [],
                    [
                        Payload("EmailAddress", "EmailAddress", false),
                    ]),
                CreateSupportBundleSatelliteEntityJson(
                    "sat_customer_profile",
                    "profile",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [],
                    [
                        Payload("EmailAddress", "EmailAddress", false),
                    ]))),
        ]);

    Assert.Equal(2, result.GeneratorDiagnostics.Count(diagnostic => diagnostic.Id == "DMV1965"));
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsUnsupportedPitShapeFromProjectedSupportBundleAndSkipsHelper() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleEntityJson(
                    "PitCustomerTimeline",
                    "Pit",
                    "CustomerTimeline",
                    "Hub",
                    "Customer",
                    [
                        Technical("CustomerHashKey", "HashKey", "Customer", "HashKey", "Text", "System.String", false),
                        Technical("LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "Iso8601UtcText", "System.DateTimeOffset", false),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1963", diagnostic.Id);
    Assert.Contains("CustomerTimeline", diagnostic.GetMessage(), StringComparison.Ordinal);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsUnsupportedBridgeShapeFromProjectedSupportBundleAndSkipsHelper() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleEntityJson(
                    "BridgeCustomerOrder",
                    "Bridge",
                    "CustomerOrder",
                    fields:
                    [
                        ParticipantReference("CustomerHashKey", "Customer"),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1964", diagnostic.Id);
    Assert.Contains("BridgeCustomerOrder", diagnostic.GetMessage(), StringComparison.Ordinal);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsDynamicQueryShapeFromProjectedSupportBundleAndSkipsHelper() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleEntityJson(
                    "PitCustomerContactTimeline",
                    "Pit",
                    "CustomerContactTimeline",
                    "Hub",
                    "Customer",
                    [
                        Technical("CustomerHashKey", "HashKey", "Customer", "HashKey", "Text", "System.String", false),
                        DrivingKey("ContactType", "ContactType"),
                        Technical("LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "Iso8601UtcText", "System.DateTimeOffset", false),
                        SnapshotReference("ContactLoadTimestamp", "Contact"),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1967", diagnostic.Id);
    Assert.Contains("dynamic runtime query behavior", diagnostic.GetMessage(), StringComparison.Ordinal);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsModelFirstUnsupportedShapeFromProjectedSupportBundleAndSkipsHelper() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonForSource(
                "model-artifact",
                "model-first-fingerprint",
                CreateSupportBundleEntityJson(
                    "PitModelFirstCustomerProfile",
                    "Pit",
                    "ModelFirstCustomerProfile",
                    "Hub",
                    "Customer",
                    [
                        Technical("CustomerHashKey", "HashKey", "Customer", "HashKey", "Text", "System.String", false),
                        Technical("LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "Iso8601UtcText", "System.DateTimeOffset", false),
                        SnapshotReference("ProfileLoadTimestamp", "Profile"),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1968", diagnostic.Id);
    Assert.Contains("model-first", diagnostic.GetMessage(), StringComparison.Ordinal);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsHelperSkippedForRuntimePitShapeAndKeepsSatelliteGeneration() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJson(
                CreateSupportBundleEntityJson(
                    "PitCustomerProfile",
                    "Pit",
                    "CustomerProfile",
                    "Hub",
                    "Customer",
                    [
                        Technical("CustomerHashKey", "HashKey", "Customer", "HashKey", "Text", "System.String", false),
                        Technical("LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "Iso8601UtcText", "System.DateTimeOffset", false),
                        SnapshotReference("ProfileLoadTimestamp", "Profile"),
                    ]),
                CreateSupportBundleSatelliteEntityJson(
                    "SatCustomerProfile",
                    "Profile",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "Customer",
                    [],
                    [
                        Payload("EmailAddress", "EmailAddress", false),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1969", diagnostic.Id);
    Assert.Contains("no typed PIT helper is emitted", diagnostic.GetMessage(), StringComparison.Ordinal);
    AssertGeneratedSource(result, "DVault.GeneratedReadModels.SatCustomerProfile.g.cs");
    Assert.DoesNotContain(
        result.GeneratedSources.Keys,
        hintName => hintName.Contains("PitCustomerProfile", StringComparison.Ordinal));
  }

  private static string AssertGeneratedSource(GeneratorRunResult result, string hintName) {
    Assert.True(
        result.GeneratedSources.TryGetValue(hintName, out var source),
        "Missing generated source " + hintName + ". Available: " + string.Join(", ", result.GeneratedSources.Keys));
    return source;
  }

  private static GeneratorRunResult RunGenerator(
      string source,
      IReadOnlyList<AdditionalText>? additionalTexts = null,
      IReadOnlyDictionary<string, string>? options = null) {
    var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);
    var syntaxTree = CSharpSyntaxTree.ParseText(source, parseOptions);
    var compilation = CSharpCompilation.Create(
        "DVaultTypedReadModelSample",
        [syntaxTree],
        CreateReferences(),
        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithNullableContextOptions(NullableContextOptions.Enable));

    var optionValues = new Dictionary<string, string>(StringComparer.Ordinal) {
      ["build_property.RootNamespace"] = "ConsumerApp",
      ["build_property.DVaultGenerateTypedReadModels"] = "true",
    };
    if (options is not null) {
      foreach (var option in options) {
        optionValues[option.Key] = option.Value;
      }
    }

    var optionsProvider = new TestAnalyzerConfigOptionsProvider(optionValues);
    GeneratorDriver driver = CSharpGeneratorDriver.Create(
        [new DataVaultTypedReadModelSourceGenerator().AsSourceGenerator()],
        additionalTexts: additionalTexts,
        parseOptions: parseOptions,
        optionsProvider: optionsProvider);
    driver = driver.RunGeneratorsAndUpdateCompilation(
        compilation,
        out var outputCompilation,
        out var generatorDiagnostics,
        TestContext.Current.CancellationToken);

    var runResult = driver.GetRunResult();
    var generatedSources = runResult.Results
        .SelectMany(result => result.GeneratedSources)
        .ToDictionary(sourceResult => sourceResult.HintName, sourceResult => sourceResult.SourceText.ToString(), StringComparer.Ordinal);
    var compilationErrors = outputCompilation.GetDiagnostics(TestContext.Current.CancellationToken)
        .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
        .ToArray();

    return new GeneratorRunResult(
        generatorDiagnostics.ToArray(),
        compilationErrors,
        generatedSources);
  }

  private static IReadOnlyList<MetadataReference> CreateReferences() {
    var trustedPlatformAssemblies = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))?
        .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries) ??
        [];

    return trustedPlatformAssemblies
        .Select(path => MetadataReference.CreateFromFile(path))
        .GroupBy(reference => reference.Display, StringComparer.Ordinal)
        .Select(group => group.First())
        .ToArray();
  }

  private static string CreateSupportBundleJson(params string[] entities) {
    return CreateSupportBundleJsonForSource(
        "model-metadata",
        "fingerprint-1",
        entities);
  }

  private static string CreateSupportBundleJsonForSource(
      string sourceKind,
      string sourceFingerprint,
      params string[] entities) {
    return $$"""
        {
          "schemaVersion": "dvault.support-bundle.v1",
          "diagnostics": {
            "explain": {
              "metadataSourceKind": "{{sourceKind}}",
              "metadataSourceFingerprint": "{{sourceFingerprint}}",
              "entities": [
                {{string.Join("," + Environment.NewLine, entities)}}
              ]
            }
          }
        }
        """;
  }

  private static string CreateSupportBundleEntityJson(
      string tableName,
      string tableKind,
      string metadataName,
      string? parentKind = null,
      string? parentName = null,
      IReadOnlyList<SupportBundleField>? fields = null) {
    var parentReferenceJson = parentKind is null || parentName is null
        ? ","
        : "," + Environment.NewLine + $$"""
                  "parentReference": {
                    "kind": "{{parentKind}}",
                    "name": "{{parentName}}"
                  },
          """;
    var properties = (fields ?? Array.Empty<SupportBundleField>())
        .Select((field, ordinal) => CreateSupportBundlePropertyJson(field, ordinal))
        .ToArray();

    return $$"""
                {
                  "tableName": "{{tableName}}",
                  "tableKind": "{{tableKind}}",
                  "metadataName": "{{metadataName}}"{{parentReferenceJson}}
                  "properties": [
                    {{string.Join("," + Environment.NewLine, properties)}}
                  ]
                }
        """;
  }

  private static string CreateSupportBundleSatelliteEntityJson(
      string tableName,
      string metadataName,
      string parentKind,
      string parentName,
      string parentHashKeyColumnName,
      string parentHashKeyMetadataName,
      IReadOnlyList<SupportBundleField> drivingKeys,
      IReadOnlyList<SupportBundleField> payloads,
      string hashDiffColumnName = "HashDiff",
      string loadTimestampColumnName = "LoadTimestamp",
      string recordSourceColumnName = "RecordSource") {
    var fields = new List<SupportBundleField>
    {
        new(parentHashKeyColumnName, "Technical", "HashKey", parentHashKeyMetadataName, "HashKey", "Text", "System.String", false),
    };
    fields.AddRange(drivingKeys);
    fields.Add(new SupportBundleField(hashDiffColumnName, "Technical", "HashDiff", "HashDiff", "HashDiff", "Text", "System.String", false));
    fields.Add(new SupportBundleField(loadTimestampColumnName, "Technical", "LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "Iso8601UtcText", "System.DateTimeOffset", false));
    fields.Add(new SupportBundleField(recordSourceColumnName, "Technical", "RecordSource", "RecordSource", "RecordSource", "Text", "System.String", false));
    fields.AddRange(payloads);

    var properties = fields
        .Select((field, ordinal) => CreateSupportBundlePropertyJson(field, ordinal))
        .ToArray();

    return $$"""
                {
                  "tableName": "{{tableName}}",
                  "tableKind": "Satellite",
                  "metadataName": "{{metadataName}}",
                  "parentReference": {
                    "kind": "{{parentKind}}",
                    "name": "{{parentName}}"
                  },
                  "properties": [
                    {{string.Join("," + Environment.NewLine, properties)}}
                  ]
                }
        """;
  }

  private static string CreateSupportBundlePropertyJson(SupportBundleField field, int ordinal) {
    var technicalRoleJson = field.TechnicalRole is null
        ? string.Empty
        : "," + Environment.NewLine + "                      \"technicalRole\": \"" + field.TechnicalRole + "\"";
    var nullableJson = field.IsNullable.HasValue
        ? "," + Environment.NewLine + "                      \"isNullable\": " + (field.IsNullable.Value ? "true" : "false")
        : string.Empty;

    return $$"""
                    {
                      "name": "{{field.ProducedName}}",
                      "role": "{{field.Role}}"{{technicalRoleJson}},
                      "metadataName": "{{field.MetadataName}}",
                      "ordinal": {{ordinal.ToString(System.Globalization.CultureInfo.InvariantCulture)}},
                      "logicalPropertyKind": "{{field.LogicalPropertyKind}}",
                      "providerProfileName": "sqlite-v1",
                      "storeType": "TEXT",
                      "valueFormat": "{{field.ValueFormat}}",
                      "clrTypeName": "{{field.ClrTypeName}}"{{nullableJson}}
                    }
        """;
  }

  private static SupportBundleField DrivingKey(
      string producedName,
      string metadataName) {
    return new SupportBundleField(
        producedName,
        "DrivingKey",
        null,
        metadataName,
        "DrivingKey",
        "Text",
        "System.String",
        false);
  }

  private static SupportBundleField Technical(
      string producedName,
      string technicalRole,
      string metadataName,
      string logicalPropertyKind,
      string valueFormat,
      string clrTypeName,
      bool? isNullable) {
    return new SupportBundleField(
        producedName,
        "Technical",
        technicalRole,
        metadataName,
        logicalPropertyKind,
        valueFormat,
        clrTypeName,
        isNullable);
  }

  private static SupportBundleField SnapshotReference(
      string producedName,
      string metadataName) {
    return new SupportBundleField(
        producedName,
        "SnapshotReference",
        "LoadTimestamp",
        metadataName,
        "SatelliteSnapshotReference",
        "Iso8601UtcText",
        "System.DateTimeOffset",
        true);
  }

  private static SupportBundleField ParticipantReference(
      string producedName,
      string metadataName) {
    return new SupportBundleField(
        producedName,
        "ParticipantReference",
        "HashKey",
        metadataName,
        "ParticipantReference",
        "Text",
        "System.String",
        false);
  }

  private static SupportBundleField Payload(
      string producedName,
      string metadataName,
      bool? isNullable,
      string clrTypeName = "System.String") {
    return new SupportBundleField(
        producedName,
        "Payload",
        null,
        metadataName,
        "PayloadText",
        "Text",
        clrTypeName,
        isNullable);
  }

  private sealed record SupportBundleField(
      string ProducedName,
      string Role,
      string? TechnicalRole,
      string MetadataName,
      string LogicalPropertyKind,
      string ValueFormat,
      string ClrTypeName,
      bool? IsNullable);

  private sealed record GeneratorRunResult(
      IReadOnlyList<Diagnostic> GeneratorDiagnostics,
      IReadOnlyList<Diagnostic> CompilationErrors,
      IReadOnlyDictionary<string, string> GeneratedSources);

  private sealed class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider {
    private readonly AnalyzerConfigOptions _options;

    public TestAnalyzerConfigOptionsProvider(IReadOnlyDictionary<string, string> values) {
      _options = new TestAnalyzerConfigOptions(values);
    }

    public override AnalyzerConfigOptions GlobalOptions => _options;

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) {
      return _options;
    }

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) {
      return _options;
    }
  }

  private sealed class TestAnalyzerConfigOptions : AnalyzerConfigOptions {
    private readonly IReadOnlyDictionary<string, string> _values;

    public TestAnalyzerConfigOptions(IReadOnlyDictionary<string, string> values) {
      _values = values;
    }

    public override bool TryGetValue(string key, out string value) {
      return _values.TryGetValue(key, out value!);
    }
  }

  private sealed class TestAdditionalText(string path, string text) : AdditionalText {
    public override string Path { get; } = path;

    public override SourceText GetText(CancellationToken cancellationToken = default) {
      return SourceText.From(text);
    }
  }

  private const string RuntimeStubs = """
      using System;
      using System.Collections.Generic;
      using System.Threading;
      using System.Threading.Tasks;

      namespace Microsoft.EntityFrameworkCore {
        public class DbContext { }
      }

      namespace DCoding.Data.DVault {
        using Microsoft.EntityFrameworkCore;

        public interface IDataVaultReadService { }

        public sealed class DataVaultLatestSatelliteReadRequest {
          public DataVaultLatestSatelliteReadRequest(
              Modeling.DataVaultSatelliteMetadata satellite,
              IEnumerable<string> parentHashKeys) {
          }
        }

        public sealed class DataVaultSatelliteProjectionRow {
          public string RequiredString(string name) {
            return "";
          }

          public string? NullableString(string name) {
            return null;
          }

          public DateTimeOffset RequiredDateTimeOffset(string name) {
            return DateTimeOffset.UtcNow;
          }
        }

        public static class DataVaultReadServiceCurrentSatelliteExtensions {
          public static Task<IReadOnlyList<TProjection>> ReadCurrentSatelliteAsync<TProjection>(
              IDataVaultReadService readService,
              DbContext dbContext,
              Modeling.DataVaultSatelliteMetadata satellite,
              IEnumerable<string> parentHashKeys,
              Func<DataVaultSatelliteProjectionRow, TProjection> projector,
              CancellationToken cancellationToken = default) {
            return Task.FromResult<IReadOnlyList<TProjection>>(Array.Empty<TProjection>());
          }

          public static Task<IReadOnlyList<TProjection>> ReadAsOfSatelliteAsync<TProjection>(
              IDataVaultReadService readService,
              DbContext dbContext,
              Modeling.DataVaultSatelliteMetadata satellite,
              IEnumerable<string> parentHashKeys,
              DateTimeOffset asOf,
              Func<DataVaultSatelliteProjectionRow, TProjection> projector,
              CancellationToken cancellationToken = default) {
            return Task.FromResult<IReadOnlyList<TProjection>>(Array.Empty<TProjection>());
          }
        }

        public static class DataVaultReadServiceTypedProjectionExtensions {
          public static Task<IReadOnlyList<TProjection>> ReadLatestSatelliteAsync<TProjection>(
              IDataVaultReadService readService,
              DbContext dbContext,
              DataVaultLatestSatelliteReadRequest request,
              Func<DataVaultSatelliteProjectionRow, TProjection> projector,
              CancellationToken cancellationToken = default) {
            return Task.FromResult<IReadOnlyList<TProjection>>(Array.Empty<TProjection>());
          }
        }
      }

      namespace DCoding.Data.DVault.Modeling {
        using System.Collections.Generic;

        public sealed class DataVaultMetadataReference {
          public static DataVaultMetadataReference Hub(string name) => new();

          public static DataVaultMetadataReference Link(string name) => new();
        }

        public sealed class DataVaultSatelliteMetadata {
          public DataVaultSatelliteMetadata(
              string name,
              DataVaultMetadataReference parent,
              IEnumerable<string> payloadNames) {
          }

          public DataVaultSatelliteMetadata(
              string name,
              DataVaultMetadataReference parent,
              IEnumerable<string> payloadNames,
              IEnumerable<string> drivingKeyNames) {
          }
        }
      }
      """;
}
