[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7QYF1BB1REM7HQZ4WWVMM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7QYF1BB1REM7HQZ4WWVMM`.
- Optimistic claim succeeded (`expectedRevision=06EYM3SRJE6RNAWBZQ9R5D0MKG`, `currentRevision=06EYM3XK8C9AWKZ3X9CNDHT3EG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7QYF1BB1REM7HQZ4WWVMM-story-write-getting-started-documentation' from source '830905d0a3ba52984e11ed3ea3bab41f33dae665'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7QYF1BB1REM7HQZ4WWVMM-story-write-getting-started-documentation` as `b4b345153e7b`.

Open questions / Risiken
- Required PO action: Decide whether 06EXB7QYF1BB1REM7HQZ4WWVMM should now be treated as an umbrella/aggregation story to advance or close from PO instead of handing it to dev.
- Required PO action: If the parent story is still expected to go to dev, add explicit parent-only remaining work that is not already covered by completed child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC.
- Required PO action: Align the parent ticket's status/labels with the actual post-child-completion workflow state.
- Risky assumption: Assuming there is no separate parent-level closure rule outside the persisted ticket/comment data.
- Split recommendation: No further split is needed; the existing split into 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC is already the implemented decomposition.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9284`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `681a1a3033b84ee4bb073642a10eb093`
- completed-at-utc: `<redacted>-02T19:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7QYF1BB1REM7HQZ4WWVMM/runs/20260502T190459050Z-681a1a3033b84ee4bb073642a10eb093.json`