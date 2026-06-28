using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultHashKeyStorageMigrationManifestValidatorTests {
  [Fact]
  public void ValidateJsonAcceptsCurrentShapeHexStringToBinaryManifest() {
    var result = DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(CreateValidManifestJson());

    Assert.True(result.IsValid, result.ToDisplayString());
    var finding = Assert.Single(result.Findings);
    Assert.Equal(DataVaultDiagnosticsIssueSeverity.Info, finding.Severity);
    Assert.Equal("hash-key-migration-manifest-compatible", finding.Code);
    Assert.Equal("$", finding.Path);
  }

  [Fact]
  public void ValidateJsonReportsUnsupportedSchemaVersion() {
    var result = Validate(root => root["schemaVersion"] = "dvault.hash-key-storage-migration.v2");

    Assert.False(result.IsValid);
    Assert.Contains(result.Findings, finding =>
        finding.Code == "hash-key-migration-schema-version-unsupported" &&
        finding.Path == "$.schemaVersion");
  }

  [Fact]
  public void ValidateJsonReportsMissingRequiredSectionsAndPerEntryFacts() {
    var result = Validate(root => {
      root.Remove("dryRun");
      Entry(root, 0).Remove("target");
    });

    Assert.False(result.IsValid);
    Assert.Contains(result.Findings, finding =>
        finding.Code == "hash-key-migration-required-section-missing" &&
        finding.Path == "$.dryRun");
    Assert.Contains(result.Findings, finding =>
        finding.Code == "hash-key-migration-required-section-missing" &&
        finding.Path == "$.entries[0].target");
  }

  [Fact]
  public void ValidateJsonReportsMissingAndDuplicateCoverageIdentity() {
    var missing = Validate(root => {
      var entries = root["entries"]!.AsArray();
      entries.RemoveAt(1);
    });

    Assert.False(missing.IsValid);
    Assert.Contains(missing.Findings, finding =>
        finding.Code == "hash-key-migration-coverage-entry-count-mismatch" &&
        finding.Path == "$.comparison.entryCount");
    Assert.Contains(missing.Findings, finding =>
        finding.Code == "hash-key-migration-coverage-participant-reference-count-mismatch" &&
        finding.Path == "$.comparison.participantReferenceColumnCount");

    var missingIdentity = Validate(root => Entry(root, 0).Remove("propertyName"));

    Assert.False(missingIdentity.IsValid);
    Assert.Contains(missingIdentity.Findings, finding =>
        finding.Code == "hash-key-migration-coverage-identity-missing" &&
        finding.TableName == "HubCustomer");

    var duplicate = Validate(root => {
      Entry(root, 1)["tableName"] = "HubCustomer";
      Entry(root, 1)["propertyName"] = "CustomerHashKey";
    });

    Assert.False(duplicate.IsValid);
    Assert.Contains(duplicate.Findings, finding =>
        finding.Code == "hash-key-migration-coverage-identity-duplicate" &&
        finding.TableName == "HubCustomer" &&
        finding.ColumnName == "CustomerHashKey");
  }

  [Fact]
  public void ValidateJsonReportsUnsupportedProviderAndCapabilityProfile() {
    var result = Validate(root => {
      root["source"]!["providerName"] = "Unit.Provider";
      root["target"]!["capabilityProfile"] = "custom-v1";
    });

    Assert.False(result.IsValid);
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-provider-unsupported");
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-capability-profile-unsupported");
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-mixed-capability-profile");
  }

  [Fact]
  public void ValidateJsonReportsMetadataSourceFingerprintDrift() {
    var result = Validate(root => root["target"]!["metadataSourceFingerprint"] = "target-fingerprint");

    Assert.False(result.IsValid);
    Assert.Contains(result.Findings, finding =>
        finding.Code == "hash-key-migration-metadata-source-fingerprint-drift" &&
        finding.Path == "$.target.metadataSourceFingerprint" &&
        finding.ExpectedValue == "source-fingerprint" &&
        finding.ActualValue == "target-fingerprint");
  }

  [Fact]
  public void ValidateJsonReportsUnsupportedPerEntryProfileFormatConversionAndHashFacts() {
    var result = Validate(root => {
      var source = Entry(root, 0)["source"]!;
      var target = Entry(root, 0)["target"]!;

      source["storageProfile"] = "Json";
      source["providerValueFormat"] = "Text";
      source["algorithmId"] = "custom-hash-v1";
      target["conversionBehavior"] = "custom-conversion";
    });

    Assert.False(result.IsValid);
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-storage-profile-unsupported");
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-provider-value-format-unsupported");
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-conversion-behavior-unsupported");
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-stable-hash-unsupported");
  }

  [Fact]
  public void ValidateJsonReportsMixedStorageProfilesAndHashFactDrift() {
    var result = Validate(root => {
      Entry(root, 1)["source"]!["storageProfile"] = "Binary";
      Entry(root, 1)["target"]!["storageProfile"] = "HexString";

      var target = Entry(root, 0)["target"]!;
      target["algorithmId"] = "sha1-v1";
      target["digestByteLength"] = 20;
      target["digestEncoding"] = "uppercase-hex-no-prefix";
    });

    Assert.False(result.IsValid);
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-mixed-source-storage-profile");
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-mixed-target-storage-profile");
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-algorithm-drift");
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-digest-length-drift");
    Assert.Contains(result.Findings, finding => finding.Code == "hash-key-migration-digest-encoding-drift");
  }

  [Fact]
  public void ValidateJsonKeepsWarningsNonBlockingForDefaultedCapabilityProfile() {
    var result = Validate(root => root["source"]!["capabilityProfileDefaulted"] = true);

    Assert.True(result.IsValid, result.ToDisplayString());
    Assert.Contains(result.Findings, finding =>
        finding.Severity == DataVaultDiagnosticsIssueSeverity.Warning &&
        finding.Code == "hash-key-migration-capability-profile-defaulted");
    Assert.Contains(result.Findings, finding =>
        finding.Severity == DataVaultDiagnosticsIssueSeverity.Info &&
        finding.Code == "hash-key-migration-manifest-compatible");
  }

  [Fact]
  public void ValidateJsonOrdersFindingsBySeverityCodeTableColumnAndPath() {
    var result = Validate(root => {
      root["target"]!["capabilityProfileDefaulted"] = true;
      root["schemaVersion"] = "dvault.hash-key-storage-migration.v2";
      Entry(root, 1)["source"]!["storageProfile"] = "Binary";
      Entry(root, 0)["target"]!["algorithmId"] = "sha1-v1";
      Entry(root, 0)["target"]!["digestByteLength"] = 20;
    });

    var expectedOrder = result.Findings
        .OrderBy(finding => GetSeveritySortKey(finding.Severity))
        .ThenBy(finding => finding.Code, StringComparer.Ordinal)
        .ThenBy(finding => finding.TableName ?? string.Empty, StringComparer.Ordinal)
        .ThenBy(finding => finding.ColumnName ?? string.Empty, StringComparer.Ordinal)
        .ThenBy(finding => finding.Path, StringComparer.Ordinal)
        .ToArray();

    Assert.Equal(expectedOrder, result.Findings);
  }

  private static DataVaultHashKeyStorageMigrationValidationResult Validate(Action<JsonObject> mutate) {
    var root = JsonNode.Parse(CreateValidManifestJson())!.AsObject();
    mutate(root);
    return DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(
        root.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
  }

  private static JsonObject Entry(JsonObject root, int index) {
    return root["entries"]!.AsArray()[index]!.AsObject();
  }

  private static int GetSeveritySortKey(DataVaultDiagnosticsIssueSeverity severity) {
    return severity switch {
      DataVaultDiagnosticsIssueSeverity.Error => 0,
      DataVaultDiagnosticsIssueSeverity.Warning => 1,
      DataVaultDiagnosticsIssueSeverity.Info => 2,
      _ => 3,
    };
  }

  private static string CreateValidManifestJson() {
    return """
        {
          "schemaVersion": "dvault.hash-key-storage-migration.v1",
          "dryRun": {
            "enabled": true,
            "status": "compatible-review-only",
            "databaseMutation": "none",
            "migrationApplication": "not-run",
            "publicHashKeyBoundary": "lowercase-hex-no-prefix",
            "targetDiagnosticsSourceKind": "unit-test"
          },
          "source": {
            "metadataSourceKind": "model-metadata",
            "metadataSourceFingerprint": "source-fingerprint",
            "providerName": "Microsoft.EntityFrameworkCore.Sqlite",
            "capabilityProfile": "sqlite-v1",
            "capabilityProfileDefaulted": false
          },
          "target": {
            "metadataSourceKind": "model-metadata",
            "metadataSourceFingerprint": "source-fingerprint",
            "providerName": "Microsoft.EntityFrameworkCore.Sqlite",
            "capabilityProfile": "sqlite-v1",
            "capabilityProfileDefaulted": false
          },
          "comparison": {
            "intendedChange": "HexString-to-Binary",
            "compatibilityStatus": "compatible-storage-profile-flip",
            "entryCount": 2,
            "hashKeyColumnCount": 1,
            "participantReferenceColumnCount": 1,
            "ordering": "ordinal by tableName then propertyName"
          },
          "entries": [
            {
              "ordinal": 0,
              "tableName": "HubCustomer",
              "tableKind": "Hub",
              "entityMetadataName": "Customer",
              "propertyName": "CustomerHashKey",
              "propertyRole": "HashKey",
              "technicalRole": "HashKey",
              "logicalPropertyKind": "HashKey",
              "propertyMetadataName": "CustomerHashKey",
              "source": {
                "storageProfile": "HexString",
                "providerStoreType": "TEXT",
                "providerValueFormat": "LowercaseHexText",
                "efClrModelType": "System.String",
                "conversionBehavior": "none-string-model",
                "algorithmId": "sha256-v1",
                "digestByteLength": 32,
                "digestEncoding": "lowercase-hex-no-prefix"
              },
              "target": {
                "storageProfile": "Binary",
                "providerStoreType": "BLOB",
                "providerValueFormat": "LowercaseHexBinary",
                "efClrModelType": "System.String",
                "conversionBehavior": "lowercase-hex-string-to-bytes",
                "algorithmId": "sha256-v1",
                "digestByteLength": 32,
                "digestEncoding": "lowercase-hex-no-prefix"
              }
            },
            {
              "ordinal": 1,
              "tableName": "LinkCustomerOrder",
              "tableKind": "Link",
              "entityMetadataName": "CustomerOrder",
              "propertyName": "OrderHashKey",
              "propertyRole": "LinkParticipantHashKey",
              "technicalRole": null,
              "logicalPropertyKind": "ParticipantReference",
              "propertyMetadataName": "OrderHashKey",
              "source": {
                "storageProfile": "HexString",
                "providerStoreType": "TEXT",
                "providerValueFormat": "LowercaseHexText",
                "efClrModelType": "System.String",
                "conversionBehavior": "none-string-model",
                "algorithmId": "sha256-v1",
                "digestByteLength": 32,
                "digestEncoding": "lowercase-hex-no-prefix"
              },
              "target": {
                "storageProfile": "Binary",
                "providerStoreType": "BLOB",
                "providerValueFormat": "LowercaseHexBinary",
                "efClrModelType": "System.String",
                "conversionBehavior": "lowercase-hex-string-to-bytes",
                "algorithmId": "sha256-v1",
                "digestByteLength": 32,
                "digestEncoding": "lowercase-hex-no-prefix"
              }
            }
          ]
        }
        """;
  }
}
