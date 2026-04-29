using System.Collections.ObjectModel;
using System.Text;

namespace DVault.Modeling;

/// <summary>
/// Provides the convention-first v1 table and column names used when no custom naming configuration is supplied.
/// </summary>
public sealed class DefaultNamingPolicy
{
    private const string EmptyObjectFallback = "Entity";
    private const string EmptyColumnFallback = "Value";

    private static readonly HashSet<string> ReservedObjectWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "As",
        "By",
        "Column",
        "Constraint",
        "Database",
        "From",
        "Group",
        "Index",
        "Join",
        "On",
        "Schema",
        "Select",
        "Table",
        "User",
        "Where",
    };

    private static readonly HashSet<string> ReservedColumnWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "As",
        "By",
        "Column",
        "Constraint",
        "Database",
        "From",
        "Group",
        "Index",
        "Join",
        "On",
        "Order",
        "Schema",
        "Select",
        "Table",
        "User",
        "Where",
    };

    private static readonly string[] FixedTechnicalColumns =
    [
        "HashDiff",
        "LoadTimestamp",
        "RecordSource",
    ];

    /// <summary>
    /// Gets the shared stateless default naming policy instance.
    /// </summary>
    public static DefaultNamingPolicy Instance { get; } = new();

    /// <summary>
    /// Gets the deterministic technical column names that do not depend on an entity base name.
    /// </summary>
    public static IReadOnlyList<string> FixedTechnicalColumnNames { get; } =
        new ReadOnlyCollection<string>(FixedTechnicalColumns);

    /// <summary>
    /// Returns a hub table name in the form Hub{Entity}.
    /// </summary>
    public string GetHubTableName(string? entityName)
    {
        return "Hub" + NormalizeObjectName(entityName);
    }

    /// <summary>
    /// Returns a link table name using an explicit relationship name when supplied, or participant names in declaration order.
    /// </summary>
    public string GetLinkTableName(string? relationshipName, IEnumerable<string?> participantNames)
    {
        if (TryNormalizeObjectName(relationshipName, out var normalizedRelationshipName))
        {
            return "Link" + normalizedRelationshipName;
        }

        ArgumentNullException.ThrowIfNull(participantNames);

        var nameBuilder = new StringBuilder();
        foreach (var participantName in participantNames)
        {
            nameBuilder.Append(NormalizeObjectName(participantName));
        }

        return "Link" + (nameBuilder.Length == 0 ? EmptyObjectFallback : nameBuilder.ToString());
    }

    /// <summary>
    /// Returns a satellite table name in the form Sat{Parent}{SatelliteDescriptor}.
    /// </summary>
    public string GetSatelliteTableName(string? parentName, string? satelliteDescriptor)
    {
        return "Sat" + NormalizeObjectName(parentName) + NormalizeObjectName(satelliteDescriptor);
    }

    /// <summary>
    /// Returns a hash key column name in the form {Base}HashKey.
    /// </summary>
    public string GetHashKeyColumnName(string? baseName)
    {
        return NormalizeObjectName(baseName) + "HashKey";
    }

    /// <summary>
    /// Returns the default hash diff column name.
    /// </summary>
    public string GetHashDiffColumnName()
    {
        return "HashDiff";
    }

    /// <summary>
    /// Returns the default load timestamp column name.
    /// </summary>
    public string GetLoadTimestampColumnName()
    {
        return "LoadTimestamp";
    }

    /// <summary>
    /// Returns the default record source column name.
    /// </summary>
    public string GetRecordSourceColumnName()
    {
        return "RecordSource";
    }

    /// <summary>
    /// Returns a safe PascalCase object base name for entities, roles, relationships, and satellite descriptors.
    /// </summary>
    public string NormalizeObjectName(string? value)
    {
        return NormalizeObjectNameCore(value, out _);
    }

    /// <summary>
    /// Returns a safe PascalCase column name for one business-key or payload property.
    /// </summary>
    public string GetColumnName(string? propertyName, IEnumerable<string>? additionalUnsafeColumnNames = null)
    {
        var unsafeColumnNames = CreateUnsafeColumnSet(additionalUnsafeColumnNames);
        var usedColumnNames = new HashSet<string>(unsafeColumnNames, StringComparer.OrdinalIgnoreCase);
        var candidate = NormalizeColumnNameCore(propertyName, unsafeColumnNames);

        return MakeUnique(candidate, usedColumnNames);
    }

    /// <summary>
    /// Returns safe PascalCase column names for business-key or payload properties in declaration order.
    /// </summary>
    public IReadOnlyList<string> GetColumnNames(
        IEnumerable<string?> propertyNames,
        IEnumerable<string>? additionalUnsafeColumnNames = null)
    {
        ArgumentNullException.ThrowIfNull(propertyNames);

        var unsafeColumnNames = CreateUnsafeColumnSet(additionalUnsafeColumnNames);
        var usedColumnNames = new HashSet<string>(unsafeColumnNames, StringComparer.OrdinalIgnoreCase);
        var columnNames = new List<string>();

        foreach (var propertyName in propertyNames)
        {
            var candidate = NormalizeColumnNameCore(propertyName, unsafeColumnNames);
            var uniqueName = MakeUnique(candidate, usedColumnNames);
            usedColumnNames.Add(uniqueName);
            columnNames.Add(uniqueName);
        }

        return columnNames;
    }

    /// <summary>
    /// Returns a safe PascalCase column base name before same-scope duplicate suffixes are applied.
    /// </summary>
    public string NormalizeColumnName(string? value)
    {
        return NormalizeColumnNameCore(value, CreateUnsafeColumnSet(null));
    }

    private static HashSet<string> CreateUnsafeColumnSet(IEnumerable<string>? additionalUnsafeColumnNames)
    {
        var unsafeColumnNames = new HashSet<string>(FixedTechnicalColumns, StringComparer.OrdinalIgnoreCase);

        if (additionalUnsafeColumnNames is null)
        {
            return unsafeColumnNames;
        }

        foreach (var columnName in additionalUnsafeColumnNames)
        {
            if (!string.IsNullOrWhiteSpace(columnName))
            {
                unsafeColumnNames.Add(columnName);
            }
        }

        return unsafeColumnNames;
    }

    private static string NormalizeColumnNameCore(string? value, ISet<string> unsafeColumnNames)
    {
        var normalized = NormalizePascalCase(value, singularizeWords: false);
        if (normalized.Length == 0)
        {
            return EmptyColumnFallback;
        }

        if (ReservedColumnWords.Contains(normalized) || unsafeColumnNames.Contains(normalized))
        {
            return normalized + EmptyColumnFallback;
        }

        return normalized;
    }

    private static string NormalizeObjectNameCore(string? value, out bool hadSemanticToken)
    {
        var normalized = NormalizePascalCase(value, singularizeWords: true);
        hadSemanticToken = normalized.Length > 0;

        if (!hadSemanticToken)
        {
            return EmptyObjectFallback;
        }

        return ReservedObjectWords.Contains(normalized)
            ? normalized + EmptyObjectFallback
            : normalized;
    }

    private static bool TryNormalizeObjectName(string? value, out string normalizedName)
    {
        normalizedName = NormalizeObjectNameCore(value, out var hadSemanticToken);
        return hadSemanticToken;
    }

    private static string MakeUnique(string candidate, ISet<string> usedColumnNames)
    {
        if (!usedColumnNames.Contains(candidate))
        {
            return candidate;
        }

        var suffix = 2;
        var uniqueName = candidate + suffix.ToStringInvariant();
        while (usedColumnNames.Contains(uniqueName))
        {
            suffix++;
            uniqueName = candidate + suffix.ToStringInvariant();
        }

        return uniqueName;
    }

    private static string NormalizePascalCase(string? value, bool singularizeWords)
    {
        var tokens = SplitIdentifierTokens(value);
        if (tokens.Count == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        foreach (var token in tokens)
        {
            var preparedToken = PrepareToken(token, singularizeWords, builder.Length == 0);
            if (preparedToken.Length == 0)
            {
                continue;
            }

            builder.Append(ToPascalToken(preparedToken));
        }

        return builder.ToString();
    }

    private static List<string> SplitIdentifierTokens(string? value)
    {
        var tokens = new List<string>();
        if (string.IsNullOrWhiteSpace(value))
        {
            return tokens;
        }

        var currentToken = new StringBuilder();
        for (var index = 0; index < value.Length; index++)
        {
            var current = value[index];
            if (!IsAsciiLetterOrDigit(current))
            {
                AddToken(tokens, currentToken);
                continue;
            }

            if (currentToken.Length > 0)
            {
                var previous = value[index - 1];
                var next = index + 1 < value.Length ? value[index + 1] : '\0';
                if (StartsNewToken(previous, current, next))
                {
                    AddToken(tokens, currentToken);
                }
            }

            currentToken.Append(current);
        }

        AddToken(tokens, currentToken);
        return tokens;
    }

    private static void AddToken(ICollection<string> tokens, StringBuilder currentToken)
    {
        if (currentToken.Length == 0)
        {
            return;
        }

        tokens.Add(currentToken.ToString());
        currentToken.Clear();
    }

    private static bool StartsNewToken(char previous, char current, char next)
    {
        if (!IsAsciiLetterOrDigit(previous))
        {
            return false;
        }

        if (IsAsciiDigit(previous) && IsAsciiLetter(current))
        {
            return true;
        }

        if (IsAsciiLower(previous) && IsAsciiUpper(current))
        {
            return true;
        }

        return IsAsciiUpper(previous) && IsAsciiUpper(current) && IsAsciiLower(next);
    }

    private static string PrepareToken(string token, bool singularizeWords, bool firstToken)
    {
        var tokenStart = 0;
        if (firstToken)
        {
            while (tokenStart < token.Length && !IsAsciiLetter(token[tokenStart]))
            {
                tokenStart++;
            }
        }

        if (tokenStart == token.Length)
        {
            return string.Empty;
        }

        var preparedToken = token[tokenStart..].ToLowerInvariant();
        return singularizeWords ? SingularizeToken(preparedToken) : preparedToken;
    }

    private static string SingularizeToken(string token)
    {
        if (token.Length <= 1)
        {
            return token;
        }

        if (token.EndsWith("ies", StringComparison.Ordinal) &&
            token.Length > 3 &&
            IsConsonant(token[^4]))
        {
            return token[..^3] + "y";
        }

        if (EndsWithAny(token, "ches", "shes", "sses", "xes", "zes", "ses"))
        {
            return token[..^2];
        }

        if (token.EndsWith('s') && !token.EndsWith("ss", StringComparison.Ordinal))
        {
            return token[..^1];
        }

        return token;
    }

    private static bool EndsWithAny(string token, params string[] suffixes)
    {
        foreach (var suffix in suffixes)
        {
            if (token.EndsWith(suffix, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsConsonant(char value)
    {
        return IsAsciiLetter(value) && value is not ('a' or 'e' or 'i' or 'o' or 'u');
    }

    private static string ToPascalToken(string token)
    {
        if (token.Length == 0)
        {
            return string.Empty;
        }

        if (token.Length == 1)
        {
            return token.ToUpperInvariant();
        }

        return char.ToUpperInvariant(token[0]) + token[1..].ToLowerInvariant();
    }

    private static bool IsAsciiLetterOrDigit(char value)
    {
        return IsAsciiLetter(value) || IsAsciiDigit(value);
    }

    private static bool IsAsciiLetter(char value)
    {
        return IsAsciiUpper(value) || IsAsciiLower(value);
    }

    private static bool IsAsciiUpper(char value)
    {
        return value is >= 'A' and <= 'Z';
    }

    private static bool IsAsciiLower(char value)
    {
        return value is >= 'a' and <= 'z';
    }

    private static bool IsAsciiDigit(char value)
    {
        return value is >= '0' and <= '9';
    }
}

file static class InvariantStringExtensions
{
    public static string ToStringInvariant(this int value)
    {
        return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
