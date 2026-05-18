[gicket-bot] PO-critic review contract

Summary
- Ticket 06F2PGN4GPQCGC5WHZQBGP4SD0 is sufficiently refined for developer handoff: the delivery contract is specific, split boundaries match repository and relation evidence, and `## Open Questions` is explicitly `none`.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGN4GPQCGC5WHZQBGP4SD0/description.md` sets `PO Handoff` to `ready_for_po_critic` and `## Open Questions` to `- none`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` exposes `IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest)`, the `DataVaultBulkSaveRequest` and `DataVaultRegistryBulkSaveRequest` types, and `SaveRequestsAsync(...)` resolves requests before strategy dispatch and falls back to the built-in writer when no provider strategy accepts.
- `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs` defines `DataVaultProviderSaveStrategyContext.ResolvedRequests`, matching the contract statement that resolved per-request metadata is available to provider strategies.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` contains bulk `Analyze(DbContext, DataVaultBulkSaveRequest)` / `Analyze(DbContext, DataVaultRegistryBulkSaveRequest)` overloads plus `ProviderNeutralFallback`, aligning diagnostics with the same ordered-batch gate model.
- `README.md:204` documents that `DataVaultBulkSaveRequest` processes ordered save requests and keeps satellite `HashDiff` state in memory across the batch; `docs/architecture/dvault-v1-explicit-save-service.md:31-33` documents provider-strategy dispatch with provider-neutral fallback.
- Visible regression coverage matches the ticket scope: `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:76` checks hook resolution before provider-strategy execution for a bulk request, and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:601`, `:706`, and `:1236` cover bulk latest-HashDiff carry/chronology and the optimized-strategy registration boundary.
- `git rev-parse HEAD` and `git rev-parse 50ffc75e15db2e4815fbded46976fcd37998d94f` both returned `50ffc75e15db2e4815fbded46976fcd37998d94f`; `git diff --name-only 25dfcccb843b47adb0719719e796ebf9855074d9..HEAD` listed only `.gicket/tickets/06F2PGN4GPQCGC5WHZQBGP4SD0/**`, so the current branch history since PO refinement is ticket metadata only, not unresolved code churn.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Visible tests name bulk fallback coverage for `DataVaultBulkSaveRequest`, but there is no equally direct, named regression in the inspected files for `DataVaultRegistryBulkSaveRequest` bulk resolution.
- The contract does not explicitly call out whether the same bulk latest-state replay expectations should be proven for multi-active satellite batches with driving keys.

Risky assumptions
- Developer handoff assumes the effective caller-visible order is the order of `DataVaultBulkSaveRequest.Requests` plus the existing hub-then-link-then-satellite grouping inside each `DataVaultSaveRequest`, not arbitrary per-operation interleaving.

AC / test suggestions
- Add one explicit acceptance/test note for registry-backed ordered bulk saves so `DataVaultRegistryBulkSaveRequest` is covered as directly as the explicit bulk request surface.
- If multi-active satellite bulk semantics are meant to match single-series bulk fallback semantics, say that explicitly in AC/test language.

Implementation watchouts
- The ticket branch currently carries only PO/ticket metadata deltas since `25dfcccb843b47adb0719719e796ebf9855074d9`; developers should confirm the remaining implementation delta versus the already-visible source/test baseline before starting work.
- Keep fallback behavior aligned with the shared diagnostics/strategy gate model in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` and with the downstream split enforced by tickets `06F2PGNGVQ3TZZWSABAK5SNFK4` and `06F2PGNT7DF4DVNKYWDFZC8DEM`.

Non-blocking notes
- Visible comments under `.gicket/tickets/06F2PGN4GPQCGC5WHZQBGP4SD0/comments/` are automation/refinement/lease records; no additional human clarification thread was needed to close `Open Questions`.

Split recommendations
- No additional split is needed; the current ticket text and relation graph already isolate the provider-neutral fallback baseline from provider-native strategies, provider integration coverage, benchmarks, and broader documentation/release-note packaging.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment