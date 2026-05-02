[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a SQLite-backed conventional EF Core comparison baseline for the documented customer profile history scenario, with automated tests as the v1 execution surface.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the documented customer profile scenario from the MVP concepts as the v1 baseline: one customer business key with profile attributes customer_name and customer_status.
- This task covers the plain EF comparison implementation only; the paired DVault implementation remains in ticket 06EXB7S6DB97GVVTS2GGZ3CCX8.
- Default execution surface is the existing tests/DCoding.Data.DVault.Tests test harness; a new sample app is not required for v1.
- The baseline should use conventional EF Core entities and normal DbContext/DbSet mapping, not DVault metadata translation or IDataVaultSaveService.

Scope In
- Implement a minimal conventional EF Core model for customer profile history on SQLite.
- Demonstrate one customer profile moving through multiple states so history is visible and comparable.
- Add repeatable automated coverage in the existing test project so the baseline can be executed in the current solution.
- Record the persisted outcomes needed for later comparison with the DVault customer profile scenario.

Scope Out
- Do not implement the DVault-backed version of the scenario in this ticket.
- Do not extend the scenario to order or link behavior; keep scope on customer identity plus profile history.
- Do not change DVault naming policies, metadata translation, hashing contracts, or explicit save service behavior.
- Do not add provider support beyond SQLite or expand into deferred capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- Do not require a new runnable examples project under examples/ for v1.

Open questions
- none

Follow-up questions
- After both comparison tasks land, should the scenario also be promoted into a runnable example under examples/ in addition to tests?
- Should a later follow-up define a shared cross-ticket assertion contract so the plain EF and DVault versions cannot drift in scenario coverage?

Risks
- Comparison value drops if this ticket and the paired DVault ticket diverge on the exact customer profile change sequence or asserted outcomes.
- A conventional EF baseline can sprawl into app-specific design if the implementation adds convenience behavior beyond the minimal comparison scenario.

Split recommendations
- If stakeholders want both automated tests and a user-facing sample runner, keep this ticket on the automated baseline and schedule the runnable example separately after the comparison pair is stable.

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