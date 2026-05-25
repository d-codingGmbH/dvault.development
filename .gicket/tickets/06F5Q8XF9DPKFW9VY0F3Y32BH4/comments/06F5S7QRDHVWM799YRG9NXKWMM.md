[gicket-bot] PO refinement contract

Summary
- Refined this story as the request-scoped retained-state and bounded diagnostics slice of the streaming-save epic; no child tickets, relation changes, description updates, attachments, or planning documents were materialized because the existing epic split already covers contract, execution, fallback/remediation, and benchmark work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Done ticket 06F5Q8X261DQHG7N1445NGXB5W already fixes the additive chunked-save boundary, caller-owned transaction and cancellation rules, and the non-goals for background ingestion and implicit SaveChanges; this story should not reopen API-shape or orchestration questions.
- The retained continuity state for this story is the per-attempt state needed to preserve hash-key and hash-diff behavior across chunk boundaries; its visible scope is per satellite table or metadata declaration, parent hash key, and canonical multi-active driving-key values when applicable.
- The outgoing blocks relations to 06F5Q8XPXEQPJTKGJ7BQGCY438 and 06F5Q8XXSBGW1B8RDRMGVF557W make this story the source of raw bounded state and memory diagnostics that later fallback/remediation and benchmark work depend on, not the place for remediation prose or benchmark evidence.
- Repository diagnostics baselines already require deterministic redacted output and finite fallback vocabularies; this story should extend those bounded patterns for streaming continuity state instead of introducing ad-hoc logs or raw value dumps.

Scope In
- Implement the retained-state layer needed to preserve chunk-boundary hash-key and hash-diff continuity for chunked saves without materializing the full logical load.
- Bound that retained state for one explicit chunked-save attempt and release it deterministically on success, failure, or cancellation.
- Emit bounded deterministic redacted diagnostics for chunk counts, retained-state usage, state-bound fallback or rejection causes, and unsupported memory-sensitive shapes.
- Add unit and integration coverage for continuity preservation, deterministic release, and bounded-diagnostics behavior.

Scope Out
- Redefining the public chunked-save contract or its non-goals; that is already owned by done ticket 06F5Q8X261DQHG7N1445NGXB5W.
- Owning the general provider-neutral chunk execution pipeline or ordinary chunk and request ordering behavior beyond the retained-state and state-diagnostics slice.
- Developer-facing remediation guidance for provider mismatch, dirty-context, transaction, or chunk-sizing issues; that remains with 06F5Q8XPXEQPJTKGJ7BQGCY438.
- Benchmark artifact production or release evidence; that remains with 06F5Q8XXSBGW1B8RDRMGVF557W.
- New advanced-configuration hook families or provider-specific tuning matrices for memory policy in v1.

Open questions
- none

Follow-up questions
- After the deterministic v1 default ships, do consumers need an optional advanced hook for retained-state thresholds or eviction policy, or is the fixed bounded policy sufficient for the planned release window?
- Once this story lands, should support-bundle or diagnostics-report surfaces gain a representative chunked-save memory and state section, or is telemetry-only coverage enough for v1?
- Do any provider packages need provider-specific optimized continuity-state diagnostics after the provider-neutral bounded-state baseline is in place?

Risks
- If continuity state is keyed too broadly, released too late, or allowed to outlive the explicit save attempt, the implementation can leak memory or contaminate later saves while appearing semantically correct in happy-path tests.
- If diagnostics emit raw hash keys, payload values, or unbounded per-parent detail, the story will violate the repository's deterministic redaction baseline and create supportability noise rather than bounded diagnostics.
- If this story absorbs remediation text or benchmark evidence work, it will duplicate the already-related sibling tickets and make ownership across the epic harder to reason about.

Split recommendations
- No additional split is recommended; the current epic already separates contract (06F5Q8X261DQHG7N1445NGXB5W), provider-neutral execution (06F5Q8X8Q72TQ5B7F2JSAJWPR8), fallback/remediation (06F5Q8XPXEQPJTKGJ7BQGCY438), and benchmark evidence (06F5Q8XXSBGW1B8RDRMGVF557W).

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