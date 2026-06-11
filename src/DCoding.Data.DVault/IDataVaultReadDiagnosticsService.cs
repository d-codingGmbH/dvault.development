using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Produces request-bound Data Vault read-strategy diagnostics.
/// </summary>
public interface IDataVaultReadDiagnosticsService {
  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific read-strategy dispatch for one latest/as-of satellite request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request);

  /// <summary>
  /// Resolves one registry-backed latest/as-of satellite read request and evaluates provider-specific read-strategy dispatch.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryLatestSatelliteReadRequest request);

  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific read-strategy dispatch for one PIT-backed as-of request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request);

  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific read-strategy dispatch for one bridge read request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultBridgeReadRequest request);

  /// <summary>
  /// Resolves one registry-backed bridge read request and evaluates provider-specific read-strategy dispatch.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryBridgeReadRequest request);
}
