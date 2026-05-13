[gicket-bot] PO refinement contract

Summary
- Refined the story as duplicate/already-satisfied backlog cleanup. The current branch already contains the latest/as-of and bridge read baseline, and no separate current-named public API is required for this ticket.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket is re-scoped as already satisfied by the current branch baseline. It should not re-request helper APIs, docs, examples, or diagnostics already present; the remaining action is backlog cleanup/closure.
- critic-item-2: `answered` - The stale narrative is corrected. Ticket 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 is treated as a completed historical predecessor, not an unresolved blocker, and child 06F1XPXY7QKTYAW43JTT3BM704 is treated as done historical implementation context, not live scope.
- critic-item-3: `answered` - No separate current-named public API is required beyond the existing latest helper family such as ReadLatestSatelliteAsync(...). Current remains product vocabulary for latest-satellite semantics. There are no exact missing docs, examples, or tests for this ticket; any future current-named alias must be a new narrow naming/API ergonomics ticket.
- critic-item-4: `answered` - The stale delivery contract is replaced with a cleanup contract. Source, README, release notes, examples, benchmarks, and bridge read source evidence already cover the requested helper surface, so this ticket must not ask developers to add the same baseline again.
- critic-item-5: `answered` - The live-planning narrative is corrected so completed related tickets are historical routing context only. The incoming done blocker is not a current delivery risk, and the done child is not remaining implementation scope.
- critic-item-6: `answered` - The remaining delta is not an implementation gap. This ticket is duplicate/retirement work, and PO explicitly declines a separate current-named public API in this story.

Clarifications
- This ticket is now duplicate/already-satisfied backlog cleanup, not a dev implementation story.
- No separate current-named public API is required in this ticket; current is accepted as product language for the existing latest-satellite helper semantics.
- No missing docs, examples, tests, or diagnostics are named for this ticket because the visible branch baseline already covers the requested latest/as-of and bridge read families.
- Existing persisted relation state is acknowledged but no longer treated as live implementation scope: child 06F1XPXY7QKTYAW43JTT3BM704 is done historical context, and blocker 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 is done historical context rather than an unresolved risk.
- No bounded child tickets, relation updates, attachments, or planning documents were materialized in this PO pass.

Scope In
- Record the ticket as already satisfied by the current branch baseline.
- Clarify that the existing latest/as-of and bridge read surface is the accepted product answer for this story.
- Prevent duplicate implementation work from being scheduled from this ticket.

Scope Out
- Adding new latest/current/as-of/bridge helper APIs under this ticket.
- Adding a current-named public alias under this ticket.
- Adding or rewriting README, release notes, examples, benchmarks, XML docs, or tests under this ticket.
- PIT maintenance, bridge row maintenance, provider-specific read optimizations, custom LINQ providers, or universal query translation guarantees.

Open questions
- none

Follow-up questions
- If customers explicitly need current-named API spelling, create a new narrow ticket that names the exact alias, documentation touchpoints, examples, and tests, instead of reopening this already-satisfied helper-surface story.
- Board maintenance may later decide whether historical done-ticket relations should be pruned, but that cleanup does not block PO-critic review of this refined contract.

Risks
- If this ticket is accidentally routed to development as implementation work, it may duplicate existing APIs/docs/tests and create unnecessary public API churn.
- Adding a current-named alias without a separate naming/API contract could fragment the documented latest-satellite vocabulary.

Split recommendations
- Do not split this ticket further. Close or retire it as already satisfied; create a separate narrow follow-up only if a future current-named public alias is explicitly required.

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