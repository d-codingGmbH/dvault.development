[gicket-bot] PO refinement contract

Summary
- Ratified the existing attached child-boundary and parent contract for 06F0ME9PM8KXH3VP59TQR0ETA8, clarified DrivingKey multi-active and hub-parent-only scope, and found no need for new child tickets, relation changes, or planning documents.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- docs/plans/06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md is already attached to this ticket and is the authoritative supplement to the shorter ticket description.
- This child owns hub and hub-parent satellite projection only, including repeated direct BusinessKey(...), Payload(...), and DrivingKey(...) selector capture plus actionable selector validation.
- DrivingKey(...) is in scope for this ticket as the only fluent multi-active opt-in for hub-parent satellites; the original short ticket text is narrowed and extended by the authoritative child addendum and parent contract.
- Link declarations and link-parent satellites remain on 06F0MEA1FF743S14XQW02H4A3W, and broader cross-path parity coverage remains on 06F0MEAD1BAA5QEVM3F9QJA38G.
- No relation cleanup is required; the existing blocks relations to 06F0MEAD1BAA5QEVM3F9QJA38G and 06F0MEB634X6CTBZ00W108G3FG remain consistent with the dependency chain.

Scope In
- Add the additive ModelBuilder.ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>) path for hub declarations by CLR entity type, projecting into DataVaultMetadataModel before reusing the existing translator.
- Capture repeated direct scalar BusinessKey(...) selectors in declaration order for hub business keys.
- Capture repeated direct scalar Payload(...) selectors and explicit satellite names for hub-parent satellites.
- Treat one or more DrivingKey(...) calls as the only fluent multi-active opt-in for hub-parent satellites and project them into DataVaultSatelliteMetadata.DrivingKeyNames in declaration order.
- Add targeted tests that prove the covered fluent hub and hub-parent satellite shapes translate to the same schema shape as the existing metadata-first baseline.

Scope Out
- Link declarations, relationship-name derivation, and link-parent satellite projection; owned by 06F0MEA1FF743S14XQW02H4A3W.
- Broader schema-parity matrix and migration-style parity coverage; owned by 06F0MEAD1BAA5QEVM3F9QJA38G.
- Save helper generation, typed save/read helpers, registry export or import, PIT or bridge work, and provider-specific SQL changes.
- Hub logical-name override support; callers needing a non-CLR hub name stay on the metadata-first path in v1.

Open questions
- none

Follow-up questions
- After registry/model-first work lands, does the team want an explicit fluent hub-name override, or should metadata-first remain the escape hatch for non-CLR logical hub names?
- Once hub-parent fluent projection is stable, should link-parent satellites be added as a separate fluent expansion rather than folded into this child?

Risks
- If selector parsing accepts anything broader than direct single-member access, the fluent surface can drift from the deterministic declaration-order contract and produce ambiguous validation behavior.
- If the implementation bypasses DataVaultMetadataModel or redefines naming and key rules locally, provider-aware schema translation can diverge from the existing metadata-first baseline and break downstream parity work.
- Changing the existing DCoding.Data.DVault.Modeling builders instead of adding the additive code-first builder family would create avoidable public API collision and compatibility risk.

Split recommendations
- No new split is required; keep the existing child plan of 06F0ME9PM8KXH3VP59TQR0ETA8 for hub and hub-parent satellite projection, 06F0MEA1FF743S14XQW02H4A3W for link projection, and 06F0MEAD1BAA5QEVM3F9QJA38G for broader parity coverage.
- Keep the current relation structure unchanged; this ticket still appropriately blocks 06F0MEAD1BAA5QEVM3F9QJA38G and 06F0MEB634X6CTBZ00W108G3FG.

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