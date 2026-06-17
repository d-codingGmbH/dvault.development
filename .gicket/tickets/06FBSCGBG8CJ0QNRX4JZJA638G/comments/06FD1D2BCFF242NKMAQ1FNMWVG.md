[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FBSCGBG8CJ0QNRX4JZJA638G'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCGBG8CJ0QNRX4JZJA638G`.
- Optimistic claim succeeded (`expectedRevision=06FD1BCMJDNXM1BR90FB68CR9W`, `currentRevision=06FD1BK3MBYY9C9BBVDVCD9K98`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps' from source 'ef367a097605fbe8f8ddbdccab388a8c8060f85a'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps` as `df87beeca7b9`.

Open questions / Risiken
- Blocking finding: The delivery contract's split plan does not match the persisted ticket graph. This ticket recommends grouped PostgreSQL+SQL Server and MySQL+Oracle follow-up plus DB2 defer, but the repository already has five provider-specific downstream `blocks` tickets (`0...
- Blocking finding: The DB2 defer decision is not reconciled with the persisted DB2 child ticket. `.gicket/tickets/06FBSCH65R88BT6PS7XV32NQ1M/description.md` still asks to `Implement or reject DB2 PIT/bridge read strategy improvements based on the audit`, which conflicts with th...
- Blocking finding: All existing downstream provider tickets still carry `needs-po`, so approving this closure-only audit now would hand off an unresolved downstream ticket topology instead of one authoritative follow-up plan.
- Required PO action: Reconcile `## Split Recommendations` with the already-persisted provider-specific child tickets: either update this ticket to endorse the five existing child tickets as the authoritative split, or explicitly supersede/replace them with a new grouped split p...
- Required PO action: Explicitly disposition the DB2 follow-up ticket `06FBSCH65R88BT6PS7XV32NQ1M`: close it as deferred/no-work, convert it into a deferred planning ticket, or revise this ticket if DB2 is actually intended to remain in active follow-up scope.
- Required PO action: Update the downstream PostgreSQL/SQL Server/MySQL/Oracle child ticket descriptions or labels so they clearly state `provider-configured PIT/bridge timing evidence for existing strategy candidates` instead of generic `implement or reject` wording.
- Risky assumption: Assumes downstream teams will infer how to reinterpret or regroup the five existing child tickets without explicit ticket updates.
- Risky assumption: Assumes the existing DB2 child ticket will be treated as deferred even though its current persisted text still invites implementation-or-reject work.
- Split recommendation: Pick one authoritative downstream topology and record it explicitly: either keep the five existing provider-specific child tickets and refine them, or supersede them with grouped evidence tickets plus a separate deferred DB2 planning ticket.
- Split recommendation: If the grouped plan is kept, explicitly supersede `06FBSCGGN528A2NC6TTA5A99X0`, `06FBSCGNY2R6PC7P4Y91RD0HVR`, `06FBSCGVAZ5G8NP1TRXFNEP6DW`, `06FBSCH0M358R5J3RGFB6GRDM4`, and `06FBSCH65R88BT6PS7XV32NQ1M` so there is one unambiguous follow-up plan.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9365`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `047f0aaf8c12474ab90d792896c8f968`
- completed-at-utc: `<redacted>-16T13:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/runs/20260616T135217114Z-047f0aaf8c12474ab90d792896c8f968.json`