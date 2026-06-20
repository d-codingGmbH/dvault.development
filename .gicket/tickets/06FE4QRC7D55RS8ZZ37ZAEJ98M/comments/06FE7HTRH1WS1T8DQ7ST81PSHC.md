[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the persisted contract has no open questions, and repository evidence consistently supports the SQL Server 50/500 gate, provider-neutral fallback wording, and the review-only SQL artifact boundary.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket` returned ticket `06FE4QRC7D55RS8ZZ37ZAEJ98M` at revision `06FE7G2H7ZVGYTVPSNG8RPFD4M`; the persisted Delivery Contract says `PO Handoff` is `ready_for_po_critic` and `## Open Questions` is `none`.
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `0fc7805e09bf599c3ec8b550a4e7e86197de6c10`, and the earlier `git diff --name-only 0fc7805e09bf599c3ec8b550a4e7e86197de6c10..HEAD` returned no files, so the review surface matches the supplied scratch snapshot.
- `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs` sets SQL Server save gating to minimum 50 total operations and maximum 500 satellite operations, and `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs` carries the same `MinimumOptimizedBatchOperationCount = 50` and `MaximumOptimizedSatelliteOperationCount = 500` constants.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs` asserts SQL Server fallback causes `SqlServerMinimumOperationThreshold` and `SqlServerMaximumSatelliteOperationThreshold` and asserts gate requirements of `MinimumTotalOperationCount == 50` and `MaximumSatelliteOperationCount == 500`.
- `artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/sqlserver-threshold-decision.md` says the SQL Server gates stay unchanged at 50/500 and that fallback rows now report `executionPath=DVault provider-neutral fallback path`, `selectedStrategy=<none>`, and `candidateStrategies=SqlServerDataVaultSaveStrategy`.
- `src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs` validates SQL Server-only `provider-native-bulk-ingestion` diagnostics and emits `schemaVersion=dvault.sql-artifact.v1`, `status=review-only`, `deployment=not-generated`, `runtimeDispatch=not-generated`, `Transfer=SqlBulkCopy`, and `CleanupBoundary=temporary-staging-table`; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs` asserts the same manifest contract.
- `docs/plans/provider-optimization-gap-matrix.md` keeps SQL Server `latest-satellite-read` at `P0.02` as an evidence gap, while `P2.02` and `P3.02` close only SQL Server PIT/bridge evidence; `docs/releases/v0.32.0.md` also says smoke-read latest-satellite rows completed via provider-neutral fallback with `selectedStrategy=<none>`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The Implementation Notes reference `DataVaultSaveTelemetryExplanation` as a source surface; the repository file exists at `src/DCoding.Data.DVault/DataVaultSaveTelemetryExplanation.cs`, but the concrete type exposed there is `DataVaultSaveTelemetryExplanationCatalog`, so downstream work should treat that note as file-level guidance rather than an exact type identifier.
- Downstream work must keep reading SQL Server latest-satellite evidence through the matrix and release-note boundary together: `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.md:68` is completed execution, but it is still `selectedStrategy=<none>` and `ProviderNeutralFallback`, which the gap matrix deliberately does not promote to completed optimized timing evidence.

AC / test suggestions
- If downstream docs add concrete examples, cite both the below-threshold case `customer-profile-scale-10x1` and the over-cap cases `customer-profile-scale-1000x1` or `customer-profile-scale-100x10` from the threshold bundle so both SQL Server fallback conditions stay explicit.
- Keep the fallback vocabulary exact in docs and tests: `executionPath=DVault provider-neutral fallback path`, `saveStrategyStatus=ProviderNeutralFallback`, `selectedStrategy=<none>`, and `candidateStrategies=SqlServerDataVaultSaveStrategy`.

Implementation watchouts
- Do not describe SQL Server as the PostgreSQL/MySQL staged-provider lane; repo code and artifact surfaces describe native `SqlBulkCopy` with temporary staging-table cleanup instead.
- Do not turn SQL Server latest-satellite into a completed optimized timing claim; the smoke-read row completed on the provider-neutral path, and `docs/plans/provider-optimization-gap-matrix.md` still keeps that row in `P0.02`.
- The branch snapshot contains no file changes beyond the supplied scratch source ref, so this review only validates ticket clarity; downstream implementation still needs actual repository edits.

Non-blocking notes
- `gicket-read-ticket` shows the ticket is still `todo`, carries `critic-needed`, and has no assignees; that is normal pre-dev routing state, not a PO-quality blocker.
- `git rev-parse --abbrev-ref HEAD` returned `HEAD`, so the scratch review worktree is detached, but the commit matches the supplied scratch-source ref and is still a valid read-only review surface.

Split recommendations
- No split is required for developer handoff if the work stays limited to SQL Server threshold wording, fallback wording, and review-only artifact evidence clarification.
- If scope expands later, split provider-configured SQL Server bulk timing promotion, SQL Server latest-satellite timing evidence, and any deployable SQL artifact or runtime-dispatch proposal into separate tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment