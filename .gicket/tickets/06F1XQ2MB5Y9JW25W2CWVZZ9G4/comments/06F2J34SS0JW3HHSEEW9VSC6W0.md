[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XQ2MB5Y9JW25W2CWVZZ9G4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ2MB5Y9JW25W2CWVZZ9G4`.
- Optimistic claim succeeded (`expectedRevision=06F1XTQG35Q5HYXX0Z8S630DJ4`, `currentRevision=06F2J1ZRC5QX6DDW97QGBT3NV4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XQ2MB5Y9JW25W2CWVZZ9G4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XQ2MB5Y9JW25W2CWVZZ9G4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XQ2MB5Y9JW25W2CWVZZ9G4-story-refresh-adoption-examples-and-production-c' from source 'f86b69332ef2edbd171f9f019685c3d289d86028'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Documentation can become misleading if it names packages or helper APIs not present in the current repository baseline.
- Provider-specific live drift guidance may overpromise support unless SQLite-first limits are kept explicit.
- A broad adoption document could grow into API reference duplication unless examples stay small and link to detailed source documents.
- Split recommendation: No split is required for this story. If implementation grows too large, keep this ticket focused on README/examples/checklist alignment and move future provider-specific deep dives, Testcontainers examples, or analyzer documentation into separate follow-u...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `25703`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0946`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `db77ccd9b3de4ef3821d2b5c581ef790`
- completed-at-utc: `<redacted>-15T00:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ2MB5Y9JW25W2CWVZZ9G4/runs/20260515T003232042Z-db77ccd9b3de4ef3821d2b5c581ef790.json`