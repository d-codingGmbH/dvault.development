[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB6P4ZNYA46MSYRGAJ9ZEPM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6P4ZNYA46MSYRGAJ9ZEPM`.
- Optimistic claim succeeded (`expectedRevision=06EXBF828KQD3B9ZEKYFHTQJ8C`, `currentRevision=06EXCRJKCGYA41X4P3ZM2K5FT4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB6P4ZNYA46MSYRGAJ9ZEPM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB6P4ZNYA46MSYRGAJ9ZEPM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB6P4ZNYA46MSYRGAJ9ZEPM-task-maintain-central-guidelines-attachment' from source 'fba0f00c06d2b5fc0a4d5a537fb62fcef8f80879'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB6P4ZNYA46MSYRGAJ9ZEPM-task-maintain-central-guidelines-attachment` as `8f935b2aa4d4`.

Open questions / Risiken
- Standards can drift if future tickets copy snippets instead of referencing the charter attachment.
- Because the guidelines are ticket-attached rather than in a normal docs tree, implementers must continue to retrieve the charter attachment during planning and development.
- Split recommendation: None; this remains a bounded governance attachment maintenance task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9019`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a7882befc6bf480191b1d7110108672e`
- completed-at-utc: `<redacted>-28T23:22:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6P4ZNYA46MSYRGAJ9ZEPM/runs/20260428T232255018Z-a7882befc6bf480191b1d7110108672e.json`