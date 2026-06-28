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
- For the supported caller-owned design-time path, run the `hash-key-storage-migration` preflight command with the captured
  source support bundle and review the generated `dvault.hash-key-storage-migration.v1` manifest before applying any schema or
  data conversion.
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

## Manifest Validation Contract

The v1 review manifest schema version is `dvault.hash-key-storage-migration.v1`. A validator must treat the manifest as a
preflight contract for one complete selected model boundary. It must not execute migrations, open a live database by default,
generate SQL, mutate data, repair schemas, backfill, dual-write, reconcile, or rehash when the manifest is invalid or
ambiguous.

The required top-level contract facts are:

- `schemaVersion`: exactly `dvault.hash-key-storage-migration.v1`.
- `selectedModelBoundary`: the adopter-owned boundary being migrated, including the metadata source kind and, when available,
  the metadata source fingerprint from the reviewed baseline.
- `reviewedSourceEvidence`: the redacted `dvault.support-bundle.v1` or equivalent translated EF metadata that supplied the
  source facts. This evidence is authoritative for storage-profile and algorithm drift; live-schema evidence is supplemental.
- `providerProfileId`: one of the built-in v1 provider capability profile ids: `sqlite-v1`, `oracle-v1`, `postgres-v1`,
  `sqlserver-v1`, `db2-v1`, or `mysql-pomelo-v1`.
- `modelHashFacts`: the model-level `algorithmId`, `digestByteLength`, and `digestEncoding`. `digestEncoding` must be
  `lowercase-hex-no-prefix`.
- `expectedStorageProfiles`: `source=HexString` and `target=Binary` for v1. Other directions, same-profile audits, and custom
  profile flows are outside this manifest version.
- `coverage`: one entry for every in-scope DVault-owned `HashKey` and `ParticipantReference` column in the selected model
  boundary.
- `validation`: deterministic `error`, `warning`, and `info` findings for the same manifest input.

Each `coverage` entry must identify the persisted column and repeat the complete compatibility fact set for both the source
and target shape:

- logical property kind: `HashKey` or `ParticipantReference`
- table name and column name
- source and target storage profile
- provider store type
- provider value format
- EF CLR model type
- conversion behavior
- `algorithmId`
- `digestByteLength`
- digest encoding

Coverage is complete-boundary coverage, not a caller-selected subset. Every generated hub, link, satellite, PIT, and bridge
hash-key or hash-key-reference column in the reviewed boundary must appear exactly once. The validator must compare coverage by
stable table and column identity after normalizing provider-specific casing only where the selected provider profile documents
that normalization.

The validator must emit blocking `error` findings for:

- a missing required top-level field or missing per-column compatibility fact
- missing or duplicate in-scope `HashKey` or `ParticipantReference` coverage
- mixed, partial, or ambiguous source or target storage profiles inside the selected boundary
- an unsupported `providerProfileId`, storage profile, provider value format, conversion behavior, stable-hash algorithm id,
  digest byte length, or digest encoding
- changed `algorithmId`, `digestByteLength`, or digest encoding between source and target facts
- any decision that treats column width, store type, or payload size alone as compatibility proof
- the `sha1-v1` versus `sha256-160-v1` same-size case, because equal 20-byte digests do not make the algorithms compatible
- any attempted migration execution, SQL generation, data conversion, repair, or live-schema-only decision while validation has
  errors

The validator may emit `warning` findings only for non-blocking evidence gaps, such as a provider live-schema reader being
unavailable or intentionally skipped after the reviewed support bundle or translated metadata supplied the authoritative facts.
Warnings must not be used for structural manifest defects, coverage gaps, unsupported values, profile drift, algorithm drift,
digest drift, or encoding drift.

The validator should emit `info` findings for recognized baseline facts and deterministic coverage totals, including the
provider profile id, stable-hash algorithm id, digest byte length, digest encoding, total coverage count, `HashKey` count, and
`ParticipantReference` count.

Finding output must be stable for the same manifest input. Sort findings by severity rank (`error`, then `warning`, then
`info`), then by stable code, table name, column name, and JSON path using ordinal string comparison. Every finding must carry
a stable severity, code, JSON path, message, and, when applicable, expected and actual values. Messages must stay redacted and
must not include raw hash-key values, raw business keys, SQL text, credentials, connection strings, or provider exception text.

Use this bounded matrix when reviewing or implementing the v1 validator:

| Scenario | Expected result |
| --- | --- |
| Complete `HexString` source to `Binary` target coverage for one supported provider, unchanged hash facts, and redacted reviewed source evidence | Valid; emit `info` baseline and coverage totals. |
| One hub, link, satellite, PIT, or bridge hash-key/reference column from the reviewed boundary is absent | Invalid; emit `error` for missing coverage. |
| The same table and column identity appears more than once | Invalid; emit `error` for duplicate coverage. |
| Source coverage mixes `HexString` and `Binary`, or target coverage mixes profiles inside the selected boundary | Invalid; emit `error` for mixed or ambiguous profile facts. |
| Provider or provider profile id is not in the built-in v1 baseline | Invalid; emit `error` for unsupported provider/profile values. |
| Source and target use different `algorithmId`, `digestByteLength`, or digest encoding | Invalid; emit `error` for hash-fact drift. |
| `sha1-v1` source is compared with `sha256-160-v1` target and both report 20 digest bytes | Invalid; emit `error` for algorithm drift despite equal byte length. |
| Provider live-schema evidence is unavailable, but reviewed support-bundle or translated metadata facts are complete | Valid with `warning`; treat live-schema evidence as supplemental only. |

## Execution Sequence

1. Freeze the target model boundary and decide whether this is a storage-only change. If the application also needs a new
   stable-hash algorithm or digest length, stop and plan that separately.
2. Capture the preflight support bundle or equivalent translated metadata baseline and preserve the source facts with the
   migration change record.
3. Run the caller-owned dry-run manifest before writing database changes:

   ```sh
   dotnet run --project src/SalesVault/SalesVault.csproj -- hash-key-storage-migration --source src/SalesVault/artifacts/dvault-source.support-bundle.json --output src/SalesVault/artifacts/dvault-hash-key-storage-migration.json
   ```

   The command compares the source support-bundle facts against the current design-time model, writes no DDL or DML, and fails
   closed if any compatibility fact changes outside the intended `HexString` to `Binary` storage-profile flip. The generated
   manifest is for CI and change-record review; it is not a migration runner, backfill tool, dual-write mode, repair path, or
   schema synchronizer.
4. Build a provider-specific consumer migration or data-move script that changes the generated hash-key and
   participant-reference storage from `HexString` to `Binary` and converts each persisted lowercase-hex digest to its byte
   representation.
5. Include all generated DVault hash-key references in the same planned cutover. Do not leave hub hash keys and link, satellite,
   PIT, or bridge references on different physical profiles inside one migrated model boundary.
6. Dry-run the full change against a restored production-like copy. Validate row counts, relationship checks, representative
   read paths, and representative save paths before attempting production cutover.
7. At cutover, freeze writes, take the approved backup or snapshot, apply the schema and data conversion, and run the same
   validation checks before resuming writers.
8. After cutover, capture a fresh support bundle or equivalent translated metadata output and compare it to the intended target
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

The checked-in quantified footprint baseline remains SQLite-local. The root [hash-key-footprint.md](../hash-key-footprint.md)
summary and its artifact bundle record SQLite examples for `sha256-v1` and `sha256-128-v1` in `HexString` and `Binary`
profiles. A provider-configured binary-vs-hex matrix is also checked in under
[`artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/`](../artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/).
Use that provider bundle as compatibility and caveat evidence only with its preserved run context: it contains completed
PostgreSQL, MySQL, Oracle, and DB2 timing rows, a skipped SQL Server lane for the local TLS/runtime setup, and failed binary
rows that expose provider-specific storage-profile incompatibilities. Do not promise provider-specific savings or performance
changes for PostgreSQL, SQL Server, Oracle, MySQL, DB2, or other providers from the SQLite evidence alone.

Provider packages may optimize write transport behind the public save service, but they must not silently replace the
registered stable hash service or normalizer with provider-side SQL hashing.
