[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCAQGWFC9S98YCVDP4V7PC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCAQGWFC9S98YCVDP4V7PC`.
- Optimistic claim succeeded (`expectedRevision=06FCYA7JQQTZFXWGJ391837E0G`, `currentRevision=06FCYADXHKR93S0S76A4DBA18R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCAQGWFC9S98YCVDP4V7PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCAQGWFC9S98YCVDP4V7PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCAQGWFC9S98YCVDP4V7PC-task-implement-accepted-db2-bulk-improvement' from source '197a19fe4c16623a1bcede5ab506391925d27ce2'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- A later reader could overstate DB2 evidence if skipped-placeholder benchmark rows or opt-in smoke coverage are treated as completed DB2 timing claims.
- Split recommendation: Do not split or reopen this ticket; if more DB2 benchmark or documentation evidence is desired later, create one separate narrow evidence-only follow-up ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `42512`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1776`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `cb419c9f84b743759a62e37816b9ce6d`
- completed-at-utc: `<redacted>-16T06:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/runs/20260616T064716500Z-cb419c9f84b743759a62e37816b9ce6d.json`