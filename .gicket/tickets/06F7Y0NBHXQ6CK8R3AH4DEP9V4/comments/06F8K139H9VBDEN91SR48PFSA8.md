[gicket-bot] PO-critic review contract

Summary
- Approved for dev: the persisted contract is bounded, `## Open Questions` is `none`, prerequisite source tickets are done, and direct repo evidence shows the current `v0.24.0`/`v0.25.0` documentation drift this task is meant to align to `v0.26.0`.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted delivery contract is developer-ready: `.gicket/tickets/06F7Y0NBHXQ6CK8R3AH4DEP9V4/description.md:17-35` defines scope and acceptance criteria, and `:50-51` shows `## Open Questions` -> `- none`.
- The repository currently has the exact baseline drift the ticket claims: `README.md:10-16` still uses package version `0.25.0`, `README.md:25` says `v0.25.0` is current, `docs/performance-profiles.md:3-5` says `Status: v0.24.0 adopter guidance`, and `docs/production-adoption-checklist.md:9` treats `v0.25.0` as the current public baseline.
- There is no current `v0.26.0` release note yet: `ls /mnt/c/Projects/DVault/docs/releases` lists through `v0.25.0.md`, and `test -f /mnt/c/Projects/DVault/docs/releases/v0.26.0.md` returned `1`.
- Read-shape vocabulary and redaction boundaries already exist: `docs/releases/v0.25.0.md:34-43` closes the kind set to `LatestSatellite`, `PitAsOf`, and `Bridge` and forbids raw keys, SQL, and connection strings; `benchmark-summary.md:49-54` already contains those three row families with fallback and SQLite-optimized variants.
- Benchmark/verifier evidence is already bounded and reusable: `docs/plans/performance-evidence-benchmark-artifact-contract.md:14-18` requires the `benchmark-summary.md/.csv/.json` triplet, and `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:303-317,373-390,768-774` checks provider-guidance rows, regression-budget rules, triplet emission, skipped-provider text, and row consistency across markdown/CSV/JSON.
- Current carried-forward provider/write boundary text already exists for the docs update to reuse: `docs/releases/v0.20.0.md:45-68` captures PostgreSQL/MySQL thresholds, SQL Server/Oracle boundaries, fallback behavior, and stored-procedure limits; `README.md:<redacted>` and `README.md:<redacted>` carry the same boundary into the current README.
- Current guardrail/idempotency sources are explicit and consumer-owned: `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:8-10` and `docs/production-adoption-checklist.md:29-41` keep migration preflight explicit; `src/DCoding.Data.DVault/DataVaultPreflightRequest.cs:76-79` and `src/DCoding.Data.DVault/DataVaultPreflight.cs:77-102` show the idempotency lane is optional and only runs when callers supply live-schema input.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A bounded example of an optional-provider skipped evidence/verifier outcome that preserves `executionStatus=skipped` and normalized skip reasons from the benchmark triplet, not only successful SQLite rows.
- A redacted side-by-side example that distinguishes provider-neutral fallback vs SQLite-selected optimized read evidence for one of `LatestSatellite`, `PitAsOf`, or `Bridge`, because `benchmark-summary.md:49-54` contains both forms.
- An idempotency-preflight example that shows what happens when live-schema input is omitted versus provided, because `src/DCoding.Data.DVault/DataVaultPreflight.cs:79-84` skips the lane when no `IdempotencyLiveSchemaReadResult` is supplied.

Risky assumptions
- `relevant architecture notes` will be interpreted as the existing guardrail/read-boundary docs already referenced by the repo, not as a sweep of every file under `docs/architecture/`.
- The documentation writer will source `benchmark verifier evidence` from the checked-in benchmark contract/tests rather than inventing a new user-facing verifier surface; a repository-wide `rg -n verifier docs benchmarks src tests` produced no current docs page using that label.

AC / test suggestions
- Reuse `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:303-317,373-390,768-774` as the evidence source for provider-guidance/verifier examples and for keeping the benchmark triplet synchronized.
- After the doc update, extend the existing mechanical guidance-check pattern from `docs/performance-profiles.md` to `README.md`, `docs/production-adoption-checklist.md`, and `docs/releases/v0.26.0.md`; `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:<redacted>` already shows the current pattern.

Implementation watchouts
- Keep SQLite as the only repository-proven optimized PIT/bridge read path and preserve fallback-vs-optimized wording; do not generalize those rows to non-SQLite providers.
- Keep the carried-forward threshold wording exact: PostgreSQL/MySQL staged boundary `60` operations, MySQL native gate `50` operations, no invented SQL Server direct/staged split, and Oracle staged path still `not-selected-no-measured-win` until new evidence exists.
- Keep migration guardrail and idempotency preflight wording explicitly consumer-owned and input-driven; current sources skip or block based on supplied preflight inputs rather than background automation.
- Reuse the root `benchmark-summary.md/.csv/.json` triplet and the existing artifact contract instead of inventing new evidence tables or doc-only benchmark formats.
- Keep stored procedures on the explicit non-default escape-hatch boundary; do not imply auto-generation, auto-management, or default runtime selection.

Non-blocking notes
- The current branch is still ticket-only metadata work; `git diff --name-only develop..HEAD -- README.md docs benchmark-summary.md benchmark-summary.csv benchmark-summary.json .gicket` returned only `.gicket/tickets/06F7Y0NBHXQ6CK8R3AH4DEP9V4/...` paths.
- The contract summary says no `description updates` were materialized in `.gicket/tickets/06F7Y0NBHXQ6CK8R3AH4DEP9V4/description.md:5` and `:48`, but the PO comments record a description refresh in `.gicket/tickets/06F7Y0NBHXQ6CK8R3AH4DEP9V4/comments/06F8JY1ZYV0ZS9HWVA3X57M0KR.md:50-57` and `.gicket/tickets/06F7Y0NBHXQ6CK8R3AH4DEP9V4/comments/06F8JYF0KQG1FKN99W67BRKH58.md:14-16`. This is process inconsistency, not a dev-blocking scope gap.

Split recommendations
- No split recommended; the scope is already bounded to documentation alignment over landed diagnostics, benchmark-artifact evidence, migration/idempotency guardrails, and stored-procedure boundary guidance.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment