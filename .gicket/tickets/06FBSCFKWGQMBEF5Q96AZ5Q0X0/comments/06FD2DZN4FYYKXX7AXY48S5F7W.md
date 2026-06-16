[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FBSCFKWGQMBEF5Q96AZ5Q0X0-task-close-sql-server-latest-satellite-read-gap' for ticket '06FBSCFKWGQMBEF5Q96AZ5Q0X0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCFKWGQMBEF5Q96AZ5Q0X0`.
- Optimistic claim succeeded (`expectedRevision=06FD266DK395SAG8G7QD5CG40M`, `currentRevision=06FD28XE6QZ3FHEVZ6P73PTCB8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FBSCFKWGQMBEF5Q96AZ5Q0X0-task-close-sql-server-latest-satellite-read-gap' and commit '4b71d3695e72' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FBSCFKWGQMBEF5Q96AZ5Q0X0-task-close-sql-server-latest-satellite-read-gap' from source '4b71d3695e72'.
- Prompt-injection writeback protection rejected tester interactive assessment writeback on attempt 1/2; retrying tester output generation before creating a durable stop.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Repository inspection supports the claimed SQL Server latest-satellite implementation and the related benchmark/document updates, but final tester signoff still requires deterministic host-si...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FBSCFKWGQMBEF5Q96AZ5Q0X0-task-close-sql-server-latest-satellite-read-gap'.
- Checked out verification commit '4b71d3695e72'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Expanded committed repository inspection with 11 branch-delta path(s) beyond the 1 ticket-declared path(s).
- 282 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off commit 4b71d3695e72 to integrator for final acceptance.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8020`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `a64538d39fff4613af22cb71e15b3b1f`
- completed-at-utc: `<redacted>-16T16:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCFKWGQMBEF5Q96AZ5Q0X0/runs/20260616T161605787Z-a64538d39fff4613af22cb71e15b3b1f.json`