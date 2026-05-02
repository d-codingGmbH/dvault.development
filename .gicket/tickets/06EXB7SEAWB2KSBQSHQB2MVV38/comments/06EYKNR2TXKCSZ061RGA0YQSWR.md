[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the refined parent story now matches directly observed repository evidence, child-ticket ownership, and the prior PO-critic feedback, with no open questions remaining.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7SEAWB2KSBQSHQB2MVV38/description.md:30-41 and :50-51 now scope HubOrder/HubProduct evidence to table-name plus row-shape proof, keep explicit schema/table assertions on LinkOrderProduct and SatOrderProductFulfillment, and set `## Open Questions` to `- none`.
- git diff 9b92ac3a..d79d1e5e -- .gicket/tickets/06EXB7SEAWB2KSBQSHQB2MVV38/description.md shows the parent contract was narrowed from full HubOrder/HubProduct schema-style proof to row-shape evidence and its Definition of Done was realigned to the existing two-child split.
- .gicket/tickets/06EXB7SEAWB2KSBQSHQB2MVV38/comments/06EYKFA1Z9YAM3R1H3HTKB07H0.md returned the story to PO over hub-proof ownership, and .gicket/tickets/06EXB7SEAWB2KSBQSHQB2MVV38/comments/06EYKJJTCFJR6BTB241RE72SY4.md answers critic-items 1-4 with the same narrowed scope now persisted in the description.
- tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:11-105 contains the conventional SQLite Order/Product/OrderLine scenario, and :436-474 configures ordinary EF entities, keys, and foreign keys without DVault metadata APIs.
- tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:110-298 contains the DVault Order/Product/OrderProduct/Fulfillment scenario using `services.AddDVault()` and `IDataVaultSaveService`; :240-298 asserts HubOrder/HubProduct row shape, all four generated table names, and explicit schema assertions for LinkOrderProduct and SatOrderProductFulfillment.
- Direct source evidence for the scoped public APIs exists in src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:29-38 (`ApplyDataVaultMetadata`), src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-25 (`AddDVault`), and src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21 (`IDataVaultSaveService`).
- The shared hub-technical-metadata layering cited by the parent is locally verifiable: tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:59-71 asserts hub row `RecordSource`/`LoadTimestamp` and scoped hash keys, and tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs:35-83 covers the four reusable technical metadata roles and default names.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The lighter HubOrder/HubProduct proof remains sufficient only while the shared hub technical-metadata coverage stays available in ExplicitDataVaultSaveServiceSqliteTests.cs and TechnicalMetadataColumnContractTests.cs, which the parent contract now explicitly relies on.
- The comparison value of the story still assumes future edits keep the conventional and DVault variants aligned to the same business narrative and data points inside NormalEfOrderProductSqliteTests.cs.

AC / test suggestions
- If stakeholders later want explicit HubOrder or HubProduct DDL-style assertions inside this story, keep that as a separate follow-up ticket rather than widening the approved parent contract.

Implementation watchouts
- Keep the delivery surface on tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs and the root DVault.slnx path; the approved contract does not reopen `examples/` or new persistence API scope.
- Do not treat the parent story as requiring full HubOrder or HubProduct schema assertions; the approved scope is hub table-name plus row-shape evidence, with explicit schema visibility reserved for LinkOrderProduct and SatOrderProductFulfillment.

Non-blocking notes
- Branch tip 2fdfa4c0 is a PO-critic lease-claim commit; the substantive PO refinement is visible in commit d79d1e5e and the persisted description/comment updates.

Split recommendations
- Keep the existing two-task split: 06EXB7SP77MW1HVW7KT4ZFV6G8 for the conventional EF baseline and 06EXB7SY3J6160R9Q35CFN6Q1W for the DVault link-and-satellite variant plus explicit relationship-table schema visibility.
- Do not add a third child unless future scope explicitly pulls in full hub DDL duplication, runnable sample-app packaging, or benchmark-specific reuse work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment