Define the deterministic validation contract for dvault.hash-key-storage-migration.v1 manifests.

Acceptance:
- Required fields, provider/profile facts, digest sizes, table/column coverage, and expected before/after storage profiles are specified.
- Validation distinguishes errors, warnings, and informational findings.
- The contract rejects ambiguous mixed-profile or missing coverage cases without attempting migration execution.
- The contract aligns with the existing Hash-Key Storage Migration Guide.