[gicket-bot] Run report (outcome: po-critic-failed)

Summary
- PO-critic review for ticket '06F7Y0FR4JS1V9WHFBP70GX1SM' failed because the model response was not parseable.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0FR4JS1V9WHFBP70GX1SM`.
- Optimistic claim succeeded (`expectedRevision=06F8BKSRTY96V2Z48KB8BQ5TJW`, `currentRevision=06F8BM49PK4XRER5HPS0YHZGGC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F7Y0FR4JS1V9WHFBP70GX1SM-epic-read-plan-explainability-and-typed-read-hel' from source '8479a3a13d3b82e39e9d7209b18e1691d02c8179'.

Open questions / Risiken
- Model response JSON parsing failed: '}' is invalid without a matching open. LineNumber: 0 | BytePositionInLine: 5732. Captured raw model response: C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260602T005718988Z-po-critic-po-critic-06F7Y0FR4JS1V9WHFBP70GX1SM....

Next steps
- Review ticket comments and bot logs.
- Retry PO-critic review after resolving the reported issue.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8972`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `df70659fc8af4887be6210482efdf12b`
- completed-at-utc: `<redacted>-02T00:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0FR4JS1V9WHFBP70GX1SM/runs/20260602T005727027Z-df70659fc8af4887be6210482efdf12b.json`