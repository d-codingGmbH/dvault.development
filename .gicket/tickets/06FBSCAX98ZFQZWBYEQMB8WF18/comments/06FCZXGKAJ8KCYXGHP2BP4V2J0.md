[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FBSCAX98ZFQZWBYEQMB8WF18-task-document-provider-bulk-outcomes-and-benchma' for ticket '06FBSCAX98ZFQZWBYEQMB8WF18'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCAX98ZFQZWBYEQMB8WF18`.
- Optimistic claim succeeded (`expectedRevision=06FCZVJT1DGZVPNCHD54S4MFN0`, `currentRevision=06FCZVS5Y9KEBBEPPGMYRPA58C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FBSCAX98ZFQZWBYEQMB8WF18-task-document-provider-bulk-outcomes-and-benchma' and commit '83c9266ee4e1' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FBSCAX98ZFQZWBYEQMB8WF18-task-document-provider-bulk-outcomes-and-benchma' from source '83c9266ee4e1'.
- Interactive tester tool loop completed review for branch 'ticket/06FBSCAX98ZFQZWBYEQMB8WF18-task-document-provider-bulk-outcomes-and-benchma'.
- Evidence: git merge-base develop 83c9266ee4e1 and git rev-list --max-count=1 develop both returned fd78c2fee3e50fdac2a097f74cce86dbad96a08d.
- Evidence: git diff --name-only develop..83c9266ee4e1 -- . ':(exclude).gicket' returned no output, so the claimed commit introduces no repository changes outside .gicket metadata.
- Evidence: git ls-files confirmed the required output paths exist: README.md, CHANGELOG.md, docs/performance-profiles.md, and docs/releases/v0.39.0.md.
- Evidence: README.md routes performance guidance to docs/performance-profiles.md and benchmarks/ and states live PostgreSQL, SQL Server, Oracle, MySQL, and DB2 validation is opt-in behind DVAULT_TEST_* connection strings.
- Evidence: docs/performance-profiles.md uses the Provider Optimization Evidence Matrix and Gap Matrix as canonical surfaces, calls benchmark-summary.md/benchmark-summary.csv/benchmark-summary.json the quick local SQLite and skipped-provider baseline, and preserves provider-neut...
- Evidence: docs/releases/v0.39.0.md defines completed-timing vs skipped-placeholder postures, says skipped optional-provider rows are not completed timing evidence, and sends external-provider claims to linked provider-threshold bundles with preserved run context.
- 61 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Proceed to integrator.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9019`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `1c010cd8f7f74ba7963ef2e58784e5c2`
- completed-at-utc: `<redacted>-16T10:24:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCAX98ZFQZWBYEQMB8WF18/runs/20260616T102430926Z-1c010cd8f7f74ba7963ef2e58784e5c2.json`