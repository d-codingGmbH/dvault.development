using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies one PIT metadata declaration from the authoritative DbContext registry for a full rebuild.
/// </summary>
public sealed class DataVaultRegistryPitRebuildRequest {
  /// <summary>
  /// Initializes a new registry-backed PIT rebuild request by logical PIT name.
  /// </summary>
  /// <param name="pitName">The exact logical PIT metadata name to resolve from the authoritative registry.</param>
  public DataVaultRegistryPitRebuildRequest(string pitName) {
    PitName = DataVaultMetadataValidation.RequireName(pitName, nameof(pitName));
  }

  /// <summary>
  /// Initializes a new registry-backed PIT rebuild request by exact CLR mapping.
  /// </summary>
  /// <param name="pitClrType">The exact CLR type mapped to a PIT declaration in the authoritative registry.</param>
  public DataVaultRegistryPitRebuildRequest(Type pitClrType) {
    ArgumentNullException.ThrowIfNull(pitClrType);

    PitClrType = pitClrType;
  }

  /// <summary>
  /// Gets the exact logical PIT metadata name to resolve, when name-based lookup was selected.
  /// </summary>
  public string? PitName { get; }

  /// <summary>
  /// Gets the exact CLR type to resolve, when CLR mapping lookup was selected.
  /// </summary>
  public Type? PitClrType { get; }
}
