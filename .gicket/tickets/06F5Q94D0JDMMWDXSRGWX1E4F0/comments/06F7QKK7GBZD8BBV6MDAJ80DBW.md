[gicket-bot] PO refinement contract

Summary
- Refinement confirms this story stays bounded to listener-driven Activity spans for the four explicit PIT/bridge maintenance entry points; no ticket description, relation, child-ticket, attachment, or planning-document write was needed because the current description, contract doc, and live relations already align.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository-tracked tracing contract fixes the ActivitySource name to `DCoding.Data.DVault`, requires `ActivityKind.Internal`, and keeps `AddDVault()` on the existing no-telemetry default when no listener is interested.
- The live relation set already matches the intended dependency graph: ticket `06F5Q93YXHSKABD2SABWY85S78` blocks this tracing story, this story blocks `06F5Q94SQ086B2DZ1AKFDXGV94`, and the parent link from `06F5Q93R4633D41Z21WQW3SVGR` remains valid.
- No bounded planning writes were materialized in this refinement pass: no child tickets, no relation cleanup, no description rewrite, no attachment, and no planning document.

Scope In
- Add listener-driven Activity spans for `IDataVaultPitMaintenanceService.RebuildAsync(...)`, `MaintainParentsAsync(...)`, `IDataVaultBridgeMaintenanceService.RebuildBridgeAsync(...)`, and `MaintainBridgeAsync(...)` using the exact maintenance span names already defined by the tracing contract.
- Populate only contract-approved maintenance/common tags and event names, including the closed vocabularies for `dvault.maintenance.kind`, `dvault.read_model.kind`, `dvault.outcome`, `dvault.failure.*`, `dvault.duration.bucket`, and bounded row/count tags already derivable from existing request/result data.
- Add focused PIT/bridge maintenance tracing coverage on top of the existing maintenance and read-model integration suites, including no-listener, listener-enabled, success, fault, cancellation, redaction, and explicit no-op scenarios.

Scope Out
- Save/read span work remains with ticket `06F5Q9463M0RSHAJJX0F3D1DB0`.
- No scheduler, hosted worker, retry policy, health check, exporter, dashboard, collector, or alerting work.
- No provider-specific SQL/query-plan capture, no table or metadata name leakage, no raw parent-hash-key leakage, no exception-message/stack-trace capture, and no maintenance semantic changes beyond bounded tracing data.

Open questions
- none

Follow-up questions
- none

Risks
- This story remains dependency-bound by the live incoming `blocks` relation from `06F5Q93YXHSKABD2SABWY85S78`; if the shared tracing contract implementation changes tag/event helper mechanics late, maintenance instrumentation may need a small alignment pass.
- No-op coverage must stay anchored to explicit existing request/result evidence; over-eager emission on rebuild paths would violate the redaction and bounded-semantics contract.

Split recommendations
- No split recommended; the story is already bounded to four explicit maintenance entry points, fixed contract vocabularies, and existing PIT/bridge maintenance test lanes.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment