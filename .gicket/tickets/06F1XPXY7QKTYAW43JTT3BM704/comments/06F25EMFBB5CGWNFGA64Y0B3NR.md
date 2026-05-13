[gicket-bot] PO refinement contract

Summary
- Reconciled the ticket as stale/duplicate of the current repository baseline. The refined contract no longer asks dev to implement the first typed latest/as-of satellite helper slice; it asks PO-critic to verify retirement/closure as already satisfied by existing source, tests, docs, and release evidence.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket should be retired/closed as already satisfied rather than handed to dev. Current context shows the requested public typed latest/as-of satellite helper, registry-backed metadata usage, diagnostics/read-strategy evidence, tests, docs, and API/release coverage already exist, so there is no remaining implementation delta for this ticket.
- critic-item-2: `answered` - The retained delivery contract is replaced with a retirement contract. Suggested title: 'Retire duplicate first typed latest/as-of satellite read-helper ticket'. Scope In, Acceptance Criteria, and Definition of Done now name only the closure/retirement delta, not implementation of a first helper slice.
- critic-item-3: `answered` - The split recommendations are explicitly retired for this ticket because it is not continuing as an implementation ticket. PIT, bridge, and reflection-based binding remain separate future/read-flow areas only if new product work is raised independently; they are not splits needed from this now-duplicate ticket.
- critic-item-4: `answered` - The stale implementation instructions are removed from the dev contract. The contract now treats those items as repository baseline evidence: typed latest/as-of helper behavior, registry-backed path, provider read-strategy diagnostics, focused tests, documentation, and API/release coverage are already present.
- critic-item-5: `answered` - The contract no longer routes to dev, so it cannot invite duplicate or no-op implementation work. The only requested action is ticket-level retirement/closure as already satisfied, with no source changes and no scope expansion.

Clarifications
- The previous delivery contract is stale and should be superseded at ticket level.
- This ticket is a duplicate/already-satisfied implementation request against the current branch baseline, not a candidate for dev handoff.
- Suggested title: Retire duplicate first typed latest/as-of satellite read-helper ticket.
- No new source, test, docs, or API snapshot work is requested by this ticket.

Scope In
- Mark this ticket as already satisfied/duplicate of the current typed latest/as-of satellite read-helper baseline.
- Preserve repository evidence that the typed latest/as-of helper, raw latest-row escape hatch, registry-backed metadata usage, provider read-strategy diagnostics, tests, and documentation/release notes already cover the requested slice.
- Prevent dev handoff for duplicate implementation work from this ticket.

Scope Out
- No new public typed latest/as-of satellite helper implementation.
- No new registry-backed adapter implementation.
- No new diagnostics, provider strategy, test, documentation, or API snapshot work solely for this ticket.
- No PIT-backed typed read-helper work.
- No bridge traversal helper work.
- No reflection-based DTO binding or auto-mapping work.

Open questions
- none

Follow-up questions
- If a future gap is found in typed latest/as-of satellite reads, should it be raised as a new narrowly scoped defect against the specific missing behavior rather than reopening this implementation ticket?
- Should product backlog cleanup add a separate administrative convention for closing duplicate/already-satisfied automation tickets when repository evidence has overtaken the original scope?

Risks
- none

Split recommendations
- Retired for this ticket: do not split PIT-backed typed read helpers from this duplicate retirement ticket; raise independent future work only if a new PIT gap is identified.
- Retired for this ticket: do not split bridge traversal typed helpers from this duplicate retirement ticket; raise independent future work only if a new bridge gap is identified.
- Retired for this ticket: do not split reflection-based DTO binding from this duplicate retirement ticket; keep it out of scope unless a separate product decision creates a new API family.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment