<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Restated the story as benchmark-evidence work on the already-visible chunked-save boundary, answered all three PO-critic blockers with current-branch source evidence, and applied no child-ticket, relation, attachment, or planning-document writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This refinement restates the delivery contract to rely only on current-branch evidence; it does not require creation of a new public chunked-save API or telemetry type.
- Current branch source already exposes the chunked save boundary through `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken)`, `DataVaultChunkedSaveRequest`, and `DataVaultSaveChunk` in `src/DCoding.Data.DVault/DataVaultSaveService.cs`.
- Current branch source already exposes chunked retained-state telemetry through `DataVaultSaveTelemetrySummary` in `src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs`.
- Current branch integration tests already prove chunk ordering, empty-chunk no-op behavior, cancellation before later chunks, caller-transaction participation, repeated-row reuse, satellite continuity, and public chunked telemetry semantics in `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`.
- No child tickets, relation changes, attachments, or planning documents were created or queued in this refinement turn.

### Scope In
- Add benchmark scenarios or a focused benchmark mode that exercises the existing chunked-save request path against equivalent ordered materialized explicit-save baselines built from the same logical input.
- Capture timing and allocation evidence for bounded chunk sizes on the required SQLite local temporary-file baseline using the existing benchmark artifact trio.
- Update benchmark documentation and evidence-facing prose so chunk boundary, exercised save path, and artifact location are clear without inventing a new public API surface.

### Scope Out
- No new public chunked-save API or chunk container design; the current branch already contains the chunked save overload and request and chunk types.
- No new telemetry summary type or new artifact columns for chunk-state diagnostics; reuse the existing `DataVaultSaveTelemetrySummary` chunked members and `executionDetail`.
- No mandatory external-provider chunk matrix and no provider-specific chunk-optimization feature work beyond measuring whichever current implementation path runs.
- No new benchmark artifact schema; stay within `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`.

## Acceptance Criteria
- Benchmark execution emits SQLite required-baseline rows that compare the existing chunked-save path with an equivalent ordered materialized explicit-save baseline using the same logical requests and comparable run inputs.
- Completed chunked rows record timing and allocation metrics, and their metadata or `executionDetail` makes the chunk boundary and exercised save path visible without changing the artifact schema.
- The artifact trio preserves the existing optional-provider rows as completed or skipped and does not regress the skipped-row semantics defined in `docs/plans/performance-evidence-benchmark-artifact-contract.md`.
- Any release-facing chunked-save performance claim is backed by a checked-in before and after artifact bundle under one explicit benchmark label using the existing artifact contract.

## Definition of Done
- Benchmark code and tests cover the new chunked-save evidence path and keep artifact output compatible with `docs/plans/performance-evidence-benchmark-artifact-contract.md`.
- Benchmark documentation explains how to run and interpret the chunked-save comparison without implying new provider-specific chunk optimizations or a new artifact schema.
- Checked-in evidence artifacts or documented artifact paths exist for any release-facing chunked-save performance claim, with comparable run context preserved.
- The refined contract remains consistent with live relation state and the current source-backed chunked API and telemetry baseline.

## Implementation Notes
- Anchor the benchmark comparison to `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken)`, `DataVaultChunkedSaveRequest`, and `DataVaultSaveChunk` in `src/DCoding.Data.DVault/DataVaultSaveService.cs`.
- Reuse the existing chunked telemetry members on `DataVaultSaveTelemetrySummary` from `src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs` when composing `executionDetail` or evidence-facing prose.
- Use the chunked integration tests in `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` as the semantic baseline for ordering, no-op empty chunks, cancellation, caller transaction ownership, repeated-row reuse, satellite continuity, retained-state high-water reporting, fallback classification, and unsupported-shape classification.
- Keep the comparison anchored to equivalent ordered materialized DVault requests rather than reopening public API design or classic-EF-versus-DVault boundary questions.
- No child-ticket, relation, attachment, or planning-document writes were applied in this refinement run; the material change is the contract restatement to source-backed evidence only.

## Open Questions
- none

## Follow-Up Questions
- After this story lands, do we want a separate ticket for provider-specific chunk-optimization evidence once any provider advertises a stable optimized chunk path?
- Should a later release note call out chunked-save benchmark findings explicitly, or is a checked-in artifact bundle plus benchmark README sufficient for the first evidence drop?
- Do we want a dedicated benchmark CLI switch for chunk-size or chunk-matrix modes, or is extending the existing SQLite matrix sufficient for v1?

## Risks
- Benchmark timings and allocations are machine-sensitive, so before and after evidence must preserve the same run-context inputs already required by `docs/plans/performance-evidence-benchmark-artifact-contract.md`.
- Chunked-save results can vary by selected strategy or retained-state fallback behavior, so evidence becomes misleading if `executionDetail` does not expose the exercised path.
- The live relation state still includes incoming `blocks` relations from `06F5Q8X8Q72TQ5B7F2JSAJWPR8` and `06F5Q8XF9DPKFW9VY0F3Y32BH4`, so delivery sequencing may still depend on upstream work.

## Split Recommendations
- No split is recommended if the story stays bounded to SQLite benchmark evidence, existing artifact files, and documentation or evidence updates on the current chunked-save boundary.
- If future work needs provider-specific chunk matrices, new public API design, or chunk-optimization feature changes, create a follow-up story instead of expanding this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Add benchmark evidence for streaming/chunked saves against materialized request baselines.

Acceptance criteria:
- Records timing and allocation behavior for bounded chunk sizes.
- Keeps optional provider rows visible as completed or skipped.
- Stores before/after evidence where release claims depend on measured behavior.