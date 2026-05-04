[gicket-bot] PO-critic review contract

Summary
- Ticket is refined enough for developer handoff: the repository already exposes the provider-strategy boundary, the persisted contract has no open questions, and the SQL Server work is cleanly scoped to the provider package with fallback preservation and separate smoke coverage.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NAMGKJ63WCXAK1J7B08TR/description.md contains `## Open Questions` with `- none`, so the persisted delivery contract has no unresolved open questions.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs directly defines `IDataVaultProviderSaveStrategy` and `DataVaultProviderSaveStrategyContext`, which is the public strategy boundary named in the ticket.
- src/DCoding.Data.DVault/DataVaultSaveService.cs dispatches registered provider strategies by descending `Priority` and falls back when none accept, which matches the contract's stated integration point for SQL Server-specific behavior.
- src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs currently only calls `services.AddDVault();`, so the gap this ticket addresses is localized and concrete: SQL Server strategy registration does not exist yet.
- docs/architecture/dvault-v1-explicit-save-service.md states provider-specific SQL belongs in provider packages and lists SQL Server as `Compatibility baseline only through AddDVaultSqlServer()`, which aligns with the ticket's scope-in and required expectation updates.
- tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs currently asserts `AddDVaultSqlServer()` registers no provider strategy, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.SqlServer.approved.txt currently exposes only `AddDVaultSqlServer`; the ticket contract already anticipates the localized test and snapshot changes this implementation will require.
- git log and git diff on `ticket/06EZ0NAMGKJ63WCXAK1J7B08TR-task-implement-sql-server-set-based-save-strateg` show only ticket metadata / handoff commits (for example `a11e4ab9`, `4fc543a0`, `4975e634`) and no source implementation yet, so there is no partial code on this branch creating scope ambiguity.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj references `Microsoft.EntityFrameworkCore.Sqlite` and optional Npgsql only; there is no default SQL Server live harness in the repo today, which supports keeping repeatable SQL Server smoke coverage in sibling ticket `06EZ0NAWNDDEP32P497E39MQXR`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not spell out a concrete mixed ordered batch example containing reused hub/link rows plus unchanged and changed satellite rows; tests should lock that behavior explicitly.
- Unsupported optimized-path shapes are intentionally open-ended beyond dirty contexts, so at least one explicit negative-shape example should be documented through tests.

Risky assumptions
- The SQL Server optimized path can remain isolated in `src/DCoding.Data.DVault.SqlServer` even though `DataVaultEfMetadataTranslator` still defaults to `DataVaultProviderCapabilityProfiles.Sqlite` today.
- Non-live coverage will catch SQL text and parameter-shape regressions before sibling ticket `06EZ0NAWNDDEP32P497E39MQXR` adds repeatable live SQL Server smoke validation.

AC / test suggestions
- Update `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs` so `AddDVaultSqlServer()` now expects a provider strategy while preserving the core `IDataVaultSaveService` registration assertion.
- Add strategy-selection coverage analogous to `tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs` proving clean SQL Server contexts select the strategy and dirty or unsupported contexts fall back.
- Add batch-order assertions covering inserted and reused hub/link rows plus unchanged and changed satellite rows so `RowsWritten` and `SavedRecords` ordering remain pinned.
- Add a provider SQL execution contract analogous to `tests/DCoding.Data.DVault.Tests/Shared/ProviderSqlExecutionContract.cs` for parameterization, transaction participation, and cancellation behavior without requiring a default-on live SQL Server instance.

Implementation watchouts
- Keep provider-specific SQL and provider-name checks out of `src/DCoding.Data.DVault`; both the architecture doc and the ticket contract reserve them for the SQL Server provider package.
- Do not satisfy the ticket with hidden per-row existence probes behind raw SQL; the contract explicitly requires set-based hub/link existence detection for the batch.
- Preserve the satellite latest-hash-diff insert-only semantics already visible in `DefaultDataVaultSaveService` and `SqliteDataVaultSaveStrategy`.
- If any public SQL Server surface changes, update XML docs and `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.SqlServer.approved.txt`.

Non-blocking notes
- README.md and docs/architecture/dvault-v1-explicit-save-service.md still describe SQL Server as fallback-only today; narrow expectation updates are already anticipated by the contract.
- A prior po-critic attempt failed only because its model response was not parseable (`.gicket/tickets/06EZ0NAMGKJ63WCXAK1J7B08TR/comments/06EZ3R5JQTDXMN5MG7V1MDRW6C.md`), not because of ticket-content ambiguity.

Split recommendations
- Keep repeatable opt-in SQL Server smoke/configuration work in sibling ticket `06EZ0NAWNDDEP32P497E39MQXR`.
- Keep any broader architecture or documentation refresh beyond brief expectation updates with the parent SQL Server optimization story rather than enlarging this implementation ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment