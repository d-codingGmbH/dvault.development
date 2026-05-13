namespace DCoding.Data.DVault;

/// <summary>
/// Imports strict JSON dvault.model.v1 artifacts into DVault metadata models and registries.
/// </summary>
public static class DataVaultModelArtifactImporter {
  /// <summary>
  /// Imports a strict JSON dvault.model.v1 artifact into provider-neutral metadata and a reusable registry.
  /// </summary>
  /// <param name="json">The strict JSON artifact content.</param>
  /// <param name="logicalSourcePath">An optional logical artifact source path used only in diagnostics.</param>
  /// <returns>The import result containing diagnostics and, when valid, metadata model and registry outputs.</returns>
  public static DataVaultModelImportResult ImportJson(
      string json,
      string? logicalSourcePath = null) {
    ArgumentNullException.ThrowIfNull(json);

    var parseResult = DataVaultModelArtifactParser.Parse(json);
    var diagnostics = parseResult.Diagnostics
        .Select(diagnostic => new DataVaultModelImportDiagnostic(
            diagnostic.Definition,
            diagnostic.Message,
            diagnostic.Path,
            logicalSourcePath))
        .ToArray();

    return new DataVaultModelImportResult(
        parseResult.Artifact,
        parseResult.MetadataModel,
        parseResult.MetadataRegistry,
        diagnostics,
        logicalSourcePath);
  }

  internal static IReadOnlyList<DataVaultProviderCapabilityProfile> CreateProviderCapabilityProfiles(
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    return
    [
        DataVaultProviderCapabilityProfiles.Sqlite.WithLoadTimestampStorage(loadTimestampStorage),
        DataVaultProviderCapabilityProfiles.Oracle.WithLoadTimestampStorage(loadTimestampStorage),
        DataVaultProviderCapabilityProfiles.Postgres.WithLoadTimestampStorage(loadTimestampStorage),
        DataVaultProviderCapabilityProfiles.SqlServer.WithLoadTimestampStorage(loadTimestampStorage),
        DataVaultProviderCapabilityProfiles.MySql.WithLoadTimestampStorage(loadTimestampStorage),
    ];
  }
}
