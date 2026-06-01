<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the PIT helper implementation story against the shipped satellite-only generator baseline and the done additive PIT-helper contract; no bounded planning writes were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence keeps the current implemented generator baseline support-bundle-driven and satellite-only (`docs/releases/v0.24.0.md`, analyzer README); this ticket is additive PIT-helper implementation, not a restatement of shipped behavior.
- Done ticket `06F7Y0GT7A5QT77TADMRZBVYN8` is the authoritative contract for PIT helper naming, support-bundle input, fingerprint gating, and runtime-boundary rules; bridge generation stays separate.
- The runtime PIT boundary is already bounded in `docs/architecture/dvault-v1-pit-bridge-boundary.md`: hub-parent ordinary PITs, hub-parent multi-active PITs with one canonical driving-key family, and bounded link-parent PITs with unique non-multi-active satellites on one declared link parent.
- Current analyzer tests already exercise PIT diagnostic-only behavior (`DMV1963`, `DMV1967`, `DMV1968`, `DMV1969`); this story should replace the supported PIT skip path with real helper emission while preserving deterministic unsupported-shape diagnostics.
- No child-ticket creation, relation mutation, attachment, planning-document write, or manual ticket-description update was applied in this refinement pass.

### Scope In
- Generate PIT read-model records and `Read{ProducedName}AsOfAsync(...)` extensions from one authoritative `dvault.support-bundle.v1` input carrying reviewed `readShape.pit` explain facts.
- Support only repository-proven PIT runtime shapes: hub-parent ordinary PITs, hub-parent PITs with one canonical multi-active driving-key family, and bounded link-parent PITs with unique non-multi-active satellites on one declared link parent.
- Emit deterministic PIT projection members and compatibility constants for `ParentHashKey`, `LoadTimestamp`, optional canonical driving keys, nullable snapshot-reference timestamps, produced column names, mapped names, metadata source kind, and metadata fingerprint.
- Construct `DataVaultPitAsOfReadRequest` values and delegate to the existing `IDataVaultReadService` PIT read path without widening runtime semantics.
- Add generator snapshot, public API/approval, and runtime parity coverage for supported helper emission and unsupported PIT diagnostics.

### Scope Out
- Bridge helper generation; that remains the sibling story `06F7Y0HJ1ZPY7ND9N8RVS92H4C`.
- Raw `dvault.model.v1` parsing, source-visible Code-First inspection, literal metadata-first inference, or fallback to unreviewed metadata sources.
- PIT maintenance/rebuild, read-time refresh, provider-specific SQL, payload joins, new runtime read primitives, or dynamic query compilation.
- Broader documentation or release-note sweep beyond minimal code-adjacent updates; the downstream docs task `06F7Y0HZKHBHMYX9EYDYFRYXZ0` remains the main documentation vehicle.

## Acceptance Criteria
- With `DVaultGenerateTypedReadModels=true` and exactly one authoritative support bundle carrying PIT read-shape explain facts, the generator emits `{ProducedName}ReadModel` and `Read{ProducedName}AsOfAsync(...)` in the existing generated namespace and extension naming pattern.
- The generated PIT helper constructs a `DataVaultPitAsOfReadRequest`, delegates to `IDataVaultReadService`, and returns `Task<IReadOnlyList<{ProducedName}ReadModel>>` without triggering PIT maintenance or adding provider-specific behavior.
- Supported PIT helper emission is limited to the repository-proven PIT runtime boundary: hub-parent ordinary PITs, shared-driving-key multi-active hub PITs, and bounded link-parent PITs with unique non-multi-active satellites on one link parent.
- Generated PIT read models project PIT-table columns only: required `ParentHashKey`, required `LoadTimestamp`, required canonical driving-key members when the supported shape includes them, nullable snapshot-reference timestamp members per included PIT segment, and the existing compatibility constants derived from authoritative produced or mapped names.
- Unsupported or insufficient PIT evidence remains deterministic and entity-specific: source or fingerprint failures stay `DMV1960` or `DMV1961`, unsupported PIT evidence stays `DMV1963`, dynamic-query or payload-join requirements stay `DMV1967`, model-first unsupported input stays `DMV1968`, and only intentionally deferred valid runtime PIT shapes may continue to use `DMV1969`.
- Tests cover generated-source snapshots, approval or public-surface updates, supported PIT helper execution against existing PIT read-service behavior, and preservation of unaffected satellite generation.

