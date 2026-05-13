[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPXJW79K94G4WG86AG2X6M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XPXJW79K94G4WG86AG2X6M`.
- Optimistic claim succeeded (`expectedRevision=06F25KYADX1MRVR6311Z26N7PC`, `currentRevision=06F25NKE9C1J7375M94Z6BJHV8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPXJW79K94G4WG86AG2X6M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPXJW79K94G4WG86AG2X6M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPXJW79K94G4WG86AG2X6M-story-add-linq-friendly-current-as-of-bridge-rea' from source 'e1e6e848e84b3d1f311d3bbeda698fd88ae5edf6'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If this ticket is accidentally routed to development as implementation work, it may duplicate existing APIs/docs/tests and create unnecessary public API churn.
- Adding a current-named alias without a separate naming/API contract could fragment the documented latest-satellite vocabulary.
- Split recommendation: Do not split this ticket further. Close or retire it as already satisfied; create a separate narrow follow-up only if a future current-named public alias is explicitly required.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `33673`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0722`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `26bad765455f44988d07cf00d7634b50`
- completed-at-utc: `<redacted>-13T19:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XPXJW79K94G4WG86AG2X6M/runs/20260513T194054164Z-26bad765455f44988d07cf00d7634b50.json`