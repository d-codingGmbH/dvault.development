[gicket-bot] PO refinement contract

Summary
- Refined the explicit-save performance story around the existing IDataVaultSaveService boundary, current SQLite benchmark baseline, and evidence-first tuning rules; no bounded planning write was materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Visible repository evidence already fixes the ordinary DVault write boundary: IDataVaultSaveService plus provider-neutral/provider-specific save strategies own normal hub, link, and satellite writes; UseDataVaultSaveChangesMetadataInterceptor(...) remains metadata-only and is out of scope for this story.
- The current benchmark baseline already covers the save-focused SQLite scenarios this ticket needs: customer-profile-history, customer-profile-bulk-insert-only, customer-profile-bulk-history, and order-product-fulfillment-history, with comparisons across conventional EF, AddDVault() provider-neutral fallback, and AddDVaultSqlite() optimized writes.
- The shared benchmark artifact contract is already defined and visible: before/after evidence must reuse benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json with comparable run context and allocation fields rather than inventing a save-specific format.
- Current save-strategy gate behavior is already part of the visible baseline: provider-specific save strategies may decline when the provider name mismatches, the DbContext already has pending tracked changes, or the batch contains multi-active satellite operations; this story should preserve those gates unless benchmark evidence explicitly justifies a change.
- Current live relation context is bounded and understandable without new writes: the story sits under epic 06F492BTNHRPBC7D24E13ECFKM, it still blocks 06F492CTREZEDXVKJ839YGCPWW and 06F492D05THPGQVT3B3K7853A0, and the incoming blocks relation from done benchmark-contract ticket 06F492BZPP5YT9SJSPDHQBGF3R is treated as historical completed context in this refinement run.
- No child tickets, relation mutations, description updates, attachments, or planning documents were materialized because the visible repository and ticket evidence was sufficient to finalize the refinement contract directly.

Scope In
- Benchmark the current explicit save workflows on the required SQLite local baseline using the existing save scenarios and the shared performance-evidence contract.
- Measure change-tracker cost, repeated tracked-row scans, metadata resolution/defaulting cost, per-row existence checks, and batch-shape overhead inside the shared explicit save pipeline.
- Apply targeted tuning to DefaultDataVaultSaveService and closely related shared save helpers only when before/after benchmark artifacts show measurable benefit.
- Include SQLite save-path internals in scope when they materially affect the measured explicit-save overhead, while keeping the work anchored to the same public save contract.
- Preserve current functional semantics for hub/link reuse, satellite hash-diff filtering, deterministic saved-record ordering, request hook resolution, and RowsWritten behavior.

Scope Out
- Making SaveChanges interception the default DVault write path or expanding the metadata interceptor beyond filling missing LoadTimestamp and RecordSource values.
- Broad read-model allocation work, query-shape/index-hint work, or compiled-model/compiled-query/DbContext-pooling evidence already owned by sibling tickets.
- Provider concurrency, upsert, merge, retry, or multi-writer semantic changes beyond the current explicit save contract.
- New provider-package strategy implementations or broad provider-threshold redesign unless later evidence justifies a separate follow-up ticket.
- Arbitrary mixed dirty-context application workflows that are not part of the current clean-context benchmark baseline, unless the measured issue is shown to come from that fallback path.

Open questions
- none

Follow-up questions
- After this story lands, should a separate ticket benchmark deliberately dirty DbContext mixed-workflow saves to decide whether the DirtyDbContext fallback needs its own optimization lane or only documentation guidance?
- If evidence shows the remaining cost is dominated by provider-specific SQL rather than shared change-tracker overhead, should follow-up work target individual SQL Server, MySQL, or Oracle strategy internals instead of expanding this story?
- Should the stale incoming blocks relation from done benchmark-contract ticket 06F492BZPP5YT9SJSPDHQBGF3R be cleaned up in a later relation-maintenance pass, even though it is treated as historical completed context for this refinement run?

Risks
- Because the harness already compares provider-neutral fallback and SQLite optimized save paths, weak evidence capture could misattribute gains to strategy selection differences instead of actual change-tracker overhead reduction.
- Aggressive attempts to bypass EF tracking or collapse state checks can easily break current RowsWritten semantics, saved-record ordering, or satellite append-only/hash-diff behavior unless backed by focused regression coverage.
- Benchmarks that accidentally mix dirty tracked state or unsupported batch shapes can force provider-neutral fallback and produce misleading conclusions unless the run context records the selected strategy and fallback reason.

Split recommendations
- No split recommended at refinement time; keep one evidence-first explicit-save performance story unless measurement later proves provider-neutral fallback and provider-specific strategy hot spots need separate follow-up tickets.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment