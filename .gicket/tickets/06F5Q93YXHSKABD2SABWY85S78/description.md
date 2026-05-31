<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence confirms DVault already has opt-in save/read telemetry (`AddDVaultTelemetry()`, `IDataVaultTelemetryObserver`, bounded summaries) while the current branch still has no Activity tracing spans; this ticket remains the contract owner and is ready for PO-critic without new child tickets or relation changes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence ratifies the current telemetry baseline: `AddDVault()` stays telemetry-free by default, `AddDVaultTelemetry()` is the opt-in built-in Metrics observer, and custom `IDataVaultTelemetryObserver` services remain optional sibling surfaces rather than prerequisites or replacements for tracing.
- The current branch snapshot shows all planned Activity span names as missing, so this story is still the authoritative pre-implementation contract rather than a duplicate of already-landed tracing work.
- No human clarification comments introduced conflicting naming, redaction, or rollout decisions during this refinement pass.
- Existing outgoing `blocks` relations to `06F5Q9463M0RSHAJJX0F3D1DB0`, `06F5Q94D0JDMMWDXSRGWX1E4F0`, and `06F5Q94KX65TXQ8EC75FWSD01W` already cover downstream implementation fan-out; no additional split or relation cleanup was justified from the current evidence.

### Scope In
- Author one authoritative DVault v1 Activity tracing contract document, preferably under `docs/architecture/`, before any implementation ticket adds spans.
- Lock the ActivitySource name, exact span names, `ActivityKind.Internal`, `Activity.Current` parent propagation, tag keys, bounded value vocabularies, event names, completion status rules, listener-driven opt-in behavior, sampling expectations, and redaction boundary.
- State how tracing complements existing `AddDVaultTelemetry()`, `IDataVaultTelemetryObserver`, and `System.Diagnostics.Metrics` surfaces without changing the default `AddDVault()` behavior.
- Define verification expectations for downstream save, read, and maintenance tracing implementation tickets.

### Scope Out
- Any product-code Activity implementation, exporter setup, dashboards, collectors, alerting, hosting, scheduler, or deployment work.
- Provider-specific SQL spans, ADO.NET child spans, query-plan capture, table-name capture, connection-string capture, or raw diagnostic text capture.
- Public API breaks or any requirement that consumers adopt OpenTelemetry.

## Acceptance Criteria
- A contract document exists and records the required ActivitySource name `DCoding.Data.DVault`, all ten span names, `ActivityKind.Internal`, `Activity.Current` propagation only, and the exact outcome, failure-kind, failure-class, duration-bucket, event-name, and tag-key vocabularies already listed on the ticket.
- The contract states that tracing is listener-driven and preserves the existing no-telemetry default: `AddDVault()` alone remains free of meaningful Activity work when no listener is interested, and implementations must rely on `ActivitySource` listener/sampling checks instead of custom DVault correlation or gating state.
- The contract explicitly reuses existing bounded save/read diagnostics vocabularies for `dvault.strategy.status` and finite fallback-cause values instead of inventing tracing-only alternatives, and it states that non-applicable common tags must be omitted rather than filled with ad hoc sentinel values.
- The contract defines the redaction boundary so Activity names, tags, events, status descriptions, and exception metadata never include raw business keys, hash keys, payload values, caller-supplied metadata names, table names, SQL text, provider error messages, exception messages, stack traces, credentials, or full diagnostic text.
- The contract defines downstream verification for no-listener behavior, listener-enabled span creation, success, fault, and cancellation status mapping, bounded event/tag emission, maintenance noop behavior where applicable, and redaction proof.

## Definition of Done
- The authoritative tracing contract document is landed on an approved documentation or planning surface and is detailed enough that downstream implementation tickets do not need PO invention for span names, tags, events, status, or redaction.
- The document explicitly identifies `IDataVaultTelemetryObserver` and Metrics as existing sibling telemetry surfaces, not prerequisites and not replacements for Activity tracing.
- Exact required names and finite vocabularies are present in the document, or the ticket description is intentionally updated in the same change to keep the contract authoritative.
- If the repository exposes markdown or link validation for docs, that validation passes; otherwise the final review confirms the document contains the exact required names and vocabularies.
- No product-code or product-test changes are required for this story beyond documentation or planning materialization.

## Implementation Notes
- Repository evidence from `docs/releases/v0.16.0.md`, `src/DCoding.Data.DVault/IDataVaultTelemetryObserver.cs`, and the telemetry service and summary types confirms the existing baseline is bounded save/read telemetry plus opt-in Metrics, so the tracing contract should align with that vocabulary instead of reopening it.
- Use the existing save/read diagnostics and telemetry summaries as the source of truth for `dvault.save.mode`, `dvault.read.family`, strategy-status names, strategy-type naming, and finite fallback-cause values; the tracing contract should point implementers at those existing enums and types rather than restating an unconstrained list.
- For common tags such as `dvault.strategy.status` and `dvault.strategy.type`, document an omission rule for operations that do not actually perform a bounded strategy selection so implementations stay low-cardinality and do not invent placeholder values.
- The current branch snapshot shows each planned span name as missing, which is consistent with this ticket being a pre-implementation contract story and with the existing `blocks` relations remaining valid downstream dependency edges.
- No bounded write action was materialized during this refinement pass: no child tickets were created, no relations were changed, no attachments were added, and no planning document was persisted from the PO tool session.

## Open Questions
- none

## Follow-Up Questions
- Should a later observability ticket add maintenance-oriented Metrics or code-facing telemetry summaries that mirror the new maintenance Activity spans, or should maintenance remain Activity-only in v1?
- After the v1 contract lands, does the release want a separate consumer-facing note mapping DVault span and tag names onto OpenTelemetry conventions or exporter examples, while keeping exporters out of this story?
- If provider-specific tracing is ever revisited after v1, should it extend only under additive child spans or stay entirely outside the DVault-owned contract surface?

## Risks
- If downstream implementation starts before the contract document lands, the existing blocked tickets can drift on tag omission rules or redaction boundaries even though the core span-name list is already bounded in this ticket.

## Split Recommendations
- No additional PO split is recommended from current evidence; the story is already bounded to one contract document and already has three downstream `blocks` relations for implementation follow-on.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

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

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Added `docs/architecture/dvault-v1-activity-tracing-contract.md` as the authoritative DVault v1 Activity tracing contract.
- The contract records ActivitySource name `DCoding.Data.DVault`, all ten required span names, `ActivityKind.Internal`, normal `Activity.Current` parent propagation, listener-driven opt-in behavior, tag/event/status vocabularies, redaction boundaries, and downstream verification expectations.
- No product-code Activity implementation, public API change, exporter setup, dashboard, collector, hosting, or test implementation was added in this contract story.

Verification
- `rg` checks confirmed the contract document contains the required ActivitySource name, all ten span names, Activity kind, parent propagation, completion status mapping, redaction section, and verification section.
- `bash tools/check-format.sh` passed.

Notes
- Existing `IDataVaultTelemetryObserver`, `AddDVaultTelemetry()`, and Metrics behavior remain documented as sibling telemetry surfaces, not prerequisites and not replacements for Activity tracing.
- The document uses dot-separated Activity tag keys while explicitly preserving existing bounded diagnostics enum member names for strategy status and fallback-cause values.
<!-- gicket-bot:developer-delivery:v1:end -->