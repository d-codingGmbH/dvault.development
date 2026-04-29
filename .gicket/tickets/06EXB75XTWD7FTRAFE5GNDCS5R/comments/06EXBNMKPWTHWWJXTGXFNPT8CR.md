[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB75XTWD7FTRAFE5GNDCS5R'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB75XTWD7FTRAFE5GNDCS5R`.
- Optimistic claim succeeded (`expectedRevision=06EXBF7BV4XGA6KFFKMDC5NC7R`, `currentRevision=06EXBNBVH09P4MM477WQC3E85R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB75XTWD7FTRAFE5GNDCS5R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB75XTWD7FTRAFE5GNDCS5R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies' from source '0448b431d8191a3741b0feb06db741beaf3b048f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies` as `b688398078b0`.

Open questions / Risiken
- Public API shape may be hard to change later, so keep the initial naming policy contract minimal and focused on names this ticket actually needs to override.
- The referenced charter is not attached to this ticket at present; developers should use any shared project-standard context available in the repository or parent planning work.
- Split recommendation: No split recommended for this ticket; interface, options hook, default behavior, and tests form one cohesive implementation slice.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `25078`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0970`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5c9b12b02ee84e5598775481f50d067a`
- completed-at-utc: `<redacted>-28T20:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/runs/20260428T204502157Z-5c9b12b02ee84e5598775481f50d067a.json`