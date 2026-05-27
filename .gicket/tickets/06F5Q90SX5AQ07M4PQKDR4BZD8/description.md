<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- The current delivery contract already resolves the PO-critic ambiguity: link-parent PIT support is runtime-path-only, public model-first `dvault.model.v1` PIT declarations/import-export/diagnostics remain out of scope, and the documentation boundary is explicit. No additional child-ticket, relation, attachment, planning-document, or description writes were required in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already supports link-parent satellite modeling and `DataVaultMetadataRegistry` accepts `DataVaultPitMetadata` whose parent is a `Link`, so this story extends an existing runtime path rather than introducing a new PIT metadata type or declaration path.
- Preserve the existing PIT row/read contract names `ParentHashKey`, `LoadTimestamp`, and ordered `<Satellite>LoadTimestamp` snapshot columns; for link-parent PITs `ParentHashKey` carries the link hash key.
- Link-parent PIT support is bounded to one declared link parent plus ordered unique non-multi-active satellites attached to that same link; hub-attached, mixed-parent, bridge-driven, and multi-active PIT shapes remain unsupported here.
- PIT reads stay on the explicit-metadata `DataVaultPitAsOfReadRequest` / `ReadPitRowsAsync(...)` / `ReadPitAsync(...)` boundary; this story does not add registry-backed PIT read requests.
- Model-first `dvault.model.v1` PIT artifacts remain hub-parent-only for this ticket: the public JSON shape, import/export, and drift/diagnostic surfaces do not gain link-parent PIT support here.
- The live incoming `blocks` relation from done ticket `06F5Q90KC6JGQPSP285XQYSPK8` is historical sequencing context rather than an active scope blocker; no relation cleanup was applied in this pass.
- No child-ticket, relation, attachment, planning-document, or further description writes were applied in this pass because the current delivery contract already reflects the required model-first scope decision.

### Scope In
- Extend PIT EF metadata translation to project one link-parent `DataVaultPitMetadata` that already reaches the existing EF/registry runtime path, with deterministic PIT table metadata and snapshot columns for attached link-parent satellites.
- Extend explicit and registry-backed PIT maintenance so rebuild and targeted parent maintenance accept the supported link-parent PIT shape and recompute history for explicit link hash keys.
- Extend provider-neutral PIT-backed reads and required diagnostics so explicit `DataVaultPitAsOfReadRequest` callers can read maintained link-parent PIT rows without changing hub-parent PIT behavior or projection semantics.
- Add unit, SQLite integration, public contract snapshot, and documentation coverage for the supported link-parent runtime baseline on the existing `DataVaultPitMetadata` / registry-backed path.

### Scope Out
- Multi-active PIT semantics, driving-key PIT row generation, or link-parent PITs that reference multi-active satellites.
- PITs that mix hub-parent and link-parent satellites, traverse bridges, or introduce a new PIT metadata/declaration surface.
- Model-first `dvault.model.v1` PIT declaration changes, including JSON import/export, drift/diagnostic, or other artifact-contract updates required to express link-parent PIT parents or link-parent satellite membership.
- Registry-backed PIT read request surfaces, automatic PIT refresh, background scheduling, `SaveChanges` hooks, or PIT/bridge orchestration.
- Provider-specific link-parent PIT read optimization, physical tuning promises, or broader benchmark/evidence work already deferred to downstream diagnostics tickets.

## Acceptance Criteria
- `ApplyDataVaultMetadata()` and the underlying PIT translator accept `DataVaultPitMetadata` whose parent is a declared link and whose referenced satellites are unique, non-multi-active, and attached to that same link, while preserving the existing `ParentHashKey` / `LoadTimestamp` / ordered snapshot-column contract.
- `IDataVaultPitMaintenanceService` rebuild and parent-maintenance paths, plus existing registry-backed maintenance adapters, accept the supported link-parent PIT shape and recompute complete PIT history for explicit link hash keys with the same empty-input no-op, targeted replacement, late-arriving correction, and deterministic pre-write validation semantics as hub-parent PIT maintenance.
- `IDataVaultReadService.ReadPitRowsAsync(...)` and `DataVaultReadServicePitExtensions.ReadPitAsync(...)` accept the supported link-parent PIT shape and return rows keyed by the link hash key through the existing raw-record and projection-row APIs without adding latest/as-of fallback reads for missing PIT snapshots.
- Deterministic diagnostics reject unsupported cases such as missing or duplicate satellites, satellites attached to a different parent, multi-active references, bridge-driven or legacy PIT shapes, and generated-model mismatches; provider-specific read strategies may still decline link-parent PITs and fall back to the provider-neutral pipeline.
- Unit tests, SQLite integration tests, and public contract/documentation updates prove the new link-parent runtime baseline while keeping existing hub-parent PIT behavior unchanged, and the updated docs explicitly state that `dvault.model.v1` PIT artifacts remain hub-parent-only for this ticket.

