namespace DCoding.Data.DVault;

/// <summary>
/// Identifies whether save telemetry describes a single explicit request, ordered bulk request, or chunked request.
/// </summary>
public enum DataVaultSaveTelemetryOperationKind {
  /// <summary>
  /// Telemetry describes one <see cref="DataVaultSaveRequest" /> invocation.
  /// </summary>
  SingleRequest,

  /// <summary>
  /// Telemetry describes one <see cref="DataVaultBulkSaveRequest" /> invocation.
  /// </summary>
  BulkRequest,

  /// <summary>
  /// Telemetry describes one <see cref="DataVaultChunkedSaveRequest" /> invocation.
  /// </summary>
  ChunkedRequest,
}
