[gicket-bot] PO refinement contract

Summary
- Refined the task around benchmark-triplet provider-read rows and verifier protection for skipped optional providers without reopening provider implementation scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository planning evidence already fixes the v1 baseline: latest satellite, PIT as-of, and bridge traversal reads are required scenarios; SQLite local temporary files is the required measured provider; PostgreSQL, SQL Server, MySQL, and Oracle remain optional-provider lanes that must stay visible as skipped when their connection strings are absent.
- The current release posture is already bounded: SQLite is the only repository-proven optimized read provider path today, and non-SQLite optimized read claims must not be inferred from provider-neutral read support or write-strategy registrations.
- No human comments or closure-evidence amendments changed scope, and no planning document, attachment, description update, child ticket, or relation mutation was materialized during this refinement.
- Live relations show this ticket is a child of 06F8KZHNYE6PAGC74BSF70WZ3W, currently blocks 06F8KZKFTCC0YXAPRTXA53DNEC, and is blocked by 06F8KZJNZ999C8NKY0S92VBDN0; no relation cleanup was justified from the current evidence.

Scope In
- Extending benchmark artifact generation so provider read optimization rows are present for the existing read-model scenarios covered by the shared benchmark artifact contract.
- Adding or tightening verifier coverage that keeps provider read rows aligned across benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json.
- Preserving optional-provider read rows as visible skipped rows, including normalized execution status, skip reason, zero iterations, blank or null metrics, deterministic execution detail, and persistedOutcome=not executed when provider connection strings are absent.
- Keeping benchmark-facing evidence summaries bounded to measured results so skipped or unexecuted rows do not become performance claims.

Scope Out
- Implementing new provider-specific read strategies or changing runtime read dispatch behavior.
- Claiming optimized read performance for PostgreSQL, SQL Server, MySQL, or Oracle without fresh measured benchmark evidence.
- Adding new benchmark scenario families beyond the existing latest satellite, PIT as-of, and bridge traversal read baseline.
- Release automation, dashboards, hosted observability, or provider credential or provisioning setup for optional benchmark lanes.

Open questions
- none

Follow-up questions
- When non-SQLite provider read optimization evidence is actually measured, should a follow-up ticket publish before-and-after artifact bundles under artifacts/benchmarks/<label>/... and update the performance-profile guidance from skipped rows to measured posture?
- Do we want later release automation or CI validation to exercise optional provider read lanes when secrets are available, rather than keeping them as local or manual evidence only?

Risks
- Execution sequencing still depends on blocking ticket 06F8KZJNZ999C8NKY0S92VBDN0; refinement is complete, but delivery ordering remains constrained by the live relation state.
- If skipped optional-provider rows are not verifier-protected, later artifact refreshes can silently drop those rows and make provider coverage look narrower than the documented contract.
- If row labels or summaries blur the difference between measured SQLite results and skipped optional-provider lanes, downstream docs or release notes may overstate provider-read optimization evidence.

Split recommendations
- No split recommended; the repository already fixes the scenario baseline, provider posture, and artifact contract, so benchmark-row extension and verifier coverage remain one bounded task.

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