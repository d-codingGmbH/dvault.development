# Changelog

This changelog summarizes the public release-note trail. The detailed release records remain under [docs/releases/](docs/releases/); those files are the source of truth for scope, evidence, non-goals, and validation notes.

## v0.39.0 - Provider Evidence Matrix Documentation Baseline

- Adds a docs-only provider-evidence release record for the matrix baseline, caveats, and follow-up posture without defining a new consumer package-version line.
- Updates the performance guidance to cite [Provider Optimization Evidence Matrix](docs/plans/provider-optimization-evidence-matrix.md) rows by `scenario`, `provider`, `baseline`, and `posture`.
- Points follow-up recommendations to [Provider Optimization Gap Matrix](docs/plans/provider-optimization-gap-matrix.md) so planning rows stay separate from measured timing claims.
- Preserves the checked-in root benchmark posture: SQLite local timing rows are completed where present, while optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 rows remain skipped placeholders when connection strings are unset.
- Keeps DB2 bounded to the current diagnostics and smoke posture; no completed DB2 timing, latest-satellite optimization, staged DB2 bulk, provider-native chunk execution, or live-schema reading is claimed.
- Records that no benchmarks were rerun, no benchmark schemas changed, no provider implementation changed, and no package publication or release automation outcome is documented by this note.

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
