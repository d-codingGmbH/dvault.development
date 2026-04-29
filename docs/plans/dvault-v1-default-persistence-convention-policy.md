# DVault V1 Default Persistence Convention Policy

## Purpose

This document defines the v1 default persistence conventions for DVault. It is a planning artifact for the foundation-stage repository and does not require source roots, test roots, persistence providers, migrations, schema generators, hashing code, or runtime configuration APIs.

The policy defines logical defaults. Provider adapters MAY map those logical defaults to native storage primitives, but they MUST preserve the logical names, field meanings, deterministic behavior, and version metadata defined here.

## Normative Terms

The terms MUST, SHOULD, and MAY are used with their usual normative meanings:

- MUST marks a required v1 default.
- SHOULD marks a recommended default that can vary only when a later ticket documents why the variation remains deterministic and provider-neutral.
- MAY marks an optional extension point.

## Baseline Scope

The v1 baseline covers the logical persistence shape for DVault records. A DVault record is the default persisted unit containing a canonical payload plus provider-neutral metadata.

This policy intentionally avoids provider-specific physical schema details. It does not define SQL column types, document-database features, migration tooling, filesystem paths, cloud storage options, or public API names.

## Logical Naming Rules

Logical names are the source of truth. Physical names MAY differ when a provider requires escaping, length limits, reserved-word avoidance, or native naming conventions, but the adapter MUST retain a reversible mapping to the logical names in this policy.

All v1 logical names MUST follow these rules:

- Names MUST use ASCII lowercase snake_case.
- Names MUST use only `a` through `z`, `0` through `9`, and `_`.
- Names MUST start with `dvault_`.
- Names MUST NOT contain provider names, SQL dialect names, region names, tenant names, environment names, or deployment-specific identifiers.
- Names MUST be stable across machines, processes, cultures, time zones, and providers.

### Default Logical Objects

The canonical DVault record artifact uses these required logical objects:

| Logical object | Required name | Purpose |
| --- | --- | --- |
| Record set | `dvault_records` | Stores one logical entry per persisted DVault record. |
| Payload set | `dvault_record_payloads` | Stores canonical payload bytes when a provider separates payloads from record metadata. |
| Metadata set | `dvault_record_metadata` | Stores metadata fields when a provider separates metadata from records. |

Adapters that store payload and metadata together MUST still expose the logical object names above in their mapping documentation. The combined physical object MUST preserve every required metadata field and the canonical payload bytes.

Future DVault artifact kinds MUST use an explicit lowercase snake_case artifact kind token. The default logical object names for a future artifact kind are:

- Record set: `dvault_<artifact_kind>_records`
- Payload set: `dvault_<artifact_kind>_payloads`
- Metadata set: `dvault_<artifact_kind>_metadata`

The canonical record artifact is the only v1 artifact kind defined by this policy. Additional artifact kinds are deferred decisions unless a later ticket explicitly approves them.

### Default Logical Indexes

Indexes are logical lookup requirements, not provider-specific index implementations. Providers that do not have a native index concept MUST document the equivalent lookup behavior they provide.

The canonical record set MUST define these logical indexes:

| Logical index | Required name | Required behavior |
| --- | --- | --- |
| Record identity unique index | `dvault_records__record_id__uk` | Enforces one logical record per `record_id`. |
| Content hash lookup index | `dvault_records__content_hash__idx` | Finds records by `content_hash_algorithm`, `content_hash_canonicalization`, and `content_hash`. |
| Artifact type lookup index | `dvault_records__artifact_type__idx` | Finds records by `artifact_type`. |
| Schema version lookup index | `dvault_records__schema_version__idx` | Finds records by `schema_version`. |

When a provider stores metadata in `dvault_record_metadata`, it MUST define the logical index `dvault_record_metadata__record_id__idx` over `record_id`.

When a provider stores payloads in `dvault_record_payloads`, it MUST define the logical index `dvault_record_payloads__content_hash__uk` over `content_hash_algorithm`, `content_hash_canonicalization`, and `content_hash`. This index is unique because canonical payload bytes with the same hash tuple are treated as the same content identity.

For any additional index approved later, the default name MUST be:

`<logical_object_name>__<field_name_1>__<field_name_n>__<suffix>`

The suffix MUST be `idx` for a lookup index and `uk` for a uniqueness constraint. Composite index fields MUST appear in the logical lookup order documented for that index.

## Required Metadata Fields

All required metadata fields are logical fields. Providers MAY choose native types, but adapters MUST preserve the value semantics below.

| Field | Required default | Semantics |
| --- | --- | --- |
| `record_id` | Required | Stable logical identity for the persisted record. It is unique within `dvault_records`. |
| `artifact_type` | `record` | Stable lowercase snake_case type token for the persisted artifact. |
| `content_hash` | Required | Lowercase hexadecimal SHA-256 digest of the canonical payload bytes. |
| `content_hash_algorithm` | `sha-256` | Hash algorithm identifier for `content_hash`. |
| `content_hash_canonicalization` | Required | Canonicalization identifier used before hashing. |
| `content_hash_encoding` | `lowercase-hex` | Encoding of the stored digest value. |
| `created_at_utc` | Required | UTC creation timestamp for this logical record. |
| `updated_at_utc` | Required only for mutable record categories | UTC timestamp for the latest logical mutation. It MUST be absent or `null` for immutable v1 records. |
| `schema_version` | `1` | Integer schema version for the logical record shape. |
| `convention_version` | `dvault.persistence-conventions.v1` | Convention policy version used to create the record. |
| `payload_encoding` | Required | Encoding or representation of the canonical payload bytes. |

### Record Identity

The default v1 record is immutable. For immutable records, if no caller-approved stable identifier exists, the default `record_id` MUST be:

`sha256:<content_hash>`

The `record_id` is a logical identity, not a storage location. Providers MUST NOT require `record_id` to encode table names, collection names, filesystem paths, bucket names, shard names, tenant names, or environment names.

If a later ticket approves mutable records or caller-supplied identifiers, `record_id` MUST remain stable across updates and `content_hash` MUST represent the current canonical payload content. That later ticket must define the mutation and conflict behavior.

### Timestamp Defaults

Timestamps MUST be stored as UTC instants using an ISO 8601 compatible representation with a `Z` UTC designator at the logical boundary.

The v1 baseline is immutable, so `created_at_utc` records when the logical record was created and `updated_at_utc` MUST be omitted or `null`. Future mutable record categories MUST populate `updated_at_utc` on every logical mutation and MUST keep `created_at_utc` unchanged.

Timestamps MUST NOT participate in content hashing unless a future ticket explicitly defines a payload format where the timestamp is part of the canonical payload.

## Hashing Defaults

Hashing defines content identity and integrity. Hashing MUST NOT define physical storage location.

### Canonical Payload Input

The hash input MUST be canonical payload bytes only. The hash input MUST exclude provider-generated values, storage locations, timestamps, logical object names, index names, and metadata fields unless those values are intentionally part of the payload itself.

The required canonicalization identifiers are:

| Identifier | Input kind | Canonicalization |
| --- | --- | --- |
| `dvault-bytes-v1` | Opaque byte payloads | Use the exact input byte sequence. |
| `dvault-text-v1` | Text payloads | Normalize Unicode to NFC, normalize line endings to LF, and encode as UTF-8 without a byte order mark. |
| `dvault-json-v1` | JSON payloads | Canonicalize using RFC 8785 JSON Canonicalization Scheme and encode as UTF-8 without a byte order mark. |

When the payload kind is known to be JSON, the default canonicalization MUST be `dvault-json-v1`. When the payload kind is known to be text but not JSON, the default canonicalization MUST be `dvault-text-v1`. When the payload kind is unknown or already binary, the default canonicalization MUST be `dvault-bytes-v1`.

### Digest Algorithm and Encoding

The v1 default hash algorithm MUST be SHA-256.

The logical `content_hash_algorithm` value MUST be `sha-256`.

The logical `content_hash` value MUST be the 64-character lowercase hexadecimal encoding of the SHA-256 digest over the canonical payload bytes. The `content_hash` value MUST NOT include the `sha256:` prefix. The prefixed form `sha256:<content_hash>` is reserved for qualified content identity values such as the default immutable `record_id`.

### Hash Usage

V1 implementations MUST use the hash tuple:

`content_hash_algorithm`, `content_hash_canonicalization`, `content_hash`

for content identity and deduplication decisions.

Two records with the same hash tuple represent the same canonical payload bytes. They MAY share physical payload storage, but they MUST remain distinguishable by `record_id` when their logical records are distinct.

On read, providers SHOULD recompute the hash from returned canonical payload bytes and compare it with the stored hash tuple. A mismatch MUST be treated as an integrity failure, not as a cache miss or recoverable naming difference.

## Provider-Neutral Mapping Requirements

Provider adapters MUST follow these constraints:

- Preserve every required logical field and logical object name in adapter mapping documentation.
- Preserve logical uniqueness for `record_id`.
- Preserve logical lookup behavior for the required logical indexes.
- Preserve exact canonical payload bytes for hash verification.
- Preserve `schema_version` and `convention_version` values.
- Avoid relying on provider-specific metadata that cannot be represented by another provider.
- Avoid using provider-generated identifiers as the only durable `record_id`.

Adapters MAY map logical objects to tables, collections, key prefixes, files, documents, buckets, or other native primitives. That mapping is valid only when the adapter can round-trip the required logical names and field values without changing their semantics.

Adapters MAY add provider-owned metadata, but provider-owned metadata MUST NOT be required to interpret the logical DVault record. Provider-owned metadata MUST NOT change `record_id`, `content_hash`, timestamp, schema, or convention semantics.

## Required Defaults and Optional Overrides

The following defaults are required for v1:

- Logical object names defined in this policy.
- Logical index names and lookup behavior defined in this policy.
- Required metadata fields and field names defined in this policy.
- Immutable record behavior unless a later ticket approves mutable records.
- SHA-256 hashing with lowercase hexadecimal digest encoding.
- Provider-neutral mapping constraints.
- `schema_version` value `1`.
- `convention_version` value `dvault.persistence-conventions.v1`.

The following categories are supported override points for future work:

- Physical provider mapping for logical objects.
- Physical provider mapping for logical indexes.
- Additional metadata fields that do not change required field semantics.
- Additional artifact kinds with explicit lowercase snake_case tokens.
- Caller-supplied stable `record_id` behavior.
- Hash canonicalization choices for new payload kinds.
- Hash algorithm changes for future versions.
- Mutable record categories and update semantics.

Overrides MUST preserve deterministic behavior unless a later ticket explicitly approves a different contract. An override MUST document:

- The logical default being overridden.
- The replacement value or behavior.
- The scope where the override applies.
- The convention or schema version that makes the override visible.
- The deterministic rule that lets two implementers derive the same result.

Overrides MUST NOT remove required metadata fields, make hashes non-deterministic, make logical identity depend on storage location, or make provider-specific metadata necessary to interpret a DVault record.

## Deferred Decisions

The following decisions are intentionally deferred and are not part of the v1 baseline:

- First-class adapter guidance for specific persistence providers.
- Runtime configuration APIs, attributes, annotations, or configuration file shapes for overrides.
- Formal migration rules for changes after `dvault.persistence-conventions.v1`.
- Mutable record update, concurrency, and conflict semantics.
- Additional DVault artifact kinds beyond the canonical record artifact.
- Provider-specific physical schema examples beyond logical mapping requirements.

These deferred items are follow-up work. They do not block implementation of the v1 default logical persistence conventions in this document.
