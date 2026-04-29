[gicket-bot] PO refinement contract

Summary
- Product decision remains: do not scaffold projects in this packaging ticket; keep it blocked from developer implementation until the foundation layout exists and a trusted blocks relation is persisted. The attempted blocks relation from 06EXB6XBV95E08R2W9ZQ1PRDPM to 06EXB6YVY0WHJYJ7ZNPE00K0AM was rejected by the local trust policy, so relation persistence still requires a trusted runtime or human action.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

Clarification resolution
- resolution-decision: `partially_resolved`
- critic-item-1: `answered` - The ticket should remain out of developer implementation until the target branch contains DVault.slnx and the packageable src/DVault project, or until it is retargeted to a branch where that layout already exists. Scaffolding is explicitly out of scope for this packaging task.
- critic-item-2: `needs_human_input` - A blocks relation is still required from the foundation layout work to this ticket. The selected default blocker remains 06EXB6XBV95E08R2W9ZQ1PRDPM -> 06EXB6YVY0WHJYJ7ZNPE00K0AM, with 06EXB6XVWBWZGN6MA3SFWGWKM4 acceptable only if the trusted runtime identifies it as the concrete project-layout owner. A bounded relation write was attempted for 06EXB6XBV95E08R2W9ZQ1PRDPM -> 06EXB6YVY0WHJYJ7ZNPE00K0AM but failed with BOT-LOCAL-TOOL-TRUST-BLOCKED, so a trusted runtime or human must persist the relation before developer routing.
- critic-item-3: `answered` - Developer-ready routing must be withheld. Do not direct-resume this ticket to dev while the target branch lacks DVault.slnx and src/DVault and while the required blocks relation is absent. Runtime-managed labels and status transitions should handle the actual metadata changes.
- critic-item-4: `answered` - Confirmed. The target branch is non-implementable for this packaging ticket because it has no packageable src/DVault project. The correct PO decision is to wait for or retarget to a branch with the foundation layout, not to scaffold that layout here.
- critic-item-5: `needs_human_input` - Confirmed. The missing blocks relation remains the only unresolved planning persistence blocker. Because the bounded relation add was trust-policy blocked, automation may still pick the ticket prematurely unless a trusted runtime or human persists the relation or applies an equivalent automation guard.

Clarifications
- This packaging ticket must not be changed to scaffold DVault.slnx, src/DVault, tests/DVault.Tests, or any .csproj.
- The ticket should wait for the foundation layout to land on the target branch or be retargeted to a branch where DVault.slnx and the packageable src/DVault project already exist.
- The current target branch remains non-implementable because the repository snapshot shows no src roots, no test roots, and no tests/DVault.Tests path.
- The required relation persistence was attempted for 06EXB6XBV95E08R2W9ZQ1PRDPM -> 06EXB6YVY0WHJYJ7ZNPE00K0AM and failed due BOT-LOCAL-TOOL-TRUST-BLOCKED; this is now the narrowed unresolved blocker.

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
- Which trusted runtime or human with relation-write permission will persist the required blocks relation now that the bounded PO relation-write attempt was denied by trust policy?

Follow-up questions
- If 06EXB6XBV95E08R2W9ZQ1PRDPM is not the concrete foundation layout owner, identify whether 06EXB6XVWBWZGN6MA3SFWGWKM4 is the correct blocker source and persist that single relation instead before developer routing.
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
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment