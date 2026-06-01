using System.Collections.Immutable;
using System.Reflection;
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
  public void GeneratesBridgeReadModelsForSupportedManyToManyAndHierarchyShapes() {
    var manyToManyResult = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreateBridgeReadShapeJson(
                    "ManyToMany",
                    "BridgeCustomerOrder",
                    "CustomerOrder",
                    [
                        new("From", "Customer", "CustomerHashKey"),
                        new("To", "Order", "OrderHashKey"),
                    ]),
                CreateSupportBundleEntityJson(
                    "BridgeCustomerOrder",
                    "Bridge",
                    "CustomerOrder",
                    fields:
                    [
                        ParticipantReference("CustomerHashKey", "Customer"),
                        ParticipantReference("OrderHashKey", "Order"),
                    ]))),
        ]);

    Assert.Empty(manyToManyResult.CompilationErrors);
    Assert.Empty(manyToManyResult.GeneratorDiagnostics);

    var manyToManySource = AssertGeneratedSource(manyToManyResult, "DVault.GeneratedReadModels.BridgeCustomerOrder.g.cs");
    Assert.Contains("public sealed record BridgeCustomerOrderReadModel(", manyToManySource, StringComparison.Ordinal);
    Assert.Contains("string CustomerHashKey", manyToManySource, StringComparison.Ordinal);
    Assert.Contains("string OrderHashKey", manyToManySource, StringComparison.Ordinal);
    Assert.Contains("public const string CustomerHashKeyMappedName = \"CustomerHashKey\";", manyToManySource, StringComparison.Ordinal);
    Assert.Contains("ReadBridgeCustomerOrderFromAsync", manyToManySource, StringComparison.Ordinal);
    Assert.Contains("ReadBridgeCustomerOrderToAsync", manyToManySource, StringComparison.Ordinal);
    Assert.Contains("DataVaultBridgeTraversalEndpoint.From", manyToManySource, StringComparison.Ordinal);
    Assert.Contains("DataVaultBridgeTraversalEndpoint.To", manyToManySource, StringComparison.Ordinal);
    Assert.Contains("row.RequiredString(\"OrderHashKey\")", manyToManySource, StringComparison.Ordinal);
    Assert.DoesNotContain("TraversalDepth", manyToManySource, StringComparison.Ordinal);

    var hierarchyResult = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreateBridgeReadShapeJson(
                    "Hierarchy",
                    "BridgeSalesRegionHierarchy",
                    "SalesRegionHierarchy",
                    [
                        new("Ancestor", "SalesRegion", "AncestorSalesRegionHashKey"),
                        new("Descendant", "SalesRegion", "DescendantSalesRegionHashKey"),
                    ]),
                CreateSupportBundleEntityJson(
                    "BridgeSalesRegionHierarchy",
                    "Bridge",
                    "SalesRegionHierarchy",
                    fields:
                    [
                        ParticipantReference("AncestorSalesRegionHashKey", "AncestorSalesRegion"),
                        ParticipantReference("DescendantSalesRegionHashKey", "DescendantSalesRegion"),
                        BridgeDepth("TraversalDepth"),
                    ]))),
        ]);

    Assert.Empty(hierarchyResult.CompilationErrors);
    Assert.Empty(hierarchyResult.GeneratorDiagnostics);

    var hierarchySource = AssertGeneratedSource(hierarchyResult, "DVault.GeneratedReadModels.BridgeSalesRegionHierarchy.g.cs");
    Assert.Contains("public sealed record BridgeSalesRegionHierarchyReadModel(", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("string AncestorSalesRegionHashKey", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("string DescendantSalesRegionHashKey", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("int TraversalDepth", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("ReadBridgeSalesRegionHierarchyAncestorAsync", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("ReadBridgeSalesRegionHierarchyDescendantAsync", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("int maximumDepth", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("DataVaultBridgeTraversalEndpoint.Ancestor", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("DataVaultBridgeTraversalEndpoint.Descendant", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("endpointHashKeys, maximumDepth", hierarchySource, StringComparison.Ordinal);
    Assert.Contains("row.RequiredInt32(\"TraversalDepth\")", hierarchySource, StringComparison.Ordinal);
  }

  [Fact]
  public async Task GeneratedBridgeHelpersDelegateThroughRuntimeReadBoundaryWithEquivalentRequestsAndProjection() {
    var manyToManyResult = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreateBridgeReadShapeJson(
                    "ManyToMany",
                    "BridgeCustomerOrder",
                    "CustomerOrder",
                    [
                        new("From", "Customer", "CustomerHashKey"),
                        new("To", "Order", "OrderHashKey"),
                    ]),
                CreateSupportBundleEntityJson(
                    "BridgeCustomerOrder",
                    "Bridge",
                    "CustomerOrder",
                    fields:
                    [
                        ParticipantReference("CustomerHashKey", "Customer"),
                        ParticipantReference("OrderHashKey", "Order"),
                    ]))),
        ]);

    Assert.Empty(manyToManyResult.CompilationErrors);
    Assert.Empty(manyToManyResult.GeneratorDiagnostics);

    var manyToManyAssembly = EmitAssembly(manyToManyResult.Compilation);
    var manyToManyRows = await InvokeGeneratedReadAsync(
        manyToManyAssembly,
        "ConsumerApp.DVault.GeneratedReadModels.BridgeCustomerOrderReadExtensions",
        "ReadBridgeCustomerOrderFromAsync",
        (object)new[] { "customer-input-hk" });
    var manyToManyRequest = GetLastBridgeRequest(manyToManyAssembly);
    var manyToManyRow = Assert.Single(manyToManyRows);

    Assert.Equal("From", GetPropertyValue(manyToManyRequest, "Endpoint")?.ToString());
    Assert.Null(GetPropertyValue(manyToManyRequest, "MaximumDepth"));
    Assert.Equal(["customer-input-hk"], Assert.IsAssignableFrom<IEnumerable<string>>(GetPropertyValue(manyToManyRequest, "EndpointHashKeys")));
    Assert.Equal("customer-row-hk", GetPropertyValue(manyToManyRow, "CustomerHashKey"));
    Assert.Equal("order-row-hk", GetPropertyValue(manyToManyRow, "OrderHashKey"));

    var hierarchyResult = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreateBridgeReadShapeJson(
                    "Hierarchy",
                    "BridgeSalesRegionHierarchy",
                    "SalesRegionHierarchy",
                    [
                        new("Ancestor", "SalesRegion", "AncestorSalesRegionHashKey"),
                        new("Descendant", "SalesRegion", "DescendantSalesRegionHashKey"),
                    ]),
                CreateSupportBundleEntityJson(
                    "BridgeSalesRegionHierarchy",
                    "Bridge",
                    "SalesRegionHierarchy",
                    fields:
                    [
                        ParticipantReference("AncestorSalesRegionHashKey", "AncestorSalesRegion"),
                        ParticipantReference("DescendantSalesRegionHashKey", "DescendantSalesRegion"),
                        BridgeDepth("TraversalDepth"),
                    ]))),
        ]);

    Assert.Empty(hierarchyResult.CompilationErrors);
    Assert.Empty(hierarchyResult.GeneratorDiagnostics);

    var hierarchyAssembly = EmitAssembly(hierarchyResult.Compilation);
    var hierarchyRows = await InvokeGeneratedReadAsync(
        hierarchyAssembly,
        "ConsumerApp.DVault.GeneratedReadModels.BridgeSalesRegionHierarchyReadExtensions",
        "ReadBridgeSalesRegionHierarchyAncestorAsync",
        (object)new[] { "ancestor-input-hk" },
        2);
    var hierarchyRequest = GetLastBridgeRequest(hierarchyAssembly);
    var hierarchyRow = Assert.Single(hierarchyRows);

    Assert.Equal("Ancestor", GetPropertyValue(hierarchyRequest, "Endpoint")?.ToString());
    Assert.Equal(2, GetPropertyValue(hierarchyRequest, "MaximumDepth"));
    Assert.Equal(["ancestor-input-hk"], Assert.IsAssignableFrom<IEnumerable<string>>(GetPropertyValue(hierarchyRequest, "EndpointHashKeys")));
    Assert.Equal("ancestor-row-hk", GetPropertyValue(hierarchyRow, "AncestorSalesRegionHashKey"));
    Assert.Equal("descendant-row-hk", GetPropertyValue(hierarchyRow, "DescendantSalesRegionHashKey"));
    Assert.Equal(2, GetPropertyValue(hierarchyRow, "TraversalDepth"));
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
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreatePitReadShapeJson(
                    "PitCustomerContactPreference",
                    "CustomerContactPreference",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "LoadTimestamp",
                    ["ContactType"],
                    [
                        new PitReadShapeSatellite("Contact", "ContactLoadTimestamp", ["ContactType"]),
                        new PitReadShapeSatellite("Preference", "PreferenceLoadTimestamp", ["PreferenceType"]),
                    ]),
                CreateSupportBundleEntityJson(
                    "PitCustomerContactPreference",
                    "Pit",
                    "CustomerContactPreference",
                    "Hub",
                    "Customer",
                    [
                        Technical("CustomerHashKey", "HashKey", "Customer", "HashKey", "Text", "System.String", false),
                        DrivingKey("ContactType", "ContactType"),
                        Technical("LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "Iso8601UtcText", "System.DateTimeOffset", false),
                        SnapshotReference("ContactLoadTimestamp", "Contact"),
                        SnapshotReference("PreferenceLoadTimestamp", "Preference"),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1967", diagnostic.Id);
    Assert.Contains("dynamic runtime query behavior", diagnostic.GetMessage(), StringComparison.Ordinal);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsDynamicBridgeShapeWhenHierarchyDepthIsUnbounded() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreateBridgeReadShapeJson(
                    "Hierarchy",
                    "BridgeSalesRegionHierarchy",
                    "SalesRegionHierarchy",
                    [
                        new("Ancestor", "SalesRegion", "AncestorSalesRegionHashKey"),
                        new("Descendant", "SalesRegion", "DescendantSalesRegionHashKey"),
                    ],
                    includeDepthPredicate: false,
                    includeDepthProjection: false),
                CreateSupportBundleEntityJson(
                    "BridgeSalesRegionHierarchy",
                    "Bridge",
                    "SalesRegionHierarchy",
                    fields:
                    [
                        ParticipantReference("AncestorSalesRegionHashKey", "AncestorSalesRegion"),
                        ParticipantReference("DescendantSalesRegionHashKey", "DescendantSalesRegion"),
                        BridgeDepth("TraversalDepth"),
                    ]))),
        ]);

    var diagnostic = Assert.Single(result.GeneratorDiagnostics);
    Assert.Equal("DMV1967", diagnostic.Id);
    Assert.Contains("maximumDepth", diagnostic.GetMessage(), StringComparison.Ordinal);
    Assert.Empty(result.GeneratedSources);
  }

  [Fact]
  public void ReportsHelperSkippedForResidualRuntimeBridgeShapeAndKeepsSatelliteGeneration() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreateBridgeReadShapeJson(
                    "ManyToMany",
                    "BridgeCustomerOrder",
                    "CustomerOrder",
                    [
                        new("From", "Customer", "CustomerHashKey"),
                        new("To", "Order", "OrderHashKey"),
                    ]),
                CreateSupportBundleEntityJson(
                    "BridgeCustomerOrder",
                    "Bridge",
                    "CustomerOrder",
                    fields:
                    [
                        ParticipantReference("CustomerHashKey", "Customer"),
                        ParticipantReference("OrderHashKey", "Order"),
                        Payload("PathSegment", "PathSegment", false),
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
    Assert.Contains("residual projected property", diagnostic.GetMessage(), StringComparison.Ordinal);
    AssertGeneratedSource(result, "DVault.GeneratedReadModels.SatCustomerProfile.g.cs");
    Assert.DoesNotContain(
        result.GeneratedSources.Keys,
        hintName => hintName.Contains("BridgeCustomerOrder", StringComparison.Ordinal));
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
  public void GeneratesPitReadModelFromRequestBoundSupportBundleReadShapeAndKeepsSatelliteGeneration() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreatePitReadShapeJson(
                    "PitCustomerProfile",
                    "CustomerProfile",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "LoadTimestamp",
                    [],
                    [
                        new PitReadShapeSatellite("Profile", "ProfileLoadTimestamp", []),
                    ]),
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

    Assert.Empty(result.CompilationErrors);
    Assert.Empty(result.GeneratorDiagnostics);

    var pitSource = AssertGeneratedSource(result, "DVault.GeneratedReadModels.PitCustomerProfile.g.cs");
    Assert.Contains("public sealed record PitCustomerProfileReadModel(", pitSource, StringComparison.Ordinal);
    Assert.Contains("string ParentHashKey", pitSource, StringComparison.Ordinal);
    Assert.Contains("global::System.DateTimeOffset LoadTimestamp", pitSource, StringComparison.Ordinal);
    Assert.Contains("global::System.DateTimeOffset? ProfileLoadTimestamp", pitSource, StringComparison.Ordinal);
    Assert.Contains("public const string ParentHashKeyProducedColumnName = \"CustomerHashKey\";", pitSource, StringComparison.Ordinal);
    Assert.Contains("public const string ProfileLoadTimestampProducedColumnName = \"ProfileLoadTimestamp\";", pitSource, StringComparison.Ordinal);
    Assert.Contains("public const string ProfileLoadTimestampMappedName = \"SnapshotLoadTimestamp\";", pitSource, StringComparison.Ordinal);
    Assert.Contains("DataVaultMetadataReference.Hub(\"Customer\")", pitSource, StringComparison.Ordinal);
    Assert.Contains("new global::DCoding.Data.DVault.DataVaultPitAsOfReadRequest(PitMetadata, parentHashKeys, asOf)", pitSource, StringComparison.Ordinal);
    Assert.Contains("readService.ReadPitRowsAsync", pitSource, StringComparison.Ordinal);
    Assert.Contains("GetSnapshotLoadTimestamp(row, \"Profile\")", pitSource, StringComparison.Ordinal);
    AssertGeneratedSource(result, "DVault.GeneratedReadModels.SatCustomerProfile.g.cs");
  }

  [Fact]
  public void GeneratesMultiActivePitReadModelWhenReadShapeProvesSharedDrivingKeys() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreatePitReadShapeJson(
                    "PitCustomerContactProfile",
                    "CustomerContactProfile",
                    "Hub",
                    "Customer",
                    "CustomerHashKey",
                    "LoadTimestamp",
                    ["ContactType"],
                    [
                        new PitReadShapeSatellite("Contact", "ContactLoadTimestamp", ["ContactType"]),
                        new PitReadShapeSatellite("Profile", "ProfileLoadTimestamp", []),
                    ]),
                CreateSupportBundleEntityJson(
                    "PitCustomerContactProfile",
                    "Pit",
                    "CustomerContactProfile",
                    "Hub",
                    "Customer",
                    [
                        Technical("CustomerHashKey", "HashKey", "Customer", "HashKey", "Text", "System.String", false),
                        DrivingKey("ContactType", "ContactType"),
                        Technical("LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "Iso8601UtcText", "System.DateTimeOffset", false),
                        SnapshotReference("ContactLoadTimestamp", "Contact"),
                        SnapshotReference("ProfileLoadTimestamp", "Profile"),
                    ]))),
        ]);

    Assert.Empty(result.CompilationErrors);
    Assert.Empty(result.GeneratorDiagnostics);

    var source = AssertGeneratedSource(result, "DVault.GeneratedReadModels.PitCustomerContactProfile.g.cs");
    Assert.Contains("string ContactType", source, StringComparison.Ordinal);
    Assert.Contains("RequiredDrivingKeyValue(row, \"ContactType\")", source, StringComparison.Ordinal);
    Assert.Contains("new global::DCoding.Data.DVault.Modeling.DataVaultPitSatelliteReferenceMetadata(\"Contact\", isMultiActive: true)", source, StringComparison.Ordinal);
    Assert.Contains("new global::DCoding.Data.DVault.Modeling.DataVaultPitSatelliteReferenceMetadata(\"Profile\")", source, StringComparison.Ordinal);
  }

  [Fact]
  public void GeneratesLinkParentPitReadModelForUniqueNonMultiActiveSatellites() {
    var result = RunGenerator(
        RuntimeStubs,
        additionalTexts:
        [
            new TestAdditionalText("sales.dvault.support-bundle.json", CreateSupportBundleJsonWithReadShape(
                CreatePitReadShapeJson(
                    "PitCustomerOrderStateFulfillment",
                    "CustomerOrderStateFulfillment",
                    "Link",
                    "CustomerOrder",
                    "CustomerOrderHashKey",
                    "LoadTimestamp",
                    [],
                    [
                        new PitReadShapeSatellite("State", "StateLoadTimestamp", []),
                        new PitReadShapeSatellite("Fulfillment", "FulfillmentLoadTimestamp", []),
                    ]),
                CreateSupportBundleEntityJson(
                    "PitCustomerOrderStateFulfillment",
                    "Pit",
                    "CustomerOrderStateFulfillment",
                    "Link",
                    "CustomerOrder",
                    [
                        Technical("CustomerOrderHashKey", "HashKey", "CustomerOrder", "HashKey", "Text", "System.String", false),
                        Technical("LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "LoadTimestamp", "Iso8601UtcText", "System.DateTimeOffset", false),
                        SnapshotReference("StateLoadTimestamp", "State"),
                        SnapshotReference("FulfillmentLoadTimestamp", "Fulfillment"),
                    ]))),
        ]);

    Assert.Empty(result.CompilationErrors);
    Assert.Empty(result.GeneratorDiagnostics);

    var source = AssertGeneratedSource(result, "DVault.GeneratedReadModels.PitCustomerOrderStateFulfillment.g.cs");
    Assert.Contains("DataVaultMetadataReference.Link(\"CustomerOrder\")", source, StringComparison.Ordinal);
    Assert.Contains("string ParentHashKey", source, StringComparison.Ordinal);
    Assert.Contains("public const string ParentHashKeyProducedColumnName = \"CustomerOrderHashKey\";", source, StringComparison.Ordinal);
    Assert.Contains("global::System.DateTimeOffset? StateLoadTimestamp", source, StringComparison.Ordinal);
    Assert.Contains("global::System.DateTimeOffset? FulfillmentLoadTimestamp", source, StringComparison.Ordinal);
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
        generatedSources,
        outputCompilation);
  }

  private static Assembly EmitAssembly(Compilation compilation) {
    using var stream = new MemoryStream();
    var result = compilation.Emit(stream, cancellationToken: TestContext.Current.CancellationToken);
    var errors = result.Diagnostics
        .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
        .ToArray();

    Assert.True(
        result.Success,
        "Expected generated compilation to emit successfully. Errors: " + string.Join(Environment.NewLine, errors));

    stream.Position = 0;
    return Assembly.Load(stream.ToArray());
  }

  private static async Task<IReadOnlyList<object>> InvokeGeneratedReadAsync(
      Assembly assembly,
      string extensionTypeName,
      string methodName,
      params object[] requestArguments) {
    var readService = Activator.CreateInstance(RequireType(assembly, "DCoding.Data.DVault.RecordingReadService"))!;
    var dbContext = Activator.CreateInstance(RequireType(assembly, "Microsoft.EntityFrameworkCore.DbContext"))!;
    var method = RequireType(assembly, extensionTypeName).GetMethod(
        methodName,
        BindingFlags.Public | BindingFlags.Static);
    Assert.NotNull(method);

    var arguments = new List<object?>
    {
        readService,
        dbContext,
    };
    arguments.AddRange(requestArguments);
    arguments.Add(CancellationToken.None);

    var task = Assert.IsAssignableFrom<Task>(method.Invoke(null, arguments.ToArray()));
    await task.ConfigureAwait(false);

    var result = task.GetType().GetProperty("Result")?.GetValue(task);
    Assert.NotNull(result);
    return ((System.Collections.IEnumerable)result).Cast<object>().ToArray();
  }

  private static object GetLastBridgeRequest(Assembly assembly) {
    var request = RequireType(assembly, "DCoding.Data.DVault.DataVaultReadServiceBridgeExtensions")
        .GetProperty("LastRequest", BindingFlags.Public | BindingFlags.Static)
        ?.GetValue(null);

    Assert.NotNull(request);
    return request;
  }

  private static Type RequireType(Assembly assembly, string typeName) {
    var type = assembly.GetType(typeName);
    Assert.NotNull(type);
    return type;
  }

  private static object? GetPropertyValue(object instance, string propertyName) {
    return instance.GetType().GetProperty(propertyName)?.GetValue(instance);
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

  private static string CreateSupportBundleJsonWithReadShape(
      string readShapeJson,
      params string[] entities) {
    return CreateSupportBundleJsonForSourceWithReadShape(
        "model-metadata",
        "fingerprint-1",
        readShapeJson,
        entities);
  }

  private static string CreateSupportBundleJsonForSource(
      string sourceKind,
      string sourceFingerprint,
      params string[] entities) {
    return CreateSupportBundleJsonForSourceWithReadShape(
        sourceKind,
        sourceFingerprint,
        readShapeJson: null,
        entities);
  }

  private static string CreateSupportBundleJsonForSourceWithReadShape(
      string sourceKind,
      string sourceFingerprint,
      string? readShapeJson,
      params string[] entities) {
    var readShapeSection = readShapeJson is null
        ? string.Empty
        : "," + Environment.NewLine + "            \"readShape\": " + readShapeJson;

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
            }{{readShapeSection}}
          }
        }
        """;
  }

  private static string CreatePitReadShapeJson(
      string pitTableName,
      string pitMetadataName,
      string parentKind,
      string parentName,
      string parentHashKeyColumnName,
      string loadTimestampColumnName,
      IReadOnlyList<string> pitDrivingKeyColumnNames,
      IReadOnlyList<PitReadShapeSatellite> satellites) {
    var referencedSatellites = satellites
        .Select(satellite => $$"""
                    {
                      "metadataName": "{{satellite.MetadataName}}",
                      "tableName": "Sat{{parentName}}{{satellite.MetadataName}}",
                      "snapshotReferenceColumnName": "{{satellite.SnapshotReferenceColumnName}}",
                      "parentHashKeyColumnName": "{{parentHashKeyColumnName}}",
                      "loadTimestampColumnName": "LoadTimestamp",
                      "drivingKeyColumnNames": {{CreateJsonStringArray(satellite.DrivingKeyColumnNames)}}
                    }
          """)
        .ToArray();
    var projectedColumns = new List<string>
    {
        $$"""
                    {
                      "role": "pitTechnicalProjection",
                      "columnNames": {{CreateJsonStringArray([parentHashKeyColumnName, loadTimestampColumnName])}}
                    }
          """,
    };
    if (pitDrivingKeyColumnNames.Count > 0) {
      projectedColumns.Add($$"""
                    {
                      "role": "pitDrivingKeyProjection",
                      "columnNames": {{CreateJsonStringArray(pitDrivingKeyColumnNames)}}
                    }
          """);
    }

    projectedColumns.Add($$"""
                    {
                      "role": "snapshotReferenceProjection",
                      "columnNames": {{CreateJsonStringArray(satellites.Select(satellite => satellite.SnapshotReferenceColumnName).ToArray())}}
                    }
        """);

    return $$"""
        {
          "kind": "PitAsOf",
          "pit": {
            "pit": {
              "metadataName": "{{pitMetadataName}}",
              "tableKind": "Pit",
              "tableName": "{{pitTableName}}"
            },
            "parentReference": {
              "kind": "{{parentKind}}",
              "name": "{{parentName}}"
            },
            "referencedSatellites": [
              {{string.Join("," + Environment.NewLine, referencedSatellites)}}
            ],
            "filterColumns": [
              {
                "role": "parentHashKeyFilter",
                "columnNames": {{CreateJsonStringArray([parentHashKeyColumnName])}}
              },
              {
                "role": "asOfCutoff",
                "columnNames": {{CreateJsonStringArray([loadTimestampColumnName])}}
              }
            ],
            "rowIdentityColumns": [
              {
                "role": "pitRowIdentity",
                "columnNames": {{CreateJsonStringArray([parentHashKeyColumnName, .. pitDrivingKeyColumnNames, loadTimestampColumnName])}}
              }
            ],
            "projectedColumns": [
              {{string.Join("," + Environment.NewLine, projectedColumns)}}
            ]
          }
        }
        """;
  }

  private static string CreateJsonStringArray(IReadOnlyList<string> values) {
    return "[" + string.Join(", ", values.Select(value => "\"" + value + "\"")) + "]";
  }

  private static string CreateBridgeReadShapeJson(
      string bridgeKind,
      string tableName,
      string metadataName,
      IReadOnlyList<BridgeReadShapeEndpoint> endpoints,
      bool includeDepthPredicate = true,
      bool includeDepthProjection = true) {
    var endpointJson = endpoints
        .Select(endpoint => $$"""
                    {
                      "endpoint": "{{endpoint.Endpoint}}",
                      "endpointName": "{{endpoint.EndpointName}}",
                      "columnName": "{{endpoint.ColumnName}}"
                    }
          """)
        .ToArray();
    var endpointColumns = endpoints
        .Select(endpoint => "\"" + endpoint.ColumnName + "\"")
        .ToArray();
    var orderingColumns = string.Equals(bridgeKind, "Hierarchy", StringComparison.Ordinal)
        ? endpointColumns.Append("\"TraversalDepth\"").ToArray()
        : endpointColumns;
    var depthPredicateJson = string.Equals(bridgeKind, "Hierarchy", StringComparison.Ordinal) && includeDepthPredicate
        ? "," + Environment.NewLine + """
                  "depthPredicate": {
                    "role": "maximumDepthPredicate",
                    "columnNames": ["TraversalDepth"]
                  },
          """
        : "," + Environment.NewLine;
    var depthProjectionJson = string.Equals(bridgeKind, "Hierarchy", StringComparison.Ordinal) && includeDepthProjection
        ? "," + Environment.NewLine + """
                    {
                      "role": "depthProjection",
                      "columnNames": ["TraversalDepth"]
                    }
          """
        : string.Empty;

    return $$"""
              {
                "kind": "Bridge",
                "bridge": {
                  "bridgeKind": "{{bridgeKind}}",
                  "bridge": {
                    "metadataName": "{{metadataName}}",
                    "tableKind": "Bridge",
                    "tableName": "{{tableName}}"
                  },
                  "endpoints": [
                    {{string.Join("," + Environment.NewLine, endpointJson)}}
                  ],
                  "filterEndpoint": "{{endpoints[0].Endpoint}}",
                  "endpointFilter": {
                    "role": "endpointHashKeyFilter",
                    "columnNames": ["{{endpoints[0].ColumnName}}"]
                  }{{depthPredicateJson}}
                  "deterministicOrdering": [
                    {
                      "role": "resultOrdering",
                      "columnNames": [{{string.Join(", ", orderingColumns)}}]
                    }
                  ],
                  "projectedColumns": [
                    {
                      "role": "endpointProjection",
                      "columnNames": [{{string.Join(", ", endpointColumns)}}]
                    }{{depthProjectionJson}}
                  ]
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

  private static SupportBundleField BridgeDepth(string producedName) {
    return new SupportBundleField(
        producedName,
        "BridgeDepth",
        null,
        "TraversalDepth",
        "BridgeDepth",
        "NativeInteger",
        "System.Int32",
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

  private sealed record PitReadShapeSatellite(
      string MetadataName,
      string SnapshotReferenceColumnName,
      IReadOnlyList<string> DrivingKeyColumnNames);

  private sealed record BridgeReadShapeEndpoint(
      string Endpoint,
      string EndpointName,
      string ColumnName);

  private sealed record GeneratorRunResult(
      IReadOnlyList<Diagnostic> GeneratorDiagnostics,
      IReadOnlyList<Diagnostic> CompilationErrors,
      IReadOnlyDictionary<string, string> GeneratedSources,
      Compilation Compilation);

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
      using System.Linq;
      using System.Threading;
      using System.Threading.Tasks;

      namespace Microsoft.EntityFrameworkCore {
        public class DbContext { }
      }

      namespace DCoding.Data.DVault {
        using Microsoft.EntityFrameworkCore;

        public interface IDataVaultReadService {
          Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
              DbContext dbContext,
              DataVaultPitAsOfReadRequest request,
              CancellationToken cancellationToken = default);
        }

        public sealed class RecordingReadService : IDataVaultReadService {
          public Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
              DbContext dbContext,
              DataVaultPitAsOfReadRequest request,
              CancellationToken cancellationToken = default) {
            return Task.FromResult<IReadOnlyList<DataVaultPitReadRecord>>(Array.Empty<DataVaultPitReadRecord>());
          }
        }

        public enum DataVaultBridgeTraversalEndpoint {
          From,
          To,
          Ancestor,
          Descendant,
        }

        public sealed class DataVaultBridgeReadRequest {
          public DataVaultBridgeReadRequest(
              Modeling.DataVaultBridgeMetadata bridge,
              DataVaultBridgeTraversalEndpoint endpoint,
              IEnumerable<string> endpointHashKeys) {
            Bridge = bridge;
            Endpoint = endpoint;
            EndpointHashKeys = endpointHashKeys.ToArray();
          }

          public DataVaultBridgeReadRequest(
              Modeling.DataVaultBridgeMetadata bridge,
              DataVaultBridgeTraversalEndpoint endpoint,
              IEnumerable<string> endpointHashKeys,
              int? maximumDepth) {
            Bridge = bridge;
            Endpoint = endpoint;
            EndpointHashKeys = endpointHashKeys.ToArray();
            MaximumDepth = maximumDepth;
          }

          public Modeling.DataVaultBridgeMetadata Bridge { get; }

          public DataVaultBridgeTraversalEndpoint Endpoint { get; }

          public IReadOnlyList<string> EndpointHashKeys { get; }

          public int? MaximumDepth { get; }
        }

        public sealed class DataVaultLatestSatelliteReadRequest {
          public DataVaultLatestSatelliteReadRequest(
              Modeling.DataVaultSatelliteMetadata satellite,
              IEnumerable<string> parentHashKeys) {
          }
        }

        public sealed class DataVaultPitAsOfReadRequest {
          public DataVaultPitAsOfReadRequest(
              Modeling.DataVaultPitMetadata pit,
              IEnumerable<string> parentHashKeys,
              DateTimeOffset asOf) {
            Pit = pit;
            ParentHashKeys = new List<string>(parentHashKeys);
            AsOf = asOf;
          }

          public Modeling.DataVaultPitMetadata Pit { get; }

          public IReadOnlyList<string> ParentHashKeys { get; }

          public DateTimeOffset AsOf { get; }
        }

        public sealed class DataVaultPitReadRecord {
          public DataVaultPitReadRecord(
              string parentHashKey,
              DateTimeOffset loadTimestamp,
              IReadOnlyDictionary<string, string> drivingKeyValues,
              IReadOnlyList<DataVaultPitSatelliteSnapshot> satelliteSnapshots) {
            ParentHashKey = parentHashKey;
            LoadTimestamp = loadTimestamp;
            DrivingKeyValues = drivingKeyValues;
            SatelliteSnapshots = satelliteSnapshots;
            SatelliteSnapshotsByName = satelliteSnapshots.ToDictionary(snapshot => snapshot.SatelliteName, StringComparer.Ordinal);
          }

          public string ParentHashKey { get; }

          public DateTimeOffset LoadTimestamp { get; }

          public IReadOnlyDictionary<string, string> DrivingKeyValues { get; }

          public IReadOnlyList<DataVaultPitSatelliteSnapshot> SatelliteSnapshots { get; }

          public IReadOnlyDictionary<string, DataVaultPitSatelliteSnapshot> SatelliteSnapshotsByName { get; }
        }

        public sealed class DataVaultPitSatelliteSnapshot {
          public DataVaultPitSatelliteSnapshot(
              string satelliteName,
              DateTimeOffset? snapshotLoadTimestamp) {
            SatelliteName = satelliteName;
            SnapshotLoadTimestamp = snapshotLoadTimestamp;
          }

          public string SatelliteName { get; }

          public DateTimeOffset? SnapshotLoadTimestamp { get; }
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

        public sealed class DataVaultBridgeProjectionRow {
          private readonly IReadOnlyDictionary<string, object> _values;

          public DataVaultBridgeProjectionRow(IReadOnlyDictionary<string, object> values) {
            _values = values;
          }

          public string RequiredString(string name) {
            return (string)_values[name];
          }

          public int RequiredInt32(string name) {
            return (int)_values[name];
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

        public static class DataVaultReadServiceBridgeExtensions {
          public static DataVaultBridgeReadRequest? LastRequest { get; private set; }

          public static Task<IReadOnlyList<TProjection>> ReadBridgeAsync<TProjection>(
              IDataVaultReadService readService,
              DbContext dbContext,
              DataVaultBridgeReadRequest request,
              Func<DataVaultBridgeProjectionRow, TProjection> projector,
              CancellationToken cancellationToken = default) {
            LastRequest = request;
            var row = new DataVaultBridgeProjectionRow(new Dictionary<string, object>(StringComparer.Ordinal) {
              ["CustomerHashKey"] = "customer-row-hk",
              ["OrderHashKey"] = "order-row-hk",
              ["AncestorSalesRegionHashKey"] = "ancestor-row-hk",
              ["DescendantSalesRegionHashKey"] = "descendant-row-hk",
              ["TraversalDepth"] = 2,
            });
            return Task.FromResult<IReadOnlyList<TProjection>>(new List<TProjection> { projector(row) });
          }
        }
      }

      namespace DCoding.Data.DVault.Modeling {
        using System.Collections.Generic;

        public enum DataVaultBridgeKind {
          ManyToMany,
          Hierarchy,
        }

        public sealed class DataVaultMetadataReference {
          public static DataVaultMetadataReference Hub(string name) => new();

          public static DataVaultMetadataReference Link(string name) => new();
        }

        public sealed class DataVaultBridgeMetadata {
          public static DataVaultBridgeMetadata ManyToMany(
              string name,
              DataVaultMetadataReference sourceHubReference,
              DataVaultMetadataReference linkReference,
              DataVaultMetadataReference targetHubReference) {
            return new();
          }

          public static DataVaultBridgeMetadata Hierarchy(
              string name,
              DataVaultMetadataReference ancestorHubReference,
              DataVaultMetadataReference linkReference,
              DataVaultMetadataReference descendantHubReference,
              int ancestorParticipantOrdinal,
              int descendantParticipantOrdinal) {
            return new();
          }
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

        public sealed class DataVaultPitSatelliteReferenceMetadata {
          public DataVaultPitSatelliteReferenceMetadata(string satelliteName, bool isMultiActive = false) {
          }
        }

        public sealed class DataVaultPitMetadata {
          public DataVaultPitMetadata(
              DataVaultMetadataReference parent,
              IEnumerable<DataVaultPitSatelliteReferenceMetadata> satellites) {
          }
        }
      }
      """;
}
