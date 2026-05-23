[gicket-bot] PO refinement contract

Summary
- Updated the delivery contract to resolve the v0.18.0 release-date blocker by authorizing the exact pending-approval placeholder and final-approval cross-reference; no child tickets, relation changes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - No authoritative local artifact in the visible ticket and repository context supplies an exact intended v0.18.0 release date. The delivery contract now records that absence explicitly and authorizes the release note to use the exact placeholder `Intended release date: pending final release approval` plus a cross-reference to the final approval record until an approver supplies the date.
- critic-item-2: `answered` - The acceptance contract is now relaxed so the developer does not need to guess a date. `docs/releases/v0.18.0.md` may either record an approved intended release date or use the exact placeholder `Intended release date: pending final release approval` with a cross-reference to the final approval record described by `docs/manual-nuget-publication.md`.
- critic-item-3: `answered` - The blocking gap is removed because the persisted contract no longer requires developers to invent an intended date. The release-note acceptance criterion is now satisfied by either an approved exact date or the authorized pending-approval placeholder plus final-approval cross-reference.

Clarifications
- No exact approved `v0.18.0` release date is visible in the current local ticket and repository context, so the release note is explicitly authorized to use `Intended release date: pending final release approval` until final approval records the date.
- `docs/manual-nuget-publication.md` remains the authoritative local source for the manual-publication boundary and for the requirement that the final release record carry a release date or intended release date.
- `docs/architecture/dvault-ef-compiled-compatibility.md` fixes the compiled-model, compiled-query, and `DbContext` pooling boundary: SQLite is the required local evidence baseline, compiled queries stay on stable direct EF shared-type-table expressions, pooled contexts require one fixed metadata/model shape, and DVault does not ship provider-specific compiled guarantees.
- The authoritative performance evidence set for this ticket remains `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, plus the checked-in bundles under `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations`, `artifacts/benchmarks/06F492CFSJHN0RGXXRG3KT63FM-explicit-save-change-tracker`, and `artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines`.
- Current-baseline user-facing docs still pointing to `v0.17.0` include at least `README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md`, so rollout work must move those pointers to `v0.18.0` while leaving earlier release notes historical.
- This refinement run applies a ticket-description contract update only. No child tickets, relation changes, attachments, or planning documents were applied or queued.

Scope In
- Create `docs/releases/v0.18.0.md` as the coordinated seven-package DVault release record and new current public baseline.
- Roll up the landed performance work into consumer-facing documentation: compiled-model/query/pooling evidence, provider-neutral read allocation tuning, explicit-save change-tracker tuning, provider-optimization regression baselines, and the shared benchmark artifact contract.
- Update current-baseline documentation pointers in `README.md`, `docs/production-adoption-checklist.md`, `docs/model-first-governance.md`, and any other user-facing current-release surface that still identifies `v0.17.0` as latest.
- Document bounded consumer guidance for compiled models, compiled queries, and `DbContext` pooling using the existing architecture note and benchmark rows, including SQLite-only evidence scope and fixed-model pooling guardrails.
- Document bounded query-shape tuning guidance using the existing request-bound read-shape diagnostics and benchmark evidence instead of presenting DVault as a raw-SQL or automatic-index advisor.
- Summarize benchmark artifact expectations, provider notes, and evidence locations by linking existing artifact bundles and naming the relevant scenario rows instead of inventing a new evidence format.

Scope Out
- No new benchmark harness features, no new performance tuning code, and no re-measurement of already landed scenarios.
- No new provider-specific compiled-model, compiled-query, or pooling guarantees beyond the documented SQLite baseline.
- No DVault-owned compiled-model generator, EF command wrapper, dynamic `IDataVaultReadService` compilation promise, or pooling support for caller-owned variable model shapes.
- No new diagnostics subsystem, raw-SQL advisor promise, or automatic index-creation guidance beyond the existing bounded read-shape diagnostics and artifact-backed recommendations.
- No duplication of full benchmark tables or raw SQL dumps into release-note prose; the release docs should summarize findings and point to existing artifacts.
- No ticket split or relation cleanup is required just to close historical done-ticket blocks on this documentation rollup task.

Open questions
- none

Follow-up questions
- After `v0.18.0` ships, should a separate ticket add configured external-provider compiled-model, compiled-query, or pooling evidence, or should those scenarios remain SQLite-only until a provider-specific need appears?
- Should a later release-ops or documentation ticket attach or cross-link a single release-approval artifact bundle for `v0.18.0`, or is referencing the checked-in repository artifact directories sufficient for the manual publication record?

Risks
- If the rollout leaves any current-baseline surface on `v0.17.0` while other docs move to `v0.18.0`, consumers will see conflicting guidance about the latest supported release posture.
- If the release notes generalize SQLite benchmark rows into provider-neutral compiled or pooling promises, consumers may infer guarantees the repository does not measure.
- If benchmark numbers are copied without the surrounding artifact context, optional-provider skip visibility, and claim boundary, readers may misinterpret local timing deltas as universal performance guarantees.
- If documentation invents an exact release date instead of using the authorized placeholder and cross-reference until approval, the release record will diverge from approved planning evidence.
- If documentation asks for per-scenario SQL capture where the claim does not depend on SQL shape, it will diverge from the settled artifact contract and the existing compiled-compatibility note.

Split recommendations
- No split recommended; the performance evidence and boundary decisions are already landed in sibling done tickets, so this task should remain one documentation and release-note rollup.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment