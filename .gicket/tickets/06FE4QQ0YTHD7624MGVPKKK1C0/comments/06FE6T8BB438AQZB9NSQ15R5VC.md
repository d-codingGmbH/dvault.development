[gicket-bot] PO-critic review contract

Summary
- Delivery contract is repository-backed, internally consistent, and has no unresolved Open Questions; the ticket is ready for developer handoff on SQL Server latest-satellite evidence/tuning.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4QQ0YTHD7624MGVPKKK1C0/description.md lines 29-50 define concrete acceptance criteria and definition-of-done items, and lines 49-50 show ## Open Questions = none.
- git -C /mnt/c/Projects/DVault log --oneline on the ticket branch shows PO handoff commit 47c5a9975 followed by PO-to-critic workflow commits; git show --stat 47c5a9975 changes only .gicket/tickets/06FE4QQ0YTHD7624MGVPKKK1C0/*, which is consistent with a refinement-only handoff.
- benchmark-summary.csv root row latest-satellite-read, SQL Server external provider, dvault-adddvaultsqlserver-optimized is skipped because DVAULT_TEST_SQLSERVER_CONNECTION_STRING is unset, and benchmark-summary.json lines 885-895 record selectedStrategy=SqlServerDataVaultReadStrategy, plannedReadStrategy=SqlServerDataVaultReadStrategy, readShape=LatestSatellite, and persistedOutcome=not executed.
- docs/plans/provider-optimization-evidence-matrix.md lines 235-238 and 283-285 explicitly bound SQL Server latest-satellite to skipped-placeholder guidance while citing the v0.32.0 smoke-read bundle as the completed-timing source only for SQL Server PIT/bridge reads.
- artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.csv rows 38-40 show SQL Server latest-satellite completed under provider-neutral fallback with selectedStrategy=<none>, while PIT and bridge completed with SqlServerDataVaultReadStrategy.
- src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultReadStrategy.cs lines 18-24 and 223-291 show the existing SQL Server latest-satellite path is real code, gated through EvaluateSqlServer(...), uses ROW_NUMBER() OVER (PARTITION BY ...), and batches parent hash keys against the 2100-parameter limit.
- src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs lines 350-357 and 861-870 constrain SQL Server latest/as-of optimized reads to matching provider names, hub-parent satellites only, and no driving-key-based multi-active shape.
- tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs lines 216-254 assert provider-strategy selection for supported SQL Server latest-satellite reads, provider-neutral fallback for unsupported link-parent shapes, and correct latest/current/as-of results; tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs lines 55-107 compare SQL Server latest/as-of rows and projections to the provider-neutral fallback.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs lines 317-338 and 457-459 preserve the SQL Server placeholder benchmark row identity plus the expected readShape=LatestSatellite and SqlServerDataVaultReadStrategy tokens.
- .gicket/relations/SR/C0/06FE4QP6FB892E7TJMB47A3MSR--06FE4QQ0YTHD7624MGVPKKK1C0--blocks.json records the shared prerequisite, and .gicket/relations/C0/K8/06FE4QQ0YTHD7624MGVPKKK1C0--06FE4QRMXVGJVA65ZR5MZ817K8--blocks.json records this ticket as the blocker for the downstream documentation follow-up.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not name a concrete evidence example near the SQL Server parent-hash batching ceiling; the risk is acknowledged, but developers should make sure at least one large-batch latest/as-of case is covered.
- The contract requires machine-readable fallback causes, but it does not spell out one concrete artifact example for the diagnostics-that-decline-provider-strategy path.

Risky assumptions
- A provider-configured SQL Server environment will be available during development; without DVAULT_TEST_SQLSERVER_CONNECTION_STRING, the repository can only preserve skipped-placeholder rows and cannot support a timing claim.
- Any allowed equivalent measured validation will preserve the same row identity and evidence-boundary discipline as the benchmark triplet, so downstream docs do not confuse latest-satellite work with the already-closed PIT/bridge timing bundle.

AC / test suggestions
- Capture evidence for both current/latest and as-of supported hub-parent reads under the existing dvault-adddvaultsqlserver-optimized lane before deciding whether to retain or change the SQL shape.
- Keep explicit negative-path coverage for provider mismatch, unsupported satellite parents, multi-active driving keys, and missing SQL Server configuration so fallback causes stay machine-readable.
- Preserve row-token alignment across benchmark artifacts, diagnostics, and tests for readShape=LatestSatellite, selectedStrategy=SqlServerDataVaultReadStrategy, and plannedReadStrategy=SqlServerDataVaultReadStrategy.

Implementation watchouts
- Do not reopen SQL Server PIT/bridge timing scope; the contract and evidence matrix treat that as already closed by the v0.32.0 smoke-read bundle.
- If the SQL text changes, preserve parity with the provider-neutral fallback for both latest and as-of reads and keep fallback behavior unchanged for unsupported shapes.
- Watch the batching boundary in SqlServerDataVaultReadStrategy: the current path reserves room for the optional as-of parameter under SQL Server's 2100-parameter limit.

Non-blocking notes
- Related prerequisite ticket 06FE4QP6FB892E7TJMB47A3MSR is done, and the downstream documentation ticket 06FE4QRMXVGJVA65ZR5MZ817K8 exists as a separate todo follow-up; the split described in the contract matches persisted relation data.
- The current branch state is ticket/refinement-only rather than implementation-bearing, which is acceptable for this pre-development PO-critic gate.

Split recommendations
- No additional split recommended; shared lane normalization is already done in 06FE4QP6FB892E7TJMB47A3MSR, this ticket owns SQL Server latest-satellite evidence/tuning, and 06FE4QRMXVGJVA65ZR5MZ817K8 remains the coordinated documentation follow-up.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment