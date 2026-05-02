<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket with an explicit order benchmark contract: compare one O-1000/SKU-COFFEE relationship workload, measure two fulfillment-state changes, exclude unchanged replay, and keep the work unsplit.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository evidence already fixes the customer benchmark baseline through docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md and the existing customer integration tests.
- The repository still has no benchmark project; benchmarks/ remains a placeholder directory, so this ticket owns creating the first benchmark executable there.
- SQLite/local remains the v1 default provider baseline. Postgres stays opt-in through DVAULT_TEST_POSTGRES_CONNECTION_STRING and is not required for default benchmark execution.
- The v1 order benchmark contract uses one order business key O-1000 and one product business key SKU-COFFEE, with a measured dataset of 1 order, 1 product, 1 order-product relationship, and 2 fulfillment history events.
- Both order benchmark suites must execute the same measured sequence: create the order/product relationship at 2026-05-01T09:30:00Z from order-entry, persist Backordered/NORTH-1 at 2026-05-01T10:00:00Z from warehouse-allocation, then persist Allocated/NORTH-1 at 2026-05-01T10:45:00Z from warehouse-allocation.
- The unchanged replay at 2026-05-01T11:15:00Z from warehouse-replay is outside the measured benchmark workload and must not add a third persisted history row in either comparison baseline.
- No bounded planning document, attachment, or child ticket was materialized during this refinement because the ticket remains one bounded benchmark task.

### Scope In
- Create one benchmark executable project under benchmarks/ using the repository net10.0 baseline and add it to DVault.slnx.
- Implement shared deterministic benchmark setup and data generation used by both scenario suites.
- Benchmark a conventional EF baseline against the DVault flow for the customer profile history scenario using the existing customer comparison contract.
- Benchmark a conventional EF baseline against the DVault flow for the reduced order/product relationship history scenario using O-1000, SKU-COFFEE, order-entry at 2026-05-01T09:30:00Z, and warehouse-allocation events at 2026-05-01T10:00:00Z and 2026-05-01T10:45:00Z.
- Document one local command or entry path for unattended benchmark execution without extra infrastructure.

### Scope Out
- Any benchmark path that requires Postgres, Docker, or DVAULT_TEST_POSTGRES_CONNECTION_STRING by default.
- New product behavior in src/DCoding.Data.DVault beyond the minimal support needed to run the benchmark scenarios.
- CI performance gates, published benchmark history, or enforced regression thresholds.
- Additional Data Vault expansion comparisons such as PIT, bridge, multi-active satellite, or provider-specific optimization benchmarks.
- Measuring the broader 2-order/3-line conventional EF reuse scenario from tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:20-72 inside this ticket's required order comparison.
- Timing the unchanged warehouse-replay case at 2026-05-01T11:15:00Z as part of the required benchmark workload.

## Acceptance Criteria
- A benchmark project exists under benchmarks/, is included in DVault.slnx, and builds on the repository net10.0 baseline.
- Running the documented local benchmark command executes both a customer-profile comparison and an order-focused comparison without requiring Postgres or other external services by default.
- The customer benchmark uses deterministic shared input matching docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md.
- The order benchmark uses this deterministic shared input: order O-1000, product SKU-COFFEE, relationship creation at 2026-05-01T09:30:00Z from order-entry, then fulfillment Backordered/NORTH-1 at 2026-05-01T10:00:00Z from warehouse-allocation, then fulfillment Allocated/NORTH-1 at 2026-05-01T10:45:00Z from warehouse-allocation; the measured dataset size is 1 order, 1 product, 1 relationship, and 2 fulfillment history events.
- The conventional EF order benchmark persists exactly 1 order row, 1 product row, 1 order-product relationship row, and exactly 2 fulfillment history rows for O-1000/SKU-COFFEE ordered by history timestamp ascending; row 1 is Backordered/NORTH-1 at 2026-05-01T10:00:00Z from warehouse-allocation and row 2 is Allocated/NORTH-1 at 2026-05-01T10:45:00Z from warehouse-allocation.
- The DVault order benchmark persists exactly 1 order hub row, 1 product hub row, 1 order-product link row, and exactly 2 fulfillment satellite rows for O-1000/SKU-COFFEE ordered by load timestamp ascending with the same two fulfillment states and record sources; the unchanged warehouse-replay case does not create a third history row in the required benchmark workload.

## Definition of Done
- The benchmark project, solution wiring, and any supporting documentation follow the shared implementation standards document already referenced by the ticket context.
- The benchmark project and existing solution build remain runnable locally, with the benchmark invocation documented for unattended developer use.
- Default benchmark execution remains SQLite/local-only and does not require Postgres configuration, Docker, or machine-specific checked-in secrets.
- Shared setup code covers deterministic business keys, timestamps, record sources, and expected persisted outcomes so the customer and order suites do not maintain separate duplicated fixture-generation logic for the same comparison concerns.

## Implementation Notes
- Reuse docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md as the exact customer input and persisted-outcome contract.
- For the order suite, do not benchmark the broader 2-order/3-line conventional EF reuse model from NormalEfOrderProductSqliteTests.cs:20-72. Use a reduced ordinary EF history model or benchmark-local fixture that matches the single O-1000/SKU-COFFEE DVault path.
- The order benchmark should execute three logical operations in order: persist order and product plus their relationship at 2026-05-01T09:30:00Z from order-entry, persist Backordered/NORTH-1 at 2026-05-01T10:00:00Z from warehouse-allocation, then persist Allocated/NORTH-1 at 2026-05-01T10:45:00Z from warehouse-allocation.
- The conventional EF order baseline should carry the same business keys, timestamps, and record-source values as the DVault order baseline, even though its physical schema can use ordinary EF entities instead of hubs, links, and satellites.
- Keep the 2026-05-01T11:15:00Z warehouse-replay case as an optional correctness reference only; exclude it from timed benchmark operations and from the required persisted benchmark rows.
- Keep benchmark code separate from test code, but extracting small reusable scenario/setup helpers is acceptable if it reduces duplication without expanding scope. Preserve the repository single root solution entry point and folder conventions.

## Open Questions
- none

## Follow-Up Questions
- Should a follow-up ticket benchmark the broader 2-order/3-line conventional EF reuse scenario from NormalEfOrderProductSqliteTests.cs as a separate relationship-reuse benchmark after this reduced cross-suite history comparison lands?
- Should a later ticket add CI-hosted benchmark reporting or regression thresholds once the first local benchmark project exists?
- After the SQLite/local v1 benchmark lands, is there product value in provider-specific benchmark tickets for Postgres or other adapters?

## Risks
- Benchmark numbers will still vary by developer machine; this ticket should establish deterministic relative comparison coverage, not a cross-machine performance gate.
- If the benchmark implementation drifts from the ratified order contract or accidentally times the broader reuse scenario, the reported comparison will stop representing the intended workload.
- If the conventional EF order baseline omits the shared timestamps or record-source values, cross-suite comparison drift can reappear even when both benchmarks still run.

## Split Recommendations
- No split recommended; the ticket remains one bounded benchmark-project task, and the order under-specification is resolved by the explicit shared contract rather than a child ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Create the benchmark project and shared benchmark setup.

## Scope
- Benchmark customer and order scenarios.
- Keep benchmark data generation deterministic.

## Acceptance Criteria
- Benchmarks can run locally.
- Benchmark code does not require Postgres by default.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.