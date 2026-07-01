# DVault Package Compatibility

This document is the current package-line and dependency baseline for DVault consumers. Use it together with the release notes, the manual publication checklist, and local validation guidance.

## Package Lines

DVault currently publishes the same coordinated package family on two visible consumer package-version lines:

| Package version line | Target framework | EF Core line |
| --- | --- | --- |
| `8.51.0` | `net8.0` | EF Core 8 |
| `10.51.0` | `net10.0` | EF Core 10 |

Use exactly one line in a consumer project. Do not mix `8.51.0` and `10.51.0` packages in one project, install example, restored target, or publish approval.

The `v0.51.0` documentation release label is not a consumer-facing NuGet package version. Publish or document `8.51.0` and `10.51.0` package versions for this baseline, and do not publish or document a consumer-facing `0.51.0` package version. Future package version movement must update the pack script, package verifier, release notes, and installation guidance together.

## Package Family

The coordinated package family contains exactly these packable package ids:

- `DCoding.Data.DVault`
- `DCoding.Data.DVault.Analyzers`
- `DCoding.Data.DVault.Db2`
- `DCoding.Data.DVault.MySql`
- `DCoding.Data.DVault.Oracle`
- `DCoding.Data.DVault.Postgres`
- `DCoding.Data.DVault.Privacy`
- `DCoding.Data.DVault.Sqlite`
- `DCoding.Data.DVault.SqlServer`

The `src/DCoding.Data` project is a non-packable source-root build anchor and is not a NuGet publication artifact.

`DCoding.Data.DVault.Privacy` is an optional provider-neutral privacy proof package. Consumers install it only when they explicitly opt into the privacy extension seam; it provides registration, options, and alias-driven encrypted payload conversion contracts for ordinary EF Core mapped payload properties. It does not provide compliance guarantees, automatic encryption/redaction, database-at-rest encryption, provider-native encrypted column/cell/row features, provider SQL crypto calls, provider-native encrypted DDL, encryption-capability probing, or runtime routing based on native encryption availability.

The privacy caveat uses the finite repository-backed provider baseline. MySQL covers the repository MySQL profile for `MySql.EntityFrameworkCore` and Pomelo rather than a separate MariaDB capability profile. These facts are guidance-only diagnostics facts unless a later provider-specific ticket owns one exact capability:

| Provider profile | Provider-native crypto capability | Status |
| --- | --- | --- |
| SQLite | SQLite encrypted-file build | `unsupported` |
| PostgreSQL | PostgreSQL deployment encryption posture | `conditional` |
| PostgreSQL | `pgcrypto` | `conditional` |
| SQL Server | Transparent Data Encryption | `conditional` |
| SQL Server | Always Encrypted | `conditional` |
| MySQL | MySQL SQL crypto functions | `conditional` |
| MySQL | MySQL file or tablespace encryption | `conditional` |
| Oracle | Transparent Data Encryption | `conditional` |
| Oracle | `DBMS_CRYPTO` | `conditional` |
| DB2 | DB2 native database encryption | `conditional` |

SQL Server `AddDVaultSqlServerAlwaysEncryptedSelection(...)` is the only current explicit provider-owned native crypto selection path. It records an alias-driven, opt-in Always Encrypted selection in redaction-safe diagnostics through `ProviderNativeCryptoSelections` when caller-owned prerequisite proof names and the active SQL Server capability profile line up with the reviewed `conditional` capability facts. It fails closed when prerequisite proof names are missing, capability facts are unavailable or unsupported, or the active capability profile is incompatible. The selection does not replace caller-owned alias registration, `DataVaultEncryptedPayloadValueConverter`, custom conversion, key-store setup, provider provisioning, re-encryption, backfill, dual-write, provider migration, deletion, backup purge, crypto-shredding, retention, or compliance ownership.

## Dependency Matrix

Patch movement is allowed only within the selected target major line and must be reflected together in project files, matrix tests, package verifier expectations, release notes, and adopter guidance.

| Target framework | Provider-neutral EF packages | DB2 | SQLite | MySQL | PostgreSQL | Oracle | SQL Server |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `net8.0` | `Microsoft.EntityFrameworkCore` `8.0.28`, `Microsoft.EntityFrameworkCore.Relational` `8.0.28`, `Microsoft.Extensions.DependencyInjection.Abstractions` `8.0.2` | `IBM.EntityFrameworkCore` `8.0.0.400` | `Microsoft.EntityFrameworkCore.Sqlite` `8.0.28` | `MySql.EntityFrameworkCore` `8.0.26` | `Npgsql.EntityFrameworkCore.PostgreSQL` `8.0.11` | `Oracle.EntityFrameworkCore` `8.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `8.0.28` |
| `net10.0` | `Microsoft.EntityFrameworkCore` `10.0.9`, `Microsoft.EntityFrameworkCore.Relational` `10.0.9`, `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.9` | `IBM.EntityFrameworkCore` `10.0.0.100` | `Microsoft.EntityFrameworkCore.Sqlite` `10.0.9` | `MySql.EntityFrameworkCore` `10.0.7` | `Npgsql.EntityFrameworkCore.PostgreSQL` `10.0.2` | `Oracle.EntityFrameworkCore` `10.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `10.0.9` |

The `MySql.EntityFrameworkCore` pins are target-specific: `8.0.26` for `net8.0` and `10.0.7` for `net10.0`. They are not permission to mix arbitrary 8.x and 10.x package lines.

## Analyzer Baseline

`DCoding.Data.DVault.Analyzers` is a local build-time package reference, not a runtime dependency. Keep analyzer references local with `PrivateAssets="all"`.

The analyzer package ships one `netstandard2.0` analyzer asset under `analyzers/dotnet/cs/` for both coordinated package lines. Supported analyzer consumption for both `8.51.0` and `10.51.0` uses either a `.NET 8 SDK` or `.NET 10 SDK` build host.

## Related Guidance

Release-note and changelog cross-references point to the current v0.51.0 release-note artifact for this package baseline.

- [DVault v0.51.0 Release Notes](releases/v0.51.0.md)
- [Manual NuGet Publication Checklist](manual-nuget-publication.md)
- [Local Validation](local-validation.md)
- [Analyzer Package Compatibility Audit](plans/analyzer-package-compatibility-audit.md)
- [Production Adoption Checklist](production-adoption-checklist.md)
