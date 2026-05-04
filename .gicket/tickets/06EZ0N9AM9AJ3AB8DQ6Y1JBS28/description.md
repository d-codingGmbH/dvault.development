<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around the documented provider-neutral fallback baseline and the SQLite-only optimized strategy baseline; no blocking PO questions remain.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- For this ticket, the v1 default behavior is the provider-neutral `AddDVault()` / `IDataVaultSaveService` fallback path, and that baseline remains supported when no provider-specific optimized strategy is registered.
- The only currently documented optimized provider baseline is SQLite via `AddDVaultSqlite()` and `SqliteDataVaultSaveStrategy`; other providers remain compatibility-baseline paths unless a later ticket adds their own optimized strategy contract.
- `Unknown provider` for this ticket means a `DbContext` / provider combination that does not match any registered optimized strategy; the expected v1 behavior is to stay on the fallback path rather than selecting an incompatible optimized implementation.
- `Missing capability registration` for this ticket means the provider package has not registered the compatible optimized strategy/capability wiring needed for dispatch; the safe bounded default is fallback selection, not implicit optimized selection.
- `Failure messages` in this ticket means the automated test assertions and diagnostics should make the broken capability gate, registration path, or unexpected strategy selection obvious when the tests fail.

### Scope In
- Add automated coverage for save-strategy dispatch between the provider-neutral fallback path and provider-specific optimized strategies.
- Cover the documented baseline cases: fallback selection, SQLite optimized selection, missing capability registration, and unknown-provider behavior.
- Assert diagnostics that identify the unexpected selected strategy or the missing capability/registration path when dispatch behavior regresses.
- Use local-only test infrastructure consistent with the existing DVault test roots and no live external database dependency.

### Scope Out
- Adding new provider-specific optimized save strategies or new provider capability profiles beyond the existing SQLite baseline.
- Reworking the broader save-service persistence contract outside the minimum needed to exercise strategy selection.
- Live external database integration coverage for PostgreSQL, SQL Server, Oracle, or MySQL.
- Changing workflow metadata, ticket routing, or non-test MVP provider policy.

## Acceptance Criteria
- Tests prove that the provider-neutral `AddDVault()` baseline selects the fallback save behavior when no compatible provider-specific optimized strategy is registered.
- Tests prove that the SQLite registration path selects the optimized SQLite strategy only when the compatible SQLite provider strategy/capability wiring is present.
- Tests cover missing capability registration and unknown-provider scenarios and confirm that dispatch does not silently choose an incompatible optimized strategy in those cases.
- When a dispatch expectation fails, the test assertions/diagnostics clearly identify the missing capability, broken registration path, or unexpected selected strategy.
- The full test coverage runs locally and deterministically without requiring live external database services.

## Definition of Done
- The new strategy-selection tests are added under the existing DVault test layout and pass in the normal local test run for this repository.
- The tests exercise selection through the production `IDataVaultSaveService` dispatch boundary instead of bypassing dispatch with direct strategy calls.
- Any supporting test-only fixtures remain local to test projects and preserve existing packable-source layout rules, including the one-member-per-file policy for packable packages.
- The resulting test suite distinguishes fallback versus optimized-path regressions with deterministic assertions.

## Implementation Notes
- Ratify the documented v0.5 baseline in the tests: provider-neutral `AddDVault()` is the compatibility fallback, and SQLite is the only current optimized provider baseline that must prove optimized dispatch.
- Use the existing local SQLite EF Core test baseline for the optimized-path proof; use local fakes, stubs, or controlled DI registration to cover fallback, missing-registration, and unknown-provider branches without external services.
- Keep the ticket focused on dispatch selection and observability, not on adding new provider feature work or broad end-to-end persistence scenarios already covered by other tickets.
- If dispatch is only observable indirectly today, a minimal non-production test seam is acceptable so long as the production dispatch rules stay unchanged and the tests still enter through the public save-service boundary.
- Where multiple strategies can be registered, tests should prove that compatibility gating controls selection and that the fallback path remains active when the optimized strategy is not compatible for the current provider/context.

## Open Questions
- none

## Follow-Up Questions
- Once other provider packages gain their own optimized strategies, should the same dispatch-contract matrix be standardized across PostgreSQL, SQL Server, Oracle, and MySQL tickets?
- Should the repository later add a shared provider-strategy test helper so future provider packages can reuse the same fallback-versus-optimized assertions?

## Risks
- If the current implementation does not expose the chosen strategy clearly enough for tests, developers may need a small observability seam or spy registration to keep the tests deterministic and focused on dispatch behavior.
- Over-coupling assertions to specific registration names or internal wiring details could make the tests brittle if provider registration is refactored without changing the intended dispatch contract.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: prove that provider strategy dispatch chooses optimized implementations only when the provider package registers compatible capabilities.

Acceptance Criteria:
- Tests cover fallback selection, optimized selection, missing capability registration, and unknown provider behavior.
- Tests do not require live external database services.
- Failure messages make it clear which provider capability or registration path is broken.