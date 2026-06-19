[gicket-bot] PO refinement contract

Summary
- Refined the ticket as a bounded Oracle PIT/bridge external-provider evidence task; repository evidence confirms the Oracle strategy candidates already exist, the current benchmark rows are still skipped placeholders, and no child-ticket, relation, attachment, or planning-document write was materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already fixes the Oracle v1 baseline: AddDVaultOracle() registers OracleDataVaultReadStrategy for PIT and bridge reads, not for latest-satellite reads.
- The current benchmark contract already carries Oracle guidance rows for pit-as-of-read and bridge-traversal-read under the dvault-adddvaultoracle-optimized / oracle-optimized-dvault baseline, but the checked-in root benchmark triplet still records Oracle as skipped when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset.
- Canonical closure evidence must follow docs/plans/provider-optimization-evidence-matrix.md; skipped-placeholder, diagnostics-only, or smoke-only Oracle rows do not satisfy completed timing evidence for this ticket.
- Oracle PIT/bridge fallback boundaries are already repository-backed and stay in scope: provider mismatch, unsupported shape, incomplete read-shape evidence, and stale read-model maintenance must continue to fall back to provider-neutral reads.
- No planning document, attachment, description update, child ticket, or relation write was applied during refinement; the live blocks relations remain unchanged.

Scope In
- Checked-in Oracle external-provider evidence for the pit-as-of-read and bridge-traversal-read scenarios using the existing dvault-adddvaultoracle-optimized baseline and OracleDataVaultReadStrategy candidate.
- Benchmark and verifier updates that promote the Oracle PIT and bridge rows from skipped-placeholder evidence to completed timing evidence when configured artifacts are present.
- Documentation and evidence-matrix updates needed to cite the Oracle artifact source and claim boundary accurately.
- Regression coverage that preserves explicit maintenance requirements and incomplete or stale evidence fallback behavior for Oracle PIT and bridge reads.

Scope Out
- Adding an Oracle latest-satellite provider strategy or changing the current provider-neutral latest-satellite posture.
- New public API surface, new read-shape design, or alternative Oracle PIT/bridge strategy invention.
- Automatic PIT or bridge maintenance, scheduler work, SaveChanges-triggered refresh, or other maintenance orchestration.
- Cross-provider evidence closure for PostgreSQL, SQL Server, MySQL, or DB2 beyond the Oracle PIT and bridge rows.

Open questions
- none

Follow-up questions
- After this Oracle PIT and bridge evidence gap closes, should Oracle latest-satellite optimization remain an explicit separate backlog capability-gap item alongside the other non-SQLite providers?
- Once delivery lands, should the live blocks chain 06FBSCGBG8CJ0QNRX4JZJA638G -> 06FBSCH0M358R5J3RGFB6GRDM4 -> 06FBSCHBJEYYERDPA7JN34Y8PG be revalidated for housekeeping?

Risks
- Completed Oracle timing evidence still depends on a reachable configured Oracle test environment; until that run is checked in, the repository only preserves skipped placeholder rows for Oracle.
- If matrix or guidance docs are updated without matching artifact-backed verifier coverage, the repository could overstate Oracle timing claims relative to the evidence contract.
- Delivery sequencing may still depend on the existing live blocks relations even though PO clarification is complete.

Split recommendations
- No additional split is recommended; the repository already bounds this ticket to two Oracle read scenarios plus the required evidence, docs, and verifier updates.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment