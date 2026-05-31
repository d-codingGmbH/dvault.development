[gicket-bot] PO refinement contract

Summary
- Refined the ticket to an additive `IDataVaultSaveService` async streaming overload over `IAsyncEnumerable<DataVaultSaveChunk>`, reusing the existing chunked-save ordering, cancellation, retained-state, telemetry, tracing, redaction, and provider-neutral boundaries; no additional split or persistent planning write was needed because downstream implementation story `06F7Y0DCHTWCN3H25XQF18QE2G` already exists.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already establishes provider-neutral chunked save as the current baseline, so v0.24 only needs an additive async source shape rather than a new persistence mode.
- Ratify one additive `IDataVaultSaveService` overload that accepts `IAsyncEnumerable<DataVaultSaveChunk>`; keep existing `DataVaultSaveRequest`, `DataVaultBulkSaveRequest`, and `DataVaultChunkedSaveRequest` overloads unchanged and authoritative for existing callers.
- Treat async streaming as the same explicit save family as existing chunked saves: each yielded `DataVaultSaveChunk` still contains an ordered finite collection of ordinary `DataVaultSaveRequest` values with the same metadata, resolver, hash-key, hash-diff, hub, link, satellite, and validation rules.
- Reuse the existing chunked-save telemetry and Activity family, including `dvault.save.chunked_request`, instead of defining a provider-native or ingestion-specific public save mode.
- No bounded ticket/planning write was materialized in this pass; the refinement relies on the checked-in streaming contract docs and the already-created implementation story `06F7Y0DCHTWCN3H25XQF18QE2G`.

Scope In
- Define the public async entry point on `IDataVaultSaveService` for caller-ordered `IAsyncEnumerable<DataVaultSaveChunk>` sources.
- Specify single-pass sequential enumeration, chunk/request ordering, failure propagation, and no-background-continuation behavior.
- Preserve caller-owned cancellation and transaction participation semantics for async streaming saves.
- Carry forward retained satellite state, provider fallback, telemetry, Activity tracing, and redaction rules from the existing provider-neutral chunked-save boundary.

Scope Out
- Any new file-ingestion, CDC, scheduler, background worker, or queue-driven save pipeline.
- Provider-native async chunk execution guarantees or provider-specific public APIs.
- Changing semantics of existing `DataVaultSaveRequest`, `DataVaultBulkSaveRequest`, or `DataVaultChunkedSaveRequest` overloads.
- Automatic retry, hidden checkpointing, resumable source consumption, or durable stream-state persistence.

Open questions
- none

Follow-up questions
- After implementation lands, should adopter guidance in `docs/performance-profiles.md` add async-stream-specific chunk-size heuristics beyond the current chunked-save guidance?
- Should the public docs add a short migration example showing when to keep `DataVaultBulkSaveRequest` versus when to switch to `IAsyncEnumerable<DataVaultSaveChunk>`?

Risks
- Long-running or poorly behaved async sources can defer faults or ignore cancellation until enumeration advances, so implementation tests need explicit coverage to keep the public contract caller-visible and deterministic.
- Very large streams can hit the existing retained-state high-water limit and fall back to per-chunk persisted latest-state lookup, which preserves correctness but may change performance characteristics.
- Reusing the existing chunked-save telemetry family avoids API sprawl, but the docs must be explicit that async streaming is a source-shape difference, not a new provider strategy or optimized ingestion claim.

Split recommendations
- No additional split is needed; use the existing downstream implementation story `06F7Y0DCHTWCN3H25XQF18QE2G` for the code, API snapshot, and test work after this ticket clears PO critic.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment