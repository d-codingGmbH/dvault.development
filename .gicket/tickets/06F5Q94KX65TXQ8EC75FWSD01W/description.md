# Goal
Add practical, benchmark-backed performance profile guidance for DVault adopters in v0.23.0.

# Background
DVault already has benchmark artifacts and an evidence contract. This ticket turns that evidence into adopter guidance. The guidance must stay honest about machine-specific timings, required SQLite local baseline, optional external providers, and consumer-owned infrastructure.

# Scope In
- Update documentation that explains performance profiles and when to start from each profile.
- Use repository evidence from `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`, and `docs/plans/performance-evidence-benchmark-artifact-contract.md`.
- Cover these profiles: small app-local vault, medium chunked ingestion, staged provider ingestion, and read-model-heavy PIT/bridge usage.
- For each profile, document the workload shape, likely DVault path, registration guidance, batch or chunk-size starting points, diagnostics/telemetry to inspect, benchmark rows that support the guidance, and stop conditions.
- Preserve existing provider-specific boundaries for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.

# Scope Out
- No product-code changes.
- No new benchmark scenarios unless the existing evidence is insufficient and the gap is explicitly documented.
- No dashboard, hosted monitoring, collector, container, database, credential, scheduler, or CI provisioning template.
- No absolute performance guarantee detached from benchmark run context.
- No claim that optional providers were executed when benchmark artifacts show skipped rows.

# Profile Content Requirements
- Small app-local vault: emphasize default `AddDVault()`, SQLite/local evidence, explicit save/read services, and when provider-neutral fallback is acceptable.
- Medium chunked ingestion: use the existing `customer-profile-streaming-save` evidence and `DataVaultChunkedSaveRequest` guidance; include starting chunk sizes from current evidence, then tell consumers to tune with their own workload.
- Staged provider ingestion: describe provider-eligible ordered bulk workloads, the documented provider thresholds/boundaries, clean-context expectations, and fallback diagnostics to inspect.
- Read-model-heavy PIT/bridge usage: use latest satellite, PIT as-of, and bridge traversal benchmark rows; point to explicit PIT/bridge maintenance boundaries and SQLite read-strategy evidence.

# Acceptance Criteria
- The guidance names the benchmark artifact files it relies on and keeps run context attached to timing claims.
- Each profile includes concrete starting guidance, diagnostics to inspect, and stop conditions.
- Provider guidance matches existing benchmark evidence and release-note posture. Skipped optional-provider rows remain described as skipped evidence, not as measured wins.
- The documentation tells adopters when to use `AddDVault()`, optional provider registrations, `AddDVaultTelemetry()`, existing diagnostics, and benchmark reruns.
- The documentation does not introduce new platform ownership for dashboards, hosting, container/database provisioning, scheduling, alerting, or credential management.
- Existing release/readme wording remains consistent with the performance-evidence artifact contract.

# Verification
- Run available docs/markdown validation if present.
- Inspect links and anchors touched by the change.
- If benchmark artifacts are regenerated, preserve the required markdown/csv/json triplet and explain the run context in the docs.