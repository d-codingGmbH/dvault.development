[gicket-bot] PO refinement contract

Summary
- Verified live ticket, comment, and relation state plus repository evidence; raw provider SQL execution currently exists only in SQLite, sibling ticket 06EZ0N9AM9AJ3AB8DQ6Y1JBS28 already owns service-level fallback-selection tests, and this ticket is ready for PO-critic as one bounded shared ADO.NET execution-contract test task with no split or ticket writes needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified relation context: the only persisted relation on this ticket is parentOf from story 06EZ0N8HW9PZAFKMM5WQD564VR, so no extra child-ticket split or relation write is currently justified.
- Repository evidence shows the only visible raw provider SQL execution surface today is SqliteDataVaultSaveStrategy in src/DCoding.Data.DVault.Sqlite, while non-SQLite provider packages currently register only the core save service and no optimized strategy.
- SQLite required-local integration coverage already exists, and the only visible external opt-in provider harness today is PostgreSQL through DVAULT_TEST_POSTGRES_CONNECTION_STRING; that is the bounded v1 reuse baseline for this contract.
- For this ticket, fallback behavior should mean execution-contract decline or unsupported-shape behavior at the SQL boundary, not service-level provider-strategy selection or unknown-provider dispatch, because that coverage already belongs to sibling ticket 06EZ0N9AM9AJ3AB8DQ6Y1JBS28.

Scope In
- Define the shared SQL execution test contract at the ADO.NET interaction level for optimized provider writers.
- Cover parameter binding, transaction participation, cancellation-token propagation, and contract-level decline or fallback behavior for the shared execution boundary.
- Add one reusable provider-agnostic assertion or fixture layer that SQLite required-local coverage uses now and external opt-in provider suites can reuse later.
- Keep the work bounded to tests and any internal execution-boundary shaping required to make those tests reusable.

Scope Out
- Service-level provider strategy registration or dispatch selection; that remains the scope of ticket 06EZ0N9AM9AJ3AB8DQ6Y1JBS28.
- New provider-specific optimized implementations, capability matrices, or benchmark work.
- New always-on live SQL Server, Oracle, or MySQL validation harnesses in default local test runs.
- Provider-specific SQL text, quoting, or placeholder assertions for non-SQLite providers, and any public API expansion that exists only to satisfy test wiring.

Open questions
- none

Follow-up questions
- When PostgreSQL optimized-provider work starts, should its first opt-in execution tests adopt this shared harness before adding provider-specific supplemental assertions?
- If later providers need broader execution capabilities such as provider-specific parameter types, batching rules, or result-set handling, should those extend this shared contract or remain provider-owned add-on tests?

Risks
- If implementation drifts back to asserting SQLite SQL strings instead of provider-neutral command behavior, the shared harness will not be reusable across providers.
- Because SQLite is the only current optimized executor, some shared-contract gaps may stay hidden until a second provider adopts the harness.
- Future providers may require execution capabilities beyond the current insert-only non-query path, which could force a later contract expansion and test refactor.

Split recommendations
- No split recommended; the parent story already separates documentation matrix work, service-level strategy-selection tests, and this lower-level SQL execution contract coverage into bounded tickets.

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