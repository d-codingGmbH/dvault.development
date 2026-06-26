<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Fresh repository inspection shows this is a bounded docs-alignment task: ratify the shipped effectivity baseline as generic link-parent satellite metadata, ratify the current support-bundle-driven typed read-helper baseline, and document deferred same-hub/dependent-child parity shapes explicitly.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Effectivity is already documented and implemented as caller-owned link-parent satellite state declared with `Link(...).Satellite<TSatellite>(...)`, `Payload(...)`, and optional `DrivingKey(...)`; this ticket does not introduce a first-class effectivity builder, metadata kind, or table family.
- Repeated same-hub links are already part of the implemented fluent metadata surface, but same-hub typed link-mapper/source-generator parity is still outside the current public claim set.
- Current typed read-helper coverage is the v1 support-bundle-driven generator contract for satellite latest/current/as-of, PIT as-of, and bounded bridge traversal helpers; raw `dvault.model.v1` files and source-visible declarations are not direct generator inputs.

### Scope In
- Review and align the current documentation that sets adopter expectations for repeated same-hub links, link-parent satellites/effectivity, typed read helpers, and typed mapper/source-generator boundaries.
- Make the docs explicitly separate implemented fluent/model metadata shapes from generated helper coverage and from deferred additive shapes.
- Correct or supersede any lingering satellite-only typed-helper wording by pointing to the current implemented support-bundle-driven satellite/PIT/bounded bridge helper contract.

### Scope Out
- Any new runtime, analyzer, or public API implementation for effectivity-specific builders, dependent child keys, same-hub typed mapper parity, or new helper shapes.
- PIT or bridge maintenance behavior changes, runtime read semantics changes, or provider behavior changes.
- Broad documentation cleanup outside the effectivity and typed-helper boundary touched by this ticket.

## Acceptance Criteria
- Affected docs state that repeated same-hub links and link-parent satellites are already implemented metadata shapes, including explicit same-hub participant roles and the generic effectivity-as-link-parent-satellite pattern.
- Affected docs distinguish the current typed read-helper contract from historical satellite-only planning by pointing readers to the implemented support-bundle-driven satellite/PIT/bounded bridge helper baseline.
- Affected docs explicitly state that same-hub typed mapper/source-generator parity, dependent child key modeling, and effectivity-specific fluent APIs remain deferred or out of scope rather than implying they already ship.
- Affected docs do not imply that raw `dvault.model.v1` artifacts or source-visible declarations directly generate typed helpers without authoritative support-bundle and request-bound `ReadShape` evidence.

## Definition of Done
- The current adopter-facing and architecture-facing docs that cover this boundary are reviewed and updated or confirmed aligned, including current guidance surfaces and any historical document that still needs an explicit supersession cue.
- No affected document leaves the false impression that effectivity has a dedicated first-class API or that typed helpers are still satellite-only.
- Any remaining unsupported shapes are labeled as deferred or non-goals with precise terminology.
- All added or updated cross-references point at existing checked-in documents.

## Implementation Notes
- Use `docs/model-first-governance.md` current limitations and `docs/production-adoption-checklist.md` as the high-level adopter-facing baseline to align.
- Use `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md` plus `src/DCoding.Data.DVault.Analyzers/README.md` for the current typed read-helper scope; keep `docs/plans/typed-read-model-generator-contract.md` historical only.
- Use `docs/architecture/dvault-v1-typed-row-mapper-contract.md` and `src/DCoding.Data.DVault/IDataVaultLinkMapper.cs` for the repeated same-hub typed mapper limitation.
- Use `docs/releases/v0.13.0.md`, `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs`, and `src/DCoding.Data.DVault/DataVaultCodeFirstSatelliteBuilder.cs` as repository evidence that same-hub roles, link-parent satellites, declaration-order payload/driving-key capture, and generic effectivity modeling are already shipped.
- Do not reopen helper-shape or fluent-naming design in this ticket; the repository already fixes the v1 baseline and this slice is about making the documentation boundaries explicit.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add runnable Code-First same-hub and effectivity examples once the boundary docs are aligned?
- Should future additive roadmap work separate dependent child key modeling from same-hub typed mapper/source-generator parity so each gap is tracked independently?

## Risks
- This boundary is described across current architecture docs, adoption guidance, model-first governance, release notes, and historical plans; partial edits could leave contradictory reader guidance.
- Readers may conflate typed read helpers with typed save mappers/source-generator parity unless the updated docs consistently use precise terminology and cross-links.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Review effectivity satellite and typed helper gaps after repeated same-hub/dependent-child decisions. Acceptance: docs distinguish implemented fluent metadata, generated helper coverage, and deferred shapes.