<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies the current shared provider optimization contract, deterministic strategy dispatch, provider-neutral fallback semantics, and the SQLite-only v0.5 optimization baseline across the five visible provider packages.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Ratify the shared extension boundary around IDataVaultProviderSaveStrategy, DataVaultProviderSaveStrategyContext, and DataVaultProviderCapabilityProfile instead of introducing provider-name branching in the core save service.
- Dispatch is deterministic for explicit saves: evaluate registered provider strategies in descending Priority order, select the first strategy whose CanSave check accepts the current DbContext and ordered request batch, and otherwise use the built-in provider-neutral IDataVaultSaveService path.
- When multiple compatible strategies share the same Priority, registration order is the v1 default tie-break because the core dispatch path preserves the registered enumerable order after sorting by priority.
- The visible v0.5 optimization baseline is bounded: SQLite is the only provider package that currently owns an optimized save strategy requirement, while PostgreSQL, SQL Server, Oracle, and MySQL remain compatibility-only provider packages for this story.
- Capability discovery for this story is bounded to explicit capability-profile declarations plus strategy self-gating; broader provider-aware metadata translation or richer provider feature taxonomies are follow-up work, not blockers here.

### Scope In
- Define or ratify the shared core contracts for provider capability profiles, provider save strategies, and provider save strategy execution context.
- Document deterministic save dispatch semantics, including priority ordering, compatibility gating, winner selection, and provider-neutral fallback behavior.
- Keep provider-specific SQL and provider-specific provider-name checks inside provider packages and strategy implementations rather than the core package.
- Document the current capability matrix and optimization-hook ownership for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Add or refine tests that prove absent registration fallback, incompatible strategy rejection, compatible SQLite selection, and deterministic winner selection when multiple strategies are registered.

### Scope Out
- Implement new optimized save strategies for PostgreSQL, SQL Server, Oracle, or MySQL.
- Redesign the write entry point away from the existing explicit IDataVaultSaveService boundary or move this work into SaveChanges interception.
- Change stable hashing, naming-policy behavior, or provider-neutral table and column conventions except where needed to document the shared optimization contract.
- Promise provider-neutral multi-writer conflict handling, retry behavior, merge semantics, upsert semantics, or bulk-load semantics beyond the current NoneInV1Unsupported baseline.
- Expand provider-aware EF metadata translation beyond the existing bounded profile baseline unless that work is scheduled separately.

## Acceptance Criteria
- The ticket documents the shared provider optimization boundary as core-owned contracts plus provider-package implementations, with no provider-specific SQL and no provider-name branching added to the core save dispatcher outside the strategy boundary.
- Explicit save dispatch is documented and test-covered as descending Priority evaluation with first-compatible-strategy wins, deterministic equal-priority tie behavior, and provider-neutral fallback when no strategy accepts the request.
- Unsupported, unknown, or unregistered provider capability wiring falls back to the provider-neutral implementation without changing the public IDataVaultSaveService caller contract.
- SQLite is documented and test-covered as the only v0.5 provider that must register an optimized save strategy and set-based existence-check behavior; PostgreSQL, SQL Server, Oracle, and MySQL remain compatibility-only baselines in this story.
- Documentation identifies which visible provider projects own which optimization hooks: src/DCoding.Data.DVault owns the contracts and fallback dispatcher, src/DCoding.Data.DVault.Sqlite owns the current optimized strategy, and src/DCoding.Data.DVault.Postgres, .SqlServer, .Oracle, and .MySql currently own only provider registration surfaces for later optimization stories.
- Any public core contract change made by this story is explicitly documented and covered by updated contract tests and public API snapshot expectations.

## Definition of Done
- Ticket text or attached planning notes state the ratified dispatch semantics and provider matrix without leaving blocking ambiguity about strategy selection or fallback behavior.
- Unit and integration coverage proves priority-based dispatch, equal-priority determinism, incompatible strategy rejection, missing-registration fallback, and SQLite optimized-path selection.
- Architecture or README-level documentation names the current optimization-hook owners and keeps the five-provider matrix aligned with the repository structure and package surfaces.
- If the shared contract surface changes, XML docs and public API snapshot files for DCoding.Data.DVault and any affected provider packages are updated.
- No new provider-specific SQL or provider-name branching is introduced in src/DCoding.Data.DVault outside the documented strategy boundary.

## Implementation Notes
- src/DCoding.Data.DVault/DataVaultSaveService.cs already provides the starting dispatcher: it orders IDataVaultProviderSaveStrategy registrations by descending Priority, invokes CanSave against the current DbContext and ordered requests, and falls back to the built-in provider-neutral writer when none match.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs is the current extension seam; keep provider-specific behavior behind this interface rather than widening IDataVaultSaveService for per-provider branches.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs is the reference provider implementation today: AddDVaultSqlite registers SqliteDataVaultSaveStrategy through DI, the strategy self-gates on the SQLite EF Core provider and a clean change tracker, and the raw SQL path stays inside the SQLite package.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs is the current bounded capability-profile baseline; it already exposes deterministic profile naming, explicit unsupported SQL-function and concurrency declarations, and logical type mappings through DataVaultProviderCapabilityProfiles.Sqlite.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs, Unit/ExplicitDataVaultSaveServiceTests.cs, Unit/DataVaultProviderCapabilityProfileTests.cs, and Integration/SqliteProviderCapabilityProfileTests.cs already cover most of the bounded baseline and should be extended only where dispatch-order semantics are still implicit.
- docs/architecture/dvault-v1-explicit-save-service.md already contains the visible five-provider v0.5 matrix and should remain the source of truth for release-scoped ownership and optimization expectations.

## Open Questions
- none

## Follow-Up Questions
- After the shared contract is accepted, which non-SQLite provider should receive the first dedicated optimized strategy story?
- Should provider-aware EF metadata translation remain on the current SQLite profile baseline until a provider story requires more, or should that become its own architecture follow-up?
- Do future provider stories need a richer capability vocabulary for upsert, bulk-load, or concurrency signals beyond the current NoneInV1Unsupported baseline?

## Risks
- If future provider packages introduce overlapping compatible strategies without explicit priority and tie-break tests, dispatch behavior can drift even though fallback remains available.
- The core translator currently carries a SQLite capability-profile baseline, so later provider-aware metadata work could expand scope unless this story stays tightly focused on save optimization contracts and dispatch.
- If the documentation matrix and provider package registrations drift apart, downstream provider stories may implement against the wrong ownership or validation expectations.

## Split Recommendations
- Keep this story limited to the shared contract, dispatch semantics, fallback behavior, and five-provider documentation; do not bundle new optimized writers for PostgreSQL, SQL Server, Oracle, or MySQL into it.
- If implementation pressure grows, defer any capability-profile expansion beyond the existing SQLite baseline to a separate architecture or provider story after this shared dispatcher contract is accepted.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: define the common provider optimization boundary before individual database providers add optimized implementations.

Scope:
- Introduce or refine contracts for provider capability discovery, optimized save strategy dispatch, and fallback selection.
- Keep the core package free of provider-specific SQL and provider-name branching outside the strategy boundary.
- Document the capability matrix for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Add tests proving that unsupported or unregistered capabilities fall back to the provider-neutral implementation.

Acceptance Criteria:
- Provider capability selection is explicit, deterministic, and test-covered.
- The provider-neutral fallback remains the default when no provider-specific implementation is available.
- Individual provider stories can implement optimized strategies without changing public core API shape unless the contract story documents and tests that change.
- Documentation identifies which provider projects own which optimization hooks.