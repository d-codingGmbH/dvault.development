namespace DVault.Modeling;

/// <summary>
/// Identifies the MVP Data Vault concepts represented by the default model configuration path.
/// </summary>
public enum DataVaultModelConcept
{
    /// <summary>
    /// Represents a hub for a stable business identity.
    /// </summary>
    Hub,

    /// <summary>
    /// Represents a link between two or more hubs.
    /// </summary>
    Link,

    /// <summary>
    /// Represents a satellite carrying descriptive or contextual attributes.
    /// </summary>
    Satellite,

    /// <summary>
    /// Represents a hash key technical identifier.
    /// </summary>
    HashKey,

    /// <summary>
    /// Represents a hash diff used to compare satellite payload state.
    /// </summary>
    HashDiff,

    /// <summary>
    /// Represents the load timestamp metadata concept.
    /// </summary>
    LoadTimestamp,

    /// <summary>
    /// Represents the record source metadata concept.
    /// </summary>
    RecordSource,
}
