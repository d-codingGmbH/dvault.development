[gicket-bot] Run report (outcome: test-workflow-failed)

Summary
- Tester workflow for ticket '06EXB74DC57F8HC98X4D6ZBHXW' failed because the interactive model response was not parseable.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB74DC57F8HC98X4D6ZBHXW`.
- Optimistic claim succeeded (`expectedRevision=06EXX5E6RY7Z4EFNKBG9J9YMV0`, `currentRevision=06EXX5K92N1JPS07ETDF2HBXDG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06EXB74DC57F8HC98X4D6ZBHXW-epic-data-vault-2-x-modeling-core' (developer-delivery-outcome contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06EXB74DC57F8HC98X4D6ZBHXW-epic-data-vault-2-x-modeling-core' from source 'ticket/06EXB74DC57F8HC98X4D6ZBHXW-epic-data-vault-2-x-modeling-core'.

Open questions / Risiken
- Model response contained invalid JSON: '}' is invalid without a matching open. LineNumber: 0 | BytePositionInLine: 5329.
- Unparseable model response captured locally at 'C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260430T133334866Z-test-tester-06EXB74DC57F8HC98X4D6ZBHXW.json'.

Next steps
- Inspect the tester model response format.
- Retry after aligning the model output with the tester assessment schema.
- Inspect the captured raw model response at 'C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260430T133334866Z-test-tester-06EXB74DC57F8HC98X4D6ZBHXW.json'.

Prompt cache usage
- prompt-tokens: `53615`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0454`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `c5c618f5715245c5a858d99b0a7d0712`
- completed-at-utc: `<redacted>-30T13:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB74DC57F8HC98X4D6ZBHXW/runs/20260430T133335859Z-c5c618f5715245c5a858d99b0a7d0712.json`