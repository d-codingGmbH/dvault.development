[gicket-bot] PO-critic review contract

Summary
- Ticket contract is source-backed, prior PO-critic blockers were resolved, and the story is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git show HEAD:.gicket/tickets/06F2PGJBRXFCP038CN6XVAYSZM/description.md shows PO Handoff decision ready_for_po_critic and Open Questions: none.
- src/DCoding.Data.DVault.Analyzers/DataVaultCodeFirstAnalyzer.cs supports only DMV1901 and DMV1902, and src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs defines only those two diagnostic ids.
- rg -n "CodeFixProvider|CodeAction|Workspace" src/DCoding.Data.DVault.Analyzers tests/DCoding.Data.DVault.Tests/Analyzers returned no matches; src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj and tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj reference only Microsoft.CodeAnalysis and Microsoft.CodeAnalysis.CSharp.
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs already covers DMV1901, DMV1902, valid direct-member cases, separate-satellite duplicate guards, and selector-variable non-reporting, so the current analyzer boundary is concrete.
- docs/releases/v0.10.0.md states the analyzer package did not provide code fixes in that release, which aligns this ticket with a first bounded code-fix slice.
- Previous PO-critic comment 06F34AKYFBE15QHM0GERMS21CW returned the ticket to PO for inferred API claims; later PO refinement comment 06F34DCPV4W87BPY855S898PFW marks critic-item-1, critic-item-2, and critic-item-3 answered and the refreshed contract reflects that.
- git show --stat --name-only --format=fuller 2977ccc46 fe206c956 3fdd053ea shows the latest commits on this branch updated only .gicket ticket metadata and comments; no implementation work has started, which is acceptable for a pre-development handoff.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete rewrite example for a supported DMV1901 anonymous-object selector in each verb family would make the expected output format easier to test, but the current contract is still actionable.
- An explicit example where DMV1902 removes the later duplicate while preserving intervening fluent calls would strengthen test design, but it is not required for handoff.

Risky assumptions
- The contract assumes analyzer-facing direct readable scalar member behavior is the operative boundary; developers should not assume the runtime selector helpers are perfectly symmetric, because src/DCoding.Data.DVault/DataVaultCodeFirstSelector.cs only explicitly rejects collection members while the analyzer treats scalars as string or value types.

AC / test suggestions
- Verify a supported DMV1901 rewrite preserves member order and surrounding chain formatting when expanding an anonymous object with two or more direct scalar members.
- Add explicit no-fix checks for method-call, nested-member, collection-valued, computed, and selector-variable DMV1901 inputs so the bounded scope cannot expand accidentally.
- Verify DMV1902 removes only the later duplicate declaration inside one hub or one satellite scope and leaves the first declaration plus unrelated calls intact.
- Re-run analyzer packaging and asset validation after any Roslyn workspace or code-fix dependency additions so analyzer-only package behavior still holds.

Implementation watchouts
- Keep the code-fix surface internal and local to src/DCoding.Data.DVault.Analyzers and tests/DCoding.Data.DVault.Tests/Analyzers, matching the contract and current package boundary.
- README guidance is currently diagnostics-only in src/DCoding.Data.DVault.Analyzers/README.md; update it only if the new bounded code-fix behavior changes consumer-visible guidance.
- docs/releases/v0.12.0.md is absent and downstream ticket 06F2PGJYY6S97B4Z8044D34K5C remains todo, so release-note alignment is a downstream coordination item rather than scope for this story.
- The live relation graph still records this story as blocking 06F2PGJGDGMXHPT1VP0ASQ5HJ4, 06F2PGJN1XCV8F7NWH567SQSKM, 06F2PGJSXP18VKKV52QZA4NP30, and 06F2PGJYY6S97B4Z8044D34K5C; implementation timing should assume those dependencies remain in force until intentionally changed.

Non-blocking notes
- The persisted contract is now source-backed and no longer relies on an existing CodeFixProvider or other pre-existing public code-fix API.
- Current branch state is ticket-metadata-only; that is expected at this pre-development gate.

Split recommendations
- No additional split is needed before developer handoff while scope stays limited to DMV1901 anonymous-object direct-member expansion and DMV1902 later-duplicate removal.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment