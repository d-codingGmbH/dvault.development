namespace DCoding.Data.DVault;

internal sealed class MySqlDataVaultProviderBehavior : IDataVaultProviderBehavior {
  private static readonly DataVaultProviderBehaviorProfile Profile = new("mysql-provider-v1");

  public int Priority => 100;

  public bool CanApply(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return MySqlDataVaultSaveStrategy.IsSupportedProviderName(context.ProviderName);
  }

  public DataVaultProviderBehaviorProfile CreateProfile(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return Profile;
  }
}
