<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the epic against the current repository baseline: the read-plan/ReadShape contract, support-bundle-driven PIT and bridge helper contract, PIT and bridge implementation, and the v0.25.0 documentation rollout are already decomposed into child tickets with done repository evidence, so this tracking epic is ready for PO-critic with no blocking PO questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already breaks this epic into done child tickets 06F7Y0FZXX5J0G7G15681HVEBR, 06F7Y0GT7A5QT77TADMRZBVYN8, 06F7Y0H83H29E1D9K5RK3K7Y9W, 06F7Y0HJ1ZPY7ND9N8RVS92H4C, and 06F7Y0HZKHBHMYX9EYDYFRYXZ0.
- Archived child 06F7Y0GFY7TP3V4B76JB759KB0 was closed as duplicate/already implemented under 06F7Y0FZXX5J0G7G15681HVEBR and does not leave residual epic scope.
- This ticket should stay a tracking parent only; child-level API shape and implementation details are already ratified in the architecture docs, generator tests, and child ticket contracts.
- The authoritative read-plan surface is IDataVaultReadDiagnosticsService.Analyze(...) returning DataVaultDiagnosticsResult with ReadStrategy plus additive ReadShape; the same bounded facts serialize under readShape in dvault.support-bundle.v1 when representative diagnostics are supplied.
- The authoritative generator boundary is support-bundle-driven: exactly one authoritative dvault.support-bundle.v1 input, optional DVaultTypedReadModelMetadataSourceFingerprint gating, and no raw dvault.model.v1 or source-visible declaration parsing inside the generator.
- The implemented helper and diagnostics vocabulary is bounded to satellite latest/current/as-of diagnostics, PIT Read...AsOfAsync helpers, and bridge Read...FromAsync/Read...ToAsync plus hierarchy Read...AncestorAsync/Read...DescendantAsync helpers with required maximumDepth.
- docs/releases/v0.25.0.md is the current coordinated public documentation baseline for this epic's contract.

### Scope In
- Bounded redacted ReadShape diagnostics for latest satellite, PIT as-of, and bridge reads, including provider strategy and fallback facts plus translated table and column facts.
- Support-bundle serialization of the same redacted readShape evidence for reviewed representative requests.
- Support-bundle-driven typed PIT helper generation for supported hub-parent, shared-driving-key multi-active hub-parent, and bounded link-parent PIT shapes.
- Support-bundle-driven typed bridge helper generation for supported many-to-many From/To and hierarchy Ancestor/Descendant traversal with required maximumDepth.
- Coordinated documentation and release-note rollout that describes the implemented read-plan and typed-helper surface consistently.

### Scope Out
- Any custom LINQ provider, alternate query planner, dashboard, or query orchestration platform.
- Raw SQL capture, provider query-plan export, physical-plan promises, automatic index advice, or secret-bearing diagnostics output.
- Raw dvault.model.v1 parsing, source-visible Code-First inspection, or literal metadata-first inference inside the typed-helper generator.
- Automatic PIT or bridge maintenance, read-time refresh, scheduling, SaveChanges orchestration, or widened runtime read semantics.
- Unbounded bridge traversal, dynamic runtime query compilation, or new runtime read primitives beyond the existing IDataVaultReadService boundary.
- Support-bundle transport automation, package-publication claims, or end-to-end sample-app expansion.

## Acceptance Criteria
- The epic's authoritative documentation treats IDataVaultReadDiagnosticsService.Analyze(...) and DataVaultDiagnosticsResult.ReadShape as the read-plan explain surface for LatestSatellite, PitAsOf, and Bridge, and support-bundle export serializes the same bounded facts under readShape.
- Redaction and omission rules remain explicit: translated metadata, enum and status values, selected strategy name when present, and expected index baselines are allowed; raw request keys, raw hash keys, as-of values, SQL text, provider query plans, credentials, connection strings, and exception or provider error text are excluded.
- With DVaultGenerateTypedReadModels=true and exactly one authoritative dvault.support-bundle.v1 input, the generator emits the implemented typed helper surface: satellite current/latest/as-of helpers, PIT Read{ProducedName}AsOfAsync helpers, and bridge Read{ProducedName}FromAsync/ToAsync or AncestorAsync/DescendantAsync helpers as appropriate.
- Generated PIT and bridge helpers stay ergonomic extensions over IDataVaultReadService that construct stable read requests and project generated rows without adding provider-specific SQL, maintenance, refresh orchestration, or new runtime read APIs.
- Unsupported or insufficient support-bundle evidence remains deterministic and bounded through DMV1960-DMV1969 diagnostics, skipping only the affected helper while preserving unrelated valid generation.
- Repository tests and docs remain aligned with the v0.25.0 baseline, including coverage for readShape serialization and redaction plus supported PIT and bridge helper generation.

## Definition of Done
- The diagnostics contract, typed-helper contract, implementation, and documentation rollout all land on one consistent repository baseline without reopening child-level API shape decisions.
- DataVault diagnostics code and tests prove request-bound ReadShape output for satellite, PIT, and bridge reads, including provider-selected and provider-neutral fallback cases and redaction of supplied request values.
- Typed read-model generator code and tests prove supported PIT and bridge helper emission, deterministic generated-source shape, and residual DMV196x behavior without regressing satellite helpers.
- README, analyzer README, architecture docs, production checklist, and docs/releases/v0.25.0.md all describe the same bounded v0.25.0 public surface.
- The epic has no remaining PO-scope blockers once the child contract, implementation, and documentation tickets are complete; historical duplicates and relation cleanup stay non-blocking follow-up only.

## Implementation Notes
- Use docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md as the diagnostics contract source of truth and docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md as the generator contract source of truth.
- Use DataVaultDiagnostics.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs as the authoritative baseline for ReadShape kinds, provider fallback facts, filter and projected column groups, and redaction behavior.
- Use src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs and tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs as the authoritative baseline for helper naming, request delegation, supported PIT and bridge shapes, and DMV196x outcomes.
- Treat docs/plans/typed-read-model-generator-contract.md as superseded historical context only; do not reopen the old satellite-only wording as the active boundary.
- Treat archived ticket 06F7Y0GFY7TP3V4B76JB759KB0 as absorbed by 06F7Y0FZXX5J0G7G15681HVEBR rather than as residual epic work.
- Do not reopen child-level method names, parameter labels, or helper class naming unless a new additive ticket intentionally changes the public contract.

## Open Questions
- none

## Follow-Up Questions
- Should a separate follow-up ticket add an end-to-end consumer sample that exports representative readShape diagnostics and compiles generated PIT and bridge helpers?
- Should superseded satellite-only planning documents receive a stronger banner or note so readers do not mistake them for the current v0.25.0 baseline?

## Risks
- Live relation state still shows historical blocks edges from done tickets 06F7Y0HZKHBHMYX9EYDYFRYXZ0 and 06F7Y0F650KM61BQXMEQPZ86DR to this epic; humans or automation could misread those as active blockers until relation cleanup happens.
- Superseded satellite-only planning context can still confuse readers if current-baseline links drift away from docs/releases/v0.25.0.md and the architecture contracts.
- ReadShape explanatory strings and expected index baselines could be over-interpreted as physical-plan guarantees if later docs or consumers blur the diagnostics-only boundary.

## Split Recommendations
- No new split is required; this epic is already decomposed into diagnostics contract, helper contract, PIT implementation, bridge implementation, and documentation rollout tickets.
- Keep any future raw-SQL or plan capture, automatic maintenance or orchestration, support-bundle transport automation, or sample-app work in separate additive tickets rather than reopening this epic.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Improve read-side ergonomics through redacted read-plan explanations and typed PIT/bridge helpers while staying inside EF Core read-service and generator boundaries.

# Scope In
- Bounded diagnostics for latest satellite, PIT, and bridge reads.
- Support-bundle driven typed PIT and bridge helper generation.

# Scope Out
No custom LINQ provider, raw SQL/query-plan capture, automatic PIT/bridge maintenance, dashboard, or query orchestration platform.