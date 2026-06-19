# DVault Package Compatibility

This document is the current package-line and dependency baseline for DVault consumers. Use it together with the release notes, the manual publication checklist, and local validation guidance.

## Package Lines

DVault currently publishes the same coordinated package family on two visible consumer package-version lines:

| Package version line | Target framework | EF Core line |
| --- | --- | --- |
| `8.41.0` | `net8.0` | EF Core 8 |
| `10.41.0` | `net10.0` | EF Core 10 |

Use exactly one line in a consumer project. Do not mix `8.41.0` and `10.41.0` packages in one project, install example, restored target, or publish approval.

The `v0.41.0` release label is a repository release tag and release-note label, not a consumer-facing NuGet package version. Publish or document `8.41.0` and `10.41.0` package versions for this baseline, and do not publish or document a consumer-facing `0.41.0` package version. Future package version movement must update the pack script, package verifier, release notes, and installation guidance together.

## Package Family

The coordinated package family contains exactly these packable package ids:

- `DCoding.Data.DVault`
- `DCoding.Data.DVault.Analyzers`
- `DCoding.Data.DVault.Db2`
- `DCoding.Data.DVault.MySql`
- `DCoding.Data.DVault.Oracle`
- `DCoding.Data.DVault.Postgres`
- `DCoding.Data.DVault.Sqlite`
- `DCoding.Data.DVault.SqlServer`

The `src/DCoding.Data` project is a non-packable source-root build anchor and is not a NuGet publication artifact.

## Dependency Matrix

Patch movement is allowed only within the selected target major line and must be reflected together in project files, matrix tests, package verifier expectations, release notes, and adopter guidance.

| Target framework | Provider-neutral EF packages | DB2 | SQLite | MySQL | PostgreSQL | Oracle | SQL Server |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `net8.0` | `Microsoft.EntityFrameworkCore` `8.0.28`, `Microsoft.EntityFrameworkCore.Relational` `8.0.28`, `Microsoft.Extensions.DependencyInjection.Abstractions` `8.0.2` | `IBM.EntityFrameworkCore` `8.0.0.400` | `Microsoft.EntityFrameworkCore.Sqlite` `8.0.28` | `MySql.EntityFrameworkCore` `8.0.26` | `Npgsql.EntityFrameworkCore.PostgreSQL` `8.0.11` | `Oracle.EntityFrameworkCore` `8.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `8.0.28` |
| `net10.0` | `Microsoft.EntityFrameworkCore` `10.0.9`, `Microsoft.EntityFrameworkCore.Relational` `10.0.9`, `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.9` | `IBM.EntityFrameworkCore` `10.0.0.100` | `Microsoft.EntityFrameworkCore.Sqlite` `10.0.9` | `MySql.EntityFrameworkCore` `10.0.7` | `Npgsql.EntityFrameworkCore.PostgreSQL` `10.0.2` | `Oracle.EntityFrameworkCore` `10.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `10.0.9` |

The `MySql.EntityFrameworkCore` pins are target-specific: `8.0.26` for `net8.0` and `10.0.7` for `net10.0`. They are not permission to mix arbitrary 8.x and 10.x package lines.

## Analyzer Baseline

`DCoding.Data.DVault.Analyzers` is a local build-time package reference, not a runtime dependency. Keep analyzer references local with `PrivateAssets="all"`.

The analyzer package currently ships one `net10.0` analyzer asset for both coordinated package lines. Supported analyzer consumption for both `8.41.0` and `10.41.0` uses a `.NET 10 SDK` build host, including `net8.0` projects on the `8.41.0` package line. The repository does not validate pure `.NET 8 SDK` analyzer consumption.

## Related Guidance

- [DVault v0.41.0 Release Notes](releases/v0.41.0.md)
- [Manual NuGet Publication Checklist](manual-nuget-publication.md)
- [Local Validation](local-validation.md)
- [Analyzer Package Compatibility Audit](plans/analyzer-package-compatibility-audit.md)
- [Production Adoption Checklist](production-adoption-checklist.md)
