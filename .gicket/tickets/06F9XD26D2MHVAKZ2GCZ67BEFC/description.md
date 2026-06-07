<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the v0.32.0 all-provider baseline task against the shared benchmark artifact contract, the completed v0.31.0 scale seed bundle, and the existing cleanup-fix smoke evidence; clarified that the scale run supplies threshold-tuning evidence while a separate smoke/read verification covers bridge-traversal cleanup, and no persistent ticket or planning write was materialized in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The authoritative artifact format remains the benchmark-summary.md / benchmark-summary.csv / benchmark-summary.json triplet from one execution, stored under a new v0.32.0 ticket-labeled artifacts/benchmarks path while keeping artifacts/benchmarks/v0.31.0-scale-5-all-providers-20260606 available for comparison.
- Repository evidence shows that --scale is limited to customer-profile scale scenarios, so the required --provider all --scale --iterations 5 --warmup 1 run is the threshold-baseline bundle and does not by itself satisfy bridge/PIT read verification.
- Repository evidence also shows that SatCustomerStatu is an intentional current fixture/model baseline and that artifacts/benchmarks/v0.31.0-all-providers-smoke-after-cleanup-fix-20260606 already proved completed bridge-traversal rows across PostgreSQL, SQL Server, MySQL, and Oracle after the cleanup fix; this ticket should repeat that bounded verification or record equivalent evidence alongside the scale bundle.
- The downstream tuning ticket set already exists and is the consumer of this evidence: story 06F9XD1T3TJK7NEBYNVT2JEPZW plus tasks 06F9XD2M71D1XFT7FJX62KD8HM, 06F9XD2TGEYEG6S0AK86YF295M, and 06F9XD33MNNVHHW232TC7T1CN8.
- No child-ticket creation, relation mutation, description update, attachment, or planning-document write was materialized in this refinement pass.

### Scope In
- Run the benchmark harness with --provider all --scale --iterations 5 --warmup 1 against SQLite, PostgreSQL, SQL Server, MySQL, and Oracle using the documented Podman-backed provider endpoints.
- Persist the resulting v0.32.0 artifact triplet under a ticket-labeled artifacts/benchmarks path and preserve the existing v0.31.0 scale bundle for side-by-side comparison.
- Perform a bounded all-provider smoke/read verification that proves bridge-traversal rows stay green after the SatCustomerStatu cleanup fix across the shared external databases.
- Record the concrete threshold-driving rows and fallback causes that the existing downstream tuning tickets must cite.

### Scope Out
- Changing provider thresholds, save/read strategy code, or other product behavior while capturing the baseline.
- Replacing the shared root benchmark-summary.md / .csv / .json rollup as part of this ticket unless a separate follow-up explicitly decides to do so.
- Adding DB2 or any new provider lane beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Automatic database orchestration, deployment tooling, or non-benchmark operational automation around the Podman containers.

## Acceptance Criteria
- A new v0.32.0 ticket-labeled artifact bundle exists under artifacts/benchmarks with benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json from one --provider all --scale --iterations 5 --warmup 1 execution, preserving the run-context and row fields required by docs/plans/performance-evidence-benchmark-artifact-contract.md.
- The scale artifact reports completed rows for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle, or preserves explicit skipped/failed rows with normalized reasons; provider lanes stay visible rather than disappearing.
- Separate bounded smoke/read evidence proves that bridge-traversal rows remain green across the shared external providers after the SatCustomerStatu cleanup fix, following the existing v0.31.0-all-providers-smoke-after-cleanup-fix-20260606 precedent or an equivalent recorded evidence surface.
- The ticket comment or committed planning/docs note identifies the concrete rows that justify the downstream tuning work, at minimum covering SQL Server rows with SqlServerMinimumOperationThreshold or SqlServerMaximumSatelliteOperationThreshold, Oracle customer-profile-scale-10000x10 with OracleMaximumSatelliteOperationThreshold, PostgreSQL small-batch optimized-overhead rows, and MySQL small-batch threshold/overhead rows.
- The run uses the existing Podman-backed provider endpoints rather than unrelated host-local services; when the benchmark runs inside a .NET SDK container, PostgreSQL is reached through the Podman network.
- No product behavior is changed; this ticket captures benchmark and verification evidence only.

## Definition of Done
- The v0.32.0 scale evidence bundle and the cleanup-verification evidence are persisted on approved repository/ticket surfaces and are readable without reopening scope questions about artifact naming or required files.
- The contract makes clear that the scale-mode bundle is the threshold baseline and that bridge-traversal cleanup verification is a separate bounded smoke/read proof, so implementation does not have to guess how to satisfy both requirements.
- The preserved v0.31.0 scale bundle remains available as the comparison seed for follow-up tuning work.
- The existing downstream tuning tickets can cite the recorded v0.32.0 rows without needing another PO clarification pass on which provider rows matter.
- No source or test behavior changes are required unless a purely operational access issue blocks the documented Podman endpoints, and any such issue must be recorded as evidence rather than silently worked around.

## Implementation Notes
- Use benchmarks/DCoding.Data.DVault.Benchmarks/README.md plus BenchmarkRunner/BenchmarkOptions as the authoritative proof that --scale narrows the matrix to customer-profile scale scenarios and that --output emits deterministic benchmark-summary.md / .csv / .json filenames.
- Use artifacts/benchmarks/v0.31.0-scale-5-all-providers-20260606 as the seed comparison bundle and preserve it unchanged.
- Use artifacts/benchmarks/v0.31.0-all-providers-smoke-after-cleanup-fix-20260606 as the historical template for the cleanup-fix verification; that bundle already shows completed bridge-traversal-read rows across PostgreSQL, SQL Server, MySQL, and Oracle at 1 iteration and 0 warmup.
- BenchmarkDatabaseProviders.cs and DataVaultPitMaintenanceRowGenerationTests.cs confirm that SatCustomerStatu is an intentional current repository baseline, so this ticket verifies cleanup behavior around that existing identifier rather than renaming it.
- The required row callout should map the observed v0.32.0 evidence to the existing downstream tickets 06F9XD1T3TJK7NEBYNVT2JEPZW, 06F9XD2M71D1XFT7FJX62KD8HM, 06F9XD2TGEYEG6S0AK86YF295M, and 06F9XD33MNNVHHW232TC7T1CN8.
- Do not rely on the checked-in root benchmark-summary triplet as the authoritative all-provider baseline for this ticket; current repository docs still describe that root rollup as the lightweight shared baseline with skipped optional external-provider rows.
- No persistent ticket or planning mutations were applied in this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- After the v0.32.0 evidence bundle lands, should a separate docs/release task promote any completed all-provider rows into the root benchmark-summary rollup or keep the root triplet as the lightweight shared baseline?
- Should follow-up tuning tickets preserve raw command stdout/stderr logs beside their before/after artifact triplets for rerun auditing even though the artifact contract itself only requires the summary triplet?
- When the downstream tuning tasks capture before/after evidence, should they reuse one shared v0.32.0 baseline label or keep provider-specific before/after bundles per task?

## Risks
- The checked-in root benchmark-summary triplet still reflects a skipped external-provider posture, so downstream work may cite the wrong baseline if the new v0.32.0 bundle is not explicitly referenced.
- Because --scale does not execute bridge or PIT read rows, relying on the scale artifact alone would miss the SatCustomerStatu cleanup verification for bridge-traversal-read.
- External-provider results depend on live Podman endpoints and conditional provider packages; misconfigured connection strings or running PostgreSQL outside the Podman network can produce false skips or failures unrelated to product behavior.
- If the implementer tweaks thresholds or provider code while capturing this baseline, the evidence becomes unusable as a pre-tuning snapshot and undermines the downstream comparison tasks.

## Split Recommendations
- No new split is justified; keep this ticket as the evidence-capture prerequisite for the existing tuning story 06F9XD1T3TJK7NEBYNVT2JEPZW and tasks 06F9XD2M71D1XFT7FJX62KD8HM, 06F9XD2TGEYEG6S0AK86YF295M, and 06F9XD33MNNVHHW232TC7T1CN8.
- If bridge-traversal cleanup verification expands beyond a quick bounded smoke/read rerun, create a separate validation-only follow-up instead of widening this baseline ticket into product-code or diagnostics work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Capture a clean v0.32.0 all-provider benchmark baseline before threshold or provider-path tuning begins.

Scope:
- Run the benchmark harness with `--provider all --scale --iterations 5 --warmup 1`.
- Preserve the artifact triplet under a v0.32.0/ticket-specific path, and keep the prior seed run `artifacts/benchmarks/v0.31.0-scale-5-all-providers-20260606` available for comparison.
- Verify that the external-provider cleanup fix for `SatCustomerStatu` keeps bridge traversal rows green across shared external databases.

Podman test environment:
- Use the existing containers named `postgres`, `sqlserver`, `mysql`, and `oracle`.
- PostgreSQL must be reached through the Podman network. When using a .NET SDK container, run it on the Podman network and point `DVAULT_TEST_POSTGRES_CONNECTION_STRING` at the PostgreSQL container IP or resolvable Podman network name.
- SQL Server, MySQL, and Oracle should likewise use the Podman container endpoints rather than relying on an unrelated developer machine service.

Acceptance criteria:
- The run reports completed rows for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle, or records an explicit operational skip/failure reason in the artifacts.
- The artifact summary calls out fallback causes for SQL Server, Oracle, PostgreSQL, and MySQL provider-optimized rows.
- The ticket comment or committed docs identify the concrete rows that justify subsequent threshold-tuning tasks.
- No product behavior is changed in this baseline-only task.