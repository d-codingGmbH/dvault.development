using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Runs library-local Data Vault model drift preflight comparisons without live database access.
/// </summary>
public static class DataVaultModelDriftPreflightReporter {
  /// <summary>
  /// Compares expected Data Vault metadata, the configured DbContext runtime model, and an explicit snapshot model.
  /// </summary>
  /// <param name="expectedMetadataModel">The expected provider-neutral Data Vault metadata model.</param>
  /// <param name="currentContext">The configured DbContext whose runtime model is compared.</param>
  /// <param name="snapshotModel">The explicit consumer-materialized snapshot model to compare.</param>
  /// <returns>A deterministic structured preflight report with all three comparison sections.</returns>
  public static DataVaultModelDriftPreflightReport Compare(
      DataVaultMetadataModel expectedMetadataModel,
      DbContext currentContext,
      IReadOnlyModel snapshotModel) {
    ArgumentNullException.ThrowIfNull(expectedMetadataModel);
    ArgumentNullException.ThrowIfNull(currentContext);
    ArgumentNullException.ThrowIfNull(snapshotModel);

    var providerCapabilities = DataVaultProviderCapabilityProfileSelection.Select(currentContext.Database.ProviderName);
    var runtimeModel = currentContext.Model;

    return new DataVaultModelDriftPreflightReport(
        DataVaultModelDriftReporter.Compare(expectedMetadataModel, runtimeModel, providerCapabilities),
        DataVaultModelDriftReporter.Compare(expectedMetadataModel, snapshotModel, providerCapabilities),
        DataVaultModelDriftReporter.CompareModels(runtimeModel, snapshotModel));
  }

  /// <summary>
  /// Compares a successful model-first import result, the configured DbContext runtime model, and an explicit snapshot model.
  /// </summary>
  /// <param name="expectedImport">The expected successful dvault.model.v1 import result.</param>
  /// <param name="currentContext">The configured DbContext whose runtime model is compared.</param>
  /// <param name="snapshotModel">The explicit consumer-materialized snapshot model to compare.</param>
  /// <returns>A deterministic structured preflight report with all three comparison sections.</returns>
  public static DataVaultModelDriftPreflightReport Compare(
      DataVaultModelImportResult expectedImport,
      DbContext currentContext,
      IReadOnlyModel snapshotModel) {
    ArgumentNullException.ThrowIfNull(expectedImport);
    ArgumentNullException.ThrowIfNull(currentContext);
    ArgumentNullException.ThrowIfNull(snapshotModel);

    var providerCapabilities = DataVaultProviderCapabilityProfileSelection.Select(currentContext.Database.ProviderName);
    var runtimeModel = currentContext.Model;

    return new DataVaultModelDriftPreflightReport(
        DataVaultModelDriftReporter.Compare(expectedImport, runtimeModel, providerCapabilities),
        DataVaultModelDriftReporter.Compare(expectedImport, snapshotModel, providerCapabilities),
        DataVaultModelDriftReporter.CompareModels(runtimeModel, snapshotModel));
  }
}
