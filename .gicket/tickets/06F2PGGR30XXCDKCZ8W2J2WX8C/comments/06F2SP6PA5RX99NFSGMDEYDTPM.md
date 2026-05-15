[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGGR30XXCDKCZ8W2J2WX8C'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGGR30XXCDKCZ8W2J2WX8C`.
- Optimistic claim succeeded (`expectedRevision=06F2PNHGVQH2PMM7HJD8VER0Y4`, `currentRevision=06F2SM85QADY6DY94TDHXNH10W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGGR30XXCDKCZ8W2J2WX8C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGGR30XXCDKCZ8W2J2WX8C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGGR30XXCDKCZ8W2J2WX8C-task-add-ci-workflow-examples-for-design-time-ch' from source 'b4f6eb00a63209b5d30608b5aee32ae7d46b6438'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGGR30XXCDKCZ8W2J2WX8C-task-add-ci-workflow-examples-for-design-time-ch` as `0e7f7978b221`.

Open questions / Risiken
- If the examples blur the consumer-owned boundary, adopters may assume DVault intercepts `dotnet ef`, auto-discovers migrations, or ships a standalone CLI.
- If the default example uses `--live-schema` instead of artifact-based drift, non-SQLite adopters could copy an unsupported or secret-dependent gate.
- If CI examples teach `export` as the blocking check, teams may validate against freshly generated artifacts instead of a reviewed committed baseline.
- Historical incoming `blocks` relations from done tickets remain live in the ticket store, so schedule views may appear more constrained than the actual implementation dependency baseline.
- Split recommendation: No additional split is recommended; the existing breakdown is already bounded across story `06F2PGGEY26Y65G97NGFKH381M`, command-surface implementation task `06F2PGGJQMKH2T5948VJH93M5R`, this CI/examples task, and broader documentation task `06F2PGHA0EXJR...
- Split recommendation: If the project later wants provider-specific operational templates or non-GitHub CI systems, capture them as separate follow-up tickets instead of widening this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9575`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b738c1b7eb16499ba198b2d08fdd1351`
- completed-at-utc: `<redacted>-15T18:14:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGGR30XXCDKCZ8W2J2WX8C/runs/20260515T181428513Z-b738c1b7eb16499ba198b2d08fdd1351.json`