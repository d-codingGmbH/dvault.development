[gicket-bot] PO refinement contract

Summary
- Verified the live local ticket, comment, attachment, and relation state from `.gicket` plus the repository save/read architecture, then bounded this story to additive explicit save/read telemetry summaries and low-cardinality counters over the existing save/read and strategy-diagnostics surfaces; no child tickets, relation edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live relations keep this story under epic `06F2PGQ27NWVZ1B1R651S7SM4M` and blocking downstream documentation ticket `06F2PGQQJB5FJGDB16M2G7CPCM`; inbound `blocks` from done `06F2PGQ6T5TGNWCBQBX3700D84` and done epic `06F2PGP7HM8F39K3J0H5JHB3B4` are satisfied historical prerequisites, not open blockers.
- Repository evidence already fixes the runtime baseline: save flows through `IDataVaultSaveService` single and bulk request APIs plus typed save extensions, while read flows through latest/current/as-of satellite, PIT, and bridge helpers anchored in `IDataVaultReadService` and its extensions.
- Request-bound strategy diagnostics are already finite and reusable through `DataVaultSaveStrategyDiagnosticsStatus`, `DataVaultReadStrategyDiagnosticsStatus`, and the corresponding fallback-cause enums; telemetry should reuse that vocabulary instead of inventing new free-form status labels.
- The repository currently exposes no telemetry API or telemetry package dependency, so v1 telemetry should stay additive and opt-in without changing `AddDVault()` defaults or requiring external packages.

Scope In
- One additive telemetry surface for explicit save operations that covers `IDataVaultSaveService.SaveAsync(...)` for single and bulk requests plus typed save extension entry points.
- One additive telemetry surface for explicit read operations that covers latest/current/as-of satellite reads, PIT reads, and bridge reads for explicit, registry-backed, and typed projection helper entry points.
- Bounded per-attempt telemetry summaries and counters for outcome, batch/request counts, requested-key counts, persisted/returned row counts, duration, strategy-selection status, selected strategy name when finite, and distinct fallback-cause kinds when provider-neutral fallback is used.
- Low-cardinality counters or histograms built on stable finite dimensions only, plus unit, integration, API-snapshot, and code-facing doc updates needed to lock the contract.
- Source-local README, XML, or API documentation sufficient to explain the public telemetry surface and let downstream documentation ticket `06F2PGQQJB5FJGDB16M2G7CPCM` finish the coordinated v0.16.0 operational write-up.

Scope Out
- Telemetry for `UseDataVaultSaveChangesMetadataInterceptor(...)`, `IDataVaultDiagnosticsService.Analyze(...)` calls themselves, design-time commands, migration guardrails, live-schema readers, or general EF Core activity outside explicit DVault save/read operations.
- Telemetry for `IDataVaultPitMaintenanceService` or `IDataVaultBridgeMaintenanceService`; this story is save/read only even though those services already exist and return row-count results.
- Provider dispatch behavior changes, threshold changes, new provider-specific read or save optimizations, or changed persistence/read semantics.
- High-cardinality tags or payloads such as hash keys, record sources, metadata names, table names, raw exception messages, or full diagnostics text in counters.
- Coordinated release-wrap work, support-bundle export, or broader operations runbooks beyond the minimal telemetry API/source docs needed here.

Open questions
- none

Follow-up questions
- Should a later observability follow-up add equivalent bounded telemetry for PIT and bridge maintenance services, which already expose row-count result types but are outside this save/read story?
- What subset of per-attempt telemetry summary data, if any, should the future support-bundle ticket serialize by default versus leave as runtime-only observability?
- After the v1 telemetry contract is stable, should the documentation ticket add troubleshooting examples for common strategy fallback causes and expected counter patterns across SQLite-optimized versus provider-neutral paths?

Risks
- A decorator-only implementation will miss documented read helper paths, leading to silent telemetry gaps unless instrumentation is anchored below the extension/helper bypass points.
- Re-deriving fallback causes separately from the existing strategy gate evaluators can cause telemetry drift from the already documented diagnostics contract.
- Counter and tag design can become operationally unsafe if implementation leaks unbounded values such as hash keys, record sources, metadata names, or exception text.
- The repository has no `docs/releases/v0.16.0.md` yet, so public-surface documentation must be explicit enough that the downstream documentation ticket can finish the coordinated release note without reopening telemetry scope.

Split recommendations
- No additional split is recommended. The story is bounded if it stays on explicit save/read telemetry only, reuses the existing strategy-diagnostics vocabulary, and leaves maintenance-service telemetry, support-bundle export, and coordinated v0.16.0 documentation wrap-up in their existing or follow-up tickets.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment