<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified ticket, comments, relations, and repository evidence; the authoritative description update is already applied at revision 06FEJ31NEAB48DEREQREFQB0N0, and the ticket is refined as bounded low-risk allocation work on the SQLite sha256-v1 HexString baseline with no further planning writes required.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The authoritative hotspot baseline is artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/ from done ticket 06FE4R1XJVQZTQ8S9WN2YE3ZKW; the ranked order starts with DefaultDataVaultSaveService.AddSatellitesAsync, then DefaultDataVaultSaveService.FilterSatellitePlansAsync and LoadLatestSatelliteHashDiffsAsync, then DefaultStableHashNormalizer.NormalizeFields, then BuiltInStableHashService.ComputeHash.
- The default v1 evidence lane remains SQLite local temporary files with sha256-v1 and HexString; optional providers in the checked-in benchmark summary are skipped placeholders unless separately rerun as explicit regression evidence.
- Satellite HashDiff generation remains caller-owned and out of scope; this ticket only targets DVault-owned canonicalization, digest generation, save-plan materialization, and latest-hash-diff replay filtering allocations.
- Verified live relation context: incoming relates from done story 06FE4R089MT3BYRCVH7Q4EX6CG is historical alignment context, incoming blocks from done hotspot ticket 06FE4R1XJVQZTQ8S9WN2YE3ZKW is completed prerequisite context, and outgoing blocks to docs ticket 06FE4R2EGQ444EGPKZBRZCDEV8 should stay in place because that docs task depends on measured results from this implementation.
- No child tickets, relation changes, attachments, or planning documents were materialized in this turn; the durable refinement change was the already-applied description update.

### Scope In
- Implement low-risk allocation reductions in the provider-neutral/common hot paths surfaced by the hotspot report, especially satellite replay and save-preparation work in DefaultDataVaultSaveService.AddSatellitesAsync, FilterSatellitePlansAsync, and LoadLatestSatelliteHashDiffsAsync.
- Reduce avoidable allocation materialization in stable-hash canonicalization and digest generation without changing the public contract, especially DefaultStableHashNormalizer.NormalizeFields and BuiltInStableHashService.ComputeHash.
- Refresh comparable before/after benchmark evidence with the existing benchmark harness and artifact contract so targeted allocation improvements are reviewable.
- Preserve current persisted outcomes, published stable-hash vectors, chunked continuity behavior, and provider strategy-selection behavior while reducing allocations.

### Scope Out
- Changing stable-hash algorithm ids, published digest vectors, canonical text rules, hash-key storage-profile contracts, or the public lowercase-hex hash-key boundary.
- Reassigning satellite HashDiff ownership to DVault or adding an internal payload-hash generator.
- Provider-specific SQL/save/read tuning, new optional-provider performance claims, or broad non-default hash-key variant validation beyond follow-up regression checks.
- Benchmark harness replacement, artifact-schema redesign, or documentation and release-note updates already owned by ticket 06FE4R2EGQ444EGPKZBRZCDEV8.
- High-risk structural rewrites whose correctness depends on new persistence semantics rather than low-risk allocation reductions.

## Acceptance Criteria
- Implementation uses the hotspot ordering from artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/allocation-hotspots.md as the bounded optimization target set and prioritizes the dominant satellite replay/save-preparation allocations before lower-ranked micro-optimizations.
- Comparable before/after evidence is produced with the existing contract from docs/plans/performance-evidence-benchmark-artifact-contract.md; if hotspot profiling is rerun, the standard benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json triplet remains authoritative and allocation-hotspots.* stays additive.
- The directly targeted rows improve or hold on mean allocated bytes, and the visible regression budget rules remain satisfied: targeted metrics improve or hold, required SQLite non-target regressions above 5% fail by default, and any configured optional-provider regression above 10% is explicitly called out and justified.
- Stable hash behavior stays bit-for-bit compatible: current published digest vectors, algorithm ids, canonical lowercase-hex output, and normalization rules remain unchanged.
- Save-path behavior stays stable: unchanged replay continues to suppress duplicate satellite writes, changed replay continues to persist the expected new state, and provider-neutral versus provider-specific strategy-selection boundaries are not widened or redefined by this ticket.

## Definition of Done
- Repository code contains only low-risk allocation reductions inside the bounded DVault-owned hot paths and does not change caller-facing hash or storage contracts.
- Benchmark evidence shows the targeted allocation rows improved or held under the same SQLite sha256-v1 HexString baseline and workload shapes used by the hotspot ticket.
- Existing unit and integration coverage, or equivalent updated tests, protect stable hash vectors, canonicalization rules, and unchanged-versus-changed satellite replay behavior after the allocation changes.
- The refreshed evidence is sufficient for downstream docs ticket 06FE4R2EGQ444EGPKZBRZCDEV8 to cite measured results without reopening product-boundary questions.
- No residual PO blocker remains once the bounded optimization targets, regression budget, and evidence contract are documented.

## Implementation Notes
- Use the 2026-06-21 hotspot artifact set from ticket 06FE4R1XJVQZTQ8S9WN2YE3ZKW as the authoritative baseline and keep the same workload families: stable-hash-canonicalization, stable-hash-digest-generation, customer-profile-hub-only-save-prep, order-product-link-bearing-save-prep, satellite-unchanged-replay-filter, and satellite-changed-replay-filter.
- The measured optimization order is already finite: start with DefaultDataVaultSaveService.AddSatellitesAsync, then FilterSatellitePlansAsync and LoadLatestSatelliteHashDiffsAsync, then DefaultStableHashNormalizer.NormalizeFields, then BuiltInStableHashService.ComputeHash.
- Prefer low-risk allocation reductions that remove intermediate materialization or repeated collection shaping inside the current algorithms; do not change observable persisted outcomes, field-order rules, or digest semantics to chase a win.
- Reuse the existing --allocation-hotspots lane for hotspot reruns and the standard root benchmark matrix for broader regression checks; --latest-indexes can remain a focused diagnostic aid, not the authoritative evidence replacement for this ticket.
- The authoritative delivery contract description has already been applied to ticket 06FE4R261S2FSQ786S4F4JE90R at revision 06FEJ31NEAB48DEREQREFQB0N0.

## Open Questions
- none

## Follow-Up Questions
- After the bounded low-risk reductions land, do any remaining stable-hash canonicalization or digest micro-optimizations warrant a separate post-v0.43 follow-up instead of extending this ticket?
- Once refreshed evidence is available, do any configured optional-provider lanes need explicit reruns before docs or release notes generalize the result beyond the required SQLite baseline?
- Should a future release promote any focused hotspot lane into the default benchmark workflow, or should it remain opt-in alongside --allocation-hotspots?

## Risks
- Allocation-focused edits in canonicalization or digest generation can accidentally change published hash outputs if normalization ordering, UTF-8 handling, or lowercase-hex materialization semantics drift.
- Replay-filter reductions can regress unchanged-versus-changed satellite behavior if latest-hash-diff lookup or retained chunk state semantics change with the allocation work.
- A win measured only on the required SQLite sha256-v1 HexString lane should not be overgeneralized to provider-specific or non-default hash-key variants without follow-up evidence.

## Split Recommendations
- No immediate split is required because the hotspot ranking already gives one bounded optimization order inside this ticket.
- If implementation naturally separates into a second round after the dominant replay/save-preparation reductions land, prefer a later follow-up ticket for secondary stable-hash canonicalization and digest micro-optimizations rather than widening this task mid-flight.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: implement only evidence-backed low-risk allocation reductions in hash pipeline hot paths. Acceptance: behavior/test vectors remain stable and benchmark evidence is updated.