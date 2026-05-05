[gicket-bot] Run report (outcome: test-workflow-failed)

Summary
- Tester workflow for ticket '06EZ0NBX79YQ0J5A9ECJG955TC' failed because the interactive model response was not parseable.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NBX79YQ0J5A9ECJG955TC`.
- Optimistic claim succeeded (`expectedRevision=06EZ7FN2PZSS908B0SJP4MN780`, `currentRevision=06EZ7GV7TTNSAPSVBF1EJ9EHYW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile' and commit '86bf61cd5a71' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile' from source '86bf61cd5a71'.

Open questions / Risiken
- Model response contained invalid JSON: '}' is invalid without a matching open. LineNumber: 0 | BytePositionInLine: 7577.
- Unparseable model response captured locally at 'C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260504T161851464Z-test-tester-06EZ0NBX79YQ0J5A9ECJG955TC.json'.

Next steps
- Inspect the tester model response format.
- Retry after aligning the model output with the tester assessment schema.
- Inspect the captured raw model response at 'C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260504T161851464Z-test-tester-06EZ0NBX79YQ0J5A9ECJG955TC.json'.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9201`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `f3411d7c6050446f85c98a291c25fd6a`
- completed-at-utc: `<redacted>-04T16:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NBX79YQ0J5A9ECJG955TC/runs/20260504T161853299Z-f3411d7c6050446f85c98a291c25fd6a.json`