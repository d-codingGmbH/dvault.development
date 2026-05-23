[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket has no unresolved Open Questions, but the durable contract regressed to the wrong request-bound diagnostics API surface and now conflicts with source, tests, and the latest PO refinement comment.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F492C50WM7V2NE0WZB3774XM/description.md:48-49 records `## Open Questions` as `- none`.
- .gicket/tickets/06F492C50WM7V2NE0WZB3774XM/description.md:13,23,28,36,44 says the baseline/request-bound owner is `IDataVaultDiagnosticsService` and removes claimed pre-existing `IDataVaultReadDiagnosticsService`.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:717-785 defines `IDataVaultDiagnosticsService` for metadata/save analysis, while :791-825 defines `IDataVaultReadDiagnosticsService` with request-bound Analyze overloads for latest satellite, registry latest, PIT, bridge, and registry bridge reads.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:71-96 resolves `provider.GetRequiredService<IDataVaultReadDiagnosticsService>()` for `ReadDiagnosticsPopulateReadShapeForExplicitRegistryPitAndBridgeRequests`.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:482-546 shows the existing public `DataVaultReadShapeDiagnostics` record family already exists and currently lacks `ProjectedColumns` and PIT `ReferencedSatelliteLookupCount`, so the requested delta is additive developer work rather than missing baseline evidence.
- .gicket/tickets/06F492C50WM7V2NE0WZB3774XM/comments/06F54T0G1323AYE9YJSGZ70KK0.md:10-29 says critic-item-1 through critic-item-7 were answered by anchoring to `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagnosticsService`, but `git show c541d4050735 -- .gicket/tickets/06F492C50WM7V2NE0WZB3774XM/description.md` shows that same handoff commit rewrote the durable contract away from that baseline.

Blocking findings
- The persisted delivery contract names the wrong existing compatibility surface for request-bound read diagnostics. It tells developers to preserve `IDataVaultDiagnosticsService` ownership and not require `IDataVaultReadDiagnosticsService`, but current source and tests show `IDataVaultReadDiagnosticsService` is already the public request-bound API.
- The ticket is internally inconsistent: the latest PO refinement comment says the baseline already includes `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagnosticsService`, while the current durable description removed that wording in commit `c541d4050735`. Developer handoff should not proceed on conflicting ticket artifacts.

Required PO actions
- Restore the durable contract to the actual source-backed baseline: `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagnosticsService` are existing public request-bound read diagnostics surfaces.
- Rewrite Acceptance Criteria, Definition of Done, and Implementation Notes so request-bound work references the correct service and proof points, including `IDataVaultReadDiagnosticsService` and `ReadDiagnosticsPopulateReadShapeForExplicitRegistryPitAndBridgeRequests`.
- Align the ticket title with the narrowed additive scope if new index-hint work is no longer intended; current source already exposes `ExpectedIndexBaseline` and `ExpectedTraversalIndexBaseline`.

Open issues ledger
- critic-item-1 [required-po-action] Restore the durable contract to the actual source-backed baseline: `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagnosticsService` are existing public request-bound read diagnostics surfaces.
- critic-item-2 [required-po-action] Rewrite Acceptance Criteria, Definition of Done, and Implementation Notes so request-bound work references the correct service and proof points, including `IDataVaultReadDiagnosticsService` and `ReadDiagnosticsPopulateReadShapeForExplicitRegistryPitAndBridgeRequests`.
- critic-item-3 [required-po-action] Align the ticket title with the narrowed additive scope if new index-hint work is no longer intended; current source already exposes `ExpectedIndexBaseline` and `ExpectedTraversalIndexBaseline`.
- critic-item-4 [blocking-finding] The persisted delivery contract names the wrong existing compatibility surface for request-bound read diagnostics. It tells developers to preserve `IDataVaultDiagnosticsService` ownership and not require `IDataVaultReadDiagnosticsService`, but current source and tests show `IDataVaultReadDiagnosticsService` is already the public request-bound API.
- critic-item-5 [blocking-finding] The ticket is internally inconsistent: the latest PO refinement comment says the baseline already includes `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagnosticsService`, while the current durable description removed that wording in commit `c541d4050735`. Developer handoff should not proceed on conflicting ticket artifacts.

Missing examples / edge cases
- No concrete depth-bounded bridge example is named even though the contract requires `depthProjection` when the request is depth-bounded.
- No concrete driving-key satellite example is named even though the contract requires conditional `drivingKeyProjection` emission.

Risky assumptions
- Assuming developers can safely treat `IDataVaultDiagnosticsService` as the request-bound compatibility anchor despite current source and tests proving `IDataVaultReadDiagnosticsService` already owns that surface.
- Assuming the title phrase `index hints` will not pull implementation back toward new index/provider advice, even though the current narrowed contract treats existing index baselines as already shipped behavior.

AC / test suggestions
- Once the baseline wording is fixed, keep request-bound assertions on `IDataVaultReadDiagnosticsService` for explicit and registry latest, PIT, and bridge requests and add checks for the new `ProjectedColumns` and PIT lookup count without changing current filter/order/index/provider semantics.
- Update the public API snapshot for additive members on `DataVaultSatelliteReadShapeDiagnostics`, `DataVaultPitReadShapeDiagnostics`, and `DataVaultBridgeReadShapeDiagnostics`.
- Keep support-bundle/export coverage proving the new facts serialize while hash-key and as-of request values remain redacted.

Implementation watchouts
- Extend the existing public read-shape records additively; do not re-home request-bound reads under a new service or the wrong existing service.
- Preserve existing `ReadShape` member meanings and the explicit-versus-registry-backed equivalence already exercised in unit and integration tests.
- Do not let the title or legacy draft expand this story back into broader join-count, predicate-decomposition, or provider-specific tuning work without a new PO refinement.

Non-blocking notes
- `## Open Questions` is already `none`, so the gate failure is source-accuracy regression in the durable contract, not unresolved product questions.
- The current narrowed additive delta itself is plausible because the existing public read-shape records do not yet expose `ProjectedColumns` or PIT `ReferencedSatelliteLookupCount`.

Split recommendations
- No split is required once PO restores the correct existing read-diagnostics baseline; the narrowed `ProjectedColumns` plus PIT lookup-count addition is still one bounded story.
- If Product still wants broader index/provider guidance beyond the existing index baseline fields, keep that as a separate follow-up story.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment