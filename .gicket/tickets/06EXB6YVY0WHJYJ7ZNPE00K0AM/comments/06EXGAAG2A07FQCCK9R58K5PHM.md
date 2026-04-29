[gicket-bot] PO refinement contract

Summary
- The packaging ticket remains blocked from developer implementation. It must wait for or be retargeted to a branch containing the packageable src/DVault project; scaffolding remains out of scope. The only unresolved blocker is persistence of the required blocks relation by a trusted runtime or human because the bounded PO relation write was denied by trust policy.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

Clarification resolution
- resolution-decision: `partially_resolved`
- critic-item-1: `answered` - Keep the ticket in PO refinement or otherwise blocked from developer implementation until the target branch includes the packageable src/DVault project. The ticket should not resume to dev on the current branch.
- critic-item-2: `needs_human_input` - The correct planning dependency is a blocks relation from the foundation layout work to this packaging ticket, with 06EXB6XBV95E08R2W9ZQ1PRDPM as the preferred blocker source. However, the bounded relation write has already failed with BOT-LOCAL-TOOL-TRUST-BLOCKED, so a trusted runtime or human with relation-write permission must persist it before developer routing. Use 06EXB6XVWBWZGN6MA3SFWGWKM4 only if the trusted runtime identifies it as the concrete project-layout owner.
- critic-item-3: `answered` - Withhold developer-ready routing until both prerequisites are true: the target branch contains DVault.slnx plus the packageable src/DVault project, and the blocks relation from the foundation layout work is persisted. Runtime-managed status or label cleanup should be handled by orchestration, not as a product acceptance item.
- critic-item-4: `answered` - Confirmed. The target branch is missing the packageable src/DVault project, so sending the ticket to dev now would reproduce the prior blocker. The product decision is to wait for the foundation layout or retarget to a branch where it already exists, not to scaffold it here.
- critic-item-5: `needs_human_input` - Confirmed. No trusted blocks relation is currently persisted because the bounded PO attempt was rejected by trust policy. This remains the narrowed blocker: a trusted runtime or human must add the dependency relation so automation cannot pick up this packaging ticket before the foundation layout exists.

Clarifications
- Do not change this packaging ticket to scaffold DVault.slnx, src/DVault, tests/DVault.Tests, or any .csproj.
- This ticket must wait for the foundation layout to land on the target branch or be retargeted to a branch where DVault.slnx and the packageable src/DVault project already exist.
- The current target branch remains non-implementable because the branch snapshot shows no src roots, no test roots, and no tests/DVault.Tests path.
- The required blocks relation should be from 06EXB6XBV95E08R2W9ZQ1PRDPM to 06EXB6YVY0WHJYJ7ZNPE00K0AM unless a trusted runtime identifies 06EXB6XVWBWZGN6MA3SFWGWKM4 as the concrete project-layout owner.
- A bounded PO relation write for 06EXB6XBV95E08R2W9ZQ1PRDPM -> 06EXB6YVY0WHJYJ7ZNPE00K0AM was attempted and failed due BOT-LOCAL-TOOL-TRUST-BLOCKED, so relation persistence requires a trusted runtime or human action.

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