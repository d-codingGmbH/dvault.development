[gicket-bot] PO-critic review contract

Summary
- Repository and child-ticket evidence supports the closure-only interpretation, but the live parent ticket still carries blocked developer/tester routing labels, so the ticket metadata is not yet consistent enough for developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md:4-15` says the parent epic is closure-only, names child stories `06EXB7FF1J9NR2849WKDR8DKPG`, `06EXB7G6YE4X0GA0CT7EPEFMPR`, `06EXB7GYQKBZ8FMQN6YDYCKATG`, and `06EXB7HYG17X73GH0K535GYJH8`, and `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md:47-48` shows `## Open Questions` -> `- none`.
- All four declared child stories are directly marked `done` in `.gicket/tickets/06EXB7FF1J9NR2849WKDR8DKPG/ticket.json:5-18`, `.gicket/tickets/06EXB7G6YE4X0GA0CT7EPEFMPR/ticket.json:5-18`, `.gicket/tickets/06EXB7GYQKBZ8FMQN6YDYCKATG/ticket.json:5-18`, and `.gicket/tickets/06EXB7HYG17X73GH0K535GYJH8/ticket.json:5-18`.
- The parent-child relation set is present on disk in `.gicket/relations/RG/PG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7FF1J9NR2849WKDR8DKPG--parentOf.json:3-5`, `.gicket/relations/RG/PR/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7G6YE4X0GA0CT7EPEFMPR--parentOf.json:3-5`, `.gicket/relations/RG/TG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7GYQKBZ8FMQN6YDYCKATG--parentOf.json:3-5`, `.gicket/relations/RG/H8/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7HYG17X73GH0K535GYJH8--parentOf.json:3-5`, plus the incoming relates link `.gicket/relations/WR/RG/06EXB4MDREV2T51VJNJEP6R0WR--06EXB7F6WNWSJJV14EXTPSFDRG--relates.json:3-5`.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-23` exposes `AddDVault()` and registers `IDataVaultSaveService`; `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:15-37` exposes `UseDataVault()` and `ApplyDataVaultMetadata()`; `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:42-99`, `101-162`, `164-228`, and `237-258` implement hub/link/satellite EF metadata translation.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21`, `35-66`, `328-355`, `389-475`, and `529-558` define the explicit save-service boundary, request normalization, hub/link reuse, and satellite hash-diff change handling; `docs/architecture/dvault-v1-explicit-save-service.md:8-27` documents the same explicit-save and no-`SaveChanges`-interceptor v1 boundary.
- SQLite and Postgres-readiness evidence matches the contract: `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:11-72`, `76-166`, and `170-349` cover SQLite hub/link persistence, reuse, and satellite history; `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:16-18`, `README.md:26-36`, `tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs:4-7`, and `tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs:10-13` keep Postgres opt-in via `DVAULT_TEST_POSTGRES_CONNECTION_STRING`.
- `git rev-parse HEAD` returned `44d0f12ee353e77f3d3704bd75fef92979cbce93` on branch `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc`, and `git diff --stat 44d0f12ee353e77f3d3704bd75fef92979cbce93..HEAD` returned no output.

Blocking findings
- none

Required PO actions
- Re-run or otherwise complete the ticket-field handoff so downstream automation no longer reads the parent epic as blocked on developer/tester execution.

Open issues ledger
- critic-item-1 [required-po-action] Re-run or otherwise complete the ticket-field handoff so downstream automation no longer reads the parent epic as blocked on developer/tester execution.

Missing examples / edge cases
- None on the parent epic beyond the ticket-field contradiction; the repository and child-ticket evidence already cover the bounded EF, save-service, SQLite, and opt-in Postgres scope.

Risky assumptions
- Assuming the stale blocked labels will be cleared later without an explicit ticket-field fix is not supported by the current persisted state.

AC / test suggestions
- Keep future technical acceptance on separate follow-up tickets if first-class Postgres runtime support or interceptor-based convenience APIs are ever approved.

Implementation watchouts
- Do not treat the stale blocked labels as permission to reopen implementation on 06EXB7F6WNWSJJV14EXTPSFDRG; the technical scope is already owned by done child stories and validated by current repository evidence.
- Any future provider-expansion or `SaveChanges`-interceptor work should be split into new tickets rather than added back onto this parent closure epic.

Non-blocking notes
- The parent ticket currently has zero persisted attachments; `find .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/attachments -maxdepth 1 -type f | wc -l` returned `0`.
- The live local ticket history contains many persisted comments even though the prompt snapshot said `Recent comments: <none>`; the review used the repository-backed `.gicket` state as the newer source of truth.

Split recommendations
- No additional split is needed inside 06EXB7F6WNWSJJV14EXTPSFDRG once the label contradiction is fixed; the four existing child stories already form the bounded delivery path.
- If first-class Postgres runtime/provider support, `SaveChanges` interception, or deferred Data Vault capabilities are approved later, create separate follow-up tickets or epics instead of reopening this parent.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment