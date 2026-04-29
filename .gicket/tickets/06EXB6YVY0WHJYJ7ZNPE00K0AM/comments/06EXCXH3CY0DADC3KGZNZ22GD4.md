[gicket-bot] PO refinement contract

Summary
- Resolved the blocker by narrowing this ticket back to packaging configuration only: do not scaffold src/DVault or tests/DVault.Tests here; run this ticket only after the foundation source/test layout exists on the target branch.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- Should this ticket be changed to explicitly scaffold a new src/DVault packageable project plus tests/DVault.Tests validation project, or should it wait for/target the branch that already contains the intended DVault source and test layout?: `answered` - This ticket must wait for or target a branch that already contains the intended packageable src/DVault project and, when available, tests/DVault.Tests validation project. It must not be expanded to scaffold the solution, source project, test project, public API, or packageable project structure.

Clarifications
- The previous contract assumption that the current branch contains src/DVault and tests/DVault.Tests is corrected: the current branch has no packageable DVault source project and no validation test project.
- The v1 decision is to keep this ticket as a packaging/build-configuration task for an existing packageable DVault project, not a project-scaffolding or API-shaping task.
- Implementation should not create src/DVault, tests/DVault.Tests, a solution file, or a new .csproj solely to make this packaging task executable.
- This clarification materially changes the implementation contract, so direct resume to development on the current branch is not appropriate unless the prerequisite project layout is already present by then.

Scope In
- Once the target branch contains an existing packageable src/DVault project, enable XML documentation output for that project build.
- Configure deterministic build settings appropriate for the existing .NET project structure when that structure is present.
- Configure SourceLink/package metadata for source-linked symbols where supported by the existing project and repository metadata.
- Ensure local package/symbol output can be produced and inspected without publishing externally after the packageable project exists.
- Use tests/DVault.Tests for validation only if that test project exists on the implementation branch.

Scope Out
- Scaffolding a new solution, src/DVault project, tests/DVault.Tests project, or packageable project structure.
- Defining public API, namespace, extension method, or test architecture as part of this packaging task.
- Publishing packages or symbols to an external feed.
- Changing public API shape solely to satisfy documentation warnings.
- Introducing a multi-project packaging strategy beyond the eventual visible packageable DVault project.

Open questions
- none

Follow-up questions
- Identify or link the existing foundation ticket or branch that introduces the solution, src/DVault, and tests/DVault.Tests layout if the backlog does not already expose that dependency.
- Decide later whether XML documentation warnings should become hard errors across all projects once additional projects are added.
- Decide later whether package verification should be automated in CI after the repository CI workflow exists.
- Decide later whether separate public API documentation quality standards are needed beyond compiler XML documentation coverage.

Risks
- Resuming development on the current branch without the foundation project layout will reproduce the same implementation blocker.
- SourceLink verification may depend on eventual repository host or remote metadata; if absent locally, implementation should configure standard settings and document the verification limit.
- Enforcing missing documentation warnings too aggressively could surface undocumented APIs; implementation should avoid broad API changes and document only what is necessary for the packaging baseline.

Split recommendations
- Do not split this ticket to include scaffolding. Scaffolding belongs in separate foundation work that creates the solution, packageable src/DVault project, and tests/DVault.Tests validation project.
- No child ticket was created in this run because the referenced planning context already states that separate foundation tickets provide the solution and main library project; create or link that foundation task only if it is not already tracked.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment