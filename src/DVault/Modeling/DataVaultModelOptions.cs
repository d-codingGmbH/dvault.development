namespace DVault.Modeling;

/// <summary>
/// Configures Data Vault model convention behavior.
/// </summary>
public sealed class DataVaultModelOptions
{
    /// <summary>
    /// Gets or sets the optional naming policy used when the modeling flow produces Data Vault names.
    /// </summary>
    public IDataVaultNamingPolicy? NamingPolicy { get; set; }

    /// <summary>
    /// Configures the naming policy and returns the current options instance.
    /// </summary>
    public DataVaultModelOptions UseNamingPolicy(IDataVaultNamingPolicy? namingPolicy)
    {
        NamingPolicy = namingPolicy;
        return this;
    }

    internal IDataVaultNamingPolicy ResolveNamingPolicy()
    {
        return NamingPolicy ?? DefaultDataVaultNamingPolicy.Instance;
    }
}
