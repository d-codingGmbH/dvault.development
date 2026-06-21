<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined as an evidence-only performance task on the existing SQLite/`sha256-v1`/`HexString` baseline: rank DVault allocation hotspots in stable-hash canonicalization, digest generation, satellite hash-diff replay filtering, and pre-write save preparation before any optimization ticket starts.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- DVault already treats satellite `HashDiff` as caller-supplied input; this ticket profiles the DVault-owned latest-hash-diff lookup/filter and replay-dedup path rather than introducing an internal payload-hash generator.
- The default v1 evidence baseline is the required SQLite benchmark lane with `sha256-v1` and `HexString` hash-key storage, which matches the checked-in root benchmark triplet and current stable-hash/storage contracts.
- Existing benchmark artifacts already capture whole-scenario mean allocated bytes; this ticket must add finer-grained hotspot ranking evidence before any allocation-reduction implementation is attempted.

### Scope In
- Profile DVault-owned allocation hotspots in `DefaultStableHashNormalizer` canonicalization and `BuiltInStableHashService` digest generation on the default hash-key baseline.
- Profile provider-neutral/common save preparation in `DefaultDataVaultSaveService`, including request resolution, hub/link hash-key save-plan creation, unique-row dedupe preparation, and row materialization before database writes.
- Profile the satellite latest-hash-diff path for unchanged and changed replay workloads, including latest-state lookup/filtering and chunked continuity-state handling where that path is exercised.
- Preserve ticket-scoped evidence and a ranked hotspot summary that identifies the measured workloads, the highest-allocation steps or methods, and the recommended optimization order.

### Scope Out
- Implementing allocation optimizations or changing runtime behavior in the same ticket; this ticket stops at measured hotspot ranking.
- Changing stable-hash algorithm ids, hash-key storage-profile contracts, or caller-facing lowercase-hex hash-key boundaries.
- Reopening hash-diff ownership; caller code continues to supply the deterministic satellite `HashDiff` value.
- Provider-specific SQL/save/read tuning, external-provider timing collection, or full hash-key variant-matrix validation beyond the default baseline.

## Acceptance Criteria
- A repeatable profiling run exists for the required SQLite baseline with `sha256-v1` and `HexString`, and it isolates DVault-owned allocation costs instead of only reporting end-to-end database timing.
- The evidence ranks allocation hotspots across four bounded surfaces: stable-hash canonicalization, digest generation, satellite latest-hash-diff replay filtering, and pre-write save preparation.
- The ranked output names the exact workload shapes used to exercise those surfaces, including a hub-only customer-profile save shape, a link-bearing order-product save shape, and a satellite unchanged-versus-changed replay shape; any provider-specific optimized lane is identified explicitly rather than treated as the provider-neutral baseline.
- When benchmark artifacts are persisted, they reuse the repository's existing benchmark triplet contract for run context and scenario evidence; any additional hotspot report is additive and does not replace `benchmark-summary.md`, `benchmark-summary.csv`, or `benchmark-summary.json`.
- The ticket closes with a prioritized list of the top allocation hotspots and the next optimization targets, with no behavior change claimed yet.

## Definition of Done
- Repository-backed evidence identifies the top allocation hotspots in the bounded DVault save-path scope and makes the ranking reviewable without rerunning scope discovery.
- The hotspot summary clearly distinguishes measured evidence from hypotheses and does not attribute caller-owned hash-diff generation costs to DVault.
- The evidence preserves enough run context to compare later optimization tickets against the same baseline and workload shapes.
- No residual PO blocker remains once the evidence-only scope, bounded baseline, and post-ranking follow-up path are documented.

## Implementation Notes
- Ratify the existing default baseline first: SQLite local temporary files, `sha256-v1`, `HexString`, and the current common/provider-neutral save path; treat optional providers and non-default hash-key variants as later validation only.
- Reuse the current benchmark harness for whole-scenario allocation baselines, especially `customer-profile-bulk-history`, `customer-profile-streaming-save`, and `order-product-fulfillment-history`, because those lanes already exercise the hub-only, chunked/replay, and link-bearing save shapes visible in the repository.
- Use the existing `--latest-indexes` workload shape or an equivalent focused lane to capture unchanged and changed satellite replay allocations; if the current optimized SQLite lane does not isolate the targeted common path, mirror that workload with an explicit common/provider-neutral measurement surface instead of assuming equivalence.
- Focus measurement inside `DefaultStableHashNormalizer`, `BuiltInStableHashService`, and the pre-write portions of `DefaultDataVaultSaveService`; likely candidates include normalized-field construction/sorting, UTF-8 byte and hex materialization, save-plan row dictionary creation, persisted-hash lookup preparation, and latest-hash-diff state materialization.
- If additional profiler output is needed for ranked method-level allocations, store it beside the ticket-scoped evidence set as supplemental material rather than as a replacement for the shared benchmark artifact contract.

## Open Questions
- none

## Follow-Up Questions
- After the ranking lands, should actual optimization work be split into separate implementation tickets for stable-hash canonicalization/hash generation versus satellite replay/save-preparation allocations?
- Once a hotspot fix is implemented, which non-default validation lanes need reruns beyond the default baseline: the bounded hash-key storage matrix, optional provider save lanes, or both?
- Should any focused hotspot lane become part of the default benchmark report after this evidence ticket, or remain opt-in like the current `--latest-indexes` mode?

## Risks
- Whole-scenario allocation numbers can hide the true in-memory hotspot order if the evidence does not separately isolate DB/EF overhead from DVault-owned canonicalization and save-preparation work.
- Because satellite `HashDiff` values are caller-supplied, the ticket can be overread if upstream payload-hash generation costs are mixed into the DVault hotspot summary.
- A ranking taken only on the default SQLite/`sha256-v1`/`HexString` baseline should not be generalized to provider-specific or non-default hash-key variants without follow-up validation.

## Split Recommendations
- Keep this ticket evidence-only. If the ranking surfaces independent hotspot families, land follow-up implementation tickets separately for stable-hash canonicalization/hash generation and for satellite replay/save-preparation allocation reduction.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: profile allocation hotspots in canonicalization, hash generation, hash diff, and save preparation paths. Acceptance: hotspots are ranked with evidence before implementation.