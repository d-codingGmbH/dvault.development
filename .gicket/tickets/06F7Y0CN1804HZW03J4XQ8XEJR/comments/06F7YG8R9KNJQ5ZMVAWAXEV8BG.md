[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now uses current-branch source evidence for the existing baseline and limits new API creation to one additive SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken) overload. IDataVaultSaveService, DataVaultChunkedSaveRequest, and DataVaultSaveChunk are evidenced as already landed.
- critic-item-2: `answered` - The unsupported existing-API inference is removed. The public API snapshot shows exactly three current IDataVaultSaveService SaveAsync overloads for DataVaultSaveRequest, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest; no async-streaming overload is treated as pre-existing.
- critic-item-3: `answered` - IAsyncEnumerable<DataVaultSaveChunk> is now framed as the new overload to add, while ordering, cancellation, transaction ownership, retained-state fallback, telemetry, tracing, and redaction are reused only from the already-evidenced chunked boundary.

Clarifications
- An authoritative description update is already materialized at .gicket/tickets/06F7Y0CN1804HZW03J4XQ8XEJR/description.md and replaces the earlier unsupported wording.
- Current branch source shows IDataVaultSaveService already exposes only the DataVaultSaveRequest, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest overloads; the IAsyncEnumerable<DataVaultSaveChunk> overload is new work.
- Current branch source shows DataVaultChunkedSaveRequest and DataVaultSaveChunk already exist, so the async contract can reuse the existing chunk payload model.

Scope In
- Define one new IDataVaultSaveService SaveAsync overload that consumes caller-ordered IAsyncEnumerable<DataVaultSaveChunk>.
- Specify single-pass sequential async enumeration, chunk ordering, request ordering, failure propagation, and no-background-continuation behavior for that new overload.
- Preserve caller-owned DbContext, transaction participation, and cancellation semantics for async streaming saves.
- Carry forward retained satellite state, provider fallback, telemetry, Activity tracing, and redaction rules from the existing provider-neutral chunked-save boundary.

Scope Out
- Any file-ingestion, CDC, scheduler, background worker, or queue-driven save pipeline.
- Provider-native async chunk execution guarantees or provider-specific public APIs.
- Changing semantics of existing DataVaultSaveRequest, DataVaultBulkSaveRequest, or DataVaultChunkedSaveRequest overloads.
- Automatic retry, hidden checkpointing, resumable source consumption, or durable stream-state persistence.

Open questions
- none

Follow-up questions
- After implementation lands, should docs/performance-profiles.md add async-stream-specific chunk-size heuristics beyond the current chunked-save guidance?
- Should public docs add a short migration example explaining when to keep DataVaultBulkSaveRequest versus when to switch to IAsyncEnumerable<DataVaultSaveChunk>?

Risks
- Long-running or poorly behaved async sources can defer faults or ignore cancellation until enumeration advances, so implementation tests need explicit coverage to keep the public contract caller-visible and deterministic.
- Very large streams can hit the existing retained-state high-water limit and fall back to per-chunk persisted latest-state lookup, which preserves correctness but may change performance characteristics.
- Reusing the existing chunked-save telemetry family avoids API sprawl, but the docs must stay explicit that async streaming is a source-shape difference, not a new provider strategy or optimized ingestion claim.

Split recommendations
- No additional split is needed; the live blocks relation 06F7Y0CN1804HZW03J4XQ8XEJR -> 06F7Y0DCHTWCN3H25XQF18QE2G already routes code, API snapshot, and test work to the existing implementation story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment