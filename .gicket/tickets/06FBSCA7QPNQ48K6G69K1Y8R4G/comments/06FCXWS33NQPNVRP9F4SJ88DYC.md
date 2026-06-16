[gicket-bot] PO refinement contract

Summary
- Repository evidence shows no remaining repository delta for 06FBSCA7QPNQ48K6G69K1Y8R4G; this ticket should be treated as closure-only follow-up alignment over already-landed PostgreSQL implementation and evidence, not as a new developer task.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Set the lifecycle to closure-only. The accepted PostgreSQL bulk improvement is already present in the repository, and this ticket has no remaining developer repository delta.
- critic-item-2: `answered` - This is not a strict duplicate of 06F9XD33MNNVHHW232TC7T1CN8. That done ticket is the authoritative PostgreSQL provider-evidence anchor, while the current ticket now serves as closure-only follow-up alignment over already-landed scope.
- critic-item-3: `answered` - Do not keep this ticket open for dev. The authoritative output is a closure-only contract that cites the landed PostgreSQL strategy, tests, and benchmark bundle; no new non-.gicket deliverable is required here.
- critic-item-4: `answered` - Confirmed. The branch contains no non-.gicket delta, so the previous pre-development implementation framing was false and must be replaced with closure-only wording.
- critic-item-5: `answered` - Lineage is resolved as absorbed already-landed scope with closure-only follow-up alignment. Ticket 06F9XD33MNNVHHW232TC7T1CN8 remains the earlier done evidence anchor, so this ticket should not continue as a separate implementation task.

Clarifications
- Lifecycle decision: closure-only. No new developer repository work is required for this ticket.
- Lineage decision: not a strict duplicate of 06F9XD33MNNVHHW232TC7T1CN8; the earlier done ticket supplies the authoritative PostgreSQL provider-configured evidence bundle, and this ticket now acts as closure alignment for already-landed scope.
- The current implementation-style title and handoff wording should be treated as historical until a later trusted ticket-write pass rewrites them to closure-only wording.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

Scope In
- Closure-only reconciliation of the already-landed AddDVaultPostgres()/PostgresDataVaultSaveStrategy PostgreSQL save-path baseline.
- Citation of the existing repository proof surfaces for PostgreSQL code, tests, docs, and benchmark evidence.
- Lineage clarification from this ticket to done evidence ticket 06F9XD33MNNVHHW232TC7T1CN8 and its checked-in provider bundle.

Scope Out
- Any new PostgreSQL product-code change, test addition, or benchmark artifact creation on this ticket.
- A fresh benchmark rerun on this ticket; if needed, that belongs in a separate evidence follow-up.
- Latest-satellite, PIT, bridge, or non-PostgreSQL optimization expansion.
- Treating the root benchmark-summary PostgreSQL skipped rows as completed timing evidence.

Open questions
- none

Follow-up questions
- Should a later trusted gicket housekeeping pass persist a title/description rewrite and any needed lineage relation to 06F9XD33MNNVHHW232TC7T1CN8?
- If product wants fresh PostgreSQL timings after the v0.39.0 documentation baseline, should that be opened as a separate evidence ticket instead of reopening this closure-only ticket?

Risks
- gicket ticket/comment/relation reads were trust-blocked earlier in the session, so live relation metadata could not be revalidated or cleaned up in this unattended run.
- Until a later trusted ticket-write pass rewrites the ticket surface, the current implementation-style title may still mislead reviewers into expecting new developer work.
- Closure evidence must continue to cite the provider-configured v0.32 PostgreSQL bundle; the root benchmark triplet preserves PostgreSQL as skipped-placeholder when the connection string is unset.

Split recommendations
- No split for this ticket; treat the current ticket as closure-only.
- If desired, open a separate housekeeping ticket for lineage or relation cleanup or a separate benchmark-evidence ticket for any fresh PostgreSQL rerun.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment