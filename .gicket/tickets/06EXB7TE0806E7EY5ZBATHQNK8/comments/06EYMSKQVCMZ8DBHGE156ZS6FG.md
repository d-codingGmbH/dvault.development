[gicket-bot] PO-critic review contract

Summary
- Ticket is now precise enough for developer handoff: the earlier order-benchmark ambiguity was closed with an explicit cross-suite contract, and the repo already exposes the required customer baseline, DVault APIs, .NET baseline, and SQLite-default constraints.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/description.md:57-58 shows ## Open Questions -> - none, so the open-question approval gate is clear.
- Previous blocking PO-critic feedback in .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/comments/06EYMD6BR4TD3X8K5MV65PH7J0.md:17-24 required an explicit order comparison contract, replay inclusion/exclusion, and clarification of reduced-vs-broader conventional EF scope.
- The follow-up PO refinement comment .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/comments/06EYMR7HK35B8D2WHSBTJP1RT4.md:10-15 marks critic-item-1 through critic-item-5 as answered, and the current persisted contract mirrors that resolution at .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/description.md:15-17, 39-41, and 51-54.
- benchmarks/.gitkeep is the only file under benchmarks/; docs/plans/shared-implementation-standards.md:63-71 reserves benchmarks/ for future benchmark projects; DVault.slnx:1-17 contains no benchmark project yet, so the ticket still matches a real repository gap.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj:2-7 sets the shared project baseline to net10.0 with nullable/docs enabled, and README.md:154-162 states Postgres integration is opt-in via DVAULT_TEST_POSTGRES_CONNECTION_STRING, so the ticket's SQLite/local default is directly supported by repository evidence.
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md:12-57, tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs:10-23 and 25-53, and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:353-477 define and verify the same C-100 customer history contract across the plain EF and DVault baselines.
- tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:20-105 shows the broader 2-order/3-line conventional EF scenario, while 120-226 and 250-259 show the reduced DVault order path with order-entry, two warehouse-allocation writes, and a warehouse-replay that writes 0 rows; the current ticket description explicitly scopes the broader EF case out at description.md:32 and 51 and codifies the reduced shared benchmark workload at 15-17 and 39-41.
- git log --oneline --decorate -n 4 on /mnt/c/Projects/DVault shows the active review branch at 197027a6 after the PO handoff commit <redacted>, and git show --stat --summary --format=fuller <redacted> shows that handoff updated .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/description.md, .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/ticket.json, and added the current refinement comment files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Benchmark timings will still be machine-relative, not cross-machine comparable; the ticket already frames them as local relative comparisons at .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/description.md:66-68.

AC / test suggestions
- During dev/test review, verify the documented benchmark entry runs both suites under SQLite/local with no default DVAULT_TEST_POSTGRES_CONNECTION_STRING dependency, matching description.md:37 and README.md:154-162.
- During dev/test review, verify the benchmark setup asserts the persisted outcome counts already fixed in description.md:39-41 so the timed workload cannot drift back toward the broader conventional EF reuse scenario.

Implementation watchouts
- The intended public DVault surfaces are present in source: AddDVault() at src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-25, ApplyDataVaultMetadata() at src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:29-38, and IDataVaultSaveService / DataVaultSaveRequest at src/DCoding.Data.DVault/DataVaultSaveService.cs:10-67.
- Keep the order benchmark aligned to the reduced single O-1000/SKU-COFFEE contract, not the broader conventional EF reuse example currently implemented at tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:20-105.
- Keep the unchanged warehouse-replay case out of the measured benchmark path; current repo behavior expects 0 rows for that replay at tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:206-226 and the ticket mirrors that at description.md:17, 33, and 54.
- Any benchmark project created under benchmarks/ needs solution wiring in DVault.slnx while preserving the repository's single root solution entry point per docs/plans/shared-implementation-standards.md:63-71.

Non-blocking notes
- The prompt snapshot said recent comments were absent, but the persisted ticket comment history under .gicket/tickets/06EXB7TE0806E7EY5ZBATHQNK8/comments/ is populated; the relevant current refinement trail is the prior blocking review 06EYMD6BR... followed by the answering PO refinement 06EYMR7HK....

Split recommendations
- No split recommended; the current persisted contract already narrows the work to one benchmark project with two explicit comparison suites and a shared deterministic setup.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment