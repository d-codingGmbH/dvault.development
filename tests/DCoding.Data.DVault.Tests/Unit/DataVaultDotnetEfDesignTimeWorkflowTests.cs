using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultDotnetEfDesignTimeWorkflowTests {
  private const string CoreProjectPath = "src/DCoding.Data.DVault/DCoding.Data.DVault.csproj";
  private const string LogicalSourcePath = "models/sales-vault.json";
  private const string WorkflowDocumentPath = "docs/architecture/dvault-dotnet-ef-design-time-workflow.md";

  [Fact]
  public void DocumentationDefinesOneConsumerOwnedFactoryWorkflow() {
    var document = ReadRepositoryFile(WorkflowDocumentPath);

    Assert.Contains("IDesignTimeDbContextFactory<TContext>", document, StringComparison.Ordinal);
    Assert.Contains("consumer-owned", document, StringComparison.OrdinalIgnoreCase);
    Assert.Contains("single project", document, StringComparison.OrdinalIgnoreCase);
    Assert.Contains("unsupported in v1", document, StringComparison.OrdinalIgnoreCase);
    Assert.Contains("Startup-project and target-project splits", document, StringComparison.Ordinal);
    Assert.Contains("IDataVaultDiagnosticsService.Analyze(DbContext)", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultDiagnosticsResult.ToDisplayString()", document, StringComparison.Ordinal);
    Assert.Contains("support-bundle", document, StringComparison.Ordinal);
    Assert.Contains("dvault.support-bundle.v1", document, StringComparison.Ordinal);
    Assert.Contains("CreateSupportBundleDiagnostics", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultLiveSchemaReadResult", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultModelDriftReport", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultModelDriftPreflightReporter.Compare", document, StringComparison.Ordinal);
    Assert.Contains("IReadOnlyModel snapshotModel", document, StringComparison.Ordinal);
    Assert.Contains("MetadataVersusRuntime", document, StringComparison.Ordinal);
    Assert.Contains("RuntimeVersusSnapshotModel", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultMigrationOperationDiagnostics.AnalyzeReport", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultMigrationGuardrailReport.ToDisplayString()", document, StringComparison.Ordinal);
    Assert.Contains("dotnet ef migrations add", document, StringComparison.Ordinal);
    Assert.Contains("dotnet ef database update", document, StringComparison.Ordinal);
    Assert.Contains("does not provide `IDesignTimeServices`", document, StringComparison.Ordinal);
    Assert.Contains("does not provide a custom `dotnet ef` shim", document, StringComparison.Ordinal);
    Assert.Contains("does not reference `Microsoft.EntityFrameworkCore.Design`", document, StringComparison.Ordinal);
  }

  [Fact]
  public void CoreDvaultProjectRemainsDesignPackageFree() {
    var project = ReadRepositoryFile(CoreProjectPath);

    Assert.DoesNotContain("Microsoft.EntityFrameworkCore.Design", project, StringComparison.Ordinal);
  }

  [Fact]
  public void ConsumerOwnedFactoryContextSupportsDiagnosticsAndMigrationGuardrailPreflightWithoutLiveDatabase() {
    using var serviceProvider = new ServiceCollection()
        .AddDVault()
        .BuildServiceProvider(validateScopes: true);
    var diagnostics = serviceProvider.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new SalesVaultDesignTimeFactory().CreateDbContext([]);

    var result = diagnostics.Analyze(context);

    Assert.True(result.Validation.IsValid, result.ToDisplayString());
    Assert.Contains("DVault diagnostics: valid", result.ToDisplayString(), StringComparison.Ordinal);

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(
        diagnostics,
        context,
        [new DropTableOperation { Name = "HubCustomer" }]);
    var reportText = report.ToDisplayString();

    Assert.True(report.HasFindings);
    Assert.False(report.IsValid);
    Assert.Contains("DVault migration guardrails: invalid", reportText, StringComparison.Ordinal);
    Assert.Contains("DVM2006", reportText, StringComparison.Ordinal);
  }

  private static string ReadRepositoryFile(string relativePath) {
    var directory = new DirectoryInfo(AppContext.BaseDirectory);

    while (directory is not null) {
      if (File.Exists(Path.Combine(directory.FullName, "DVault.slnx"))) {
        return File.ReadAllText(Path.Combine(directory.FullName, relativePath));
      }

      directory = directory.Parent;
    }

    throw new InvalidOperationException("Unable to locate the DVault repository root from the test output directory.");
  }

  private sealed class SalesVaultDesignTimeFactory {
    public SalesVaultDesignTimeContext CreateDbContext(string[] args) {
      ArgumentNullException.ThrowIfNull(args);

      var importResult = DataVaultModelArtifactImporter.ImportJson(
          ReadRepositoryFile(LogicalSourcePath),
          LogicalSourcePath);
      if (!importResult.IsValid) {
        throw new InvalidOperationException(DataVaultModelImportResult.FormatDiagnostics(importResult.Diagnostics));
      }

      var optionsBuilder = new DbContextOptionsBuilder<SalesVaultDesignTimeContext>()
          .UseSqlite("Data Source=:memory:");
      optionsBuilder.UseDataVaultMetadata(importResult);

      return new SalesVaultDesignTimeContext(optionsBuilder.Options);
    }
  }

  private sealed class SalesVaultDesignTimeContext(DbContextOptions<SalesVaultDesignTimeContext> options) : DbContext(options) {
  }
}
