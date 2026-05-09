<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Ratified attached child-boundary addenda for the three implementation tickets so they now reference the parent contract and explicitly assign DrivingKey multi-active and parity coverage ownership.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The existing three-child split remains the implementation plan, but each child now has an attached authoritative child-boundary addendum in docs/plans that supplements the shorter legacy child description.
- 06F0ME9PM8KXH3VP59TQR0ETA8 owns hub and hub-parent satellite projection, including repeated BusinessKey(...), Payload(...), and DrivingKey(...) selector capture, DrivingKey multi-active opt-in, and selector validation.
- 06F0MEA1FF743S14XQW02H4A3W remains link-only and does not own hub/satellite or DrivingKey selector work.
- 06F0MEAD1BAA5QEVM3F9QJA38G owns schema parity coverage for hub, link, ordinary satellite, and the covered hub-parent DrivingKey multi-active shape.

### Scope In
- Maintain the durable parent contract note and authoritative child-boundary addenda that define the implementation split for the existing three children.
- Additive EF Core Code-First contract for hub declarations by CLR entity type with repeated business-key selectors.
- Hub-parent satellite declarations with repeated payload selectors and the reserved DrivingKey(...) multi-active opt-in verb.
- Link declarations over previously configured hubs with optional explicit relationship name and canonical participant ordering.
- Selector-shape validation and actionable failure messaging for unsupported expressions.
- Compatibility guidance that fluent declarations project into DataVaultMetadataModel and remain schema-equivalent to the metadata-first baseline for covered scenarios.

### Scope Out
- Implementation of the fluent builders and overloads themselves.
- Link-parent satellite fluent declarations in the current v1 Code-First baseline.
- PIT, bridge, model-first, registry export/import, and typed save/read helper APIs.
- SaveChanges interception or automatic write behavior.
- Breaking changes to existing metadata-first APIs or the current DCoding.Data.DVault.Modeling surface.

## Acceptance Criteria
- docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md exists as the durable parent design note with representative hub, ordinary satellite, multi-active opt-in, and link snippets.
- Child tickets 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G each carry an authoritative addendum that references ticket 06F0ME976PM5455JK04S6GPNNW and/or docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md as their boundary.
- The hub/satellite child boundary explicitly includes repeated BusinessKey(...), Payload(...), and DrivingKey(...) selector capture and validation, and assigns DrivingKey(...) as the only fluent multi-active opt-in for the covered hub-parent shape.
- The parity child boundary explicitly covers parity for the covered DrivingKey(...) multi-active shape, including canonical driving-key ordering and equivalent table, column, key, and index shape versus metadata-first declarations.
- The parent contract defines an additive ModelBuilder.ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>) entry point in DCoding.Data.DVault and keeps existing metadata-first overloads intact.
- The contract keeps LoadTimestamp and RecordSource out of domain entities by default and does not promise SaveChanges interception.

## Definition of Done
- The PO-reviewed parent design note remains checked in at docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md.
- Each child implementation ticket carries an attached authoritative child-boundary addendum that references this parent contract as its boundary.
- 06F0ME9PM8KXH3VP59TQR0ETA8 explicitly owns DrivingKey multi-active selector capture and validation, and 06F0MEAD1BAA5QEVM3F9QJA38G explicitly owns parity coverage for that covered shape.
- No blocking PO questions remain about entry-point placement, selector rules, participant ordering, multi-active verb shape, child ownership boundaries, or compatibility with the current metadata-first and explicit-save boundaries.
- Current relation state remains consistent with the intended split and requires no cleanup.

## Implementation Notes
- Use new root-namespace public builder types in a DataVaultCodeFirst*Builder naming family to avoid colliding with the existing string-based DCoding.Data.DVault.Modeling builders.
- Default hub names to CLR type names in v1; callers needing different logical hub names can continue using metadata-first declarations until a later expansion justifies an override.
- Keep the bounded fluent implementation baseline at hub-parent satellites only; DrivingKey(...) remains the reserved multi-active opt-in verb and aligns with docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md.
- Project fluent declarations through DataVaultMetadataModel and the existing provider-aware ApplyDataVaultMetadata(..., DataVaultMetadataModel, ...) path so 06F0MEAD1BAA5QEVM3F9QJA38G can assert parity against the metadata-first baseline.
- Selector validation should fail fast on anonymous-object, method-call, constant, collection-navigation, and nested-navigation expressions, with messages that name the fluent API being used.
- Treat docs/plans/06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md, docs/plans/06F0MEA1FF743S14XQW02H4A3W-fluent-link-child-boundary.md, and docs/plans/06F0MEAD1BAA5QEVM3F9QJA38G-fluent-parity-child-boundary.md as authoritative supplements to the shorter child ticket descriptions.

## Open Questions
- none

## Follow-Up Questions
- If the v0.6 release train needs fluent link-parent satellites, should that land as a dedicated follow-up ticket instead of broadening the current hub-parent-only implementation slice?
- After the registry/model-first work lands, does the team want an explicit Code-First hub-name override, or is metadata-first intended to remain the escape hatch for non-CLR logical hub names?

## Risks
- If implementation broadens selector support beyond direct scalar member access without updating the contract and parity coverage, validation behavior and schema-equivalence tests can drift.
- If a child implementation ignores its authoritative boundary addendum and follows only the shorter legacy description, DrivingKey multi-active ownership or parity expectations could be missed.
- If future work repurposes the existing DCoding.Data.DVault.Modeling builders instead of keeping the new EF-specific surface additive, the public API becomes harder to reason about and migrate.
- If consumers immediately need non-CLR logical hub names, the v1 default-to-type-name decision may force temporary fallback to metadata-first declarations.

## Split Recommendations
- No new split is required; keep the existing child plan of 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G, using the attached child-boundary addenda as the authoritative assignment.
- If fluent link-parent satellites, broader multi-active projection beyond the covered hub-parent shape, or a Code-First hub-name override become release-critical, split them into dedicated follow-up tickets rather than widening the current children.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Define the public fluent Code-First contract before implementation so the API is small, discoverable, and compatible with the current metadata model.

## Scope In

- API shape for hub business keys, ordinary satellites, multi-active opt-in, and links.
- Namespace and extension-method placement.
- Error messages for unsupported selector shapes.
- Compatibility notes for existing metadata APIs.

## Scope Out

- Implementation of the fluent API.
- Model-first files.
- Save helper generation.

## Acceptance Criteria

- Representative user code snippets for hub, satellite, and link configuration exist as tests or a design note.
- The contract keeps load timestamp and record source out of domain entities by default.
- The design avoids promising SaveChanges interception.
- Follow-up tasks can use the contract as an implementation boundary.