[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the documented provider-neutral fallback baseline and the SQLite-only optimized strategy baseline; no blocking PO questions remain.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- For this ticket, the v1 default behavior is the provider-neutral `AddDVault()` / `IDataVaultSaveService` fallback path, and that baseline remains supported when no provider-specific optimized strategy is registered.
- The only currently documented optimized provider baseline is SQLite via `AddDVaultSqlite()` and `SqliteDataVaultSaveStrategy`; other providers remain compatibility-baseline paths unless a later ticket adds their own optimized strategy contract.
- `Unknown provider` for this ticket means a `DbContext` / provider combination that does not match any registered optimized strategy; the expected v1 behavior is to stay on the fallback path rather than selecting an incompatible optimized implementation.
- `Missing capability registration` for this ticket means the provider package has not registered the compatible optimized strategy/capability wiring needed for dispatch; the safe bounded default is fallback selection, not implicit optimized selection.
- `Failure messages` in this ticket means the automated test assertions and diagnostics should make the broken capability gate, registration path, or unexpected strategy selection obvious when the tests fail.

Scope In
- Add automated coverage for save-strategy dispatch between the provider-neutral fallback path and provider-specific optimized strategies.
- Cover the documented baseline cases: fallback selection, SQLite optimized selection, missing capability registration, and unknown-provider behavior.
- Assert diagnostics that identify the unexpected selected strategy or the missing capability/registration path when dispatch behavior regresses.
- Use local-only test infrastructure consistent with the existing DVault test roots and no live external database dependency.

Scope Out
- Adding new provider-specific optimized save strategies or new provider capability profiles beyond the existing SQLite baseline.
- Reworking the broader save-service persistence contract outside the minimum needed to exercise strategy selection.
- Live external database integration coverage for PostgreSQL, SQL Server, Oracle, or MySQL.
- Changing workflow metadata, ticket routing, or non-test MVP provider policy.

Open questions
- none

Follow-up questions
- Once other provider packages gain their own optimized strategies, should the same dispatch-contract matrix be standardized across PostgreSQL, SQL Server, Oracle, and MySQL tickets?
- Should the repository later add a shared provider-strategy test helper so future provider packages can reuse the same fallback-versus-optimized assertions?

Risks
- If the current implementation does not expose the chosen strategy clearly enough for tests, developers may need a small observability seam or spy registration to keep the tests deterministic and focused on dispatch behavior.
- Over-coupling assertions to specific registration names or internal wiring details could make the tests brittle if provider registration is refactored without changing the intended dispatch contract.

Split recommendations
- none

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