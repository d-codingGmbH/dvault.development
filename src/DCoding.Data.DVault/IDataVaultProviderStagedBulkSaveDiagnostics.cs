using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Optional diagnostics extension for provider save strategies that evaluate staged-provider bulk execution.
/// </summary>
public interface IDataVaultProviderStagedBulkSaveDiagnostics {
  /// <summary>
  /// Evaluates bounded staged-provider bulk diagnostics for the supplied request batch.
  /// </summary>
  /// <param name="dbContext">The caller-owned Entity Framework context.</param>
  /// <param name="requests">The ordered explicit save request batch.</param>
  /// <returns>Bounded staged-provider bulk diagnostics, or <c>null</c> when staged execution was not evaluated.</returns>
  DataVaultStagedProviderBulkDiagnostics? EvaluateStagedProviderBulkSave(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests);
}
