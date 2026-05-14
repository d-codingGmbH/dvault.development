[gicket-bot] PO-critic review contract

Summary
- Ticket 06F1XQ25KK4VY4MYJSDG9V4BZM is ready for developer handoff; the delivery contract is specific, locally evidenced, and has no unresolved Open Questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XQ25KK4VY4MYJSDG9V4BZM/description.md contains a gicket-bot human-ticket-refinement-contract with PO Handoff decision ready_for_po_critic and ## Open Questions set to none.
- Ticket comment 06F2HEG5EEEVZET4Z3WS38XY78.md records the PO refinement contract, including PostgreSQL as the first fixture, DVAULT_TEST_POSTGRES_CONNECTION_STRING, Podman/Docker docs, and no new child tickets or relation changes.
- Ticket comments 06F2HFN4AJQRVNZKEF30TDH82M.md and 06F2HFNBZZ7WF1E793T3M3R538.md are po-critic claim/lease metadata only; no later scope-changing comment was observed.
- Branch history shows commit 572a7739a04d handoff po->po-critic and b03576728d53 lease claim po-critic on ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample.
- git diff 67f54b6168949878ad8dfeb9abc359f7f49081a1..572a7739a04d for this ticket changed only .gicket ticket description, comments, events, and ticket.json files.
- Relation file 06F1XQ1VWEX0WPAXE78FHSWJ8G--06F1XQ25KK4VY4MYJSDG9V4BZM--parentOf.json shows this task is under parent story 06F1XQ1VWEX0WPAXE78FHSWJ8G.
- examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs uses DVAULT_TEST_POSTGRES_CONNECTION_STRING, AddDVaultPostgres(), UseNpgsql(connectionString), and UseDataVaultMetadata(), and exits successfully with a skip message when the env var is missing.
- examples/README.md documents the PostgreSQL quickstart path and says DCoding.Data.DVault.PostgresQuickstart uses AddDVaultPostgres() with a developer-managed connection string.
- README.md documents opt-in local Postgres integration tests with Category=ProviderIntegration.ExternalOptIn, Provider=Postgres, and -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured, while stating normal dotnet test does not require Postgres or Docker.
- tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs defines DVAULT_TEST_POSTGRES_CONNECTION_STRING and a skip message that says Docker and database provisioning are external to DVault.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj conditionally restores Npgsql.EntityFrameworkCore.PostgreSQL only when $(DVAULT_TEST_POSTGRES_CONNECTION_STRING) is non-empty.
- tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs defines ProviderIntegration.ExternalOptIn and Postgres provider trait values.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs exposes public AddDVaultPostgres(IServiceCollection) and registers PostgreSQL provider behavior/save strategy.
- docs/releases/v0.6.0.md records the visible PostgreSQL validation baseline as docker.io/postgres:18 with Npgsql.EntityFrameworkCore.PostgreSQL 10.0.1.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The docker.io/postgres:18 image baseline is local release-document evidence; implementation should avoid implying DVault owns image lifecycle or container provisioning.
- Podman and Docker networking can differ by host, so the sample must make connection-string overrides visible rather than assuming localhost always works.

AC / test suggestions
- Developer validation should either run the documented Postgres command with Category=ProviderIntegration.ExternalOptIn&Provider=Postgres and -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured or record the explicit missing-runtime/missing-configuration behavior.
- Confirm the sample also exercises or links to examples/DCoding.Data.DVault.PostgresQuickstart so the same connection string validates the runnable quickstart path.
- Confirm default local test guidance remains clear that external databases, Docker, and Podman are not required.

Implementation watchouts
- Use DVAULT_TEST_POSTGRES_CONNECTION_STRING exactly; do not add a parallel environment variable for the first sample.
- Keep credentials placeholder-only and local, with no checked-in secrets or machine-specific connection strings.
- Include required database/user privileges for temporary schema creation and cleanup, matching PostgresDataVaultSchemaTests behavior.
- Keep the conditional provider package restore marker -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured aligned with the integration test project.
- If pinning a container image, keep docker.io/postgres:18 explicit and documented as the current repo baseline.
- Do not introduce mandatory CI container startup, default test-suite dependency on Docker/Podman, or mandatory provider package restore beyond the existing conditional behavior.

Non-blocking notes
- The incoming blocks are from done tickets and are historical/non-blocking under the prompt rules.
- No new split is needed for developer handoff.

Split recommendations
- No split recommended; the ticket is already scoped as the first PostgreSQL provider-container fixture sample while the wider provider matrix remains out of scope.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment