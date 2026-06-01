<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the bridge-helper story against the completed architecture contract and current repository baseline. The ticket is already bounded to support-bundle-driven many-to-many and hierarchy bridge helper generation, with documentation rollout left to the downstream docs task.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the bridge endpoint vocabulary to From, To, Ancestor, and Descendant, and DataVaultBridgeReadRequest already requires bounded maximumDepth for hierarchy bridges while rejecting depth on many-to-many bridges.
- The authoritative design source is docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md; related story 06F7Y0GT7A5QT77TADMRZBVYN8 is done and should be treated as historical contract input rather than a new PO blocker.
- Relation context is coherent for planning: this ticket is a child of epic 06F7Y0FR4JS1V9WHFBP70GX1SM, it currently blocks docs task 06F7Y0HZKHBHMYX9EYDYFRYXZ0, and no new child ticket or planning document is needed because implementation and docs are already separated.

### Scope In
- Generate support-bundle-driven typed bridge helpers for supported many-to-many bridge reads using Read{ProducedName}FromAsync and Read{ProducedName}ToAsync.
- Generate support-bundle-driven typed bridge helpers for supported hierarchy bridge reads using Read{ProducedName}AncestorAsync and Read{ProducedName}DescendantAsync with required bounded maximumDepth.
- Emit bridge read-model records and constants that project endpoint hash-key members in generated order and TraversalDepth for hierarchy bridges.
- Preserve deterministic diagnostics and helper isolation so unsupported bridge shapes fail or skip per-entity without suppressing unrelated satellite or bridge helpers.

### Scope Out
- Typed PIT helper generation, PIT acceptance criteria, and any PIT-specific read-shape work.
- Bridge or PIT maintenance, read-time refresh, SaveChanges orchestration, provider-specific SQL, or dynamic query compilation.
- Raw dvault.model.v1 parsing, source-visible Code-First inference, or model-first expansion beyond the support-bundle boundary.
- Release-note, README, and read-plan documentation rollout tracked by downstream task 06F7Y0HZKHBHMYX9EYDYFRYXZ0.

## Acceptance Criteria
- When exactly one authoritative dvault.support-bundle.v1 includes bounded readShape.bridge explain evidence for a supported many-to-many bridge, the generator emits {ProducedName}ReadModel plus {ProducedName}ReadExtensions with Read{ProducedName}FromAsync and Read{ProducedName}ToAsync under the existing typed read-model namespace pattern.
- When the authoritative support bundle includes bounded hierarchy bridge evidence, the generator emits Read{ProducedName}AncestorAsync and Read{ProducedName}DescendantAsync, and each hierarchy method requires an explicit inclusive maximumDepth parameter.
- Generated bridge helpers construct stable bridge metadata and read-request values over the existing IDataVaultReadService boundary and preserve current runtime semantics instead of introducing new runtime APIs or provider-specific behavior.
- Generated bridge read models expose compatibility constants ProducedTableName, MetadataSourceKind, MetadataSourceFingerprint, {MemberName}ProducedColumnName, and {MemberName}MappedName, and project only bridge-row members: endpoint hash keys in generated order plus TraversalDepth for hierarchy bridges.
- Missing or ambiguous support-bundle input, unsupported bridge helper evidence, name collisions, dynamic or unbounded traversal shapes, and intentional residual skips surface deterministic DMV1960, DMV1961, DMV1964, DMV1965, DMV1967, or DMV1969 diagnostics as appropriate, while unrelated valid helpers continue generating.
- Coverage proves supported many-to-many and hierarchy helper emission, deterministic generated-source shape, and runtime-equivalent bridge projections without regressing existing satellite helper generation.

## Definition of Done
- Source generator bridge paths replace the current bridge skip-only behavior for supported shapes and keep unsupported residual shapes on deterministic diagnostics.
- Generator unit or approval tests cover many-to-many and hierarchy success cases plus bridge-specific DMV1964, DMV1967, and DMV1969 outcomes and isolation from unrelated satellite helpers.
- Runtime-oriented tests verify generated bridge helpers preserve existing bridge read semantics, including the closed endpoint vocabulary and bounded hierarchy depth handling.
- No new public runtime read primitive, provider-specific query surface, or documentation-only scope is introduced in this ticket.

## Implementation Notes
- Extend DataVaultTypedReadModelSourceGenerator to consume authoritative support-bundle bridge explain facts instead of reporting bridge helpers as always skipped; reuse the existing namespace, record, extension-type, and constant conventions already used for typed satellite helpers.
- Map helper methods directly to the closed DataVaultBridgeTraversalEndpoint vocabulary: many-to-many uses From and To, hierarchy uses Ancestor and Descendant, and hierarchy generation must not emit an unbounded overload.
- Delegate through the existing bridge read-service path by constructing DataVaultBridgeReadRequest values and projecting maintained bridge rows; do not add maintenance calls, SQL generation, graph or path APIs, or raw metadata parsing.
- Use the existing diagnostics and read-shape baseline as repository evidence: current runtime diagnostics already expose bridge kind and endpoint filter facts, and current runtime request validation already enforces bounded hierarchy depth.

## Open Questions
- none

## Follow-Up Questions
- When downstream docs task 06F7Y0HZKHBHMYX9EYDYFRYXZ0 executes, should release and read-plan wording move typed bridge helpers from future additive contract language into the active implementation baseline?

## Risks
- Helper generation is gated by request-bound readShape.bridge support-bundle evidence; redaction or missing endpoint order, filter, or depth facts will intentionally suppress bridge helpers even when runtime bridge metadata exists.
- Hierarchy helpers must preserve the current inclusive maximumDepth boundary exactly; emitting an unbounded overload or widening depth semantics would silently change runtime behavior.
- Deterministic generated-name collisions across bridge types, methods, or constants must still fail with DMV1965 instead of partially emitting broken helper code.

## Split Recommendations
- No additional split is recommended. The current story is already bridge-only, the upstream contract story is complete, and downstream documentation work is already separated into ticket 06F7Y0HZKHBHMYX9EYDYFRYXZ0.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Generate typed bridge read helpers for supported reviewed metadata shapes.

# Scope In
- Generate helper methods and projection models for supported many-to-many and hierarchy bridge read shapes.
- Call existing bridge read APIs internally and preserve endpoint role semantics.

# Scope Out
No bridge maintenance, graph traversal platform, recursive query generator surface, or provider-specific SQL generation.

# Acceptance Criteria
- Generated bridge helpers compile, preserve runtime read semantics, and have snapshot/integration coverage.
- Unsupported bridge shapes emit deterministic diagnostics.