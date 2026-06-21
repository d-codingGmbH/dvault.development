<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around the existing hash-key benchmark harness: land provider-configured binary-vs-hex evidence with the bounded four-variant matrix, promote only benchmark-backed timing claims, and update performance/evidence docs to call out wins, neutral cases, and caveats without changing storage-profile behavior.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use the existing bounded hash-key matrix variants already defined in the repo: `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, and `sha256-128-v1-binary`.
- Reuse the existing benchmark artifact contract: `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, plus same-label `hash-key-footprint.md`, `hash-key-footprint.csv`, and `hash-key-footprint.json` sidecars.
- Treat the matrix as variant-driven evidence on existing benchmark scenarios, not as a new scenario family or a new artifact schema.
- Treat provider-configured completed rows as measured timing evidence and skipped rows as placeholders only; keep footprint-only facts separate from timing claims.
- For this ticket, the provider comparison boundary follows the current runner and artifact contract: required SQLite local baseline plus optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 lanes.
- Binary-vs-hex conclusions must compare like-for-like algorithm pairs first; shortened-digest variants are a separate dimension and cannot be described as pure binary-storage wins.

### Scope In
- Checked-in provider-configured hash-key matrix evidence using the existing bounded four-variant harness and artifact contract.
- SQLite required-baseline rows plus optional-provider comparison rows for the benchmark families the current runner already emits: `provider-native-bulk-ingestion`, `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read`, including retained PostgreSQL direct/UNNEST and MySQL multi-row rows where applicable.
- Canonical evidence-surface and adopter-facing documentation updates that summarize measured wins, neutral or regressive cases, and provider-specific caveats by `scenario`, `provider`, `baseline`, and `posture`.
- Footprint sidecars and supporting documentation that explain provider store types and payload-byte differences while preserving the lowercase-hex logical boundary.

### Scope Out
- New stable-hash algorithms, extra variant combinations, or a second benchmark output schema.
- Changes to runtime save/read strategies, public hash-key types, or the logical lowercase-hex DVault boundary.
- Automatic migration, backfill, dual-write, or database-provisioning work for hex-to-binary adoption.
- New provider support or widened read/save shape claims beyond the existing benchmark and strategy boundaries.
- Release/version/package-publication changes unless another ticket explicitly asks for release-alignment docs.

## Acceptance Criteria
- A checked-in provider-configured evidence set for this ticket uses the existing benchmark triplet plus footprint sidecars from the same execution and preserves run context, provider filter, hash-key variants, and optional-provider execution status.
- The landed evidence keeps deterministic variant metadata visible, including `hashKeyVariant`, `stableHashAlgorithm`, `digestBytes`, `hashKeyStorage`, and `hashKeyPayloadBytes`, so provider comparisons remain reproducible and row identity stays stable.
- Performance documentation summarizes measured outcomes per provider and scenario, explicitly separating binary-vs-hex comparisons within the same algorithm from shortened-digest comparisons, and identifies measured wins, neutral or regressive cases, and caveats.
- Canonical evidence surfaces add or cite the provider binary-vs-hex rows by `scenario`, `provider`, `baseline`, and `posture`; only completed provider-configured rows are promoted as timing claims.
- Skipped, failed, or unconfigured provider rows remain visible as placeholders or caveats and are not presented as measured provider performance.
- Documentation that currently scopes hash-key evidence to the SQLite-only bundle is updated to point to the new provider-configured evidence while preserving migration and compatibility caveats.

## Definition of Done
- The bounded matrix remains reproducible through the existing `--hash-key-storage-matrix` path without introducing a new harness or new benchmark-summary columns.
- Checked-in artifacts live under explicit ticket or release labels and include `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, `hash-key-footprint.md`, `hash-key-footprint.csv`, and `hash-key-footprint.json` from the same run context.
- `docs/performance-profiles.md`, `docs/plans/provider-optimization-evidence-matrix.md`, and the other hash-key evidence entry points that still describe SQLite-only evidence are aligned with the landed provider evidence.
- Documentation keeps `HexString` as the compatibility default and preserves the existing migration-guide and lowercase-hex public-boundary caveats when discussing any binary wins.
- If code or docs change benchmark participation wording, the repo's existing benchmark option and artifact-metadata validation continues to pass.

## Implementation Notes
- Reuse the existing `--hash-key-storage-matrix` mode and `BenchmarkHashKeyVariant.BoundedStorageMatrix` instead of adding new CLI surface or a parallel benchmark harness.
- Keep the current external-provider row families emitted by `BenchmarkRunner`: provider-neutral fallback plus provider-optimized save rows, latest-satellite reads, PIT reads, and bridge reads, with the retained PostgreSQL and MySQL save rows where the harness already exposes them.
- When multiple variants run, keep the current baseline-naming convention that appends the variant label to non-default baselines and preserve the same variant tokens in `executionDetail` and footprint sidecars.
- Use the existing evidence contract to map completed rows to `completed-timing` and unconfigured rows to `skipped-placeholder`; keep `storage-footprint` rows or payload-byte facts out of cross-provider timing conclusions.
- If current docs and runner behavior disagree on DB2 participation in the hash-key matrix lane, align the docs to the landed benchmark behavior rather than reopening provider selection as a new architecture question.
- Do not claim that binary storage changes DVault's public model boundary; request, save, read, diagnostics, and support-bundle surfaces stay on lowercase hexadecimal string values.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add higher-iteration reruns for providers where binary-vs-hex results are directionally flat or noisy before any stronger recommendation is published?
- After provider evidence lands, should adopter guidance ever recommend a provider-specific binary default for new projects, or should `HexString` remain the compatibility default regardless of measured wins?
- Does any provider need a later scale-matrix or latest-index follow-up if the standard binary-vs-hex matrix shows materially different behavior under larger data sets or replay-heavy workloads?

## Risks
- Provider timing is hardware- and environment-sensitive; conclusions are only valid with the preserved artifact triplet and run context and should not be generalized beyond those bundles.
- Some providers may show clear storage-footprint reductions without a matching timing win, or may trade time versus allocation differently across save and read scenarios.
- The bounded matrix mixes storage-profile and digest-width variants, so summary language can misattribute shortened-digest gains to binary storage if comparisons are not written carefully.
- Collecting comparable evidence across all optional providers depends on reachable provider environments; missing lanes must be treated as incomplete coverage, not silently satisfied by skipped placeholders.
- Current docs still contain SQLite-only hash-key evidence language, so documentation alignment is part of avoiding contradictory adoption guidance.

## Split Recommendations
- If capturing comparable configured evidence across PostgreSQL, SQL Server, MySQL, Oracle, and DB2 in one pass proves operationally unstable, split evidence collection by provider family but keep one aggregation step that updates the canonical evidence surfaces only after all required bundles exist.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: measure binary-vs-hex storage footprint and timing across supported providers. Acceptance: performance docs identify measured wins, neutral cases, and caveats.