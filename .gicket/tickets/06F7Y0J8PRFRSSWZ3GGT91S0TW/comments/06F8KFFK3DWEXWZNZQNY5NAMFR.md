[gicket-bot] PO refinement contract

Summary
- Epic scope is already decomposed into seven persisted child tickets and matches the checked-in v0.26.0 documentation boundary for provider diagnostics, benchmark evidence, migration/idempotency guardrails, and the stored-procedure artifact boundary; no new bounded writes were needed in this refinement pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- No new child tickets, relation edits, description updates, attachments, or planning documents were materialized because the existing split and repository evidence already bound the scope tightly enough for PO-critic review.
- Treat the checked-in v0.26.0 documentation set as the authoritative baseline for this epic rather than reopening naming or boundary decisions already ratified in `README.md`, `docs/performance-profiles.md`, and `docs/releases/v0.26.0.md`.

Scope In
- Provider performance explainability through the existing documentation and diagnostics boundary: benchmark artifact verifier evidence, bounded provider-eligibility facts, and performance-profile guidance.
- Schema-safety guidance through documented migration guardrails and idempotency preflight expectations, not automated repair or deployment flows.
- The documented high-performance artifact boundary for future stored-procedure or provider-specific SQL proposals, including the explicit decision gate that keeps those artifacts outside the default runtime path.
- Epic-level coordination across the already materialized child-ticket split until the linked slices are completed and closure evidence is coherent.

Scope Out
- Automatic stored-procedure deployment, default stored-procedure runtime execution, or provider-specific SQL artifact generation.
- DBA workflow automation, dashboards, hosted observability, platform schedulers, or background-work orchestration.
- New runtime behavior, new diagnostics payload fields, new benchmark artifact schema, new benchmark rows, or package-publication/release automation claims under this epic.
- Non-SQLite optimized latest-satellite, PIT, or bridge read performance claims without new benchmark evidence.

Open questions
- none

Follow-up questions
- When the linked implementation/documentation tickets finish, should epic closure also remove the two remaining incoming `blocks` relations if they are no longer semantically needed?
- After v0.26.0, is there a separate backlog need to gather repository-backed optimized read evidence for non-SQLite providers, or should those claims remain intentionally deferred?

Risks
- The benchmark artifacts are observational, not SLA commitments; if later closure evidence drops the documented run-context caveats, adopters may overread the timing numbers.
- The live relation graph still shows two incoming blockers, so epic closure could stall operationally even though no additional PO clarification is required.

Split recommendations
- No further split recommended. The epic already has seven persisted child tickets and should remain a coordination/closure parent.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment