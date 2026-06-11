using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Produces stable Data Vault validation, explain, and request-bound save-strategy diagnostics.
/// </summary>
public interface IDataVaultDiagnosticsService {
  /// <summary>
  /// Analyzes a provider-neutral metadata model using the default SQLite capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(DataVaultMetadataModel metadataModel);

  /// <summary>
  /// Analyzes a provider-neutral metadata model using an explicit provider capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DataVaultMetadataModel metadataModel,
      DataVaultProviderCapabilityProfile providerCapabilities);

  /// <summary>
  /// Analyzes an immutable metadata registry using the default selected capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(DataVaultMetadataRegistry metadataRegistry);

  /// <summary>
  /// Analyzes an immutable metadata registry using an explicit provider capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DataVaultMetadataRegistry metadataRegistry,
      DataVaultProviderCapabilityProfile providerCapabilities);

  /// <summary>
  /// Builds and analyzes fluent code-first Data Vault metadata declarations.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(Action<DataVaultCodeFirstModelBuilder> configureModel);

  /// <summary>
  /// Builds and analyzes fluent code-first Data Vault metadata declarations using an explicit provider capability profile.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      Action<DataVaultCodeFirstModelBuilder> configureModel,
      DataVaultProviderCapabilityProfile providerCapabilities);

  /// <summary>
  /// Analyzes the Data Vault metadata already projected on a DbContext without evaluating request-bound strategy dispatch.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(DbContext dbContext);

  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific save-strategy dispatch for one explicit save request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultSaveRequest request);

  /// <summary>
  /// Analyzes a DbContext and evaluates provider-specific save-strategy dispatch for one ordered explicit bulk save request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultBulkSaveRequest request);

  /// <summary>
  /// Resolves one registry-backed save request and evaluates provider-specific save-strategy dispatch for the resolved request.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistrySaveRequest request);

  /// <summary>
  /// Resolves one registry-backed bulk save request and evaluates provider-specific save-strategy dispatch for the resolved batch.
  /// </summary>
  DataVaultDiagnosticsResult Analyze(
      DbContext dbContext,
      DataVaultRegistryBulkSaveRequest request);
}
