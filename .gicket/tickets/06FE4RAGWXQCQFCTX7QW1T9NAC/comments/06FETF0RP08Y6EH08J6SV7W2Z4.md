[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the delivery contract is concrete, the open-questions section is resolved, and direct repository/ticket evidence matches a bounded pre-development package-skeleton task.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4RAGWXQCQFCTX7QW1T9NAC/description.md contains a full delivery contract with explicit scope in/out, 6 acceptance criteria, 5 definition-of-done items, and '## Open Questions' set to 'none'.
- git -C /mnt/c/Projects/DVault log --oneline --decorate -n 5 shows HEAD 0679c2a62 is the po-critic lease-claim commit and the prior substantive ticket commit is 1bd838a9f ('handoff po->po-critic').
- git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD lists only .gicket/tickets/06FE4RAGWXQCQFCTX7QW1T9NAC/** files, so the branch is still metadata-only and this is a normal pre-development critic gate rather than an implementation review.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs directly defines the existing AddDVault() entry point that the contract says the new AddDVaultPrivacy(...) seam must layer on top of.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md explicitly requires the privacy add-on to be opt-in, layered on AddDVault(), alias-driven, and not a default runtime behavior change.
- docs/plans/dvault-model-v1-schema-contract.md already defines personalData[].encryptedPayloadAlias as the provider-neutral lookup key for future privacy work, which grounds the placeholder options/contracts called for by this ticket.
- Directory.Build.props, DVault.slnx, tools/pack-release-packages.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerificationCommand.cs, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs all currently hardcode the existing eight-package / 16 .nupkg / 14 .snupkg baseline that this ticket is meant to expand.
- ls /mnt/c/Projects/DVault/src shows no src/DCoding.Data.DVault.Privacy directory yet, and DVault.slnx has no privacy project entry yet, which matches the intended pre-development scope.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The install-guidance update should make it explicit whether consumers add DCoding.Data.DVault.Privacy alongside DCoding.Data.DVault for both 8.43.0 and 10.43.0 package lines, not instead of the core package.
- The acceptance/test lane should make the opt-in and non-compliance wording observable enough that README/package-verifier updates cannot silently omit it.

Risky assumptions
- Assumes the stale-doc cleanup is aimed at current package-family guidance surfaces, not historical release notes that intentionally describe older eight-package baselines unless those notes are still referenced as current installation guidance.
- Assumes the initial skeleton can stay provider-neutral and dependency-light, with no provider package changes beyond coordinated packaging/docs/test surfaces, until later privacy tickets consume encryptedPayloadAlias-driven seams.

AC / test suggestions
- Add a new public API snapshot and approved snapshot file for DCoding.Data.DVault.Privacy beside the existing package snapshots in tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/.
- Extend package-verifier coverage to assert the new artifact counts and that the packed root README mentions the optional privacy package for both package lines.
- Add a small registration smoke test proving services.AddDVault().AddDVaultPrivacy(...) composes without changing ordinary AddDVault() behavior when the privacy package is not referenced by callers.

Implementation watchouts
- The eight-package assumption is duplicated in more places than the three docs named in the contract; current guidance also appears in README.md, docs/production-adoption-checklist.md, docs/plans/shared-implementation-standards.md, and current release-baseline docs, so the stale-doc search needs to be repo-wide but focused on current guidance.
- Directory.Build.props, DVault.slnx, tools/pack-release-packages.sh, PackageVerificationCommand.cs, PackageVerifier.cs, PackageVerifierTests.cs, and ApiSurfaceSnapshotTests.cs all encode today's package family; missing any one of them will leave the package line inconsistent.
- Keep the new surface intentionally thin: the architecture contract explicitly forbids default SaveChanges behavior changes, provider-name branching in core, silent plaintext fallback, and any implied compliance guarantee.

Non-blocking notes
- Current branch evidence is metadata-only, which is consistent with a pre-development PO-critic handoff.
- The persisted contract already says no split is recommended, and the repository evidence supports that as one bounded skeleton/package-family coordination change.

Split recommendations
- No split recommended; the new project, coordinated pack/verify updates, and current package-family guidance updates remain one bounded developer handoff.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment