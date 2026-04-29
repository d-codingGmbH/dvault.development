[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB6NWYVB37D7S74VB3PVTCC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6NWYVB37D7S74VB3PVTCC`.
- Optimistic claim succeeded (`expectedRevision=06EXBF5DPJHTS4SET85VF6XP0M`, `currentRevision=06EXE5J0J3RDKKBNRT2W2XXPD4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB6NWYVB37D7S74VB3PVTCC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB6NWYVB37D7S74VB3PVTCC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB6NWYVB37D7S74VB3PVTCC-story-capture-shared-implementation-standards' from source 'bd62eb36121048945ea0ff0672d62487d13a8400'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Repository evidence shows transitional naming/layout signals between README reservations and the visible src/DVault project; this is manageable as follow-up cleanup but should not be silently duplicated in future standards.
- If downstream tickets copy standards instead of referencing the shared artifact, repository-wide governance may drift.
- CI enforcement is documented but not necessarily implemented yet, so future CI/build tickets must remember to wire bash tools/check-format.sh as a blocking step.
- Split recommendation: No additional split is recommended in this PO pass because two child tickets are already linked by parentOf relations from this story.
- Split recommendation: Keep future provider-specific conventions, CI wiring, and project-layout reconciliation as separate follow-up work rather than expanding this shared-standards story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `58409`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0416`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `fa4d1776c54e407094d7fa093d9b1245`
- completed-at-utc: `<redacted>-29T02:36:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6NWYVB37D7S74VB3PVTCC/runs/20260429T023617568Z-fa4d1776c54e407094d7fa093d9b1245.json`