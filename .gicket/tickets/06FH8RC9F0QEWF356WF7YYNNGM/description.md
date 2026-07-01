<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Ratified this ticket as the save-only implementation child for provider optimization parity: keep the repository-backed PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save boundaries from the refreshed gap and evidence matrices, preserve provider-neutral fallback and diagnostics contracts, and leave read plus PIT maintenance work outside this ticket.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md as the authoritative planning and evidence surfaces for this ticket's save scope.
- Treat artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/ as the completed-timing baseline for the selected provider save rows; do not plan fresh benchmark reruns in this ticket.
- The live relation set is already coherent: this ticket is a child of story 06FH8R9DPSKTNYB46HHVJMZ9P8, it blocks documentation ticket 06FH8REKX113JRZQ42HEB1NVZ8, and its incoming block from done ticket 06FH8RATZGZRVAJVC4ERV0ACYW is historical upstream context rather than an active blocker.
- No bounded planning writes, child-ticket creation, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.

### Scope In
- Provider save-path parity only for the rows already selected in the refreshed matrix: PostgreSQL, SQL Server, MySQL, Oracle, and DB2 provider-native bulk-ingestion behavior.
- Repository-backed threshold and lane decisions for retained, staged, direct, or deliberate provider-neutral fallback behavior.
- Diagnostics and fallback semantics that prove when a provider strategy is selected versus when provider-neutral save remains the public path.
- Tests and benchmark-facing execution-detail coverage that keep the selected save boundaries explicit.

### Scope Out
- Latest-satellite, PIT, bridge, or PIT-maintenance implementation work; those belong to separate planning surfaces or tickets.
- Fresh benchmark execution, external provider provisioning, or artifact regeneration.
- Release-note, changelog, or performance-profile documentation updates beyond downstream ticket 06FH8REKX113JRZQ42HEB1NVZ8.
- Stored-procedure dispatch, deployment automation, new provider baselines, staged DB2 bulk, provider-native chunk execution, or Oracle staged bulk claims without new evidence.

## Acceptance Criteria
- PostgreSQL save scope is bounded to the repository-backed split: retained direct or UNNEST below 60 operations and staged COPY at 60-plus operations, with provider-neutral fallback preserved when strategy gates decline.
- SQL Server save scope keeps the existing SqlBulkCopy gate: clean context, at least 100 total operations, at least 900 total operations for mixed hub/link plus satellite batches, and no more than 500 satellite operations.
- MySQL save scope keeps the existing three-lane outcome: retained multi-row for smaller eligible batches, staged bulk for satellite-only 100-plus or mixed 100-to-303-operation batches, and deliberate provider-neutral fallback for large mixed batches above 303 operations or tiny satellite-history fallback cases.
- Oracle save scope keeps only the direct optimized batching lane for clean contexts at 50-plus operations with at most 10000 satellite operations; staged Oracle bulk remains out of scope until new evidence shows a measured win.
- DB2 save scope keeps clean-context set-based execution with the measured 1000-row command cap and explicitly excludes staged DB2 bulk, provider-native chunk execution, dirty-context save claims, and unsupported save shapes.
- Diagnostics and tests preserve selectedStrategy or provider-neutral fallback evidence for each save lane, using the existing benchmark and diagnostics vocabulary rather than inventing a new contract.

## Definition of Done
- The ticket contract clearly treats this task as the save-only child of the provider optimization parity story and does not reopen already split read work.
- All save-boundary decisions are ratified from current repository evidence, including PostgreSQL 60-operation staging, SQL Server 100/900/500 thresholds, MySQL retained/staged/fallback windows, Oracle direct-only boundary, and DB2 clean-context 1000-row-cap behavior.
- Provider-neutral fallback remains the explicit public behavior for provider mismatch, dirty contexts, unsupported shapes, and threshold-declined batches.
- Existing code and test surfaces remain aligned: DataVaultProviderSaveStrategyGateEvaluator, provider save strategies, and benchmark or diagnostics tests continue to describe the same bounded save behavior.
- No blocking PO clarification remains about this ticket's scope, baseline, or relation to the downstream docs ticket.

## Implementation Notes
- Repository code already fixes the visible save gates in src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs and the provider strategy implementations under src/DCoding.Data.DVault.Postgres, src/DCoding.Data.DVault.SqlServer, src/DCoding.Data.DVault.MySql, src/DCoding.Data.DVault.Oracle, and src/DCoding.Data.DVault.Db2.
- BenchmarkScenarioExecutionTests already encode the expected save execution-detail tokens for PostgreSQL retained and staged rows, SQL Server SqlBulkCopy thresholds, MySQL retained and staged rows plus deliberate provider-neutral fallback, Oracle direct-only behavior, and DB2 optimized save behavior.
- Use sqlserver-threshold-decision.md as the companion authority for the SQL Server 100/900/500 gate instead of reopening that threshold choice.
- Preserve the current diagnostics contract: selectedStrategy tokens, bounded fallback causes, and provider-neutral fallback wording are part of the acceptance boundary, not optional implementation details.
- Keep the MySQL large-mixed fallback marker and Oracle stagedOracleBulk=not-selected-no-measured-win behavior intact rather than converting them into vague or newly widened strategy claims.
- Do not pull PIT maintenance decisions into this task; MySQL maintenance is source and test backed only, Oracle remains deferred, and DB2 PIT full-rebuild work is a separate follow-up lane.
- No bounded planning writes were applied during this run.

## Open Questions
- none

## Follow-Up Questions
- Should the team materialize a separate DB2 PIT full-rebuild maintenance child so the accepted maintenance lane is tracked beside the existing save and read children?
- After this save ticket and the read ticket land, should the remaining blocks chain be simplified so only active implementation dependencies remain on the parent story and downstream docs ticket?
- If a later parity pass reopens provider save work, should it be limited to new evidence-backed lanes such as staged DB2 bulk or Oracle staged bulk rather than reusing the closed P1 save rows?

## Risks
- The current ticket description still reads like a fresh implementation discovery task; without this refinement, downstream work could rerun already closed save evidence or reopen settled thresholds.
- Because the repository already contains closed save evidence rows, implementers may overreach into read or PIT-maintenance work unless the save-only boundary stays explicit.
- Future work can accidentally widen DB2 or Oracle scope if staged bulk or provider-native chunk execution is treated as implied parity rather than as separate evidence-gated follow-up.

## Split Recommendations
- Do not split this ticket further by provider; the current repository evidence and shared save-gate surface keep PostgreSQL, SQL Server, MySQL, Oracle, and DB2 within one bounded save-parity task.
- Keep read-path work in sibling ticket 06FH8RDS25081N5S181C7TQGTG and documentation or evidence publication work in 06FH8REKX113JRZQ42HEB1NVZ8.
- If the team wants to pursue DB2 PIT full-rebuild maintenance, open one separate child limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) through IDataVaultProviderPitMaintenanceStrategy.
- Any future Oracle staged bulk, staged DB2 bulk, provider-native chunk execution, or maintenance-evidence expansion should be separate later tickets rather than enlarging this save task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement selected save-path improvements from the refreshed gap matrix. Scope includes provider-specific bulk/staged/direct strategy fixes, threshold tuning, diagnostics/fallback behavior, and tests. Do not add deployment automation, stored-procedure default dispatch, or provider behavior without evidence.