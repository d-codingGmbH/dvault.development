# Manual NuGet Publication Checklist

This checklist governs manual publication for the coordinated DVault NuGet family. It documents release criteria, required evidence, package validation, publish order, stop conditions, and the current NuGet consumer baseline.

Publishing remains manual. This document does not introduce release credentials, CI/CD automation, package push tooling, product-code behavior changes, package metadata changes, or provider implementation changes.

## Package Family

The v1 coordinated release family contains exactly these seven packable packages:

- `DCoding.Data.DVault`
- `DCoding.Data.DVault.Analyzers`
- `DCoding.Data.DVault.MySql`
- `DCoding.Data.DVault.Oracle`
- `DCoding.Data.DVault.Postgres`
- `DCoding.Data.DVault.Sqlite`
- `DCoding.Data.DVault.SqlServer`

Manual publication must not proceed for only a subset of this family. Each package-version line is approved, validated, and published as one synchronized seven-package family.

The v0.33 compatibility release uses two consumer package-version lines over these unchanged package ids:

| Package version line | Target framework | EF Core line |
| --- | --- | --- |
| `8.33.0` | `net8.0` | EF Core 8 |
| `10.33.0` | `net10.0` | EF Core 10 |

Do not publish or document a consumer-facing `0.33.0` package version. Do not combine `8.33.0` and `10.33.0` packages in one consumer install example or one publish approval.

The `src/DCoding.Data` project is a non-packable source-root build anchor for the namespace family. It is not a NuGet publication artifact and is outside the coordinated publication scope.

## Current Consumer Guidance

Developer and consumer setup is NuGet-based for published releases. The README installation guidance is the current baseline and should show separate `8.33.0` / `net8.0` / EF Core 8 and `10.33.0` / `net10.0` / EF Core 10 `dotnet add package` commands for `DCoding.Data.DVault` plus the optional provider package family. Analyzer examples must stay local with `PrivateAssets="all"` and use the same package-version line selected for the runtime and provider packages.

Source or project-reference consumption remains useful for repository development, debugging, and unpublished local changes, but it is no longer the primary consumer installation path for released packages.

## Release Criteria

Before final publish approval, the maintainer performing the release must confirm:

- the release covers all seven package ids listed in this document
- one aligned package version is used for all seven packages in the selected package-version line
- the selected package-version line is intentional for the coordinated release and is not being applied to only a provider-specific subset
- release notes or changelog content has been prepared and reviewed for the coordinated release
- all required pre-publish validation commands have passed against the same checkout and selected package version
- package verification confirms each provider package depends on the packed `DCoding.Data.DVault` package version for both `net8.0` and `net10.0` dependency groups
- package verification confirms the packaged README guidance separates `8.33.0` for `net8.0` and EF Core 8 from `10.33.0` for `net10.0` and EF Core 10
- final publish approval has been recorded before the first package push

## Release Notes Evidence

The release operator must prepare and review release notes or changelog content before final publish approval. A dedicated changelog file is not required by this checklist. When no dedicated changelog exists, record the release-note evidence in the release ticket, release approval note, or another auditable release record.

Minimum auditable release-note content is:

- selected coordinated package-version line
- release date or intended release date
- the seven package ids covered by the release
- notable user-facing changes, fixes, documentation changes, and packaging changes
- known limitations or compatibility notes relevant to consumers
- reviewer or approver identity for the final publish approval

Do not push packages until this release-note evidence has been reviewed as part of final approval.

## Required Pre-Publish Evidence

Run the current repository validation baseline from the repository root before any package push. Use a .NET 10 SDK checkout; the helper projects stay `net10.0`, and the pack plus package-verification steps prove the runtime/provider `net8.0` and `net10.0` package dependency groups. These commands are the minimum required evidence for manual publication:

```sh
dotnet build DVault.slnx --nologo
dotnet test DVault.slnx --nologo
dotnet pack DVault.slnx --configuration Release --nologo
bash tools/verify-packages.sh
bash tools/check-format.sh
```

Do not replace these commands with an undocumented automation path. Later release automation may wrap them, but the manual release evidence must still show equivalent build, test, pack, package-verification, and formatting results.

## Version And Dependency Alignment

Use one aligned package version across all seven packages in the selected package-version line. For the v0.33 compatibility release, validate `8.33.0` and `10.33.0` as separate publish approvals; do not publish `0.33.0` and do not mix packages from both lines in one consumer example or approval record. Package versions are derived from Git tags with the `v` prefix by MinVer. Before final approval, inspect the package outputs produced by `dotnet pack DVault.slnx --configuration Release --nologo` through the package verification gate:

```sh
bash tools/verify-packages.sh
```

Package verification is the manual dependency-alignment gate. It must confirm the exact seven package set, six matching symbol packages for the runtime/provider packages, package README and XML metadata, analyzer assets, provider dependency alignment, and the `net8.0` / `net10.0` nuspec dependency groups. The core package must expose the expected EF Core, EF Core Relational, and `Microsoft.Extensions.DependencyInjection.Abstractions` versions for each target group. Each provider package must depend on the packed `DCoding.Data.DVault` version for both target groups and use the correct target-specific `Microsoft.EntityFrameworkCore.Relational` and `Microsoft.Extensions.DependencyInjection.Abstractions` versions when those direct dependencies are present.

If verification reports that a package is missing a target-framework dependency group, a provider package is missing a `DCoding.Data.DVault` dependency or depends on a different core version, one target group mixes EF Core lines, packaged README guidance is stale or mixed-line, XML docs or analyzer assets are missing, or symbols drift, stop the release. Correct the package inputs, rebuild, repack, and rerun the full required pre-publish evidence before requesting approval again.

## Anti-Partial-Publication Flow

Follow this sequence exactly for the coordinated manual release:

1. Confirm the release scope is the full seven-package family.
2. Select one package-version line for this approval: `8.33.0` for `net8.0` and EF Core 8, or `10.33.0` for `net10.0` and EF Core 10.
3. Set or confirm that selected aligned package version for all seven packages.
4. Prepare and review release notes or changelog content for the coordinated release.
5. Run `dotnet build DVault.slnx --nologo`.
6. Run `dotnet test DVault.slnx --nologo`.
7. Run `dotnet pack DVault.slnx --configuration Release --nologo`.
8. Run `bash tools/verify-packages.sh`.
9. Run `bash tools/check-format.sh`.
10. Review the validation evidence, target-framework dependency groups, packaged README guidance, symbols, analyzer assets, XML docs, and provider dependency alignment.
11. Record final publish approval for the selected package-version line.
12. Push `DCoding.Data.DVault` first.
13. Push `DCoding.Data.DVault.Analyzers`.
14. Push `DCoding.Data.DVault.MySql`.
15. Push `DCoding.Data.DVault.Oracle`.
16. Push `DCoding.Data.DVault.Postgres`.
17. Push `DCoding.Data.DVault.Sqlite`.
18. Push `DCoding.Data.DVault.SqlServer`.
19. Record the completed publication outcome for all seven packages in the selected package-version line.

The provider publish order is policy for this manual release flow: MySql, Oracle, Postgres, Sqlite, then SqlServer. Do not infer a different order from project layout or provider dependency shape.

## Stop Conditions

Stop the release immediately when any validation, package verification, approval, or push step fails. Do not continue to later packages after a failure. Do not publish a replacement subset to work around a failed package.

If a failure happens before any package is pushed, correct the issue and restart the checklist from the beginning.

If a failure happens after the core package or an earlier provider package has already been pushed, stop immediately and escalate through the release approval channel. The release record must identify which packages were already pushed, which package failed, the failing command or push step, and the decision for recovery. Do not push remaining providers until the recovery decision is explicit and approved.

## Final Approval Record

Before the first package push, the final approval record must include:

- coordinated release version
- selected package-version line and its target framework / EF Core line
- confirmation that all seven packages are in scope
- location of the reviewed release notes or changelog content
- validation evidence for the five required commands
- confirmation that `bash tools/verify-packages.sh` passed dual-target dependency-group checks, packaged README guidance checks, metadata, XML docs, analyzer assets, symbols, and provider dependency alignment against the packed core version
- approval to publish the core package first and then providers in the documented order

After publication completes, update the release record with the final outcome for each package id.
