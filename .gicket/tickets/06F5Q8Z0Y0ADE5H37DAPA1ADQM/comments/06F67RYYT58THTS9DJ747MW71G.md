[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno\u0027 at commit \u0027fee5b0d7b7c8\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno",
    "commitSha": "fee5b0d7b7c8",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Representative save diagnostics can distinguish ordinary provider-neutral fallback from staged-provider fallback or decline using finite machine-readable cause kinds with deterministic human-readable explanation and remediation text.",
      "satisfied": true,
      "reason": "Persisted developer-delivery evidence states the implementation added finite staged-provider lifecycle and provider-caveat vocabulary and extended fallback cause kinds with deterministic explanation and remediation text, and verification confirmed the staged diagnostics additions and explanation catalog at commit fee5b0d7b7c8."
    },
    {
      "expectation": "For staged-provider evaluation, surfaced diagnostics preserve candidate ordering, selected-strategy identity when applicable, request count, total operation count, hub, link, and satellite operation counts, and relevant staged lifecycle or provider-caveat classification while staying redacted and bounded.",
      "satisfied": true,
      "reason": "Persisted developer-delivery evidence says staged diagnostics now surface candidate ordering, selected-strategy identity, staged lifecycle or caveat classification, and request, total, hub, link, and satellite operation counts with bounded redaction, and verification confirmed the bounded staged diagnostics type, telemetry summary surface, and telemetry observer instrumentation on the verified commit."
    },
    {
      "expectation": "Additive diagnostics align with the settled staging contract from 06F5Q8YKR31DXGRXVPJ9031BQW by reporting dirty-context, unsupported-shape, transaction-participation, cleanup, or provider-limitation outcomes without introducing a new save contract.",
      "satisfied": true,
      "reason": "The delivery evidence explicitly frames the work as additive extensions on existing save diagnostics and telemetry surfaces, lists dirty-context, unsupported-shape, transaction-participation, cleanup, and provider-limitation staged outcomes, and states the optional provider diagnostics interface was added without introducing a new save entrypoint."
    },
    {
      "expectation": "When representative request-bound save diagnostics are supplied, support-bundle or equivalent explain output reuses the same staged fallback vocabulary and does not emit raw SQL, credentials, hash keys, payload values, or stage-row contents.",
      "satisfied": true,
      "reason": "Persisted developer-delivery evidence states support-bundle JSON and explain output reuse the staged fallback vocabulary and preserve redaction boundaries, and the recorded automated coverage includes support-bundle serialization and redaction behavior."
    },
    {
      "expectation": "Automated tests cover new staged fallback cause kinds, explanation and remediation text, candidate and selected-strategy reporting, operation-count reporting, and redaction behavior.",
      "satisfied": true,
      "reason": "Verification recorded a successful \u0060dotnet test DVault.slnx --nologo\u0060, and persisted developer-delivery evidence ties the added unit and integration coverage to staged fallback causes, explanation and remediation text, candidate ordering, selected-strategy preservation, operation-count reporting, support-bundle serialization, and redaction boundaries."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The ticket has one authoritative refinement contract that treats this as additive save explainability work over existing diagnostics, telemetry, and support-bundle surfaces.",
      "satisfied": true,
      "reason": "The ticket description contains one persisted delivery contract block, and that contract explicitly treats the work as additive save explainability over existing diagnostics, telemetry, and support-bundle surfaces."
    },
    {
      "expectation": "Downstream provider, benchmark, and documentation tickets can rely on one finite staged fallback vocabulary and one redaction policy without reopening save-boundary or transaction-contract questions.",
      "satisfied": true,
      "reason": "Verified repository evidence shows one finite staged-provider lifecycle and provider-caveat vocabulary plus bounded staged diagnostics surfaces, and persisted delivery evidence states the same vocabulary is reused across diagnostics, telemetry, and support-bundle output."
    },
    {
      "expectation": "Any public additions remain additive extensions to existing diagnostics types rather than a new persistence API or staging-management contract.",
      "satisfied": true,
      "reason": "Repository and delivery evidence show additive extensions to existing diagnostics types, including the staged diagnostics types and optional provider diagnostics interface, while explicitly stating no new persistence API or staging-management contract was introduced."
    },
    {
      "expectation": "Tests prove the new staged fallback reporting paths touched by the implementation.",
      "satisfied": true,
      "reason": "The tester verification run passed \u0060dotnet test DVault.slnx --nologo\u0060, and the persisted delivery evidence maps that passing coverage to the new staged fallback reporting paths touched by the implementation."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027fee5b0d7b7c8\u0027 on branch \u0027ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno\u0027.",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using System.Text;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore.Infrastructure;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore.Metadata;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: DataVaultProviderValueFormat LoadTimestampValueFormat,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: string LoadTimestampStoreType,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: /// Gets the value format used when PIT rows persist satellite snapshot load-timestamp references.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: /// Gets the provider store type used when PIT rows persist satellite snapshot load-timestamp references.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: Error,",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: using System.Diagnostics.Metrics;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: /// Records explicit DVault save and read telemetry through \u003Csee cref=\u0022Meter\u0022 /\u003E counters and histograms.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: public sealed class DataVaultMeterTelemetryObserver : IDataVaultTelemetryObserver, IDisposable {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Counts explicit DVault save attempts.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Counts rows written by successful explicit DVault save attempts.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Counts saved-record summaries returned by successful explicit DVault save attempts.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Counts distinct provider-neutral save fallback-cause kinds.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Records explicit DVault save attempt durations in milliseconds.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Records explicit save request counts per DVault save attempt.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Records hub, link, and satellite operation counts per DVault save attempt.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Records observed chunk counts per DVault chunked save attempt.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Records processed non-empty chunk counts per DVault chunked save attempt.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Records retained satellite continuity-state count when DVault save telemetry is emitted.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Records retained satellite continuity-state high-water counts per DVault save attempt.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Counts distinct chunked-save retained-state fallback-cause kinds.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Counts distinct chunked-save unsupported or memory-sensitive shape kinds.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Counts explicit DVault read attempts.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Counts rows returned by successful explicit DVault read attempts.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Counts distinct provider-neutral read fallback-cause kinds.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Records explicit DVault read attempt durations in milliseconds.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs\u0027: description: \u0022Records requested parent or endpoint hash-key counts per DVault read attempt.\u0022);",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: using System.Collections.ObjectModel;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: using System.Diagnostics;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: using Microsoft.EntityFrameworkCore.Metadata;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: request.LoadTimestamp,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: /// Groups registry-backed DVault save operations that share one load timestamp and record source.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: /// \u003Cparam name=\u0022loadTimestamp\u0022\u003EThe caller-visible load timestamp to persist as UTC metadata.\u003C/param\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: DateTimeOffset loadTimestamp,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: : this(loadTimestamp, recordSource, hubOperations, linkOperations, []) {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: LoadTimestamp = loadTimestamp.ToUniversalTime();",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: /// Gets the caller-supplied load timestamp normalized to a UTC instant.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveService.cs\u0027: public DateTimeOffset LoadTimestamp { get; }",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs\u0027: internal static class DataVaultSaveTelemetryExplanationCatalog {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs\u0027: private static readonly DataVaultChunkedSaveTransactionExplanation ChunkedTransaction =",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs\u0027: new(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs\u0027: \u0022Chunked execution participates in the caller-owned DbContext current transaction and does not create, commit, roll back, or suppress transactions for the caller.\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs\u0027: \u0022For all-or-nothing behavior across chunks, open the transaction before invoking the save service and roll it back if the save fails or is canceled.\u0022);",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs\u0027: /// Bounded summary emitted for one explicit DVault save attempt.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs\u0027: public sealed class DataVaultSaveTelemetrySummary {",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnostics.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnostics.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnostics.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnostics.cs\u0027: /// Bounded diagnostics for one staged-provider bulk save evaluation.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnostics.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnostics.cs\u0027: public sealed class DataVaultStagedProviderBulkDiagnostics {",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnosticsSupport.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnosticsSupport.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnosticsSupport.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnosticsSupport.cs\u0027: internal static class DataVaultStagedProviderBulkDiagnosticsSupport {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnosticsSupport.cs\u0027: public static DataVaultStagedProviderBulkDiagnostics? TryEvaluate(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnosticsSupport.cs\u0027: IDataVaultProviderSaveStrategy strategy,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnosticsSupport.cs\u0027: DbContext dbContext,",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkLifecyclePhase.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkLifecyclePhase.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkLifecyclePhase.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkLifecyclePhase.cs\u0027: /// Identifies the bounded staged-provider bulk save lifecycle phase reported by diagnostics.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkLifecyclePhase.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkLifecyclePhase.cs\u0027: public enum DataVaultStagedProviderBulkLifecyclePhase {",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkProviderCaveatKind.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkProviderCaveatKind.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkProviderCaveatKind.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkProviderCaveatKind.cs\u0027: /// Classifies the provider caveat associated with staged-provider bulk save diagnostics.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkProviderCaveatKind.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultStagedProviderBulkProviderCaveatKind.cs\u0027: public enum DataVaultStagedProviderBulkProviderCaveatKind {",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs\u0027: using System.Diagnostics;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs\u0027: internal sealed record DataVaultSaveTelemetryStrategySelection(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs\u0027: IDataVaultProviderSaveStrategy? Strategy,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs\u0027: DataVaultSaveStrategyDiagnosticsStatus Status,",
    "Committed repository path \u0027src/DCoding.Data.DVault/IDataVaultProviderStagedBulkSaveDiagnostics.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/IDataVaultProviderStagedBulkSaveDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/IDataVaultProviderStagedBulkSaveDiagnostics.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/IDataVaultProviderStagedBulkSaveDiagnostics.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/IDataVaultProviderStagedBulkSaveDiagnostics.cs\u0027: /// Optional diagnostics extension for provider save strategies that evaluate staged-provider bulk execution.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/IDataVaultProviderStagedBulkSaveDiagnostics.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/IDataVaultProviderStagedBulkSaveDiagnostics.cs\u0027: public interface IDataVaultProviderStagedBulkSaveDiagnostics {",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027 exists at verified commit \u0027fee5b0d7b7c8\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs\u0027: Assert.Equal(DataVaultDiagnosticsIssueSeverity.Error, issue.Severity);",
    "Committed branch delta contains 14 inspectable repository path(s): Modified: src/DCoding.Data.DVault/DataVaultDiagnostics.cs, Modified: src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs, Modified: src/DCoding.Data.DVault/DataVaultSaveService.cs, Modified: src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs, Modified: src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs, Added: src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnostics.cs, Added: src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnosticsSupport.cs, Added: src/DCoding.Data.DVault/DataVaultStagedProviderBulkLifecyclePhase.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 202 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/developer-experience, area/diagnostics, area/ef-core, area/provider-support, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno\u0027.",
    "Ticket history references implementation commit \u0027fee5b0d7b7c8\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to the integrator gate using branch \u0060ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno\u0060 at commit \u0060fee5b0d7b7c8\u0060."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F5Q8Z0Y0ADE5H37DAPA1ADQM`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno' at commit 'fee5b0d7b7c8'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno`
- implementation-commit: `fee5b0d7b7c8`
- implementation-pr: `<none>`
- implementation-change: `<none>`