[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence matches the intended epic outputs: docs/performance-profiles.md is the authoritative decision-tree contract, docs/releases/v0.31.0.md exists, examples/README.md carries the quickstart and observability examples, and README.md plus docs/production-adoption-checklist.md point to v0.31.0 as the current baseline.
- All planned children are done: 06F8KZR38EDSVZBCTC0XYR4R80, 06F8KZRSTHAGSP6GPGFBFQGY08, 06F8KZSCGZBKAC4YZH5SY3NX68, 06F8KZSNDXXEEHF53HN14QFK14, and 06F8KZSYCVZ21MS983501BZG18.
- Live relation files still show one stale incoming blocks edge 06F8KZSYCVZ21MS983501BZG18 -> 06F8KZQNH8CCMTJW9P95W1N388; its removal was already queued as outbox mutation-cbf0c2b6c804940f for replay on the child ticket owner branch.
- No new child tickets, planning documents, attachments, or ticket-description updates were materialized in this refinement.

Scope In
- Track the completed v0.31.0 docs-and-examples release across decision guidance, observability posture, realistic EF Core quickstart evidence, and release-note/navigation consistency.
- Verify that the epic's repository-visible outputs exist and remain bounded to documentation and examples.
- Treat completed child work and relation cleanup as the remaining coordination surface for the epic.

Scope Out
- New runtime behavior, benchmark reruns, provider-specific SQL artifact workflow, or new documentation scope beyond the landed v0.31.0 surfaces.
- Dashboards, exporters, collectors, hosting, ingestion orchestration, automatic PIT or bridge maintenance, or runtime routing changes.
- Additional child-ticket splits for this epic unless new scope is introduced later.

Open questions
- none

Follow-up questions
- none

Risks
- Later documentation edits could reintroduce baseline drift or broaden v0.31.0 claims beyond the explicit non-goals if they stop linking back to the authoritative docs.

Split recommendations
- No further split recommended; the epic is already decomposed into five completed child tickets.
- Any future work on relation-graph hygiene or post-v0.31 release governance should be tracked as separate maintenance tickets rather than reopening this epic.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 4
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment