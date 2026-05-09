[gicket-bot] PO-critic review contract

Summary
- The parent fluent Code-First contract is ready for developer handoff. The prior child-boundary blocker is now resolved by attached authoritative addenda, and the current source supports the compatibility anchors the contract relies on.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F0ME976PM5455JK04S6GPNNW/description.md` contains `PO Handoff` with `decision: ready_for_po_critic` and `## Open Questions` followed by `- none`.
- `git show --name-status cc09edcaa` shows the PO->PO-critic handoff commit added `docs/plans/06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md`, `docs/plans/06F0MEA1FF743S14XQW02H4A3W-fluent-link-child-boundary.md`, `docs/plans/06F0MEAD1BAA5QEVM3F9QJA38G-fluent-parity-child-boundary.md`, plus the three child `attachments/manifest.json` files.
- `.gicket/tickets/06F0ME9PM8KXH3VP59TQR0ETA8/attachments/manifest.json`, `.gicket/tickets/06F0MEA1FF743S14XQW02H4A3W/attachments/manifest.json`, and `.gicket/tickets/06F0MEAD1BAA5QEVM3F9QJA38G/attachments/manifest.json` each attach the corresponding authoritative child-boundary addendum, with `addedAt` timestamps `<redacted>-09T09:05:47.7610000Z`, `<redacted>-09T09:06:01.4960000Z`, and `<redacted>-09T09:06:14.9820000Z`.
- `docs/plans/06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md` assigns repeated `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)` selector capture/validation to ticket `06F0ME9PM8KXH3VP59TQR0ETA8` and states `DrivingKey(...)` is the only fluent multi-active opt-in for the covered hub-parent shape.
- `docs/plans/06F0MEA1FF743S14XQW02H4A3W-fluent-link-child-boundary.md` keeps ticket `06F0MEA1FF743S14XQW02H4A3W` link-only and explicitly excludes hub/satellite and `DrivingKey(...)` selector work; `docs/plans/06F0MEAD1BAA5QEVM3F9QJA38G-fluent-parity-child-boundary.md` requires parity coverage for the covered `DrivingKey(...)` shape and canonical driving-key ordering.
- `.gicket/tickets/06F0ME976PM5455JK04S6GPNNW/comments/06F0R2BF2T6ZFXEN0Q7DQ2Z6H8.md` marks critic items 1 through 4 as `answered`, and those answers match the current addenda/manifests on disk.
- `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` exposes the existing metadata-first anchors `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` and the provider-aware overloads the contract says the fluent path must reuse; `src/DCoding.Data.DVault/Modeling/DataVaultModelBuilder.cs` shows the existing `DCoding.Data.DVault.Modeling.DataVaultModelBuilder` surface remains present.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` defines `DataVaultLinkMetadata` with the validation message `A link requires at least two hub endpoints.` and `DataVaultSatelliteMetadata` constructors for ordinary and driving-key variants plus ordered `DrivingKeyNames`, matching the contract's bounded compatibility assumptions.
- `.gicket/relations/NW/A8/06F0ME976PM5455JK04S6GPNNW--06F0ME9PM8KXH3VP59TQR0ETA8--blocks.json`, `.gicket/relations/NW/3W/06F0ME976PM5455JK04S6GPNNW--06F0MEA1FF743S14XQW02H4A3W--blocks.json`, and `.gicket/relations/NW/8G/06F0ME976PM5455JK04S6GPNNW--06F0MEAD1BAA5QEVM3F9QJA38G--blocks.json` show the parent ticket still blocks all three implementation children, consistent with the intended split.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The checked-in parent design note shows a single `DrivingKey(...)` example; parity coverage should still include a 2+ driving-key case to exercise canonical ordering explicitly.
- The contract names ambiguous-participant failure for links, but the representative examples do not show that edge case directly.

Risky assumptions
- Developers must treat the attached child addenda as authoritative over the shorter legacy child `description.md` text, which still omits some of the refined boundary detail.
- Approval assumes downstream child-ticket workflow-state cleanup can happen independently of this parent ticket, because the parent contract evidence is complete even though the child `ticket.json` files still carry `needs-po`.

AC / test suggestions
- Add a parity test with two `DrivingKey(...)` calls so canonical driving-key ordering is asserted against the metadata-first baseline, not just described in prose.
- Add validation coverage for duplicate logical member names across repeated `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)` calls.
- Add link failure coverage for both missing-hub and ambiguous-hub participant resolution, since those cases are part of the bounded contract.

Implementation watchouts
- Keep the new `DataVaultCodeFirst*Builder` family additive in `DCoding.Data.DVault`; do not repurpose the existing `DCoding.Data.DVault.Modeling.DataVaultModelBuilder` surface.
- Project fluent declarations through `DataVaultMetadataModel` and the existing provider-aware `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel, ...)` path so parity tests compare like-for-like metadata.
- Preserve declaration order for repeated `BusinessKey(...)`, `Payload(...)`, `DrivingKey(...)`, and link participants because the contract makes that order canonical.
- Do not widen this slice to link-parent satellites, `SaveChanges` interception, or non-CLR hub-name overrides.

Non-blocking notes
- The earlier PO-critic blocker recorded in `.gicket/tickets/06F0ME976PM5455JK04S6GPNNW/comments/06F0QWXC10TN0C1CNBASW5D7HW.md` was missing child-boundary propagation; the current handoff commit `cc09edcaa` and the child attachment manifests resolve that exact gap.

Split recommendations
- No new split is needed. Keep the existing three-child plan `06F0ME9PM8KXH3VP59TQR0ETA8`, `06F0MEA1FF743S14XQW02H4A3W`, and `06F0MEAD1BAA5QEVM3F9QJA38G`, with the attached addenda as the authoritative boundary.
- If link-parent satellites, broader multi-active coverage, or a Code-First hub-name override become release-critical, open dedicated follow-up tickets instead of widening this contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment