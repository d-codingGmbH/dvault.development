[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma' for ticket '06F9XD26D2MHVAKZ2GCZ67BEFC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9XD26D2MHVAKZ2GCZ67BEFC`.
- Optimistic claim succeeded (`expectedRevision=06FA41K6PF49CMPXCXVEGSMJW4`, `currentRevision=06FA41Z9MQPYK7R1ZCNYH553MR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma' and commit '19f419e88241' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma' from source '19f419e88241'.
- Interactive tester tool loop completed review for branch 'ticket/06F9XD26D2MHVAKZ2GCZ67BEFC-task-capture-v0-32-0-all-provider-podman-benchma'.
- Evidence: git diff --name-status develop...19f419e88241 shows only .gitignore, .gicket/tickets/06F9XD26D2MHVAKZ2GCZ67BEFC/*, and six new files under artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted> and artifacts/benchmarks/v0.32.0-06F9XD26...
- Evidence: git ls-tree -r --name-only 19f419e88241 lists benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json in both new v0.32.0 artifact directories.
- Evidence: git show 19f419e88241:artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted>/benchmark-summary.json shows iterations 5, warmupIterations 1, providerFilter all, and optionalProviders entries for PostgreSQL, SQL Server, MySQL, and Oracle...
- Evidence: A scenarioName count over the scale JSON at commit 19f419e88241 returned 120 rows, and the same count over the smoke-read JSON returned 50 rows.
- Evidence: Commit-scoped inspection of the smoke-read JSON shows completed bridge-traversal-read rows for PostgreSQL, SQL Server, MySQL, and Oracle with iterations 1 and provider-specific read strategies in executionDetail.
- Evidence: git show 19f419e88241:.gitignore contains allow-list entries for exactly the two new v0.32.0 artifact directories and their benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json files.
- 45 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator.
- Use the persisted v0.32.0 scale bundle, the v0.32.0 smoke-read bundle, and the Developer Evidence Capture ticket comment as the review surfaces for downstream tuning and release-note consumers.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8187`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `f0dfe45d74ac47e1a8ab40ff5fcd123d`
- completed-at-utc: `<redacted>-07T12:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9XD26D2MHVAKZ2GCZ67BEFC/runs/20260607T122837182Z-f0dfe45d74ac47e1a8ab40ff5fcd123d.json`