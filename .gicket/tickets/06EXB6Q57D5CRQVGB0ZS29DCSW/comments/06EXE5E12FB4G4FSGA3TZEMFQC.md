[gicket-bot] PO-critic review contract

Summary
- The ticket is ready for developer handoff as a docs-only task. The persisted contract has no open questions, clearly resolves the build/test validation conflict, and the committed planning document satisfies the deferred-capabilities scope without introducing project or source artifacts.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Current branch is ticket/06EXB6Q57D5CRQVGB0ZS29DCSW-task-document-deferred-data-vault-capabilities at HEAD 04d27029311561e18e9ac9d0e1902cfa196771f5.
- .gicket/tickets/06EXB6Q57D5CRQVGB0ZS29DCSW/description.md contains PO Handoff decision ready_for_po_critic and ## Open Questions with '- none'.
- Comment .gicket/tickets/06EXB6Q57D5CRQVGB0ZS29DCSW/comments/06EXE3PQW2PT327EPQ81S0J4CW.md records the PO refinement resolution: waive/replace dotnet build/test for this docs-only no-project baseline and do not add DVault.sln, projects, source, tests, or build-only artifacts.
- Comment .gicket/tickets/06EXB6Q57D5CRQVGB0ZS29DCSW/comments/06EXE3QEYVNX6AZQ80TRCHYAJ0.md records handoff commit 907f5feb1ffc on the target branch; comment 06EXE3QJWDYBKMYFVZKSQMEQ10.md reports outcome po-refinement-ready.
- git ls-files for *.sln, *.csproj, *.fsproj, *.vbproj, src/**, test/**, tests/**, and docs/plans/deferred-data-vault-capabilities.md returned only docs/plans/deferred-data-vault-capabilities.md.
- git show HEAD:docs/plans/deferred-data-vault-capabilities.md contains the committed document with Deferred Capabilities rows for PIT table generation, Bridge table generation, Multi-active satellites, and Provider-specific optimizations.
- docs/plans/deferred-data-vault-capabilities.md states the capabilities are post-MVP, are not required for the MVP release, and must not block the first package.
- docs/plans/deferred-data-vault-capabilities.md Planning Guardrails say not to treat the deferred items as MVP requirements and not to introduce current API, generator, adapter, or provider capability commitments.
- Sibling context .gicket/tickets/06EXB6PX7ZGYNR2SXF44C5VPJM/description.md covers hub, link, satellite, hash key, hash diff, load timestamp, and record source support and says the document avoids promising unimplemented automation; the deferred-capabilities document mirrors that boundary.
- git log --follow -- docs/plans/deferred-data-vault-capabilities.md shows the planning document was added in commit 519e63f [06EXB6Q57D5CRQVGB0ZS29DCSW] DEV-FAILED-SNAPSHOT failure-snapshot (build); later PO comments resolved the failure as a validation-process issue for the docs-only baseline.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The docs-only validation waiver is explicitly scoped to the current no-project baseline; if a real .NET project or solution is introduced later, build/test expectations must be revisited.

AC / test suggestions
- For this ticket, validation should inspect the committed planning document and record the git ls-files evidence that no tracked solution, project, src, test, or tests paths exist.

Implementation watchouts
- Do not add DVault.sln, a .NET project, source files, test files, or placeholder build artifacts for this ticket.
- Keep any future edits to planning language only; avoid final API names, generator methods, provider capability flags, or implementation commitments.

Non-blocking notes
- git status shows a dirty worktree with many .gicket files and docs/plans/deferred-data-vault-capabilities.md modified; targeted git diff --ignore-space-at-eol for the ticket description and planning document was empty, and git show --check HEAD for the planning document reported no issues, so this appears to be local line-ending or metadata noise rather than a content blocker.
- git ls-files .gicket/relations | rg 06EXB6Q57D5CRQVGB0ZS29DCSW found one parent relation from 06EXB6PNA0VA1XTR85B6X3T7ZG to this ticket; no evidence of newly required child tickets was found for this review.

Split recommendations
- No split is required for this ticket; the contract appropriately keeps the deferred-capabilities documentation as one docs-only task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment