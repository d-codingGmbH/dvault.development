<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the live local ticket, comment, attachment, and relation state from `.gicket` plus the repository save/read architecture, then bounded this story to additive explicit save/read telemetry summaries and low-cardinality counters over the existing save/read and strategy-diagnostics surfaces; no child tickets, relation edits, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Live relations keep this story under epic `06F2PGQ27NWVZ1B1R651S7SM4M` and blocking downstream documentation ticket `06F2PGQQJB5FJGDB16M2G7CPCM`; inbound `blocks` from done `06F2PGQ6T5TGNWCBQBX3700D84` and done epic `06F2PGP7HM8F39K3J0H5JHB3B4` are satisfied historical prerequisites, not open blockers.
- Repository evidence already fixes the runtime baseline: save flows through `IDataVaultSaveService` single and bulk request APIs plus typed save extensions, while read flows through latest/current/as-of satellite, PIT, and bridge helpers anchored in `IDataVaultReadService` and its extensions.
- Request-bound strategy diagnostics are already finite and reusable through `DataVaultSaveStrategyDiagnosticsStatus`, `DataVaultReadStrategyDiagnosticsStatus`, and the corresponding fallback-cause enums; telemetry should reuse that vocabulary instead of inventing new free-form status labels.
- The repository currently exposes no telemetry API or telemetry package dependency, so v1 telemetry should stay additive and opt-in without changing `AddDVault()` defaults or requiring external packages.

### Scope In
- One additive telemetry surface for explicit save operations that covers `IDataVaultSaveService.SaveAsync(...)` for single and bulk requests plus typed save extension entry points.
- One additive telemetry surface for explicit read operations that covers latest/current/as-of satellite reads, PIT reads, and bridge reads for explicit, registry-backed, and typed projection helper entry points.
- Bounded per-attempt telemetry summaries and counters for outcome, batch/request counts, requested-key counts, persisted/returned row counts, duration, strategy-selection status, selected strategy name when finite, and distinct fallback-cause kinds when provider-neutral fallback is used.
- Low-cardinality counters or histograms built on stable finite dimensions only, plus unit, integration, API-snapshot, and code-facing doc updates needed to lock the contract.
- Source-local README, XML, or API documentation sufficient to explain the public telemetry surface and let downstream documentation ticket `06F2PGQQJB5FJGDB16M2G7CPCM` finish the coordinated v0.16.0 operational write-up.

### Scope Out
- Telemetry for `UseDataVaultSaveChangesMetadataInterceptor(...)`, `IDataVaultDiagnosticsService.Analyze(...)` calls themselves, design-time commands, migration guardrails, live-schema readers, or general EF Core activity outside explicit DVault save/read operations.
- Telemetry for `IDataVaultPitMaintenanceService` or `IDataVaultBridgeMaintenanceService`; this story is save/read only even though those services already exist and return row-count results.
- Provider dispatch behavior changes, threshold changes, new provider-specific read or save optimizations, or changed persistence/read semantics.
- High-cardinality tags or payloads such as hash keys, record sources, metadata names, table names, raw exception messages, or full diagnostics text in counters.
- Coordinated release-wrap work, support-bundle export, or broader operations runbooks beyond the minimal telemetry API/source docs needed here.

## Acceptance Criteria
- The public telemetry contract is opt-in and additive: applications that keep the current `AddDVault()` default registration without opting into telemetry preserve existing save/read behavior and do not need to configure counters or listeners.
- Each explicit save attempt emits one bounded telemetry outcome that identifies whether the call was single-request or bulk, reports batch/request and operation counts, reports `RowsWritten` and saved-record counts, records elapsed duration, and classifies strategy selection using the existing save-strategy status and fallback-cause vocabulary.
- Each explicit read attempt emits one bounded telemetry outcome that identifies the read family (`latest/current/as-of satellite`, `PIT`, or `bridge`), reports requested-key counts and returned-row counts, records elapsed duration, and classifies strategy selection using the existing read-strategy status and fallback-cause vocabulary.
- Read telemetry covers helper paths that currently bypass a simple service decorator, including typed latest/as-of satellite projections and bridge helper extensions, so emitted telemetry is complete for the documented public read surface rather than only for direct interface calls.
- Failure outcomes are observable without high-cardinality leakage: failed save/read attempts still emit one bounded failure outcome with duration and finite result classification, while raw exception text stays out of counters and tags.
- Automated coverage proves selected-strategy and provider-neutral fallback cases for save and read flows, proves exactly-once telemetry emission for representative helper paths, and updates the public API snapshot plus code-facing docs for any new public telemetry types or registration APIs.

## Definition of Done
- Public API, XML docs, and the approved API snapshot capture the new telemetry hook/counter surface and keep it additive beside the existing explicit save/read services.
- Unit and integration coverage verify save single/bulk telemetry, latest/as-of satellite telemetry, PIT telemetry, bridge telemetry, success and failure outcomes, and representative strategy/fallback classifications.
- The implementation reuses or mirrors the established request-bound strategy vocabulary closely enough that downstream observability work can rely on stable status and cause names without reopening contract questions.
- README or equivalent code-facing docs explain how to enable the telemetry surface and what bounded data it emits; broader v0.16.0 operational packaging can remain with ticket `06F2PGQQJB5FJGDB16M2G7CPCM`.
- Any intentionally deferred observability work remains explicitly out of scope and is not hidden behind vague telemetry claims.

## Implementation Notes
- Prefer a library-owned telemetry hook inside the save/read implementation path over a pure `IDataVaultSaveService` or `IDataVaultReadService` decorator; current read helpers can bypass a decorator because `DataVaultReadServiceTypedProjectionExtensions` fall back to pipeline reads when the service is not `IDataVaultSatelliteProjectionReadService`, and `DataVaultReadServiceBridgeExtensions` bypass non-`DefaultDataVaultReadService` instances.
- Reuse the existing request-bound strategy explanation baseline from `IDataVaultDiagnosticsService`, `IDataVaultReadDiagnosticsService`, and the `DataVault*StrategyDiagnostics*` / `DataVault*StrategyFallbackCauseKind` types as the authoritative finite vocabulary for telemetry status and fallback classification.
- Use the visible request/result shapes as the v1 counting baseline: `DataVaultBulkSaveRequest.Requests.Count`, per-request hub/link/satellite operation counts, `DataVaultSaveResult.RowsWritten`, returned read-row counts, and requested parent/endpoint hash-key counts.
- Keep metric and tag cardinality bounded and deterministic; finite enums, provider names, request family, and success/failure are acceptable, while hash keys, metadata names, record sources, produced table names, and raw diagnostic or exception text are not.
- Keep telemetry observational only. It must not alter provider strategy selection, fallback behavior, batching, PIT/bridge maintenance, SaveChanges interception, or caller-visible save/read results.

## Open Questions
- none

## Follow-Up Questions
- Should a later observability follow-up add equivalent bounded telemetry for PIT and bridge maintenance services, which already expose row-count result types but are outside this save/read story?
- What subset of per-attempt telemetry summary data, if any, should the future support-bundle ticket serialize by default versus leave as runtime-only observability?
- After the v1 telemetry contract is stable, should the documentation ticket add troubleshooting examples for common strategy fallback causes and expected counter patterns across SQLite-optimized versus provider-neutral paths?

## Risks
- A decorator-only implementation will miss documented read helper paths, leading to silent telemetry gaps unless instrumentation is anchored below the extension/helper bypass points.
- Re-deriving fallback causes separately from the existing strategy gate evaluators can cause telemetry drift from the already documented diagnostics contract.
- Counter and tag design can become operationally unsafe if implementation leaks unbounded values such as hash keys, record sources, metadata names, or exception text.
- The repository has no `docs/releases/v0.16.0.md` yet, so public-surface documentation must be explicit enough that the downstream documentation ticket can finish the coordinated release note without reopening telemetry scope.

## Split Recommendations
- No additional split is recommended. The story is bounded if it stays on explicit save/read telemetry only, reuses the existing strategy-diagnostics vocabulary, and leaves maintenance-service telemetry, support-bundle export, and coordinated v0.16.0 documentation wrap-up in their existing or follow-up tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Expose metrics around row counts, batches, durations, fallback paths, and diagnostics.

## Scope
- Refine and complete the work for "Add save/read telemetry hooks and counters" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.