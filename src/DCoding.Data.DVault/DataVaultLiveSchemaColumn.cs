namespace DCoding.Data.DVault;

/// <summary>
/// Describes one live Data Vault table column.
/// </summary>
/// <param name="ColumnName">The physical column name.</param>
/// <param name="Ordinal">The zero-based physical column ordinal.</param>
/// <param name="ProviderStorageType">The provider storage type reported by the live database catalog.</param>
public sealed record DataVaultLiveSchemaColumn(
    string ColumnName,
    int Ordinal,
    string ProviderStorageType);
