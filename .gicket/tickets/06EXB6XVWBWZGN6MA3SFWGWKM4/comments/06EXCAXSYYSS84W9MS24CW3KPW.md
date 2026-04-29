[gicket-bot] PO refinement contract

Summary
- Refined the project-setup task using persisted ticket state, comments, relations, attachments, and repository evidence. No child tickets or planning documents were created because the scope is already bounded to creating the initial class library project.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current ticket is a child of 06EXB6XBV95E08R2W9ZQ1PRDPM via an existing parentOf relation; no additional relation work is needed.
- The current ticket has no product-relevant human comments and no persisted attachments; referenced repository documents are sufficient context for this refinement.
- Although the branch snapshot named src/DVault and tests/DVault.Tests as intended roots, direct repository reads show those directories and project files do not yet exist. This ticket should create the source project rather than modify an existing one.
- The v1 default project path is src/DVault/DVault.csproj, matching the visible branch planning snapshot and the package/library naming convention in the ticket title.
- No repository-level Directory.Build.props exists yet, so nullable reference types, XML docs, target framework, and documentation warning behavior should be set in the new project file unless a later standards ticket introduces shared build props.

Scope In
- Create the main DVault class library project at src/DVault/DVault.csproj.
- Configure the project to target net10.0.
- Set RootNamespace to DCoding.Data.DVault.
- Enable nullable reference types.
- Enable XML documentation generation.
- Configure public and protected API documentation enforcement for the project, including treating missing XML documentation warnings as errors for the relevant compiler warning IDs.
- Add only minimal source needed for a valid class library if the SDK template or build requires it.

Scope Out
- Creating the tests/DVault.Tests project or any test framework setup.
- Adding DVault domain behavior, storage abstractions, serialization, encryption, or data model APIs.
- Creating NuGet packaging metadata beyond what is required for the basic class library project.
- Creating or refactoring repository-wide build files such as Directory.Build.props.
- Changing workflow metadata, labels, status, or runtime handoff fields.

Open questions
- none

Follow-up questions
- Should a later foundation ticket introduce shared Directory.Build.props settings once multiple projects exist?
- Should a later test-foundation ticket create tests/DVault.Tests and choose the test framework baseline?
- Should package metadata, analyzers, strong naming, SourceLink, and publish settings be handled in a separate packaging or release-readiness ticket?

Risks
- The net10.0 SDK may not be installed in all local environments yet, so build verification may be environment-limited even when the project file is correct.
- Documentation enforcement choices should stay narrowly tied to public/protected API XML documentation so this setup ticket does not unexpectedly enforce unrelated analyzer policy.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment