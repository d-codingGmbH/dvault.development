[gicket-bot] PO-critic review contract

Summary
- Ticket 06EXB7TP9PF2XFRQ9MG7CJQR10 is sufficiently refined and aligned to the current benchmark surface; it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06EXB7TP9PF2XFRQ9MG7CJQR10 revision 06EYP1BEPQPHZJ2RH02YYY0Q18 contains `## Open Questions` -> `none` and explicitly defines deterministic filenames `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` plus the required Markdown/JSON context fields.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs` directly enumerates the four current baselines as `CustomerProfilePlainEfBenchmark`, `CustomerProfileDataVaultBenchmark`, `OrderProductPlainEfBenchmark`, and `OrderProductDataVaultBenchmark`, and the runner already prints `Provider: SQLite local temporary files`.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkOptions.cs` and `benchmarks/DCoding.Data.DVault.Benchmarks/Program.cs` show the existing CLI surface already supports `--iterations` and `--warmup`, matching the contract's compatibility requirement.
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` already documents SQLite temporary-file execution only and the two comparison families, so the ticket's SQLite-only scope matches current repository documentation.
- `benchmarks/DCoding.Data.DVault.Benchmarks/ScenarioContracts.cs` fixes the current scenario inputs around `C-100`, `O-1000`, and `SKU-COFFEE`, supporting the contract statement that this ticket emits artifacts from existing shared scenario contracts rather than adding scenarios.
- `git log --oneline -1` on branch `ticket/06EXB7TP9PF2XFRQ9MG7CJQR10-task-emit-benchmark-artifacts-suitable-for-docum` shows HEAD `61bdb59e` is the PO-critic lease-claim commit, and `git ls-files benchmark-summary.md benchmark-summary.csv benchmark-summary.json` returned no tracked artifact files, so there is no conflicting implementation or pre-existing checked-in artifact behavior to reconcile.
- `gicket-read-ticket-comments` returned only bot claim/refinement/handoff comments for this ticket and no later human clarification that contradicts the persisted delivery contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not state whether the selected output directory must already exist or may be created by the command; this is small enough to leave to implementation and documentation unless the PO wants a stricter UX rule.
- The contract does not require checked-in sample artifacts, which is acceptable for v1 because follow-up ticket 06EXB82GR48V364RP5NHYE2T70 is already called out separately.

Risky assumptions
- Implementation should not infer permission to widen scope into Postgres, Docker, CI publication, or new benchmark scenarios; the persisted contract and README evidence are explicitly SQLite-only and current-baseline-only.
- Implementation should not assume CSV must carry the full run-level context envelope; the contract and implementation notes reserve richer context for Markdown and JSON.

AC / test suggestions
- Acceptance testing should run one benchmark invocation with minimal values such as `--iterations 1 --warmup 0` and assert that exactly `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` are emitted to the chosen directory.
- Acceptance testing should compare the row set across the Markdown table, CSV rows, and JSON summary rows to confirm the same four baselines, same row order, and same summary values are emitted from one run.
- Acceptance testing should verify Markdown and JSON include provider, iterations, warmup count, OS description, OS/process architecture, processor count, and .NET runtime version, while the README documents that downstream docs must preserve that context.

Implementation watchouts
- Keep the existing `BenchmarkSummary` and console table as the canonical row source so the three artifact formats do not drift from the current benchmark output surface.
- Preserve the current `--iterations` and `--warmup` behavior exposed by `BenchmarkOptions.Parse` and `Program.Main` while adding output-directory-driven artifact emission.
- Keep the deterministic filenames and SQLite wording aligned between the runner and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`.

Non-blocking notes
- The benchmark README already gives a concrete local invocation, so the documentation update has a clear benchmark-owned home rather than needing a new publication surface.
- The current branch tip is only the PO-critic claim commit, so this review is evaluating the ticket contract rather than an in-flight implementation diff.

Split recommendations
- No split recommended; the repository already keeps the relevant runner, scenario contracts, and benchmark README in one bounded benchmark surface, which matches the ticket's existing split recommendation.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment