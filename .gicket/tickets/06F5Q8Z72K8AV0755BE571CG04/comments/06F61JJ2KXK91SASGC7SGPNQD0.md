[gicket-bot] PO-critic review contract

Summary
- Contract is consistent with the observed SQL Server provider boundary and dependency state; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q8Z72K8AV0755BE571CG04/description.md` contains `PO Handoff` = `ready_for_po_critic` and `## Open Questions` = `none`.
- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` registers `SqlServerDataVaultSaveStrategy` via `AddDVaultSqlServer()`, while `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs` and `src/DCoding.Data.DVault/DataVaultSaveService.cs` confirm the existing provider-strategy boundary and public single/bulk/chunked `IDataVaultSaveService` overloads referenced by the contract.
- `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs` currently uses set-based insert helpers and `OPENJSON`; `rg -n "SqlBulkCopy" /mnt/c/Projects/DVault` returned no hits, so the staged/`SqlBulkCopy`-style path described by this story is not already present in the repository.
- `tests/DCoding.Data.DVault.Tests/Integration/SqlServerIntegrationTestConfiguration.cs` gates live SQL Server validation behind `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, and `benchmark-summary.md` plus `benchmark-summary.csv`/`benchmark-summary.json` already preserve visible skipped SQL Server provider rows when that variable is not configured.
- `git log --oneline --decorate --max-count=8 --graph ticket/06F5Q8Z72K8AV0755BE571CG04-story-implement-sql-server-staged-bulk-save-stra` shows only PO/PO-critic workflow commits at the branch tip (`5a292203c`, `088ef4578`, `b0d62c129`), and `git diff --name-only develop..ticket/06F5Q8Z72K8AV0755BE571CG04-story-implement-sql-server-staged-bulk-save-stra` lists only `.gicket` metadata files on this branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developers will interpret `equivalent SQL Server-native transfer mechanism` as a staged bulk-transfer requirement and not as the already-present `OPENJSON` insert path.
- At least one opt-in validation lane will run against a real SQL Server instance often enough to prove staging cleanup, cancellation, and caller-owned transaction behavior under live conditions.

AC / test suggestions
- Make evidence explicit for one eligible ordered bulk batch that selects the SQL Server staged path and one declined batch that falls back through the provider-neutral writer.
- Verify success, exception, and cancellation cleanup behavior with caller-owned transactions still participating in the operation.
- If benchmark artifacts are refreshed, keep the visible skipped SQL Server rows when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset and add configured SQL Server provider rows when it is set.

Implementation watchouts
- This is a refinement of the existing `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs` boundary, not a new public `IDataVaultSaveService` entry point or a parallel save pipeline.
- Keep compatibility with the current SQL Server gate semantics already visible in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` and `docs/architecture/dvault-v1-explicit-save-service.md`.
- Follow-up benchmark ticket `06F5Q900FC0P3HBZP81CVK7264` already depends on this story, so any benchmark work here should stay at the contract-preserving evidence-row level rather than expanding into matrix or regression-budget scope.

Non-blocking notes
- none

Split recommendations
- No split recommended; the current contract still fits one bounded provider-specific implementation story plus the existing opt-in evidence obligations.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment