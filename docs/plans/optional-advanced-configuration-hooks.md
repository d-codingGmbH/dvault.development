# Optional Advanced Configuration Hooks

Status: v1 planning contract
Ticket: 06EXB6QX6JJX9H7CZT3YAXSAD4
Milestone: Foundation and architecture

## Purpose

This document defines the v1 plan for optional advanced configuration hooks in DVault. The plan keeps the normal DVault path convention-first and zero-configuration while identifying the bounded extension points advanced users may need later.

The hook plan is architecture-level. It does not require runtime implementation, public API names, provider-specific option matrices, configuration file formats, migrations, or additional provider ecosystems.

## Configuration Model

DVault v1 should expose one coherent advanced configuration surface when implementation work reaches these hooks. That surface groups related customization by responsibility:

- Naming conventions.
- Hashing behavior.
- Record source resolution.
- Timestamp sourcing and formatting.
- Provider behavior.

Every hook is optional. When a hook is unset, DVault uses the v1 default for that category. Users should be able to configure one category without restating defaults for the others.

Advanced hooks are additive overrides. They can wrap or replace the behavior for their own category, but they must not make ordinary vault setup require custom configuration.

## Zero-Configuration Defaults

The default path requires no user action:

| Hook category | Default behavior | User configuration |
| --- | --- | --- |
| Naming conventions | Use DVault's standard deterministic naming conventions for model names, technical columns, logical persistence objects, and logical indexes. | Optional. |
| Hashing behavior | Use DVault-owned deterministic hashing defaults for stable model hashes and logical persistence content hashes. | Optional. |
| Record source resolution | Treat record source as required lineage metadata supplied by the ingest or modeling context, represented with the default record source naming convention. | Optional. |
| Timestamp sourcing and formatting | Store timestamps as UTC instants at the logical boundary, format them with ISO 8601 compatible UTC text where persisted, and keep timestamps out of content hashes unless a later payload contract explicitly includes them. | Optional. |
| Provider behavior | Use provider-neutral DVault semantics and allow adapters to map logical names and fields to native provider primitives without changing logical meaning. | Optional. |

Unset hooks must inherit these defaults. Defaults must be deterministic across machines, processes, cultures, time zones, providers, and repeated runs unless a later ticket explicitly changes a versioned convention.

## Naming Hook

Default behavior:

- Model table and technical column names follow the default naming policy documented in `docs/naming/default-naming-policy.md`.
- Logical persistence object, index, and metadata field names follow `docs/plans/dvault-v1-default-persistence-convention-policy.md`.
- Defaults are deterministic, provider-neutral, and stable for equivalent model inputs.

Optional customization:

- Advanced users may customize names for existing system compatibility, tenant-specific prefixes, legacy persistence layouts, provider constraints, or organizational naming standards.
- Custom naming may adapt physical provider names, but it must preserve required logical names and mappings when a logical persistence contract exists.
- A naming override must be scoped so changing one name family does not require redefining unrelated name families.

Validation expectations:

- Custom naming must fail clearly for empty, ambiguous, duplicate, unsafe, or non-deterministic names.
- Custom naming must not silently remove required technical columns, logical metadata fields, logical indexes, or convention version markers.

Future expansion boundary:

- Provider-specific reserved word catalogs, quoting behavior, identifier length handling, and physical name escape rules are future provider tickets unless they are required by a generic provider-neutral contract.

## Hashing Hook

Default behavior:

- Stable model hashing uses the default `sha256-v1` algorithm identifier and deterministic normalized text rules from `docs/plans/stable-hashing-contract.md`.
- Logical persistence content hashing uses SHA-256 over canonical payload bytes, stores lowercase hexadecimal digest text, and records the `sha-256` algorithm value defined in `docs/plans/dvault-v1-default-persistence-convention-policy.md`.
- Hashing is for deterministic identity, change detection, and integrity checks. It is not a password hashing, encryption, signing, message authentication, or secret-management boundary.

Optional customization:

- Advanced users may provide a replacement hashing behavior when they need compatibility with an existing vault, a controlled algorithm migration, a deterministic test double, or a future approved hash version.
- A replacement that changes digest values must expose a distinct stable algorithm identifier.
- Hashing customization must remain independent from provider storage location and from timestamp generation unless a later payload contract explicitly includes those values in canonical payload input.

Validation expectations:

- Custom hashing must fail clearly for null input, unsupported value types, invalid normalized values, non-deterministic output, missing algorithm identifiers, or digest values that do not match their declared encoding.
- Custom hashing must not use process-local salts, random values, current culture, current time, machine identifiers, provider-generated identifiers, current directory values, or serializer iteration order as hidden inputs.

Future expansion boundary:

- Algorithm migration, multiple simultaneous hash versions, persisted hash backfill, and provider-accelerated hashing require separate versioned contracts.

## Record Source Hook

Default behavior:

- Record source remains required lineage metadata for hub, link, and satellite records as described in `docs/architecture/mvp-data-vault-concepts.md`.
- The default record source column name is the standard technical column name from the default naming policy.
- The default value source is the ingest or modeling context that accepted the row into the vault. In early examples this can be explicit text such as `crm-import` or `orders-import`.

Optional customization:

- Advanced users may customize record source derivation when the source must come from an envelope field, file name, stream name, tenant boundary, source-system mapping, or existing lineage catalog.
- A custom resolver may normalize a supplied source value, derive one from approved context, or reject a record that lacks sufficient lineage.
- A custom resolver must be scoped to lineage resolution and must not change hash input, timestamp generation, or provider mapping unless those categories are configured separately.

Validation expectations:

- Custom record source resolution must fail clearly when it produces a missing, empty, ambiguous, or non-reproducible source value.
- Custom record source resolution must preserve lineage semantics and must not silently replace unknown sources with a generic fallback.

Future expansion boundary:

- Source catalog integration, multi-tenant lineage rules, source trust levels, and provider-specific lineage metadata are future tickets.

## Timestamp Hook

Default behavior:

- Load timestamps record when a vault row was accepted into the persistence model, as documented in `docs/architecture/mvp-data-vault-concepts.md`.
- Logical persistence timestamps are UTC instants and use ISO 8601 compatible representations with a `Z` UTC designator at the logical boundary.
- Immutable v1 logical records use `created_at_utc`; `updated_at_utc` is absent or null unless a later mutable-record contract requires it.
- Timestamps do not participate in content hashes unless a later payload contract explicitly makes a timestamp part of the canonical payload.

Optional customization:

- Advanced users may customize the time source for deterministic tests, replay imports, externally supplied load timestamps, or controlled clock behavior.
- Advanced users may customize formatting or normalization at the logical boundary when a provider requires a native representation, provided the UTC instant semantics are preserved.
- A timestamp hook must be scoped separately from hashing so clock behavior does not accidentally change content identity.

Validation expectations:

- Custom timestamp behavior must fail clearly for missing required timestamps, non-UTC logical values, ambiguous offsets, non-normalized formats, unsupported precision, or values that cannot round trip through the provider boundary.
- Custom timestamp behavior must not silently use local time, current culture, provider defaults, or lossy conversion when the logical contract requires UTC.

Future expansion boundary:

- Deterministic test-time injection modes, wall-clock production modes, replay semantics, mutable record timestamps, and provider precision matrices require separate implementation tickets.

## Provider Behavior Hook

Default behavior:

- Provider behavior starts from the provider-neutral logical contracts in `docs/plans/dvault-v1-default-persistence-convention-policy.md`.
- Adapters may map logical objects to native tables, collections, files, documents, buckets, key prefixes, or other provider primitives only when they preserve required logical names, field meanings, deterministic behavior, and version metadata.
- Provider-owned metadata may exist, but it must not be required to interpret a logical DVault record.

Optional customization:

- Advanced users may customize bounded provider behavior such as physical object mapping, provider-specific options, batching behavior, or native feature use when the adapter can still round trip the logical DVault contract.
- Provider customization must be isolated to provider behavior and must not redefine naming, hashing, record source, or timestamp semantics unless those hooks are explicitly configured too.
- Provider-specific options should be additive and scoped to the provider that understands them.

Validation expectations:

- Provider customization must fail clearly when a requested option would drop required fields, change logical identity, weaken required lookup behavior, lose canonical payload bytes, hide convention version metadata, or make records provider-only without an approved contract.
- Unknown provider options must not be silently ignored when ignoring them would change persistence behavior.

Future expansion boundary:

- Concrete provider option matrices, dialect-specific DDL, migration behavior, provider-specific optimizations, and additional provider ecosystems are future provider or adapter tickets.

## Current V1 Decisions

- The default DVault path remains zero-configuration.
- Advanced hooks are optional and grouped behind one conceptual advanced configuration surface.
- Unset categories inherit defaults instead of forcing users to configure every category.
- Defaults are documented before customization behavior.
- Custom hooks must fail clearly when invalid instead of silently changing vault behavior.
- Generic provider behavior is a bounded extension point; provider-specific matrices are deferred.
- The current plan does not add runtime APIs or bind concrete method, parameter, helper, or file names.

## Non-Blocking Follow-Up Questions

- Once advanced hook implementation begins, should the configuration surface be documented as stable public API immediately or remain experimental for the first implementation pass?
- Which concrete provider ecosystems should receive provider-specific options first after the generic provider behavior boundary is implemented?
- Should timestamp customization expose deterministic test-time injection and wall-clock production behavior as separate documented modes?