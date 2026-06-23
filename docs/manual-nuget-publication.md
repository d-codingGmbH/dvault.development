# Manual NuGet Publication Checklist

This checklist governs manual publication for the coordinated DVault NuGet family. It documents release criteria, required evidence, package validation, publish order, stop conditions, and the current NuGet consumer baseline.

Publishing remains manual. This document does not introduce release credentials, CI/CD automation, package push tooling, product-code behavior changes, package metadata changes, or provider implementation changes.

## Package Family

The v1 coordinated release family contains exactly these nine packable packages:

- `DCoding.Data.DVault`
- `DCoding.Data.DVault.Analyzers`
- `DCoding.Data.DVault.Db2`
- `DCoding.Data.DVault.MySql`
- `DCoding.Data.DVault.Oracle`
- `DCoding.Data.DVault.Postgres`
- `DCoding.Data.DVault.Privacy`
- `DCoding.Data.DVault.Sqlite`
- `DCoding.Data.DVault.SqlServer`

Manual publication must not proceed for only a subset of this family. Each package-version line is approved, validated, and published as one synchronized nine-package family.

`DCoding.Data.DVault.Privacy` is optional for consumers and opt-in at runtime. It is a provider-neutral registration and alias-driven encrypted payload conversion proof, not a compliance feature, automatic encryption/redaction feature, or provider-native encryption feature.

The current v0.45.0 documentation baseline uses two consumer package-version lines over these package ids:

| Package version line | Target framework | EF Core line |
| --- | --- | --- |
| `8.45.0` | `net8.0` | EF Core 8 |
| `10.45.0` | `net10.0` | EF Core 10 |

Do not publish or document a consumer-facing `0.45.0` package version from the v0.45.0 release label. Do not combine `8.45.0` and `10.45.0` packages in one consumer install example or one publish approval.

The `src/DCoding.Data` project is a non-packable source-root build anchor for the namespace family. It is not a NuGet publication artifact and is outside the coordinated publication scope.

## Current Consumer Guidance

Developer and consumer setup is NuGet-based for published releases. The README installation guidance is the current v0.45.0 baseline and should show separate `8.45.0` / `net8.0` / EF Core 8 and `10.45.0` / `net10.0` / EF Core 10 `dotnet add package` commands for `DCoding.Data.DVault` plus the optional provider package family, including `DCoding.Data.DVault.Db2`, and the optional privacy proof package. Analyzer examples must stay local with `PrivateAssets="all"` and use the same package-version line selected for the runtime and provider packages. Projects that reference `DCoding.Data.DVault.Analyzers` must build on the `.NET 10 SDK` host baseline for both coordinated package lines; this repository does not validate pure `.NET 8 SDK` analyzer consumption.

Source or project-reference consumption remains useful for repository development, debugging, and unpublished local changes, but it is no longer the primary consumer installation path for released packages.

## Release Criteria

Before final publish approval, the maintainer performing the release must confirm:

- the release covers all nine package ids listed in this document
- one aligned package version is used for all nine packages in the selected package-version line
- the selected package-version line is intentional for the coordinated release and is not being applied to only a provider-specific subset
- release notes or changelog content has been prepared and reviewed for the coordinated release
- all required pre-publish validation commands have passed against the same checkout and selected package version
- package verification confirms each provider package depends on the packed `DCoding.Data.DVault` package version from the same package-version line and carries only that line's target-framework dependency group
- package verification confirms the packaged README guidance separates `8.45.0` for `net8.0` and EF Core 8 from `10.45.0` for `net10.0` and EF Core 10
- release notes and README guidance explain that `v0.45.0` is the release tag and release-note label, not a consumer NuGet package version, and keep the dependency-line, analyzer-compatibility, manual-publication, and validation guidance aligned with the published package-version lines
- final publish approval has been recorded before the first package push

## Release Notes Evidence

The release operator must prepare and review release notes or changelog content before final publish approval. When a dedicated changelog exists, link it from the approval record together with the release notes. When no dedicated changelog exists, record the release-note evidence in the release ticket, release approval note, or another auditable release record.

Minimum auditable release-note content is:

- selected coordinated package-version line
- release date or intended release date
- the nine package ids covered by the release
- notable user-facing changes, fixes, documentation changes, packaging changes, and any hash-key storage-profile adoption guidance relevant to the release
- known limitations or compatibility notes relevant to consumers
- reviewer or approver identity for the final publish approval

Do not push packages until this release-note evidence has been reviewed as part of final approval.

## Required Pre-Publish Evidence

Run the current repository validation baseline from the repository root before any package push. Use a .NET 10 SDK checkout; the helper projects stay `net10.0`, and the pack plus package-verification steps prove the runtime/provider `net8.0` and `net10.0` package dependency groups. These commands are the minimum required evidence for manual publication:

```sh
dotnet build DVault.slnx --nologo
dotnet test DVault.slnx --nologo
bash tools/pack-release-packages.sh
bash tools/verify-packages.sh
bash tools/check-format.sh
```

Do not replace these commands with an undocumented automation path. Later release automation may wrap them, but the manual release evidence must still show equivalent build, test, pack, package-verification, and formatting results.

## Current v0.44 Dependency Matrix

The v0.45.0 baseline is target-specific. `8.45.0` / `net8.0` uses the EF Core 8 dependency line, and `10.45.0` / `net10.0` uses the EF Core 10 dependency line. Patch movement is allowed only within the selected target major line and must be reflected together in the project files, matrix tests, package verifier, release notes, and adopter guidance.

| Target framework | Provider-neutral EF packages | DB2 | SQLite | MySQL | PostgreSQL | Oracle | SQL Server |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `net8.0` | `Microsoft.EntityFrameworkCore` `8.0.28`, `Microsoft.EntityFrameworkCore.Relational` `8.0.28`, `Microsoft.Extensions.DependencyInjection.Abstractions` `8.0.2` | `IBM.EntityFrameworkCore` `8.0.0.400` | `Microsoft.EntityFrameworkCore.Sqlite` `8.0.28` | `MySql.EntityFrameworkCore` `8.0.26` | `Npgsql.EntityFrameworkCore.PostgreSQL` `8.0.11` | `Oracle.EntityFrameworkCore` `8.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `8.0.28` |
| `net10.0` | `Microsoft.EntityFrameworkCore` `10.0.9`, `Microsoft.EntityFrameworkCore.Relational` `10.0.9`, `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.9` | `IBM.EntityFrameworkCore` `10.0.0.100` | `Microsoft.EntityFrameworkCore.Sqlite` `10.0.9` | `MySql.EntityFrameworkCore` `10.0.7` | `Npgsql.EntityFrameworkCore.PostgreSQL` `10.0.2` | `Oracle.EntityFrameworkCore` `10.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `10.0.9` |

The analyzer package is a local build-time asset, not a runtime dependency. `DCoding.Data.DVault.Analyzers` ships one `net10.0` analyzer asset for both coordinated package lines, analyzer references stay local with `PrivateAssets="all"`, and supported analyzer consumption for both the `8.45.0` and `10.45.0` package lines uses a `.NET 10 SDK` build host.

## Version And Dependency Alignment

Use one aligned package version across all nine packages in the selected package-version line. For the v0.45.0 documentation baseline, validate `8.45.0` and `10.45.0` as separate publish approvals; do not publish `0.45.0` from the release label and do not mix packages from both lines in one consumer example or approval record. The `v0.45.0` Git tag is the release-note tag. Package versions for the visible consumer lines are set explicitly by `bash tools/pack-release-packages.sh` through MinVer version overrides. Before final approval, inspect the package outputs produced by the release pack script through the package verification gate:

```sh
bash tools/verify-packages.sh
```

Package verification is the manual dependency-alignment gate. It must confirm the exact eighteen package artifacts across the two package lines, sixteen matching symbol packages for the runtime, provider, and privacy packages, package README and XML metadata, analyzer assets, provider and privacy dependency alignment, and the line-specific `net8.0` or `net10.0` nuspec dependency group for each package version. The core package must expose the expected EF Core, EF Core Relational, and `Microsoft.Extensions.DependencyInjection.Abstractions` versions for its selected target group. Each provider and privacy package must depend on the packed `DCoding.Data.DVault` version from the same package line and use the correct target-specific provider dependency, `Microsoft.EntityFrameworkCore.Relational`, and `Microsoft.Extensions.DependencyInjection.Abstractions` versions when those direct dependencies are present. The DB2 provider package must use `IBM.EntityFrameworkCore` `8.0.0.400` for the `net8.0` line and `10.0.0.100` for the `net10.0` line.

If verification reports that a package is missing a target-framework dependency group, a provider package is missing a `DCoding.Data.DVault` dependency or depends on a different core version, one target group mixes EF Core lines, packaged README guidance is stale or mixed-line, XML docs or analyzer assets are missing, or symbols drift, stop the release. Correct the package inputs, rebuild, repack, and rerun the full required pre-publish evidence before requesting approval again.

## Anti-Partial-Publication Flow

Follow this sequence exactly for the coordinated manual release:

1. Confirm the release scope is the full nine-package family.
2. Select one package-version line for this approval: `8.45.0` for `net8.0` and EF Core 8, or `10.45.0` for `net10.0` and EF Core 10.
3. Set or confirm that selected aligned package version for all nine packages.
4. Prepare and review release notes or changelog content for the coordinated release.
5. Run `dotnet build DVault.slnx --nologo`.
6. Run `dotnet test DVault.slnx --nologo`.
7. Run `bash tools/pack-release-packages.sh`.
8. Run `bash tools/verify-packages.sh`.
9. Run `bash tools/check-format.sh`.
10. Review the validation evidence, target-framework dependency groups, packaged README guidance, symbols, analyzer assets, XML docs, and provider dependency alignment.
11. Record final publish approval for the selected package-version line.
12. Push `DCoding.Data.DVault` first.
13. Push `DCoding.Data.DVault.Privacy`.
14. Push `DCoding.Data.DVault.Analyzers`.
15. Push `DCoding.Data.DVault.Db2`.
16. Push `DCoding.Data.DVault.MySql`.
17. Push `DCoding.Data.DVault.Oracle`.
18. Push `DCoding.Data.DVault.Postgres`.
19. Push `DCoding.Data.DVault.Sqlite`.
20. Push `DCoding.Data.DVault.SqlServer`.
21. Record the completed publication outcome for all nine packages in the selected package-version line.

The provider publish order is policy for this manual release flow: Db2, MySql, Oracle, Postgres, Sqlite, then SqlServer. Do not infer a different order from project layout or provider dependency shape.

## Stop Conditions

Stop the release immediately when any validation, package verification, approval, or push step fails. Do not continue to later packages after a failure. Do not publish a replacement subset to work around a failed package.

If a failure happens before any package is pushed, correct the issue and restart the checklist from the beginning.

If a failure happens after the core package or an earlier provider package has already been pushed, stop immediately and escalate through the release approval channel. The release record must identify which packages were already pushed, which package failed, the failing command or push step, and the decision for recovery. Do not push remaining providers until the recovery decision is explicit and approved.

## Final Approval Record

Before the first package push, the final approval record must include:

- coordinated release version
- selected package-version line and its target framework / EF Core line
- confirmation that all nine packages are in scope
- location of the reviewed release notes or changelog content
- validation evidence for the five required commands
- confirmation that `bash tools/verify-packages.sh` passed line-specific dependency-group checks, packaged README guidance checks, metadata, XML docs, analyzer assets, symbols, and provider dependency alignment against the packed core version
- approval to publish the core package first and then providers in the documented order

After publication completes, update the release record with the final outcome for each package id.
