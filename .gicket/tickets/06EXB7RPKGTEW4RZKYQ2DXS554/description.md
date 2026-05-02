<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Ticket 06EXB7RPKGTEW4RZKYQ2DXS554 remains a coordination-only umbrella for the shared two-event C-100 customer-profile comparison and is not a third implementation ticket.
- Child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 owns the plain EF comparison slice and is already done.
- Child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8 owns the DVault comparison slice and is already done.
- Persisted parentOf relations already link the parent to both child tickets.
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md remains the authoritative shared two-event C-100 comparison contract.
- No new child tickets, relation writes, attachments, or planning documents were materialized in this refinement run.

### Scope In
- Keep the parent as umbrella coordination for the shared two-event C-100 customer-profile comparison across child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.
- Maintain docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md as the authoritative shared scenario and persisted-outcome contract.
- Ratify that the existing repository evidence in tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs already satisfies the two child-owned implementation slices.
- Record explicitly that the parent owns no separate src/ or tests/ implementation slice and should only advance through coordination completion or closure handling.

### Scope Out
- Any new parent-owned src/ or tests/ implementation work for this story.
- Re-implementing behavior already owned by child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 or child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8.
- A third implementation pass under the parent ticket.
- Standalone sample applications, a new examples/ surface, or broader relationship demos in this ticket.
- Additional timestamps, replay or dedup variants, or deferred Data Vault capabilities beyond the locked two-event contract.

## Acceptance Criteria
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md remains the authoritative shared comparison contract for child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.
- Repository evidence for the plain EF side exists in tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs and asserts exactly two persisted C-100 history rows for the locked two-event scenario.
- Repository evidence for the DVault side exists in tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs, where DefaultSaveServicePersistsCustomerProfileSatelliteHistoryThroughSqlite asserts exactly one HubCustomer row plus two SatCustomerProfile rows for the same two events and reuses the hub on the second event instead of inserting a duplicate.
- Both child tickets remain the only owners of the storage-specific implementation details for this story.
- The parent contract explicitly remains coordination-only and does not define a separate parent-owned repository implementation or test slice.

## Definition of Done
- The acceptance criteria are satisfied.
- Both child tickets remain linked from the parent through the existing parentOf relations.
- The shared comparison contract stays aligned with the repository test evidence and continues to bound the scenario to the two locked C-100 events.
- No open parent-owned code or test slice remains after refinement.
- The delivery stays within the SQLite-focused MVP boundary and does not widen into a runnable example track or deferred Data Vault capabilities.

## Implementation Notes
- Use docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md as the single source of truth for the shared inputs and persisted outcomes across both child tickets.
- Plain EF evidence already exists in tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs and asserts exactly two persisted customer profile history rows for business key C-100.
- DVault evidence already exists in tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs; DefaultSaveServicePersistsCustomerProfileSatelliteHistoryThroughSqlite asserts one HubCustomer row, two SatCustomerProfile rows, a reused second hub save with 0 new rows, and one second satellite insert.
- src/DCoding.Data.DVault/DataVaultSaveService.cs exposes the existing IDataVaultSaveService, DataVaultSaveRequest, and DataVaultSatelliteSaveOperation boundary already cited by the child-ticket contract.
- Customer identity stays in the hub and customer profile attributes stay in the satellite, consistent with docs/architecture/mvp-data-vault-concepts.md and docs/architecture/dvault-v1-explicit-save-service.md.
- No new planning document, attachment, relation, or child-ticket materialization was needed because the existing shared contract and relations already bound the umbrella story correctly.

## Open Questions
- none

## Follow-Up Questions
- After this umbrella story is closed or otherwise advanced, should the same comparison scenario also be promoted from test-only coverage into a runnable example or documentation sample?
- If more comparison scenarios are added later, should a follow-up ticket introduce shared fixtures or assertion helpers so the plain EF and DVault baselines stay synchronized in code as well as in the planning contract?

## Risks
- Scope confusion could return if the parent is later reopened as a third implementation ticket instead of remaining an umbrella coordination story.
- The comparison loses value if either child implementation or the shared planning document drifts from the locked two-event C-100 contract.
- If the SQLite DVault baseline or naming conventions change, the shared contract and both test surfaces may require coordinated updates.

## Split Recommendations
- No further split is recommended; keep the existing split into child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8 and do not add a new parent-owned implementation slice.
- If stakeholders later want a runnable example, broader relationship demos, or additional history variants, create separate follow-up tickets instead of reopening this parent story as developer work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Create a simple scenario that demonstrates hub and satellite historization.

## Scope
- Implement normal EF baseline and DVault version.
- Use Sqlite for examples and tests.

## Acceptance Criteria
- Customer changes produce understandable history in DVault tables.
- Baseline EF version exists for comparison.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.