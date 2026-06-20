<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence confirms the SQL Server refinement should ratify the 50/500 native-bulk gate, provider-neutral fallback wording, and the review-only SQL artifact dry-run boundary; no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- For SQL Server save work, the repository already fixes the bounded starting gate: `SqlServerDataVaultSaveStrategy` is eligible only for clean SQL Server contexts with at least 50 total hub/link/satellite operations and no more than 500 satellite operations; otherwise the save falls back to the provider-neutral writer.
- The checked-in SQL Server threshold evidence is the historical authority for this ticket's gate wording: `artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/sqlserver-threshold-decision.md` keeps the 50/500 decision and corrects fallback-row wording to match diagnostics.
- The repository's staged-insert story for SQL Server is bounded to the existing review-only `dvault.sql-artifact.v1` dry-run lane: `DataVaultSqlArtifactManifestExporter` and `DataVaultDesignTimeCommandTests` show `SqlBulkCopy`, a temporary staging-table cleanup boundary, and no deployable SQL payload or runtime dispatch.
- SQL Server latest-satellite timing is still not completed evidence in the current baseline; only SQL Server `pit-as-of-read` and `bridge-traversal-read` can cite completed provider-configured timing from the v0.32.0 smoke-read bundle.
- This refinement did not materialize child tickets, relation edits, description edits, attachments, or planning documents.

### Scope In
- Ticket-level refinement of the SQL Server `provider-native-bulk-ingestion` threshold contract and its finite fallback conditions.
- Clarification of which repository evidence surfaces are authoritative for SQL Server native-bulk and dry-run artifact claims.
- Clarification of the bounded SQL Server temporary-staging artifact lane so it is not confused with a deployable runtime feature.

### Scope Out
- Changing runtime threshold code, save-strategy dispatch, or SQL Server provider behavior.
- Creating new benchmark runs or promoting root skipped-placeholder rows to completed-timing without a new provider-configured artifact triplet.
- Expanding SQL Server read optimization claims beyond the existing PIT/bridge completed evidence and latest-satellite guidance posture.
- Implementing deployable stored procedures, sidecar SQL files, or runtime SQL artifact dispatch.

