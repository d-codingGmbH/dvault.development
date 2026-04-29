namespace DCoding.Data.DVault;

/// <summary>
/// Describes when a Data Vault technical metadata column contract is expected to be present.
/// </summary>
public enum TechnicalMetadataColumnRequiredness
{
    /// <summary>
    /// The metadata column is required when a consuming model declares the corresponding role.
    /// </summary>
    RequiredWhenDeclared,
}
