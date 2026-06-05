[gicket-bot] PO refinement contract

Summary
- Reconciled the ticket against current `develop`, confirmed the requested decision-tree documentation is already present, rewrote the ticket as a no-work-required closure candidate, removed two stale outgoing `blocks` relations, and recorded the remaining incoming relation cleanup as queued.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Reconciled against current `develop`: the requested practical examples, fallback guidance, rerun triggers, and checklist posture are already present, so the ticket is now an already-satisfied/no-work-required closure candidate rather than a developer handoff.
- critic-item-2: `answered` - Because no residual repository gap was found, the contract was rewritten around delta-based closure criteria instead of broad new doc work: verify the already-landed guidance, require any future reopen to name an exact missing section on `develop`, and clean up stale `blocks` relations.
- critic-item-3: `answered` - The unclear repository change is resolved by explicitly acknowledging that `docs/performance-profiles.md` already contains the examples, fallback branches, and rerun or stop-condition guidance that the earlier contract still requested; no duplicate developer doc diff should be commissioned.
- critic-item-4: `answered` - The Definition of Done now matches the verified branch state: the diff versus `develop` is `.gicket`-only, so no documentation diff outside `.gicket` is expected from this ticket. Two stale outgoing `blocks` relations were removed on this ticket branch, and the historical incoming `blocks` cleanup was queued for replay on the source owner branch.

Clarifications
- The authoritative ticket description was updated on 2026-06-05 to a closure/no-work-required contract in `.gicket/tickets/06F8KZRSTHAGSP6GPGFBFQGY08/description.md`.
- `docs/performance-profiles.md` already covers the decision-tree branches, fallback cases, and rerun or stop guidance that the earlier ticket text still requested.
- `docs/production-adoption-checklist.md` already contains the relevant routing and evidence-posture guidance, so no checklist delta is justified from current evidence.

Scope In
- Confirm that current `develop` already satisfies the documentation outcome this ticket targeted.
- Keep the ticket on a closure/no-work-required path unless a later review names an exact missing line-level gap on `develop`.
- Complete stale relation housekeeping that still implies unresolved documentation work.

Scope Out
- Any new documentation edits without a repository-proven missing section, example, or wording gap.
- Reopening broad example, fallback, or rerun asks that are already present in current repository docs.
- Inventing residual scope, a split, or a fresh developer handoff without evidence from current `develop`.

Open questions
- none

Follow-up questions
- If a later reviewer still wants a documentation follow-up, which exact missing line-level gap in `docs/performance-profiles.md` is absent on current `develop`, and why is it not already covered by the cited sections?

Risks
- If the queued cleanup for the historical incoming `blocks` relation is never replayed on the source ticket branch, live cross-ticket state may continue to imply a blocker that no longer exists.
- Reopening the ticket without naming a specific missing section or wording gap would recreate the same no-delta developer handoff.

Split recommendations
- No split is justified. Prefer closure/no-work-required handling; only open a new follow-up if a concrete missing documentation delta is identified on `develop`.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment