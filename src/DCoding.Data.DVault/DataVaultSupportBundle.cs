using System.Text.Json.Serialization;

namespace DCoding.Data.DVault;

/// <summary>
/// Stable redacted support-bundle payload for Data Vault configuration and provider-behavior troubleshooting.
/// </summary>
public sealed class DataVaultSupportBundle {
  /// <summary>
  /// The support-bundle JSON schema version.
  /// </summary>
  public const string CurrentSchemaVersion = "dvault.support-bundle.v1";

  /// <summary>
  /// Initializes a new support-bundle payload.
  /// </summary>
  /// <param name="diagnostics">The diagnostics result that supplies validation, explain, save-strategy, and read-strategy data.</param>
  /// <param name="liveSchema">Optional opt-in live-schema read data.</param>
  /// <param name="drift">Optional opt-in model or live-schema drift data.</param>
  public DataVaultSupportBundle(
      DataVaultDiagnosticsResult diagnostics,
      DataVaultLiveSchemaReadResult? liveSchema = null,
      DataVaultModelDriftReport? drift = null) {
    ArgumentNullException.ThrowIfNull(diagnostics);

    Diagnostics = diagnostics;
    LiveSchema = liveSchema;
    Drift = drift;
  }

  /// <summary>
  /// Gets the support-bundle JSON schema version.
  /// </summary>
  [JsonPropertyOrder(0)]
  public string SchemaVersion => CurrentSchemaVersion;

  /// <summary>
  /// Gets the diagnostics result that supplies validation, explain, save-strategy, and read-strategy data.
  /// </summary>
  [JsonPropertyOrder(1)]
  public DataVaultDiagnosticsResult Diagnostics { get; }

  /// <summary>
  /// Gets optional opt-in live-schema read data.
  /// </summary>
  [JsonPropertyOrder(2)]
  public DataVaultLiveSchemaReadResult? LiveSchema { get; }

  /// <summary>
  /// Gets optional opt-in model or live-schema drift data.
  /// </summary>
  [JsonPropertyOrder(3)]
  public DataVaultModelDriftReport? Drift { get; }
}
