<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the provider-neutral chunked save execution story against the landed contract ticket and current repo baseline; no bounded planning writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Related contract ticket `06F5Q8X261DQHG7N1445NGXB5W` is `done`, so the streaming explicit-save contract is landed and not a blocker for this story.
- Current repository evidence shows the production API still exposes only single-request and ordered-bulk `IDataVaultSaveService.SaveAsync(...)` overloads; `DataVaultChunkedSaveRequest` and `DataVaultSaveChunk` exist only in contract docs and test fixtures today.
- The repo already contains contract-level SQLite proof for ordering, cancellation, transaction participation, hub/link reuse, and satellite continuity, but those tests currently use private `ChunkedSaveContractRequest` / `ChunkedSaveContractChunk` harness types rather than a real public API.
- Existing relations already express the intended split: this story sits under epic `06F5Q8WVYMV8KQPAENPEEE3YM4` and blocks `06F5Q8XPXEQPJTKGJ7BQGCY438` (fallback/remediation guidance) and `06F5Q8XXSBGW1B8RDRMGVF557W` (benchmark evidence).
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this refinement run.

### Scope In
- Add the additive chunked explicit-save public surface in the core `DCoding.Data.DVault` package: `DataVaultChunkedSaveRequest`, `DataVaultSaveChunk`, and `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken)`.
- Implement one provider-neutral chunked execution path that processes bounded chunks in caller order without flattening the full logical load into one bulk request.
- Preserve existing load-timestamp, record-source, validation, ordering, cancellation, and caller-owned transaction semantics from the current explicit save pipeline.
- Carry hub/link reuse and satellite hash-diff/latest-state continuity across chunk boundaries so chunked results match equivalent ordered bulk input.
- Update unit/public-API/integration coverage to exercise the production chunked API instead of the current contract-only harness.

### Scope Out
- Provider-specific chunk optimizations or changes to provider package strategy contracts.
- Fallback/remediation explanation and user-visible retained-state guidance already separated into ticket `06F5Q8XPXEQPJTKGJ7BQGCY438`.
- Benchmark expansion and release evidence already separated into ticket `06F5Q8XXSBGW1B8RDRMGVF557W`.
- Background ingestion, schedulers, queues, file ingestion, CDC ingestion, or implicit `SaveChanges` interception.
- New hidden metadata lanes or chunk-specific registry abstractions beyond the explicit request contract.

## Acceptance Criteria
- The core package exposes additive public `DataVaultChunkedSaveRequest` and `DataVaultSaveChunk` types plus `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken)` without changing the existing single-request or ordered-bulk overload semantics.
- Chunked execution processes chunks in caller order, processes requests inside each chunk in caller order, treats empty chunk sequences and empty chunks as no-ops, and returns deterministic `DataVaultSaveResult` / `SavedRecords` ordering equivalent to the same ordered input sent through the established bulk pipeline.
- The provider-neutral chunked path preserves hub and link idempotent reuse by hash key, preserves satellite hash-diff replay/latest-state continuity across chunk boundaries keyed by satellite table plus parent/driving-key identity, and stops continuation to later chunks on validation, resolver, failure, or cancellation conditions.
- Chunked execution participates in the caller's current or ambient transaction, does not create, commit, rollback, or suppress transactions on the caller's behalf, and propagates cancellation before later chunks run.
- Automated coverage verifies the real chunked API for the existing contract scenarios: ordering, cancellation before later chunks, caller-transaction rollback, repeated hub/link reuse across chunks, and satellite replay/change behavior across chunks.

## Definition of Done
- Public API approval artifacts and XML-doc-visible surface are updated for the new chunked request types and save-service overload.
- Core save-service tests pass with the production chunked API and no regression to existing single-request, ordered-bulk, or provider strategy batch-contract coverage.
- The SQLite integration suite exercises the production chunked API rather than private contract-only wrapper types for the five contract scenarios already present in `ExplicitDataVaultSaveServiceSqliteTests`.
- The implementation remains additive to the existing explicit save boundary and does not fold blocked remediation or benchmark work into this ticket.

