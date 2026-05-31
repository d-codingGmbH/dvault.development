# Goal
Define the v0.24 async streaming save contract before implementation.

# Scope In
- Decide the narrow public entry point for async chunk sources, preserving existing DataVaultSaveRequest, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest semantics.
- Define ordering, cancellation, transaction participation, retained satellite state, telemetry, Activity tracing, provider fallback, and redaction behavior.
- Explicitly keep file ingestion, CDC, schedulers, and provider-native chunk execution claims out of scope.

# Acceptance Criteria
- Contract material exists and explains how async streaming differs from the already implemented provider-neutral chunked save request.
- Downstream implementation can proceed without inventing API shape, telemetry, or boundary rules.