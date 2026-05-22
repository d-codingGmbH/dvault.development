[gicket-bot] PO-critic review contract

Summary
- Detailed pre-development contract with no open questions; repo evidence confirms the cited reusable diagnostics/drift/guardrail surfaces and the branch is still ticket-metadata-only, so the story is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned 10 comments, all bot lease/claim/handover/refinement comments; there is no conflicting human discussion or attachment-driven requirement in the observed comment history.
- Repository search `rg -n 'class DataVaultDesignTimeCommand|class DataVaultDesignTimeCommandHost|interface IDataVaultDiagnosticsService|class DataVaultModelDriftPreflightReporter|class DataVaultMigrationOperationDiagnostics|class DataVaultDiagnosticsResult' /mnt/c/Projects/DVault/src /mnt/c/Projects/DVault/tests` matched `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs`, `DataVaultDesignTimeCommandHost.cs`, `DataVaultDiagnostics.cs`, `DataVaultModelDriftPreflightReporter.cs`, and `DataVaultMigrationOperationDiagnostics.cs`, confirming the contract references existing repo surfaces rather than speculative APIs.
- `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` already exposes the consumer-owned verbs `validate`, `export`, `support-bundle`, `drift`, and `guardrail`; `src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs` already carries consumer-owned `CreateDbContext`, `ResolveMigrationOperations`, optional `CreateSupportBundleDiagnostics`, and optional `LiveSchemaReader`, which aligns with the ticket's additive facade scope and consumer-owned wrapper boundary.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` defines `IDataVaultDiagnosticsService.Analyze(DbContext)` and `DataVaultDiagnosticsResult` with `ReadStrategy` and `ToDisplayString()`; `src/DCoding.Data.DVault/DataVaultModelDriftPreflightReporter.cs` already compares expected metadata/import plus runtime model plus explicit `IReadOnlyModel` snapshot; `src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs` already exposes `AnalyzeReport(...)` for migration guardrails.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs`, `DataVaultModelDriftPreflightReporterTests.cs`, and `DataVaultMigrationOperationDiagnosticsTests.cs` show repo-local tests already exercise validate/support-bundle/drift/guardrail behavior, snapshot preflight comparisons, and the DVM2001-DVM2006 migration taxonomy that this story is supposed to compose.
- `git -C /mnt/c/Projects/DVault diff --stat develop..ticket/06F492BG6BZYYFMBE5WK7CB024-story-add-consumer-owned-preflight-command-aggre` showed only `.gicket/tickets/06F492BG6BZYYFMBE5WK7CB024/...` description/comment/event/ticket metadata changes (27 files changed, 492 insertions, 5 deletions) and no `src/` or `tests/` changes on the ticket branch yet.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete example of one aggregate call that includes artifact drift, snapshot preflight, migration operations, and representative request diagnostics together would reduce interpretation drift around overall section ordering and block/skip rollup.
- The contract allows caller-owned request context as representative requests or precomputed diagnostics results, but it does not show a concrete example for both shapes or say what happens if both are supplied at once.
- The contract is clear that omitted lanes are skipped or not provided, but it does not include a compact example matrix covering omission of artifact input, snapshot input, migration operations, and request-bound diagnostics in the same story text.

Risky assumptions
- Developers will follow the authoritative contract rather than the title shorthand 'preflight command aggregator'; the repository already has command-host plumbing, and the ticket only authorizes a library-owned in-process facade plus thin consumer-owned wrapper, not a first-party standalone CLI.
- The future story 06F492B9PR036PDNN52S06S9BC can extend the same request-diagnostics envelope additively; if the new lane is shaped too narrowly around today's save/read strategy payloads, later query-shape/index-hint work may pressure the contract.

AC / test suggestions
- Add an explicit test case for overall blocking precedence when validation passes but drift or guardrail lanes block, and when blocking lanes are omitted so the result is passed-with-skips rather than failed.
- Add an explicit test or AC note for deterministic human-readable section order and for preserving original DMV/DVM/drift codes without renaming or reclassification.
- Add an explicit test or AC note for input authority when the expected model comes from `DataVaultMetadataModel` versus a successful `DataVaultModelImportResult`, or state that those input forms are mutually exclusive per call.

Implementation watchouts
- Keep composition over existing objects and semantics: `DataVaultDiagnosticsResult`, `DataVaultModelDriftReport`/preflight sections, and `DataVaultMigrationGuardrailReport` should be carried forward rather than copied into a parallel taxonomy.
- Keep snapshot materialization, artifact selection/import, migration operation resolution, and representative request selection consumer-owned; the existing command host and docs already enforce that boundary.
- Make pass/block/skip behavior deterministic for every optional lane because this facade is intended for CI assertions, startup gates, and thin command wrappers.

Non-blocking notes
- The repository already has an adjacent aggregated troubleshooting surface through `support-bundle` in `DataVaultDesignTimeCommand`; that makes the distinction between troubleshooting export and blocking preflight aggregation important, but the current contract states that distinction clearly enough to proceed.
- No assignees are currently set on the ticket, but that is a workflow detail, not a PO-refinement blocker for developer handoff.

Split recommendations
- No split recommended. The contract already keeps richer query-shape/index-hint diagnostics on 06F492B9PR036PDNN52S06S9BC and downstream documentation/adoption work on 06F492BNDPWS9P4EDSV0W7G6VM.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment