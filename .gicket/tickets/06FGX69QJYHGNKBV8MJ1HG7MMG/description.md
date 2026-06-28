Implement a small parser/validator for dvault.hash-key-storage-migration.v1 manifests according to the accepted contract.

Acceptance:
- Valid manifests produce deterministic validation results.
- Invalid schema version, missing table/column coverage, digest-size mismatch, and unsafe mixed storage profile cases are covered by tests.
- The validator does not execute migrations, inspect live databases, or alter EF models.
- Public API shape stays narrow and appropriate for diagnostics/preflight usage.