[gicket-bot] PO refinement contract

Summary
- Refined the v0.18.0 documentation and release-note task against checked-in repository evidence, current ticket state, and live relations; no durable planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The checked-in baseline already fixes the compiled-model/query/pooling decision boundary: docs/architecture/dvault-ef-compiled-compatibility.md documents SQLite as the required local evidence baseline, stable EF shared-type-table query shapes for EF.CompileQuery(...), fixed metadata/model-shape assumptions for AddDbContextPool<TContext>(...), and no DVault-owned compiled-model generator or provider-specific compiled guarantee.
- The repository already contains the evidence surface this task must package: root benchmark-summary.md/.csv/.json and the checked-in artifact bundles under artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations, artifacts/benchmarks/06F492CFSJHN0RGXXRG3KT63FM-explicit-save-change-tracker, and artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines.
- The shared artifact and SQL-capture rules are already settled by docs/plans/performance-evidence-benchmark-artifact-contract.md: SQLite is the required completed baseline, optional PostgreSQL/SQL Server/MySQL/Oracle rows remain visible as completed or skipped, and SQL capture is required only when a claim depends on query shape, index usage, batching behavior, or materialization behavior.
- The query-shape guidance baseline is already repository-owned through the existing request-bound read-shape diagnostics surface and its landed performance-oriented extension, so v0.18 documentation should explain how to use that bounded guidance rather than inventing a broader SQL-advisor claim.
- Current current-baseline documentation still points at v0.17.0 in at least README.md, docs/production-adoption-checklist.md, and docs/model-first-governance.md, so v0.18.0 rollout work includes updating those current-release pointers while leaving older release notes historical.
- No child tickets, relation changes, description updates, attachments, or planning documents were applied or queued in this run because the visible repository and ticket evidence already bound the release-note scope.

Scope In
- Create the coordinated docs/releases/v0.18.0.md release record for the seven-package DVault family and make it the new current public baseline.
- Roll up the landed performance work into consumer-facing documentation: compiled-model/query/pooling evidence, provider-neutral read allocation tuning, explicit-save change-tracker tuning, provider optimization regression baselines, and the shared benchmark artifact contract.
- Update current-baseline documentation pointers and versioned current-release summaries in README.md, docs/production-adoption-checklist.md, docs/model-first-governance.md, and any other user-facing current-baseline surface that still identifies v0.17.0 as latest.
- Document the bounded consumer guidance for compiled models, compiled queries, and DbContext pooling using the existing architecture note and benchmark rows, including SQLite-only evidence scope and fixed-model pooling guardrails.
- Document bounded query-shape tuning guidance using the existing request-bound read-shape diagnostics and benchmark evidence instead of presenting DVault as a raw-SQL or automatic-index advisor.
- Summarize benchmark artifact expectations, provider notes, and evidence locations by linking the existing artifact bundles and naming the relevant scenario rows instead of inventing a new evidence format.

Scope Out
- No new benchmark harness features, no new performance tuning code, and no re-measurement of already landed scenarios; this task packages already checked-in evidence.
- No new provider-specific compiled-model, compiled-query, or pooling guarantees beyond the documented SQLite baseline.
- No DVault-owned compiled-model generator, EF command wrapper, dynamic IDataVaultReadService compilation promise, or pooling support for caller-owned variable model shapes.
- No new diagnostics subsystem, no raw-SQL advisor promise, and no automatic index-creation guidance beyond the existing bounded read-shape diagnostics and artifact-backed recommendations.
- No duplication of full benchmark tables or raw SQL dumps into release-note prose; the release docs should summarize findings and point to existing artifacts.
- No ticket split or relation cleanup is required just to close historical done-ticket blocks on this still-active documentation rollup task.

Open questions
- none

Follow-up questions
- After v0.18.0 ships, should a separate ticket add configured external-provider compiled-model, compiled-query, or pooling evidence, or should those scenarios remain SQLite-only until a provider-specific need appears?
- Should a later release-ops or documentation ticket attach or cross-link a single release-approval artifact bundle for v0.18.0, or is referencing the checked-in repository artifact directories sufficient for the manual publication record?

Risks
- If the rollout leaves any current-baseline surface on v0.17.0 while other docs move to v0.18.0, consumers will see conflicting guidance about the latest supported release posture.
- If the release notes generalize SQLite benchmark rows into provider-neutral compiled or pooling promises, consumers may infer guarantees the repository does not measure.
- If benchmark numbers are copied without the surrounding artifact context, optional-provider skip visibility, and claim boundary, readers may misinterpret local timing deltas as universal performance guarantees.
- If documentation asks for per-scenario SQL capture where the claim does not depend on SQL shape, it will diverge from the settled artifact contract and the existing compiled-compatibility note.

Split recommendations
- No split recommended; the performance evidence and boundary decisions are already landed in sibling done tickets, so this task should remain one documentation and release-note rollup.

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