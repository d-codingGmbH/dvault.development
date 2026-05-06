namespace DCoding.Data.DVault;

internal sealed class SqlServerDataVaultProviderBehavior : IDataVaultProviderBehavior {
  private static readonly DataVaultProviderBehaviorProfile Profile = new("sqlserver-provider-v1");

  public int Priority => 100;

  public bool CanApply(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return string.Equals(context.ProviderName, SqlServerDataVaultSaveStrategy.SqlServerProviderName, StringComparison.Ordinal);
  }

  public DataVaultProviderBehaviorProfile CreateProfile(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return Profile;
  }
}
