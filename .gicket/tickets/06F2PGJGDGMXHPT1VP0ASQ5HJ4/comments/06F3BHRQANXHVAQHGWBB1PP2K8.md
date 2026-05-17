[gicket-bot] PO refinement contract

Summary
- Ratified ticket 06F2PGJGDGMXHPT1VP0ASQ5HJ4 as the v0.12 source-generated mapper-helper roll-up: the three-child split is already materialized, all three children are done, and current repository evidence matches the bounded contract, implementation, tests, and documentation surface. No child tickets, relation writes, attachments, or planning documents were created in this refinement run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The story now has three persisted child tickets linked by `parentOf`, and all three are `done`: 06F2PGJN1XCV8F7NWH567SQSKM for the generator contract, 06F2PGJSXP18VKKV52QZA4NP30 for generator implementation, and 06F2PGJYY6S97B4Z8044D34K5C for v0.12 documentation and release-note closure.
- Ticket event history shows the former `blocks` edge from this story to 06F2PGJYY6S97B4Z8044D34K5C was already removed and replaced with `parentOf` on 2026-05-17, so the delivery split is already materialized cleanly in the live relation graph.
- Repository evidence matches that split: `src/DCoding.Data.DVault.Analyzers/DataVaultMappingSourceGenerator.cs` implements the generator, `src/DCoding.Data.DVault/DataVaultHubMappingAttribute.cs` plus related binding attributes expose the compile-time declaration surface, `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultMappingSourceGeneratorTests.cs` covers DMV1950-DMV1955, and `docs/releases/v0.12.0.md` plus `README.md` record the public baseline.
- Generated helpers stay on the existing explicit save boundary. They implement `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, or `IDataVaultSatelliteMapper<TSource>` and construct `DataVaultRegistry*SaveOperation` values; callers still own `loadTimestamp`, `recordSource`, `DbContext`, and `IDataVaultSaveService` orchestration.
- The bounded v1 generated-shape baseline is hubs, links with unique participant hub names, ordinary hub-parent satellites, and hub-parent multi-active satellites. Link-parent satellites and repeated-participant or self-link generated mappings are not part of this story.
- Local ticket comments for this story are automation claim/lease comments only; no human clarification needs to be incorporated.
- Incoming `blocks` relations from done story 06F2PGJBRXFCP038CN6XVAYSZM and done epic 06F2PGFT8Z406HFBJGQSY7YRJ0 are historical satisfied dependencies. No relation cleanup was materialized in this pass.

Scope In
- Story-level roll-up of the already materialized v0.12 generator contract, implementation, and documentation work for source-generated DVault mapper helpers.
- Public compile-time mapping declaration attributes in `DCoding.Data.DVault` for hub, link, and hub-parent satellite mapping inputs.
- Source-generator output in `DCoding.Data.DVault.Analyzers` that emits deterministic registry-backed mapper helpers and compile-time diagnostics for malformed mapping declarations.
- Tests and public documentation updates required to make the bounded generated-helper slice adoption-ready within the current release.

Scope Out
- Any new metadata authority, runtime-discovered registration system, hidden save orchestration, automatic `loadTimestamp` or `recordSource` handling, or automatic hash-key/hash-diff derivation.
- Generated support for link-parent satellites, repeated-participant links, self-links, or other excluded runtime shapes beyond the ratified v1 baseline.
- A new generator package family, broader provider/runtime refactors, or unrelated analyzer and code-fix backlog beyond the bounded generator-release story.
- A runnable generator-based example application or deeper post-release documentation expansion beyond the current v0.12 closure.

Open questions
- none

Follow-up questions
- Should a later follow-on ticket add generated support for link-parent satellites on the same explicit save boundary?
- Should repeated-participant or self-link mappings get a separate generator follow-on with explicit participant-alias semantics instead of widening the v1 slice?
- Should a later docs/examples ticket add a runnable end-to-end consumer sample that uses the generated mapping attributes and generated mappers?

Risks
- If future work presents generated helpers as a fourth metadata authority or a hidden persistence layer, the story boundary will sprawl beyond the ratified explicit-save model.
- If future docs duplicate analyzer-package suppression and capability details outside `src/DCoding.Data.DVault.Analyzers/README.md`, public guidance can drift.
- If later generator expansion reaches excluded link or satellite shapes without a separate ticket, runtime-boundary assumptions around unique participant names and supported parents can be broken.

Split recommendations
- No additional split is recommended. The existing child-ticket separation across contract, implementation, and documentation is already sufficient.
- If the team wants generated support for link-parent satellites or repeated-participant/self-link mappings, create separate follow-on tickets rather than widening this story.
- If the team wants richer adoption material, create a separate documentation or examples ticket for runnable generator-based samples or capability tables.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment