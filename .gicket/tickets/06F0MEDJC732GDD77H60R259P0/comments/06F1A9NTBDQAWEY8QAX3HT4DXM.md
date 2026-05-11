[gicket-bot] PO-critic review contract

Summary
- Tracking-only parent ticket closure audit found blocking readiness gaps.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md persists PO Handoff decision `ready_for_po_critic` and `## Open Questions` contains only `none`.
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/comments/06F1A604MFZMN20088VWHKS20R.md records capable-runner validation at commit 3967d99c57977b65770dff03c79b0f938ade059d: PackageVerifierTests passed, `dotnet pack` succeeded, `bash tools/verify-packages.sh` succeeded, and six `.nupkg` plus six `.snupkg` artifacts were freshly produced at 0.5.1-alpha.0.69.
- `git merge-base --is-ancestor 3967d99c57977b65770dff03c79b0f938ade059d HEAD` confirmed that accepted validation commit is an ancestor of current HEAD 273fb9a2a on branch ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u.
- `git diff --name-status develop...HEAD` shows scoped changes to README.md, docs/releases/v0.6.0.md, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs; `git diff --check` returned no whitespace errors for those files.
- README.md:10-15 contains all six v0.6.0 `dotnet add package` commands; docs/releases/v0.6.0.md:1,17,21-25,41-53 documents v0.6.0 scope, shipped highlights, known limitations, and tagged-release validation handoff.
- PackageVerifier.cs:14 sets ExpectedReadmeInstallVersion to 0.6.0 and PackageVerifier.cs:416 validates provider dependencies against the packed core version; PackageVerifierTests.cs:12 and :283 generate README install checks with 0.6.0, and targeted rg found no stale 0.5.0/0.5.1-alpha.0.58 package-verifier expectations.
- Direct public API evidence exists for the documented compatibility surface: ApplyDataVaultMetadata at DataVaultCodeFirstModelBuilderExtensions.cs:16 and :41; Hub/Link at DataVaultCodeFirstModelBuilder.cs:23, :40, :50; BusinessKey/Satellite at DataVaultCodeFirstHubBuilder.cs:25, :53; DrivingKey/Payload at DataVaultCodeFirstSatelliteBuilder.cs:22, :38; Participant at DataVaultCodeFirstLinkBuilder.cs:18; IDataVaultReadService and typed read helpers at IDataVaultReadService.cs:8, :16 and DataVaultReadServiceTypedProjectionExtensions.cs:48.

Blocking findings
- No outgoing parentOf child tickets were found for the tracking-only parent ticket.

Required PO actions
- Resolve the tracking-parent closure audit findings before this parent ticket can be closed.

Open issues ledger
- critic-item-1 [required-po-action] Resolve the tracking-parent closure audit findings before this parent ticket can be closed.
- critic-item-2 [blocking-finding] No outgoing parentOf child tickets were found for the tracking-only parent ticket.

Missing examples / edge cases
- none

Risky assumptions
- Final audited 0.6.0 package filenames and publish approval remain release-operator work after the v0.6.0 tag exists; this review relies on the contract-approved pre-tag MinVer validation evidence.

AC / test suggestions
- Keep final release validation tied to docs/manual-nuget-publication.md: build, test, pack, verify-packages, and check-format from the tagged checkout before publication.
- On any future package-validation rerun, preserve the current verifier expectation that README install strings are 0.6.0 while provider dependencies match the packed core artifact version.

Implementation watchouts
- Do not reinterpret pre-tag 0.5.1-alpha.0.69 MinVer artifacts as a requirement for final v0.6.0 filenames before the tag exists.
- Do not present a public Code-First-to-registry bridge; source and release notes explicitly keep that out of v0.6.0.

Non-blocking notes
- Read-only review did not rerun `dotnet pack` or `tools/verify-packages.sh`; the ticket now contains accepted capable-runner evidence for those commands.
- Local `artifacts/packages` currently contains the six 0.5.1-alpha.0.69 packages and matching symbols, and `git ls-files artifacts/packages` returned no tracked files.

Split recommendations
- none

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment