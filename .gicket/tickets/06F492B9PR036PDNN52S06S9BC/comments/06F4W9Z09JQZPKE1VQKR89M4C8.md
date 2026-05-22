[gicket-bot] PO refinement contract

Summary
- Verified live ticket/comments/relations and current-branch source; the contract now relies only on evidenced APIs and treats query-shape diagnostics as one new additive DataVaultDiagnosticsResult member with new public model(s). No child tickets, relation writes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is anchored only to visible branch APIs: IDataVaultDiagnosticsService.Analyze(DbContext) remains request-unbound, IDataVaultReadDiagnosticsService exposes request-bound Analyze overloads for latest-satellite, registry latest-satellite, PIT, bridge, and registry bridge requests, and DataVaultDiagnosticsResult currently has constructor members Validation, Explain, SaveStrategy, and Issues plus init-only ReadStrategy. Query-shape diagnostics are therefore specified as one new additive member and new public model(s), not as a preexisting type.
- critic-item-2: `answered` - Resolved by narrowing the delivery contract to additive work only. The current branch exposes no public query-shape/read-shape diagnostics type, so this ticket does not assume one exists; it creates the new member/model(s) on DataVaultDiagnosticsResult while leaving the existing diagnostics service interfaces in place.
- critic-item-3: `answered` - The cited identifiers are source-backed exactly as follows: the five request-bound Analyze overloads exist on IDataVaultReadDiagnosticsService; DataVaultDiagnosticsResult exposes Validation, Explain, SaveStrategy, and Issues through its primary constructor and ReadStrategy as an init-only property; registry latest-satellite and registry bridge diagnostics already normalize to explicit request forms before analysis. No existing query-shape diagnostics type is evidenced, so the contract keeps that payload as new additive work.

Clarifications
- Request-unbound Analyze(DbContext) stays on IDataVaultDiagnosticsService; request-bound read diagnostics stay on IDataVaultReadDiagnosticsService and are limited to the visible latest-satellite, registry latest-satellite, PIT, bridge, and registry bridge overloads.
- Current/as-of helper coverage stays in scope through the existing latest-satellite request family because the helper extensions construct DataVaultLatestSatelliteReadRequest or DataVaultRegistryLatestSatelliteReadRequest; this ticket does not add separate current/as-of diagnostics APIs.
- Registry-backed latest-satellite and bridge diagnostics already resolve metadata and normalize to explicit DataVaultLatestSatelliteReadRequest and DataVaultBridgeReadRequest instances before analysis; PIT remains explicit-request only in this ticket.
- Live relations were verified: parentOf from epic 06F492A3MPSGP3KXDNZECN01QM, outgoing blocks to 06F492BNDPWS9P4EDSV0W7G6VM and 06F492C50WM7V2NE0WZB3774XM, and one incoming blocks relation from done ticket 06F492B40K7B0WWPKH8N3PPG3G; because that source ticket is done and this ticket snapshot is isBlocked=false, no relation write is required in this refinement pass.
- The latest blocking PO-critic contract comment 06F4V2FFXA8GKK81NZH62Y6BTW and relation-follow-up comment 06F4V2RDNTNG17W17N8J1Z0X2G were reviewed; no newer human comment changes scope.
- No child tickets, relation writes, attachments, description-side bounded writes, or planning documents were materialized in this refinement pass.

Scope In
- Add one new additive read-shape/query-shape diagnostics member on DataVaultDiagnosticsResult, with explicit public model(s), for request-bound latest/current/as-of satellite, PIT as-of, and bridge read diagnostics.
- Populate that new member through the existing IDataVaultReadDiagnosticsService.Analyze(...) overloads and the existing registry-backed latest-satellite and bridge diagnostics paths after registry resolution; PIT remains explicit-request only in this ticket.
- Serialize the new additive diagnostics member through the existing DataVaultSupportBundle and DataVaultSupportBundleExporter.ExportJson(...) path.
- Update docs, public API snapshot, unit tests, and integration tests for the additive read-shape diagnostics contract and provider caveat/fallback reporting.

Scope Out
- No new standalone diagnostics service, telemetry surface, CLI-only API, or replacement of existing IDataVaultDiagnosticsService or IDataVaultReadDiagnosticsService entry points.
- No new registry-backed PIT request type or registry-specific PIT diagnostics overload in this ticket.
- No change to read execution semantics, result row shapes, PIT or bridge maintenance behavior, or provider strategy selection rules.
- No raw SQL dump, live EXPLAIN/query-plan capture, request hash keys, or payload values in the new diagnostics payload.

Open questions
- none

Follow-up questions
- Should a later tooling ticket add a condensed query-shape identifier or CLI-friendly summary once the structured payload stabilizes?
- Should future provider-specific optimized read strategies add provider-native explainers beyond the provider-neutral read-shape facts once non-SQLite optimized read strategies exist?

Risks
- DataVaultDiagnosticsResult and dvault.support-bundle.v1 are stable public surfaces, so the new member and supporting model(s) must remain strictly additive and version-safe.
- If the new payload leaks raw SQL, request hash keys, or payload values, it breaks the existing redaction-safe support-bundle boundary.
- Registry-backed and explicit diagnostics must stay semantically equivalent after normalization or support bundles will diverge for the same logical read.
- Index guidance must stay derived from translated metadata rather than hand-maintained strings or it will drift from actual projected schema.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment