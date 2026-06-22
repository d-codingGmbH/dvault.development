<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket as the provider-neutral opt-in `DCoding.Data.DVault.Privacy` skeleton: add the new multi-target package, expand coordinated pack/verify/docs surfaces, and preserve the no-mandatory-privacy-dependency boundary for existing DVault packages.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket is only the provider-neutral optional package skeleton under `src/DCoding.Data.DVault.Privacy` with package id/root namespace `DCoding.Data.DVault.Privacy`; it does not add provider-specific privacy packages in v1.
- The new package follows the runtime/provider package baseline with `net8.0;net10.0`, package metadata, symbols, README packing, and solution inclusion aligned to the existing DVault package family.
- Because `README.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, `tools/pack-release-packages.sh`, `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, and related tests currently hardcode an eight-package family, this ticket includes expanding those coordinated surfaces for the privacy package.
- The skeleton establishes only opt-in startup and dependency seams such as `AddDVaultPrivacy(...)` layered on top of `AddDVault()`; actual encryption, pseudonymization, redaction, provider-native execution, and compliance workflows stay in follow-on tickets.

### Scope In
- Create the `DCoding.Data.DVault.Privacy` project directory, csproj, namespace baseline, and solution entry.
- Multi-target `net8.0` and `net10.0` with dependency pins aligned to the existing runtime/provider package lines.
- Add the minimal public opt-in registration surface and placeholder options/abstractions needed to establish the privacy extension boundary.
- Update packaging, verification, and documentation surfaces so the optional privacy package becomes a first-class coordinated package artifact.
- Preserve current core and provider default behavior by keeping privacy references opt-in only.

### Scope Out
- No field-level encryption, decryption, pseudonymization, redaction, export filtering, or retention execution.
- No provider-specific privacy strategies, DDL, migrations, or provider-native encryption features.
- No changes to ordinary `AddDVault()`, default save/read services, PIT/bridge maintenance, or `SaveChanges` behavior for callers that do not reference the privacy package.
- No compliance guarantees, KMS/HSM ownership, or key lifecycle/workflow orchestration.
- No model-first parser, code-first metadata registration, or EF translation implementation beyond the skeleton contracts needed to compile and pack the new package.

## Acceptance Criteria
- Repository contains a new packable `src/DCoding.Data.DVault.Privacy/DCoding.Data.DVault.Privacy.csproj` project in `DVault.slnx` with package id/root namespace `DCoding.Data.DVault.Privacy`, `net8.0;net10.0`, documentation/symbol generation, root README packing, and package metadata consistent with the existing DVault runtime/provider packages.
- The privacy package references the provider-neutral core package and only the target-line dependencies it needs for an opt-in registration seam; no existing core or provider project gains a required reference back to the privacy package.
- A minimal public opt-in surface exists in the privacy package, centered on a dedicated registration extension layered after `AddDVault()` and placeholder options/contracts for future alias-driven privacy flows, without implementing automatic runtime privacy behavior.
- Coordinated packaging surfaces are updated so `bash tools/pack-release-packages.sh` and `tools/DCoding.Data.DVault.PackageVerification` include the privacy package on both package lines, with artifact expectations adjusted from 16 `.nupkg` and 14 `.snupkg` to 18 `.nupkg` and 16 `.snupkg`.
- Documentation that defines the coordinated package family and install/validation/publication guidance is updated to include the optional privacy package and to state that it is opt-in and not a compliance or provider-native encryption feature.
- Public API snapshot coverage and package-verifier coverage are updated for the new package so the package boundary is locked by tests.

## Definition of Done
- `dotnet build DVault.slnx --nologo` succeeds with the new project included.
- Relevant unit coverage for public API snapshots and package verification passes after the new package is added.
- `bash tools/pack-release-packages.sh` and `bash tools/verify-packages.sh` succeed with the privacy package in the coordinated artifact set.
- Repository documentation no longer describes the coordinated family as exactly eight packable packages where that statement would now be stale.
- Core and provider packages remain usable without referencing `DCoding.Data.DVault.Privacy`, and no mandatory privacy dependency is introduced.

## Implementation Notes
- Update `Directory.Build.props` so `DCoding.Data.DVault.Privacy` is treated as a packable package with the same MinVer flow as the other runtime/provider packages.
- Add the new project to `DVault.slnx` under `/src/` and mirror the csproj conventions already used by `src/DCoding.Data.DVault` and the provider packages.
- Extend `tools/pack-release-packages.sh`, `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` together; they currently enumerate the package family explicitly.
- Extend `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs` and add an approved snapshot file for the new public package surface.
- Update `README.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, and any package-family compatibility/adoption docs that currently state DVault has eight packable packages.
- Keep the initial public surface intentionally thin: registration, options, and seams only, with naming and comments aligned to `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` and `docs/plans/dvault-model-v1-schema-contract.md`.

## Open Questions
- none

## Follow-Up Questions
- Which concrete follow-on capability should consume the skeleton first: code-first or registry privacy metadata APIs, model-first parser consumption of `personalData`, or provider-neutral encrypted payload mapping?
- When provider-specific privacy optimizations are approved later, should they extend the existing provider packages or ship as separate provider-specific privacy packages?
- Should the first non-skeleton privacy capability prefer explicit helpers only, or add a provider-neutral value-conversion proof as the earliest execution lane?

## Risks
- Packaging and publication surfaces currently hardcode an eight-package family; missing any of those coordinated updates will break pack/verify automation or leave release guidance inconsistent.
- The live relation graph still shows incoming `blocks` relations from `06FE4R9ZC210EE5AW4WCWQN32G` and `06FE4RA88AV7ZRRPMDS8YADEX4`, so downstream implementation may still depend on upstream privacy-metadata tickets even after the skeleton is refined.
- The live relation graph shows this ticket blocking `06FE4RASEQZN7XEYH1XR4H06PR` and `06FE4RB219AXVF2535MFF36PN4`, so over-designing the skeleton API here would create avoidable churn for dependent tickets.

## Split Recommendations
- No split recommended; the new project, coordinated pack/verify updates, and package-family documentation changes are one bounded change set for the privacy package skeleton.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: create the optional privacy package structure, target frameworks, package metadata, and dependency boundaries. Acceptance: core DVault packages do not gain mandatory privacy dependencies.