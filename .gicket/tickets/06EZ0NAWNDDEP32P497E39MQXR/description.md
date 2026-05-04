<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket by making its dependency on 06EZ0NAMGKJ63WCXAK1J7B08TR explicit in contract text, moving SQL Server to `ProviderIntegration.ExternalOptIn`, and pinning the SQL Server opt-in env var, command, and source-of-truth updates.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket depends on 06EZ0NAMGKJ63WCXAK1J7B08TR for SQL Server strategy implementation and validates that optimized path after the sibling lands; it does not own strategy implementation itself.
- SQL Server joins `ProviderIntegration.ExternalOptIn`, represented in the integration suite by `ProviderTestCategories.ExternalProviderIntegration` plus `ProviderTestCategories.SqlServerProvider`.
- The opt-in environment variable for this ticket is `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`.
- The representative repo-root validation command is `dotnet test DVault.slnx --filter FullyQualifiedName~SqlServer`.
- Required source-of-truth updates for this ticket are `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs`.

### Scope In
- Add a SQL Server integration-test configuration helper and usage pattern inside `tests/DCoding.Data.DVault.Tests/Integration` using environment-backed discovery and deterministic skip behavior.
- Document the required local SQL Server configuration, the `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` contract, and the representative repo-root run command in `README.md`.
- Add SQL Server smoke coverage for one representative hub save, one link save, and one satellite save against the optimized SQL Server save path supplied by 06EZ0NAMGKJ63WCXAK1J7B08TR.
- Update `docs/architecture/dvault-v1-explicit-save-service.md` and `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` so SQL Server is classified as external opt-in coverage and stays out of default smoke runs.

### Scope Out
- Implementing the SQL Server provider save strategy itself; that remains with 06EZ0NAMGKJ63WCXAK1J7B08TR.
- Always-on CI or mandatory local SQL Server setup for default test runs.
- Broader SQL Server coverage such as batching, concurrency, retry, duplicate reuse, or performance validation beyond the three representative smoke scenarios.
- Provider-neutral dispatcher changes or provider-name branches in core code.

## Acceptance Criteria
- An opt-in SQL Server integration configuration exists in `tests/DCoding.Data.DVault.Tests/Integration`, sourced from `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, and missing configuration yields a deterministic skip message.
- The documented SQL Server lane runs from the repo root with `dotnet test DVault.slnx --filter FullyQualifiedName~SqlServer`, stays out of default runs, and mirrors the Postgres conditional provider-loading pattern so default executions stay clean.
- One representative hub, link, and satellite explicit-save scenario exercises the SQL Server optimized save path delivered by 06EZ0NAMGKJ63WCXAK1J7B08TR rather than the provider-neutral fallback.
- `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` explicitly classify SQL Server as `ProviderIntegration.ExternalOptIn` and align the discovery and documentation baseline with the new lane.

## Definition of Done
- The SQL Server configuration helper and focused configured-versus-unconfigured coverage are added alongside the existing integration-test support code.
- The targeted SQL Server smoke tests pass when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is supplied and skip cleanly with the deterministic missing-configuration message when it is absent.
- `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` is updated so the discovered type list and trait assertions cover the new SQL Server classes and keep them out of default smoke coverage.
- The relevant documentation updates land in `README.md` and `docs/architecture/dvault-v1-explicit-save-service.md`, and the shared formatting gate plus the documented targeted test command remain green.

## Implementation Notes
- Mirror the existing Postgres opt-in pattern: environment-backed `FromEnvironment` discovery, deterministic skip messaging, `Assert.Skip` on missing configuration, and reflection-based provider hookup to avoid dirtying default runs.
- Use the existing integration project under `tests/DCoding.Data.DVault.Tests/Integration`; do not create a new test project or a new categorization surface.
- Update `ProviderIntegrationCategoryDiscoveryTests.cs` so the discovered type list and trait assertions cover the new SQL Server classes and document SQL Server as external opt-in coverage.
- Use `README.md` as the user-facing source of truth for the env var and command, and `docs/architecture/dvault-v1-explicit-save-service.md` as the architecture-facing source of truth for the provider-strategy dependency and validation posture.
- Use the existing SQLite explicit-save scenarios as the assertion template, but run the SQL Server lane only against the optimized SQL Server provider path from 06EZ0NAMGKJ63WCXAK1J7B08TR; do not add provider-name branches to core code.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add a reproducible local SQL Server bootstrap recipe or a CI-hosted SQL Server lane once the opt-in smoke baseline proves stable?
- After this smoke baseline lands, do we want broader SQL Server coverage for duplicate reuse, batching, or failure translation beyond the one-scenario-per-entity-type contract?

## Risks
- Because SQL Server remains external and opt-in, regressions can escape default automation unless contributors run the documented SQL Server command when the sibling strategy or docs change.
- The ticket is sequenced behind 06EZ0NAMGKJ63WCXAK1J7B08TR, so delayed or divergent strategy delivery there will delay or reshape this smoke-test lane.
- Different local SQL Server versions, authentication modes, or connection defaults can still create environment-specific failures unless `README.md` pins the expected connection assumptions.

## Split Recommendations
- No new split: keep SQL Server strategy implementation in 06EZ0NAMGKJ63WCXAK1J7B08TR and keep this ticket focused on opt-in configuration, documentation, category-baseline updates, and three representative smoke tests.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: provide repeatable SQL Server smoke coverage for the optimized save strategy.

Acceptance Criteria:
- The test configuration is opt-in and documented.
- The smoke suite verifies one hub, one link, and one satellite write scenario.
- Skipped tests report missing configuration rather than failing noisily.