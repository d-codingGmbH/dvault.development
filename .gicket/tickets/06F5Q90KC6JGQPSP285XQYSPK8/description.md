<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement confirms this is a bounded additive story: add registry-backed PIT maintenance over the existing explicit hub-parent PIT maintenance pipeline, keep future link-parent and multi-active PIT work in the already split child stories, and make no planning or ticket writes in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The authoritative baseline is the existing explicit `IDataVaultPitMaintenanceService`; registry-backed support must resolve one `DataVaultPitMetadata` from `UseDataVaultMetadata()` and then delegate into `DataVaultPitRebuildRequest` or `DataVaultPitParentMaintenanceRequest` rather than introducing a second maintenance engine.
- `DataVaultMetadataRegistry` already supports exact PIT lookup by logical name and exact CLR type through `TryGetPit(string, ...)`, `TryGetPit(Type, ...)`, and `DataVaultMetadataClrMapping.Pit(...)`; this story should reuse that exact-match behavior and must not add fuzzy matching or first-match fallback.
- The incoming `blocks` relation from done task `06F5Q90718D21DN1N1Q2AP7YEM` is historical only and is not a live blocker for this ticket.
- No bounded child-ticket, relation, description, attachment, or planning-document writes were applied during this refinement run.

### Scope In
- Add additive registry-backed PIT maintenance request surface(s) for callers that configure an authoritative registry through `UseDataVaultMetadata()`.
- Support exact PIT resolution by logical name and by exact CLR mapping registered through `DataVaultMetadataClrMapping.Pit(...)`.
- Delegate registry-backed rebuild and parent-maintenance calls to the existing explicit PIT maintenance pipeline so current validation, no-op handling, and row-generation semantics stay unchanged.
- Add tests, public API snapshot updates, and current-surface documentation updates required to expose the new registry-backed maintenance path.

### Scope Out
- Link-parent PIT maintenance or link-parent PIT read behavior.
- Multi-active PIT maintenance semantics or driving-key PIT row rules.
- Automatic, background, scheduled, or `SaveChanges`-triggered PIT maintenance.
- Provider-specific PIT maintenance optimization or any non-registry metadata resolution heuristics.

## Acceptance Criteria
- Callers with an authoritative registry selected through `UseDataVaultMetadata()` can invoke registry-backed PIT rebuild and bounded parent maintenance without supplying `DataVaultPitMetadata` directly.
- The registry-backed surface resolves PIT metadata by exact logical name and by exact CLR mapping registered through `DataVaultMetadataClrMapping.Pit(...)`, then delegates to explicit `DataVaultPitRebuildRequest` and `DataVaultPitParentMaintenanceRequest` execution.
- Before any writes, the registry-backed path throws deterministic `InvalidOperationException` diagnostics when the DbContext lacks an authoritative registry, the requested PIT logical name is missing, the requested PIT CLR mapping is missing, or the resolved PIT falls outside the supported hub-parent non-multi-active baseline.
- Empty parent-hash-key requests remain no-op, and parent-hash-key validation, ordinal deduplication, UTC/timestamp handling, and PIT row-generation behavior remain identical to the explicit maintenance path.
- Unit tests, SQLite integration tests, and public API snapshot coverage cover both name-based and CLR-mapped rebuild and parent-maintenance flows.

## Definition of Done
- The new public API surface is snapshot-approved and documented beside the existing explicit PIT maintenance guidance.
- Current public guidance that still excludes registry-backed PIT maintenance is updated where applicable, at least in the README and any active adoption guidance that currently marks it out of scope, while preserving the exclusions for link-parent PITs, multi-active PITs, automatic orchestration, and provider-specific optimization.
- Regression coverage demonstrates no behavior change for callers that keep using explicit `DataVaultPitMetadata` requests.

## Implementation Notes
- Follow the existing bridge registry-maintenance pattern: resolve registry metadata once via `DataVaultRegistryMetadataResolver`, then delegate to the explicit maintenance service.
- Add PIT resolver support for exact-name and exact-CLR lookup instead of duplicating registry lookup logic in each extension method.
- CLR ambiguity is already a registry-construction error enforced by `DataVaultMetadataRegistry.Create(...)`; maintenance-time CLR resolution should only succeed on an exact registered mapping and otherwise fail deterministically.
- The repository already proves the hub-parent non-multi-active maintenance baseline through explicit PIT maintenance tests, so this story should layer registry resolution on top of that baseline rather than reopen PIT shape semantics.

## Open Questions
- none

## Follow-Up Questions
- After this lands, should a separate registry-backed PIT as-of read request surface be planned, or should PIT reads remain explicit-metadata-only for now?

## Risks
- The current README and production-adoption guidance explicitly say registry-backed PIT maintenance is out of scope; leaving those statements unchanged would create public contract drift after implementation.
- Because downstream link-parent and multi-active PIT stories are already split out, accidentally broadening validation or row-generation semantics in this story would blur ticket boundaries and risk regressions in the existing hub-parent baseline.
- This ticket currently blocks `06F5Q90SX5AQ07M4PQKDR4BZD8` and `06F5Q9102970H1VQN16QWRGQX0`, so incomplete registry error handling or missing tests would delay both follow-on PIT stories.

## Split Recommendations
- No additional split is recommended. The work is already bounded to additive registry resolution over the existing PIT maintenance engine, and the larger link-parent and multi-active PIT expansions are already split into `06F5Q90SX5AQ07M4PQKDR4BZD8` and `06F5Q9102970H1VQN16QWRGQX0`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Add registry-backed PIT maintenance request support analogous to existing registry-backed save/read paths.

Acceptance criteria:
- Resolves PIT metadata by logical name or CLR mapping from UseDataVaultMetadata.
- Provides deterministic validation and diagnostics for missing, ambiguous, or incompatible metadata.
- Adds unit and integration coverage for registry-backed PIT rebuild and parent maintenance.