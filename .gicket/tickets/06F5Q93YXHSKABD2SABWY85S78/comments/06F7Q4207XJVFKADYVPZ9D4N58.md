[gicket-bot] PO refinement contract

Summary
- Repository evidence confirms DVault already has opt-in save/read telemetry (`AddDVaultTelemetry()`, `IDataVaultTelemetryObserver`, bounded summaries) while the current branch still has no Activity tracing spans; this ticket remains the contract owner and is ready for PO-critic without new child tickets or relation changes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence ratifies the current telemetry baseline: `AddDVault()` stays telemetry-free by default, `AddDVaultTelemetry()` is the opt-in built-in Metrics observer, and custom `IDataVaultTelemetryObserver` services remain optional sibling surfaces rather than prerequisites or replacements for tracing.
- The current branch snapshot shows all planned Activity span names as missing, so this story is still the authoritative pre-implementation contract rather than a duplicate of already-landed tracing work.
- No human clarification comments introduced conflicting naming, redaction, or rollout decisions during this refinement pass.
- Existing outgoing `blocks` relations to `06F5Q9463M0RSHAJJX0F3D1DB0`, `06F5Q94D0JDMMWDXSRGWX1E4F0`, and `06F5Q94KX65TXQ8EC75FWSD01W` already cover downstream implementation fan-out; no additional split or relation cleanup was justified from the current evidence.

Scope In
- Author one authoritative DVault v1 Activity tracing contract document, preferably under `docs/architecture/`, before any implementation ticket adds spans.
- Lock the ActivitySource name, exact span names, `ActivityKind.Internal`, `Activity.Current` parent propagation, tag keys, bounded value vocabularies, event names, completion status rules, listener-driven opt-in behavior, sampling expectations, and redaction boundary.
- State how tracing complements existing `AddDVaultTelemetry()`, `IDataVaultTelemetryObserver`, and `System.Diagnostics.Metrics` surfaces without changing the default `AddDVault()` behavior.
- Define verification expectations for downstream save, read, and maintenance tracing implementation tickets.

Scope Out
- Any product-code Activity implementation, exporter setup, dashboards, collectors, alerting, hosting, scheduler, or deployment work.
- Provider-specific SQL spans, ADO.NET child spans, query-plan capture, table-name capture, connection-string capture, or raw diagnostic text capture.
- Public API breaks or any requirement that consumers adopt OpenTelemetry.

Open questions
- none

Follow-up questions
- Should a later observability ticket add maintenance-oriented Metrics or code-facing telemetry summaries that mirror the new maintenance Activity spans, or should maintenance remain Activity-only in v1?
- After the v1 contract lands, does the release want a separate consumer-facing note mapping DVault span and tag names onto OpenTelemetry conventions or exporter examples, while keeping exporters out of this story?
- If provider-specific tracing is ever revisited after v1, should it extend only under additive child spans or stay entirely outside the DVault-owned contract surface?

Risks
- If downstream implementation starts before the contract document lands, the existing blocked tickets can drift on tag omission rules or redaction boundaries even though the core span-name list is already bounded in this ticket.

Split recommendations
- No additional PO split is recommended from current evidence; the story is already bounded to one contract document and already has three downstream `blocks` relations for implementation follow-on.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment