using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class ExternalProviderLiveSchemaModelOptions {
  private ExternalProviderLiveSchemaModelOptions(
      DataVaultProviderCapabilityProfile providerCapabilities,
      string? defaultSchema,
      string tableNamePrefix,
      IReadOnlyDictionary<string, string> tableNameOverrides,
      IReadOnlyDictionary<string, string> identifierNameOverrides) {
    ProviderCapabilities = providerCapabilities;
    DefaultSchema = defaultSchema;
    TableNamePrefix = tableNamePrefix;
    TableNameOverrides = tableNameOverrides;
    IdentifierNameOverrides = identifierNameOverrides;
    ExpectedSnapshot = LiveSchemaReaderContractFixture.CreateExpectedSnapshot(
        providerCapabilities,
        ResolveTableName,
        ResolveIdentifierName);
  }

  public DataVaultProviderCapabilityProfile ProviderCapabilities { get; }

  public string? DefaultSchema { get; }

  public string TableNamePrefix { get; }

  public IReadOnlyDictionary<string, string> TableNameOverrides { get; }

  public IReadOnlyDictionary<string, string> IdentifierNameOverrides { get; }

  public DataVaultLiveSchemaSnapshot ExpectedSnapshot { get; }

  public static ExternalProviderLiveSchemaModelOptions ForPostgres(string schemaName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(schemaName);

    return new ExternalProviderLiveSchemaModelOptions(
        DataVaultProviderCapabilityProfiles.Postgres,
        schemaName,
        tableNamePrefix: string.Empty,
        tableNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal),
        identifierNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal));
  }

  public static ExternalProviderLiveSchemaModelOptions ForSqlServer(string schemaName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(schemaName);

    return new ExternalProviderLiveSchemaModelOptions(
        DataVaultProviderCapabilityProfiles.SqlServer,
        schemaName,
        tableNamePrefix: string.Empty,
        tableNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal),
        identifierNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal));
  }

  public static ExternalProviderLiveSchemaModelOptions ForMySql(string tableNamePrefix) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableNamePrefix);

    return new ExternalProviderLiveSchemaModelOptions(
        DataVaultProviderCapabilityProfiles.MySql,
        defaultSchema: null,
        tableNamePrefix,
        tableNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal),
        identifierNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal));
  }

  public static ExternalProviderLiveSchemaModelOptions ForDb2(string suffix) {
    ArgumentException.ThrowIfNullOrWhiteSpace(suffix);

    var tableNameOverrides = new Dictionary<string, string>(StringComparer.Ordinal) {
      ["HubCustomer"] = "DVHCU" + suffix,
      ["HubOrder"] = "DVHOR" + suffix,
      ["LinkCustomerOrder"] = "DVLCO" + suffix,
      ["SatCustomerContact"] = "DVSCC" + suffix,
      ["SatCustomerOrderState"] = "DVSCOS" + suffix,
    };
    var identifierNameOverrides = new Dictionary<string, string>(StringComparer.Ordinal) {
      ["PkHubCustomerCustomerHashKey"] = "DPKHC" + suffix,
      ["IxHubCustomerBusinessKeyCustomerId"] = "DIXHCBK" + suffix,
      ["PkHubOrderOrderHashKey"] = "DPKHO" + suffix,
      ["IxHubOrderBusinessKeyOrderId"] = "DIXHOBK" + suffix,
      ["PkLinkCustomerOrderCustomerOrderHashKey"] = "DPKCO" + suffix,
      ["IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey"] = "DIXCOREL" + suffix,
      ["PkSatCustomerContactCustomerHashKeyLoadTimestamp"] = "DPKSCC" + suffix,
      ["IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp"] = "DIXSCCP" + suffix,
      ["PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp"] = "DPKSCOS" + suffix,
      ["IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp"] = "DIXSCOSP" + suffix,
    };

    return new ExternalProviderLiveSchemaModelOptions(
        DataVaultProviderCapabilityProfiles.Db2,
        defaultSchema: null,
        tableNamePrefix: string.Empty,
        tableNameOverrides,
        identifierNameOverrides);
  }

  public static ExternalProviderLiveSchemaModelOptions ForOracle(string suffix) {
    ArgumentException.ThrowIfNullOrWhiteSpace(suffix);

    var tableNameOverrides = new Dictionary<string, string>(StringComparer.Ordinal) {
      ["HubCustomer"] = "DVHCU" + suffix,
      ["HubOrder"] = "DVHOR" + suffix,
      ["LinkCustomerOrder"] = "DVLCO" + suffix,
      ["SatCustomerContact"] = "DVSCC" + suffix,
      ["SatCustomerOrderState"] = "DVSCOS" + suffix,
    };
    var identifierNameOverrides = new Dictionary<string, string>(StringComparer.Ordinal) {
      ["PkHubCustomerCustomerHashKey"] = "DPKHC" + suffix,
      ["IxHubCustomerBusinessKeyCustomerId"] = "DIXHCBK" + suffix,
      ["PkHubOrderOrderHashKey"] = "DPKHO" + suffix,
      ["IxHubOrderBusinessKeyOrderId"] = "DIXHOBK" + suffix,
      ["PkLinkCustomerOrderCustomerOrderHashKey"] = "DPKCO" + suffix,
      ["IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey"] = "DIXCOREL" + suffix,
      ["PkSatCustomerContactCustomerHashKeyLoadTimestamp"] = "DPKSCC" + suffix,
      ["IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp"] = "DIXSCCP" + suffix,
      ["PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp"] = "DPKSCOS" + suffix,
      ["IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp"] = "DIXSCOSP" + suffix,
    };

    return new ExternalProviderLiveSchemaModelOptions(
        DataVaultProviderCapabilityProfiles.Oracle,
        defaultSchema: null,
        tableNamePrefix: string.Empty,
        tableNameOverrides,
        identifierNameOverrides);
  }

  public string ResolveTableName(string producedName) {
    return TableNameOverrides.TryGetValue(producedName, out var tableName)
        ? tableName
        : TableNamePrefix + producedName;
  }

  public string ResolveIdentifierName(string producedName) {
    return IdentifierNameOverrides.TryGetValue(producedName, out var identifierName)
        ? identifierName
        : LiveSchemaReaderContractFixture.ResolvePhysicalIdentifierName(producedName, ProviderCapabilities);
  }
}
