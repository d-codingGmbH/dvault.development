[gicket-bot] PO-critic review contract

Summary
- The ticket contract is specific, repository-grounded, and has no unresolved open questions; it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted contract in `.gicket/tickets/06EXB8202A88KJJP7WEGBESBYM/description.md:11-15,31-52` fixes the six-package scope, the exact repo-root validation path, and records `## Open Questions` as `- none`.
- `docs/manual-nuget-publication.md:7-26,28-124` already matches the contract: exactly six package ids, source-based pre-publication guidance, release-note evidence, the five repo-root validation commands, publish order, stop conditions, and final approval record requirements.
- `DVault.slnx:5-27` includes `src/DCoding.Data` plus exactly six DVault package projects and the package-verification tool; `src/DCoding.Data/DCoding.Data.csproj:2-8` marks the anchor project `IsPackable=false`, matching the ticket's out-of-scope statement.
- `README.md:7-17,21-22,158-170` distinguishes current source consumption from future NuGet guidance and repeats the same five-command local validation baseline used by the ticket contract.
- Direct source evidence backs the README quickstart API assumptions: `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-25` defines `AddDVault`, `src/DCoding.Data.DVault/DataVaultSaveService.cs:10-149` defines `IDataVaultSaveService` and `DataVaultSaveRequest`, and `src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs:6-49` defines `DataVaultMetadataModel`; provider extension methods were also found by `rg -n` in the five provider packages.
- Package verification is concrete, not prose-only: `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:15-52,70-107,186-258,261-406` enforces exactly six `.nupkg` and six `.snupkg` artifacts, expected package ids/metadata, README presence/content, XML docs, symbol PDBs, and provider-to-core dependency version alignment; `PackageVerificationCommand.cs:15-21` summarizes the pass criteria.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract treats 'absence of unintended test/helper/benchmark publication artifacts' as control over the produced package artifact set in `bin/packages/`, consistent with `README.md:170` and `PackageVerifier.cs:70-107,186-229`; implementation should not silently broaden or narrow that meaning.
- Release-note evidence location is intentionally flexible in `docs/manual-nuget-publication.md:40-53`; delivery still needs a clearly auditable record in the ticket or release approval path.

AC / test suggestions
- Keep final delivery evidence tied to the five repo-root commands plus a captured `bash tools/verify-packages.sh` pass, because those are the contract's canonical release gates.
- If implementation discovers a need to publish less than the full six-package family or to change publish order/push tooling, reopen PO refinement instead of treating that as an in-scope dev detail.

Implementation watchouts
- Maintain alignment between `docs/manual-nuget-publication.md` and `README.md` installation/validation guidance; the ticket already identifies documentation drift as a release risk.
- Do not treat `src/DCoding.Data` or the tool project in `DVault.slnx` as publishable artifacts even though they are part of the repository solution.
- Preserve the documented anti-partial-publication flow, especially the explicit approval boundary before the first push and the stop/escalation behavior after any failed push step.

Non-blocking notes
- The latest ticket comments under `.gicket/tickets/06EXB8202A88KJJP7WEGBESBYM/comments/` are lease/claim orchestration entries, not unresolved product discussions.

Split recommendations
- Keep CI automation, credentials, and package-push tooling in a separate follow-on story, as already suggested by the persisted contract.
- Keep post-publication NuGet-first install guidance and versioned package examples in a separate documentation story after the first public release.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment