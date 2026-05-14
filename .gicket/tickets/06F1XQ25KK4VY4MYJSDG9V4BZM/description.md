<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket against local repository evidence. PostgreSQL is the v1 first provider-container fixture sample, aligned to the existing Postgres quickstart, opt-in integration tests, and DVAULT_TEST_POSTGRES_CONNECTION_STRING. No child tickets, planning documents, attachments, or relation changes were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use PostgreSQL for the first provider container fixture sample. Repository evidence already has a PostgreSQL quickstart, Postgres external opt-in integration tests, AddDVaultPostgres coverage, and README commands for DVAULT_TEST_POSTGRES_CONNECTION_STRING.
- SQLite remains the required local no-container baseline; it is not the target for this container fixture sample.
- The sample should provide developer-managed Podman/Docker setup guidance and hand off a normal connection string to existing DVault tests/examples. It must not make container startup mandatory for default test runs or CI.
- The visible container image baseline is docker.io/postgres:18 from repository release documentation; if implementation pins an image, start from that checked-in baseline and keep the tag explicit in the sample.
- Current ticket comments are bot claim/lease metadata only and do not alter product scope.
- Live relation state was verified: incoming parentOf from 06F1XQ1VWEX0WPAXE78FHSWJ8G, incoming blocks from done tickets 06F1XQ03MADSPQD0AJN6R50D44 and 06F1XPX99KQRB09GRQG50Z75FM. The done-source blocks are historical/non-blocking under the refinement rules and were left unchanged.

### Scope In
- Add a runnable PostgreSQL provider container fixture sample for local development.
- Expose a DVAULT_TEST_POSTGRES_CONNECTION_STRING value compatible with PostgresIntegrationTestConfiguration and the PostgreSQL quickstart.
- Document Podman and Docker expectations, including image/tag, port, database, user, password placeholder, cleanup, and required schema/table privileges.
- Show the existing repo-root command for running Postgres external opt-in tests with the configured connection string and MSBuild marker property.
- Keep missing runtime, missing image, missing connection string, or unreachable database outcomes explicit and developer-readable.
- Shape the sample so later MySQL, SQL Server, and Oracle fixtures can reuse the same documentation pattern.

### Scope Out
- No full provider matrix in this task.
- No mandatory CI container startup or default test-suite dependency on Docker or Podman.
- No checked-in secrets, machine-specific connection strings, or bundled database images.
- No replacement of existing SQLite required-local coverage.
- No broad Testcontainers helper framework for every provider; that remains parent/helper-story scope.
- No benchmark container orchestration or provider-performance certification expansion.

## Acceptance Criteria
- A PostgreSQL container fixture sample is checked in under the existing docs/examples conventions and can be followed locally with Podman or Docker.
- The sample documents an exact DVAULT_TEST_POSTGRES_CONNECTION_STRING compatible with existing Postgres integration tests and examples, using placeholder credentials only.
- The sample includes the repo-root Postgres test command using Category=ProviderIntegration.ExternalOptIn and Provider=Postgres, plus the non-secret MSBuild marker property required for conditional provider package restore in the integration test project.
- The sample either links to or demonstrates the existing PostgreSQL quickstart path so the same connection string can exercise a runnable example.
- When the container runtime, image, configuration, or database is unavailable, the expected failure or skip behavior is explicit and does not break default local test execution.
- The reusable pattern names the lifecycle steps future provider fixtures need: start, configure connection string, run targeted validation, inspect skip/failure output, and clean up.

## Definition of Done
- The checked-in sample contains no real credentials and keeps all local secrets in environment variables or local-only command input.
- README or examples documentation links to the PostgreSQL fixture sample without weakening the existing statement that default tests do not require external databases or Docker/Podman.
- Existing Postgres configuration tests and provider category conventions remain aligned with the documented environment variable and skip message contract.
- The sample is validated by running the documented commands, or by recording the explicit missing-runtime/missing-configuration behavior when a container runtime is not available locally.
- No source changes introduce mandatory provider package restore for default test runs beyond the existing conditional integration-test behavior.

## Implementation Notes
- Prefer PostgreSQL because the repository already has examples/DCoding.Data.DVault.PostgresQuickstart, PostgresDataVaultSchemaTests, PostgresOptimizedDataVaultSaveServiceTests, and README opt-in Postgres commands.
- Use DVAULT_TEST_POSTGRES_CONNECTION_STRING exactly; do not introduce a parallel environment variable for the first sample.
- For integration tests, keep the existing pattern of setting the environment variable and passing -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured so Npgsql.EntityFrameworkCore.PostgreSQL is restored for that run.
- Document that the configured database/user must allow temporary schema creation and cleanup, matching current Postgres integration-test behavior.
- No planning document, attachment, child ticket, or relation mutation was materialized during this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- After the PostgreSQL sample lands, should the parent helper story prioritize MySQL, SQL Server, or Oracle as the next provider fixture?
- Should a later task convert the docs-only sample into reusable Testcontainers-based test helpers, or keep provider startup as documented local commands?
- Should future provider fixture samples share one consolidated local-provider guide once at least two external providers have working samples?

## Risks
- Container guidance can drift from README if it implies DVault provisions databases by default; documentation must preserve the opt-in boundary.
- Hardcoded ports or credentials can conflict with developer machines; the sample should make overrides and cleanup clear.
- Podman and Docker networking differ on some hosts, so the sample should call out connection-string adjustment rather than hiding runtime-specific assumptions.

## Split Recommendations
- No new split is recommended. This task is already the bounded first-provider sample under parent story 06F1XQ1VWEX0WPAXE78FHSWJ8G, while the full provider matrix remains out of scope.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Create the first provider container fixture sample and connect it to DVault integration-test conventions.

## Scope In

- Choose one reliable provider as the first fixture.
- Expose a connection string compatible with existing tests/examples.
- Document Podman/Docker command expectations.

## Scope Out

- No full provider matrix in this task.
- No mandatory CI container startup.

## Acceptance Criteria

- The sample can be run locally.
- Failure/skips are explicit when runtime is missing.
- The pattern is reusable for later providers.

## Implementation Notes

- Start with the provider most reliable in local development.

## Open Questions

- none