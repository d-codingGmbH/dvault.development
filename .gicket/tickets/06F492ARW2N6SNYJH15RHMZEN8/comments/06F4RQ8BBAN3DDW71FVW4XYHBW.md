[gicket-bot] Run report (outcome: test-workflow-failed)

Summary
- Tester workflow for ticket '06F492ARW2N6SNYJH15RHMZEN8' failed because the interactive model response was not parseable.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492ARW2N6SNYJH15RHMZEN8`.
- Optimistic claim succeeded (`expectedRevision=06F4RNJZTHPBXAVNWEH8BC57T4`, `currentRevision=06F4RNTQ7V0GAWN6ME72DHR89C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in' and commit '55a5782224e3' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in' from source '55a5782224e3'.

Open questions / Risiken
- Model response contained invalid JSON: '}' is invalid without a matching open. LineNumber: 0 | BytePositionInLine: 5497.
- Unparseable model response captured locally at 'C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260521T210701350Z-test-tester-06F492ARW2N6SNYJH15RHMZEN8.json'.

Next steps
- Inspect the tester model response format.
- Retry after aligning the model output with the tester assessment schema.
- Inspect the captured raw model response at 'C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260521T210701350Z-test-tester-06F492ARW2N6SNYJH15RHMZEN8.json'.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8761`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `93c58a674d6f4970a8089818c9557d56`
- completed-at-utc: `<redacted>-21T21:07:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492ARW2N6SNYJH15RHMZEN8/runs/20260521T210706711Z-93c58a674d6f4970a8089818c9557d56.json`