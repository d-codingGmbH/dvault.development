<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Narrowed the story from unsupported unique-constraint handling to the repository-evidenced generated secondary-index and primary-key guardrail surfaces; no child-ticket, relation, attachment, planning-document, or direct ticket-description write was materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The story scope is narrowed from generated unique secondary-index or unique-constraint surfaces to generated secondary indexes and primary keys already produced by the current DVault baseline.
- Unique compatibility in this story is limited to the existing generated unique secondary-index surface, such as the hub business-key index emitted with IsUnique=true; separate DVault-owned unique-constraint objects are out of scope until the repository exposes them.
- Repository evidence already fixes the provider-specific effective index-shape baseline: SQL Server and Postgres keep native include columns, Sqlite and Oracle append unsupported include columns to the key, MySql ignores include columns, and Oracle omits secondary indexes covered by the primary key.
- The prompt snapshot remained the ticket and comment evidence for this pass because gicket ticket, comment, and relation reads were trust-blocked; no relation-dependent split or cleanup decision was required.

### Scope In
- Provider-specific guardrail checks for generated secondary indexes and primary keys against the effective provider shape selected by the translator.
- Unique secondary-index compatibility only where the current generated index baseline already uses IsUnique=true, including provider-aware column, include-column, and duplicate-index-covered-by-primary-key behavior.
- Guardrail validation of generated load timestamp and PIT satellite snapshot reference columns for presence, nullability, store type, and provider value-format compatibility with the selected loadTimestampStorage token.
- Provider-aware destructive-change and suspicious-replacement detection for DVault-owned generated tables, columns, primary keys, and secondary indexes before migration application.
- Structured diagnostics and report output through existing DataVaultDiagnostics, DataVaultMigrationGuardrailReport, DataVaultPreflight, and guardrail command surfaces with provider/profile and effective-shape evidence.
- Automated coverage across the finite five-provider baseline and representative loadTimestampStorage variants.

### Scope Out
- Separate EF unique-constraint operation handling such as AddUniqueConstraintOperation or DropUniqueConstraintOperation until the repository exposes a DVault-owned unique-constraint baseline.
- Changing docs/naming/default-naming-policy.md, the provider-neutral logical naming baseline, or the finite supported-provider baseline.
- Automatic migration execution, migration rewriting, schema repair, or interception of consumer-owned EF command execution.
- Broad live-schema or idempotency feature expansion beyond the migration-guardrail lane needed to explain provider-specific index and timestamp drift.
- New provider packages, open-ended vendor keyword research, or a consumer-visible override API for physical naming or provider-specific guardrail policy.
- Release-note, adoption-guide, and public documentation work already scoped to task 06F8KZNNS76TD9Z7ESB173FZ68.

## Acceptance Criteria
- Given any supported provider profile and loadTimestampStorage selection, migration guardrail evaluation uses the same effective generated shape the translator emits for generated secondary indexes and primary keys, including omitted Oracle primary-key-covered secondary indexes, native include columns on SQL Server and Postgres, appended include columns on Sqlite and Oracle, and ignored include columns on MySql.
- A migration that creates, recreates, renames, drops, or replaces a DVault-owned generated secondary index or primary key with provider-incompatible columns, uniqueness, include handling, or duplicate-index behavior returns blocking guardrail findings before migration application.
- A migration that makes generated load timestamp or PIT satellite snapshot reference columns nullable, changes their provider store type, or changes their provider value format away from the selected profile plus loadTimestampStorage contract returns blocking guardrail findings.
- Suspicious drop-plus-add or provider-specific replacement patterns affecting DVault-owned generated tables, columns, primary keys, or secondary indexes are reported deterministically through the guardrail report with the offending operation path and remediation guidance.
- Provider-specific guardrail findings name the selected provider/profile and the effective generated object shape needed to explain the failure through the existing diagnostics/report surfaces without executing or rewriting the migration.

## Definition of Done
- DataVaultMigrationOperationDiagnostics and the central DVM catalog cover the provider-specific migration risks in this story for generated secondary indexes, primary keys, and timestamp storage drift without creating a second provider-facts source of truth.
- The selected provider capability profile and loadTimestampStorage mapping remain the authoritative source for effective index shape and timestamp storage expectations used by guardrail evaluation.
- Guardrail and preflight outputs stay bounded, deterministic, and automation-friendly through existing report and command surfaces; DVault still does not apply or rewrite migrations.
- Unit tests lock the visible five-provider baseline and the expected safe, risky, and incompatible outcomes for representative generated secondary-index and primary-key migration operations.
- The separate downstream documentation task 06F8KZNNS76TD9Z7ESB173FZ68 remains blocked by implementation completion rather than being widened into this story.

## Implementation Notes
- Use src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs, src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs, and diagnostics explain output as the authoritative provider-shape baseline instead of hard-coding a second migration-only matrix.
- The current translator already exposes the relevant provider differences: Oracle disallows redundant secondary indexes covered by the primary key, MySql ignores unsupported include columns, and SQL Server and Postgres persist native include annotations.
- The current repository baseline shows generated unique behavior only through generated indexes with IsUnique=true; the modeling and diagnostics constraint surfaces expose primary-key constraints today and do not expose a separate DVault unique-constraint family.
- Keep migration-operation handling bounded to the currently evidenced EF surfaces: CreateIndex, DropIndex, RenameIndex, AddPrimaryKey, and DropPrimaryKey, plus the existing generated table and column operations already covered by DataVaultMigrationOperationDiagnostics.
- Use existing annotations and explain data such as ProducedName, MetadataName, ProviderProfile, ProviderValueFormat, descending-column metadata, and included-column metadata to report logical-to-physical traceability in provider-specific findings.
- Keep the migration execution boundary consumer-owned: the story should stop at validation and reporting in diagnostics, preflight, and guardrail command surfaces.

## Open Questions
- none

## Follow-Up Questions
- After implementation, should task 06F8KZNNS76TD9Z7ESB173FZ68 explicitly document the per-provider effective index-shape differences and the narrowed unique secondary-index baseline now enforced by guardrails?
- If a later ticket adds a DVault-owned unique-constraint baseline, should it explicitly extend DataVaultConstraintKind, diagnostics explain output, and migration-operation handling before re-opening unique-constraint guardrails?
- Should a later follow-up widen provider profile data from the current shared MaximumIdentifierLength surface to object-class-specific limits where a supported provider proves narrower constraint classes?

## Risks
- Provider package upgrades can change emitted EF migration operation shapes, so tests must lock the bounded supported operation set or provider-specific drift may evade guardrails.
- Tightening timestamp nullability and store-type checks can surface previously tolerated migrations as blocking findings, which is correct but may require clearer upgrade guidance for consumers.
- The broader provider identifier contract mentions future constraint classes, so developers may try to widen this implementation beyond the current primary-key and secondary-index baseline unless the narrowed story boundary stays explicit.

## Split Recommendations
- No PO split is required after narrowing scope; the visible repository baseline keeps provider-specific migration guardrails bounded to one implementation story plus the already-separate documentation task.
- If implementation later needs DVault-owned unique-constraint modeling, diagnostics, or migration-operation support, queue that as a separate follow-up ticket instead of widening this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Extend migration guardrails for provider-specific index, unique constraint, nullable timestamp, included-column, and destructive generated-structure risks while keeping migration execution consumer-owned.