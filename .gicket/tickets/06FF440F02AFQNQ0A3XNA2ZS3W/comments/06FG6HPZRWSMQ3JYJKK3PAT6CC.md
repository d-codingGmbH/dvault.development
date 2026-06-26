[gicket-bot] PO refinement contract

Summary
- Refinement ratifies the current repository baseline: dependent child key modeling stays out of the DVault public library surface for now; no planning document, attachment, description, or relation write was applied in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository guidance already treats dependent child key modeling as deferred/outside the current public claim set while the visible modeling baseline remains hubs, links, satellites, PITs, and bridges.
- This ticket should formalize a defer-now contract, not approve a new first-class dependent-child feature for the current DVault EF Core surface.

Scope In
- Define whether dependent child key modeling belongs in the current DVault EF Core public surface.
- State the accepted baseline that remains supported now: existing hub, link, satellite, PIT, and bridge concepts plus already documented repeated same-hub roles, link-parent satellites, and multi-active driving keys where those shapes are already supported.
- Document the API, migration, diagnostics, and non-goal consequences of deferring first-class dependent child key modeling.

Scope Out
- Adding a new dependent-child metadata concept, parent-reference kind, fluent builder verb, model-first schema section, runtime mapper contract, or save/read API in this ticket.
- Changing existing shipped hub/link/satellite/PIT/bridge semantics, support-bundle shapes, provider strategy behavior, or telemetry vocabularies.
- Treating any undocumented workaround or implicit projection as approved first-class dependent child modeling.

Open questions
- none

Follow-up questions
- If product direction changes later, should a future dependent-child feature be modeled as a first-class concept or as a narrowly defined extension over existing link/satellite semantics?
- If future first-class support is revisited, which boundary should own it first: <redacted> metadata, Code-First fluent APIs, or `dvault.model.v1` import/export?
- If future first-class support is revisited, should typed mappers/generators and read diagnostics participate in the first release or remain deferred behind the base metadata contract?

Risks
- Because this ticket now ratifies a defer-now decision, downstream implementation work must not assume first-class dependent-child support exists implicitly.
- The existing `blocks` relation to 06FF441DM4F4ZDTHY9ZZD9RA8R may continue to hold dependent downstream work until that ticket is aligned with this contract.

Split recommendations
- If the team later chooses to pursue first-class dependent child modeling, split it into separate follow-on tickets for contract/design, metadata and model-first schema changes, Code-First API changes, runtime translation and migration behavior, and diagnostics/tooling parity rather than reopening all of that scope in one ticket.

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