## Implementation Notes
- Current core implementation evidence points to `src/DCoding.Data.DVault/DataVaultSaveService.cs` as the owning surface; today it only exposes single-request and `DataVaultBulkSaveRequest` overloads, so this story owns the first production chunked API entry point.
- Do not satisfy the story by flattening every chunk into one giant `DataVaultBulkSaveRequest`; the ticket explicitly owns bounded chunk-by-chunk execution.
- Do not rely only on the current private test helper that loops `SaveAsync(..., DataVaultBulkSaveRequest)` per chunk; production code needs one chunked-service boundary that owns cross-chunk continuity and deterministic cancellation/ordering semantics.
- Keep the existing `IDataVaultProviderSaveStrategyContext.Requests` and `ResolvedRequests` flat-batch contract intact for v1; this story should not require provider package API churn to land the provider-neutral chunked path.
- A safe bounded default for v1 is to retain only the continuity state required to match ordered-bulk semantics across chunks, especially latest satellite state keyed by satellite table, parent hash key, and canonical multi-active driving-key values.

## Open Questions
- none

## Follow-Up Questions
- When ticket `06F5Q8XPXEQPJTKGJ7BQGCY438` is refined, what user-visible explanation and remediation guidance should ship for retained-state growth or deterministic rejection cases that remain outside this core execution ticket?
- When chunk-specific provider optimizations are eventually considered, should they extend the current strategy interface or remain behind a core chunk orchestration layer that continues handing provider strategies flat per-chunk batches?
- After the production API lands, does the next release note or README refresh need a public chunked-save example, or is architecture/test evidence sufficient until benchmark ticket `06F5Q8XXSBGW1B8RDRMGVF557W` lands?

## Risks
- Existing provider strategies are built around flat `IReadOnlyList<DataVaultSaveRequest>` batches; a naive implementation that simply re-enters the current public bulk overload per chunk could accidentally blur the intended provider-neutral execution boundary or fragment cross-chunk state ownership.
- Cross-chunk latest-state retention can still grow with the number of distinct satellite parent/driving-key series in a single logical request; user-facing explanation and remediation for that growth remains downstream work.
- Benchmark and release-evidence expectations for chunk sizes and throughput are intentionally deferred to ticket `06F5Q8XXSBGW1B8RDRMGVF557W`, so reviewers should not treat missing benchmark artifacts as a blocker for this implementation ticket.

## Split Recommendations
- No additional split is recommended; the live ticket graph already separates the landed contract (`06F5Q8X261DQHG7N1445NGXB5W`), this provider-neutral execution story, fallback/remediation guidance (`06F5Q8XPXEQPJTKGJ7BQGCY438`), and benchmark evidence (`06F5Q8XXSBGW1B8RDRMGVF557W`).

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Implement the provider-neutral execution path for streaming/chunked save requests.

Acceptance criteria:
- Processes bounded chunks without materializing the entire logical load.
- Preserves hub/link idempotency, satellite hash-diff behavior, ordering, cancellation, and transactions.
- Adds unit and integration coverage for chunk boundaries, duplicates, satellite replay, and cancellation.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Implemented the additive production chunked explicit-save API: `DataVaultChunkedSaveRequest`, `DataVaultSaveChunk`, and `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken)`.
- Added bounded chunk orchestration that processes chunks in caller order, observes cancellation before later chunks, treats empty chunk sequences and empty chunks as no-ops, and does not flatten all chunk requests into one bulk request.
- Replaced the SQLite private chunked contract harness with the production API for ordering, cancellation, transaction participation, hub/link reuse, and satellite continuity coverage.
- Updated unit validation coverage and the public API approval snapshot for the new public surface.

### Verification
- `dotnet build DVault.slnx --nologo --no-restore` passed after the final implementation changes.
- `dotnet test DVault.slnx --nologo --no-build` passed; SQLite/local suites ran, and 16 external-provider tests were skipped because their opt-in connection strings were not configured.
- `bash tools/check-format.sh` passed.

### Notes
- Build output still includes existing NU1900 warnings from NuGet vulnerability-cache writes under the read-only home cache, plus pre-existing analyzer warnings; no build errors remained.
- Chunk-specific provider optimizations and retained-state user guidance remain outside this ticket as planned.
<!-- gicket-bot:developer-delivery:v1:end -->