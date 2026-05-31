[gicket-bot] PO refinement contract

Summary
- Refined this story against the landed async chunked save contract and current typed mapper helper surface. The ticket is bounded to additive async-source helpers that feed the existing async chunked save entry point while keeping mapping, load timestamp, record source, and chunk boundaries caller-owned.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The existing async save boundary is already `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)`; this story adds helper or adaptor APIs on top of that boundary rather than a new save-service contract.
- The manual async path stays on the explicit request boundary: callers supply the mapping that produces `DataVaultSaveRequest` values and the helper only batches or forwards them in order.
- The typed async path reuses the existing `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, and `IDataVaultSatelliteMapper<TSource>` contracts and keeps `loadTimestamp` and `recordSource` explicit at the call site.
- Typed helper coverage should match the current typed save-helper baseline, including ordinary hub-parent satellite support only; callers needing multi-active or link-parent satellite convenience can use the explicit request-mapping path.

Scope In
- Add additive async-source helper or adaptor APIs for `IAsyncEnumerable<TSource>` that preserve source order and compose with the existing async chunked save entry point.
- Support caller-supplied mapping from each async source item to an explicit `DataVaultSaveRequest`, with caller-owned bounded chunk sizing or equivalent visible chunk-boundary input.
- Support async typed-helper usage for existing hub, link, and ordinary hub-parent satellite mapper contracts by assembling the same per-item registry-backed request shape used by the current single and `IEnumerable<TSource>` helpers.
- Preserve current explicit-save semantics for cancellation, transaction ownership, sequential chunk advancement, and deterministic saved-record ordering.

Scope Out
- No new provider-specific async chunk execution, background ingestion, scheduler orchestration, or `SaveChanges` interception path.
- No CSV, JSON, or file ingestion pipeline, schema inference, mapper discovery, or generic ETL subsystem.
- No hidden load timestamp, record source, business-key, or chunk-size defaults; those remain caller-visible inputs.
- No expansion of typed helper coverage beyond the existing typed contract boundaries such as repeated-name link mappers or convenience support for non-ordinary satellite helper shapes.

Open questions
- none

Follow-up questions
- Should a later story add separate convenience APIs for async sources that already produce registry-backed requests instead of full explicit `DataVaultSaveRequest` values?
- Should a later story add optional higher-level chunking helpers for callers that want custom boundary strategies beyond the initial bounded default?
- After the public async helper baseline lands, do we want benchmark or guidance updates that recommend chunk-size ranges for common provider profiles?

Risks
- The public API can become noisy if both adapter-style and direct save-style helpers are added without a tight naming and documentation story.
- Typed async helper expectations may be misread as broader than the current typed helper contract, especially for generated satellite mappers that target unsupported convenience shapes.
- A careless implementation could accidentally pre-buffer the full async source or hide chunk defaults, which would violate the landed async chunked save contract.

Split recommendations
- none

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