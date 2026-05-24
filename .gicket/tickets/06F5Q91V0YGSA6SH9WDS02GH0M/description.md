Goal: Improve compile-time ergonomics and hash compatibility confidence without hiding Data Vault semantics.

Acceptance criteria:
- Generates typed read helpers only for stable metadata-defined read shapes.
- Adds hash canonicalization governance and compatibility vectors.
- Keeps dynamic IDataVaultReadService requests as the default runtime-built path.