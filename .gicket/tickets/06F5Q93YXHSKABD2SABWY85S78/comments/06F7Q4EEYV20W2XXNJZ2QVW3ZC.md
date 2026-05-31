[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q93YXHSKABD2SABWY85S78'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93YXHSKABD2SABWY85S78`.
- Optimistic claim succeeded (`expectedRevision=06F72ZKXPKXNBEANNN68J0G7VC`, `currentRevision=06F7Q22NKCBYYZ7PPATCJJF1C0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q93YXHSKABD2SABWY85S78': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q93YXHSKABD2SABWY85S78': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q93YXHSKABD2SABWY85S78-story-define-opt-in-activity-tracing-contract-an' from source '698d392e500e85d143665735fb11699c45543735'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If downstream implementation starts before the contract document lands, the existing blocked tickets can drift on tag omission rules or redaction boundaries even though the core span-name list is already bounded in this ticket.
- Split recommendation: No additional PO split is recommended from current evidence; the story is already bounded to one contract document and already has three downstream `blocks` relations for implementation follow-on.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `39965`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0609`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4eeec08b04104a2db5cb23fe26ddb864`
- completed-at-utc: `<redacted>-31T01:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93YXHSKABD2SABWY85S78/runs/20260531T010653791Z-4eeec08b04104a2db5cb23fe26ddb864.json`