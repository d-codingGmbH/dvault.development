[gicket-bot] PO refinement contract

Summary
- Refined the ticket to the narrow typed latest/as-of satellite read-helper slice over the existing latest-row read service, with focused tests and one documentation example; no blocking PO questions remain.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The first helper slice is the typed latest/as-of satellite projection path over the existing IDataVaultReadService latest-row baseline; PIT and bridge helpers are not part of this ticket.
- Use returned projection results and projection diagnostics as the default test evidence for this slice; stable SQL assertions are optional and not required.
- The helper stays delegate-based and exact-name-based through a projection row contract; no reflection-based DTO binding is added.
- Registry-backed usage through UseDataVaultMetadata() is in scope as a thin adapter over the same typed projection pipeline.
- A concise README or quickstart-style snippet satisfies the example requirement; this ticket does not need a new standalone sample app.

Scope In
- Add a public typed helper path for latest and optional as-of satellite reads that projects IDataVaultReadService results through a caller-supplied delegate.
- Expose exact-name projection access to the technical fields plus declared driving-key and payload values for the selected satellite row.
- Add a registry-backed latest/as-of adapter that resolves satellite metadata from the authoritative DVault registry and reuses the same typed helper behavior.
- Add focused unit and integration tests in the existing tests/DCoding.Data.DVault.Tests layout.
- Add one bounded documentation snippet showing latest and as-of typed read usage.

Scope Out
- No PIT-backed read helper work.
- No bridge traversal read helper work.
- No rewrite or removal of the raw ReadLatestSatelliteRowsAsync(...) escape hatch or DataVaultSatelliteReadRecord shape.
- No reflection-based DTO binding, auto-mapping, or broader projection framework.
- No provider-specific read optimization work beyond composing with the current provider read-strategy dispatch.
- No broad read-helper family expansion beyond this first typed latest/as-of satellite slice.

Open questions
- none

Follow-up questions
- If later read-helper slices are added, should PIT and bridge typed projections continue the same exact-name projector pattern rather than introducing a different binding model?
- If consumers later want non-string or auto-bound projections, should that be a separate API family instead of extending this first slice?
- Do provider-specific read-strategy benchmarks need separate projection-path evidence beyond the existing latest-row baseline?

Risks
- If the typed helper is implemented by post-processing raw records instead of sharing the projection pipeline, provider-strategy behavior can drift from raw reads.
- The exact-name projector contract creates a public naming surface; documentation, API snapshots, and diagnostics must stay synchronized to avoid confusing breaks.
- The ticket title is broad enough to invite PIT or bridge expansion; the implementation needs to stay anchored to latest/as-of satellite projections to preserve scope.

Split recommendations
- Split PIT-backed typed read helpers into a separate ticket if work extends beyond latest/as-of satellite projections.
- Split bridge traversal typed helpers and any bridge-specific diagnostics into a separate ticket.
- Split reflection-based DTO binding or additional non-string projection accessor families into a separate ticket.

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