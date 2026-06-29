namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable diagnostics fact for one personal-data marker on a satellite payload field.
/// </summary>
public sealed record DataVaultPrivacyPersonalDataCoverageFact(
    string SatelliteName,
    string SatelliteParentKind,
    string SatelliteParentName,
    string FieldName,
    string EncryptedPayloadAlias,
    string CoverageStatus,
    string Message) {
  /// <summary>
  /// Gets the diagnostics path for the marked payload field.
  /// </summary>
  public string Path => "metadata.satellites/" + SatelliteName + "/personalData/" + FieldName;
}
