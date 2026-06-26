<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies the current repository baseline: dependent child key modeling stays out of the DVault public library surface for now; no planning document, attachment, description, or relation write was applied in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository guidance already treats dependent child key modeling as deferred/outside the current public claim set while the visible modeling baseline remains hubs, links, satellites, PITs, and bridges.
- This ticket should formalize a defer-now contract, not approve a new first-class dependent-child feature for the current DVault EF Core surface.

### Scope In
- Define whether dependent child key modeling belongs in the current DVault EF Core public surface.
- State the accepted baseline that remains supported now: existing hub, link, satellite, PIT, and bridge concepts plus already documented repeated same-hub roles, link-parent satellites, and multi-active driving keys where those shapes are already supported.
- Document the API, migration, diagnostics, and non-goal consequences of deferring first-class dependent child key modeling.

### Scope Out
- Adding a new dependent-child metadata concept, parent-reference kind, fluent builder verb, model-first schema section, runtime mapper contract, or save/read API in this ticket.
- Changing existing shipped hub/link/satellite/PIT/bridge semantics, support-bundle shapes, provider strategy behavior, or telemetry vocabularies.
- Treating any undocumented workaround or implicit projection as approved first-class dependent child modeling.

## Acceptance Criteria
- The contract explicitly records that dependent child key modeling is deferred for the current DVault library surface.
- The contract names the current supported baseline that remains in force: hubs, links, satellites, PITs, bridges, repeated same-hub participant roles, link-parent satellites, and multi-active driving keys as already documented in the repository.
- The contract states that this ticket does not add a new public API, `dvault.model.v1` token or section, metadata concept/reference kind, or support-bundle/read-diagnostics shape.
- The contract states that unsupported dependent-child shapes must fail deterministically through the existing unsupported-capability or validation boundary instead of being silently projected into existing metadata constructs.
- The contract states that no migration or provider-identifier widening is approved now; any future first-class dependent-child feature requires a separate follow-on contract for generated names, columns, keys, indexes, and migration diagnostics.

## Definition of Done
- A PO-facing contract or ticket description records the defer-now decision and the finite current baseline it preserves.
- The contract includes explicit non-goals for new metadata kinds, builder verbs, model-first schema extensions, runtime read/write behavior, and provider-specific DDL changes.
- The contract gives downstream developers enough direction to reject unsupported dependent-child requests without reopening baseline questions about hubs, links, satellites, diagnostics, or migrations.
- No blocking PO questions remain for this ticket.

## Implementation Notes
- Use the existing finite metadata surface as the source of truth: the visible concepts remain hub, link, satellite, PIT, and bridge rather than a new dependent-child concept.
- Do not add a hidden adapter, alias, or undocumented projection rule that treats dependent-child declarations as if they were normal hub/link/satellite metadata.
- If this ticket produces documentation updates, keep diagnostics guidance bounded to the existing unsupported-capability validation path and do not introduce speculative new runtime behavior.
- Downstream work blocked by this ticket should consume the defer-now decision rather than widening the public surface implicitly.

## Open Questions
- none

## Follow-Up Questions
- If product direction changes later, should a future dependent-child feature be modeled as a first-class concept or as a narrowly defined extension over existing link/satellite semantics?
- If future first-class support is revisited, which boundary should own it first: metadata-first/runtime metadata, Code-First fluent APIs, or `dvault.model.v1` import/export?
- If future first-class support is revisited, should typed mappers/generators and read diagnostics participate in the first release or remain deferred behind the base metadata contract?

## Risks
- Because this ticket now ratifies a defer-now decision, downstream implementation work must not assume first-class dependent-child support exists implicitly.
- The existing `blocks` relation to 06FF441DM4F4ZDTHY9ZZD9RA8R may continue to hold dependent downstream work until that ticket is aligned with this contract.

## Split Recommendations
- If the team later chooses to pursue first-class dependent child modeling, split it into separate follow-on tickets for contract/design, metadata and model-first schema changes, Code-First API changes, runtime translation and migration behavior, and diagnostics/tooling parity rather than reopening all of that scope in one ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Evaluate whether dependent child key modeling belongs in the DVault EF Core library surface now. Acceptance: defines accepted/deferred shapes, API impact, migration impact, diagnostics, and non-goals before implementation.