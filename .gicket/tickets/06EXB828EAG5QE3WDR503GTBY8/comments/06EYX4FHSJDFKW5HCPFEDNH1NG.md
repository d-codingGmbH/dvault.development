[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB828EAG5QE3WDR503GTBY8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB828EAG5QE3WDR503GTBY8`.
- Optimistic claim succeeded (`expectedRevision=06EYX00BQY480JCM1PSDV5B6E8`, `currentRevision=06EYX30D877STR886H7J8FZB0C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB828EAG5QE3WDR503GTBY8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB828EAG5QE3WDR503GTBY8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB828EAG5QE3WDR503GTBY8-task-add-local-pack-command-and-package-verifica' from source '017ca84edc0ce88340293dba8737fb614d968a66'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-LIMIT-EXCEEDED.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Future additions or renames in the packable package matrix will require the verification baseline and tests to be updated in lockstep.
- Overly strict archive-structure assertions can create noisy failures; checks should target semantic package facts rather than incidental ZIP ordering, timestamps, or other non-semantic packaging details.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9165`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4f72c6e816d641af8f1e654c1f3657d4`
- completed-at-utc: `<redacted>-03T16:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB828EAG5QE3WDR503GTBY8/runs/20260503T160034625Z-4f72c6e816d641af8f1e654c1f3657d4.json`