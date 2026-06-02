# DVault V1 Activity Tracing Contract

Status: v1 contract
Ticket: 06F5Q93YXHSKABD2SABWY85S78
Current public baseline: [DVault v0.26.0 Release Notes](../releases/v0.26.0.md)
Activity tracing feature-introduction baseline: [DVault v0.23.0 Release Notes](../releases/v0.23.0.md)
Telemetry baseline: [DVault v0.16.0 Release Notes](../releases/v0.16.0.md)

## Decision

DVault v1 Activity tracing is an opt-in `System.Diagnostics.ActivitySource` surface for explicit save, read, and read-model maintenance operations. The ActivitySource name is:

```text
DCoding.Data.DVault
```

Every DVault-created Activity uses `ActivityKind.Internal`. Implementations use normal `Activity.Current` parent propagation only. DVault must not create custom trace identifiers, custom baggage, custom correlation storage, or DVault-specific parent selection.

Tracing complements the existing telemetry surfaces. `IDataVaultTelemetryObserver`, `DataVaultSaveTelemetrySummary`, `DataVaultReadTelemetrySummary`, `AddDVaultTelemetry()`, and the built-in `System.Diagnostics.Metrics` observer remain sibling telemetry surfaces. They are not prerequisites for Activity tracing and do not replace Activity tracing. `AddDVault()` alone must preserve the current no-telemetry default: without an interested Activity listener, DVault performs no meaningful Activity work beyond the runtime checks needed to discover listener interest.

## Opt-In And Sampling

Tracing is listener-driven. Applications opt in by registering an `ActivityListener`, OpenTelemetry tracing provider, or equivalent listener for the `DCoding.Data.DVault` ActivitySource. DVault does not add exporters, collectors, dashboards, alerts, hosting, or OpenTelemetry package requirements.

Implementations must rely on `ActivitySource` listener and sampling checks instead of custom DVault correlation or gating state. The implementation path should avoid building tag arrays, event payloads, exception metadata, or redacted diagnostic summaries until ActivitySource listener/sampling checks show the data can be observed. When `StartActivity(...)` returns `null`, the operation continues without trace tags or events.

If an Activity is created but `Activity.IsAllDataRequested` is false, implementations must keep work minimal and may omit optional tags and events. Required completion status and outcome tags are emitted only when the Activity instance is available for data.

## Span Names

The Activity name vocabulary is closed for v1. These names are exact and are the only DVault-owned root spans for this contract:

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

`dvault.operation` must equal the Activity name. Implementations must not append table names, metadata names, provider names, request identifiers, counts, outcomes, or exception names to Activity names.

## Tag Keys

Activity tag keys are dot-separated and intentionally separate from existing meter tag spellings. The v1 common tag keys are:

- `dvault.operation`
- `dvault.provider`
- `dvault.strategy.status`
- `dvault.strategy.type`
- `dvault.outcome`
- `dvault.failure.kind`
- `dvault.failure.class`
- `dvault.exception.type`
- `dvault.duration.bucket`

Save span tag keys are:

- `dvault.save.mode`
- `dvault.request.count`
- `dvault.operation.count`
- `dvault.row.count`
- `dvault.saved_record.count`
- `dvault.chunk.count`
- `dvault.processed_chunk.count`
- `dvault.retained_state.high_water`
- `dvault.fallback.cause`
- `dvault.unsupported_shape`

Read span tag keys are:

- `dvault.read.family`
- `dvault.read.mode`
- `dvault.requested_key.count`
- `dvault.returned_row.count`
- `dvault.fallback.cause`

Maintenance span tag keys are:

- `dvault.maintenance.kind`
- `dvault.read_model.kind`
- `dvault.parent_key.count`
- `dvault.affected_row.count`
- `dvault.rebuild.scope`
- `dvault.fallback.cause`

Non-applicable tags must be omitted. Implementations must not fill tags with ad hoc sentinel values such as `none`, `unknown`, `n/a`, empty strings, zero counts for unavailable data, or provider-specific placeholder text. Counts that are actually observed and equal to zero may be emitted as numeric zero.

## Common Tag Values

`dvault.provider` is the Entity Framework provider name when it is already available from the operation context. It must be omitted when unavailable. It must not contain a connection string, server name, database name, user name, schema name, or provider error text.

`dvault.strategy.status` reuses the existing diagnostics enum member names. Save spans use `DataVaultSaveStrategyDiagnosticsStatus`; read spans use `DataVaultReadStrategyDiagnosticsStatus`. The shared v1 status vocabulary is:

- `NotEvaluated`
- `ProviderStrategySelected`
- `ProviderNeutralFallback`

`dvault.strategy.type` is the selected provider strategy type name already exposed by request-bound diagnostics and telemetry summaries. It is emitted only when a strategy is selected and the selected strategy name is available. It must be omitted for `NotEvaluated`, provider-neutral fallback, maintenance operations without a strategy selection, and any operation that cannot provide a bounded strategy type name.

`dvault.outcome` values are:

- `success`
- `fault`
- `canceled`

`dvault.failure.kind` values are:

- `fault`
- `cancellation`

`dvault.failure.class` values are:

- `validation`
- `unsupported_shape`
- `provider`
- `timeout`
- `cancellation`
- `unknown`

`dvault.exception.type` is the exception CLR type name for failed operations. It must not include exception messages, stack traces, provider messages, SQL text, table names, hash-key values, payload values, or caller-supplied metadata names.

`dvault.duration.bucket` values are:

- `lt_10ms` for durations below 10 milliseconds
- `10_99ms` for durations greater than or equal to 10 milliseconds and below 100 milliseconds
- `100_999ms` for durations greater than or equal to 100 milliseconds and below 1 second
- `1_9s` for durations greater than or equal to 1 second and below 10 seconds
- `ge_10s` for durations greater than or equal to 10 seconds

## Save Tag Values

`dvault.save.mode` reuses `DataVaultSaveTelemetryOperationKind` member names:

- `SingleRequest`
- `BulkRequest`
- `ChunkedRequest`

`dvault.request.count`, `dvault.operation.count`, `dvault.row.count`, `dvault.saved_record.count`, `dvault.chunk.count`, `dvault.processed_chunk.count`, and `dvault.retained_state.high_water` are non-negative numeric values taken from bounded save result and telemetry summary data.

`dvault.fallback.cause` on save spans reuses finite save fallback enum member names already emitted by `DataVaultSaveTelemetrySummary`. Provider strategy fallback values are:

- `ProviderNameMismatch`
- `UnknownOrUnregisteredProviderName`
- `NoProviderSpecificStrategyRegistered`
- `DirtyDbContext`
- `MultiActiveSatelliteOperations`
- `SqlServerMinimumOperationThreshold`
- `SqlServerMaximumSatelliteOperationThreshold`
- `MySqlMinimumOperationThreshold`
- `OracleMinimumOperationThreshold`
- `StrategyDeclined`
- `OracleMaximumSatelliteOperationThreshold`
- `StagedProviderBulkDirtyDbContext`
- `StagedProviderBulkUnsupportedShape`
- `StagedProviderBulkTransactionParticipationUnsupported`
- `StagedProviderBulkCleanupFailed`
- `StagedProviderBulkProviderLimitation`

Chunked retained-state fallback values are:

- `RetainedSatelliteSeriesLimitReached`

`dvault.unsupported_shape` reuses finite chunked unsupported-shape enum member names:

- `RetainedSatelliteSeriesLimitExceeded`

Fallback and unsupported-shape tags may be emitted on the Activity or on `dvault.fallback.recorded` events. Multiple fallback causes must be represented as repeated bounded values, not as a comma-delimited string.

## Read Tag Values

`dvault.read.family` reuses `DataVaultReadTelemetryFamily` member names:

- `LatestSatellite`
- `Pit`
- `Bridge`

`dvault.read.mode` values are:

- `Current` for latest/current satellite reads without an as-of cutoff
- `AsOf` for latest-satellite as-of reads and PIT as-of reads
- `Traversal` for bridge traversal reads

`dvault.requested_key.count` and `dvault.returned_row.count` are non-negative numeric values taken from bounded read request, result, and telemetry summary data.

`dvault.fallback.cause` on read spans reuses `DataVaultReadStrategyFallbackCauseKind` member names:

- `ProviderNameMismatch`
- `UnknownOrUnregisteredProviderName`
- `NoProviderSpecificStrategyRegistered`
- `UnsupportedSatelliteParent`
- `MultiActiveSatelliteUnsupported`
- `StrategyDeclined`
- `UnsupportedPitShape`
- `UnsupportedBridgeShape`

Fallback tags may be emitted on the Activity or on `dvault.fallback.recorded` events. Multiple fallback causes must be represented as repeated bounded values, not as a comma-delimited string.

## Maintenance Tag Values

Maintenance spans cover explicit read-model maintenance only. They do not imply background scheduling, provider startup work, EF `SaveChanges` interception, or automatic read-model refresh.

`dvault.maintenance.kind` values are:

- `PitRebuild`
- `PitMaintainParents`
- `BridgeRebuild`
- `BridgeMaintainIncremental`

`dvault.read_model.kind` values are:

- `Pit`
- `Bridge`

`dvault.rebuild.scope` values are:

- `Full` for complete PIT or bridge rebuild spans
- `Parents` for targeted PIT parent maintenance spans
- `Incremental` for bridge incremental maintenance spans

`dvault.parent_key.count` is the number of parent hash keys considered by PIT parent maintenance. It must never include the key values. It is omitted for operations that do not take explicit parent keys.

`dvault.affected_row.count` is a non-negative numeric count of rows changed by the operation. For PIT maintenance it is `RowsDeleted + RowsWritten`. For bridge maintenance it is `RowsInserted + RowsUpdated + RowsDeleted`.

No v1 maintenance-specific fallback-cause enum exists. `dvault.fallback.cause` must be omitted for maintenance operations unless a downstream implementation deliberately reuses one of the finite existing save/read fallback enum member names from an already observed bounded diagnostics surface.

Sampled no-op maintenance operations still create the appropriate maintenance span. They complete with `dvault.outcome=success`, `ActivityStatusCode.Ok`, zero affected rows, and the `dvault.maintenance.noop` event. Unsampled or unlistened no-op operations must not allocate event payloads.

## Events

The v1 Activity event-name vocabulary is closed:

- `dvault.strategy.selected`
- `dvault.fallback.recorded`
- `dvault.chunk.processed`
- `dvault.maintenance.noop`
- `dvault.failure.recorded`

Event attributes, when emitted, must use the tag keys and value vocabularies from this contract. Events must not carry raw request values, raw diagnostic text, generated SQL, provider messages, exception messages, stack traces, table names, or metadata names.

`dvault.strategy.selected` records bounded strategy status and selected strategy type when a save or read strategy selection is observed.

`dvault.fallback.recorded` records one finite fallback cause per event or repeated bounded event attribute. It must not combine multiple causes into an unbounded string.

`dvault.chunk.processed` records bounded chunk progress for chunked saves. It can include non-negative chunk and processed-chunk counts but must not include source record values, hash keys, payload values, table names, retained-state entries, or per-parent listings.

`dvault.maintenance.noop` records an explicit maintenance operation that performed no writes.

`dvault.failure.recorded` records bounded failure kind, failure class, and exception type after a fault or cancellation.

## Completion Status

Successful completion sets:

- `ActivityStatusCode.Ok`
- `dvault.outcome=success`

Faulted completion sets:

- `ActivityStatusCode.Error`
- `dvault.outcome=fault`
- `dvault.failure.kind=fault`
- `dvault.failure.class` using the finite failure-class vocabulary
- `dvault.exception.type` when an exception is available
- `dvault.failure.recorded` when events are requested

Canceled completion sets:

- `ActivityStatusCode.Error`
- `dvault.outcome=canceled`
- `dvault.failure.kind=cancellation`
- `dvault.failure.class=cancellation`
- `dvault.exception.type` when an `OperationCanceledException` or derived exception is available
- `dvault.failure.recorded` when events are requested

Status descriptions must be omitted or use only static bounded text from this contract. They must not include exception messages, provider error messages, SQL text, generated table names, metadata names, hash keys, payload values, credentials, stack traces, or full diagnostics text.

Failure classification is intentionally finite. Validation failures map to `validation`; unsupported DVault shape or contract violations map to `unsupported_shape`; database/provider execution failures map to `provider`; timeout-specific failures map to `timeout`; cancellations map to `cancellation`; otherwise use `unknown`.

## Redaction Boundary

DVault Activity names, tags, events, status descriptions, and exception metadata must never include:

- raw business keys
- hash-key values
- payload values
- caller-supplied metadata names
- generated table names
- generated column names
- SQL text
- query plans
- connection strings
- credentials
- server names or database names
- provider error messages
- exception messages
- stack traces
- full diagnostic text
- per-parent retained-state entries

The tracing surface is for low-cardinality operational shape and outcome evidence. It is not a data inspection, SQL inspection, support-bundle, or diagnostics text transport. Existing diagnostics and support-bundle surfaces remain responsible for their own redacted evidence contracts.

## Verification Expectations

Downstream tracing implementation tickets must include focused verification for:

- No-listener behavior: `AddDVault()` with no interested `ActivityListener` performs the save, read, or maintenance operation without creating Activity instances and without building meaningful tag or event payloads.
- Listener-enabled span creation: each implemented operation starts the exact span name from this contract with `ActivityKind.Internal` and normal `Activity.Current` parent propagation.
- Success mapping: successful operations set `ActivityStatusCode.Ok`, `dvault.outcome=success`, bounded count tags, and applicable save/read/maintenance tags.
- Fault mapping: faulted operations set `ActivityStatusCode.Error`, `dvault.outcome=fault`, `dvault.failure.kind=fault`, a finite `dvault.failure.class`, redacted `dvault.exception.type`, and no raw exception or provider text.
- Cancellation mapping: canceled operations set `ActivityStatusCode.Error`, `dvault.outcome=canceled`, `dvault.failure.kind=cancellation`, `dvault.failure.class=cancellation`, and no raw cancellation message.
- Bounded tag and event emission: strategy, fallback, chunk, and maintenance events use only the closed event names, tag keys, and bounded values from this contract.
- Omission rules: non-applicable strategy, fallback, provider, save, read, and maintenance tags are omitted rather than populated with placeholder values.
- Maintenance no-op behavior: applicable PIT parent-maintenance and bridge-maintenance no-op cases emit `dvault.maintenance.noop` only when the Activity is sampled and requested.
- Redaction proof: Activity names, tags, events, status descriptions, and exception metadata do not contain raw business keys, hash keys, payload values, metadata names, table names, SQL text, provider messages, exception messages, stack traces, credentials, or full diagnostic text.

Product-code implementation, public API changes, tests, exporters, dashboards, collectors, and hosting setup are outside this contract story.
