namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the closed v1 Data Vault technical metadata column roles.
/// </summary>
public enum TechnicalMetadataColumnRole
{
    /// <summary>
    /// Stable hash key used for Data Vault keying and joins.
    /// </summary>
    HashKey,

    /// <summary>
    /// Hash diff used for satellite change detection.
    /// </summary>
    HashDiff,

    /// <summary>
    /// Timestamp recording when a row was loaded into the vault.
    /// </summary>
    LoadTimestamp,

    /// <summary>
    /// Lineage value identifying the originating source system, feed, or batch.
    /// </summary>
    RecordSource,
}