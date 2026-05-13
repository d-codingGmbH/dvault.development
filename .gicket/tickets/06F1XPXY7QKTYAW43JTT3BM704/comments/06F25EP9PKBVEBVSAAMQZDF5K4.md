[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPXY7QKTYAW43JTT3BM704'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XPXY7QKTYAW43JTT3BM704`.
- Optimistic claim succeeded (`expectedRevision=06F25DQJ47KR6N9Z96PTHS55TW`, `currentRevision=06F25DTBVRPGG7GPY05P0KP8CR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPXY7QKTYAW43JTT3BM704': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPXY7QKTYAW43JTT3BM704': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPXY7QKTYAW43JTT3BM704-task-implement-first-read-helper-api-slice-and-t' from source '83b4c0de9865c00c038c971c9304a3a1da523dbc'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Split recommendation: Retired for this ticket: do not split PIT-backed typed read helpers from this duplicate retirement ticket; raise independent future work only if a new PIT gap is identified.
- Split recommendation: Retired for this ticket: do not split bridge traversal typed helpers from this duplicate retirement ticket; raise independent future work only if a new bridge gap is identified.
- Split recommendation: Retired for this ticket: do not split reflection-based DTO binding from this duplicate retirement ticket; keep it out of scope unless a separate product decision creates a new API family.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `36995`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0657`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `244c01e541e34a30a857db9e90b1fd52`
- completed-at-utc: `<redacted>-13T19:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XPXY7QKTYAW43JTT3BM704/runs/20260513T190526221Z-244c01e541e34a30a857db9e90b1fd52.json`