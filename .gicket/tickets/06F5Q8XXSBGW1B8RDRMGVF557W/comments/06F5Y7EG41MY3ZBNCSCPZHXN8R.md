[gicket-bot] PO refinement contract

Summary
- Restated the ticket as benchmark-only work with source-backed evidence for the existing chunked-save API and telemetry surfaces, confirmed the current benchmark artifact set still has no streaming or chunked rows, and made no child-ticket or relation changes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Replaced inferred existing-API and type wording with current-branch source evidence. IDataVaultSaveService already exposes SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...); DataVaultChunkedSaveRequest and DataVaultSaveChunk are implemented in source; and DataVaultSaveTelemetrySummary already carries chunked retained-state members. The contract is restated as benchmark and evidence work on those existing surfaces rather than API or type design.
- critic-item-2: `answered` - The contract no longer relies on inferred existence. Current-branch source plus the public API snapshot prove the public chunked-save surface is already exported, so this story stays focused on adding benchmark scenarios and artifacts for that existing API.
- critic-item-3: `answered` - The unsupported claim was narrowed to source-backed surfaces only. The existing public baseline is limited to the chunked save overload, DataVaultChunkedSaveRequest and DataVaultSaveChunk, and the DataVaultSaveTelemetrySummary retained-state members already present in source and exercised by tests. The checked-in benchmark summaries still contain no streaming or chunked rows, so the missing work is benchmark evidence, not public API creation.

Clarifications
- Current-branch source proves the v1 public chunked-save baseline already exists in src/DCoding.Data.DVault/DataVaultSaveService.cs through IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...) plus the DataVaultChunkedSaveRequest and DataVaultSaveChunk types.
- Current-branch source proves the existing bounded telemetry surface already includes chunked retained-state members in src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs: ChunkCount, ProcessedChunkCount, RetainedStateCurrentCount, RetainedStateHighWaterCount, ChunkedStateFallbackCauseKinds, and UnsupportedShapeKinds.
- Current tests exercise the existing public chunked-save and telemetry baseline in tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs, and the approved public API snapshot also records those surfaces.
- The checked-in benchmark artifact trio still contains no stream or chunked scenario rows, so this story remains benchmark and evidence work rather than API or telemetry design work.
- git diff --name-only e69f3d5cc885e75fc867f9ef9c633994056b31ea..HEAD over benchmark, docs, src, and tests returned no matching file changes, so no streaming benchmark evidence has been materialized on the current branch yet.
- No child tickets, relation changes, attachments, or planning documents were materialized in this refinement run.

Scope In
- Add benchmark scenarios or a documented focused benchmark mode that exercises the existing DataVaultChunkedSaveRequest path against equivalent ordered materialized explicit-save baselines built from the same logical input.
- Collect timing and allocation evidence for bounded chunk sizes on the required SQLite local temporary-file baseline using the existing benchmark artifact trio.
- Update benchmark documentation and any evidence-facing prose needed to explain the chunked scenario scope, executionDetail metadata, and artifact location for release-facing claims.

Scope Out
- No new public chunked-save API or chunk container type design; IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...), DataVaultChunkedSaveRequest, and DataVaultSaveChunk already exist in current-branch source.
- No new telemetry summary type or retained-state diagnostics contract design; DataVaultSaveTelemetrySummary already exposes the chunked retained-state and unsupported-shape members needed for evidence-facing metadata.
- No mandatory new external-provider streaming matrix; the existing optional-provider rows remain the visible completed or skipped boundary through the current benchmark contract.
- No new benchmark artifact file format or row-schema expansion for chunk metadata.
- No provider-specific chunk-optimization feature work beyond measuring the behavior selected by the current implementation.

Open questions
- none

Follow-up questions
- After this story lands, do we want a separate ticket for provider-specific chunk-optimization evidence once any provider advertises a stable optimized chunk path?
- Should a later release note call out chunked-save benchmark findings explicitly, or is a checked-in artifact bundle plus benchmark README sufficient for the first evidence drop?
- Do we want a dedicated benchmark CLI switch for streaming or chunk-size matrices, or is adding the scenarios to the existing SQLite matrix sufficient for v1?

Risks
- Benchmark timings and allocations are machine-sensitive; before and after comparisons must preserve the same run-context inputs already required by the artifact contract.
- Chunked-save results can vary by selected strategy or retained-state fallback behavior, so rows will be misleading if executionDetail does not expose the exercised path.
- The ticket currently has two incoming blocks relations, so delivery sequencing may still depend on upstream work even though PO scope is ready.

Split recommendations
- No split is recommended if the ticket stays bounded to SQLite evidence, existing artifact files, and documentation or evidence updates.
- If future work needs provider-specific chunk matrices or chunk-optimization feature changes, create a follow-up story instead of expanding this ticket.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment