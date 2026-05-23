<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Current branch evidence directly confirms the existing read-diagnostics service and carrier types, so this remains one additive ReadShape extension with no bounded planning writes needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Live relation state remains unchanged: epic 06F492BTNHRPBC7D24E13ECFKM is parentOf this ticket and this ticket still blocks 06F492CAB2293R7BGJWMWMRKT4 and 06F492D05THPGQVT3B3K7853A0.
- The current branch already contains the public read-diagnostics baseline this story extends: DataVaultDiagnosticsResult exposes ReadShape, DataVaultReadShapeDiagnostics already models satellite, PIT, and bridge branches, and IDataVaultReadDiagnosticsService already owns request-bound read Analyze entry points.
- IDataVaultDiagnosticsService remains the metadata and save-analysis surface; this ticket does not create a new root diagnostics service.
- Existing source already defines ExpectedIndexBaseline on satellite and PIT read-shape diagnostics and ExpectedTraversalIndexBaseline on bridge read-shape diagnostics, so the historical index hints wording is legacy title text rather than a net-new subsystem requirement.
- No child tickets, relation mutations, description writes, attachments, or planning documents were applied or queued in this run because the branch snapshot and existing ticket context resolved the PO-critic findings without a split or durable planning artifact.

### Scope In
- Add projected-column group facts to the existing satellite read-shape diagnostics branch exposed through DataVaultDiagnosticsResult.ReadShape for latest, current, and as-of satellite requests.
- Add projected-column group facts and ReferencedSatelliteLookupCount to the existing PIT read-shape diagnostics branch.
- Add projected-column group facts to the existing bridge read-shape diagnostics branch.
- Preserve additive compatibility, explicit-versus-registry-backed equivalence, redaction behavior, and the existing index or traversal baseline fields while extending the read-shape payload.

### Scope Out
- No new public read-diagnostics service and no replacement of IDataVaultReadDiagnosticsService, DataVaultDiagnosticsResult.ReadShape, or DataVaultReadShapeDiagnostics.
- No net-new index-hint subsystem and no rename or semantic reset of ExpectedIndexBaseline or ExpectedTraversalIndexBaseline.
- No raw SQL, execution plans, live schema inspection, automatic index creation, or provider-specific physical-plan advice.
- No disclosure of request values and no broader join-count, predicate decomposition, analyzer, telemetry, or release-automation work in this story.

## Acceptance Criteria
- IDataVaultReadDiagnosticsService continues to own request-bound Analyze calls for explicit and registry-backed latest, current, and as-of satellite requests plus PIT and bridge requests, and DataVaultDiagnosticsResult.ReadShape continues to carry the result through the existing DataVaultReadShapeDiagnostics family with additive members only.
- Satellite read diagnostics add ProjectedColumns on the existing satellite read-shape branch, emitting deterministic DataVaultReadShapeColumnSet roles technicalProjection and payloadProjection and emitting drivingKeyProjection only when the satellite metadata declares driving keys. For the Profile test metadata in DataVaultDiagnosticsTests, payloadProjection includes CustomerName and CustomerTier.
- PIT read diagnostics add ProjectedColumns and ReferencedSatelliteLookupCount on the existing PIT read-shape branch, emitting deterministic DataVaultReadShapeColumnSet roles pitTechnicalProjection, snapshotReferenceProjection, and satellitePayloadProjection. For PitCustomerProfileStatus, ReferencedSatelliteLookupCount equals 2 and snapshotReferenceProjection covers the Profile and Status snapshot reference columns already surfaced by current PIT diagnostics output.
- Bridge read diagnostics add ProjectedColumns on the existing bridge read-shape branch, emitting a deterministic endpointProjection column set and emitting depthProjection when the request is depth-bounded. For BridgeCustomerOrder, endpointProjection includes CustomerHashKey and OrderHashKey.
- ReadDiagnosticsPopulateReadShapeForExplicitRegistryPitAndBridgeRequests, SupportBundleSerializesReadShapeWithoutRequestValues, and any existing public API compatibility snapshot coverage prove the additive members while preserving the current filter, order, index, provider, and redaction baseline, including ExpectedIndexBaseline and ExpectedTraversalIndexBaseline behavior.

## Definition of Done
- Implementation stays centered in src/DCoding.Data.DVault/DataVaultDiagnostics.cs and preserves IDataVaultReadDiagnosticsService as the request-bound entry surface while leaving IDataVaultDiagnosticsService ownership of metadata and save analysis unchanged.
- The supported satellite, PIT, and bridge read families populate the new ProjectedColumns facts, and PIT reads also populate ReferencedSatelliteLookupCount, within the existing DataVaultDiagnosticsResult.ReadShape and DataVaultReadShapeDiagnostics family while current observable read-diagnostics semantics remain intact.
- Redaction, support-bundle coverage, and explicit-versus-registry-backed equivalence continue to pass through the existing diagnostics tests.
- Any user-facing documentation updated for this work describes it as an additive extension of the existing read-shape diagnostics surface, not as a brand-new read-diagnostics API or a new index-hint subsystem.

## Implementation Notes
- Use src/DCoding.Data.DVault/DataVaultDiagnostics.cs as the primary implementation ownership point and extend the existing public carrier records already defined there.
- Preserve the request-bound service entry point proven by provider.GetRequiredService<IDataVaultReadDiagnosticsService>() in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs; do not reroute this story through IDataVaultDiagnosticsService.
- Extend DataVaultDiagnosticsResult.ReadShape and the existing satellite, PIT, and bridge read-shape records additively for projected-column facts and PIT ReferencedSatelliteLookupCount; keep ExpectedIndexBaseline and ExpectedTraversalIndexBaseline semantics intact.
- Until a separate ticket-admin cleanup can update the stored title string, treat the delivery contract rather than the historical title wording as the authoritative scope boundary.

## Open Questions
- none

## Follow-Up Questions
- If product still wants the stored ticket title to drop the historical and index hints wording now that existing index baselines are baseline-only, should runtime or a human perform that metadata cleanup in a separate ticket-admin pass?
- If product later wants richer predicate decomposition, join-plan hints, or provider-specific tuning guidance beyond projected columns and PIT lookup counts, should that ship as a separate follow-up story?

## Risks
- If implementation routes request-bound work through the wrong service or replaces the existing ReadShape carrier instead of extending it additively, current consumers may break.
- If projection role names vary by provider or request path, explicit and registry-backed diagnostics become harder to compare; keep role names deterministic and provider-neutral.
- If downstream readers follow the unchanged title instead of the delivery contract, they may assume net-new index-hint work that is out of scope until metadata cleanup occurs.

## Split Recommendations
- No split is required. This remains one bounded additive read-shape diagnostics extension on top of the existing request-bound baseline already visible in the current branch.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Build on v0.17 query-shape diagnostics with performance-oriented signals: projected columns, join count, predicate shape, ordering, likely index needs, and provider-specific caveats. Output should guide consumers without pretending to be a full database advisor.