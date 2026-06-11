using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the request-bound Data Vault read shape analyzed by diagnostics.
/// </summary>
public enum DataVaultReadShapeKind {
  /// <summary>
  /// Latest or as-of satellite read over one satellite table.
  /// </summary>
  LatestSatellite,

  /// <summary>
  /// PIT-backed as-of read over one maintained PIT table.
  /// </summary>
  PitAsOf,

  /// <summary>
  /// Bridge read over one maintained bridge table.
  /// </summary>
  Bridge,
}
