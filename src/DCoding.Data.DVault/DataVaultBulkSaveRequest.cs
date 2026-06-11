using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Groups multiple explicit DVault save requests that should be processed as one ordered batch.
/// </summary>
public sealed class DataVaultBulkSaveRequest {
  /// <summary>
  /// Initializes a new explicit bulk save request.
  /// </summary>
  /// <param name="requests">The save requests to process in caller-supplied order.</param>
  public DataVaultBulkSaveRequest(IEnumerable<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    Requests = RequireRequests(requests, nameof(requests));
  }

  /// <summary>
  /// Gets the save requests processed in caller-supplied order.
  /// </summary>
  public IReadOnlyList<DataVaultSaveRequest> Requests { get; }

  private static IReadOnlyList<DataVaultSaveRequest> RequireRequests(
      IEnumerable<DataVaultSaveRequest> requests,
      string parameterName) {
    var values = requests.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault bulk save request collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}
