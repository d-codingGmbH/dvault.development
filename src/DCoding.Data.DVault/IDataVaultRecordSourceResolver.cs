namespace DCoding.Data.DVault;

/// <summary>
/// Resolves the effective record source for one explicit Data Vault save request.
/// </summary>
public interface IDataVaultRecordSourceResolver {
  /// <summary>
  /// Resolves the required record source to use for every operation in the request.
  /// </summary>
  /// <param name="context">The record-source resolution context for the save request.</param>
  /// <returns>The effective record source, or <see langword="null" /> when resolution fails.</returns>
  string? ResolveRecordSource(DataVaultRecordSourceResolutionContext context);
}
