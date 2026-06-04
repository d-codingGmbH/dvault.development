[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the contract is now bounded to repository-evidenced secondary-index, primary-key, and timestamp guardrails, and Open Questions is none.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/description.md:12-26 narrows scope to generated secondary indexes and primary keys, limits uniqueness to existing generated unique secondary indexes, and explicitly scopes out separate AddUniqueConstraintOperation and DropUniqueConstraintOperation handling.
- .gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/description.md:55-56 shows Open Questions -> none, so the persisted contract has no unresolved open questions.
- git diff --unified=0 234002eb0 198381aa8 -- .gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/description.md shows the PO refinement removed unique-constraint claims from Scope In and Acceptance Criteria and added explicit Scope Out for separate unique-constraint EF operations.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:176-185 emits an existing generated hub business-key index with IsUnique true, and :803-910 applies provider-effective index shaping including Oracle PK-covered-index omission and provider-specific include-column handling.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:455-558 encodes the five-provider load-timestamp store types and value formats plus Oracle allowsIndexesCoveredByPrimaryKey false and MySQL unsupportedIncludedIndexColumnMode Ignore.
- src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:178-185 and :239-267 already handle the bounded EF migration operations named in the contract: CreateIndex, DropIndex, RenameIndex, AddPrimaryKey, and DropPrimaryKey.
- rg -n AddUniqueConstraintOperation|DropUniqueConstraintOperation|UniqueConstraint src tests returned no matches, and src/DCoding.Data.DVault/Modeling/IDataVaultNamingPolicy.cs:184-193 exposes only PrimaryKey and ForeignKey, which matches the contract's new unique-constraint scope-out.
- git diff --name-only develop..HEAD lists only .gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC artifacts, and git log --oneline --decorate --max-count=8 shows only ticket workflow commits on ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua, so this remains a pre-development ticket-handoff branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add one explicit Oracle example where a PK-covered generated secondary index is omitted physically, then a migration tries to rename, drop, or recreate it, so the expected guardrail outcome is unambiguous.
- Add one MySQL example that distinguishes include-only drift from a provider-visible index-shape change, since MySQL ignores include columns.
- Add worked timestamp-drift examples for both LoadTimestamp and PIT snapshot reference columns across provider-default, iso-8601-utc-text, and utc-ticks.

Risky assumptions
- The contract leaves representative loadTimestampStorage variants to implementation judgment instead of enumerating an exact provider-by-token matrix.
- The bounded EF migration-operation set will stay stable enough that provider package upgrades do not introduce new in-scope operation shapes without tests catching them.

AC / test suggestions
- Keep at least one pass/fail example for each provider behavior class: native includes for SQL Server and Postgres, append-to-key fallback for Sqlite and Oracle, ignore-includes for MySQL, and Oracle duplicate-index omission.
- Lock regression tests around the current bounded operation set and fail visibly if new EF migration operation shapes become relevant to DVault-owned generated indexes or primary keys.
- Make the timestamp expectations explicit in tests for nullability, store type, and value format on both generated load timestamp and PIT snapshot reference columns.

Implementation watchouts
- Keep provider-shape truth anchored in DataVaultProviderCapabilities and DataVaultEfMetadataTranslator; do not introduce a second migration-only provider matrix.
- Preserve existing diagnostics traceability fields such as ProducedName, MetadataName, ProviderProfile, ProviderStorageType, ProviderValueFormat, descending-column metadata, and included-column metadata when explaining failures.
- Do not widen the lane back into unique-constraint modeling unless the repository first exposes a concrete DVault-owned unique-constraint surface and matching migration-operation evidence.

Non-blocking notes
- The previous PO-critic blocker was directly addressed: .gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/comments/06F967789CFBAHVQHV5RDKEAKW.md required scope narrowing, and .gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/comments/06F969HY3T3NZZAP5WEQ475C5C.md records the durable refinement and handoff back to po-critic.
- The story already blocks downstream documentation ticket 06F8KZNNS76TD9Z7ESB173FZ68 via .gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/events/06F8M01HFAPP25GDP01194TR68.json, so documentation scope remains separated.

Split recommendations
- No split is needed for the current narrowed secondary-index, primary-key, and timestamp-guardrail lane.
- If later work needs DVault-owned unique-constraint modeling or provider-specific unique-constraint migration support, open a separate follow-up ticket instead of widening this story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment