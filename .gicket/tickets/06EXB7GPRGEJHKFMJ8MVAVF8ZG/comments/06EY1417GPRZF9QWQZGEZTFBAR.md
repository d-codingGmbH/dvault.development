[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the refined contract matches the current SQLite/EF Core 10 repository baseline, explicitly scopes migration work out, and leaves no unresolved PO questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06EXB7GPRGEJHKFMJ8MVAVF8ZG revision 06EY133ZPVRBTHGZ721M504Y1G contains `## Open Questions` = `none` and PO handoff decision `ready_for_po_critic`.
- gicket-read-ticket-comments returned 14 workflow/refinement comments, including the PO refinement comment that restates the SQLite-only baseline and the migration scope-out.
- Branch history `git log --oneline --decorate -n 12 ticket/06EXB7GPRGEJHKFMJ8MVAVF8ZG-task-add-schema-and-migration-snapshot-tests` shows only PO/PO-critic handoff commits on this branch (HEAD 35eb6d84; earlier e969446e/27cf2565), with the last code baseline coming from `develop` at c0ace7dd for upstream ticket 06EXB7GESWZZTZG7XYAKTTKQRW.
- `git diff --name-only c0ace7dd..35eb6d84` lists only `.gicket/...` ticket files; no `src/` or `tests/` files have been changed on this ticket branch yet.
- `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` already contains the exact SQLite integration harness and negative baseline the contract references: `ApplyDataVaultMetadataCreatesExpectedSqliteSchema()` and `UseDataVaultAloneDoesNotCreateDataVaultTablesInSqlite()`.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` pins `Microsoft.EntityFrameworkCore.Sqlite` `10.0.0`, and `DVault.slnx` includes that integration project, matching the ticket's EF Core 10 / solution-entry assumptions.
- `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` publicly exposes `UseDataVault(ModelBuilder)` and `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)`, the exact APIs named in the contract.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` requires at least two link endpoints, and `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` iterates `link.Participants` to build link columns/indexes, so the repo supports the representative hub/link/satellite metadata surface the ticket targets.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` already contains `ApplyDataVaultMetadataPreservesDeclaredBusinessKeyOrder()` and `ApplyDataVaultMetadataTranslatesLinkParentSatellites()`, directly anchoring the business-key-order and link-satellite cases called out in the contract.
- `rg -n "Microsoft\.EntityFrameworkCore\.Design|MigrationBuilder|ModelSnapshot|__EFMigrationsHistory|Add-Migration|dotnet ef" /mnt/c/Projects/DVault` returned no matches, supporting the contract's decision to scope migration snapshots and design-time tooling out of this ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The ticket title still says `schema and migration snapshot tests`; approval assumes implementers follow the authoritative delivery contract text, which explicitly excludes migration snapshot work for this ticket.
- Approval assumes a two-endpoint relationship is sufficient for the contract's `multi-participant link` example, because the current repo evidence centers on two-participant `CustomerOrder` link shapes even though the metadata API supports 2+ endpoints.
- Approval assumes a committed baseline may be either plain text files or another equally reviewable source-controlled format, because the contract expresses a preference rather than a single mandatory snapshot artifact format.

AC / test suggestions
- Keep the business-key ordering case explicit by using a fixture like the existing `Customer Id` + `Source System` ordering pattern from `DataVaultEfMetadataTranslationTests.cs` so naming or index-order drift is visible.
- Require snapshot failures to show focused table/column/key/index deltas rather than a single opaque blob mismatch.
- Keep the `UseDataVault()` negative baseline as its own named case instead of burying it inside the positive schema snapshot.

Implementation watchouts
- Do not add `Microsoft.EntityFrameworkCore.Design`, migration files, or `dotnet ef` scaffolding; the current repository shows no migration baseline to extend.
- Reuse `SqliteTestDatabase` and the existing `tests/DCoding.Data.DVault.Tests/Integration` path instead of introducing a second SQLite test harness.
- Canonicalize schema output before comparison; raw provider DDL from `sqlite_master` alone would be brittle across EF Core or SQLite version changes.
- Keep snapshot content readable enough that naming/order regressions can be reviewed directly in source control.

Non-blocking notes
- The branch is currently ticket-metadata-only since `develop`; no code implementation is already in flight on this ticket branch.
- The delivery contract already points at the correct repository entry points: `DVault.slnx`, `tests/DCoding.Data.DVault.Tests`, and the existing SQLite integration project.
- Renaming the ticket later to remove `migration` would reduce ambiguity, but it is not required for developer handoff because the persisted contract text is clear.

Split recommendations
- Keep provider-specific migration-script snapshots in a follow-up ticket once EF design-time or migration infrastructure exists; current repository search found no migration baseline or `Microsoft.EntityFrameworkCore.Design` usage.
- If equivalent coverage is later needed for additional providers, split by provider instead of widening this SQLite-focused ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment