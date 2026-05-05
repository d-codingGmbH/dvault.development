[gicket-bot] PO refinement contract

Summary
- Bounded repository evidence already supports a ready-for-critic story contract: existing done child tickets `06EZ0NBX79YQ0J5A9ECJG955TC` and `06EZ0NC3VNZ5FP9XDYVX9DHW1G` cover the materialized split, while the branch documents Pomelo-only optimized MySQL save behavior, opt-in live validation, and SQLite-only benchmark scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The story already has two materialized `parentOf` child tickets: `06EZ0NBX79YQ0J5A9ECJG955TC` for the optimized writer and capability-profile work, and `06EZ0NC3VNZ5FP9XDYVX9DHW1G` for opt-in MySQL integration configuration and smoke coverage; both are already `done`.
- The upstream provider-contract story `06EZ0N8HW9PZAFKMM5WQD564VR` and strategy-selection test task `06EZ0N9AM9AJ3AB8DQ6Y1JBS28` are already `done`, so their blocker relations should be treated as satisfied prerequisite context rather than fresh PO blockers.
- The supported EF Core MySQL baseline for this story is `Pomelo.EntityFrameworkCore.MySql`; unsupported providers such as `MySql.EntityFrameworkCore` remain outside scope and must fall back to the provider-neutral writer.
- The existing `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` caller path remains the activation contract; `AddDVaultMySql()` wires Pomelo profile selection and the optimized strategy without introducing a new public MySQL-specific model-builder API.
- Benchmark guidance for this story is bounded to documentation that MySQL has no required benchmark baseline in v0.5; the repository benchmark runner remains SQLite-specific.

Scope In
- Register MySQL-specific provider capabilities and an optimized save strategy in `src/DCoding.Data.DVault.MySql`.
- Use MySQL-compatible set-based insert-only writes for hub and link rows plus latest-state satellite hash-diff checks while preserving Data Vault insert-only semantics.
- Keep provider-neutral fallback behavior when the active provider is not Pomelo or when the `DbContext` has pending tracked EF changes.
- Provide default local smoke and contract coverage plus opt-in live MySQL integration guidance using `DVAULT_TEST_MYSQL_CONNECTION_STRING`.
- Document that live MySQL validation is opt-in and that benchmark execution remains SQLite-only.

Scope Out
- Support for EF Core MySQL providers other than `Pomelo.EntityFrameworkCore.MySql`.
- Mandatory local MySQL setup, Docker provisioning, or CI-managed MySQL infrastructure.
- A MySQL-specific benchmark project, a required MySQL benchmark gate, or non-SQLite benchmark runner changes.
- New upsert, merge, retry, or multi-writer concurrency semantics beyond the current insert-only and fallback contract.
- Broader provider-dispatch architecture work already settled by tickets `06EZ0N8HW9PZAFKMM5WQD564VR` and `06EZ0N9AM9AJ3AB8DQ6Y1JBS28`.

Open questions
- none

Follow-up questions
- Should a later ticket add explicit support for `MySql.EntityFrameworkCore` or MariaDB-specific compatibility beyond the Pomelo baseline?
- Should the repository later add deeper live MySQL coverage for links, satellites, and reuse-path assertions beyond the current external opt-in smoke proof?
- Should MySQL receive a dedicated benchmark or CI automation ticket later, or should performance validation continue to rely on the shared SQLite benchmark runner plus targeted provider smoke tests?

Risks
- Live MySQL execution remains opt-in, so runtime differences across real MySQL environments can still surface after merge even though the default local coverage stays green.
- Pomelo-only provider detection is intentionally narrow; accidental widening or package drift could silently change fallback versus optimized selection behavior.
- Because MySQL benchmark coverage is explicitly out of scope here, provider-specific performance regressions may need separate follow-up measurement.

Split recommendations
- No further split is recommended; the story is already appropriately materialized through child tickets `06EZ0NBX79YQ0J5A9ECJG955TC` and `06EZ0NC3VNZ5FP9XDYVX9DHW1G`, both linked by `parentOf` and already `done`.

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