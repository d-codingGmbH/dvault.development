<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence matches the intended epic outputs: docs/performance-profiles.md is the authoritative decision-tree contract, docs/releases/v0.31.0.md exists, examples/README.md carries the quickstart and observability examples, and README.md plus docs/production-adoption-checklist.md point to v0.31.0 as the current baseline.
- All planned children are done: 06F8KZR38EDSVZBCTC0XYR4R80, 06F8KZRSTHAGSP6GPGFBFQGY08, 06F8KZSCGZBKAC4YZH5SY3NX68, 06F8KZSNDXXEEHF53HN14QFK14, and 06F8KZSYCVZ21MS983501BZG18.
- Live relation files still show one stale incoming blocks edge 06F8KZSYCVZ21MS983501BZG18 -> 06F8KZQNH8CCMTJW9P95W1N388; its removal was already queued as outbox mutation-cbf0c2b6c804940f for replay on the child ticket owner branch.
- No new child tickets, planning documents, attachments, or ticket-description updates were materialized in this refinement.

### Scope In
- Track the completed v0.31.0 docs-and-examples release across decision guidance, observability posture, realistic EF Core quickstart evidence, and release-note/navigation consistency.
- Verify that the epic's repository-visible outputs exist and remain bounded to documentation and examples.
- Treat completed child work and relation cleanup as the remaining coordination surface for the epic.

### Scope Out
- New runtime behavior, benchmark reruns, provider-specific SQL artifact workflow, or new documentation scope beyond the landed v0.31.0 surfaces.
- Dashboards, exporters, collectors, hosting, ingestion orchestration, automatic PIT or bridge maintenance, or runtime routing changes.
- Additional child-ticket splits for this epic unless new scope is introduced later.

## Acceptance Criteria
- The epic keeps exactly the five existing child tickets and each is completed or explicitly closed with evidence.
- The repository contains the coordinated v0.31.0 release surfaces: docs/performance-profiles.md, docs/releases/v0.31.0.md, examples/README.md, README.md, and docs/production-adoption-checklist.md aligned around the same baseline and non-goals.
- The documented v0.31.0 story consistently points adopters to the benchmark triplet, decision-tree guidance, quickstart evidence, and observability contract without adding package-publication or runtime-behavior claims.

## Definition of Done
- All five child tickets remain done and no additional child scope is needed for the epic.
- The repository-visible v0.31.0 documentation baseline remains present and internally consistent.
- The stale 06F8KZSYCVZ21MS983501BZG18 -> 06F8KZQNH8CCMTJW9P95W1N388 blocks edge is either applied away or accounted for by the queued owner-branch replay.
- No product-code changes, benchmark artifact rewrites, attachments, or planning documents are required for this epic refinement.

## Implementation Notes
- Repository evidence now matches the epic's done criteria: docs/releases/v0.31.0.md states the v0.31.0 boundary, README.md names it the current coordinated baseline, docs/production-adoption-checklist.md names it the current public baseline, and examples/README.md keeps observability and quickstart examples bounded and sanitized.
- The previously executed remove-relation mutation for relation id 06F8KZSYCVZ21MS983501BZG18--06F8KZQNH8CCMTJW9P95W1N388--blocks was not applied on this branch because branch ownership belongs to the child ticket; it was successfully queued for replay on ticket/06F8KZSYCVZ21MS983501BZG18-task-update-v0-31-0-performance-guidance-release.
- No further split is justified; this epic is now a tracking parent whose remaining work is completion and cleanup confirmation rather than new planning.

## Open Questions
- none

## Follow-Up Questions
- none

## Risks
- Later documentation edits could reintroduce baseline drift or broaden v0.31.0 claims beyond the explicit non-goals if they stop linking back to the authoritative docs.

## Split Recommendations
- No further split recommended; the epic is already decomposed into five completed child tickets.
- Any future work on relation-graph hygiene or post-v0.31 release governance should be tracked as separate maintenance tickets rather than reopening this epic.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Plan and coordinate v0.31.0 as a documentation-and-example release for performance decision guidance and observability adoption.

Release intent
- Help adopters choose between existing DVault save/read profiles, diagnostics, tracing, metrics, and examples without adding dashboards, hosted observability, ingestion orchestration, background maintenance, or platform behavior.
- Keep the release focused on repository-visible documentation and examples. Runtime behavior changes belong in separate tickets and are out of scope for this epic.

Expected child flow
- First define the authoritative decision-tree contract in `docs/performance-profiles.md`.
- Then add practical decision-tree documentation and fallback examples based on that contract.
- Then add bounded observability and realistic EF Core examples that link back to the decision-tree guidance.
- Finally update release documentation and top-level navigation after the child outputs exist.

Done for the epic
- All v0.31.0 child tickets are completed or explicitly closed as not needed with evidence.
- `docs/releases/v0.31.0.md` exists and links to the final guidance and examples.
- The final documentation consistently states the non-goals: no dashboards, exporters, collectors, hosting, scheduling, automatic PIT/bridge maintenance, provider-specific SQL artifact workflow, or runtime routing beyond existing diagnostics-gated behavior.