using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DCoding.Data.DVault;

internal static class DataVaultSqlArtifactManifestExporter {
  public const string CurrentSchemaVersion = "dvault.sql-artifact.v1";
  public const string SupportedWorkloadLabel = "provider-native-bulk-ingestion";

  private const string SqlServerExternalProviderLabel = "SQL Server external provider";
  private const string SqlServerSelectedStrategyName = "SqlServerDataVaultSaveStrategy";
  private const string SqlServerOptimizedBaselineName = "dvault-adddvaultsqlserver-optimized";
  private const string ProviderNeutralFallbackBaselineName = "dvault-adddvault-fallback";

  private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

  public static string ExportSqlServerProviderNativeBulkIngestionDryRunJson(
      DataVaultDiagnosticsResult diagnostics,
      string diagnosticsSourceKind) {
    ArgumentNullException.ThrowIfNull(diagnostics);
    ArgumentException.ThrowIfNullOrWhiteSpace(diagnosticsSourceKind);

    ValidateSqlServerProviderNativeBulkIngestionDiagnostics(diagnostics);

    return JsonSerializer.Serialize(
        CreateSqlServerProviderNativeBulkIngestionDryRunManifest(diagnostics, diagnosticsSourceKind),
        SerializerOptions);
  }

  public static void ValidateSupportedWorkload(string workloadLabel) {
    ArgumentException.ThrowIfNullOrWhiteSpace(workloadLabel);

    if (!string.Equals(workloadLabel, SupportedWorkloadLabel, StringComparison.Ordinal)) {
      throw new InvalidOperationException(
          "The SQL artifact dry-run prototype only supports workload '" +
          SupportedWorkloadLabel +
          "'.");
    }
  }

  private static void ValidateSqlServerProviderNativeBulkIngestionDiagnostics(
      DataVaultDiagnosticsResult diagnostics) {
    if (string.IsNullOrWhiteSpace(diagnostics.Explain.MetadataSourceKind)) {
      throw new InvalidOperationException(
          "The SQL artifact dry-run manifest requires diagnostics with a metadata-source kind.");
    }

    if (string.IsNullOrWhiteSpace(diagnostics.Explain.MetadataSourceFingerprint)) {
      throw new InvalidOperationException(
          "The SQL artifact dry-run manifest requires diagnostics with a metadata-source fingerprint.");
    }

    if (!string.Equals(diagnostics.Explain.ProviderName, KnownProviderNames.SqlServer, StringComparison.Ordinal)) {
      throw new InvalidOperationException(
          "The SQL artifact dry-run manifest requires explain diagnostics for provider '" +
          KnownProviderNames.SqlServer +
          "'.");
    }

    if (!string.Equals(diagnostics.SaveStrategy.ProviderName, KnownProviderNames.SqlServer, StringComparison.Ordinal)) {
      throw new InvalidOperationException(
          "The SQL artifact dry-run manifest requires request-bound save-strategy diagnostics for provider '" +
          KnownProviderNames.SqlServer +
          "'.");
    }

    if (!string.Equals(
        diagnostics.Explain.CapabilityProfileName,
        DataVaultProviderCapabilityProfiles.SqlServer.ProfileName,
        StringComparison.Ordinal)) {
      throw new InvalidOperationException(
          "The SQL artifact dry-run manifest requires capability profile '" +
          DataVaultProviderCapabilityProfiles.SqlServer.ProfileName +
          "'.");
    }

    if (diagnostics.SaveStrategy.Status != DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected ||
        !string.Equals(diagnostics.SaveStrategy.SelectedStrategyName, SqlServerSelectedStrategyName, StringComparison.Ordinal)) {
      throw new InvalidOperationException(
          "The SQL artifact dry-run manifest requires request-bound save diagnostics that select '" +
          SqlServerSelectedStrategyName +
          "'.");
    }
  }

