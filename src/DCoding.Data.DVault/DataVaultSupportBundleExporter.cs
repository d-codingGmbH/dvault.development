using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DCoding.Data.DVault;

/// <summary>
/// Exports deterministic redacted Data Vault support bundles to JSON.
/// </summary>
public static class DataVaultSupportBundleExporter {
  private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

  /// <summary>
  /// Exports a support-bundle payload to deterministic redacted JSON.
  /// </summary>
  /// <param name="bundle">The support-bundle payload to export.</param>
  /// <returns>The deterministic redacted JSON artifact.</returns>
  public static string ExportJson(DataVaultSupportBundle bundle) {
    ArgumentNullException.ThrowIfNull(bundle);

    var node = JsonSerializer.SerializeToNode(bundle, SerializerOptions) ??
        throw new InvalidOperationException("The Data Vault support bundle could not be serialized.");
    DataVaultSupportBundleRedactor.Redact(node);

    return node.ToJsonString(SerializerOptions);
  }

  /// <summary>
  /// Exports diagnostics and optional support sections to deterministic redacted JSON.
  /// </summary>
  /// <param name="diagnostics">The diagnostics result that supplies validation, explain, save-strategy, and read-strategy data.</param>
  /// <param name="liveSchema">Optional opt-in live-schema read data.</param>
  /// <param name="drift">Optional opt-in model or live-schema drift data.</param>
  /// <returns>The deterministic redacted JSON artifact.</returns>
  public static string ExportJson(
      DataVaultDiagnosticsResult diagnostics,
      DataVaultLiveSchemaReadResult? liveSchema = null,
      DataVaultModelDriftReport? drift = null) {
    return ExportJson(new DataVaultSupportBundle(diagnostics, liveSchema, drift));
  }

  private static JsonSerializerOptions CreateSerializerOptions() {
    var options = new JsonSerializerOptions {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true,
    };
    options.Converters.Add(new JsonStringEnumConverter());

    return options;
  }
}
