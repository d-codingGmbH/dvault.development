[gicket-bot] PO-critic review contract

Summary
- Current branch evidence does not support this handoff: the ticket contract describes future provider-reader implementation work, but the branch contains only ticket metadata updates, `DataVaultLiveSchemaReader` is still SQLite-only, and non-SQLite live-schema reader coverage is absent.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `git diff --name-only develop...HEAD` on branch `ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers` lists only `.gicket/tickets/06F2PGG8ZKSYGC8863118H56G8/...` files; no `src/` or `tests/` paths changed.
- `git log --oneline --max-count=5` shows only lease/handover commits on this branch (`d33454ad8`, `ea5f49cdb`, `feb681e22`, `bf85e8973`) after `develop`; there is no implementation commit for ticket `06F2PGG8ZKSYGC8863118H56G8`.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:14-34` still hardcodes `SqliteProviderName` and returns `DataVaultLiveSchemaReadResult.UnsupportedProvider(providerName)` for every non-SQLite provider.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:11-18` already recognizes `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.SqlServer`, `Oracle.EntityFrameworkCore`, `Pomelo.EntityFrameworkCore.MySql`, and `MySql.EntityFrameworkCore`, so the missing behavior is the live-schema reader dispatch/implementation, not provider-name discovery.
- `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixtures.cs:35-136` provides Postgres/SQL Server/Oracle/MySQL fixtures, but `rg -n "DataVaultLiveSchemaReader\.ReadAsync\(" tests/DCoding.Data.DVault.Tests/Integration tests/DCoding.Data.DVault.Tests/Unit` finds read calls only in `tests/DCoding.Data.DVault.Tests/Integration/SqliteLiveSchemaDriftTests.cs:22,143,165`.
- `README.md:457-473` still documents SQLite as the only supported v1 live-schema reader and says PostgreSQL, SQL Server, Oracle, and MySQL are not first-class supported readers.
- The persisted contract in `.gicket/tickets/06F2PGG8ZKSYGC8863118H56G8/description.md:31-42` requires concrete non-SQLite dispatch and provider integration coverage, while `.gicket/tickets/06F2PGG8ZKSYGC8863118H56G8/description.md:52-53` shows `## Open Questions` is `none`.
- Related blocker ticket `06F2PGG57K3S7CJQP5QX9AWW3G` is `done` in `.gicket/tickets/06F2PGG57K3S7CJQP5QX9AWW3G/ticket.json:3-19`, so the shared contract baseline exists; this ticket's missing piece is still the provider-reader implementation itself.

Blocking findings
- This closure-only/handoff state is unsupported by the repository: compared with `develop`, the branch contains ticket/comment updates only and no product or test implementation for the claimed provider-reader work.
- Acceptance criteria 1, 2, and 5 are not satisfied because `DataVaultLiveSchemaReader.ReadAsync(...)` still dispatches only to SQLite and classifies every non-SQLite provider as `UnsupportedProvider`.
- Acceptance criterion 3 and Definition of Done items 1-2 are not satisfied because there is no direct Postgres/SQL Server/Oracle/MySQL `DataVaultLiveSchemaReader.ReadAsync(...)` integration coverage on the branch; only SQLite live-schema tests are present.

Required PO actions
- Return this ticket to PO refinement and remove the unsupported implication that the current branch is ready for closure/handoff without developer implementation.
- If implementation exists elsewhere, attach the exact branch/ref/commit and related test evidence; otherwise keep this as actual developer work and do not resend to PO-critic until non-ticket `src/` and `tests/` evidence is present.
- If release scope is intended to remain SQLite-only, narrow this ticket contract accordingly and move first-class PostgreSQL/SQL Server/Oracle/MySQL live-schema readers into a separate implementation ticket instead of asserting them on this branch.

Open issues ledger
- critic-item-1 [required-po-action] Return this ticket to PO refinement and remove the unsupported implication that the current branch is ready for closure/handoff without developer implementation.
- critic-item-2 [required-po-action] If implementation exists elsewhere, attach the exact branch/ref/commit and related test evidence; otherwise keep this as actual developer work and do not resend to PO-critic until non-ticket `src/` and `tests/` evidence is present.
- critic-item-3 [required-po-action] If release scope is intended to remain SQLite-only, narrow this ticket contract accordingly and move first-class PostgreSQL/SQL Server/Oracle/MySQL live-schema readers into a separate implementation ticket instead of asserting them on this branch.
- critic-item-4 [blocking-finding] This closure-only/handoff state is unsupported by the repository: compared with `develop`, the branch contains ticket/comment updates only and no product or test implementation for the claimed provider-reader work.
- critic-item-5 [blocking-finding] Acceptance criteria 1, 2, and 5 are not satisfied because `DataVaultLiveSchemaReader.ReadAsync(...)` still dispatches only to SQLite and classifies every non-SQLite provider as `UnsupportedProvider`.
- critic-item-6 [blocking-finding] Acceptance criterion 3 and Definition of Done items 1-2 are not satisfied because there is no direct Postgres/SQL Server/Oracle/MySQL `DataVaultLiveSchemaReader.ReadAsync(...)` integration coverage on the branch; only SQLite live-schema tests are present.

Missing examples / edge cases
- No concrete acceptance example requires a recognized provider to return `Unavailable` after connection open succeeds but catalog inspection fails.
- No concrete acceptance example proves both MySQL provider names (`Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`) share the same dispatch and result expectations.
- No concrete acceptance example shows Oracle shortened physical names, PostgreSQL/SQL Server schema isolation, and MySQL table prefixes still produce zero-blocking-drift output.

Risky assumptions
- Assuming the presence of `ExternalProviderLiveSchemaFixture` means provider reader support already exists; repository search shows `DataVaultLiveSchemaReader.ReadAsync(...)` is only exercised in SQLite tests.
- Assuming built-in provider capability selection automatically gives live-schema reader support; `DataVaultLiveSchemaReader.cs:31-34` bypasses those non-SQLite profiles and returns `UnsupportedProvider` instead.
- Assuming the ticket title and PO contract are enough to prove closure readiness; branch history and diff show only ticket-state commits after `develop`.

AC / test suggestions
- Require one direct evidence item per provider: provision through `ExternalProviderLiveSchemaFixture`, call `DataVaultLiveSchemaReader.ReadAsync(...)`, and compare against `ExpectedSnapshot` with zero blocking drift differences.
- State explicitly that unknown providers must stay `UnsupportedProvider` while recognized-provider catalog failures must classify as `Unavailable`.
- Require deterministic ordering/signature evidence for the new provider readers, not only SQLite parity evidence.

Implementation watchouts
- Provider dispatch must stay aligned with `DataVaultProviderCapabilityProfileSelection.cs:11-18`, especially the dual MySQL provider-name baseline.
- The fixture contract already exists in `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixtures.cs`; future acceptance should verify against that exact baseline rather than inferred catalog shapes.
- Public docs currently advertise SQLite-only live-schema support (`README.md:457-473`), so any future reader landing needs an explicit documentation/release-note handoff to ticket `06F2PGHA0EXJRGDHM4GQM7NPYR` or an equivalent minimal doc update.

Non-blocking notes
- Current ticket comments under `.gicket/tickets/06F2PGG8ZKSYGC8863118H56G8/comments/` are automation claim/lease/handoff traffic plus the PO refinement contract; no human clarification comment was found.
- The contract is internally clear on routing because `.gicket/tickets/06F2PGG8ZKSYGC8863118H56G8/description.md:52-53` records `## Open Questions` as `none`.

Split recommendations
- Do not split purely to paper over the missing implementation evidence; first correct the unsupported closure-only/handoff framing.
- If this is re-opened as real developer work, keeping one bounded ticket still looks reasonable because the shared contract/fixture baseline already exists; split by provider only if provider-specific catalog behavior or external setup becomes independently reviewable.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment