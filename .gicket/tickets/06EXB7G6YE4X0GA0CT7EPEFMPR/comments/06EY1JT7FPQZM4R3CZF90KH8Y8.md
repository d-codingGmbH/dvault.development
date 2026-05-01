[gicket-bot] PO-critic review contract

Summary
- Parent story is consistently refined as an umbrella over two done child tickets, and current repository evidence matches the SQLite EnsureCreated plus committed schema snapshot baseline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7G6YE4X0GA0CT7EPEFMPR/description.md has Open Questions = none and states the parent has no uncaptured developer-owned slice remaining beyond child tickets 06EXB7GESWZZTZG7XYAKTTKQRW and 06EXB7GPRGEJHKFMJ8MVAVF8ZG.
- git diff --stat 988702590554acf8e98015ecce765a48d40980ce..1aca4a0a125b70cea07c9cf4f062cdfc74e774e6 reports only .gicket ticket-file changes, matching the contract that no new parent-story implementation slice remains.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs, src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs, and src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs directly expose the claimed SQLite path: ApplyDataVaultMetadata calls the translator, the translator builds hub, link, and satellite entities with named keys and indexes, and DataVaultProviderCapabilityProfiles.Sqlite defines the SQLite profile.
- tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs calls context.Database.EnsureCreated in all three integration tests, asserts concrete hub, link, and satellite schema names and indexes, compares against tests/DCoding.Data.DVault.Tests/Integration/Snapshots/SqliteDataVaultSchemaSnapshot.txt, and keeps the negative UseDataVault-alone baseline.
- Repository grep for EnsureCreated, migration, and design-time references finds EnsureCreated in the SQLite schema tests and migration or design-time mentions only in docs and in .gicket/tickets/06EXB7GPRGEJHKFMJ8MVAVF8ZG/description.md, which explicitly defers migration snapshot coverage.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep future story-level wording anchored to the existing EnsureCreated path so the legacy migration-or-create-database phrasing does not reintroduce migration scope by implication.

Implementation watchouts
- Do not reopen parent-story implementation scope on this branch; the validated execution boundary is the two done child tickets plus their already-integrated commits c0ace7dd and bc36e503.
- Treat migration behavior as explicitly out of scope even though child ticket 06EXB7GPRGEJHKFMJ8MVAVF8ZG still has a legacy title mentioning migration snapshot tests; its refined description defers migration-specific coverage.

Non-blocking notes
- .gicket/tickets/06EXB7G6YE4X0GA0CT7EPEFMPR/comments/06EY1HEKJABA3B6BP4W6M405NM.md records follow-ups for both parentOf child paths and the two downstream blocks paths, so the relation narrative in the contract is backed by persisted ticket comments.

Split recommendations
- No additional split recommended; the story already separates mapping work into 06EXB7GESWZZTZG7XYAKTTKQRW and schema-regression coverage into 06EXB7GPRGEJHKFMJ8MVAVF8ZG.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment