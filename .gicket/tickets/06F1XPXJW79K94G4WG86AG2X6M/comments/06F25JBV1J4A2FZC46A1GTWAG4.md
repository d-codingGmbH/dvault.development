[gicket-bot] PO refinement contract

Summary
- Refined the story against the v0.8.0 branch baseline: keep "current" aligned with existing latest-satellite semantics, scope the work to explicit EF Core-friendly helper APIs over the established latest/as-of and bridge read surfaces, and leave live ticket relations unchanged.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the current v0.8.0 branch surface as the refinement baseline; the referenced v0.6.0 release notes are historical context, not the active branch contract.
- "Current" in this story ratifies the existing latest-satellite read semantics by parent hash key rather than introducing a new bitemporal concept.
- Bridge traversal already has provider-neutral read infrastructure in the branch (`DataVaultBridgeReadPipeline`, `DataVaultBridgeReadRecord`, `DataVaultBridgeProjectionRow`); this story is about public helper ergonomics, examples, and diagnostics on top of that baseline.
- Existing live planning context remains in place: child ticket `06F1XPXY7QKTYAW43JTT3BM704`, outgoing `blocks` relations to `06F1XPYA9MD0T9C4651ND8KX0W` and `06F1XPYW5PVKRTK4A91M6GHHF8`, and incoming `blocks` relation from `06F1XPRY3ZDB6W1WQ9ABRRJ2V4`.
- No new child tickets, relation updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Add explicit public helper APIs for latest/current satellite reads, as-of satellite reads, and bridge traversal reads on the existing provider-neutral DVault read boundary.
- Keep the helpers EF Core-friendly by using caller-owned delegates/projection patterns and normal application query composition rather than a custom query provider.
- Document metadata requirements, exact-name bridge projection expectations, supported shapes, and failure modes.
- Add representative tests and runnable or compile-checked examples that show when to use the helpers versus lower-level raw read pipelines.

Scope Out
- PIT-backed as-of reads, PIT maintenance, and PIT row refresh behavior.
- Hidden materialized view or bridge maintenance workflows.
- A custom LINQ provider, intercepted query translation layer, or a promise of universal provider translation for every helper shape.
- Provider-specific read optimizations or broad redesign of the underlying read-service contract.

Open questions
- none

Follow-up questions
- After this helper surface lands, should a later ticket align bridge-helper examples and docs with any future PIT-backed as-of helper story so callers get one consolidated advanced-read guide?
- If downstream consumers need stronger provider-translation guarantees, should that be handled as a separate provider-capability ticket instead of widening this story?

Risks
- The incoming `blocks` relation from `06F1XPRY3ZDB6W1WQ9ABRRJ2V4` remains a delivery risk until that prerequisite is resolved.
- If implementation drifts toward returning provider-translatable `IQueryable` abstractions instead of explicit helper APIs, it will conflict with the story's stated non-goals and likely expand scope.

Split recommendations
- No additional split is justified from the current evidence; keep the existing child ticket `06F1XPXY7QKTYAW43JTT3BM704` and the separate PIT-backed planning lane rather than creating new child tickets during this PO pass.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment