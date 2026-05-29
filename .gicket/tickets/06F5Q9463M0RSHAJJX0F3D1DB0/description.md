# Goal
Implement listener-driven Activity tracing for explicit save and read service operations using the tracing contract from ticket `06F5Q93YXHSKABD2SABWY85S78`.

# Background
The contract ticket owns names, tags, events, status behavior, and redaction. This ticket owns product-code instrumentation for `IDataVaultSaveService` and `IDataVaultReadService` only. Existing metrics and `IDataVaultTelemetryObserver` summaries must continue to work exactly as before.

# Scope In
- Add the shared ActivitySource surface required by the contract, using ActivitySource name `DCoding.Data.DVault`.
- Instrument `IDataVaultSaveService` explicit save paths for single request, ordered bulk request, and chunked request spans.
- Instrument `IDataVaultReadService` read paths for latest/current/as-of satellite reads, PIT reads, and bridge reads.
- Populate only contract-approved tags and events using existing telemetry summary and diagnostics data where possible.
- Cover success, fault, and cancellation completion behavior.
- Add focused unit/integration tests for listener-disabled behavior, listener-enabled Activities, redaction, and existing telemetry compatibility.

# Scope Out
- No PIT or bridge maintenance spans; those are owned by ticket `06F5Q94D0JDMMWDXSRGWX1E4F0`.
- No changes to provider strategy selection, SQL shape, batching thresholds, persistence semantics, benchmark harness, dashboards, exporters, hosting, or OpenTelemetry package requirements.
- No raw SQL, query-plan, connection-string, business-key, hash-key, payload, record-source, exception-message, or stack-trace capture.
- No public API break. Adding a public ActivitySource holder is allowed only if it follows existing repository API policy and snapshot tests are updated intentionally.

# Span Coverage
- `DataVaultSaveTelemetryOperationKind.SingleRequest` -> `dvault.save.single_request`.
- `DataVaultSaveTelemetryOperationKind.BulkRequest` -> `dvault.save.bulk_request`.
- `DataVaultSaveTelemetryOperationKind.ChunkedRequest` -> `dvault.save.chunked_request`.
- `DataVaultReadTelemetryFamily.LatestSatellite` -> `dvault.read.latest_satellite`.
- `DataVaultReadTelemetryFamily.Pit` -> `dvault.read.pit`.
- `DataVaultReadTelemetryFamily.Bridge` -> `dvault.read.bridge`.

# Tag And Event Rules
- Use the exact tag keys and value vocabularies from the contract ticket.
- Use existing save/read telemetry and diagnostics values for operation kind, read family, provider name, selected strategy type name, strategy status, finite fallback causes, counts, duration bucket, unsupported shape summary, and outcome.
- Emit `dvault.strategy.selected` only with bounded strategy/provider/status data.
- Emit `dvault.fallback.recorded` only when finite fallback causes exist.
- Emit `dvault.chunk.processed` for chunked saves with chunk index/count and bounded row/count data only.
- Emit `dvault.failure.recorded` for fault or cancellation classification without raw exception messages.

# Acceptance Criteria
- With no interested Activity listener, save/read operations complete without emitted Activities and without changing observable results.
- With an Activity listener enabled, each covered save/read operation emits exactly one top-level Activity with the expected span name and `ActivityKind.Internal`.
- Success spans set `ActivityStatusCode.Ok` and `dvault.outcome=success`.
- Fault and cancellation spans set `ActivityStatusCode.Error` and the contract-approved failure/outcome tags.
- Tags and events contain only low-cardinality counts, enum names, provider invariant/type names, and bounded classifications approved by the contract.
- Existing metrics and `IDataVaultTelemetryObserver` tests still pass, including the existing redaction guarantees.
- Public API snapshot changes, if any, are intentional and documented by tests.

# Verification
- Run the focused DVault unit/integration tests that cover save/read telemetry and Activity tracing.
- Run the existing telemetry tests to prove observer and metric behavior were not regressed.
- Run public API snapshot tests if any public surface is added.