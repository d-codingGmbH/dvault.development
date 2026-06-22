[gicket-bot] PO refinement contract

Summary
- Refined the ticket as the provider-neutral opt-in `DCoding.Data.DVault.Privacy` skeleton: add the new multi-target package, expand coordinated pack/verify/docs surfaces, and preserve the no-mandatory-privacy-dependency boundary for existing DVault packages.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket is only the provider-neutral optional package skeleton under `src/DCoding.Data.DVault.Privacy` with package id/root namespace `DCoding.Data.DVault.Privacy`; it does not add provider-specific privacy packages in v1.
- The new package follows the runtime/provider package baseline with `net8.0;net10.0`, package metadata, symbols, README packing, and solution inclusion aligned to the existing DVault package family.
- Because `README.md`, `docs/local-validation.md`, `docs/manual-nuget-publication.md`, `tools/pack-release-packages.sh`, `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, and related tests currently hardcode an eight-package family, this ticket includes expanding those coordinated surfaces for the privacy package.
- The skeleton establishes only opt-in startup and dependency seams such as `AddDVaultPrivacy(...)` layered on top of `AddDVault()`; actual encryption, pseudonymization, redaction, provider-native execution, and compliance workflows stay in follow-on tickets.

Scope In
- Create the `DCoding.Data.DVault.Privacy` project directory, csproj, namespace baseline, and solution entry.
- Multi-target `net8.0` and `net10.0` with dependency pins aligned to the existing runtime/provider package lines.
- Add the minimal public opt-in registration surface and placeholder options/abstractions needed to establish the privacy extension boundary.
- Update packaging, verification, and documentation surfaces so the optional privacy package becomes a first-class coordinated package artifact.
- Preserve current core and provider default behavior by keeping privacy references opt-in only.

Scope Out
- No field-level encryption, decryption, pseudonymization, redaction, export filtering, or retention execution.
- No provider-specific privacy strategies, DDL, migrations, or provider-native encryption features.
- No changes to ordinary `AddDVault()`, default save/read services, PIT/bridge maintenance, or `SaveChanges` behavior for callers that do not reference the privacy package.
- No compliance guarantees, KMS/HSM ownership, or key lifecycle/workflow orchestration.
- No model-first parser, code-first metadata registration, or EF translation implementation beyond the skeleton contracts needed to compile and pack the new package.

Open questions
- none

Follow-up questions
- Which concrete follow-on capability should consume the skeleton first: <redacted> or registry privacy metadata APIs, model-first parser consumption of `personalData`, or provider-neutral encrypted payload mapping?
- When provider-specific privacy optimizations are approved later, should they extend the existing provider packages or ship as separate provider-specific privacy packages?
- Should the first non-skeleton privacy capability prefer explicit helpers only, or add a provider-neutral value-conversion proof as the earliest execution lane?

Risks
- Packaging and publication surfaces currently hardcode an eight-package family; missing any of those coordinated updates will break pack/verify automation or leave release guidance inconsistent.
- The live relation graph still shows incoming `blocks` relations from `06FE4R9ZC210EE5AW4WCWQN32G` and `06FE4RA88AV7ZRRPMDS8YADEX4`, so downstream implementation may still depend on upstream privacy-metadata tickets even after the skeleton is refined.
- The live relation graph shows this ticket blocking `06FE4RASEQZN7XEYH1XR4H06PR` and `06FE4RB219AXVF2535MFF36PN4`, so over-designing the skeleton API here would create avoidable churn for dependent tickets.

Split recommendations
- No split recommended; the new project, coordinated pack/verify updates, and package-family documentation changes are one bounded change set for the privacy package skeleton.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment