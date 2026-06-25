using DCoding.Data.DVault.Modeling;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultModelArtifactParserTests {
  [Fact]
  public void ValidMinimalArtifactDefaultsOptionalSectionsAndBuildsRegistry() {
    var result = DataVaultModelArtifactParser.Parse(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            {
              "name": "Customer",
              "businessKeys": ["TenantId", "CustomerNumber"]
            }
          ]
        }
        """);

    AssertValid(result);
    Assert.Equal(DataVaultLoadTimestampStorage.ProviderDefault, result.LoadTimestampStorage);
    Assert.Empty(result.Artifact!.Links);
    Assert.Empty(result.Artifact.Satellites);
    Assert.Empty(result.Artifact.Pits);
    Assert.Empty(result.Artifact.Bridges);
    Assert.Single(result.MetadataModel!.Hubs);
    Assert.NotNull(result.MetadataRegistry);
    Assert.True(result.MetadataRegistry!.TryGetHub("Customer", out var hub));
    Assert.Equal(["TenantId", "CustomerNumber"], hub!.BusinessKeyNames);
  }

  [Fact]
  public void ValidFullArtifactPreservesDeclarationOrderAndRepresentableMetadata() {
    var result = DataVaultModelArtifactParser.Parse(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "loadTimestampStorage": "iso-8601-utc-text",
          "hubs": [
            {
              "name": "Customer",
              "businessKeys": ["TenantId", "CustomerNumber"]
            },
            {
              "name": "Order",
              "businessKeys": ["OrderNumber"]
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
              "name": "CustomerProfile",
              "parent": {
                "kind": "hub",
                "name": "Customer"
              },
              "payload": ["Name", "EmailAddress"]
            },
            {
              "name": "CustomerContactByType",
              "parent": {
                "kind": "hub",
                "name": "Customer"
              },
              "drivingKeys": ["ContactType", "SourceSystem"],
              "payload": ["ContactValue", "VerifiedAt"]
            }
          ],
          "pits": [
            {
              "name": "CustomerPit",
              "hub": "Customer",
              "satellites": ["CustomerProfile", "CustomerContactByType"]
            }
          ],
          "bridges": [
            {
              "name": "CustomerOrderBridge",
              "kind": "many-to-many",
              "source": "CustomerOrder",
              "endpoints": {
                "from": {
                  "hub": "Customer"
                },
                "to": {
                  "hub": "Order"
                }
              }
            }
          ]
        }
        """);

    AssertValid(result);
    Assert.Equal(DataVaultLoadTimestampStorage.Iso8601UtcText, result.LoadTimestampStorage);
    Assert.Equal(["Customer", "Order"], result.MetadataModel!.Hubs.Select(hub => hub.Name));
    Assert.Equal(["CustomerOrder"], result.MetadataModel.Links.Select(link => link.Name));
    Assert.Equal(["CustomerProfile", "CustomerContactByType"], result.MetadataModel.Satellites.Select(satellite => satellite.Name));
    Assert.Empty(result.MetadataModel.PointInTimeTables);
    Assert.Equal(["CustomerPit"], result.MetadataModel.Pits.Select(pit => pit.Name));
    Assert.Equal(["CustomerOrderBridge"], result.MetadataModel.Bridges.Select(bridge => bridge.Name));
    Assert.Equal(["ContactType", "SourceSystem"], result.MetadataModel.Satellites[1].DrivingKeyNames);
    Assert.True(result.MetadataRegistry!.TryGetProviderCapabilityProfile("sqlite-v1-loadts-iso8601", out var providerProfile));
    Assert.Equal(
        DataVaultProviderValueFormat.Iso8601UtcText,
        providerProfile!.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.LoadTimestamp).ValueFormat);
  }

  [Fact]
  public void ValidHierarchyArtifactPreservesRoleBearingRecursiveDeclarations() {
    var result = DataVaultModelArtifactParser.Parse(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "loadTimestampStorage": "utc-ticks",
          "hubs": [
            {
              "name": "SalesRegion",
              "businessKeys": ["RegionCode"]
            }
          ],
          "links": [
            {
              "name": "SalesRegionParentChild",
              "participants": [
                {
                  "hub": "SalesRegion",
                  "role": "ParentRegion"
                },
                {
                  "hub": "SalesRegion",
                  "role": "ChildRegion"
                }
              ]
            }
          ],
          "bridges": [
            {
              "name": "SalesRegionHierarchyBridge",
              "kind": "hierarchy",
              "source": "SalesRegionParentChild",
              "endpoints": {
                "ancestor": {
                  "hub": "SalesRegion",
                  "role": "ParentRegion"
                },
                "descendant": {
                  "hub": "SalesRegion",
                  "role": "ChildRegion"
                }
              }
            }
          ]
        }
        """);

    AssertValid(result);
    Assert.Equal(DataVaultLoadTimestampStorage.UtcTicks, result.LoadTimestampStorage);
    Assert.Equal(
        ["ParentRegion", "ChildRegion"],
        result.Artifact!.Links.Single().Participants.Select(participant => participant.Role));
    Assert.Equal(["SalesRegion", "SalesRegion"], result.MetadataModel!.Links.Single().Participants.Select(participant => participant.HubReference.Name));
    Assert.Equal(DataVaultBridgeKind.Hierarchy, result.MetadataModel.Bridges.Single().Kind);
  }

  [Fact]
  public void ValidSatellitePersonalDataProjectsToRuntimeMetadata() {
    var result = DataVaultModelArtifactParser.Parse(
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
              "name": "CustomerProfile",
              "parent": {
                "kind": "hub",
                "name": "Customer"
              },
              "payload": ["Name", "EmailAddress", "PhoneNumber"],
              "personalData": [
                {
                  "field": "EmailAddress",
                  "encryptedPayloadAlias": "CustomerProfileEmailEncrypted"
                },
                {
                  "field": "PhoneNumber",
                  "encryptedPayloadAlias": "CustomerProfilePhoneEncrypted"
                }
              ]
            }
          ]
        }
        """);

    AssertValid(result);

    var artifactSatellite = Assert.Single(result.Artifact!.Satellites);
    Assert.Equal(["EmailAddress", "PhoneNumber"], artifactSatellite.PersonalData.Select(personalData => personalData.Field));

    var satellite = Assert.Single(result.MetadataModel!.Satellites);
    Assert.Equal(
        ["CustomerProfileEmailEncrypted", "CustomerProfilePhoneEncrypted"],
        satellite.PersonalDataFields.Select(personalData => personalData.EncryptedPayloadAlias));
  }

  [Fact]
  public void RejectsSchemaVersionProblemsWithStableDiagnostics() {
    AssertInvalid(
        """
        {
          "hubs": []
        }
        """,
        "schema-version",
        "DMV1001",
        "/schemaVersion");
    AssertInvalid(
        """
        {
          "schemaVersion": 1
        }
        """,
        "schema-version",
        "DMV1001",
        "/schemaVersion");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v2"
        }
        """,
        "schema-version",
        "DMV1002",
        "/schemaVersion");
  }

  [Fact]
  public void RejectsUnknownAndProviderSpecificFields() {
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "unexpected": true
        }
        """,
        "shape",
        "DMV1101",
        "/unexpected");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "providers": {}
        }
        """,
        "provider-choice",
        "DMV1502",
        "/providers");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            {
              "name": "Customer",
              "businessKeys": ["CustomerNumber"],
              "unknown": "value"
            }
          ]
        }
        """,
        "shape",
        "DMV1101",
        "/hubs/0/unknown");
  }

  [Fact]
  public void RejectsReferencesAndDuplicatesWithoutProducingMetadata() {
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Customer", "businessKeys": ["CustomerNumber"] },
            { "name": "Customer", "businessKeys": ["TenantId"] }
          ]
        }
        """,
        "duplicate",
        "DMV1201",
        "/hubs/1/name");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Customer", "businessKeys": ["CustomerNumber"] }
          ],
          "links": [
            {
              "name": "CustomerOrder",
              "participants": [
                { "hub": "Customer" },
                { "hub": "Order" }
              ]
            }
          ]
        }
        """,
        "reference",
        "DMV1301",
        "/links/0/participants/1/hub");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Customer", "businessKeys": ["CustomerNumber"] }
          ],
          "links": [
            {
              "name": "CustomerOrder",
              "participants": [
                { "hub": "Customer" },
                { "hub": "Customer", "role": "Buyer" }
              ]
            }
          ]
        }
        """,
        "recursive-participant-binding",
        "DMV1602",
        "/links/0/participants");
  }

  [Fact]
  public void RejectsUnsupportedCapabilitiesAndSemanticConflicts() {
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "loadTimestampStorage": "native-date-time"
        }
        """,
        "provider-choice",
        "DMV1502",
        "/loadTimestampStorage");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Customer", "businessKeys": ["CustomerNumber"] }
          ],
          "satellites": [
            {
              "name": "CustomerContact",
              "parent": { "kind": "hub", "name": "Customer" },
              "drivingKeys": ["ContactType"],
              "payload": ["ContactType", "Value"]
            }
          ]
        }
        """,
        "shape",
        "DMV1701",
        "/satellites/0/drivingKeys");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Customer", "businessKeys": ["CustomerNumber"] }
          ],
          "links": [
            {
              "name": "CustomerParent",
              "participants": [
                { "hub": "Customer", "role": "Parent" },
                { "hub": "Customer", "role": "Child" }
              ]
            }
          ],
          "bridges": [
            {
              "name": "CustomerBridge",
              "kind": "many-to-many",
              "source": "CustomerParent",
              "endpoints": {
                "from": { "hub": "Customer" },
                "to": { "hub": "Customer", "role": "Child" }
              }
            }
          ]
        }
        """,
        "recursive-participant-binding",
        "DMV1601",
        "/bridges/0/endpoints/from");
  }

  [Fact]
  public void RejectsInvalidSatellitePersonalDataDeclarations() {
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Customer", "businessKeys": ["CustomerNumber"] }
          ],
          "satellites": [
            {
              "name": "CustomerProfile",
              "parent": { "kind": "hub", "name": "Customer" },
              "payload": ["EmailAddress"],
              "personalData": [
                { "field": "EmailAddress", "encryptedPayloadAlias": "CustomerProfileEmailEncrypted" },
                { "field": "EmailAddress", "encryptedPayloadAlias": "CustomerProfileEmailEncrypted2" }
              ]
            }
          ]
        }
        """,
        "privacy-metadata",
        "DMV1802",
        "/satellites/0/personalData/1/field");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Customer", "businessKeys": ["CustomerNumber"] }
          ],
          "satellites": [
            {
              "name": "CustomerContact",
              "parent": { "kind": "hub", "name": "Customer" },
              "drivingKeys": ["ContactType"],
              "payload": ["ContactValue"],
              "personalData": [
                { "field": "ContactType", "encryptedPayloadAlias": "CustomerContactEncrypted" }
              ]
            }
          ]
        }
        """,
        "privacy-metadata",
        "DMV1803",
        "/satellites/0/personalData/0/field");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Customer", "businessKeys": ["CustomerNumber"] }
          ],
          "satellites": [
            {
              "name": "CustomerProfile",
              "parent": { "kind": "hub", "name": "Customer" },
              "payload": ["EmailAddress"],
              "personalData": [
                {
                  "field": "EmailAddress",
                  "encryptedPayloadAlias": "CustomerProfileEmailEncrypted",
                  "sql": "encrypt"
                }
              ]
            }
          ]
        }
        """,
        "provider-choice",
        "DMV1502",
        "/satellites/0/personalData/0/sql");
  }

  [Fact]
  public void RejectsPitParentMismatchAndNamingCollisions() {
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Customer", "businessKeys": ["CustomerNumber"] },
            { "name": "Order", "businessKeys": ["OrderNumber"] }
          ],
          "satellites": [
            {
              "name": "OrderStatus",
              "parent": { "kind": "hub", "name": "Order" },
              "payload": ["Status"]
            }
          ],
          "pits": [
            {
              "name": "CustomerPit",
              "hub": "Customer",
              "satellites": ["OrderStatus"]
            }
          ]
        }
        """,
        "reference",
        "DMV1303",
        "/pits/0/satellites");
    AssertInvalid(
        """
        {
          "schemaVersion": "dvault.model.v1",
          "hubs": [
            { "name": "Order", "businessKeys": ["OrderNumber"] },
            { "name": "Orders", "businessKeys": ["OrdersNumber"] }
          ]
        }
        """,
        "naming",
        "DMV1401",
        "/hubs/1/name");
  }

  private static void AssertValid(DataVaultModelArtifactParseResult result) {
    Assert.True(result.IsValid, FormatDiagnostics(result));
    Assert.Empty(result.Diagnostics);
    Assert.NotNull(result.Artifact);
    Assert.NotNull(result.MetadataModel);
    Assert.NotNull(result.MetadataRegistry);
  }

  private static void AssertInvalid(
      string json,
      string category,
      string code,
      string path) {
    var result = DataVaultModelArtifactParser.Parse(json);

    Assert.False(result.IsValid);
    Assert.Null(result.Artifact);
    Assert.Null(result.MetadataModel);
    Assert.Null(result.MetadataRegistry);
    Assert.Contains(
        result.Diagnostics,
        diagnostic => diagnostic.Category == category &&
            diagnostic.Code == code &&
            diagnostic.Path == path);
  }

  private static string FormatDiagnostics(DataVaultModelArtifactParseResult result) {
    return string.Join(
        Environment.NewLine,
        result.Diagnostics.Select(diagnostic =>
            diagnostic.Severity + " " + diagnostic.Category + " " + diagnostic.Code + " " + diagnostic.Path + ": " + diagnostic.Message));
  }
}
