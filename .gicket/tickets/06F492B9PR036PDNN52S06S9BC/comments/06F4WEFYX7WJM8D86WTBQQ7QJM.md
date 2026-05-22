[gicket-bot] PO-critic review contract

Summary
- Persisted contract is source-backed, has no unresolved open questions, and is ready for developer handoff as additive read/query-shape diagnostics work.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F492B9PR036PDNN52S06S9BC/comments/06F4V08FVVH8DP81G22CF3TS0G.md and .gicket/tickets/06F492B9PR036PDNN52S06S9BC/comments/06F4W9Z09JQZPKE1VQKR89M4C8.md mark critic-item-1/2/3 answered and restate the ticket as additive work, superseding the older blocking comment .gicket/tickets/06F492B9PR036PDNN52S06S9BC/comments/06F4V2FFXA8GKK81NZH62Y6BTW.md.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs shows `IDataVaultDiagnosticsService.Analyze(DbContext)` remains request-unbound and `IDataVaultReadDiagnosticsService` exposes exactly five request-bound overloads for `DataVaultLatestSatelliteReadRequest`, `DataVaultRegistryLatestSatelliteReadRequest`, `DataVaultPitAsOfReadRequest`, `DataVaultBridgeReadRequest`, and `DataVaultRegistryBridgeReadRequest`.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs normalizes registry-backed latest-satellite diagnostics to `new DataVaultLatestSatelliteReadRequest(...)` and registry-backed bridge diagnostics to `new DataVaultBridgeReadRequest(...)`; `rg -n RegistryPit|DataVaultRegistryPit src/DCoding.Data.DVault tests` returned no matches, matching the contract's explicit-request-only PIT scope.
- src/DCoding.Data.DVault/DataVaultReadServiceCurrentSatelliteExtensions.cs constructs `DataVaultLatestSatelliteReadRequest` and `DataVaultRegistryLatestSatelliteReadRequest` for current/as-of helpers, which supports the contract's latest-satellite-family clarification.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs defines `DataVaultDiagnosticsResult` with constructor members `Validation`, `Explain`, `SaveStrategy`, and `Issues`, plus init-only `ReadStrategy`, so the new read/query-shape payload is correctly framed as a fresh additive member rather than an existing surface.
- src/DCoding.Data.DVault/DataVaultSupportBundle.cs and src/DCoding.Data.DVault/DataVaultSupportBundleExporter.cs show support-bundle export already flows through `DataVaultDiagnosticsResult`, uses `JsonPropertyOrder`, camelCase serialization, and redaction before JSON output.
- tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs plus tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt provide the public-API snapshot mechanism, and existing diagnostics/read integration coverage already exists in tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs, DataVaultPitReadServiceSqliteTests.cs, and DataVaultBridgeReadServiceSqliteTests.cs.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit expected serialization example for the new member when `IDataVaultDiagnosticsService.Analyze(DbContext)` is request-unbound would reduce ambiguity between `null`, omitted, and default-object output.
- A concrete fallback example for an unsupported latest-satellite shape, not just PIT/bridge provider fallback, would make the provider-caveat requirement easier to validate.

Risky assumptions
- The new payload can surface useful query-shape facts without leaking raw SQL, request hash keys, or payload values; the contract forbids all of those.
- Index guidance is assumed to stay metadata-derived and provider-neutral; if developers hand-code strings, the support-bundle contract will drift from translated schema.

AC / test suggestions
- Add a regression that `IDataVaultDiagnosticsService.Analyze(DbContext)` leaves the new member unpopulated while each request-bound `IDataVaultReadDiagnosticsService.Analyze(...)` overload populates it.
- Add equivalence tests proving registry-backed latest-satellite and bridge diagnostics serialize the same logical read-shape payload as their normalized explicit request forms.
- Add support-bundle export tests for SQLite-selected and provider-neutral fallback cases across latest-satellite, PIT, and bridge diagnostics.

Implementation watchouts
- Keep the change strictly additive on `DataVaultDiagnosticsResult` and the public API snapshot; do not repurpose or rename `ReadStrategy`.
- Preserve the existing registry-to-explicit normalization path for latest-satellite and bridge diagnostics, and do not introduce a registry-backed PIT diagnostics overload in this ticket.
- Keep the new support-bundle JSON on the existing deterministic camelCase redaction path in `DataVaultSupportBundleExporter.ExportJson(...)`.

Non-blocking notes
- .gicket/tickets/06F492BNDPWS9P4EDSV0W7G6VM/ticket.json and .gicket/tickets/06F492C50WM7V2NE0WZB3774XM/ticket.json remain `todo`, while the incoming blocker source .gicket/tickets/06F492B40K7B0WWPKH8N3PPG3G/ticket.json is `done`; the current ticket itself is `isBlocked: false`.
- `git diff --name-only 44abc14ed6a9a3bd81d4b65c6f3df0e38fd47196..HEAD` returned no paths, so this review stayed at ticket/contract level rather than drifting into implementation review.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment