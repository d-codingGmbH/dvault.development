<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to a bounded PIT-backed as-of read contract that extends the existing `IDataVaultReadService` projector pattern and the documented `DataVaultPitMetadata` baseline; no new split, attachment, planning document, or relation change was needed for this refinement pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 public read boundary should stay on `IDataVaultReadService`; the PIT-backed contract should add a PIT request/raw-record pair that follows the existing latest/as-of projector pattern instead of creating a separate reflection-based read stack.
- The contract should be anchored to `DataVaultPitMetadata` for one hub plus its ordered non-multi-active hub-attached satellites; it should not reopen link-based PITs, bridge traversal reads, or PIT over multi-active satellites.
- The older `DataVaultPointInTimeMetadata` and `DataVaultModelBuilder.PointInTime(...)` surface remains historical and out of scope for this ticket; v1 naming and examples should use the newer `Pit` vocabulary.
- Timestamp handling stays logical and provider-neutral: callers supply an `asOf` instant as `DateTimeOffset`, and provider timestamp storage modes remain an implementation detail behind the existing capability-profile pipeline.
- Missing PIT rows for requested parent hash keys should yield no projected record for those parents, and an existing PIT row with an absent satellite snapshot should surface that satellite segment as absent rather than silently falling back to non-PIT latest/as-of reads.

### Scope In
- Document the PIT-backed as-of request/response contract for reading one declared PIT by parent hash key set and `asOf` instant.
- Define the raw PIT read-record shape needed for typed projector delegates, including PIT load timestamp and per-satellite snapshot access in declared order.
- Specify behavior for multiple satellites, missing PIT rows, and missing per-satellite snapshot values inside an otherwise matched PIT row.
- Specify deterministic diagnostics for unsupported multi-active satellite, bridge, or other out-of-baseline PIT declarations.
- Provide worked examples and fixture expectations that align with current DVault naming and read-service conventions.

### Scope Out
- PIT row population, refresh scheduling, late-arriving reconciliation, and any other PIT maintenance behavior.
- Provider-specific SQL, indexing, query optimization, or storage-tuning work.
- Bridge traversal helpers, link-based PIT parents, and PIT over multi-active satellites.
- Renaming, reconciling, or deprecating the older `PointInTime` modeling surface.
- Reflection-based DTO binding or a second public read service just for PIT.

## Acceptance Criteria
- The contract defines a provider-neutral PIT read request on `IDataVaultReadService` that accepts one `DataVaultPitMetadata` declaration, one or more parent hash keys, and an `asOf` instant, and it states that the service resolves the latest PIT row visible at or before that instant per requested parent.
- The contract defines a raw PIT read-record shape that exposes the parent hash key, PIT load timestamp, and per-satellite snapshot data keyed by declared satellite name and ordered by the `DataVaultPitMetadata` declaration so a caller-owned projector can build typed read models.
- The contract states that a missing PIT row yields no result for that parent, while unsupported or inconsistent PIT metadata shapes fail deterministically through diagnostics instead of silently falling back to latest-satellite logic.
- The contract explicitly rejects unsupported v1 shapes, including multi-active satellite references, bridge-driven reads, link-based PIT parents, and any request that tries to read outside the bounded `DataVaultPitMetadata` baseline.
- The contract and examples show that timestamp storage modes remain internal and do not change the caller-facing `DateTimeOffset` API.
- Documentation or fixture examples cover at least one multi-satellite typed projection example and one missing-PIT-row example before implementation starts.

## Definition of Done
- A planning-level contract is written in ticket or repository documentation with the bounded v1 PIT read surface, examples, and non-goals.
- Expected request and raw-record/projection shapes are captured in API fixtures, snapshots, or equivalent tests so downstream implementation has a stable contract target.
- The contract cross-references the current latest/as-of satellite read baseline and confirms PIT reads extend it without changing existing latest-satellite behavior.
- Unsupported multi-active, bridge, and legacy `PointInTime` cases are called out as diagnostics or out-of-scope behavior in the final contract text.

## Implementation Notes
- Repository evidence already fixes the surrounding baseline: the README and v0.6.0 release notes show `IDataVaultReadService`, `DataVaultLatestSatelliteReadRequest`, caller-owned projector delegates, and raw row reads as the existing public read model to stay compatible with.
- The deferred capability record already bounds PIT metadata to one hub plus ordered hub-attached satellites and explicitly leaves link-based PITs, multi-active PIT semantics, bridge interactions, and provider-specific optimization deferred; this ticket should ratify that as the v1 read-contract boundary instead of reopening those topics.
- When a PIT row is found, the raw record should preserve PIT declaration order for satellite segments so typed projectors remain deterministic across multi-satellite reads.
- No ticket attachment or planning document was materialized in this run; the refinement relies on existing repository documents already referenced by the ticket context.
- Persisted relations were reviewed and left unchanged; the live relation state still shows one upstream `blocks`, one parent relation, and two downstream `blocks` relations for this ticket.

## Open Questions
- none

## Follow-Up Questions
- After the contract is implemented, should a higher-level convenience helper be added for common named read-model cases, or is the projector-only baseline sufficient for v1?
- Once PIT-backed reads exist, does the roadmap want a separate follow-up to unify or retire the older `PointInTime` modeling vocabulary?
- Do downstream docs want runnable quickstart coverage for PIT-backed reads in addition to the contract examples, or can that wait until PIT row maintenance exists?

## Risks
- The main scope-creep risk is pulling legacy `PointInTime` naming, PIT maintenance, or provider-specific optimization into this ticket; any of those would turn a bounded contract task into multi-ticket design work.
- If the raw PIT read-record shape does not make missing satellite snapshot state explicit, downstream typed projectors may implement inconsistent null-or-absence behavior across satellites.
- The live upstream `blocks` relation means a later change to PIT metadata rules could still force this contract to be revised, even though the current repository documents are strong enough for PO refinement now.

## Split Recommendations
- No new split is recommended from current evidence; keep this ticket as the bounded public-contract-and-examples decision and let the already-related downstream work consume the finalized contract.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Design a PIT-backed read API that builds on existing PIT metadata and remains compatible with typed read projections.

## Scope In

- API shape for as-of snapshot reads.
- Handling of multiple satellites, missing PIT rows, and timestamp storage modes.
- Diagnostics for unsupported multi-active or bridge interactions.

## Scope Out

- Implementation.
- Provider-specific optimization.

## Acceptance Criteria

- The contract is documented with examples before implementation.
- It does not conflict with the existing latest/as-of satellite read service.
- Tests or fixtures capture expected request and response shapes.