[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git log on branch ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc shows HEAD 69cfdb8e88335df7d78604017e121e464ddefdce after PO handoff commits 46f32f877bb86cb51f49828eee1731afd297ab95 and 2545cc28d7496d40f27605b64c2fa494cb1c22ab.
- .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md:11-35 says the live parent label set already matches only area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready, and description.md:28-35 requires no developer/tester blocking labels; description.md:46-47 shows Open Questions -> none.
- Parent relation evidence exists at .gicket/relations/RG/PG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7FF1J9NR2849WKDR8DKPG--parentOf.json, .gicket/relations/RG/PR/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7G6YE4X0GA0CT7EPEFMPR--parentOf.json, .gicket/relations/RG/TG/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7GYQKBZ8FMQN6YDYCKATG--parentOf.json, .gicket/relations/RG/H8/06EXB7F6WNWSJJV14EXTPSFDRG--06EXB7HYG17X73GH0K535GYJH8--parentOf.json, plus incoming relates file .gicket/relations/WR/RG/06EXB4MDREV2T51VJNJEP6R0WR--06EXB7F6WNWSJJV14EXTPSFDRG--relates.json.
- Repository evidence cited by the contract is present: src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:15-37 exposes UseDataVault and ApplyDataVaultMetadata; src/DCoding.Data.DVault/DataVaultSaveService.cs:10-18 exposes public IDataVaultSaveService; tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:11,76,170 covers SQLite hub/link reuse and satellite hash-diff behavior; tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:17 and README.md:30-33 preserve the opt-in Postgres hook.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not show an explicit example of the transient label set allowed during PO-critic review versus the steady-state closure-only label set after review clears.

Risky assumptions
- That automation will ignore blocked/dev and blocked/test on the live ticket because the description says the epic is closure-only.
- That po-critic.on-success -> dev is harmless for a parent epic with no remaining developer-owned implementation slice.

AC / test suggestions
- Add a ticket-level acceptance criterion naming the exact persisted status/label state required before a closure-only epic can clear PO-critic.

Implementation watchouts
- Any automation keyed off blocked/dev or blocked/test can still reopen this closure epic's execution path even though the repository evidence already lives under the four child stories.
- Future first-class Postgres/provider work, SaveChanges interception, or other deferred Data Vault capabilities should be tracked in new follow-up tickets or epics rather than reopening this parent.

Non-blocking notes
- Open Questions is explicitly none in .gicket/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/description.md:46-47, so the rejection is not due to unresolved contract questions.
- The four child stories are already done and the repository files cited by the contract are present, so the remaining gap is ticket-metadata alignment rather than missing repository scope.
- No attachment files were found for the parent ticket, which is consistent with the contract's zero-attachment claim.

Split recommendations
- No additional split is needed for the current epic scope; the existing four child stories remain the bounded delivery path.
- Any future first-class Postgres/provider support, SaveChanges interception, or deferred Data Vault capabilities should be scheduled as separate follow-up tickets or epics.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment