<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story into a metadata-driven v1 generator contract aligned with current PIT/bridge, compiled-query, and dvault.model.v1 repository decisions; no new child tickets or relation writes are needed because the satellite and PIT/bridge implementation split already exists in 06F5Q92AHG0ZCTVQGC6NAYVP9C and 06F5Q92R02HB7FCE1AWKXPTMRW.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the v1 read boundary: generated helpers cover stable metadata-defined latest/current/as-of satellite reads plus explicit PIT as-of and bridge traversal reads, while dynamic runtime-built requests stay on IDataVaultReadService.
- PIT and bridge reads remain explicit read-model consumers of already-maintained rows; this ticket does not reopen PIT/bridge maintenance, background orchestration, or automatic SaveChanges refresh.
- Model-first input means dvault.model.v1 metadata projected into the EF or DVault metadata surface, code-first input means projected CLR or EF metadata including compiled-model usage, and metadata-first input means direct authoritative DVault metadata; the generator contract should normalize all three into one authoritative descriptor model.
- No additional ticket split or relation cleanup is needed now because the existing downstream stories already cover the justified implementation partition between latest or as-of satellite work and PIT or bridge work.

### Scope In
- Define the authoritative v1 contract for typed read-model source generation over stable DVault metadata-defined read shapes.
- Fix supported latest/current/as-of satellite, PIT as-of, and bridge traversal shapes, including the bounded PIT and bridge cases already documented in repository architecture notes.
- Define deterministic generated type and member naming, produced table and column name binding, nullability mapping, and projection semantics from authoritative metadata.
- Define analyzer and generator diagnostics for unsupported metadata, stale authoritative metadata fingerprints, and request shapes outside the bounded contract.
- Define how metadata-first, model-first, and code-first inputs participate through one authoritative metadata source boundary.

### Scope Out
- Runtime compilation of arbitrary dynamic IDataVaultReadService requests or user-authored query shapes.
- Provider-specific SQL, compiled-model tooling, migrations, or cross-provider performance guarantees beyond already-documented provider-neutral behavior and the existing SQLite-proven optimized PIT and bridge path.
- Automatic PIT or bridge maintenance, background refresh, save-pipeline changes, or orchestration that mutates read tables.
- Schema changes to dvault.model.v1 or new model-first artifact dialects.
- Hash canonicalization governance work except where existing produced names or metadata fingerprints must be consumed by the generator contract.

## Acceptance Criteria
- The contract enumerates the exact v1 supported generated read shapes: stable latest/current/as-of satellite projections, supported PIT as-of projections, and supported bridge traversal projections, with unsupported dynamic, provider-specific, or unbounded variants called out explicitly.
- The contract specifies deterministic naming and projection rules from authoritative DVault metadata, including how logical metadata names, produced entity and property names, endpoint roles, traversal depth, PIT segment columns, and CLR nullability flow into generated APIs.
- The contract states one execution boundary: generated artifacts compose over existing DVault metadata and documented read surfaces, use stable direct EF projection patterns only where the repository already documents them, and never promise provider-specific SQL generation.
- The contract defines diagnostics for unsupported PIT or bridge baselines, unsupported multi-active or participant shapes, stale metadata-source fingerprints, and generator inputs whose authoritative metadata source cannot be resolved deterministically.
- The contract defines how metadata-first, model-first dvault.model.v1, and code-first or compiled-model inputs are normalized so downstream generator implementation tickets can share one contract.

## Definition of Done
- A single authoritative planning or handoff surface captures the v1 generator contract, its non-goals, and the downstream consumer tickets that implement it.
- The contract references existing repository decisions for PIT and bridge boundaries, compiled-model compatibility, and dvault.model.v1 instead of reopening those decisions.
- Downstream implementation tickets can implement latest or as-of and PIT or bridge projector generation without reopening supported-shape, naming, or diagnostic scope questions.
- No blocking PO questions remain about supported input modes, read-shape families, or excluded runtime behaviors.

## Implementation Notes
- Use the existing annotation and metadata surface as the generator evidence baseline: ProducedName, MetadataName, EntityKind, PropertyRole, TechnicalColumnRole, ProviderLogicalPropertyKind, MetadataSourceKind, and MetadataSourceFingerprint are already defined in core code and should anchor generated naming and stale-input diagnostics.
- Normalize all input modes into one intermediate descriptor model after DVault metadata projection. Model-first should arrive via dvault.model.v1 projection, code-first should arrive via projected EF or DVault metadata including UseModel compiled-model usage, and metadata-first should arrive from the authoritative registry or metadata objects.
- Keep dynamic IDataVaultReadService request construction as the default non-generated runtime path. Generated code should only target stable shapes that can be proven from metadata and existing repository contracts.
- PIT and bridge generation must respect the explicit maintenance and read boundary: generated helpers consume already-maintained rows, preserve endpoint-role and depth semantics, and do not imply maintenance orchestration or registry-backed read shapes that the architecture note excludes.
- Treat this story as the contract parent for 06F5Q92AHG0ZCTVQGC6NAYVP9C and 06F5Q92R02HB7FCE1AWKXPTMRW; those children implement the contract rather than redefine it.

## Open Questions
- none

## Follow-Up Questions
- After the contract lands, confirm whether packaging and distribution of generator plus analyzer assets should stay in the existing analyzers surface or move into a dedicated package or story.
- When the child implementation tickets start, verify whether an additional test-vector ticket is needed for stale metadata fingerprint diagnostics across metadata-first, model-first, and compiled-model code-first flows.

## Risks
- If the contract blurs the boundary between generated stable helpers and dynamic IDataVaultReadService requests, downstream stories can drift into unsupported arbitrary query compilation.
- PIT and bridge support is bounded by the repository's existing architecture notes; over-promising link-parent, tuple-filter, provider-specific, or maintenance-coupled behavior would create delivery churn.
- Model-first and compiled-model inputs only stay safe if the contract ties generation to one authoritative metadata source and fingerprint; otherwise stale generated code and mismatched produced names become likely.

## Split Recommendations
- No additional split is needed now. Keep this story focused on the authoritative v1 contract and let 06F5Q92AHG0ZCTVQGC6NAYVP9C cover latest or as-of satellite projector implementation while 06F5Q92R02HB7FCE1AWKXPTMRW covers PIT or bridge projector implementation.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Define the source-generator contract for typed DVault read models.

Acceptance criteria:
- Specifies supported latest/as-of/PIT/bridge shapes, generated names, nullability, projections, and diagnostics.
- Excludes unstable dynamic request compilation and provider-specific SQL promises.
- Defines how metadata-first, model-first, and Code-First inputs participate.