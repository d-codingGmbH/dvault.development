[gicket-bot] PO-critic review contract

Summary
- The contract and repository evidence support closing this epic as a tracking umbrella, but the persisted ticket workflow state still marks it as an active blocked implementation item, so it is not ready for developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md` states the epic is a closure/tracking item, lists child stories `06EXB7FF1J9NR2849WKDR8DKPG`, `06EXB7G6YE4X0GA0CT7EPEFMPR`, `06EXB7GYQKBZ8FMQN6YDYCKATG`, and `06EXB7HYG17X73GH0K535GYJH8`, and has `## Open Questions` -> `- none`.
- Relation files `.gicket/relations/RG/PG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7FF1J9NR2849WKDR8DKPG--parentOf.json`, `.gicket/relations/RG/PR/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7G6YE4X0GA0CT7EPEFMPR--parentOf.json`, `.gicket/relations/RG/TG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7GYQKBZ8FMQN6YDYCKATG--parentOf.json`, `.gicket/relations/RG/H8/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7HYG17X73GH0K535GYJH8--parentOf.json`, and `.gicket/relations/WR/RG/06EXB4MDREV2T51VJNJEP6R0WR--06EXB7F6WNWSJJV14EXTPSFDRG--relates.json` confirm the four outgoing `parentOf` links and the incoming `relates` link from `06EXB4MDREV2T51VJNJEP6R0WR`.
- `git show --stat --summary HEAD` shows HEAD `44379985fcc9badcaef360d4a22689d32b8c87d6` is only the PO-critic lease-claim commit, and `git diff --name-only b36c9330..HEAD -- . ':(exclude).gicket/**'` returned no non-`.gicket` changes on top of `develop` commit `b36c9330`.
- Repository evidence cited by the contract is present locally: `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` exposes `UseDataVault()` and `ApplyDataVaultMetadata()`, `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` defines the translator, `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` registers `IDataVaultSaveService`, `src/DCoding.Data.DVault/DataVaultSaveService.cs` defines the explicit save boundary, `docs/architecture/dvault-v1-explicit-save-service.md` documents it, `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` covers hub/link persistence, reuse, and satellite hash-diff behavior, and `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` keeps `Npgsql.EntityFrameworkCore.PostgreSQL` behind `Condition="'$(DVAULT_TEST_POSTGRES_CONNECTION_STRING)' != ''"`.
- No attachments directory exists at `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/attachments`, matching the contract's zero-attachment claim.

Blocking findings
- There is no implementation delta left to hand to a developer on this branch: HEAD is only the PO-critic claim commit, there are no non-`.gicket` changes relative to `develop`, and all four child stories named as the bounded delivery path are already `done`.

Required PO actions
- Make the post-critic route explicit at ticket level for this closure epic so automation does not send it back to a developer role with no remaining implementation slice.

Open issues ledger
- critic-item-1 [required-po-action] Make the post-critic route explicit at ticket level for this closure epic so automation does not send it back to a developer role with no remaining implementation slice.
- critic-item-2 [blocking-finding] There is no implementation delta left to hand to a developer on this branch: HEAD is only the PO-critic claim commit, there are no non-`.gicket` changes relative to `develop`, and all four child stories named as the bounded delivery path are already `done`.

Missing examples / edge cases
- The contract does not name the exact final status/label combination expected for a closure-only epic after PO-critic review, which leaves the automation outcome ambiguous.
- The contract notes future relation-hygiene questions, but it does not give a concrete ticket-level example of what should happen if downstream tickets still reference this umbrella after closure.

Risky assumptions
- none

AC / test suggestions
- Add a ticket-level acceptance criterion that names the exact status/label state expected once a closure-only epic clears PO-critic.
- Add a ticket-level check that every child listed in the bounded delivery path must be `done` before the umbrella epic can leave PO-critic.

Implementation watchouts
- Any automation keyed off `blocked/dev` or `blocked/test` can reopen the epic's execution path even though the code evidence is already on `develop`.
- Future Postgres expansion or save-path convenience work should be created as a separate follow-up ticket; reopening this epic would blur closure scope back into implementation scope.

Non-blocking notes
- The latest PO automation comment `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/comments/06EY3C6QSF3TVFBEFFGFWP9GYW.md` already says the ticket is ready for PO-critic; later comments `06EY3CB8S5K8ARQSB7P7X8E7N4.md` and `06EY3CBD4AA7VV84Z4MW3520JW.md` are claim/lease housekeeping only.

Split recommendations
- No additional split is recommended; the four existing `parentOf` child stories already cover the bounded delivery path.
- If workflow-governance cleanup is needed for closure-only epics, track that under a separate follow-up ticket instead of reopening `06EXB7F6WNWSJJV14EXTPSFDRG`.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment