using System.Xml.Linq;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class EfCoreProviderVersionMatrixTests {
  [Fact]
  public void CoreProjectPinsEfCorePackageLineForEachSupportedTargetFramework() {
    var project = LoadProject("src/DCoding.Data.DVault/DCoding.Data.DVault.csproj");

    AssertTargetFrameworks(project, ["net8.0", "net10.0"]);
    AssertPackageReferences(
        project,
        "net8.0",
        [
            new("Microsoft.EntityFrameworkCore", "8.0.27"),
            new("Microsoft.EntityFrameworkCore.Relational", "8.0.27"),
            new("Microsoft.Extensions.DependencyInjection.Abstractions", "8.0.2"),
        ]);
    AssertPackageReferences(
        project,
        "net10.0",
        [
            new("Microsoft.EntityFrameworkCore", "10.0.8"),
            new("Microsoft.EntityFrameworkCore.Relational", "10.0.8"),
            new("Microsoft.Extensions.DependencyInjection.Abstractions", "10.0.8"),
        ]);
  }

  [Fact]
  public void IntegrationProjectPinsProviderPackageMatrixWithoutChangingOptInGates() {
    var project = LoadProject("tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj");

    AssertTargetFrameworks(project, ["net8.0", "net10.0"]);
    AssertProviderPackageReferences(
        project,
        "net8.0",
        [
            new("Microsoft.EntityFrameworkCore.Sqlite", "8.0.27"),
            new("IBM.EntityFrameworkCore", "8.0.0.400", "'$(DVAULT_TEST_DB2_CONNECTION_STRING)' != ''"),
            new("MySql.EntityFrameworkCore", "10.0.7", "'$(DVAULT_TEST_MYSQL_CONNECTION_STRING)' != ''"),
            new("Npgsql.EntityFrameworkCore.PostgreSQL", "8.0.11", "'$(DVAULT_TEST_POSTGRES_CONNECTION_STRING)' != ''"),
            new("Oracle.EntityFrameworkCore", "8.23.26200", "'$(DVAULT_TEST_ORACLE_CONNECTION_STRING)' != ''"),
            new("Microsoft.EntityFrameworkCore.SqlServer", "8.0.27", "'$(DVAULT_TEST_SQLSERVER_CONNECTION_STRING)' != ''"),
        ]);
    AssertProviderPackageReferences(
        project,
        "net10.0",
        [
            new("Microsoft.EntityFrameworkCore.Sqlite", "10.0.8"),
            new("IBM.EntityFrameworkCore", "10.0.0.100", "'$(DVAULT_TEST_DB2_CONNECTION_STRING)' != ''"),
            new("MySql.EntityFrameworkCore", "10.0.7", "'$(DVAULT_TEST_MYSQL_CONNECTION_STRING)' != ''"),
            new("Npgsql.EntityFrameworkCore.PostgreSQL", "10.0.2", "'$(DVAULT_TEST_POSTGRES_CONNECTION_STRING)' != ''"),
            new("Oracle.EntityFrameworkCore", "10.23.26200", "'$(DVAULT_TEST_ORACLE_CONNECTION_STRING)' != ''"),
            new("Microsoft.EntityFrameworkCore.SqlServer", "10.0.8", "'$(DVAULT_TEST_SQLSERVER_CONNECTION_STRING)' != ''"),
        ]);
    AssertCompileRemoved(project, "net8.0", "BenchmarkScenarioExecutionTests.cs");
    AssertProjectReferenceCondition(
        project,
        "../../../benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj",
        "'$(TargetFramework)' == 'net10.0'");
    AssertProjectReferenceCondition(
        project,
        "../../../src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj",
        "'$(DVAULT_TEST_DB2_CONNECTION_STRING)' != ''");
  }

  [Fact]
  public void UnitProjectPinsSqliteMatrixAndKeepsPackageVerifierOffNet8CompilePath() {
    var project = LoadProject("tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj");

    AssertTargetFrameworks(project, ["net8.0", "net10.0"]);
    AssertPackageReference(project, "net8.0", new("Microsoft.EntityFrameworkCore.Sqlite", "8.0.27"));
    AssertPackageReference(project, "net8.0", new("Microsoft.Extensions.DependencyInjection", "8.0.1"));
    AssertPackageReference(project, "net10.0", new("Microsoft.EntityFrameworkCore.Sqlite", "10.0.8"));
    AssertPackageReference(project, "net10.0", new("Microsoft.Extensions.DependencyInjection", "10.0.8"));
    AssertCompileRemoved(project, "net8.0", "PackageVerifierTests.cs");
    AssertProjectReferenceCondition(
        project,
        "../../../tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj",
        "'$(TargetFramework)' == 'net10.0'");
  }

  [Fact]
  public void Db2ProviderProjectPinsIbmProviderPackageLineForEachSupportedTargetFramework() {
    var project = LoadProject("src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj");

    AssertTargetFrameworks(project, ["net8.0", "net10.0"]);
    AssertPackageReferences(
        project,
        "net8.0",
        [
            new("IBM.EntityFrameworkCore", "8.0.0.400"),
            new("Microsoft.Extensions.DependencyInjection.Abstractions", "8.0.2"),
        ]);
    AssertPackageReferences(
        project,
        "net10.0",
        [
            new("IBM.EntityFrameworkCore", "10.0.0.100"),
            new("Microsoft.Extensions.DependencyInjection.Abstractions", "10.0.8"),
        ]);
  }

  private static XDocument LoadProject(string repositoryRelativePath) {
    return XDocument.Load(GetRepositoryPath(repositoryRelativePath));
  }

  private static void AssertTargetFrameworks(XDocument project, IReadOnlyList<string> expectedTargetFrameworks) {
    var targetFrameworks = project
        .Root!
        .Elements("PropertyGroup")
        .Elements("TargetFrameworks")
        .SingleOrDefault()?
        .Value;

    Assert.Equal(string.Join(';', expectedTargetFrameworks), targetFrameworks);
  }

  private static void AssertProviderPackageReferences(
      XDocument project,
      string targetFramework,
      IReadOnlyList<ExpectedPackageReference> expectedReferences) {
    var expectedReferenceIds = expectedReferences
        .Select(reference => reference.Include)
        .Order(StringComparer.Ordinal)
        .ToArray();
    var actualReferenceIds = GetPackageReferences(project, targetFramework)
        .Where(reference => IsProviderMatrixReference(reference.Include))
        .Select(reference => reference.Include)
        .Order(StringComparer.Ordinal)
        .ToArray();

    Assert.Equal(expectedReferenceIds, actualReferenceIds);
    AssertPackageReferences(project, targetFramework, expectedReferences);
  }

  private static void AssertPackageReferences(
      XDocument project,
      string targetFramework,
      IReadOnlyList<ExpectedPackageReference> expectedReferences) {
    foreach (var expectedReference in expectedReferences) {
      AssertPackageReference(project, targetFramework, expectedReference);
    }
  }

  private static void AssertPackageReference(
      XDocument project,
      string targetFramework,
      ExpectedPackageReference expectedReference) {
    var actualReference = GetPackageReferences(project, targetFramework)
        .SingleOrDefault(reference => string.Equals(reference.Include, expectedReference.Include, StringComparison.Ordinal));

    Assert.True(
        actualReference is not null,
        "Missing PackageReference '" + expectedReference.Include + "' for target framework '" + targetFramework + "'.");
    Assert.True(
        string.Equals(actualReference!.Version, expectedReference.Version, StringComparison.Ordinal),
        "PackageReference '" + expectedReference.Include + "' for target framework '" + targetFramework +
        "' expected version '" + expectedReference.Version + "' but found '" + actualReference.Version + "'.");
    Assert.True(
        string.Equals(actualReference.Condition, expectedReference.Condition, StringComparison.Ordinal),
        "PackageReference '" + expectedReference.Include + "' for target framework '" + targetFramework +
        "' expected condition '" + expectedReference.Condition + "' but found '" + actualReference.Condition + "'.");
  }

  private static void AssertCompileRemoved(XDocument project, string targetFramework, string expectedRemovedFile) {
    var removedFiles = GetTargetItemGroups(project, targetFramework)
        .SelectMany(group => group.Elements("Compile"))
        .Select(element => element.Attribute("Remove")?.Value ?? string.Empty)
        .ToArray();

    Assert.Contains(expectedRemovedFile, removedFiles);
  }

  private static void AssertProjectReferenceCondition(
      XDocument project,
      string expectedInclude,
      string expectedCondition) {
    var projectReference = project
        .Descendants("ProjectReference")
        .SingleOrDefault(reference => string.Equals(reference.Attribute("Include")?.Value, expectedInclude, StringComparison.Ordinal));

    Assert.True(projectReference is not null, "Missing ProjectReference '" + expectedInclude + "'.");
    Assert.Equal(expectedCondition, projectReference!.Attribute("Condition")?.Value ?? string.Empty);
  }

  private static IReadOnlyList<ProjectPackageReference> GetPackageReferences(
      XDocument project,
      string targetFramework) {
    return GetTargetItemGroups(project, targetFramework)
        .SelectMany(group => group.Elements("PackageReference"))
        .Select(reference => new ProjectPackageReference(
            Assert.IsType<string>(reference.Attribute("Include")?.Value),
            Assert.IsType<string>(reference.Attribute("Version")?.Value),
            reference.Attribute("Condition")?.Value ?? string.Empty))
        .ToArray();
  }

  private static IReadOnlyList<XElement> GetTargetItemGroups(XDocument project, string targetFramework) {
    var expectedCondition = "'$(TargetFramework)' == '" + targetFramework + "'";
    return project
        .Root!
        .Elements("ItemGroup")
        .Where(group => string.Equals(group.Attribute("Condition")?.Value, expectedCondition, StringComparison.Ordinal))
        .ToArray();
  }

  private static bool IsProviderMatrixReference(string packageId) {
    return string.Equals(packageId, "Microsoft.EntityFrameworkCore.Sqlite", StringComparison.Ordinal) ||
        string.Equals(packageId, "IBM.EntityFrameworkCore", StringComparison.Ordinal) ||
        string.Equals(packageId, "MySql.EntityFrameworkCore", StringComparison.Ordinal) ||
        string.Equals(packageId, "Npgsql.EntityFrameworkCore.PostgreSQL", StringComparison.Ordinal) ||
        string.Equals(packageId, "Oracle.EntityFrameworkCore", StringComparison.Ordinal) ||
        string.Equals(packageId, "Microsoft.EntityFrameworkCore.SqlServer", StringComparison.Ordinal);
  }

  private static string GetRepositoryPath(string repositoryRelativePath) {
    var directory = new DirectoryInfo(AppContext.BaseDirectory);
    while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "DVault.slnx"))) {
      directory = directory.Parent;
    }

    Assert.NotNull(directory);

    return Path.Combine(directory!.FullName, repositoryRelativePath.Replace('/', Path.DirectorySeparatorChar));
  }

  private sealed record ExpectedPackageReference(
      string Include,
      string Version,
      string Condition = "");

  private sealed record ProjectPackageReference(
      string Include,
      string Version,
      string Condition);
}
