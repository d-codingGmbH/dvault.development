[gicket-bot] PO refinement contract

Summary
- Repository evidence confirms the SQL Server refinement should ratify the 50/500 native-bulk gate, provider-neutral fallback wording, and the review-only SQL artifact dry-run boundary; no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- For SQL Server save work, the repository already fixes the bounded starting gate: `SqlServerDataVaultSaveStrategy` is eligible only for clean SQL Server contexts with at least 50 total hub/link/satellite operations and no more than 500 satellite operations; otherwise the save falls back to the provider-neutral writer.
- The checked-in SQL Server threshold evidence is the historical authority for this ticket's gate wording: `artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/sqlserver-threshold-decision.md` keeps the 50/500 decision and corrects fallback-row wording to match diagnostics.
- The repository's staged-insert story for SQL Server is bounded to the existing review-only `dvault.sql-artifact.v1` dry-run lane: `DataVaultSqlArtifactManifestExporter` and `DataVaultDesignTimeCommandTests` show `SqlBulkCopy`, a temporary staging-table cleanup boundary, and no deployable SQL payload or runtime dispatch.
- SQL Server latest-satellite timing is still not completed evidence in the current baseline; only SQL Server `pit-as-of-read` and `bridge-traversal-read` can cite completed provider-configured timing from the v0.32.0 smoke-read bundle.
- This refinement did not materialize child tickets, relation edits, description edits, attachments, or planning documents.

Scope In
- Ticket-level refinement of the SQL Server `provider-native-bulk-ingestion` threshold contract and its finite fallback conditions.
- Clarification of which repository evidence surfaces are authoritative for SQL Server native-bulk and dry-run artifact claims.
- Clarification of the bounded SQL Server temporary-staging artifact lane so it is not confused with a deployable runtime feature.

Scope Out
- Changing runtime threshold code, save-strategy dispatch, or SQL Server provider behavior.
- Creating new benchmark runs or promoting root skipped-placeholder rows to completed-timing without a new provider-configured artifact triplet.
- Expanding SQL Server read optimization claims beyond the existing PIT/bridge completed evidence and latest-satellite guidance posture.
- Implementing deployable stored procedures, sidecar SQL files, or runtime SQL artifact dispatch.

Open questions
- none

Follow-up questions
- Should a later ticket add a fresh provider-configured SQL Server `provider-native-bulk-ingestion` artifact triplet so the current matrix can promote that row from skipped-placeholder guidance to completed-timing evidence?
- Should SQL Server latest-satellite timing remain a separate follow-up from this save-threshold story, consistent with `P0.02` in the provider optimization gap matrix?

Risks
- The repo uses both historical completed threshold bundles and current root skipped-placeholder rows; careless wording could overstate what is actually measured timing versus planning guidance.
- Calling the SQL Server path staged bulk without the current native-`SqlBulkCopy` and temporary-staging-table boundary would blur it with PostgreSQL/MySQL staged-provider lanes and misstate runtime behavior.
- The dry-run SQL artifact lane is easy to overread as a deployable implementation; the current repo only supports review-only manifest output for SQL Server and explicitly excludes runtime dispatch.

Split recommendations
- No immediate split is required; the current ticket is already bounded if it stays on SQL Server save-threshold and dry-run artifact evidence clarification.
- If more scope is needed later, split provider-configured SQL Server bulk timing promotion, SQL Server latest-satellite timing evidence, and any deployable SQL artifact/runtime dispatch proposal into separate tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment