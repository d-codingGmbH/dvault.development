<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Bounded repository evidence already supports a ready-for-critic story contract: existing done child tickets `06EZ0NBX79YQ0J5A9ECJG955TC` and `06EZ0NC3VNZ5FP9XDYVX9DHW1G` cover the materialized split, while the branch documents Pomelo-only optimized MySQL save behavior, opt-in live validation, and SQLite-only benchmark scope.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The story already has two materialized `parentOf` child tickets: `06EZ0NBX79YQ0J5A9ECJG955TC` for the optimized writer and capability-profile work, and `06EZ0NC3VNZ5FP9XDYVX9DHW1G` for opt-in MySQL integration configuration and smoke coverage; both are already `done`.
- The upstream provider-contract story `06EZ0N8HW9PZAFKMM5WQD564VR` and strategy-selection test task `06EZ0N9AM9AJ3AB8DQ6Y1JBS28` are already `done`, so their blocker relations should be treated as satisfied prerequisite context rather than fresh PO blockers.
- The supported EF Core MySQL baseline for this story is `Pomelo.EntityFrameworkCore.MySql`; unsupported providers such as `MySql.EntityFrameworkCore` remain outside scope and must fall back to the provider-neutral writer.
- The existing `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` caller path remains the activation contract; `AddDVaultMySql()` wires Pomelo profile selection and the optimized strategy without introducing a new public MySQL-specific model-builder API.
- Benchmark guidance for this story is bounded to documentation that MySQL has no required benchmark baseline in v0.5; the repository benchmark runner remains SQLite-specific.

### Scope In
- Register MySQL-specific provider capabilities and an optimized save strategy in `src/DCoding.Data.DVault.MySql`.
- Use MySQL-compatible set-based insert-only writes for hub and link rows plus latest-state satellite hash-diff checks while preserving Data Vault insert-only semantics.
- Keep provider-neutral fallback behavior when the active provider is not Pomelo or when the `DbContext` has pending tracked EF changes.
- Provide default local smoke and contract coverage plus opt-in live MySQL integration guidance using `DVAULT_TEST_MYSQL_CONNECTION_STRING`.
- Document that live MySQL validation is opt-in and that benchmark execution remains SQLite-only.

### Scope Out
- Support for EF Core MySQL providers other than `Pomelo.EntityFrameworkCore.MySql`.
- Mandatory local MySQL setup, Docker provisioning, or CI-managed MySQL infrastructure.
- A MySQL-specific benchmark project, a required MySQL benchmark gate, or non-SQLite benchmark runner changes.
- New upsert, merge, retry, or multi-writer concurrency semantics beyond the current insert-only and fallback contract.
- Broader provider-dispatch architecture work already settled by tickets `06EZ0N8HW9PZAFKMM5WQD564VR` and `06EZ0N9AM9AJ3AB8DQ6Y1JBS28`.

## Acceptance Criteria
- `AddDVaultMySql()` registers `DataVaultProviderCapabilityProfiles.MySql`, the Pomelo provider-name mapping, and `MySqlDataVaultSaveStrategy` inside the MySQL provider package.
- For clean `Pomelo.EntityFrameworkCore.MySql` contexts, the optimized strategy persists hub and link rows through parameterized MySQL insert-only SQL and filters satellite writes by latest hash diff so unchanged satellite state is not reinserted.
- When the active EF Core provider is not Pomelo or the current `DbContext` has pending tracked changes, the MySQL strategy declines and the existing provider-neutral save service persists the request instead.
- Automated coverage proves provider registration, Pomelo-only capability selection, SQL generation and parameterization, fallback dispatch, and opt-in live MySQL smoke behavior without requiring MySQL for the default local test run.
- Repository documentation states that live MySQL execution is `ProviderIntegration.ExternalOptIn` via `DVAULT_TEST_MYSQL_CONNECTION_STRING` and that benchmark coverage remains SQLite-specific rather than a MySQL requirement.

## Definition of Done
- The MySQL provider package contains the optimized writer and Pomelo capability-profile registration, and no MySQL-specific SQL leaks into `src/DCoding.Data.DVault`.
- Unit, integration, snapshot, and package-verification coverage for the MySQL path passes, and default `dotnet test DVault.slnx --nologo` does not require a MySQL server.
- `README.md` and `docs/architecture/dvault-v1-explicit-save-service.md` align on the Pomelo baseline, fallback behavior, opt-in live MySQL validation, and SQLite-only benchmark posture.
- The existing child tickets `06EZ0NBX79YQ0J5A9ECJG955TC` and `06EZ0NC3VNZ5FP9XDYVX9DHW1G` remain the only split needed for this story.

## Implementation Notes
- Repository evidence already shows `src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs` using `INSERT IGNORE` for unique hub and link rows, parameter batching, current-transaction participation, cancellation propagation, and latest-state satellite filtering before insert.
- `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` already registers the Pomelo provider-profile selection and `IDataVaultProviderSaveStrategy` wiring in the MySQL package.
- The bounded validation surfaces for this story are already visible in `tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs`, and the MySQL integration configuration and reflection helpers.
- `README.md` and `docs/architecture/dvault-v1-explicit-save-service.md` already document the opt-in MySQL validation path and the no-MySQL-benchmark requirement.
- No new child tickets, relations, attachments, or planning documents are required in this PO pass because the existing `parentOf` split is already materialized and completed.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add explicit support for `MySql.EntityFrameworkCore` or MariaDB-specific compatibility beyond the Pomelo baseline?
- Should the repository later add deeper live MySQL coverage for links, satellites, and reuse-path assertions beyond the current external opt-in smoke proof?
- Should MySQL receive a dedicated benchmark or CI automation ticket later, or should performance validation continue to rely on the shared SQLite benchmark runner plus targeted provider smoke tests?

## Risks
- Live MySQL execution remains opt-in, so runtime differences across real MySQL environments can still surface after merge even though the default local coverage stays green.
- Pomelo-only provider detection is intentionally narrow; accidental widening or package drift could silently change fallback versus optimized selection behavior.
- Because MySQL benchmark coverage is explicitly out of scope here, provider-specific performance regressions may need separate follow-up measurement.

## Split Recommendations
- No further split is recommended; the story is already appropriately materialized through child tickets `06EZ0NBX79YQ0J5A9ECJG955TC` and `06EZ0NC3VNZ5FP9XDYVX9DHW1G`, both linked by `parentOf` and already `done`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: implement and validate a MySQL-specific optimized save path in the existing MySQL provider project.

Scope:
- Use MySQL-compatible SQL for optimized existence checks and insert-only writes.
- Preserve provider-neutral fallback behavior.
- Add opt-in validation and benchmark guidance for MySQL environments.

Acceptance Criteria:
- The MySQL provider registers explicit optimized capabilities.
- The optimized strategy keeps Data Vault insert-only semantics intact.
- Tests or smoke coverage demonstrate behavior without requiring MySQL for default local validation.