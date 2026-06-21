<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Re-routed ticket 06FE4R2EGQ444EGPKZBRZCDEV8 from an invalid closure-only path back to a normal pre-development docs task for dev; repository evidence still shows missing v0.43 deliverables, and no child tickets, relation changes, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This is a normal pre-development documentation task for `dev`, not a closure-only or no-work-required ticket.
- Current repository evidence still exposes the public docs baseline as v0.42.0 and lacks `docs/releases/v0.43.0.md`, so the v0.43 docs story must be implemented rather than merely audited.
- Public DVault hash-key values remain lowercase hexadecimal strings even when physical storage is `Binary`; post-persistence storage or algorithm changes remain caller-owned migration work routed through `docs/hash-key-storage-migration.md` and the dry-run manifest workflow.
- Analyzer guidance remains bounded to local `PrivateAssets='all'` usage, one `net10.0` analyzer asset, and a `.NET 10 SDK` build-host baseline; do not broaden the claim to pure `.NET 8 SDK` analyzer-host compatibility.
- This refinement keeps the scoped docs contract intact and only corrects workflow interpretation; no child tickets, relation changes, attachments, or planning documents were materialized.

### Scope In
- Add `docs/releases/v0.43.0.md` summarizing binary adoption guidance, analyzer DX, provider binary-vs-hex evidence, and allocation evidence with explicit caveats and non-goal boundaries.
- Add a matching `CHANGELOG.md` v0.43.0 entry aligned with the v0.43 release note.
- Update current-baseline docs that still point at v0.42.0 so they present the v0.43 binary-first, analyzer, performance, and release-versus-package-line story consistently on touched surfaces.
- Update performance and adoption guidance to cite the checked-in hash-key matrix bundle, footprint sidecars, hotspot artifacts, refreshed allocation evidence, and migration guide by their actual labels and measured boundaries.
- Keep analyzer docs aligned with the existing project-local diagnostics scope, supported diagnostic ranges, `PrivateAssets='all'`, and `.NET 10 SDK` build-host guidance without widening analyzer behavior or compatibility claims.

### Scope Out
- Runtime, analyzer, benchmark-harness, or provider implementation changes.
- New benchmark reruns, provider setup work, or artifact-schema redesign.
- Automatic migration, rehash, backfill, dual-write, repair, or public `byte[]` hash-key behavior.
- New provider-wide timing claims derived from skipped, failed, diagnostics-only, smoke-only, or storage-footprint rows.
- Package publication approval, signed NuGet push, or release automation outcomes.

## Acceptance Criteria
- `docs/releases/v0.43.0.md` and a matching `CHANGELOG.md` v0.43.0 entry are added and cite the already-landed binary adoption, analyzer, provider matrix, and allocation evidence with explicit caveats and non-goals.
- Touched current-baseline install, release, and adoption docs remove the v0.42-only framing on those surfaces and present one consistent v0.43 documentation story without documenting a consumer-facing `0.43.0` package version or mixed-line install guidance.
- Binary-first guidance says new projects should opt into the binary-first profile, existing persisted `HexString` setups remain compatible until separately migrated, and the migration guide plus dry-run manifest lane are the required path for post-persistence storage changes.
- Performance guidance cites the provider binary-vs-hex bundle, footprint sidecars, hotspot baseline, and refreshed allocation artifacts by their actual labels and preserves completed, skipped, failed, diagnostics-only, and storage-footprint boundaries.
- Allocation docs keep the bounded hotspot story intact: measured DVault-owned surfaces are save preparation, latest-hash-diff replay filtering, stable-hash canonicalization, and digest generation, while caller-owned `HashDiff` generation and database write/setup work remain outside the profiled boundary.
- Analyzer docs stay bounded to project-local, source-visible tooling guidance and do not imply runtime guards, provider-specific lifecycle guarantees, whole-application inference, or pure `.NET 8 SDK` analyzer-host support.

## Definition of Done
- A v0.43 release note exists, the changelog is updated, and the touched adopter, performance, analyzer, and install surfaces tell one consistent v0.43 docs story.
- Touched docs route new-project readers to the binary-first setup path and existing persisted-model readers to the migration guide and dry-run manifest path without implying automatic migration, rehash, backfill, dual-write, or repair.
- Touched docs cite the landed benchmark artifacts and keep machine-specific timing and allocation claims attached to their preserved run context instead of restating them as universal promises.
- Touched docs keep the analyzer package local-tooling boundary, `PrivateAssets='all'` posture, and `.NET 10 SDK` build-host baseline aligned across release-facing guidance.
- The ticket can be closed only after the repository actually contains the new release-note and changelog docs work; this refinement alone is not closure evidence.

## Implementation Notes
- Use done story `06FE4R089MT3BYRCVH7Q4EX6CG` and downstream done tickets as the scope boundary; this ticket is the final docs-consolidation lane downstream of landed evidence, not a new product-definition step.
- Likely touched docs are `README.md`, `docs/getting-started.md`, `docs/production-adoption-checklist.md`, `docs/performance-profiles.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/package-compatibility.md`, `docs/manual-nuget-publication.md`, `docs/local-validation.md`, `CHANGELOG.md`, and the new `docs/releases/v0.43.0.md`.
- For binary-storage evidence, use `artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/` with `hash-key-footprint.*`; keep like-for-like algorithm comparisons separate from shortened-digest comparisons and keep failed or skipped rows as caveats, not successes.
- For allocation evidence, use `artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/` and `artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-20260621/`; treat the before-and-after pair as the comparative source and keep additive `allocation-hotspots.*` sidecars secondary to the standard benchmark triplet.
- Keep the measured allocation story concrete and bounded: dominant ranked work is `DefaultDataVaultSaveService.AddSatellitesAsync`, then `FilterSatellitePlansAsync` and `LoadLatestSatelliteHashDiffsAsync`, then `DefaultStableHashNormalizer.NormalizeFields`, then `BuiltInStableHashService.ComputeHash`; do not turn that ranking into a universal promise.
- This refinement pass created no child tickets, relation edits, attachments, or planning documents.

## Open Questions
- none

## Follow-Up Questions
- After v0.43 docs land, do any provider-specific binary-storage caveats warrant separate post-v0.43 adopter guidance instead of one shared baseline note?
- Should a later release promote `--allocation-hotspots` from an opt-in benchmark lane to a standard release-validation companion artifact, or should it remain a focused diagnostics tool?
- If future evidence proves pure `.NET 8 SDK` analyzer consumption, should that be handled as a separate compatibility ticket instead of broadening the current analyzer claim retrospectively?

## Risks
- Docs can overstate binary-storage wins or allocation reductions if they summarize skipped, failed, diagnostics-only, smoke-only, or storage-footprint rows as general results.
- Docs can regress product clarity if they present binary-first as an automatic migration path or imply a public `byte[]` hash-key model.
- Release-facing guidance can drift if README, release notes, package compatibility, analyzer install guidance, validation guidance, and adoption docs are not updated coherently on the same current-baseline story.
- Because the branch currently lacks documentation implementation beyond ticket metadata, closure evidence still depends on dev landing the repository docs changes.

## Split Recommendations
- No split is needed; the remaining work is already a bounded v0.43 docs-consolidation lane for release notes, baseline docs, analyzer guidance, and performance evidence citations.
- If later evidence supports materially different provider-specific binary-storage guidance, capture that in a separate post-v0.43 documentation ticket instead of widening this shared baseline update.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: update binary adoption docs, analyzer docs, performance profiles, and release notes. Acceptance: docs tie v0.43 changes to measured evidence and migration non-goals.