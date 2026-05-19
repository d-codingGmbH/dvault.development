<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified local ticket, comment, relation, and repository state from repository-local .gicket artifacts and source files; refined this story to one concrete additive delta: current/as-of convenience overloads over the existing latest-satellite read request family, and explicitly deferred PIT-backed historical ergonomics with no relation, child-ticket, attachment, or planning-document changes in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current means the latest visible satellite row when no asOf cutoff is supplied; this ticket does not rename or remove the existing latest/as-of vocabulary.
- The concrete story outcome is an additive convenience overload layer over the existing latest-satellite request family, not a new read service and not a direct PIT/history feature.
- No child tickets, relation changes, attachments, or planning documents were created or modified in this refinement pass.

### Scope In
- Add explicit-metadata convenience overloads ReadCurrentSatelliteRowsAsync, ReadCurrentSatelliteAsync, ReadAsOfSatelliteRowsAsync, and ReadAsOfSatelliteAsync over the existing latest-satellite request pipeline.
- Add registry-backed UseDataVaultMetadata convenience overloads with the same current/as-of semantics for parent reference, satellite name, and parent hash key inputs.
- Update README examples, public API snapshots, and tests for the new current/as-of convenience layer while preserving the existing latest-request baseline.
- Preserve existing UTC normalization, ordinal parent-hash-key deduplication, deterministic ordering, missing-parent empty results, hub-parent support, link-parent support, and multi-active driving-key series behavior.

### Scope Out
- Any PIT-backed historical read API changes, PIT request aliases, or PIT fallback behavior.
- PIT row population, PIT rebuild or incremental maintenance, and any read-time PIT refresh semantics.
- Provider-aware PIT or bridge read optimization work owned by 06F2PGPRGN0EVGD6RY5KY9M56W.
- Breaking rename, removal, or semantic change to DataVaultLatestSatelliteReadRequest, DataVaultRegistryLatestSatelliteReadRequest, ReadLatestSatelliteRowsAsync(...), or ReadLatestSatelliteAsync(...).
- Bridge read or bridge maintenance changes unrelated to the latest-satellite current/as-of convenience layer.

## Acceptance Criteria
- Explicit-metadata callers can perform current reads through ReadCurrentSatelliteRowsAsync or ReadCurrentSatelliteAsync without manually constructing a DataVaultLatestSatelliteReadRequest, and returned rows match today's latest-satellite semantics exactly.
- Explicit-metadata callers can perform as-of reads through ReadAsOfSatelliteRowsAsync or ReadAsOfSatelliteAsync by supplying a DateTimeOffset asOf, and returned rows match today's DataVaultLatestSatelliteReadRequest(..., asOf) semantics exactly.
- Registry-backed UseDataVaultMetadata callers can perform the same current and as-of reads through overloads that accept DataVaultMetadataReference parent, satelliteName, parentHashKeys, and optional asOf, and missing registry metadata still fails before query orchestration with the existing deterministic errors.
- The new convenience overloads preserve the existing latest/as-of behavior for hub-parent satellites, link-parent satellites, and multi-active driving-key series, including UTC-normalized cutoffs, StringComparer.Ordinal parent-hash-key deduplication, deterministic ordering, and empty results for missing parents.
- The existing latest request constructors and ReadLatestSatelliteRowsAsync or ReadLatestSatelliteAsync compatibility surface remain supported and unchanged; the new overloads delegate to that baseline rather than replacing it.
- README examples, integration tests, and the public API snapshot are updated to cover at least one explicit current example, one explicit as-of example, one registry-backed current example, and one registry-backed as-of example.
- This ticket does not add PIT-backed historical convenience helpers; PIT-backed reads continue to use DataVaultPitAsOfReadRequest, ReadPitRowsAsync(...), and ReadPitAsync(...) unchanged.

## Definition of Done
- Integration coverage proves the new explicit and registry-backed current/as-of overloads return the same observable results as the existing latest-satellite request pipeline, including missing-parent, missing-metadata, link-parent, and multi-active series cases.
- Public API snapshot coverage captures the new overload family and confirms the pre-existing latest request and typed read surface remains compatibility-stable.
- README and any touched release documentation explain that current is an additive convenience name over the latest-satellite baseline and that PIT-backed historical reads remain a separate surface.
- Implementation reuses the existing latest-satellite validation and read pipeline so UTC normalization, ordinal deduplication, deterministic ordering, provider-neutral fallback behavior, and current diagnostics do not drift.

## Implementation Notes
- Implement the convenience layer as extension methods on the existing read-service surface. Explicit overloads should accept DataVaultSatelliteMetadata satellite and IEnumerable<string> parentHashKeys; as-of variants add DateTimeOffset asOf. Registry-backed overloads should accept DataVaultMetadataReference parent, string satelliteName, IEnumerable<string> parentHashKeys, and asOf on the as-of variants.
- Mirror both raw-row and typed-projection shapes: ReadCurrentSatelliteRowsAsync and ReadAsOfSatelliteRowsAsync return DataVaultSatelliteReadRecord rows, and ReadCurrentSatelliteAsync and ReadAsOfSatelliteAsync return typed projections through the existing DataVaultSatelliteProjectionRow contract.
- Each overload should construct the existing DataVaultLatestSatelliteReadRequest or DataVaultRegistryLatestSatelliteReadRequest and delegate to the current ReadLatestSatelliteRowsAsync or ReadLatestSatelliteAsync implementation so no duplicate query-selection logic is introduced.
- Do not add PIT request wrappers, PIT convenience overloads, or latest-to-PIT fallback behavior here; PIT history remains on DataVaultPitAsOfReadRequest plus ReadPitRowsAsync and ReadPitAsync and depends on maintained PIT rows from docs/plans/pit-maintenance-service-v1-contract.md.
- Local .gicket verification in this pass found no attachments or child tickets and confirmed the existing parentOf and blocks relations remain unchanged.

## Open Questions
- none

## Follow-Up Questions
- After this convenience layer lands, should end-user documentation keep latest as the canonical term everywhere, or prefer current in tutorial-style examples while retaining latest as the compatibility baseline?
- Does 06F2PGPXVAYRBC94RQ7X5V4DVG need additional release-note language that contrasts the new current/as-of convenience overloads with the existing request-object baseline?
- Should a separate future ticket add analogous convenience helpers for PIT-backed history or bridge reads, or keep those surfaces request-object only?

## Risks
- If the new convenience overloads reimplement selection logic instead of delegating to the existing latest-satellite request pipeline, current/as-of behavior could drift from the already-tested baseline.
- If developers treat this story as a PIT/history change instead of a latest-satellite convenience change, they could blur the documented boundary between latest/as-of satellite reads and separate PIT-backed historical reads.
- Downstream tickets 06F2PGPRGN0EVGD6RY5KY9M56W and 06F2PGPXVAYRBC94RQ7X5V4DVG remain live blocked-by dependents, so this story should stay tightly bounded and avoid reopening optimization or documentation scope that those tickets already own.

## Split Recommendations
- No new split is needed in this PO pass. The story is now bounded to current/as-of convenience overloads only, while PIT maintenance, PIT-backed history, optimization, and broader documentation already have separate tickets or contracts.
- If a future product decision wants PIT-backed historical convenience names or a broader vocabulary rename away from latest, create a separate follow-up ticket rather than expanding this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Expose typed current and historical reads that are ergonomic for EF Core users.

## Scope
- Refine and complete the work for "Improve current and as-of query APIs" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.