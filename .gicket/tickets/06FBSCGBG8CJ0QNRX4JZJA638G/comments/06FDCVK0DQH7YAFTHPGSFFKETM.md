[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps' for ticket '06FBSCGBG8CJ0QNRX4JZJA638G'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCGBG8CJ0QNRX4JZJA638G`.
- Optimistic claim succeeded (`expectedRevision=06FDCQNNMSDBYZC0KC5RRX4KVW`, `currentRevision=06FDCR6ZFEP0CD6YY5FY13BDHG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps' and commit 'cda5da3e184a' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps' from source 'cda5da3e184a'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: The claimed commit cda5da3e184a adds only tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs. Branch and repository inspection support the ticket contract state, a...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps'.
- Checked out verification commit 'cda5da3e184a'.
- Derived 1 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 1 repository path(s) at commit 'cda5da3e184a'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 68 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator using branch `ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps` at commit `cda5da3e184a`.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8134`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `f38163a479f4465f92029603e11a4dd0`
- completed-at-utc: `<redacted>-17T16:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/runs/20260617T163338362Z-f38163a479f4465f92029603e11a4dd0.json`