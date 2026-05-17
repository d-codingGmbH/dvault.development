[gicket-bot] Run report (outcome: po-refinement-failed)

Summary
- PO refinement for ticket '06F2PGKAQVVF8GEZVVC8SHFASG' failed because the model response was not parseable.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKAQVVF8GEZVVC8SHFASG`.
- Optimistic claim succeeded (`expectedRevision=06F3EPFKZ0NW19Y30495DDZF1C`, `currentRevision=06F3EPKE7062414ARXWH4KMSVC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites' from source '5117c5735340c0dbbdd98c202616c71d53462a97'.

Open questions / Risiken
- Model response JSON parsing failed: '0x1F' is an invalid start of a property name. Expected a '"'. LineNumber: 0 | BytePositionInLine: 1. Captured raw model response: C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260517T191836968Z-po-po-refinement-06F2PGKAQV...

Next steps
- Review ticket comments and bot logs.
- Retry PO refinement after resolving the reported issue.

Prompt cache usage
- prompt-tokens: `40938`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0594`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `04b576adaf59402fb7447b41cc187ece`
- completed-at-utc: `<redacted>-17T19:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKAQVVF8GEZVVC8SHFASG/runs/20260517T191840663Z-04b576adaf59402fb7447b41cc187ece.json`