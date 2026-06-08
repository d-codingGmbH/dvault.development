[gicket-bot] PO-critic review contract

Summary
- Ticket contract is clear, bounded, and evidence-backed; ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket revision 06FA5N4E45A1929PCY59G5FE3R shows `## Open Questions` = `none`, a bounded `Scope In/Out`, and acceptance criteria that require same-input before/after v0.32.0 artifacts plus PostgreSQL no-change unless a fresh ticket-local before snapshot reproduces the regression.
- repository-read-text on `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted>/benchmark-summary.md` shows PostgreSQL `customer-profile-scale-10x1` optimized 14.595 ms vs fallback 28.393 ms and `customer-profile-scale-10x10` optimized 22.236 ms vs 26.005 ms, matching the contract's claim that the 2026-06-07 baseline reversed the 2026-06-06 seed regression.
- Repository evidence in `artifacts/benchmarks/v0.31.0-scale-5-all-providers-<redacted>/benchmark-summary.md` shows the historical opposite PostgreSQL tiny-row result (`customer-profile-scale-10x1` 34.508 ms vs 25.631 ms; `customer-profile-scale-10x10` 31.335 ms vs 30.635 ms), which justifies the ticket's explicit historical-seed-vs-authoritative-v0.32.0-baseline boundary.
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` already documents that PostgreSQL direct/UNNEST, MySQL multi-row, and staged-provider paths must remain distinguishable in benchmark artifacts, and `docs/plans/performance-evidence-benchmark-artifact-contract.md` requires comparable before/after artifact sets under one label with the same run inputs.
- Repository source surfaces align with the requested scope: `src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs` keeps PostgreSQL staged bulk at 60 operations with retained direct/UNNEST below that boundary, `src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs` exposes MySQL 50-operation optimized gating plus 60-operation staged gating, and `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` defines `ProviderStrategySelected`, `ProviderNeutralFallback`, and `MySqlMinimumOperationThreshold` diagnostics named in the ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The implementation will need to reproduce the same Podman-backed provider setup and keep before/after inputs identical, because the visible benchmark history already flips between the 2026-06-06 and 2026-06-07 bundles.
- The execution-detail wording issue is assumed to be solvable within the existing artifact contract without adding new artifact columns, consistent with the current README and scope boundaries.

AC / test suggestions
- Closure evidence should cite the exact before and after artifact directories and explicitly call out the tiny MySQL rows (`customer-profile-scale-10x1`, `customer-profile-scale-10x10`) plus the PostgreSQL guardrail rows (`customer-profile-scale-100x10`, `customer-profile-scale-1000x10`).
- Test coverage should assert the emitted execution-detail/diagnostics difference among provider-neutral fallback, retained non-staged provider path, and staged-provider decline for both PostgreSQL and MySQL.

Implementation watchouts
- Do not let benchmark `executionDetail` keep preselected staged-bulk wording when diagnostics report `ProviderNeutralFallback` or staged-provider decline.
- Keep MySQL tuning bounded to tiny workloads; the ticket contract explicitly treats unstable mid-sized rows as risk, not scope.
- Do not change PostgreSQL eligibility unless the ticket's own fresh before snapshot reproduces the small-batch regression.

Non-blocking notes
- The comment stream is workflow-only; there is no human discussion to reconcile before developer handoff.

Split recommendations
- No split is needed if implementation stays on MySQL tiny-workload eligibility plus PostgreSQL diagnostics/no-change, as already stated in the delivery contract.
- If a fresh ticket-local PostgreSQL before snapshot reproduces a separate small-batch regression that needs its own eligibility rule, open a dedicated follow-up instead of expanding this task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment