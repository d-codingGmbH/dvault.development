using System.Collections.ObjectModel;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Captures the provider-neutral v1 Data Vault defaults used when callers do not supply model configuration.
/// </summary>
public sealed class DataVaultConventions {
  private const string DefaultStableHashAlgorithmId = "sha256-v1";
  private const int DefaultStableHashDigestByteLength = 32;
  private const string DefaultPersistenceContentHashAlgorithm = "sha-256";
  private const string DefaultPersistenceConventionVersion = "dvault.persistence-conventions.v1";
  internal const string DefaultProfileName = "default";
  internal const string BinaryFirstProfileName = "binary-first";
  internal const string ExplicitProviderProfileName = "explicit-provider";

  private static readonly DataVaultModelConcept[] DefaultModelConcepts =
  [
      DataVaultModelConcept.Hub,
        DataVaultModelConcept.Link,
        DataVaultModelConcept.Satellite,
        DataVaultModelConcept.Bridge,
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
      int stableHashDigestByteLength,
      DataVaultHashKeyStorageProfile hashKeyStorageProfile,
      string profileName,
      string persistenceContentHashAlgorithm,
      string persistenceConventionVersion,
      IReadOnlyList<string> logicalObjectNames) {
    NamingPolicy = namingPolicy;
    ModelConcepts = modelConcepts;
    StableHashAlgorithmId = stableHashAlgorithmId;
    StableHashDigestByteLength = stableHashDigestByteLength;
    HashKeyStorageProfile = hashKeyStorageProfile;
    ProfileName = profileName;
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
      DefaultStableHashDigestByteLength,
      DataVaultHashKeyStorageProfile.HexString,
      DefaultProfileName,
      DefaultPersistenceContentHashAlgorithm,
      DefaultPersistenceConventionVersion,
      new ReadOnlyCollection<string>(DefaultLogicalObjectNames));

  internal static DataVaultConventions BinaryFirst { get; } = CreateWithStableHashAlgorithm(
      DefaultStableHashAlgorithmId,
      DefaultStableHashDigestByteLength,
      DataVaultHashKeyStorageProfile.Binary,
      BinaryFirstProfileName);

  internal static DataVaultConventions CreateWithStableHashAlgorithm(
      string stableHashAlgorithmId,
      int stableHashDigestByteLength,
      DataVaultHashKeyStorageProfile hashKeyStorageProfile = DataVaultHashKeyStorageProfile.HexString,
      string profileName = DefaultProfileName) {
    ArgumentNullException.ThrowIfNull(stableHashAlgorithmId);
    ArgumentException.ThrowIfNullOrWhiteSpace(profileName);
    if (stableHashDigestByteLength <= 0) {
      throw new ArgumentOutOfRangeException(nameof(stableHashDigestByteLength));
    }

    if (!Enum.IsDefined(hashKeyStorageProfile)) {
      throw new ArgumentOutOfRangeException(nameof(hashKeyStorageProfile));
    }

    if (stableHashAlgorithmId == DefaultStableHashAlgorithmId &&
        stableHashDigestByteLength == DefaultStableHashDigestByteLength &&
        hashKeyStorageProfile == DataVaultHashKeyStorageProfile.HexString &&
        string.Equals(profileName, DefaultProfileName, StringComparison.Ordinal)) {
      return Default;
    }

    return new DataVaultConventions(
        Default.NamingPolicy,
        Default.ModelConcepts,
        stableHashAlgorithmId,
        stableHashDigestByteLength,
        hashKeyStorageProfile,
        profileName,
        Default.PersistenceContentHashAlgorithm,
        Default.PersistenceConventionVersion,
        Default.LogicalObjectNames);
  }

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
  /// Gets the stable hashing service digest byte length used to size generated hash-key columns.
  /// </summary>
  public int StableHashDigestByteLength { get; }

  /// <summary>
  /// Gets the physical storage profile used for generated hash-key columns.
  /// </summary>
  public DataVaultHashKeyStorageProfile HashKeyStorageProfile { get; }

  /// <summary>
  /// Gets the named DVault conventions profile selected by the high-level setup path.
  /// </summary>
  public string ProfileName { get; }

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
