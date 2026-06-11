using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Defines the explicit DVault v1 write boundary used by callers instead of SaveChanges interception.
/// </summary>
public interface IDataVaultSaveService {
  /// <summary>
  /// Persists the requested Data Vault hub, link, and satellite rows through the supplied Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The explicit save request containing write metadata and row operations.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultSaveRequest request,
      CancellationToken cancellationToken = default);

  /// <summary>
  /// Persists multiple explicit Data Vault save requests as one ordered batch through the supplied Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The explicit bulk save request containing the ordered write requests.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultBulkSaveRequest request,
      CancellationToken cancellationToken = default);

  /// <summary>
  /// Persists ordered bounded chunks of explicit Data Vault save requests through the supplied Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The chunked save request containing caller-ordered bounded chunks.</param>
  /// <param name="cancellationToken">A token used to observe cancellation before continuing to later chunks.</param>
  /// <returns>The persisted row summary, including saved hash-key values in chunk order.</returns>
  Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      DataVaultChunkedSaveRequest request,
      CancellationToken cancellationToken = default);

  /// <summary>
  /// Persists ordered bounded chunks from an async source through the supplied Entity Framework context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="chunks">The async chunk source containing caller-ordered bounded chunks.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while enumerating and processing chunks.</param>
  /// <returns>The persisted row summary, including saved hash-key values in chunk order.</returns>
  Task<DataVaultSaveResult> SaveAsync(
      DbContext dbContext,
      IAsyncEnumerable<DataVaultSaveChunk> chunks,
      CancellationToken cancellationToken = default);
}
