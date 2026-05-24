[gicket-bot] PO-critic review contract

Summary
- Epic contract is clear, all seven child tickets are materialized and done, and the cited repository evidence exists with no unresolved PO-level questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F492BTNHRPBC7D24E13ECFKM/description.md` contains `PO Handoff` = `ready_for_po_critic`, `## Open Questions` = `- none`, and `## Split Recommendations` = `No new split recommended`.
- `docs/plans/performance-evidence-benchmark-artifact-contract.md` defines the shared `benchmark-summary.md`/`.csv`/`.json` triplet and before/after artifact layout, and the repository contains the three cited bundles at `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations`, `artifacts/benchmarks/06F492CFSJHN0RGXXRG3KT63FM-explicit-save-change-tracker`, and `artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines` with `before` and `after` triplets present in each bundle.
- Root evidence is present at `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`; `benchmark-summary.md` shows `Benchmark baselines: 32`, required provider `SQLite local temporary files`, visible skipped optional-provider rows for PostgreSQL/SQL Server/MySQL/Oracle, and completed rows for `latest-satellite-read`, `pit-as-of-read`, `bridge-traversal-read`, `compiled-model-startup`, `compiled-query-hub-read`, and `dbcontext-pooling-dvault-operation`.
- `benchmark-summary.json` contains the authoritative `optionalProviders` array and normalized skip reasons for `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_MYSQL_CONNECTION_STRING`, and `DVAULT_TEST_ORACLE_CONNECTION_STRING`.
- `docs/releases/v0.18.0.md` sets `Intended release date: pending final release approval`, treats v0.18.0 as the coordinated baseline, points to the root benchmark triplet and the three benchmark bundles, and keeps package publication as a separate manual activity.
- Direct source evidence for the claimed diagnostics surface exists in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`: `IDataVaultReadDiagnosticsService` is declared there, and `DataVaultDiagnosticsResult.ReadShape` is a request-bound `DataVaultReadShapeDiagnostics?` payload.
- `docs/architecture/dvault-ef-compiled-compatibility.md` explicitly bounds compiled-model/query/pooling evidence to the SQLite local baseline and documents the `compiled-model-startup`, `compiled-query-hub-read`, and `dbcontext-pooling-dvault-operation` rows.
- Branch-history check: `git rev-parse HEAD` returned `abf13e1d86fb5f2a15a541721d3dbe23be7ea8f4` on `ticket/06F492BTNHRPBC7D24E13ECFKM-epic-performance-analysis-and-query-tuning`; `git diff --name-only abf13e1d86fb5f2a15a541721d3dbe23be7ea8f4..HEAD` was empty; `git show --stat HEAD` shows only lease-claim changes under `.gicket/tickets/06F492BTNHRPBC7D24E13ECFKM/`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- This approval assumes the epic is a rollup/handoff ticket over already-landed child work, not a request for separate epic-level implementation beyond the seven child tickets.
- Optional external-provider lanes are assumed acceptable as skipped when unconfigured because the contract, `benchmark-summary.md`, and `benchmark-summary.json` all preserve visible skipped rows with normalized reasons instead of omitting them.

AC / test suggestions
- If this epic is later used for closure gating, add a final verification step that the root benchmark triplet still contains the named SQLite rows and the visible optional-provider matrix before status is advanced beyond the development path.
- Future follow-up tickets can keep asserting that `IDataVaultReadDiagnosticsService` returns the additive `ReadShape` payload for latest/PIT/bridge requests and that release docs retain the manual-publication placeholder until final approval is recorded.

Implementation watchouts
- Do not broaden the documented claim boundary during downstream work: compiled-model/query/pooling guidance is currently SQLite-bounded in `docs/architecture/dvault-ef-compiled-compatibility.md` and `docs/releases/v0.18.0.md`.
- Do not drop skipped optional-provider rows from benchmark artifacts when providers are unconfigured; the contract requires visible rows with normalized skip reasons.
- The current HEAD commit is ticket-admin only; any downstream work should preserve the existing evidence contract and release-note/manual-publication boundaries rather than treating the empty po-critic diff as missing implementation work.

Non-blocking notes
- `docs/production-adoption-checklist.md`, `docs/model-first-governance.md`, and `README.md` locally reference v0.18.0 as the current public baseline, which is consistent with the epic's documentation-rollup claim.
- The latest persisted PO run report is `.gicket/tickets/06F492BTNHRPBC7D24E13ECFKM/comments/06F5EBBFVKQZ5BRR227B1NFMVG.md`, which states the ticket was processed by PO refinement and handed off to `po-critic`.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment