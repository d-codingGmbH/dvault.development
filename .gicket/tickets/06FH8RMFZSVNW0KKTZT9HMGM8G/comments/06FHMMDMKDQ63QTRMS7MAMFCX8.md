[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo' for ticket '06FH8RMFZSVNW0KKTZT9HMGM8G'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RMFZSVNW0KKTZT9HMGM8G`.
- Optimistic claim succeeded (`expectedRevision=06FHMHH2WM61QN12W8Y8E9DP3W`, `currentRevision=06FHMHXVTGMBA7B8NRN0CGAQT0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo' and commit '83fa6a29a74b' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo' from source '83fa6a29a74b'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Repository review found no direct structural defect: commit 83fa6a29a74b only adds unit coverage in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, while the inspected repo...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo'.
- Checked out verification commit '83fa6a29a74b'.
- Derived 1 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 1 repository path(s) at commit '83fa6a29a74b'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 61 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator with the recorded verification evidence for branch `ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo` at commit `83fa6a29a74b`.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8117`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `831a148fa05a431eb0e4190cc24eb2ce`
- completed-at-utc: `<redacted>-30T20:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RMFZSVNW0KKTZT9HMGM8G/runs/20260630T205630103Z-831a148fa05a431eb0e4190cc24eb2ce.json`