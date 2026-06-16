[gicket-bot] PO refinement contract

Summary
- Chose no-work-required routing for ticket 06FBSCAQGWFC9S98YCVDP4V7PC because the DB2 baseline already landed in the repository; the contract now points to the v0.34.0 release note, DB2 smoke coverage, and skipped-placeholder benchmark evidence. No bounded ticket or planning writes were materialized because relation/history tooling remained trust-blocked.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Chosen routing: close this ticket as no-work-required. The DB2 implementation baseline already landed, so this ticket should not stay open or be re-scoped in place.
- critic-item-2: `answered` - This refinement adds the required audit note in contract form by pointing to the landed v0.34.0 DB2 baseline, the DB2 smoke tests, and the root benchmark placeholder rows. No persisted relation write was materialized because gicket relation/history reads were BOT-LOCAL-TOOL-TRUST-BLOCKED in this run.
- critic-item-3: `answered` - The refined contract now treats the current open-work metadata as stale and routes the ticket to closure, so the ticket no longer implies unfinished staged DB2 bulk capability.
- critic-item-4: `answered` - There is no remaining developer implementation objective on this ticket. The valid next action is no-work-required closure; any later DB2 evidence request belongs in a new narrow evidence-only ticket.

Clarifications
- Routing choice: close this ticket as no-work-required; do not keep it open as an implementation ticket and do not re-scope it in place.
- Audit note for traceability: the landed DB2 baseline is the v0.34.0 release contract in docs/releases/v0.34.0.md, the opt-in save/read smoke coverage in tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs, and the root benchmark placeholder rows in benchmark-summary.md.
- The current branch already carries AddDVaultDb2() registration, Db2DataVaultSaveStrategy clean-context save support, and diagnostics-gated Db2DataVaultReadStrategy PIT/bridge dispatch; latest-satellite remains provider-neutral.
- No child tickets, relation writes, description edits, attachments, or planning documents were materialized in this run because repository evidence was sufficient and gicket relation/history surfaces were trust-blocked.

Scope In
- Close the current ticket as no-work-required against the already-landed DB2 baseline.
- Preserve an audit trail that points to the v0.34.0 release note, DB2 smoke tests, and root benchmark placeholder evidence.
- Keep the current DB2 boundary explicit: clean-context save plus diagnostics-gated PIT/bridge reads only.

Scope Out
- Any staged DB2 bulk implementation.
- DB2 latest-satellite optimized read dispatch.
- Provider-native chunk execution or staged bulk benchmarking claims for DB2.
- Completed DB2 timing claims without provider-configured benchmark artifacts.
- Re-scoping this ticket in place to new evidence work.

Open questions
- none

Follow-up questions
- If the team still wants provider-configured DB2 timing or documentation beyond the current audit note, should a new narrowly named evidence-only ticket be created rather than reopening this closed implementation ticket?
- When ticket relation/history tooling is allowed again, should the closure record add a relation or comment back to the originating spike or decision ticket for extra traceability?

Risks
- Until runtime closes or relabels the ticket, the historical open-work title can still imply unfinished DB2 bulk implementation.
- No live DB2 timing claim exists in the repository today because the checked-in benchmark rows are skipped placeholders when DVAULT_TEST_DB2_CONNECTION_STRING is unset.
- Relation/history surfaces were trust-blocked in this run, so the audit trail is contract-text-only rather than a persisted ticket relation.

Split recommendations
- Do not split this ticket; closure is the correct routing.
- If more DB2 evidence is desired later, create one separate evidence/documentation ticket scoped to provider-configured benchmark capture or documentation updates only.

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