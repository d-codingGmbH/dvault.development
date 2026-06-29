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
  source support bundle, then validate and review the generated `dvault.hash-key-storage-migration.v1` manifest before
  applying any schema or data conversion.
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

The v1 review manifest schema version is `dvault.hash-key-storage-migration.v1`. The design-time
`hash-key-storage-migration` command produces this manifest from a reviewed source `dvault.support-bundle.v1` and the current
configured design-time model. The lower-level machine-checkable validation surface is
`DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(...)`; applications that want one consumer-owned preflight
entrypoint can pass the serialized manifest through `DataVaultPreflightRequest.HashKeyStorageMigrationManifestJson` and run
`DataVaultPreflight.Run(...)`.

Both validation surfaces treat the manifest as a review artifact for one complete selected model boundary. They do not execute
migrations, open a live database by default, generate SQL, mutate data, repair schemas, backfill, dual-write, reconcile, or
rehash when the manifest is invalid or ambiguous.

The implemented top-level manifest shape is:

- `schemaVersion`: exactly `dvault.hash-key-storage-migration.v1`.
- `dryRun`: `enabled=true`, `status=compatible-review-only`, `databaseMutation=none`, `migrationApplication=not-run`,
  `publicHashKeyBoundary=lowercase-hex-no-prefix`, and the `targetDiagnosticsSourceKind` used for the current model facts.
- `source` and `target`: endpoint metadata for the reviewed source baseline and current design-time target, including
  `metadataSourceKind`, `metadataSourceFingerprint`, `providerName`, `capabilityProfile`, and
  `capabilityProfileDefaulted`.
- `comparison`: `intendedChange=HexString-to-Binary`, `compatibilityStatus=compatible-storage-profile-flip`, deterministic
  `entryCount`, `hashKeyColumnCount`, `participantReferenceColumnCount`, and `ordering=ordinal by tableName then propertyName`.
- `entries`: one ordered entry for every in-scope DVault-owned `HashKey` and `ParticipantReference` column in the compared
  model boundary.

Each `entries` item identifies the persisted column and repeats the complete compatibility fact set for both `source` and
`target`:

- `ordinal`, `tableName`, `tableKind`, `entityMetadataName`, `propertyName`, `propertyRole`, `technicalRole`,
  `logicalPropertyKind`, and `propertyMetadataName`.
- `storageProfile`: `HexString` for the source facts and `Binary` for the target facts.
- `providerStoreType` and `providerValueFormat`: `LowercaseHexText` for source provider values and `LowercaseHexBinary` for
  target provider values.
- `efClrModelType`: `System.String` for both source and target, because public and EF model hash-key values remain strings.
- `conversionBehavior`: `none-string-model` for source and `lowercase-hex-string-to-bytes` for target.
- `algorithmId`, `digestByteLength`, and `digestEncoding`; digest encoding must stay `lowercase-hex-no-prefix`.

Coverage is complete-boundary coverage, not a caller-selected subset. Every generated hub, link, satellite, PIT, and bridge
hash-key or hash-key-reference column in the reviewed boundary must appear exactly once. The visible built-in v1 capability
profile baseline is finite: `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`.

The validator emits blocking `error` findings for:

- a missing required top-level section or missing per-column compatibility fact
- malformed `dryRun`, `source`, `target`, `comparison`, or `entries` values
- missing, duplicate, non-contiguous, or count-mismatched `HashKey` or `ParticipantReference` coverage
- mixed, partial, or ambiguous source or target storage profiles inside the selected boundary
- an unsupported provider, capability profile, storage profile, provider value format, conversion behavior, stable-hash
  algorithm id, digest byte length, or digest encoding
- changed provider, capability profile, metadata source fingerprint, `algorithmId`, `digestByteLength`, or digest encoding
  between source and target facts
- a target provider store type that has not changed for the `HexString` to `Binary` storage-profile flip
- the `sha1-v1` versus `sha256-160-v1` same-size case, because equal 20-byte digests do not make the algorithms compatible

Warnings are non-structural only and do not block `DataVaultPreflight.Run(...)`. The visible warning lane is a defaulted
endpoint capability profile (`capabilityProfileDefaulted=true`), which should be reviewed for provider provenance before
planning a cutover. Warnings must not be used for structural manifest defects, coverage gaps, unsupported values, profile
drift, algorithm drift, digest drift, or encoding drift.

The validator emits `info` findings for recognized compatible manifests and keeps output deterministic for the same input. It
sorts findings by severity rank (`error`, then `warning`, then `info`), stable code, table name, column name, and JSON path
using ordinal string comparison. Every finding carries a stable severity, code, JSON path, message, and, when applicable,
expected and actual values. Messages stay redacted and do not include raw hash-key values, raw business keys, SQL text,
credentials, connection strings, or provider exception text.

Use this bounded matrix when reviewing the v1 validator result:

| Scenario | Expected result |
| --- | --- |
| Complete `HexString` source to `Binary` target entries for one supported provider, unchanged hash facts, and reviewed source support-bundle facts | Valid; emit `info` compatibility finding. |
| One hub, link, satellite, PIT, or bridge hash-key/reference column from the reviewed boundary is absent or the comparison counts do not match the entries | Invalid; emit `error` for missing or count-mismatched coverage. |
| The same `tableName` and `propertyName` identity appears more than once | Invalid; emit `error` for duplicate coverage. |
| Source entries mix `HexString` and `Binary`, or target entries mix profiles inside the selected boundary | Invalid; emit `error` for mixed profile facts. |
| Provider or capability profile is not in the built-in v1 baseline | Invalid; emit `error` for unsupported provider or profile values. |
| Source and target use different `algorithmId`, `digestByteLength`, or digest encoding | Invalid; emit `error` for hash-fact drift. |
| `sha1-v1` source is compared with `sha256-160-v1` target and both report 20 digest bytes | Invalid; emit `error` for algorithm drift despite equal byte length. |
| A source or target endpoint has `capabilityProfileDefaulted=true`, while the rest of the manifest is structurally valid | Valid with `warning`; review provider provenance before cutover planning. |

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
4. Validate and review the generated manifest before building migration work. Use
   `DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(...)` directly, or pass the manifest JSON to
   `DataVaultPreflight.Run(...)` through `DataVaultPreflightRequest.HashKeyStorageMigrationManifestJson`. Treat any error
   finding as a failed pre-change gate. Warning-only manifests are non-blocking but still require review.
5. Build a provider-specific consumer migration or data-move script that changes the generated hash-key and
   participant-reference storage from `HexString` to `Binary` and converts each persisted lowercase-hex digest to its byte
   representation.
6. Include all generated DVault hash-key references in the same planned cutover. Do not leave hub hash keys and link, satellite,
   PIT, or bridge references on different physical profiles inside one migrated model boundary.
7. Dry-run the full change against a restored production-like copy. Validate row counts, relationship checks, representative
   read paths, and representative save paths before attempting production cutover.
8. At cutover, freeze writes, take the approved backup or snapshot, apply the schema and data conversion, and run the same
   validation checks before resuming writers.
9. After cutover, capture a fresh support bundle or equivalent translated metadata output and compare it to the intended target
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
| Before migration | The `dvault.hash-key-storage-migration.v1` manifest validates without error findings through the direct validator or the aggregate preflight lane. |
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
