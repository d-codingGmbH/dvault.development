[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract is detailed, matches the existing read-diagnostics/readShape surface in source and tests, and has no unresolved Open Questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- HEAD:.gicket/tickets/06F7Y0FZXX5J0G7G15681HVEBR/description.md records PO Handoff decision `ready_for_po_critic` and `## Open Questions` as `- none`.
- Comment `06F85FADA2ZX2A1GCFPF05W01C.md` restates the refined contract and records `acceptance-criteria items: 8`, `definition-of-done items: 4`, and `implementation-notes items: 8`.
- `git log --oneline --decorate -n 4` on branch `ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr` shows only workflow commits `1a707dc1e`, `05c3b9ef2`, and `16f58a161`; `git show --name-only -n 4` touches only `.gicket/tickets/06F7Y0FZXX5J0G7G15681HVEBR/*`, so this remains a pre-development contract branch.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs defines `IDataVaultReadDiagnosticsService.Analyze(...)` overloads and `DataVaultDiagnosticsResult` with sibling `ReadStrategy` and additive `ReadShape` properties.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs defines `DataVaultReadShapeKind` values `LatestSatellite`, `PitAsOf`, and `Bridge`, plus `DataVaultReadStrategyDiagnosticsStatus` values `NotEvaluated`, `ProviderStrategySelected`, and `ProviderNeutralFallback` and the finite `DataVaultReadStrategyFallbackCauseKind` vocabulary referenced by the ticket.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs defines the current satellite, PIT, and bridge read-shape payload records with provider, filter, ordering/selection, projected-column, row-identity, referenced-satellite, endpoint, and expected-index/traversal fields matching the contract's named surface.
- src/DCoding.Data.DVault/DataVaultActivityTracing.cs defines the reused request-mode terms `Current`, `AsOf`, and `Traversal`.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs asserts `ReadShape.Kind == PitAsOf`, provider-selected versus provider-neutral fallback, referenced satellites, filter columns, and expected PIT index baselines.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs asserts `ReadShape.Kind == Bridge`, `BridgeKind.ManyToMany`, endpoint filter details, traversal index baseline, and provider-neutral fallback behavior.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs `SupportBundleSerializesReadShapeWithoutRequestValues` verifies exported JSON contains `readShape` but omits `secret-customer-hash-key`; docs/releases/v0.16.0.md documents the same bounded export posture without raw request values or SQL text.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No reviewed sample JSON is persisted yet for non-applicable member omission, such as a bridge payload without `DepthPredicate` or a provider-neutral fallback payload without a selected strategy name.
- No concrete public example yet contrasts provider-selected and provider-neutral serialized `readShape` output side by side; current evidence is in tests and release prose.

Risky assumptions
- The contract assumes consumers understand that selected strategy identity remains on sibling `DataVaultDiagnosticsResult.ReadStrategy.SelectedStrategyName`; `DataVaultReadShapeProviderDiagnostics` currently carries provider/profile/status/fallback data but not that name.
- The contract assumes explanatory string members such as `SeriesSelectionRule`, `PitRowSelectionRule`, `SnapshotLookupBehavior`, and `SupportedEndpointRules` are bounded guidance text, not byte-for-byte stability promises.

AC / test suggestions
- Keep explicit serialization coverage for omission of non-applicable optional members, especially missing `DepthPredicate` and missing selected-strategy name in provider-neutral fallback cases.
- Keep representative read-shape serialization coverage for satellite current/as-of, PIT as-of, and bridge traversal payloads so the contract remains anchored to concrete JSON outputs.
- If release guidance is updated, include one reviewed redacted `readShape` JSON example that shows secret-bearing request values are omitted.

Implementation watchouts
- Do not introduce a second competing explain surface; keep the contract anchored to `IDataVaultReadDiagnosticsService.Analyze(...)`, `DataVaultDiagnosticsResult.ReadStrategy`, and additive `ReadShape`.
- Keep provider strategy facts on the existing finite vocabularies in `DataVaultReadStrategyDiagnosticsStatus` and `DataVaultReadStrategyFallbackCauseKind` rather than expanding into free-form provider prose.
- State clearly that expected index/traversal baselines are translated metadata guidance, not observed physical query-plan guarantees.
- Preserve the existing support-bundle redaction boundary: no raw request keys, raw hash keys, timestamps, SQL text, query plans, credentials, connection strings, provider error text, or exception text.

Non-blocking notes
- Comment `06F85FS11891A74JFK8XT4BR2M.md` shows this ticket is upstream for queued follow-up work on `06F7Y0GFY7TP3V4B76JB759KB0` and `06F7Y0GT7A5QT77TADMRZBVYN8`; those downstream queues do not block this ticket's own handoff.
- The branch history and touched paths are ticket-metadata only, which is consistent with a normal pre-development PO-critic gate.

Split recommendations
- None. The persisted contract already says no split is recommended, and the observed repository baseline supports one bounded contract story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment