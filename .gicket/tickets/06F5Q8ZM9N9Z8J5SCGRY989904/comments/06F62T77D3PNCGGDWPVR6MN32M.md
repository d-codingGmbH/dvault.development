[gicket-bot] PO-critic review contract

Summary
- Contract is developer-ready: the persisted delivery contract is specific, `## Open Questions` is `none`, the Oracle baseline is directly evidenced in source, and the settled staging-SPI dependency is already done.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git rev-parse HEAD` returned `b696f6d73a01b055fee4ef605f106c195e2718a5`, matching the provided `scratch-source-ref`; `git log --oneline b696f6d73a01b055fee4ef605f106c195e2718a5..HEAD` and `git diff --name-only b696f6d73a01b055fee4ef605f106c195e2718a5..HEAD` returned no output.
- `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` shows `AddDVaultOracle()` registering `OracleDataVaultSaveStrategy` and `DataVaultProviderCapabilityProfiles.Oracle`.
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs` defines `OracleProviderName = Oracle.EntityFrameworkCore`, `MinimumOptimizedBatchOperationCount = 50`, `MaximumOptimizedSatelliteOperationCount = 10000`, builds unique-row plus satellite save plans, and contains Oracle array-binding support via `SupportsOracleArrayBinding(...)` / `ArrayBindCount`.
- `tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs` includes `AddDVaultOracleBulkStrategyPersistsOrderedHubLinkAndSatelliteBatchWhenConfigured()`, and `tests/DCoding.Data.DVault.Tests/Integration/OracleIntegrationTestConfiguration.cs` makes Oracle live tests opt-in behind `DVAULT_TEST_ORACLE_CONNECTION_STRING`.
- `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` each contain visible Oracle provider-native rows with `executionStatus=skipped`, skip reason `not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty.`, and planned strategy detail naming `OracleDataVaultSaveStrategy`.
- `docs/architecture/dvault-v1-explicit-save-service.md` and `docs/releases/v0.19.0.md` both keep `IDataVaultSaveService` as the public write boundary and state that staged provider bulk ingestion remains outside the current public claim set.
- The ticket snapshot lists `Recent comments (oldest to newest): <none>`, so there is no comment-thread ambiguity blocking handoff.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete example of the narrowest eligible staged Oracle shape versus a shape that must stay on direct Oracle execution would reduce interpretation drift, but the current contract is still actionable.
- A concrete cleanup/cancellation example for an Oracle environment that lacks required staging-object privileges would help test planning, but the risk is already called out in the ticket.

Risky assumptions
- A stale planning note at `docs/plans/provider-optimization-closure-alignment-follow-up.md` still describes Oracle optimization as hub/link-only and says Oracle capability-profile auto-registration is not proven; this ticket assumes developers will treat current source, `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and the delivery contract as authoritative.
- Oracle live execution may remain unavailable on some developer machines; the ticket assumes visible skipped Oracle rows are acceptable interim evidence until a configured Oracle environment is available, consistent with the existing benchmark artifact contract.

AC / test suggestions
- When implementation starts, persist one explicit before/after Oracle benchmark label that compares retained direct Oracle execution against any staged path under the shared artifact contract.
- Keep at least one proof case each for selected staged-path behavior, retained direct-path behavior, provider-neutral fallback, dirty-context decline, multi-active-satellite decline, below-50-operation decline, and over-10000-satellite decline.

Implementation watchouts
- Current branch history is unchanged from `scratch-source-ref` `b696f6d73a01b055fee4ef605f106c195e2718a5`; all implementation and evidence work still remains ahead. Per the role guidance, that is a developer-handoff watchout, not a PO blocker.
- Because `docs/releases/v0.19.0.md` and `docs/architecture/dvault-v1-explicit-save-service.md` still treat staged provider bulk ingestion as outside the current public claim set, any Oracle staged work must stay behind `AddDVaultOracle()` / `IDataVaultSaveService` and avoid widening public API claims.

Non-blocking notes
- The ticket already separates Oracle implementation scope from staged-bulk diagnostics ticket `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and broader benchmark-matrix ticket `06F5Q900FC0P3HBZP81CVK7264`.

Split recommendations
- No additional split is recommended now; keep a follow-up only if Oracle evaluation reveals materially different viable staging mechanisms with different privilege or cleanup assumptions.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment