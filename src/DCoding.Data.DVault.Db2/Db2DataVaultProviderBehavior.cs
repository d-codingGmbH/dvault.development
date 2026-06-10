namespace DCoding.Data.DVault;

internal sealed class Db2DataVaultProviderBehavior : IDataVaultProviderBehavior {
  internal const string ProviderName = "IBM.EntityFrameworkCore";

  private static readonly DataVaultProviderBehaviorProfile Profile = new("db2-provider-v1");

  public int Priority => 100;

  public bool CanApply(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return string.Equals(context.ProviderName, ProviderName, StringComparison.Ordinal);
  }

  public DataVaultProviderBehaviorProfile CreateProfile(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return Profile;
  }
}
