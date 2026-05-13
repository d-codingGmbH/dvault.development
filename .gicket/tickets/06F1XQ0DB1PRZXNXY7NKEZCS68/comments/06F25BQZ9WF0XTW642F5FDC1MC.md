[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XQ0DB1PRZXNXY7NKEZCS68'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ0DB1PRZXNXY7NKEZCS68`.
- Optimistic claim succeeded (`expectedRevision=06F1XTQ81S0TCW6PFFCRWRS790`, `currentRevision=06F25B5X4YY4E3W33C3J8EDMJG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XQ0DB1PRZXNXY7NKEZCS68': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XQ0DB1PRZXNXY7NKEZCS68': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XQ0DB1PRZXNXY7NKEZCS68-task-define-bulk-strategy-contract-and-fallback' from source '727c4e4475ae236dac5fa105fb08308a25e56727'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- A contract that is too narrow could force provider packages to add parallel strategy APIs later; the context should carry enough ordered request and hashing information for known provider optimization paths.
- Diagnostics that are only log-text based may be brittle in tests; prefer a stable observable diagnostic surface already used by the project if one exists.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `24341`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0999`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `54b80f3d0c5f4a11bf52b68f497a32fb`
- completed-at-utc: `<redacted>-13T18:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ0DB1PRZXNXY7NKEZCS68/runs/20260513T185233821Z-54b80f3d0c5f4a11bf52b68f497a32fb.json`