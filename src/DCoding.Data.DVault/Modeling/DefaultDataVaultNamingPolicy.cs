namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Adapts the provider-neutral v1 default naming policy to the model-building override surface.
/// </summary>
public sealed class DefaultDataVaultNamingPolicy : IDataVaultNamingPolicy
{
    private static readonly DefaultNamingPolicy DefaultPolicy = DefaultNamingPolicy.Instance;

    /// <summary>
    /// Gets the shared stateless default naming policy instance.
    /// </summary>
    public static DefaultDataVaultNamingPolicy Instance { get; } = new();

    private DefaultDataVaultNamingPolicy()
    {
    }

    /// <inheritdoc />
    public string GetHubTableName(DataVaultHubNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return DefaultPolicy.GetHubTableName(context.EntityName);
    }

    /// <inheritdoc />
    public string GetLinkTableName(DataVaultLinkNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return DefaultPolicy.GetLinkTableName(context.RelationshipName, context.ParticipantNames);
    }

    /// <inheritdoc />
    public string GetSatelliteTableName(DataVaultSatelliteNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return DefaultPolicy.GetSatelliteTableName(context.ParentEntityName, context.SatelliteName);
    }

    /// <inheritdoc />
    public string GetTechnicalColumnName(DataVaultTechnicalColumnNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Kind switch
        {
            DataVaultTechnicalColumnKind.HashKey => DefaultPolicy.GetHashKeyColumnName(context.BaseName),
            DataVaultTechnicalColumnKind.HashDiff => DefaultPolicy.GetHashDiffColumnName(),
            DataVaultTechnicalColumnKind.LoadTimestamp => DefaultPolicy.GetLoadTimestampColumnName(),
            DataVaultTechnicalColumnKind.RecordSource => DefaultPolicy.GetRecordSourceColumnName(),
            _ => throw new ArgumentOutOfRangeException(nameof(context), context.Kind, "Unsupported technical column kind."),
        };
    }

    /// <inheritdoc />
    public string GetIndexName(DataVaultIndexNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return "Ix" + NormalizeProducedName(context.TableName) + GetIndexKindToken(context.Kind) + JoinProducedNames(context.ColumnNames);
    }

    /// <inheritdoc />
    public string GetConstraintName(DataVaultConstraintNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return GetConstraintKindToken(context.Kind) + NormalizeProducedName(context.TableName) + JoinProducedNames(context.ColumnNames);
    }

    internal static string NormalizeColumnName(string? value)
    {
        return DefaultPolicy.NormalizeColumnName(value);
    }

    internal static IReadOnlyList<string> GetColumnNames(
        IEnumerable<string?> propertyNames,
        IEnumerable<string>? additionalUnsafeColumnNames = null)
    {
        return DefaultPolicy.GetColumnNames(propertyNames, additionalUnsafeColumnNames);
    }

    private static string GetIndexKindToken(DataVaultIndexKind kind)
    {
        return kind switch
        {
            DataVaultIndexKind.BusinessKey => "BusinessKey",
            DataVaultIndexKind.Relationship => "Relationship",
            DataVaultIndexKind.SatelliteParent => "SatelliteParent",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unsupported index kind."),
        };
    }

    private static string GetConstraintKindToken(DataVaultConstraintKind kind)
    {
        return kind switch
        {
            DataVaultConstraintKind.PrimaryKey => "Pk",
            DataVaultConstraintKind.ForeignKey => "Fk",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unsupported constraint kind."),
        };
    }

    private static string JoinProducedNames(IEnumerable<string> names)
    {
        return string.Concat(names.Select(NormalizeProducedName));
    }

    private static string NormalizeProducedName(string? value)
    {
        return DefaultPolicy.NormalizeProducedIdentifier(value);
    }
}
