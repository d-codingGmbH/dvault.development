using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal enum DataVaultProviderIdentifierKind {
  Table,
  Column,
  PrimaryKey,
  Index,
  Constraint,
}

internal sealed record DataVaultProviderIdentifierCandidate(
    DataVaultProviderIdentifierKind Kind,
    string LogicalName,
    string? MetadataName,
    string Scope,
    string Path);

internal sealed record DataVaultProviderIdentifierProjection(
    DataVaultProviderIdentifierCandidate Candidate,
    string PhysicalName,
    bool IsDerived);

internal sealed record DataVaultProviderIdentifierPreflightIssue(
    DataVaultProviderIdentifierKind Kind,
    string LogicalName,
    string? MetadataName,
    string Scope,
    string Path,
    string ProviderProfileName,
    string FailureClass,
    string Message) {
  public string? AttemptedPhysicalName { get; init; }

  public int? MaximumIdentifierLength { get; init; }
}

internal sealed class DataVaultProviderIdentifierProjectionSet {
  private readonly IReadOnlyDictionary<string, DataVaultProviderIdentifierProjection> _projectionsByPath;

  public DataVaultProviderIdentifierProjectionSet(
      IReadOnlyList<DataVaultProviderIdentifierProjection> projections) {
    ArgumentNullException.ThrowIfNull(projections);

    Projections = projections.ToArray();
    var projectionsByPath = new Dictionary<string, DataVaultProviderIdentifierProjection>(StringComparer.Ordinal);
    foreach (var projection in Projections) {
      projectionsByPath.TryAdd(projection.Candidate.Path, projection);
    }

    _projectionsByPath = projectionsByPath;
  }

  public IReadOnlyList<DataVaultProviderIdentifierProjection> Projections { get; }

  public string GetPhysicalName(string path) {
    ArgumentException.ThrowIfNullOrWhiteSpace(path);

    if (_projectionsByPath.TryGetValue(path, out var projection)) {
      return projection.PhysicalName;
    }

    throw new InvalidOperationException("No Data Vault provider identifier projection exists for path '" + path + "'.");
  }
}

internal sealed record DataVaultProviderIdentifierPreflightResult(
    DataVaultProviderIdentifierProjectionSet ProjectionSet,
    IReadOnlyList<DataVaultProviderIdentifierPreflightIssue> Issues);

internal static class DataVaultProviderIdentifierPreflight {
  private const int MinimumHashLength = 8;
  private const int HashLengthIncrement = 4;
  private const int MaximumHashLength = 64;
  private const string ScopeSeparator = "\u001f";

  private static readonly IReadOnlySet<string> CommonReservedWords = new HashSet<string>(
      [
          "add",
          "all",
          "alter",
          "and",
          "as",
          "by",
          "column",
          "constraint",
          "create",
          "database",
          "delete",
          "drop",
          "from",
          "group",
          "index",
          "insert",
          "into",
          "join",
          "key",
          "not",
          "null",
          "on",
          "order",
          "primary",
          "schema",
          "select",
          "table",
          "unique",
          "update",
          "user",
          "values",
          "where",
      ],
      StringComparer.OrdinalIgnoreCase);

  public static DataVaultProviderIdentifierPreflightResult Analyze(
      DataVaultProviderCapabilityProfile providerCapabilities,
      IEnumerable<DataVaultProviderIdentifierCandidate> candidates) {
    ArgumentNullException.ThrowIfNull(providerCapabilities);
    ArgumentNullException.ThrowIfNull(candidates);

    var rules = DataVaultProviderIdentifierRules.Create(providerCapabilities);
    var candidateArray = candidates.ToArray();
    var issues = new List<DataVaultProviderIdentifierPreflightIssue>();
    var projections = new List<DataVaultProviderIdentifierProjection>();

    foreach (var candidate in candidateArray) {
      ArgumentNullException.ThrowIfNull(candidate);
      if (TryCreateProjection(candidate, rules, hashLength: null, out var projection, out var issue)) {
        projections.Add(projection);
      }
      else if (issue is not null) {
        issues.Add(issue);
      }
    }

    issues.AddRange(CreateDuplicateProducedNameIssues(candidateArray, rules));
    ResolveProjectionCollisions(projections, issues, rules);

    return new DataVaultProviderIdentifierPreflightResult(
        new DataVaultProviderIdentifierProjectionSet(projections),
        issues);
  }

  public static DataVaultDiagnosticsIssue CreateDiagnosticIssue(
      DataVaultProviderIdentifierPreflightIssue issue,
      string? providerName = null) {
    ArgumentNullException.ThrowIfNull(issue);

    var providerContext = string.IsNullOrWhiteSpace(providerName)
        ? "provider <none>"
        : "provider '" + providerName + "'";
    var metadataContext = string.IsNullOrWhiteSpace(issue.MetadataName)
        ? string.Empty
        : ", metadata name '" + issue.MetadataName + "'";
    var attemptedContext = string.IsNullOrWhiteSpace(issue.AttemptedPhysicalName)
        ? string.Empty
        : ", attempted physical name '" + issue.AttemptedPhysicalName + "'";
    var limitContext = issue.MaximumIdentifierLength.HasValue
        ? ", limit " + issue.MaximumIdentifierLength.Value.ToString(CultureInfo.InvariantCulture)
        : string.Empty;

    return new DataVaultDiagnosticsIssue(
        DataVaultDiagnosticsIssueSeverity.Error,
        "DVM2009",
        "Provider identifier preflight failed for " +
            providerContext +
            ", profile '" +
            issue.ProviderProfileName +
            "', object class '" +
            FormatKind(issue.Kind) +
            "', logical produced name '" +
            issue.LogicalName +
            "'" +
            metadataContext +
            attemptedContext +
            limitContext +
            ", failure class '" +
            issue.FailureClass +
            "'. " +
            issue.Message,
        issue.Path);
  }

  internal static DataVaultProviderIdentifierRules GetRules(
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return DataVaultProviderIdentifierRules.Create(providerCapabilities);
  }

  private static IEnumerable<DataVaultProviderIdentifierPreflightIssue> CreateDuplicateProducedNameIssues(
      IReadOnlyList<DataVaultProviderIdentifierCandidate> candidates,
      DataVaultProviderIdentifierRules rules) {
    foreach (var group in candidates.GroupBy(CreateLogicalScopeKey, StringComparer.Ordinal)) {
      if (group.Count() <= 1) {
        continue;
      }

      foreach (var candidate in group) {
        yield return CreateIssue(
            candidate,
            rules,
            "duplicate-produced-name",
            "The generated logical produced name appears more than once in the same provider-visible identifier scope. Rename the source model declaration or role so provider-neutral naming produces a distinct logical name.",
            attemptedPhysicalName: candidate.LogicalName);
      }
    }
  }

  private static void ResolveProjectionCollisions(
      List<DataVaultProviderIdentifierProjection> projections,
      List<DataVaultProviderIdentifierPreflightIssue> issues,
      DataVaultProviderIdentifierRules rules) {
    var groups = projections
        .GroupBy(CreatePhysicalScopeKey, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .ToArray();

    foreach (var group in groups) {
      if (group
          .Select(projection => projection.Candidate.LogicalName)
          .Distinct(StringComparer.Ordinal)
          .Count() != group.Count()) {
        continue;
      }

      DataVaultProviderIdentifierProjection[]? resolvedProjections = null;
      for (var hashLength = MinimumHashLength; hashLength <= MaximumHashLength; hashLength += HashLengthIncrement) {
        var candidateProjections = new List<DataVaultProviderIdentifierProjection>();
        var candidateIssues = new List<DataVaultProviderIdentifierPreflightIssue>();
        foreach (var projection in group) {
          if (TryCreateProjection(projection.Candidate, rules, hashLength, out var resolvedProjection, out var issue)) {
            candidateProjections.Add(resolvedProjection);
          }
          else if (issue is not null) {
            candidateIssues.Add(issue);
          }
        }

        if (candidateIssues.Count > 0) {
          issues.AddRange(candidateIssues);
          break;
        }

        if (candidateProjections
            .Select(CreatePhysicalScopeKey)
            .Distinct(StringComparer.Ordinal)
            .Count() == candidateProjections.Count) {
          resolvedProjections = candidateProjections.ToArray();
          break;
        }
      }

      if (resolvedProjections is null) {
        foreach (var projection in group) {
          issues.Add(CreateIssue(
              projection.Candidate,
              rules,
              "post-projection-collision",
              "The generated identifier could not be projected to a unique provider-safe physical name in the same provider-visible scope. Rename the source model declaration or split the model so the provider can represent the generated shape.",
              projection.PhysicalName));
        }

        continue;
      }

      foreach (var oldProjection in group) {
        projections.Remove(oldProjection);
      }

      projections.AddRange(resolvedProjections);
    }
  }

  private static bool TryCreateProjection(
      DataVaultProviderIdentifierCandidate candidate,
      DataVaultProviderIdentifierRules rules,
      int? hashLength,
      out DataVaultProviderIdentifierProjection projection,
      out DataVaultProviderIdentifierPreflightIssue? issue) {
    projection = null!;
    issue = null;

    if (string.IsNullOrWhiteSpace(candidate.LogicalName)) {
      issue = CreateIssue(
          candidate,
          rules,
          "empty-name",
          "Generated Data Vault identifiers must not be empty. Rename the source model declaration or role so provider-neutral naming produces a non-empty name.",
          attemptedPhysicalName: null);
      return false;
    }

    var logicalName = candidate.LogicalName;
    var candidateName = CreateProjectionBaseName(logicalName, candidate.Kind, rules);
    var requiresDerivation = hashLength.HasValue ||
        !IsSafePhysicalName(logicalName, rules) ||
        ExceedsLimit(logicalName, rules);
    if (!requiresDerivation) {
      projection = new DataVaultProviderIdentifierProjection(candidate, logicalName, IsDerived: false);
      return true;
    }

    var effectiveHashLength = hashLength.GetValueOrDefault(MinimumHashLength);
    string physicalName;
    if (!hashLength.HasValue && IsSafePhysicalName(candidateName, rules) && !ExceedsLimit(candidateName, rules)) {
      physicalName = candidateName;
    }
    else if (!TryCreateHashedName(
        logicalName,
        candidateName,
        rules,
        effectiveHashLength,
        out physicalName,
        out var failureMessage)) {
      issue = CreateIssue(
          candidate,
          rules,
          "length-limit",
          failureMessage,
          attemptedPhysicalName: null);
      return false;
    }

    if (!IsSafePhysicalName(physicalName, rules) || ExceedsLimit(physicalName, rules)) {
      issue = CreateIssue(
          candidate,
          rules,
          GetUnsafeFailureClass(physicalName, rules),
          "The generated identifier cannot be projected to a provider-safe unquoted physical name. Rename the source model declaration or role so provider-neutral naming produces a safe logical name.",
          physicalName);
      return false;
    }

    projection = new DataVaultProviderIdentifierProjection(candidate, physicalName, IsDerived: true);
    return true;
  }

  private static string CreateProjectionBaseName(
      string logicalName,
      DataVaultProviderIdentifierKind kind,
      DataVaultProviderIdentifierRules rules) {
    var baseName = IsReservedWord(logicalName, rules)
        ? logicalName + GetObjectClassSuffix(kind)
        : logicalName;

    if (HasOnlyValidIdentifierCharacters(baseName, rules)) {
      return baseName;
    }

    var builder = new StringBuilder(baseName.Length);
    foreach (var character in baseName) {
      if (builder.Length == 0) {
        if (rules.IsValidFirstCharacter(character)) {
          builder.Append(character);
        }

        continue;
      }

      if (rules.IsValidSubsequentCharacter(character)) {
        builder.Append(character);
      }
    }

    if (builder.Length == 0) {
      builder.Append(GetObjectClassSuffix(kind));
    }

    return builder.ToString();
  }

  private static bool TryCreateHashedName(
      string logicalName,
      string projectionBaseName,
      DataVaultProviderIdentifierRules rules,
      int hashLength,
      out string physicalName,
      out string failureMessage) {
    physicalName = string.Empty;
    failureMessage = string.Empty;
    if (hashLength is < MinimumHashLength or > MaximumHashLength) {
      failureMessage = "The generated identifier cannot be projected because the required hash suffix length is outside the bounded provider preflight range.";
      return false;
    }

    var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(logicalName)))
        .ToLowerInvariant()[..hashLength];

    if (rules.MaximumIdentifierLength is null) {
      physicalName = projectionBaseName + "_" + hash;
      return true;
    }

    var maximumIdentifierLength = rules.MaximumIdentifierLength.Value;
    var prefixLength = maximumIdentifierLength - hashLength - 1;
    if (prefixLength < 1) {
      failureMessage = "The provider identifier limit is too small to hold a one-character prefix, separator, and " +
          hashLength.ToString(CultureInfo.InvariantCulture) +
          "-character deterministic hash suffix. Rename the source model declaration or choose a provider/profile with a wider identifier limit.";
      return false;
    }

    physicalName = projectionBaseName[..Math.Min(prefixLength, projectionBaseName.Length)] + "_" + hash;
    return true;
  }

  private static bool IsSafePhysicalName(
      string name,
      DataVaultProviderIdentifierRules rules) {
    return !string.IsNullOrWhiteSpace(name) &&
        HasOnlyValidIdentifierCharacters(name, rules) &&
        !IsReservedWord(name, rules) &&
        !ExceedsLimit(name, rules);
  }

  private static bool HasOnlyValidIdentifierCharacters(
      string name,
      DataVaultProviderIdentifierRules rules) {
    if (string.IsNullOrEmpty(name) || !rules.IsValidFirstCharacter(name[0])) {
      return false;
    }

    for (var index = 1; index < name.Length; index++) {
      if (!rules.IsValidSubsequentCharacter(name[index])) {
        return false;
      }
    }

    return true;
  }

  private static bool IsReservedWord(
      string name,
      DataVaultProviderIdentifierRules rules) {
    return rules.ReservedWords.Contains(name);
  }

  private static bool ExceedsLimit(
      string name,
      DataVaultProviderIdentifierRules rules) {
    return rules.MaximumIdentifierLength.HasValue &&
        name.Length > rules.MaximumIdentifierLength.Value;
  }

  private static string GetUnsafeFailureClass(
      string physicalName,
      DataVaultProviderIdentifierRules rules) {
    if (string.IsNullOrWhiteSpace(physicalName)) {
      return "empty-name";
    }

    if (ExceedsLimit(physicalName, rules)) {
      return "length-limit";
    }

    if (IsReservedWord(physicalName, rules)) {
      return "reserved-word";
    }

    return "requires-quoting";
  }

  private static DataVaultProviderIdentifierPreflightIssue CreateIssue(
      DataVaultProviderIdentifierCandidate candidate,
      DataVaultProviderIdentifierRules rules,
      string failureClass,
      string message,
      string? attemptedPhysicalName) {
    return new DataVaultProviderIdentifierPreflightIssue(
        candidate.Kind,
        candidate.LogicalName,
        candidate.MetadataName,
        candidate.Scope,
        candidate.Path,
        rules.ProfileName,
        failureClass,
        message) {
      AttemptedPhysicalName = attemptedPhysicalName,
      MaximumIdentifierLength = rules.MaximumIdentifierLength,
    };
  }

  private static string CreateLogicalScopeKey(DataVaultProviderIdentifierCandidate candidate) {
    return string.Join(
        ScopeSeparator,
        candidate.Kind.ToString(),
        candidate.Scope,
        candidate.LogicalName);
  }

  private static string CreatePhysicalScopeKey(DataVaultProviderIdentifierProjection projection) {
    return string.Join(
        ScopeSeparator,
        projection.Candidate.Kind.ToString(),
        projection.Candidate.Scope,
        projection.PhysicalName);
  }

  private static string GetObjectClassSuffix(DataVaultProviderIdentifierKind kind) {
    return kind switch {
      DataVaultProviderIdentifierKind.Table => "Table",
      DataVaultProviderIdentifierKind.Column => "Column",
      DataVaultProviderIdentifierKind.Index => "Index",
      DataVaultProviderIdentifierKind.PrimaryKey => "Key",
      DataVaultProviderIdentifierKind.Constraint => "Constraint",
      _ => "Identifier",
    };
  }

  private static string FormatKind(DataVaultProviderIdentifierKind kind) {
    return kind switch {
      DataVaultProviderIdentifierKind.PrimaryKey => "primary-key",
      _ => kind.ToString().ToLowerInvariant(),
    };
  }

  internal sealed class DataVaultProviderIdentifierRules {
    private DataVaultProviderIdentifierRules(
        string profileName,
        int? maximumIdentifierLength,
        IReadOnlySet<string> reservedWords) {
      ProfileName = profileName;
      MaximumIdentifierLength = maximumIdentifierLength;
      ReservedWords = reservedWords;
    }

    public string ProfileName { get; }

    public int? MaximumIdentifierLength { get; }

    public IReadOnlySet<string> ReservedWords { get; }

    public static DataVaultProviderIdentifierRules Create(
        DataVaultProviderCapabilityProfile providerCapabilities) {
      var reservedWords = IsSupportedProfile(providerCapabilities.ProfileName)
          ? CommonReservedWords
          : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      return new DataVaultProviderIdentifierRules(
          providerCapabilities.ProfileName,
          providerCapabilities.MaximumIdentifierLength,
          reservedWords);
    }

    public bool IsValidFirstCharacter(char character) {
      return IsAsciiLetter(character) || character == '_';
    }

    public bool IsValidSubsequentCharacter(char character) {
      return IsAsciiLetter(character) || char.IsAsciiDigit(character) || character == '_';
    }

    private static bool IsSupportedProfile(string profileName) {
      return profileName.StartsWith("sqlite-", StringComparison.Ordinal) ||
          profileName.StartsWith("oracle-", StringComparison.Ordinal) ||
          profileName.StartsWith("postgres-", StringComparison.Ordinal) ||
          profileName.StartsWith("sqlserver-", StringComparison.Ordinal) ||
          profileName.StartsWith("db2-", StringComparison.Ordinal) ||
          profileName.StartsWith("mysql-", StringComparison.Ordinal);
    }

    private static bool IsAsciiLetter(char character) {
      return character is >= 'A' and <= 'Z' or >= 'a' and <= 'z';
    }
  }
}
