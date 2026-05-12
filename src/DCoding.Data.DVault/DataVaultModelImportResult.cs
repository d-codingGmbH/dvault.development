using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Contains the parsed metadata output and diagnostics for a dvault.model.v1 import attempt.
/// </summary>
public sealed class DataVaultModelImportResult {
  internal DataVaultModelImportResult(
      DataVaultModelArtifact? artifact,
      DataVaultMetadataModel? metadataModel,
      DataVaultMetadataRegistry? metadataRegistry,
      IReadOnlyList<DataVaultModelImportDiagnostic> diagnostics,
      string? logicalSourcePath) {
    Artifact = artifact;
    MetadataModel = metadataModel;
    MetadataRegistry = metadataRegistry;
    Diagnostics = diagnostics;
    LogicalSourcePath = string.IsNullOrWhiteSpace(logicalSourcePath) ? null : logicalSourcePath;
  }

  internal DataVaultModelArtifact? Artifact { get; }

  /// <summary>
  /// Gets a value indicating whether the import completed without error diagnostics.
  /// </summary>
  public bool IsValid => Diagnostics.All(
      diagnostic => !string.Equals(diagnostic.Severity, "error", StringComparison.Ordinal));

  /// <summary>
  /// Gets the provider-neutral metadata model built from the artifact when parsing and import validation succeeded.
  /// </summary>
  public DataVaultMetadataModel? MetadataModel { get; }

  /// <summary>
  /// Gets the immutable metadata registry built from the artifact when parsing and import validation succeeded.
  /// </summary>
  public DataVaultMetadataRegistry? MetadataRegistry { get; }

  /// <summary>
  /// Gets the structured import and projection diagnostics.
  /// </summary>
  public IReadOnlyList<DataVaultModelImportDiagnostic> Diagnostics { get; }

  /// <summary>
  /// Gets the caller-supplied logical artifact source path, when one was supplied.
  /// </summary>
  public string? LogicalSourcePath { get; }

  /// <summary>
  /// Gets the load-timestamp storage choice declared by the artifact.
  /// </summary>
  public DataVaultLoadTimestampStorage LoadTimestampStorage =>
      Artifact?.LoadTimestampStorage ?? DataVaultLoadTimestampStorage.ProviderDefault;

  /// <summary>
  /// Projects the imported registry into the supplied Entity Framework model builder and returns projection diagnostics.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <returns>The current result when projection succeeds; otherwise a copy containing the projection diagnostic.</returns>
  public DataVaultModelImportResult ApplyTo(ModelBuilder modelBuilder) {
    return ApplyToCore(modelBuilder, providerCapabilities: null);
  }

  /// <summary>
  /// Projects the imported registry into the supplied Entity Framework model builder for one provider profile and returns projection diagnostics.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <param name="providerCapabilities">The provider capability profile used to project storage metadata.</param>
  /// <returns>The current result when projection succeeds; otherwise a copy containing the projection diagnostic.</returns>
  public DataVaultModelImportResult ApplyTo(
      ModelBuilder modelBuilder,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    return ApplyToCore(modelBuilder, providerCapabilities);
  }

  private DataVaultModelImportResult ApplyToCore(
      ModelBuilder modelBuilder,
      DataVaultProviderCapabilityProfile? providerCapabilities) {
    ArgumentNullException.ThrowIfNull(modelBuilder);

    if (!IsValid || MetadataRegistry is null) {
      return this;
    }

    try {
      DataVaultModelBuilderExtensions.ApplyDataVaultMetadataRegistry(
          modelBuilder,
          MetadataRegistry,
          DataVaultMetadataSourceKinds.ModelArtifact,
          providerCapabilities);
      return this;
    }
    catch (Exception exception) when (IsProjectionException(exception)) {
      var diagnostic = new DataVaultModelImportDiagnostic(
          "error",
          "projection",
          "DMV1801",
          "The imported artifact could not be projected to Entity Framework metadata: " + exception.Message,
          DataVaultModelArtifactParser.ResolveDeclarationPath(Artifact, exception),
          LogicalSourcePath);
      return WithDiagnostic(diagnostic);
    }
  }

  internal DataVaultMetadataRegistry RequireMetadataRegistry() {
    if (IsValid && MetadataRegistry is not null) {
      return MetadataRegistry;
    }

    throw new InvalidOperationException(
        "The Data Vault model import result cannot be used because it contains error diagnostics." +
        Environment.NewLine +
        FormatDiagnostics(Diagnostics));
  }

  internal void ThrowIfInvalid() {
    if (IsValid) {
      return;
    }

    throw new InvalidOperationException(
        "The Data Vault model import result contains error diagnostics." +
        Environment.NewLine +
        FormatDiagnostics(Diagnostics));
  }

  internal static string FormatDiagnostics(IEnumerable<DataVaultModelImportDiagnostic> diagnostics) {
    return string.Join(
        Environment.NewLine,
        diagnostics.Select(diagnostic =>
            diagnostic.Severity +
            " " +
            diagnostic.Category +
            " " +
            diagnostic.Code +
            " " +
            FormatLocation(diagnostic) +
            ": " +
            diagnostic.Message));
  }

  private DataVaultModelImportResult WithDiagnostic(DataVaultModelImportDiagnostic diagnostic) {
    return new DataVaultModelImportResult(
        Artifact,
        MetadataModel,
        MetadataRegistry,
        Diagnostics.Concat([diagnostic]).ToArray(),
        LogicalSourcePath);
  }

  private static bool IsProjectionException(Exception exception) {
    return exception is ArgumentException or InvalidOperationException or NotSupportedException;
  }

  private static string FormatLocation(DataVaultModelImportDiagnostic diagnostic) {
    if (string.IsNullOrWhiteSpace(diagnostic.LogicalSourcePath)) {
      return diagnostic.JsonPointer;
    }

    return diagnostic.LogicalSourcePath + diagnostic.JsonPointer;
  }
}
