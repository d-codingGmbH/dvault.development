using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one unsafe generated Data Vault row detected by the optional SaveChanges guard.
/// </summary>
public sealed class DataVaultSaveChangesGuardFinding {
  /// <summary>
  /// Initializes a new instance of the DataVaultSaveChangesGuardFinding class.
  /// </summary>
  /// <param name="tableName">The effective generated table or shared-type entity name.</param>
  /// <param name="entityKind">The generated Data Vault entity kind.</param>
  /// <param name="metadataName">The provider-neutral Data Vault metadata name.</param>
  /// <param name="state">The tracked Entity Framework state that triggered the finding.</param>
  /// <param name="reasons">The deterministic finding reasons.</param>
  public DataVaultSaveChangesGuardFinding(
      string tableName,
      DataVaultTableKind entityKind,
      string metadataName,
      EntityState state,
      IEnumerable<string> reasons) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentException.ThrowIfNullOrWhiteSpace(metadataName);
    ArgumentNullException.ThrowIfNull(reasons);

    var reasonArray = reasons.ToArray();
    if (reasonArray.Length == 0) {
      throw new ArgumentException("At least one guard finding reason is required.", nameof(reasons));
    }

    foreach (var reason in reasonArray) {
      ArgumentException.ThrowIfNullOrWhiteSpace(reason);
    }

    TableName = tableName;
    EntityKind = entityKind;
    MetadataName = metadataName;
    State = state;
    Reasons = reasonArray;
  }

  /// <summary>
  /// Gets the effective generated table or shared-type entity name.
  /// </summary>
  public string TableName { get; }

  /// <summary>
  /// Gets the generated Data Vault entity kind.
  /// </summary>
  public DataVaultTableKind EntityKind { get; }

  /// <summary>
  /// Gets the provider-neutral Data Vault metadata name.
  /// </summary>
  public string MetadataName { get; }

  /// <summary>
  /// Gets the tracked Entity Framework state that triggered the finding.
  /// </summary>
  public EntityState State { get; }

  /// <summary>
  /// Gets the deterministic finding reasons.
  /// </summary>
  public IReadOnlyList<string> Reasons { get; }

  /// <summary>
  /// Formats this finding as a deterministic single-line explanation.
  /// </summary>
  /// <returns>A deterministic explanation suitable for exceptions, warnings, tests, and diagnostics.</returns>
  public string ToDisplayString() {
    return EntityKind +
        " '" +
        MetadataName +
        "' mapped to '" +
        TableName +
        "' in " +
        State +
        " state: " +
        string.Join("; ", Reasons);
  }
}
