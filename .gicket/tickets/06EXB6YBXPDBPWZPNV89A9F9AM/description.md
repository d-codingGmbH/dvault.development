<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the package metadata story using ticket state, relation state, and repository evidence from src/DVault/DVault.csproj and Directory.Build.props. No new child tickets, relations, attachments, or planning documents were created; existing parentOf child relations remain 06EXB6YKXPPC6GPNHB02CBDPKW and 06EXB6YVY0WHJYJ7ZNPE00K0AM.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 package identity is ratified as PackageId DCoding.Data.DVault, matching the README library identity and the active src/DVault/DVault.csproj manifest.
- The owning packable project for this story is src/DVault/DVault.csproj; broader source-root naming cleanup is not required for this packaging metadata ticket.
- Apache-2.0 is the v1 license metadata choice through PackageLicenseExpression.
- Repository metadata is satisfied by RepositoryUrl https://github.com/d-codingGmbH/dvault.development.git and RepositoryType git.
- Deterministic and repository-aware build defaults belong in Directory.Build.props so they apply consistently across projects.
- The story prepares local package creation only; it must not add automatic NuGet publishing.

### Scope In
- Define and verify NuGet package metadata for the DVault library project, including PackageId, Title, Authors, Description, PackageTags, README packaging, license expression, repository URL, and repository type.
- Ensure symbols are produced using IncludeSymbols and SymbolPackageFormat snupkg.
- Ensure deterministic build metadata is enabled through shared MSBuild properties such as Deterministic, ContinuousIntegrationBuild, DebugType portable, PublishRepositoryUrl, and EmbedUntrackedSources.
- Ensure dotnet pack can be run locally from the repository against the owning source project and writes package artifacts to the repository package output location.
- Confirm the repository does not introduce an automatic publish step as part of this story.

### Scope Out
- Publishing to NuGet or any package registry.
- Adding NuGet credentials, API keys, release secrets, or deployment workflows.
- Defining final public release versioning, signing policy, package icon ownership, or release notes automation.
- Renaming product namespaces, moving source roots, or changing public API behavior beyond what is necessary for package metadata.
- Provider-specific Data Vault functionality, schema generation, migrations, or advanced capabilities such as PIT, bridges, multi-active satellites, or provider optimizations.

## Acceptance Criteria
- src/DVault/DVault.csproj declares PackageId DCoding.Data.DVault and includes package title, authors, description, useful non-duplicated tags, README packaging, Apache-2.0 license expression, repository URL, and repository type.
- Repository-wide MSBuild metadata enables deterministic portable packages with repository/source metadata and does not conflict with the project-level package manifest.
- Local dotnet pack against src/DVault/DVault.csproj succeeds on the supported .NET 10 SDK baseline and emits both a .nupkg and .snupkg under bin/packages or the documented package output path.
- The produced NuGet package contains the README at the package root and exposes the expected package metadata when inspected locally.
- No CI workflow, MSBuild target, script, or configuration introduced by this work pushes packages to NuGet or another remote feed automatically.

## Definition of Done
- All acceptance criteria are satisfied and evidenced by local pack output or equivalent local inspection.
- Repository formatting standards remain satisfied, including the shared bash tools/check-format.sh gate where available for the changed files.
- The implementation follows docs/plans/shared-implementation-standards.md and docs/formatting.md for layout, encoding, and build metadata conventions.
- Package artifacts are generated only as local build outputs and are not committed unless an existing repository policy explicitly allows them.
- Any metadata values that differ from the ratified v1 defaults are documented in the ticket or implementation notes before handoff.

## Implementation Notes
- Keep PackageId as DCoding.Data.DVault for v1; do not reopen package naming unless a governance ticket changes the library identity.
- Use src/DVault/DVault.csproj as the package manifest source of truth for this story.
- Directory.Build.props already carries the appropriate deterministic build baseline: Deterministic true, ContinuousIntegrationBuild true, DebugType portable, PublishRepositoryUrl true, EmbedUntrackedSources true, RepositoryType git, and the shared RepositoryUrl.
- The current manifest already includes symbols via IncludeSymbols true and SymbolPackageFormat snupkg; preserve that behavior.
- PackageOutputPath resolving to bin/packages is acceptable for local pack artifacts, provided package outputs remain uncommitted build output.
- Existing parentOf relations from this story to 06EXB6YKXPPC6GPNHB02CBDPKW and 06EXB6YVY0WHJYJ7ZNPE00K0AM were observed; no additional split was materialized in this PO pass.

## Open Questions
- none

## Follow-Up Questions
- Before first real publication, decide the release credential workflow, registry ownership, package signing requirements, and whether a package icon or project URL should be added.
- Before public release, confirm whether the repository URL should remain the development repository URL or move to a public canonical repository URL.
- A later cleanup ticket may reconcile older README layout wording with the active src/DVault project path if that remains inconsistent after sibling scaffold tickets land.

## Risks
- Local pack success depends on the .NET 10 SDK baseline being available in the developer or CI environment.
- The repository currently shows multiple historical project/root names in snapshots; developers should target src/DVault/DVault.csproj for this story to avoid packaging the wrong project.
- Because publishing is intentionally out of scope, registry-specific validation will remain deferred until a release/publishing ticket.

## Split Recommendations
- No new split is recommended for this PO refinement. The ticket already has two persisted child relations, 06EXB6YKXPPC6GPNHB02CBDPKW and 06EXB6YVY0WHJYJ7ZNPE00K0AM, and this parent story is ready for PO-critic review without additional child-ticket materialization.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Prepare package metadata without publishing to NuGet yet.

## Scope
- Define package id, description, repository metadata, license metadata, symbols, and deterministic builds.

## Acceptance Criteria
- dotnet pack can run locally.
- The package is not published automatically.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.