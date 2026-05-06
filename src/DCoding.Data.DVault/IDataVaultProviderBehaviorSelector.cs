using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Selects the effective provider-behavior profile from registered optional overrides.
/// </summary>
public interface IDataVaultProviderBehaviorSelector {
  /// <summary>
  /// Selects the effective provider behavior for an Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose active provider should be inspected.</param>
  /// <returns>The selected provider-behavior profile, or the provider-neutral baseline when no override applies.</returns>
  DataVaultProviderBehaviorProfile SelectBehavior(DbContext dbContext);

  /// <summary>
  /// Selects the effective provider behavior for a provider context.
  /// </summary>
  /// <param name="context">The provider context being evaluated.</param>
  /// <returns>The selected provider-behavior profile, or the provider-neutral baseline when no override applies.</returns>
  DataVaultProviderBehaviorProfile SelectBehavior(DataVaultProviderBehaviorContext context);
}
