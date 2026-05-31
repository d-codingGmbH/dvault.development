<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this story against the landed async chunked save contract and current typed mapper helper surface. The ticket is bounded to additive async-source helpers that feed the existing async chunked save entry point while keeping mapping, load timestamp, record source, and chunk boundaries caller-owned.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The existing async save boundary is already `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)`; this story adds helper or adaptor APIs on top of that boundary rather than a new save-service contract.
- The manual async path stays on the explicit request boundary: callers supply the mapping that produces `DataVaultSaveRequest` values and the helper only batches or forwards them in order.
- The typed async path reuses the existing `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, and `IDataVaultSatelliteMapper<TSource>` contracts and keeps `loadTimestamp` and `recordSource` explicit at the call site.
- Typed helper coverage should match the current typed save-helper baseline, including ordinary hub-parent satellite support only; callers needing multi-active or link-parent satellite convenience can use the explicit request-mapping path.

### Scope In
- Add additive async-source helper or adaptor APIs for `IAsyncEnumerable<TSource>` that preserve source order and compose with the existing async chunked save entry point.
- Support caller-supplied mapping from each async source item to an explicit `DataVaultSaveRequest`, with caller-owned bounded chunk sizing or equivalent visible chunk-boundary input.
- Support async typed-helper usage for existing hub, link, and ordinary hub-parent satellite mapper contracts by assembling the same per-item registry-backed request shape used by the current single and `IEnumerable<TSource>` helpers.
- Preserve current explicit-save semantics for cancellation, transaction ownership, sequential chunk advancement, and deterministic saved-record ordering.

### Scope Out
- No new provider-specific async chunk execution, background ingestion, scheduler orchestration, or `SaveChanges` interception path.
- No CSV, JSON, or file ingestion pipeline, schema inference, mapper discovery, or generic ETL subsystem.
- No hidden load timestamp, record source, business-key, or chunk-size defaults; those remain caller-visible inputs.
- No expansion of typed helper coverage beyond the existing typed contract boundaries such as repeated-name link mappers or convenience support for non-ordinary satellite helper shapes.

## Acceptance Criteria
- An additive async helper path lets callers take `IAsyncEnumerable<TSource>` plus explicit request mapping and feed the existing `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)` entry point without materializing the full source first.
- An additive typed async helper path lets callers use existing hub, link, and ordinary hub-parent satellite mapper contracts with caller-supplied `loadTimestamp`, `recordSource`, and bounded chunk sizing while preserving source and chunk order.
- Helper-generated async chunks and saves preserve the same visible semantics as the landed async chunked save contract: no background continuation, no reordering, cancellation before later chunks, and participation in the caller's current transaction.
- Tests cover ordering, chunk-boundary handling, mapper or request-factory failures, cancellation, and compatibility with generated typed mappers where the current typed helper surface already supports them.

## Definition of Done
- The additive public helper surface is implemented, XML-documented, and reflected in the public API snapshot.
- Focused unit tests prove async chunk assembly order, empty or no-op behavior, failure wrapping, and cancellation behavior without full-source buffering.
- Focused integration tests prove supported typed async helper flows save successfully through the async chunked save boundary and preserve deterministic saved-record ordering.
- Relevant contract or release documentation is updated to show that the new helpers are convenience layers over the existing explicit async chunked save boundary.

## Implementation Notes
- Keep the work on the existing helper extension surfaces instead of introducing a new service abstraction or a second async chunk wrapper type.
- Use the existing async chunked save overload as the execution boundary; helper logic should only adapt source items into bounded `DataVaultSaveChunk` values or delegate into that overload.
- Treat chunk sizing as a caller-owned bounded request-count or equivalent visible chunk-boundary setting; do not auto-tune chunk sizes by provider or telemetry.
- For typed helpers, keep the current stable exception-context pattern from `DataVaultSaveServiceTypedExtensions` so mapper failures identify the logical target, source type, and bounded position context.
- When a source item needs mixed hub, link, and satellite orchestration or a typed-helper shape that current typed helpers do not cover, the supported escape hatch is the explicit `DataVaultSaveRequest` mapping path.

## Open Questions
- none

## Follow-Up Questions
- Should a later story add separate convenience APIs for async sources that already produce registry-backed requests instead of full explicit `DataVaultSaveRequest` values?
- Should a later story add optional higher-level chunking helpers for callers that want custom boundary strategies beyond the initial bounded default?
- After the public async helper baseline lands, do we want benchmark or guidance updates that recommend chunk-size ranges for common provider profiles?

## Risks
- The public API can become noisy if both adapter-style and direct save-style helpers are added without a tight naming and documentation story.
- Typed async helper expectations may be misread as broader than the current typed helper contract, especially for generated satellite mappers that target unsupported convenience shapes.
- A careless implementation could accidentally pre-buffer the full async source or hide chunk defaults, which would violate the landed async chunked save contract.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Reduce boilerplate for caller-owned async domain sources while keeping DVault saves explicit.

# Scope In
- Add helper/adaptor APIs for IAsyncEnumerable<TSource> where callers supply explicit mapping to DataVaultSaveRequest values or existing typed mapper contracts.
- Keep chunk sizing, load timestamp, record source, and business-key mapping caller-visible.

# Scope Out
No CSV/JSON ingestion, schema inference, entity tracking magic, or generic ETL subsystem.

# Acceptance Criteria
- Helpers compose with the async chunked save entry point.
- Tests cover ordering, chunk sizing, mapper failures, cancellation, and generated typed mapper compatibility where applicable.