  private static DataVaultSqlArtifactManifest CreateSqlServerProviderNativeBulkIngestionDryRunManifest(
      DataVaultDiagnosticsResult diagnostics,
      string diagnosticsSourceKind) {
    return new DataVaultSqlArtifactManifest(
        CurrentSchemaVersion,
        new DataVaultSqlArtifactDryRun(
            Enabled: true,
            Status: "review-only",
            Deployment: "not-generated",
            RuntimeDispatch: "not-generated",
            PayloadPolicy: "manifest-only-no-sidecar-sql"),
        new DataVaultSqlArtifactProvider(
            KnownProviderNames.SqlServer,
            SqlServerExternalProviderLabel,
            diagnostics.Explain.CapabilityProfileName,
            SqlServerSelectedStrategyName),
        new DataVaultSqlArtifactMetadataSource(
            diagnostics.Explain.MetadataSourceKind,
            diagnostics.Explain.MetadataSourceFingerprint!),
        new DataVaultSqlArtifactWorkload(
            SupportedWorkloadLabel,
            "one provider-eligible bulk request",
            OrderProductPairCount: 20,
            HubOperationCount: 40,
            OrderProductLinkCount: 20,
            LinkOperationCount: 20,
            FulfillmentSatelliteOperationCount: 3,
            SatelliteOperationCount: 3,
            UnchangedReplayCount: 1,
            TotalOperationCount: 63,
            SelectedStrategy: SqlServerSelectedStrategyName,
            Transfer: "SqlBulkCopy",
            NativeBulkBoundary: "50-plus-operations",
            CleanupBoundary: "temporary-staging-table"),
        new DataVaultSqlArtifactDiagnosticsReference(
            diagnosticsSourceKind,
            diagnostics.SaveStrategy.Status.ToString(),
            diagnostics.SaveStrategy.SelectedStrategyName!),
        new DataVaultSqlArtifactEvidence(
            ["benchmark-summary.md", "benchmark-summary.csv", "benchmark-summary.json"],
            [
                new DataVaultSqlArtifactBenchmarkRowReference(
                    SupportedWorkloadLabel,
                    SqlServerExternalProviderLabel,
                    ProviderNeutralFallbackBaselineName,
                    "provider-neutral-fallback"),
                new DataVaultSqlArtifactBenchmarkRowReference(
                    SupportedWorkloadLabel,
                    SqlServerExternalProviderLabel,
                    SqlServerOptimizedBaselineName,
                    "sqlserver-optimized-dry-run-reference"),
            ]),
        new DataVaultSqlArtifactSemanticParity(
            Ordering: "caller request order with hub, link, then satellite operation groups and staged ordinal ordering",
            LoadTimestamp: "caller-supplied DataVaultSaveRequest load timestamp through the sqlserver-v1 native DateTimeOffset mapping",
            RecordSource: "caller-supplied DataVaultSaveRequest record source without manifest payload values",
            HashKey: "stable hash service over normalized business-key and participant fields",
            HashDiff: "request-bound satellite operation hash diff",
            LatestStateBehavior: "latest satellite hash-diff lookup skips the unchanged replay and advances state by load timestamp",
            Cancellation: "design-time generation has no database side effects; selected strategy observes cancellation at provider operations",
            Cleanup: "temporary staging table boundary owned by the SQL Server provider save strategy",
            CallerOwnedTransaction: "selected strategy participates in an existing caller transaction when present"),
        [
            new DataVaultSqlArtifactEntry(
                Ordinal: 0,
                ArtifactKind: "dry-run-manifest",
                ObjectIdentity: "sqlserver-provider-native-bulk-ingestion",
                LifecycleIntent: "review-only",
                PayloadFiles: []),
        ],
        SidecarPayloads: []);
  }

  private static JsonSerializerOptions CreateSerializerOptions() {
    return new JsonSerializerOptions {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true,
    };
  }

  private sealed record DataVaultSqlArtifactManifest(
      [property: JsonPropertyOrder(0)] string SchemaVersion,
      [property: JsonPropertyOrder(1)] DataVaultSqlArtifactDryRun DryRun,
      [property: JsonPropertyOrder(2)] DataVaultSqlArtifactProvider Provider,
      [property: JsonPropertyOrder(3)] DataVaultSqlArtifactMetadataSource MetadataSource,
      [property: JsonPropertyOrder(4)] DataVaultSqlArtifactWorkload Workload,
      [property: JsonPropertyOrder(5)] DataVaultSqlArtifactDiagnosticsReference RequestDiagnostics,
      [property: JsonPropertyOrder(6)] DataVaultSqlArtifactEvidence Evidence,
      [property: JsonPropertyOrder(7)] DataVaultSqlArtifactSemanticParity SemanticParity,
      [property: JsonPropertyOrder(8)] IReadOnlyList<DataVaultSqlArtifactEntry> Entries,
      [property: JsonPropertyOrder(9)] IReadOnlyList<DataVaultSqlArtifactPayloadFile> SidecarPayloads);

  private sealed record DataVaultSqlArtifactDryRun(
      bool Enabled,
      string Status,
      string Deployment,
      string RuntimeDispatch,
      string PayloadPolicy);

  private sealed record DataVaultSqlArtifactProvider(
      string Name,
      string ExternalProviderLabel,
      string CapabilityProfile,
      string SelectedStrategy);

  private sealed record DataVaultSqlArtifactMetadataSource(string Kind, string Fingerprint);

  private sealed record DataVaultSqlArtifactWorkload(
      string Label,
      string RequestShape,
      int OrderProductPairCount,
      int HubOperationCount,
      int OrderProductLinkCount,
      int LinkOperationCount,
      int FulfillmentSatelliteOperationCount,
      int SatelliteOperationCount,
      int UnchangedReplayCount,
      int TotalOperationCount,
      string SelectedStrategy,
      string Transfer,
      string NativeBulkBoundary,
      string CleanupBoundary);

  private sealed record DataVaultSqlArtifactDiagnosticsReference(
      string SourceKind,
      string SaveStrategyStatus,
      string SelectedStrategyName);

  private sealed record DataVaultSqlArtifactEvidence(
      IReadOnlyList<string> BenchmarkArtifactTriplet,
      IReadOnlyList<DataVaultSqlArtifactBenchmarkRowReference> BenchmarkRows);

  private sealed record DataVaultSqlArtifactBenchmarkRowReference(
      string Scenario,
      string Provider,
      string Baseline,
      string Role);

  private sealed record DataVaultSqlArtifactSemanticParity(
      string Ordering,
      string LoadTimestamp,
      string RecordSource,
      string HashKey,
      string HashDiff,
      string LatestStateBehavior,
      string Cancellation,
      string Cleanup,
      string CallerOwnedTransaction);

  private sealed record DataVaultSqlArtifactEntry(
      int Ordinal,
      string ArtifactKind,
      string ObjectIdentity,
      string LifecycleIntent,
      IReadOnlyList<DataVaultSqlArtifactPayloadFile> PayloadFiles);

  private sealed record DataVaultSqlArtifactPayloadFile(
      string Path,
      string Sha256);
}
