[gicket-bot] PO refinement contract

Summary
- Ratified this ticket as the save-only implementation child for provider optimization parity: keep the repository-backed PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save boundaries from the refreshed gap and evidence matrices, preserve provider-neutral fallback and diagnostics contracts, and leave read plus PIT maintenance work outside this ticket.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md as the authoritative planning and evidence surfaces for this ticket's save scope.
- Treat artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/ as the completed-timing baseline for the selected provider save rows; do not plan fresh benchmark reruns in this ticket.
- The live relation set is already coherent: this ticket is a child of story 06FH8R9DPSKTNYB46HHVJMZ9P8, it blocks documentation ticket 06FH8REKX113JRZQ42HEB1NVZ8, and its incoming block from done ticket 06FH8RATZGZRVAJVC4ERV0ACYW is historical upstream context rather than an active blocker.
- No bounded planning writes, child-ticket creation, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Provider save-path parity only for the rows already selected in the refreshed matrix: PostgreSQL, SQL Server, MySQL, Oracle, and DB2 provider-native bulk-ingestion behavior.
- Repository-backed threshold and lane decisions for retained, staged, direct, or deliberate provider-neutral fallback behavior.
- Diagnostics and fallback semantics that prove when a provider strategy is selected versus when provider-neutral save remains the public path.
- Tests and benchmark-facing execution-detail coverage that keep the selected save boundaries explicit.

Scope Out
- Latest-satellite, PIT, bridge, or PIT-maintenance implementation work; those belong to separate planning surfaces or tickets.
- Fresh benchmark execution, external provider provisioning, or artifact regeneration.
- Release-note, changelog, or performance-profile documentation updates beyond downstream ticket 06FH8REKX113JRZQ42HEB1NVZ8.
- Stored-procedure dispatch, deployment automation, new provider baselines, staged DB2 bulk, provider-native chunk execution, or Oracle staged bulk claims without new evidence.

Open questions
- none

Follow-up questions
- Should the team materialize a separate DB2 PIT full-rebuild maintenance child so the accepted maintenance lane is tracked beside the existing save and read children?
- After this save ticket and the read ticket land, should the remaining blocks chain be simplified so only active implementation dependencies remain on the parent story and downstream docs ticket?
- If a later parity pass reopens provider save work, should it be limited to new evidence-backed lanes such as staged DB2 bulk or Oracle staged bulk rather than reusing the closed P1 save rows?

Risks
- The current ticket description still reads like a fresh implementation discovery task; without this refinement, downstream work could rerun already closed save evidence or reopen settled thresholds.
- Because the repository already contains closed save evidence rows, implementers may overreach into read or PIT-maintenance work unless the save-only boundary stays explicit.
- Future work can accidentally widen DB2 or Oracle scope if staged bulk or provider-native chunk execution is treated as implied parity rather than as separate evidence-gated follow-up.

Split recommendations
- Do not split this ticket further by provider; the current repository evidence and shared save-gate surface keep PostgreSQL, SQL Server, MySQL, Oracle, and DB2 within one bounded save-parity task.
- Keep read-path work in sibling ticket 06FH8RDS25081N5S181C7TQGTG and documentation or evidence publication work in 06FH8REKX113JRZQ42HEB1NVZ8.
- If the team wants to pursue DB2 PIT full-rebuild maintenance, open one separate child limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) through IDataVaultProviderPitMaintenanceStrategy.
- Any future Oracle staged bulk, staged DB2 bulk, provider-native chunk execution, or maintenance-evidence expansion should be separate later tickets rather than enlarging this save task.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment