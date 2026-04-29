namespace DVault.Modeling;

/// <summary>
/// Provides deterministic built-in names when no custom naming policy is configured.
/// </summary>
public sealed class DefaultDataVaultNamingPolicy : IDataVaultNamingPolicy
{
    private const string EmptyValueFallback = "Unnamed";

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
        return ComposeName("Hub", context.EntityName);
    }

    /// <inheritdoc />
    public string GetLinkTableName(DataVaultLinkNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return ComposeName("Link", context.RelationshipName);
    }

    /// <inheritdoc />
    public string GetSatelliteTableName(DataVaultSatelliteNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return ComposeName("Satellite", context.ParentEntityName, context.SatelliteName);
    }

    /// <inheritdoc />
    public string GetTechnicalColumnName(DataVaultTechnicalColumnNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return ComposeName("TechnicalColumn", context.Kind.ToString(), context.BaseName);
    }

    /// <inheritdoc />
    public string GetIndexName(DataVaultIndexNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return ComposeName(
            "Index",
            context.Kind.ToString(),
            context.TableName,
            context.IsUnique ? "Unique" : "NonUnique",
            JoinNameParts(context.ColumnNames));
    }

    /// <inheritdoc />
    public string GetConstraintName(DataVaultConstraintNameContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return ComposeName(
            "Constraint",
            context.Kind.ToString(),
            context.TableName,
            JoinNameParts(context.ColumnNames));
    }

    internal static string NormalizeColumnName(string? value)
    {
        return NormalizeNamePart(value);
    }

    private static string ComposeName(string family, params string?[] parts)
    {
        return family + "__" + JoinNameParts(parts);
    }

    private static string JoinNameParts(IReadOnlyList<string?> parts)
    {
        return string.Join("__", parts.Select(NormalizeNamePart));
    }

    private static string NormalizeNamePart(string? value)
    {
        var trimmed = value?.Trim();
        return string.IsNullOrEmpty(trimmed) ? EmptyValueFallback : trimmed;
    }
}