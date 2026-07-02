using System.IO.Compression;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Linq;

namespace DCoding.Data.DVault.PackageVerification;

public sealed class PackageVerifier {
  private const string CorePackageId = "DCoding.Data.DVault";
  private const string Db2PackageId = "DCoding.Data.DVault.Db2";
  private const string IbmEntityFrameworkCorePackageId = "IBM.EntityFrameworkCore";
  private const string Net8TargetFramework = "net8.0";
  private const string Net10TargetFramework = "net10.0";
  private const string ExpectedAuthors = "d-coding GmbH";
  private const string ExpectedLicenseExpression = "Apache-2.0";
  private const string ExpectedRepositoryType = "git";
  private const string ExpectedRepositoryUrl = "https://github.com/d-codingGmbH/dvault.development.git";
  private const string ExpectedProjectUrl = "https://github.com/d-codingGmbH/dvault.development";
  private const string ExpectedCopyright = "Copyright (c) d-coding GmbH";
  private const string ExpectedReleaseNotes = "DVault v0.100.0 breaking release: publish package versions 8.100.0 for net8.0/EF Core 8 and 10.100.0 for net10.0/EF Core 10; generated hash-key and participant-reference columns now default to binary physical storage; existing HexString schemas should stay on the previous package line or opt into the HexString compatibility profile until migration is reviewed.";
  private const string ExpectedReadmeFile = "README.md";
  private const string ExpectedAnalyzerBuildHostGuidance = "Build projects that reference `DCoding.Data.DVault.Analyzers` with either a `.NET 8 SDK` or `.NET 10 SDK` host. The package ships one `netstandard2.0` analyzer asset under `analyzers/dotnet/cs/` for both coordinated package lines.";
  private const string ExpectedAnalyzerTargetFrameworkMoniker = ".NETStandard,Version=v2.0";

  private static readonly string[] DisallowedAnalyzerBuildHostContradictionFragments = [
      "This repository validates pure `.NET 8 SDK` analyzer consumption",
      "requires a `.NET 10 SDK` build host",
      "uses a `.NET 10 SDK` build host",
      "Build projects that reference `DCoding.Data.DVault.Analyzers` with a `.NET 10 SDK` host",
      "one `net10.0` analyzer asset",
      "net10-only analyzer",
  ];

  private static readonly string[] DisallowedAnalyzerTargetFrameworkFragments = [
      ".NETCoreApp,Version=v10.0",
      ".NETCoreApp,Version=v8.0",
  ];

  private static readonly string[] ExpectedAnalyzerCompanionAssets = [
      "Microsoft.CodeAnalysis.CSharp.Workspaces.dll",
      "Microsoft.CodeAnalysis.Workspaces.dll",
      "System.Composition.AttributedModel.dll",
      "System.Composition.Hosting.dll",
      "System.Composition.Runtime.dll",
      "System.Composition.TypedParts.dll",
      "System.Text.Json.dll",
  ];

  private static readonly string[] DisallowedMixedLineInstallClaimFragments = [
      "You can mix `8.100.0` and `10.100.0`",
      "you can mix `8.100.0` and `10.100.0`",
      "Projects may mix `8.100.0` and `10.100.0`",
      "projects may mix `8.100.0` and `10.100.0`",
      "Consumers may mix `8.100.0` and `10.100.0`",
      "consumers may mix `8.100.0` and `10.100.0`",
      "Use `8.100.0` runtime packages with `10.100.0` analyzer",
      "Use `10.100.0` runtime packages with `8.100.0` analyzer",
  ];

  private static readonly ExpectedPackageLine[] ExpectedPackageLines = [
      new("8.100.0", Net8TargetFramework, "EF Core 8"),
      new("10.100.0", Net10TargetFramework, "EF Core 10"),
  ];

  private static readonly string[] DisallowedInstallVersionFragments = [
      "--version 0.32.0",
      "--version 0.33.0",
      "--version 0.34.0",
      "--version 0.35.0",
      "--version 0.36.0",
      "--version 0.37.0",
      "--version 0.38.0",
      "--version 0.39.0",
      "--version 0.40.0",
      "--version 0.41.0",
      "--version 0.42.0",
      "--version 0.43.0",
      "--version 0.44.0",
      "--version 0.45.0",
      "--version 0.46.0",
      "--version 0.47.0",
      "--version 0.48.0",
      "--version 0.49.0",
      "--version 0.50.0",
      "--version 0.51.0",
      "--version 0.100.0",
      "--version 8.37.0",
      "--version 10.37.0",
      "--version 8.38.0",
      "--version 10.38.0",
      "--version 8.39.0",
      "--version 10.39.0",
      "--version 8.40.0",
      "--version 10.40.0",
      "--version 8.41.0",
      "--version 10.41.0",
      "--version 8.42.0",
      "--version 10.42.0",
      "--version 8.43.0",
      "--version 10.43.0",
      "--version 8.44.0",
      "--version 10.44.0",
      "--version 8.45.0",
      "--version 10.45.0",
      "--version 8.46.0",
      "--version 10.46.0",
      "--version 8.47.0",
      "--version 10.47.0",
      "--version 8.48.0",
      "--version 10.48.0",
      "--version 8.49.0",
      "--version 10.49.0",
      "--version 8.50.0",
      "--version 10.50.0",
      "--version 8.51.0",
      "--version 10.51.0",
      "Version=\"0.32.0\"",
      "Version=\"0.33.0\"",
      "Version=\"0.34.0\"",
      "Version=\"0.35.0\"",
      "Version=\"0.36.0\"",
      "Version=\"0.37.0\"",
      "Version=\"0.38.0\"",
      "Version=\"0.39.0\"",
      "Version=\"0.40.0\"",
      "Version=\"0.41.0\"",
      "Version=\"0.42.0\"",
      "Version=\"0.43.0\"",
      "Version=\"0.44.0\"",
      "Version=\"0.45.0\"",
      "Version=\"0.46.0\"",
      "Version=\"0.47.0\"",
      "Version=\"0.48.0\"",
      "Version=\"0.49.0\"",
      "Version=\"0.50.0\"",
      "Version=\"0.51.0\"",
      "Version=\"0.100.0\"",
      "Version=\"8.37.0\"",
      "Version=\"10.37.0\"",
      "Version=\"8.38.0\"",
      "Version=\"10.38.0\"",
      "Version=\"8.39.0\"",
      "Version=\"10.39.0\"",
      "Version=\"8.40.0\"",
      "Version=\"10.40.0\"",
      "Version=\"8.41.0\"",
      "Version=\"10.41.0\"",
      "Version=\"8.42.0\"",
      "Version=\"10.42.0\"",
      "Version=\"8.43.0\"",
      "Version=\"10.43.0\"",
      "Version=\"8.44.0\"",
      "Version=\"10.44.0\"",
      "Version=\"8.45.0\"",
      "Version=\"10.45.0\"",
      "Version=\"8.46.0\"",
      "Version=\"10.46.0\"",
      "Version=\"8.47.0\"",
      "Version=\"10.47.0\"",
      "Version=\"8.48.0\"",
      "Version=\"10.48.0\"",
      "Version=\"8.49.0\"",
      "Version=\"10.49.0\"",
      "Version=\"8.50.0\"",
      "Version=\"10.50.0\"",
      "Version=\"8.51.0\"",
      "Version=\"10.51.0\"",
  ];

  private static readonly IReadOnlyList<ExpectedPackage> ExpectedPackages = [
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
          "DCoding.Data.DVault.Privacy",
          "DVault Privacy Extensions",
          "Provider-neutral opt-in privacy extension proof, structural alias coverage reporting, and alias-driven encrypted payload conversion seams for DCoding.Data.DVault.",
          ["dvault", "data-vault", "privacy", "security", "gdpr", "dsgvo", "ef-core", "persistence"],
          false,
          false,
          false,
          false,
          true,
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

  private static readonly IReadOnlyDictionary<string, ExpectedPackage> ExpectedPackageById =
      ExpectedPackages.ToDictionary(package => package.Id, StringComparer.Ordinal);

  public PackageVerificationResult Verify(PackageVerificationOptions options) {
    ArgumentNullException.ThrowIfNull(options);

    var issues = new List<PackageVerificationIssue>();
    var packageDirectory = Path.GetFullPath(options.PackageDirectory);

    if (!Directory.Exists(packageDirectory)) {
      issues.Add(new PackageVerificationIssue(
          PackageVerificationOptions.DefaultPackageDirectory,
          "Package directory does not exist at '" + options.PackageDirectory + "'. Run 'bash tools/pack-release-packages.sh' from the repository root first."));
      return new PackageVerificationResult(issues);
    }

    var unexpectedFiles = Directory
        .EnumerateFiles(packageDirectory)
        .Where(path =>
            !string.Equals(Path.GetExtension(path), ".nupkg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(Path.GetExtension(path), ".snupkg", StringComparison.OrdinalIgnoreCase))
        .Order(StringComparer.Ordinal)
        .ToArray();

    var expectedPackageArtifactCount = ExpectedPackages.Count * ExpectedPackageLines.Length;
    var expectedSymbolsArtifactCount = ExpectedPackages.Count(package => !package.IsAnalyzer) * ExpectedPackageLines.Length;
    foreach (var unexpectedFile in unexpectedFiles) {
      issues.Add(new PackageVerificationIssue(
          Path.GetFileName(unexpectedFile),
          "Unexpected file artifact in package directory. Expected only the " + expectedPackageArtifactCount + " .nupkg files and " + expectedSymbolsArtifactCount + " .snupkg files produced by tools/pack-release-packages.sh."));
    }

    var packageArchives = ReadArchives(packageDirectory, PackageArtifactKind.Package, issues);
    var symbolArchives = ReadArchives(packageDirectory, PackageArtifactKind.Symbols, issues);

    ValidateArtifactSet(packageArchives, PackageArtifactKind.Package, issues);
    ValidateArtifactSet(symbolArchives, PackageArtifactKind.Symbols, issues);

    var packageByIdentity = GetSingleArchiveByIdentity(packageArchives);
    var symbolsByIdentity = GetSingleArchiveByIdentity(symbolArchives);

    foreach (var expectedPackage in ExpectedPackages) {
      foreach (var packageLine in ExpectedPackageLines) {
        var packageIdentity = new PackageIdentity(expectedPackage.Id, packageLine.Version);
        var packageArchive = packageByIdentity.TryGetValue(packageIdentity, out var matchingPackageArchive)
            ? matchingPackageArchive
            : null;

        if (packageArchive is not null) {
          var coreVersion = packageByIdentity.TryGetValue(new PackageIdentity(CorePackageId, packageLine.Version), out var corePackage)
              ? corePackage.Version
              : string.Empty;
          ValidatePackageArchive(packageArchive, expectedPackage, packageLine, coreVersion, issues);
        }

        if (!expectedPackage.IsAnalyzer &&
            symbolsByIdentity.TryGetValue(packageIdentity, out var symbolsArchive)) {
          ValidateSymbolsArchive(symbolsArchive, expectedPackage, packageLine, packageArchive?.Version, issues);
        }
      }
    }

    return new PackageVerificationResult(issues);
  }

  private static IReadOnlyList<PackageArchive> ReadArchives(
      string packageDirectory,
      PackageArtifactKind artifactKind,
      List<PackageVerificationIssue> issues) {
    var extension = artifactKind == PackageArtifactKind.Package ? "*.nupkg" : "*.snupkg";
    var archives = new List<PackageArchive>();

    foreach (var filePath in Directory.EnumerateFiles(packageDirectory, extension).Order(StringComparer.Ordinal)) {
      var archive = ReadArchive(filePath, artifactKind, issues);
      if (archive is not null) {
        archives.Add(archive);
      }
    }

    return archives;
  }

  private static PackageArchive? ReadArchive(
      string filePath,
      PackageArtifactKind artifactKind,
      List<PackageVerificationIssue> issues) {
    try {
      using var archive = ZipFile.OpenRead(filePath);
      var entries = archive
          .Entries
          .Select(entry => entry.FullName.Replace('\\', '/'))
          .ToHashSet(StringComparer.Ordinal);
      var nuspecEntries = archive
          .Entries
          .Where(entry => entry.FullName.EndsWith(".nuspec", StringComparison.OrdinalIgnoreCase))
          .ToArray();

      if (nuspecEntries.Length != 1) {
        issues.Add(new PackageVerificationIssue(
            Path.GetFileName(filePath),
            "Expected exactly one .nuspec entry but found " + nuspecEntries.Length + "."));
        return null;
      }

      using var nuspecStream = nuspecEntries[0].Open();
      var nuspec = XDocument.Load(nuspecStream, LoadOptions.None);
      var metadata = GetMetadataElement(nuspec);
      if (metadata is null) {
        issues.Add(new PackageVerificationIssue(
            Path.GetFileName(filePath),
            "The nuspec is missing its metadata element."));
        return null;
      }

      var id = GetElementValue(metadata, "id");
      var version = GetElementValue(metadata, "version");
      if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(version)) {
        issues.Add(new PackageVerificationIssue(
            Path.GetFileName(filePath),
            "The nuspec metadata must contain non-empty id and version values."));
        return null;
      }

      var readme = ReadTextEntry(archive, ExpectedReadmeFile);
      return new PackageArchive(
          filePath,
          Path.GetFileName(filePath),
          artifactKind,
          id,
          version,
          nuspec,
          entries,
          readme);
    }
    catch (Exception exception) when (exception is InvalidDataException or IOException or UnauthorizedAccessException or System.Xml.XmlException) {
      issues.Add(new PackageVerificationIssue(
          Path.GetFileName(filePath),
          "Could not inspect package artifact: " + exception.Message));
      return null;
    }
  }

  private static void ValidateArtifactSet(
      IReadOnlyList<PackageArchive> archives,
      PackageArtifactKind artifactKind,
      List<PackageVerificationIssue> issues) {
    var extension = artifactKind == PackageArtifactKind.Package ? ".nupkg" : ".snupkg";
    IReadOnlyList<ExpectedPackage> expectedPackages = GetExpectedPackagesForArtifactKind(artifactKind);
    var expectedFileCount = expectedPackages.Count * ExpectedPackageLines.Length;

    if (archives.Count != expectedFileCount) {
      issues.Add(new PackageVerificationIssue(
          extension,
          "Expected exactly " + expectedFileCount + " " + extension + " artifacts but found " + archives.Count + "."));
    }

    foreach (var archive in archives) {
      if (!expectedPackages.Any(package => string.Equals(package.Id, archive.Id, StringComparison.Ordinal))) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Unexpected " + extension + " artifact '" + archive.FileName + "'. Expected only: " + string.Join(", ", expectedPackages.Select(package => package.Id)) + "."));
      }

      if (!ExpectedPackageLines.Any(packageLine => string.Equals(packageLine.Version, archive.Version, StringComparison.Ordinal))) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Unexpected package version '" + archive.Version + "' in artifact '" + archive.FileName + "'. Expected only package lines: " + string.Join(", ", ExpectedPackageLines.Select(packageLine => packageLine.Version)) + "."));
      }

      var expectedFileName = archive.Id + "." + archive.Version + extension;
      if (!string.Equals(archive.FileName, expectedFileName, StringComparison.Ordinal)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Artifact filename '" + archive.FileName + "' does not match nuspec id/version. Expected '" + expectedFileName + "'."));
      }
    }

    foreach (var expectedPackage in expectedPackages) {
      foreach (var packageLine in ExpectedPackageLines) {
        var matchingArchives = archives
            .Where(archive =>
                string.Equals(archive.Id, expectedPackage.Id, StringComparison.Ordinal) &&
                string.Equals(archive.Version, packageLine.Version, StringComparison.Ordinal))
            .ToArray();

        if (matchingArchives.Length == 0) {
          issues.Add(new PackageVerificationIssue(
              expectedPackage.Id,
              "Missing expected " + extension + " artifact for package line '" + packageLine.Version + "' in the package directory."));
        }
        else if (matchingArchives.Length > 1) {
          issues.Add(new PackageVerificationIssue(
              expectedPackage.Id,
              "Expected exactly one " + extension + " artifact for package line '" + packageLine.Version + "' but found " + matchingArchives.Length + ": " + string.Join(", ", matchingArchives.Select(archive => archive.FileName)) + "."));
        }
      }
    }
  }

  private static IReadOnlyList<ExpectedPackage> GetExpectedPackagesForArtifactKind(PackageArtifactKind artifactKind) {
    return artifactKind == PackageArtifactKind.Package
        ? ExpectedPackages
        : [.. ExpectedPackages.Where(package => !package.IsAnalyzer)];
  }

  private static IReadOnlyDictionary<PackageIdentity, PackageArchive> GetSingleArchiveByIdentity(IReadOnlyList<PackageArchive> archives) {
    return archives
        .GroupBy(archive => new PackageIdentity(archive.Id, archive.Version))
        .Where(group => group.Count() == 1)
        .ToDictionary(group => group.Key, group => group.Single());
  }

  private static void ValidatePackageArchive(
      PackageArchive archive,
      ExpectedPackage expectedPackage,
      ExpectedPackageLine packageLine,
      string coreVersion,
      List<PackageVerificationIssue> issues) {
    var metadata = GetRequiredMetadataElement(archive);
    AssertMetadataValue(archive, metadata, "id", expectedPackage.Id, issues);
    AssertMetadataValue(archive, metadata, "version", packageLine.Version, issues);
    AssertMetadataValue(archive, metadata, "title", expectedPackage.Title, issues);
    AssertMetadataValue(archive, metadata, "authors", ExpectedAuthors, issues);
    AssertMetadataValue(archive, metadata, "description", expectedPackage.Description, issues);
    AssertMetadataValue(archive, metadata, "projectUrl", ExpectedProjectUrl, issues);
    AssertMetadataValue(archive, metadata, "releaseNotes", ExpectedReleaseNotes, issues);
    AssertMetadataValue(archive, metadata, "copyright", ExpectedCopyright, issues);
    AssertMetadataValue(archive, metadata, "readme", ExpectedReadmeFile, issues);
    ValidateTags(archive, metadata, expectedPackage, issues);
    ValidateLicense(archive, metadata, issues);
    ValidateRepository(archive, metadata, issues);
    ValidateReadme(archive, issues);
    ValidateXmlDocumentation(archive, expectedPackage, packageLine, issues);
    ValidateAnalyzerAssets(archive, expectedPackage, issues);
    ValidateDependencyGroups(archive, expectedPackage, packageLine, coreVersion, issues);
  }

  private static void ValidateSymbolsArchive(
      PackageArchive archive,
      ExpectedPackage expectedPackage,
      ExpectedPackageLine packageLine,
      string? packageVersion,
      List<PackageVerificationIssue> issues) {
    if (!string.IsNullOrWhiteSpace(packageVersion) &&
        !string.Equals(archive.Version, packageVersion, StringComparison.Ordinal)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Symbols package version '" + archive.Version + "' does not match package version '" + packageVersion + "'."));
    }

    var expectedPdbPath = expectedPackage.IsAnalyzer
        ? "analyzers/dotnet/cs/" + expectedPackage.Id + ".pdb"
        : "lib/" + packageLine.TargetFramework + "/" + expectedPackage.Id + ".pdb";
    if (!archive.Entries.Contains(expectedPdbPath)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Symbols package '" + archive.FileName + "' is missing expected PDB entry '" + expectedPdbPath + "'."));
    }
  }

  private static void AssertMetadataValue(
      PackageArchive archive,
      XElement metadata,
      string elementName,
      string expected,
      List<PackageVerificationIssue> issues) {
    var actual = GetElementValue(metadata, elementName);
    if (!string.Equals(actual, expected, StringComparison.Ordinal)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec metadata '" + elementName + "' is '" + actual + "' but expected '" + expected + "'."));
    }
  }

  private static void ValidateTags(
      PackageArchive archive,
      XElement metadata,
      ExpectedPackage expectedPackage,
      List<PackageVerificationIssue> issues) {
    var actualTags = SplitTags(GetElementValue(metadata, "tags"));
    if (!actualTags.SequenceEqual(expectedPackage.Tags, StringComparer.Ordinal)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec metadata 'tags' is '" + string.Join(" ", actualTags) + "' but expected '" + string.Join(" ", expectedPackage.Tags) + "'."));
    }
  }

  private static void ValidateLicense(PackageArchive archive, XElement metadata, List<PackageVerificationIssue> issues) {
    var license = GetChild(metadata, "license");
    if (license is null) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec metadata is missing license expression '" + ExpectedLicenseExpression + "'."));
      return;
    }

    var type = license.Attribute("type")?.Value ?? string.Empty;
    var expression = license.Value.Trim();
    if (!string.Equals(type, "expression", StringComparison.Ordinal) ||
        !string.Equals(expression, ExpectedLicenseExpression, StringComparison.Ordinal)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec license is type='" + type + "' value='" + expression + "' but expected Apache-2.0 expression metadata."));
    }
  }

  private static void ValidateRepository(PackageArchive archive, XElement metadata, List<PackageVerificationIssue> issues) {
    var repository = GetChild(metadata, "repository");
    if (repository is null) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec metadata is missing repository information."));
      return;
    }

    var type = repository.Attribute("type")?.Value ?? string.Empty;
    var url = repository.Attribute("url")?.Value ?? string.Empty;
    if (!string.Equals(type, ExpectedRepositoryType, StringComparison.Ordinal) ||
        !string.Equals(url, ExpectedRepositoryUrl, StringComparison.Ordinal)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec repository metadata is type='" + type + "' url='" + url + "' but expected type='" + ExpectedRepositoryType + "' url='" + ExpectedRepositoryUrl + "'."));
    }
  }

  private static void ValidateReadme(
      PackageArchive archive,
      List<PackageVerificationIssue> issues) {
    if (!archive.Entries.Contains(ExpectedReadmeFile)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Package archive '" + archive.FileName + "' is missing root README.md."));
      return;
    }

    if (archive.ReadmeText is null) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Packaged README.md does not contain the current NuGet installation guidance."));
      return;
    }

    ValidateReadmeDoesNotUseDisallowedInstallVersions(archive, issues);
    ValidateReadmeDoesNotUseMixedLineInstallClaims(archive, issues);
    ValidateReadmeContainsAnalyzerBuildHostGuidance(archive, issues);
    ValidateReadmeDoesNotContradictAnalyzerBuildHostGuidance(archive, issues);

    if (ExpectedPackageById.TryGetValue(archive.Id, out var currentPackage) && currentPackage.IsAnalyzer) {
      foreach (var packageLine in ExpectedPackageLines) {
        if (!archive.ReadmeText.Contains(packageLine.TargetFramework, StringComparison.Ordinal) ||
            !archive.ReadmeText.Contains(packageLine.EfCoreLine, StringComparison.Ordinal)) {
          issues.Add(new PackageVerificationIssue(
              archive.Id,
              "Packaged analyzer README.md does not label the " + packageLine.TargetFramework + " / " + packageLine.EfCoreLine + " PrivateAssets installation guidance for version " + packageLine.Version + "."));
        }

        var expectedAnalyzerReference =
            "<PackageReference Include=\"" + currentPackage.Id + "\" Version=\"" + packageLine.Version + "\" PrivateAssets=\"all\" />";
        if (!archive.ReadmeText.Contains(expectedAnalyzerReference, StringComparison.Ordinal)) {
          issues.Add(new PackageVerificationIssue(
              archive.Id,
              "Packaged analyzer README.md does not contain the " + packageLine.TargetFramework + " / " + packageLine.EfCoreLine + " PrivateAssets installation guidance for version " + packageLine.Version + "."));
        }
      }

      return;
    }

    foreach (var packageLine in ExpectedPackageLines) {
      if (!archive.ReadmeText.Contains(packageLine.TargetFramework, StringComparison.Ordinal) ||
          !archive.ReadmeText.Contains(packageLine.EfCoreLine, StringComparison.Ordinal)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Packaged README.md does not label the " + packageLine.TargetFramework + " / " + packageLine.EfCoreLine + " NuGet installation guidance for version " + packageLine.Version + "."));
      }

      foreach (var expectedPackage in ExpectedPackages.Where(package => !package.IsAnalyzer)) {
        var expectedInstallCommand =
            "dotnet add package " + expectedPackage.Id + " --version " + packageLine.Version;
        if (!archive.ReadmeText.Contains(expectedInstallCommand, StringComparison.Ordinal)) {
          issues.Add(new PackageVerificationIssue(
              archive.Id,
              "Packaged README.md does not contain the " + packageLine.TargetFramework + " / " + packageLine.EfCoreLine + " NuGet installation guidance for package " + expectedPackage.Id + " version " + packageLine.Version + "."));
        }
      }

      var expectedAnalyzerReference =
          "<PackageReference Include=\"DCoding.Data.DVault.Analyzers\" Version=\"" + packageLine.Version + "\" PrivateAssets=\"all\" />";
      if (!archive.ReadmeText.Contains(expectedAnalyzerReference, StringComparison.Ordinal)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Packaged README.md does not contain the " + packageLine.TargetFramework + " / " + packageLine.EfCoreLine + " analyzer PrivateAssets guidance for version " + packageLine.Version + "."));
      }
    }
  }

  private static void ValidateReadmeDoesNotUseDisallowedInstallVersions(
      PackageArchive archive,
      List<PackageVerificationIssue> issues) {
    foreach (var disallowedFragment in DisallowedInstallVersionFragments) {
      if (archive.ReadmeText?.Contains(disallowedFragment, StringComparison.Ordinal) == true) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Packaged README.md must not document stale or planning-release install version fragment '" + disallowedFragment + "'; use separate 8.100.0 and 10.100.0 package-line guidance."));
      }
    }
  }

  private static void ValidateReadmeDoesNotUseMixedLineInstallClaims(
      PackageArchive archive,
      List<PackageVerificationIssue> issues) {
    foreach (var disallowedFragment in DisallowedMixedLineInstallClaimFragments) {
      if (archive.ReadmeText?.Contains(disallowedFragment, StringComparison.Ordinal) == true) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Packaged README.md must not document mixed-line package installation claim '" + disallowedFragment + "'; use exactly one of the 8.100.0 or 10.100.0 package lines in a consumer project."));
      }
    }
  }

  private static void ValidateReadmeContainsAnalyzerBuildHostGuidance(
      PackageArchive archive,
      List<PackageVerificationIssue> issues) {
    if (archive.ReadmeText?.Contains(ExpectedAnalyzerBuildHostGuidance, StringComparison.Ordinal) != true) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Packaged README.md must state that DCoding.Data.DVault.Analyzers is supported on both .NET 8 SDK and .NET 10 SDK build-host baselines through one netstandard2.0 analyzer asset under analyzers/dotnet/cs/."));
    }
  }

  private static void ValidateReadmeDoesNotContradictAnalyzerBuildHostGuidance(
      PackageArchive archive,
      List<PackageVerificationIssue> issues) {
    foreach (var disallowedFragment in DisallowedAnalyzerBuildHostContradictionFragments) {
      if (archive.ReadmeText?.Contains(disallowedFragment, StringComparison.Ordinal) == true) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Packaged README.md must not contradict the dual .NET 8 SDK and .NET 10 SDK analyzer-host baseline for DCoding.Data.DVault.Analyzers; remove stale analyzer-host claim '" + disallowedFragment + "'."));
      }
    }
  }

  private static void ValidateXmlDocumentation(
      PackageArchive archive,
      ExpectedPackage expectedPackage,
      ExpectedPackageLine packageLine,
      List<PackageVerificationIssue> issues) {
    var expectedXmlPath = expectedPackage.IsAnalyzer
        ? "analyzers/dotnet/cs/" + expectedPackage.Id + ".xml"
        : "lib/" + packageLine.TargetFramework + "/" + expectedPackage.Id + ".xml";
    if (!archive.Entries.Contains(expectedXmlPath)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Package archive '" + archive.FileName + "' is missing generated XML documentation entry '" + expectedXmlPath + "'."));
    }
  }

  private static void ValidateAnalyzerAssets(
      PackageArchive archive,
      ExpectedPackage expectedPackage,
      List<PackageVerificationIssue> issues) {
    if (!expectedPackage.IsAnalyzer) {
      return;
    }

    var expectedAnalyzerPath = "analyzers/dotnet/cs/" + expectedPackage.Id + ".dll";
    if (!archive.Entries.Contains(expectedAnalyzerPath)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Analyzer package archive '" + archive.FileName + "' is missing analyzer asset '" + expectedAnalyzerPath + "'."));
      return;
    }

    foreach (var expectedCompanionAsset in ExpectedAnalyzerCompanionAssets) {
      var expectedCompanionPath = "analyzers/dotnet/cs/" + expectedCompanionAsset;
      if (!archive.Entries.Contains(expectedCompanionPath)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Analyzer package archive '" + archive.FileName + "' is missing analyzer companion asset '" + expectedCompanionPath + "'."));
      }
    }

    var analyzerBytes = ReadEntryBytes(archive.FilePath, expectedAnalyzerPath, archive.Id, issues);
    var analyzerTargetFrameworkMoniker = ReadAnalyzerTargetFrameworkMoniker(analyzerBytes);
    if (!string.Equals(analyzerTargetFrameworkMoniker, ExpectedAnalyzerTargetFrameworkMoniker, StringComparison.Ordinal)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Analyzer asset '" + expectedAnalyzerPath + "' must target '" + ExpectedAnalyzerTargetFrameworkMoniker + "' so both .NET 8 SDK and .NET 10 SDK hosts can load the same package asset."));
    }

    foreach (var disallowedFragment in DisallowedAnalyzerTargetFrameworkFragments) {
      if (string.Equals(analyzerTargetFrameworkMoniker, disallowedFragment, StringComparison.Ordinal) ||
          ContainsUtf8Fragment(analyzerBytes, disallowedFragment)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Analyzer asset '" + expectedAnalyzerPath + "' must not carry stale target framework marker '" + disallowedFragment + "'; expected '" + ExpectedAnalyzerTargetFrameworkMoniker + "'."));
      }
    }
  }

  private static void ValidateDependencyGroups(
      PackageArchive archive,
      ExpectedPackage expectedPackage,
      ExpectedPackageLine packageLine,
      string coreVersion,
      List<PackageVerificationIssue> issues) {
    var expectedDependencies = GetExpectedDependencies(expectedPackage, packageLine.TargetFramework, coreVersion);
    if (expectedDependencies.Count == 0) {
      return;
    }

    if ((expectedPackage.IsProvider || expectedPackage.UsesCorePackageDependency) &&
        string.IsNullOrWhiteSpace(coreVersion)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Cannot verify package dependency groups because the core package version is unavailable."));
      return;
    }

    var metadata = GetRequiredMetadataElement(archive);
    var dependencies = GetChild(metadata, "dependencies");
    if (dependencies is null) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec dependencies metadata is missing; expected the '" + packageLine.TargetFramework + "' dependency group for package line '" + packageLine.Version + "'."));
      return;
    }

    var dependencyGroups = dependencies
        .Elements()
        .Where(element => string.Equals(element.Name.LocalName, "group", StringComparison.Ordinal))
        .ToArray();

    foreach (var unexpectedGroup in dependencyGroups.Where(group =>
        !string.Equals(group.Attribute("targetFramework")?.Value, packageLine.TargetFramework, StringComparison.Ordinal))) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec dependencies metadata contains unexpected dependency group '" + (unexpectedGroup.Attribute("targetFramework")?.Value ?? string.Empty) + "' for package line '" + packageLine.Version + "'."));
    }

    var matchingGroups = dependencyGroups
        .Where(group => string.Equals(group.Attribute("targetFramework")?.Value, packageLine.TargetFramework, StringComparison.Ordinal))
        .ToArray();
    if (matchingGroups.Length == 0) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec dependencies metadata is missing the '" + packageLine.TargetFramework + "' dependency group for package line '" + packageLine.Version + "'."));
      return;
    }

    if (matchingGroups.Length > 1) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Nuspec dependencies metadata contains " + matchingGroups.Length + " '" + packageLine.TargetFramework + "' dependency groups for package line '" + packageLine.Version + "'; expected exactly one."));
      return;
    }

    ValidateDependencyGroup(
        archive,
        packageLine.TargetFramework,
        expectedDependencies,
        matchingGroups[0],
        issues);
  }

  private static void ValidateDependencyGroup(
      PackageArchive archive,
      string targetFramework,
      IReadOnlyList<ExpectedDependency> expectedDependencies,
      XElement dependencyGroup,
      List<PackageVerificationIssue> issues) {
    var actualDependencies = dependencyGroup
        .Elements()
        .Where(element => string.Equals(element.Name.LocalName, "dependency", StringComparison.Ordinal))
        .Select(element => new PackageDependency(
            element.Attribute("id")?.Value ?? string.Empty,
            element.Attribute("version")?.Value ?? string.Empty))
        .ToArray();

    foreach (var expectedDependency in expectedDependencies) {
      var matchingDependencies = actualDependencies
          .Where(dependency => string.Equals(dependency.Id, expectedDependency.Id, StringComparison.Ordinal))
          .ToArray();
      if (matchingDependencies.Length == 0) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Dependency group '" + targetFramework + "' is missing dependency '" + expectedDependency.Id + "' version '" + expectedDependency.Version + "'."));
        continue;
      }

      if (matchingDependencies.Length > 1) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Dependency group '" + targetFramework + "' contains " + matchingDependencies.Length + " entries for dependency '" + expectedDependency.Id + "'; expected exactly one."));
        continue;
      }

      var actualDependency = matchingDependencies[0];
      if (!string.Equals(actualDependency.Version, expectedDependency.Version, StringComparison.Ordinal)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Dependency group '" + targetFramework + "' dependency '" + expectedDependency.Id + "' uses version '" + actualDependency.Version + "' but expected '" + expectedDependency.Version + "'."));
      }
    }

    foreach (var actualDependency in actualDependencies) {
      if (!expectedDependencies.Any(expected => string.Equals(expected.Id, actualDependency.Id, StringComparison.Ordinal))) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Dependency group '" + targetFramework + "' contains unexpected dependency '" + actualDependency.Id + "' version '" + actualDependency.Version + "'."));
      }
    }

    ValidateEfCoreDependencyLine(archive, targetFramework, actualDependencies, issues);
  }

  private static void ValidateEfCoreDependencyLine(
      PackageArchive archive,
      string targetFramework,
      IReadOnlyList<PackageDependency> dependencies,
      List<PackageVerificationIssue> issues) {
    var expectedVersionPrefix = targetFramework switch {
      Net8TargetFramework => "8.",
      Net10TargetFramework => "10.",
      _ => string.Empty,
    };

    if (expectedVersionPrefix.Length == 0) {
      return;
    }

    foreach (var dependency in dependencies.Where(dependency => IsEfCoreLineDependency(dependency.Id))) {
      if (!dependency.Version.StartsWith(expectedVersionPrefix, StringComparison.Ordinal)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Dependency group '" + targetFramework + "' mixes EF Core lines: dependency '" + dependency.Id + "' uses version '" + dependency.Version + "' but expected an " + expectedVersionPrefix.TrimEnd('.') + ".x dependency."));
      }
    }
  }

  private static IReadOnlyList<ExpectedDependency> GetExpectedDependencies(
      ExpectedPackage expectedPackage,
      string targetFramework,
      string coreVersion) {
    var expectedDependencies = new List<ExpectedDependency>();

    if (string.Equals(expectedPackage.Id, CorePackageId, StringComparison.Ordinal)) {
      expectedDependencies.Add(new ExpectedDependency("Microsoft.EntityFrameworkCore", GetEfCoreVersion(targetFramework)));
    }
    else if (expectedPackage.IsProvider || expectedPackage.UsesCorePackageDependency) {
      expectedDependencies.Add(new ExpectedDependency(CorePackageId, coreVersion));
    }

    if (expectedPackage.UsesDb2ProviderDependency) {
      expectedDependencies.Add(new ExpectedDependency(IbmEntityFrameworkCorePackageId, GetDb2ProviderVersion(targetFramework)));
    }

    if (string.Equals(expectedPackage.Id, CorePackageId, StringComparison.Ordinal) ||
        expectedPackage.UsesEfRelationalDependency) {
      expectedDependencies.Add(new ExpectedDependency("Microsoft.EntityFrameworkCore.Relational", GetEfCoreVersion(targetFramework)));
    }

    if (string.Equals(expectedPackage.Id, CorePackageId, StringComparison.Ordinal) ||
        expectedPackage.IsProvider ||
        expectedPackage.UsesDependencyInjectionAbstractionsDependency) {
      expectedDependencies.Add(new ExpectedDependency(
          "Microsoft.Extensions.DependencyInjection.Abstractions",
          GetDependencyInjectionAbstractionsVersion(targetFramework)));
    }

    return expectedDependencies;
  }

  private static string GetEfCoreVersion(string targetFramework) {
    return targetFramework switch {
      Net8TargetFramework => "8.0.28",
      Net10TargetFramework => "10.0.9",
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

  private static bool IsEfCoreLineDependency(string packageId) {
    return packageId.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) ||
        string.Equals(packageId, IbmEntityFrameworkCorePackageId, StringComparison.Ordinal);
  }

  private static XElement GetRequiredMetadataElement(PackageArchive archive) {
    return GetMetadataElement(archive.Nuspec) ??
        throw new InvalidOperationException("Archive '" + archive.FileName + "' was accepted without nuspec metadata.");
  }

  private static XElement? GetMetadataElement(XDocument nuspec) {
    return nuspec.Root?
        .Elements()
        .FirstOrDefault(element => string.Equals(element.Name.LocalName, "metadata", StringComparison.Ordinal));
  }

  private static XElement? GetChild(XElement element, string childName) {
    return element
        .Elements()
        .FirstOrDefault(child => string.Equals(child.Name.LocalName, childName, StringComparison.Ordinal));
  }

  private static string GetElementValue(XElement element, string childName) {
    return GetChild(element, childName)?.Value.Trim() ?? string.Empty;
  }

  private static string[] SplitTags(string tags) {
    return tags
        .Split([' ', ';', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
  }

  private static string? ReadTextEntry(ZipArchive archive, string entryName) {
    var entry = archive
        .Entries
        .FirstOrDefault(candidate => string.Equals(candidate.FullName, entryName, StringComparison.Ordinal));

    if (entry is null) {
      return null;
    }

    using var stream = entry.Open();
    using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);
    return reader.ReadToEnd();
  }

  private static byte[] ReadEntryBytes(
      string archivePath,
      string entryName,
      string packageId,
      List<PackageVerificationIssue> issues) {
    try {
      using var archive = ZipFile.OpenRead(archivePath);
      var entry = archive
          .Entries
          .FirstOrDefault(candidate => string.Equals(candidate.FullName.Replace('\\', '/'), entryName, StringComparison.Ordinal));
      if (entry is null) {
        return [];
      }

      using var stream = entry.Open();
      using var memory = new MemoryStream();
      stream.CopyTo(memory);
      return memory.ToArray();
    }
    catch (Exception exception) when (exception is InvalidDataException or IOException or UnauthorizedAccessException) {
      issues.Add(new PackageVerificationIssue(
          packageId,
          "Could not inspect package entry '" + entryName + "': " + exception.Message));
      return [];
    }
  }

  private static bool ContainsUtf8Fragment(byte[] bytes, string fragment) {
    return Encoding.UTF8.GetString(bytes).Contains(fragment, StringComparison.Ordinal);
  }

  private static string ReadAnalyzerTargetFrameworkMoniker(byte[] bytes) {
    try {
      using var stream = new MemoryStream(bytes, writable: false);
      using var peReader = new PEReader(stream);
      if (!peReader.HasMetadata) {
        return ReadUtf8AnalyzerTargetFrameworkFallback(bytes);
      }

      var metadataReader = peReader.GetMetadataReader();
      var assemblyDefinition = metadataReader.GetAssemblyDefinition();
      foreach (var attributeHandle in assemblyDefinition.GetCustomAttributes()) {
        var attribute = metadataReader.GetCustomAttribute(attributeHandle);
        if (!IsTargetFrameworkAttribute(metadataReader, attribute.Constructor)) {
          continue;
        }

        var valueReader = metadataReader.GetBlobReader(attribute.Value);
        if (valueReader.ReadUInt16() != 1) {
          return string.Empty;
        }

        return valueReader.ReadSerializedString() ?? string.Empty;
      }
    }
    catch (Exception exception) when (exception is BadImageFormatException or IOException or InvalidOperationException) {
      return ReadUtf8AnalyzerTargetFrameworkFallback(bytes);
    }

    return string.Empty;
  }

  private static string ReadUtf8AnalyzerTargetFrameworkFallback(byte[] bytes) {
    var text = Encoding.UTF8.GetString(bytes);
    if (text.Contains(ExpectedAnalyzerTargetFrameworkMoniker, StringComparison.Ordinal)) {
      return ExpectedAnalyzerTargetFrameworkMoniker;
    }

    foreach (var disallowedFragment in DisallowedAnalyzerTargetFrameworkFragments) {
      if (text.Contains(disallowedFragment, StringComparison.Ordinal)) {
        return disallowedFragment;
      }
    }

    return string.Empty;
  }

  private static bool IsTargetFrameworkAttribute(MetadataReader metadataReader, EntityHandle constructor) {
    var attributeType = constructor.Kind switch {
      HandleKind.MemberReference => metadataReader.GetMemberReference((MemberReferenceHandle)constructor).Parent,
      HandleKind.MethodDefinition => metadataReader.GetMethodDefinition((MethodDefinitionHandle)constructor).GetDeclaringType(),
      _ => default(EntityHandle),
    };

    return IsTargetFrameworkAttributeType(metadataReader, attributeType);
  }

  private static bool IsTargetFrameworkAttributeType(MetadataReader metadataReader, EntityHandle attributeType) {
    return attributeType.Kind switch {
      HandleKind.TypeReference => IsTargetFrameworkTypeReference(metadataReader, (TypeReferenceHandle)attributeType),
      HandleKind.TypeDefinition => IsTargetFrameworkTypeDefinition(metadataReader, (TypeDefinitionHandle)attributeType),
      _ => false,
    };
  }

  private static bool IsTargetFrameworkTypeReference(MetadataReader metadataReader, TypeReferenceHandle typeReferenceHandle) {
    var typeReference = metadataReader.GetTypeReference(typeReferenceHandle);
    return metadataReader.StringComparer.Equals(typeReference.Namespace, "System.Runtime.Versioning") &&
        metadataReader.StringComparer.Equals(typeReference.Name, "TargetFrameworkAttribute");
  }

  private static bool IsTargetFrameworkTypeDefinition(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle) {
    var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);
    return metadataReader.StringComparer.Equals(typeDefinition.Namespace, "System.Runtime.Versioning") &&
        metadataReader.StringComparer.Equals(typeDefinition.Name, "TargetFrameworkAttribute");
  }

  private sealed record ExpectedPackage(
      string Id,
      string Title,
      string Description,
      string[] Tags,
      bool IsProvider,
      bool IsAnalyzer,
      bool UsesEfRelationalDependency = false,
      bool UsesDb2ProviderDependency = false,
      bool UsesCorePackageDependency = false,
      bool UsesDependencyInjectionAbstractionsDependency = false);

  private sealed record ExpectedPackageLine(string Version, string TargetFramework, string EfCoreLine);

  private sealed record ExpectedDependency(string Id, string Version);

  private sealed record PackageIdentity(string Id, string Version);

  private sealed record PackageArchive(
      string FilePath,
      string FileName,
      PackageArtifactKind ArtifactKind,
      string Id,
      string Version,
      XDocument Nuspec,
      IReadOnlySet<string> Entries,
      string? ReadmeText);

  private sealed record PackageDependency(string Id, string Version);

  private enum PackageArtifactKind {
    Package,
    Symbols,
  }
}
