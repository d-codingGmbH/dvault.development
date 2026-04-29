[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the persisted contract has no unresolved Open Questions, the current branch now contains the target project and validation surface, and the contract keeps the work scoped to packaging/build configuration rather than scaffolding or API changes.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted delivery contract contains '## Open Questions' with '- none', so there are no unresolved open questions blocking approve_for_dev.
- Persisted delivery contract states the ticket targets existing src/DVault/DVault.csproj and tests/DVault.Tests/DVault.Tests.csproj, and explicitly scopes out scaffolding new solution/source/test projects.
- repository-list-directory src/DVault returned src/DVault/DVault.csproj plus C# sources under src/DVault/Modeling, confirming the packageable project exists on the current review branch.
- repository-read-text src/DVault/DVault.csproj shows TargetFramework net10.0, ImplicitUsings enable, Nullable enable, and GenerateDocumentationFile true, with Microsoft.AspNetCore.App framework reference.
- repository-list-directory tests/DVault.Tests returned tests/DVault.Tests/DVault.Tests.csproj plus Integration, Modeling, Shared, and Unit test project/files, confirming the validation surface exists.
- repository-read-text Directory.Build.props shows only DefaultItemExcludes and test output path settings for DVault.Tests.Shared, DVault.Tests.Unit, and DVault.Tests.Integration; it does not already define deterministic build, SourceLink, symbol package, or documentation warning policy settings.
- gicket-read-ticket-comments returned recent PO refinement comments including human unblock note and PO refinement contract stating the foundation layout is visible after merging develop, stale blocked routing labels were removed, and handoff to po-critic was committed on branch ticket/06EXB6YVY0WHJYJ7ZNPE00K0AM-task-configure-xml-documentation-and-determinist as 357280e5c1cb.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- README.md still describes src/DCoding.Data.DVault and tests/DCoding.Data.DVault.* placeholders rather than src/DVault and tests/DVault.Tests, but the ticket contract directly clarifies the current target paths, so this is not a handoff blocker for this packaging task.
- SourceLink verification may be limited if repository host/remote metadata is absent locally; the contract already calls out that risk and asks implementation to document the verification limit.

AC / test suggestions
- Keep the existing AC requiring build output XML documentation, missing public/protected documentation diagnostics, deterministic settings, SourceLink configuration, and local nupkg/snupkg inspection.
- During implementation validation, include direct local commands for src/DVault/DVault.csproj package/build output inspection and tests/DVault.Tests validation, or document exact environmental blockers.

Implementation watchouts
- Do not scaffold new projects or solution membership as part of this ticket; use src/DVault/DVault.csproj or a shared build props/targets file as the configuration surface.
- The current project already has GenerateDocumentationFile=true, so implementation should focus on verifying XML artifact behavior and adding only missing deterministic build, documentation warning policy, SourceLink, and package/symbol metadata settings.
- Avoid public API changes solely to satisfy documentation warnings; keep generated nupkg, snupkg, bin, and obj artifacts out of commits.

Non-blocking notes
- Earlier shell-command attempts using chained commands were blocked by the runtime, but repository-list-directory, repository-read-text, gicket-read-ticket, and gicket-read-ticket-comments provided enough direct evidence for ticket-level review.

Split recommendations
- Do not split this ticket for scaffolding; branch evidence confirms the required source and test layout is present.
- Leave CI automation of package verification and broader XML documentation warning hardening to the follow-up questions already listed in the contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment