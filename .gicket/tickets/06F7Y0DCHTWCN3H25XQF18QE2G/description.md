# Goal
Implement the async chunked save entry point selected by the v0.24 contract.

# Scope In
- Process async chunks in caller order without materializing the complete source first.
- Preserve explicit load timestamp, record source, cancellation, transaction, telemetry, Activity tracing, and redaction behavior.
- Update public API snapshots for intentional public surface changes.

# Scope Out
No file ingestion, background worker, automatic retry loop, scheduler, or provider-native chunk execution guarantee.

# Acceptance Criteria
- Async chunk input works for large ordered sources with bounded materialization.
- Tests cover no-op sources, cancellation, transaction participation, telemetry/tracing, failure cleanup, and compatibility with existing save paths.