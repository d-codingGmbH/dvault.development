[gicket-bot] PO refinement contract

Summary
- Reclassified the epic to closure-only over already-landed v0.24.0 async streaming and EF safety guidance, and explicitly neutralized the stale legacy draft that still implied new diagnostics.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Reclassify this epic as closure-only and no-new-dev-work. It ratifies the already-landed v0.24.0 boundary and should not reopen developer work from this ticket unless a separate future ticket defines a concrete new deliverable.
- critic-item-2: `answered` - Archive the legacy `Original Ticket Draft` as superseded background only, or replace it with a one-line archival note. Its Goal, Scope In, Scope Out, and Acceptance Criteria are not authoritative live scope for this epic.
- critic-item-3: `answered` - Neutralize the stale legacy diagnostic promises at the old Scope In and Acceptance Criteria lines so the live contract states only the settled v0.24.0 boundary: guidance-only EF safety, fixed-model-only compiled/pooling guidance, and implemented analyzer coverage limited to `DMV1910` and `DMV1911`.

Clarifications
- This epic is closure-only and no-new-dev-work over the already-landed v0.24.0 repository and documentation surface.
- The only authoritative scope is the delivery contract; any preserved legacy draft is archival background only and is non-binding.
- Async streaming in scope means the additive `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)` overload and the async mapping helpers on the existing explicit save boundary.
- EF safety in scope is guidance-only: registry-backed `UseDataVaultMetadata(...)` isolates DVault-owned metadata sources, while caller-owned model-shape discriminators remain application `IModelCacheKeyFactory` responsibilities.
- `UseModel(...)` and `AddDbContextPool<TContext>(...)` remain fixed-model guidance only; they are not generalized safe defaults for variable realized model shapes.
- Implemented analyzer scope remains `DMV1910` and `DMV1911`; this epic does not promise new model-cache, compiled-model, or pooling diagnostics.

Scope In
- Ratify the already-landed async chunk-source explicit-save boundary on `IDataVaultSaveService`, including the `IAsyncEnumerable<DataVaultSaveChunk>` overload and async helper methods.
- Ratify save-path guidance across `DataVaultBulkSaveRequest`, `DataVaultChunkedSaveRequest`, and async chunk-source input without changing caller-owned timestamps, ordering, cancellation, or transaction behavior.
- Ratify the settled EF safety documentation boundary: registry-backed `UseDataVaultMetadata(...)` cache-key isolation, caller-owned `IModelCacheKeyFactory` discriminators, and fixed-model-only guidance for `UseModel(...)` and `AddDbContextPool<TContext>(...)`.
- Keep README, release-note wording, analyzer README guidance, public API evidence, tests, and benchmark artifacts aligned to one v0.24.0 story.
- Archive or explicitly neutralize the stale legacy draft so it no longer reads as live scope.

Scope Out
- New analyzer IDs or runtime guards for caller-owned model-cache, compiled-model, or pooled-context safety.
- Provider-native async writes, provider-native chunk execution, or provider-specific async performance claims.
- Any new ingestion platform, scheduler, background worker, dashboard, or automatic deployment work.
- Replacing the explicit `IDataVaultSaveService` boundary with implicit `SaveChanges` streaming or another default persistence path.
- Reopening new developer work from this epic without a separate concrete follow-on ticket.

Open questions
- none

Follow-up questions
- Should a future follow-on ticket add analyzer or runtime guardrails for variable-model compiled-model or pooled-context misuse beyond the current documentation-only boundary?
- Should a future provider-evidence ticket collect non-SQLite async/provider benchmark runs once external provider connection strings are configured?
- Should a future release add distinct public diagnostics or telemetry language for async chunk-source saves beyond the shared `ChunkedRequest` family?

Risks
- The epic title can still be misread as promising new EF safety diagnostics unless the closure-only framing and archived legacy-draft note stay explicit.
- Optional external-provider benchmark rows remain skipped when `DVAULT_TEST_*` connection strings are unset, so performance wording must stay bounded to the checked-in SQLite/provider-neutral evidence.
- Consumers can still misuse compiled models or pooled contexts for variable realized model shapes; the current mitigation is documentation plus caller-owned cache-key design, not enforcement.

Split recommendations
- Keep any future provider-native async write or provider-specific async execution claims in a separate follow-on ticket.
- Keep any future model-cache, compiled-model, or pooling analyzer/runtime guardrails in a separate follow-on ticket.
- If future development work is later desired, reopen only a new concrete ticket for that deliverable instead of reopening this closure-only epic.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment