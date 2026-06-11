using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed record DataVaultChunkedSaveTelemetryState(
    int ChunkCount,
    int ProcessedChunkCount,
    int RetainedStateCurrentCount,
    int RetainedStateHighWaterCount,
    IReadOnlyList<DataVaultChunkedSaveStateFallbackCauseKind> StateFallbackCauseKinds,
    IReadOnlyList<DataVaultChunkedSaveUnsupportedShapeKind> UnsupportedShapeKinds) {
  public static DataVaultChunkedSaveTelemetryState Empty { get; } = new(
      ChunkCount: 0,
      ProcessedChunkCount: 0,
      RetainedStateCurrentCount: 0,
      RetainedStateHighWaterCount: 0,
      StateFallbackCauseKinds: Array.Empty<DataVaultChunkedSaveStateFallbackCauseKind>(),
      UnsupportedShapeKinds: Array.Empty<DataVaultChunkedSaveUnsupportedShapeKind>());
}
