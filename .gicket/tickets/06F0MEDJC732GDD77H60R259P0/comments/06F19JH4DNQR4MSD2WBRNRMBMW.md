[gicket-bot] PO-critic review contract

Summary
- Return to PO: the docs contract is source-backed and Open Questions is none, but the current handoff is not safely actionable because approval would route to dev while the persisted contract allows dev only on a network/cache-enabled mutable runner and otherwise requires release-validation with a complete NuGet cache.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md:7-9 records PO Handoff decision ready_for_po_critic; lines 41-42 record Open Questions as none.
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md:12-19 and 31-38 require dotnet pack and bash tools/verify-packages.sh pass evidence from a network/cache-enabled or complete-cache runner, and say resume to dev only under a capable runner.
- Comment 06F19G8EGAF6Q5SQB3PWKZDBKW.md:17-18 says the prior dev run failed restore-dependent validation because NuGet network access was denied and the local cache was incomplete; lines 24-25 warn direct tester return would repeat the package-verification blocker.
- Comment 06F19GPDR6STDAQTQ770KD4N1C.md:12 answers the routing question: use the first capable lane, prefer capable dev, otherwise release-validation with a complete NuGet cache; lines 36-39 restate that repeating the network-restricted/cache-incomplete sandbox will not satisfy the contract.
- git log --oneline -n 8 shows the latest sequence a7e88a54 lease claim po-critic after 70da1111 handoff po->po-critic, 8fc0fa89 handoff dev->po, and prior po-critic->dev handoff.
- git diff --name-only over README.md, docs/releases/v0.6.0.md, docs/manual-nuget-publication.md, tools/verify-packages.sh, DVault.slnx, and the ticket files produced no output in the current worktree.
- README.md:10-15 documents all six v0.6.0 package install commands; README.md:24 and 50-68 show the Code-First happy path; README.md:210 and 301-303 preserve metadata-first compatibility; README.md:355-359 lists build/test/pack/verify/check-format.
- docs/releases/v0.6.0.md:8-17 documents the six-package v0.6.0 scope; lines 21-28 summarize Code-First, registry, typed reads, diagnostics, and quickstarts; lines 51-61 keep pack and verify-packages as required validation before final publication evidence.
- docs/manual-nuget-publication.md:57-65 requires dotnet build, test, pack, verify-packages, and check-format; lines 71-77 define verify-packages as the six-package dependency-alignment gate.
- tools/verify-packages.sh:7-9 runs tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj.
- Source evidence backs the documented public APIs: DataVaultModelBuilderExtensions.cs:95-104 exposes ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>), DataVaultCodeFirstModelBuilder.cs:23-55 exposes Hub and Link, DataVaultCodeFirstHubBuilder.cs:25-55 exposes BusinessKey and Satellite, DataVaultCodeFirstSatelliteBuilder.cs:22-45 exposes DrivingKey and Payload, and DataVaultCodeFirstLinkBuilder.cs:18-23 exposes Participant<TEntity>().

Blocking findings
- The ticket is not ready for unconditional developer handoff through the current PO-critic success path. The contract permits dev only if the next dev runner is network/cache-enabled and mutable, but the latest dev evidence shows the runner was network-restricted and cache-incomplete, and the current ticket metadata does not encode a capable-runner guarantee.
- The PO contract also names release-validation as the fallback lane, but the current PO-critic role path provided to this run has success -> dev and failure -> po. Without a ticket-level routing change or explicit capable-dev assignment, approving would likely repeat the known failed loop.

Required PO actions
- Keep the ticket out of tester until successful dotnet pack DVault.slnx --configuration Release --nologo and bash tools/verify-packages.sh output is recorded from the capable lane.
- Do not request docs, product-code, package metadata, provider behavior, or release automation edits merely to work around the sandbox limitation.

Open issues ledger
- critic-item-1 [required-po-action] Keep the ticket out of tester until successful dotnet pack DVault.slnx --configuration Release --nologo and bash tools/verify-packages.sh output is recorded from the capable lane.
- critic-item-2 [required-po-action] Do not request docs, product-code, package metadata, provider behavior, or release automation edits merely to work around the sandbox limitation.
- critic-item-3 [blocking-finding] The ticket is not ready for unconditional developer handoff through the current PO-critic success path. The contract permits dev only if the next dev runner is network/cache-enabled and mutable, but the latest dev evidence shows the runner was network-restricted and cache-incomplete, and the current ticket metadata does not encode a capable-runner guarantee.
- critic-item-4 [blocking-finding] The PO contract also names release-validation as the fallback lane, but the current PO-critic role path provided to this run has success -> dev and failure -> po. Without a ticket-level routing change or explicit capable-dev assignment, approving would likely repeat the known failed loop.

Missing examples / edge cases
- No PO-level missing documentation example was found; the remaining missing artifact is executable package-validation pass evidence.
- If capable-runner package verification fails for repository-content reasons, the ticket needs the exact failing command and output so the follow-up is concrete rather than another routing clarification.

Risky assumptions
- Assuming a normal dev handoff will automatically land on a capable runner is risky because the immediately preceding dev run did not.
- Assuming release-validation is available is not enough unless the ticket metadata/routing actually sends this ticket there.
- Treating the current no-network/cache-incomplete restore failure as package-validation evidence would violate the persisted contract.

AC / test suggestions
- Record the capable lane used, checkout/commit, and successful output for dotnet pack DVault.slnx --configuration Release --nologo.
- Record successful output for bash tools/verify-packages.sh against the same checkout/package artifacts.
- If either command fails outside an environmental/cache limitation, attach the full command output and route a concrete packaging follow-up.

Implementation watchouts
- Run pack before verify-packages so artifacts/packages exists and matches the package verifier expectations.
- Do not send to tester before both package-validation commands pass.
- Do not edit README.md or docs/releases/v0.6.0.md unless capable-runner validation exposes a real repository/docs issue.

Non-blocking notes
- The documentation surface itself appears aligned with the v0.6.0 scope and backed by source evidence for the public APIs referenced.
- The upstream tickets that block this docs ticket are done; the return is about routing/capable-runner enforcement, not upstream feature availability or unresolved Open Questions.

Split recommendations
- No split recommended now; split only if capable-runner output proves a real non-environmental packaging defect that needs separate remediation.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment