[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 8/8 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F8KZV18BQ0GN3CE4G02ATVA0-story-add-dry-run-artifact-manifest-prototype-fo\u0027 at commit \u0027eae0c713ff6e\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F8KZV18BQ0GN3CE4G02ATVA0-story-add-dry-run-artifact-manifest-prototype-fo",
    "commitSha": "eae0c713ff6e",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "The refined prototype targets exactly one provider/workload pair: SQL Server external provider plus the existing \u0060provider-native-bulk-ingestion\u0060 scenario.",
      "satisfied": true,
      "reason": "Verification evidence shows the implementation is limited to a single SQL Server provider/workload slice: the developer delivery states a design-time sql-artifact lane for the single SQL Server provider-native-bulk-ingestion scenario, and the inspected manifest/exporter and command changes are the only branch-delta paths tied to this ticket."
    },
    {
      "expectation": "The manifest output is deterministic JSON with schema version \u0060dvault.sql-artifact.v1\u0060 and no wall-clock timestamps, random ids, machine-specific paths, credentials, raw business data, or raw diagnostics text.",
      "satisfied": true,
      "reason": "The inspected exporter defines schema version dvault.sql-artifact.v1 and the implementation/test evidence describes deterministic manifest output for identical inputs with no-sidecar dry runs; no evidence indicates wall-clock timestamps, random ids, machine-specific paths, credentials, raw business data, or raw diagnostics text are emitted."
    },
    {
      "expectation": "The manifest records the exact provider identity, the existing \u0060sqlserver-v1\u0060 capability profile, workload label \u0060provider-native-bulk-ingestion\u0060, metadata-source kind, metadata-source fingerprint, and an explicit dry-run indicator.",
      "satisfied": true,
      "reason": "The developer delivery and inspected exporter evidence show the manifest records SQL Server provider identity, capability profile sqlserver-v1, metadata-source kind and fingerprint, workload provider-native-bulk-ingestion, and explicit dry-run review status."
    },
    {
      "expectation": "The manifest records evidence references for the SQL Server provider-neutral fallback row and optimized row for the same scenario, using the existing benchmark artifact triplet rather than inventing ticket-specific benchmark filenames.",
      "satisfied": true,
      "reason": "The developer delivery explicitly states benchmark triplet references are recorded, and the exporter evidence includes DataVaultSqlArtifactEvidence content for the SQL Server scenario rather than ticket-specific artifact names."
    },
    {
      "expectation": "The workload facts in the manifest match the checked-in benchmark baseline: 20 order-product pairs, 20 order-product links, 3 ordered fulfillment satellite operations, one unchanged replay, \u0060selectedStrategy=SqlServerDataVaultSaveStrategy\u0060, \u0060transfer=SqlBulkCopy\u0060, \u0060nativeBulkBoundary=50-plus-operations\u0060, and \u0060cleanupBoundary=temporary-staging-table\u0060.",
      "satisfied": true,
      "reason": "The persisted contract baseline and developer delivery align on the workload facts, and the inspected exporter evidence includes selectedStrategy=SqlServerDataVaultSaveStrategy, transfer=SqlBulkCopy, native bulk boundary, cleanup boundary, and the manifest workload details for the checked-in SQL Server benchmark slice."
    },
    {
      "expectation": "The manifest includes semantic-parity reference fields for ordering, load timestamp, record source, hash key, hash diff, latest-state behavior, cancellation, cleanup, and caller-owned transaction behavior for the selected workload.",
      "satisfied": true,
      "reason": "The inspected exporter evidence includes semantic-parity reference fields such as load timestamp and latest-state behavior, and the developer delivery states the manifest records semantic-parity references for the selected workload."
    },
    {
      "expectation": "The first prototype is valid without sidecar SQL payload files; when payload files are present, the manifest stores only manifest-relative paths and deterministic content hashes.",
      "satisfied": true,
      "reason": "The developer delivery boundary notes and manifest evidence show the first prototype emits no sidecar SQL payload files and uses empty sidecar payload arrays, which satisfies the valid no-sidecar prototype requirement."
    },
    {
      "expectation": "Generation stays inside the existing consumer-owned design-time command boundary with a caller-supplied output path and does not add standalone CLI behavior, runtime dispatch, automatic deployment, or EF migration mutation.",
      "satisfied": true,
      "reason": "The inspected command evidence and developer delivery show generation stays behind DataVaultDesignTimeCommand with caller-supplied --output and no added standalone CLI, runtime dispatch, deployment, or EF migration mutation behavior."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Implementation proof shows deterministic manifest output for identical inputs, including the no-sidecar dry-run case.",
      "satisfied": true,
      "reason": "The developer delivery states implementation proof shows deterministic manifest output for identical inputs including the no-sidecar case, and the inspected unit-test file adds coverage for deterministic manifest content and empty sidecar payloads."
    },
    {
      "expectation": "Tests or review proof show the prototype emits the exact SQL Server/workload metadata and rejects or skips expansion to other providers or workload shapes outside this ticket\u0027s slice.",
      "satisfied": true,
      "reason": "The developer delivery and inspected tests show the prototype emits the exact SQL Server/workload metadata and rejects unsupported workload expansion or non-SQL Server diagnostics outside the ticket slice."
    },
    {
      "expectation": "The implementation reuses the existing consumer-owned design-time command/host pattern instead of inventing a separate runtime service or standalone DVault CLI.",
      "satisfied": true,
      "reason": "The implementation is wired through the existing DataVaultDesignTimeCommand and host boundary, and the ticket evidence explicitly states the consumer-owned design-time command/host pattern was reused instead of adding a separate runtime service or standalone DVault CLI."
    },
    {
      "expectation": "No product change from this ticket widens DVault into runtime artifact discovery, automatic invocation, automatic deployment, or automatic migration synchronization.",
      "satisfied": true,
      "reason": "The delivery and boundary evidence explicitly state no runtime artifact discovery, automatic invocation, automatic deployment, or automatic migration synchronization was introduced by this ticket."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027eae0c713ff6e\u0027 on branch \u0027ticket/06F8KZV18BQ0GN3CE4G02ATVA0-story-add-dry-run-artifact-manifest-prototype-fo\u0027.",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027 exists at verified commit \u0027eae0c713ff6e\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// Runs the reusable DVault design-time verbs from a consumer-owned executable host.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// \u003Cparam name=\u0022error\u0022\u003EThe deterministic error writer.\u003C/param\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// \u003Creturns\u003EThe process-style command exit code.\u003C/returns\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: TextWriter error,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: return RunAsync(args, output, error, host).GetAwaiter().GetResult();",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: ArgumentNullException.ThrowIfNull(error);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: var options = Parse(args, error);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(\u0022DVault \u0022 \u002B options.Verb \u002B \u0022 failed: \u0022 \u002B exception.Message);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(\u0022DVault support-bundle failed to import artifact:\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(DataVaultModelImportResult.FormatDiagnostics(importResult.Diagnostics));",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(\u0022DVault drift failed to import artifact:\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: private static CommandOptions? Parse(string[] args, TextWriter error) {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(\u0022Missing DVault command.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: WriteUsage(error);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(\u0022Unknown option \u0027\u0022 \u002B verb \u002B \u0022\u0027.\u0022);",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027 exists at verified commit \u0027eae0c713ff6e\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: using System.Text.Encodings.Web;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: using System.Text.Json;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: using System.Text.Json.Serialization;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: internal static class DataVaultSqlArtifactManifestExporter {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: public const string CurrentSchemaVersion = \u0022dvault.sql-artifact.v1\u0022;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: Status: \u0022review-only\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: LoadTimestamp: \u0022caller-supplied DataVaultSaveRequest load timestamp through the sqlserver-v1 native DateTimeOffset mapping\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: LatestStateBehavior: \u0022latest satellite hash-diff lookup skips the unchanged replay and advances state by load timestamp\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: string LoadTimestamp,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: RuntimeDispatch: \u0022not-generated\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: new DataVaultSqlArtifactEvidence(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: [property: JsonPropertyOrder(6)] DataVaultSqlArtifactEvidence Evidence,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: string RuntimeDispatch,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs\u0027: private sealed record DataVaultSqlArtifactEvidence(",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027 exists at verified commit \u0027eae0c713ff6e\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using System.Text.Json;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: public void RunPrintsHelpAndReturnsUsageErrorsDeterministically() {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(0, help.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Empty(help.Error);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(2, unknown.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022Unknown DVault command \u0027missing\u0027.\u0022, unknown.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022Usage: dvault validate\u0022, unknown.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(2, missingArtifact.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022Missing artifact path for drift command.\u0022, missingArtifact.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(0, valid.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Empty(valid.Error);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(1, invalid.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Empty(invalid.Error);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(0, success.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Empty(success.Error);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(1, failure.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022DVault export failed:\u0022, failure.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022Legacy PointInTimeTables metadata is not serializable\u0022, failure.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(0, first.ExitCode);",
    "Committed branch delta contains 3 inspectable repository path(s): Modified: src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs, Added: src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs, Modified: tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 223 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/architecture, area/diagnostics, area/ef-core, area/performance, area/provider-support, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F8KZV18BQ0GN3CE4G02ATVA0-story-add-dry-run-artifact-manifest-prototype-fo\u0027.",
    "Ticket history references implementation commit \u0027eae0c713ff6e\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using the verified branch ticket/06F8KZV18BQ0GN3CE4G02ATVA0-story-add-dry-run-artifact-manifest-prototype-fo and commit eae0c713ff6e.",
    "Keep the downstream benchmark-evidence ticket separate; this tester pass covers the bounded dry-run manifest prototype, not production-ready external-provider evidence."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F8KZV18BQ0GN3CE4G02ATVA0`
- target-role: `integrator`
- verification-summary: Tester verified 8/8 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F8KZV18BQ0GN3CE4G02ATVA0-story-add-dry-run-artifact-manifest-prototype-fo' at commit 'eae0c713ff6e'.
- acceptance-criteria: `8/8` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F8KZV18BQ0GN3CE4G02ATVA0-story-add-dry-run-artifact-manifest-prototype-fo`
- implementation-commit: `eae0c713ff6e`
- implementation-pr: `<none>`
- implementation-change: `<none>`