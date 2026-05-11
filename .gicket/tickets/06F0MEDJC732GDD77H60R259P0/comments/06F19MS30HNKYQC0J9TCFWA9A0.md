[gicket-bot] PO-critic review contract

Summary
- Return to PO: the documentation scope is source-backed and Open Questions is none, but the current ticket is still not safe for the configured PO-critic success path because approval routes to generic dev while the contract allows dev only with an explicit capable-runner guarantee and otherwise requires release-validation.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md:7-9 records PO Handoff decision ready_for_po_critic; lines 49-50 record Open Questions as none.
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md:12-15 says the product decision is capable-runner routing, dev is preferred only if explicitly network/cache-enabled and mutable, release-validation is fallback, and tester remains blocked until both package validation commands pass.
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md:20-21 and 32-34 require recorded successful dotnet pack DVault.slnx --configuration Release --nologo and bash tools/verify-packages.sh evidence.
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/comments/06F19K2W9JMBCNAY0JWA3Z4F7W.md:12-15 answers the latest PO refinement: tester remains blocked, dev is allowed only with a network/cache-enabled mutable runner, otherwise route to release-validation, and normal dev success path must not be used without that guarantee.
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/comments/06F19G8EGAF6Q5SQB3PWKZDBKW.md:17-18 records prior restore-dependent validation failed because NuGet network access was denied and the local package cache was incomplete; lines 24-25 say this run cannot produce pass evidence and direct tester return would repeat the blocker.
- A targeted git diff --name-only over README.md, docs/releases/v0.6.0.md, docs/manual-nuget-publication.md, tools/verify-packages.sh, DVault.slnx, and the ticket description/json produced no output.
- README.md:10-15 documents all six v0.6.0 package install commands; README.md:24 and 50-77 show the Code-First happy path; README.md:352-364 lists local validation including dotnet pack and bash tools/verify-packages.sh and describes the verifier.
- docs/releases/v0.6.0.md:8-17 documents the six-package v0.6.0 scope; lines 21-28 summarize Code-First, registry, typed reads, diagnostics, and quickstarts; lines 51-61 keep pack and verify-packages as required validation before final publication evidence.
- docs/manual-nuget-publication.md:55-67 requires build, test, pack, verify-packages, and check-format evidence; lines 71-77 define verify-packages as the six-package dependency-alignment gate.
- tools/verify-packages.sh:7-9 runs tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj.
- Source evidence backs the documented APIs: DataVaultCodeFirstModelBuilderExtensions.cs:16-30 exposes ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>), DataVaultCodeFirstModelBuilder.cs:23-55 exposes Hub and Link, DataVaultCodeFirstHubBuilder.cs:25-55 exposes BusinessKey and Satellite, DataVaultCodeFirstSatelliteBuilder.cs:22-45 exposes DrivingKey and Payload, DataVaultCodeFirstLinkBuilder.cs:18-23 exposes Participant<TEntity>(), IDataVaultReadService.cs:16-19 exposes ReadLatestSatelliteRowsAsync, DataVaultReadServiceTypedProjectionExtensions.cs:48-53 exposes typed ReadLatestSatelliteAsync, and DataVaultDbContextOptionsBuilderExtensions.cs:16-60 exposes UseDataVaultMetadata overloads.

Blocking findings
- The persisted contract is still conditional on runner capability, but this PO-critic run only has configured success -> dev and failure -> po; there is no observed ticket field, label, assignee, or runtime guarantee that success will assign a network/cache-enabled mutable dev runner.
- Approving for dev through the current success path would risk violating description.md:13-15 and 44-45, because the ticket itself says normal dev is acceptable only with an explicit capable-runner guarantee and otherwise requires release-validation with a complete NuGet cache.
- Required package-validation pass evidence is still absent from the ticket history; searches found requests and failures, not recorded successful dotnet pack plus verify-packages output.

Required PO actions
- Add or obtain an explicit ticket-level routing/assignment guarantee that PO-critic success will land on a network/cache-enabled mutable dev runner, or route the ticket to release-validation with a complete NuGet cache before requesting PO-critic approval again.
- Do not request repository edits merely to work around sandbox network/cache restrictions.

Open issues ledger
- critic-item-1 [required-po-action] Add or obtain an explicit ticket-level routing/assignment guarantee that PO-critic success will land on a network/cache-enabled mutable dev runner, or route the ticket to release-validation with a complete NuGet cache before requesting PO-critic approval again.
- critic-item-2 [required-po-action] Do not request repository edits merely to work around sandbox network/cache restrictions.
- critic-item-3 [blocking-finding] The persisted contract is still conditional on runner capability, but this PO-critic run only has configured success -> dev and failure -> po; there is no observed ticket field, label, assignee, or runtime guarantee that success will assign a network/cache-enabled mutable dev runner.
- critic-item-4 [blocking-finding] Approving for dev through the current success path would risk violating description.md:13-15 and 44-45, because the ticket itself says normal dev is acceptable only with an explicit capable-runner guarantee and otherwise requires release-validation with a complete NuGet cache.
- critic-item-5 [blocking-finding] Required package-validation pass evidence is still absent from the ticket history; searches found requests and failures, not recorded successful dotnet pack plus verify-packages output.

Missing examples / edge cases
- No missing documentation example was found at PO level; the missing artifact is executable package-validation pass evidence.
- If capable-runner validation fails for repository-content reasons, the failing command output must be captured so the follow-up is concrete.

Risky assumptions
- Assuming generic dev handoff will automatically use a capable runner is risky because prior dev evidence showed network-restricted/cache-incomplete execution.
- Assuming release-validation will happen is not enough unless ticket metadata/routing actually sends the ticket there.
- Treating the current or prior network-restricted/cache-incomplete failures as pass evidence would violate the persisted contract.

AC / test suggestions
- Record the capable lane name, checkout/commit, and successful dotnet pack DVault.slnx --configuration Release --nologo output.
- Record successful bash tools/verify-packages.sh output against the same checkout/package artifacts.
- If either command fails outside an environmental/cache limitation, attach the full failing output and route a concrete packaging follow-up before tester handoff.

Implementation watchouts
- Run dotnet pack before bash tools/verify-packages.sh so artifacts/packages exists and matches verifier expectations.
- Do not edit README.md or docs/releases/v0.6.0.md unless capable-runner validation exposes a real repository/docs issue.
- The current dirty worktree contains unrelated .gicket/.gitignore, .gicket/project.json, .gicket/types.json, and .gicket-bot/.gitignore modifications; targeted diff showed no changes in the docs, validation script, solution, or ticket contract files reviewed.

Non-blocking notes
- The documentation surface itself appears aligned with the v0.6.0 scope and backed by direct source evidence for the public APIs referenced.
- Open Questions is none, so the return is not for unresolved PO questions; it is for the remaining routing/capable-runner enforceability gap.

Split recommendations
- No split recommended now. Split only if capable-runner output proves a real non-environmental packaging defect that needs separate remediation.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment