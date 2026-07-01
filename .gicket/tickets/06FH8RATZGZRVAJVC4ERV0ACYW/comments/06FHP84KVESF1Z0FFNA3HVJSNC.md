[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FH8RATZGZRVAJVC4ERV0ACYW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RATZGZRVAJVC4ERV0ACYW`.
- Optimistic claim succeeded (`expectedRevision=06FHNWC8WADVWXYTAD1AGYYKB4`, `currentRevision=06FHP66YH2W2B4ZDGA64PHN568`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8RATZGZRVAJVC4ERV0ACYW-task-refresh-provider-benchmark-gap-matrix-and-c' from source 'e4b676fa674c5da9644a1e70ac55f85115be003a'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FH8RATZGZRVAJVC4ERV0ACYW-task-refresh-provider-benchmark-gap-matrix-and-c` as `dadf42b4a3c0`.

Open questions / Risiken
- Risky assumption: Downstream work will treat the 2026-06-23 save/read rows as closed evidence and will not reopen them just because the legacy draft mentioned rerunning benchmarks.
- Risky assumption: The optional DB2 PIT maintenance lane will not be lost before someone creates the separate bounded child ticket described in the matrix and feasibility note.
- Split recommendation: Keep save strategy parity in 06FH8RC9F0QEWF356WF7YYNNGM and read parity in 06FH8RDS25081N5S181C7TQGTG.
- Split recommendation: Only add one extra child if the team wants the accepted DB2 lane now, and keep it limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy.
- Split recommendation: Do not fold Oracle PIT maintenance reopen work, MySQL PIT maintenance timing evidence work, or bridge-maintenance push-down into this ticket or the existing save/read children.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9047`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7b799984d6a84d7dbc8c45e39e351c51`
- completed-at-utc: `<redacted>-01T00:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RATZGZRVAJVC4ERV0ACYW/runs/20260701T004227670Z-7b799984d6a84d7dbc8c45e39e351c51.json`