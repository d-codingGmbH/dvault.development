<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Restated the story against prompt-visible branch evidence, removed unsupported inferred builder/type claims, kept the work as one bounded Code-First story, and made no child-ticket, relation, or planning-document changes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current prompt-visible branch evidence is explicit: public DataVaultCodeFirstLinkBuilder currently exposes only Participant<TEntity>(), so link-parent satellite declaration is net-new additive API work.
- The established fluent satellite pattern in docs/plans/fluent-code-first-api-contract.md is a Satellite(...) entry point with nested Payload(...) and optional DrivingKey(...); this story extends that behavior to link-parent satellites without reopening unrelated link-shape work.
- The contract intentionally avoids assuming any already-existing public satellite builder type or hub helper beyond what is visible in the prompt-backed branch snapshot.
- No child tickets, relation edits, or planning documents were materialized in this run; previous tool result tc2 already shows documentation follow-through remains tracked separately by the existing blocks relation to ticket 06F2PGM9038RXVJH0RJFYEJEV0.

### Scope In
- Add an additive generic link-level Satellite<TSatellite>(string satelliteName, configure) fluent entry point on DataVaultCodeFirstLinkBuilder so callers can declare payload members and optional driving keys for a link-parent satellite.
- Preserve current Link(...) overload behavior and Participant<TEntity>() declaration semantics for existing callers.
- Extend internal Code-First link declaration and metadata projection so ordered link participants and link-parent satellite declarations survive into the projected metadata model with the resolved link as the satellite parent.
- Add regression tests covering fluent link-satellite declaration, metadata projection, and at least one downstream artifact, schema, or projection surface.

### Scope Out
- Assuming an existing product CLR type named State or any other repository-owned sample entity.
- Participant aliases or roles, repeated same-hub participant support, recursive same-hub link modeling, same-as links, effectivity satellites, PIT changes, or bridge changes.
- Save pipeline refactors, source-generator or compile-time mapping expansion, or README and release-note authoring already tracked on ticket 06F2PGM9038RXVJH0RJFYEJEV0.

## Acceptance Criteria
- DataVaultCodeFirstLinkBuilder gains an additive generic Satellite<TSatellite>(...) fluent entry point, and existing Link(...) overloads plus Participant<TEntity>() behavior remain unchanged for current callers.
- The link-level satellite configure callback supports the established Code-First satellite verbs needed by this story: payload member declaration and optional driving-key declaration in caller order.
- Code-First metadata projection carries link-parent satellite declarations alongside ordered link participants and emits each link-parent satellite with the resolved link as parent.
- Regression tests show a caller-owned CLR type can declare a link-parent satellite and that the declaration reaches metadata projection plus at least one downstream contract surface.

## Definition of Done
- A developer can declare a link-parent satellite from the existing link builder without regressing current participant declaration behavior.
- Projected metadata includes link-parent satellite output with preserved caller order for payload and optional driving-key declarations.
- Automated tests cover API shape, metadata translation, and one downstream output path for link-parent satellites.
- Documentation and release-note follow-through stays on ticket 06F2PGM9038RXVJH0RJFYEJEV0.

## Implementation Notes
- Keep source-backed branch evidence front and center: DataVaultCodeFirstLinkBuilder currently only appends participant CLR types through Participant<TEntity>(), so the new satellite declaration must be additive to that public surface.
- Follow the fluent naming and verb pattern already documented in docs/plans/fluent-code-first-api-contract.md: link-parent satellites should use a Satellite(...) entry point with nested Payload(...) and optional DrivingKey(...) configuration semantics.
- Do not require a specific pre-existing builder type name in the contract. If current branch source already contains reusable satellite-declaration infrastructure, implementation may reuse it; otherwise the necessary link-satellite builder and storage can be introduced explicitly within this story.
- Tests should use caller-owned sample CLR types defined in test code as needed rather than depending on any repository-owned State type.

## Open Questions
- none

## Follow-Up Questions
- After this story lands, should source-generator or compile-time mapping parity for link-parent satellites be tracked as a separate ticket?
- After ticket 06F2PGM9038RXVJH0RJFYEJEV0 lands, should a runnable example or quickstart cover end-to-end declaration, save, and read usage for a Code-First link-parent satellite?

## Risks
- Scope creep into participant aliases, roles, or same-hub recursive link shapes remains the main delivery risk because the current ticket stays bounded to link-parent satellite parity.
- The prompt-backed branch snapshot does not prove an existing reusable satellite builder or storage path, so implementation may need both new public API and new internal declaration plumbing inside the same story.
- Public documentation remains hub-parent-focused until ticket 06F2PGM9038RXVJH0RJFYEJEV0 lands, so short-term documentation drift is still possible.

## Split Recommendations
- No further split is recommended from current prompt-backed evidence; keep this story focused on the additive link-parent satellite declaration and projection gap.
- Keep documentation and release-note work on ticket 06F2PGM9038RXVJH0RJFYEJEV0, and raise any later generator, mapping, or example expansion as separate follow-up work if needed.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Allow fluent declaration of satellites attached to links.

## Scope
- Refine and complete the work for "Add Code-First link-parent satellites" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.