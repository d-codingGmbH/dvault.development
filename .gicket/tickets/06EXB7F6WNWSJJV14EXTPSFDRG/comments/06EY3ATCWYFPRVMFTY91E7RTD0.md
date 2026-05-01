[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7F6WNWSJJV14EXTPSFDRG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7F6WNWSJJV14EXTPSFDRG`.
- Optimistic claim succeeded (`expectedRevision=06EY39750YFPY0MTSKFVB807VR`, `currentRevision=06EY39APTD3V9N891PFRZVY01M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc' from source 'e97132036ee13d823aba02b3b8c502651d52cad8'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc` as `f289d084e9ff`.

Open questions / Risiken
- Required PO action: Update the parent ticket's status/labels/handoff so it represents a closure/tracking item instead of active blocked work; specifically clear workflow metadata that implies pending developer or tester action on `06EXB7F6WNWSJJV14EXTPSFDRG`.
- Required PO action: Make the next automation path explicit at the ticket level, not only in prose comments, so the parent epic is not dispatched to dev again while its contract says no implementation slice remains.
- Required PO action: If the workflow cannot represent a closure-only epic on this path, move this parent ticket onto the correct completion/closure route before re-running PO-critic.
- Risky assumption: The contract assumes closure intent alone is sufficient, but the observed role-path for a successful PO-critic review still routes to dev unless ticket-level workflow metadata changes prevent that misroute.
- Split recommendation: No new implementation split is needed for this parent epic itself.
- Split recommendation: If first-class Postgres runtime/provider support or save-path convenience APIs are later approved, schedule them as separate follow-up tickets or an epic instead of reopening `06EXB7F6WNWSJJV14EXTPSFDRG`.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9214`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `6810f631210e4bd996987d8ae3284d64`
- completed-at-utc: `<redacted>-01T03:53:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/runs/20260501T035312556Z-6810f631210e4bd996987d8ae3284d64.json`