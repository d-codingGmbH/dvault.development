namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Identifies one EF mapped property covered by a registered encrypted-payload alias.
/// </summary>
public sealed record DataVaultPrivacyCoveredProperty(
    string EntityTypeName,
    string PropertyName);
