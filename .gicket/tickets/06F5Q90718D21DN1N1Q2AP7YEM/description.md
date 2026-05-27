<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Authoritative ticket contract already reflects the required contract refresh, corrected relation/risk text, and provider-specific v0.20.0 documentation boundary, so the ticket is ready to return to PO-critic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This refinement has already materialized one authoritative delivery-contract refresh only; no child tickets, relation changes, attachments, or planning documents were created.
- v0.19.0 remains the current public baseline and keeps staged provider bulk ingestion outside that release's claim set, so this ticket documents the v0.20.0 boundary rather than reopening v0.19.0.
- The public write baseline remains provider-neutral explicit save through `IDataVaultSaveService`; `DataVaultBulkSaveRequest` stays the compatibility baseline for already-materialized ordered saves, and `DataVaultChunkedSaveRequest` stays the provider-neutral bounded streaming path rather than a provider-native bulk default.
- The v0.20.0 optimized-path narrative is provider-specific: staged bulk is the preferred optimized path only where repository evidence already shows supported or measured staged behavior, SQL Server stays on native-bulk wording, Oracle keeps the retained direct optimized path, and stored procedures remain non-default escape-hatch guidance only.

### Scope In
- Update `README.md` and `docs/production-adoption-checklist.md` to present the v0.20.0 write-path hierarchy as provider-neutral explicit save baseline plus provider-specific optimized paths, without presenting stored procedures as the default recommendation.
- Update `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `docs/releases/v0.20.0.md` to explain the benchmark-visible provider boundaries, named artifact files, and shared evidence contract for staged, direct, and native-bulk claims.
- Document provider-specific optimized-path wording precisely: staged bulk where the repository already shows supported or measured staged behavior, SQL Server native-bulk wording, and Oracle direct optimized exception until a measured staged Oracle path exists.
- Document stored procedures only as an explicit design-time or provider-specific escape hatch that requires confirmed provider evidence and migration-synchronization guidance.

### Scope Out
- Implementing staged bulk ingestion, provider-native chunk execution, or automatic stored-procedure generation behavior in product code.
- Introducing new benchmark artifact schemas, new performance harnesses, or release automation changes.
- Designing generic stored-procedure scaffolding or migration helpers beyond documenting the boundary and caveats.

## Acceptance Criteria
- `README.md` and `docs/production-adoption-checklist.md` keep `IDataVaultSaveService` as the public write boundary, keep `DataVaultBulkSaveRequest` as the compatibility baseline for already-materialized ordered saves, and keep `DataVaultChunkedSaveRequest` as provider-neutral bounded chunking guidance rather than a blanket provider-optimized default.
- The v0.20.0 docs describe staged bulk as the preferred provider-optimized path only where repository evidence already shows a supported or measured staged provider path, currently PostgreSQL staged COPY and MySQL staged bulk; the same docs keep SQL Server phrased as current native-bulk guidance and Oracle phrased as the retained direct optimized exception until benchmark evidence proves a staged Oracle win.
- Stored-procedure guidance stays non-default and explicit: the docs present it only as an escape hatch that depends on confirmed provider evidence and migration-synchronization rules, and no updated document implies DVault auto-generates or auto-manages stored procedures as a standard runtime path.
- Benchmark-facing documentation updates land in `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `docs/releases/v0.20.0.md`, reuse the root `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` triplet plus the shared artifact contract, and preserve provider-specific boundary wording instead of inventing new evidence files or schema.
- Release notes, README, benchmark-facing docs, and the production checklist tell one consistent v0.20.0 story about provider-neutral explicit save baseline, provider-specific optimized paths, benchmark evidence boundary, and stored-procedure exceptions.

## Definition of Done
- `README.md`, `docs/production-adoption-checklist.md`, `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`, and `docs/releases/v0.20.0.md` are updated and cross-consistent.
- Documentation explicitly distinguishes the historical v0.19.0 baseline from the new v0.20.0 provider-specific optimized-path guidance.
- No updated document implies that DVault auto-generates or auto-manages stored procedures as a standard runtime path.
- Any benchmark references point back to the existing benchmark artifact contract and authoritative benchmark summary files.

## Implementation Notes
- Use the existing documentation baselines already present in the repository: `README.md`, `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`, `docs/releases/v0.19.0.md`, `docs/releases/v0.19.0/README.md`, `docs/production-adoption-checklist.md`, `docs/model-first-governance.md`, and `docs/plans/performance-evidence-benchmark-artifact-contract.md`.
- Preserve the current public framing from v0.19.0 that staged provider bulk ingestion was previously outside the claim set, then document the v0.20.0 change as a deliberate boundary shift rather than a retroactive rewrite of older release notes.
- Keep benchmark terminology aligned with the existing artifact vocabulary: `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, and before/after bundles where applicable under the shared contract.
- Keep provider-specific wording aligned with current benchmark-facing repository evidence: PostgreSQL staged COPY with a retained direct or UNNEST boundary below 60 operations, MySQL staged bulk at 60+ operations with a retained multi-row boundary above 50 and below 60, SQL Server current native bulk wording, and Oracle retained direct optimized batching with `stagedOracleBulk=not-selected-no-measured-win`.
- No attachments, child tickets, relation mutations, or planning documents were materialized in this refinement; the only persisted planning action was the authoritative delivery-contract refresh.

## Open Questions
- none

## Follow-Up Questions
- After provider evidence is stable, does the roadmap want a future provider-by-provider decision matrix covering staged bulk, retained direct or multi-row paths, chunked explicit save, and explicit stored-procedure escape hatches?
- If a later Oracle benchmark proves a staged win over the retained direct path, should that change land as a separate Oracle comparison follow-up rather than widening this ticket beyond the current repository-evidenced boundary?

## Risks
- The persisted relation graph still carries incoming `blocks` links from done stories `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and `06F5Q900FC0P3HBZP81CVK7264`; treat them as historical rather than active blockers, but reopened implementation or evidence changes would still require documentation wording updates before release.
- Because three downstream tickets are currently blocked by this documentation ticket, ambiguity in the provider-specific write-path hierarchy or stored-procedure caveats will propagate quickly.
- If provider evidence or migration-synchronization rules are incomplete at doc-authoring time, the stored-procedure section can overclaim unsupported automation.
- If v0.20.0 release prose generalizes staged bulk beyond measured or supported provider lanes, adopter guidance can overstate SQL Server or Oracle behavior relative to the current repository evidence.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Update docs for staged bulk ingestion and explicitly position stored procedures.

Acceptance criteria:
- Documents staged bulk as the preferred provider-optimized path over automatic stored-procedure generation.
- Captures stored procedures as a possible explicit design-time escape hatch only after provider evidence and migration synchronization are understood.
- Updates README, production checklist, benchmark docs, and release notes.