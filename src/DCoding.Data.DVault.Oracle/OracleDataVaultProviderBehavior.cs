namespace DCoding.Data.DVault;

internal sealed class OracleDataVaultProviderBehavior : IDataVaultProviderBehavior {
  private static readonly DataVaultProviderBehaviorProfile Profile = new("oracle-provider-v1");

  public int Priority => 100;

  public bool CanApply(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return string.Equals(context.ProviderName, OracleDataVaultSaveStrategy.OracleProviderName, StringComparison.Ordinal);
  }

  public DataVaultProviderBehaviorProfile CreateProfile(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return Profile;
  }
}
