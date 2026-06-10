[gicket-bot] PO refinement contract

Summary
- Re-triaged this ticket as already satisfied on develop: DB2 package-verification implementation is already present, so no verifier-code handoff remains; any residual publication-checklist alignment belongs with ticket 06F9G8HRZ72XP5Z7FNWM6MBMQC.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This ticket should be treated as closure-only / already satisfied, not handed to a developer for new verifier work. The only visible remaining work is documentation alignment outside this ticket's verifier scope.
- critic-item-2: `answered` - If any work remains, it is documentation-only and should be folded into ticket 06F9G8HRZ72XP5Z7FNWM6MBMQC. This ticket should not be retargeted back to verifier implementation; the residual mismatch is docs/manual-nuget-publication.md alignment against the DB2-inclusive README baseline.
- critic-item-3: `answered` - The ticket should no longer be positioned as a normal pre-development handoff. The develop..HEAD comparison shows no remaining verifier implementation delta, so sending this to development would be a no-op.
- critic-item-4: `answered` - The repository already contains the DB2 package-verification implementation on develop across the pack script, package verifier tool, verifier tests, and version-matrix tests, so this ticket should be closed as already satisfied instead of handed to a developer for more verifier work.

Clarifications
- This re-triage treats the current ticket as closure-only: DB2 package-verification implementation is already present on develop, so no developer-facing verifier delta remains.
- The only visible remaining mismatch is documentation alignment: README.md already includes DCoding.Data.DVault.Db2 for 8.34.0 and 10.34.0, while docs/manual-nuget-publication.md still describes a historical seven-package family and 8.33.0/10.33.0 lines.
- Residual documentation alignment should be absorbed by ticket 06F9G8HRZ72XP5Z7FNWM6MBMQC rather than reopened as verifier implementation work on this ticket.
- No bounded write was materialized in this run, so the persisted inbound `blocks` relation from 06F9G8HBXS7Y42J7XFSQKZ2AZ8 still exists and remains stale workflow state.
- No child tickets, planning documents, or attachments were materialized because the remaining scope is already bounded by the existing documentation follow-up ticket.

Scope In
- Confirm from repository and ticket evidence that DB2 package-verification coverage is already landed on develop.
- Reclassify this ticket from implementation work to closure-only PO triage.
- Route any remaining docs/manual-nuget-publication.md alignment to ticket 06F9G8HRZ72XP5Z7FNWM6MBMQC rather than reopening verifier implementation scope here.

Scope Out
- New package-verifier code, test, or pack-script changes for DB2.
- Runtime/provider behavior changes in src/DCoding.Data.DVault.Db2.
- Editing docs/manual-nuget-publication.md from this ticket instead of through the documentation follow-up stream.

Open questions
- none

Follow-up questions
- Should ticket 06F9G8HRZ72XP5Z7FNWM6MBMQC have its authoritative description expanded to name docs/manual-nuget-publication.md explicitly so the publication-checklist alignment owner is unambiguous?

Risks
- If this ticket is left framed as dev work, workflow will continue to point engineers at a no-op verifier implementation handoff.
- Until the documentation follow-up updates docs/manual-nuget-publication.md, repository guidance will remain split between the README's DB2-inclusive package family and the checklist's historical seven-package baseline.

Split recommendations
- Do not create a child implementation ticket from this item; the verifier work is already landed.
- Keep any remaining publication-checklist alignment with ticket 06F9G8HRZ72XP5Z7FNWM6MBMQC or a separate documentation-only follow-up, not with this closure-only ticket.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment