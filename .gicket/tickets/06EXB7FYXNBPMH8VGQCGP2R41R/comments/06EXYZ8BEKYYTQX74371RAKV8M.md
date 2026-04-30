[gicket-bot] Run report (outcome: po-refinement-failed)

Summary
- PO refinement for ticket '06EXB7FYXNBPMH8VGQCGP2R41R' failed because the model response was not parseable.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7FYXNBPMH8VGQCGP2R41R`.
- Optimistic claim succeeded (`expectedRevision=06EXYYW2SM8ATPQPKJD4F8GP94`, `currentRevision=06EXYYYEFZCWH5WP81M6HDVTRC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7FYXNBPMH8VGQCGP2R41R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7FYXNBPMH8VGQCGP2R41R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7FYXNBPMH8VGQCGP2R41R-task-translate-dvault-metadata-into-ef-model-met' from source 'f227420797163198dd6fcd9f6a103ebdf8354be9'.

Open questions / Risiken
- Model response must provide non-empty 'summary'. Captured raw model response: C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260430T174325473Z-po-po-refinement-06EXB7FYXNBPMH8VGQCGP2R41R.json.

Next steps
- Review ticket comments and bot logs.
- Retry PO refinement after resolving the reported issue.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `68864`
- effective-cache-ratio: `0.5145`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b4a2b0f1392540eeabdbe794f80ccffd`
- completed-at-utc: `<redacted>-30T17:43:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7FYXNBPMH8VGQCGP2R41R/runs/20260430T174326550Z-b4a2b0f1392540eeabdbe794f80ccffd.json`