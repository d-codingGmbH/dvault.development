[gicket-bot] PO refinement contract

Summary
- Refined the ticket with an explicit order benchmark contract: compare one O-1000/SKU-COFFEE relationship workload, measure two fulfillment-state changes, exclude unchanged replay, and keep the work unsplit.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket now fixes one shared order comparison contract. Both benchmark suites must use order business key O-1000 and product business key SKU-COFFEE, create their relationship at <redacted>-01T09:30:00Z from record source order-entry, persist fulfillment state Backordered/NORTH-1 at <redacted>-01T10:00:00Z from warehouse-allocation, then persist fulfillment state Allocated/NORTH-1 at <redacted>-01T10:45:00Z from warehouse-allocation. The measured dataset is 1 order, 1 product, 1 order-product relationship, and 2 measured fulfillment-history events.
- critic-item-2: `answered` - The unchanged replay at <redacted>-01T11:15:00Z from record source warehouse-replay is excluded from the measured benchmark workload. It may remain as a non-timed correctness reference, but the required benchmark contract and persisted benchmark outcomes stop after the two changed fulfillment events.
- critic-item-3: `answered` - The conventional EF order benchmark should not model the broader 2-order/3-line reuse scenario from NormalEfOrderProductSqliteTests.cs:20-72. It should use a reduced single-relationship workload that directly matches the DVault order-product history path: one order O-1000, one product SKU-COFFEE, one relationship, and two measured fulfillment history rows.
- critic-item-4: `answered` - The under-specification finding is resolved by replacing the generic order-baseline reference with one deterministic cross-suite contract. The developer no longer has to invent the comparison workload; the ticket now names the exact business keys, timestamps, record sources, dataset size, operation sequence, and required persisted outcomes for the order benchmark.
- critic-item-5: `answered` - Acceptance criterion 3 is now aligned by requiring the conventional EF order benchmark to carry the same order-entry and warehouse-allocation timestamps and record-source values as the DVault benchmark. Both suites share <redacted>-01T09:30:00Z order-entry, <redacted>-01T10:00:00Z warehouse-allocation, and <redacted>-01T10:45:00Z warehouse-allocation, while the <redacted>-01T11:15:00Z warehouse-replay case stays out of measured benchmark results.

Clarifications
- Current repository evidence already fixes the customer benchmark baseline through docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md and the existing customer integration tests.
- The repository still has no benchmark project; benchmarks/ remains a placeholder directory, so this ticket owns creating the first benchmark executable there.
- SQLite/local remains the v1 default provider baseline. Postgres stays opt-in through DVAULT_TEST_POSTGRES_CONNECTION_STRING and is not required for default benchmark execution.
- The v1 order benchmark contract uses one order business key O-1000 and one product business key SKU-COFFEE, with a measured dataset of 1 order, 1 product, 1 order-product relationship, and 2 fulfillment history events.
- Both order benchmark suites must execute the same measured sequence: create the order/product relationship at <redacted>-01T09:30:00Z from order-entry, persist Backordered/NORTH-1 at <redacted>-01T10:00:00Z from warehouse-allocation, then persist Allocated/NORTH-1 at <redacted>-01T10:45:00Z from warehouse-allocation.
- The unchanged replay at <redacted>-01T11:15:00Z from warehouse-replay is outside the measured benchmark workload and must not add a third persisted history row in either comparison baseline.
- No bounded planning document, attachment, or child ticket was materialized during this refinement because the ticket remains one bounded benchmark task.

Scope In
- Create one benchmark executable project under benchmarks/ using the repository net10.0 baseline and add it to DVault.slnx.
- Implement shared deterministic benchmark setup and data generation used by both scenario suites.
- Benchmark a conventional EF baseline against the DVault flow for the customer profile history scenario using the existing customer comparison contract.
- Benchmark a conventional EF baseline against the DVault flow for the reduced order/product relationship history scenario using O-1000, SKU-COFFEE, order-entry at <redacted>-01T09:30:00Z, and warehouse-allocation events at <redacted>-01T10:00:00Z and <redacted>-01T10:45:00Z.
- Document one local command or entry path for unattended benchmark execution without extra infrastructure.

Scope Out
- Any benchmark path that requires Postgres, Docker, or DVAULT_TEST_POSTGRES_CONNECTION_STRING by default.
- New product behavior in src/DCoding.Data.DVault beyond the minimal support needed to run the benchmark scenarios.
- CI performance gates, published benchmark history, or enforced regression thresholds.
- Additional Data Vault expansion comparisons such as PIT, bridge, multi-active satellite, or provider-specific optimization benchmarks.
- Measuring the broader 2-order/3-line conventional EF reuse scenario from tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:20-72 inside this ticket's required order comparison.
- Timing the unchanged warehouse-replay case at <redacted>-01T11:15:00Z as part of the required benchmark workload.

Open questions
- none

Follow-up questions
- Should a follow-up ticket benchmark the broader 2-order/3-line conventional EF reuse scenario from NormalEfOrderProductSqliteTests.cs as a separate relationship-reuse benchmark after this reduced cross-suite history comparison lands?
- Should a later ticket add CI-hosted benchmark reporting or regression thresholds once the first local benchmark project exists?
- After the SQLite/local v1 benchmark lands, is there product value in provider-specific benchmark tickets for Postgres or other adapters?

Risks
- Benchmark numbers will still vary by developer machine; this ticket should establish deterministic relative comparison coverage, not a cross-machine performance gate.
- If the benchmark implementation drifts from the ratified order contract or accidentally times the broader reuse scenario, the reported comparison will stop representing the intended workload.
- If the conventional EF order baseline omits the shared timestamps or record-source values, cross-suite comparison drift can reappear even when both benchmarks still run.

Split recommendations
- No split recommended; the ticket remains one bounded benchmark-project task, and the order under-specification is resolved by the explicit shared contract rather than a child ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment