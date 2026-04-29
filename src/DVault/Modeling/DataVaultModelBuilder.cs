namespace DVault.Modeling;

/// <summary>
/// Provides provider-neutral convention state for a Data Vault model builder.
/// </summary>
public sealed partial class DataVaultModelBuilder
{
    /// <summary>
    /// Gets the active Data Vault conventions after UseDataVault has been applied.
    /// </summary>
    public DataVaultConventions? Conventions { get; private set; }

    /// <summary>
    /// Gets a value indicating whether Data Vault conventions are enabled for this model builder.
    /// </summary>
    public bool IsDataVaultEnabled => Conventions is not null;

    internal void UseConventions(DataVaultConventions conventions)
    {
        ArgumentNullException.ThrowIfNull(conventions);

        Conventions = conventions;
    }
}
