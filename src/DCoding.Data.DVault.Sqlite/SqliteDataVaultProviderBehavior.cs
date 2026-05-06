namespace DCoding.Data.DVault;

internal sealed class SqliteDataVaultProviderBehavior : IDataVaultProviderBehavior {
  private static readonly DataVaultProviderBehaviorProfile Profile = new("sqlite-provider-v1");

  public int Priority => 100;

  public bool CanApply(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return string.Equals(context.ProviderName, SqliteDataVaultSaveStrategy.ProviderName, StringComparison.Ordinal);
  }

  public DataVaultProviderBehaviorProfile CreateProfile(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return Profile;
  }
}
