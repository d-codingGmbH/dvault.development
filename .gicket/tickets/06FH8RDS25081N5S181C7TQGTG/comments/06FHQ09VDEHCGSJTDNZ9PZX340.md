[gicket-bot] PO-critic review contract

Summary
- Contract is developer-ready: there are no persisted open questions, the scope boundaries are explicit, and local repository evidence matches the cited provider-registration, parity/fallback, and closure-bundle surfaces.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FH8RDS25081N5S181C7TQGTG/description.md` has `## Open Questions` -> `- none` and explicit Scope Out entries for save-path work, docs/performance guidance, PIT maintenance/bridge maintenance push-down, and fresh benchmarking.
- Provider registrations exist in `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:24-26`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:25-27`, `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:28-30`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:24-26`, `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs:24-26`, and `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:31-33`, each wiring `IDataVaultProviderReadStrategy`, `IDataVaultProviderPitReadStrategy`, and `IDataVaultProviderBridgeReadStrategy`.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs:16,112,231,289,367` covers latest-satellite, PIT, bridge, Postgres latest-satellite as-of, and binary hash-key parity against provider-neutral reads.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs` asserts finite fallback causes for provider mismatch, unsupported shapes, incomplete read-shape evidence, and stale maintenance across providers (for example lines 182-196, 505-528, 678-701, 713-758).
- `benchmark-summary.md:8-15` records PostgreSQL/SQL Server/MySQL/Oracle/DB2 as skipped when connection strings are unset, and `benchmark-summary.md:66-75` shows the external latest/PIT/bridge rows as visible `not executed` placeholders with planned strategies.
- `artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/README.md` lists completed latest/PIT/bridge read timings for PostgreSQL, SQL Server, MySQL, Oracle, and DB2, and `docs/plans/provider-optimization-evidence-matrix.md:321-335` records those rows as `completed-timing`.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:<redacted>,<redacted>,<redacted>` asserts that the evidence matrix and gap matrix cite the 2026-06-23 closure bundle and do not keep PIT/bridge evidence-gap rows open for the covered providers.
- `git log --oneline -- .gicket/tickets/06FH8RDS25081N5S181C7TQGTG` on branch `ticket/06FH8RDS25081N5S181C7TQGTG-task-close-selected-provider-latest-satellite-pi` shows recent commits `d85cfdd80`, `d13008332`, and `f0d04a309` are handoff/lease metadata; `git diff --name-only 6b6b5b019..f0d04a309` changed only `.gicket/tickets/06FH8RDS25081N5S181C7TQGTG/*`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developers will treat root `benchmark-summary.*` external-provider read rows as skipped placeholders and will cite `artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/` for completed external-provider timing, as required by the ticket contract and `docs/plans/provider-optimization-evidence-matrix.md`.
- Developers will not reopen PIT maintenance, bridge maintenance push-down, save-path parity, or documentation work that the ticket description explicitly marks out-of-scope and assigns to sibling tickets `06FH8RC9F0QEWF356WF7YYNNGM` and `06FH8REKX113JRZQ42HEB1NVZ8`.

AC / test suggestions
- Developer handoff should keep pointing to the exact repository anchors already named in the contract: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs`.
- When later comments or closure evidence claim external-provider timing closure, cite the provider-specific closure-bundle artifact file for the affected lane, not only the root `benchmark-summary.*` placeholder rows.

Implementation watchouts
- `git diff --name-only 6b6b5b019..f0d04a309` is metadata-only for this ticket, so developers should treat the repository baseline cited in the contract as the implementation anchor rather than expect fresh source changes from the handoff commits.
- Oracle read timing claims are tied to the existing `InitialLOBFetchSize`/`FetchSize` tuning in `src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs:85-89`, and DB2 read closure is tied to the checked-in 2026-06-23 closure bundle; widening either lane beyond the documented bounds would exceed this ticket.

Non-blocking notes
- Comment files under `.gicket/tickets/06FH8RDS25081N5S181C7TQGTG/comments/` are bot orchestration/refinement comments only; no later ticket discussion adds new unresolved PO questions.
- The read/save/doc split remains intact in persisted tickets: parent story `06FH8R9DPSKTNYB46HHVJMZ9P8` plus sibling tasks `06FH8RC9F0QEWF356WF7YYNNGM` and `06FH8REKX113JRZQ42HEB1NVZ8` all exist as separate `todo` tickets.

Split recommendations
- Do not split this read ticket further; the persisted contract already keeps save-path and documentation work in sibling tickets.
- If later work is needed, keep DB2 PIT full-rebuild maintenance as a separate follow-up instead of reopening the closed latest-satellite/PIT/bridge read closure rows.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment