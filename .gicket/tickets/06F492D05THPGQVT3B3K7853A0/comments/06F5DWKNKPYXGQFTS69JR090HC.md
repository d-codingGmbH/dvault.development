[gicket-bot] PO-critic review contract

Summary
- The ticket is ready for developer handoff: the persisted delivery contract has `## Open Questions` set to `none`, the earlier release-date blocker was explicitly resolved in later ticket comments, and the repository contains the cited baseline docs, benchmark summaries, and artifact bundles the rollout must use.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `repository-list-directory` on `docs/releases` returned `docs/releases/v0.5.0.md` through `docs/releases/v0.17.0.md` and no `docs/releases/v0.18.0.md`, which matches the ticket scope to create the new coordinated release record.
- `git diff --name-status e2de538f7a2c0eb69836c44fb105c19f7d2233e0..ticket/06F492D05THPGQVT3B3K7853A0-task-update-v0-18-0-documentation-and-release-no -- README.md docs benchmark-summary.md benchmark-summary.csv benchmark-summary.json artifacts/benchmarks .gicket` returned no paths, so the reviewed branch currently contains the refined contract but not the documentation rollout yet.
- `README.md:686-702`, `docs/production-adoption-checklist.md:9`, and `docs/model-first-governance.md:3-5` still identify `v0.17.0` as the current public baseline, which directly matches the rollout work the ticket assigns.
- `README.md:527` and `README.md:697` already document the bounded request-bound read-shape diagnostics surface and explicitly frame it as deterministic explainability rather than raw SQL or provider-magic claims, which supports the query-shape guidance acceptance criteria.
- `docs/architecture/dvault-ef-compiled-compatibility.md` states SQLite is the required compiled-model/query/pooling baseline and excludes provider-specific compiled guarantees; `benchmark-summary.md:51-56` and `benchmark-summary.csv:20-25` contain the `compiled-model-startup`, `compiled-query-hub-read`, and `dbcontext-pooling-dvault-operation` SQLite rows the release note must summarize.
- `docs/plans/performance-evidence-benchmark-artifact-contract.md` requires the benchmark summary triplet and visible skipped optional-provider rows; `benchmark-summary.md:57-64` and `benchmark-summary.json` keep PostgreSQL, SQL Server, MySQL, and Oracle `provider-native-bulk-ingestion` rows visible as `skipped`, and `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations`, `artifacts/benchmarks/06F492CFSJHN0RGXXRG3KT63FM-explicit-save-change-tracker`, and `artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines` all exist.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract names `README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md`, but developers should still search for any other user-facing `v0.17.0` current-baseline references before closing the rollout.
- The placeholder path is authorized, but the contract does not give sample wording for the final-approval cross-reference; the release note should keep that sentence explicit and auditable.

Risky assumptions
- Assumes a forward-looking cross-reference to the final approval record is acceptable before the final approval artifact itself exists, because the contract explicitly authorizes the pending-approval placeholder path.
- Assumes the existing README read-shape diagnostics section is the intended canonical local source for the `request-bound read-shape diagnostics surface` referenced by the acceptance criteria.

AC / test suggestions
- Verify `docs/releases/v0.18.0.md` uses either an approved date or the exact placeholder `Intended release date: pending final release approval` and explicitly points readers to the final approval record required by `docs/manual-nuget-publication.md`.
- Verify the final docs name `compiled-model-startup`, `compiled-query-hub-read`, and `dbcontext-pooling-dvault-operation`, keep SQLite as the required baseline, and avoid provider-specific compiled guarantees.
- Run a repo search for user-facing `v0.17.0` current-baseline claims so the rollout updates all true current-release surfaces without rewriting historical release notes.
- Verify provider-optimization summaries preserve visible completed-or-skipped optional-provider rows for PostgreSQL, SQL Server, MySQL, and Oracle instead of collapsing them away.

Implementation watchouts
- Do not generalize SQLite compiled-model/query/pooling evidence into provider-neutral guarantees.
- Do not drop skipped optional-provider rows when summarizing provider-optimization evidence; the benchmark contract keeps PostgreSQL, SQL Server, MySQL, and Oracle visible even when unconfigured.
- Do not invent raw-SQL, automatic-index, or per-scenario SQL-capture promises where the claim is only timing/allocation based.
- Keep earlier release notes historical; move current-baseline pointers to `v0.18.0` without rewriting older release records.

Non-blocking notes
- Current branch evidence shows no rollout files changed yet, but that is normal pre-development state for this gate: the diff against `e2de538f7a2c0eb69836c44fb105c19f7d2233e0` on the reviewed surfaces was empty.
- The earlier PO-critic date blocker is superseded by the later PO refinement comment that explicitly authorizes the placeholder release-date path.

Split recommendations
- No split recommended; the scope already fits one documentation and release-note rollup, and the delivery contract explicitly says historical done-ticket blocks do not require a split.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment