namespace DCoding.Data.DVault;

/// <summary>
/// Resolves the effective load timestamp for one explicit Data Vault save request.
/// </summary>
public interface IDataVaultLoadTimestampResolver {
  /// <summary>
  /// Resolves the UTC load timestamp to use for every operation in the request.
  /// </summary>
  /// <param name="context">The timestamp resolution context for the save request.</param>
  /// <returns>The effective UTC load timestamp, or <see langword="null" /> when resolution fails.</returns>
  DateTimeOffset? ResolveLoadTimestamp(DataVaultLoadTimestampResolutionContext context);
}
