[gicket-bot] PO refinement contract

Summary
- PO clarification is resolved: the target branch now has the foundation layout, including src/DVault/DVault.csproj and tests/DVault.Tests, so this ticket should proceed as configuration work against the existing project rather than scaffold new projects or remain blocked on the prior missing-layout finding.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - The earlier blocked-state requirement is superseded by current branch evidence. The target branch now includes src/DVault with DVault.csproj, tests/DVault.Tests, and DVault.slnx, so the ticket no longer needs to remain blocked for missing foundation layout.
- critic-item-2: `answered` - Do not add a new blocking dependency relation now. The dependency relation was needed only while the foundation layout was absent; current branch evidence shows the prerequisite layout is present, so persisting a new blocks relation would recreate stale routing friction. The contract should instead state that implementation targets the existing src/DVault project now visible on the branch.
- critic-item-3: `answered` - Developer-ready routing no longer needs to be withheld for missing layout because the prerequisite layout is present. Runtime-managed labels and statuses should remain outside the product contract; PO scope is to update the ticket contract so implementation targets the existing project rather than scaffolding or waiting.
- critic-item-4: `answered` - The blocking finding is no longer true for the current branch. src/DVault/DVault.csproj exists and is the packageable project surface for this ticket. The ticket must still not scaffold new projects; it should configure and verify the visible existing project.
- critic-item-5: `answered` - The missing-blocks-relation risk is resolved by the prerequisite now being present on the target branch. A blocks relation is no longer required to prevent premature pickup because automation picking up the ticket now is acceptable after the PO contract is updated to target the existing layout.

Clarifications
- Do not scaffold DVault.slnx, src/DVault, tests/DVault.Tests, or any new project as part of this packaging ticket.
- The ticket now targets the existing packageable project at src/DVault/DVault.csproj on the current branch.
- The existing validation surface is tests/DVault.Tests/DVault.Tests.csproj; use it when relevant for local validation.
- The prior open question about which trusted runtime or human must persist a blocks relation is closed because the branch now contains the prerequisite layout.
- No child tickets or planning documents were created during this clarification pass.

Scope In
- Configure or verify XML documentation output for the existing src/DVault/DVault.csproj build.
- Configure deterministic build settings for the existing .NET project structure or shared build configuration.
- Configure SourceLink and package metadata for source-linked symbols where supported by the existing project and repository metadata.
- Produce and inspect local package and symbol artifacts without publishing externally.
- Run relevant validation against tests/DVault.Tests when the implementation changes are ready.

Scope Out
- Scaffolding a new solution, src/DVault project, tests/DVault.Tests project, or packageable project structure.
- Defining public API, namespace, extension method, or test architecture as part of this packaging task.
- Publishing packages or symbols to an external feed.
- Changing public API shape solely to satisfy documentation warnings.
- Introducing a multi-project packaging strategy beyond the visible packageable DVault project.

Open questions
- none

Follow-up questions
- Decide later whether XML documentation warnings should become hard errors across all projects once additional projects are added.
- Decide later whether package verification should be automated in CI after the repository CI workflow exists.
- Decide later whether separate public API documentation quality standards are needed beyond compiler XML documentation coverage.
- If solution-level packaging becomes required, decide in a separate foundation or build-system ticket whether DVault.slnx should include all project references.

Risks
- SourceLink verification may depend on eventual repository host or remote metadata; if absent locally, implementation should configure standard settings and document the verification limit.
- Enforcing missing documentation warnings too aggressively could surface undocumented APIs; implementation should avoid broad API changes and document only what is necessary for the packaging baseline.
- DVault.slnx exists but was observed as minimal; implementation should target src/DVault/DVault.csproj directly unless solution membership is confirmed separately.

Split recommendations
- Do not split this ticket to include scaffolding; the required source and test layout is already visible on the current branch.
- No child ticket is recommended for the old dependency-relation blocker because the prerequisite is now satisfied by the branch state.

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