<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement incorporates the PO-critic finding into the parent story: one narrow implementation gap remains in hierarchy bridge metadata validation, and it belongs in this story before final closure.

### PO Handoff
- decision: `ready_for_dev`
- meaning: ticket can move to developer implementation for the remaining hierarchy-validation gap

### Clarifications
- Repository-backed v1 scope is already fixed by docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md and current source: bridge support is opt-in, additive, and limited to many-to-many plus hierarchy bridge metadata over existing hubs and links.
- Provider-neutral EF projection is the bounded implementation target: bridge tables are shared-type entities with deterministic produced names, ordered endpoint hash-key columns, bridge-specific keys/indexes, annotations, and no implicit EF foreign keys, navigations, save-service changes, migrations, or provider-specific DDL behavior in v1.
- Existing parentOf split is already materialized on the parent ticket: 06EZ0NV0Y81AE1Z1Q3223TX2S4 owns bridge metadata and validation, 06EZ0NV7KG94MTMNXMGVRYVW9C owns translator/generation behavior, 06EZ0NVE88WW9PMM04NVAZHRG0 is the historical docs child, and done follow-up 06F03T9R8QK81VQCC158NJ62YG is the documentation-reconciliation gate before story closure.
- No new child ticket is required for the remaining gap; keeping the focused validation fix on this parent avoids another tracking loop after the bridge metadata child is already done.

### Scope In
- Opt-in bridge metadata declarations in the modeling surface and DataVaultMetadataModel bridge collection.
- Baseline bridge kinds limited to many-to-many traversal and hierarchy traversal over existing hubs and links.
- Provider-neutral EF metadata generation for bridge shared-type entities, including deterministic table/column/key/index naming and bridge/property annotations.
- Validation for missing or wrong references, invalid endpoint bindings, incorrect many-to-many link membership, invalid recursive hierarchy shapes, and other unsupported bounded bridge definitions.
- Tests and documentation that prove and explain the supported baseline, minimal examples, and explicit v1 limitations.

### Scope Out
- Bridge row population, traversal maintenance, closure-state management, or any runtime orchestration beyond EF metadata projection.
- EF relationship graph generation, implicit foreign keys or navigations, save-service changes, migrations, or provider-specific DDL/optimization behavior.
- Effectivity windows, path payload columns, advanced hierarchy semantics, complex traversal variants, and PIT or multi-active interactions.
- Any default-on behavior that changes existing link semantics when no bridge metadata is declared.

## Acceptance Criteria
- Bridge support is opt-in and leaves existing hub/link/satellite projection behavior unchanged when no bridge metadata is declared.
- Many-to-many bridge declarations require exactly one existing link and exactly two distinct hub endpoints named from and to; hierarchy bridge declarations require one recursive self-link with exactly two participants, both over the same hub type, and explicit ancestor and descendant role bindings.
- The EF translator produces provider-neutral shared-type bridge entities with deterministic names, ordered endpoint hash-key columns, primary keys, traversal indexes, and bridge/property annotations consistent with the v1 bridge contract.
- Hierarchy bridge projection adds only the TraversalDepth column as bridge-depth metadata; many-to-many bridge projection adds only endpoint hash-key columns.
- Validation and translator tests cover deterministic naming, endpoint order, keys, indexes, link references, annotation roles, validation failures, and rejection of unsupported projection features or advanced bridge semantics outside the baseline. Hierarchy validation must include negative coverage for links such as Employee-Employee-Department and Employee-Employee-Employee, because those are not the contracted two-participant self-link shape.
- Documentation includes a minimal bridge example and clearly distinguishes implemented bridge baseline behavior from deferred advanced capabilities; the reconciliation work tracked by 06F03T9R8QK81VQCC158NJ62YG must remain reflected in durable docs.

## Definition of Done
- Modeling, translation, tests, and docs all align with the v1 bridge contract in docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md.
- Child-ticket outputs stay within the established split: metadata/validation in 06EZ0NV0Y81AE1Z1Q3223TX2S4, translator/generation in 06EZ0NV7KG94MTMNXMGVRYVW9C, and documentation alignment through the existing docs children including 06F03T9R8QK81VQCC158NJ62YG.
- Repository tests prove deterministic bridge metadata projection and failure handling for unsupported or invalid bridge definitions, including hierarchy links that contain extra participants or mixed hub types.
- Durable documentation no longer contradicts current bridge source and test behavior and keeps deferred capabilities explicitly out of the implemented baseline.

## Implementation Notes
- Current source already reflects the baseline vocabulary: src/DCoding.Data.DVault/DataVaultAnnotationNames.cs contains Bridge and BridgeDepth-related annotation/property-role values, and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs already contains dedicated bridge translation paths for many-to-many and hierarchy bridge projections.
- Use the bridge contract document as the authoritative naming, role, and ownership reference; it explicitly assigns metadata validation to 06EZ0NV0Y81AE1Z1Q3223TX2S4 and translator-time unsupported-feature failures to 06EZ0NV7KG94MTMNXMGVRYVW9C. The remaining parent-story implementation must tighten DataVaultMetadataModel.ValidateHierarchyBridge so hierarchy bridges only accept exactly two link participants and both participants refer to the same hub type.
- Keep bridge work additive to the current hub/link/satellite baseline documented in docs/architecture/mvp-data-vault-concepts.md and the deferred capability record; bridge support must not reopen save-service, provider profile, or runtime maintenance scope under this story.
- Treat 06F03T9R8QK81VQCC158NJ62YG as the active guardrail for documentation accuracy because the earlier docs child 06EZ0NVE88WW9PMM04NVAZHRG0 captured an older deferred-only snapshot.

## Open Questions
- none

## Follow-Up Questions
- When bridge projection baseline is complete, should a later ticket define how bridge rows are populated and maintained at runtime, or should bridge consumers be expected to manage data population externally in the near term?
- Which deferred bridge expansions, if any, should be prioritized next: provider-specific physical optimization, effectivity/path payload support, advanced hierarchy semantics, or PIT/multi-active interactions?

## Risks
- If hierarchy validation remains broader than the v1 contract, bridge metadata can describe unsupported traversal shapes that the translator would still project.
- If documentation drifts back toward a deferred-only description, consumers may misread the implemented baseline or assume unsupported runtime bridge behavior that this story does not deliver.

## Split Recommendations
- No additional split is recommended: the parent story can carry the narrow hierarchy-validation correction directly, because all earlier bridge children are already done and the remaining work is a closure gap discovered at parent review.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: add baseline bridge table modeling and EF generation for relationship traversal scenarios.

Scope:
- Define bridge metadata for many-to-many and hierarchy-style traversal over existing hubs and links.
- Generate provider-neutral EF metadata for bridge structures.
- Document supported traversal shapes and explicit limitations.

Acceptance Criteria:
- Bridge modeling is opt-in and does not change existing link behavior by default.
- Tests cover deterministic naming, keys, link references, and validation failures.
- Documentation includes a minimal bridge example and explains unsupported advanced traversal patterns.