using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Groups ordered bounded chunks of explicit DVault save requests for provider-neutral chunked execution.
/// </summary>
public sealed class DataVaultChunkedSaveRequest {
  /// <summary>
  /// Initializes a new explicit chunked save request.
  /// </summary>
  /// <param name="chunks">The chunks to process in caller-supplied order.</param>
  public DataVaultChunkedSaveRequest(IEnumerable<DataVaultSaveChunk> chunks) {
    ArgumentNullException.ThrowIfNull(chunks);

    Chunks = RequireChunks(chunks, nameof(chunks));
  }

  /// <summary>
  /// Gets the chunks processed in caller-supplied order.
  /// </summary>
  public IReadOnlyList<DataVaultSaveChunk> Chunks { get; }

  private static IReadOnlyList<DataVaultSaveChunk> RequireChunks(
      IEnumerable<DataVaultSaveChunk> chunks,
      string parameterName) {
    var values = chunks.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault chunked save request collections must not contain null chunks.", parameterName);
      }
    }

    return values;
  }
}
