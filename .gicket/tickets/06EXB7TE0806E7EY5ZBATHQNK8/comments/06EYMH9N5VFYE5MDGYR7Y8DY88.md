[gicket-bot] Run report (outcome: po-refinement-failed)

Summary
- PO refinement for ticket '06EXB7TE0806E7EY5ZBATHQNK8' failed because the model response was not parseable.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7TE0806E7EY5ZBATHQNK8`.
- Optimistic claim succeeded (`expectedRevision=06EYMD82JWFZ47TCWXMGDYFZVM`, `currentRevision=06EYMFV5CPNJD2ME93P727CS28`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7TE0806E7EY5ZBATHQNK8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7TE0806E7EY5ZBATHQNK8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7TE0806E7EY5ZBATHQNK8-task-add-benchmark-project-for-scenario-comparis' from source 'fb82b3a3f546240e1015af28581e570eeca35b7f'.

Open questions / Risiken
- Model response JSON parsing failed: '0x1F' is an invalid start of a property name. Expected a '"'. LineNumber: 0 | BytePositionInLine: 1. Captured raw model response: C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260502T195815210Z-po-po-refinement-06EXB7TE08...

Next steps
- Review ticket comments and bot logs.
- Retry PO refinement after resolving the reported issue.

Prompt cache usage
- prompt-tokens: `82772`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0294`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `467c331e1d07441bbfc5d348ece9b905`
- completed-at-utc: `<redacted>-02T19:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7TE0806E7EY5ZBATHQNK8/runs/20260502T195816733Z-467c331e1d07441bbfc5d348ece9b905.json`