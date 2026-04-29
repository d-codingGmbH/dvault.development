[gicket-bot] PO refinement contract

Summary
- Refined the package metadata story using ticket state, relation state, and repository evidence from src/DVault/DVault.csproj and Directory.Build.props. No new child tickets, relations, attachments, or planning documents were created; existing parentOf child relations remain 06EXB6YKXPPC6GPNHB02CBDPKW and 06EXB6YVY0WHJYJ7ZNPE00K0AM.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 package identity is ratified as PackageId DCoding.Data.DVault, matching the README library identity and the active src/DVault/DVault.csproj manifest.
- The owning packable project for this story is src/DVault/DVault.csproj; broader source-root naming cleanup is not required for this packaging metadata ticket.
- Apache-2.0 is the v1 license metadata choice through PackageLicenseExpression.
- Repository metadata is satisfied by RepositoryUrl https://github.com/d-codingGmbH/dvault.development.git and RepositoryType git.
- Deterministic and repository-aware build defaults belong in Directory.Build.props so they apply consistently across projects.
- The story prepares local package creation only; it must not add automatic NuGet publishing.

Scope In
- Define and verify NuGet package metadata for the DVault library project, including PackageId, Title, Authors, Description, PackageTags, README packaging, license expression, repository URL, and repository type.
- Ensure symbols are produced using IncludeSymbols and SymbolPackageFormat snupkg.
- Ensure deterministic build metadata is enabled through shared MSBuild properties such as Deterministic, ContinuousIntegrationBuild, DebugType portable, PublishRepositoryUrl, and EmbedUntrackedSources.
- Ensure dotnet pack can be run locally from the repository against the owning source project and writes package artifacts to the repository package output location.
- Confirm the repository does not introduce an automatic publish step as part of this story.

Scope Out
- Publishing to NuGet or any package registry.
- Adding NuGet credentials, API keys, release secrets, or deployment workflows.
- Defining final public release versioning, signing policy, package icon ownership, or release notes automation.
- Renaming product namespaces, moving source roots, or changing public API behavior beyond what is necessary for package metadata.
- Provider-specific Data Vault functionality, schema generation, migrations, or advanced capabilities such as PIT, bridges, multi-active satellites, or provider optimizations.

Open questions
- none

Follow-up questions
- Before first real publication, decide the release credential workflow, registry ownership, package signing requirements, and whether a package icon or project URL should be added.
- Before public release, confirm whether the repository URL should remain the development repository URL or move to a public canonical repository URL.
- A later cleanup ticket may reconcile older README layout wording with the active src/DVault project path if that remains inconsistent after sibling scaffold tickets land.

Risks
- Local pack success depends on the .NET 10 SDK baseline being available in the developer or CI environment.
- The repository currently shows multiple historical project/root names in snapshots; developers should target src/DVault/DVault.csproj for this story to avoid packaging the wrong project.
- Because publishing is intentionally out of scope, registry-specific validation will remain deferred until a release/publishing ticket.

Split recommendations
- No new split is recommended for this PO refinement. The ticket already has two persisted child relations, 06EXB6YKXPPC6GPNHB02CBDPKW and 06EXB6YVY0WHJYJ7ZNPE00K0AM, and this parent story is ready for PO-critic review without additional child-ticket materialization.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment