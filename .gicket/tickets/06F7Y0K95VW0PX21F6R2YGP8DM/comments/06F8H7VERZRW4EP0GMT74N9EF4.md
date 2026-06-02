[gicket-bot] PO-critic review contract

Summary
- Ticket 06F7Y0K95VW0PX21F6R2YGP8DM is ready for developer handoff: the delivery contract is bounded, `## Open Questions` is `none`, and the repository already exposes the exact artifact, docs, and diagnostics surfaces the verifier must guard.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06F7Y0K95VW0PX21F6R2YGP8DM has PO handoff `decision: ready_for_po_critic` and `## Open Questions` set to `none` in the delivery contract.
- `git rev-parse --abbrev-ref HEAD` returned `ticket/06F7Y0K95VW0PX21F6R2YGP8DM-story-add-benchmark-regression-artifact-verifier`; `git rev-parse HEAD` returned `29a1b21bdc81735253787fc6e90442b18b0c014a`, matching the supplied scratch-source-ref; `git diff --stat 29a1b21bdc81735253787fc6e90442b18b0c014a..HEAD` returned no output.
- `git log --oneline --max-count=3 --no-decorate` shows `[06F7Y0K95VW0PX21F6R2YGP8DM]` handoff/claim commits (`3f9249989`, `4f7e666cc`, `29a1b21bd`) and no later implementation commit.
- Root repository files `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` already exist; `benchmark-summary.md` currently reports `Benchmark baselines: 38`, SQLite as the required provider, and skipped optional PostgreSQL/SQL Server/MySQL/Oracle rows.
- The current root artifact set already contains the row identities the story names, including `customer-profile-streaming-save` rows for `materialized-explicit-bulk`, `chunked-save-bounded-10`, `async-source-bounded-10`, and `chunked-save-bounded-5`, plus `provider-native-bulk-ingestion` rows for PostgreSQL, SQL Server, MySQL, and Oracle with skipped-row semantics.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` already enumerates expected benchmark rows, asserts `optionalProviders` length `4`, verifies skipped rows keep null JSON metrics and `persistedOutcome` `not executed`, and checks async-source and provider-boundary execution-detail text.
- `docs/performance-profiles.md` is already anchored to the root triplet, copies current run-context facts, and defines the four checked-in profile names `Small app-local vault`, `Medium chunked ingestion`, `Staged provider ingestion`, and `Read-model heavy` with supporting-row values that match the current root artifact.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` defines the closed `DataVaultPerformanceProfileCategory` set with `SmallAppLocalVault`, `MediumChunkedIngestion`, `StagedProviderIngestion`, and `ReadModelHeavy`, and the same file emits provider-tuning recommendations for the current diagnostics surfaces.
- `docs/plans/performance-evidence-benchmark-artifact-contract.md` already defines the skipped-row contract, the optional-provider matrix, and the default regression budgets: required SQLite non-target regressions over `5%` fail by default and configured optional-provider regressions over `10%` require callout and justification.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Clarify whether the verifier should compare documentation timing values against rounded display values from markdown/CSV or normalized values derived from raw JSON doubles; `docs/performance-profiles.md` currently cites three-decimal numbers while `benchmark-summary.json` keeps higher precision.
- Clarify whether the diagnostics-side `1:1` category check should treat the authoritative source as the closed enum/public API surface in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`, or whether the verifier must also prove every category is currently emitted by runtime recommendations.
- An explicit failed-row example is not present in the checked-in root triplet; skipped-row behavior is well evidenced, but failed-row verifier messaging will need to be derived from the artifact contract rather than from a current repository example.

Risky assumptions
- Assumes v1 intentionally stays bounded to the root triplet, `docs/performance-profiles.md`, and the current diagnostics/profile-category surfaces, not to `README.md`, `docs/production-adoption-checklist.md`, or historical/exploratory benchmark bundles; that matches the story Scope Out and Follow-Up Questions.
- Assumes the regression-budget expectations can be represented deterministically in test fixtures/code and checked against the contract document without introducing a second silent source of truth.
- Assumes `provider-tuning recommendation category set` means the repository-backed closed category surface, not a guarantee that every category presently appears in an observed runtime recommendation path.

AC / test suggestions
- Keep one acceptance path that validates row-identity parity across markdown/CSV/JSON by normalized `(scenario, provider, baseline)` keys and verifies skipped-row blanks/nulls exactly.
- Keep one acceptance path that cross-checks `docs/performance-profiles.md` run-context facts, four profile names, and cited supporting-row mean-ms values against the verified root artifact source.
- Keep one acceptance path that compares the closed diagnostics category surface against the four checked-in performance-profile names so doc/diagnostic drift is caught deterministically.

Implementation watchouts
- Do not silently widen v1 to every `artifacts/benchmarks/*` bundle; the repository contains exploratory and historical directories with older shapes, and the story intentionally bounds verification to the active evidence surface.
- Normalize optional-provider skipped rows carefully: `iterations=0`, blank markdown/CSV metrics, JSON `null` metrics, explicit skip reason, planned execution detail, and `persistedOutcome=not executed` are all part of the contract.
- Preserve provider-boundary distinctions that current guidance depends on, especially PostgreSQL retained direct-or-UNNEST vs staged COPY, MySQL multi-row vs staged bulk, SQL Server single native bulk boundary, and Oracle `stagedOracleBulk=not-selected-no-measured-win`.

Non-blocking notes
- No recent ticket comments were supplied in the persisted snapshot.
- This branch is still at the PO-to-PO-critic handoff ref, which is acceptable for a normal pre-development PO gate because missing implementation evidence is not itself a PO blocker here.
- Existing benchmark artifact tests and docs already give the developer concrete repository anchors, so the story is not asking them to discover the target surface from prose alone.

Split recommendations
- No split is required for the bounded verifier story. If the team later wants README/production-checklist citation verification or historical before/after bundle validation, keep that as follow-up work instead of widening this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment