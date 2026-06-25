[gicket-bot] PO refinement contract

Summary
- Refinement confirms the runnable PostgreSQL quickstart baseline already exists in the repository; the remaining work is a bounded docs-only parity pass that adds concise PostgreSQL setup notes to broader quickstart surfaces, aligns any touched package/version text with the current 8.47.0 and 10.47.0 consumer lines, and keeps external database setup opt-in.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already defines the PostgreSQL quickstart shape in examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs: AddDVault with UseBinaryFirstProfile, AddDVaultPostgres(), UseNpgsql(connectionString), and the existing metadata-backed quickstart flow.
- The exact opt-in gate already used by the runnable quickstart and external-provider tests is DVAULT_TEST_POSTGRES_CONNECTION_STRING; a missing value is a successful skip path for the PostgreSQL example, not a default validation failure.
- The current package-line authority is the root README installation guidance with 8.47.0 for net8.0 and 10.47.0 for net10.0; touched quickstart docs must stay aligned with that baseline.
- This ticket stays documentation-only. No new container provisioning, deployment automation, provider runtime changes, or database lifecycle tooling is needed.

Scope In
- Adopter-facing quickstart documentation updates where the current setup story is SQLite-first or provider-neutral and needs a concise PostgreSQL parity note.
- Concise PostgreSQL install/setup guidance that names the DVault provider package DCoding.Data.DVault.Postgres and the normal EF Core PostgreSQL provider package Npgsql.EntityFrameworkCore.PostgreSQL.
- Provider registration guidance that mirrors the existing repository baseline by calling out AddDVaultPostgres() alongside the binary-first new-project posture and UseNpgsql(connectionString).
- A short opt-in note that points readers to the existing DVAULT_TEST_POSTGRES_CONNECTION_STRING quickstart/test flow and existing repository docs for local validation or fixture details.
- Alignment of any touched quickstart package-version commands or copied install text with the current 8.47.0 and 10.47.0 consumer package lines.

Scope Out
- New product code, provider behavior changes, or new runnable example projects.
- Docker or Podman automation, database provisioning, credential management, deployment automation, CI orchestration, or hosted infrastructure guidance.
- Broader provider performance, PIT/bridge, or benchmark-evidence documentation work outside the bounded quickstart/setup note.
- Changing the default local validation contract so PostgreSQL becomes required for ordinary build or test runs.

Open questions
- none

Follow-up questions
- Should a later docs-maintenance ticket collapse repeated package-version blocks across README, examples, and release-aligned docs into a more canonical install surface to reduce future version drift?

Risks
- Quickstart docs currently duplicate install/version text across multiple files; if touched surfaces are not aligned in this ticket, stale package numbers can remain visible to adopters.
- If the PostgreSQL parity note drifts from the runnable example or local-validation command, readers may incorrectly infer that external PostgreSQL setup is required for default DVault validation.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment