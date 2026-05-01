[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7F6WNWSJJV14EXTPSFDRG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7F6WNWSJJV14EXTPSFDRG`.
- Optimistic claim succeeded (`expectedRevision=06EY3NG7VTDC9KAS1H3AQRHGH4`, `currentRevision=06EY3NKYBZPCV6MMFAHZ0NRKDW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc' from source '44d0f12ee353e77f3d3704bd75fef92979cbce93'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc` as `5acccacccc2b`.

Open questions / Risiken
- Required PO action: Re-run or otherwise complete the ticket-field handoff so downstream automation no longer reads the parent epic as blocked on developer/tester execution.
- Risky assumption: Assuming the stale blocked labels will be cleared later without an explicit ticket-field fix is not supported by the current persisted state.
- Split recommendation: No additional split is needed inside 06EXB7F6WNWSJJV14EXTPSFDRG once the label contradiction is fixed; the four existing child stories already form the bounded delivery path.
- Split recommendation: If first-class Postgres runtime/provider support, `SaveChanges` interception, or deferred Data Vault capabilities are approved later, create separate follow-up tickets or epics instead of reopening this parent.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8276`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `1314cfe95b1c4e6a9f50e82e1d8e0f63`
- completed-at-utc: `<redacted>-01T04:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/runs/20260501T044806079Z-1314cfe95b1c4e6a9f50e82e1d8e0f63.json`