## Acceptance Criteria
- The refined ticket states that SQL Server provider-native bulk is a bounded starting gate, not a universal guarantee, and preserves the exact repo-backed thresholds: at least 50 total operations, at most 500 satellite operations, clean context, SQL Server provider match, and diagnostics-selected `SqlServerDataVaultSaveStrategy`.
- The refined ticket cites the authoritative evidence surfaces for this story: `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, `docs/performance-profiles.md`, `docs/releases/v0.32.0.md`, and the checked-in SQL Server threshold decision bundle.
- Fallback wording is aligned with repository diagnostics and tests: when the SQL Server candidate declines, the recorded path is provider-neutral fallback with `selectedStrategy=<none>` and the SQL Server candidate retained in diagnostics, not an executed SQL Server native/staged bulk lane.
- Any SQL Server staged-insert or artifact wording stays inside the current review-only `dvault.sql-artifact.v1` boundary: SQL Server only, `provider-native-bulk-ingestion` workload, `SqlBulkCopy` transfer, temporary staging-table cleanup, no deployable payloads, and no runtime dispatch.
- The refined ticket keeps SQL Server latest-satellite timing out of completed-timing claims and does not reopen PIT/bridge rows that already have completed provider-configured v0.32.0 smoke-read evidence.

## Definition of Done
- The PO handoff text can be implemented without reopening threshold numbers, fallback wording, or artifact-lane boundaries.
- Measured evidence versus skipped-placeholder guidance is separated clearly enough that downstream docs or code work cannot accidentally promote the wrong SQL Server row.
- The ticket explicitly distinguishes SQL Server native bulk from PostgreSQL/MySQL staged-provider lanes and from any future deployable SQL artifact story.
- No blocking PO questions remain for this ticket's bounded refinement scope.

## Implementation Notes
- Use `DataVaultProviderSaveStrategyGateEvaluator`, `DataVaultSaveTelemetryExplanation`, `DataVaultDiagnosticsTests`, and `BenchmarkScenarioExecutionTests` as the source-of-truth surfaces for gate values and fallback vocabulary.
- Use `sqlserver-threshold-decision.md` and the v0.32.0 release/performance docs as the evidence narrative for why the 50/500 gate stays in place.
- Preserve the current nuance that the root `benchmark-summary.*` SQL Server save rows are skipped placeholders when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset; do not treat those root quick rows as completed timing for new claims.
- When the ticket references SQL artifact evidence, keep the current dry-run manifest prerequisites: SQL Server diagnostics, selected `SqlServerDataVaultSaveStrategy`, `SqlBulkCopy` transfer, temporary staging-table cleanup, semantic-parity review, and consumer-owned deployment, rollback, transaction, and operational concerns.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add a fresh provider-configured SQL Server `provider-native-bulk-ingestion` artifact triplet so the current matrix can promote that row from skipped-placeholder guidance to completed-timing evidence?
- Should SQL Server latest-satellite timing remain a separate follow-up from this save-threshold story, consistent with `P0.02` in the provider optimization gap matrix?

## Risks
- The repo uses both historical completed threshold bundles and current root skipped-placeholder rows; careless wording could overstate what is actually measured timing versus planning guidance.
- Calling the SQL Server path staged bulk without the current native-`SqlBulkCopy` and temporary-staging-table boundary would blur it with PostgreSQL/MySQL staged-provider lanes and misstate runtime behavior.
- The dry-run SQL artifact lane is easy to overread as a deployable implementation; the current repo only supports review-only manifest output for SQL Server and explicitly excludes runtime dispatch.

## Split Recommendations
- No immediate split is required; the current ticket is already bounded if it stays on SQL Server save-threshold and dry-run artifact evidence clarification.
- If more scope is needed later, split provider-configured SQL Server bulk timing promotion, SQL Server latest-satellite timing evidence, and any deployable SQL artifact/runtime dispatch proposal into separate tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: revisit SQL Server bulk threshold and staged insert decisions where previous time gains were modest. Acceptance: thresholds are evidence-backed and fallback remains predictable.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- Added `sqlserver-threshold-decision.md` at the repository root as the explicit validation artifact for ticket `06FE4QRC7D55RS8ZZ37ZAEJ98M`.
- Updated provider evidence and gap docs so SQL Server `provider-native-bulk-ingestion` / `dvault-adddvaultsqlserver-optimized` is closed only through the configured 2026-06-20 SQL Server bulk-threshold benchmark bundle.
- Kept the SQL Server gate unchanged: at least 50 total hub/link/satellite operations, no more than 500 satellite operations, clean SQL Server context, and diagnostics selecting `SqlServerDataVaultSaveStrategy`.
- Preserved provider-neutral fallback wording for declined or unregistered SQL Server candidates: `selectedStrategy=<none>` with bounded fallback causes.
- Kept `dvault.sql-artifact.v1` inside the existing review-only manifest boundary: SQL Server only, `provider-native-bulk-ingestion`, `SqlBulkCopy`, temporary staging-table cleanup, no deployable payload, and no runtime dispatch.
- Kept `P0.02` latest-satellite as a separate unpromoted evidence gap even though the bulk-threshold triplet contains an incidental SQL Server latest-satellite row.

## Verification Notes

- `bash tools/check-format.sh` passed.
- `git diff --check -- sqlserver-threshold-decision.md docs/plans/provider-optimization-evidence-matrix.md docs/plans/provider-optimization-gap-matrix.md docs/performance-profiles.md docs/releases/v0.32.0.md` passed.
- `dotnet build DVault.slnx --nologo` was started and restored/compiled multiple projects, but was stopped before a final pass/fail summary after an extended silent build phase. It emitted NU1900 warnings from a read-only NuGet vulnerability-cache path.
- `dotnet test DVault.slnx --nologo` was not run because the build command did not reach a final status in the bounded validation window.

<!-- gicket-bot:developer-delivery:v1:end -->