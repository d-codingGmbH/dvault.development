<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the v0.12.0 documentation ticket into a bounded release-closure doc sweep: create `docs/releases/v0.12.0.md`, align public versioned guidance to `0.12.0`, and update root/public docs for the shipped analyzer code-fix and source-generator surface; no child tickets, relation writes, attachments, or planning documents were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows `docs/releases` stops at `v0.11.0`, while release `06F2PH99NN9B0S4RZW0NPST1CR` is active as `v0.12.0 - Analyzer and Generator Ergonomics`; this ticket owns the missing coordinated `docs/releases/v0.12.0.md` closure.
- Done sibling tickets already establish the feature baseline this ticket must document: `06F2PGHWEWYJZSRQ9QPT4NJ0QM` and `06F2PGJBRXFCP038CN6XVAYSZM` cover analyzer diagnostics and bounded code fixes, `06F2PGJ28KVSZAAFRA40D94128` covers package-local analyzer configuration and suppression guidance, and `06F2PGJN1XCV8F7NWH567SQSKM` plus `06F2PGJSXP18VKKV52QZA4NP30` define and implement the source-generator contract.
- Current repository guidance is inconsistent with shipped behavior: root `README.md`, `examples/README.md`, `docs/model-first-governance.md`, and `src/DCoding.Data.DVault.Analyzers/README.md` still contain `0.11.0` baseline references, and root `README.md` still describes the analyzer package as only the earlier high-confidence DMV1901/DMV1902 slice.
- The analyzer package README is already the authoritative deep guide for installation and suppression details; broader docs should point to it and update scope statements, but should not duplicate a second conflicting suppression contract.
- The v0.12 public docs should explicitly state that generated mapper helpers stay on the existing `DataVaultRegistry*SaveOperation` and `IDataVault*Mapper<TSource>` boundary and do not hide caller-owned `loadTimestamp`, `recordSource`, or `IDataVaultSaveService` orchestration.
- Incoming `blocks` relations from done tickets `06F2PGFT8Z406HFBJGQSY7YRJ0`, `06F2PGHQ2GATEM13M5QK1MSX1G`, and `06F2PGJBRXFCP038CN6XVAYSZM` are satisfied historical context rather than PO blockers; incoming `parentOf` relations from epic `06F2PGHJAFMH80TZAMANQWH9PW` and story `06F2PGJGDGMXHPT1VP0ASQ5HJ4` keep this ticket inside the active analyzer/generator release graph.
- No child tickets, relation writes, attachments, or planning documents were materialized during this refinement pass.

### Scope In
- Create `docs/releases/v0.12.0.md` as the coordinated public release record for `v0.12.0 - Analyzer and Generator Ergonomics`, following the established release-note structure used by `docs/releases/v0.10.0.md` and `docs/releases/v0.11.0.md`.
- Update root `README.md` so installation snippets, current-release references, notable-change summary, and limitations match the shipped analyzer code-fix and source-generator surface.
- Version-align and minimally refresh other public docs that currently present outdated `0.11.0` or Code-First-only analyzer guidance: `examples/README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md` where their current-baseline text points at `v0.11.0`.
- Document the consumer-facing generator boundary: compile-time mapping declarations live in `DCoding.Data.DVault`, generation happens via the optional analyzer package, and generated helpers feed the existing explicit save flow rather than introducing a new metadata authority or hidden persistence API.
- Carry the existing DMV1901/DMV1902 analyzer baseline forward into the current public package description while clearly separating that carried-forward baseline from new v0.12 additions such as bounded code fixes, DMV1950-DMV1955 mapping diagnostics, and generated mapper helpers.

### Scope Out
- Any new analyzer diagnostics, code fixes, source-generator features, runtime API changes, or package-shape changes; this ticket documents shipped behavior only.
- A new runnable quickstart, sample application, or end-to-end example project for generated mapper adoption.
- A second detailed suppression or analyzer-configuration guide outside `src/DCoding.Data.DVault.Analyzers/README.md`.
- Manual-publication workflow changes or package-verification rule changes unless a touched consumer doc would otherwise stay factually inconsistent.
- Relation cleanup, child-ticket creation, or broader planning-graph reshaping beyond this ticket's bounded documentation closure.

## Acceptance Criteria
- `docs/releases/v0.12.0.md` exists and records the seven-package coordinated release, aligned `0.12.0` version, analyzer/generator highlights, compatibility notes, known limitations, documentation updates, and validation-evidence pointers consistent with repository state.
- The v0.12 release notes accurately distinguish carried-forward analyzer baseline behavior from new v0.12 additions: DMV1901/DMV1902 remain part of the current package surface, while bounded code fixes, DMV1950-DMV1955 mapping diagnostics, and source-generated mapper helpers are called out as the new ergonomics layer.
- Root `README.md` no longer presents `v0.11.0` as the current public baseline and no longer describes the analyzer package as only the earlier Code-First selector slice; it documents the current analyzer/generator surface at a high level and points to the packaged analyzer README for detailed suppression guidance.
- Public installation guidance consistently states that `DCoding.Data.DVault.Analyzers` is optional developer tooling for projects that own DVault Code-First declarations or compile-time mapping declarations, and versioned package examples touched by this ticket are aligned to `0.12.0`.
- Broader docs touched by this ticket explain that generated helpers still use the existing `DataVaultRegistry*SaveOperation` and explicit `IDataVaultSaveService` boundary, with caller-owned `loadTimestamp` and `recordSource`, and do not imply a fourth metadata authority or hidden persistence path.
- Any touched supporting docs stay minimal and non-conflicting: quickstart and adoption docs may acknowledge the analyzer/generator package and current release baseline, but detailed rule-by-rule suppression mechanics remain package-local in `src/DCoding.Data.DVault.Analyzers/README.md`.

## Definition of Done
- Repository-facing public guidance has one current coordinated release record at `docs/releases/v0.12.0.md` and the previously visible `0.11.0` current-baseline references are updated wherever this ticket touches them.
- README-level consumer guidance is internally consistent with the shipped analyzer package README, current mapping attributes and mapper contracts in `DCoding.Data.DVault`, and generator diagnostics and tests already present on the branch.
- The ticket leaves no PO-level ambiguity about how v0.12 positions manual typed mappers versus generated helpers: both stay on the same explicit registry-backed save boundary, with generation as optional compile-time ergonomics.
- No additional child ticket, attachment, planning document, or relation change is required for PO-critic review.

## Implementation Notes
- Use `src/DCoding.Data.DVault.Analyzers/README.md` as the authoritative package-local source for installation wording, implemented diagnostic ids, bounded code-fix scope, and suppression mechanics; broader docs should summarize and link rather than restate that content in full.
- Use current repository code and tests as the release-note truth set: `DataVaultCodeFirstAnalyzer` and its tests for DMV1901/DMV1902 behavior, `DataVaultCodeFirstCodeFixProvider` and analyzer README for bounded code-fix coverage, and mapping attributes plus `DataVaultMappingSourceGenerator` tests for the generator surface and DMV1950-DMV1955 diagnostics.
- Root README updates should explicitly compare manual typed mappers and generated helpers at the boundary level: manual mappers and generated mappers both construct `DataVaultRegistry*SaveOperation` values and continue to rely on caller-supplied `loadTimestamp`, `recordSource`, and explicit save calls.
- Keep `examples/README.md` and the current quickstarts metadata-first unless a short consistency note is needed; this ticket does not need to retrofit the runnable examples into generator-based flows to close the release docs.
- If `docs/model-first-governance.md` is touched, limit the change to current-baseline references rather than reopening model-first behavior that the v0.12 analyzer/generator release did not change.
- Do not present DMV1901/DMV1902 as first introduced in v0.12; describe them as the carried-forward analyzer baseline and position v0.12 additions around bounded code fixes, mapping diagnostics, and generated helper output.
- No child tickets, relation rewrites, attachments, or planning documents were created during refinement.

## Open Questions
- none

## Follow-Up Questions
- Should a later documentation or examples ticket add a runnable consumer sample that uses the generated mapping attributes and generated mappers end-to-end, instead of keeping v0.12 at README and release-note level only?
- If future releases add more DMV195x diagnostics or broader generator shapes, should root-level docs grow a short diagnostic or capability table, or continue to keep detailed rule inventory package-local?

## Risks
- If broader docs duplicate the package-local suppression contract, README-level guidance can drift from the packaged analyzer README that consumers actually receive.
- If v0.12 docs describe generated helpers as a new metadata authority or hidden persistence layer, adopters may misunderstand the preserved explicit `IDataVaultSaveService` boundary.
- If `docs/releases/v0.12.0.md` fails to distinguish older DMV1901/DMV1902 baseline behavior from new v0.12 additions, release history will be misleading even if the technical behavior description is otherwise correct.
- If touched docs update version snippets but leave `v0.11.0` current-baseline prose in place elsewhere in the same surfaces, the public release narrative will remain inconsistent.

## Split Recommendations
- No additional split is recommended. The existing separation is already sufficient: package-local analyzer configuration and suppression guidance stays with done task `06F2PGJ28KVSZAAFRA40D94128`, generator contract and implementation stay with done tasks `06F2PGJN1XCV8F7NWH567SQSKM` and `06F2PGJSXP18VKKV52QZA4NP30`, and this ticket closes the coordinated v0.12 documentation and release-note sweep.
- If the team later wants a runnable generator sample or a broader diagnostics catalog page, create that as a separate follow-on documentation ticket instead of widening this release-closure task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Close the analyzer/generator release with adoption-ready docs.

## Scope
- Refine and complete the work for "Update v0.12.0 documentation and release notes" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.