[gicket-bot] PO refinement contract

Summary
- PO clarification remains: this packaging ticket must not scaffold DVault source or test projects. It is blocked from developer implementation until the target branch contains the foundation DVault solution and packageable src/DVault project, and the missing dependency relation still needs trusted persistence.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

Clarification resolution
- resolution-decision: `partially_resolved`
- critic-item-1: `answered` - Return/keep this ticket in PO refinement or blocked planning flow until the target branch includes the packageable src/DVault project. The product decision is to wait for or retarget to a branch with the foundation layout, not to scaffold the layout in this ticket.
- critic-item-2: `needs_human_input` - The required relation should be persisted as blocks relation(s) from the foundation layout ticket(s) to this packaging ticket, specifically 06EXB6XBV95E08R2W9ZQ1PRDPM and/or 06EXB6XVWBWZGN6MA3SFWGWKM4 blocking 06EXB6YVY0WHJYJ7ZNPE00K0AM. Current observed relations still do not include that blocks relation, and a prior bounded write was denied by trust policy, so a trusted runtime or human must perform the relation update.
- critic-item-3: `answered` - Developer-ready routing must be withheld until src/DVault exists on the target branch or the ticket is retargeted to a branch that already contains the packageable project. Status and label updates are runtime-managed, so this clarification answers the routing rule without adding workflow metadata to Acceptance Criteria or Definition of Done.
- critic-item-4: `answered` - Confirmed. The target branch still lacks the packageable src/DVault project, so sending this ticket to development now would violate its own Definition of Done. The ticket remains blocked on foundation layout rather than changing scope to scaffold it.
- critic-item-5: `needs_human_input` - Confirmed. The prerequisite foundation backlog items are referenced but no observed blocks relation currently protects this ticket from being picked up before the layout exists. The remaining blocker is dependency materialization by a trusted runtime or human, not a product-scope question.

Clarifications
- The active product decision is unchanged: this ticket configures packaging/build behavior only after an existing packageable src/DVault project is present.
- Do not scaffold DVault.slnx, src/DVault, tests/DVault.Tests, or any new .csproj under this ticket.
- The current branch is not implementable for this ticket because it has no src root, no tests root, no solution file, and no packageable project.
- The ticket should wait for the foundation layout to land on the target branch, or be retargeted to a branch that already contains that layout.
- The missing dependency relation remains the only blocking planning materialization item identified in this pass; no child ticket, planning document, or attachment was created.

Scope In
- After the target branch contains an existing packageable src/DVault project, enable XML documentation output for that project build.
- Configure deterministic build settings appropriate for the existing .NET project structure once present.
- Configure SourceLink and package metadata for source-linked symbols where supported by the existing project and repository metadata.
- Ensure local package and symbol output can be produced and inspected without publishing externally after the packageable project exists.
- Use tests/DVault.Tests for validation only if that test project exists on the implementation branch.

Scope Out
- Scaffolding a new solution, src/DVault project, tests/DVault.Tests project, or packageable project structure.
- Defining public API, namespace, extension method, or test architecture as part of this packaging task.
- Publishing packages or symbols to an external feed.
- Changing public API shape solely to satisfy documentation warnings.
- Introducing a multi-project packaging strategy beyond the eventual visible packageable DVault project.

Open questions
- Who or which trusted runtime will persist the required blocks relation(s) from the foundation layout work to this ticket, given the available bounded relation write path previously failed trust policy and the relation is still absent?

Follow-up questions
- If neither 06EXB6XBV95E08R2W9ZQ1PRDPM nor 06EXB6XVWBWZGN6MA3SFWGWKM4 owns creation of DVault.slnx, src/DVault, and the packageable main library, identify the single correct foundation ticket and link it as the blocker before developer routing.
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