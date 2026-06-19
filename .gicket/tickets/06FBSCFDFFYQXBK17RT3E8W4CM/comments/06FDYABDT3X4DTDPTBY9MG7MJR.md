[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap' for ticket '06FBSCFDFFYQXBK17RT3E8W4CM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCFDFFYQXBK17RT3E8W4CM`.
- Optimistic claim succeeded (`expectedRevision=06FDVPGYNJM4W2G35BDX6HW1EM`, `currentRevision=06FDY79P4HWG7XS78N99MWHFK8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap' and commit '993c587b8f1c' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap' from source '993c587b8f1c'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Commit 993c587b8f1c contains coherent repository evidence for the implemented PostgreSQL latest-satellite lane: AddDVaultPostgres() registers PostgresDataVaultReadStrategy for IDataVaultProvi...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap'.
- Checked out verification commit '993c587b8f1c'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 12 repository path(s) at commit '993c587b8f1c'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 280 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator using branch ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap at commit 993c587b8f1c.
- Keep any later PostgreSQL latest-satellite timing claim gated on a provider-configured completed benchmark row, since the current root PostgreSQL benchmark row remains a skipped-placeholder.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.6965`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `ff49221ce4e348b5bdf065583699e450`
- completed-at-utc: `<redacted>-19T09:14:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCFDFFYQXBK17RT3E8W4CM/runs/20260619T091454789Z-ff49221ce4e348b5bdf065583699e450.json`