## Definition of Done
- README, production-adoption guidance, deferred-capability/planning text, and active release-note language that currently says link-parent PITs are unsupported are updated to the new bounded support statement, and that statement explicitly says the change applies only to the existing `DataVaultPitMetadata` / registry-backed runtime path while model-first `dvault.model.v1` PIT declarations/import-export/diagnostics remain out of scope for this ticket.
- Regression coverage proves existing hub-parent PIT translation, maintenance, diagnostics, and read behaviors remain unchanged.
- The resulting contract leaves downstream ticket `06F5Q91DR1555RSBQT7KDST684` focused on broader diagnostics/benchmark evidence rather than core link-parent PIT enablement.

## Implementation Notes
- Reuse the existing `DataVaultPitMetadata` + `DataVaultMetadataReference.Link(...)` model path already accepted by `DataVaultMetadataRegistry`; do not add a new PIT metadata type or alternate naming scheme.
- Relax hub-only guards in `DataVaultEfMetadataTranslator`, `DataVaultPitMaintenanceShapeValidator`, and `DataVaultPitReadPipeline`, then validate that each PIT satellite resolves to the same declared link parent and remains non-multi-active.
- Keep the existing generic parent-oriented contracts (`ParentHashKey`, parent reference annotations, `DataVaultPitReadRecord`, typed projection helpers, registry-backed maintenance adapters) rather than adding link-specific public API names.
- Keep the current model-first PIT artifact contract unchanged in parser/export/diagnostic surfaces for this story; any future link-parent PIT JSON contract needs separate planning instead of piggybacking on this runtime story.
- Preserve the current provider-neutral fallback model: correctness lands in translation, maintenance, provider-neutral reads, and diagnostics first; provider-specific link-parent PIT strategy acceptance is follow-up work.
- Convert existing negative tests for `link-based PIT tables` into supported-shape coverage for the bounded link-parent baseline, while keeping negative coverage for multi-active and mismatched-parent cases.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket extend SQLite or other provider-specific PIT read strategies to accept the new link-parent baseline instead of declining to provider-neutral fallback?
- After link-parent PIT support lands, should PIT reads remain explicit-metadata-only, or is a separate registry-backed PIT as-of read request worth planning?
- Should a separate future ticket extend `dvault.model.v1` PIT declarations/import/export/drift diagnostics from the current hub-parent artifact contract to link-parent PIT artifacts?

## Risks
- README, production-adoption guidance, deferred-capabilities planning text, and existing release notes currently describe link-parent PITs as unsupported; partial doc updates would create public contract drift.
- Because this story intentionally broadens the runtime `DataVaultPitMetadata` path without broadening the current model-first PIT artifact contract, incomplete docs could imply `dvault.model.v1` link-parent PIT support that import/export/diagnostics still do not provide.
- The current codebase has separate hub-only guards in PIT translation, maintenance validation, read validation, and strategy diagnostics, so updating only one path would leave inconsistent behavior or regress hub-parent compatibility.
- Downstream diagnostics/benchmark work already depends on this story, so incomplete link-parent validation or missing regression coverage would delay later PIT evidence tickets.

## Split Recommendations
- No additional split is required for the runtime story. If product direction later requires model-first link-parent PIT artifacts, plan that as a separate additive ticket across `dvault.model.v1` JSON, import/export, and drift/diagnostic surfaces.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Add PIT support where the PIT parent is a link rather than a hub.

Acceptance criteria:
- Projects deterministic link-parent PIT columns and validates referenced link-parent satellites.
- Supports rebuild and targeted parent maintenance using link hash keys.
- Extends read diagnostics and read APIs without breaking hub-parent PIT behavior.