# DVault Hash-Key Footprint Summary

## Run Context

- Benchmark artifact triplet: benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json
- Hash key variants: sha256-v1-hex, sha256-v1-binary, sha256-128-v1-hex, sha256-128-v1-binary
- Required provider: SQLite local temporary files

## Footprint Rows

| Variant | Provider | Stable hash algorithm | Digest bytes | Hex characters | Storage profile | Hash key store type | Participant reference store type | Hash key value format | Participant reference value format | Hash key payload bytes | Parent reference payload bytes | Two-column hash-reference index payload bytes | Completed rows | Skipped rows | Failed rows |
| --- | --- | --- | ---: | ---: | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| sha256-v1-hex | SQLite local temporary files | sha256-v1 | 32 | 64 | HexString | TEXT | TEXT | LowercaseHexText | LowercaseHexText | 64 | 64 | 128 | 24 | 0 | 0 |
| sha256-v1-binary | SQLite local temporary files | sha256-v1 | 32 | 64 | Binary | BLOB | BLOB | LowercaseHexBinary | LowercaseHexBinary | 32 | 32 | 64 | 24 | 0 | 0 |
| sha256-128-v1-hex | SQLite local temporary files | sha256-128-v1 | 16 | 32 | HexString | TEXT | TEXT | LowercaseHexText | LowercaseHexText | 32 | 32 | 64 | 24 | 0 | 0 |
| sha256-128-v1-binary | SQLite local temporary files | sha256-128-v1 | 16 | 32 | Binary | BLOB | BLOB | LowercaseHexBinary | LowercaseHexBinary | 16 | 16 | 32 | 24 | 0 | 0 |
