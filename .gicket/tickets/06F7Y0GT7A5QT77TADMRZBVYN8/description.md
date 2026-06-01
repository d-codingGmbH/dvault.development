<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to a bounded additive contract for support-bundle explain-driven typed PIT and bridge helpers over the existing `IDataVaultReadService` boundary; no bounded planning writes were materialized in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence keeps v0.22.0 as satellite-only typed helper shipment; this story is additive and must not restate PIT or bridge helpers as already shipped.
- Typed PIT and bridge helpers consume exactly one authoritative `dvault.support-bundle.v1` input carrying reviewed PIT/bridge explain metadata plus the existing optional `DVaultTypedReadModelMetadataSourceFingerprint`; no raw `dvault.model.v1`, source-visible Code-First callbacks, or literal metadata-first declarations are generator inputs.
- Helpers stay ergonomic extensions over existing provider-neutral PIT and bridge reads and do not generate provider-specific SQL, perform PIT/bridge maintenance, schedule refresh, or widen runtime read semantics.
- No ticket description update, relation change, child-ticket creation, attachment, or planning-document write was applied during this refinement pass.

### Scope In
- Define generated PIT and bridge helper naming from the existing typed satellite pattern: `{ProducedName}ReadModel` plus `Read{ProducedName}...Async` extension methods over `IDataVaultReadService`.
- Support PIT helpers only for repository-proven runtime PIT shapes: hub-parent ordinary PITs, hub-parent PITs whose referenced multi-active satellites share one canonical driving-key family, and bounded link-parent PITs with unique non-multi-active satellites on the same declared link parent.
- Support bridge helpers only for repository-proven runtime bridge shapes: many-to-many endpoint traversal and hierarchy traversal with required bounded `maximumDepth`.
- Define generated projection members, nullability, and public constants for produced or mapped column names, metadata source kind, and metadata fingerprint so PIT and bridge helpers follow the existing typed satellite compatibility pattern.
- Define explicit unsupported-shape diagnostics and helper-skip behavior for PIT and bridge shapes that remain outside the bounded contract.

### Scope Out
- Any raw model parsing, source inspection, or fallback inference outside reviewed support-bundle metadata.
- Provider-specific SQL generation, provider physical-plan promises, custom LINQ or query providers, or dynamic request compilation.
- Automatic PIT or bridge maintenance, read-time refresh, scheduling, or `SaveChanges` orchestration.
- Graph or path APIs beyond the existing many-to-many and hierarchy bridge traversal boundary.
- Unbounded PIT tuple filters, cross-product multi-active semantics, delete-aware bridge maintenance behavior, or new runtime read primitives.

## Acceptance Criteria
- The contract explicitly states that the baseline before this story is satellite-only typed helper generation and that PIT and bridge helper support is additive.
- Supported PIT helper shapes and unsupported PIT residual shapes are enumerated against the existing runtime PIT boundary, including the multi-active driving-key-family restriction and bounded link-parent allowance.
- Supported bridge helper shapes and unsupported bridge residual shapes are enumerated against the existing runtime bridge boundary, including hierarchy `maximumDepth` requirements and the closed many-to-many or hierarchy endpoint vocabularies.
- Generated API shape is fixed: PIT helpers emit `Read{ProducedName}AsOfAsync`; bridge helpers emit direction-specific traversal methods aligned to the closed endpoint vocabulary; all helpers delegate to `IDataVaultReadService` rather than widening runtime behavior.
- Generated read-model projection rules are fixed, including required technical members, nullable PIT snapshot-reference members, hierarchy `TraversalDepth`, public produced or mapped-name constants, and metadata fingerprint or source constants.
- Unsupported or insufficient support-bundle evidence produces explicit diagnostics and skips only the affected helper while preserving unrelated helper generation.

## Definition of Done
- Authoritative contract text is available for developers and PO critic review, covering naming, method surfaces, supported shapes, unsupported-shape diagnostics, projection and nullability rules, and fingerprint behavior.
- The contract ties PIT and bridge helper generation to one authoritative `dvault.support-bundle.v1` input and the existing fingerprint gate, with no raw-model fallback.
- The contract preserves the existing provider-neutral runtime boundary: helpers read maintained PIT or bridge tables only and do not add maintenance or provider-specific execution obligations.
- The contract is specific enough that implementation can add generator and approval-test coverage without reopening public API shape decisions.

## Implementation Notes
- Use the existing generated-type naming baseline from typed satellite helpers: keep the produced entity name in the generated record and extension method names, for example `PitCustomerTimelineReadModel`, `ReadPitCustomerTimelineAsOfAsync`, `BridgeCustomerOrderReadModel`, and direction-specific bridge traversal helpers.
- Generated PIT read models should map PIT table columns only: required parent hash key, required PIT `LoadTimestamp`, optional canonical driving-key members when the runtime shape supports them, and nullable snapshot-reference timestamp members per included PIT segment; they must not materialize satellite payload joins.
- Generated bridge read models should map provider-neutral bridge row columns only: non-null endpoint hash-key members in generated column order and required `TraversalDepth` for hierarchy bridges.
- Carry forward the typed satellite compatibility pattern by emitting public constants for produced table name, per-member produced column name, per-member mapped name, `MetadataSourceKind`, and `MetadataSourceFingerprint`.
- Keep missing support bundle and stale fingerprint behavior aligned with the existing generator boundary; PIT or bridge-specific unsupported shapes should surface per-entity diagnostics and skip only the affected helper so satellite generation and other valid helpers still continue.
- Residual unsupported cases should include any shape that would require runtime tuple expansion, extra read-time joins, raw support-bundle gaps, unsupported CLR or redaction gaps, or provider-specific traversal semantics.

## Open Questions
- none

## Follow-Up Questions
- When implementation is scheduled, should PIT and bridge helper emission ship in one release or stage PIT first and bridge second?
- After implementation lands, should public examples live only in analyzer README and tests or also in release notes and adoption guidance?

## Risks
- Live relation state still shows incoming blocker tickets `06F7Y0F650KM61BQXMEQPZ86DR` and `06F7Y0FZXX5J0G7G15681HVEBR`; implementation should verify whether those blockers remain semantically active or need relation cleanup before development starts.
- PIT helper support is only safe when the support-bundle export path can carry the bounded PIT read-shape facts needed for parent identity, canonical driving-key families, and segment snapshot references without reintroducing raw-model parsing.
- Bridge helper ergonomics must not drift into broader graph traversal or provider-specific behavior; the contract needs to stay constrained to the existing many-to-many and hierarchy read-service semantics.

## Split Recommendations
- No bounded child-ticket split was materialized in this pass; keep the contract-definition story unified and split later engineering delivery into PIT and bridge implementation tickets only if development capacity or test volume warrants it.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Define a narrow generator contract for typed PIT and bridge helpers based on reviewed support-bundle metadata.

# Scope In
- Decide generated type names, method names, projection shapes, nullability, fingerprint behavior, diagnostics, and unsupported-shape reporting.
- Reuse existing runtime read services; helpers must not generate provider-specific SQL or perform maintenance.

# Scope Out
No raw model parsing beyond reviewed support-bundle explain metadata, no automatic dynamic request compilation, and no custom LINQ provider.

# Acceptance Criteria
- Supported and unsupported PIT/bridge shapes are explicit.
- Public API and generated output compatibility expectations are clear before implementation.