<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- An authoritative description update is already materialized at .gicket/tickets/06F7Y0CN1804HZW03J4XQ8XEJR/description.md and replaces the earlier unsupported wording.
- Current branch source shows IDataVaultSaveService already exposes only the DataVaultSaveRequest, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest overloads; the IAsyncEnumerable<DataVaultSaveChunk> overload is new work.
- Current branch source shows DataVaultChunkedSaveRequest and DataVaultSaveChunk already exist, so the async contract can reuse the existing chunk payload model.

### Scope In
- Define one new IDataVaultSaveService SaveAsync overload that consumes caller-ordered IAsyncEnumerable<DataVaultSaveChunk>.
- Specify single-pass sequential async enumeration, chunk ordering, request ordering, failure propagation, and no-background-continuation behavior for that new overload.
- Preserve caller-owned DbContext, transaction participation, and cancellation semantics for async streaming saves.
- Carry forward retained satellite state, provider fallback, telemetry, Activity tracing, and redaction rules from the existing provider-neutral chunked-save boundary.

### Scope Out
- Any file-ingestion, CDC, scheduler, background worker, or queue-driven save pipeline.
- Provider-native async chunk execution guarantees or provider-specific public APIs.
- Changing semantics of existing DataVaultSaveRequest, DataVaultBulkSaveRequest, or DataVaultChunkedSaveRequest overloads.
- Automatic retry, hidden checkpointing, resumable source consumption, or durable stream-state persistence.

## Acceptance Criteria
- The contract defines one additive IDataVaultSaveService async overload that consumes IAsyncEnumerable<DataVaultSaveChunk> and leaves the existing single-request, ordered-bulk, and DataVaultChunkedSaveRequest save contracts unchanged.
- DVault enumerates the async source once, processes yielded chunks in source order, processes requests within each chunk in caller order, and preserves existing hub-then-link-then-satellite ordering inside each request.
- The caller retains ownership of DbContext, current or ambient transaction, and cancellation token; DVault does not create, commit, roll back, or suppress transactions.
- Cancellation is observed before continuing to later chunks, async enumeration or processing failures stop later chunks, and the returned task does not hide background continuation after completion, fault, or cancellation.
- Retained satellite state, provider fallback, telemetry, Activity tracing, and redaction follow the existing provider-neutral chunked-save boundary, including the same finite fallback diagnostics when retained state is cleared.

## Definition of Done
- The PO handoff clearly distinguishes the new async streaming overload from the existing materialized DataVaultChunkedSaveRequest path and is sufficient for implementation without reopening API-shape or boundary questions.
- Implementation story 06F7Y0DCHTWCN3H25XQF18QE2G can proceed by adding the new overload while reusing the ratified chunked telemetry family and transaction/cancellation rules.
- Planned implementation and test work stays bounded to no-op streams, ordered multi-chunk saves, cancellation during async enumeration, transaction participation, retained-state fallback, and tracing/telemetry continuity rather than broader ingestion or provider-native features.

## Implementation Notes
- src/DCoding.Data.DVault/DataVaultSaveService.cs is the baseline evidence: the interface currently has exactly three public save overloads, and the async streaming overload is to be added beside them rather than treated as existing.
- Reuse DataVaultChunkedSaveRequest, DataVaultSaveChunk, and DataVaultSaveRequest directly; a second public chunk wrapper is unnecessary for v0.24.
- Preserve the existing chunked boundary documented in docs/architecture/dvault-v1-streaming-explicit-save-contract.md for ordering, metadata, cancellation, transaction ownership, retained-state fallback, and failure propagation.
- Reuse the existing chunked tracing and telemetry family, including dvault.save.chunked_request, chunk counts, processed-chunk counts, fallback causes, and unsupported-shape reporting; do not introduce a provider-native save mode.
- Persistent planning work in this pass is limited to the already-applied description update; no new child ticket, relation mutation, attachment, or planning document was required.

## Open Questions
- none

## Follow-Up Questions
- After implementation lands, should docs/performance-profiles.md add async-stream-specific chunk-size heuristics beyond the current chunked-save guidance?
- Should public docs add a short migration example explaining when to keep DataVaultBulkSaveRequest versus when to switch to IAsyncEnumerable<DataVaultSaveChunk>?

## Risks
- Long-running or poorly behaved async sources can defer faults or ignore cancellation until enumeration advances, so implementation tests need explicit coverage to keep the public contract caller-visible and deterministic.
- Very large streams can hit the existing retained-state high-water limit and fall back to per-chunk persisted latest-state lookup, which preserves correctness but may change performance characteristics.
- Reusing the existing chunked-save telemetry family avoids API sprawl, but the docs must stay explicit that async streaming is a source-shape difference, not a new provider strategy or optimized ingestion claim.

## Split Recommendations
- No additional split is needed; the live blocks relation 06F7Y0CN1804HZW03J4XQ8XEJR -> 06F7Y0DCHTWCN3H25XQF18QE2G already routes code, API snapshot, and test work to the existing implementation story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Define the v0.24 async streaming save contract before implementation.

# Scope In
- Decide the narrow public entry point for async chunk sources, preserving existing DataVaultSaveRequest, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest semantics.
- Define ordering, cancellation, transaction participation, retained satellite state, telemetry, Activity tracing, provider fallback, and redaction behavior.
- Explicitly keep file ingestion, CDC, schedulers, and provider-native chunk execution claims out of scope.

# Acceptance Criteria
- Contract material exists and explains how async streaming differs from the already implemented provider-neutral chunked save request.
- Downstream implementation can proceed without inventing API shape, telemetry, or boundary rules.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added and repaired repository contract documentation for the v0.24 async `IAsyncEnumerable<DataVaultSaveChunk>` save source boundary.
- Kept production source unchanged; implementation story `06F7Y0DCHTWCN3H25XQF18QE2G` remains responsible for adding the executable overload and tests.

### Repository Updates
- `docs/architecture/dvault-v1-streaming-explicit-save-contract.md` now distinguishes the landed materialized `DataVaultChunkedSaveRequest` path from the new async source overload contract.
- Repair note: the established v0.21 marker sentence expected by `StreamingExplicitSaveContractSnapshotTests` is preserved verbatim, with the async-source clarification moved into a separate sentence.
- `docs/performance-profiles.md` distinguishes current chunked benchmark guidance from the async streaming source shape and avoids new performance or provider-native claims.

### Verification
- `git diff --check -- docs/architecture/dvault-v1-streaming-explicit-save-contract.md docs/performance-profiles.md` passed.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --filter FullyQualifiedName~StreamingExplicitSaveContractSnapshotTests --nologo` passed; Microsoft.Testing.Platform ignored the VSTest filter and ran the full unit project: 406 total, 0 failed.
- `dotnet test DVault.slnx --nologo` passed. The integration project reported 203 total, 0 failed, 182 succeeded, 21 skipped; the unit project reported 406 total, 0 failed.
- `dotnet build DVault.slnx --nologo` passed with existing warning output and 0 errors.
- `bash tools/check-format.sh` passed.

### Handoff Notes
- The async overload contract is source-shape-only: same `DataVaultSaveChunk` payload model, same provider-neutral chunked boundary, same chunked telemetry family, and no hidden background continuation.
- Validation emitted existing warning noise, including NU1900 warnings from the sandbox's read-only NuGet vulnerability cache and existing analyzer warnings, but no command failed.
- No product clarification is open for this contract story.
<!-- gicket-bot:developer-delivery:v1:end -->