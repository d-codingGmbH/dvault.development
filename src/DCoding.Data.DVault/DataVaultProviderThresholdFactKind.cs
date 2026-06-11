using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the bounded threshold fact carried by provider tuning diagnostics.
/// </summary>
public enum DataVaultProviderThresholdFactKind {
  /// <summary>
  /// A provider strategy requires at least the specified total operation count.
  /// </summary>
  MinimumTotalOperationCount,

  /// <summary>
  /// A provider strategy accepts at most the specified satellite operation count.
  /// </summary>
  MaximumSatelliteOperationCount,
}
