[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F5Q8XPXEQPJTKGJ7BQGCY438'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q8XPXEQPJTKGJ7BQGCY438`.
- Optimistic claim succeeded (`expectedRevision=06F5X9VCA35ZV517X8JEPVB3WR`, `currentRevision=06F5XDF3VH503DNZK0A1ARBWPG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q8XPXEQPJTKGJ7BQGCY438-story-explain-streaming-fallback-and-remediation' from source 'dedb3fa142c3290cd591e7ecf303829b7a8ceead'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F5Q8XPXEQPJTKGJ7BQGCY438-story-explain-streaming-fallback-and-remediation` as `cf33d4f1c99c`.

Open questions / Risiken
- Risky assumption: Consumers who need the new guidance will have `AddDVaultTelemetry()` or a custom `IDataVaultTelemetryObserver` configured; the default `AddDVault()` path remains telemetry-free.
- Risky assumption: Provider-strategy gate changes will keep the current finite enum vocabulary stable enough for explanation/remediation mapping without silent drift.
- Risky assumption: Per-attempt aggregate guidance is acceptable even when different chunks contribute different fallback causes.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8081`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `3f3d7648ee7147f79b05e095da893e7e`
- completed-at-utc: `<redacted>-25T10:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q8XPXEQPJTKGJ7BQGCY438/runs/20260525T104525281Z-3f3d7648ee7147f79b05e095da893e7e.json`