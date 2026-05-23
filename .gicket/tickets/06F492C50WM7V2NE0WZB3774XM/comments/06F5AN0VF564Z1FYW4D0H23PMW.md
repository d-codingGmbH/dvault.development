[gicket-bot] PO refinement contract

Summary
- Current branch evidence directly confirms the existing read-diagnostics service and carrier types, so this remains one additive ReadShape extension with no bounded planning writes needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now anchors its API statements to source-backed existing surfaces: DataVaultDiagnostics.cs already defines IDataVaultReadDiagnosticsService for request-bound read Analyze overloads, DataVaultDiagnosticsResult.ReadShape, and DataVaultReadShapeDiagnostics, so the story extends those existing types additively rather than inferring a missing API.
- critic-item-2: `answered` - The current branch snapshot already contains the questioned public API and type surfaces. The contract is restated as additive work on the existing ReadShape records and baseline index fields, not creation of a new public API.
- critic-item-3: `answered` - The API split is directly visible: IDataVaultDiagnosticsService exposes metadata and save-analysis Analyze overloads, while IDataVaultReadDiagnosticsService exposes latest or registry latest, PIT, and bridge read Analyze overloads. Tests resolve IDataVaultReadDiagnosticsService from DI and assert ReadShape population for explicit and registry-backed requests.

Clarifications
- Live relation state remains unchanged: epic 06F492BTNHRPBC7D24E13ECFKM is parentOf this ticket and this ticket still blocks 06F492CAB2293R7BGJWMWMRKT4 and 06F492D05THPGQVT3B3K7853A0.
- The current branch already contains the public read-diagnostics baseline this story extends: DataVaultDiagnosticsResult exposes ReadShape, DataVaultReadShapeDiagnostics already models satellite, PIT, and bridge branches, and IDataVaultReadDiagnosticsService already owns request-bound read Analyze entry points.
- IDataVaultDiagnosticsService remains the metadata and save-analysis surface; this ticket does not create a new root diagnostics service.
- Existing source already defines ExpectedIndexBaseline on satellite and PIT read-shape diagnostics and ExpectedTraversalIndexBaseline on bridge read-shape diagnostics, so the historical index hints wording is legacy title text rather than a net-new subsystem requirement.
- No child tickets, relation mutations, description writes, attachments, or planning documents were applied or queued in this run because the branch snapshot and existing ticket context resolved the PO-critic findings without a split or durable planning artifact.

Scope In
- Add projected-column group facts to the existing satellite read-shape diagnostics branch exposed through DataVaultDiagnosticsResult.ReadShape for latest, current, and as-of satellite requests.
- Add projected-column group facts and ReferencedSatelliteLookupCount to the existing PIT read-shape diagnostics branch.
- Add projected-column group facts to the existing bridge read-shape diagnostics branch.
- Preserve additive compatibility, explicit-versus-registry-backed equivalence, redaction behavior, and the existing index or traversal baseline fields while extending the read-shape payload.

Scope Out
- No new public read-diagnostics service and no replacement of IDataVaultReadDiagnosticsService, DataVaultDiagnosticsResult.ReadShape, or DataVaultReadShapeDiagnostics.
- No net-new index-hint subsystem and no rename or semantic reset of ExpectedIndexBaseline or ExpectedTraversalIndexBaseline.
- No raw SQL, execution plans, live schema inspection, automatic index creation, or provider-specific physical-plan advice.
- No disclosure of request values and no broader join-count, predicate decomposition, analyzer, telemetry, or release-automation work in this story.

Open questions
- none

Follow-up questions
- If product still wants the stored ticket title to drop the historical and index hints wording now that existing index baselines are baseline-only, should runtime or a human perform that metadata cleanup in a separate ticket-admin pass?
- If product later wants richer predicate decomposition, join-plan hints, or provider-specific tuning guidance beyond projected columns and PIT lookup counts, should that ship as a separate follow-up story?

Risks
- If implementation routes request-bound work through the wrong service or replaces the existing ReadShape carrier instead of extending it additively, current consumers may break.
- If projection role names vary by provider or request path, explicit and registry-backed diagnostics become harder to compare; keep role names deterministic and provider-neutral.
- If downstream readers follow the unchanged title instead of the delivery contract, they may assume net-new index-hint work that is out of scope until metadata cleanup occurs.

Split recommendations
- No split is required. This remains one bounded additive read-shape diagnostics extension on top of the existing request-bound baseline already visible in the current branch.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment