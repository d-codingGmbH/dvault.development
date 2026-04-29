[gicket-bot] PO refinement contract

Summary
- PO decision remains: do not scaffold DVault source/test projects in this packaging ticket; it must wait for or target a branch containing the foundation DVault solution and packageable src/DVault project. The required dependency relation could not be persisted in this run because the bounded relation write was denied by trust policy, so the ticket is only partially resolved and must not resume to dev yet.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

Clarification resolution
- resolution-decision: `partially_resolved`
- critic-item-1: `answered` - Keep this ticket out of development until the target branch includes the packageable src/DVault project. The product decision is to wait for or retarget to the foundation layout, not to scaffold src/DVault or tests/DVault.Tests here. Runtime-managed status and label handoff should keep it from developer-ready routing on the current branch.
- critic-item-2: `needs_human_input` - The required dependency should be represented as blocks relation(s) from the foundation layout ticket(s) to this packaging ticket: 06EXB6XBV95E08R2W9ZQ1PRDPM blocks 06EXB6YVY0WHJYJ7ZNPE00K0AM and 06EXB6XVWBWZGN6MA3SFWGWKM4 blocks 06EXB6YVY0WHJYJ7ZNPE00K0AM. A bounded write was attempted for 06EXB6XVWBWZGN6MA3SFWGWKM4 -> 06EXB6YVY0WHJYJ7ZNPE00K0AM, but it failed with BOT-LOCAL-TOOL-TRUST-BLOCKED, so the relation was not created and still needs a trusted runtime/user action.
- critic-item-3: `answered` - Developer-ready routing must be withheld until the prerequisite project layout is present on the target branch or this ticket is retargeted to a branch that already contains it. This clarification materially affects implementation routing, so direct resume to dev on the current branch is not allowed. Workflow labels/status changes are runtime-managed and are not part of Acceptance Criteria or Definition of Done.
- critic-item-4: `answered` - Confirmed. The target branch is missing the packageable src/DVault project required by the ticket, so sending it to development now would violate the ticket Definition of Done and reproduce the previous implementation blocker. The correct action is to keep it blocked by foundation layout work, not to scaffold that layout here.
- critic-item-5: `needs_human_input` - Confirmed. The dependency relation is still not observed and the attempted relation write was blocked by trust policy, so automation can still pick up this ticket before the foundation layout exists unless a trusted process persists the blocks relation(s) or otherwise prevents developer routing.

Clarifications
- This ticket remains a packaging/build-configuration task for an existing packageable DVault project; it must not scaffold DVault.slnx, src/DVault, tests/DVault.Tests, or a new .csproj.
- The current branch evidence is authoritative for this pass: there is no packageable src/DVault project, no solution file, no src root, and no test root to configure.
- The ticket should wait for or be retargeted to a branch that contains the foundation DVault solution/library layout before development resumes.
- The dependency relation(s) from the foundation layout tickets to this ticket are required planning materialization, but no relation was created in this run because the bounded gicket-add-relation command was denied by trust policy.
- No child ticket, planning document, or attachment was created in this run; scaffolding stays assigned to existing foundation work rather than being split into this packaging ticket.

Scope In
- After the target branch contains an existing packageable src/DVault project, enable XML documentation output for that project build.
- Configure deterministic build settings appropriate for the existing .NET project structure when that structure is present.
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
- Who or which trusted runtime will persist the required blocks relation(s), given the bounded gicket-add-relation call was denied by trust policy and did not create the dependency?

Follow-up questions
- If either referenced foundation ticket does not actually own creation of DVault.slnx, src/DVault, and the packageable main library, identify the single correct foundation ticket and link it as the blocker before developer routing.
- Decide later whether XML documentation warnings should become hard errors across all projects once additional projects are added.
- Decide later whether package verification should be automated in CI after the repository CI workflow exists.
- Decide later whether separate public API documentation quality standards are needed beyond compiler XML documentation coverage.

Risks
- Until the blocks relation is persisted or an equivalent automation guard is applied, automation can pick up this ticket before the required foundation layout exists.
- Resuming development on the current branch without the foundation project layout will reproduce the same implementation blocker.
- SourceLink verification may depend on eventual repository host or remote metadata; if absent locally, implementation should configure standard settings and document the verification limit.
- Enforcing missing documentation warnings too aggressively could surface undocumented APIs; implementation should avoid broad API changes and document only what is necessary for the packaging baseline.

Split recommendations
- Do not split this ticket to include scaffolding. Scaffolding belongs in separate foundation work that creates the solution, packageable src/DVault project, and tests/DVault.Tests validation project.
- No child ticket was created in this run because the referenced foundation backlog items already cover the solution and main library prerequisite; the remaining required action is dependency relation persistence, not new scope creation.

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