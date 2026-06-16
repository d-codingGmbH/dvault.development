[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - No additional DB2 benchmark or documentation work is reopened on 06FBSCAQGWFC9S98YCVDP4V7PC. The ticket remains closure-only; if stakeholders later want provider-configured DB2 benchmark artifacts or extra DB2 documentation, that work must go on one new narrow evidence-only follow-up ticket. No child ticket was materialized in this run because the current context does not show an active request for that extra evidence.
- critic-item-2: `answered` - Confirmed from the authoritative PO-critic ledger and the ticket snapshot that the same workflow-state mismatch remains visible in ticket metadata. That mismatch is workflow-only, already recorded, and runtime-managed after contract acceptance; it does not reopen scope, add new PO questions, or justify new ticket work on this closure-only DB2 ticket.

Clarifications
- This ticket stays on a no-work-required closure path because the landed DB2 baseline already exists in repository code, tests, release notes, and benchmark audit artifacts.
- No child tickets, relation changes, description rewrites, attachments, or planning documents were materialized in this run.
- Any later DB2 benchmark or documentation expansion must be handled by a separate narrow evidence-only ticket, not by reopening 06FBSCAQGWFC9S98YCVDP4V7PC.

Scope In
- Confirm that the checked-in DB2 baseline already satisfies this ticket without new implementation work.
- Preserve audit anchors to docs/releases/v0.34.0.md, src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs, tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs, and benchmark-summary.md.
- Keep the DB2 baseline bounded to optimized clean-context save plus diagnostics-gated PIT and bridge reads, while latest-satellite stays provider-neutral.

Scope Out
- Any new staged DB2 bulk implementation.
- Provider-native chunk execution or widened DB2 timing claims.
- DB2 latest-satellite optimized read dispatch or provider-specific PIT and bridge maintenance.
- Reopening this ticket for extra DB2 benchmark or documentation work.

Open questions
- none

Follow-up questions
- If stakeholders later want provider-configured DB2 benchmark artifacts or additional DB2 documentation beyond the current baseline, which single narrow evidence-only follow-up ticket should own that work?

Risks
- A later reader could overstate DB2 evidence if skipped-placeholder benchmark rows or opt-in smoke coverage are treated as completed DB2 timing claims.

Split recommendations
- Do not split or reopen this ticket; if more DB2 benchmark or documentation evidence is desired later, create one separate narrow evidence-only follow-up ticket.

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