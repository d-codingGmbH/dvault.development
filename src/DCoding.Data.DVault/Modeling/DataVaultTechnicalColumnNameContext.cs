namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes a technical column name request.
/// </summary>
public sealed record DataVaultTechnicalColumnNameContext(
    DataVaultTechnicalColumnKind Kind,
    string BaseName,
    string OwnerTableName);
