[gicket-bot] PO refinement contract

Summary
- Verified the ticket, bot-only comments, parent story relation, charter attachment, empty src/test roots, and repository remote. No child tickets, relations, attachments, or planning documents were created. The ticket is scoped well, but the package license metadata is not yet decided in repository evidence.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

Clarifications
- This task is a child of story 06EXB6YBXPDBPWZPNV89A9F9AM, Establish package identity and project metadata.
- The v1 package identity should use PackageId DCoding.Data.DVault, matching the charter-required default namespace and library identity.
- The visible target for metadata is the planned main library project under src/DVault; the current branch only contains empty src/DVault and tests/DVault.Tests directories, so project creation remains with the foundation task.
- Use the visible origin remote as the repository baseline: https://github.com/d-codingGmbH/dvault.development.git.
- No publishing workflow should be added or enabled by this ticket.

Scope In
- Add NuGet package metadata to the main DCoding.Data.DVault library project once the project file exists.
- Include package id, authors, English description, tags, package readme metadata, license metadata, repository URL/type, and symbols package settings.
- Configure local package output for inspection, including snupkg symbols where supported by the SDK settings in the project.
- Ensure README packaging is wired so the generated package references and includes a readme file.
- Verify that no CI or local workflow introduced by this ticket publishes to NuGet.

Scope Out
- Publishing to NuGet or adding credentials, API keys, release tokens, or push commands.
- Creating the main library project or solution skeleton if that separate foundation work has not landed yet.
- Full README quickstart authoring beyond any minimal package readme content needed for metadata correctness.
- SourceLink and deterministic-build policy beyond avoiding conflicts with the sibling XML documentation and deterministic builds task.
- Package verification automation beyond local manual inspection; the separate packaging verification ticket covers that.

Open questions
- Which license metadata is approved for DCoding.Data.DVault: a specific SPDX PackageLicenseExpression or a PackageLicenseFile backed by an approved repository license file? The repository currently has no LICENSE/NOTICE file and no persisted ticket evidence choosing a license.

Follow-up questions
- Before a public NuGet release, confirm the final NuGet owner/profile, repository visibility, icon, project URL, release notes, and signing requirements.
- After the README quickstart ticket lands, confirm whether the package readme should remain the root README.md or a package-specific README under the library project.

Risks
- Development may be blocked until the foundation task creates the main DCoding.Data.DVault project file under src/DVault.
- Choosing license metadata without an approved legal or project decision would create publication risk even if this ticket does not publish.
- README, SourceLink, and package verification work overlaps adjacent tickets, so implementation should stay narrowly on metadata and local inspection.

Split recommendations
- No split is recommended for this ticket; the remaining blocker is a bounded license metadata decision, not excessive implementation size.

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