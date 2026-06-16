[gicket-bot] PO refinement contract

Summary
- Refined the Oracle bulk-gap ticket to a bounded no-op/defer recommendation: keep the existing direct Oracle array-binding path and current 50-operation/10000-satellite gates, and do not pursue staged Oracle bulk or threshold tuning without new provider-configured benchmark evidence. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The prompt snapshot already supplied the ticket description and showed no recent comments. Runtime re-reads of ticket, comment, and relation state through the gicket read tools were trust-blocked, so this refinement uses the supplied ticket snapshot plus repository evidence only.
- The repository already ratifies the v1 Oracle bulk baseline: `OracleDataVaultSaveStrategy` is the current provider-specific save path, it retains direct Oracle batching, it opportunistically uses Oracle array binding when the provider command exposes `ArrayBindCount`, and staged Oracle bulk remains `not-selected-no-measured-win`.
- The root `benchmark-summary.*` Oracle bulk row is a skipped placeholder when `DVAULT_TEST_ORACLE_CONNECTION_STRING` is unset; completed Oracle timing evidence for this decision lives in the checked-in v0.32 artifact bundle, not in the skipped root row.

Scope In
- Evaluate the current Oracle `provider-native-bulk-ingestion` gap only: array binding behavior, staged-bulk posture, threshold gates, and benchmark evidence boundaries.
- Ratify the current recommendation for gap-matrix row `P1.04` as a documentation/no-op decision unless new provider-configured evidence justifies reopening implementation work.
- Document the stop and fallback conditions that bound Oracle bulk claims: provider mismatch, dirty context, multi-active satellite operations, batches below 50 total operations, and batches above 10000 satellite operations.

Scope Out
- Implementing a new staged Oracle bulk path in this ticket.
- Tuning the current Oracle thresholds without new benchmark evidence.
- Re-running benchmarks or turning skipped root Oracle rows into completed timing claims in this ticket.
- Oracle latest-satellite, PIT as-of, or bridge read evidence gaps; those remain separate backlog rows and are not part of this bulk-only refinement.

Open questions
- none

Follow-up questions
- If Oracle customer workloads later require provider-specific optimization above 10000 satellite operations or for multi-active satellite shapes, should that be opened as a separate implementation-plus-evidence ticket rather than expanding this evaluation ticket?
- If staged Oracle bulk is reconsidered later, should the success bar require a measured win over both the provider-neutral fallback and the retained direct Oracle path before any threshold or routing change is accepted?

Risks
- The current recommendation is intentionally bounded by existing repository evidence. Without a new provider-configured benchmark rerun, the product should not broaden Oracle timing claims beyond the documented direct-path cases.
- Oracle workloads outside the current gate, especially over 10000 satellite operations or multi-active satellite batches, still rely on fallback behavior; that remains a known capability boundary rather than a resolved optimization.
- The v0.32 completed Oracle evidence still showed conventional EF outperforming the retained direct Oracle path in the recorded large-batch comparisons, so any future product messaging must avoid implying a universal Oracle performance win.

Split recommendations
- No split is needed for this ticket. Keep it as a bounded evaluation that concludes with a keep-as-is recommendation, and open a separate follow-up only if new Oracle workload evidence justifies staged bulk or threshold changes.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment