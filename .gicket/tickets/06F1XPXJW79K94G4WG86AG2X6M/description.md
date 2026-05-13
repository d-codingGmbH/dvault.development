<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story as duplicate/already-satisfied backlog cleanup. The current branch already contains the latest/as-of and bridge read baseline, and no separate current-named public API is required for this ticket.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket is now duplicate/already-satisfied backlog cleanup, not a dev implementation story.
- No separate current-named public API is required in this ticket; current is accepted as product language for the existing latest-satellite helper semantics.
- No missing docs, examples, tests, or diagnostics are named for this ticket because the visible branch baseline already covers the requested latest/as-of and bridge read families.
- Existing persisted relation state is acknowledged but no longer treated as live implementation scope: child 06F1XPXY7QKTYAW43JTT3BM704 is done historical context, and blocker 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 is done historical context rather than an unresolved risk.
- No bounded child tickets, relation updates, attachments, or planning documents were materialized in this PO pass.

### Scope In
- Record the ticket as already satisfied by the current branch baseline.
- Clarify that the existing latest/as-of and bridge read surface is the accepted product answer for this story.
- Prevent duplicate implementation work from being scheduled from this ticket.

### Scope Out
- Adding new latest/current/as-of/bridge helper APIs under this ticket.
- Adding a current-named public alias under this ticket.
- Adding or rewriting README, release notes, examples, benchmarks, XML docs, or tests under this ticket.
- PIT maintenance, bridge row maintenance, provider-specific read optimizations, custom LINQ providers, or universal query translation guarantees.

## Acceptance Criteria
- The refined contract states that the original helper-surface request is already satisfied by the current branch baseline.
- The refined contract states that no separate current-named public API is required beyond the existing latest helper family.
- The refined contract no longer describes 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 as an unresolved blocker or 06F1XPXY7QKTYAW43JTT3BM704 as live implementation scope.
- No new product-code, documentation, example, or test work is requested from this ticket.

## Definition of Done
- PO-critic can review this as duplicate/already-satisfied backlog cleanup.
- Downstream dev handoff is avoided because there is no remaining implementation delta.
- Related done tickets are treated as historical context in the contract, not active blockers or active child work.
- Any future request for a current-named alias is split into a new narrow ticket with exact API, docs, examples, and tests named there.

## Implementation Notes
- Do not add a ReadCurrent... API from this ticket. If product later wants that spelling, create a separate naming/API ergonomics story.
- Use README.md, docs/releases/v0.7.0.md, examples/README.md, benchmarks/DCoding.Data.DVault.Benchmarks/README.md, and the DataVaultBridgeRead* source files as the branch evidence that the requested read families already exist.
- Treat docs/releases/v0.6.0.md as historical context only; the visible branch baseline is v0.8.0 documentation plus v0.7.0 read-flow release documentation.
- Keep outgoing blocks relations to downstream tickets as persisted relation context, but do not convert this cleanup ticket into new implementation scope.

## Open Questions
- none

## Follow-Up Questions
- If customers explicitly need current-named API spelling, create a new narrow ticket that names the exact alias, documentation touchpoints, examples, and tests, instead of reopening this already-satisfied helper-surface story.
- Board maintenance may later decide whether historical done-ticket relations should be pruned, but that cleanup does not block PO-critic review of this refined contract.

## Risks
- If this ticket is accidentally routed to development as implementation work, it may duplicate existing APIs/docs/tests and create unnecessary public API churn.
- Adding a current-named alias without a separate naming/API contract could fragment the documented latest-satellite vocabulary.

## Split Recommendations
- Do not split this ticket further. Close or retire it as already satisfied; create a separate narrow follow-up only if a future current-named public alias is explicitly required.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Provide EF Core-friendly read helper APIs for common Data Vault read cases.

## Scope In

- Design APIs for current satellite, as-of timestamp, and bridge traversal reads.
- Keep APIs composable with EF Core query patterns where practical.
- Document metadata requirements and limitations.
- Add representative tests and examples.

## Scope Out

- No hidden materialized view maintenance.
- No custom query provider.
- No promise that every helper remains fully provider-translatable in v1.

## Acceptance Criteria

- Examples compile and run in tests or examples.
- Unsupported shapes fail with clear diagnostics.
- Docs compare helpers to lower-level read pipelines.

## Implementation Notes

- Prefer explicit extension methods over broad magic.

## Open Questions

- none