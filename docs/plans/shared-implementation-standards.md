# Shared Implementation Standards

Status: v1 shared standards
Ticket: 06EXB6NWYVB37D7S74VB3PVTCC
Milestone: Foundation and architecture

## Purpose

This document is the shared implementation standards artifact for DVault foundation work. Downstream tickets should reference this document when they need repository formatting, layout, .NET baseline, documentation, naming, hashing, persistence, or Data Vault concept standards.

These standards consolidate existing repository decisions. They do not replace the referenced source documents, and they do not introduce product-code behavior, provider-specific persistence behavior, migrations, schema generation, runtime configuration APIs, or CI workflow files.

## Relationship Context

This story belongs to charter ticket 06EXB4MDREV2T51VJNJEP6R0WR. It is the parent of downstream tickets 06EXB6P4ZNYA46MSYRGAJ9ZEPM and 06EXB6PDF0DSHE68B3V0656DJM, and it blocks 06EXB6XBV95E08R2W9ZQ1PRDPM.

Those tickets should reference this artifact instead of copying standards into their own descriptions or implementation notes. Future governance work may attach this document to the charter epic through the approved attachment surface.

## Source Of Truth Documents

Use these documents as the authoritative sources for their covered decisions:

| Area | Source |
| --- | --- |
| Formatting and encoding | `docs/formatting.md`, `.editorconfig`, `.gitattributes`, `tools/check-format.sh` |
| Repository layout | `README.md` |
| Current .NET baseline and package compatibility contract | `DVault.slnx`, `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`, this document's `.NET Project Baseline` and `Current Package Compatibility Contract` sections |
| Data Vault MVP concepts | `docs/architecture/mvp-data-vault-concepts.md` |
| Default table and column naming | `docs/naming/default-naming-policy.md` |
| Stable hashing | `docs/plans/stable-hashing-contract.md` |
| V1 persistence conventions | `docs/plans/dvault-v1-default-persistence-convention-policy.md` |

When a downstream ticket needs detailed rules from one of these areas, it should link to the specific source document above. This artifact exists to route implementers to the correct source and record the shared v1 defaults in one place.

## Formatting And Encoding

The canonical formatting and encoding policy is `docs/formatting.md`, backed by root `.editorconfig`, root `.gitattributes`, and the non-mutating shell gate:

```sh
bash tools/check-format.sh
```

Governed repository text files use these v1 defaults:

- two-space indentation with spaces by default
- LF line endings
- UTF-8 without BOM
- final newline for non-empty files
- no trailing whitespace
- no tabs except for Makefile syntax in `Makefile`, `makefile`, and `*.mk`
- same-line opening braces for brace-based source files

The root `.gitattributes` normalizes governed text to LF. The formatting gate fails when files violate UTF-8 validity, BOM, CRLF, trailing whitespace, final newline, tab, or required policy-source checks. It also verifies the required `.editorconfig` and `.gitattributes` policy entries.

Manual review is not an accepted substitute for the gate. The first CI workflow or application build definition added to the repository must run `bash tools/check-format.sh` as a blocking step. Later language-specific formatters may be added, but they must preserve these repository-level results and run through the same gate or an equivalent gate documented here.

## Repository Layout

The repository layout baseline is `README.md`.

V1 layout defaults:

- `DVault.slnx` is the root solution file and the repository-level .NET entry point for `dotnet build` and `dotnet test`.
- The repository root should keep a single dotnet-discoverable project or solution entry point so no-argument build and test automation stays deterministic.
- `src/DCoding.Data.DVault/` contains the main library project. The package id and root namespace are `DCoding.Data.DVault`.
- `tests/DCoding.Data.DVault.Tests/` contains unit, integration, and shared test projects.
- `docs/` contains documentation, architecture notes, naming policies, and governed planning artifacts.
- `examples/` is reserved for future runnable DVault API examples.
- `benchmarks/` is reserved for future performance benchmark projects.
- Empty scaffold folders use `.gitkeep` files so the intended layout exists in clean checkouts.
- New project files should be added to `DVault.slnx` when those projects are created unless a later governance ticket documents an exception.

The active v1 repository layout uses `src/DCoding.Data.DVault/` and `tests/DCoding.Data.DVault.Tests/` as project paths while preserving `DCoding.Data.DVault` as the package and root namespace identity.

## .NET Project Baseline

The current .NET implementation baseline is visible in `DVault.slnx` and `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`.

Outside the current package compatibility contract below, new DVault .NET projects should use these defaults unless a later ticket documents a specific exception:

- packable runtime/provider projects that ship to consumers target `net8.0;net10.0`
- analyzer, tooling, benchmark, or repository helper projects may stay on `net10.0` when they are not consumer runtime packages
- `Nullable` set to `enable`
- `ImplicitUsings` set to `enable`
- `GenerateDocumentationFile` set to `true`
- same-line C# braces, enforced through `.editorconfig` and the formatting gate

Public API source should include generated XML documentation coverage where the project enables documentation generation. Compiler or analyzer warnings introduced by this baseline should be addressed in the owning feature ticket rather than avoided by disabling the shared settings.

## Current Package Compatibility Contract

The current v0.43.0 package compatibility baseline continues the visible dual consumer package-version contract from v0.36.0, advances the package lines to `8.43.0` and `10.43.0`, carries forward the DB2 provider package baseline, stable hash algorithm-selection guidance, binary hash-key storage adoption guidance, and analyzer build-host guidance, and aligns target-specific dependency pins and package verification documentation. It does not by itself publish packages, provision external databases, create release automation, change the default stable hash algorithm or storage profile, add automatic persisted-hash migration, or split the DVault library into a platform/tool-suite surface. DB2 execution claims stay limited to the registered clean-context save strategy, diagnostics-gated PIT/bridge read strategy, opt-in live smoke evidence documented for the v0.34.0 DB2 baseline, and external opt-in live-schema evidence owned by the consuming environment.

The coordinated package family contains these eight package IDs across all compatibility lines:

- `DCoding.Data.DVault`
- `DCoding.Data.DVault.Analyzers`
- `DCoding.Data.DVault.Db2`
- `DCoding.Data.DVault.MySql`
- `DCoding.Data.DVault.Oracle`
- `DCoding.Data.DVault.Postgres`
- `DCoding.Data.DVault.Sqlite`
- `DCoding.Data.DVault.SqlServer`

Do not introduce line-specific package IDs, duplicate artifact names, or split package families for the `net8.0` and `net10.0` lines.

The release label is not the consumer-facing NuGet package version. The current baseline exposes exactly two aligned package-version lines:

| Package version line | Target framework | EF Core line |
| --- | --- | --- |
| `8.43.0` | `net8.0` | EF Core 8 |
| `10.43.0` | `net10.0` | EF Core 10 |

Do not publish or document a consumer-facing `0.43.0` DVault package version from the v0.43.0 release label. Do not combine `8.43.0` and `10.43.0` packages in one published artifact family or consumer example.

Each resolved target must use exactly one compatible EF/provider dependency line. Runtime, provider, integration-test, benchmark, example, and verifier project files may use conditional `PackageReference` entries only for target-framework selection and for the existing opt-in external-provider test switches. A single resolved target must not restore both the 8.x and 10.x dependency lines together. Patch movement is allowed only within the selected target major line and must be reflected together in the project files, matrix tests, package verifier, and current documentation baseline.

Provider-neutral EF Core, `Microsoft.EntityFrameworkCore.Relational`, and `Microsoft.Extensions.DependencyInjection.Abstractions` references must stay on the selected target's EF Core major line. Patch updates may advance within that line to the latest accepted repository baseline, but they must not jump to a newer EF/DI major line only because the target framework can restore it. The current accepted provider-neutral baseline is `8.0.28` / `8.0.28` / `8.0.2` for `net8.0` and `10.0.9` / `10.0.9` / `10.0.9` for `net10.0`.

The required EF/provider package evidence for the compatibility lines is:

| Target framework | Provider-neutral EF packages | DB2 | SQLite | MySQL | PostgreSQL | Oracle | SQL Server |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `net8.0` | `Microsoft.EntityFrameworkCore` `8.0.28`, `Microsoft.EntityFrameworkCore.Relational` `8.0.28`, `Microsoft.Extensions.DependencyInjection.Abstractions` `8.0.2` | `IBM.EntityFrameworkCore` `8.0.0.400` | `Microsoft.EntityFrameworkCore.Sqlite` `8.0.28` | `MySql.EntityFrameworkCore` `8.0.26` | `Npgsql.EntityFrameworkCore.PostgreSQL` `8.0.11` | `Oracle.EntityFrameworkCore` `8.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `8.0.28` |
| `net10.0` | `Microsoft.EntityFrameworkCore` `10.0.9`, `Microsoft.EntityFrameworkCore.Relational` `10.0.9`, `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.9` | `IBM.EntityFrameworkCore` `10.0.0.100` | `Microsoft.EntityFrameworkCore.Sqlite` `10.0.9` | `MySql.EntityFrameworkCore` `10.0.7` | `Npgsql.EntityFrameworkCore.PostgreSQL` `10.0.2` | `Oracle.EntityFrameworkCore` `10.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `10.0.9` |

Provider-neutral EF Core references, provider packages, integration tests, matrix tests, release notes, and package verifier expectations must follow the selected target's EF Core major line. The `MySql.EntityFrameworkCore` pins are target-specific: `8.0.26` for `net8.0` and `10.0.7` for `net10.0`. Downstream tests and documentation must describe those as target-specific accepted baselines rather than as a standing cross-line exception or permission for arbitrary mixed-line resolution.

`DCoding.Data.DVault.Db2` registers `AddDVaultDb2()`, DB2 provider behavior for `IBM.EntityFrameworkCore`, the `db2-v1` provider capability profile, a diagnostics-gated optimized clean-context save strategy for ordinary hub, link, and satellite rows, and diagnostics-gated latest-satellite/PIT/bridge read dispatch. DB2 live-schema reading is built in through `IBM.EntityFrameworkCore` and remains external opt-in evidence. DB2 still does not add a staged bulk path, provider-native chunk execution, container provisioning, timing claims beyond the scoped DB2 hotspot bundle, or a default CI database requirement.

`DCoding.Data.DVault.Analyzers` remains coordinated family tooling, not a runtime dependency. Consuming projects should keep analyzer/source-generator references local with `PrivateAssets="all"`. Package verification for the analyzer line must prove analyzer assets are present, while runtime package verification must not treat the analyzer as a transitive runtime dependency.

Downstream package verification, matrix tests, release notes, README guidance, and CI documentation are incomplete if they blur release label `v0.43.0` with package versions `8.43.0` and `10.43.0`, omit one of the required EF/provider pins above, omit the DB2 package from the eight-package family, allow a mixed 8.x/10.x restored target, overstate DB2 beyond the registered save and PIT/bridge read strategies, overstate stable hash opt-in ids as security or compliance controls, overstate binary hash-key storage as an automatic migration or public byte-key contract, overstate analyzer support beyond the `.NET 10 SDK` build-host baseline, or export analyzer assets as runtime dependencies.

## V0.33 Compatibility Contract

This historical section records the previous v0.33 compatibility contract for release-note links and audit context. Planning release `v0.33.0` defined a dual consumer package-line contract over the then-existing DVault package family. It did not by itself edit project files, add provider behavior, provision external databases, publish packages, or create release automation.

The coordinated package family remains these seven package IDs across all compatibility lines:

- `DCoding.Data.DVault`
- `DCoding.Data.DVault.Analyzers`
- `DCoding.Data.DVault.MySql`
- `DCoding.Data.DVault.Oracle`
- `DCoding.Data.DVault.Postgres`
- `DCoding.Data.DVault.Sqlite`
- `DCoding.Data.DVault.SqlServer`

Do not introduce line-specific package IDs, duplicate artifact names, or split package families for the net8.0 and net10.0 lines.

The planning release number is not the consumer-facing NuGet package version. `v0.33.0` produces exactly two aligned package-version lines:

| Package version line | Target framework | EF Core line |
| --- | --- | --- |
| `8.33.0` | `net8.0` | EF Core 8 |
| `10.33.0` | `net10.0` | EF Core 10 |

Do not publish or document a consumer-facing `0.33.0` DVault package version for this planning release. Do not combine `8.33.0` and `10.33.0` packages in one published artifact family or consumer example.

Each resolved target must use exactly one compatible EF/provider dependency line. Runtime, provider, integration-test, and verifier project files may use conditional `PackageReference` entries only for target-framework selection and for the existing opt-in external-provider test switches. A single resolved target must not restore both the 8.x and 10.x dependency lines together.

The required provider package evidence for the compatibility lines is:

| Target framework | SQLite | MySQL | PostgreSQL | Oracle | SQL Server |
| --- | --- | --- | --- | --- | --- |
| `net8.0` | `Microsoft.EntityFrameworkCore.Sqlite` `8.0.27` | `MySql.EntityFrameworkCore` `10.0.7` | `Npgsql.EntityFrameworkCore.PostgreSQL` `8.0.11` | `Oracle.EntityFrameworkCore` `8.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `8.0.27` |
| `net10.0` | `Microsoft.EntityFrameworkCore.Sqlite` `10.0.8` | `MySql.EntityFrameworkCore` `10.0.7` | `Npgsql.EntityFrameworkCore.PostgreSQL` `10.0.2` | `Oracle.EntityFrameworkCore` `10.23.26200` | `Microsoft.EntityFrameworkCore.SqlServer` `10.0.8` |

Provider-neutral EF Core references must follow the target's EF Core line. The `MySql.EntityFrameworkCore` `10.0.7` pin is the required evidence exception for both targets and must be called out explicitly in tests and documentation instead of treated as permission for arbitrary mixed-line resolution.

`DCoding.Data.DVault.Analyzers` remains coordinated family tooling, not a runtime dependency. Consuming projects should keep analyzer/source-generator references local with `PrivateAssets="all"`. Package verification for the analyzer line must prove analyzer assets are present, while runtime package verification must not treat the analyzer as a transitive runtime dependency.

Downstream package verification, matrix tests, release notes, README guidance, and CI documentation are incomplete if they blur planning release `v0.33.0` with package versions `8.33.0` and `10.33.0`, omit one of the required provider pins above, allow a mixed 8.x/10.x restored target, or export analyzer assets as runtime dependencies.

## Namespaces And Naming

The visible modeling namespace evidence is `DCoding.Data.DVault.Modeling` in `src/DCoding.Data.DVault/Modeling/DefaultNamingPolicy.cs`. Downstream tickets should follow nearby namespace and folder patterns unless their contract explicitly owns namespace changes.

Default model table and column naming is governed by `docs/naming/default-naming-policy.md`. Downstream implementations must reference that document for:

- PascalCase object and column identifiers
- hub, link, and satellite table prefixes
- hash key, hash diff, load timestamp, and record source technical columns
- provider-neutral reserved-word handling
- deterministic duplicate suffixes
- finite v1 singularization behavior

Do not restate or fork the default naming policy in implementation tickets. If a ticket needs to change naming semantics, it must update the naming policy through a specific governance or implementation contract.

## Data Vault Concepts

MVP Data Vault concepts are governed by `docs/architecture/mvp-data-vault-concepts.md`.

The v1 conceptual baseline includes hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources. Early examples may stay conceptual and SQLite-oriented, but feature work should preserve the documented separation between business identity, relationships, descriptive history, and lineage metadata.

The Data Vault concepts document is intentionally not a final schema-generation contract. Implementation tickets that create schema generation, loading automation, migrations, validation tooling, or provider-specific behavior must document those responsibilities explicitly.

## Stable Hashing

Stable hashing is governed by `docs/plans/stable-hashing-contract.md`.

The v1 defaults are:

- a replaceable hash service boundary equivalent to the contract documented there
- default algorithm identifier `sha256-v1`
- SHA-256 over UTF-8 bytes without a byte order mark
- lowercase hexadecimal digest values
- deterministic canonical text inputs
- invariant, culture-independent formatting rules
- no process-local salts, random values, timestamps, machine identifiers, or platform-default encoding

The built-in non-default stable hash ids are `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`. They are explicit opt-in choices for non-adversarial Data Vault identity hashing only, not security or compliance controls. Changing algorithm id or truncation after hub keys, link keys, hash diffs, or other stable-hash values have been persisted is caller-owned compatibility work with no automatic DVault rehash, backfill, migration, repair, or reconciliation.

This artifact does not redefine canonical value normalization or test vectors. Hashing implementations and tests must use the stable hashing contract as the source of truth.

## V1 Persistence Conventions

Provider-neutral persistence defaults are governed by `docs/plans/dvault-v1-default-persistence-convention-policy.md`.

The v1 baseline defines logical DVault record persistence conventions, including required logical object, index, field, metadata, canonicalization, and versioning expectations. Provider adapters may map logical names to native storage primitives only when they preserve the documented logical names, field meanings, deterministic behavior, and version metadata.

Provider-specific physical schema details, migrations, runtime configuration APIs, and dialect-specific behavior remain outside this shared standards story. They require separate tickets.

## Documentation Standards

Governed planning, standards, and architecture documents belong under `docs/`, using existing `docs/plans/` and `docs/architecture/` patterns.

Downstream documentation should:

- reference shared source documents instead of copying policy text
- identify whether a decision is a v1 default or a deferred follow-up
- keep provider-specific details out of provider-neutral standards unless the ticket explicitly owns those details
- preserve the formatting policy in `docs/formatting.md`
- keep operational ticket or bot metadata out of product documentation

Documentation changes that introduce standards should name the owning ticket when useful and should make validation paths concrete enough for downstream automation and review.

## V1 Defaults

These decisions are current v1 defaults:

- Repository text formatting and encoding follow `docs/formatting.md`.
- Local formatting validation runs with `bash tools/check-format.sh`.
- Layout follows the README baseline for `DVault.slnx`, `src/DCoding.Data.DVault/`, `tests/DCoding.Data.DVault.Tests/`, `docs/`, `examples/`, `benchmarks/`, and tracked placeholder folders.
- Current packable runtime/provider projects target `net8.0;net10.0`; analyzer, tooling, benchmark, and repository helper projects may stay on `net10.0` when they are not consumer runtime packages.
- Release baseline `v0.43.0` uses the dual package-line compatibility contract in this document: `8.43.0` for `net8.0` and EF Core 8, `10.43.0` for `net10.0` and EF Core 10, eight coordinated package IDs including `DCoding.Data.DVault.Db2`, no consumer-facing `0.43.0` package line, no mixed-line restored targets, DB2 execution documented as optimized save plus latest-satellite/PIT/bridge read dispatch with external opt-in live-schema evidence and scoped DB2 hotspot timing, stable hash algorithm selection documented as a caller-owned compatibility decision, binary hash-key storage documented as explicit opt-in physical storage rather than an automatic migration, and analyzer consumption documented against the `.NET 10 SDK` build-host baseline.
- C# and other brace-based source files use same-line opening braces.
- Current modeling namespace evidence is `DCoding.Data.DVault.Modeling`.
- Model table and column naming follows `docs/naming/default-naming-policy.md`.
- Stable hashing follows `docs/plans/stable-hashing-contract.md`.
- Provider-neutral persistence conventions follow `docs/plans/dvault-v1-default-persistence-convention-policy.md`.
- MVP Data Vault concept language follows `docs/architecture/mvp-data-vault-concepts.md`.

## Deferred Follow-Up Work

These categories remain deferred until separate tickets explicitly own them:

- Creating or wiring CI workflows.
- Adding provider-specific persistence behavior, migrations, schema generation, or runtime configuration APIs.
- Expanding Data Vault concepts beyond the MVP concept set.
- Changing stable hashing algorithms, canonical normalization, or security-specific hashing behavior.
- Changing default table or column naming semantics.
- Adding language-specific formatters beyond the current shell gate.
- Expanding future release notes, broader adopter documentation, or package publication automation beyond the dedicated multitarget project, provider matrix, verifier, README, manual release, and CI guidance tickets.

## Acceptance Baseline

A downstream ticket satisfies this shared-standards dependency when it references this document and follows the relevant source-of-truth document for the area it modifies. No unresolved PO-level architecture questions remain here for formatting, encoding, repository layout, documentation baseline, current .NET baseline, the v0.41 net8.0/net10.0 compatibility contract, v1 naming defaults, v1 hashing defaults, or v1 persistence defaults.
