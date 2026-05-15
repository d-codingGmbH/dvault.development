using System.Security.Cryptography;
using System.Text;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault.Tests.Shared;

public static class LiveSchemaReaderContractFixture {
  public const string CustomerHubName = "Customer";
  public const string OrderHubName = "Order";
  public const string CustomerOrderLinkName = "CustomerOrder";
  public const string ContactSatelliteName = "Contact";
  public const string StateSatelliteName = "State";

  public static readonly IReadOnlyList<string> ProducedTableNames =
  [
      "HubCustomer",
      "HubOrder",
      "LinkCustomerOrder",
      "SatCustomerContact",
      "SatCustomerOrderState",
  ];

  public static DataVaultMetadataModel CreateCanonicalMetadataModel() {
    return new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata(CustomerHubName, ["Customer Id"]),
            new DataVaultHubMetadata(OrderHubName, ["Order Id"]),
        ],
        [
            new DataVaultLinkMetadata(
                CustomerOrderLinkName,
                [
                    DataVaultMetadataReference.Hub(CustomerHubName),
                    DataVaultMetadataReference.Hub(OrderHubName),
                ]),
        ],
        [
            new DataVaultSatelliteMetadata(
                ContactSatelliteName,
                DataVaultMetadataReference.Hub(CustomerHubName),
                ["Email Address"]),
            new DataVaultSatelliteMetadata(
                StateSatelliteName,
                DataVaultMetadataReference.Link(CustomerOrderLinkName),
                ["State Code"]),
        ]);
  }

  public static DataVaultMetadataModel CreateCustomerOnlyMetadataModel() {
    return new DataVaultMetadataModel([new DataVaultHubMetadata(CustomerHubName, ["Customer Id"])], [], []);
  }

  public static DataVaultLiveSchemaSnapshot CreateExpectedSnapshot(
      DataVaultProviderCapabilityProfile providerCapabilities,
      Func<string, string>? tableNameResolver = null,
      Func<string, string>? identifierNameResolver = null) {
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    tableNameResolver ??= static producedName => producedName;
    identifierNameResolver ??= static producedName => producedName;

    var hashKeyStoreType = GetStoreType(providerCapabilities, DataVaultLogicalPropertyKind.HashKey);
    var hashDiffStoreType = GetStoreType(providerCapabilities, DataVaultLogicalPropertyKind.HashDiff);
    var loadTimestampStoreType = GetStoreType(providerCapabilities, DataVaultLogicalPropertyKind.LoadTimestamp);
    var recordSourceStoreType = GetStoreType(providerCapabilities, DataVaultLogicalPropertyKind.RecordSource);
    var participantReferenceStoreType = GetStoreType(providerCapabilities, DataVaultLogicalPropertyKind.ParticipantReference);
    var businessKeyStoreType = GetStoreType(providerCapabilities, DataVaultLogicalPropertyKind.BusinessKey);
    var payloadStoreType = GetStoreType(providerCapabilities, DataVaultLogicalPropertyKind.PayloadText);

    return new DataVaultLiveSchemaSnapshot(
        [
            new DataVaultLiveSchemaTable(
                tableNameResolver("HubCustomer"),
                [
                    new DataVaultLiveSchemaColumn("CustomerHashKey", 0, hashKeyStoreType),
                    new DataVaultLiveSchemaColumn("LoadTimestamp", 1, loadTimestampStoreType),
                    new DataVaultLiveSchemaColumn("RecordSource", 2, recordSourceStoreType),
                    new DataVaultLiveSchemaColumn("CustomerId", 3, businessKeyStoreType),
                ],
                new DataVaultLiveSchemaPrimaryKey(
                    identifierNameResolver("PkHubCustomerCustomerHashKey"),
                    ["CustomerHashKey"]),
                [
                    new DataVaultLiveSchemaIndex(
                        identifierNameResolver("IxHubCustomerBusinessKeyCustomerId"),
                        ["CustomerId"],
                        isUnique: true),
                ]),
            new DataVaultLiveSchemaTable(
                tableNameResolver("HubOrder"),
                [
                    new DataVaultLiveSchemaColumn("OrderHashKey", 0, hashKeyStoreType),
                    new DataVaultLiveSchemaColumn("LoadTimestamp", 1, loadTimestampStoreType),
                    new DataVaultLiveSchemaColumn("RecordSource", 2, recordSourceStoreType),
                    new DataVaultLiveSchemaColumn("OrderId", 3, businessKeyStoreType),
                ],
                new DataVaultLiveSchemaPrimaryKey(
                    identifierNameResolver("PkHubOrderOrderHashKey"),
                    ["OrderHashKey"]),
                [
                    new DataVaultLiveSchemaIndex(
                        identifierNameResolver("IxHubOrderBusinessKeyOrderId"),
                        ["OrderId"],
                        isUnique: true),
                ]),
            new DataVaultLiveSchemaTable(
                tableNameResolver("LinkCustomerOrder"),
                [
                    new DataVaultLiveSchemaColumn("CustomerOrderHashKey", 0, hashKeyStoreType),
                    new DataVaultLiveSchemaColumn("LoadTimestamp", 1, loadTimestampStoreType),
                    new DataVaultLiveSchemaColumn("RecordSource", 2, recordSourceStoreType),
                    new DataVaultLiveSchemaColumn("CustomerHashKey", 3, participantReferenceStoreType),
                    new DataVaultLiveSchemaColumn("OrderHashKey", 4, participantReferenceStoreType),
                ],
                new DataVaultLiveSchemaPrimaryKey(
                    identifierNameResolver("PkLinkCustomerOrderCustomerOrderHashKey"),
                    ["CustomerOrderHashKey"]),
                [
                    new DataVaultLiveSchemaIndex(
                        identifierNameResolver("IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey"),
                        ["CustomerHashKey", "OrderHashKey"],
                        isUnique: false),
                ]),
            new DataVaultLiveSchemaTable(
                tableNameResolver("SatCustomerContact"),
                [
                    new DataVaultLiveSchemaColumn("CustomerHashKey", 0, hashKeyStoreType),
                    new DataVaultLiveSchemaColumn("HashDiff", 1, hashDiffStoreType),
                    new DataVaultLiveSchemaColumn("LoadTimestamp", 2, loadTimestampStoreType),
                    new DataVaultLiveSchemaColumn("RecordSource", 3, recordSourceStoreType),
                    new DataVaultLiveSchemaColumn("EmailAddress", 4, payloadStoreType),
                ],
                new DataVaultLiveSchemaPrimaryKey(
                    identifierNameResolver("PkSatCustomerContactCustomerHashKeyLoadTimestamp"),
                    ["CustomerHashKey", "LoadTimestamp"]),
                [
                    new DataVaultLiveSchemaIndex(
                        identifierNameResolver("IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp"),
                        GetSatelliteParentIndexColumns("CustomerHashKey", "LoadTimestamp", "HashDiff", providerCapabilities),
                        isUnique: false),
                ]),
            new DataVaultLiveSchemaTable(
                tableNameResolver("SatCustomerOrderState"),
                [
                    new DataVaultLiveSchemaColumn("CustomerOrderHashKey", 0, hashKeyStoreType),
                    new DataVaultLiveSchemaColumn("HashDiff", 1, hashDiffStoreType),
                    new DataVaultLiveSchemaColumn("LoadTimestamp", 2, loadTimestampStoreType),
                    new DataVaultLiveSchemaColumn("RecordSource", 3, recordSourceStoreType),
                    new DataVaultLiveSchemaColumn("StateCode", 4, payloadStoreType),
                ],
                new DataVaultLiveSchemaPrimaryKey(
                    identifierNameResolver("PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp"),
                    ["CustomerOrderHashKey", "LoadTimestamp"]),
                [
                    new DataVaultLiveSchemaIndex(
                        identifierNameResolver(
                            "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp"),
                        GetSatelliteParentIndexColumns(
                            "CustomerOrderHashKey",
                            "LoadTimestamp",
                            "HashDiff",
                            providerCapabilities),
                        isUnique: false),
                ]),
        ]);
  }

  public static IReadOnlyList<string> CreateSnapshotSignatures(DataVaultLiveSchemaSnapshot snapshot) {
    ArgumentNullException.ThrowIfNull(snapshot);

    var signatures = new List<string>();
    foreach (var table in snapshot.Tables) {
      signatures.Add("table:" + table.TableName);
      signatures.Add(
          "columns:" +
          table.TableName +
          ":" +
          string.Join(
              "|",
              table.Columns.Select(column =>
                  column.Ordinal + ":" + column.ColumnName + ":" + column.ProviderStorageType)));
      signatures.Add(
          "primary-key:" +
          table.TableName +
          ":" +
          table.PrimaryKey.ConstraintName +
          ":" +
          string.Join("|", table.PrimaryKey.ColumnNames));

      foreach (var index in table.Indexes) {
        signatures.Add(
            "index:" +
            table.TableName +
            ":" +
            index.IndexName +
            ":" +
            index.IsUnique +
            ":" +
            string.Join("|", index.ColumnNames));
      }
    }

    return signatures;
  }

  public static string ResolvePhysicalIdentifierName(
      string producedName,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentException.ThrowIfNullOrWhiteSpace(producedName);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    if (providerCapabilities.MaximumIdentifierLength is not { } maximumIdentifierLength ||
        producedName.Length <= maximumIdentifierLength) {
      return producedName;
    }

    const int hashLength = 8;
    var prefixLength = Math.Max(1, maximumIdentifierLength - hashLength - 1);
    var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(producedName)))
        .ToLowerInvariant()[..hashLength];

    return producedName[..prefixLength] + "_" + hash;
  }

  private static string GetStoreType(
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultLogicalPropertyKind logicalPropertyKind) {
    return providerCapabilities.GetRequiredTypeMapping(logicalPropertyKind).NativeStoreType;
  }

  private static IReadOnlyList<string> GetSatelliteParentIndexColumns(
      string parentHashKeyColumnName,
      string loadTimestampColumnName,
      string hashDiffColumnName,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    if (SupportsIncludedIndexProperties(providerCapabilities) ||
        providerCapabilities.UnsupportedIncludedIndexColumnMode == DataVaultUnsupportedIncludedIndexColumnMode.Ignore) {
      return [parentHashKeyColumnName, loadTimestampColumnName];
    }

    return [parentHashKeyColumnName, loadTimestampColumnName, hashDiffColumnName];
  }

  private static bool SupportsIncludedIndexProperties(DataVaultProviderCapabilityProfile providerCapabilities) {
    return providerCapabilities.ProfileName.StartsWith("sqlserver-", StringComparison.Ordinal) ||
        providerCapabilities.ProfileName.StartsWith("postgres-", StringComparison.Ordinal);
  }
}
