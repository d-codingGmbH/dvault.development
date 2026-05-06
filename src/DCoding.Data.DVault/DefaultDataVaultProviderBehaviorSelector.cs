using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultProviderBehaviorSelector : IDataVaultProviderBehaviorSelector {
  private readonly IReadOnlyList<IDataVaultProviderBehavior> _providerBehaviors;

  public DefaultDataVaultProviderBehaviorSelector(IEnumerable<IDataVaultProviderBehavior> providerBehaviors) {
    ArgumentNullException.ThrowIfNull(providerBehaviors);

    var behaviorArray = providerBehaviors.ToArray();
    foreach (var providerBehavior in behaviorArray) {
      if (providerBehavior is null) {
        throw new ArgumentException("Data Vault provider behavior registrations must not contain null values.", nameof(providerBehaviors));
      }
    }

    _providerBehaviors = behaviorArray
        .OrderByDescending(providerBehavior => providerBehavior.Priority)
        .ToArray();
  }

  public DataVaultProviderBehaviorProfile SelectBehavior(DbContext dbContext) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return SelectBehavior(new DataVaultProviderBehaviorContext(dbContext));
  }

  public DataVaultProviderBehaviorProfile SelectBehavior(DataVaultProviderBehaviorContext context) {
    ArgumentNullException.ThrowIfNull(context);

    foreach (var providerBehavior in _providerBehaviors) {
      if (!providerBehavior.CanApply(context)) {
        continue;
      }

      var profile = providerBehavior.CreateProfile(context);
      if (profile is null) {
        throw new InvalidOperationException("Data Vault provider behavior returned null.");
      }

      return profile;
    }

    return DataVaultProviderBehaviorProfiles.ProviderNeutral;
  }
}
