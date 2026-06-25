[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FF43T2EK3CBYHTR287YWC5NR-task-add-postgresql-binary-first-provider-quicks' for ticket '06FF43T2EK3CBYHTR287YWC5NR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43T2EK3CBYHTR287YWC5NR`.
- Optimistic claim succeeded (`expectedRevision=06FFX0RACAXR52RCFHZRT5VH6W`, `currentRevision=06FFXES9R2XTY02A3C73NDAT2G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FF43T2EK3CBYHTR287YWC5NR-task-add-postgresql-binary-first-provider-quicks' and commit 'febcdbea8958' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FF43T2EK3CBYHTR287YWC5NR-task-add-postgresql-binary-first-provider-quicks' from source 'febcdbea8958'.
- Interactive tester tool loop completed review for branch 'ticket/06FF43T2EK3CBYHTR287YWC5NR-task-add-postgresql-binary-first-provider-quicks'.
- Evidence: `git diff --name-only develop...febcdbea8958` shows repo changes only in `README.md`, `docs/getting-started.md`, and `examples/README.md` plus `.gicket` ticket metadata files.
- Evidence: `README.md:86-96` adds a PostgreSQL parity note with `DCoding.Data.DVault.Postgres`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `AddDVaultPostgres()`, `UseNpgsql(connectionString)`, `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, and the no-provisioning boundary.
- Evidence: `docs/getting-started.md:27-37` adds the same opt-in PostgreSQL setup note and points to `examples/DCoding.Data.DVault.PostgresQuickstart/README.md` and `docs/local-validation.md#postgresql`.
- Evidence: `examples/README.md:29-59` now uses `8.47.0` and `10.47.0` across the touched package/version blocks and explicitly names the PostgreSQL provider packages.
- Evidence: `examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs:6-24`, `examples/DCoding.Data.DVault.PostgresQuickstart/README.md:53-79`, and `docs/local-validation.md:41-71` already contain the referenced environment variable, provider registration shape, opt-in test fl...
- Evidence: `src/DCoding.Data.DVault.Postgres/DCoding.Data.DVault.Postgres.csproj:8`, `examples/DCoding.Data.DVault.PostgresQuickstart/DCoding.Data.DVault.PostgresQuickstart.csproj:11`, and `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15` confir...
- 41 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Proceed to integrator review.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8347`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `9e35043b249a4aac84c16f779c5083dc`
- completed-at-utc: `<redacted>-25T12:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43T2EK3CBYHTR287YWC5NR/runs/20260625T122842943Z-9e35043b249a4aac84c16f779c5083dc.json`