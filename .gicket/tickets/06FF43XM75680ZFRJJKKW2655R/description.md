<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Confirmed the persisted ticket body at revision 06FGPY452D6MTAS5T8CHTZGW5C was rewritten in this PO pass and now consistently presents the aggregate same-hub contract; the stale follow-up and risk text about a missing description rewrite is gone.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This PO pass already rewrote the ticket description, and the aggregate delivery contract now lives in the authoritative ticket body.
- Repeated same-hub v1 scope stays bounded to explicit relationship names and distinct produced participant names carried through modeling, generated mappers, and explicit-save persistence.
- Typed helper generation remains support-bundle-driven and does not parse raw dvault.model.v1 files or source-visible declarations directly.
- Dependent child key modeling remains deferred, and effectivity remains the existing link-parent satellite pattern.

### Scope In
- Ratify the finite v1 same-hub story boundary across support-bundle facts, generated typed link-mapper parity, and documentation or contract alignment.
- Require explicit relationship names and distinct role-bearing produced participant names for repeated same-hub links so metadata names, produced columns, and generated bindings stay deterministic.
- Keep same-hub generator parity provider-neutral and on the existing IDataVaultLinkMapper<TSource> plus IDataVaultSaveService explicit-save boundary.
- Carry forward the already-decided nearby boundaries for deferred dependent child modeling and effectivity-as-link-parent-satellite guidance.

### Scope Out
- Ambiguous repeated same-hub links that omit explicit roles or reuse the same produced participant name.
- New implicit persistence behavior, SaveChanges-driven write paths, provider-specific SQL generation, or a separate same-hub save contract.
- Raw dvault.model.v1 direct typed-helper generation, source-visible direct helper inference, or wider typed-helper parity beyond this same-hub story.
- New dependent child metadata concepts, effectivity-specific fluent APIs, or other broader modeling expansions.

## Acceptance Criteria
- Explicit same-hub links require an explicit relationship name and unique role-bearing participant names; no inferred relationship names are approved for this story.
- Generated same-hub link mappers preserve exact produced participant names in declaration order and work through the existing IDataVaultLinkMapper<TSource> and IDataVaultSaveService path with caller-supplied loadTimestamp and recordSource.
- Support-bundle or explain inputs preserve stable ordered participant facts sufficient to distinguish same-hub roles without provider-specific SQL or dynamic runtime inference.
- Ordinary distinct-hub behavior remains unchanged, and ambiguous or duplicate same-hub shapes fail deterministically through existing validation or diagnostic boundaries.
- Docs and contract text keep adjacent non-goals explicit: dependent child modeling stays deferred, effectivity stays link-parent satellite guidance, and raw model-first artifacts are not direct generator inputs.

## Definition of Done
- The parent story contract records the aggregate same-hub boundary and the child-slice scope that implements or documents it.
- Repository evidence still aligns across modeling and runtime, generator and analyzer, tests, and documentation baselines for explicit role-bearing same-hub links.
- The authoritative ticket description contains the aggregate contract, no blocking PO questions remain, and no further ticket split is required for this bounded story.
- No separate planning document, attachment, or relation rewrite is required for this pass.

## Implementation Notes
- Use src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs as the authoritative same-hub modeling baseline: explicit relationship name, distinct roles, and deterministic produced columns.
- Use src/DCoding.Data.DVault.Analyzers/DataVaultMappingSourceGenerator.cs, src/DCoding.Data.DVault/DataVaultLinkMappingAttribute.cs, src/DCoding.Data.DVault/DataVaultLinkParticipantBindingAttribute.cs, and src/DCoding.Data.DVault/IDataVaultLinkMapper.cs as the authoritative generated-mapper boundary: unique produced participant names, not distinct hub types.
- Use tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultMappingSourceGeneratorTests.cs and tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs as proof of successful role-bearing same-hub generation, deterministic duplicate rejection, and explicit-save-path persistence.
- Use docs/architecture/dvault-v1-typed-row-mapper-contract.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/model-first-governance.md, and docs/production-adoption-checklist.md as the current contract baseline for same-hub mapper scope, support-bundle input limits, and deferred adjacent gaps.
- This refinement pass rewrote the ticket description so the aggregate contract now lives in the authoritative ticket body; no separate planning document, attachment, or relation change was applied.

## Open Questions
- none

## Follow-Up Questions
- If product later wants model-first same-hub typed mapper generation or clearer public naming than ParticipantHubName, handle it as a separate additive compatibility ticket rather than widening this bounded v1 story.

## Risks
- Public names such as ParticipantHubName and ParticipantHubNames remain semantically awkward for same-hub role-bearing mappings, so incomplete documentation alignment could still make the supported pattern harder to discover.
- Historical duplicate-scope noise may still make some aggregate views harder to read even though the bounded v1 contract itself is now explicit in the ticket body.

## Split Recommendations
- No additional split recommended; the existing child-ticket breakdown already covers support-bundle facts, generated mapper parity, documentation alignment, and the nearby deferred-scope decisions.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the bounded generator and typed mapper scope for repeated same-hub role-bearing links. Acceptance: scope requires unambiguous roles, stable metadata, no inferred relationship names, and no provider-specific SQL generation.