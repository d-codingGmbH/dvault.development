[gicket-bot] PO refinement contract

Summary
- Refined the benchmark ticket into one bounded benchmark-project task: create the first benchmarks project, compare the repository's existing customer and order-centric scenarios, keep SQLite/local execution as the v1 default, and avoid any split or planning artifact writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already fixes the v1 benchmark comparison baselines: the customer profile history scenario is covered by the plain EF customer-history test and the DVault explicit-save customer satellite tests, while the order-centric scenario is the order/product relationship flow covered by the plain EF order/product test and the DVault order-product link-plus-satellite tests.
- The repository does not already contain a benchmark project; `benchmarks/` is still a reserved placeholder directory, so this ticket owns creating the first benchmark project there rather than extending an existing harness.
- The current local provider baseline is SQLite, and Postgres remains opt-in through `DVAULT_TEST_POSTGRES_CONNECTION_STRING`; benchmark execution for this ticket must therefore run without Postgres by default.
- Incoming completed `blocks` work for the customer-profile and link/satellite scenario tickets means this benchmark ticket can build on already-landed scenario code instead of inventing new domain cases.

Scope In
- Create one benchmark executable project under `benchmarks/` using the repository's .NET 10 baseline and add it to `DVault.slnx`.
- Implement shared deterministic benchmark setup and data generation used by both scenario suites.
- Benchmark a conventional EF baseline against the DVault flow for the customer profile history scenario.
- Benchmark a conventional EF baseline against the DVault flow for the order/product relationship scenario.
- Document one local command or entry path for running the benchmarks without extra infrastructure.

Scope Out
- Any benchmark path that requires Postgres, Docker, or `DVAULT_TEST_POSTGRES_CONNECTION_STRING` by default.
- New product behavior in `src/DCoding.Data.DVault` beyond the minimal support needed to run the benchmark scenarios.
- CI performance gates, published benchmark history, or enforced regression thresholds.
- Additional Data Vault expansion comparisons such as PIT, bridge, multi-active satellite, or provider-specific optimization benchmarks.

Open questions
- none

Follow-up questions
- Should a later ticket add CI-hosted benchmark reporting or regression thresholds once the first local benchmark project exists?
- After the SQLite/local v1 benchmark lands, is there product value in separate provider-specific benchmark tickets for Postgres or other adapters?

Risks
- Benchmark numbers will still vary by developer machine; this ticket should establish deterministic relative comparison coverage, not a cross-machine performance gate.
- If the benchmark implementation drifts from the scenario shapes already proven in integration tests, its results may become less representative of the intended customer and order workflows.

Split recommendations
- No split recommended; the work is bounded to one benchmark project, two repository-backed scenario suites, and shared deterministic setup.

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