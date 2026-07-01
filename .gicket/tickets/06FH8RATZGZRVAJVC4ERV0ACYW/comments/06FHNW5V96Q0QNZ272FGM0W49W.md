[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8RATZGZRVAJVC4ERV0ACYW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RATZGZRVAJVC4ERV0ACYW`.
- Optimistic claim succeeded (`expectedRevision=06FH8SEVWHGZ3ZTQW9F0DGY4ZG`, `currentRevision=06FHNT18JVRATM82HV2NEGFFF8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8RATZGZRVAJVC4ERV0ACYW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8RATZGZRVAJVC4ERV0ACYW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8RATZGZRVAJVC4ERV0ACYW-task-refresh-provider-benchmark-gap-matrix-and-c' from source 'f9c16a2b5a11b18db6480249b83de531e8986727'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FH8RATZGZRVAJVC4ERV0ACYW-task-refresh-provider-benchmark-gap-matrix-and-c` as `873c702b132f`.

Open questions / Risiken
- The ticket draft still mentions rerunning targeted benchmarks, so without this refinement downstream work could duplicate the already checked-in 2026-06-23 closure evidence.
- Because no DB2 PIT maintenance implementation child is currently visible, the accepted DB2 maintenance lane could be lost between planning and delivery.
- The v2 gap matrix is closure-oriented; if downstream tickets treat every remaining fallback boundary as implementation scope, they will overrun the bounded parity plan.
- Split recommendation: Do not split the current ticket further for save or read work; those paths already have bounded implementation children 06FH8RC9F0QEWF356WF7YYNNGM and 06FH8RDS25081N5S181C7TQGTG.
- Split recommendation: Create one additional child only if the team wants to pursue the accepted DB2 PIT maintenance lane now: limit it to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy.
- Split recommendation: Keep Oracle PIT maintenance reopen work, MySQL PIT maintenance timing evidence, and any bridge-maintenance push-down or staged DB2 bulk work as separate later tickets rather than enlarging the current parity implementation children.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9309`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0ee75831fe35490db0484c37201d6651`
- completed-at-utc: `<redacted>-30T23:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RATZGZRVAJVC4ERV0ACYW/runs/20260630T235012026Z-0ee75831fe35490db0484c37201d6651.json`