[gicket-bot] PO-critic review contract

Summary
- Return to PO: the repository and child-ticket evidence support this epic as a closure/tracking item, but the current parent ticket state still routes like an active blocked work item rather than a non-dev closure path.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Current branch HEAD is `e97132036ee13d823aba02b3b8c502651d52cad8` on `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc`; `git log --oneline -5` shows only ticket-orchestration commits after the PO handoff (`24ef8d84`, `bf1830aa`, `e9713203`), not a new implementation commit for this parent epic.
- `git diff --name-only 24ef8d8409e6..HEAD` lists only `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/...` comments/events/ticket metadata plus child ticket metadata files; it does not list `src/`, `tests/`, or `docs/` implementation files, which supports that this branch is carrying workflow updates rather than a fresh developer slice for the parent epic.
- `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md` explicitly says the parent is a `closure/tracking epic`, names child stories `06EXB7FF1J9NR2849WKDR8DKPG`, `06EXB7G6YE4X0GA0CT7EPEFMPR`, `06EXB7GYQKBZ8FMQN6YDYCKATG`, and `06EXB7HYG17X73GH0K535GYJH8` as the complete delivery path, and has `## Open Questions` set to `- none`.
- Relation files `.gicket/relations/RG/PG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7FF1J9NR2849WKDR8DKPG--parentOf.json`, `.gicket/relations/RG/PR/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7G6YE4X0GA0CT7EPEFMPR--parentOf.json`, `.gicket/relations/RG/TG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7GYQKBZ8FMQN6YDYCKATG--parentOf.json`, and `.gicket/relations/RG/H8/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7HYG17X73GH0K535GYJH8--parentOf.json` directly persist the four child links claimed by the contract.
- Repository evidence for already-delivered implementation is present in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` (`UseDataVault`, `ApplyDataVaultMetadata`), `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` (hub/link/satellite EF projection), `src/DCoding.Data.DVault/DataVaultSaveService.cs` (`IDataVaultSaveService` plus explicit hub/link/satellite save operations), `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` (SQLite persistence/reuse/hash-diff coverage), `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` (conditional `Npgsql.EntityFrameworkCore.PostgreSQL` package), `tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs` (env-var opt-in), and `tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs` (configured Postgres schema test).

Blocking findings
- none

Required PO actions
- Update the parent ticket's status/labels/handoff so it represents a closure/tracking item instead of active blocked work; specifically clear workflow metadata that implies pending developer or tester action on `06EXB7F6WNWSJJV14EXTPSFDRG`.
- Make the next automation path explicit at the ticket level, not only in prose comments, so the parent epic is not dispatched to dev again while its contract says no implementation slice remains.
- If the workflow cannot represent a closure-only epic on this path, move this parent ticket onto the correct completion/closure route before re-running PO-critic.

Open issues ledger
- critic-item-1 [required-po-action] Update the parent ticket's status/labels/handoff so it represents a closure/tracking item instead of active blocked work; specifically clear workflow metadata that implies pending developer or tester action on `06EXB7F6WNWSJJV14EXTPSFDRG`.
- critic-item-2 [required-po-action] Make the next automation path explicit at the ticket level, not only in prose comments, so the parent epic is not dispatched to dev again while its contract says no implementation slice remains.
- critic-item-3 [required-po-action] If the workflow cannot represent a closure-only epic on this path, move this parent ticket onto the correct completion/closure route before re-running PO-critic.

Missing examples / edge cases
- No additional implementation examples are blocking on the parent epic; the gap is workflow-state clarity on the parent ticket itself.

Risky assumptions
- The contract assumes closure intent alone is sufficient, but the observed role-path for a successful PO-critic review still routes to dev unless ticket-level workflow metadata changes prevent that misroute.

AC / test suggestions
- Add a parent-epic acceptance criterion or DoD line that names the required terminal or closure-oriented ticket state/labels, so PO-critic can verify workflow alignment from persisted fields instead of relying on comment prose.

Implementation watchouts
- Do not reopen this parent epic for future provider expansion; the repository and contract already bound Postgres work to opt-in readiness only, with separate follow-up work required for first-class provider support.

Non-blocking notes
- The repository evidence is consistent with the parent epic being a completed umbrella over the four child stories, not a missing implementation slice.
- `## Open Questions` is resolved to `none`, so the remaining blocker is ticket-level workflow alignment rather than missing technical scope.

Split recommendations
- No new implementation split is needed for this parent epic itself.
- If first-class Postgres runtime/provider support or save-path convenience APIs are later approved, schedule them as separate follow-up tickets or an epic instead of reopening `06EXB7F6WNWSJJV14EXTPSFDRG`.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment