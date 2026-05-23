<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Updated the delivery contract to resolve the v0.18.0 release-date blocker by authorizing the exact pending-approval placeholder and final-approval cross-reference; no child tickets, relation changes, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- No exact approved `v0.18.0` release date is visible in the current local ticket and repository context, so the release note is explicitly authorized to use `Intended release date: pending final release approval` until final approval records the date.
- `docs/manual-nuget-publication.md` remains the authoritative local source for the manual-publication boundary and for the requirement that the final release record carry a release date or intended release date.
- `docs/architecture/dvault-ef-compiled-compatibility.md` fixes the compiled-model, compiled-query, and `DbContext` pooling boundary: SQLite is the required local evidence baseline, compiled queries stay on stable direct EF shared-type-table expressions, pooled contexts require one fixed metadata/model shape, and DVault does not ship provider-specific compiled guarantees.
- The authoritative performance evidence set for this ticket remains `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, plus the checked-in bundles under `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations`, `artifacts/benchmarks/06F492CFSJHN0RGXXRG3KT63FM-explicit-save-change-tracker`, and `artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines`.
- Current-baseline user-facing docs still pointing to `v0.17.0` include at least `README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md`, so rollout work must move those pointers to `v0.18.0` while leaving earlier release notes historical.
- This refinement run applies a ticket-description contract update only. No child tickets, relation changes, attachments, or planning documents were applied or queued.

### Scope In
- Create `docs/releases/v0.18.0.md` as the coordinated seven-package DVault release record and new current public baseline.
- Roll up the landed performance work into consumer-facing documentation: compiled-model/query/pooling evidence, provider-neutral read allocation tuning, explicit-save change-tracker tuning, provider-optimization regression baselines, and the shared benchmark artifact contract.
- Update current-baseline documentation pointers in `README.md`, `docs/production-adoption-checklist.md`, `docs/model-first-governance.md`, and any other user-facing current-release surface that still identifies `v0.17.0` as latest.
- Document bounded consumer guidance for compiled models, compiled queries, and `DbContext` pooling using the existing architecture note and benchmark rows, including SQLite-only evidence scope and fixed-model pooling guardrails.
- Document bounded query-shape tuning guidance using the existing request-bound read-shape diagnostics and benchmark evidence instead of presenting DVault as a raw-SQL or automatic-index advisor.
- Summarize benchmark artifact expectations, provider notes, and evidence locations by linking existing artifact bundles and naming the relevant scenario rows instead of inventing a new evidence format.

### Scope Out
- No new benchmark harness features, no new performance tuning code, and no re-measurement of already landed scenarios.
- No new provider-specific compiled-model, compiled-query, or pooling guarantees beyond the documented SQLite baseline.
- No DVault-owned compiled-model generator, EF command wrapper, dynamic `IDataVaultReadService` compilation promise, or pooling support for caller-owned variable model shapes.
- No new diagnostics subsystem, raw-SQL advisor promise, or automatic index-creation guidance beyond the existing bounded read-shape diagnostics and artifact-backed recommendations.
- No duplication of full benchmark tables or raw SQL dumps into release-note prose; the release docs should summarize findings and point to existing artifacts.
- No ticket split or relation cleanup is required just to close historical done-ticket blocks on this documentation rollup task.

## Acceptance Criteria
- `docs/releases/v0.18.0.md` exists and records the coordinated seven-package release scope, either the approved intended release date or the exact placeholder `Intended release date: pending final release approval` plus a cross-reference to the final approval record described by `docs/manual-nuget-publication.md`, notable user-facing performance and documentation changes, compatibility notes, known limitations, validation evidence, and the manual-publication boundary already defined by `docs/manual-nuget-publication.md`.
- The `v0.18.0` release notes summarize the landed evidence for `compiled-model-startup`, `compiled-query-hub-read`, and `dbcontext-pooling-dvault-operation` using the bounded SQLite claim from `docs/architecture/dvault-ef-compiled-compatibility.md`: `UseModel(runtimeModel)` is consumer-owned runtime-model usage, compiled queries stay on stable direct EF shared-type-table expressions, and pooled contexts require one fixed metadata/model shape.
- The `v0.18.0` release notes summarize the landed provider-neutral read, explicit-save, and provider-optimization evidence using the existing artifact contract and checked-in artifact bundles, including the rule that optional PostgreSQL, SQL Server, MySQL, and Oracle rows remain visible as completed or skipped rather than silently disappearing.
- `README.md`, `docs/production-adoption-checklist.md`, `docs/model-first-governance.md`, and each other current-baseline user-facing document touched by the rollout stop describing `v0.17.0` as the current public baseline and instead point readers at `v0.18.0` while preserving earlier release notes as historical.
- The documentation explains that query-shape tuning guidance comes from the existing request-bound read-shape diagnostics surface plus bounded benchmark evidence, not from raw SQL output, automatic index creation, or provider-specific physical-plan promises.
- The documentation explicitly states that SQL capture is required only when a claim depends on SQL shape or index/materialization behavior; compiled-model/query/pooling rollout prose does not promise per-scenario SQL captures because the checked-in architecture note already limits those claims to timing and allocation evidence, and the final docs point readers to the shared artifact contract and checked-in benchmark bundles for supporting detail.

## Definition of Done
- User-facing current-release documentation consistently treats `v0.18.0` as the latest coordinated baseline and leaves earlier version notes as historical context rather than competing current guidance.
- Release-note and supporting-document prose accurately matches the checked-in repository evidence for compiled-model/query/pooling, provider-neutral read allocations, explicit-save tuning, provider-optimization baselines, and bounded query-shape guidance without inventing broader guarantees.
- Consumers can follow the docs to the existing benchmark evidence and understand the required SQLite baseline, optional external-provider behavior, and when SQL capture is or is not part of the claim.
- The release note does not invent schedule data: it either records an approved intended release date or uses the exact pending-approval placeholder and approval-record cross-reference authorized by this contract.
- No PO-level ambiguity remains about which benchmark artifacts, provider notes, query-shape guardrails, pooled-context assumptions, and release-date fallback the `v0.18.0` documentation must present.

## Implementation Notes
- Use `docs/architecture/dvault-ef-compiled-compatibility.md`, `docs/plans/performance-evidence-benchmark-artifact-contract.md`, root `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, and the checked-in artifact bundles under `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations`, `artifacts/benchmarks/06F492CFSJHN0RGXXRG3KT63FM-explicit-save-change-tracker`, and `artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines` as the authoritative local evidence set.
- Treat the existing scenario ids as fixed release-note vocabulary: `customer-profile-history`, `customer-profile-bulk-insert-only`, `customer-profile-bulk-history`, `order-product-fulfillment-history`, `latest-satellite-read`, `pit-as-of-read`, `bridge-traversal-read`, `compiled-model-startup`, `compiled-query-hub-read`, `dbcontext-pooling-dvault-operation`, and `provider-native-bulk-ingestion`.
- Summarize the compiled-model/query/pooling recommendation once and link the relevant benchmark artifact bundle or root summary rather than enumerating per-scenario SQL files; the current architecture note already says those rows do not require companion SQL captures.
- Keep provider notes aligned with the benchmark contract and current root benchmark summary: SQLite is the required local completed baseline; PostgreSQL, SQL Server, MySQL, and Oracle remain optional external-provider lanes whose rows stay visible with normalized skipped reasons when not configured.
- If no approved date is present when authoring `docs/releases/v0.18.0.md`, use the exact line `Intended release date: pending final release approval` and point readers to the final approval record required by `docs/manual-nuget-publication.md`; do not infer a calendar date from earlier release cadence or neighboring release notes.
- Limit baseline-pointer edits to true current-release surfaces; do not rewrite historical release notes or older version references whose purpose is to preserve release history.

## Open Questions
- none

## Follow-Up Questions
- After `v0.18.0` ships, should a separate ticket add configured external-provider compiled-model, compiled-query, or pooling evidence, or should those scenarios remain SQLite-only until a provider-specific need appears?
- Should a later release-ops or documentation ticket attach or cross-link a single release-approval artifact bundle for `v0.18.0`, or is referencing the checked-in repository artifact directories sufficient for the manual publication record?

## Risks
- If the rollout leaves any current-baseline surface on `v0.17.0` while other docs move to `v0.18.0`, consumers will see conflicting guidance about the latest supported release posture.
- If the release notes generalize SQLite benchmark rows into provider-neutral compiled or pooling promises, consumers may infer guarantees the repository does not measure.
- If benchmark numbers are copied without the surrounding artifact context, optional-provider skip visibility, and claim boundary, readers may misinterpret local timing deltas as universal performance guarantees.
- If documentation invents an exact release date instead of using the authorized placeholder and cross-reference until approval, the release record will diverge from approved planning evidence.
- If documentation asks for per-scenario SQL capture where the claim does not depend on SQL shape, it will diverge from the settled artifact contract and the existing compiled-compatibility note.

## Split Recommendations
- No split recommended; the performance evidence and boundary decisions are already landed in sibling done tickets, so this task should remain one documentation and release-note rollup.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Document performance evidence, benchmark artifacts, provider notes, query-shape tuning guidance, and consumer recommendations for compiled models, compiled queries, and DbContext pooling.