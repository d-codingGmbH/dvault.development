[gicket-bot] PO refinement contract

Summary
- Refined the story as an additive diagnostics and support-bundle contract expansion over the existing provider capability and strategy explainability baseline; no ticket mutations were needed because the current epic and blocker relations already match the scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Existing repository evidence already defines the baseline: DataVaultDiagnosticsResult exposes capability profile name, provider behavior profile name, selected strategy status and name, candidate ordering, and finite fallback-cause enums and messages; this story expands that bounded explain surface instead of creating a second classification system.
- Current live relations already place this story under epic 06F492A3MPSGP3KXDNZECN01QM and as a blocker for 06F492BG6BZYYFMBE5WK7CB024, 06F492B9PR036PDNN52S06S9BC, and 06F492BNDPWS9P4EDSV0W7G6VM, so this ticket should own the reusable diagnostics contract those downstream stories consume.
- The current provider-behavior baseline remains provider-neutral-v1; refinement should expose current provider capability and dispatch rules without inventing new provider-behavior implementations.
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this pass because the existing persisted structure already matches the refined scope.

Scope In
- Add additive machine-readable explanation fields on diagnostics and support-bundle surfaces for selected provider capability profile details: profile name and defaulted state, provider name, load-timestamp and snapshot-reference storage and value formats, relevant type-mapping facts, identifier-length behavior, included-index fallback behavior, and declared SQL-function and concurrency posture.
- Add additive machine-readable explanation fields for provider-specific save and read strategy eligibility and fallback behavior, including candidate strategy name, priority, evaluation order, supported provider names, and bounded gate reasons and thresholds already enforced by the implementation.
- Update concise human-readable diagnostics rendering only as needed so ToDisplayString surfaces the expanded provider and strategy explanation without raw SQL or unbounded internal detail.
- Add tests and snapshots for diagnostics analysis and support-bundle serialization that cover selected-strategy and provider-neutral fallback scenarios across the request families already supported by the repository baseline.

Scope Out
- Changing actual runtime strategy-selection behavior, save behavior, or read behavior.
- Query-shape diagnostics, preflight command aggregation, and documentation or release-note rollout, which remain in the already-linked blocked tickets.
- Raw SQL text, exception payloads, connection details, or other unredacted or high-cardinality support-bundle output.
- New provider-behavior profiles or benchmark-driven recommendation engines beyond the current declared capability and gate baseline.

Open questions
- none

Follow-up questions
- Should a later story add consumer-facing recommendation text layered on top of the bounded machine-readable facts, or is structured diagnostics plus documentation sufficient?
- After this lands, should the preflight and support-bundle workflow accept representative request fixtures by convention so save and read strategy explain sections are easier to populate in CI evidence?
- Do future provider-behavior profiles need to become richer than the current provider-neutral-v1 name-only baseline, or should provider behavior stay implicit until a concrete hook requires more structure?

Risks
- Because support-bundle JSON already ships the diagnostics sections, expanding explain output is a contract-sensitive change that needs additive-only evolution and deterministic ordering.
- If provider capability or gate descriptions are duplicated instead of derived from existing profiles and evaluators, the explain output can drift from actual dispatch behavior.
- This ticket remains a prerequisite for 06F492BG6BZYYFMBE5WK7CB024, 06F492B9PR036PDNN52S06S9BC, and 06F492BNDPWS9P4EDSV0W7G6VM; underspecified output here would force those stories to infer provider behavior from source code again.

Split recommendations
- No additional split is required at PO refinement time; keep this story focused on the reusable diagnostics and support-bundle contract, while downstream consumption remains in 06F492BG6BZYYFMBE5WK7CB024, 06F492B9PR036PDNN52S06S9BC, and 06F492BNDPWS9P4EDSV0W7G6VM.

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