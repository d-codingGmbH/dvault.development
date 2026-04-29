using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one reusable Data Vault technical metadata column contract.
/// </summary>
public sealed record TechnicalMetadataColumnContract
{
    private static readonly TechnicalMetadataColumnContract[] DefaultContractArray =
    [
        new(
            TechnicalMetadataColumnRole.HashKey,
            "Stable hashed identifier derived from business key values for Data Vault keying and joins.",
            TechnicalMetadataColumnRequiredness.RequiredWhenDeclared,
            "HashKey"),
        new(
            TechnicalMetadataColumnRole.HashDiff,
            "Hash of descriptive or change-detection attributes for satellite change detection.",
            TechnicalMetadataColumnRequiredness.RequiredWhenDeclared,
            "HashDiff"),
        new(
            TechnicalMetadataColumnRole.LoadTimestamp,
            "Timestamp recording when the row was loaded into the vault.",
            TechnicalMetadataColumnRequiredness.RequiredWhenDeclared,
            "LoadTimestamp"),
        new(
            TechnicalMetadataColumnRole.RecordSource,
            "Lineage value identifying the originating source system, feed, or batch.",
            TechnicalMetadataColumnRequiredness.RequiredWhenDeclared,
            "RecordSource"),
    ];

    private TechnicalMetadataColumnContract(
        TechnicalMetadataColumnRole role,
        string semanticPurpose,
        TechnicalMetadataColumnRequiredness requirednessExpectation,
        string defaultEffectiveColumnName,
        string? effectiveColumnName = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(semanticPurpose);
        ArgumentException.ThrowIfNullOrWhiteSpace(defaultEffectiveColumnName);

        Role = role;
        SemanticPurpose = semanticPurpose;
        RequirednessExpectation = requirednessExpectation;
        DefaultEffectiveColumnName = defaultEffectiveColumnName;
        EffectiveColumnName = effectiveColumnName ?? defaultEffectiveColumnName;
    }

    /// <summary>
    /// Gets the default reusable v1 technical metadata column contracts.
    /// </summary>
    public static IReadOnlyList<TechnicalMetadataColumnContract> Defaults { get; } =
        new ReadOnlyCollection<TechnicalMetadataColumnContract>(DefaultContractArray);

    /// <summary>
    /// Gets the metadata role identity.
    /// </summary>
    public TechnicalMetadataColumnRole Role { get; private init; }

    /// <summary>
    /// Gets the semantic purpose for the metadata role.
    /// </summary>
    public string SemanticPurpose { get; private init; }

    /// <summary>
    /// Gets when the metadata column is expected to be present.
    /// </summary>
    public TechnicalMetadataColumnRequiredness RequirednessExpectation { get; private init; }

    /// <summary>
    /// Gets the v1 default effective column name for the role.
    /// </summary>
    public string DefaultEffectiveColumnName { get; private init; }

    /// <summary>
    /// Gets the current effective column name after an optional override.
    /// </summary>
    public string EffectiveColumnName { get; private init; }

    /// <summary>
    /// Returns the default contract for one v1 metadata role.
    /// </summary>
    public static TechnicalMetadataColumnContract ForRole(TechnicalMetadataColumnRole role)
    {
        return role switch
        {
            TechnicalMetadataColumnRole.HashKey => DefaultContractArray[0],
            TechnicalMetadataColumnRole.HashDiff => DefaultContractArray[1],
            TechnicalMetadataColumnRole.LoadTimestamp => DefaultContractArray[2],
            TechnicalMetadataColumnRole.RecordSource => DefaultContractArray[3],
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported technical metadata column role."),
        };
    }

    /// <summary>
    /// Returns a contract instance with an overridden current effective column name.
    /// </summary>
    public TechnicalMetadataColumnContract WithEffectiveColumnName(string effectiveColumnName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(effectiveColumnName);

        return this with
        {
            EffectiveColumnName = effectiveColumnName,
        };
    }
}