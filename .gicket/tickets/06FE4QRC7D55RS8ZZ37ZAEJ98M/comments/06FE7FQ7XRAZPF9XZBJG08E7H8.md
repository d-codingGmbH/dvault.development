[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4QRC7D55RS8ZZ37ZAEJ98M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QRC7D55RS8ZZ37ZAEJ98M`.
- Optimistic claim succeeded (`expectedRevision=06FE4QTK0TT2S62WM05SXX926C`, `currentRevision=06FE7DZG7XT318XHG2FWPSBH9R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4QRC7D55RS8ZZ37ZAEJ98M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4QRC7D55RS8ZZ37ZAEJ98M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4QRC7D55RS8ZZ37ZAEJ98M-task-refine-sql-server-bulk-thresholds-and-stage' from source 'cf8ba9f3a09d52dbfe51eed52e41e315b9b632b9'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4QRC7D55RS8ZZ37ZAEJ98M-task-refine-sql-server-bulk-thresholds-and-stage` as `73f0e38748bd`.

Open questions / Risiken
- The repo uses both historical completed threshold bundles and current root skipped-placeholder rows; careless wording could overstate what is actually measured timing versus planning guidance.
- Calling the SQL Server path staged bulk without the current native-`SqlBulkCopy` and temporary-staging-table boundary would blur it with PostgreSQL/MySQL staged-provider lanes and misstate runtime behavior.
- The dry-run SQL artifact lane is easy to overread as a deployable implementation; the current repo only supports review-only manifest output for SQL Server and explicitly excludes runtime dispatch.
- Split recommendation: No immediate split is required; the current ticket is already bounded if it stays on SQL Server save-threshold and dry-run artifact evidence clarification.
- Split recommendation: If more scope is needed later, split provider-configured SQL Server bulk timing promotion, SQL Server latest-satellite timing evidence, and any deployable SQL artifact/runtime dispatch proposal into separate tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9194`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `972aef48ee234031962b68e6c5ce7b05`
- completed-at-utc: `<redacted>-20T06:36:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QRC7D55RS8ZZ37ZAEJ98M/runs/20260620T063639783Z-972aef48ee234031962b68e6c5ce7b05.json`