[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff. The persisted refinement contract is explicit and repository-backed, `## Open Questions` is `none`, and the cited source/docs already support the save-boundary, fallback, diagnostics, and benchmark-gate claims.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSC8TS7R98ZEBDKE5XG2KTC/description.md:1-67` contains the persisted Delivery Contract; `PO Handoff` is `ready_for_po_critic` at lines 7-9 and `## Open Questions` is `none` at lines 51-52.
- The ticket comment set under `.gicket/tickets/06FBSC8TS7R98ZEBDKE5XG2KTC/comments/` is automation/PO-handoff only; comment `06FCVHE3ETSB30KW9MD1TCK7GW.md` restates the refinement contract and no comment raises an unresolved scope objection.
- `git log --max-count=5 main..HEAD` shows the branch tip is ticket-workflow commits `c952fa20b`, `8a1a82f5c`, `38742387e`, and `8b4354cb5` on top of `develop` commit `1d6daced6`; `git diff --name-only main..HEAD` produced no non-`.gicket` paths, so this branch changes ticket metadata only.
- Direct source evidence for the existing public write boundary exists in `src/DCoding.Data.DVault/IDataVaultSaveService.cs:13-60` and `src/DCoding.Data.DVault/IDataVaultProviderSaveStrategy.cs:10-33`; per-chunk dispatch through the same service boundary is visible in `src/DCoding.Data.DVault/DefaultDataVaultSaveService.cs:249-339`, and provider extension entry points already exist in `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs`.
- `docs/architecture/dvault-v1-explicit-save-service.md:38-50,60-64,80-84` documents caller-owned transaction behavior, provider-neutral fallback, provider-name/clean-context/no-multi-active gate conditions, and the supported provider baseline including DB2.
- `docs/performance-profiles.md:17,113,275,287,330` requires exact-request diagnostics and benchmark artifacts for provider-specific claims, and `docs/releases/v0.32.0.md:49,74-80` ties threshold claims to the exact provider/workload plus preserved diagnostics and artifact-triplet evidence.
- `benchmark-summary.md:10-15,63-74` shows PostgreSQL, SQL Server, MySQL, Oracle, and DB2 optional provider rows are currently `skipped`/`not executed` when connection strings are unset, matching the contract's statement that the root triplet alone cannot satisfy a new provider-bulk acceptance gate.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Future provider implementation tickets will pin one exact provider/workload comparator and preserved artifact bundle before development starts; this story intentionally leaves that selection to later ticket creation.
- Implementers will read `ordered explicit bulk batch or per-chunk ordered batch` consistently with the current `DefaultDataVaultSaveService` behavior and not as approval for a new provider-native chunk-execution surface.

AC / test suggestions
- For each future provider ticket, require one accepted-batch diagnostics example and one declined-batch diagnostics example showing provider name, strategy status, selected strategy or fallback causes, and redacted observability output.
- Require the preserved benchmark artifact triplet plus any provider-specific threshold note for the exact provider/workload before a child ticket may claim a native bulk path.
- Include explicit fallback evidence for dirty contexts, provider-name mismatch, multi-active satellite batches, and missing benchmark evidence so later reviews can verify semantic parity and bounded fallback.

Implementation watchouts
- The root `benchmark-summary.md` is only a row-identity and SQLite-local quick baseline; external-provider `provider-native-bulk-ingestion` rows are skipped there and must not be cited as completed threshold evidence.
- Keep future work inside the existing `IDataVaultSaveService` plus DI-selected `IDataVaultProviderSaveStrategy` boundary; do not widen into `SaveChanges` interception, runtime SQL artifact dispatch, or a new public API.
- Chunked saves remain caller-ordered and caller-transaction-owned; future provider work must preserve per-chunk ordering/cancellation semantics and must not auto-open, commit, or roll back transactions.

Non-blocking notes
- The branch contains no repository doc/source changes beyond `.gicket` metadata, so the ticket is correctly acting as a contract-only refinement gate rather than mixing PO scope with implementation.

Split recommendations
- No further split is needed for this acceptance-contract story.
- Keep later implementation work per provider and split runtime save-strategy changes from design-time SQL artifact review when both are proposed.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment