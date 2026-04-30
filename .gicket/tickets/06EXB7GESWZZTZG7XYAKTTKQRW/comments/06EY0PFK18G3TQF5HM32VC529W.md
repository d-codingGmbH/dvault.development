[gicket-bot] PO-critic review contract

Summary
- Source-backed SQLite mapping contract is detailed enough for developer handoff; remaining gaps are non-blocking assumptions around schema-level naming inspection and story-level gating.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git log --oneline --decorate -n 4 ticket/06EXB7GESWZZTZG7XYAKTTKQRW-task-map-hubs-links-and-satellites-to-sqlite-tab` shows head `ef62c090` as the PO-critic claim, preceded by `ba18cc19` handoff and `7a1f08dd` PO claim, matching a pure PO/critic review stage.
- `git diff --name-only develop..ticket/06EXB7GESWZZTZG7XYAKTTKQRW-task-map-hubs-links-and-satellites-to-sqlite-tab` lists only `.gicket/tickets/...` files for 06EXB7GESWZZTZG7XYAKTTKQRW and 06EXB7GPRGEJHKFMJ8MVAVF8ZG; no `src/` or `tests/` files differ from `develop` on this branch.
- Persisted contract `.gicket/tickets/06EXB7GESWZZTZG7XYAKTTKQRW/description.md:30-49` defines 6 acceptance criteria and 4 definition-of-done items, and `.gicket/tickets/06EXB7GESWZZTZG7XYAKTTKQRW/description.md:51-52` says `## Open Questions` is `none`.
- `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:15-36` shows `UseDataVault()` only sets the conventions annotation and `ApplyDataVaultMetadata()` already exists as the explicit translation entry point.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelBuilderExtensionsTests.cs:40-59` proves a bare `UseDataVault()` stores the conventions annotation and creates no entity types.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:30-77`, `89-149`, and `152-248` already derive hub/link/satellite key/index shapes and stamp `ProducedName`, `EntityKind`, `ParentReference*`, `PropertyRole`, `TechnicalColumnRole`, and `Ordinal` annotations.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:42-106` and `147-193` already lock exact provider-neutral names/shapes for `HubCustomer`, `LinkCustomerOrder`, `SatCustomerContact`, and `SatCustomerOrderState`, including PK/index compositions and no relationships.
- `tests/DCoding.Data.DVault.Tests/Shared/SqliteTestDatabase.cs:22-69` provides an existing ephemeral SQLite helper anchor, while `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj:27-29` and `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-20` show the repo is on `net10.0`/EF Core 10 and currently lacks direct EF relational/SQLite package references; `rg -n "Microsoft.EntityFrameworkCore.(Sqlite|Relational)|UseSqlite|sqlite_master" src tests -g '!bin' -g '!obj'` returned no matches.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract states declared-order satellite payload columns, but the currently source-backed representative provider-neutral tests only show single-payload satellites; a multi-payload satellite example would make that edge case more observable.
- The contract covers hub-parent and link-parent satellites, but it does not name a representative schema case with more than one participant or more than one payload column at the SQLite schema-inspection layer.

Risky assumptions
- Approval assumes EF Core 10 + SQLite can expose deterministic schema-level PK/index naming closely enough to the current provider-neutral `ProducedName` baseline; the repository currently has no relational/SQLite naming precedent or existing `UseSqlite`/`sqlite_master` coverage.

AC / test suggestions
- If exact PK/index names are expected in the created SQLite schema, state explicitly whether the assertion source is EF relational metadata, `sqlite_master` SQL text, PRAGMA output, or a combination.
- Add one explicit multi-payload satellite example to make declared-order payload mapping testable beyond the current single-column representative cases.

Implementation watchouts
- Use the existing `DCoding.Data.DVault:ProducedName` annotations as the naming source of truth; do not re-derive physical names in a separate SQLite-only path.
- Preserve the current opt-in behavior where `UseDataVault()` alone stays conventions-only and SQLite schema mapping is layered on top of `ApplyDataVaultMetadata()`.
- Keep foreign keys, navigations, migrations, provider abstraction, and advanced configuration hooks out of scope for this ticket.

Non-blocking notes
- Comment history under `.gicket/tickets/06EXB7GESWZZTZG7XYAKTTKQRW/comments/` is bot-only orchestration/refinement content; there is no additional human example data or discussion to mine for edge cases.
- Related ticket snapshots show upstream `06EXB7FYXNBPMH8VGQCGP2R41R` is `done`, while follow-up tasks `06EXB7GPRGEJHKFMJ8MVAVF8ZG` and `06EXB7J6HCA9QZ3DPP5Z03YGJ0` remain separate `todo` items, which matches the scoped split in the contract.

Split recommendations
- No split change recommended; the verified split between provider-neutral translation, this SQLite mapping task, provider abstraction, and schema-regression follow-up is coherent.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment