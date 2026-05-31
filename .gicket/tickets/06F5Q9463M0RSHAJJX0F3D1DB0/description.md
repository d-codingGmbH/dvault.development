<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the contract to explicitly cover latest-satellite typed projection helpers and to pin latest-satellite root-span ownership to the terminal repo-owned execution path for row versus projection reads; no child-ticket, relation, attachment, or planning-document writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current branch evidence shows three public save boundaries in src/DCoding.Data.DVault/DataVaultSaveService.cs, two public read-interface members in src/DCoding.Data.DVault/IDataVaultReadService.cs, and a separate public latest-satellite typed projection helper in src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs plus the public registry typed latest overload recorded in tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:953 and :956.
- ReadCurrentSatelliteAsync(...), ReadAsOfSatelliteAsync(...), and their registry typed variants delegate into the latest-satellite typed helper path, so they inherit latest-satellite tracing from that terminal execution path and must not create wrapper root spans.
- No child-ticket, relation, attachment, or planning-document writes were materialized in this pass.

### Scope In
- Instrument the three public IDataVaultSaveService.SaveAsync overloads in src/DCoding.Data.DVault/DataVaultSaveService.cs with listener-driven Activity tracing that uses the ActivitySource name DCoding.Data.DVault from docs/architecture/dvault-v1-activity-tracing-contract.md.
- Instrument repo-owned latest-satellite row and typed projection execution paths so each explicit latest/current/as-of satellite execution emits exactly one dvault.read.latest_satellite root span, including IDataVaultReadService.ReadLatestSatelliteRowsAsync(...), DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection>, and the public registry typed latest overload recorded in tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:953 and :956.
- Treat ReadCurrentSatelliteRowsAsync(...), ReadAsOfSatelliteRowsAsync(...), ReadCurrentSatelliteAsync(...), ReadAsOfSatelliteAsync(...), and the registry latest/current/as-of helper variants as in-scope delegating callers whose executions must be covered by that same latest-satellite root span without creating wrapper root spans.
- Instrument IDataVaultReadService.ReadPitRowsAsync(...) and DataVaultReadServicePitExtensions.ReadPitAsync(...) so each execution emits one dvault.read.pit root span.
- Instrument DataVaultReadServiceBridgeExtensions.ReadBridgeRowsAsync(...), DataVaultReadServiceBridgeExtensions.ReadBridgeAsync(...), and registry bridge helpers so each execution emits one dvault.read.bridge root span across both DefaultDataVaultReadService and DataVaultBridgeReadPipeline branches.
- Keep tracing compatible with the existing telemetry observer and meter baseline documented in docs/releases/v0.16.0.md.

### Scope Out
- Do not add new IDataVaultReadService members solely for latest/current/as-of satellite, PIT typed projection, registry, or bridge tracing; reuse the existing helper and delegate paths.
- Do not introduce a public code-facing tracing API by default; if one is deliberately added, treat it as additive public surface and update tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt in the same change.
- Do not instrument PIT or bridge maintenance operations; those remain in 06F5Q94D0JDMMWDXSRGWX1E4F0.
- Do not change provider strategy selection, SQL shape, batching thresholds, persistence semantics, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements.
- Do not capture raw SQL, metadata names, table names, hash keys, payload values, record-source values, exception messages, or stack traces in Activity names, tags, or events.
- Do not require tracing coverage for consumer-owned custom IDataVaultSaveService or IDataVaultReadService implementations beyond repo-owned core code and helper surfaces.

## Acceptance Criteria
- With no interested Activity listener, the covered repo-owned save and read paths preserve current observable behavior and create no Activity instances.
- With a listener enabled, the three IDataVaultSaveService.SaveAsync overloads each emit exactly one top-level ActivityKind.Internal span named dvault.save.single_request, dvault.save.bulk_request, or dvault.save.chunked_request.
- With a listener enabled, each latest-satellite execution emits exactly one top-level dvault.read.latest_satellite span at the terminal repo-owned execution path actually used: IDataVaultReadService.ReadLatestSatelliteRowsAsync(...) for row reads or the typed projection execution path reached from DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection> and the registry typed latest overload for typed projection reads.
- ReadCurrentSatelliteRowsAsync(...), ReadAsOfSatelliteRowsAsync(...), ReadCurrentSatelliteAsync(...), ReadAsOfSatelliteAsync(...), and the registry latest/current/as-of helpers inherit that same latest-satellite span and do not add a second root span.
- With a listener enabled, IDataVaultReadService.ReadPitRowsAsync(...) and DataVaultReadServicePitExtensions.ReadPitAsync(...) emit exactly one top-level dvault.read.pit span per execution.
- With a listener enabled, DataVaultReadServiceBridgeExtensions.ReadBridgeRowsAsync(...), DataVaultReadServiceBridgeExtensions.ReadBridgeAsync(...), and registry bridge helpers emit exactly one top-level dvault.read.bridge span per execution across both the DefaultDataVaultReadService and DataVaultBridgeReadPipeline branches.
- Successful operations set ActivityStatusCode.Ok and dvault.outcome=success; faulted and canceled operations set ActivityStatusCode.Error and use only contract-approved bounded failure tags and failure event data.
- Existing telemetry observer, meter, latest-satellite row/projection, PIT, bridge, chunked-save, redaction, and public API snapshot coverage continues to pass, and any intentional new public tracing API addition updates tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt in the same change.

## Definition of Done
- All covered repo-owned save and read paths use one shared ActivitySource name, DCoding.Data.DVault, with ActivityKind.Internal only, normal Activity.Current parent propagation, and no custom trace identifiers, baggage, or DVault-specific parent selection.
- Latest-satellite tracing ownership lives only in the terminal repo-owned execution boundary for the selected path: row reads at the latest-row execution path and typed projection reads at the latest-projection execution path reached from DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection>; current/as-of and registry helper layers stay pass-through and do not create wrapper root spans.
- All tags and events stay within the closed vocabulary from docs/architecture/dvault-v1-activity-tracing-contract.md, omit non-applicable values, and use only bounded counts or existing enum or type-name surfaces.
- Any ActivitySource holder/helper introduced for this story is new internal implementation by default; if a public code-facing tracing API is intentionally introduced, it is treated as an additive API and the approved public API snapshot is updated in the same change.
- Tag and event construction stays behind listener and sampling checks so StartActivity(...) returning null preserves the minimal-overhead baseline.

## Implementation Notes
- docs/architecture/dvault-v1-activity-tracing-contract.md remains the authoritative source for the ActivitySource name, span names, tag and event vocabulary, and listener/sampling behavior; docs/releases/v0.16.0.md remains the telemetry compatibility baseline.
- src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs defines the public explicit typed latest helper ReadLatestSatelliteAsync<TProjection>, and its internal ReadLatestProjectionRowsAsync(...) dispatches either to IDataVaultSatelliteProjectionReadService.ReadLatestSatelliteProjectionRowsAsync(...) or DataVaultSatelliteReadPipeline.ReadLatestProjectionRowsAsync(...).
- src/DCoding.Data.DVault/DefaultDataVaultReadService.cs contains separate repo-owned latest-satellite row and projection execution paths with ReadLatestSatelliteRowsCoreAsync(...) and ReadLatestSatelliteProjectionRowsCoreAsync(...), so tracing ownership cannot be anchored only to IDataVaultReadService.ReadLatestSatelliteRowsAsync(...).
- src/DCoding.Data.DVault/DataVaultReadServiceCurrentSatelliteExtensions.cs and src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs show current/as-of and registry typed helpers delegate into the latest-satellite typed helper surface; those wrappers should remain span-transparent.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:953 and :956 record the explicit and registry typed latest overloads as public API surface and therefore as required contract coverage.
- src/DCoding.Data.DVault/DataVaultSaveService.cs remains the only current-branch public save boundary in scope: three IDataVaultSaveService.SaveAsync overloads plus registry save adapters that delegate into those overloads.

## Open Questions
- none

## Follow-Up Questions
- If consumers later need a code-facing ActivitySource accessor instead of listener registration by source name, should that ship in a separate additive API ticket with explicit public API snapshot review?

## Risks
- If implementation adds wrapper Activities in DataVaultReadServiceCurrentSatelliteExtensions.cs or DataVaultReadServiceRegistryExtensions.cs instead of only at the terminal latest-satellite execution boundary, duplicate root spans can leak into listener output.
- If typed projection tracing is added only to IDataVaultReadService.ReadLatestSatelliteRowsAsync(...) and not to the projection execution path used by DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection>, typed current/as-of and registry helpers will miss dvault.read.latest_satellite coverage.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity(...) returns null.
- If implementation adds a public tracing accessor instead of an internal helper without same-change snapshot review, it can widen the package surface beyond this story's default boundary.

## Split Recommendations
- No split is recommended; current branch evidence still supports one bounded story for save and read tracing, while PIT and bridge maintenance tracing remains separate in 06F5Q94D0JDMMWDXSRGWX1E4F0.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

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