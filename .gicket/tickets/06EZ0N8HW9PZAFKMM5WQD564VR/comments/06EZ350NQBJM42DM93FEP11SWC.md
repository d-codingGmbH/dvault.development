[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0N8HW9PZAFKMM5WQD564VR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0N8HW9PZAFKMM5WQD564VR`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y44KW826FN8T6XKSW62TM`, `currentRevision=06EZ33F5XFQMS7G92XSGHKFRDC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0N8HW9PZAFKMM5WQD564VR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0N8HW9PZAFKMM5WQD564VR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0N8HW9PZAFKMM5WQD564VR-story-define-provider-optimization-contract-and' from source 'cbcc66a0d1f6679f52c9df705d1972c0c2edf82d'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If future provider packages introduce overlapping compatible strategies without explicit priority and tie-break tests, dispatch behavior can drift even though fallback remains available.
- The core translator currently carries a SQLite capability-profile baseline, so later provider-aware metadata work could expand scope unless this story stays tightly focused on save optimization contracts and dispatch.
- If the documentation matrix and provider package registrations drift apart, downstream provider stories may implement against the wrong ownership or validation expectations.
- Split recommendation: Keep this story limited to the shared contract, dispatch semantics, fallback behavior, and five-provider documentation; do not bundle new optimized writers for PostgreSQL, SQL Server, Oracle, or MySQL into it.
- Split recommendation: If implementation pressure grows, defer any capability-profile expansion beyond the existing SQLite baseline to a separate architecture or provider story after this shared dispatcher contract is accepted.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9091`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a4a8e557cf6b4f75ae619eaf77f78172`
- completed-at-utc: `<redacted>-04T06:01:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0N8HW9PZAFKMM5WQD564VR/runs/20260504T060146479Z-a4a8e557cf6b4f75ae619eaf77f78172.json`