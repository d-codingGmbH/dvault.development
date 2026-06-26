[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF442BD5V9CTTNXQQAR3EQTW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF442BD5V9CTTNXQQAR3EQTW`.
- Optimistic claim succeeded (`expectedRevision=06FF44S045GTGR51M0BZC1QYWC`, `currentRevision=06FG6PYN3BBJ4MF533YZWNDFY8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF442BD5V9CTTNXQQAR3EQTW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF442BD5V9CTTNXQQAR3EQTW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF442BD5V9CTTNXQQAR3EQTW-task-review-effectivity-and-typed-helper-gap-doc' from source 'c1667f949b181acea04c3ae66d2630b400dbc7bd'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF442BD5V9CTTNXQQAR3EQTW-task-review-effectivity-and-typed-helper-gap-doc` as `49674a76a4eb`.

Open questions / Risiken
- This boundary is described across current architecture docs, adoption guidance, model-first governance, release notes, and historical plans; partial edits could leave contradictory reader guidance.
- Readers may conflate typed read helpers with typed save mappers/source-generator parity unless the updated docs consistently use precise terminology and cross-links.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8308`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `2715a0af72fa43b298a00e88ba45f88d`
- completed-at-utc: `<redacted>-26T10:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF442BD5V9CTTNXQQAR3EQTW/runs/20260626T100236397Z-2715a0af72fa43b298a00e88ba45f88d.json`