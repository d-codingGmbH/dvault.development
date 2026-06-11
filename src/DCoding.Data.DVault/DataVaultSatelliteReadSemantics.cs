using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies whether a latest-satellite request is current or as-of bounded.
/// </summary>
public enum DataVaultSatelliteReadSemantics {
  /// <summary>
  /// The request selects current/latest persisted rows without an as-of cutoff.
  /// </summary>
  Current,

  /// <summary>
  /// The request selects rows visible at the supplied as-of cutoff.
  /// </summary>
  AsOf,
}
