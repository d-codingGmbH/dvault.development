[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB755X9TGQW2EG1G30GJG28'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB755X9TGQW2EG1G30GJG28`.
- Optimistic claim succeeded (`expectedRevision=06EXC0BPVN3X7R0JPKWJE0DM18`, `currentRevision=06EXD1NYK4HZ1NHDJ9JBND8XA0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB755X9TGQW2EG1G30GJG28': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB755X9TGQW2EG1G30GJG28': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB755X9TGQW2EG1G30GJG28-task-define-technical-metadata-column-contracts' from source 'bb6a7a5fe7ea5a67dc41b3cf0901cd94a19c7d54'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB755X9TGQW2EG1G30GJG28-task-define-technical-metadata-column-contracts` as `20a090d98d7d`.

Open questions / Risiken
- Development remains blocked until the foundation source/test scaffold exists or the workflow explicitly accepts a documentation-only artifact for this ticket.
- A later convention-policy ticket may introduce provider-specific casing rules, but it should adapt from these logical v1 defaults rather than block the current contract.
- If downstream vault-structure tickets assume different metadata roles or defaults, they will need to align with this shared baseline instead of creating parallel definitions.
- Split recommendation: No child split is needed for the four-role contract itself. Keep project/test scaffolding in the separate foundation setup work and relate this ticket to that dependency when the exact ticket id is available.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `35089`
- cached-tokens: `12160`
- effective-cache-ratio: `0.3465`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `3d22cc7ee26e4a6a9d343a533f53b04a`
- completed-at-utc: `<redacted>-29T00:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB755X9TGQW2EG1G30GJG28/runs/20260429T000024481Z-3d22cc7ee26e4a6a9d343a533f53b04a.json`