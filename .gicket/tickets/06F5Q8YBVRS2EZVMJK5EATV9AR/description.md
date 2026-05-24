Goal: Add provider-specific staged bulk ingestion paths using temporary/transient staging and native bulk APIs where evidence justifies them.

Acceptance criteria:
- Keeps IDataVaultSaveService as the public write boundary and EF metadata as the model source.
- Uses diagnostics gates and provider-neutral fallback for unsupported shapes.
- Treats generated stored procedures as a documented optional future escape hatch, not the default architecture.