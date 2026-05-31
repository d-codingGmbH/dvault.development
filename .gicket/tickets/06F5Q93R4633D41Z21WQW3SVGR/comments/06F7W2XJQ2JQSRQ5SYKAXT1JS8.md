[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q93R4633D41Z21WQW3SVGR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93R4633D41Z21WQW3SVGR`.
- Optimistic claim succeeded (`expectedRevision=06F7300X7E1MMH5RX0R0CEBER8`, `currentRevision=06F7W1ABF5Y7YRP5QCJZA20FAG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q93R4633D41Z21WQW3SVGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q93R4633D41Z21WQW3SVGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q93R4633D41Z21WQW3SVGR-epic-tracing-and-performance-guidance' from source '61d8d0c2f1a8fbeca05ed7bf1b18058cf05f83ba'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q93R4633D41Z21WQW3SVGR-epic-tracing-and-performance-guidance` as `3101b84518ea`.

Open questions / Risiken
- This epic still has a live incoming blocks relation from 06F5Q93H60W6X8FJ88PWTR6NG4, so closure can lag even though the epic scope itself is already refined.
- Two child-ticket reads hit the structured result bytes cap in this slice, so current refinement relies on repository evidence rather than a fresh persisted description snapshot for 06F5Q93YXHSKABD2SABWY85S78 and 06F5Q9463M0RSHAJJX0F3D1DB0.
- If any child ticket reopens with scope that exceeds the fixed tracing vocabulary or evidence-bound performance posture, the epic could drift beyond its current bounded v0.23.0 release story.
- Split recommendation: No additional split recommended; the epic is already decomposed into bounded child tickets for contract, save/read tracing, maintenance tracing, performance guidance, and coordinated docs.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `58458`
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
- run-id: `f27217a4c33440189d792800652b9c19`
- completed-at-utc: `<redacted>-31T12:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93R4633D41Z21WQW3SVGR/runs/20260531T123916399Z-f27217a4c33440189d792800652b9c19.json`