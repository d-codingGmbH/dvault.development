using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one bounded chunk of explicit DVault save requests.
/// </summary>
public sealed class DataVaultSaveChunk {
  /// <summary>
  /// Initializes a new explicit save chunk.
  /// </summary>
  /// <param name="requests">The save requests to process in caller-supplied order inside this chunk.</param>
  public DataVaultSaveChunk(IEnumerable<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    Requests = RequireRequests(requests, nameof(requests));
  }

  /// <summary>
  /// Gets the bounded save requests processed in caller-supplied order inside this chunk.
  /// </summary>
  public IReadOnlyList<DataVaultSaveRequest> Requests { get; }

  private static IReadOnlyList<DataVaultSaveRequest> RequireRequests(
      IEnumerable<DataVaultSaveRequest> requests,
      string parameterName) {
    var values = requests.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault save chunks must not contain null requests.", parameterName);
      }
    }

    return values;
  }
}
