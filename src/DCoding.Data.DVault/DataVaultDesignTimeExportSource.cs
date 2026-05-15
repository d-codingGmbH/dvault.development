using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one explicit source that can be exported to canonical <c>dvault.model.v1</c> JSON by a design-time command.
/// </summary>
public sealed class DataVaultDesignTimeExportSource {
  private readonly Func<string> _exportJson;

  private DataVaultDesignTimeExportSource(Func<string> exportJson) {
    _exportJson = exportJson;
  }

  /// <summary>
  /// Creates an export source from fluent Code-First Data Vault declarations.
  /// </summary>
  /// <param name="configureModel">The fluent Code-First Data Vault metadata declarations to export.</param>
  /// <returns>An export source that emits canonical <c>dvault.model.v1</c> JSON.</returns>
  public static DataVaultDesignTimeExportSource FromCodeFirst(Action<DataVaultCodeFirstModelBuilder> configureModel) {
    ArgumentNullException.ThrowIfNull(configureModel);

    return new DataVaultDesignTimeExportSource(() => DataVaultModelArtifactExporter.ExportJson(configureModel));
  }

  /// <summary>
  /// Creates an export source from an existing provider-neutral metadata model.
  /// </summary>
  /// <param name="metadataModel">The already-materialized metadata model to export.</param>
  /// <returns>An export source that emits canonical <c>dvault.model.v1</c> JSON.</returns>
  public static DataVaultDesignTimeExportSource FromMetadataModel(DataVaultMetadataModel metadataModel) {
    ArgumentNullException.ThrowIfNull(metadataModel);

    return new DataVaultDesignTimeExportSource(() => DataVaultModelArtifactExporter.ExportJson(metadataModel));
  }

  /// <summary>
  /// Creates an export source from an existing immutable metadata registry.
  /// </summary>
  /// <param name="metadataRegistry">The already-materialized metadata registry to export.</param>
  /// <returns>An export source that emits canonical <c>dvault.model.v1</c> JSON.</returns>
  public static DataVaultDesignTimeExportSource FromMetadataRegistry(DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    return new DataVaultDesignTimeExportSource(() => DataVaultModelArtifactExporter.ExportJson(metadataRegistry));
  }

  internal string ExportJson() {
    return _exportJson();
  }
}
