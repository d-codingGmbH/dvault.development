[gicket-bot] PO refinement contract

Summary
- Verified repository, ticket, comment, and relation state locally. The durable description already matches the source-backed request-bound read-diagnostics baseline, keeps this as one additive read-shape diagnostics story, and leaves only ticket-title cleanup as non-blocking follow-up metadata work because no title-mutation tool is exposed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Current source and tests confirm the durable request-bound baseline already includes DataVaultDiagnosticsResult.ReadShape, DataVaultReadShapeDiagnostics, and IDataVaultReadDiagnosticsService, and the current authoritative description now states that same baseline.
- critic-item-2: `answered` - Acceptance Criteria, Definition of Done, and Implementation Notes already keep request-bound work on IDataVaultReadDiagnosticsService and use ReadDiagnosticsPopulateReadShapeForExplicitRegistryPitAndBridgeRequests plus support-bundle and public-API proof points to prove the additive extension.
- critic-item-3: `answered` - Source already exposes ExpectedIndexBaseline and ExpectedTraversalIndexBaseline, so net-new index-hint work is not intended. The authoritative description narrows scope accordingly; the stored title still says 'and index hints' because this run has no title-mutation surface, so title cleanup stays as separate metadata follow-up instead of reopening scope.
- critic-item-4: `answered` - The current request-bound public API is IDataVaultReadDiagnosticsService. IDataVaultDiagnosticsService remains the metadata and save-analysis surface, so compatibility language must preserve that split and the durable description now does.
- critic-item-5: `answered` - The earlier inconsistency from commit c541d4050735 has been superseded: the current durable description restores the same baseline that source and tests expose, and the recent human comment explicitly notes that the latest description already carries that source-backed clarification.

Clarifications
- Verified live relations remain unchanged: epic 06F492BTNHRPBC7D24E13ECFKM is parentOf this ticket, and this ticket still blocks 06F492CAB2293R7BGJWMWMRKT4 and 06F492D05THPGQVT3B3K7853A0.
- Verified the existing API split in source and tests: IDataVaultReadDiagnosticsService owns request-bound read Analyze calls and populates DataVaultDiagnosticsResult.ReadShape and DataVaultReadShapeDiagnostics; IDataVaultDiagnosticsService remains the metadata and save-analysis surface.
- Existing source already exposes ExpectedIndexBaseline and ExpectedTraversalIndexBaseline; this story treats them as baseline facts rather than net-new index-hint work.
- No child tickets, relation mutations, description writes, attachments, or planning documents were materialized or queued in this run because the durable description already matches the source-backed baseline and the exposed tool surface has no title-mutation operation.

Scope In
- Add projected-column group facts for latest, current, and as-of satellite read-shape diagnostics on the existing DataVaultDiagnosticsResult.ReadShape surface.
- Add projected-column group facts and ReferencedSatelliteLookupCount for PIT read-shape diagnostics on the existing DataVaultReadShapeDiagnostics family.
- Add projected-column group facts for bridge read-shape diagnostics on the existing request-bound read diagnostics surface.
- Preserve additive compatibility, explicit-versus-registry-backed equivalence, redaction behavior, and any existing public API snapshot coverage while extending the read-shape payload.

Scope Out
- No new root read-diagnostics service and no replacement of IDataVaultReadDiagnosticsService, DataVaultDiagnosticsResult.ReadShape, or DataVaultReadShapeDiagnostics.
- No net-new index-hint subsystem and no rename or semantic reset of ExpectedIndexBaseline or ExpectedTraversalIndexBaseline.
- No raw SQL, execution plans, live schema inspection, automatic index creation, or provider-specific physical-plan advice.
- No disclosure of request values such as parent hash keys or raw as-of timestamps in diagnostics or support-bundle output.
- No broader join-count, predicate-decomposition, provider package, analyzer, save-diagnostics, telemetry, or release-automation work in this story.

Open questions
- none

Follow-up questions
- If product still wants the stored ticket title to drop the historical 'and index hints' wording now that existing index baselines are baseline-only, should runtime or a human perform that metadata cleanup in a separate ticket-admin pass?
- If product later wants richer predicate decomposition, join-plan hints, or provider-specific tuning guidance beyond projected columns and PIT lookup counts, should that ship as a separate follow-up story?

Risks
- If implementation routes request-bound work through the wrong service or replaces the existing ReadShape carrier instead of extending it additively, current consumers may break.
- If projection role names vary by provider or request path, explicit and registry-backed diagnostics become harder to compare; keep role names deterministic and provider-neutral.
- If downstream readers follow the unchanged title instead of the delivery contract, they may assume net-new index-hint work that is out of scope until metadata cleanup occurs.

Split recommendations
- No split is required. This remains one bounded additive read-shape diagnostics refinement on top of the existing request-bound baseline.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment