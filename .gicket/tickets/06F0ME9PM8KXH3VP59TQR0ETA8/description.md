<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Ratified the existing attached child-boundary and parent contract for 06F0ME9PM8KXH3VP59TQR0ETA8, clarified DrivingKey multi-active and hub-parent-only scope, and found no need for new child tickets, relation changes, or planning documents.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- docs/plans/06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md is already attached to this ticket and is the authoritative supplement to the shorter ticket description.
- This child owns hub and hub-parent satellite projection only, including repeated direct BusinessKey(...), Payload(...), and DrivingKey(...) selector capture plus actionable selector validation.
- DrivingKey(...) is in scope for this ticket as the only fluent multi-active opt-in for hub-parent satellites; the original short ticket text is narrowed and extended by the authoritative child addendum and parent contract.
- Link declarations and link-parent satellites remain on 06F0MEA1FF743S14XQW02H4A3W, and broader cross-path parity coverage remains on 06F0MEAD1BAA5QEVM3F9QJA38G.
- No relation cleanup is required; the existing blocks relations to 06F0MEAD1BAA5QEVM3F9QJA38G and 06F0MEB634X6CTBZ00W108G3FG remain consistent with the dependency chain.

### Scope In
- Add the additive ModelBuilder.ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>) path for hub declarations by CLR entity type, projecting into DataVaultMetadataModel before reusing the existing translator.
- Capture repeated direct scalar BusinessKey(...) selectors in declaration order for hub business keys.
- Capture repeated direct scalar Payload(...) selectors and explicit satellite names for hub-parent satellites.
- Treat one or more DrivingKey(...) calls as the only fluent multi-active opt-in for hub-parent satellites and project them into DataVaultSatelliteMetadata.DrivingKeyNames in declaration order.
- Add targeted tests that prove the covered fluent hub and hub-parent satellite shapes translate to the same schema shape as the existing metadata-first baseline.

### Scope Out
- Link declarations, relationship-name derivation, and link-parent satellite projection; owned by 06F0MEA1FF743S14XQW02H4A3W.
- Broader schema-parity matrix and migration-style parity coverage; owned by 06F0MEAD1BAA5QEVM3F9QJA38G.
- Save helper generation, typed save/read helpers, registry export or import, PIT or bridge work, and provider-specific SQL changes.
- Hub logical-name override support; callers needing a non-CLR hub name stay on the metadata-first path in v1.

## Acceptance Criteria
- A new additive fluent overload accepts hub declarations by CLR entity type, builds provider-neutral metadata, and reuses the existing ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel, ...) projection path without regressing current metadata-first overloads.
- Repeated direct single-member BusinessKey(...), Payload(...), and DrivingKey(...) calls preserve declaration order and produce deterministic hub and satellite tables, columns, keys, and indexes matching the existing metadata-first schema rules for the covered hub-parent shapes.
- DrivingKey(...) is the only fluent multi-active opt-in for this child; one or more calls populate DataVaultSatelliteMetadata.DrivingKeyNames and yield the existing multi-active satellite key and index ordering for hub-parent satellites.
- Unsupported selector shapes such as anonymous-object, computed, or non-member selectors fail with actionable validation messages that direct callers to use repeated single-member selector calls.
- Targeted tests prove schema equivalence for covered hub and hub-parent satellite scenarios, and existing metadata-first tests continue to pass unchanged.

## Definition of Done
- Public API and snapshot coverage reflect the additive fluent overload and new root-namespace DataVaultCodeFirst*Builder types without breaking the existing DCoding.Data.DVault.Modeling builders.
- The fluent path emits the same provider-neutral metadata names and canonical ordering that the current translator and provider capability profiles already expect, including multi-active driving-key columns.
- Tests cover ordinary hub-parent satellites, the covered DrivingKey(...) multi-active hub-parent satellite scenario, and validation failures for unsupported selectors.
- No link, link-parent satellite, save-service, registry/model-first, PIT, or bridge behavior is introduced by this ticket.

## Implementation Notes
- Repository evidence shows the current public surface only exposes metadata-model overloads plus the older string-based DCoding.Data.DVault.Modeling builders, so this work should add a new root-namespace DataVaultCodeFirst*Builder family instead of mutating the existing modeling builders.
- The existing translator already projects DataVaultSatelliteMetadata.DrivingKeyNames into DataVaultPropertyRole.DrivingKey columns and canonical primary/index ordering, so the main implementation work is selector capture plus metadata projection rather than new provider-specific translation rules.
- Keep the default provider-neutral hub name equal to the CLR type name; do not add a separate hub-name override in this ticket.
- Use the existing metadata-first multi-active tests and naming rules as the schema baseline for the fluent path rather than creating a second naming convention.
- The authoritative planning artifacts already exist: docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md and the attached child addendum docs/plans/06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md.

## Open Questions
- none

## Follow-Up Questions
- After registry/model-first work lands, does the team want an explicit fluent hub-name override, or should metadata-first remain the escape hatch for non-CLR logical hub names?
- Once hub-parent fluent projection is stable, should link-parent satellites be added as a separate fluent expansion rather than folded into this child?

## Risks
- If selector parsing accepts anything broader than direct single-member access, the fluent surface can drift from the deterministic declaration-order contract and produce ambiguous validation behavior.
- If the implementation bypasses DataVaultMetadataModel or redefines naming and key rules locally, provider-aware schema translation can diverge from the existing metadata-first baseline and break downstream parity work.
- Changing the existing DCoding.Data.DVault.Modeling builders instead of adding the additive code-first builder family would create avoidable public API collision and compatibility risk.

## Split Recommendations
- No new split is required; keep the existing child plan of 06F0ME9PM8KXH3VP59TQR0ETA8 for hub and hub-parent satellite projection, 06F0MEA1FF743S14XQW02H4A3W for link projection, and 06F0MEAD1BAA5QEVM3F9QJA38G for broader parity coverage.
- Keep the current relation structure unchanged; this ticket still appropriately blocks 06F0MEAD1BAA5QEVM3F9QJA38G and 06F0MEB634X6CTBZ00W108G3FG.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Implement the fluent Code-First path for hub and ordinary satellite declarations and project it into the existing provider-aware EF metadata translator.

## Scope In

- Hub business-key selector capture.
- Satellite payload selector capture.
- Equivalent hub and satellite metadata generation.
- Tests comparing generated schema with metadata-first declarations.

## Scope Out

- Link declarations.
- Save helper generation.
- Model-first file import.

## Acceptance Criteria

- Fluent hub and satellite declarations produce deterministic tables, columns, keys, and indexes.
- Invalid selectors fail with actionable validation messages.
- Existing metadata-first tests continue to pass unchanged.