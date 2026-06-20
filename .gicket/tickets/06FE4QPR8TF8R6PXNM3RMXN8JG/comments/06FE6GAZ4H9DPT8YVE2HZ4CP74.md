[gicket-bot] PO refinement contract

Summary
- Verified the current PostgreSQL latest-satellite tuning ticket against live repository and relation evidence; it is ready for PO-critic with a bounded evidence-first contract and no additional split or relation write applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live relation state was verified locally: done story 06FE4QNWP9606HTB92MTVQMYDG relates to this ticket, done task 06FE4QP6FB892E7TJMB47A3MSR still has a historical incoming blocks link, and this ticket currently blocks downstream documentation task 06FE4QRMXVGJVA65ZR5MZ817K8; no relation write was applied in this pass.
- The root benchmark triplet already records the current PostgreSQL latest-satellite row as a skipped placeholder with selectedStrategy=PostgresDataVaultReadStrategy, plannedReadStrategy=PostgresDataVaultReadStrategy, and readShape=LatestSatellite; that row is guidance only and not completed timing evidence until a provider-configured run completes.
- A historical measured comparator already exists in artifacts/benchmarks/v0.31.0-all-providers-smoke-<redacted>/benchmark-summary.*: PostgreSQL latest-satellite-read completed at mean 25.723 ms with provider-neutral fallback because no latest-satellite provider strategy was registered at that time.
- Current code already bounds the PostgreSQL strategy to provider-name match, hub-parent satellites, and non-multi-active shapes, and the provider package currently emits a windowed ROW_NUMBER() latest-row query.
- The current v0.42 documentation baseline already states that non-SQLite latest-satellite rows are not completed timing claims and that unsupported providers or shapes fall back to provider-neutral reads.

Scope In
- Tune or explicitly retain the PostgreSQL latest-satellite read SQL shape for supported hub-parent, non-multi-active requests using preserved evidence rather than assumption.
- Capture a provider-configured PostgreSQL latest-satellite timing artifact or equivalent preserved benchmark comparison against the provider-neutral or historical fallback comparator before claiming a win.
- Keep IDataVaultReadDiagnosticsService output, benchmark execution-detail tokens, and tests aligned with the chosen PostgreSQL latest-satellite path so selected strategy versus fallback remains auditable.
- Preserve the current gate boundary: provider mismatch, unsupported satellite parent, multi-active driving keys, or diagnostics that do not select PostgresDataVaultReadStrategy must fall back.

Scope Out
- Broad release-note, matrix, and adopter-document promotion work that already sits in downstream docs task 06FE4QRMXVGJVA65ZR5MZ817K8.
- Widening PostgreSQL latest-satellite support beyond hub-parent, non-multi-active shapes or changing PIT/bridge scope.
- Treating latest-satellite save-side index experiments or save-path benchmarks as proof of read-strategy improvement.
- Any measured external-provider timing claim without a preserved provider-configured artifact triplet and run context.

Open questions
- none

Follow-up questions
- After tuning lands, should the historical incoming blocks link from done ticket 06FE4QP6FB892E7TJMB47A3MSR to this ticket be cleaned up as relation housekeeping?
- Should downstream docs task 06FE4QRMXVGJVA65ZR5MZ817K8 attach the eventual provider-configured PostgreSQL latest-satellite artifact directly once this ticket fixes the strategy decision?

Risks
- Because the root PostgreSQL latest-satellite row is currently a skipped placeholder, the team could overstate strategy registration as measured timing unless a provider-configured completed artifact is preserved.
- Changing the SQL shape without preserving parity and fallback tests could drift latest-row semantics or diagnostics behavior.
- The strongest checked-in PostgreSQL latest-index numbers currently describe save-side lookup and index experiments, so mixing them into read-strategy claims would blur the evidence contract.

Split recommendations
- No additional PO split is needed; provider-specific latest-satellite tuning is already isolated to this ticket and broader documentation follow-through already exists in 06FE4QRMXVGJVA65ZR5MZ817K8.
- If relation normalization is wanted later, handle it as separate housekeeping rather than widening this tuning ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment