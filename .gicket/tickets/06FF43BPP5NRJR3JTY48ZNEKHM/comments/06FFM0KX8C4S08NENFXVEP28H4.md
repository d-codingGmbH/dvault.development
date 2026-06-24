[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded PIT full-rebuild benchmark-contract normalization task; no child tickets, relation changes, description writes, or planning documents were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket is about benchmark artifact row normalization for PIT full-rebuild maintenance, not about adding new provider PIT maintenance strategies or widening benchmark schemas.
- The bounded repository baseline is already fixed: PIT maintenance evidence must use scenario `pit-full-rebuild-maintenance`, not `pit-as-of-read` or `bridge-traversal-read`.
- Provider-neutral comparator rows must preserve whether the provider-specific maintenance path was not selected, the bounded fallback causes when present, and the benchmark run context needed for later evidence citations.
- No ticket description update, relation change, attachment, or planning-document write was applied during this refinement pass.

Scope In
- Normalize provider-neutral comparator rows for PIT full-rebuild maintenance benchmark artifacts.
- Keep PostgreSQL and SQL Server maintenance lanes structurally comparable across markdown, CSV, and JSON outputs.
- Preserve provider-neutral fallback posture, bounded fallback-cause vocabulary, and run-context evidence needed for later evidence-matrix or release-note citations.
- Update or add benchmark-contract verification coverage so comparator rows stay stable across artifact formats.

Scope Out
- Implement new PostgreSQL, SQL Server, MySQL, Oracle, or DB2 PIT maintenance strategies.
- Claim PIT maintenance evidence from `pit-as-of-read` or `bridge-traversal-read` benchmark rows.
- Invent a new benchmark schema, new result columns, or ad hoc provider-specific comparator prose outside the existing artifact contract.
- Broaden this ticket into bridge maintenance, automatic refresh, or non-maintenance read optimization work.

Open questions
- none

Follow-up questions
- After PostgreSQL and SQL Server comparator rows are normalized, should later PIT maintenance tickets for MySQL, Oracle, or DB2 adopt the same comparator-row contract from the start?
- If downstream docs or manifests still need special-case wording after this change, should a follow-up ticket add a representative PIT maintenance comparator example to the evidence-matrix documentation?

Risks
- If comparator rows drift into provider-specific prose instead of bounded contract tokens, evidence-matrix and release-note consumers will need brittle special-case parsing.
- If PIT read or bridge read rows are cited as maintenance evidence, the resulting claims will violate the repository's documented evidence boundary.
- If one provider lane preserves provider-neutral fallback detail differently from the other, PostgreSQL and SQL Server citations will remain non-comparable even if both rows exist.

Split recommendations
- No split recommended; the repository evidence supports a single bounded benchmark-contract normalization slice for PostgreSQL and SQL Server comparator rows.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment