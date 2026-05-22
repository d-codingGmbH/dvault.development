[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract is detailed, repository evidence supports the claimed benchmark/artifact baseline, and the contract has no unresolved open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F492BZPP5YT9SJSPDHQBGF3R/description.md:32-56` defines concrete acceptance criteria and DoD, and `description.md:55-56` says `## Open Questions` -> `- none`.
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md:22-64` documents `--provider`, `--load-timestamp-storage`, `--latest-indexes`, deterministic `benchmark-summary.md|csv|json` outputs, row fields, and optional-provider skipped-row behavior.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs:11-40` writes `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`; `BenchmarkArtifacts.cs:131-160` includes iterations, warmup, load timestamp storage, provider filter, OS/runtime fields in the run context.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:19-193` asserts the standard SQLite history/read matrix plus skipped optional-provider bulk rows, `:226-317` asserts deterministic artifact filenames/columns/JSON fields, and `:339-357` asserts the latest-index matrix mode.
- Repository artifact examples confirm the provider-matrix rule: `artifacts/benchmarks/baseline-2026-05-08-scale-5/benchmark-summary.json` shows PostgreSQL, SQL Server, MySQL, and Oracle optional providers with `executionStatus=skipped` plus explicit env-var skip reasons, while `artifacts/benchmarks/baseline-2026-05-08-standard-5-all-providers-fixed/benchmark-summary.json` shows those four providers as `executionStatus=completed`.
- Branch history is ticket-only on this review surface: `git diff --name-only develop...HEAD` listed only `.gicket/tickets/06F492BZPP5YT9SJSPDHQBGF3R/**`, and `git rev-parse --short HEAD` returned `0b5de976b`, matching the supplied scratch-source ref.
- `gicket-read-ticket-comments` returned 10 comments consisting of bot lease/handoff/report traffic plus the PO refinement contract comment; no later comment reopened scope or added unresolved PO questions.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not explicitly say how `executionStatus=failed` rows should be judged in the final evidence bundle even though the harness can emit failed rows via `BenchmarkSummary.CreateFailed(...)` in `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs:443-463`.
- The acceptance language keeps latest-index and scale modes at `when relevant`; downstream tickets will still need to state why those modes do or do not apply to a given performance claim.

Risky assumptions
- This approval assumes developers will treat current source/tests as the baseline for `loadTimestampStorage` and `providerFilter`; the checked-in example bundles under `artifacts/benchmarks/baseline-2026-05-08-*` do not currently show those fields even though the current artifact writer does.
- This approval assumes the follow-up questions in `description.md:58-60` are intentionally deferred discussion items, not handoff blockers, because the persisted `## Open Questions` section is explicitly `none`.

AC / test suggestions
- Add one explicit AC or contract-test expectation for failed benchmark rows so exception evidence is standardized alongside skipped rows.
- Add one concrete example of a query-shape claim that requires SQL capture versus a save-path claim that does not, to reduce downstream interpretation drift.
- When this story lands, refresh at least one checked-in benchmark bundle so the repository contains an example artifact set with the current run-context keys and any newly added allocation fields.

Implementation watchouts
- Historical benchmark bundles in `artifacts/benchmarks/**` are mixed-era evidence; developers should not infer the full current artifact schema only from older `baseline-2026-05-08-*` samples.
- The existing artifact schema/tests already lock filenames and timing/status columns, but allocation metrics and SQL-capture sidecar evidence are still additive work called out in `description.md:51-52,63-65`.
- The default matrix is intentionally asymmetric today: SQLite covers history plus read scenarios, while non-SQLite default rows are provider-native bulk only.

Non-blocking notes
- The branch under review currently changes only ticket-state files, which is acceptable for this pre-development PO gate.
- The repository already contains many persisted benchmark bundles under `artifacts/benchmarks/**/benchmark-summary.{md,csv,json}`, which supports the story's claim that a reusable artifact convention already exists.

Split recommendations
- No split recommended; the persisted contract already positions this ticket as the single benchmark-evidence blocker for the listed downstream stories.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment