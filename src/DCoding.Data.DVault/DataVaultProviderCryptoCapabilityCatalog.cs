namespace DCoding.Data.DVault;

internal static class DataVaultProviderCryptoCapabilityCatalog {
  private const string CapabilityKindDeploymentAtRest = "deployment-at-rest";
  private const string CapabilityKindDriverMediated = "driver-mediated";
  private const string CapabilityKindEncryptedFile = "encrypted-file";
  private const string CapabilityKindSqlFunction = "sql-function";
  private const string StatusConditional = "conditional";
  private const string StatusUnsupported = "unsupported";

  private static readonly IReadOnlyDictionary<string, IReadOnlyList<CapabilityTemplate>> TemplatesByProfileName =
      new Dictionary<string, IReadOnlyList<CapabilityTemplate>>(StringComparer.Ordinal) {
        [DataVaultProviderCapabilityProfiles.Sqlite.ProfileName] =
        [
            new(
                "encrypted-file",
                "SQLite encrypted-file build",
                CapabilityKindEncryptedFile,
                StatusUnsupported,
                "DVault's built-in SQLite package does not configure or probe encrypted SQLite builds; applications must choose and validate any encrypted-file provider or extension outside DVault."),
        ],
        [DataVaultProviderCapabilityProfiles.Postgres.ProfileName] =
        [
            new(
                "deployment-encryption",
                "PostgreSQL deployment encryption posture",
                CapabilityKindDeploymentAtRest,
                StatusConditional,
                "PostgreSQL at-rest protection is deployment-owned storage, volume, managed-service, or operating-system encryption; DVault reports this as guidance only and does not verify activation."),
            new(
                "pgcrypto",
                "pgcrypto",
                CapabilityKindSqlFunction,
                StatusConditional,
                "pgcrypto requires an application-managed extension, SQL usage, and key-handling design; DVault does not emit pgcrypto calls or inspect extension availability."),
        ],
        [DataVaultProviderCapabilityProfiles.SqlServer.ProfileName] =
        [
            new(
                "transparent-data-encryption",
                "Transparent Data Encryption",
                CapabilityKindDeploymentAtRest,
                StatusConditional,
                "SQL Server TDE is database and operations configuration outside DVault; diagnostics do not verify database encryption state."),
            new(
                "always-encrypted",
                "Always Encrypted",
                CapabilityKindDriverMediated,
                StatusConditional,
                "SQL Server Always Encrypted depends on driver, column, enclave, and key-store configuration owned by the application and database estate; DVault does not route runtime behavior to it."),
        ],
        [DataVaultProviderCapabilityProfiles.MySql.ProfileName] =
        [
            new(
                "sql-crypto-functions",
                "MySQL SQL crypto functions",
                CapabilityKindSqlFunction,
                StatusConditional,
                "MySQL SQL crypto requires explicit application SQL and key-handling review; DVault does not generate SQL crypto calls or probe function availability."),
            new(
                "file-or-tablespace-encryption",
                "MySQL file or tablespace encryption",
                CapabilityKindDeploymentAtRest,
                StatusConditional,
                "MySQL file or tablespace encryption is server, storage-engine, and keyring configuration outside DVault; diagnostics do not verify activation."),
        ],
        [DataVaultProviderCapabilityProfiles.Oracle.ProfileName] =
        [
            new(
                "transparent-data-encryption",
                "Transparent Data Encryption",
                CapabilityKindDeploymentAtRest,
                StatusConditional,
                "Oracle TDE is database and wallet or key-management configuration outside DVault; diagnostics do not verify encryption state."),
            new(
                "dbms_crypto",
                "DBMS_CRYPTO",
                CapabilityKindSqlFunction,
                StatusConditional,
                "DBMS_CRYPTO requires explicit application SQL or PL/SQL and key-handling review; DVault does not generate DBMS_CRYPTO calls or probe package privileges."),
        ],
        [DataVaultProviderCapabilityProfiles.Db2.ProfileName] =
        [
            new(
                "native-database-encryption",
                "DB2 native database encryption",
                CapabilityKindDeploymentAtRest,
                StatusConditional,
                "DB2 native database encryption is database and key-management configuration outside DVault; diagnostics do not verify activation or key-store state."),
        ],
      };

  public static IReadOnlyList<DataVaultProviderCryptoCapabilityFact> SelectReviewedCapabilities(
      string? providerName,
      string capabilityProfileName,
      bool capabilityProfileDefaulted) {
    ArgumentException.ThrowIfNullOrWhiteSpace(capabilityProfileName);

    if (capabilityProfileDefaulted) {
      return Array.Empty<DataVaultProviderCryptoCapabilityFact>();
    }

    if (!string.IsNullOrWhiteSpace(providerName) &&
        (!DataVaultProviderCapabilityProfileSelection.TrySelectRegistered(providerName, out var selectedProfile) ||
            selectedProfile is null ||
            !string.Equals(selectedProfile.ProfileName, capabilityProfileName, StringComparison.Ordinal))) {
      return Array.Empty<DataVaultProviderCryptoCapabilityFact>();
    }

    if (!TemplatesByProfileName.TryGetValue(capabilityProfileName, out var templates)) {
      return Array.Empty<DataVaultProviderCryptoCapabilityFact>();
    }

    return templates
        .Select(template => template.Create(providerName, capabilityProfileName))
        .ToArray();
  }

  private sealed record CapabilityTemplate(
      string CapabilityFamily,
      string CapabilityLabel,
      string CapabilityKind,
      string Status,
      string Guidance) {
    public DataVaultProviderCryptoCapabilityFact Create(
        string? providerName,
        string capabilityProfileName) {
      return new DataVaultProviderCryptoCapabilityFact(
          providerName,
          capabilityProfileName,
          CapabilityFamily,
          CapabilityLabel,
          CapabilityKind,
          Status,
          Guidance);
    }
  }
}
