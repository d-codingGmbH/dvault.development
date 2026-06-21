# Hash-Key Storage Migration Guide

Use this guide when an application owner wants to move existing persisted DVault hash-key storage from the default
`HexString` physical profile to the explicit opt-in `Binary` physical profile. DVault keeps one logical hash-key
representation: public APIs, save requests, read requests, diagnostics, explain output, and support bundles continue to use
canonical lowercase hexadecimal strings without a prefix.

This is an adopter-owned migration plan. DVault does not automatically migrate, backfill, dual-write, repair, reconcile, or
rehash persisted keys.

## When To Use This Guide

For new schemas or new projects, select the binary-first profile before creating the database:

```csharp
services.AddDVault(options => options.UseBinaryFirstProfile());
modelBuilder.ApplyDataVaultMetadataWithBinaryFirstProfile(vault => {
  // Code-First hub, link, and satellite declarations.
});
```

That new-schema path changes the recommended physical storage for generated hash-key columns, but it does not migrate an
existing database or configuration.

For an existing persisted database, keep `HexString` until the application owner has a reviewed migration, reset, or data-move
plan. Changing the hash-key storage profile after data exists is persisted compatibility work and must be validated before any
cutover.

Do not combine a stable-hash algorithm change with a storage-profile migration. Treat algorithm, digest-length, truncation, and
storage-profile changes as separate compatibility events.

## Compatibility Baseline

The storage-profile boundary is:

| Profile | Physical storage contract | Caller-facing value |
| --- | --- | --- |
| `HexString` | Persist canonical lowercase hexadecimal digest text. EF model CLR type remains `string`; conversion behavior is `none-string-model`. | Lowercase hexadecimal string. |
| `Binary` | Persist digest bytes. EF model and public DVault boundaries remain lowercase hexadecimal `string` values; conversion behavior is `lowercase-hex-string-to-bytes`. | Lowercase hexadecimal string. |

The visible built-in v1 stable-hash sizing baseline is:

| `algorithmId` | Digest bytes | Hex characters |
| --- | ---: | ---: |
| `sha256-v1` | 32 | 64 |
| `sha1-v1` | 20 | 40 |
| `sha256-128-v1` | 16 | 32 |
| `sha256-160-v1` | 20 | 40 |

Column width and provider store type are not enough to prove compatibility. For example, `sha1-v1` and `sha256-160-v1` both
produce 20 digest bytes and 40 lowercase-hex characters, but they are incompatible algorithm choices and require an explicit
reviewed migration, reset, or data-move plan outside a storage-only change.

## Preflight Checklist

Before scaffolding or applying any migration step:

- Capture a redacted `dvault.support-bundle.v1` output or equivalent translated EF metadata baseline from the configured
  consumer application. Use live-schema facts only where the selected provider exposes them under the application's normal
  operational controls.
- Identify every DVault-owned `HashKey` and `ParticipantReference` column across generated hubs, links, satellites, PITs, and
  bridges in the model boundary being migrated.
- Compare the source and target storage profile for every identified property. A storage-only migration should move
  `HexString` to `Binary` intentionally and consistently for the selected model boundary.
- Confirm the active `algorithmId`, `digestByteLength`, and digest encoding remain unchanged. Digest encoding stays
  `lowercase-hex-no-prefix`.
- Compare provider store type, provider value format, EF CLR model type, and conversion behavior for every hash-key and
  hash-key-reference property. The planned target must explain each change from text storage to binary storage.
- Verify public and request-level values remain lowercase hexadecimal strings. The data conversion changes persisted bytes, not
  caller-facing value types.
- Confirm the migration plan covers every persisted reference to the changed hash keys, including participant-reference
  columns and any maintained PIT or bridge tables that store hash-key references.
- Establish an application-owned write freeze, backup, restore test, and rollback decision point before changing persisted
  data.
- Fail closed when any persisted compatibility fact differs unexpectedly: storage profile, stable-hash `algorithmId`, digest
  byte length, provider store type, provider value format, EF CLR projection or conversion behavior, or equivalent persisted
  shape.

Support bundles and diagnostics must stay redacted. Do not put raw hash-key values, raw business keys, request values, SQL
text, credentials, connection strings, provider messages, or diagnostic payload text into migration records.

## Execution Sequence

1. Freeze the target model boundary and decide whether this is a storage-only change. If the application also needs a new
   stable-hash algorithm or digest length, stop and plan that separately.
2. Capture the preflight support bundle or equivalent translated metadata baseline and preserve the source facts with the
   migration change record.
3. Build a provider-specific consumer migration or data-move script that changes the generated hash-key and
   participant-reference storage from `HexString` to `Binary` and converts each persisted lowercase-hex digest to its byte
   representation.
4. Include all generated DVault hash-key references in the same planned cutover. Do not leave hub hash keys and link, satellite,
   PIT, or bridge references on different physical profiles inside one migrated model boundary.
5. Dry-run the full change against a restored production-like copy. Validate row counts, relationship checks, representative
   read paths, and representative save paths before attempting production cutover.
6. At cutover, freeze writes, take the approved backup or snapshot, apply the schema and data conversion, and run the same
   validation checks before resuming writers.
7. After cutover, capture a fresh support bundle or equivalent translated metadata output and compare it to the intended target
   facts. Resume writes only after the target profile, algorithm id, digest byte length, provider store type, provider value
   format, and conversion behavior match the plan.

Keep ordinary DVault calls unchanged after cutover. Save and read requests still carry lowercase hexadecimal hash-key values
through the public service boundaries.

## Rollback Expectations

Rollback is caller-owned operational work:

- Before writes resume, rollback should restore the approved backup or apply a reviewed reverse conversion to the previous
  `HexString` shape, then re-run the preflight and representative validation checks.
- After writes resume, rollback is a new data-move event. Freeze writers again, preserve post-cutover data, and validate any
  reverse conversion before exposing the previous profile.
- If support-bundle, translated metadata, or live-schema facts drift from the approved plan, stop the cutover and keep the
  system in a fail-closed state until the discrepancy is reviewed.
- If an algorithm id, digest length, or digest encoding changed as part of the attempted migration, do not treat it as a binary
  storage rollback. Plan the algorithm compatibility event separately.

DVault does not provide hidden repair, reconcile, dual-write, or rehash behavior for these rollback paths.

## Validation Checkpoints

Use concrete checkpoints before, during, and after cutover:

| Phase | Checkpoint |
| --- | --- |
| Before migration | Source support-bundle or translated metadata facts are captured for every `HashKey` and `ParticipantReference`. |
| Before migration | `algorithmId`, `digestByteLength`, and `lowercase-hex-no-prefix` encoding are unchanged from the approved source baseline. |
| Before migration | Target provider store type, value format, and conversion behavior are explicitly reviewed for the selected provider profile. |
| Dry run | All generated hash-key and reference columns are converted together on a restored copy, with application-owned row-count and relationship checks passing. |
| Cutover | Writers are frozen, backup or snapshot is complete, schema and data conversion run in the approved order, and validation passes before writers resume. |
| After cutover | Fresh support-bundle or translated metadata facts show `Binary`, the same `algorithmId`, the same digest byte length, expected provider store types, expected provider value formats, and `lowercase-hex-string-to-bytes` conversion behavior. |
| After cutover | Representative save and read flows continue to use lowercase hexadecimal strings at the public boundary. |

## Provider Caveats

The visible built-in provider profile baseline is finite: `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`,
and `mysql-pomelo-v1`. Do not infer guarantees for an unrecognized provider from those profiles.

Provider live-schema evidence is not identical across providers. The support bundle and translated metadata facts are the
authoritative preflight baseline for storage-profile and algorithm drift. Use provider live-schema reads as supplemental
evidence only when the selected provider exposes them under the consumer application's operational controls. DB2 live-schema
reading is outside the hash-key storage-profile contract, even though DB2 may expose storage-type facts as external opt-in
evidence.

The checked-in quantified footprint evidence is SQLite-local. The root [hash-key-footprint.md](../hash-key-footprint.md)
summary and its artifact bundle record SQLite examples for `sha256-v1` and `sha256-128-v1` in `HexString` and `Binary`
profiles. Keep storage and lookup/read claims scoped to that bundle unless a future provider-specific evidence bundle is
checked in. Do not promise provider-specific savings or performance changes for PostgreSQL, SQL Server, Oracle, MySQL, DB2, or
other providers from the SQLite evidence alone.

Provider packages may optimize write transport behind the public save service, but they must not silently replace the
registered stable hash service or normalizer with provider-side SQL hashing.
