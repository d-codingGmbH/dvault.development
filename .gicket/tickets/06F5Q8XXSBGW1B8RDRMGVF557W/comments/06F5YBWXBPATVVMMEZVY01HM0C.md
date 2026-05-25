[gicket-bot] PO refinement contract

Summary
- Restated the story as benchmark-evidence work on the already-visible chunked-save boundary, answered all three PO-critic blockers with current-branch source evidence, and applied no child-ticket, relation, attachment, or planning-document writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract should stop inferring a missing API or type. Current branch source already defines `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken)`, `DataVaultChunkedSaveRequest`, and `DataVaultSaveChunk`, so this ticket can be refined as benchmark evidence on an existing boundary.
- critic-item-2: `answered` - The delivery contract should treat chunked save as already present in the current branch instead of inferring future public API work. The approved public API snapshot visibly exports `DataVaultChunkedSaveRequest`, so the story stays scoped to benchmark scenarios, evidence artifacts, and documentation.
- critic-item-3: `answered` - The benchmark anchor is the current chunked-save implementation surface, not an inferred new API. Use `DataVaultSaveService.cs` for the runtime and source baseline, `DataVaultSaveTelemetrySummary.cs` for existing chunked retained-state diagnostics, and the existing chunked integration tests as the semantic benchmark baseline; the public snapshot already shows the chunked request type is exported.

Clarifications
- This refinement restates the delivery contract to rely only on current-branch evidence; it does not require creation of a new public chunked-save API or telemetry type.
- Current branch source already exposes the chunked save boundary through `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken)`, `DataVaultChunkedSaveRequest`, and `DataVaultSaveChunk` in `src/DCoding.Data.DVault/DataVaultSaveService.cs`.
- Current branch source already exposes chunked retained-state telemetry through `DataVaultSaveTelemetrySummary` in `src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs`.
- Current branch integration tests already prove chunk ordering, empty-chunk no-op behavior, cancellation before later chunks, caller-transaction participation, repeated-row reuse, satellite continuity, and public chunked telemetry semantics in `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`.
- No child tickets, relation changes, attachments, or planning documents were created or queued in this refinement turn.

Scope In
- Add benchmark scenarios or a focused benchmark mode that exercises the existing chunked-save request path against equivalent ordered materialized explicit-save baselines built from the same logical input.
- Capture timing and allocation evidence for bounded chunk sizes on the required SQLite local temporary-file baseline using the existing benchmark artifact trio.
- Update benchmark documentation and evidence-facing prose so chunk boundary, exercised save path, and artifact location are clear without inventing a new public API surface.

Scope Out
- No new public chunked-save API or chunk container design; the current branch already contains the chunked save overload and request and chunk types.
- No new telemetry summary type or new artifact columns for chunk-state diagnostics; reuse the existing `DataVaultSaveTelemetrySummary` chunked members and `executionDetail`.
- No mandatory external-provider chunk matrix and no provider-specific chunk-optimization feature work beyond measuring whichever current implementation path runs.
- No new benchmark artifact schema; stay within `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`.

Open questions
- none

Follow-up questions
- After this story lands, do we want a separate ticket for provider-specific chunk-optimization evidence once any provider advertises a stable optimized chunk path?
- Should a later release note call out chunked-save benchmark findings explicitly, or is a checked-in artifact bundle plus benchmark README sufficient for the first evidence drop?
- Do we want a dedicated benchmark CLI switch for chunk-size or chunk-matrix modes, or is extending the existing SQLite matrix sufficient for v1?

Risks
- Benchmark timings and allocations are machine-sensitive, so before and after evidence must preserve the same run-context inputs already required by `docs/plans/performance-evidence-benchmark-artifact-contract.md`.
- Chunked-save results can vary by selected strategy or retained-state fallback behavior, so evidence becomes misleading if `executionDetail` does not expose the exercised path.
- The live relation state still includes incoming `blocks` relations from `06F5Q8X8Q72TQ5B7F2JSAJWPR8` and `06F5Q8XF9DPKFW9VY0F3Y32BH4`, so delivery sequencing may still depend on upstream work.

Split recommendations
- No split is recommended if the story stays bounded to SQLite benchmark evidence, existing artifact files, and documentation or evidence updates on the current chunked-save boundary.
- If future work needs provider-specific chunk matrices, new public API design, or chunk-optimization feature changes, create a follow-up story instead of expanding this ticket.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment