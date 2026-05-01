[gicket-bot] PO-critic review contract

Summary
- Repository and ticket evidence match a closure-only epic, but the live ticket metadata still marks the parent as blocked for dev/test, so it is not ready for handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md says the epic is a closure/tracking item, lists child tickets 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8 as the full delivery path, and has `## Open Questions` -> `- none`.
- Relation files .gicket/relations/RG/PG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7FF1J9NR2849WKDR8DKPG--parentOf.json, .gicket/relations/RG/PR/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7G6YE4X0GA0CT7EPEFMPR--parentOf.json, .gicket/relations/RG/TG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7GYQKBZ8FMQN6YDYCKATG--parentOf.json, .gicket/relations/RG/H8/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7HYG17X73GH0K535GYJH8--parentOf.json, and .gicket/relations/WR/RG/06EXB4MDREV2T51VJNJEP6R0WR--06EXB7F6WNWSJJV14EXTPSFDRG--relates.json match the contract's stated decomposition and incoming relates link.
- `git rev-parse --abbrev-ref HEAD` returned `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc`; `git rev-parse HEAD` returned `e2aab038dfb6104ba98f6eefa99f92684b740562`; `git diff --stat e2aab038dfb6104ba98f6eefa99f92684b740562..HEAD` returned no output.
- Repository evidence named in the contract is present: src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs, src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs, src/DCoding.Data.DVault/DataVaultSaveService.cs, src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs, tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs, tests/DCoding.Data.DVault.Tests/Integration/NpgsqlProviderReflection.cs, and README.md.

Blocking findings
- none

Required PO actions
- Re-run PO handoff only after the ticket-level routing metadata no longer implies remaining developer or tester work on the parent epic.

Open issues ledger
- critic-item-1 [required-po-action] Re-run PO handoff only after the ticket-level routing metadata no longer implies remaining developer or tester work on the parent epic.

Missing examples / edge cases
- none

Risky assumptions
- This review assumes the stale blocked labels are the only remaining ticket-level contradiction; repository, relation, and child-ticket evidence do not suggest reopened implementation scope.
- This review assumes workflow can avoid routing a closure-only epic back into implementation once the live labels are corrected; the prompt policy still lists `po-critic.on-success: dev`.

AC / test suggestions
- Add a workflow regression check or reviewer checklist item that closure/tracking epics cannot be approved while live labels still imply pending implementation ownership.

Implementation watchouts
- Do not reopen EF integration, SQLite persistence, explicit save-service, or Postgres-readiness implementation on this parent epic; the current repository evidence already maps those slices to completed child work.
- Do not use this parent as the vehicle for first-class Postgres runtime support or SaveChanges interception; keep those as separate follow-up work.

Non-blocking notes
- The prompt snapshot said `Recent comments: <none>`, but the local ticket now contains recent PO/PO-critic handoff comments under .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/comments/.
- The ticket directory listing for .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/ showed comments/, description.md, events/, and ticket.json; no ticket-local attachments directory was observed.

Split recommendations
- If first-class Postgres runtime/provider support is approved later, create a separate follow-up ticket or epic instead of reopening 06EXB7F6WNWSJJV14EXTPSFDRG.
- If workflow governance keeps misrouting closure-only epics, create a separate governance ticket for a closure/completion route rather than using this parent epic as an executable handoff.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment