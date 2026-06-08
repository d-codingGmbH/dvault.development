<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined against live ticket/relation state plus the completed 2026-06-07 v0.32.0 Podman scale bundle; safe scope is MySQL tiny-workload eligibility and benchmark/diagnostic clarity, while PostgreSQL seed regressions from 2026-06-06 are treated as historical unless reproduced in fresh before/after evidence.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Treat `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607` as the current authoritative baseline and `artifacts/benchmarks/v0.31.0-scale-5-all-providers-20260606` as historical seed evidence when they conflict.
- Historical v0.31.0 seed evidence from 2026-06-06 showed PostgreSQL optimized slower than fallback at `customer-profile-scale-10x1` (34.508 ms vs 25.631 ms) and `customer-profile-scale-10x10` (31.335 ms vs 30.635 ms), but the completed v0.32.0 baseline from 2026-06-07 reverses both rows to 14.595 ms vs 28.393 ms and 22.236 ms vs 26.005 ms respectively.
- MySQL tiny rows remain the consistent live small-batch problem across the visible bundles: in v0.32.0, `customer-profile-scale-10x1` is 28.798 ms optimized-registration vs 22.111 ms fallback with `MySqlMinimumOperationThreshold` causes, and `customer-profile-scale-10x10` is 43.905 ms optimized vs 37.033 ms fallback even when `MySqlStagedDataVaultSaveStrategy` is selected.
- Benchmark execution detail currently hardcodes PostgreSQL/MySQL optimized-path wording in `BenchmarkRunner` even when diagnostics report `ProviderNeutralFallback` or staged-provider decline; treat that as an actionable diagnostics/benchmark-artifact clarity gap, not just a labeling quirk.

### Scope In
- Use the same Podman-backed provider setup and v0.32.0 artifact conventions to capture before/after evidence for any code or threshold change.
- For MySQL, evaluate deliberate provider-neutral fallback or higher eligibility thresholds only for tiny workloads with consistently worse live evidence, starting with `customer-profile-scale-10x1` and `customer-profile-scale-10x10`.
- Preserve and, if needed, clarify the existing MySQL two-lane distinction: below 60 operations can retain multi-row or provider-neutral behavior, while staged bulk remains the larger-batch lane.
- For PostgreSQL, keep the current optimized eligibility unless a fresh before snapshot on this ticket reproduces the small-batch regression; still fix diagnostic and artifact wording so retained direct or UNNEST behavior is distinguishable from staged COPY and from provider-neutral fallback.
- Update tests and benchmark-contract assertions that describe selected strategy, fallback causes, staged-provider phase, and execution-detail text for the adjusted providers.

### Scope Out
- SQL Server and Oracle threshold work already owned by tickets `06F9XD2M71D1XFT7FJX62KD8HM` and `06F9XD2TGEYEG6S0AK86YF295M`.
- Any provider-wide retuning justified only by the historical 2026-06-06 seed bundle when the 2026-06-07 v0.32.0 baseline disagrees.
- New provider lanes, DB orchestration, stored-procedure deployment, or changes to the public `IDataVaultSaveService` contract.
- Promoting external-provider rows into the root checked-in `benchmark-summary.*` rollup as part of this ticket.

### Benchmark Environment
- Use local Podman containers named `postgres` and `mysql`; PostgreSQL uses `docker.io/postgres:18` published as `5432:5432`, and MySQL uses `docker.io/mysql:9.7` published as `3306:3306`.
- If the PostgreSQL container is missing, recreate it with `podman run -d --name postgres -e POSTGRES_PASSWORD=local-secret -e POSTGRES_USER=dvault -e POSTGRES_DB=dvault_tests -p 5432:5432 -v pgdata18:/var/lib/postgresql docker.io/postgres:18`; PostgreSQL 18 mounts at `/var/lib/postgresql`, not `/var/lib/postgresql/data`.
- If the MySQL container is missing, recreate it with `podman run -d --name mysql -e MYSQL_ROOT_PASSWORD=root-secret -e MYSQL_USER=dvault -e MYSQL_PASSWORD=local-secret -e MYSQL_DATABASE=dvault_tests -p 3306:3306 -v mysqldata97:/var/lib/mysql docker.io/mysql:9.7`.
- For PostgreSQL/MySQL-only benchmark runs, clear `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` and `DVAULT_TEST_ORACLE_CONNECTION_STRING` first so provider selection is not polluted by another local database.
- PostgreSQL benchmark runs must address PostgreSQL through the Podman network, not through host `localhost`, because host networking has produced unreliable connections for this benchmark setup.
- Inspect the PostgreSQL container IP with `podman inspect postgres --format "{{.NetworkSettings.IPAddress}}"`, run the benchmark from a .NET SDK container on the Podman network, set `DVAULT_TEST_POSTGRES_CONNECTION_STRING` to `Host=<postgres-container-ip>;Port=5432;Database=dvault_tests;Username=dvault;Password=local-secret;Include Error Detail=true;SSL Mode=Disable;Timeout=10;Command Timeout=60`, and pass `-p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured`.
- For MySQL, start `mysql`, set `DVAULT_TEST_MYSQL_CONNECTION_STRING` to `Server=127.0.0.1;Port=3306;Database=dvault_tests;User=dvault;Password=local-secret;SslMode=Disabled;AllowPublicKeyRetrieval=True`, and pass `-p:DVAULT_TEST_MYSQL_CONNECTION_STRING=Configured`.
- The expected PostgreSQL benchmark shape is an SDK-container run on the Podman network: `podman run --rm --network podman -v C:\Projects\DVault:/workspace -w /workspace -e DVAULT_TEST_POSTGRES_CONNECTION_STRING="<postgres-container-ip-connection-string>" mcr.microsoft.com/dotnet/sdk:10.0 dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured -- --provider postgres --scale --iterations 5 --warmup 1 --output artifacts/benchmarks/<ticket-labeled-postgres-output>`.
- The expected MySQL benchmark shape is a host run against the published local port: `dotnet run --project benchmarks\DCoding.Data.DVault.Benchmarks\DCoding.Data.DVault.Benchmarks.csproj --configuration Release -p:DVAULT_TEST_MYSQL_CONNECTION_STRING=Configured -- --provider mysql --scale --iterations 5 --warmup 1 --output artifacts\benchmarks\<ticket-labeled-mysql-output>`.

## Acceptance Criteria
- Before/after artifacts under an approved v0.32.0 ticket-labeled benchmark path show PostgreSQL and MySQL rows for the same scale scenarios, run inputs, and provider setup, and explicitly cite the baseline bundle used for comparison.
- MySQL tiny-workload rows prove one of two bounded outcomes: either the provider-neutral lane is deliberately selected and measurably better for `customer-profile-scale-10x1` and `customer-profile-scale-10x10`, or the ticket documents with fresh evidence why no eligibility change is safe.
- PostgreSQL changes are allowed only if the ticket's own before snapshot reproduces a small-batch optimized-overhead regression; otherwise PostgreSQL remains a diagnostics and benchmark-clarity task with no eligibility change.
- Benchmark and request-bound diagnostics make these states unambiguous for PostgreSQL and MySQL rows: provider strategy selected, retained non-staged provider path, staged-provider decline, and provider-neutral fallback.
- Medium and large wins that the live v0.32.0 bundle already shows must remain materially intact, especially PostgreSQL `customer-profile-scale-100x10` and `customer-profile-scale-1000x10` plus MySQL `customer-profile-scale-1000x10`, `customer-profile-scale-10000x1`, and larger comparable rows.
- Tests cover any adjusted threshold or fallback decision plus the emitted diagnostic and benchmark execution-detail state, and `dotnet test DVault.slnx --nologo` plus `bash tools/check-format.sh` pass.

## Definition of Done
- The ticket leaves one authoritative interpretation of the evidence: v0.32.0 2026-06-07 artifacts are the current baseline, v0.31.0 2026-06-06 artifacts are historical comparison data, and PostgreSQL seed-only regressions are not treated as live facts unless reproduced.
- MySQL tiny-workload behavior is either retuned or explicitly ratified as no-change with fresh measured rationale, and the resulting diagnostics explain why the chosen path executed or declined.
- PostgreSQL benchmark and detail text no longer implies staged COPY execution when the live row actually used retained direct or UNNEST behavior or provider-neutral fallback.
- The resulting before/after bundle and tests are sufficient for downstream documentation work without reopening which rows count as the safe small-batch boundary.

## Implementation Notes
- Use `artifacts/benchmarks/v0.31.0-scale-5-all-providers-20260606` as historical seed context and `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607` as the current authoritative before baseline when selecting rows to compare.
- The relevant benchmark surfaces are `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs`, `CustomerProfileBenchmarks.cs`, and `README.md`; current code hardcodes PostgreSQL and MySQL optimized execution-path labels before appending actual diagnostics.
- The relevant provider and diagnostics surfaces are `src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs`, `src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs`, and `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`.
- Existing tests already expose the baseline gates that this ticket must preserve or adjust: `tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs`.
- No persistent ticket or planning write was materialized during this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- After this ticket lands, should the parent story `06F9XD1T3TJK7NEBYNVT2JEPZW` add a separate benchmark-stability follow-up if PostgreSQL or MySQL medium rows continue to flip between v0.31 and v0.32 style outcomes?
- If MySQL mid-sized rows remain inconsistent across reruns, should a later task introduce a separate evidence-only calibration band instead of widening this ticket beyond tiny workloads?

## Risks
- The visible benchmark history already flips PostgreSQL tiny-row results between 2026-06-06 and 2026-06-07, so any one-off rerun can mislead unless before and after inputs stay identical and the comparison path is explicitly recorded.
- MySQL medium rows (`100x1`, `100x10`, `1000x1`) are not stable across the two visible bundles, so tuning above tiny workloads can easily trade one regression for another.
- Current execution-detail wording can overstate provider-specific execution even when diagnostics show fallback or staged decline, which risks incorrect release-note or documentation claims if not corrected alongside any threshold change.
- Because this ticket blocks documentation task `06F8KZVRARQPG482YKCQ686PNM`, leaving benchmark wording ambiguous can propagate stale provider claims downstream even if runtime behavior is correct.

## Split Recommendations
- No additional split is required if implementation keeps the ticket bounded to MySQL tiny-workload eligibility plus PostgreSQL diagnostics or no-change unless reproduced.
- If a fresh PostgreSQL before snapshot reproduces a separate small-batch regression that needs its own eligibility rule, create a dedicated follow-up instead of widening the MySQL tuning work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Tune PostgreSQL and MySQL small-batch provider save eligibility where the optimized path has more overhead than provider-neutral fallback.

Observed seed evidence:
- PostgreSQL `customer-profile-scale-10x1` optimized was slower than provider-neutral fallback despite no fallback cause.
- MySQL small rows, including `customer-profile-scale-10x1` and `customer-profile-scale-10x10`, showed provider-path overhead or minimum-threshold fallback behavior.

Scope:
- Review PostgreSQL and MySQL small-batch boundaries, staged-provider caveats, and diagnostics.
- Prefer provider-neutral fallback for tiny workloads when benchmark evidence shows the provider path is consistently slower.
- Keep larger-batch optimized wins intact, especially PostgreSQL medium/large rows and MySQL larger history rows.

Podman test environment:
- Use existing `postgres` and `mysql` Podman containers.
- PostgreSQL must be reached through the Podman network when the benchmark runs inside an SDK container.
- Preserve before/after benchmark artifacts under the v0.32.0 evidence path.

Acceptance criteria:
- Before/after artifacts show small-batch PostgreSQL/MySQL behavior and confirm that medium/large optimized rows do not regress materially.
- Diagnostics identify whether small batches declined because of threshold, unsupported shape, or deliberate provider-neutral fallback.
- Tests cover the threshold decision and strategy diagnostic status for the adjusted providers.
- `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh` pass.
