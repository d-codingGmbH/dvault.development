<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- PO clarification is resolved: the target branch now has the foundation layout, including src/DVault/DVault.csproj and tests/DVault.Tests, so this ticket should proceed as configuration work against the existing project rather than scaffold new projects or remain blocked on the prior missing-layout finding.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Do not scaffold DVault.slnx, src/DVault, tests/DVault.Tests, or any new project as part of this packaging ticket.
- The ticket now targets the existing packageable project at src/DVault/DVault.csproj on the current branch.
- The existing validation surface is tests/DVault.Tests/DVault.Tests.csproj; use it when relevant for local validation.
- The prior open question about which trusted runtime or human must persist a blocks relation is closed because the branch now contains the prerequisite layout.
- No child tickets or planning documents were created during this clarification pass.

### Scope In
- Configure or verify XML documentation output for the existing src/DVault/DVault.csproj build.
- Configure deterministic build settings for the existing .NET project structure or shared build configuration.
- Configure SourceLink and package metadata for source-linked symbols where supported by the existing project and repository metadata.
- Produce and inspect local package and symbol artifacts without publishing externally.
- Run relevant validation against tests/DVault.Tests when the implementation changes are ready.

### Scope Out
- Scaffolding a new solution, src/DVault project, tests/DVault.Tests project, or packageable project structure.
- Defining public API, namespace, extension method, or test architecture as part of this packaging task.
- Publishing packages or symbols to an external feed.
- Changing public API shape solely to satisfy documentation warnings.
- Introducing a multi-project packaging strategy beyond the visible packageable DVault project.

## Acceptance Criteria
- Building src/DVault/DVault.csproj emits XML documentation output in expected build and package artifacts.
- Missing XML documentation for public or protected APIs is reported during build as warnings or errors consistent with the repository warning policy.
- Deterministic build settings are enabled for the existing packageable project or shared build configuration.
- SourceLink is configured where supported so generated symbols can map back to repository source information.
- A local package build of the existing project produces package and symbol artifacts that can be inspected locally without publishing.

## Definition of Done
- Implementation is limited to build/package configuration and minimal supporting documentation needed to validate it.
- Relevant build, test, and package commands complete locally, or the exact environmental blocker is documented with the command attempted.
- Generated package and symbol artifacts are verified locally for XML documentation and source/symbol metadata.
- tests/DVault.Tests validation is run where applicable for the implementation branch.
- Generated nupkg, snupkg, bin, and obj artifacts are not committed.

## Implementation Notes
- Use src/DVault/DVault.csproj as the primary implementation target.
- The project already has GenerateDocumentationFile set to true; implementation should verify whether this satisfies the XML documentation requirement and add only the remaining build/package configuration needed for deterministic output, warning behavior, SourceLink, and package artifacts.
- Prefer shared .NET build configuration such as Directory.Build.props or Directory.Build.targets if appropriate; otherwise configure src/DVault/DVault.csproj directly.
- Use standard .NET properties such as Deterministic, ContinuousIntegrationBuild, GenerateDocumentationFile, symbol package settings, and SourceLink package/reference settings compatible with the repository host.
- Do not add a new blocks relation for the earlier missing-layout prerequisite unless future branch evidence again shows the packageable project is absent.

## Open Questions
- none

## Follow-Up Questions
- Decide later whether XML documentation warnings should become hard errors across all projects once additional projects are added.
- Decide later whether package verification should be automated in CI after the repository CI workflow exists.
- Decide later whether separate public API documentation quality standards are needed beyond compiler XML documentation coverage.
- If solution-level packaging becomes required, decide in a separate foundation or build-system ticket whether DVault.slnx should include all project references.

## Risks
- SourceLink verification may depend on eventual repository host or remote metadata; if absent locally, implementation should configure standard settings and document the verification limit.
- Enforcing missing documentation warnings too aggressively could surface undocumented APIs; implementation should avoid broad API changes and document only what is necessary for the packaging baseline.
- DVault.slnx exists but was observed as minimal; implementation should target src/DVault/DVault.csproj directly unless solution membership is confirmed separately.

## Split Recommendations
- Do not split this ticket to include scaffolding; the required source and test layout is already visible on the current branch.
- No child ticket is recommended for the old dependency-relation blocker because the prerequisite is now satisfied by the branch state.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Configure build settings that support documentation and reproducible package output.

## Scope
- Enable XML documentation output.
- Enable deterministic builds and SourceLink where appropriate.

## Acceptance Criteria
- Missing docs for public/protected APIs surface as build warnings or errors.
- Package symbols can be inspected locally.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.