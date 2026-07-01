<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the read-parity task around the current repository baseline: the branch already shows bounded provider read strategy registrations, parity/fallback tests, skipped-placeholder root rows, and the 2026-06-23 closure bundle, so no further read-ticket split is needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Treat `docs/plans/provider-optimization-gap-matrix.md` and `docs/plans/provider-optimization-evidence-matrix.md` as the authoritative decision and row-lookup surfaces for this ticket.
- The current completed-timing source for external-provider read closure is `artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/`; do not reopen those rows as unmeasured gaps.
- The repository-backed provider set for this task is PostgreSQL, SQL Server, MySQL, Oracle, and DB2, with SQLite as the local reference baseline.
- Latest-satellite scope stays limited to hub-parent, non-multi-active satellites; PIT and bridge scope stays limited to already-maintained read models with complete read-shape evidence and fresh maintenance.
- Provider-neutral fallback remains required for provider mismatch, unsupported shapes, incomplete read-shape evidence, or stale read-model maintenance signals.
- The incoming `blocks` relation from done ticket `06FH8RATZGZRVAJVC4ERV0ACYW` is historical routing context, not an active blocker, because the source ticket is `done` and this ticket is not marked blocked.

### Scope In
- Selected latest-satellite and maintained PIT/bridge read-path parity work for the repository-backed external-provider set already evidenced in the branch.
- Shared relational read-pipeline changes and provider-specific read tuning that preserve row/projection parity with the provider-neutral path.
- Unit and benchmark-verifier coverage that proves strategy selection, fallback boundaries, and correct evidence posture for completed versus skipped provider rows.

### Scope Out
- Save-path threshold or staged-bulk work owned by sibling ticket `06FH8RC9F0QEWF356WF7YYNNGM`.
- Documentation, performance-profile, and release-note work owned by sibling ticket `06FH8REKX113JRZQ42HEB1NVZ8`.
- Provider-specific PIT maintenance expansion, bridge-maintenance push-down, staged DB2 bulk, and provider-native chunk execution.
- Fresh provider benchmarking or infrastructure provisioning beyond the checked-in root triplet and 2026-06-23 closure bundle.

## Acceptance Criteria
- The covered provider extensions register the existing diagnostics-gated read strategies through the current `IDataVaultProviderReadStrategy`, `IDataVaultProviderPitReadStrategy`, and `IDataVaultProviderBridgeReadStrategy` seams instead of bypassing provider-neutral dispatch.
- For supported requests, selected provider latest-satellite and PIT/bridge candidate paths return the same rows and typed projections as the provider-neutral baseline; unsupported or stale/incomplete shapes fall back with finite read-strategy causes.
- Latest-satellite support remains bounded to hub-parent, non-multi-active satellites; PIT and bridge support remains bounded to already-maintained read models with complete read-shape evidence and fresh maintenance.
- Benchmark and verifier expectations preserve skipped-placeholder root rows with planned strategy facts when external-provider connection strings are unset and cite the 2026-06-23 closure bundle for completed PostgreSQL, SQL Server, MySQL, Oracle, and DB2 read timing.
- No ticket requirement widens this task into save-path work, public documentation work, or PIT/bridge maintenance expansion.

## Definition of Done
- Repository source and tests prove the selected read candidate paths, provider dispatch, and fallback boundaries for the intended provider and shape set.
- Checked-in evidence surfaces distinguish completed timing rows in the 2026-06-23 closure bundle from skipped-placeholder root guidance rows and do not promote skipped rows into timing claims.
- The existing parent story/read/save/doc split remains intact, with no remaining PO blocker about provider set, shape boundary, or evidence source.
- Any future DB2 PIT maintenance implementation remains a separate follow-up rather than being folded into this read ticket.

## Implementation Notes
- `src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs` is the shared PIT/bridge read pipeline; it already keeps the provider connection open through the read, batches hash-key lookups, and applies the PIT `asOf` cutoff in SQL before row selection.
- `src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs` is the authoritative gate/fallback seam for provider mismatch, unsupported shapes, incomplete read-shape evidence, and stale maintenance signals.
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs` carries the bounded Oracle read-command tuning (`InitialLOBFetchSize` and `FetchSize`) that the closure bundle cites for latest/PIT/bridge timing.
- Provider service extensions already register `IDataVaultProviderReadStrategy`, `IDataVaultProviderPitReadStrategy`, and `IDataVaultProviderBridgeReadStrategy` for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs` and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs` are the repository-backed parity and fallback coverage anchors.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` preserves the root skipped-provider read rows, closure-bundle citations, and matrix/performance-profile assertions for the read lanes.
- No bounded ticket or planning writes were materialized in this refinement run.

## Open Questions
- none

## Follow-Up Questions
- Should a later maintenance-focused child be created for the accepted DB2 ordinary hub-parent PIT full-rebuild lane rather than reopening closed read rows?
- Once the read and save implementation children finish, should the remaining parent-story blocking chain be simplified before closure?

## Risks
- If downstream work treats remaining fallback boundaries as open implementation gaps, this ticket will sprawl into PIT maintenance, bridge maintenance, or save-path work that already belongs elsewhere.
- The root `benchmark-summary.*` files still show skipped external-provider read rows; without explicit closure-bundle citation, reviewers can misread intentional placeholders as missing evidence.
- The stale inbound `blocks` relation from the done matrix-refresh ticket can confuse workflow history until it is cleaned up.

## Split Recommendations
- Do not split this ticket further; save-path work and documentation work already have separate bounded children.
- If the team wants additional implementation after this ticket, create one separate DB2 PIT maintenance child limited to `IBM.EntityFrameworkCore` ordinary hub-parent `RebuildAsync(...)` push-down through `IDataVaultProviderPitMaintenanceStrategy`.
- Any Oracle PIT maintenance reopen, MySQL PIT maintenance timing evidence, bridge-maintenance push-down, or DB2 staged-bulk follow-up should stay in separate later tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement selected read-path improvements from the refreshed gap matrix for latest-satellite, PIT, bridge, or maintenance-read shapes. Keep provider-specific SQL behind existing diagnostics-gated strategy selection and provider-neutral fallback. Add parity tests and skipped-provider evidence where live infrastructure is unavailable.