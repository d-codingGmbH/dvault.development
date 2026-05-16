<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Reframed the story around current-branch evidence: DMV1901 and DMV1902 diagnostics exist today, no code-fix surface exists yet, and this ticket may create the first minimal internal analyzer-package code-fix implementation for DMV1901 anonymous-object direct-member expansion and DMV1902 later-duplicate removal only. No child tickets, relation writes, attachments, or planning documents were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current branch evidence shows the analyzer baseline is limited to DMV1901 and DMV1902 in src/DCoding.Data.DVault.Analyzers/DataVaultCodeFirstAnalyzer.cs and src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs.
- Current branch evidence also shows no existing CodeFixProvider, CodeAction, Workspace, or code-fix test surface under src/DCoding.Data.DVault.Analyzers or tests/DCoding.Data.DVault.Tests; this story therefore authorizes creating the minimal new internal code-fix implementation surface required for the bounded cases.
- The existing analyzer test project is tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj, and it currently references only Microsoft.CodeAnalysis and Microsoft.CodeAnalysis.CSharp.
- docs/releases/v0.12.0.md is still missing on the branch snapshot, so coordinated v0.12 release-note authoring remains downstream with 06F2PGJYY6S97B4Z8044D34K5C.
- This story stays under epic 06F2PGHJAFMH80TZAMANQWH9PW; incoming blocks edges from done story 06F2PGHQ2GATEM13M5QK1MSX1G and done epic 06F2PGFT8Z406HFBJGQSY7YRJ0 are historical context, not active blockers.
- Live repository relation files still show this story blocking 06F2PGJGDGMXHPT1VP0ASQ5HJ4, 06F2PGJN1XCV8F7NWH567SQSKM, 06F2PGJSXP18VKKV52QZA4NP30, and 06F2PGJYY6S97B4Z8044D34K5C; no relation writes were materialized in this refinement run.
- No child tickets, relation writes, attachments, or planning documents were created in this run.

### Scope In
- Create the analyzer package's first bounded Roslyn code-fix implementation inside src/DCoding.Data.DVault.Analyzers for existing diagnostics DMV1901 and DMV1902.
- Support DMV1902 by removing the later duplicate BusinessKey(...), Payload(...), or DrivingKey(...) invocation inside one relevant fluent scope while keeping the first declaration authoritative.
- Support DMV1901 only when the flagged selector is an anonymous-object list of direct readable scalar members that can be expanded into repeated same-verb single-member calls in source order.
- Add or update tests in the existing analyzer test project and update src/DCoding.Data.DVault.Analyzers/README.md only if consumer-visible analyzer guidance changes.
- Keep public diagnostic ids, titles, and existing analyzer rule semantics aligned with DMV1901 and DMV1902.

### Scope Out
- No code-fix coverage for method-call, nested-member, computed, collection-valued, selector-variable, or other non-mechanical selector shapes.
- No new diagnostics, broader dataflow or model validation, metadata-first or model-first diagnostics, provider diagnostics, or source-generator work.
- No separate VSIX, standalone code-fix package, repo-wide refactor, or package-family reshaping under this ticket.
- No creation of docs/releases/v0.12.0.md or broader release-note closure beyond directly touched analyzer package guidance; that remains with 06F2PGJYY6S97B4Z8044D34K5C.
- No relation-graph cleanup work is included in this ticket.

## Acceptance Criteria
- The ticket creates code-fix behavior only for the bounded mechanical cases: DMV1902 later-duplicate removal and DMV1901 anonymous-object direct-member expansion.
- Applying the DMV1901 code fix rewrites one supported anonymous-object selector into repeated BusinessKey(...), Payload(...), or DrivingKey(...) calls that each target one direct readable scalar member and preserve original member order.
- Applying the DMV1902 code fix removes only the redundant later declaration and keeps the earlier declaration and surrounding fluent scope intact.
- No code fix is offered for method-call, nested-member, computed, collection-valued, selector-variable, or other non-mechanical DMV1901 shapes.
- Tests cover both offered code-fix cases and explicit no-fix cases, and the analyzer's supported diagnostics remain DMV1901 and DMV1902.
- If consumer-visible analyzer package guidance changes, src/DCoding.Data.DVault.Analyzers/README.md is updated to describe the bounded code-fix behavior and existing suppression paths.

## Definition of Done
- A minimal new internal code-fix implementation and only the Roslyn workspace/code-fix dependencies required for it are added inside the existing analyzer package and existing analyzer test project.
- Verification shows correct rewritten source for supported DMV1901 and DMV1902 cases and no offered code fix for excluded shapes.
- Analyzer packaging and existing analyzer assets still work from the current package flow after the new code-fix implementation is added.
- Repository guidance remains consistent across analyzer source, analyzer tests, and src/DCoding.Data.DVault.Analyzers/README.md.
- Any release-note impact needed for coordinated v0.12 closure is handed to existing downstream doc task 06F2PGJYY6S97B4Z8044D34K5C; no extra child split is required here.

## Implementation Notes
- Use src/DCoding.Data.DVault.Analyzers/DataVaultCodeFirstAnalyzer.cs and src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs as the fixed diagnostic baseline; do not add new diagnostic ids under this ticket.
- Use tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs and tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj as the existing analyzer-test baseline; extend this slice for code-fix verification instead of creating a new package boundary.
- Current branch contains no CodeFixProvider, CodeAction, Workspace, or Roslyn code-fix dependency references in the analyzer package or analyzer test project, so any new provider/type and any required Roslyn workspace/code-fix dependencies may be created here but should stay minimal and local to those existing projects.
- Use src/DCoding.Data.DVault/DataVaultCodeFirstSelector.cs, src/DCoding.Data.DVault/DataVaultCodeFirstHubBuilder.cs, src/DCoding.Data.DVault/DataVaultCodeFirstSatelliteBuilder.cs, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs as the safety boundary for mechanically correctable selector shapes.
- Preserve call order when expanding supported DMV1901 anonymous-object selectors; emit repeated same-verb direct-member calls rather than introducing helpers or renaming members.
- For DMV1902, delete the flagged later invocation rather than renaming or reordering declarations.
- Keep any new code-fix/provider types internal unless Roslyn registration requires otherwise; this ticket does not authorize a broader public API surface.
- Keep README updates limited to bounded code-fix behavior and suppression guidance; broader release-note authoring stays downstream.

## Open Questions
- none

## Follow-Up Questions
- Should the live blocks edges from this story to 06F2PGJGDGMXHPT1VP0ASQ5HJ4, 06F2PGJN1XCV8F7NWH567SQSKM, and 06F2PGJSXP18VKKV52QZA4NP30 be audited separately if source-generation work can proceed independently of analyzer code fixes?
- When 06F2PGJYY6S97B4Z8044D34K5C runs, should the v0.12 release notes call out the new bounded code-fix coverage separately from the underlying DMV1901 and DMV1902 diagnostics?
- After this bounded slice lands, is there value in a separate follow-on ticket for broader refactor or fix-all ergonomics beyond the single-location mechanical fixes?

## Risks
- A too-aggressive DMV1901 expansion could silently change intent, so the ticket must stay limited to anonymous-object direct-member expansion and explicitly no-fix elsewhere.
- Adding Roslyn workspace and code-fix dependencies to a package that currently ships analyzer-only assets widens packaging and validation surface and needs verification against the existing analyzer package flow.
- docs/releases/v0.12.0.md is still absent on the branch snapshot, so downstream v0.12 documentation work must realign release notes and versioned examples when this story is delivered.
- The live relation graph currently blocks source-generation tickets as well as the v0.12 documentation task, which may create scheduling pressure until those dependencies are intentionally confirmed or cleaned up.

## Split Recommendations
- No additional split is recommended while the story stays limited to DMV1902 later-duplicate removal and DMV1901 anonymous-object direct-member expansion.
- If the team wants code-fix coverage for non-mechanical selector shapes, broader Roslyn refactorings, or fix-all ergonomics, create a separate follow-on ticket instead of widening this story.
- Keep coordinated v0.12 release-note closure in existing task 06F2PGJYY6S97B4Z8044D34K5C rather than creating another documentation child from this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Provide safe code fixes where the intended correction is mechanical.

## Scope
- Refine and complete the work for "Add code fixes for common DVault analyzer findings" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.