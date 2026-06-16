<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this ticket to an implementation path: SQL Server latest-satellite remains a bounded single-ticket capability gap, and the visible repo evidence supports closing it with strategy, diagnostics, fallback, test, and evidence-surface updates rather than a no-work-required rejection.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository evidence still matches the documented gap: `AddDVaultSqlServer()` registers SQL Server optimized PIT/bridge reads only, `SqlServerDataVaultReadStrategy` does not currently implement `IDataVaultProviderReadStrategy`, and benchmark/docs still encode SQL Server latest-satellite as `providerSpecificReadStrategy=not registered for latest satellite reads`.
- No separate v0.41 criteria artifact was visible in the inspected repository surfaces, so this refinement treats the checked-in benchmark contract, evidence matrix, gap matrix, diagnostics tests, and performance guidance as the authoritative bounded criteria baseline.
- The visible evidence supports implementing the SQL Server latest-satellite improvement, not rejecting it as no-work-required; no checked-in repository evidence closes P0.02 already.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run. Live `gicket-read-ticket*` and relation reads were trust-policy blocked, so refinement relies on the supplied ticket snapshot plus bounded repository evidence.

### Scope In
- Add SQL Server provider-specific latest/current and as-of satellite read support behind the existing `IDataVaultReadService` boundary.
- Register the SQL Server latest read strategy through `AddDVaultSqlServer()` and keep diagnostics selection/fallback behavior aligned with the existing provider strategy model.
- Update SQL Server latest-satellite gate, fallback, benchmark-contract, and evidence-surface expectations so repository guidance no longer says the strategy is unregistered.
- Add or update SQL Server latest-satellite unit/integration coverage for registration, selection, fallback, and result parity against the provider-neutral path.
- Align repository planning/evidence docs that currently classify SQL Server latest-satellite as a capability gap so they match the implemented strategy and any remaining evidence-only follow-up posture.

### Scope Out
- PostgreSQL, MySQL, Oracle, and DB2 latest-satellite capability-gap work remains out of scope for this ticket.
- SQL Server PIT and bridge algorithm changes are out of scope except for keeping their existing behavior and shared strategy naming consistent.
- SQL Server save-strategy, bulk-threshold, staging-table, or provider-native chunking changes are out of scope.
- No new public API, query-planner surface, scheduler/maintenance runtime, benchmark schema, or deployable SQL artifact surface is introduced by this ticket.
- Do not widen provider claims into completed external-provider timing evidence when the SQL Server benchmark lane is still skipped or unconfigured.

## Acceptance Criteria
- `AddDVaultSqlServer()` registers a SQL Server latest-satellite provider read strategy through `IDataVaultProviderReadStrategy`, and supported SQL Server latest-satellite requests can select `SqlServerDataVaultReadStrategy` instead of falling immediately to `NoProviderSpecificStrategyRegistered`.
- The SQL Server optimized latest-satellite strategy is bounded to the visible v1 latest-read gate model: provider name must match SQL Server, only hub-parent satellites are supported, multi-active driving keys remain unsupported, and unsupported providers or shapes still fall back through the provider-neutral read pipeline with deterministic fallback causes.
- Unit and integration coverage prove SQL Server latest current/as-of satellite reads return the expected rows and projections for supported shapes, and cover registration, diagnostics selection, and fallback behavior for unsupported or mismatched cases.
- Benchmark-contract and artifact expectations are updated so the SQL Server `latest-satellite-read` row no longer advertises `providerSpecificReadStrategy=not registered for latest satellite reads`; when the SQL Server lane is not configured, the checked-in root artifact triplet may remain `skipped-placeholder` but must carry the correct planned/selected path and strategy tokens without claiming completed timing.
- Repository evidence/planning surfaces that currently describe SQL Server latest-satellite as P0.02 capability-gap work are updated or reclassified consistently with the implemented strategy, while other non-SQLite latest-satellite provider gaps remain explicitly out of scope.

## Definition of Done
- All touched latest-satellite strategy, diagnostics, benchmark-contract, and registration tests pass under normal repository validation, and any SQL Server opt-in smoke/integration tests follow the existing connection-string-gated conventions.
- The checked-in benchmark artifact triplet and dependent evidence/planning documents are internally consistent with the implementation and do not leave stale 'unregistered latest-satellite strategy' wording for SQL Server.
- Provider-neutral fallback behavior remains intact for unsupported latest-satellite requests, and the ticket does not widen the public surface or provider promises beyond SQL Server latest-satellite support.

## Implementation Notes
- Extend the existing `SqlServerDataVaultReadStrategy` type rather than introducing a new public read surface, so the strategy name stays `SqlServerDataVaultReadStrategy` across DI registration, diagnostics, benchmark detail tokens, and evidence documents.
- Mirror the existing optimized latest-satellite gate boundary already used by `EvaluateLatestSatellite(...)`: SQL Server provider-name match, hub-parent satellites only, and no multi-active driving keys unless this ticket also proves and documents a deliberate boundary expansion.
- Current tests and docs explicitly assume relational non-SQLite providers expose only optimized PIT/bridge reads. Update those expectations specifically for SQL Server latest-satellite without silently widening the same promise to PostgreSQL, MySQL, Oracle, or DB2.
- Use the existing SQL Server latest-hash-diff query patterns and tests in `SqlServerDataVaultSaveStrategy` as bounded local evidence for batched latest-per-parent SQL shaping, but keep the read feature on the established `IDataVaultReadService` and projection-helper surfaces.
- Update benchmark execution-detail mapping, `BenchmarkScenarioExecutionTests`, the provider evidence matrix, the provider gap matrix, and `docs/performance-profiles.md` so SQL Server latest-satellite moves out of the current 'strategy not registered' wording. If no configured SQL Server benchmark run is available, keep the row as skipped-placeholder and record planned strategy/path facts only.

## Open Questions
- none

## Follow-Up Questions
- After this capability gap closes, should any remaining SQL Server external-provider latest-satellite timing collection be tracked as a separate evidence-gap ticket, or deferred until the next provider benchmark refresh?
- Should the same latest-satellite strategy pattern later be applied to PostgreSQL, MySQL, or Oracle, or should those providers stay explicitly parked in the gap matrix until separate tickets are opened?

## Risks
- The SQL Server benchmark lane is opt-in and connection-string-gated; if local SQL Server execution is unavailable, reviewers must accept skipped-placeholder artifact evidence rather than completed external timing for this ticket.
- If code changes land without matching updates to the evidence matrix, gap matrix, benchmark expectations, and performance guidance, the repository will carry contradictory SQL Server latest-satellite claims.
- Live ticket comment/relation state could not be refreshed because the `gicket-read-ticket*` and relation tool calls were trust-policy blocked; no blocker is visible in the supplied snapshot, but relation housekeeping was not re-verified.

## Split Recommendations
- No split recommended; the visible repository evidence keeps this as one bounded SQL Server capability-gap task covering strategy registration, gating, diagnostics, tests, benchmark/evidence alignment, and fallback preservation.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use v0.39 evidence and v0.41 criteria to implement or reject a SQL Server latest-satellite read strategy improvement. Acceptance: tests, diagnostics, fallback, and benchmark evidence are updated, or no-work-required is documented.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- Implemented SQL Server latest/current/as-of satellite provider reads through the existing `SqlServerDataVaultReadStrategy` and `IDataVaultReadService` dispatch path.
- Registered the SQL Server strategy from `AddDVaultSqlServer()` as `IDataVaultProviderReadStrategy` while preserving existing PIT/bridge registrations.
- Kept the latest-read support boundary to SQL Server provider-name matches, hub-parent satellites, and non-multi-active shapes; unsupported shapes continue to fall back through provider-neutral reads with deterministic fallback causes.
- Updated benchmark detail tokens, root benchmark artifacts, tests, and evidence/planning documentation so SQL Server latest-satellite no longer appears as an unregistered capability gap. The root SQL Server latest-satellite row remains skipped when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset and does not claim completed timing.
- Validation passed for `bash tools/check-format.sh`, targeted `git diff --check`, and JSON parsing of `benchmark-summary.json`. Build/test execution without restore was blocked by missing local NuGet/cache assets (`Microsoft.EntityFrameworkCore.Analyzers` 8.0.28/10.0.9, `xunit.analyzers` 1.27.0, and net8.0 project assets); no restore was run under the automation boundary.
<!-- gicket-bot:developer-delivery:v1:end -->