[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract is explicit, repository-backed, and has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0ME9PM8KXH3VP59TQR0ETA8/description.md contains the authoritative delivery contract with PO handoff ready_for_po_critic and ## Open Questions set to none.
- .gicket/tickets/06F0ME9PM8KXH3VP59TQR0ETA8/attachments/manifest.json attaches 06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md; the matching repo file docs/plans/06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md scopes this child to hub and hub-parent satellites only and makes DrivingKey(...) the only fluent multi-active opt-in.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs currently exposes only ApplyDataVaultMetadata(..., DataVaultMetadataModel...) overloads; rg -n "DataVaultCodeFirst" src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests returned no matches; src/DCoding.Data.DVault/Modeling/DataVaultModel.cs shows the existing string-based DataVaultModelBuilder.Hub(string entityName, ...) surface.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs projects satellite.DrivingKeyNames; tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs contains ApplyDataVaultMetadataProjectsMultiActiveSatelliteDrivingKeysInCanonicalOrder; tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs asserts the same canonical SatCustomerContactChannel column, primary-key, and index order.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: a worked example with two BusinessKey(...) calls plus a multi-active satellite on the same hub would make declaration-order expectations faster to validate during implementation.
- Non-blocking: an explicit rejected-selector example for each verb (BusinessKey, Payload, DrivingKey) would make the actionable-validation expectation more concrete.

Risky assumptions
- The handoff assumes sibling tickets 06F0MEA1FF743S14XQW02H4A3W and 06F0MEAD1BAA5QEVM3F9QJA38G can remain out of scope for this child; the current child boundary and relation files support that separation.

AC / test suggestions
- Add a targeted parity test for a fluent hub with two ordered BusinessKey(...) selectors against the existing metadata-first hub baseline.
- Add a targeted parity test for a hub-parent satellite with two ordered DrivingKey(...) selectors and repeated Payload(...) calls against the existing SatCustomerContactChannel multi-active baseline.
- Add separate validation tests for anonymous-object, computed or method-call, and nested-navigation selectors so each failure names the offending API and tells callers to use repeated single-member selectors.

Implementation watchouts
- Keep the fluent path additive: route through DataVaultMetadataModel and reuse ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel, ...) instead of duplicating naming or key logic.
- Preserve the current multi-active column order already asserted in source tests: parent hash key, driving keys in declaration order, HashDiff, LoadTimestamp, RecordSource, payload.
- Do not mutate the existing DCoding.Data.DVault.Modeling builder family; repository source and public API snapshot show the current public surface is the older string-based model builder.

Non-blocking notes
- .gicket/relations/A8/8G/06F0ME9PM8KXH3VP59TQR0ETA8--06F0MEAD1BAA5QEVM3F9QJA38G--blocks.json and .gicket/relations/A8/FG/06F0ME9PM8KXH3VP59TQR0ETA8--06F0MEB634X6CTBZ00W108G3FG--blocks.json still show this ticket blocking 06F0MEAD1BAA5QEVM3F9QJA38G and 06F0MEB634X6CTBZ00W108G3FG, matching the persisted contract.

Split recommendations
- No new split is needed; keep hub and hub-parent satellite projection on 06F0ME9PM8KXH3VP59TQR0ETA8, link projection on 06F0MEA1FF743S14XQW02H4A3W, and broader parity coverage on 06F0MEAD1BAA5QEVM3F9QJA38G.
- Keep the existing relation structure unchanged.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment