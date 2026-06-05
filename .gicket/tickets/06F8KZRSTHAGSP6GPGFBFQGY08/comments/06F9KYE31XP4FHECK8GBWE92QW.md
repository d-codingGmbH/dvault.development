[gicket-bot] PO refinement contract

Summary
- Verified that the repository already carries the v0.31 decision-tree contract and checklist routing baseline; this ticket remains a bounded follow-up for concrete adopter-facing examples, fallback examples, and explicit rerun or non-optimization guidance in `docs/performance-profiles.md`.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current branch already treats `docs/performance-profiles.md` as the authoritative v0.31 performance-profile guide, so this ticket should extend that guide with adopter-facing examples instead of redefining the decision tree.
- The completed upstream story `06F8KZR38EDSVZBCTC0XYR4R80` is historical contract context, not an open blocker; the live relation set still shows its incoming `blocks` edge plus the parent epic `06F8KZQNH8CCMTJW9P95W1N388` and this ticket's outgoing `blocks` edges to `06F8KZSCGZBKAC4YZH5SY3NX68` and `06F8KZSNDXXEEHF53HN14QFK14`.
- The checked-in evidence baseline already fixes the v1 defaults: SQLite is the only repository-proven optimized latest-satellite provider path, while PostgreSQL, SQL Server, MySQL, and Oracle stay diagnostics-gated PIT or bridge and staged-ingestion candidate lanes with optional benchmark rows currently visible as skipped.
- The repository already contains a Performance Profiles routing bullet and detailed evidence bullets in `docs/production-adoption-checklist.md`, so checklist work is optional and should stay limited to a short discoverability pointer only if the final doc wording reveals a real navigation gap.

Scope In
- Add concrete adopter-facing save-path examples in `docs/performance-profiles.md` for a small materialized batch, the bounded chunked-ingestion starting shape already visible in the benchmark rows, an already-asynchronous chunk source over the same bounded save contract, and a diagnostics-gated staged provider-ingestion decision.
- Add concrete adopter-facing read-path examples in `docs/performance-profiles.md` for SQLite latest-satellite reads, explicitly maintained PIT as-of reads, and explicitly maintained bridge traversal reads, including the required maintenance-freshness and `ReadShape` evidence checks before claiming optimization.
- Add explicit fallback examples that show when adopters stay on provider-neutral `AddDVault()` and `IDataVaultSaveService` or `IDataVaultReadService` behavior because diagnostics decline, evidence is incomplete, maintenance is stale, or optional-provider benchmark rows are skipped.
- Add an explicit `when not to optimize` or equivalent summary that tells adopters to keep the default provider-neutral path when the workload is small, memory is acceptable, the request shape is unsupported, or repository evidence does not justify a stronger claim.
- Keep any checklist change bounded to one short non-duplicative pointer to the final decision tree only if the current checklist wording is not enough to route adopters there.

Scope Out
- New APIs, new benchmarks, benchmark reruns, generated SQL or stored-procedure artifacts, automatic PIT or bridge maintenance, dashboards, exporters, or implementation or code changes.
- Rewriting the v0.31 contract section, README, release notes, or the full production checklist instead of building concrete examples on top of the current contract.
- Any new provider claim that goes beyond the current checked-in evidence posture for SQLite latest-satellite reads or diagnostics-gated optional-provider PIT or bridge and staged-ingestion candidate paths.

Open questions
- none

Follow-up questions
- When the later release-doc or README coordination work runs, should the public-baseline references that still point at earlier release notes be advanced to the final release record that carries this v0.31 documentation slice?

Risks
- If the examples merely paraphrase the contract instead of showing adopter decision points and fallback cases, the ticket will not materially improve the practical guidance gap this follow-up is supposed to cover.
- If non-SQLite latest-satellite reads or optional-provider PIT or bridge rows are presented as measured wins instead of diagnostics-gated or skipped evidence, the docs will overclaim beyond the repository baseline.
- If the guide omits maintenance freshness, `ReadShape`, or diagnostic fallback examples, adopters may treat PIT or bridge optimization as automatic rather than caller-owned and evidence-gated.
- If the checklist is expanded beyond a short pointer, it risks becoming a second source of truth that duplicates `docs/performance-profiles.md`.

Split recommendations
- No additional split is justified. Keep this ticket as the bounded practical-example follow-up under epic `06F8KZQNH8CCMTJW9P95W1N388` and leave release-note or README coordination to the existing downstream release-doc work.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment