[gicket-bot] PO refinement contract

Summary
- Refined the PIT documentation ticket around the existing SQLite/public-API baseline: document when PIT tables are useful, state that PIT generation is still deferred in v0.5, and anchor the example to the existing customer profile history scenario instead of introducing new runtime PIT features.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- For this ticket, 'PIT support' means documentation of PIT use cases and the current v0.5 support boundary, not implementation of PIT table generation or a PIT-specific runtime API.
- The bounded example baseline is the existing SQLite-oriented customer profile history scenario already fixed in docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md and covered by current integration tests.
- The example should use the current public path only: AddDVault(), ApplyDataVaultMetadata(), IDataVaultSaveService, and EF queries over generated shared-type tables; it may show a manual/as-of read pattern but must not invent a PIT-specific API surface.
- The documentation should explicitly ratify the current architecture decision that hub, link, and satellite modeling is the v0.5 baseline and PIT tables remain an opt-in deferred capability family.

Scope In
- Add README and/or docs guidance explaining when PIT tables are useful in Data Vault terms, especially for repeated time-sliced reads and simplifying joins across historical satellite data.
- Document the current v0.5 boundary: DVault supports hub, link, and satellite modeling plus explicit save/query flows, but does not generate or manage PIT tables yet.
- Provide one minimal runnable example scenario based on the existing SQLite customer profile history flow that demonstrates the current public-API path and a PIT-adjacent as-of read/query explanation.
- Make limitations and future work explicit, including refresh semantics, temporal grain, persisted-versus-computed shape, and late-arriving data handling as deferred decisions.

Scope Out
- Implementing PIT table generation, PIT refresh jobs, PIT metadata/model types, or PIT-specific save/query APIs.
- Changing AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), IDataVaultSaveService, or provider capability profiles to add PIT behavior in this ticket.
- Adding provider-specific PIT SQL, migrations, indexing strategy, or cross-provider optimization commitments.
- Creating a new standalone examples application or broader example infrastructure beyond the current docs-plus-existing-test baseline.

Open questions
- none

Follow-up questions
- When the dedicated PIT implementation story resumes, should the first supported shape be persisted PIT tables, computed query helpers, or both?
- What PIT refresh and late-arriving-data semantics should be standardized first, and do those commitments stay provider-neutral or become provider-specific?
- Once the repository activates the examples/ surface, should this documentation scenario graduate into a dedicated runnable sample project?

Risks
- The main documentation risk is overstating support and accidentally promising generated PIT behavior that the deferred capability record explicitly leaves open.
- If the example depends on provider-specific SQL or a new example project, the ticket will drift beyond the current SQLite/local-validation baseline and broaden scope unnecessarily.
- If the docs introduce a brand-new business scenario instead of reusing the shared customer profile contract, the repository can accumulate scenario drift between docs, tests, and benchmark narratives.

Split recommendations
- If work expands from documentation into PIT runtime behavior, move generated tables, refresh semantics, and API design into the dedicated PIT story 06EZ0NSXY2Y1JZ8SSCX177C770.
- If the team wants a standalone runnable sample rather than a docs-backed example, split that into a separate example-infrastructure ticket so this task stays documentation-only.

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