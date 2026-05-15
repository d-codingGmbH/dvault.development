[gicket-bot] PO-critic review contract

Summary
- PO-critic approves ticket 06F1XQ1VWEX0WPAXE78FHSWJ8G for developer handoff; the persisted contract is bounded to the PostgreSQL first-provider fixture pattern, has Open Questions set to none, and is supported by repository and child-ticket evidence.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XQ1VWEX0WPAXE78FHSWJ8G/description.md contains the delivery contract with PO Handoff decision ready_for_po_critic and ## Open Questions set to none.
- .gicket/relations/8G/ZM/06F1XQ1VWEX0WPAXE78FHSWJ8G--06F1XQ25KK4VY4MYJSDG9V4BZM--parentOf.json shows parent story 06F1XQ1VWEX0WPAXE78FHSWJ8G owns child task 06F1XQ25KK4VY4MYJSDG9V4BZM.
- Child tester handoff comment 06F2HQFHTY5VSJNBJ4QYTB34SM.md records 6/6 acceptance criteria and 5/5 DoD verified at commit e3a50b2e61b0 on branch ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample.
- git merge-base HEAD develop is 8d4e86bd5, matching develop, and HEAD is 71bce0fd6 on ticket/06F1XQ1VWEX0WPAXE78FHSWJ8G-story-add-testcontainers-integration-helpers-and.
- git show --name-status e3a50b2e61b0 shows the child implementation added examples/DCoding.Data.DVault.PostgresQuickstart/README.md and modified examples/README.md.
- examples/README.md lines 5-8 identify SQLite as no external infrastructure and PostgreSQL as developer-managed through AddDVaultPostgres and UseDataVaultMetadata.
- examples/README.md lines 28-36 document DVAULT_TEST_POSTGRES_CONNECTION_STRING, the Postgres quickstart command, and the opt-in fixture link while stating default dotnet test does not require PostgreSQL, Docker, or Podman.
- examples/DCoding.Data.DVault.PostgresQuickstart/README.md lines 5-11 document docker.io/postgres:18, DVAULT_TEST_POSTGRES_CONNECTION_STRING, and placeholder/local-only password handling.
- examples/DCoding.Data.DVault.PostgresQuickstart/README.md lines 28-49 provide Podman and Docker commands plus the exported connection string using DVAULT_POSTGRES_PASSWORD.
- examples/DCoding.Data.DVault.PostgresQuickstart/README.md lines 63-71 document the repo-root opt-in test command with Category=ProviderIntegration.ExternalOptIn, Provider=Postgres, and -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured.
- examples/DCoding.Data.DVault.PostgresQuickstart/README.md lines 73-101 document missing runtime/image/configuration, unreachable database, insufficient privileges, cleanup, and the reusable provider fixture lifecycle.
- examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs lines 6-13 use DVAULT_TEST_POSTGRES_CONNECTION_STRING and exit successfully with the documented skip message when missing.
- examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs lines 16-22 call AddDVault, AddDVaultPostgres, UseNpgsql(connectionString), and UseDataVaultMetadata.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs lines 15-25 directly define the public AddDVaultPostgres extension and register Postgres provider behavior/save strategy.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj lines 15-20 conditionally restores Npgsql.EntityFrameworkCore.PostgreSQL only when $(DVAULT_TEST_POSTGRES_CONNECTION_STRING) is non-empty.
- tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs lines 3-7 define DVAULT_TEST_POSTGRES_CONNECTION_STRING and a skip message stating Docker and database provisioning are external to DVault.
- tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs lines 7-12 define ProviderIntegration.ExternalOptIn and Postgres provider trait values matching the documented filter.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract intentionally treats PostgreSQL as the first external fixture and defers SQL Server, MySQL, and Oracle; keep that boundary during development because the legacy title can imply a broader provider matrix.
- The documentation relies on docker.io/postgres:18 being the approved visible baseline; if the image/tag changes, update the ticket or implementation evidence rather than silently substituting it.
- Local Podman/Docker networking and hardcoded host ports can vary by machine, so developer validation should preserve the hostname and port override guidance.

AC / test suggestions
- Developer/test handoff should verify default dotnet test remains free of external database/container requirements.
- For opt-in validation, use the documented command: dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=Postgres" -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured.
- If a local runtime is unavailable, record the explicit missing-runtime or missing-configuration behavior rather than widening the story scope.

Implementation watchouts
- Use DVAULT_TEST_POSTGRES_CONNECTION_STRING exactly; do not introduce a parallel connection-string variable.
- Keep credentials placeholder/local-only and avoid checked-in machine-specific connection strings.
- Preserve conditional provider package restore via the non-secret MSBuild marker property; default test runs must not restore provider packages solely for external integrations.
- Do not add CI provider matrix work, Testcontainers abstraction work, or additional provider fixtures under this story unless a separate ticket is created.

Non-blocking notes
- The parent story mostly ratifies and carries forward the completed child fixture sample, so developer work may be review/verification-oriented rather than a large code change.
- Persisted current comments include bot run and lease metadata even though the prompt snapshot listed no recent comments; none of the observed comments reopened scope or introduced unresolved questions.

Split recommendations
- No new split is required now; the done child task 06F1XQ25KK4VY4MYJSDG9V4BZM already covers the first PostgreSQL provider fixture sample.
- If product later wants a full provider fixture matrix, split MySQL, SQL Server, and Oracle into provider-specific tickets because images, licensing, authentication, and privilege setup differ.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment