using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Carries provider identity for provider-behavior hook selection.
/// </summary>
public sealed class DataVaultProviderBehaviorContext {
  /// <summary>
  /// Initializes a provider-behavior context from an Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose active provider should be inspected.</param>
  public DataVaultProviderBehaviorContext(DbContext dbContext) {
    ArgumentNullException.ThrowIfNull(dbContext);

    DbContext = dbContext;
    ProviderName = dbContext.Database.ProviderName;
  }

  /// <summary>
  /// Initializes a provider-behavior context from an explicit provider name.
  /// </summary>
  /// <param name="providerName">The active Entity Framework provider name, when known.</param>
  public DataVaultProviderBehaviorContext(string? providerName) {
    ProviderName = providerName;
  }

  /// <summary>
  /// Gets the Entity Framework context being evaluated, when selection was context-based.
  /// </summary>
  public DbContext? DbContext { get; }

  /// <summary>
  /// Gets the active Entity Framework provider name, when known.
  /// </summary>
  public string? ProviderName { get; }
}
