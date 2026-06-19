[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCH65R88BT6PS7XV32NQ1M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCH65R88BT6PS7XV32NQ1M`.
- Optimistic claim succeeded (`expectedRevision=06FD6EE67HZPQ5KDE23RFZCA6M`, `currentRevision=06FDTKYA3A47FZE7N5G99SPV00`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCH65R88BT6PS7XV32NQ1M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCH65R88BT6PS7XV32NQ1M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCH65R88BT6PS7XV32NQ1M-task-close-db2-pit-and-bridge-read-gaps' from source 'cbfcab91bf17db53da2b4ff514069ab6d333b7d5'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCH65R88BT6PS7XV32NQ1M-task-close-db2-pit-and-bridge-read-gaps` as `0c951823d18f`.

Open questions / Risiken
- The main contract risk is overclaiming DB2 performance from smoke-only, diagnostics-only, or skipped-placeholder evidence; the checked-in repo explicitly disallows that promotion.
- Future DB2 evidence work depends on an opt-in external environment and may stall without an approved connection-string-backed benchmark lane.
- Because DB2 keeps the narrower v0.34 boundary, later tickets can accidentally mix PIT/bridge timing follow-up with out-of-scope DB2 latest-satellite or broader provider-expansion work.
- Split recommendation: No split is recommended now; the current evidence already justifies a defer/no-work-required refinement rather than more child tickets.
- Split recommendation: If DB2 evidence work is later approved, create a new follow-up ticket for the approved environment-backed benchmark run instead of widening this ticket into mixed implementation and evidence scope.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9514`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `99506aabb1eb4cbba7e408b1d8ab3bbf`
- completed-at-utc: `<redacted>-19T00:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCH65R88BT6PS7XV32NQ1M/runs/20260619T004942695Z-99506aabb1eb4cbba7e408b1d8ab3bbf.json`