[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZVRARQPG482YKCQ686PNM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZVRARQPG482YKCQ686PNM`.
- Optimistic claim succeeded (`expectedRevision=06F9GF37GC8EAW302HPDQP186C`, `currentRevision=06FAJND1EDE6V769XA1ZGP440C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZVRARQPG482YKCQ686PNM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZVRARQPG482YKCQ686PNM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZVRARQPG482YKCQ686PNM-task-update-v0-32-0-artifact-lane-documentation' from source '0339ca754811985b1b6bec1a4d2dd59a59d287e8'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZVRARQPG482YKCQ686PNM-task-update-v0-32-0-artifact-lane-documentation` as `1b42281e28b2`.

Open questions / Risiken
- The current source exporter is narrower than the high-level provider baseline; docs must avoid implying that all five supported providers already have implemented artifact exporters.
- The evidence gate depends on shared benchmark artifacts and request-bound diagnostics; release prose that collapses skipped optional-provider rows into completed claims would overstate the repository evidence.
- If the evidence or prototype child tickets change manifest fields, workload naming, or prototype scope before merge, the v0.32 docs will need a final consistency pass.
- Split recommendation: No additional split is recommended for this ticket; the existing parent artifact-lane contract already separates documentation scope from evidence and dry-run prototype work.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8555`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7341dd72e49548c6be479e17ac7aa001`
- completed-at-utc: `<redacted>-08T22:36:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZVRARQPG482YKCQ686PNM/runs/20260608T223627463Z-7341dd72e49548c6be479e17ac7aa001.json`