[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo' and commit '83fa6a29a74b' for ticket '06FH8RMFZSVNW0KKTZT9HMGM8G'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RMFZSVNW0KKTZT9HMGM8G`.
- Optimistic claim succeeded (`expectedRevision=06FHMDFPASKCZXMEZJGX3ZC55R`, `currentRevision=06FHMDX1B9KY0SQJM8AFD61N5M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded developer ticket mutation surface enabled: gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-relation.
- Prepared interactive developer scratch worktree for target branch 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo' from source 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo'.
- Planned implementation step: Extended `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs` with deterministic fail-closed diagnostics coverage for SQL Server provider-native crypto selection against a PostgreSQL capability profile.
- Planned implementation step: Added a DbContext-based unsupported-provider path check proving SQL Server native selection fails closed on SQLite instead of auto-routing or downgrading.
- Planned implementation step: Added direct SQL Server selection-provider tests for unavailable reviewed capability facts and unsupported Always Encrypted capability facts.
- Planned implementation step: Added support-bundle assertions proving native-selection diagnostics do not expose caller-owned prerequisite strings containing connection-string, secret, raw SQL, or key-material-like values.
- Resolved branch route (fallback): base 'develop', work 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo'.
- 11 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Focused `dotnet test` could not execute in this run because required EF Core analyzer packages were absent from the local restore cache and network-dependent restore was outside the execution boundary.

Next steps
- Push branch 'ticket/06FH8RMFZSVNW0KKTZT9HMGM8G-task-implement-provider-native-crypto-usage-proo' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9206`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `e0582a68773741b2b986488fe902356e`
- completed-at-utc: `<redacted>-30T20:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RMFZSVNW0KKTZT9HMGM8G/runs/20260630T204255159Z-e0582a68773741b2b986488fe902356e.json`