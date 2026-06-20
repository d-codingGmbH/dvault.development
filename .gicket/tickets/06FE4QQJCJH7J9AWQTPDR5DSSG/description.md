<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around an evidence-backed deferral: current repository state proves Oracle latest-satellite strategy registration, SQL shape, parity, and finite fallback gates, but it still does not provide completed Oracle latest-satellite timing evidence. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Treat this as an evidence-and-decision ticket, not a PIT tuning implementation ticket: the immediate output is a verified recommendation on whether Oracle latest-satellite evidence is strong enough to support PIT tuning claims.
- The authoritative current capability baseline is newer than the historical v0.32.0 smoke-read artifact dated 2026-06-07: current code and tests show Oracle latest-satellite strategy registration and parity, but that historical artifact still matters because it is the last checked-in configured Oracle latest-satellite run and it completed through provider-neutral fallback with selectedStrategy=<none>.
- Current root benchmark guidance remains non-timing evidence for Oracle latest-satellite: benchmark-summary.csv keeps the Oracle latest-satellite row as a skipped placeholder when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, even though the row now names OracleDataVaultReadStrategy as the planned strategy.

### Scope In
- Verify the current Oracle latest-satellite capability posture from repository code, tests, benchmark artifacts, and planning docs.
- Document the Oracle latest-satellite SQL shape used by the provider strategy for supported requests.
- Document the finite runtime fallback and diagnostics boundary for Oracle latest-satellite reads and the separate benchmark-evidence stop conditions.
- Decide whether Oracle latest-satellite evidence is strong enough to support PIT tuning claims now, or whether that tuning claim should be deferred.

### Scope Out
- Rerunning Oracle benchmarks or provisioning DVAULT_TEST_ORACLE_CONNECTION_STRING.
- Implementing Oracle PIT tuning or changing provider read/write code.
- Broadening Oracle latest-satellite support beyond hub-parent, non-multi-active satellites.
- Adding automatic PIT maintenance, new public read APIs, or release-document changes outside this ticket's refinement contract.

## Acceptance Criteria
- The ticket records that Oracle latest-satellite timing remains an evidence gap in the current checked-in root benchmark baseline: the Oracle row is skipped when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, and the last checked-in configured Oracle latest-satellite artifact on 2026-06-07 completed through provider-neutral fallback with selectedStrategy=<none>.
- The ticket records the current repository-proven Oracle latest-satellite request boundary: AddDVaultOracle registers OracleDataVaultReadStrategy, and the strategy is only eligible for Oracle provider hub-parent non-multi-active satellite reads.
- The ticket records the selected Oracle latest-satellite SQL shape at a planning level: a ROW_NUMBER window over ParentHashKey ordered by LoadTimestamp descending, with parent-hash-key batching and an optional as-of timestamp predicate.
- The ticket records the fallback boundary needed for downstream tuning decisions: provider mismatch, link-parent satellites, multi-active satellites, or diagnostics not selecting the Oracle strategy fall back to provider-neutral reads; missing configured benchmark evidence prevents timing promotion even when capability exists.
- The ticket makes an evidence-backed decision to defer Oracle latest-satellite-driven PIT tuning claims until a provider-configured Oracle latest-satellite benchmark lane exists, while allowing PIT work that does not depend on that missing timing claim.

## Definition of Done
- PO handoff distinguishes current capability evidence from measured timing evidence so downstream work does not overclaim Oracle latest-satellite performance.
- The next reviewer can see, without reopening architecture questions, why Oracle latest-satellite is a bounded evidence-gap follow-up rather than a fresh strategy-design problem.
- The refinement contract captures that current Oracle PIT and bridge evidence can stand independently from the unresolved Oracle latest-satellite timing lane.
- No additional child split, relation cleanup, attachment, or planning document is required for this ticket to proceed to PO critic.

## Implementation Notes
- src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs registers OracleDataVaultReadStrategy for IDataVaultProviderReadStrategy, IDataVaultProviderPitReadStrategy, and IDataVaultProviderBridgeReadStrategy.
- src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs builds Oracle latest-satellite reads with ROW_NUMBER() OVER (PARTITION BY ParentHashKey ORDER BY LoadTimestamp DESC), IN-batched parent hash keys, and an optional <= asOf filter before returning provider-normalized rows.
- src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs constrains Oracle latest-satellite reads to Oracle provider hub-parent non-multi-active shapes; the explicit latest-satellite gate requirements are ProviderNameMismatch, UnsupportedSatelliteParent, and MultiActiveSatelliteUnsupported.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs prove Oracle latest/as-of eligibility and parity against provider-neutral fallback for supported shapes.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs expects the current skipped Oracle latest-satellite guidance row to carry readShape=LatestSatellite plus selectedStrategy/plannedReadStrategy=OracleDataVaultReadStrategy, which confirms current row identity but not measured timing.
- docs/plans/provider-optimization-gap-matrix.md P0.04 keeps Oracle latest-satellite as an evidence gap, and docs/plans/provider-optimization-evidence-matrix.md plus docs/releases/v0.41.0.md and docs/releases/v0.32.0.md already separate current registration/parity from historical 2026-06-07 fallback evidence.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement run.

## Open Questions
- none

## Follow-Up Questions
- When an Oracle-configured benchmark environment is available, should the next evidence-gap ticket collect a dedicated Oracle latest-satellite comparator before any Oracle PIT tuning claim is promoted in release-facing docs?
- If Oracle PIT tuning proceeds before Oracle latest-satellite timing exists, should release documentation explicitly call out that PIT evidence is accepted independently from the missing latest-satellite timing lane?

## Risks
- If downstream PIT tuning work treats current Oracle latest-satellite capability evidence as equivalent to measured timing evidence, release or performance guidance could overclaim Oracle read performance.
- The historical 2026-06-07 smoke-read artifact still shows provider-neutral fallback for Oracle latest-satellite, so documentation must clearly distinguish that historical configured run from the newer v0.41+ registration and parity baseline.
- Until a configured Oracle latest-satellite benchmark lane exists, provider-specific tuning thresholds for adjacent read models can be justified, but end-to-end Oracle latest-satellite improvement claims remain unproven.

## Split Recommendations
- No additional split is justified from current evidence; the existing Oracle latest-satellite evidence-gap track is already bounded by docs/plans/provider-optimization-gap-matrix.md P0.04.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: verify Oracle latest-satellite timing, selected SQL shape, fallback behavior, and diagnostics before PIT tuning. Acceptance: tuning or deferral decision is evidence-backed.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- No repository file changes are required for this developer pass. The checked-out branch already satisfies the delivery contract for Oracle latest-satellite evidence and fallback documentation.
- The implementation decision remains an evidence-backed deferral: Oracle latest-satellite capability is repository-proven, but measured Oracle latest-satellite timing is still missing.

### Verified Decision
- Defer Oracle latest-satellite-driven PIT tuning claims until a provider-configured Oracle latest-satellite benchmark lane exists.
- Allow Oracle PIT or bridge work that cites its own maintained-shape evidence independently from the missing latest-satellite timing lane.

### Verified Repository Evidence
- `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` registers `OracleDataVaultReadStrategy` for `IDataVaultProviderReadStrategy`, `IDataVaultProviderPitReadStrategy`, and `IDataVaultProviderBridgeReadStrategy`.
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs` builds Oracle latest-satellite reads with `ROW_NUMBER() OVER (PARTITION BY ParentHashKey ORDER BY LoadTimestamp DESC)`, parent-hash-key `IN` batching, and an optional `<= asOf` load-timestamp predicate.
- `src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs` keeps latest-satellite optimized reads bounded to matching provider, hub-parent satellite, and non-multi-active shapes.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs` covers Oracle latest-satellite accept/fail-closed gate behavior.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs` compares Oracle latest/as-of latest-satellite rows and projections against provider-neutral fallback behavior for supported shapes.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` expects the Oracle latest-satellite guidance row to preserve `readShape=LatestSatellite`, `selectedStrategy=OracleDataVaultReadStrategy`, and `plannedReadStrategy=OracleDataVaultReadStrategy`.
- `benchmark-summary.csv` keeps the current Oracle latest-satellite row skipped because `DVAULT_TEST_ORACLE_CONNECTION_STRING` is unset; the row is guidance, not measured timing.
- `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.csv` preserves the 2026-06-07 Oracle latest-satellite configured artifact as provider-neutral fallback with `selectedStrategy=<none>`.
- `docs/plans/provider-optimization-gap-matrix.md` keeps Oracle latest-satellite at P0.04 as an evidence gap.
- `docs/plans/provider-optimization-evidence-matrix.md`, `docs/releases/v0.41.0.md`, and `docs/releases/v0.32.0.md` preserve the distinction between current registration/parity and missing latest-satellite timing evidence.

### Verification
- `git diff --name-only -- <expected evidence paths>` returned no repository-path diff after verification.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --no-restore --filter FullyQualifiedName~DataVaultProviderReadStrategyTests` completed successfully for both target frameworks. Microsoft.Testing.Platform ignored the VSTest filter, so the full unit project ran: `net8.0` passed 583/583 and `net10.0` passed 617/617.
- `dotnet test DVault.slnx --nologo` was attempted first but interrupted after restore/build warnings and no progress for several minutes; the warnings were NuGet vulnerability-cache writes blocked by the sandboxed global HTTP cache path.

### Open Questions
- none

### Risks
- Downstream work must not treat the skipped current Oracle latest-satellite guidance row as measured timing evidence.
- Downstream work must not treat the 2026-06-07 Oracle PIT/bridge timing closure as Oracle latest-satellite timing closure.

<!-- gicket-bot:developer-delivery:v1:end -->