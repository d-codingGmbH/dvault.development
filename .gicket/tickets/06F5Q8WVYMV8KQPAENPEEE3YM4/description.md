Goal: Add memory-bounded streaming and chunked explicit save inputs for large DVault loads while preserving IDataVaultSaveService as the write boundary.

Acceptance criteria:
- Defines the public streaming/chunking contract.
- Keeps DbContext ownership, transactions, cancellation, load timestamp, record source, and fallback semantics explicit.
- Excludes background workers, schedulers, file ingestion, CDC, and platform orchestration.