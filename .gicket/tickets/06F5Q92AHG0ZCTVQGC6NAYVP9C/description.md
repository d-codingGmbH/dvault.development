<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the satellite-only typed read-model generator story against the existing generator contract and current read-service/analyzer baselines; no split or relation changes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket is the satellite-only implementation slice of `docs/plans/typed-read-model-generator-contract.md`; PIT and bridge generation stay in sibling ticket `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Repository evidence already fixes the runtime baseline: `IDataVaultReadService`, `DataVaultLatestSatelliteReadRequest`, `DataVaultReadServiceCurrentSatelliteExtensions`, and `DataVaultSatelliteProjectionRow` define current/latest/as-of satellite semantics, so this story generates compile-time wrappers over that surface instead of adding a new read engine.
- Repository evidence also fixes the generator host: `DCoding.Data.DVault.Analyzers` already carries `DataVaultMappingSourceGenerator`, so the typed satellite generator belongs in that analyzer/source-generator package and should use the satellite-relevant portion of the reserved `DMV1960`-`DMV1969` range.
- No child tickets, relation edits, description edits, attachments, or planning documents were materialized because the existing contract document already bounds this story sufficiently for PO handoff.

### Scope In
- Normalize one authoritative metadata source for satellite generation from the supported metadata-first, model-first, or code-first inputs, preserving source kind/fingerprint, produced names, metadata names, parent reference data, property roles, provider logical/value metadata, ordinals, CLR types, and nullability needed by generated helpers.
- Generate satellite-specific `{SatelliteProducedName}ReadModel` and `{SatelliteProducedName}ReadExtensions` types in `{RootNamespace}.DVault.GeneratedReadModels` with `Read...CurrentAsync`, `Read...LatestAsync`, and `Read...AsOfAsync` helpers over `IDataVaultReadService`.
- Support hub-parent, link-parent, and deterministic multi-active satellites within the documented v1 provider-neutral string payload/driving-key boundary.
- Emit bounded diagnostics for unresolved authoritative metadata, stale fingerprints, unsupported satellite shapes, deterministic name collisions, dynamic/provider-specific query requirements, and conservative payload-nullability fallback.

### Scope Out
- Generated PIT or bridge helpers, PIT/bridge descriptor validation, and PIT/bridge diagnostics; those remain in `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Any new runtime read engine, arbitrary caller-defined request compilation, provider-specific SQL generation, or automatic PIT/bridge maintenance.
- Runtime-selected payload subsets, caller-supplied filter predicates, ordering customization, unbounded traversal, or non-string payload/driving-key type support beyond the documented v1 boundary.
- Changes to `dvault.model.v1` governance beyond consuming already-supported model-first inputs for satellite helper generation.

## Acceptance Criteria
- For each supported satellite metadata declaration, the consuming compilation receives generated `ReadModel` and `ReadExtensions` source under the documented namespace and naming rules, with `Current`, `Latest`, and `AsOf` methods bound to that satellite.
- Generated satellite row types preserve exact produced table/column bindings and expose the parent hash key, driving keys in metadata order, `HashDiff`, `LoadTimestamp`, `RecordSource`, and payload properties with nullability derived from authoritative CLR/EF metadata.
- Generated methods use the existing latest-satellite read contract through `IDataVaultReadService` and `DataVaultLatestSatelliteReadRequest`, or an equivalent stable direct EF projection explicitly allowed by the contract, without introducing provider-specific SQL or caller-owned projector delegates.
- When authoritative metadata cannot be resolved deterministically, fingerprints drift, bindings or normalized public names collide, or the requested shape falls outside the bounded satellite contract, generation stops or skips with the documented `DMV196x` diagnostics instead of emitting unstable helpers.
- Repository tests cover positive generation for representative hub-parent, link-parent, and multi-active satellite shapes plus negative diagnostics for stale fingerprints, unsupported bindings, nullability fallback, and naming-collision edge cases.

## Definition of Done
- Analyzer-package implementation and tests land in the existing `DCoding.Data.DVault.Analyzers` and `tests/DCoding.Data.DVault.Tests/Analyzers` generator harness, and generated helpers compile against the current `DCoding.Data.DVault` runtime APIs without introducing a new public runtime query surface.
- Generated satellite helpers behave consistently with the current/latest/as-of satellite semantics already exposed by `DataVaultReadServiceCurrentSatelliteExtensions` and `DataVaultSatelliteProjectionRow`.
- Developer-facing analyzer/generator documentation is updated enough to explain the typed satellite read-model generator boundary, supported inputs, and `DMV196x` failure cases.
- Regression coverage protects deterministic naming, metadata-source and fingerprint handling, payload nullability, multi-active driving-key ordering, and unsupported-shape diagnostics.

## Implementation Notes
- Reuse the existing analyzer/source-generator package boundary: the new generator should sit beside `DataVaultMappingSourceGenerator` and share the current Roslyn test infrastructure rather than introducing a separate package.
- The runtime side already exposes the necessary anchor types: `DataVaultAnnotationNames` for authoritative metadata, `DataVaultLatestSatelliteReadRequest` for current/latest/as-of request semantics, `DataVaultReadServiceCurrentSatelliteExtensions` for convenience baselines, and `DataVaultSatelliteProjectionRow` for exact-name projection behavior.
- Generated public names are contract-fixed: normalize produced names into PascalCase, prefix keyword or digit-leading identifiers with `Dvault`, append numeric suffixes only per the documented collision rules, and raise a diagnostic when the public API shape would otherwise become ambiguous or unstable.
- Payload nullability should follow authoritative CLR/EF metadata; when nullability cannot be proven, emit a nullable payload property and the informational diagnostic rather than guessing a required property.
- Keep technical columns non-null and string or UTC-typed as already enforced by the runtime projection surface; non-string payload or driving-key CLR mappings remain outside this ticket's supported baseline.

## Open Questions
- none

## Follow-Up Questions
- After the satellite slice lands, decide whether public README and release-note examples should show generated satellite read models immediately or wait until sibling PIT/bridge generation ticket `06F5Q92R02HB7FCE1AWKXPTMRW` lands so the typed-read story can be presented as one coordinated feature.
- A later contract pass can decide whether v2 should widen generated payload and driving-key CLR type support beyond the current provider-neutral string boundary.
- Once both child tickets land, confirm whether the analyzer package README should present the full `DMV1960`-`DMV1969` catalog in one consolidated typed-read generator section.

## Risks
- The contract intentionally bounds generated satellite properties to shapes that can be proven from authoritative metadata; consumers with provider-specific or non-string payload mappings will receive diagnostics or skipped generation rather than helpers.
- Metadata-source ambiguity or source-fingerprint drift will hard-fail generation by design, so teams using multiple declaration paths must keep one authoritative source visible per generated scope.
- If any supported shape uses the optional stable direct EF projection path, implementation must stay inside the repository's compiled-model and compiled-query compatibility boundary to avoid subtle provider regressions.

## Split Recommendations
- No further split is recommended: the repository already isolates PIT and bridge generation into `06F5Q92R02HB7FCE1AWKXPTMRW`, and the remaining satellite generator and analyzer slice is bounded enough for one implementation story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Generate typed projectors for stable latest/current/as-of satellite read shapes.

Acceptance criteria:
- Emits compile-time DTO/projector helpers for supported satellite metadata.
- Uses existing read services or stable direct EF projections according to the generator contract.
- Adds analyzer diagnostics for unsupported shapes and stale generated metadata.