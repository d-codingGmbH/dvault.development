# Goal
Create the source-of-truth Activity tracing contract for DVault v0.23.0 before any implementation ticket adds spans.

# Background
DVault already has opt-in metrics and bounded telemetry summaries. Activity tracing must complement those surfaces without changing the existing default: `AddDVault()` remains telemetry-free unless the application explicitly enables listeners or telemetry. This ticket is the contract owner for the release. Downstream implementation tickets must follow this contract instead of redefining names, tags, events, redaction, or status behavior.

# Scope In
- Add or update one architecture/planning document for the DVault v1 Activity tracing contract, preferably under `docs/architecture/`.
- Define the ActivitySource name, span names, Activity kind, parent/correlation behavior, tag keys, tag value vocabularies, event names, completion status rules, sampling behavior, and redaction boundary.
- Define how tracing relates to existing `AddDVaultTelemetry()`, `IDataVaultTelemetryObserver`, and `System.Diagnostics.Metrics` behavior.
- Define verification expectations for implementation tickets.

# Scope Out
- No Activity implementation in product code.
- No OpenTelemetry exporter, dashboard, alerting, collector, hosting, scheduler, or deployment setup.
- No provider-specific SQL tracing, ADO.NET child-span wrapping, query-plan capture, connection-string capture, or raw exception-message capture.
- No public API break and no requirement that consumers use OpenTelemetry.

# Required Contract Decisions
- ActivitySource name: `DCoding.Data.DVault`.
- Activity kind: `ActivityKind.Internal` for every DVault-created Activity.
- Parent/correlation: use normal `Activity.Current` propagation only. Do not create custom trace identifiers, custom baggage, or DVault-specific correlation storage.
- Opt-in behavior: DVault must not allocate meaningful Activity work when no listener is interested. Implementation should use listener/sampling checks provided by `ActivitySource`.
- Span names:
  - `dvault.save.single_request`
  - `dvault.save.bulk_request`
  - `dvault.save.chunked_request`
  - `dvault.read.latest_satellite`
  - `dvault.read.pit`
  - `dvault.read.bridge`
  - `dvault.maintenance.pit.rebuild`
  - `dvault.maintenance.pit.maintain_parents`
  - `dvault.maintenance.bridge.rebuild`
  - `dvault.maintenance.bridge.maintain_incremental`
- Common tag keys: `dvault.operation`, `dvault.provider`, `dvault.strategy.status`, `dvault.strategy.type`, `dvault.outcome`, `dvault.failure.kind`, `dvault.failure.class`, `dvault.exception.type`, `dvault.duration.bucket`.
- Save tag keys: `dvault.save.mode`, `dvault.request.count`, `dvault.operation.count`, `dvault.row.count`, `dvault.saved_record.count`, `dvault.chunk.count`, `dvault.processed_chunk.count`, `dvault.retained_state.high_water`, `dvault.fallback.cause`, `dvault.unsupported_shape`.
- Read tag keys: `dvault.read.family`, `dvault.read.mode`, `dvault.requested_key.count`, `dvault.returned_row.count`, `dvault.fallback.cause`.
- Maintenance tag keys: `dvault.maintenance.kind`, `dvault.read_model.kind`, `dvault.parent_key.count`, `dvault.affected_row.count`, `dvault.rebuild.scope`, `dvault.fallback.cause`.
- Event names: `dvault.strategy.selected`, `dvault.fallback.recorded`, `dvault.chunk.processed`, `dvault.maintenance.noop`, `dvault.failure.recorded`.
- Outcome values: `success`, `fault`, `canceled`.
- Failure kind values: `fault`, `cancellation`.
- Failure class values: `validation`, `unsupported_shape`, `provider`, `timeout`, `cancellation`, `unknown`.
- Duration bucket values: `lt_10ms`, `10_99ms`, `100_999ms`, `1_9s`, `ge_10s`.
- Strategy status values must reuse the existing enum member names from the save/read diagnostics surfaces. Fallback cause values must reuse the existing finite fallback enum member names.
- Completion: success sets `ActivityStatusCode.Ok` and `dvault.outcome=success`; fault and cancellation set `ActivityStatusCode.Error` with `dvault.outcome=fault` or `dvault.outcome=canceled`.

# Redaction Boundary
The contract must explicitly forbid raw business data in Activity names, tags, events, status descriptions, or exception metadata. Do not emit raw hash keys, business keys, payload values, record source values, metadata object names supplied by a caller, table names, SQL text, query plans, connection strings, credentials, provider error messages, exception messages, stack traces, or full diagnostic text. Counts, enum names, provider invariant names, public strategy type names, and bounded success/failure classifications are allowed.

# Acceptance Criteria
- A contract document exists and contains all Required Contract Decisions above with the same exact names and value vocabularies unless the ticket text is intentionally updated in the same change.
- The document states that tracing is opt-in/listener-driven and does not change the default `AddDVault()` behavior.
- The document states how save/read/maintenance implementation tickets must verify no-listener behavior, listener-enabled spans, success/failure/cancellation status, and redaction.
- The document identifies `IDataVaultTelemetryObserver` and Metrics as existing sibling telemetry surfaces, not replacements and not prerequisites.
- The document has enough detail for the child implementation tickets to proceed without asking PO to invent span names or redaction rules.

# Verification
- Documentation-only change is acceptable for this ticket.
- Run available markdown/link validation if the repository has one. If no markdown validation exists, verify by inspecting the added/updated document and confirming the exact names above are present.
- No product test changes are expected in this ticket.