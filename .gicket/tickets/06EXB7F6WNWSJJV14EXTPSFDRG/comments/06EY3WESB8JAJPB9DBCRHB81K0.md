[gicket-bot] PO-critic review contract

Summary
- The epic's closure-only contract is supported by repository and child-ticket evidence, but the live parent ticket fields still contradict that contract by retaining developer/tester blocking labels, so it is not ready for downstream handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md:5-20,28-31,46-47 says the epic is a closure/tracking record, excludes blocked developer/tester routing labels from its steady-state baseline, uses the four listed child stories as the full delivery path, and has Open Questions = none.
- Comment 06EY3V0AJB1YVNT11Y43QCER20.md says critic-item-1 and critic-item-2 were answered by making blocked-label removal part of the parent epic's persisted end state, but the latest ticket.json still contains those blocked labels.
- Relation events 06EXB8D04TN9H4GD64VWA0YBV4.json, 06EXB8D6YNREQBVHET08ZH6WTM.json, 06EXB8DE4F5MKD0R4ANMN327EM.json, and 06EXB8DQ87NFM38KH8MWWQSFY4.json record the four parentOf edges from 06EXB7F6WNWSJJV14EXTPSFDRG to its child stories; 06EXB8FQ799RAV6E95FC0469CM.json records the incoming relates link from 06EXB4MDREV2T51VJNJEP6R0WR.
- Attachment check under .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/attachments returned 0 files.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:15-37 exposes UseDataVault() and ApplyDataVaultMetadata(), and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:7-25,42-99 applies SQLite-backed hub/link/satellite EF metadata translation.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21,27-67 and docs/architecture/dvault-v1-explicit-save-service.md:8-27 define and document the explicit IDataVaultSaveService write boundary instead of SaveChanges interception.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:11-260 covers SQLite hub/link persistence, cross-context reuse, and satellite hash-diff change history.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-18, tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs:9-34, and README.md:26-36 keep Postgres as an opt-in surface behind DVAULT_TEST_POSTGRES_CONNECTION_STRING.
- git show --stat --oneline 877f85f7d9d9bf0caf91cf747def655a78399f4c shows the current branch HEAD is a PO-critic lease-claim commit touching .gicket ticket/comment/event files only, and git diff --stat 44d0f12ee353e77f3d3704bd75fef92979cbce93..877f85f7d9d9bf0caf91cf747def655a78399f4c shows ticketing metadata churn but no src/ or tests/ implementation edits.

Blocking findings
- none

Required PO actions
- Re-run PO refinement or handback only after the ticket-field state matches the contract's stated steady-state label baseline.

Open issues ledger
- critic-item-1 [required-po-action] Re-run PO refinement or handback only after the ticket-field state matches the contract's stated steady-state label baseline.

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep any future acceptance or test expansion for first-class Postgres runtime support or interceptor/convenience APIs on separate follow-up tickets, not on this closure epic.

Implementation watchouts
- Do not reopen implementation on 06EXB7F6WNWSJJV14EXTPSFDRG; the repository evidence and child tickets show the bounded EF, SQLite persistence, explicit save-service, and Postgres opt-in slices are already materialized elsewhere.
- If future provider-specific or SaveChanges-interceptor work is approved, split it into new tickets instead of extending this parent closure record.

Non-blocking notes
- The persisted delivery contract has no unresolved open questions: .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md:46-47 is none.
- The live .gicket repository contains recent comment history even though the prompt snapshot said Recent comments: none; this review used the newer persisted repository state.

Split recommendations
- No additional split is needed for the parent epic once the label contradiction is fixed; the four existing child stories already bound the delivery path.
- Create separate follow-up tickets or epics for any future first-class Postgres runtime support, SaveChanges interception, or deferred Data Vault capabilities.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment