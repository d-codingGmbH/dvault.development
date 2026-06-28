Turn the existing dvault.hash-key-storage-migration.v1 manifest guidance into a deterministic validation aid for consumer-owned HexString-to-Binary migration planning.

Acceptance:
- The manifest schema and validation rules are explicit enough for automated checks.
- Consumers can validate a migration dry-run plan before changing EF migrations, storage profiles, or data movement scripts.
- The feature does not perform automatic persisted-hash migration and does not change logical public hash-key values.
- Diagnostics and docs distinguish planning evidence from execution.