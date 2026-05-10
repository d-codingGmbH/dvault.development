using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides typed projection helpers over the provider-neutral Data Vault latest/as-of satellite read path.
/// </summary>
public static class DataVaultReadServiceTypedProjectionExtensions {
  private static readonly HashSet<string> ReservedProjectionNames = new(StringComparer.Ordinal)
  {
      DataVaultSatelliteProjectionRow.ParentHashKeyName,
      DataVaultSatelliteProjectionRow.HashDiffName,
      DataVaultSatelliteProjectionRow.LoadTimestampName,
      DataVaultSatelliteProjectionRow.RecordSourceName,
  };

  /// <summary>
  /// Reads latest satellite rows and maps each selected row through a caller-supplied typed projection delegate.
  /// </summary>
  /// <typeparam name="TProjection">The caller-owned projection type returned by the delegate.</typeparam>
  /// <param name="readService">The explicit read service that anchors the provider-neutral latest-row helper surface.</param>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The latest satellite read request.</param>
  /// <param name="projector">The delegate that maps one selected satellite row to one typed projection.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading rows.</param>
  /// <returns>The typed projection rows grouped by parent hash key and driving-key identity.</returns>
  /// <exception cref="InvalidOperationException">
  /// Thrown when a typed projection mapped name is missing, null where required, invalid for the requested accessor, or
  /// collides with a reserved technical field name.
  /// </exception>
  /// <example>
  /// <code>
  /// <![CDATA[
  /// var rows = await readService.ReadLatestSatelliteAsync(
  ///     context,
  ///     new DataVaultLatestSatelliteReadRequest(contactSatellite, [customerHashKey], cutoffUtc),
  ///     row => new CustomerContactRead(
  ///         row.RequiredString("ParentHashKey"),
  ///         row.RequiredString("HashDiff"),
  ///         row.RequiredDateTimeOffset("LoadTimestamp"),
  ///         row.RequiredString("RecordSource"),
  ///         row.RequiredString("ContactType"),
  ///         row.NullableString("EmailAddress")));
  /// ]]>
  /// </code>
  /// </example>
  public static async Task<IReadOnlyList<TProjection>> ReadLatestSatelliteAsync<TProjection>(
      this IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      Func<DataVaultSatelliteProjectionRow, TProjection> projector,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(readService);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    ArgumentNullException.ThrowIfNull(projector);

    ValidateReservedProjectionNames(request.Satellite);

    var rows = await ReadLatestProjectionRowsAsync(
        readService,
        dbContext,
        request,
        cancellationToken).ConfigureAwait(false);
    var projections = new TProjection[rows.Count];

    for (var index = 0; index < rows.Count; index++) {
      projections[index] = projector(rows[index]);
    }

    return projections;
  }

  internal static void ValidateReservedProjectionNames(DataVaultSatelliteMetadata satellite) {
    foreach (var drivingKeyName in satellite.DrivingKeyNames) {
      ValidateReservedProjectionName(satellite.Name, drivingKeyName);
    }

    foreach (var payloadName in satellite.PayloadColumns.Select(column => column.ColumnName)) {
      ValidateReservedProjectionName(satellite.Name, payloadName);
    }
  }

  private static Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestProjectionRowsAsync(
      IDataVaultReadService readService,
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken) {
    return readService is IDataVaultSatelliteProjectionReadService projectionReadService
        ? projectionReadService.ReadLatestSatelliteProjectionRowsAsync(dbContext, request, cancellationToken)
        : DataVaultSatelliteReadPipeline.ReadLatestProjectionRowsAsync(dbContext, request, cancellationToken);
  }

  private static void ValidateReservedProjectionName(
      string satelliteName,
      string mappedName) {
    if (!ReservedProjectionNames.Contains(mappedName)) {
      return;
    }

    throw DataVaultSatelliteProjectionFailures.Create(
        DataVaultSatelliteProjectionFailures.InvalidValue,
        satelliteName,
        mappedName,
        "collides with a reserved typed projection technical field name");
  }
}
