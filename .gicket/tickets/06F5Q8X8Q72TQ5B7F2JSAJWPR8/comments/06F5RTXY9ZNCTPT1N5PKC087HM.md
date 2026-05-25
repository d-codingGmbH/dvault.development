[gicket-bot] PO refinement contract

Summary
- Refined the provider-neutral chunked save execution story against the landed contract ticket and current repo baseline; no bounded planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Related contract ticket `06F5Q8X261DQHG7N1445NGXB5W` is `done`, so the streaming explicit-save contract is landed and not a blocker for this story.
- Current repository evidence shows the production API still exposes only single-request and ordered-bulk `IDataVaultSaveService.SaveAsync(...)` overloads; `DataVaultChunkedSaveRequest` and `DataVaultSaveChunk` exist only in contract docs and test fixtures today.
- The repo already contains contract-level SQLite proof for ordering, cancellation, transaction participation, hub/link reuse, and satellite continuity, but those tests currently use private `ChunkedSaveContractRequest` / `ChunkedSaveContractChunk` harness types rather than a real public API.
- Existing relations already express the intended split: this story sits under epic `06F5Q8WVYMV8KQPAENPEEE3YM4` and blocks `06F5Q8XPXEQPJTKGJ7BQGCY438` (fallback/remediation guidance) and `06F5Q8XXSBGW1B8RDRMGVF557W` (benchmark evidence).
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Add the additive chunked explicit-save public surface in the core `DCoding.Data.DVault` package: `DataVaultChunkedSaveRequest`, `DataVaultSaveChunk`, and `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken)`.
- Implement one provider-neutral chunked execution path that processes bounded chunks in caller order without flattening the full logical load into one bulk request.
- Preserve existing load-timestamp, record-source, validation, ordering, cancellation, and caller-owned transaction semantics from the current explicit save pipeline.
- Carry hub/link reuse and satellite hash-diff/latest-state continuity across chunk boundaries so chunked results match equivalent ordered bulk input.
- Update unit/public-API/integration coverage to exercise the production chunked API instead of the current contract-only harness.

Scope Out
- Provider-specific chunk optimizations or changes to provider package strategy contracts.
- Fallback/remediation explanation and user-visible retained-state guidance already separated into ticket `06F5Q8XPXEQPJTKGJ7BQGCY438`.
- Benchmark expansion and release evidence already separated into ticket `06F5Q8XXSBGW1B8RDRMGVF557W`.
- Background ingestion, schedulers, queues, file ingestion, CDC ingestion, or implicit `SaveChanges` interception.
- New hidden metadata lanes or chunk-specific registry abstractions beyond the explicit request contract.

Open questions
- none

Follow-up questions
- When ticket `06F5Q8XPXEQPJTKGJ7BQGCY438` is refined, what user-visible explanation and remediation guidance should ship for retained-state growth or deterministic rejection cases that remain outside this core execution ticket?
- When chunk-specific provider optimizations are eventually considered, should they extend the current strategy interface or remain behind a core chunk orchestration layer that continues handing provider strategies flat per-chunk batches?
- After the production API lands, does the next release note or README refresh need a public chunked-save example, or is architecture/test evidence sufficient until benchmark ticket `06F5Q8XXSBGW1B8RDRMGVF557W` lands?

Risks
- Existing provider strategies are built around flat `IReadOnlyList<DataVaultSaveRequest>` batches; a naive implementation that simply re-enters the current public bulk overload per chunk could accidentally blur the intended provider-neutral execution boundary or fragment cross-chunk state ownership.
- Cross-chunk latest-state retention can still grow with the number of distinct satellite parent/driving-key series in a single logical request; user-facing explanation and remediation for that growth remains downstream work.
- Benchmark and release-evidence expectations for chunk sizes and throughput are intentionally deferred to ticket `06F5Q8XXSBGW1B8RDRMGVF557W`, so reviewers should not treat missing benchmark artifacts as a blocker for this implementation ticket.

Split recommendations
- No additional split is recommended; the live ticket graph already separates the landed contract (`06F5Q8X261DQHG7N1445NGXB5W`), this provider-neutral execution story, fallback/remediation guidance (`06F5Q8XPXEQPJTKGJ7BQGCY438`), and benchmark evidence (`06F5Q8XXSBGW1B8RDRMGVF557W`).

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment