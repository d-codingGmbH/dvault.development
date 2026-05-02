<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Materialized `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md`, fixed the exact two-event customer-profile sequence and plain-EF stored-row assertions, and documented the shared contract with paired ticket `06EXB7S6DB97GVVTS2GGZ3CCX8`.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md` is now the authoritative shared comparison artifact for this ticket and paired ticket `06EXB7S6DB97GVVTS2GGZ3CCX8`.
- Use one customer business key `C-100` with profile attributes `customer_name` and `customer_status`.
- Event 1 is the initial state: `2026-04-29T10:15:00Z`, `crm-import`, `customer_name = Alice Adams`, `customer_status = prospect`.
- Event 2 is the changed state: `2026-04-29T11:30:00Z`, `crm-change`, `customer_name = Alice Baker`, `customer_status = active`.
- For the plain EF baseline, the comparison target is an exact stored-outcome contract: exactly two history rows ordered ascending by the persisted history timestamp, with no extra unchanged replay row.

### Scope In
- Implement a minimal conventional EF Core customer profile history baseline on SQLite using ordinary entities and a regular `DbContext`/`DbSet` model.
- Execute the exact two-event `C-100` scenario from the shared comparison contract.
- Add repeatable automated coverage in `tests/DCoding.Data.DVault.Tests` so the baseline runs in the current solution layout.
- Assert and document the exact persisted history rows needed for comparison with the paired DVault ticket.

### Scope Out
- Do not implement the DVault-backed version of the scenario in this ticket.
- Do not extend the scenario to order, link, or broader relationship behavior.
- Do not add provider support beyond SQLite or expand into deferred capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- Do not require a new runnable sample app under `examples/` for this v1 baseline.
- Do not widen the scenario beyond the locked two-event customer-profile contract for this ticket.

## Acceptance Criteria
- A conventional EF Core customer profile baseline exists using ordinary CLR entities, a regular `DbContext`/`DbSet` model, and SQLite persistence.
- The baseline executes through automated tests in `tests/DCoding.Data.DVault.Tests` under the existing solution layout.
- Applying the two shared events for customer `C-100` produces exactly two persisted customer profile history rows ordered by the persisted history timestamp: row 1 `Alice Adams` / `prospect` / `2026-04-29T10:15:00Z` / `crm-import`; row 2 `Alice Baker` / `active` / `2026-04-29T11:30:00Z` / `crm-change`.
- The automated assertions prove that no extra unchanged replay row is inserted for this v1 plain EF baseline scenario.
- The resulting comparison notes and assertions stay aligned with `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md` so the paired DVault ticket uses the same input history sequence.

## Definition of Done
- The acceptance criteria are satisfied.
- New or updated tests are included through the existing `tests/DCoding.Data.DVault.Tests` project structure and are intended to run with the normal repository `dotnet test` flow.
- Scenario-specific comparison notes remain consistent with `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md`.
- Shared implementation standards and the current repository layout/.NET baseline are followed.

## Implementation Notes
- Use the existing test harness under `tests/DCoding.Data.DVault.Tests`; repository evidence already shows the integration project references SQLite in `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj`.
- Implement the baseline with ordinary EF Core entities and explicit history rows, not DVault metadata translation or `IDataVaultSaveService`.
- Use the deterministic timestamps and record sources from `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md` so repeated runs stay stable.
- The paired DVault ticket keeps its own storage shape, but it must consume the same two business events and deterministic timestamps from the shared comparison contract.

## Open Questions
- none

## Follow-Up Questions
- After both comparison tickets land, should the scenario also be promoted into a runnable example under `examples/`?
- Once both baselines exist, should a follow-up add shared assertion helpers so cross-ticket comparison stays synchronized in code as well as documentation?

## Risks
- Comparison value drops if the plain EF implementation inserts convenience rows or models more than the exact two-event shared baseline.
- If the paired DVault ticket diverges from `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md`, later side-by-side evaluation will lose fidelity.

## Split Recommendations
- Keep any runnable example or broader demo separate; this ticket should stay focused on the automated plain EF baseline and the locked comparison contract.
- If stakeholders later want additional change-history variants or replay/deduplication cases, schedule them as separate follow-up tickets instead of widening this v1 baseline.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Create a plain EF implementation of the customer profile scenario.

## Scope
- Use conventional EF entities and Sqlite storage.

## Acceptance Criteria
- The baseline can be executed by tests or sample runner.
- The baseline records behavior needed for comparison.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.