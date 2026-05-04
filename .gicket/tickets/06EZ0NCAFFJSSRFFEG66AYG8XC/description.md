<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to bound v1 provider comparison to SQLite as the required baseline plus opt-in PostgreSQL via DVAULT_TEST_POSTGRES_CONNECTION_STRING, kept SQL Server/Oracle/MySQL out of scope until they own optimized strategies, and retained existing child ticket 06EZ0NCGYCADKEYGR16J5PJFS0 as the SQLite artifact baseline; no new child tickets or planning documents were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The comparison baseline remains the provider-neutral fallback save path versus provider-specific strategies selected through the current strategy-dispatch architecture.
- SQLite is the required v1 benchmark baseline for every run.
- PostgreSQL is the only optional external provider in scope for v1 comparison reporting, and it is discovered by the DVAULT_TEST_POSTGRES_CONNECTION_STRING environment variable.
- SQL Server, Oracle, and MySQL are compatibility-only provider packages for this story and are out of scope until their own optimized strategy tickets exist.
- If PostgreSQL is not configured, its provider dependency is unavailable, or the connection cannot be opened, the artifact must still emit skipped PostgreSQL entries with normalized human-readable reasons.
- Existing child ticket 06EZ0NCGYCADKEYGR16J5PJFS0 remains the SQLite artifact baseline slice under this story; this refinement adds the external-provider boundary and discovery contract rather than replacing that work.

### Scope In
- Consolidate benchmark output so each scenario row or record shows provider, strategy, dataset size, change ratio, execution status, and measured results in one stable artifact.
- Support direct fallback-versus-optimized comparison for SQLite and for PostgreSQL when the PostgreSQL benchmark path is configured.
- Keep SQLite-only local benchmark execution useful when no external provider is configured.
- Emit explicit skipped PostgreSQL entries when the PostgreSQL benchmark path is not configured, not reachable, or otherwise unavailable.
- Document the exact provider-discovery contract, run prerequisites, and skipped-row interpretation guidance.

### Scope Out
- Benchmark comparison, skipped-row reporting, or configuration discovery for MySQL, Oracle, or SQL Server in this story.
- Implementing new provider-specific save optimizations or changing the public IDataVaultSaveService, IDataVaultProviderSaveStrategy, or provider capability profile contracts.
- Adding benchmark-specific provider CLI flags or alternate connection-input surfaces beyond the existing benchmark options and the reused DVAULT_TEST_POSTGRES_CONNECTION_STRING env var.
- Requiring every external provider package in normal local developer environments.
- CI orchestration, environment provisioning, dashboards, or long-term benchmark history beyond the single-run consolidated artifact.

## Acceptance Criteria
- A benchmark run produces a stable report artifact whose rows or records identify provider, strategy, dataset size, change ratio, execution status, and comparable measured results for each scenario.
- SQLite rows are always present as the required v1 baseline.
- PostgreSQL is the only optional external provider in v1; when DVAULT_TEST_POSTGRES_CONNECTION_STRING is present and the provider is reachable, the artifact includes comparable PostgreSQL fallback and optimized rows for the same scenario.
- When DVAULT_TEST_POSTGRES_CONNECTION_STRING is missing, the provider dependency is unavailable, or the PostgreSQL connection is unreachable, the artifact still includes skipped PostgreSQL entries with a normalized human-readable reason instead of silently omitting PostgreSQL.
- MySQL, Oracle, and SQL Server are not required comparison targets and do not need fallback-only or skipped rows in v1.
- Documentation explains the DVAULT_TEST_POSTGRES_CONNECTION_STRING prerequisite, SQLite-only local behavior, and how to interpret skipped PostgreSQL entries and fallback-versus-optimized comparisons.
- The artifact format is stable enough to archive as release evidence without manual reshaping of provider names, scenario fields, or skip semantics.

## Definition of Done
- Benchmark code or configuration under the benchmark area generates the consolidated provider report for SQLite and the optional PostgreSQL path described by the acceptance criteria.
- Representative validation shows SQLite scenarios in the consolidated report and shows either PostgreSQL comparison rows when configured or skipped PostgreSQL rows when not configured or unreachable.
- Automated validation or focused tests cover the stable report shape, DVAULT_TEST_POSTGRES_CONNECTION_STRING discovery behavior, and skipped-provider behavior so silent omission regressions are caught.
- Documentation updates are checked in with benchmark run instructions, the PostgreSQL environment-variable contract, and interpretation guidance.
- The resulting benchmark surface preserves the existing explicit save-service and provider-strategy boundaries and does not reopen compatibility-only providers inside the v1 artifact contract.

## Implementation Notes
- Use the current explicit save-service architecture as the benchmark target: the provider-neutral fallback writer is the baseline and registered IDataVaultProviderSaveStrategy implementations are the optimized variants when CanSave accepts the scenario.
- Treat DataVaultProviderCapabilityProfiles.Sqlite and the existing repo-local benchmark harness as the mandatory baseline surface for this story.
- Treat PostgreSQL as the only external v1 participant because AddDVaultPostgres registers PostgresDataVaultSaveStrategy, while the MySQL, Oracle, and SQL Server packages currently expose only compatibility registration surfaces with no optimized strategy registration.
- The benchmark runner configuration contract for v1 is a non-empty DVAULT_TEST_POSTGRES_CONNECTION_STRING process environment variable; the existing --iterations, --warmup, and --output flags remain the only benchmark CLI options in scope.
- Skipped PostgreSQL reporting should be first-class artifact output, not console-only diagnostics, and should use normalized reason categories such as not configured, provider dependency unavailable, or connection unreachable.
- Existing child ticket 06EZ0NCGYCADKEYGR16J5PJFS0 already covers the SQLite artifact surface; this story adds the provider-bounding and external-discovery contract needed for optional PostgreSQL comparison without widening to other providers.

## Open Questions
- none

## Follow-Up Questions
- Should later provider tickets add SQL Server, Oracle, or MySQL rows only after each provider owns an optimized strategy path?
- Should a later benchmark-infrastructure ticket introduce benchmark-specific env-var names or CLI overrides instead of reusing DVAULT_TEST_POSTGRES_CONNECTION_STRING?
- Should CI eventually provision PostgreSQL so release evidence includes more than the SQLite baseline by default?

## Risks
- Reusing a test-named environment variable for benchmarks may confuse users unless the README and skipped-row reasons are explicit.
- If skipped-provider reason normalization drifts, archived artifacts may still be hard to compare across machines.
- Absolute PostgreSQL timings can vary substantially across local environments, so the report must keep scenario metadata and skip semantics prominent.

## Split Recommendations
- Keep this story focused on the consolidated artifact plus the SQLite-required and PostgreSQL-optional contract; move SQL Server, Oracle, and MySQL expansion into separate provider tickets.
- If benchmark-specific configuration surfaces or CI provisioning grow beyond straightforward env-var discovery, split that infrastructure work into separate follow-up tickets rather than widening this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: make provider optimization results comparable across fallback, SQLite, and external providers.

Scope:
- Extend benchmark output so provider, strategy, dataset size, change ratio, and fallback/classic comparison are visible.
- Keep local benchmark execution useful even when external providers are unavailable.
- Record skipped provider benchmarks explicitly instead of silently omitting them.

Acceptance Criteria:
- Benchmark artifacts can compare fallback and optimized strategies for at least SQLite and any configured external providers.
- The report format is stable enough to support release-quality evidence.
- Documentation explains how to run provider-specific benchmarks and interpret skipped providers.