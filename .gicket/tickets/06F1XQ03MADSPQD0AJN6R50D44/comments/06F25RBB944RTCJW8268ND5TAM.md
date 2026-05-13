[gicket-bot] PO-critic review contract

Summary
- Ticket refinement matches the current DVault repository and ticket state; no blocking open questions remain, and the story is ready for developer handoff as a planning/ratification parent over already-integrated child work.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XQ03MADSPQD0AJN6R50D44/description.md has `## Open Questions` = `none` and defines this story as the parent planning ticket over child `06F1XQ0DB1PRZXNXY7NKEZCS68`.
- .gicket/tickets/06F1XQ0DB1PRZXNXY7NKEZCS68/comments/06F25KTGPVKXBC5GVZFSRME4E8.md records tester verification of `6/6` acceptance criteria and `5/5` definition-of-done items on branch `ticket/06F1XQ0DB1PRZXNXY7NKEZCS68-task-define-bulk-strategy-contract-and-fallback` at commit `6a4b7c488655`.
- Relation files `.gicket/relations/FM/44/06F1XPX99KQRB09GRQG50Z75FM--06F1XQ03MADSPQD0AJN6R50D44--parentOf.json`, `.gicket/relations/44/68/06F1XQ03MADSPQD0AJN6R50D44--06F1XQ0DB1PRZXNXY7NKEZCS68--parentOf.json`, `.gicket/relations/44/8G/06F1XQ03MADSPQD0AJN6R50D44--06F1XQ1VWEX0WPAXE78FHSWJ8G--blocks.json`, `.gicket/relations/44/ZM/06F1XQ03MADSPQD0AJN6R50D44--06F1XQ25KK4VY4MYJSDG9V4BZM--blocks.json`, and `.gicket/relations/V4/44/06F1XPRY3ZDB6W1WQ9ABRRJ2V4--06F1XQ03MADSPQD0AJN6R50D44--blocks.json` materialize the parent epic, done child, two follow-on blocked tickets, and historical incoming block from done epic `06F1XPRY3ZDB6W1WQ9ABRRJ2V4`.
- `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs:10-33` exposes `IDataVaultProviderSaveStrategy`; `:39-109` exposes `DataVaultProviderSaveStrategyContext` with `DbContext`, ordered `Requests`, `ResolvedRequests`, `IStableHashService`, and `IStableHashNormalizer`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:834-876` sorts provider strategies by descending `Priority`, routes both single-save and `DataVaultBulkSaveRequest` paths through `SaveRequestsAsync`, calls `CanSave`, and only then selects provider strategy execution; `:879-910` is the provider-neutral fallback writer.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:631-637` analyzes `DataVaultBulkSaveRequest`; `:793-878` reports `ProviderStrategySelected` vs `ProviderNeutralFallback`, candidate ordering, and fallback causes.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:60-84,87-112,142-166,300-367` and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs:153-178,236-266` cover no-strategy fallback, compatible strategy selection, ordered bulk evaluation, descending-priority dispatch, and equal-priority registration-order ties.
- Provider packages already register strategy baselines in `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-31`, `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-24`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:15-23`, `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-26`, and `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:15-23`; benchmark posture is explicitly bounded in `benchmarks/DCoding.Data.DVault.Benchmarks/README.md:9-18,66-87`.
- `git log --graph --max-count=20 --all --grep='06F1XQ03MADSPQD0AJN6R50D44|06F1XQ0DB1PRZXNXY7NKEZCS68'` shows child ticket `06F1XQ0DB1PRZXNXY7NKEZCS68` auto-integrated into `develop` at `21b25286d`, while current story branch head is `e65c42db4`; `git diff --name-only develop...HEAD` lists only `.gicket/tickets/06F1XQ03MADSPQD0AJN6R50D44/**` files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developer handoff assumes implementers follow the delivery contract, not just the story title and legacy draft; the title still says 'Add optional provider bulk insert strategy SPI' while the contract ratifies the existing `IDataVaultProviderSaveStrategy` surface.
- This parent story assumes no new repository implementation is required on its own branch because the integrated child ticket already carries the core contract/test slice and the current story-branch diff is ticket-only.

AC / test suggestions
- Use `DataVaultSaveStrategySelectionTests`, `DataVaultDiagnosticsIntegrationTests`, and `ExplicitDataVaultSaveServiceTests`/`ExplicitDataVaultSaveServiceSqliteTests` as the named acceptance anchors during dev/test handoff.
- If any follow-up work expands beyond the existing child ticket, add an explicit acceptance statement that such work belongs to `06F1XQ1VWEX0WPAXE78FHSWJ8G` or `06F1XQ25KK4VY4MYJSDG9V4BZM`, not this parent story.

Implementation watchouts
- Do not introduce a second `IDataVaultProviderBulkInsertStrategy`-style surface; a repository search for `IDataVaultProviderBulkInsertStrategy|BulkInsertStrategy` under `src`, `tests`, and `docs` returned no matches, and the existing SPI is already public.
- Keep the core dispatcher provider-neutral; `rg -n 'ProviderName' src/DCoding.Data.DVault/DataVaultSaveService.cs` returned no matches, and `docs/architecture/dvault-v1-explicit-save-service.md:31-35` says provider-name branching belongs outside the core service.
- Preserve deterministic diagnostics and tie-break behavior: descending `Priority`, DI order for ties, and request-bound fallback causes for both single and bulk saves.

Non-blocking notes
- Current branch history is ticket-management only: the visible delta from `develop` is confined to `.gicket/tickets/06F1XQ03MADSPQD0AJN6R50D44/**`.
- The historical incoming `blocks` relation from done epic `06F1XPRY3ZDB6W1WQ9ABRRJ2V4` is still materialized, but the source ticket is `done` and the contract already treats it as non-blocking.

Split recommendations
- No split needed; child `06F1XQ0DB1PRZXNXY7NKEZCS68` already owns the core contract/fallback-test slice, and follow-on proof work remains in `06F1XQ1VWEX0WPAXE78FHSWJ8G` and `06F1XQ25KK4VY4MYJSDG9V4BZM`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment