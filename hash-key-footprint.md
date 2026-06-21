# DVault Hash-Key Footprint Summary

This summary routes v0.42.0 hash-key storage guidance to the checked-in provider-configured binary-vs-hex matrix for ticket `06FE4R1N2ADN77NDFDP4GR7020`. The ticket-labeled artifact bundle is authoritative for timing rows, provider discovery, skipped rows, failed rows, and run context. The root footprint sidecars mirror the same generated footprint metadata for quick validation.

## Authoritative Artifacts

Provider-configured binary-vs-hex matrix:

- [benchmark-summary.md](artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.md)
- [benchmark-summary.csv](artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.csv)
- [benchmark-summary.json](artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/benchmark-summary.json)
- [hash-key-footprint.md](artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.md)
- [hash-key-footprint.csv](artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.csv)
- [hash-key-footprint.json](artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-20260621/hash-key-footprint.json)

Root footprint sidecars:

- [hash-key-footprint.csv](hash-key-footprint.csv)
- [hash-key-footprint.json](hash-key-footprint.json)

Carried-forward SQLite-local storage baseline:

- [benchmark-summary.md](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.md)
- [benchmark-summary.csv](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.csv)
- [benchmark-summary.json](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.json)
- [hash-key-footprint.md](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.md)
- [hash-key-footprint.csv](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.csv)
- [hash-key-footprint.json](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.json)

## Evidence Boundary

- Required provider: SQLite local temporary files.
- Provider filter: `all`.
- Iterations: `1`.
- Warmup iterations: `0`.
- Hash key variants: `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, and `sha256-128-v1-binary`.
- Optional provider status in the provider-configured matrix: PostgreSQL, MySQL, Oracle, and DB2 discovery completed; SQL Server was skipped because the local instance required encryption unsupported by the local runtime.
- Completed provider-configured rows may support timing claims only when cited with the ticket artifact triplet and run context. Skipped SQL Server rows and failed PostgreSQL, MySQL, Oracle, and DB2 rows are caveats and follow-up evidence, not successful provider timing.
- The footprint rows below record deterministic storage-profile, store-type, value-format, and payload-byte facts for the required SQLite footprint sidecar. Cross-provider timing conclusions must come from `benchmark-summary.*` rows with `executionStatus=completed`.
- Binary-vs-hex conclusions must compare like-for-like algorithm pairs first. The `sha256-128-v1` variants are shortened-digest evidence and cannot be described as pure binary-storage wins.

## Footprint Rows

| Variant | Provider | Stable hash algorithm | Digest bytes | Hex characters | Storage profile | Hash key store type | Participant reference store type | Hash key value format | Participant reference value format | Hash key payload bytes | Parent reference payload bytes | Two-column hash-reference index payload bytes | Completed rows | Skipped rows | Failed rows |
| --- | --- | --- | ---: | ---: | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| sha256-v1-hex | SQLite local temporary files | sha256-v1 | 32 | 64 | HexString | TEXT | TEXT | LowercaseHexText | LowercaseHexText | 64 | 64 | 128 | 24 | 0 | 0 |
| sha256-v1-binary | SQLite local temporary files | sha256-v1 | 32 | 64 | Binary | BLOB | BLOB | LowercaseHexBinary | LowercaseHexBinary | 32 | 32 | 64 | 24 | 0 | 0 |
| sha256-128-v1-hex | SQLite local temporary files | sha256-128-v1 | 16 | 32 | HexString | TEXT | TEXT | LowercaseHexText | LowercaseHexText | 32 | 32 | 64 | 24 | 0 | 0 |
| sha256-128-v1-binary | SQLite local temporary files | sha256-128-v1 | 16 | 32 | Binary | BLOB | BLOB | LowercaseHexBinary | LowercaseHexBinary | 16 | 16 | 32 | 24 | 0 | 0 |

## Adoption Notes

Logical hash-key values remain canonical lowercase hexadecimal strings at DVault request, save, read, diagnostics, and support-bundle boundaries. `HexString` is the compatible default storage profile. `Binary` is explicit opt-in physical storage only and keeps the EF model and public DVault boundaries on lowercase hexadecimal string values.

Changing the stable hash algorithm, digest length, truncation policy, or hash-key storage profile after persistence is caller-owned compatibility work. DVault does not automatically migrate, backfill, dual-write, repair, reconcile, or rehash persisted keys.

For existing persisted databases, plan and validate hex-to-binary adoption with the [Hash-Key Storage Migration Guide](docs/hash-key-storage-migration.md). Keep storage and performance claims scoped to checked-in artifact bundles, cite only completed provider-configured timing rows, and keep failed or skipped provider rows as caveats.
