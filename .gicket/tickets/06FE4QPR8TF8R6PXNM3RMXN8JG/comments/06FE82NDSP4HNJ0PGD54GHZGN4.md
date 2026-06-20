[gicket-bot] PO refinement contract

Summary
- Verified and resolved the only blocking PO-critic contradiction: the persisted ticket description now explicitly records the materialized description update and no other bounded planning writes were applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Resolved. The authoritative delivery contract no longer claims that no description update occurred. The persisted description now explicitly states that this PO refinement pass materialized the ticket-description contract update, which matches the earlier PO run report that said the durable refinement contract in the ticket description was updated.

Clarifications
- The authoritative delivery contract in the ticket description was already updated on the ticket owner branch to remove the internal contradiction called out by PO-critic.
- No child tickets, relation writes, attachments, or planning documents were materialized while resolving this finding.
- The existing downstream documentation split remains the same: broader evidence-matrix and release-note promotion work stays in 06FE4QRMXVGJVA65ZR5MZ817K8.
- The current contract still treats PostgreSQL latest-satellite timing as unmeasured until a provider-configured completed artifact or preserved benchmark comparison is available.

Scope In
- Tune or explicitly retain the PostgreSQL latest-satellite read SQL shape for supported hub-parent, non-multi-active requests using preserved evidence rather than assumption.
- Capture a provider-configured PostgreSQL latest-satellite timing artifact or preserved benchmark comparison against the provider-neutral or historical fallback comparator before claiming a win.
- Keep IDataVaultReadDiagnosticsService output, benchmark execution-detail tokens, and tests aligned with the chosen PostgreSQL latest-satellite path so selected strategy versus fallback remains auditable.
- Preserve the current gate boundary: provider mismatch, unsupported satellite parent, multi-active driving keys, or diagnostics that do not select PostgresDataVaultReadStrategy must fall back.

Scope Out
- Broad release-note, matrix, and adopter-document promotion work that already sits in downstream docs task 06FE4QRMXVGJVA65ZR5MZ817K8.
- Widening PostgreSQL latest-satellite support beyond hub-parent, non-multi-active shapes or changing PIT/bridge scope.
- Treating latest-satellite save-side index experiments or save-path benchmarks as proof of read-strategy improvement.
- Any measured external-provider timing claim without a preserved provider-configured artifact triplet or equivalent preserved run context.

Open questions
- none

Follow-up questions
- Should downstream docs task 06FE4QRMXVGJVA65ZR5MZ817K8 attach the eventual provider-configured PostgreSQL latest-satellite artifact directly once this ticket fixes the strategy decision?

Risks
- Because the root PostgreSQL latest-satellite row is still a skipped placeholder, the team could overstate strategy registration as measured timing unless a provider-configured completed artifact or equivalently preserved comparator is stored.
- Changing the SQL shape without preserving parity and fallback coverage could drift latest-row semantics or diagnostics behavior.
- The strongest checked-in latest-index PostgreSQL numbers describe save-side lookup and index experiments, so using them as read-strategy proof would blur the evidence contract.

Split recommendations
- No additional PO split is needed; provider-specific latest-satellite tuning remains isolated to this ticket and broader documentation follow-through already exists in 06FE4QRMXVGJVA65ZR5MZ817K8.
- If relation normalization is wanted later, handle the historical done-ticket blocks link as separate housekeeping rather than widening this tuning ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment