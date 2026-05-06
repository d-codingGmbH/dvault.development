namespace DCoding.Data.DVault;

internal sealed class PostgresDataVaultProviderBehavior : IDataVaultProviderBehavior {
  private static readonly DataVaultProviderBehaviorProfile Profile = new("postgres-provider-v1");

  public int Priority => 100;

  public bool CanApply(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return string.Equals(context.ProviderName, PostgresDataVaultSaveStrategy.NpgsqlProviderName, StringComparison.Ordinal);
  }

  public DataVaultProviderBehaviorProfile CreateProfile(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return Profile;
  }
}
