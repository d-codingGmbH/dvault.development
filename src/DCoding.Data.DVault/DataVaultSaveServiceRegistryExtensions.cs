using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides registry-backed save adapters over the explicit DVault save service.
/// </summary>
public static class DataVaultSaveServiceRegistryExtensions {
  /// <summary>
  /// Resolves hub, link, and satellite metadata from the authoritative DbContext registry and persists the resulting explicit request.
  /// </summary>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed save request containing logical metadata names and row operations.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before write orchestration starts when the DbContext has no authoritative registry source or a required metadata
  /// declaration is missing from that source.
  /// </exception>
  /// <remarks>
  /// This adapter resolves metadata once and then delegates to the existing explicit request pipeline. Callers that invoke
  /// <see cref="IDataVaultSaveService.SaveAsync(DbContext, DataVaultSaveRequest, CancellationToken)" /> or
  /// <see cref="IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest, CancellationToken)" /> keep explicit
  /// caller-supplied metadata precedence and bypass registry resolution.
  /// </remarks>
  public static Task<DataVaultSaveResult> SaveAsync(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      DataVaultRegistrySaveRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    return saveService.SaveAsync(
        dbContext,
        ResolveRequest(registry, request),
        cancellationToken);
  }

  /// <summary>
  /// Resolves all registry-backed save requests from the authoritative DbContext registry and persists them as one ordered batch.
  /// </summary>
  /// <param name="saveService">The explicit save service that performs the validated write pipeline.</param>
  /// <param name="dbContext">The context whose options selected the authoritative Data Vault metadata source.</param>
  /// <param name="request">The registry-backed bulk save request containing ordered logical-name requests.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while saving changes.</param>
  /// <returns>The persisted row summary, including saved hash-key values.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown before write orchestration starts when the DbContext has no authoritative registry source or a required metadata
  /// declaration is missing from that source.
  /// </exception>
  /// <remarks>
  /// All metadata declarations are resolved before the underlying explicit save service is called, so missing registry entries
  /// fail deterministically without partial persistence. Explicit request overloads remain the advanced path when the caller
  /// wants supplied metadata to take precedence over the registry.
  /// </remarks>
  public static Task<DataVaultSaveResult> SaveAsync(
      this IDataVaultSaveService saveService,
      DbContext dbContext,
      DataVaultRegistryBulkSaveRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(saveService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    var registry = DataVaultRegistryMetadataResolver.ResolveRequiredRegistry(dbContext);
    var resolvedRequests = request.Requests
        .Select(current => ResolveRequest(registry, current))
        .ToArray();

    return saveService.SaveAsync(
        dbContext,
        new DataVaultBulkSaveRequest(resolvedRequests),
        cancellationToken);
  }

  internal static DataVaultSaveRequest ResolveRequest(
      DataVaultMetadataRegistry registry,
      DataVaultRegistrySaveRequest request) {
    var hubOperations = request.HubOperations
        .Select(operation => new DataVaultHubSaveOperation(
            DataVaultRegistryMetadataResolver.GetRequiredHub(registry, operation.HubName),
            operation.BusinessKeyValues))
        .ToArray();
    var linkOperations = request.LinkOperations
        .Select(operation => new DataVaultLinkSaveOperation(
            DataVaultRegistryMetadataResolver.GetRequiredLink(registry, operation.LinkName),
            operation.ParticipantHashKeyValues))
        .ToArray();
    var satelliteOperations = request.SatelliteOperations
        .Select(operation => new DataVaultSatelliteSaveOperation(
            DataVaultRegistryMetadataResolver.GetRequiredSatellite(registry, operation.Parent, operation.SatelliteName),
            operation.ParentHashKey,
            operation.DrivingKeyValues,
            operation.PayloadValues,
            operation.HashDiff))
        .ToArray();

    return new DataVaultSaveRequest(
        request.LoadTimestamp,
        request.RecordSource,
        hubOperations,
        linkOperations,
        satelliteOperations);
  }
}
