[gicket-bot] PO-critic review contract

Summary
- Tracking-only parent ticket closure audit found blocking readiness gaps.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NWCA6NEZH8VBJNGW4FVHG/description.md:7-50 sets PO Handoff to ready_for_po_critic, scopes work to durable docs plus the named existing suites, and records Open Questions as none.
- .gicket/tickets/06EZ0NWCA6NEZH8VBJNGW4FVHG/comments/06F036Y9V7B1NMWP4P92H8PWD8.md and .gicket/tickets/06EZ0NWCA6NEZH8VBJNGW4FVHG/comments/06F0377FNKD8TN2SJTK54HJZ9W.md repeat the bounded docs/tests scope, the README wording risk, and No split recommended; no later comment introduces a new PO blocker.
- git log --oneline --grep for 06EZ0NW61GFJN90PSB5N934G2G and 06EZ0NVX3RYPTFZKYCYEH9HB8W shows commits cb0f0d84c and c827e5982 as AUTO-INTEGRATION squash into develop.
- Direct source evidence matches the contract surface: src/DCoding.Data.DVault/Modeling/DataVaultModel.cs:603-628 exports `DataVaultSatelliteBuilder.DrivingKey(...)`; src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:736-845 exports the multi-active metadata constructor and driving-key validation; src/DCoding.Data.DVault/DataVaultSaveService.cs:245-325 exports the multi-active save-operation constructor and driving-key-value validation.
- The named suites already cover the ticket seams: tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:136-158, tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:187-214, tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:380-426, and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:<redacted>.
- README.md:158-160 still frames multi-active satellites as deferred and opt-in without driving-key guidance; source-backed future-work limits are explicit in src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:490-512 and the provider CanSave guards that exclude multi-active batches in SQLite, Postgres, SQL Server, MySQL, and Oracle.

Blocking findings
- No outgoing parentOf child tickets were found for the tracking-only parent ticket.

Required PO actions
- Resolve the tracking-parent closure audit findings before this parent ticket can be closed.

Open issues ledger
- critic-item-1 [required-po-action] Resolve the tracking-parent closure audit findings before this parent ticket can be closed.
- critic-item-2 [blocking-finding] No outgoing parentOf child tickets were found for the tracking-only parent ticket.

Missing examples / edge cases
- none

Risky assumptions
- The contract leaves the durable-doc destination open; implementation needs a non-planning repo surface so the explanation does not remain only under docs/plans.

AC / test suggestions
- Treat the four named existing suites as the acceptance-proof anchors instead of creating parallel ad hoc coverage.

Implementation watchouts
- Do not invent new public API names in docs; the source-backed surfaces are `DataVaultSatelliteBuilder.DrivingKey(...)`, `DataVaultSatelliteMetadata(..., drivingKeyNames)`, and `DataVaultSatelliteSaveOperation(..., drivingKeyValues, ...)`.
- Keep `hashDiff` described as payload-state change detection, not driving-key identity.
- Keep PIT-over-multi-active and provider-optimized multi-active save behavior in future-work language because current source rejects or declines those paths.

Non-blocking notes
- git diff --name-only develop...HEAD shows only .gicket/tickets/06EZ0NWCA6NEZH8VBJNGW4FVHG changes on this branch, so developer work has not started yet.

Split recommendations
- No split recommended; the persisted contract is already bounded to durable docs plus ratification or extension of existing suites, and the prerequisite contract and persistence slices are done.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment