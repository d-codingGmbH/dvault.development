[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZQNH8CCMTJW9P95W1N388'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZQNH8CCMTJW9P95W1N388`.
- Optimistic claim succeeded (`expectedRevision=06F9JF83QP3ZE76RHVJNP0B2QW`, `currentRevision=06F9WFVE7GQ962VB9R5J34ZSSM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZQNH8CCMTJW9P95W1N388': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZQNH8CCMTJW9P95W1N388': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZQNH8CCMTJW9P95W1N388-epic-performance-decision-guidance-and-observabi' from source 'dc82631ddfede5c035398737ff9bba6387e3dc81'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Later documentation edits could reintroduce baseline drift or broaden v0.31.0 claims beyond the explicit non-goals if they stop linking back to the authoritative docs.
- Split recommendation: No further split recommended; the epic is already decomposed into five completed child tickets.
- Split recommendation: Any future work on relation-graph hygiene or post-v0.31 release governance should be tracked as separate maintenance tickets rather than reopening this epic.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9034`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5891f66b153a4ef6986a653650911953`
- completed-at-utc: `<redacted>-06T18:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZQNH8CCMTJW9P95W1N388/runs/20260606T185841742Z-5891f66b153a4ef6986a653650911953.json`