[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key' for ticket '06FGX5R67T2G0FEGMWE0JBEKJ8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX5R67T2G0FEGMWE0JBEKJ8`.
- Optimistic claim succeeded (`expectedRevision=06FGYZ4PMV0412DGHF3XJVP478`, `currentRevision=06FGZQX19J0X8YZGF0WDGK7QXR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key' from source 'ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Interactive review of branch ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key found the expected privacy quickstart wiring, aligned documentation, fail-close...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key'.
- Expanded deterministic verification evidence using 5 developer verification hint(s) across 2 hinted repository path(s) at commit '95edc6ddd01c'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- Executed tester command `bash tools/check-format.sh`.
- Restored verification branch 'ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key' after tester verification.
- 86 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Committed branch delta against base branch 'develop' did not contain non-ticket repository paths to inspect.

Next steps
- Hand off to integrator using branch `ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key` at commit `95edc6ddd01c`; tester verification succeeded with `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh`.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7388`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `8670800f1b5747ce9492f7d661412f23`
- completed-at-utc: `<redacted>-28T20:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX5R67T2G0FEGMWE0JBEKJ8/runs/20260628T202648770Z-8670800f1b5747ce9492f7d661412f23.json`