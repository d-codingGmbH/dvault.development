[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FH8R9DPSKTNYB46HHVJMZ9P8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8R9DPSKTNYB46HHVJMZ9P8`.
- Optimistic claim succeeded (`expectedRevision=06FHQN0DNN5FZN6H63Z9SD40G4`, `currentRevision=06FHQNQFEE1GYQ7E4K2TQXWMZ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8R9DPSKTNYB46HHVJMZ9P8-story-close-provider-optimization-parity-gaps-fr' from source '214d1d9a73682ae1369e8a4feb3eff5f881f5866'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FH8R9DPSKTNYB46HHVJMZ9P8-story-close-provider-optimization-parity-gaps-fr` as `23f4e55a3c39`.

Open questions / Risiken
- Risky assumption: Developers will follow the authoritative Delivery Contract rather than the broader legacy draft/title wording; the contract is precise, but the short legacy framing still reads like fresh implementation discovery.
- Split recommendation: Do not split save, read, or documentation work further; the contract already points to children `06FH8RATZGZRVAJVC4ERV0ACYW`, `06FH8RC9F0QEWF356WF7YYNNGM`, `06FH8RDS25081N5S181C7TQGTG`, and `06FH8REKX113JRZQ42HEB1NVZ8`.
- Split recommendation: If the team later pursues DB2 PIT-maintenance parity, create one separate child limited to `IBM.EntityFrameworkCore` ordinary hub-parent `RebuildAsync(...)` push-down through `IDataVaultProviderPitMaintenanceStrategy`.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `7296`
- effective-cache-ratio: `0.0189`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `b712cfbceec645ad959aa1c919bdb310`
- completed-at-utc: `<redacted>-01T04:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8R9DPSKTNYB46HHVJMZ9P8/runs/20260701T041115722Z-b712cfbceec645ad959aa1c919bdb310.json`