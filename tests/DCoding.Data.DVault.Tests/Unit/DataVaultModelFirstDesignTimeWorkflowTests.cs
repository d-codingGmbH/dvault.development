using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultModelFirstDesignTimeWorkflowTests {
  private const string LogicalSourcePath = "models/sales-vault.json";

  [Fact]
  public void ValidArtifactProjectsIntoSqliteDesignTimeMetadataWithoutBlockingDrift() {
    var importResult = DataVaultModelArtifactImporter.ImportJson(
        ReadRepositoryFile(LogicalSourcePath),
        LogicalSourcePath);

    Assert.True(importResult.IsValid, DataVaultModelImportResult.FormatDiagnostics(importResult.Diagnostics));

    var optionsBuilder = new DbContextOptionsBuilder<SalesVaultDesignTimeContext>()
        .UseSqlite("Data Source=:memory:");
    optionsBuilder.UseDataVaultMetadata(importResult);

    using var context = new SalesVaultDesignTimeContext(optionsBuilder.Options);

    var report = DataVaultModelDriftReporter.Compare(importResult, context);

    Assert.False(report.HasBlockingDifferences, report.ToDisplayString());
  }

  [Fact]
  public void UnsupportedSchemaVersionExposesSourceScopedDmv1002Diagnostic() {
    var importResult = DataVaultModelArtifactImporter.ImportJson(
        """
        {
          "schemaVersion": "dvault.model.v2"
        }
        """,
        LogicalSourcePath);

    Assert.False(importResult.IsValid);
    var diagnostic = Assert.Single(importResult.Diagnostics);

    Assert.Equal("DMV1002", diagnostic.Code);
    Assert.Equal("schema-version", diagnostic.Category);
    Assert.Equal(LogicalSourcePath, diagnostic.LogicalSourcePath);
    Assert.Equal("/schemaVersion", diagnostic.JsonPointer);
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

  private sealed class SalesVaultDesignTimeContext(DbContextOptions<SalesVaultDesignTimeContext> options) : DbContext(options) {
  }
}
