# DVault Hash-Key Footprint Summary

This summary routes v0.36.0 adopter guidance to the checked-in SQLite-local hash-key storage evidence bundle. The detailed artifact sidecars remain authoritative:

- [benchmark-summary.md](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.md)
- [benchmark-summary.csv](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.csv)
- [benchmark-summary.json](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.json)
- [hash-key-footprint.md](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.md)
- [hash-key-footprint.csv](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.csv)
- [hash-key-footprint.json](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.json)

## Evidence Boundary

- Required provider: SQLite local temporary files.
- Provider filter: `sqlite`.
- Iterations: `1`.
- Warmup iterations: `0`.
- Hash key variants: `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, and `sha256-128-v1-binary`.
- Performance and storage claims must stay scoped to this checked-in bundle unless a future provider-specific bundle is added.

## Footprint Rows

| Variant | Provider | Stable hash algorithm | Digest bytes | Hex characters | Storage profile | Hash key store type | Participant reference store type | Hash key payload bytes | Parent reference payload bytes | Two-column hash-reference index payload bytes |
| --- | --- | --- | ---: | ---: | --- | --- | --- | ---: | ---: | ---: |
| sha256-v1-hex | SQLite local temporary files | sha256-v1 | 32 | 64 | HexString | TEXT | TEXT | 64 | 64 | 128 |
| sha256-v1-binary | SQLite local temporary files | sha256-v1 | 32 | 64 | Binary | BLOB | BLOB | 32 | 32 | 64 |
| sha256-128-v1-hex | SQLite local temporary files | sha256-128-v1 | 16 | 32 | HexString | TEXT | TEXT | 32 | 32 | 64 |
| sha256-128-v1-binary | SQLite local temporary files | sha256-128-v1 | 16 | 32 | Binary | BLOB | BLOB | 16 | 16 | 32 |

## Adoption Notes

Logical hash-key values remain canonical lowercase hexadecimal strings at DVault request, save, read, diagnostics, and support-bundle boundaries. `HexString` is the compatible default storage profile. `Binary` is explicit opt-in physical storage only and keeps the EF model and public DVault boundaries on lowercase hexadecimal string values.

Changing the stable hash algorithm, digest length, truncation policy, or hash-key storage profile after persistence is caller-owned compatibility work. DVault does not automatically migrate, backfill, dual-write, repair, reconcile, or rehash persisted keys.

For existing persisted databases, plan and validate hex-to-binary adoption with the [Hash-Key Storage Migration Guide](docs/hash-key-storage-migration.md). Keep storage and performance claims scoped to the SQLite-local evidence bundle unless a future provider-specific bundle is checked in.
