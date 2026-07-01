[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FH8RATZGZRVAJVC4ERV0ACYW-task-refresh-provider-benchmark-gap-matrix-and-c' for ticket '06FH8RATZGZRVAJVC4ERV0ACYW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RATZGZRVAJVC4ERV0ACYW`.
- Optimistic claim succeeded (`expectedRevision=06FHPAR836HAEAW03SESYGXPFW`, `currentRevision=06FHPB4SHH8D24AYRAR22DT82C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FH8RATZGZRVAJVC4ERV0ACYW-task-refresh-provider-benchmark-gap-matrix-and-c' and commit '1d09d41306ef' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FH8RATZGZRVAJVC4ERV0ACYW-task-refresh-provider-benchmark-gap-matrix-and-c' from source '1d09d41306ef'.
- Interactive tester tool loop completed review for branch 'ticket/06FH8RATZGZRVAJVC4ERV0ACYW-task-refresh-provider-benchmark-gap-matrix-and-c'.
- Evidence: git rev-parse --abbrev-ref HEAD returned ticket/06FH8RATZGZRVAJVC4ERV0ACYW-task-refresh-provider-benchmark-gap-matrix-and-c, and git rev-parse HEAD returned aa317e4e605711eebab5f457d3815cc49283790d.
- Evidence: git diff --stat develop...HEAD over .gicket/tickets/06FH8RATZGZRVAJVC4ERV0ACYW, docs/plans/provider-optimization-gap-matrix.md, docs/plans/provider-optimization-evidence-matrix.md, and artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-2026...
- Evidence: git diff --unified=0 develop...HEAD -- .gicket/tickets/06FH8RATZGZRVAJVC4ERV0ACYW/description.md showed the legacy rerun request replaced by the structured delivery contract that names docs/plans/provider-optimization-gap-matrix.md, docs/plans/provider-optimization-e...
- Evidence: git ls-files confirmed docs/plans/provider-optimization-gap-matrix.md, docs/plans/provider-optimization-evidence-matrix.md, artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/README.md, and .gicket/tickets/06FH8RATZGZRVAJVC4ERV0ACY...
- Evidence: git ls-files artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted> listed README.md plus benchmark-summary.md/.csv/.json triplets for postgres-podman-live, sqlserver-live, mysql-live, oracle-lob-prefetch, and db2-rowcap-1000.
- Evidence: artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/README.md records completed save/latest/PIT/bridge timings for PostgreSQL, SQL Server, MySQL, Oracle, and DB2, including DB2 save 101.037 ms / latest 14.615 ms / PIT 27.207 ms / br...
- 61 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Proceed to integrator.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.5406`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `445631e658b148e18a74c3c39e19823f`
- completed-at-utc: `<redacted>-01T01:01:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RATZGZRVAJVC4ERV0ACYW/runs/20260701T010142392Z-445631e658b148e18a74c3c39e19823f.json`