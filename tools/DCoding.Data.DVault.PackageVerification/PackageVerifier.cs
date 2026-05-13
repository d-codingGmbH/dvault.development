using System.IO.Compression;
using System.Xml.Linq;

namespace DCoding.Data.DVault.PackageVerification;

public sealed class PackageVerifier {
  private const string CorePackageId = "DCoding.Data.DVault";
  private const string TargetFramework = "net10.0";
  private const string ExpectedAuthors = "d-coding GmbH";
  private const string ExpectedLicenseExpression = "Apache-2.0";
  private const string ExpectedRepositoryType = "git";
  private const string ExpectedRepositoryUrl = "https://github.com/d-codingGmbH/dvault.development.git";
  private const string ExpectedReadmeFile = "README.md";

  private static readonly IReadOnlyList<ExpectedPackage> ExpectedPackages = [
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

  private static readonly IReadOnlyDictionary<string, ExpectedPackage> ExpectedPackageById =
      ExpectedPackages.ToDictionary(package => package.Id, StringComparer.Ordinal);

  public PackageVerificationResult Verify(PackageVerificationOptions options) {
    ArgumentNullException.ThrowIfNull(options);

    var issues = new List<PackageVerificationIssue>();
    var packageDirectory = Path.GetFullPath(options.PackageDirectory);

    if (!Directory.Exists(packageDirectory)) {
      issues.Add(new PackageVerificationIssue(
          PackageVerificationOptions.DefaultPackageDirectory,
          "Package directory does not exist at '" + options.PackageDirectory + "'. Run 'dotnet pack DVault.slnx --configuration Release --nologo' from the repository root first."));
      return new PackageVerificationResult(issues);
    }

    var unexpectedFiles = Directory
        .EnumerateFiles(packageDirectory)
        .Where(path =>
            !string.Equals(Path.GetExtension(path), ".nupkg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(Path.GetExtension(path), ".snupkg", StringComparison.OrdinalIgnoreCase))
        .Order(StringComparer.Ordinal)
        .ToArray();

    foreach (var unexpectedFile in unexpectedFiles) {
      issues.Add(new PackageVerificationIssue(
          Path.GetFileName(unexpectedFile),
          "Unexpected file artifact in package directory. Expected only the six .nupkg files and six .snupkg files produced from DVault.slnx."));
    }

    var packageArchives = ReadArchives(packageDirectory, PackageArtifactKind.Package, issues);
    var symbolArchives = ReadArchives(packageDirectory, PackageArtifactKind.Symbols, issues);

    ValidateArtifactSet(packageArchives, PackageArtifactKind.Package, issues);
    ValidateArtifactSet(symbolArchives, PackageArtifactKind.Symbols, issues);

    var packageById = GetSingleArchiveById(packageArchives);
    var symbolsById = GetSingleArchiveById(symbolArchives);
    var coreVersion = packageById.TryGetValue(CorePackageId, out var corePackage)
        ? corePackage.Version
        : string.Empty;

    foreach (var expectedPackage in ExpectedPackages) {
      if (packageById.TryGetValue(expectedPackage.Id, out var packageArchive)) {
        ValidatePackageArchive(packageArchive, expectedPackage, coreVersion, issues);
      }

      if (symbolsById.TryGetValue(expectedPackage.Id, out var symbolsArchive)) {
        ValidateSymbolsArchive(symbolsArchive, expectedPackage, packageArchive?.Version, issues);
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
    var expectedFileCount = ExpectedPackages.Count;

    if (archives.Count != expectedFileCount) {
      issues.Add(new PackageVerificationIssue(
          extension,
          "Expected exactly " + expectedFileCount + " " + extension + " artifacts but found " + archives.Count + "."));
    }

    foreach (var archive in archives) {
      if (!ExpectedPackageById.ContainsKey(archive.Id)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Unexpected " + extension + " artifact '" + archive.FileName + "'. Expected only: " + string.Join(", ", ExpectedPackages.Select(package => package.Id)) + "."));
      }

      var expectedFileName = archive.Id + "." + archive.Version + extension;
      if (!string.Equals(archive.FileName, expectedFileName, StringComparison.Ordinal)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Artifact filename '" + archive.FileName + "' does not match nuspec id/version. Expected '" + expectedFileName + "'."));
      }
    }

    foreach (var expectedPackage in ExpectedPackages) {
      var matchingArchives = archives
          .Where(archive => string.Equals(archive.Id, expectedPackage.Id, StringComparison.Ordinal))
          .ToArray();

      if (matchingArchives.Length == 0) {
        issues.Add(new PackageVerificationIssue(
            expectedPackage.Id,
            "Missing expected " + extension + " artifact in the package directory."));
      }
      else if (matchingArchives.Length > 1) {
        issues.Add(new PackageVerificationIssue(
            expectedPackage.Id,
            "Expected exactly one " + extension + " artifact but found " + matchingArchives.Length + ": " + string.Join(", ", matchingArchives.Select(archive => archive.FileName)) + "."));
      }
    }
  }

  private static IReadOnlyDictionary<string, PackageArchive> GetSingleArchiveById(IReadOnlyList<PackageArchive> archives) {
    return archives
        .GroupBy(archive => archive.Id, StringComparer.Ordinal)
        .Where(group => group.Count() == 1)
        .ToDictionary(group => group.Key, group => group.Single(), StringComparer.Ordinal);
  }

  private static void ValidatePackageArchive(
      PackageArchive archive,
      ExpectedPackage expectedPackage,
      string coreVersion,
      List<PackageVerificationIssue> issues) {
    var metadata = GetRequiredMetadataElement(archive);
    AssertMetadataValue(archive, metadata, "id", expectedPackage.Id, issues);
    AssertMetadataValue(archive, metadata, "title", expectedPackage.Title, issues);
    AssertMetadataValue(archive, metadata, "authors", ExpectedAuthors, issues);
    AssertMetadataValue(archive, metadata, "description", expectedPackage.Description, issues);
    AssertMetadataValue(archive, metadata, "readme", ExpectedReadmeFile, issues);
    ValidateTags(archive, metadata, expectedPackage, issues);
    ValidateLicense(archive, metadata, issues);
    ValidateRepository(archive, metadata, issues);
    ValidateReadme(archive, coreVersion, issues);
    ValidateXmlDocumentation(archive, expectedPackage, issues);

    if (expectedPackage.IsProvider) {
      ValidateProviderDependency(archive, coreVersion, issues);
    }
  }

  private static void ValidateSymbolsArchive(
      PackageArchive archive,
      ExpectedPackage expectedPackage,
      string? packageVersion,
      List<PackageVerificationIssue> issues) {
    if (!string.IsNullOrWhiteSpace(packageVersion) &&
        !string.Equals(archive.Version, packageVersion, StringComparison.Ordinal)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Symbols package version '" + archive.Version + "' does not match package version '" + packageVersion + "'."));
    }

    var expectedPdbPath = "lib/" + TargetFramework + "/" + expectedPackage.Id + ".pdb";
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
      string expectedInstallVersion,
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

    foreach (var expectedPackage in ExpectedPackages) {
      var expectedInstallCommand =
          "dotnet add package " + expectedPackage.Id + " --version " + expectedInstallVersion;
      if (!archive.ReadmeText.Contains(expectedInstallCommand, StringComparison.Ordinal)) {
        issues.Add(new PackageVerificationIssue(
            archive.Id,
            "Packaged README.md does not contain the current NuGet installation guidance."));
        return;
      }
    }
  }

  private static void ValidateXmlDocumentation(
      PackageArchive archive,
      ExpectedPackage expectedPackage,
      List<PackageVerificationIssue> issues) {
    var expectedXmlPath = "lib/" + TargetFramework + "/" + expectedPackage.Id + ".xml";
    if (!archive.Entries.Contains(expectedXmlPath)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Package archive '" + archive.FileName + "' is missing generated XML documentation entry '" + expectedXmlPath + "'."));
    }
  }

  private static void ValidateProviderDependency(
      PackageArchive archive,
      string coreVersion,
      List<PackageVerificationIssue> issues) {
    if (string.IsNullOrWhiteSpace(coreVersion)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Cannot verify provider dependency version because the core package version is unavailable."));
      return;
    }

    var metadata = GetRequiredMetadataElement(archive);
    var dependencies = GetChild(metadata, "dependencies");
    var dependency = dependencies?
        .Descendants()
        .FirstOrDefault(element =>
            string.Equals(element.Name.LocalName, "dependency", StringComparison.Ordinal) &&
            string.Equals(element.Attribute("id")?.Value, CorePackageId, StringComparison.Ordinal));

    if (dependency is null) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Provider package does not declare a dependency on " + CorePackageId + "."));
      return;
    }

    var dependencyVersion = dependency.Attribute("version")?.Value ?? string.Empty;
    if (!string.Equals(dependencyVersion, coreVersion, StringComparison.Ordinal)) {
      issues.Add(new PackageVerificationIssue(
          archive.Id,
          "Provider dependency on " + CorePackageId + " uses version '" + dependencyVersion + "' but expected packed core version '" + coreVersion + "'."));
    }
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

  private sealed record ExpectedPackage(
      string Id,
      string Title,
      string Description,
      string[] Tags,
      bool IsProvider);

  private sealed record PackageArchive(
      string FilePath,
      string FileName,
      PackageArtifactKind ArtifactKind,
      string Id,
      string Version,
      XDocument Nuspec,
      IReadOnlySet<string> Entries,
      string? ReadmeText);

  private enum PackageArtifactKind {
    Package,
    Symbols,
  }
}
