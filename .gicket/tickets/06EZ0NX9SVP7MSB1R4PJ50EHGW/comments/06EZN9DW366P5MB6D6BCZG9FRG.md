[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NX9SVP7MSB1R4PJ50EHGW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06EZ0NX9SVP7MSB1R4PJ50EHGW`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y4Z0SQY4K83Q817M9VKS0`, `currentRevision=06EZN91G4ZQ44210CR6MX542EW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NX9SVP7MSB1R4PJ50EHGW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NX9SVP7MSB1R4PJ50EHGW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NX9SVP7MSB1R4PJ50EHGW-task-document-advanced-hook-validation-and-failu' from source '91e8543d8fb203d919d7cffa6c4fe2b5b49a44a1'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NX9SVP7MSB1R4PJ50EHGW-task-document-advanced-hook-validation-and-failu` as `b03521720f42`.

Open questions / Risiken
- The main delivery risk is documentation overclaiming runtime support for all five hook categories; current code evidence only shows concrete resolver configuration for load timestamp and record source.
- Provider override documentation can become misleading if it implies provider-specific option matrices are approved in this ticket; keep those as future provider work.
- Split recommendation: No split is recommended; the work is a bounded documentation refinement task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `36661`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0663`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `afd5f49415294c76880ea890487e6751`
- completed-at-utc: `<redacted>-06T00:17:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06EZ0NX9SVP7MSB1R4PJ50EHGW/runs/20260506T001738155Z-afd5f49415294c76880ea890487e6751.json`