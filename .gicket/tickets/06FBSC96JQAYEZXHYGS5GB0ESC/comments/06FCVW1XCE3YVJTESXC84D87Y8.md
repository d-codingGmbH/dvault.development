[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded SQL Server bulk-strategy evaluation ticket anchored to the visible v0.39 matrices, v0.32 SQL Server threshold evidence, and the current SQL Server save-strategy baseline.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already shows a SQL Server optimized save baseline: SqlServerDataVaultSaveStrategy uses temporary staging tables plus SqlBulkCopy, and the checked-in docs/artifacts describe that as the current SQL Server native bulk path.
- Current SQL Server save gating is already bounded and ratified in the visible baseline: provider-native dispatch starts at 50 total hub/link/satellite operations and accepts at most 500 satellite operations.
- The v0.39 root benchmark triplet keeps SQL Server rows as skipped placeholders when DVAULT_TEST_SQLSERVER_CONNECTION_STRING is unset, so new timing claims cannot come from the root triplet alone.
- Completed SQL Server threshold evidence already exists in the checked-in v0.32 bundles, including the SQL Server threshold decision that kept the 50/500 gates unchanged.
- No repository-visible TVP implementation or TVP benchmark evidence was found; the visible alternate SQL payload surface inside the SQL Server strategy is OPENJSON, not TVP.
- This ticket is an evaluation-and-recommendation task only: it decides whether the correct next step is implement, tune threshold, document no-op, or defer with reason.

Scope In
- Review the SQL Server provider-native-bulk-ingestion evidence-gap baseline using the v0.39 evidence matrix, the gap-matrix P1.02 row, the performance guidance, the v0.32 SQL Server threshold bundle, and the current SQL Server save-strategy code.
- Compare the visible SQL Server save-path story across staged temporary-table plus SqlBulkCopy execution, provider-neutral fallback, the visible OPENJSON payload surface, and the absence of a repository-visible TVP path.
- Decide whether the repository-backed recommendation is implement, tune threshold, document no-op, or defer with reason.
- Record the exact rationale, evidence posture, and stop or fallback conditions behind that recommendation.

Scope Out
- Implementing a new TVP path or any other new SQL Server save-path code in this ticket.
- Rerunning benchmarks, changing benchmark schemas, or altering the evidence-manifest contract.
- Changing non-SQL Server providers or opening latest-satellite, PIT, or bridge read work.
- Publishing new release claims or new SQL Server timing claims beyond the checked-in evidence baseline.

Open questions
- none

Follow-up questions
- If the recommendation is anything other than document no-op, should the next execution ticket benchmark the proposed change directly against the current staged SqlBulkCopy lane and provider-neutral fallback on provider-configured SQL Server runs?
- If TVP remains interesting after this evaluation, should it be treated as a separate bounded comparison against both the current SqlBulkCopy staged path and the visible OPENJSON payload surface rather than as the default next implementation step?

Risks
- The current repository baseline may justify document no-op or defer more strongly than implement because no TVP implementation or TVP benchmark evidence is checked in.
- The v0.39 root benchmark triplet cannot resolve the SQL Server evidence gap by itself because those rows are skipped when the SQL Server connection string is unset.
- Any threshold-tuning recommendation without new benchmark evidence would conflict with the already-ratified v0.<redacted> gate story.

Split recommendations
- If the evaluation finds both a threshold-retuning question and a separate TVP-versus-current-path design question, keep this ticket on the recommendation only and split any later execution into one evidence-backed implementation ticket rather than widening this ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment