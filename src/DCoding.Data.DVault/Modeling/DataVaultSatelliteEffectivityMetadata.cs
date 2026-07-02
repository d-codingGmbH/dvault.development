using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes effectivity payload fields carried by a Data Vault satellite.
/// </summary>
public sealed class DataVaultSatelliteEffectivityMetadata {
  /// <summary>
  /// Initializes a new satellite effectivity metadata declaration.
  /// </summary>
  /// <param name="effectiveFromFieldName">The payload field that stores the lower effectivity boundary.</param>
  /// <param name="effectiveToFieldName">The optional payload field that stores the upper effectivity boundary.</param>
  /// <param name="currentFlagFieldName">The optional payload field that stores a current-row marker or status value.</param>
  public DataVaultSatelliteEffectivityMetadata(
      string effectiveFromFieldName,
      string? effectiveToFieldName = null,
      string? currentFlagFieldName = null) {
    EffectiveFromFieldName = DataVaultMetadataValidation.RequireName(
        effectiveFromFieldName,
        nameof(effectiveFromFieldName));
    EffectiveToFieldName = NormalizeOptionalName(effectiveToFieldName, nameof(effectiveToFieldName));
    CurrentFlagFieldName = NormalizeOptionalName(currentFlagFieldName, nameof(currentFlagFieldName));
    ValidateDistinctFieldNames();
  }

  /// <summary>
  /// Gets the payload field that stores the lower effectivity boundary.
  /// </summary>
  public string EffectiveFromFieldName { get; }

  /// <summary>
  /// Gets the optional payload field that stores the upper effectivity boundary.
  /// </summary>
  public string? EffectiveToFieldName { get; }

  /// <summary>
  /// Gets the optional payload field that stores a current-row marker or status value.
  /// </summary>
  public string? CurrentFlagFieldName { get; }

  internal DataVaultEffectivityRole? GetRole(string fieldName) {
    if (string.Equals(fieldName, EffectiveFromFieldName, StringComparison.Ordinal)) {
      return DataVaultEffectivityRole.EffectiveFrom;
    }

    if (string.Equals(fieldName, EffectiveToFieldName, StringComparison.Ordinal)) {
      return DataVaultEffectivityRole.EffectiveTo;
    }

    return string.Equals(fieldName, CurrentFlagFieldName, StringComparison.Ordinal)
        ? DataVaultEffectivityRole.CurrentFlag
        : null;
  }

  private static string? NormalizeOptionalName(string? value, string parameterName) {
    if (value is null) {
      return null;
    }

    ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
    return value;
  }

  private void ValidateDistinctFieldNames() {
    var names = new HashSet<string>(StringComparer.Ordinal) {
        EffectiveFromFieldName,
    };

    if (EffectiveToFieldName is not null && !names.Add(EffectiveToFieldName)) {
      throw new ArgumentException(
          "Satellite effectivity field names must be distinct by ordinal comparison.",
          nameof(EffectiveToFieldName));
    }

    if (CurrentFlagFieldName is not null && !names.Add(CurrentFlagFieldName)) {
      throw new ArgumentException(
          "Satellite effectivity field names must be distinct by ordinal comparison.",
          nameof(CurrentFlagFieldName));
    }
  }
}
