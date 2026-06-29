namespace DCoding.Data.DVault;

/// <summary>
/// Identifies an EF mapped property covered by an encrypted-payload alias.
/// </summary>
public sealed record DataVaultPrivacyCoveredPropertyFact(
    string EntityTypeName,
    string PropertyName);
