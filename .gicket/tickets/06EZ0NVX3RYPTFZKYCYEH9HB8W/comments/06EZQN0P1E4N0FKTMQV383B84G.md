[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NVX3RYPTFZKYCYEH9HB8W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NVX3RYPTFZKYCYEH9HB8W`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y4PSCQP9HWDFKTBY5JX4C`, `currentRevision=06EZQJ8S10YAEQGTKG1PWSK30G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NVX3RYPTFZKYCYEH9HB8W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NVX3RYPTFZKYCYEH9HB8W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NVX3RYPTFZKYCYEH9HB8W-task-define-multi-active-satellite-driving-key-c' from source '9913563ecfc2844152bb02cc6371662dab0f12b1'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If implementation allows volatile descriptive fields or metadata-derived values into the driving key, unchanged suppression can degrade into insert-every-time behavior.
- If downstream work computes hash diff from only driving-key members instead of the full payload, non-key payload changes inside one concurrent row partition can be missed.
- If reviewers read this contract as a promise of provider-specific uniqueness indexes or multi-writer conflict handling, downstream delivery can overstate guarantees that the current provider-neutral baseline does not make.
- Split recommendation: No additional split is needed. Keep this ticket as the contract-definition slice, keep persistence behavior in 06EZ0NW61GFJN90PSB5N934G2G, and keep docs/tests in 06EZ0NWCA6NEZH8VBJNGW4FVHG.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9543`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5cb43adcc5bf4cd9a45e15109aed6788`
- completed-at-utc: `<redacted>-06T05:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NVX3RYPTFZKYCYEH9HB8W/runs/20260506T054752995Z-5cb43adcc5bf4cd9a45e15109aed6788.json`