[gicket-bot] PO refinement contract

Summary
- Validated that this ticket should remain closure-only: the DB2 baseline already landed in the repository, and no additional DB2 evidence or documentation work was materialized here.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Kept the ticket closure-only. The authoritative contract already routes this ticket to no-work-required closure, and repository evidence shows the landed DB2 baseline is AddDVaultDb2 registration, optimized clean-context save, diagnostics-gated PIT/bridge reads, opt-in DB2 smoke coverage, and skipped-placeholder benchmark rows. Any later DB2 benchmark or documentation expansion should be opened as a separate narrow evidence-only ticket instead of reopening this implementation ticket.

Clarifications
- Keep this ticket on a no-work-required closure path; do not reopen it for new DB2 implementation, benchmark, or documentation scope.
- Repository-backed audit anchors are docs/releases/v0.34.0.md, src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs, tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs, and benchmark-summary.md.
- No child tickets, relation changes, description rewrites, attachments, or planning documents were materialized in this run.

Scope In
- Confirm that the already-landed DB2 baseline satisfies this ticket without new implementation work.
- Preserve an audit trail pointing to the v0.34.0 release note, AddDVaultDb2 registration, opt-in DB2 smoke coverage, and skipped-placeholder benchmark evidence.
- Keep the explicit DB2 boundary: clean-context save plus diagnostics-gated PIT/bridge reads only.

Scope Out
- Any new staged DB2 bulk implementation.
- Provider-native chunk execution or staged-bulk benchmarking claims for DB2.
- DB2 latest-satellite optimized read dispatch or widened DB2 timing claims.
- Re-scoping this ticket in place to new DB2 evidence or documentation work.

Open questions
- none

Follow-up questions
- If the team still wants provider-configured DB2 benchmark evidence or additional DB2 documentation beyond the current baseline, should a separate evidence-only ticket be opened with explicit artifact and documentation scope?

Risks
- A later reader could overstate the DB2 evidence if skipped-placeholder benchmark rows or opt-in smoke coverage are treated as completed DB2 timing claims.

Split recommendations
- Do not split or reopen this ticket; if more DB2 evidence is desired later, create one separate narrow evidence/documentation ticket.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment