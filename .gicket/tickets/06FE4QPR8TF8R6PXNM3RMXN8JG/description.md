<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified and resolved the only blocking PO-critic contradiction: the persisted ticket description now explicitly records the materialized description update and no other bounded planning writes were applied.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The authoritative delivery contract in the ticket description was already updated on the ticket owner branch to remove the internal contradiction called out by PO-critic.
- No child tickets, relation writes, attachments, or planning documents were materialized while resolving this finding.
- The existing downstream documentation split remains the same: broader evidence-matrix and release-note promotion work stays in 06FE4QRMXVGJVA65ZR5MZ817K8.
- The current contract still treats PostgreSQL latest-satellite timing as unmeasured until a provider-configured completed artifact or preserved benchmark comparison is available.

### Scope In
- Tune or explicitly retain the PostgreSQL latest-satellite read SQL shape for supported hub-parent, non-multi-active requests using preserved evidence rather than assumption.
- Capture a provider-configured PostgreSQL latest-satellite timing artifact or preserved benchmark comparison against the provider-neutral or historical fallback comparator before claiming a win.
- Keep IDataVaultReadDiagnosticsService output, benchmark execution-detail tokens, and tests aligned with the chosen PostgreSQL latest-satellite path so selected strategy versus fallback remains auditable.
- Preserve the current gate boundary: provider mismatch, unsupported satellite parent, multi-active driving keys, or diagnostics that do not select PostgresDataVaultReadStrategy must fall back.

### Scope Out
- Broad release-note, matrix, and adopter-document promotion work that already sits in downstream docs task 06FE4QRMXVGJVA65ZR5MZ817K8.
- Widening PostgreSQL latest-satellite support beyond hub-parent, non-multi-active shapes or changing PIT/bridge scope.
- Treating latest-satellite save-side index experiments or save-path benchmarks as proof of read-strategy improvement.
- Any measured external-provider timing claim without a preserved provider-configured artifact triplet or equivalent preserved run context.

## Acceptance Criteria
- For PostgreSQL latest-satellite reads, the implemented path is either a measured improvement or an evidence-backed retain-current decision, and the preserved artifact clearly shows the comparator used.
- If the PostgreSQL SQL shape changes, unit and integration coverage still proves provider-neutral parity for supported shapes and still rejects provider mismatch, link-parent satellites, and multi-active satellites with provider-neutral fallback.
- Benchmark or diagnostics output for the PostgreSQL latest-satellite lane makes the chosen path auditable with bounded tokens such as selectedStrategy, plannedReadStrategy, readShape=LatestSatellite, and fallback causes when applicable.
- No documentation or code in this ticket promotes the root skipped PostgreSQL latest-satellite row into completed timing evidence without a provider-configured completed run.
- Any targeted diagnostics or narrow developer-facing notes added here explain the chosen PostgreSQL path while leaving broader evidence-matrix and release-document promotion to the downstream docs ticket.

## Definition of Done
- A developer can point to one authoritative PostgreSQL latest-satellite decision: tuned SQL shape or explicit retention of the current windowed query, with preserved evidence for why.
- Repository tests cover the PostgreSQL latest-satellite command shape or selection behavior being kept, changed, or intentionally retained, plus fallback and parity behavior.
- The ticket leaves the provider boundary unchanged: PostgresDataVaultReadStrategy is diagnostics-gated and provider-neutral fallback remains the public safety net.
- Any evidence cited for the decision is stored as a preserved benchmark artifact or checked-in contract surface, not a transient local observation.
- Downstream docs work has enough bounded input to update matrices and release notes without reopening the strategy-selection decision.

## Implementation Notes
- Repository and ticket evidence still support the same bounded strategy baseline: PostgresDataVaultReadStrategy remains diagnostics-gated to provider-name match, hub-parent satellites, and non-multi-active shapes, with provider-neutral fallback outside that boundary.
- The current implementation baseline remains the windowed ROW_NUMBER() latest-row query described in the authoritative contract.
- The strongest historical comparator remains the checked-in v0.31 smoke artifact where PostgreSQL latest-satellite executed through provider-neutral fallback; the root benchmark row is still only a skipped placeholder and not completed timing evidence.
- The only materialized planning write in this PO correction pass was the ticket-description contract update already applied through gicket-update-ticket-description; no child tickets, relation mutations, attachments, or planning documents were added.

## Open Questions
- none

## Follow-Up Questions
- Should downstream docs task 06FE4QRMXVGJVA65ZR5MZ817K8 attach the eventual provider-configured PostgreSQL latest-satellite artifact directly once this ticket fixes the strategy decision?

## Risks
- Because the root PostgreSQL latest-satellite row is still a skipped placeholder, the team could overstate strategy registration as measured timing unless a provider-configured completed artifact or equivalently preserved comparator is stored.
- Changing the SQL shape without preserving parity and fallback coverage could drift latest-row semantics or diagnostics behavior.
- The strongest checked-in latest-index PostgreSQL numbers describe save-side lookup and index experiments, so using them as read-strategy proof would blur the evidence contract.

## Split Recommendations
- No additional PO split is needed; provider-specific latest-satellite tuning remains isolated to this ticket and broader documentation follow-through already exists in 06FE4QRMXVGJVA65ZR5MZ817K8.
- If relation normalization is wanted later, handle the historical done-ticket blocks link as separate housekeeping rather than widening this tuning ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: use normalized evidence to tune PostgreSQL latest-satellite strategy selection or SQL shape where justified. Acceptance: fallback remains available and docs/diagnostics explain the chosen path.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Decision
- Retain the current PostgreSQL latest-satellite command shape: the provider strategy continues to use the checked-in windowed `ROW_NUMBER()` query for supported hub-parent, non-multi-active latest-satellite reads.
- No completed PostgreSQL timing win is claimed in this ticket. The root PostgreSQL latest-satellite row remains a skipped placeholder when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset.
- The retained path is now auditable through `latestSatelliteSqlShape=windowed-row-number` alongside `selectedStrategy=PostgresDataVaultReadStrategy`, `plannedReadStrategy=PostgresDataVaultReadStrategy`, and `readShape=LatestSatellite`.

### Repository Delivery
- Tightened configured PostgreSQL latest-satellite benchmark selection so it must select `PostgresDataVaultReadStrategy` before timing.
- Preserved the retained SQL shape in `PostgresProviderCapabilityTests` and benchmark execution-detail assertions.
- Updated the root benchmark triplet and provider evidence/gap matrices to carry the retained-shape token without promoting the skipped placeholder into completed timing evidence.

### Verification
- `bash tools/check-format.sh` passed.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --filter PostgresStrategyRetainsWindowedLatestSatelliteReadSqlInsideProviderPackage` passed; the Microsoft Testing Platform ignored the VSTest filter and ran the full unit suite for net8.0 and net10.0.
- `timeout 180s dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --no-build -- --filter PostgresLatestSatelliteBenchmarkDetailsRecordRetainedWindowedSqlShapeAndStrategyGate` passed for net8.0 and net10.0.
- `timeout 600s dotnet build DVault.slnx --nologo --no-restore --disable-build-servers -maxcpucount:1` passed with warnings only.

### Evidence Boundary
- The checked-in root triplet records the PostgreSQL latest-satellite row as skipped and not executed while preserving selected/planned strategy and retained SQL-shape tokens.
- Historical provider-configured fallback artifacts remain comparator context only; they do not prove a measured win for the retained selected strategy.
- Completed PostgreSQL latest-satellite timing still requires a provider-configured artifact run with `DVAULT_TEST_POSTGRES_CONNECTION_STRING` set and diagnostics selecting `PostgresDataVaultReadStrategy`.

### Open Questions
- none

### Risks
- A future docs pass could still overstate the skipped root row unless it preserves the distinction between row-identity evidence and completed provider timing.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Decision
- Retain the current PostgreSQL latest-satellite command shape: the provider strategy continues to use the checked-in windowed `ROW_NUMBER()` query for supported hub-parent, non-multi-active latest-satellite reads.
- No completed PostgreSQL timing win is claimed in this ticket. The root PostgreSQL latest-satellite row remains a skipped placeholder when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset.
- The retained path is auditable through `latestSatelliteSqlShape=windowed-row-number` alongside `selectedStrategy=PostgresDataVaultReadStrategy`, `plannedReadStrategy=PostgresDataVaultReadStrategy`, and `readShape=LatestSatellite`.

### Rework Resolution
- Resolved the repair-loop quality failure by restoring the missing final newline in `benchmark-summary.csv`.
- The final repository diff for this repair is limited to `benchmark-summary.csv` and only removes the `No newline at end of file` condition.
- Downstream docs ticket `06FE4QRMXVGJVA65ZR5MZ817K8` remains the owner for broader matrix and release-document promotion.

### Repository Delivery
- Kept the PostgreSQL latest-satellite retained-shape benchmark artifact row intact with `latestSatelliteSqlShape=windowed-row-number`.
- Preserved the prior bounded source/test surfaces for strategy selection and retained SQL-shape assertions.
- Fixed the checked-in CSV artifact formatting so the configured repository format policy can pass.

### Verification
- `tail -c 1 benchmark-summary.csv | od -An -t x1` returned `0a`.
- `git diff -- benchmark-summary.csv` shows only the final-newline restoration.
- `git diff --name-only -- benchmark-summary.csv benchmark-summary.json benchmark-summary.md benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExecutionDetails.cs benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs docs/plans/provider-optimization-evidence-matrix.md docs/plans/provider-optimization-gap-matrix.md tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs` returned only `benchmark-summary.csv`.
- `git diff --check -- benchmark-summary.csv` passed.
- `bash tools/check-format.sh` passed with `Formatting check passed.`
- `dotnet build DVault.slnx --nologo` and `dotnet build DVault.slnx --nologo --no-restore --disable-build-servers -maxcpucount:1` were attempted in this repair run, but both were stopped after prolonged no-output stalls following `NU1900` read-only NuGet vulnerability-cache warnings.

### Evidence Boundary
- The checked-in root triplet still records the PostgreSQL latest-satellite row as skipped and not executed while preserving selected/planned strategy and retained SQL-shape tokens.
- Historical provider-configured fallback artifacts remain comparator context only; they do not prove a measured win for the retained selected strategy.
- Completed PostgreSQL latest-satellite timing still requires a provider-configured artifact run with `DVAULT_TEST_POSTGRES_CONNECTION_STRING` set and diagnostics selecting `PostgresDataVaultReadStrategy`.

### Open Questions
- none

### Risks
- Full solution build/test validation should be rerun by tester in the normal validation workspace because local build attempts stalled after NuGet cache warnings in this repair run.
- A future docs pass could still overstate the skipped root row unless it preserves the distinction between row-identity evidence and completed provider timing.
<!-- gicket-bot:developer-delivery:v1:end -->