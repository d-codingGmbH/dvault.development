[gicket-bot] PO refinement contract

Summary
- Refined the fluent Code-First API contract, confirmed the existing three-child implementation split already matches the plan, and created docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md; no relation changes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Created docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md as the durable design note with representative hub, satellite, multi-active, and link snippets.
- The additive EF Core entry point is `ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>)` in `DCoding.Data.DVault`; existing metadata-first overloads and `DCoding.Data.DVault.Modeling` builders stay supported unchanged.
- The already-created child split remains the implementation plan: 06F0ME9PM8KXH3VP59TQR0ETA8 for hub/ordinary satellite projection, 06F0MEA1FF743S14XQW02H4A3W for link projection, and 06F0MEAD1BAA5QEVM3F9QJA38G for schema parity tests.
- No relation cleanup or new child materialization was needed because the current parentOf and blocks links already align with the bounded plan.

Scope In
- Additive EF Core Code-First contract for hub declarations by CLR entity type with repeated business-key selectors.
- Nested ordinary satellite declarations with repeated payload selectors and the reserved `DrivingKey(...)` multi-active opt-in verb.
- Link declarations over previously configured hubs with optional explicit relationship name and canonical participant ordering.
- Selector-shape validation and actionable failure messaging for unsupported expressions.
- Compatibility guidance that the fluent surface projects into `DataVaultMetadataModel` and then the existing provider-aware EF translator.

Scope Out
- Implementation of the fluent builders and overloads themselves.
- Link-parent satellite fluent declarations in the current v1 Code-First baseline.
- PIT, bridge, model-first, registry export/import, and typed save/read helper APIs.
- Any `SaveChanges` interception or automatic write behavior.
- Breaking changes to existing metadata-first APIs or the current `DCoding.Data.DVault.Modeling` surface.

Open questions
- none

Follow-up questions
- If the v0.6 release train needs fluent link-parent satellites, should that land as a dedicated follow-up ticket instead of broadening the current hub-parent-only implementation slice?
- After the registry/model-first work lands, does the team want an explicit Code-First hub-name override, or is metadata-first intended to remain the escape hatch for non-CLR logical hub names?

Risks
- If implementation broadens selector support beyond direct scalar member access without updating the contract, validation behavior and schema parity tests can drift.
- If future work repurposes the existing `DCoding.Data.DVault.Modeling` builders instead of keeping the new EF-specific surface additive, the public API becomes harder to reason about and migrate.
- If consumers immediately need non-CLR logical hub names, the v1 default-to-type-name decision may force temporary fallback to metadata-first declarations.

Split recommendations
- No new split is required for this ticket; keep the existing child plan of 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G.
- If fluent multi-active projection or link-parent satellites become in-scope for the same release train, split them into dedicated implementation tickets rather than widening the current ordinary-satellite and link tasks.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment