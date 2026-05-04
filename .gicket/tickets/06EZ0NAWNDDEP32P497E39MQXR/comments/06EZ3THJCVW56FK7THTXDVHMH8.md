[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NAWNDDEP32P497E39MQXR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NAWNDDEP32P497E39MQXR`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y3CY05EJDQ4J6NZN99REW`, `currentRevision=06EZ3SE7V3TAAW82JJPG96YK0W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0NAWNDDEP32P497E39MQXR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NAWNDDEP32P497E39MQXR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NAWNDDEP32P497E39MQXR-task-add-sql-server-opt-in-integration-configura' from source 'f3e5edc4986519b7419c227048cd2b69120bea16'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NAWNDDEP32P497E39MQXR-task-add-sql-server-opt-in-integration-configura` as `b423983d693a`.

Open questions / Risiken
- Because the SQL Server lane is external and opt-in, regressions can escape default automation unless contributors run the documented smoke command.
- Different local SQL Server versions or connection defaults can create environment-specific failures unless the documentation pins the supported setup tightly.
- Split recommendation: No split recommended; the repository already has a single bounded integration-test lane and established provider-test conventions, so configuration plus three smoke scenarios fits one task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `49923`
- cached-tokens: `10624`
- effective-cache-ratio: `0.2128`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `9663bf6f5e444c2c83c92a3621ac0714`
- completed-at-utc: `<redacted>-04T07:35:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NAWNDDEP32P497E39MQXR/runs/20260504T073549967Z-9663bf6f5e444c2c83c92a3621ac0714.json`