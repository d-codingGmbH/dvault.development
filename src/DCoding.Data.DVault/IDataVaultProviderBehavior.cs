namespace DCoding.Data.DVault;

/// <summary>
/// Defines one optional provider-behavior override that can adapt provider-specific physical behavior without
/// changing DVault naming, hashing, record-source, or timestamp semantics.
/// </summary>
public interface IDataVaultProviderBehavior {
  /// <summary>
  /// Gets the override priority used when multiple provider-behavior hooks are registered.
  /// Higher values are evaluated first; equal values use dependency-injection registration order as the tie-break.
  /// </summary>
  int Priority { get; }

  /// <summary>
  /// Determines whether this provider behavior applies to the supplied provider context.
  /// </summary>
  /// <param name="context">The provider context being evaluated.</param>
  /// <returns><see langword="true" /> when this behavior should be selected.</returns>
  bool CanApply(DataVaultProviderBehaviorContext context);

  /// <summary>
  /// Creates the provider-behavior profile selected for the supplied provider context.
  /// </summary>
  /// <param name="context">The provider context being evaluated.</param>
  /// <returns>The selected provider-behavior profile.</returns>
  DataVaultProviderBehaviorProfile CreateProfile(DataVaultProviderBehaviorContext context);
}
