[gicket-bot] PO refinement contract

Summary
- Packaging ticket remains blocked from development: it must wait for or retarget to a branch that already contains DVault.slnx and the packageable src/DVault project. Scaffolding stays out of scope, and a trusted dependency relation from the foundation work is still required before developer routing.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

Clarification resolution
- resolution-decision: `partially_resolved`
- critic-item-1: `answered` - Return/keep this ticket in PO refinement or blocked planning state until the target branch contains the packageable src/DVault project. Runtime-managed status and labels are not part of the delivery contract, but the product decision is that development must not resume on the current branch.
- critic-item-2: `needs_human_input` - The correct blocker relation should be persisted from the foundation layout work before developer routing. Current evidence identifies 06EXB6XBV95E08R2W9ZQ1PRDPM as the solution/folder skeleton story and 06EXB6XVWBWZGN6MA3SFWGWKM4 as its child library-project task, but this run cannot confirm that the required blocks relation was created; trusted runtime or human action must add it.
- critic-item-3: `answered` - Withhold developer-ready routing until the prerequisite layout is present or the ticket is retargeted to a branch containing it. Workflow label/status handoff is runtime-managed, so this clarification only records the planning constraint and does not add status labels to Acceptance Criteria or DoD.
- critic-item-4: `answered` - Confirmed: the current target branch is not implementable for this ticket. The ticket must wait for the foundation branch/layout or be retargeted; developer implementation must not scaffold src/DVault or tests/DVault.Tests here.
- critic-item-5: `needs_human_input` - Confirmed: the prerequisite backlog items exist, but the packaging ticket still lacks an observed blocks relation from the solution/library foundation task(s). This remains the only unresolved blocking planning materialization item and requires trusted persistence before developer routing.

Clarifications
- Product decision: do not expand this packaging ticket to scaffold DVault.slnx, src/DVault, tests/DVault.Tests, or any new .csproj.
- This ticket should wait for the foundation layout to land on the target branch, or be retargeted to a branch where DVault.slnx and src/DVault already exist.
- The current branch remains non-implementable because repository evidence shows no src root, no tests root, and no packageable DVault project.
- The remaining blocker is not product scope; it is persistence of the blocks relation from the foundation layout work to this packaging ticket.

Scope In
- After the target branch contains an existing packageable src/DVault project, enable XML documentation output for that project build.
- Configure deterministic build settings for the existing .NET project structure once present.
- Configure SourceLink and package metadata for source-linked symbols where supported by the existing project and repository metadata.
- Produce and inspect local package and symbol artifacts without publishing externally after the packageable project exists.
- Use tests/DVault.Tests for validation only if that test project exists on the implementation branch.

Scope Out
- Scaffolding a new solution, src/DVault project, tests/DVault.Tests project, or packageable project structure.
- Defining public API, namespace, extension method, or test architecture as part of this packaging task.
- Publishing packages or symbols to an external feed.
- Changing public API shape solely to satisfy documentation warnings.
- Introducing a multi-project packaging strategy beyond the eventual visible packageable DVault project.

Open questions
- Who or which trusted runtime will persist the required blocks relation from the foundation layout work to 06EXB6YVY0WHJYJ7ZNPE00K0AM, given the current relation read still shows it absent?

Follow-up questions
- If neither 06EXB6XBV95E08R2W9ZQ1PRDPM nor 06EXB6XVWBWZGN6MA3SFWGWKM4 is chosen as the concrete blocker source, identify the single correct foundation ticket and link it before developer routing.
- Decide later whether XML documentation warnings should become hard errors across all projects once additional projects are added.
- Decide later whether package verification should be automated in CI after the repository CI workflow exists.
- Decide later whether separate public API documentation quality standards are needed beyond compiler XML documentation coverage.

Risks
- Until the blocks relation is persisted or an equivalent automation guard is applied, automation can pick up this ticket before the required foundation layout exists.
- Resuming development on the current branch without the foundation project layout will reproduce the same implementation blocker.
- SourceLink verification may depend on eventual repository host or remote metadata; if absent locally, implementation should configure standard settings and document the verification limit.
- Enforcing missing documentation warnings too aggressively could surface undocumented APIs; implementation should avoid broad API changes and document only what is necessary for the packaging baseline.

Split recommendations
- Do not split this ticket to include scaffolding; scaffolding belongs in separate foundation work that creates the solution, packageable src/DVault project, and tests/DVault.Tests validation project.
- No child ticket is recommended because the referenced foundation backlog items already cover the prerequisite layout; the remaining required action is dependency relation persistence, not new scope creation.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment