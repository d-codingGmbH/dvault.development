using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using DCoding.Data.DVault.PackageVerification;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class PackageVerifierTests {
  private const string CorePackageId = "DCoding.Data.DVault";
  private const string Db2PackageId = "DCoding.Data.DVault.Db2";
  private const string IbmEntityFrameworkCorePackageId = "IBM.EntityFrameworkCore";
  private const string Net8PackageLineVersion = "8.36.0";
  private const string Net10PackageLineVersion = "10.36.0";
  private const string Net8TargetFramework = "net8.0";
  private const string Net10TargetFramework = "net10.0";
  private const string Authors = "d-coding GmbH";
  private const string RepositoryUrl = "https://github.com/d-codingGmbH/dvault.development.git";
  private const string ExpectedAnalyzerBuildHostGuidance = "Build projects that reference `DCoding.Data.DVault.Analyzers` with a `.NET 10 SDK` host, including `net8.0` projects using the `8.36.0` package line.";
  private static readonly XNamespace NuspecNamespace = "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd";
  private static readonly PackageLine[] PackageLines = [
      new(Net8PackageLineVersion, Net8TargetFramework, "EF Core 8"),
      new(Net10PackageLineVersion, Net10TargetFramework, "EF Core 10"),
  ];

  private static readonly IReadOnlyList<TestPackageDefinition> PackageDefinitions = [
      new(
          CorePackageId,
          "DVault",
          "Convention-first .NET library extending Entity Framework Core for Data Vault 2.x-oriented persistence.",
          ["dvault", "data-vault", "data-modeling", "dotnet", "entity-framework", "ef-core", "data-vault-2", "persistence"],
          false,
          false),
      new(
          "DCoding.Data.DVault.Analyzers",
          "DVault Roslyn Analyzers",
          "Roslyn analyzers and source generators for high-confidence DVault compile-time metadata declarations.",
          ["dvault", "data-vault", "roslyn", "analyzer", "source-generator", "diagnostics", "ef-core"],
          false,
          true),
      new(
          Db2PackageId,
          "DVault DB2 Provider Extensions",
          "DB2 provider extensions and optimized save/read strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "db2", "ibm", "ef-core", "persistence"],
          true,
          false,
          false,
          true),
      new(
          "DCoding.Data.DVault.MySql",
          "DVault MySQL Provider Extensions",
          "MySQL provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "mysql", "ef-core", "persistence"],
          true,
          false),
      new(
          "DCoding.Data.DVault.Oracle",
          "DVault Oracle Provider Extensions",
          "Oracle provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "oracle", "ef-core", "persistence"],
          true,
          false),
      new(
          "DCoding.Data.DVault.Postgres",
          "DVault PostgreSQL Provider Extensions",
          "PostgreSQL provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "postgresql", "postgres", "ef-core", "persistence"],
          true,
          false,
          true),
      new(
          "DCoding.Data.DVault.Sqlite",
          "DVault SQLite Provider Extensions",
          "SQLite provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "sqlite", "ef-core", "persistence"],
          true,
          false,
          true),
      new(
          "DCoding.Data.DVault.SqlServer",
          "DVault SQL Server Provider Extensions",
          "SQL Server provider extensions and optimized write strategies for DCoding.Data.DVault.",
          ["dvault", "data-vault", "sql-server", "ef-core", "persistence"],
          true,
          false,
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
          false,
          false),
        new PackageArchiveOptions(),
        PackageLines[0]);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data" &&
            issue.Message.Contains("Unexpected .nupkg artifact", StringComparison.Ordinal));
  }

  [Fact]
  public void StaleExpectedPackageVersionFailsAsUnexpectedArtifact() {
    using var packageDirectory = PackageDirectory.Create();
    WritePackageMatrix(packageDirectory.Path);
    WritePackage(
        packageDirectory.Path,
        PackageDefinitions.Single(package => package.Id == CorePackageId),
        new PackageArchiveOptions(),
        new PackageLine("1.2.2", Net8TargetFramework, "EF Core 8"));

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == CorePackageId &&
            issue.Message.Contains("Unexpected package version '1.2.2'", StringComparison.Ordinal));
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
  public void RuntimeReadmeMustContainBothPackageLineInstallGuides() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options[CorePackageId].ReadmeText =
        CreateRuntimePackageReadme([new PackageLine(Net8PackageLineVersion, Net8TargetFramework, "EF Core 8")]);
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == CorePackageId &&
            issue.Message.Contains("net10.0 / EF Core 10", StringComparison.Ordinal) &&
            issue.Message.Contains(Net10PackageLineVersion, StringComparison.Ordinal));
  }

  [Fact]
  public void AnalyzerReadmeMustContainPrivateAssetsGuidanceForBothPackageLines() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.Analyzers"].ReadmeText =
        CreateAnalyzerPackageReadme(
            "DCoding.Data.DVault.Analyzers",
            [new PackageLine(Net8PackageLineVersion, Net8TargetFramework, "EF Core 8")]);
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.Analyzers" &&
            issue.Message.Contains("net10.0 / EF Core 10", StringComparison.Ordinal) &&
            issue.Message.Contains(Net10PackageLineVersion, StringComparison.Ordinal) &&
            issue.Message.Contains("PrivateAssets", StringComparison.Ordinal));
  }

  [Fact]
  public void RuntimeReadmeMustStateAnalyzerBuildHostSdkBaseline() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options[CorePackageId].ReadmeText =
        CreateRuntimePackageReadme().Replace(ExpectedAnalyzerBuildHostGuidance, string.Empty, StringComparison.Ordinal);
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == CorePackageId &&
            issue.Message.Contains(".NET 10 SDK build-host baseline", StringComparison.Ordinal) &&
            issue.Message.Contains(Net8PackageLineVersion, StringComparison.Ordinal));
  }

  [Fact]
  public void AnalyzerReadmeMustStateAnalyzerBuildHostSdkBaseline() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.Analyzers"].ReadmeText =
        CreateAnalyzerPackageReadme("DCoding.Data.DVault.Analyzers")
            .Replace(ExpectedAnalyzerBuildHostGuidance, string.Empty, StringComparison.Ordinal);
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.Analyzers" &&
            issue.Message.Contains(".NET 10 SDK build-host baseline", StringComparison.Ordinal) &&
            issue.Message.Contains(Net8PackageLineVersion, StringComparison.Ordinal));
  }

  [Fact]
  public void RuntimeReadmeMustNotContradictAnalyzerBuildHostSdkBaseline() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options[CorePackageId].ReadmeText =
        CreateRuntimePackageReadme() +
        "Build projects that reference `DCoding.Data.DVault.Analyzers` with a `.NET 8 SDK` host.\n";
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == CorePackageId &&
            issue.Message.Contains("must not contradict the .NET 10 SDK build-host baseline", StringComparison.Ordinal) &&
            issue.Message.Contains(".NET 8 SDK", StringComparison.Ordinal));
  }

  [Fact]
  public void AnalyzerReadmeMustNotContradictAnalyzerBuildHostSdkBaseline() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.Analyzers"].ReadmeText =
        CreateAnalyzerPackageReadme("DCoding.Data.DVault.Analyzers") +
        "The current analyzer package supports pure `.NET 8 SDK` analyzer consumption.\n";
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.Analyzers" &&
            issue.Message.Contains("must not contradict the .NET 10 SDK build-host baseline", StringComparison.Ordinal) &&
            issue.Message.Contains("pure `.NET 8 SDK` analyzer consumption", StringComparison.Ordinal));
  }

  [Fact]
  public void ReadmeMustNotUseStaleOrPlanningReleaseInstallVersions() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options[CorePackageId].ReadmeText =
        CreateRuntimePackageReadme() +
        "dotnet add package DCoding.Data.DVault --version 0.33.0\n";
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == CorePackageId &&
            issue.Message.Contains("must not document stale or planning-release install version fragment", StringComparison.Ordinal) &&
            issue.Message.Contains("0.33.0", StringComparison.Ordinal));
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
            issue.Message.Contains("Dependency group 'net8.0'", StringComparison.Ordinal) &&
            issue.Message.Contains("uses version '9.9.9'", StringComparison.Ordinal) &&
            issue.Message.Contains("expected '" + Net8PackageLineVersion + "'", StringComparison.Ordinal));
  }

  [Fact]
  public void ProviderDependencyGroupMustMatchEfCoreLineForTargetFramework() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.Postgres"].OverrideDependencyVersion(
        Net8TargetFramework,
        "Microsoft.EntityFrameworkCore.Relational",
        "10.0.9");
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.Postgres" &&
            issue.Message.Contains("Dependency group 'net8.0'", StringComparison.Ordinal) &&
            issue.Message.Contains("Microsoft.EntityFrameworkCore.Relational", StringComparison.Ordinal) &&
            issue.Message.Contains("uses version '10.0.9'", StringComparison.Ordinal) &&
            issue.Message.Contains("expected '8.0.28'", StringComparison.Ordinal));
    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.Postgres" &&
            issue.Message.Contains("mixes EF Core lines", StringComparison.Ordinal) &&
            issue.Message.Contains("Microsoft.EntityFrameworkCore.Relational", StringComparison.Ordinal));
  }

  [Fact]
  public void Db2ProviderDependencyMustMatchTargetFrameworkProviderLine() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options[Db2PackageId].OverrideDependencyVersion(
        Net8TargetFramework,
        IbmEntityFrameworkCorePackageId,
        "10.0.0.100");
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == Db2PackageId &&
            issue.Message.Contains("Dependency group 'net8.0'", StringComparison.Ordinal) &&
            issue.Message.Contains(IbmEntityFrameworkCorePackageId, StringComparison.Ordinal) &&
            issue.Message.Contains("uses version '10.0.0.100'", StringComparison.Ordinal) &&
            issue.Message.Contains("expected '8.0.0.400'", StringComparison.Ordinal));
    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == Db2PackageId &&
            issue.Message.Contains("mixes EF Core lines", StringComparison.Ordinal) &&
            issue.Message.Contains(IbmEntityFrameworkCorePackageId, StringComparison.Ordinal));
  }

  [Fact]
  public void ProviderDependencyGroupMustMatchDependencyInjectionLineForTargetFramework() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options["DCoding.Data.DVault.MySql"].OverrideDependencyVersion(
        Net8TargetFramework,
        "Microsoft.Extensions.DependencyInjection.Abstractions",
        "10.0.9");
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == "DCoding.Data.DVault.MySql" &&
            issue.Message.Contains("Dependency group 'net8.0'", StringComparison.Ordinal) &&
            issue.Message.Contains("Microsoft.Extensions.DependencyInjection.Abstractions", StringComparison.Ordinal) &&
            issue.Message.Contains("uses version '10.0.9'", StringComparison.Ordinal) &&
            issue.Message.Contains("expected '8.0.2'", StringComparison.Ordinal));
  }

  [Fact]
  public void MissingDependencyGroupFailsWithTargetFrameworkName() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options[CorePackageId].OmittedDependencyGroups.Add(Net10TargetFramework);
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == CorePackageId &&
            issue.Message.Contains("missing the 'net10.0' dependency group", StringComparison.Ordinal));
  }

  [Fact]
  public void PackageLineMustNotCarryOtherTargetFrameworkDependencyGroups() {
    using var packageDirectory = PackageDirectory.Create();
    var options = CreatePackageOptions();
    options[CorePackageId].AdditionalDependencyGroups.Add(Net10TargetFramework);
    WritePackageMatrix(packageDirectory.Path, options);

    var result = Verify(packageDirectory.Path);

    Assert.Contains(
        result.Issues,
        issue => issue.PackageId == CorePackageId &&
            issue.Message.Contains("unexpected dependency group 'net10.0'", StringComparison.Ordinal) &&
            issue.Message.Contains(Net8PackageLineVersion, StringComparison.Ordinal));
  }

  [Fact]
  public void OracleProjectDoesNotReferenceNonOracleProviderPackages() {
    var projectPath = GetRepositoryPath(
        "src/DCoding.Data.DVault.Oracle/DCoding.Data.DVault.Oracle.csproj");
    var project = XDocument.Load(projectPath);
    var packageReferences = project
        .Descendants("PackageReference")
        .Select(reference => Assert.IsType<string>(reference.Attribute("Include")?.Value))
        .Distinct(StringComparer.Ordinal)
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
        packageId.Contains("Db2", StringComparison.OrdinalIgnoreCase) ||
        packageId.Contains("IBM.EntityFrameworkCore", StringComparison.OrdinalIgnoreCase) ||
        packageId.Contains("Npgsql", StringComparison.OrdinalIgnoreCase) ||
        packageId.Contains("PostgreSQL", StringComparison.OrdinalIgnoreCase) ||
        packageId.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) ||
        packageId.Contains("MySql", StringComparison.OrdinalIgnoreCase);
  }

  private static Dictionary<string, PackageArchiveOptions> CreatePackageOptions() {
    return PackageDefinitions.ToDictionary(
        package => package.Id,
        package => new PackageArchiveOptions { WriteSymbols = !package.IsAnalyzer },
        StringComparer.Ordinal);
  }

  private static void WritePackageMatrix(
      string packageDirectory,
      IReadOnlyDictionary<string, PackageArchiveOptions>? options = null) {
    options ??= CreatePackageOptions();

    foreach (var packageLine in PackageLines) {
      foreach (var package in PackageDefinitions) {
        var packageOptions = options[package.Id];
        if (packageOptions.WritePackage) {
          WritePackage(packageDirectory, package, packageOptions, packageLine);
        }

        if (!package.IsAnalyzer && packageOptions.WriteSymbols) {
          WriteSymbolsPackage(packageDirectory, package, packageOptions, packageLine);
        }
      }
    }
  }

  private static string CreatePackagedReadme(TestPackageDefinition package) {
    return package.IsAnalyzer
        ? CreateAnalyzerPackageReadme(package.Id)
        : CreateRuntimePackageReadme();
  }

  private static string CreateRuntimePackageReadme() {
    return CreateRuntimePackageReadme(PackageLines);
  }

  private static string CreateRuntimePackageReadme(IReadOnlyList<PackageLine> packageLines) {
    var builder = new StringBuilder();
    builder
        .AppendLine(ExpectedAnalyzerBuildHostGuidance);

    foreach (var packageLine in packageLines) {
      builder
          .Append(packageLine.TargetFramework)
          .Append(" / ")
          .Append(packageLine.EfCoreLine)
          .AppendLine();

      foreach (var package in PackageDefinitions.Where(package => !package.IsAnalyzer)) {
        builder
            .Append("dotnet add package ")
            .Append(package.Id)
            .Append(" --version ")
            .Append(packageLine.Version)
            .AppendLine();
      }

      builder
          .Append("<PackageReference Include=\"DCoding.Data.DVault.Analyzers\" Version=\"")
          .Append(packageLine.Version)
          .AppendLine("\" PrivateAssets=\"all\" />");
    }

    return builder.ToString();
  }

  private static string CreateAnalyzerPackageReadme(
      string packageId,
      IReadOnlyList<PackageLine>? packageLines = null) {
    packageLines ??= [
        .. PackageLines,
    ];

    var builder = new StringBuilder();
    builder
        .AppendLine(ExpectedAnalyzerBuildHostGuidance);

    foreach (var packageLine in packageLines) {
      builder
          .Append(packageLine.TargetFramework)
          .Append(" / ")
          .Append(packageLine.EfCoreLine)
          .AppendLine();
      builder
          .Append("<PackageReference Include=\"")
          .Append(packageId)
          .Append("\" Version=\"")
          .Append(packageLine.Version)
          .AppendLine("\" PrivateAssets=\"all\" />");
    }

    return builder.ToString();
  }

  private static void WritePackage(
      string packageDirectory,
      TestPackageDefinition package,
      PackageArchiveOptions options,
      PackageLine packageLine) {
    var filePath = Path.Combine(packageDirectory, package.Id + "." + packageLine.Version + ".nupkg");
    using var archive = ZipFile.Open(filePath, ZipArchiveMode.Create);

    WriteTextEntry(archive, package.Id + ".nuspec", CreateNuspec(package, options, packageLine));
    if (options.IncludeReadme) {
      WriteTextEntry(
          archive,
          "README.md",
          options.ReadmeText ?? CreatePackagedReadme(package));
    }

    if (options.IncludeXmlDocumentation) {
      var xmlPath = package.IsAnalyzer
          ? "analyzers/dotnet/cs/" + package.Id + ".xml"
          : "lib/" + packageLine.TargetFramework + "/" + package.Id + ".xml";
      WriteTextEntry(archive, xmlPath, "<doc />\n");
    }

    if (package.IsAnalyzer) {
      WriteBinaryEntry(archive, "analyzers/dotnet/cs/" + package.Id + ".dll", [1, 2, 3]);
    }
  }

  private static void WriteSymbolsPackage(
      string packageDirectory,
      TestPackageDefinition package,
      PackageArchiveOptions options,
      PackageLine packageLine) {
    var filePath = Path.Combine(packageDirectory, package.Id + "." + packageLine.Version + ".snupkg");
    using var archive = ZipFile.Open(filePath, ZipArchiveMode.Create);

    WriteTextEntry(archive, package.Id + ".nuspec", CreateNuspec(package, options, packageLine));
    if (options.IncludeSymbolPdb) {
      var pdbPath = package.IsAnalyzer
          ? "analyzers/dotnet/cs/" + package.Id + ".pdb"
          : "lib/" + packageLine.TargetFramework + "/" + package.Id + ".pdb";
      WriteBinaryEntry(archive, pdbPath, [1, 2, 3]);
    }
  }

  private static string CreateNuspec(
      TestPackageDefinition package,
      PackageArchiveOptions options,
      PackageLine packageLine) {
    var dependencies = new XElement(NuspecNamespace + "dependencies");
    if (!package.IsAnalyzer) {
      if (!options.OmittedDependencyGroups.Contains(packageLine.TargetFramework)) {
        dependencies.Add(CreateDependencyGroup(package, options, packageLine));
      }

      foreach (var targetFramework in options.AdditionalDependencyGroups) {
        dependencies.Add(CreateDependencyGroup(
            package,
            options,
            new PackageLine(packageLine.Version, targetFramework, GetEfCoreLine(targetFramework))));
      }
    }

    var metadata = new XElement(
        NuspecNamespace + "metadata",
        new XElement(NuspecNamespace + "id", package.Id),
        new XElement(NuspecNamespace + "version", packageLine.Version),
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
        dependencies);

    var document = new XDocument(
        new XDeclaration("1.0", "utf-8", null),
        new XElement(NuspecNamespace + "package", metadata));

    return document.ToString(SaveOptions.DisableFormatting);
  }

  private static XElement CreateDependencyGroup(
      TestPackageDefinition package,
      PackageArchiveOptions options,
      PackageLine packageLine) {
    var group = new XElement(
        NuspecNamespace + "group",
        new XAttribute("targetFramework", packageLine.TargetFramework));

    foreach (var dependency in GetDependencies(package, options, packageLine)) {
      group.Add(new XElement(
          NuspecNamespace + "dependency",
          new XAttribute("id", dependency.Id),
          new XAttribute("version", dependency.Version)));
    }

    return group;
  }

  private static IReadOnlyList<TestDependency> GetDependencies(
      TestPackageDefinition package,
      PackageArchiveOptions options,
      PackageLine packageLine) {
    var dependencies = new List<TestDependency>();

    if (string.Equals(package.Id, CorePackageId, StringComparison.Ordinal)) {
      dependencies.Add(new TestDependency("Microsoft.EntityFrameworkCore", GetEfCoreVersion(packageLine.TargetFramework)));
    }
    else if (package.IsProvider) {
      dependencies.Add(new TestDependency(CorePackageId, options.CoreDependencyVersion ?? packageLine.Version));
    }

    if (package.UsesDb2ProviderDependency) {
      dependencies.Add(new TestDependency(IbmEntityFrameworkCorePackageId, GetDb2ProviderVersion(packageLine.TargetFramework)));
    }

    if (string.Equals(package.Id, CorePackageId, StringComparison.Ordinal) ||
        package.UsesEfRelationalDependency) {
      dependencies.Add(new TestDependency("Microsoft.EntityFrameworkCore.Relational", GetEfCoreVersion(packageLine.TargetFramework)));
    }

    if (string.Equals(package.Id, CorePackageId, StringComparison.Ordinal) ||
        package.IsProvider) {
      dependencies.Add(new TestDependency(
          "Microsoft.Extensions.DependencyInjection.Abstractions",
          GetDependencyInjectionAbstractionsVersion(packageLine.TargetFramework)));
    }

    return dependencies
        .Select(dependency => dependency with {
          Version = options.GetDependencyVersion(packageLine.TargetFramework, dependency.Id, dependency.Version),
        })
        .ToArray();
  }

  private static string GetEfCoreVersion(string targetFramework) {
    return targetFramework switch {
      Net8TargetFramework => "8.0.28",
      Net10TargetFramework => "10.0.9",
      _ => throw new InvalidOperationException("Unsupported dependency target framework '" + targetFramework + "'."),
    };
  }

  private static string GetEfCoreLine(string targetFramework) {
    return targetFramework switch {
      Net8TargetFramework => "EF Core 8",
      Net10TargetFramework => "EF Core 10",
      _ => throw new InvalidOperationException("Unsupported dependency target framework '" + targetFramework + "'."),
    };
  }

  private static string GetDependencyInjectionAbstractionsVersion(string targetFramework) {
    return targetFramework switch {
      Net8TargetFramework => "8.0.2",
      Net10TargetFramework => "10.0.9",
      _ => throw new InvalidOperationException("Unsupported dependency target framework '" + targetFramework + "'."),
    };
  }

  private static string GetDb2ProviderVersion(string targetFramework) {
    return targetFramework switch {
      Net8TargetFramework => "8.0.0.400",
      Net10TargetFramework => "10.0.0.100",
      _ => throw new InvalidOperationException("Unsupported DB2 provider target framework '" + targetFramework + "'."),
    };
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
      bool IsProvider,
      bool IsAnalyzer,
      bool UsesEfRelationalDependency = false,
      bool UsesDb2ProviderDependency = false);

  private sealed record PackageLine(string Version, string TargetFramework, string EfCoreLine);

  private sealed record TestDependency(string Id, string Version);

  private sealed class PackageArchiveOptions {
    private readonly Dictionary<string, Dictionary<string, string>> dependencyVersionOverrides = new(StringComparer.Ordinal);

    public bool WritePackage { get; set; } = true;

    public bool WriteSymbols { get; set; } = true;

    public bool IncludeReadme { get; set; } = true;

    public bool IncludeXmlDocumentation { get; set; } = true;

    public bool IncludeSymbolPdb { get; set; } = true;

    public string? ReadmeText { get; set; }

    public string Authors { get; set; } = PackageVerifierTests.Authors;

    public string? CoreDependencyVersion { get; set; }

    public HashSet<string> OmittedDependencyGroups { get; } = new(StringComparer.Ordinal);

    public HashSet<string> AdditionalDependencyGroups { get; } = new(StringComparer.Ordinal);

    public void OverrideDependencyVersion(string targetFramework, string packageId, string version) {
      if (!dependencyVersionOverrides.TryGetValue(targetFramework, out var overrides)) {
        overrides = new Dictionary<string, string>(StringComparer.Ordinal);
        dependencyVersionOverrides.Add(targetFramework, overrides);
      }

      overrides[packageId] = version;
    }

    public string GetDependencyVersion(string targetFramework, string packageId, string defaultVersion) {
      return dependencyVersionOverrides.TryGetValue(targetFramework, out var overrides) &&
          overrides.TryGetValue(packageId, out var version)
          ? version
          : defaultVersion;
    }
  }
}