## Definition of Done
- Supported PIT helpers build in the repository and generated-source or public-surface approvals are updated for the new emitted types and extension methods.
- Analyzer and generator tests cover supported PIT emission plus bounded unsupported-shape diagnostics without regressing existing satellite or bridge behavior.
- Implementation preserves the authoritative support-bundle and fingerprint boundary and does not add raw-model parsing, provider-specific SQL, PIT maintenance, or broader runtime read semantics.
- The ticket is ready for downstream documentation work once PIT helper emission and coverage land; no additional PO scope decision is needed for bridge helpers or the broader docs task.

## Implementation Notes
- Use the additive contract in `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md` as the API and shape source of truth; current repository code still has a diagnostic-only PIT path in `DataVaultTypedReadModelSourceGenerator.cs` that reports `DMV1969` for otherwise valid PIT shapes.
- Drive helper emission from request-bound `readShape.pit` support-bundle facts, not raw runtime metadata alone; required evidence includes PIT parent identity, produced PIT table, parent hash-key column, `LoadTimestamp`, included segment snapshot-reference bindings, and any canonical driving-key columns.
- Reuse the existing typed satellite naming and constant pattern: `{ProducedName}ReadModel`, `{ProducedName}ReadExtensions`, `ProducedTableName`, `MetadataSourceKind`, `MetadataSourceFingerprint`, `{MemberName}ProducedColumnName`, and `{MemberName}MappedName`.
- Project PIT rows through existing exact-name PIT projection behavior and do not materialize satellite payload columns, hash diffs, record sources, or joined satellite rows.
- Extend the existing PIT generator tests around `DMV1963`, `DMV1967`, `DMV1968`, and `DMV1969` so supported PIT shapes now emit helpers while unsupported residual shapes keep deterministic skip or diagnostic behavior.
- The current `blocks` relation from this ticket to docs task `06F7Y0HZKHBHMYX9EYDYFRYXZ0` is still consistent with the intended implementation-before-docs sequence.

## Open Questions
- none

## Follow-Up Questions
- After implementation lands, should the historical `06F7Y0GT7A5QT77TADMRZBVYN8 -> 06F7Y0H83H29E1D9K5RK3K7Y9W` `blocks` relation be cleaned up by a relation-audit pass, or is that dependency history intentionally preserved?
- When the downstream docs task runs, should PIT helper examples focus only on supported shared-driving-key multi-active cases, or also include explicit rejected-shape examples?

## Risks
- If support-bundle export does not actually carry the required request-bound `readShape.pit` facts for parent identity, segment snapshot references, deterministic ordering, and column bindings, supported runtime PIT shapes will still collapse to diagnostics instead of helper emission.
- Shared-driving-key multi-active PIT support is only safe when the support bundle proves one canonical driving-key name or order family; mismatches must keep diagnostic-only behavior.
- Link-parent PIT helper emission must stay constrained to unique non-multi-active satellites on one declared link parent so the generator does not imply model-first link-parent PIT artifact support or broader runtime semantics.
- Live relation state still includes a historical `blocks` edge from done contract ticket `06F7Y0GT7A5QT77TADMRZBVYN8`; ticket metadata is currently `isBlocked: false`, but dependency-graph cleanup may still be needed later.

## Split Recommendations
- No further child split is justified from current evidence: PIT implementation is already separated from the bridge-helper story `06F7Y0HJ1ZPY7ND9N8RVS92H4C` and the downstream documentation task `06F7Y0HZKHBHMYX9EYDYFRYXZ0`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Generate typed PIT read helpers for supported reviewed metadata shapes.

# Scope In
- Generate helper methods and projection models for supported PIT read shapes.
- Call existing IDataVaultReadService PIT APIs internally.
- Respect fingerprint, diagnostics, nullability, redaction, and unsupported-shape behavior.

# Acceptance Criteria
- Generated PIT helpers compile, run against existing PIT read service behavior, and have snapshot coverage.
- Unsupported PIT shapes emit deterministic diagnostics.