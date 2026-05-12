namespace DCoding.Data.DVault;

/// <summary>
/// Describes one structured diagnostic produced while importing or projecting a dvault.model.v1 artifact.
/// </summary>
public sealed class DataVaultModelImportDiagnostic {
  internal DataVaultModelImportDiagnostic(
      string severity,
      string category,
      string code,
      string message,
      string jsonPointer,
      string? logicalSourcePath) {
    Severity = severity;
    Category = category;
    Code = code;
    Message = message;
    JsonPointer = jsonPointer;
    LogicalSourcePath = string.IsNullOrWhiteSpace(logicalSourcePath) ? null : logicalSourcePath;
  }

  /// <summary>
  /// Gets the diagnostic severity.
  /// </summary>
  public string Severity { get; }

  /// <summary>
  /// Gets the stable diagnostic category.
  /// </summary>
  public string Category { get; }

  /// <summary>
  /// Gets the stable diagnostic code.
  /// </summary>
  public string Code { get; }

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
