using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DCoding.Data.DVault;

/// <summary>
/// Supplies the consumer-owned dependencies used by the reusable DVault design-time command runner.
/// </summary>
public sealed class DataVaultDesignTimeCommandHost {
  /// <summary>
  /// Initializes a new design-time command host.
  /// </summary>
  /// <param name="diagnostics">The diagnostics service used for validation and migration guardrail baselines.</param>
  /// <param name="createDbContext">The consumer-owned factory for the configured design-time DbContext.</param>
  /// <param name="exportSource">The explicit metadata source used by the export command.</param>
  /// <param name="resolveMigrationOperations">The consumer-owned resolver for scaffolded migration UpOperations by migration name.</param>
  public DataVaultDesignTimeCommandHost(
      IDataVaultDiagnosticsService diagnostics,
      Func<DbContext> createDbContext,
      DataVaultDesignTimeExportSource exportSource,
      Func<string, IEnumerable<MigrationOperation>> resolveMigrationOperations) {
    ArgumentNullException.ThrowIfNull(diagnostics);
    ArgumentNullException.ThrowIfNull(createDbContext);
    ArgumentNullException.ThrowIfNull(exportSource);
    ArgumentNullException.ThrowIfNull(resolveMigrationOperations);

    Diagnostics = diagnostics;
    CreateDbContext = createDbContext;
    ExportSource = exportSource;
    ResolveMigrationOperations = resolveMigrationOperations;
  }

  /// <summary>
  /// Gets the diagnostics service used for validation and migration guardrail baselines.
  /// </summary>
  public IDataVaultDiagnosticsService Diagnostics { get; }

  /// <summary>
  /// Gets the consumer-owned factory for the configured design-time DbContext.
  /// </summary>
  public Func<DbContext> CreateDbContext { get; }

  /// <summary>
  /// Gets an optional consumer-owned factory for support-bundle diagnostics with caller-supplied request context.
  /// </summary>
  public Func<DbContext, DataVaultDiagnosticsResult>? CreateSupportBundleDiagnostics { get; init; }

  /// <summary>
  /// Gets the explicit metadata source used by the export command.
  /// </summary>
  public DataVaultDesignTimeExportSource ExportSource { get; }

  /// <summary>
  /// Gets the consumer-owned resolver for scaffolded migration UpOperations by migration name.
  /// </summary>
  public Func<string, IEnumerable<MigrationOperation>> ResolveMigrationOperations { get; }

  /// <summary>
  /// Gets the optional live-schema reader used by the opt-in live drift command lane.
  /// </summary>
  public IDataVaultLiveSchemaReader? LiveSchemaReader { get; init; }
}
