[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FE4R1N2ADN77NDFDP4GR7020-task-add-provider-binary-vs-hex-benchmark-matrix' for ticket '06FE4R1N2ADN77NDFDP4GR7020'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4R1N2ADN77NDFDP4GR7020`.
- Optimistic claim succeeded (`expectedRevision=06FEP9ABENY168TYRFDT62HCGW`, `currentRevision=06FEP9JB6KRVQEB1RAREY1BXSC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FE4R1N2ADN77NDFDP4GR7020-task-add-provider-binary-vs-hex-benchmark-matrix' and commit '5b14571cd850' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FE4R1N2ADN77NDFDP4GR7020-task-add-provider-binary-vs-hex-benchmark-matrix' from source '5b14571cd850'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Repository review found the required provider hash-key matrix bundle, root footprint sidecars, and documentation alignment, but the claimed commit also updates benchmark and local-validation ...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FE4R1N2ADN77NDFDP4GR7020-task-add-provider-binary-vs-hex-benchmark-matrix'.
- Checked out verification commit '5b14571cd850'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Expanded committed repository inspection with 10 branch-delta path(s) beyond the 6 ticket-declared path(s).
- Inspected committed repository state for 16 repository path(s) at commit '5b14571cd850'.
- 303 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to the integrator gate using commit 5b14571cd850 and the checked-in bundle at artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-<redacted>/ as the verified evidence set.
- Keep SQL Server skipped rows and failed PostgreSQL/MySQL/Oracle/DB2 binary rows framed as caveats only if the integrator or release notes cite this benchmark evidence.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.6889`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `5fd4600fc0ff402d9bd093112142443f`
- completed-at-utc: `<redacted>-21T17:17:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4R1N2ADN77NDFDP4GR7020/runs/20260621T171734138Z-5fd4600fc0ff402d9bd093112142443f.json`