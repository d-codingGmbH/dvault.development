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
| Current .NET baseline | `DVault.slnx`, `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` |
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

New DVault .NET projects should use these defaults unless a later ticket documents a specific exception:

- `TargetFramework` set to `net10.0`
- `Nullable` set to `enable`
- `ImplicitUsings` set to `enable`
- `GenerateDocumentationFile` set to `true`
- same-line C# braces, enforced through `.editorconfig` and the formatting gate

Public API source should include generated XML documentation coverage where the project enables documentation generation. Compiler or analyzer warnings introduced by this baseline should be addressed in the owning feature ticket rather than avoided by disabling the shared settings.

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
- Current .NET projects target `net10.0` with nullable enabled, implicit usings enabled, and generated XML documentation enabled.
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

## Acceptance Baseline

A downstream ticket satisfies this shared-standards dependency when it references this document and follows the relevant source-of-truth document for the area it modifies. No unresolved PO-level architecture questions remain here for formatting, encoding, repository layout, documentation baseline, current .NET baseline, v1 naming defaults, v1 hashing defaults, or v1 persistence defaults.
