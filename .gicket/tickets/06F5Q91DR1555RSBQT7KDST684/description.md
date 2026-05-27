<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Rewrote the story around the actual bridge baseline: PIT work stays evidence-focused, and bridge work is limited to diagnostics and benchmark evidence over append-only MaintainBridgeAsync(...) plus RebuildBridgeAsync(...) for shrink; no child tickets, relation changes, description updates, attachments, or planning documents were applied in this refinement pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This story now assumes the actual bridge baseline: bridge maintenance remains explicit and non-delete-aware, with append-only MaintainBridgeAsync(...) and RebuildBridgeAsync(...) as the current shrink-safe recomputation path.
- Registry-backed PIT coverage stays bounded to PIT maintenance-name resolution plus explicit DataVaultPitAsOfReadRequest diagnostics after metadata resolution because the visible API still has no DataVaultRegistryPitAsOfReadRequest equivalent.
- The verified repo baseline already has read-shape diagnostics through IDataVaultReadDiagnosticsService and PIT/bridge maintenance result summaries, so this story extends those evidence surfaces instead of inventing a separate maintenance-diagnostics API.
- Current relation context remains unchanged: the ticket still blocks 06F5Q91M0PM17RP43ZQRPBDXP0, remains a child of 06F5Q90CSKMGK3NZZ25XTW6W4C, and retains incoming historical blocks links from done upstream stories including 06F5Q916BXE2N372SWMH1X776G.
- No child tickets, relation changes, description updates, attachments, or planning documents were applied in this refinement pass.

### Scope In
- Add diagnostics coverage that proves registry-backed PIT maintenance-name resolution, explicit PIT read diagnostics, and current bridge read diagnostics stay consistent with the implemented feature contracts.
- Add link-parent PIT evidence that preserves ParentHashKey as the link hash key, shows ordered snapshot-column behavior, and records provider-neutral fallback when provider-specific PIT optimization declines the request shape.
- Add multi-active PIT evidence that preserves tuple-aware row identity, driving-key projection, and expected index and read-shape semantics for the bounded shared-driving-key baseline.
- Add bridge evidence only for the current explicit non-delete-aware maintenance contract: append-only MaintainBridgeAsync(...) result visibility, compatible post-maintenance many-to-many and hierarchy reads, and RebuildBridgeAsync(...) as the shrink-safe path when row removal or increased TraversalDepth must be handled.
- Add benchmark rows or checked-in artifact bundles only where release or documentation claims depend on measured behavior, using diagnostics-backed execution detail and the existing benchmark artifact contract.

### Scope Out
- Adding a new delete-aware bridge maintenance behavior, API, or incremental shrink-safe reconciliation path in this story.
- Re-implementing core link-parent PIT, multi-active PIT, PIT maintenance, or bridge maintenance functionality already present in the repository baseline.
- Adding a new public maintenance-diagnostics service or a new public registry-backed PIT as-of read request.
- Provider-specific link-parent or multi-active PIT read optimization, provider-specific bridge maintenance optimization, raw SQL or physical-plan promises, or automatic index creation.
- Automatic PIT or bridge maintenance orchestration, new model-first PIT artifact shapes, or benchmark-schema changes outside docs/plans/performance-evidence-benchmark-artifact-contract.md.
- Broader README, architecture, release-note, or production-checklist completeness work already tracked by 06F5Q91M0PM17RP43ZQRPBDXP0 except for narrowly necessary wording that must ship with the evidence itself.

## Acceptance Criteria
- Diagnostics tests and integration coverage show explicit PIT read diagnostics and metadata-resolved PIT maintenance-name resolution paths stay equivalent for the implemented PIT surfaces, and fallback or gate details remain visible when provider-specific strategies decline a request shape.
- Link-parent PIT evidence shows ParentHashKey-based row identity, declared snapshot-column order, provider-neutral fallback status, and no claim of provider-specific PIT optimization or a new registry PIT read API.
- Multi-active PIT evidence shows tuple-aware row identity, PIT driving-key projection, deterministic row-selection and snapshot-lookup behavior, and expected index baselines for the shared-driving-key shape.
- Bridge evidence proves the current explicit maintenance contract only: append-only MaintainBridgeAsync(...) keeps insert, update, and unchanged outcomes visible, post-maintenance many-to-many and hierarchy bridge reads remain correct, and any shrink or removal scenario cited by this ticket uses RebuildBridgeAsync(...) rather than implying incremental delete-aware maintenance.
- Any new measured claim introduced by this ticket is backed by new root benchmark rows or a checked-in artifact bundle that conforms to docs/plans/performance-evidence-benchmark-artifact-contract.md, and unsupported PIT shapes remain visible as provider-neutral fallback evidence rather than implied optimization.
- Benchmark-facing or diagnostics-facing wording added by this ticket does not promise delete-aware bridge maintenance, raw SQL, provider physical plans, automatic index creation, automatic PIT or bridge maintenance, or non-existent public request types.

## Definition of Done
- Required unit, integration, and approval-snapshot coverage lands without widening the public API beyond the existing read-diagnostics, PIT read, bridge read, and maintenance-result surfaces.
- Benchmark harness changes and any checked-in artifacts are reproducible under the existing root triplet or artifacts/benchmarks/<label>/before and after contract and preserve strategy and fallback execution detail.
- Any narrowly necessary benchmark-surface or diagnostics-surface documentation is updated so shipped evidence matches the actual non-delete-aware bridge baseline, while the broader completeness rollup remains delegated to 06F5Q91M0PM17RP43ZQRPBDXP0.
- The final contract leaves no PO ambiguity about registry-backed PIT meaning, link-parent and multi-active PIT optimization boundaries, or the bridge baseline of append-only MaintainBridgeAsync(...) plus RebuildBridgeAsync(...) for shrink.

## Implementation Notes
- Use IDataVaultReadDiagnosticsService read-shape outputs, support-bundle serialization, and preflight representative-diagnostics flow as the primary diagnostics contract; do not add a parallel maintenance-diagnostics API in this story.
- Treat registry-backed PIT coverage as DataVaultPitMaintenanceServiceRegistryExtensions resolution plus explicit DataVaultPitAsOfReadRequest diagnostics after metadata resolution because no DataVaultRegistryPitAsOfReadRequest surface is visible in the repository baseline.
- Current verified gaps are evidence-oriented: ordinary PIT, registry-backed latest and bridge reads, and multi-active PIT read shape already have baseline coverage, while link-parent PIT read-shape evidence and bridge evidence over the current non-delete-aware maintenance contract still need explicit coverage.
- Likely touch points remain tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs, benchmarks/DCoding.Data.DVault.Benchmarks/ReadModelBenchmarks.cs, benchmarks/DCoding.Data.DVault.Benchmarks/README.md, and the root benchmark-summary.* files or a checked-in artifacts/benchmarks/<ticket-label> bundle.
- For bridge work, do not add a new delete-aware path in this story; if shrink or row-removal behavior needs end-to-end proof, use RebuildBridgeAsync(...) as the current bounded contract and verify the resulting bridge reads.
- No persistent ticket, relation, attachment, or planning-document writes were applied during this refinement run.

## Open Questions
- none

## Follow-Up Questions
- Should a separate additive ticket introduce a public DataVaultRegistryPitAsOfReadRequest so PIT read diagnostics can mirror the registry-backed latest-satellite and bridge read request surfaces?
- If stakeholders later want diagnostics or benchmark evidence for a delete-aware bridge maintenance path, should that be handled only after a real delete-aware bridge capability lands in a separate additive ticket?
- Should provider-specific PIT strategies later accept link-parent or multi-active PIT shapes, or should those scenarios remain documented as provider-neutral fallback baselines?

## Risks
- If implementation or docs reintroduce delete-aware bridge wording, this ticket could again overclaim behavior the repository does not currently implement.
- The current root benchmark matrix only covers ordinary PIT and bridge read scenarios; new rows or bundles must make fallback and strategy status explicit or the evidence could be misread as provider-specific optimization support.
- Bridge shrink scenarios can cause scope creep toward a new delete-aware maintenance capability unless development stays bounded to current RebuildBridgeAsync(...) evidence.
- Broader README and release-note completeness work is intentionally downstream, so weak handoff between this evidence ticket and 06F5Q91M0PM17RP43ZQRPBDXP0 could leave published guidance behind the verified behavior.

## Split Recommendations
- No additional split is recommended if this story stays evidence-only over the existing PIT and non-delete-aware bridge contracts.
- If stakeholders want a new public delete-aware bridge maintenance path or incremental shrink-safe reconciliation behavior, create a separate additive capability ticket instead of broadening this evidence story.
- If broader README, architecture, or release-note completeness work grows beyond narrowly necessary benchmark-surface wording, keep it on 06F5Q91M0PM17RP43ZQRPBDXP0.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Extend diagnostics and benchmarks for completed PIT and bridge shapes.

Acceptance criteria:
- Adds read/maintenance diagnostics for registry-backed PIT, link-parent PIT, multi-active PIT, and delete-aware bridge operations.
- Adds benchmark rows or evidence bundles where release claims depend on measured behavior.
- Avoids provider-specific physical-plan promises or automatic index creation.