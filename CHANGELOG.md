# Changelog

This changelog summarizes the public release-note trail. The detailed release records remain under [docs/releases/](docs/releases/); those files are the source of truth for scope, evidence, non-goals, and validation notes.

## v0.46.0 - Reserved Interactive Provider Optimization

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.46.0` / `net8.0` / EF Core 8 and `10.46.0` / `net10.0` / EF Core 10.
- Records that the v0.46.0 release label maps to consumer package versions `8.46.0` and `10.46.0`, not to a `0.46.0` package version.
- Closes the manual provider optimization pass with checked-in PostgreSQL, SQL Server, MySQL, Oracle, and DB2 benchmark triplets for provider-native save, latest-satellite read, PIT read, and bridge read rows.
- Tightens the shared relational PIT/bridge read path by keeping one provider connection open, pushing PIT as-of filtering into SQL, narrowing satellite replay reads, and preserving provider-neutral ordinal-signature semantics.
- Adds Oracle command-level LOB-prefetch/fetch-buffer tuning and records the DB2 1000-row clean-context set-based save cap as the measured boundary.
- Updates the provider optimization gap matrix and performance profiles so the completed timing rows are closed while fallback boundaries, unsupported shapes, and write-side bridge maintenance push-down remain explicit non-goals.
- Updates README, package compatibility, manual publication, local validation, production adoption, analyzer guidance, package creation, and package verification to the v0.46.0 baseline.

See [DVault v0.46.0 Release Notes](docs/releases/v0.46.0.md).

## v0.45.0 - Server-Side PIT and Bridge Maintenance Exploration

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.45.0` / `net8.0` / EF Core 8 and `10.45.0` / `net10.0` / EF Core 10.
- Records that the v0.45.0 release label maps to consumer package versions `8.45.0` and `10.45.0`, not to a `0.45.0` package version.
- Documents that `AddDVaultPostgres()` registers `PostgresDataVaultPitMaintenanceStrategy` for clean PostgreSQL full rebuilds of ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs when no caller transaction is already active.
- Documents that `AddDVaultSqlServer()` replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService` for clean SQL Server full rebuilds of ordinary hub-parent PITs only.
- Keeps unsupported, mismatched, dirty-context, maintain-parents, PostgreSQL caller-transaction, SQL Server multi-active, SQL Server link-parent, and SQL Server no-savepoint maintenance requests on provider-neutral fallback paths.
- Keeps automatic PIT/bridge maintenance, read-time refresh, EF `SaveChanges` interception, background scheduling, bridge maintenance push-down, and benchmark-backed provider-maintenance timing claims outside this baseline.
- Updates the architecture boundary, performance guidance, and provider evidence matrix so maintained-bridge read evidence is not treated as bridge-maintenance push-down proof.
- Updates README, package compatibility, manual publication, local validation, production adoption, analyzer guidance, package creation, and package verification to the v0.45.0 baseline.

See [DVault v0.45.0 Release Notes](docs/releases/v0.45.0.md).

## v0.44.0 - Optional Privacy Extension Foundation

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.44.0` / `net8.0` / EF Core 8 and `10.44.0` / `net10.0` / EF Core 10.
- Records that the v0.44.0 release label maps to consumer package versions `8.44.0` and `10.44.0`, not to a `0.44.0` package version.
- Adds the optional `DCoding.Data.DVault.Privacy` package as a provider-neutral, opt-in privacy proof package.
- Introduces explicit registration, encrypted-payload alias configuration, caller-owned key-provider seams, and an EF Core value-converter proof for selected payload properties.
- Documents fail-closed behavior for missing aliases, missing key providers, marker-only providers, declined conversions, and null conversion outputs.
- Adds model-first `personalData` metadata guidance and the optional privacy architecture boundary while keeping key material, provider-native encryption, deletion, retention, PIT/bridge cleanup, and compliance workflows outside DVault-owned runtime behavior.
- Updates README, package compatibility, manual publication, local validation, production adoption, analyzer guidance, and package verification to the v0.44.0 baseline.

See [DVault v0.44.0 Release Notes](docs/releases/v0.44.0.md).

## v0.43.0 - Binary Adoption, Analyzer Guidance, and Allocation Evidence

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.43.0` / `net8.0` / EF Core 8 and `10.43.0` / `net10.0` / EF Core 10.
- Records that the v0.43.0 release label maps to consumer package versions `8.43.0` and `10.43.0`, not to a `0.43.0` package version.
- Routes new projects to the binary-first profile while keeping public DVault hash-key values as lowercase hexadecimal strings and keeping existing persisted `HexString` setups compatible until a reviewed migration, reset, or data-move plan exists.
- Cites the checked-in provider binary-vs-hex hash-key matrix, `hash-key-footprint.*` sidecars, and hash-key storage migration guide while preserving completed, skipped, failed, diagnostics-only, and storage-footprint boundaries.
- Cites the before/after allocation hotspot artifacts and keeps the bounded hotspot story on DVault-owned save preparation, latest-hash-diff replay filtering, stable-hash canonicalization, and digest generation.
- Keeps analyzer guidance local to source-visible project tooling with `PrivateAssets="all"`, one `net10.0` analyzer asset, and a `.NET 10 SDK` build-host baseline for both package lines.
- Updates README, package compatibility, manual publication, local validation, production adoption, performance, and analyzer guidance to the v0.43.0 baseline without adding package-publication approval or automatic migration claims.

See [DVault v0.43.0 Release Notes](docs/releases/v0.43.0.md).

## v0.42.0 - Provider Performance Evidence and Tuning

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.42.0` / `net8.0` / EF Core 8 and `10.42.0` / `net10.0` / EF Core 10.
- Records that the v0.42.0 release label maps to consumer package versions `8.42.0` and `10.42.0`, not to a `0.42.0` package version.
- Fixes the v0.42 provider evidence promotion rules: completed timing requires a provider-configured benchmark artifact triplet with preserved run context; skipped placeholders, diagnostics-only rows, smoke-only rows, storage-footprint rows, and gap-matrix recommendations are not timing evidence.
- Ratifies the provider tuning starting gates for PostgreSQL direct/UNNEST versus staged COPY, SQL Server native bulk, MySQL retained and staged paths, Oracle direct optimized batching, and DB2 clean-context set-based save only.
- Keeps latest-satellite tuning limited to PostgreSQL, SQL Server, MySQL, Oracle, and DB2 hub-parent non-multi-active shapes, with provider-neutral fallback for unsupported providers, unsupported shapes, incomplete evidence, stale PIT/bridge maintenance, provider mismatch, or diagnostics that do not select the provider strategy.
- Updates README, package compatibility, manual publication, local validation, production adoption, performance, evidence matrix, and gap matrix guidance to distinguish measured improvements, deferred gaps, provider-specific caveats, and historical baselines.
- Updates package creation and verification so `8.42.0` and `10.42.0` are the expected package outputs and stale `8.41.0` / `10.41.0` plus non-package `0.42.0` install guidance is rejected.

See [DVault v0.42.0 Release Notes](docs/releases/v0.42.0.md).

## v0.41.0 - Provider Read Strategy Parity

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.41.0` / `net8.0` / EF Core 8 and `10.41.0` / `net10.0` / EF Core 10.
- Records that the v0.41.0 release label maps to consumer package versions `8.41.0` and `10.41.0`, not to a `0.41.0` package version.
- Completes diagnostics-gated read strategy parity for supported latest-satellite, PIT, and bridge reads across SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 provider packages.
- Hardens SQL Server, Oracle, and DB2 latest-satellite strategy dispatch so DI/interface calls use the intended provider-specific implementations.
- Fixes provider-specific relational PIT and bridge raw reads to preserve the public lowercase-hex hash-key boundary when generated hash-key columns use binary physical storage.
- Adds parity coverage that compares provider-specific latest-satellite, PIT, bridge, typed projection, and binary-storage read behavior against the provider-neutral fallback path.
- Updates README, package compatibility, manual publication, local validation, production adoption, performance, analyzer, and example guidance to the `8.41.0` / `10.41.0` package baseline.
- Updates package creation and verification so `8.41.0` and `10.41.0` are the expected package outputs and stale `8.40.0` / `10.40.0` install guidance is rejected.

See [DVault v0.41.0 Release Notes](docs/releases/v0.41.0.md).

## v0.40.0 - Provider Bulk Strategy Expansion

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.40.0` / `net8.0` / EF Core 8 and `10.40.0` / `net10.0` / EF Core 10.
- Records that the v0.40.0 release label maps to consumer package versions `8.40.0` and `10.40.0`, not to a `0.40.0` package version.
- Keeps provider bulk work inside the existing `IDataVaultSaveService` and provider-strategy boundary; no new platform, scheduler, stored-procedure runtime dispatch, or deployment surface is introduced.
- Hardens shared-type batch filtering by de-duplicating string values and building balanced `OrElse` predicates for larger generated equality batches.
- Adds unit coverage for the large-batch predicate shape so future changes keep duplicate removal and bounded predicate depth intact.
- Moves benchmark table creation behind `IBenchmarkDatabase.EnsureCreatedAsync`, allowing provider-specific benchmark databases to own schema creation behavior.
- Improves DB2 benchmark setup by using relational table creation for the existing database, cleaning up uppercase produced table names, and accepting both current IBM DB2 connection type names.
- Updates README, package compatibility, manual publication, local validation, production adoption, performance, analyzer, and example guidance to the `8.40.0` / `10.40.0` package baseline.
- Updates package creation and verification so `8.40.0` and `10.40.0` are the expected package outputs and stale `8.39.0` / `10.39.0` install guidance is rejected.

See [DVault v0.40.0 Release Notes](docs/releases/v0.40.0.md).

## v0.39.0 - Provider Evidence Matrix Documentation Baseline

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.39.0` / `net8.0` / EF Core 8 and `10.39.0` / `net10.0` / EF Core 10.
- Records that the v0.39.0 release label maps to consumer package versions `8.39.0` and `10.39.0`, not to a `0.39.0` package version.
- Adds the provider-evidence release record for the matrix baseline, caveats, and follow-up posture.
- Updates the performance guidance to cite [Provider Optimization Evidence Matrix](docs/plans/provider-optimization-evidence-matrix.md) rows by `scenario`, `provider`, `baseline`, and `posture`.
- Points follow-up recommendations to [Provider Optimization Gap Matrix](docs/plans/provider-optimization-gap-matrix.md) so planning rows stay separate from measured timing claims.
- Preserves the checked-in root benchmark posture: SQLite local timing rows are completed where present, while optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 rows remain skipped placeholders when connection strings are unset.
- Keeps DB2 bounded to the current diagnostics and smoke posture; no completed DB2 timing, latest-satellite optimization, staged DB2 bulk, provider-native chunk execution, or live-schema reading is claimed.
- Records that no benchmarks were rerun, no benchmark schemas changed, no provider implementation changed, and no NuGet publication or release automation outcome is documented by this note.

See [DVault v0.39.0 Release Notes](docs/releases/v0.39.0.md).

## v0.38.0 - Binary-First New Project Profile

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.38.0` / `net8.0` / EF Core 8 and `10.38.0` / `net10.0` / EF Core 10.
- Records that the v0.38.0 release label maps to consumer package versions `8.38.0` and `10.38.0`, not to a `0.38.0` package version.
- Adds explicit binary-first setup APIs for new projects through `AddDVault(options => options.UseBinaryFirstProfile())` and `modelBuilder.UseDataVaultBinaryFirstProfile()`.
- Keeps the existing-project default compatible: `AddDVault()` and `UseDataVault()` still use `HexString` physical hash-key storage unless a caller opts into binary storage.
- Reports the selected hash-key storage profile through diagnostics and support-bundle evidence while preserving lowercase hexadecimal public hash-key values.
- Updates quickstarts and getting-started guidance so new projects start on binary physical hash-key storage without implying automatic migration for existing persisted keys.
- Carries forward the no-automatic-migration posture: DVault does not automatically rehash, backfill, dual-write, repair, or migrate persisted hash-key storage when the storage profile or stable hash algorithm changes.

See [DVault v0.38.0 Release Notes](docs/releases/v0.38.0.md).

## v0.37.0 - Dependency Line and Analyzer Compatibility

- Defines the current coordinated package and documentation baseline for the visible consumer package lines: `8.37.0` / `net8.0` / EF Core 8 and `10.37.0` / `net10.0` / EF Core 10.
- Records that the v0.37.0 release label maps to consumer package versions `8.37.0` and `10.37.0`, not to a `0.37.0` package version.
- Documents the accepted target-specific dependency matrix:

| Target framework | Provider-neutral EF packages | DB2 | SQLite | MySQL | PostgreSQL | Oracle | SQL Server |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `net8.0` | `Microsoft.EntityFrameworkCore` `8.0.28`, `Microsoft.EntityFrameworkCore.Relational` `8.0.28`, `Microsoft.Extensions.DependencyInjection.Abstractions` `8.0.2` | `IBM.EntityFrameworkCore` `8.0.0.400` | `Microsoft.EntityFrameworkCore.Sqlite` `8.0.28` | `MySql.EntityFrameworkCore` `8.0.26` | `Npgsql.EntityFrameworkCore.PostgreSQL` `8.0.11` | `Oracle.EntityFrameworkCore` `8.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `8.0.28` |
| `net10.0` | `Microsoft.EntityFrameworkCore` `10.0.9`, `Microsoft.EntityFrameworkCore.Relational` `10.0.9`, `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.9` | `IBM.EntityFrameworkCore` `10.0.0.100` | `Microsoft.EntityFrameworkCore.Sqlite` `10.0.9` | `MySql.EntityFrameworkCore` `10.0.7` | `Npgsql.EntityFrameworkCore.PostgreSQL` `10.0.2` | `Oracle.EntityFrameworkCore` `10.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `10.0.9` |

- Keeps `DCoding.Data.DVault.Analyzers` as one `net10.0` analyzer asset used through local `PrivateAssets="all"` references on the `.NET 10 SDK` build-host baseline for both coordinated package lines.
- Carries forward the binary-first adoption guidance for new projects while keeping existing `HexString`-compatible databases and configurations valid until an owner-planned reviewed migration, reset, or data move is executed.
- Carries forward the no-automatic-migration posture: DVault does not automatically rehash, backfill, dual-write, repair, or migrate persisted hash-key storage when the storage profile or stable hash algorithm changes.
- Carries forward the repository validation evidence story: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `bash tools/pack-release-packages.sh`, `bash tools/verify-packages.sh`, and `bash tools/check-format.sh`.
- Leaves package publication, runtime behavior changes, analyzer retargeting, and pure `.NET 8 SDK` analyzer consumption outside this baseline.

See [DVault v0.37.0 Release Notes](docs/releases/v0.37.0.md).

## v0.36.0 - Binary Hash-Key Storage Adoption Guidance

- Defined the coordinated package baseline for `8.36.0` / `net8.0` / EF Core 8 and `10.36.0` / `net10.0` / EF Core 10.
- Documented `HexString` as the compatible default hash-key storage profile and `Binary` as explicit opt-in physical storage for generated hash-key columns.
- Kept public hash-key values as canonical lowercase hexadecimal strings across save, read, diagnostics, and support-bundle boundaries.
- Recorded that existing `HexString`-compatible databases and configurations remain valid until the application owner intentionally plans and executes a separate reviewed migration, reset, or data-move change.
- Carried forward stable hash algorithm-selection guidance and recorded that algorithm or storage-profile changes are caller-owned compatibility work; DVault does not automatically rehash, backfill, dual-write, repair, or migrate persisted hash-key storage for those changes.
- Kept package publication separate from repository package creation and verification.

See [DVault v0.36.0 Release Notes](docs/releases/v0.36.0.md).

## Recent Releases

| Release | Focus |
| --- | --- |
| [v0.35.0](docs/releases/v0.35.0.md) | Stable hash algorithm-selection guidance and dual package-line continuation. |
| [v0.34.0](docs/releases/v0.34.0.md) | DB2 provider package baseline with optimized save and PIT/bridge read strategy evidence. |
| [v0.33.0](docs/releases/v0.33.0.md) | Parallel `net8.0` and `net10.0` consumer package-version lines. |
| [v0.32.0](docs/releases/v0.32.0.md) | Benchmark-driven provider threshold evidence and review-only SQL artifact manifest lane. |
| [v0.31.0](docs/releases/v0.31.0.md) | Performance decision-tree and observability guidance. |
| [v0.30.0](docs/releases/v0.30.0.md) | Typed helper support-bundle freshness baseline. |
| [v0.29.0](docs/releases/v0.29.0.md) | Provider schema guardrails. |
| [v0.28.0](docs/releases/v0.28.0.md) | Provider read optimization evidence boundary. |
| [v0.27.0](docs/releases/v0.27.0.md) | EF lifecycle analyzer guardrails. |
| [v0.26.0](docs/releases/v0.26.0.md) | Provider-tuning diagnostics and benchmark verifier evidence. |
| [v0.25.0](docs/releases/v0.25.0.md) | ReadShape and typed helper boundary. |
| [v0.24.0](docs/releases/v0.24.0.md) | Async streaming and EF safety boundary. |
| [v0.23.0](docs/releases/v0.23.0.md) | Earlier provider/read documentation baseline. |
| [v0.22.0](docs/releases/v0.22.0.md) | Earlier production adoption and documentation baseline. |
| [v0.21.0](docs/releases/v0.21.0.md) | PIT/bridge maintenance boundary. |

Older release notes are kept in [docs/releases/](docs/releases/) for audit context.
