using System.Collections.ObjectModel;

namespace DVault.Modeling;

/// <summary>
/// Captures the provider-neutral v1 Data Vault defaults used when callers do not supply model configuration.
/// </summary>
public sealed class DataVaultConventions
{
    private const string DefaultStableHashAlgorithmId = "sha256-v1";
    private const string DefaultPersistenceContentHashAlgorithm = "sha-256";
    private const string DefaultPersistenceConventionVersion = "dvault.persistence-conventions.v1";

    private static readonly DataVaultModelConcept[] DefaultModelConcepts =
    [
        DataVaultModelConcept.Hub,
        DataVaultModelConcept.Link,
        DataVaultModelConcept.Satellite,
        DataVaultModelConcept.HashKey,
        DataVaultModelConcept.HashDiff,
        DataVaultModelConcept.LoadTimestamp,
        DataVaultModelConcept.RecordSource,
    ];

    private static readonly string[] DefaultLogicalObjectNames =
    [
        "dvault_records",
        "dvault_record_payloads",
        "dvault_record_metadata",
    ];

    private DataVaultConventions(
        DefaultNamingPolicy namingPolicy,
        IReadOnlyList<DataVaultModelConcept> modelConcepts,
        string stableHashAlgorithmId,
        string persistenceContentHashAlgorithm,
        string persistenceConventionVersion,
        IReadOnlyList<string> logicalObjectNames)
    {
        NamingPolicy = namingPolicy;
        ModelConcepts = modelConcepts;
        StableHashAlgorithmId = stableHashAlgorithmId;
        PersistenceContentHashAlgorithm = persistenceContentHashAlgorithm;
        PersistenceConventionVersion = persistenceConventionVersion;
        LogicalObjectNames = logicalObjectNames;
    }

    /// <summary>
    /// Gets the shared deterministic v1 convention set used by AddDVault and UseDataVault.
    /// </summary>
    public static DataVaultConventions Default { get; } = new(
        DefaultNamingPolicy.Instance,
        new ReadOnlyCollection<DataVaultModelConcept>(DefaultModelConcepts),
        DefaultStableHashAlgorithmId,
        DefaultPersistenceContentHashAlgorithm,
        DefaultPersistenceConventionVersion,
        new ReadOnlyCollection<string>(DefaultLogicalObjectNames));

    /// <summary>
    /// Gets the default table and column naming policy for hubs, links, satellites, and technical columns.
    /// </summary>
    public DefaultNamingPolicy NamingPolicy { get; }

    /// <summary>
    /// Gets the MVP Data Vault concept vocabulary supported by the optionless model configuration path.
    /// </summary>
    public IReadOnlyList<DataVaultModelConcept> ModelConcepts { get; }

    /// <summary>
    /// Gets the stable hashing service algorithm identifier reserved for the default hash service boundary.
    /// </summary>
    public string StableHashAlgorithmId { get; }

    /// <summary>
    /// Gets the default logical persistence content hash algorithm value.
    /// </summary>
    public string PersistenceContentHashAlgorithm { get; }

    /// <summary>
    /// Gets the default logical persistence convention version.
    /// </summary>
    public string PersistenceConventionVersion { get; }

    /// <summary>
    /// Gets the default logical persistence object names from the v1 persistence convention policy.
    /// </summary>
    public IReadOnlyList<string> LogicalObjectNames { get; }
}