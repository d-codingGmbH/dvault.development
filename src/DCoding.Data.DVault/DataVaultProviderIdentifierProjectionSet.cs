using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal sealed class DataVaultProviderIdentifierProjectionSet {
  private readonly IReadOnlyDictionary<string, DataVaultProviderIdentifierProjection> _projectionsByPath;

  public DataVaultProviderIdentifierProjectionSet(
      IReadOnlyList<DataVaultProviderIdentifierProjection> projections) {
    ArgumentNullException.ThrowIfNull(projections);

    Projections = projections.ToArray();
    var projectionsByPath = new Dictionary<string, DataVaultProviderIdentifierProjection>(StringComparer.Ordinal);
    foreach (var projection in Projections) {
      projectionsByPath.TryAdd(projection.Candidate.Path, projection);
    }

    _projectionsByPath = projectionsByPath;
  }

  public IReadOnlyList<DataVaultProviderIdentifierProjection> Projections { get; }

  public string GetPhysicalName(string path) {
    ArgumentException.ThrowIfNullOrWhiteSpace(path);

    if (_projectionsByPath.TryGetValue(path, out var projection)) {
      return projection.PhysicalName;
    }

    throw new InvalidOperationException("No Data Vault provider identifier projection exists for path '" + path + "'.");
  }
}
