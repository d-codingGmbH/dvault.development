[gicket-bot] PO-critic review contract

Summary
- Return to PO: the customer benchmark baseline is well pinned, but the order benchmark comparison is not yet specified tightly enough to support the required shared deterministic benchmark setup.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/description.md:50-51 records "## Open Questions" -> "- none".
- benchmarks/.gitkeep exists and find benchmarks -maxdepth 2 returned only benchmarks and benchmarks/.gitkeep; DVault.slnx:1-17 currently contains no benchmark project, matching README.md:138 and docs/plans/shared-implementation-standards.md:69.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj:3 sets the repository .NET baseline to net10.0; README.md:154-159 makes Postgres opt-in via DVAULT_TEST_POSTGRES_CONNECTION_STRING, so SQLite/local default is directly supported.
- Customer comparison is locked by docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md:12-55 and is implemented consistently in tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs:10-23,25-53 and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:353-477.
- The ticket claims in .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/description.md:12,19-21,33-35,44-45 that existing repository evidence already fixes both benchmark baselines and that both suites can share deterministic business keys, timestamps, record-source values, and scenario shape.
- Direct repo evidence does not fully support that order-scenario claim: tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:20-72,77-105 uses a conventional EF workload with 2 orders, 2 products, and 3 order lines, while the DVault flow in the same file at lines 111-275 uses 1 order hub, 1 product hub, 1 order-product link, 2 persisted fulfillment-history rows, and an unchanged replay at lines 206-226 that writes 0 rows.

Blocking findings
- The order benchmark comparison is under-specified. The delivery contract says current repository evidence already fixes the order baseline, but the observed conventional EF and DVault order scenarios do not share one explicit deterministic event contract or one obvious comparable workload. That forces the developer to invent the benchmark comparison at the heart of the ticket.
- Acceptance criterion 3 requires shared business keys, timestamps, record-source values, and scenario shape across benchmark suites. The observed conventional EF order test does not expose the same timestamp/record-source driven history sequence that the DVault order test uses (order-entry, warehouse-allocation, warehouse-replay), so the current contract does not tell the developer how those values are supposed to align.

Required PO actions
- Amend this ticket in place with an explicit order comparison contract, equivalent in precision to the customer-profile contract, naming the exact order/product business keys, timestamps, record sources, dataset size, and operation sequence that both the conventional EF and DVault order benchmarks must execute.
- State whether the order benchmark includes the unchanged replay case from tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:206-226 or excludes it from the measured workload.
- Clarify whether the conventional EF order benchmark should model the broader 2-order/3-line reuse scenario from tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:20-72 or a reduced single-relationship workload that directly matches the DVault order-product history path.

Open issues ledger
- critic-item-1 [required-po-action] Amend this ticket in place with an explicit order comparison contract, equivalent in precision to the customer-profile contract, naming the exact order/product business keys, timestamps, record sources, dataset size, and operation sequence that both the conventional EF and DVault order benchmarks must execute.
- critic-item-2 [required-po-action] State whether the order benchmark includes the unchanged replay case from tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:206-226 or excludes it from the measured workload.
- critic-item-3 [required-po-action] Clarify whether the conventional EF order benchmark should model the broader 2-order/3-line reuse scenario from tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:20-72 or a reduced single-relationship workload that directly matches the DVault order-product history path.
- critic-item-4 [blocking-finding] The order benchmark comparison is under-specified. The delivery contract says current repository evidence already fixes the order baseline, but the observed conventional EF and DVault order scenarios do not share one explicit deterministic event contract or one obvious comparable workload. That forces the developer to invent the benchmark comparison at the heart of the ticket.
- critic-item-5 [blocking-finding] Acceptance criterion 3 requires shared business keys, timestamps, record-source values, and scenario shape across benchmark suites. The observed conventional EF order test does not expose the same timestamp/record-source driven history sequence that the DVault order test uses (order-entry, warehouse-allocation, warehouse-replay), so the current contract does not tell the developer how those values are supposed to align.

Missing examples / edge cases
- The ticket does not currently say whether product reuse across multiple orders is part of the order benchmark scenario or whether the benchmark should narrow to one order/product pair.
- The ticket does not currently say whether a no-op replay that produces 0 DVault writes is part of the order comparison workload.
- The ticket does not currently distinguish measured benchmark work from one-time setup work such as database creation and fixture seeding.

Risky assumptions
- Assuming the developer can infer a fair order comparison workload from the current tests without introducing scenario drift.
- Assuming one shared deterministic benchmark setup can be extracted for the order scenario even though the observed conventional EF and DVault order tests are shaped differently.
- Assuming relative benchmark results remain meaningful if the two order implementations do not process the same explicit business-event contract.

AC / test suggestions
- Add one acceptance criterion or referenced contract artifact for the order comparison that lists the exact conventional EF operations and DVault operations to benchmark, with fixed values and expected history/replay behavior.
- Keep the customer benchmark tied to docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md and add an order-side equivalent rather than relying on prose like "order/product relationship flow".
- State that the documented local benchmark entry must run under SQLite with no DVAULT_TEST_POSTGRES_CONNECTION_STRING requirement, consistent with README.md:154-159.

Implementation watchouts
- Reuse the existing public DVault surfaces already present in source: AddDVault() in src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-23, ApplyDataVaultMetadata() in src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:29-38, and IDataVaultSaveService/DataVaultSaveRequest in src/DCoding.Data.DVault/DataVaultSaveService.cs:10-67.
- If the order benchmark includes replay behavior, remember that the current DVault save service suppresses unchanged latest satellite payloads; tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:224-226 already expect 0 rows for the unchanged replay.
- Any new benchmark project must be added to DVault.slnx while preserving the repository's single root solution entry point; DVault.slnx:1-17 currently has no benchmark project.

Non-blocking notes
- The formal Open Questions section is resolved to "none", so the return decision is based on contract clarity rather than the open-question gate.
- The repository already has the prerequisite scenario code on the current branch surface; git log on the review branch shows integrated scenario commits 804d0036 for 06EXB7SY3J6160R9Q35CFN6Q1W, ad703186 for 06EXB7RYFJ3YQDB1E4QHPP8034, and 550473c9 for 06EXB7S6DB97GVVTS2GGZ3CCX8.
- The follow-up blocked ticket .gicket/tickets/06EXB7TP9PF2XFRQ9MG7CJQR10 is already reserved for benchmark artifacts/documentation output, so this ticket should stay focused on the runnable benchmark project itself.

Split recommendations
- No implementation split is required if Product updates this ticket in place with a precise order comparison contract before developer handoff.
- If Product cannot express the order comparison contract concisely in this ticket, split a small contract/refinement follow-up first and keep benchmark implementation blocked on that clarification.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment