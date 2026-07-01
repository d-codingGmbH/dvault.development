[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FH8RDS25081N5S181C7TQGTG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RDS25081N5S181C7TQGTG`.
- Optimistic claim succeeded (`expectedRevision=06FHPTX9WWNQDZFHB5DB387998`, `currentRevision=06FHPYTDDSKTTQVMBJF6GTX650`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8RDS25081N5S181C7TQGTG-task-close-selected-provider-latest-satellite-pi' from source 'f0d04a309a7e83d9dd6cce7f6019ff4b54b06a9e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FH8RDS25081N5S181C7TQGTG-task-close-selected-provider-latest-satellite-pi` as `17b171176436`.

Open questions / Risiken
- Risky assumption: Developers will treat root `benchmark-summary.*` external-provider read rows as skipped placeholders and will cite `artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/` for completed external-provider timing, as required by...
- Risky assumption: Developers will not reopen PIT maintenance, bridge maintenance push-down, save-path parity, or documentation work that the ticket description explicitly marks out-of-scope and assigns to sibling tickets `06FH8RC9F0QEWF356WF7YYNNGM` and `06FH8REKX113JRZQ42HEB1...
- Split recommendation: Do not split this read ticket further; the persisted contract already keeps save-path and documentation work in sibling tickets.
- Split recommendation: If later work is needed, keep DB2 PIT full-rebuild maintenance as a separate follow-up instead of reopening the closed latest-satellite/PIT/bridge read closure rows.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9181`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `99108d228ebe46c796691eacb43eae6f`
- completed-at-utc: `<redacted>-01T02:29:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RDS25081N5S181C7TQGTG/runs/20260701T022938181Z-99108d228ebe46c796691eacb43eae6f.json`