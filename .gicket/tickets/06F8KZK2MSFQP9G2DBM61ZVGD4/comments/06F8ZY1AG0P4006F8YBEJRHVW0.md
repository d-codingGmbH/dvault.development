[gicket-bot] PO-critic review contract

Summary
- Approved for dev: the contract is bounded, Open Questions is none, and the repository already defines the benchmark harness, current triplet, verifier surface, and provider-read strategy boundaries needed for this row-extension task.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZK2MSFQP9G2DBM61ZVGD4/description.md:17-44 scopes work to benchmark artifact generation, verifier coverage, skipped-row semantics, and measured-evidence wording, and .gicket/tickets/06F8KZK2MSFQP9G2DBM61ZVGD4/description.md:46-47 says Open Questions is none.
- git diff --name-only develop..HEAD lists only .gicket/tickets/06F8KZK2MSFQP9G2DBM61ZVGD4/**, and git --no-pager log --oneline --decorate -5 shows only ticket-local claim/handoff commits on this branch before develop at 8e66849bd; this is a normal pre-development PO gate state.
- benchmark-summary.md:49-54 already carries the required SQLite latest-satellite-read, pit-as-of-read, and bridge-traversal-read rows, giving the exact current row identities the ticket says to extend.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:19-23, :122-156, and :773-823 already define the artifact triplet headers, current SQLite read rows, and cross-format/context parsing and consistency checks, so the verifier surface is concrete and bounded.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md:16-18 documents the current benchmark posture: optional providers already use skipped-row semantics, SQLite read rows are measured today, and non-SQLite provider-specific PIT/bridge read evidence is not yet emitted in the default matrix.
- Direct source evidence exists for the provider-read boundary: src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:28-33 registers latest/PIT/bridge SQLite read strategies, while src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:21-25, src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:24-28, and the analogous SQL Server/Oracle extensions register PIT/bridge read strategies only; benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:78-85 currently maps a named benchmark read strategy only for SQLite.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not explicitly spell out the exact optional-provider read row keys by scenario + provider + baseline; dev will need to derive them from the existing read-strategy surfaces and benchmark row conventions.
- The ticket text names unset connection strings, but the current benchmark README also mentions dependency-unavailable and connection-open-failure skipped cases; keeping those normalized skip modes aligned is an edge case to watch.

Risky assumptions
- Assumes optional-provider read rows are required only where a provider-specific read surface already exists; the direct source boundary today is SQLite latest/PIT/bridge plus non-SQLite PIT/bridge, not non-SQLite latest-satellite.
- Assumes skipped optional-provider rows can be surfaced without changing the shared artifact schema, because the existing contract already covers executionStatus, skipReason, iterations=0, executionDetail, and persistedOutcome=not executed.
- Assumes the stale blocked-by wording in the ticket description is informational only, since the persisted ticket state has isBlocked=false and the upstream blocker is already done.

AC / test suggestions
- Make the verifier enumerate the exact required provider-read row keys and assert them across markdown, CSV, and JSON so later artifact refreshes cannot silently narrow provider coverage.
- Keep one assertion that any prose or summary touched by the task still presents SQLite as the only measured optimized read provider and treats skipped optional-provider rows as non-performance evidence.
- Add an explicit expected-skipped-row assertion for each optional provider read lane, including blank markdown/CSV metric cells, JSON null metrics, iterations=0, explicit skip reason, and persistedOutcome=not executed.

Implementation watchouts
- Current read benchmark construction lives in benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs:215-245; that surface currently instantiates only SQLite read benchmarks, so row-extension work must stay inside the harness/evidence boundary without drifting into new runtime read-strategy implementation.
- Current benchmark helper strategy naming in benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:78-85 is SQLite-only for read strategies, so planned executionDetail strings for optional providers need to be grounded in the real PIT/bridge strategy names rather than inferred from save-path naming.
- The existing benchmark verifier already parses optional-provider context and triplet row consistency in tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:779-823; changes that update rows but not context consistency will regress the checked-in artifact contract.

Non-blocking notes
- The PO refinement comment .gicket/tickets/06F8KZK2MSFQP9G2DBM61ZVGD4/comments/06F8ZVGSJ7JAFTFAECZRJQTMTW.md:3-18 records the intended handoff to po-critic and that the durable refinement contract was updated on this ticket branch.
- No split is needed from a ticket-quality perspective once the existing benchmark harness and verifier surfaces are used as the implementation boundary.

Split recommendations
- No split recommended; the repository already isolates this as one harness/verifier/evidence task separate from provider implementation and broader documentation work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment