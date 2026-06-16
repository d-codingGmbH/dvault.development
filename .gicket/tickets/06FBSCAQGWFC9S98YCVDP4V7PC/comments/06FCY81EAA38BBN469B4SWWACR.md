[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCAQGWFC9S98YCVDP4V7PC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCAQGWFC9S98YCVDP4V7PC`.
- Optimistic claim succeeded (`expectedRevision=06FCY6THFY8MYA8Q77P7D0M74W`, `currentRevision=06FCY711R49XARH2H32T7EJ9TG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCAQGWFC9S98YCVDP4V7PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCAQGWFC9S98YCVDP4V7PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCAQGWFC9S98YCVDP4V7PC-task-implement-accepted-db2-bulk-improvement' from source 'ce08bcae99d093c9168022ab3878669e7ae15abf'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCAQGWFC9S98YCVDP4V7PC-task-implement-accepted-db2-bulk-improvement` as `d469d3cd071d`.

Open questions / Risiken
- A later reader could overstate the DB2 evidence if skipped-placeholder benchmark rows or opt-in smoke coverage are treated as completed DB2 timing claims.
- Split recommendation: Do not split or reopen this ticket; if more DB2 evidence is desired later, create one separate narrow evidence/documentation ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `47154`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1602`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `37a32984764642cfb45a41a2dd9449f9`
- completed-at-utc: `<redacted>-16T06:30:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/runs/20260616T063053124Z-37a32984764642cfb45a41a2dd9449f9.json`