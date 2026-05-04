<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified live ticket, comment, and relation state plus repository evidence; raw provider SQL execution currently exists only in SQLite, sibling ticket 06EZ0N9AM9AJ3AB8DQ6Y1JBS28 already owns service-level fallback-selection tests, and this ticket is ready for PO-critic as one bounded shared ADO.NET execution-contract test task with no split or ticket writes needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Verified relation context: the only persisted relation on this ticket is parentOf from story 06EZ0N8HW9PZAFKMM5WQD564VR, so no extra child-ticket split or relation write is currently justified.
- Repository evidence shows the only visible raw provider SQL execution surface today is SqliteDataVaultSaveStrategy in src/DCoding.Data.DVault.Sqlite, while non-SQLite provider packages currently register only the core save service and no optimized strategy.
- SQLite required-local integration coverage already exists, and the only visible external opt-in provider harness today is PostgreSQL through DVAULT_TEST_POSTGRES_CONNECTION_STRING; that is the bounded v1 reuse baseline for this contract.
- For this ticket, fallback behavior should mean execution-contract decline or unsupported-shape behavior at the SQL boundary, not service-level provider-strategy selection or unknown-provider dispatch, because that coverage already belongs to sibling ticket 06EZ0N9AM9AJ3AB8DQ6Y1JBS28.

### Scope In
- Define the shared SQL execution test contract at the ADO.NET interaction level for optimized provider writers.
- Cover parameter binding, transaction participation, cancellation-token propagation, and contract-level decline or fallback behavior for the shared execution boundary.
- Add one reusable provider-agnostic assertion or fixture layer that SQLite required-local coverage uses now and external opt-in provider suites can reuse later.
- Keep the work bounded to tests and any internal execution-boundary shaping required to make those tests reusable.

### Scope Out
- Service-level provider strategy registration or dispatch selection; that remains the scope of ticket 06EZ0N9AM9AJ3AB8DQ6Y1JBS28.
- New provider-specific optimized implementations, capability matrices, or benchmark work.
- New always-on live SQL Server, Oracle, or MySQL validation harnesses in default local test runs.
- Provider-specific SQL text, quoting, or placeholder assertions for non-SQLite providers, and any public API expansion that exists only to satisfy test wiring.

## Acceptance Criteria
- A shared execution-contract test harness proves parameter binding, transaction participation, cancellation propagation, and decline or fallback signaling without requiring provider-specific SQL text assertions.
- The contract assertions operate on provider-neutral command-execution behavior, so non-SQLite consumers are not forced to match SQLite quoting or placeholder syntax.
- SQLite required-local coverage consumes the shared contract harness against the existing optimized SQLite path, demonstrating the harness works with a real provider implementation.
- The shared contract is shaped so external opt-in provider suites can reuse it later without changing the contract semantics or making live external databases mandatory for default local validation.
- Fallback coverage in this ticket is limited to SQL-execution-boundary behavior; it does not duplicate service-level strategy-selection coverage already owned by ticket 06EZ0N9AM9AJ3AB8DQ6Y1JBS28.

## Definition of Done
- The repository contains one reusable shared test support layer for provider SQL execution contract assertions and at least one provider-specific consumer of that layer.
- SQLite required-local tests cover the shared execution contract with clear failures for parameter binding, transaction participation, cancellation propagation, and decline or fallback clauses.
- Default local test runs do not gain a new mandatory live external database dependency; any external provider reuse remains opt-in.
- Any supporting execution abstraction introduced to enable the tests remains internal unless a separate contract explicitly approves a public API change.

## Implementation Notes
- Use the existing test-project split as the v1 layout default: reusable contract helpers in tests/DCoding.Data.DVault.Tests/Shared, with provider-owned consumer tests carrying their own category and provider traits.
- Current source evidence for the execution boundary is the SQLite path in DVaultSqliteServiceCollectionExtensions: it opens DbConnection, reuses or creates a DbTransaction, creates DbCommand parameters, and forwards the supplied CancellationToken into ADO.NET calls.
- DefaultDataVaultSaveService remains the owner of provider-neutral fallback after CanSave selection; this ticket should not reopen or duplicate that dispatch loop's coverage.
- Prefer provider-neutral doubles or fakes for low-level command, transaction, and cancellation assertions, then keep the SQLite integration consumer thin and behavior-focused rather than SQL-string-focused.
- PostgreSQL is the first visible external opt-in reuse target because the integration project already carries conditional Npgsql support and DVAULT_TEST_POSTGRES_CONNECTION_STRING; SQL Server, Oracle, and MySQL adoption belongs to their provider stories when scheduled.

## Open Questions
- none

## Follow-Up Questions
- When PostgreSQL optimized-provider work starts, should its first opt-in execution tests adopt this shared harness before adding provider-specific supplemental assertions?
- If later providers need broader execution capabilities such as provider-specific parameter types, batching rules, or result-set handling, should those extend this shared contract or remain provider-owned add-on tests?

## Risks
- If implementation drifts back to asserting SQLite SQL strings instead of provider-neutral command behavior, the shared harness will not be reusable across providers.
- Because SQLite is the only current optimized executor, some shared-contract gaps may stay hidden until a second provider adopts the harness.
- Future providers may require execution capabilities beyond the current insert-only non-query path, which could force a later contract expansion and test refactor.

## Split Recommendations
- No split recommended; the parent story already separates documentation matrix work, service-level strategy-selection tests, and this lower-level SQL execution contract coverage into bounded tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: add shared contract tests for the optimized provider execution boundary.

Acceptance Criteria:
- Contract tests validate parameter binding, transaction participation, cancellation propagation, and fallback behavior.
- Tests are provider-agnostic and reusable by SQLite plus external provider smoke suites.
- The tests avoid asserting SQLite-specific SQL syntax for non-SQLite providers.