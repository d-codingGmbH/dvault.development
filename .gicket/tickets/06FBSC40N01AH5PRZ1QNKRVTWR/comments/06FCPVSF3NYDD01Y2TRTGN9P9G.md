[gicket-bot] PO-critic review contract

Summary
- Delivery contract is bounded to the existing benchmark harness/provider matrix, has no unresolved open questions, and is ready for developer handoff; current branch state is still ticket-metadata-only pre-development.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkHashKeyVariant.cs` defines the bounded labels `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, and `sha256-128-v1-binary`, and `BenchmarkOptions.cs` exposes `--hash-key-storage-matrix` with provider filters limited to `all|sqlite|postgres|sqlserver|mysql|oracle`.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs` iterates `options.EffectiveHashKeyVariants` in both `CreateSqliteBenchmarks(...)` and `CreateProviderBenchmarks(...)`, while `BenchmarkRunContext.cs` records `HashKeyVariants` and `OptionalProviders` in run context.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` asserts the four variant labels and SQLite footprint rows, and the checked-in root `benchmark-summary.md` and `benchmark-summary.json` already contain skipped PostgreSQL, SQL Server, MySQL, and Oracle read/save guidance rows with planned strategy details.
- `hash-key-footprint.md` scopes current storage evidence to SQLite only and lists the four variants, while `docs/architecture/dvault-v1-pit-bridge-boundary.md` states SQLite is the only optimized latest-satellite provider and `docs/plans/provider-optimization-evidence-matrix.md` keeps DB2 as diagnostics-only/smoke-only rather than a benchmark lane.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract is clear on the standard SQLite-inclusive matrix run, but it does not explicitly say whether provider-filtered runs such as `--provider postgres` should omit excluded providers entirely or preserve placeholder rows; verify that boundary during implementation/tests.
- Latest-index coverage is mentioned in scope, and `BenchmarkRunner.cs` currently adds latest-index benchmarks only for SQLite; keep that SQLite-only boundary explicit when the four-variant matrix is verified.

Risky assumptions
- Approval assumes the lingering `blocks` relation files from `06FBSC3N7ZFVQW3AV2JJ8T7Q7W` and `06FBSC0MNH0YAWQ4NY2WSC8KJG` are treated as historical because both related tickets are `done` and this ticket's `ticket.json` has `is-blocked: false`.
- Approval assumes developers will generate or verify matrix-specific artifact output during implementation, because the current branch delta from `b079192dc` is ticket metadata only and does not yet land repo changes for this ticket.

AC / test suggestions
- Verify a `--hash-key-storage-matrix` run records all four variant labels in run context and carries deterministic `hashKeyVariant=` execution detail without inventing new benchmark row fields.
- Verify an `all` provider matrix run keeps SQLite completed rows plus skipped placeholder rows for unconfigured PostgreSQL, SQL Server, MySQL, and Oracle lanes, preserving planned or selected strategy details for save, latest-satellite, PIT, and bridge rows.
- Verify multi-variant runs emit `hash-key-footprint.md`, `hash-key-footprint.csv`, and `hash-key-footprint.json`, and keep those sidecars documented as SQLite-local storage evidence rather than cross-provider timing proof.
- Verify SQLite latest-index rows remain variant-suffixed across the four hash-key variants without implying non-SQLite latest-index or latest-satellite optimization.

Implementation watchouts
- Keep DB2 out of this ticket's benchmark lane and provider filter surface; the existing repository documents DB2 as diagnostics-only or smoke-only, not as a benchmark lane.
- Do not convert non-SQLite latest-satellite guidance rows into optimized binary-vs-hex timing claims unless a provider-specific latest-satellite strategy is actually implemented and measured.
- Do not broaden this ticket into checked-in external-provider evidence bundles; downstream evidence population is already split into ticket `06FBSC4BEBGSVVTJSQXM1Z74CC`.
- Treat the current branch as pre-development handoff state, not partial implementation, because the visible branch diff is ticket metadata only.

Non-blocking notes
- The repository already contains the core vocabulary this ticket reuses: four bounded hash-key variants, optional provider labels for PostgreSQL/SQL Server/MySQL/Oracle, and SQLite-only footprint evidence boundaries.

Split recommendations
- No split recommended; harness/dimension work is already separated from downstream provider-evidence collection in ticket `06FBSC4BEBGSVVTJSQXM1Z74CC`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment