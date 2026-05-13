[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPYW5PVKRTK4A91M6GHHF8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XPYW5PVKRTK4A91M6GHHF8`.
- Optimistic claim succeeded (`expectedRevision=06F1XTPV9967S8V3NCHPZQ6EHR`, `currentRevision=06F25RX8MK17ZAND8N54K9KCWR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPYW5PVKRTK4A91M6GHHF8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPYW5PVKRTK4A91M6GHHF8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPYW5PVKRTK4A91M6GHHF8-task-add-compiled-query-model-compatibility-test' from source '70bb43955d7a281ad5d7aa9f70f71413d61b0358'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- EF Core compiled model setup can become brittle if it relies on generated artifacts; keep this ticket focused on deterministic test fixtures and checked-in code only.
- Compiled query support can be overinterpreted as coverage for all LINQ/read shapes unless the supported shape and limitations are explicit.
- Provider-specific behavior may leak into the tests if the fixture setup is not kept close to the existing provider-neutral or SQLite baseline.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `25653`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0948`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `32fa6f78e4714b049fbb3d27d205b548`
- completed-at-utc: `<redacted>-13T19:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XPYW5PVKRTK4A91M6GHHF8/runs/20260513T195248714Z-32fa6f78e4714b049fbb3d27d205b548.json`