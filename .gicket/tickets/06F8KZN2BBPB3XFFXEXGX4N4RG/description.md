<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the provider identifier preflight story against the landed guardrail contract and visible repository baselines; no child tickets, relation writes, description writes, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The predecessor contract ticket 06F8KZMRXRHRKHV56Y96M4S90G is done, so this story should implement the already-ratified provider identifier and DDL guardrail contract rather than reopen supported-provider or naming-policy decisions.
- The supported-provider baseline for this story is the finite repository set already named in the contract: Sqlite, Oracle, Postgres, SqlServer, and MySql.
- The provider-neutral logical naming baseline remains the existing default naming policy; this story only adds provider-aware preflight validation and deterministic physical-name safety handling where the contract allows it.
- The ticket snapshot shows no recent human comments, and no bounded child-ticket, relation, attachment, or planning-document writes were needed for this refinement pass.
- Broader provider-specific migration guardrail expansion stays in the separate downstream story 06F8KZNBGB8FPW6TK5A8SAJMVC and is not absorbed here.

### Scope In
- Preflight validation for generated table, column, index, key, and constraint names before DVault-owned schema generation or migration DDL emits unsafe provider-specific identifiers.
- Consumption of provider capability/profile facts needed for identifier safety, including identifier-length limits where declared, reserved-word handling, duplicate produced-name detection, and post-truncation collision handling.
- Deterministic diagnostics and guardrail reporting that identify the provider profile, logical or produced name, affected artifact kind, and failure class when a generated name is unsafe or unsupported.
- Validation of unsafe naming-policy and provider combinations when the selected logical naming output cannot be projected to a stable provider-safe physical name within the v1 contract.
- Coverage for the finite supported-provider baseline already visible in the repository and contract.

### Scope Out
- Changing the provider-neutral logical naming rules in docs/naming/default-naming-policy.md or the v1 persistence token set.
- Automatic repair or execution of consumer-authored migrations, raw SQL, or arbitrary third-party DDL.
- Broader provider-specific migration guardrails for destructive changes, nullable timestamp behavior, included-column policy, or non-identifier migration risks already scoped to the separate migration-guardrail story.
- Adding new provider packages, open-ended vendor keyword research, or a broad new public override surface for custom physical naming.

## Acceptance Criteria
- For each supported provider profile, DVault preflight checks generated table, column, index, key, and constraint names against the contract-defined identifier-safety inputs before unsafe DDL is emitted.
- Preflight detects reserved words, duplicate produced names in the same provider-visible scope, post-truncation collisions, and unsafe naming-policy and provider combinations, and it fails fast when no contract-approved safe projection exists.
- When the contract permits provider-safe physical-name derivation, the result is deterministic across runs and preserves logical-to-physical traceability through the existing annotation and diagnostic surfaces.
- Diagnostics identify the provider profile, artifact kind, logical or produced name, and failure reason in bounded DVault diagnostic or report output without rewriting user-authored SQL or migrations.
- Tests cover the finite supported-provider baseline and prove representative success and failure cases for length limits, reserved words, duplicate names, and collision scenarios.

## Definition of Done
- The implementation consumes the authoritative provider and profile selection surfaces already identified by the contract and does not introduce a second source of truth for supported providers or naming rules.
- Unsafe generated identifiers are rejected at the preflight boundary before schema generation or migration guardrail output can emit provider-unsafe DDL.
- Safe provider-specific projections remain deterministic and traceable through existing DVault annotations and diagnostics.
- Automated tests lock the visible provider baseline and the expected diagnostics for representative safe and unsafe naming cases.
- The delivered story stays bounded to identifier preflight checks and leaves broader migration-guardrail expansion to the downstream ticket.

## Implementation Notes
- Use docs/plans/provider-identifier-ddl-guardrail-contract.md as the authoritative contract for supported providers, provider profile facts, deterministic physical-name projection, and fail-fast boundaries.
- Treat src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs and src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs as the provider-profile selection anchors called out by the contract.
- Reuse DataVaultAnnotationNames.ProducedName, MetadataName, ProviderProfile, ProviderLogicalPropertyKind, ProviderStorageType, and ProviderValueFormat for logical-to-physical traceability rather than inventing parallel metadata.
- Route bounded failure reporting through the existing diagnostic and report surfaces named by the contract, including DataVaultDiagnostics, DataVaultMigrationOperationDiagnostics, DataVaultMigrationGuardrailReport, and DataVaultActivityTracing failure and provider tags.
- Keep the validation boundary ahead of emitted DDL, at model-validation and-or migration-guardrail generation time, so unsafe identifiers fail deterministically instead of surfacing as database-specific runtime errors.
- The current contract already exposes a finite baseline with MySql carrying a declared 64-character identifier cap and the other supported profiles using only the identifier-safety facts explicitly declared in the repository contract; implementation should not guess undocumented provider behavior outside that baseline.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add a consumer-visible override hook for provider-specific shortening or quoting, or should v1 stay convention-only?
- After this story lands, do we want separate maintenance coverage to detect provider-package drift in reserved-word sets or identifier-limit facts?

## Risks
- Provider upgrades can change reserved words or identifier behavior, so the finite contract baseline may need maintenance follow-up when dependencies move.
- If deterministic shortening or collision rules change after implementation, existing generated schemas and migration artifacts could churn.
- Fail-fast validation may expose previously hidden unsafe names in existing models, which is correct but may need rollout communication for consumers.

## Split Recommendations
- No PO split is required for this story; keep broader migration-risk work in 06F8KZNBGB8FPW6TK5A8SAJMVC rather than widening this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement preflight checks for generated table, column, index, and constraint names against provider identifier length limits, reserved words, duplicate produced names, and unsafe naming-policy combinations.