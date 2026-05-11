<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the provider-neutral PIT snapshot read-service ticket against the existing v1 API contract, release-note baseline, and current provider-neutral read patterns; no child tickets, relation changes, or new planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Ratify the existing v1 contract in docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md as the source of truth for this ticket's public surface.
- V1 stays on the DataVaultPitMetadata baseline only; the historical DataVaultPointInTimeMetadata / ModelBuilder.PointInTime surface is out of scope.
- One request targets exactly one PIT declaration with one hub parent and ordered hub-attached non-multi-active satellites.
- A missing PIT row at or before the requested as-of timestamp yields no read/projection record for that parent hash key rather than a fallback latest-satellite read.
- No child tickets, relation updates, or additional planning documents were created during this refinement pass.

### Scope In
- Implement provider-neutral PIT as-of reads for generated PIT tables by parent hub hash key batch and as-of timestamp.
- Add the approved request/raw-record/projection surface for PIT reads on IDataVaultReadService and its typed projector helper.
- Resolve the latest PIT row visible at or before the normalized as-of timestamp for each requested parent hash key.
- Join configured satellites from the selected PIT row in declared PIT satellite order and surface empty or missing snapshot states according to the approved contract.
- Validate supported PIT metadata and generated EF entity shape before querying, with deterministic diagnostics for unsupported shapes.

### Scope Out
- PIT row population, refresh orchestration, or late-arriving reconciliation.
- Provider-specific PIT read optimizations or provider-specific SQL.
- Bridge traversal reads.
- Link-parent PITs, link-attached satellites, or multi-active PIT/satellite shapes.
- Reflection-based DTO binding or any change to existing latest/as-of satellite behavior.

## Acceptance Criteria
- IDataVaultReadService exposes the contract-approved provider-neutral PIT raw-row entry point plus the typed projector helper.
- Request validation requires PIT metadata, deduplicates parent hash keys with StringComparer.Ordinal, rejects null/empty/whitespace hash keys, and normalizes AsOf to UTC.
- For each requested parent hash key, the service selects the latest PIT row whose PIT LoadTimestamp is visible at or before AsOf; parents without a visible PIT row return no result.
- Satellite snapshot materialization follows the declared PIT satellite order and uses registry/model metadata and existing EF annotations where available instead of duplicating naming logic.
- Unsupported PIT shapes or malformed generated PIT entities fail with deterministic diagnostics consistent with the approved provider-neutral read contract.
- Automated tests cover timestamp storage options, empty result sets, missing snapshot states, and unsupported-shape diagnostics without relying on provider-specific optimizations.

## Definition of Done
- The core package compiles with the additive PIT read request, raw record, projection helper, and pipeline surface while preserving existing latest/as-of and bridge read behavior.
- Provider-neutral tests pass for supported PIT reads and failure-mode diagnostics across the existing timestamp storage baselines.
- Any public API snapshots, contract fixtures, or developer-facing notes required by the new PIT read surface are updated to match the approved planning contract.
- No provider-specific package work is required for this ticket beyond keeping the shared provider-neutral baseline green.

## Implementation Notes
- Mirror the existing provider-neutral read pattern already visible in bridge/latest reads: validate metadata and generated shared-type entities first, query through EF, then materialize a raw row shape plus caller-owned typed projection support.
- Reuse existing DVault annotations such as EntityKind, MetadataName, PropertyRole, and the established SnapshotReference property role when validating PIT table/entity bindings.
- Prefer registry/model metadata and projected EF metadata for PIT table and column discovery where exposed; keep any deterministic naming fallback isolated to gaps already present in the metadata surface.
- Keep PIT reads additive to the current IDataVaultReadService boundary; do not fold PIT behavior into latest/as-of satellite APIs or introduce reflection-based projection.
- Use docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md as the implementation contract/fixture baseline; no new planning document was written in this pass.

## Open Questions
- none

## Follow-Up Questions
- When PIT row maintenance is later scoped, should refresh/orchestration remain a separate service boundary from the read pipeline?
- After provider-neutral correctness is proven, which provider packages, if any, justify provider-specific PIT read optimizations based on measured workload evidence?
- Which release note and README updates should accompany the first tagged release that promotes PIT-backed reads beyond the current v0.6.0 'not delivered' baseline?

## Risks
- If generated PIT entities do not already expose the expected metadata annotations or snapshot-reference columns consistently, implementation may spill into separate modeling/projection work rather than staying a pure read-service task.
- Joining multiple satellites through PIT snapshot references may surface provider-neutral EF translation edge cases across timestamp storage modes, so the failure-mode and timestamp-option test matrix needs to stay explicit.
- The current release-note baseline still says PIT-backed read APIs are not delivered, so public API completion here must stay coordinated with the next release packaging/documentation pass.

## Split Recommendations
- No additional split is recommended now; the existing contract already bounds v1 to one hub-parent PIT read shape and leaves provider-specific optimization, PIT maintenance, bridge traversal, and multi-active cases for later work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Implement the baseline PIT-backed as-of read service using generated Data Vault tables and existing provider-neutral EF query capabilities.

## Scope In

- Reads by hub hash key and as-of timestamp.
- Joins from PIT rows to configured satellites.
- Tests for timestamp storage options and empty or missing snapshot states.

## Scope Out

- Provider-specific read optimization.
- Bridge traversal reads.

## Acceptance Criteria

- Correctness is proven independently from provider-specific optimization.
- The implementation uses registry/model metadata rather than duplicated table-name construction where available.
- Unsupported PIT shapes return deterministic diagnostics.