[gicket-bot] PO refinement contract

Summary
- Ratified attached child-boundary addenda for the three implementation tickets so they now reference the parent contract and explicitly assign DrivingKey multi-active and parity coverage ownership.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - All three child implementation tickets now explicitly reference ticket 06F0ME976PM5455JK04S6GPNNW and docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md as their authoritative boundary through attached authoritative child-boundary addenda.
- critic-item-2: `answered` - The hub/satellite child boundary now explicitly includes DrivingKey selector capture, multi-active opt-in projection, validation, and a parity-equivalent acceptance addendum, while the parity child boundary now explicitly requires parity coverage for the covered DrivingKey multi-active shape and canonical driving-key ordering.
- critic-item-3: `answered` - The parent Definition of Done required child tickets to reference this contract as their boundary, and that requirement is now satisfied because each child ticket carries an attached authoritative addendum naming the parent ticket and parent contract.
- critic-item-4: `answered` - The previously unassigned DrivingKey and covered multi-active parity work is now explicitly assigned: 06F0ME9PM8KXH3VP59TQR0ETA8 owns DrivingKey capture and validation for hub-parent satellites, 06F0MEA1FF743S14XQW02H4A3W explicitly excludes that work, and 06F0MEAD1BAA5QEVM3F9QJA38G owns parity coverage for the covered shape.

Clarifications
- The existing three-child split remains the implementation plan, but each child now has an attached authoritative child-boundary addendum in docs/plans that supplements the shorter legacy child description.
- 06F0ME9PM8KXH3VP59TQR0ETA8 owns hub and hub-parent satellite projection, including repeated BusinessKey(...), Payload(...), and DrivingKey(...) selector capture, DrivingKey multi-active opt-in, and selector validation.
- 06F0MEA1FF743S14XQW02H4A3W remains link-only and does not own hub/satellite or DrivingKey selector work.
- 06F0MEAD1BAA5QEVM3F9QJA38G owns schema parity coverage for hub, link, ordinary satellite, and the covered hub-parent DrivingKey multi-active shape.

Scope In
- Maintain the durable parent contract note and authoritative child-boundary addenda that define the implementation split for the existing three children.
- Additive EF Core Code-First contract for hub declarations by CLR entity type with repeated business-key selectors.
- Hub-parent satellite declarations with repeated payload selectors and the reserved DrivingKey(...) multi-active opt-in verb.
- Link declarations over previously configured hubs with optional explicit relationship name and canonical participant ordering.
- Selector-shape validation and actionable failure messaging for unsupported expressions.
- Compatibility guidance that fluent declarations project into DataVaultMetadataModel and remain schema-equivalent to the metadata-first baseline for covered scenarios.

Scope Out
- Implementation of the fluent builders and overloads themselves.
- Link-parent satellite fluent declarations in the current v1 Code-First baseline.
- PIT, bridge, model-first, registry export/import, and typed save/read helper APIs.
- SaveChanges interception or automatic write behavior.
- Breaking changes to existing metadata-first APIs or the current DCoding.Data.DVault.Modeling surface.

Open questions
- none

Follow-up questions
- If the v0.6 release train needs fluent link-parent satellites, should that land as a dedicated follow-up ticket instead of broadening the current hub-parent-only implementation slice?
- After the registry/model-first work lands, does the team want an explicit Code-First hub-name override, or is metadata-first intended to remain the escape hatch for non-CLR logical hub names?

Risks
- If implementation broadens selector support beyond direct scalar member access without updating the contract and parity coverage, validation behavior and schema-equivalence tests can drift.
- If a child implementation ignores its authoritative boundary addendum and follows only the shorter legacy description, DrivingKey multi-active ownership or parity expectations could be missed.
- If future work repurposes the existing DCoding.Data.DVault.Modeling builders instead of keeping the new EF-specific surface additive, the public API becomes harder to reason about and migrate.
- If consumers immediately need non-CLR logical hub names, the v1 default-to-type-name decision may force temporary fallback to metadata-first declarations.

Split recommendations
- No new split is required; keep the existing child plan of 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G, using the attached child-boundary addenda as the authoritative assignment.
- If fluent link-parent satellites, broader multi-active projection beyond the covered hub-parent shape, or a Code-First hub-name override become release-critical, split them into dedicated follow-up tickets rather than widening the current children.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment