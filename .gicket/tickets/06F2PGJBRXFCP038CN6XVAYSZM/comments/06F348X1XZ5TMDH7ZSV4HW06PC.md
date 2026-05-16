[gicket-bot] PO refinement contract

Summary
- Refined the story to a bounded code-fix slice over the existing analyzer baseline: keep work in DCoding.Data.DVault.Analyzers, add fixes only where the correction is mechanical, and leave broader docs, relation cleanup, and source-generation work to existing downstream tickets. No child tickets, relation writes, attachments, or planning documents were created in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository source and tests bound the current analyzer baseline to DMV1901 and DMV1902 only; no CodeFixProvider exists yet.
- This story stays under epic 06F2PGHJAFMH80TZAMANQWH9PW; incoming blocks edges from done story 06F2PGHQ2GATEM13M5QK1MSX1G and done epic 06F2PGFT8Z406HFBJGQSY7YRJ0 are historical context, not active blockers.
- Ratified safe auto-fix scope for DMV1901: offer a fix only when the unsupported selector is an anonymous-object list of direct readable scalar members that can be expanded into repeated same-verb calls in source order.
- Ratified safe auto-fix scope for DMV1902: remove the later duplicate BusinessKey(...), Payload(...), or DrivingKey(...) invocation and keep the first declaration authoritative.
- Do not offer DMV1901 fixes for method-call, nested-member, collection-valued, computed, selector-variable, or other non-mechanical shapes.
- Keep the work inside the existing analyzer package and analyzer test project; do not introduce a separate VSIX or standalone code-fix package.
- The live graph still shows this story blocking 06F2PGJGDGMXHPT1VP0ASQ5HJ4, 06F2PGJN1XCV8F7NWH567SQSKM, 06F2PGJSXP18VKKV52QZA4NP30, and 06F2PGJYY6S97B4Z8044D34K5C; this refinement run did not rewrite those relations.

Scope In
- Add bounded Roslyn code-fix behavior for current DVault analyzer findings inside src/DCoding.Data.DVault.Analyzers.
- Support DMV1902 duplicate-removal fixes for duplicate BusinessKey(...), Payload(...), and DrivingKey(...) declarations inside one relevant fluent scope.
- Support DMV1901 expansion of anonymous-object direct-member selectors into repeated same-verb single-member calls in original order.
- Add or update analyzer code-fix tests and analyzer-package README guidance for offered-fix and no-fix behavior.
- Keep public diagnostic ids, titles, and current analyzer rule semantics aligned with DMV1901 and DMV1902.

Scope Out
- Fixes for non-mechanical selector shapes such as method calls, nested members, computed expressions, collection-valued members, selector variables, or inferred rewrites.
- New diagnostics, broader dataflow or model validation, metadata-first or model-first diagnostics, provider diagnostics, or source-generator work.
- Separate package or VSIX boundaries, repo-wide refactors, or package-family reshaping.
- Creation of docs/releases/v0.12.0.md or broader release-doc closure beyond directly touched analyzer package docs; that remains with 06F2PGJYY6S97B4Z8044D34K5C.
- Relation-graph cleanup for the existing source-generation and documentation dependencies.

Open questions
- none

Follow-up questions
- Should the live blocks edges from this story to 06F2PGJGDGMXHPT1VP0ASQ5HJ4, 06F2PGJN1XCV8F7NWH567SQSKM, and 06F2PGJSXP18VKKV52QZA4NP30 be audited separately if source-generation work can proceed independently of analyzer code fixes?
- When 06F2PGJYY6S97B4Z8044D34K5C runs, should the v0.12 release notes call out the new mechanical code-fix coverage separately from the underlying DMV1901 and DMV1902 diagnostics?
- After this bounded slice lands, is there value in a separate follow-on ticket for broader refactor or fix-all ergonomics beyond the single-location mechanical fixes?

Risks
- A too-aggressive DMV1901 fixer could silently change intent, so the ticket must stay limited to anonymous-object direct-member expansion and explicitly no-fix elsewhere.
- Adding Roslyn workspace and code-fix dependencies to a package that currently ships analyzer-only assets widens package-validation surface and needs verification against the existing analyzer packaging checks.
- Versioned package examples on the branch still point at 0.11.0, so downstream v0.12 documentation work must realign examples when this story is delivered.
- The live relation graph currently blocks source-generation tickets as well as the v0.12 documentation task, which may create scheduling pressure until those dependencies are intentionally confirmed or cleaned up.

Split recommendations
- No additional split is recommended if the story stays limited to DMV1902 duplicate removal and DMV1901 anonymous-object expansion.
- If the team wants fixes for non-mechanical selector shapes or broader Roslyn refactorings, create a separate follow-on ticket instead of widening this story.
- Keep coordinated release-note closure in existing task 06F2PGJYY6S97B4Z8044D34K5C rather than creating another documentation child from this story.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment