# Hash Key Storage Profile Contract

Status: v1 design contract
Ticket: 06F9GF5FV54DGWY9GA8ZEZWM5R
Milestone: Foundation and architecture

## Purpose

DVault hash keys have one logical representation and a bounded set of physical storage profiles. The logical representation is
always canonical lowercase hexadecimal text without a prefix at API, request, metadata, diagnostics, and support-bundle
boundaries. Physical storage is provider profile metadata and must not change the caller-facing hash-key value type.

This contract applies to DVault-owned hash-key columns and hash-key-reference columns on hubs, links, satellites, PITs, and
bridges. It does not change HashDiff/content-hash behavior, does not add provider-side SQL hashing, and does not implement
automatic repair, rehashing, backfill, or dual-write migration behavior.

## Storage Profiles

The bounded v1 storage-profile vocabulary is:

| Profile | Contract |
| --- | --- |
| `HexString` | Default. Persist the canonical lowercase hexadecimal digest text. EF model CLR type remains `string`; conversion behavior is `none-string-model`. |
| `Binary` | Explicit opt-in. Persist digest bytes while the EF model and public DVault boundaries remain lowercase hexadecimal `string` values; conversion behavior is `lowercase-hex-string-to-bytes`. |

`HexString` is the default for all built-in provider profiles. `Binary` is a persistence-only profile; callers still supply,
receive, inspect, and compare hash keys as lowercase hexadecimal strings.

## Stable Hash Sizing

One stable-hash algorithm is active for a model. The active `algorithmId` and digest byte length size every DVault-owned
hash-key and hash-key-reference column in that model.

The built-in v1 sizing baseline is:

| `algorithmId` | Digest bytes | Hex characters |
| --- | ---: | ---: |
| `sha256-v1` | 32 | 64 |
| `sha1-v1` | 20 | 40 |
| `sha256-128-v1` | 16 | 32 |
| `sha256-160-v1` | 20 | 40 |

For `HexString`, provider profiles that declare bounded text storage use the active hex character count. SQLite remains `TEXT`.
For `Binary`, provider profiles that declare bounded binary storage use the active digest byte count. PostgreSQL uses `bytea`
for binary digest storage.

## Required Metadata Facts

Provider capability profiles and translated EF metadata must expose these facts for every hash key and hash-key reference:

- logical property kind: `HashKey` or `ParticipantReference`
- storage profile: `HexString` or `Binary`
- provider store type
- provider value format
- EF CLR model type and conversion behavior
- active stable-hash `algorithmId`
- declared `digestByteLength`
- digest encoding: `lowercase-hex-no-prefix`

Diagnostics and `dvault.support-bundle.v1` output must carry the same facts without raw hash-key values or raw business keys.
The reviewed support bundle is the authoritative preflight baseline when checking algorithm or storage drift.

## Guardrails

Migration and preflight guardrails must fail closed when a DVault-owned hash-key or hash-key-reference column changes any
persisted compatibility fact without an intentional contract change:

- storage profile
- stable-hash `algorithmId`
- digest byte length
- provider store type
- provider value format
- EF CLR projection or conversion behavior
- equivalent persisted shape

Column width and provider store type are not sufficient compatibility checks. A change from `sha1-v1` to `sha256-160-v1` is
incompatible even though both algorithms produce 20 digest bytes and 40 lowercase-hex characters. Such a change requires an
explicit reviewed migration, reset, or data-move plan outside this default fail-closed posture.

## Provider Baseline

The visible built-in provider profile baseline is:

- `sqlite-v1`
- `oracle-v1`
- `postgres-v1`
- `sqlserver-v1`
- `db2-v1`
- `mysql-pomelo-v1`

DB2 live-schema reading is outside this hash-key storage contract. The DB2 catalog reader may expose the storage-type facts
described here as external opt-in evidence, but this contract does not provision DB2 databases or change live-schema ownership.

## Non-Goals

This contract does not:

- change public hash-key values from `string` to `byte[]`
- change request, save, read, explain, or support-bundle boundaries to emit raw bytes
- add automatic rehashing, repair, backfill, reset, or dual-write migration tooling
- add provider-side SQL hashing
- change HashDiff/content-hash storage
- define DB2 catalog-reader behavior beyond the storage-type facts above
