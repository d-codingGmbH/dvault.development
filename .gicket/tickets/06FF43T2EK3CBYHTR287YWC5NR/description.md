<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement confirms the runnable PostgreSQL quickstart baseline already exists in the repository; the remaining work is a bounded docs-only parity pass that adds concise PostgreSQL setup notes to broader quickstart surfaces, aligns any touched package/version text with the current 8.47.0 and 10.47.0 consumer lines, and keeps external database setup opt-in.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already defines the PostgreSQL quickstart shape in examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs: AddDVault with UseBinaryFirstProfile, AddDVaultPostgres(), UseNpgsql(connectionString), and the existing metadata-backed quickstart flow.
- The exact opt-in gate already used by the runnable quickstart and external-provider tests is DVAULT_TEST_POSTGRES_CONNECTION_STRING; a missing value is a successful skip path for the PostgreSQL example, not a default validation failure.
- The current package-line authority is the root README installation guidance with 8.47.0 for net8.0 and 10.47.0 for net10.0; touched quickstart docs must stay aligned with that baseline.
- This ticket stays documentation-only. No new container provisioning, deployment automation, provider runtime changes, or database lifecycle tooling is needed.

### Scope In
- Adopter-facing quickstart documentation updates where the current setup story is SQLite-first or provider-neutral and needs a concise PostgreSQL parity note.
- Concise PostgreSQL install/setup guidance that names the DVault provider package DCoding.Data.DVault.Postgres and the normal EF Core PostgreSQL provider package Npgsql.EntityFrameworkCore.PostgreSQL.
- Provider registration guidance that mirrors the existing repository baseline by calling out AddDVaultPostgres() alongside the binary-first new-project posture and UseNpgsql(connectionString).
- A short opt-in note that points readers to the existing DVAULT_TEST_POSTGRES_CONNECTION_STRING quickstart/test flow and existing repository docs for local validation or fixture details.
- Alignment of any touched quickstart package-version commands or copied install text with the current 8.47.0 and 10.47.0 consumer package lines.

### Scope Out
- New product code, provider behavior changes, or new runnable example projects.
- Docker or Podman automation, database provisioning, credential management, deployment automation, CI orchestration, or hosted infrastructure guidance.
- Broader provider performance, PIT/bridge, or benchmark-evidence documentation work outside the bounded quickstart/setup note.
- Changing the default local validation contract so PostgreSQL becomes required for ordinary build or test runs.

## Acceptance Criteria
- At least one primary quickstart surface that currently reads as SQLite-only or provider-neutral now includes a concise PostgreSQL parity note that keeps binary-first as the recommended new-project posture.
- The updated guidance explicitly identifies the PostgreSQL DVault package and describes the matching provider registration path with AddDVaultPostgres() and UseNpgsql(connectionString), rather than implying provider-neutral registration alone is sufficient.
- The updated guidance explains that repository PostgreSQL quickstart and live-provider test execution are opt-in behind DVAULT_TEST_POSTGRES_CONNECTION_STRING and routes readers to existing local-validation or PostgreSQL quickstart docs instead of introducing new provisioning instructions.
- Any touched install commands or versioned package blocks use the current repository-visible consumer lines 8.47.0 and 10.47.0.
- The updated docs continue to state or clearly preserve the boundary that DVault does not provision PostgreSQL containers, databases, credentials, or deployment infrastructure.

## Definition of Done
- The targeted documentation files are updated and internally consistent with the existing repository quickstart, local-validation, and installation surfaces.
- Referenced commands, environment variables, package ids, and example paths all exist in the repository and match current names.
- The PostgreSQL note stays concise and reuses existing fixture/local-validation surfaces by reference instead of duplicating a full container lifecycle walkthrough.
- No product-code files, provider runtime behavior, or test automation surfaces are changed for this ticket.

## Implementation Notes
- Use examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs as the authoritative runnable baseline for terminology and setup shape instead of inventing a different PostgreSQL registration flow.
- Keep the docs explicit that consumer applications still need the ordinary Npgsql EF Core provider package in addition to DCoding.Data.DVault.Postgres.
- If examples/README.md is touched, correct or replace its stale 8.45.0 and 10.45.0 package-line text so the quickstart surfaces do not drift from the current 8.47.0 and 10.47.0 baseline.
- Preserve the developer-managed database boundary already documented in examples/DCoding.Data.DVault.PostgresQuickstart/README.md and docs/local-validation.md: the connection string gate and non-secret MSBuild marker are opt-in local setup details, not default validation requirements.
- Do not expand this ticket into provider-optimization claims, container tutorials, or deployment instructions; this is quickstart parity guidance only.

## Open Questions
- none

## Follow-Up Questions
- Should a later docs-maintenance ticket collapse repeated package-version blocks across README, examples, and release-aligned docs into a more canonical install surface to reduce future version drift?

## Risks
- Quickstart docs currently duplicate install/version text across multiple files; if touched surfaces are not aligned in this ticket, stale package numbers can remain visible to adopters.
- If the PostgreSQL parity note drifts from the runnable example or local-validation command, readers may incorrectly infer that external PostgreSQL setup is required for default DVault validation.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add concise PostgreSQL setup guidance aligned with binary-first profile, package lines, provider registration, and opt-in connection-string testing. Acceptance: no container provisioning or deployment automation is introduced.