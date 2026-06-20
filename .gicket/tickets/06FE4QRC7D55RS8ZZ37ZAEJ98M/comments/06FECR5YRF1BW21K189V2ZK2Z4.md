[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FE4QRC7D55RS8ZZ37ZAEJ98M-task-refine-sql-server-bulk-thresholds-and-stage' for ticket '06FE4QRC7D55RS8ZZ37ZAEJ98M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QRC7D55RS8ZZ37ZAEJ98M`.
- Optimistic claim succeeded (`expectedRevision=06FEBJJKN57C5VG9SXDQXCVF2R`, `currentRevision=06FECPHNK5YC0EV88NMV337D1C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FE4QRC7D55RS8ZZ37ZAEJ98M-task-refine-sql-server-bulk-thresholds-and-stage' and commit '46b489e8b961' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FE4QRC7D55RS8ZZ37ZAEJ98M-task-refine-sql-server-bulk-thresholds-and-stage' from source '46b489e8b961'.
- Interactive tester tool loop completed review for branch 'ticket/06FE4QRC7D55RS8ZZ37ZAEJ98M-task-refine-sql-server-bulk-thresholds-and-stage'.
- Evidence: git diff --name-only develop...46b489e8b961 shows non-metadata repo changes only in sqlserver-threshold-decision.md, docs/performance-profiles.md, docs/plans/provider-optimization-evidence-matrix.md, docs/plans/provider-optimization-gap-matrix.md, docs/releases/v0.32...
- Evidence: git diff 46b489e8b961..HEAD shows only .gicket metadata paths, so current repository file inspection matches the claimed commit for non-metadata files.
- Evidence: git ls-tree -r --name-only 46b489e8b961 -- sqlserver-threshold-decision.md artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/sqlserver-threshold-decision.md lists both required repository output paths.
- Evidence: git diff --check develop...46b489e8b961 over the changed repo files returned no output.
- Evidence: sqlserver-threshold-decision.md records the unchanged 50 minimum total operations and 500 maximum satellite operations gate, clean SQL Server context, SqlServerDataVaultSaveStrategy selection, provider-neutral fallback with selectedStrategy=<none>, the review-only dv...
- Evidence: artifacts/benchmarks/06FE4QRC7D55RS8ZZ37ZAEJ98M-sqlserver-bulk-thresholds-<redacted>/benchmark-summary.md records completed SQL Server provider-native-bulk-ingestion with selectedStrategy=SqlServerDataVaultSaveStrategy, transfer=SqlBulkCopy, nativeBulkBoundary=50-plus-...
- 45 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8951`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `688cd267c88c43b49a9f3da594b3dd24`
- completed-at-utc: `<redacted>-20T18:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QRC7D55RS8ZZ37ZAEJ98M/runs/20260620T185240509Z-688cd267c88c43b49a9f3da594b3dd24.json`