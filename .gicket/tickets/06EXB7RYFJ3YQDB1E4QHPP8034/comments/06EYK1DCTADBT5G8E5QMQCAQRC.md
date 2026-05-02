[gicket-bot] PO refinement contract

Summary
- Materialized `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md`, fixed the exact two-event customer-profile sequence and plain-EF stored-row assertions, and documented the shared contract with paired ticket `06EXB7S6DB97GVVTS2GGZ3CCX8`.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket now fixes one concrete two-event history sequence for customer `C-100`: `<redacted>-29T10:15:00Z` / `crm-import` / `Alice Adams` / `prospect`, then `<redacted>-29T11:30:00Z` / `crm-change` / `Alice Baker` / `active`, and it defines the exact two plain-EF persisted history rows that must be asserted.
- critic-item-2: `answered` - This run materialized one governed shared comparison contract at `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md`. It lists both tickets and fixes the same event sequence and persisted-outcome expectations they must follow, which is the shared-artifact answer to the cross-ticket drift concern.
- critic-item-3: `answered` - The comparison baseline is clarified as an exact persisted outcome contract. For the plain EF ticket, developers must assert the complete stored result after both events, not only that the history is generally understandable.
- critic-item-4: `answered` - The missing second state is now fixed explicitly: event 2 changes the same business key `C-100` to `Alice Baker` / `active` at `<redacted>-29T11:30:00Z`, and the plain EF contract enumerates the two stored rows including timestamps and record sources.
- critic-item-5: `answered` - Cross-ticket drift is now constrained by the shared comparison contract, which defines both the plain EF two-row outcome and the DVault one-hub/two-satellite outcome for the same two events.

Clarifications
- `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md` is now the authoritative shared comparison artifact for this ticket and paired ticket `06EXB7S6DB97GVVTS2GGZ3CCX8`.
- Use one customer business key `C-100` with profile attributes `customer_name` and `customer_status`.
- Event 1 is the initial state: `<redacted>-29T10:15:00Z`, `crm-import`, `customer_name = Alice Adams`, `customer_status = prospect`.
- Event 2 is the changed state: `<redacted>-29T11:30:00Z`, `crm-change`, `customer_name = Alice Baker`, `customer_status = active`.
- For the plain EF baseline, the comparison target is an exact stored-outcome contract: exactly two history rows ordered ascending by the persisted history timestamp, with no extra unchanged replay row.

Scope In
- Implement a minimal conventional EF Core customer profile history baseline on SQLite using ordinary entities and a regular `DbContext`/`DbSet` model.
- Execute the exact two-event `C-100` scenario from the shared comparison contract.
- Add repeatable automated coverage in `tests/DCoding.Data.DVault.Tests` so the baseline runs in the current solution layout.
- Assert and document the exact persisted history rows needed for comparison with the paired DVault ticket.

Scope Out
- Do not implement the DVault-backed version of the scenario in this ticket.
- Do not extend the scenario to order, link, or broader relationship behavior.
- Do not add provider support beyond SQLite or expand into deferred capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- Do not require a new runnable sample app under `examples/` for this v1 baseline.
- Do not widen the scenario beyond the locked two-event customer-profile contract for this ticket.

Open questions
- none

Follow-up questions
- After both comparison tickets land, should the scenario also be promoted into a runnable example under `examples/`?
- Once both baselines exist, should a follow-up add shared assertion helpers so cross-ticket comparison stays synchronized in code as well as documentation?

Risks
- Comparison value drops if the plain EF implementation inserts convenience rows or models more than the exact two-event shared baseline.
- If the paired DVault ticket diverges from `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md`, later side-by-side evaluation will lose fidelity.

Split recommendations
- Keep any runnable example or broader demo separate; this ticket should stay focused on the automated plain EF baseline and the locked comparison contract.
- If stakeholders later want additional change-history variants or replay/deduplication cases, schedule them as separate follow-up tickets instead of widening this v1 baseline.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment