<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement confirms this story stays bounded to listener-driven Activity spans for the four explicit PIT/bridge maintenance entry points; no ticket description, relation, child-ticket, attachment, or planning-document write was needed because the current description, contract doc, and live relations already align.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository-tracked tracing contract fixes the ActivitySource name to `DCoding.Data.DVault`, requires `ActivityKind.Internal`, and keeps `AddDVault()` on the existing no-telemetry default when no listener is interested.
- The live relation set already matches the intended dependency graph: ticket `06F5Q93YXHSKABD2SABWY85S78` blocks this tracing story, this story blocks `06F5Q94SQ086B2DZ1AKFDXGV94`, and the parent link from `06F5Q93R4633D41Z21WQW3SVGR` remains valid.
- No bounded planning writes were materialized in this refinement pass: no child tickets, no relation cleanup, no description rewrite, no attachment, and no planning document.

### Scope In
- Add listener-driven Activity spans for `IDataVaultPitMaintenanceService.RebuildAsync(...)`, `MaintainParentsAsync(...)`, `IDataVaultBridgeMaintenanceService.RebuildBridgeAsync(...)`, and `MaintainBridgeAsync(...)` using the exact maintenance span names already defined by the tracing contract.
- Populate only contract-approved maintenance/common tags and event names, including the closed vocabularies for `dvault.maintenance.kind`, `dvault.read_model.kind`, `dvault.outcome`, `dvault.failure.*`, `dvault.duration.bucket`, and bounded row/count tags already derivable from existing request/result data.
- Add focused PIT/bridge maintenance tracing coverage on top of the existing maintenance and read-model integration suites, including no-listener, listener-enabled, success, fault, cancellation, redaction, and explicit no-op scenarios.

### Scope Out
- Save/read span work remains with ticket `06F5Q9463M0RSHAJJX0F3D1DB0`.
- No scheduler, hosted worker, retry policy, health check, exporter, dashboard, collector, or alerting work.
- No provider-specific SQL/query-plan capture, no table or metadata name leakage, no raw parent-hash-key leakage, no exception-message/stack-trace capture, and no maintenance semantic changes beyond bounded tracing data.

## Acceptance Criteria
- With no interested Activity listener, all four maintenance entry points complete with unchanged observable behavior and without emitted Activities or meaningful tag/event allocation beyond listener checks.
- With a listener enabled, each covered call emits exactly one top-level `ActivityKind.Internal` span named `dvault.maintenance.pit.rebuild`, `dvault.maintenance.pit.maintain_parents`, `dvault.maintenance.bridge.rebuild`, or `dvault.maintenance.bridge.maintain_incremental` as applicable.
- Successful spans set `ActivityStatusCode.Ok`, `dvault.outcome=success`, `dvault.operation` equal to the span name, the correct `dvault.maintenance.kind`, the correct `dvault.read_model.kind`, and only the bounded maintenance tags that are actually applicable to that operation.
- Faulted and canceled spans set `ActivityStatusCode.Error`, emit `dvault.outcome`, `dvault.failure.kind`, `dvault.failure.class`, and bounded `dvault.exception.type` per contract, and never include raw exception/provider text.
- Applicable no-op cases emit `dvault.maintenance.noop` only when an Activity exists and the no-op condition is explicitly known from existing request/result data; non-applicable operations omit the event instead of inventing placeholder semantics.
- Focused PIT/bridge maintenance tests and existing PIT/bridge read-model integration coverage continue to pass, and public API snapshot tests are updated only if the implementation introduces a public surface.

## Definition of Done
- The implementation reuses the shared DVault Activity tracing contract without introducing a new ActivitySource, custom correlation, or automatic maintenance orchestration.
- Maintenance spans omit non-applicable tags instead of using sentinel values and keep redaction boundaries intact for keys, metadata names, table names, SQL text, provider messages, exception messages, and other unbounded diagnostics.
- Affected-row math follows the contract baseline: PIT uses `RowsDeleted + RowsWritten`, bridge uses `RowsInserted + RowsUpdated + RowsDeleted`, and parent-key counts never expose raw key values.
- Repository-focused verification covers the existing PIT and bridge maintenance integration suites plus new Activity listener assertions for success, fault, cancellation, listener-disabled behavior, and redaction.

## Implementation Notes
- Use the contract's listener-driven fast path: if `StartActivity(...)` returns `null`, continue without building tags or events; if an Activity exists but `IsAllDataRequested` is false, keep emitted data to the required bounded minimum.
- Ratify the closed maintenance vocabularies from the contract instead of reopening naming decisions: `dvault.maintenance.kind` = `PitRebuild|PitMaintainParents|BridgeRebuild|BridgeMaintainIncremental`, `dvault.read_model.kind` = `Pit|Bridge`, and `dvault.rebuild.scope` = `Full|Parents|Incremental`.
- Keep no-op assertions tied to repository-proven explicit cases rather than forcing new semantics: PIT empty parent-key maintenance is an established no-op, and bridge incremental no-op coverage should rely on zero change counts already surfaced by the existing result model.
- The repository evidence already points to the maintenance test baseline in `DataVaultPitMaintenanceServiceSqliteTests.cs` and `DataVaultBridgeMaintenanceServiceSqliteTests.cs`; extend those suites first, and keep existing PIT/bridge read-model integration tests in the verification pass.

## Open Questions
- none

## Follow-Up Questions
- none

## Risks
- This story remains dependency-bound by the live incoming `blocks` relation from `06F5Q93YXHSKABD2SABWY85S78`; if the shared tracing contract implementation changes tag/event helper mechanics late, maintenance instrumentation may need a small alignment pass.
- No-op coverage must stay anchored to explicit existing request/result evidence; over-eager emission on rebuild paths would violate the redaction and bounded-semantics contract.

## Split Recommendations
- No split recommended; the story is already bounded to four explicit maintenance entry points, fixed contract vocabularies, and existing PIT/bridge maintenance test lanes.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

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