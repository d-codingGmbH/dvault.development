[gicket-bot] PO-critic review contract

Summary
- Delivery contract is coherent, tied to direct repository evidence, and ready for developer handoff; no persisted PO questions remain.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- PO comment .gicket/tickets/06F9XD26D2MHVAKZ2GCZ67BEFC/comments/06FA3AK940TJBF2GGC4CTPP4XM.md explicitly hands off with decision `ready_for_po_critic`, and no later ticket comment reopens scope or questions.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkOptions.cs parses `--scale`, BenchmarkRunner.cs routes `options.ScaleMatrix` to `CreateScaleBenchmarks(...)`, and `CreateScaleBenchmarks(...)` only iterates `CustomerProfileBulkScenarios.ScaleMatrix`; the scale run is customer-profile-only and cannot satisfy read verification by itself.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs writes deterministic `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`, matching both the ticket AC and docs/plans/performance-evidence-benchmark-artifact-contract.md.
- artifacts/benchmarks/v0.31.0-scale-5-all-providers-<redacted>/benchmark-summary.md exists and shows all optional providers completed plus the seed fallback rows the contract calls out: SQL Server `SqlServerMinimumOperationThreshold` / `SqlServerMaximumSatelliteOperationThreshold`, Oracle `customer-profile-scale-10000x10` with `OracleMaximumSatelliteOperationThreshold`, PostgreSQL rows carrying `smallBatchBoundary=direct-or-unnest`, and MySQL small-batch `MySqlMinimumOperationThreshold`.
- artifacts/benchmarks/v0.31.0-all-providers-smoke-after-cleanup-fix-<redacted>/benchmark-summary.md exists with `Iterations: 1`, `Warmup iterations: 0`, all optional providers completed, and completed `bridge-traversal-read` rows for PostgreSQL, SQL Server, MySQL, and Oracle.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkDatabaseProviders.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceRowGenerationTests.cs both reference `SatCustomerStatu`, so the ticket is correctly treating that name as the current intentional baseline rather than a typo to rename first.
- Downstream consumer tickets already exist and match the cited evidence targets: .gicket/tickets/06F9XD1T3TJK7NEBYNVT2JEPZW/description.md, 06F9XD2M71D1XFT7FJX62KD8HM/description.md, 06F9XD2TGEYEG6S0AK86YF295M/description.md, and 06F9XD33MNNVHHW232TC7T1CN8/description.md all describe the same SQL Server, Oracle, PostgreSQL, and MySQL findings this baseline should feed.
- The repo-root benchmark-summary.md still shows PostgreSQL/SQL Server/MySQL/Oracle skipped because the connection-string env vars are unset, which supports the contract warning not to use the root triplet as the authoritative all-provider baseline.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract leaves the exact v0.32 ticket-labeled artifact directory name flexible rather than prescribing one concrete example path; this is acceptable but the implementer should choose a stable label once evidence is captured.
- The contract does not prescribe a fixed ticket-comment template for the downstream row callout; the implementer should still enumerate exact scenario/provider/baseline rows when recording the evidence.

Risky assumptions
- Assuming the repo-root `benchmark-summary.*` files are the baseline would be wrong; the current root triplet is a lightweight SQLite-plus-skipped-external rollup, not the required all-provider v0.32 evidence set.
- Assuming `--scale` also covers PIT/bridge verification would be wrong; BenchmarkRunner.cs restricts scale mode to customer-profile scale scenarios, so the read verification must be recorded separately.
- Assuming a transient provider outage can be silently worked around would be wrong; the contract requires skipped/failed provider rows or equivalent recorded operational evidence instead of disappearing lanes.
- Assuming this ticket may tune thresholds or provider behavior would be wrong; scope-out is evidence capture only.

AC / test suggestions
- When the developer records the ticket comment, cite exact v0.32 rows by scenario/provider/baseline and map each row to the consumer tickets for SQL Server, Oracle, PostgreSQL, and MySQL.
- Keep the cleanup verification evidence as a distinct smoke/read surface and include its iterations/warmup context so it is visibly separate from the `--scale` bundle.
- If any provider cannot run, preserve the lane with a normalized skip/failure reason in the artifact triplet instead of omitting it.

Implementation watchouts
- Set the external-provider connection-string env vars before restore/build/run so the conditional provider packages and all-provider rows are actually present.
- Use the existing Podman-backed endpoints; when running inside a .NET SDK container, PostgreSQL must be reached through the Podman network rather than assumed localhost.
- Do not replace the repo-root benchmark triplet as part of this task unless a separate follow-up explicitly widens scope.
- Preserve `artifacts/benchmarks/v0.31.0-scale-5-all-providers-<redacted>` unchanged for side-by-side comparison.

Non-blocking notes
- The current owner branch is still pre-development: diff vs `develop` is limited to this ticket's `.gicket` metadata/comment files, which is consistent with a PO refinement pass.
- Related tuning story/task tickets exist but are still `needs-po`; that does not block this prerequisite evidence-capture task from moving to development.

Split recommendations
- No split is required now; keep this ticket as the baseline-evidence prerequisite for story `06F9XD1T3TJK7NEBYNVT2JEPZW` and tasks `06F9XD2M71D1XFT7FJX62KD8HM`, `06F9XD2TGEYEG6S0AK86YF295M`, and `06F9XD33MNNVHHW232TC7T1CN8`.
- If the cleanup-verification rerun expands beyond a bounded smoke/read proof, split that expansion into a separate validation-only follow-up instead of widening this baseline ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment