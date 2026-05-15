[gicket-bot] PO-critic review contract

Summary
- Return to PO: the branch still shows ticket-only churn, SQLite-only live-schema dispatch, and no direct non-SQLite reader evidence, so this handoff is not ready for developer routing.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `git rev-parse HEAD` and `git rev-parse 05d9653055555e1cd312fec1d7dff81477a301a2` both resolve to `05d9653055555e1cd312fec1d7dff81477a301a2`, and `git diff --stat 05d9653055555e1cd312fec1d7dff81477a301a2..HEAD` is empty, so this review saw the same repo state as the supplied scratch snapshot.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:13-34` still hardcodes `SqliteProviderName` and returns `DataVaultLiveSchemaReadResult.UnsupportedProvider(providerName)` for every non-SQLite provider.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:11-18` already recognizes `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.SqlServer`, `Oracle.EntityFrameworkCore`, `Pomelo.EntityFrameworkCore.MySql`, and `MySql.EntityFrameworkCore`, so provider-name recognition exists but live-schema reader dispatch does not.
- `rg -n "DataVaultLiveSchemaReader\.ReadAsync\(" /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Integration -g '*LiveSchema*'` finds calls only in `tests/DCoding.Data.DVault.Tests/Integration/SqliteLiveSchemaDriftTests.cs:22,143,165`; non-SQLite files such as `tests/DCoding.Data.DVault.Tests/Integration/PostgresLiveSchemaFixtureContractTests.cs:10-22` validate fixture expectations only.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:16-20` keeps non-SQLite provider packages behind `DVAULT_TEST_*_CONNECTION_STRING` conditions, while `README.md:457-473` and `docs/production-adoption-checklist.md:29,45` still document SQLite as the only first-class live-schema reader and non-SQLite evidence as opt-in/external.
- The authoritative contract and latest PO refinement comment both say there is no attached implementation ref and no non-ticket `src/` or `tests/` diff: `.gicket/tickets/06F2PGG8ZKSYGC8863118H56G8/description.md:12-20,35-46` and `.gicket/tickets/06F2PGG8ZKSYGC8863118H56G8/comments/06F2QG38Q63D4X68XM3BVG05QM.md:12-24`.

Blocking findings
- Definition of Done items 1-2 are not met: compared with `develop`, the branch contains ticket metadata updates only and no non-ticket `src/` or `tests/` implementation evidence for provider catalog readers.
- Acceptance criteria 1, 2, and 5 are not met because `DataVaultLiveSchemaReader.ReadAsync(...)` still routes only SQLite and classifies recognized non-SQLite providers as `UnsupportedProvider`.
- Acceptance criterion 3 is not met because direct PostgreSQL, SQL Server, Oracle, and MySQL live-schema reader execution is not evidenced; the only observed direct `ReadAsync(...)` tests are SQLite tests.

Required PO actions
- Return ticket `06F2PGG8ZKSYGC8863118H56G8` to PO refinement instead of developer handoff; the current `ready_for_po_critic` routing is unsupported by repository evidence.
- Do not resend this ticket to PO-critic until the ticket cites an exact implementation branch/ref/commit with matching non-ticket `src/` and `tests/` evidence for the provider readers.
- If product intends to keep the release SQLite-only, update the delivery contract and ticket routing accordingly, and move first-class PostgreSQL/SQL Server/Oracle/MySQL live-schema readers into a separate implementation ticket rather than leaving them implied here.

Open issues ledger
- critic-item-1 [required-po-action] Return ticket `06F2PGG8ZKSYGC8863118H56G8` to PO refinement instead of developer handoff; the current `ready_for_po_critic` routing is unsupported by repository evidence.
- critic-item-2 [required-po-action] Do not resend this ticket to PO-critic until the ticket cites an exact implementation branch/ref/commit with matching non-ticket `src/` and `tests/` evidence for the provider readers.
- critic-item-3 [required-po-action] If product intends to keep the release SQLite-only, update the delivery contract and ticket routing accordingly, and move first-class PostgreSQL/SQL Server/Oracle/MySQL live-schema readers into a separate implementation ticket rather than leaving them implied here.
- critic-item-4 [blocking-finding] Definition of Done items 1-2 are not met: compared with `develop`, the branch contains ticket metadata updates only and no non-ticket `src/` or `tests/` implementation evidence for provider catalog readers.
- critic-item-5 [blocking-finding] Acceptance criteria 1, 2, and 5 are not met because `DataVaultLiveSchemaReader.ReadAsync(...)` still routes only SQLite and classifies recognized non-SQLite providers as `UnsupportedProvider`.
- critic-item-6 [blocking-finding] Acceptance criterion 3 is not met because direct PostgreSQL, SQL Server, Oracle, and MySQL live-schema reader execution is not evidenced; the only observed direct `ReadAsync(...)` tests are SQLite tests.

Missing examples / edge cases
- A concrete acceptance example for a recognized provider whose connection/catalog inspection fails and must return `Unavailable` rather than `UnsupportedProvider`.
- A concrete acceptance example proving both MySQL provider names (`Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`) share the same dispatch and outcome expectations.
- A concrete acceptance example proving Oracle physical-name overrides, PostgreSQL/SQL Server schema isolation, and MySQL prefixes still yield deterministic zero-blocking-drift snapshots.

Risky assumptions
- Assuming `ExternalProviderLiveSchemaFixture` or conditional provider package references prove live-schema reader delivery; the observed direct reader calls remain SQLite-only.
- Assuming existing provider capability selection automatically supplies live-schema reader dispatch; `DataVaultLiveSchemaReader.cs:31-34` bypasses non-SQLite capability profiles and returns `UnsupportedProvider`.
- Assuming the persisted `ready_for_po_critic` handoff can override current branch evidence even when no implementation ref or non-ticket code/test changes are attached.

AC / test suggestions
- Require one direct evidence item per provider: provision through `ExternalProviderLiveSchemaFixture`, call `DataVaultLiveSchemaReader.ReadAsync(...)`, and compare against `ExpectedSnapshot` with zero blocking drift differences.
- Keep an explicit negative-case criterion that unknown providers remain `UnsupportedProvider` while recognized-provider inspection/connectivity failures classify as `Unavailable`.
- Require future PO-critic reruns to name the exact implementation branch/ref/commit alongside the provider-specific test evidence used for review.

Implementation watchouts
- Dispatch expectations must stay aligned with `DataVaultProviderCapabilityProfileSelection.cs:11-18`, especially the dual MySQL provider-name baseline.
- Future acceptance should validate against `ExternalProviderLiveSchemaModelOptions.ExpectedSnapshot` so schema scoping, Oracle identifier overrides, MySQL prefixes, and deterministic ordering remain stable across providers.
- Current docs and release evidence still advertise SQLite-first live-schema support, so any later dev-ready handoff needs an explicit documentation/release-note follow-up path such as ticket `06F2PGHA0EXJRGDHM4GQM7NPYR`.

Non-blocking notes
- `.gicket/tickets/06F2PGG8ZKSYGC8863118H56G8/description.md:48-49` records `## Open Questions` as `none`; this return is driven by missing implementation evidence, not unresolved open questions.

Split recommendations
- Keep this as one bounded implementation ticket if product still wants PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers in the same release slice.
- Only split if product explicitly narrows scope back to SQLite-only or if provider-specific external setup becomes independently reviewable enough to justify separate implementation tickets.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment