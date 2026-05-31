# Goal
Plan and deliver focused EF Core library improvements for async explicit saves and model-cache safety without turning DVault into an ingestion platform.

# Scope In
- Async explicit save inputs over the existing chunked save semantics.
- Analyzer/runtime diagnostics for EF Core model-cache, compiled-model, and DbContext pooling risks caused by caller-owned dynamic DVault model shape.
- Benchmark, allocation, telemetry, docs, and release evidence.

# Scope Out
- File ingestion, CDC, schedulers, background jobs, dashboards, hosted workers, or automatic database deployment.
- Replacing the existing explicit IDataVaultSaveService boundary.

# Acceptance Criteria
- Child tickets define and implement the bounded async streaming and EF safety work.
- Documentation states how the feature relates to existing DataVaultChunkedSaveRequest and consumer-owned EF model cache responsibilities. TicketSpec