[gicket-bot] PO-critic review contract

Summary
- Repository and relation evidence support a closure-only parent epic, but the live parent ticket still carries blocked developer/tester routing labels, so it is not ready for handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `git rev-parse --abbrev-ref HEAD` returned `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc`; `git rev-parse HEAD` and `git log -1 --oneline HEAD` both matched scratch ref `d5f071ace490057874f3e2a8c742dc6557c7b264`.
- `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md` marks the parent as a closure/tracking epic, names child stories `06EXB7FF1J9NR2849WKDR8DKPG`, `06EXB7G6YE4X0GA0CT7EPEFMPR`, `06EXB7GYQKBZ8FMQN6YDYCKATG`, and `06EXB7HYG17X73GH0K535GYJH8` as the full delivery path, and has `## Open Questions` -> `- none`.
- Relation files `.gicket/relations/RG/PG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7FF1J9NR2849WKDR8DKPG--parentOf.json`, `.gicket/relations/RG/PR/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7G6YE4X0GA0CT7EPEFMPR--parentOf.json`, `.gicket/relations/RG/TG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7GYQKBZ8FMQN6YDYCKATG--parentOf.json`, `.gicket/relations/RG/H8/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7HYG17X73GH0K535GYJH8--parentOf.json`, and `.gicket/relations/WR/RG/06EXB4MDREV2T51VJNJEP6R0WR--06EXB7F6WNWSJJV14EXTPSFDRG--relates.json` match the contract's stated topology.
- `rg -n` directly confirmed the contract's repository evidence: `UseDataVault` and `ApplyDataVaultMetadata` in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`, `internal static class DataVaultEfMetadataTranslator` in `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`, `public interface IDataVaultSaveService` in `src/DCoding.Data.DVault/DataVaultSaveService.cs`, `DefaultSaveServicePersistsSatelliteRowsOnlyWhenHashDiffChanges` in `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`, the conditional `Npgsql.EntityFrameworkCore.PostgreSQL` reference in `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj`, and README Postgres opt-in via `DVAULT_TEST_POSTGRES_CONNECTION_STRING`.
- Earlier PO-critic comment `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/comments/06EY3JSWYZG94EWW5Q3E7DCMHR.md` already returned the ticket to PO because the epic was closure-only while the live parent metadata still marked it blocked for dev/test.

Blocking findings
- The latest PO pass did not actually persist the required routing cleanup before re-entering PO-critic; `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/comments/06EY3RCKPTSZ7ABZABF1SYBJ30.md` defers the label cleanup to downstream automation instead of leaving the ticket in a closure-compatible state now.

Required PO actions
- Persist an explicit closure-compatible status/label end state for this parent epic instead of relying on a later developer-oriented handoff to clean up the routing metadata.

Open issues ledger
- critic-item-1 [required-po-action] Persist an explicit closure-compatible status/label end state for this parent epic instead of relying on a later developer-oriented handoff to clean up the routing metadata.
- critic-item-2 [blocking-finding] The latest PO pass did not actually persist the required routing cleanup before re-entering PO-critic; `.gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/comments/06EY3RCKPTSZ7ABZABF1SYBJ30.md` defers the label cleanup to downstream automation instead of leaving the ticket in a closure-compatible state now.

Missing examples / edge cases
- The contract still does not persist a concrete example of the exact final status/label combination expected after a closure-only epic clears PO-critic, which is why the routing contradiction keeps recurring.

Risky assumptions
- That runtime will clear stale blocked routing labels later even though the latest persisted `ticket.json` still carries them.
- That a PO-critic success path ending in `dev` is safe for a closure-only parent epic with no remaining developer-owned work.

AC / test suggestions
- Add a ticket-level acceptance criterion or reviewer checklist item that closure/tracking epics cannot clear PO-critic while live labels still imply dev/test execution.
- State the exact post-review status/label set expected for closure-only epics so PO and PO-critic can verify the same end state.

Implementation watchouts
- Do not reopen EF model-building, SQLite persistence, explicit save-service, or Postgres-readiness work on this parent epic; those slices are already represented by done child stories.
- If future work is approved for first-class Postgres runtime/provider support or SaveChanges interception, treat it as separate follow-up work rather than extending this closure ticket.

Non-blocking notes
- `find .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG -maxdepth 1 -type d` only showed the ticket root plus `comments` and `events`; no ticket-local attachments directory was observed.
- Repository evidence and child-ticket completion do support the closure-only interpretation; the remaining problem is ticket-field routing alignment, not missing implementation evidence.

Split recommendations
- No additional split is needed for the parent epic itself.
- If workflow-governance cleanup or first-class Postgres runtime/provider support is approved later, create separate follow-up tickets instead of reopening `06EXB7F6WNWSJJV14EXTPSFDRG`.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment