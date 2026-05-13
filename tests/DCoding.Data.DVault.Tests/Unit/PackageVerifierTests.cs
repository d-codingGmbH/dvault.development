using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using DCoding.Data.DVault.PackageVerification;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class PackageVerifierTests {
  private const string CorePackageId = "DCoding.Data.DVault";
  private const string PackageVersion = "1.2.3";
  private const string ReadmeInstallVersion = PackageVersion;
  private const string TargetFramework = "net10.0";
  private const string Authors = "d-coding GmbH";
  private const string RepositoryUrl = "https://github.com/d-codingGmbH/dvault.development.git";
  private static readonly XNamespace NuspecNamespace = "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd";

  private static readonly IReadOnlyList<TestPackageDefinition> PackageDefinitions = [
      new(
          CorePackageId,
          "DVault",
          "Convention-first .NET 10 library extending Entity Framework for Data Vault 2.x-oriented persistence.",
          ["dvault", "data-vault", "data-modeling", "dotnet", "entity-framework", "ef-core", "data-vault-2", "persistence"],
          false),
      new(
          "DCoding.Data.DVault.MySql",
          "DVault MySQL Provider Extensions",
          "MySQL provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "mysql", "ef-core", "persistence"],
          true),
      new(
          "DCoding.Data.DVault.Oracle",
          "DVault Oracle Provider Extensions",
          "Oracle provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "oracle", "ef-core", "persistence"],
          true),
      new(
          "DCoding.Data.DVault.Postgres",
          "DVault PostgreSQL Provider Extensions",
          "PostgreSQL provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "postgresql", "postgres", "ef-core", "persistence"],
          true),
      new(
          "DCoding.Data.DVault.Sqlite",
          "DVault SQLite Provider Extensions",
          "SQLite provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "sqlite", "ef-core", "persistence"],
          true),
      new(
          "DCoding.Data.DVault.SqlServer",
          "DVault SQL Server Provider Extensions",
          "SQL Server provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "sql-server", "ef-core", "persistence"],
          true),
  ];

  [Fact]
  public void PassingPackageMatrixSucceeds() {
    using var packageDirectory = PackageDirectory.Create();
    WritePackageMatrix(packageDirectory.Path);

    var result = Verify(packageDirectory.Path);

    Assert.True(result.Succeeded, FormatIssues(result));
  }

  [Fact]
  public void MissingPackageArtifactFailsWithPackageName() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.Sqlite"].WritePackage = false;
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.Sqlite" &&
            issue.Message.Contains("Missing expected .nupkg artifact", StringComparison.Ordinal));
  }

  [Fact]
  public void UnexpectedNonPackablePackageArtifactFailsWithPackageName() {
    using var packageDirectory = PackageDirectory.Create();
    WritePackageMatrix(packageDirectory.Path);
    WritePackage(
        packageDirectory.Path,
        new TestPackageDefinition(
            "DCoding.Data",
            "DCoding.Data",
            "Non-packable source-root build anchor for the DCoding.Data namespace family.",
            ["dvault"],
            false),
        new PackageArchiveOptions());

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data" &&
            issue.Message.Contains("Unexpected .nupkg artifact", StringComparison.Ordinal));
  }

  [Fact]
  public void StaleExpectedPackageVersionFailsAsDuplicateArtifact() {
    using var packageDirectory = PackageDirectory.Create();
    WritePackageMatrix(packageDirectory.Path);
    WritePackage(
        packageDirectory.Path,
        PackageDefinitions.Single(package => package.Id == CorePackageId),
        new PackageArchiveOptions { Version = "1.2.2" });

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == CorePackageId &&
            issue.Message.Contains("Expected exactly one .nupkg artifact", StringComparison.Ordinal));
  }

  [Fact]
  public void MissingReadmeFailsWithPackageName() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options[CorePackageId].IncludeReadme = false;
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == CorePackageId &&
            issue.Message.Contains("missing root README.md", StringComparison.Ordinal));
  }

  [Fact]
  public void MissingXmlDocumentationFailsWithPackageName() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.Postgres"].IncludeXmlDocumentation = false;
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.Postgres" &&
            issue.Message.Contains("missing generated XML documentation", StringComparison.Ordinal));
  }

  [Fact]
  public void MissingSymbolPdbFailsWithPackageName() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.SqlServer"].IncludeSymbolPdb = false;
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.SqlServer" &&
            issue.Message.Contains("missing expected PDB entry", StringComparison.Ordinal));
  }

  [Fact]
  public void IncorrectMetadataFailsWithActionableMessage() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.Oracle"].Authors = "Wrong Author";
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.Oracle" &&
            issue.Message.Contains("Nuspec metadata 'authors'", StringComparison.Ordinal) &&
            issue.Message.Contains("d-coding GmbH", StringComparison.Ordinal));
  }

  [Fact]
  public void ProviderDependencyMustMatchPackedCoreVersion() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.MySql"].CoreDependencyVersion = "9.9.9";
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.MySql" &&
            issue.Message.Contains("uses version '9.9.9'", StringComparison.Ordinal) &&
            issue.Message.Contains("expected packed core version '" + PackageVersion + "'", StringComparison.Ordinal));
  }

  [Fact]
  public void OracleProjectDoesNotReferenceNonOracleProviderPackages() {
    var projectPath = GetRepositoryPath(
        "src/DCoding.Data.DVault.Oracle/DCoding.Data.DVault.Oracle.csproj");
    var project = XDocument.Load(projectPath);
    var packageReferences = project
        .Descendants("PackageReference")
        .Select(reference => Assert.IsType<string>(reference.Attribute("Include")?.Value))
        .Order(StringComparer.Ordinal)
        .ToArray();
    var projectReferences = project
        .Descendants("ProjectReference")
        .Select(reference => Path.GetFileNameWithoutExtension(Assert.IsType<string>(reference.Attribute("Include")?.Value)))
        .Order(StringComparer.Ordinal)
        .ToArray();

    Assert.Equal(["Microsoft.Extensions.DependencyInjection.Abstractions"], packageReferences);
    Assert.Equal(["DCoding.Data.DVault"], projectReferences);
    Assert.DoesNotContain(packageReferences, IsNonOracleDatabaseProviderReference);
    Assert.DoesNotContain(projectReferences, reference =>
        !string.Equals(reference, "DCoding.Data.DVault", StringComparison.Ordinal));
  }

  private static PackageVerificationResult Verify(string packageDirectory) {
    return new PackageVerifier().Verify(new PackageVerificationOptions(packageDirectory));
  }

  private static string GetRepositoryPath(string repositoryRelativePath) {
    var directory = new DirectoryInfo(AppContext.BaseDirectory);
    while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "DVault.slnx"))) {
      directory = directory.Parent;
    }

    Assert.NotNull(directory);

    return Path.Combine(directory!.FullName, repositoryRelativePath.Replace('/', Path.DirectorySeparatorChar));
  }

  private static bool IsNonOracleDatabaseProviderReference(string packageId) {
    return packageId.Contains("Sqlite", StringComparison.OrdinalIgnoreCase) ||
        packageId.Contains("Npgsql", StringComparison.OrdinalIgnoreCase) ||
        packageId.Contains("PostgreSQL", StringComparison.OrdinalIgnoreCase) ||
        packageId.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) ||
        packageId.Contains("MySql", StringComparison.OrdinalIgnoreCase);
  }

  private static Dictionary<string, PackageArchiveOptions> CreatePackageOptions() {
    return PackageDefinitions.ToDictionary(
        package => package.Id,
        _ => new PackageArchiveOptions(),
        StringComparer.Ordinal);
  }

  private static void WritePackageMatrix(
      string packageDirectory,
      IReadOnlyDictionary<string, PackageArchiveOptions>? options = null) {
    options ??= CreatePackageOptions();

    foreach (var package in PackageDefinitions) {
      var packageOptions = options[package.Id];
      if (packageOptions.WritePackage) {
        WritePackage(packageDirectory, package, packageOptions);
      }

      if (packageOptions.WriteSymbols) {
        WriteSymbolsPackage(packageDirectory, package, packageOptions);
      }
    }
  }

  private static void WritePackage(
      string packageDirectory,
      TestPackageDefinition package,
      PackageArchiveOptions options) {
    var filePath = Path.Combine(packageDirectory, package.Id + "." + options.Version + ".nupkg");
    using var archive = ZipFile.Open(filePath, ZipArchiveMode.Create);

    WriteTextEntry(archive, package.Id + ".nuspec", CreateNuspec(package, options));
    if (options.IncludeReadme) {
      WriteTextEntry(
          archive,
          "README.md",
          string.Join(
              "\n",
              PackageDefinitions.Select(package =>
                  "dotnet add package " + package.Id + " --version " + ReadmeInstallVersion)) + "\n");
    }

    if (options.IncludeXmlDocumentation) {
      WriteTextEntry(archive, "lib/" + TargetFramework + "/" + package.Id + ".xml", "<doc />\n");
    }
  }

  private static void WriteSymbolsPackage(
      string packageDirectory,
      TestPackageDefinition package,
      PackageArchiveOptions options) {
    var filePath = Path.Combine(packageDirectory, package.Id + "." + options.Version + ".snupkg");
    using var archive = ZipFile.Open(filePath, ZipArchiveMode.Create);

    WriteTextEntry(archive, package.Id + ".nuspec", CreateNuspec(package, options));
    if (options.IncludeSymbolPdb) {
      WriteBinaryEntry(archive, "lib/" + TargetFramework + "/" + package.Id + ".pdb", [1, 2, 3]);
    }
  }

  private static string CreateNuspec(TestPackageDefinition package, PackageArchiveOptions options) {
    var metadata = new XElement(
        NuspecNamespace + "metadata",
        new XElement(NuspecNamespace + "id", package.Id),
        new XElement(NuspecNamespace + "version", options.Version),
        new XElement(NuspecNamespace + "title", package.Title),
        new XElement(NuspecNamespace + "authors", options.Authors),
        new XElement(
            NuspecNamespace + "license",
            new XAttribute("type", "expression"),
            "Apache-2.0"),
        new XElement(NuspecNamespace + "readme", "README.md"),
        new XElement(NuspecNamespace + "description", package.Description),
        new XElement(NuspecNamespace + "tags", string.Join(" ", package.Tags)),
        new XElement(
            NuspecNamespace + "repository",
            new XAttribute("type", "git"),
            new XAttribute("url", RepositoryUrl)),
        new XElement(
            NuspecNamespace + "dependencies",
            new XElement(
                NuspecNamespace + "group",
                new XAttribute("targetFramework", TargetFramework))));

    if (package.IsProvider) {
      metadata
          .Element(NuspecNamespace + "dependencies")!
          .Element(NuspecNamespace + "group")!
          .Add(new XElement(
              NuspecNamespace + "dependency",
              new XAttribute("id", CorePackageId),
              new XAttribute("version", options.CoreDependencyVersion ?? options.Version)));
    }

    var document = new XDocument(
        new XDeclaration("1.0", "utf-8", null),
        new XElement(NuspecNamespace + "package", metadata));

    return document.ToString(SaveOptions.DisableFormatting);
  }

  private static void WriteTextEntry(ZipArchive archive, string entryName, string content) {
    var entry = archive.CreateEntry(entryName);
    using var stream = entry.Open();
    using var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    writer.Write(content);
  }

  private static void WriteBinaryEntry(ZipArchive archive, string entryName, byte[] content) {
    var entry = archive.CreateEntry(entryName);
    using var stream = entry.Open();
    stream.Write(content);
  }

  private static string FormatIssues(PackageVerificationResult result) {
    return string.Join(Environment.NewLine, result.Issues.Select(issue => issue.ToString()));
  }

  private sealed class PackageDirectory : IDisposable {
    private PackageDirectory(string path) {
      Path = path;
    }

    public string Path { get; }

    public static PackageDirectory Create() {
      var path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "dvault-package-verification-" + Guid.NewGuid().ToString("N"));
      Directory.CreateDirectory(path);
      return new PackageDirectory(path);
    }

    public void Dispose() {
      if (Directory.Exists(Path)) {
        Directory.Delete(Path, recursive: true);
      }
    }
  }

  private sealed record TestPackageDefinition(
      string Id,
      string Title,
      string Description,
      string[] Tags,
      bool IsProvider);

  private sealed class PackageArchiveOptions {
    public bool WritePackage { get; set; } = true;

    public bool WriteSymbols { get; set; } = true;

    public bool IncludeReadme { get; set; } = true;

    public bool IncludeXmlDocumentation { get; set; } = true;

    public bool IncludeSymbolPdb { get; set; } = true;

    public string Version { get; set; } = PackageVersion;

    public string Authors { get; set; } = PackageVerifierTests.Authors;

    public string? CoreDependencyVersion { get; set; }
  }
}
