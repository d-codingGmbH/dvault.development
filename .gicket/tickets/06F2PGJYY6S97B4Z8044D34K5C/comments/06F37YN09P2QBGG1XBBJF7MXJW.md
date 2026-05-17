[gicket-bot] PO refinement contract

Summary
- Refined the v0.12.0 documentation ticket into a bounded release-closure doc sweep: create `docs/releases/v0.12.0.md`, align public versioned guidance to `0.12.0`, and update root/public docs for the shipped analyzer code-fix and source-generator surface; no child tickets, relation writes, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows `docs/releases` stops at `v0.11.0`, while release `06F2PH99NN9B0S4RZW0NPST1CR` is active as `v0.12.0 - Analyzer and Generator Ergonomics`; this ticket owns the missing coordinated `docs/releases/v0.12.0.md` closure.
- Done sibling tickets already establish the feature baseline this ticket must document: `06F2PGHWEWYJZSRQ9QPT4NJ0QM` and `06F2PGJBRXFCP038CN6XVAYSZM` cover analyzer diagnostics and bounded code fixes, `06F2PGJ28KVSZAAFRA40D94128` covers package-local analyzer configuration and suppression guidance, and `06F2PGJN1XCV8F7NWH567SQSKM` plus `06F2PGJSXP18VKKV52QZA4NP30` define and implement the source-generator contract.
- Current repository guidance is inconsistent with shipped behavior: root `README.md`, `examples/README.md`, `docs/model-first-governance.md`, and `src/DCoding.Data.DVault.Analyzers/README.md` still contain `0.11.0` baseline references, and root `README.md` still describes the analyzer package as only the earlier high-confidence DMV1901/DMV1902 slice.
- The analyzer package README is already the authoritative deep guide for installation and suppression details; broader docs should point to it and update scope statements, but should not duplicate a second conflicting suppression contract.
- The v0.12 public docs should explicitly state that generated mapper helpers stay on the existing `DataVaultRegistry*SaveOperation` and `IDataVault*Mapper<TSource>` boundary and do not hide caller-owned `loadTimestamp`, `recordSource`, or `IDataVaultSaveService` orchestration.
- Incoming `blocks` relations from done tickets `06F2PGFT8Z406HFBJGQSY7YRJ0`, `06F2PGHQ2GATEM13M5QK1MSX1G`, and `06F2PGJBRXFCP038CN6XVAYSZM` are satisfied historical context rather than PO blockers; incoming `parentOf` relations from epic `06F2PGHJAFMH80TZAMANQWH9PW` and story `06F2PGJGDGMXHPT1VP0ASQ5HJ4` keep this ticket inside the active analyzer/generator release graph.
- No child tickets, relation writes, attachments, or planning documents were materialized during this refinement pass.

Scope In
- Create `docs/releases/v0.12.0.md` as the coordinated public release record for `v0.12.0 - Analyzer and Generator Ergonomics`, following the established release-note structure used by `docs/releases/v0.10.0.md` and `docs/releases/v0.11.0.md`.
- Update root `README.md` so installation snippets, current-release references, notable-change summary, and limitations match the shipped analyzer code-fix and source-generator surface.
- Version-align and minimally refresh other public docs that currently present outdated `0.11.0` or Code-First-only analyzer guidance: `examples/README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md` where their current-baseline text points at `v0.11.0`.
- Document the consumer-facing generator boundary: compile-time mapping declarations live in `DCoding.Data.DVault`, generation happens via the optional analyzer package, and generated helpers feed the existing explicit save flow rather than introducing a new metadata authority or hidden persistence API.
- Carry the existing DMV1901/DMV1902 analyzer baseline forward into the current public package description while clearly separating that carried-forward baseline from new v0.12 additions such as bounded code fixes, DMV1950-DMV1955 mapping diagnostics, and generated mapper helpers.

Scope Out
- Any new analyzer diagnostics, code fixes, source-generator features, runtime API changes, or package-shape changes; this ticket documents shipped behavior only.
- A new runnable quickstart, sample application, or end-to-end example project for generated mapper adoption.
- A second detailed suppression or analyzer-configuration guide outside `src/DCoding.Data.DVault.Analyzers/README.md`.
- Manual-publication workflow changes or package-verification rule changes unless a touched consumer doc would otherwise stay factually inconsistent.
- Relation cleanup, child-ticket creation, or broader planning-graph reshaping beyond this ticket's bounded documentation closure.

Open questions
- none

Follow-up questions
- Should a later documentation or examples ticket add a runnable consumer sample that uses the generated mapping attributes and generated mappers end-to-end, instead of keeping v0.12 at README and release-note level only?
- If future releases add more DMV195x diagnostics or broader generator shapes, should root-level docs grow a short diagnostic or capability table, or continue to keep detailed rule inventory package-local?

Risks
- If broader docs duplicate the package-local suppression contract, README-level guidance can drift from the packaged analyzer README that consumers actually receive.
- If v0.12 docs describe generated helpers as a new metadata authority or hidden persistence layer, adopters may misunderstand the preserved explicit `IDataVaultSaveService` boundary.
- If `docs/releases/v0.12.0.md` fails to distinguish older DMV1901/DMV1902 baseline behavior from new v0.12 additions, release history will be misleading even if the technical behavior description is otherwise correct.
- If touched docs update version snippets but leave `v0.11.0` current-baseline prose in place elsewhere in the same surfaces, the public release narrative will remain inconsistent.

Split recommendations
- No additional split is recommended. The existing separation is already sufficient: package-local analyzer configuration and suppression guidance stays with done task `06F2PGJ28KVSZAAFRA40D94128`, generator contract and implementation stay with done tasks `06F2PGJN1XCV8F7NWH567SQSKM` and `06F2PGJSXP18VKKV52QZA4NP30`, and this ticket closes the coordinated v0.12 documentation and release-note sweep.
- If the team later wants a runnable generator sample or a broader diagnostics catalog page, create that as a separate follow-on documentation ticket instead of widening this release-closure task.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment