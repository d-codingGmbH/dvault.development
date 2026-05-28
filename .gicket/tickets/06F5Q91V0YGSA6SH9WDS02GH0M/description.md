<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the repository and persisted ticket state, then aligned the epic contract around the shipped v0.22 satellite-only typed-read boundary and queued planning-document supersession for the stale PIT/bridge helper plan surfaces.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Epic ticket 06F5Q91V0YGSA6SH9WDS02GH0M revision 06F6XNSSG7AXMGFMWFBDDH7GF8 now treats docs/releases/v0.22.0.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/model-first-governance.md, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs, and docs/plans/stable-hashing-contract.md as the authoritative v0.22 contract surface.
- Done child 06F5Q922T5B21GJN49FYN6DJH0 and the older docs/plans/typed-read-model-generator-contract.md text are explicitly historical design context for this epic, not the shipped v0.22 boundary.
- Queued planning-document mutations mutation-baa7edf5136439f2 and mutation-67556c67217d884c are recorded in the epic contract to rewrite docs/plans/typed-read-model-generator-contract.md and docs/plans/README.md on develop so PIT and bridge helper promises stop presenting as current v0.22 contract text.
- No new child tickets, attachments, or relation writes were needed; the existing seven parentOf relations remain the authoritative decomposition.

### Scope In
- Opt-in typed read-model generation from exactly one authoritative dvault.support-bundle.v1 artifact after metadata projection into EF and DVault metadata.
- Typed Current, Latest, and AsOf satellite helper generation for supported hub-parent, link-parent, and deterministic multi-active satellite shapes over existing IDataVaultReadService reads.
- Explicit DMV1960 through DMV1969 diagnostics for missing, stale, ambiguous, unsupported, colliding, or skipped generated-helper cases.
- Stable hash canonicalization governance, published compatibility vectors, and regression coverage for sha256-v1.
- Documentation and planning-surface alignment for the shipped v0.22 typed-read and hash-governance boundary, including explicit supersession of earlier PIT and bridge helper planning promises.

### Scope Out
- Raw dvault.model.v1 additional-file parsing by the typed read-model generator.
- PIT or bridge typed helper emission in the shipped v0.22 boundary.
- Provider-specific SQL generation, dynamic request compilation, or automatic support-bundle routing or publication.
- Runtime boundary expansion beyond existing IDataVaultReadService surfaces and documented consumer-owned compiled EF query alternatives.
- Automatic satellite hashDiff generation, binary scalar hashing changes, or any unversioned change to sha256-v1 semantics.

## Acceptance Criteria
- The epic is only satisfied if typed read-model generation is opt-in, consumes exactly one authoritative dvault.support-bundle.v1 input, and keeps metadata-source fingerprint validation explicit.
- Supported generated helpers remain limited to stable satellite shapes and emit typed Current, Latest, and AsOf helpers over the existing IDataVaultReadService boundary.
- PIT, bridge, dynamic, provider-specific, or otherwise out-of-contract shapes surface through documented DMV196x diagnostics or existing runtime read surfaces rather than generated helpers.
- Dynamic IDataVaultReadService requests remain the default runtime-built path, and consumer-owned compiled EF queries remain the documented stable direct-query alternative for fixed shapes.
- The hash-governance boundary stays documented and test-backed through docs/plans/stable-hashing-contract.md and the stable-hash tests.
- Reviewers do not need to infer that older PIT or bridge helper planning text is historical: the epic contract and queued planning-document supersession explicitly mark docs/plans/typed-read-model-generator-contract.md and 06F5Q922T5B21GJN49FYN6DJH0 as non-authoritative for the shipped v0.22 boundary.

## Definition of Done
- The existing seven-child relation set remains the authoritative decomposition for this epic, and each child ticket is done without a remaining PO blocker on the parent.
- Repository docs, analyzer evidence, generator tests, and the epic handoff text all describe the same support-bundle-driven satellite-only helper boundary with PIT and bridge left to runtime or diagnostic surfaces.
- Queued replay rewrites docs/plans/typed-read-model-generator-contract.md and docs/plans/README.md so they no longer present PIT and bridge helper promises as the current v0.22 contract.
- Stable hash canonicalization and compatibility vectors remain published in docs/plans/stable-hashing-contract.md and covered by unit tests without unversioned semantic drift.
- No blocking PO questions remain about generated-helper scope, hash-governance scope, or excluded runtime behavior for this epic.

## Implementation Notes
- Treat docs/releases/v0.22.0.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/model-first-governance.md, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs, and docs/plans/stable-hashing-contract.md as the authoritative v0.22 baseline.
- Treat done child 06F5Q922T5B21GJN49FYN6DJH0 and the pre-v0.22 docs/plans/typed-read-model-generator-contract.md wording as historical design context that is explicitly superseded for this shipped boundary.
- 06F5Q92AHG0ZCTVQGC6NAYVP9C remains the landed satellite-helper implementation; 06F5Q92YGB53W7YG6VCMA3FZJR remains the residual diagnostic or code-fix follow-up; 06F5Q934MSKVCQAHPCWEM29CZW and 06F5Q93AVHRYJBAPJCJEB4N7KG remain the hash-governance slice; 06F5Q93H60W6X8FJ88PWTR6NG4 remains the docs rollup.
- 06F5Q92R02HB7FCE1AWKXPTMRW stays done with closure or no-work-required, and PIT or bridge shapes remain runtime or diagnostic territory rather than shipped typed-helper scope.
- Persistent planning work in this refinement pass was the applied epic description update plus queued planning-document mutations mutation-baa7edf5136439f2 and mutation-67556c67217d884c; no attachment, relation, or child-ticket write was needed.

## Open Questions
- none

## Follow-Up Questions
- If the product later wants shipped PIT or bridge typed helpers instead of the current runtime or diagnostic-only handling, should that land as a new additive follow-up rather than reopening this epic?
- If automatic satellite hashDiff generation or binary scalar hashing becomes a requirement later, should that ship under a separately versioned contract instead of changing sha256-v1 behavior?

## Risks
- Until the queued develop-branch replay lands, readers of docs/plans/typed-read-model-generator-contract.md or docs/plans/README.md on develop can still encounter the older PIT or bridge helper wording that this epic now explicitly supersedes.
- Future docs or implementation work could overstate the shipped typed-read boundary by implying PIT or bridge helper emission before a separate additive ticket lands.
- Any unversioned change to the sha256-v1 canonicalization rules or published vectors would break the compatibility contract this epic establishes.
- If DMV196x unsupported-shape behavior regresses, consumers may no longer distinguish unsupported metadata from misconfiguration, which would blur the current satellite-only boundary.

## Split Recommendations
- No additional split is recommended now; the existing seven-child decomposition is already persisted and complete for this epic.
- If future work expands into shipped PIT or bridge helpers, automatic hashDiff generation, or new hash encodings, create additive follow-up tickets instead of reopening this parent epic.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Improve compile-time ergonomics and hash compatibility confidence without hiding Data Vault semantics.

Acceptance criteria:
- Generates typed read helpers only for stable metadata-defined read shapes.
- Adds hash canonicalization governance and compatibility vectors.
- Keeps dynamic IDataVaultReadService requests as the default runtime-built path.