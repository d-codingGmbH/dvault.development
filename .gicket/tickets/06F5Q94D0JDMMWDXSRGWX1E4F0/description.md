# Goal
Implement listener-driven Activity tracing for explicit PIT and bridge maintenance operations using the tracing contract from ticket `06F5Q93YXHSKABD2SABWY85S78`.

# Background
DVault keeps PIT and bridge maintenance as explicit caller-owned service calls. Tracing should make those calls visible when a listener is enabled, while preserving the existing boundary: DVault does not schedule, orchestrate, host, or monitor maintenance jobs.

# Scope In
- Instrument `IDataVaultPitMaintenanceService.RebuildAsync(...)` as `dvault.maintenance.pit.rebuild`.
- Instrument `IDataVaultPitMaintenanceService.MaintainParentsAsync(...)` as `dvault.maintenance.pit.maintain_parents`.
- Instrument `IDataVaultBridgeMaintenanceService.RebuildBridgeAsync(...)` as `dvault.maintenance.bridge.rebuild`.
- Instrument `IDataVaultBridgeMaintenanceService.MaintainBridgeAsync(...)` as `dvault.maintenance.bridge.maintain_incremental`.
- Populate only contract-approved maintenance, strategy/fallback, outcome, count, and duration-bucket tags.
- Add focused tests for listener-disabled behavior, listener-enabled Activities, success, fault, cancellation, no-op maintenance, and redaction.

# Scope Out
- No save/read spans; those are owned by ticket `06F5Q9463M0RSHAJJX0F3D1DB0`.
- No scheduler, background service, retry policy, hosted worker, health check, dashboard, exporter, collector, or alerting setup.
- No provider-specific SQL tracing, generated SQL capture, query-plan capture, connection-string capture, metadata/table-name leakage, raw parent-key leakage, exception-message capture, or stack-trace capture.
- No change to PIT/bridge maintenance semantics or result models unless strictly required for bounded tracing data and covered by compatibility tests.

# Tag And Event Rules
- Use the exact tag keys and value vocabularies from the contract ticket.
- For PIT operations, `dvault.read_model.kind` must distinguish PIT from bridge with a bounded value such as `pit`.
- For bridge operations, `dvault.read_model.kind` must distinguish bridge from PIT with a bounded value such as `bridge`.
- Use `dvault.maintenance.kind` to distinguish rebuild and incremental/parent-bounded maintenance.
- Emit count tags only for parent-key count, affected/deleted/inserted row summaries, and other bounded counts available from existing request/result objects.
- Emit `dvault.maintenance.noop` only when an explicit no-op condition is known from existing result data.
- Emit `dvault.failure.recorded` for fault or cancellation classification without raw exception messages.

# Acceptance Criteria
- With no interested Activity listener, PIT/bridge maintenance completes without emitted Activities and without changing observable results.
- With an Activity listener enabled, each covered maintenance call emits exactly one top-level Activity with the expected span name and `ActivityKind.Internal`.
- Success spans set `ActivityStatusCode.Ok` and `dvault.outcome=success`.
- Fault and cancellation spans set `ActivityStatusCode.Error` and the contract-approved failure/outcome tags.
- Maintenance tags and events contain only low-cardinality counts, enum names, provider/type names, and bounded classifications approved by the contract.
- Existing PIT/bridge maintenance and read-model integration tests still pass.
- The implementation remains independent from application-owned scheduling and monitoring infrastructure.

# Verification
- Run focused PIT and bridge maintenance tests, including new Activity listener assertions.
- Run existing PIT/bridge read-model integration tests to guard behavior.
- Run public API snapshot tests if any public surface is added.