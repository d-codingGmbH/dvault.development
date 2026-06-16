[gicket-bot] PO-critic review contract

Summary
- Ticket `06FBSC96JQAYEZXHYGS5GB0ESC` is sufficiently refined for developer handoff: the delivery contract is bounded, `## Open Questions` is `none`, repository-backed evidence sources are concrete, and contingent follow-up work is already captured.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSC96JQAYEZXHYGS5GB0ESC/description.md` contains the current refinement contract with `PO Handoff decision: ready_for_po_critic`, acceptance criteria that name `docs/plans/provider-optimization-evidence-matrix.md`, gap-matrix row `P1.02`, `docs/performance-profiles.md`, `docs/releases/v0.32.0.md`, the SQL Server threshold artifact, and the current SQL Server save-path code, and `## Open Questions` is `none`.
- `docs/plans/provider-optimization-gap-matrix.md` row `P1.02` explicitly classifies SQL Server `provider-native-bulk-ingestion` as an `Evidence gap` and says the root triplet is skipped when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset.
- `benchmark-summary.md` lines `66-67` and `benchmark-summary.csv` rows `33-34` for SQL Server `provider-native-bulk-ingestion` are `skipped` with `not configured: DVAULT_TEST_SQLSERVER_CONNECTION_STRING is not set or empty`, while `docs/performance-profiles.md` points developers to the v0.32 bundles for completed external-provider timing.
- `artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/sqlserver-threshold-decision.md` keeps the SQL Server gates unchanged at `50` minimum operations and `500` maximum satellite operations and records a provider-native lane at `100` satellite operations with provider-neutral fallback outside that boundary.
- `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs` hard-codes the SQL Server gate at `50` minimum operations and `500` maximum satellite operations; `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs` creates temporary staging tables, writes them with `SqlBulkCopy`, and also exposes `OPENJSON` builders.
- A repository search for `TVP`, `table-valued`, and `TableValued` under `src`, `docs`, `tests`, `benchmark-summary*`, and `artifacts/benchmarks` returned no repository-visible TVP implementation or TVP evidence; `.gicket/relations/SC/7C/06FBSC96JQAYEZXHYGS5GB0ESC--06FBSCA23YR3P9XRQA6MMYKV7C--blocks.json` plus `.gicket/tickets/06FBSCA23YR3P9XRQA6MMYKV7C/description.md` show follow-up work is already captured and conditional on this evaluation recommending implementation.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The evaluation still assumes the v0.32 SQL Server evidence is representative of the current save-path baseline; if the developer finds material post-v0.32 SQL Server save-path drift while evaluating, that drift should be called out before recommending threshold changes or new implementation.
- The existing follow-up ticket `06FBSCA23YR3P9XRQA6MMYKV7C` must remain contingent: if this evaluation lands on `document no-op` or `defer`, that ticket should close as no-work-required rather than forcing implementation.
- Any stronger TVP recommendation would need new repo-visible design or benchmark evidence; this ticket can only compare against the observed staged `SqlBulkCopy` lane, provider-neutral fallback, and visible `OPENJSON` surface.

AC / test suggestions
- In the final evaluation output, cite at least one below-threshold SQL Server fallback case and one in-threshold selected-strategy case from the checked-in SQL Server threshold bundle so the `50/500` boundary is evidenced both when it declines and when it selects.
- If the recommendation is anything other than `document no-op`, require the final evaluation to state the exact missing proof needed to change guidance: provider-configured benchmark triplet plus request-bound diagnostics against the current staged `SqlBulkCopy` lane and provider-neutral fallback.

Implementation watchouts
- Keep the ticket single-purpose: evaluate and recommend; do not widen it into TVP implementation, threshold retuning, or benchmark reruns.
- Do not restate the skipped root v0.39 SQL Server rows as completed timing evidence.
- Treat `OPENJSON` as the visible alternate SQL payload surface only; it is not evidence of a TVP path.

Non-blocking notes
- PO refinement comment `.gicket/tickets/06FBSC96JQAYEZXHYGS5GB0ESC/comments/06FCVW49SQY3D810HK768957F0.md` says the interactive PO loop hit `tool_call_limit_reached` and fell back to legacy planning, but the persisted contract and repository-backed facts were still locally verifiable.
- The ticket currently has no assignees; that does not block approval because runtime policy routes a successful PO-critic outcome to `dev`.
- The follow-up implementation ticket description is already properly conditional: `Implement the accepted SQL Server bulk improvement, if the spike recommends one... close with no-work-required if the spike rejects implementation.`

Split recommendations
- Keep this ticket on the evaluation output only; if implementation is recommended, let `06FBSCA23YR3P9XRQA6MMYKV7C` carry the execution work.
- If the evaluation separates a threshold-retuning question from a TVP-versus-current-path comparison, split those into distinct follow-up tickets instead of widening this one.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment