[gicket-bot] PO-critic review contract

Summary
- Provider baseline and translator/diagnostics anchors are solid, but the contract over-claims unique-constraint coverage without local API evidence and the downstream documentation dependency is not yet persisted as blocked.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/description.md:55-56` says `## Open Questions` -> `- none`, so the persisted contract has no unresolved open questions.
- `.gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/description.md:18-22`, `34-36`, and `50-52` put `unique secondary-index or unique-constraint surfaces` and `bounded uniqueness surfaces` directly into Scope In, Acceptance Criteria, and Implementation Notes.
- `src/DCoding.Data.DVault/Modeling/IDataVaultNamingPolicy.cs:184-193` defines `DataVaultConstraintKind` with only `PrimaryKey` and `ForeignKey`.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted>` currently materializes `Constraints` from the primary key only, while `src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:239-273` handles index and primary-key operations but has no unique-constraint operation cases.
- Repo search `rg -n "AddUniqueConstraintOperation|DropUniqueConstraintOperation|UniqueConstraint" src tests -S` returned no matches.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:420-558` and `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:805-859,913-938` already ground the five-provider baseline, Oracle PK-covered-index omission, include-column behavior, and provider store-type/value-format annotations that this story wants to reuse.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:237-297` exposes property nullability/store type/value format and index descending/include metadata, but `src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:<redacted>` reduces the migration baseline to column role/metadata plus index property names/uniqueness only.
- `git log --oneline --max-count=4` on branch `ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua` shows only ticket-handoff commits after `develop`, and `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/...` files.

Blocking findings
- The contract makes unique-constraint compatibility part of the story, but local source evidence does not identify a concrete DVault constraint surface or EF migration operation to target. Today the repo exposes only primary-key/foreign-key constraint kinds, diagnostics only emit primary-key constraints, migration diagnostics only handle index and primary-key ops, and repo-wide search found no `AddUniqueConstraintOperation`/`DropUniqueConstraintOperation` evidence. The ticket needs either a concrete API/type citation or a narrowed scope.

Required PO actions
- Either narrow the ticket from `unique-constraint surfaces` to the generated index/primary-key surfaces already evidenced in the repo, or add direct source-backed evidence naming the exact in-scope EF/Core operation types and baseline objects for unique-constraint handling.

Open issues ledger
- critic-item-1 [required-po-action] Either narrow the ticket from `unique-constraint surfaces` to the generated index/primary-key surfaces already evidenced in the repo, or add direct source-backed evidence naming the exact in-scope EF/Core operation types and baseline objects for unique-constraint handling.
- critic-item-2 [blocking-finding] The contract makes unique-constraint compatibility part of the story, but local source evidence does not identify a concrete DVault constraint surface or EF migration operation to target. Today the repo exposes only primary-key/foreign-key constraint kinds, diagnostics only emit primary-key constraints, migration diagnostics only handle index and primary-key ops, and repo-wide search found no `AddUniqueConstraintOperation`/`DropUniqueConstraintOperation` evidence. The ticket needs either a concrete API/type citation or a narrowed scope.

Missing examples / edge cases
- If unique-constraint handling stays in scope, add at least one concrete example showing which provider/EF migration operation sequence represents an `equivalent generated uniqueness surface` and what report path/finding is expected.
- Add one example that distinguishes a safe Oracle omitted PK-covered secondary index from an incompatible replacement or rename, so the absence/presence rule is unambiguous.
- Add a timestamp-drift example for both `LoadTimestamp` and PIT snapshot reference columns across `provider-default`, `iso-8601-utc-text`, and `utc-ticks`, covering nullability, store type, and value-format failures.

Risky assumptions
- The contract assumes provider-specific uniqueness behavior can be implemented against an existing public/source surface, but the current repo does not show a unique-constraint API or migration-operation path.
- The contract assumes the separate documentation task is effectively blocked even though its persisted ticket state is not blocked yet.
- The contract assumes provider packages will surface the in-scope uniqueness behavior through a bounded, known set of operation shapes without citing the exact shapes.

AC / test suggestions
- Name the exact EF migration operation types expected for every in-scope path; if unique-constraint scope remains, call out the exact unique-constraint operation types explicitly instead of using only prose.
- Add one worked acceptance example for each provider behavior class: native includes (SQL Server/Postgres), append-to-key fallback (Sqlite/Oracle), ignore-includes (MySql), and Oracle duplicate-index omission.
- Add explicit pass/fail examples for timestamp drift showing expected vs incompatible nullability/store type/value format for both `LoadTimestamp` and PIT snapshot reference columns.

Implementation watchouts
- `DataVaultPropertyExplain` already carries `IsNullable`, `StoreType`, `ValueFormat`, and `ProviderProfileName`, but `DataVaultMigrationColumnBaseline` currently drops those fields; implementation will need to preserve them instead of inventing a second provider matrix.
- `DataVaultIndexExplain` already carries `DescendingPropertyNames` and `IncludedPropertyNames`, but `DataVaultMigrationIndexBaseline` currently keeps only `PropertyNames` and `IsUnique`.
- If unique-constraint scope remains, explain/baseline/report types need explicit constraint modeling because the current explain path only materializes primary-key constraints.

Non-blocking notes
- The provider baseline itself is well grounded: the contract doc lists the five supported providers and the translator/provider-capability code matches the described include handling, duplicate-index rule, and load-timestamp mappings.
- Open Questions is explicitly `none`, so this ticket is not blocked by unresolved question text.
- The owner branch currently contains ticket metadata changes only; there is no implementation diff yet, which is acceptable for a pre-development PO handoff.

Split recommendations
- Keep provider-specific index/timestamp guardrails in one implementation story.
- If PO wants to preserve unique-constraint behavior but it requires new explicit constraint surfaces or separate provider research, split that uniqueness-surface lane into a follow-up instead of leaving it underspecified in this story.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment