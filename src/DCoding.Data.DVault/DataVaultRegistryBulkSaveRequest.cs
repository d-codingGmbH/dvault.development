using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Groups multiple registry-backed DVault save requests that should be processed as one ordered batch.
/// </summary>
public sealed class DataVaultRegistryBulkSaveRequest {
  /// <summary>
  /// Initializes a new registry-backed bulk save request.
  /// </summary>
  /// <param name="requests">The registry-backed save requests to resolve and process in caller-supplied order.</param>
  public DataVaultRegistryBulkSaveRequest(IEnumerable<DataVaultRegistrySaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    Requests = RequireRequests(requests, nameof(requests));
  }

  /// <summary>
  /// Gets the registry-backed save requests processed in caller-supplied order.
  /// </summary>
  public IReadOnlyList<DataVaultRegistrySaveRequest> Requests { get; }

  private static IReadOnlyList<DataVaultRegistrySaveRequest> RequireRequests(
      IEnumerable<DataVaultRegistrySaveRequest> requests,
      string parameterName) {
    var values = requests.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Data Vault registry bulk save request collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}
