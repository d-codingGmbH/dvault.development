<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the ticket, bot-only comments, parent story relation, charter attachment, current foundation project layout, and repository remote. No child tickets, relations, attachments, or planning documents were created. The package license metadata decision is now persisted: use SPDX PackageLicenseExpression `Apache-2.0` for DCoding.Data.DVault.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket is ready for PO-critic review

### Clarifications
- This task is a child of story 06EXB6YBXPDBPWZPNV89A9F9AM, Establish package identity and project metadata.
- The v1 package identity should use PackageId DCoding.Data.DVault, matching the charter-required default namespace and library identity.
- The visible target for metadata is the main library project under src/DVault/DVault.csproj; project creation remains out of scope for this ticket because the foundation layout now exists on the branch.
- Use the visible origin remote as the repository baseline: https://github.com/d-codingGmbH/dvault.development.git.
- No publishing workflow should be added or enabled by this ticket.

### Scope In
- Add NuGet package metadata to the main DCoding.Data.DVault library project once the project file exists.
- Include package id, authors, English description, tags, package readme metadata, SPDX license expression `Apache-2.0`, repository URL/type, and symbols package settings.
- Configure local package output for inspection, including snupkg symbols where supported by the SDK settings in the project.
- Ensure README packaging is wired so the generated package references and includes a readme file.
- Verify that no CI or local workflow introduced by this ticket publishes to NuGet.

### Scope Out
- Publishing to NuGet or adding credentials, API keys, release tokens, or push commands.
- Creating the main library project or solution skeleton if that separate foundation work has not landed yet.
- Full README quickstart authoring beyond any minimal package readme content needed for metadata correctness.
- SourceLink and deterministic-build policy beyond avoiding conflicts with the sibling XML documentation and deterministic builds task.
- Package verification automation beyond local manual inspection; the separate packaging verification ticket covers that.

## Acceptance Criteria
- The package metadata for DCoding.Data.DVault is present on the main library project and can be seen in locally produced package output.
- The generated local package contains the expected package id, authors, description, tags, repository metadata, readme metadata, license metadata, and symbols settings.
- The package readme file is included in the package and all package-facing text is in English.
- No workflow, script, target, or documented command added by this ticket publishes the package to NuGet.
- The implementation does not conflict with the sibling task for XML documentation, deterministic builds, and SourceLink.

## Definition of Done
- Local package inspection evidence is produced by dotnet pack or equivalent package inspection without uploading anything.
- The metadata follows the charter identity DCoding.Data.DVault and the repository's formatting expectations: UTF-8, LF, two-space indentation where applicable, and English documentation text.
- The package symbols configuration produces or is ready to produce an inspectable snupkg locally.
- No NuGet publishing endpoint, token, or automatic publish step exists as part of this change.
- The approved license metadata decision is applied as PackageLicenseExpression `Apache-2.0` before development is considered complete.

## Implementation Notes
- Prefer the main library project file for v1 metadata because only one package project is visible in planning; a central props file is only needed if the foundation branch establishes that convention first.
- Use PackageId DCoding.Data.DVault and a description aligned to the charter: a convention-first .NET 10 library extending Entity Framework for Data Vault 2.x-oriented persistence.
- Use package tags such as dotnet, entity-framework, ef-core, data-vault, data-vault-2, dvault, and persistence unless the project later standardizes a different tag set.
- For a project under src/DVault, a root README.md can be packed by including it as a Pack item with PackagePath set to the package root and PackageReadmeFile set to README.md.
- Authors can default to d-coding GmbH based on the visible GitHub organization and actor email unless a later branding policy overrides it.
- Use PackageLicenseExpression `Apache-2.0`; do not add a PackageLicenseFile unless a later legal/release ticket creates a repository LICENSE file and changes the package policy.
- If SourceLink settings are added elsewhere, do not duplicate or fight those settings here; keep this ticket focused on package metadata and publish prevention.

## Open Questions
- none

## Follow-Up Questions
- Before a public NuGet release, confirm the final NuGet owner/profile, repository visibility, icon, project URL, release notes, and signing requirements.
- After the README quickstart ticket lands, confirm whether the package readme should remain the root README.md or a package-specific README under the library project.

## Risks
- Development should target the existing main DCoding.Data.DVault project file under src/DVault and avoid moving packaging metadata into unrelated projects.
- Apache-2.0 has been selected for NuGet package metadata; changing that later should be handled by a separate legal/release decision.
- README, SourceLink, and package verification work overlaps adjacent tickets, so implementation should stay narrowly on metadata and local inspection.

## Split Recommendations
- No split is recommended for this ticket; the license metadata decision is now resolved and implementation remains bounded.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Add package metadata needed for a future NuGet release.

## Scope
- Include package id, authors, description, tags, readme, license, repository URL, and symbols settings.

## Acceptance Criteria
- Package metadata is complete enough for local inspection.
- No workflow publishes the package.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.