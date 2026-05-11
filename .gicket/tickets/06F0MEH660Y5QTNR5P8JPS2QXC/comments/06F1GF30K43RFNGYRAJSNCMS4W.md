[gicket-bot] PO refinement contract

Summary
- Refined the provider-neutral PIT snapshot read-service ticket against the existing v1 API contract, release-note baseline, and current provider-neutral read patterns; no child tickets, relation changes, or new planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ratify the existing v1 contract in docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md as the source of truth for this ticket's public surface.
- V1 stays on the DataVaultPitMetadata baseline only; the historical DataVaultPointInTimeMetadata / ModelBuilder.PointInTime surface is out of scope.
- One request targets exactly one PIT declaration with one hub parent and ordered hub-attached non-multi-active satellites.
- A missing PIT row at or before the requested as-of timestamp yields no read/projection record for that parent hash key rather than a fallback latest-satellite read.
- No child tickets, relation updates, or additional planning documents were created during this refinement pass.

Scope In
- Implement provider-neutral PIT as-of reads for generated PIT tables by parent hub hash key batch and as-of timestamp.
- Add the approved request/raw-record/projection surface for PIT reads on IDataVaultReadService and its typed projector helper.
- Resolve the latest PIT row visible at or before the normalized as-of timestamp for each requested parent hash key.
- Join configured satellites from the selected PIT row in declared PIT satellite order and surface empty or missing snapshot states according to the approved contract.
- Validate supported PIT metadata and generated EF entity shape before querying, with deterministic diagnostics for unsupported shapes.

Scope Out
- PIT row population, refresh orchestration, or late-arriving reconciliation.
- Provider-specific PIT read optimizations or provider-specific SQL.
- Bridge traversal reads.
- Link-parent PITs, link-attached satellites, or multi-active PIT/satellite shapes.
- Reflection-based DTO binding or any change to existing latest/as-of satellite behavior.

Open questions
- none

Follow-up questions
- When PIT row maintenance is later scoped, should refresh/orchestration remain a separate service boundary from the read pipeline?
- After provider-neutral correctness is proven, which provider packages, if any, justify provider-specific PIT read optimizations based on measured workload evidence?
- Which release note and README updates should accompany the first tagged release that promotes PIT-backed reads beyond the current v0.6.0 'not delivered' baseline?

Risks
- If generated PIT entities do not already expose the expected metadata annotations or snapshot-reference columns consistently, implementation may spill into separate modeling/projection work rather than staying a pure read-service task.
- Joining multiple satellites through PIT snapshot references may surface provider-neutral EF translation edge cases across timestamp storage modes, so the failure-mode and timestamp-option test matrix needs to stay explicit.
- The current release-note baseline still says PIT-backed read APIs are not delivered, so public API completion here must stay coordinated with the next release packaging/documentation pass.

Split recommendations
- No additional split is recommended now; the existing contract already bounds v1 to one hub-parent PIT read shape and leaves provider-specific optimization, PIT maintenance, bridge traversal, and multi-active cases for later work.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment