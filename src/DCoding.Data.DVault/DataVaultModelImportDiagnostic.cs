namespace DCoding.Data.DVault;

/// <summary>
/// Describes one structured diagnostic produced while importing or projecting a dvault.model.v1 artifact.
/// </summary>
public sealed class DataVaultModelImportDiagnostic {
  internal DataVaultModelImportDiagnostic(
      DataVaultDiagnosticDefinition definition,
      string message,
      string jsonPointer,
      string? logicalSourcePath) {
    ArgumentNullException.ThrowIfNull(definition);

    Definition = definition;
    Message = message;
    JsonPointer = jsonPointer;
    LogicalSourcePath = string.IsNullOrWhiteSpace(logicalSourcePath) ? null : logicalSourcePath;
  }

  internal DataVaultDiagnosticDefinition Definition { get; }

  /// <summary>
  /// Gets the diagnostic severity.
  /// </summary>
  public string Severity => Definition.Severity;

  /// <summary>
  /// Gets the stable diagnostic category.
  /// </summary>
  public string Category => Definition.Category;

  /// <summary>
  /// Gets the stable diagnostic code.
  /// </summary>
  public string Code => Definition.Code;

  /// <summary>
  /// Gets the diagnostic message.
  /// </summary>
  public string Message { get; }

  /// <summary>
  /// Gets the JSON Pointer for the artifact element that caused the diagnostic.
  /// </summary>
  public string JsonPointer { get; }

  /// <summary>
  /// Gets the caller-supplied logical artifact source path, when one was supplied.
  /// </summary>
  public string? LogicalSourcePath { get; }
}
