<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Adjusted the story contract so current HubOrder/HubProduct table-name and row-shape proof is sufficient, while explicit schema assertions stay scoped to LinkOrderProduct and SatOrderProductFulfillment; the existing two-child split remains valid and no new planning artifacts were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Persisted split remains unchanged: story 06EXB7SEAWB2KSBQSHQB2MVV38 still has child task 06EXB7SP77MW1HVW7KT4ZFV6G8 for the conventional EF baseline and child task 06EXB7SY3J6160R9Q35CFN6Q1W for the DVault variant; no new child tickets, relations, attachments, or planning documents were created in this PO pass.
- The v1 example surface remains tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs rather than examples/ or a standalone sample app.
- Story-level DVault proof is intentionally layered: the order-product scenario shows HubOrder and HubProduct table presence plus row shape, while shared repository tests and documents already cover the reusable hub technical-metadata contract and default naming behavior.
- The bounded comparison model remains conventional EF Order/Product/OrderLine versus DVault HubOrder/HubProduct with LinkOrderProduct and SatOrderProductFulfillment.
- Ticket comments still introduce no human scope changes, and the ticket has no persisted attachments relevant to this refinement decision.

### Scope In
- Deliver one small SQLite-backed order/product comparison scenario that keeps the normal EF and DVault variants aligned to the same business narrative.
- Keep the conventional side as ordinary EF Core Order, Product, and OrderLine entity mapping in the existing integration-test surface.
- Keep the DVault side as Order and Product hubs, one OrderProduct link, and a link satellite that records relationship-context history over time.
- Make generated structures visible by asserting all four generated table names, by asserting hub row shape for HubOrder and HubProduct, and by keeping explicit schema/table assertions for LinkOrderProduct and SatOrderProductFulfillment.

### Scope Out
- Do not introduce a separate runnable sample application under examples/ for this story.
- Do not widen the story into new generic persistence APIs beyond the existing ApplyDataVaultMetadata, AddDVault, and IDataVaultSaveService surfaces.
- Do not add PIT tables, bridge tables, multi-active satellites, provider-specific optimizations, or non-SQLite baseline requirements.
- Do not duplicate standalone hub DDL or full hub column-list assertions in this story when the reusable hub technical-metadata baseline is already covered by shared save-service and contract tests.

## Acceptance Criteria
- The repository contains a conventional EF Core SQLite scenario for Order, Product, and OrderLine that uses ordinary entity types, keys, and foreign keys rather than DVault metadata APIs.
- The repository contains a DVault SQLite scenario for the same business narrative using Order and Product hubs, an OrderProduct link, and a Fulfillment-style satellite attached to that link, written through the existing IDataVaultSaveService boundary.
- The DVault scenario demonstrates relationship history by persisting at least two distinct satellite versions for the same order-product relationship and by showing that an unchanged latest replay does not create a new historical row.
- The order-product scenario explicitly shows the generated structures for HubOrder, HubProduct, LinkOrderProduct, and SatOrderProductFulfillment; HubOrder and HubProduct are satisfied by table-name plus row-shape assertions for their business-key and hash-key columns, while LinkOrderProduct and SatOrderProductFulfillment keep explicit schema/table assertions including the expected technical metadata columns and naming-convention outputs.
- The normal EF and DVault variants stay small, deterministic, and clearly comparable so they can be reused by later documentation or benchmark work.

## Definition of Done
- Child task 06EXB7SP77MW1HVW7KT4ZFV6G8 continues to own the conventional EF baseline and child task 06EXB7SY3J6160R9Q35CFN6Q1W continues to own the DVault scenario plus explicit LinkOrderProduct and SatOrderProductFulfillment schema visibility; no third child is required under this revised parent contract.
- Automated proof remains under the existing tests/DCoding.Data.DVault.Tests integration surface and on the root DVault.slnx validation path.
- Shared standards and referenced repository decisions remain followed, including the SQLite-focused MVP concepts, default naming policy, stable hashing contract, formatting rules, and net10.0 baseline.
- No unresolved PO-level decisions remain about the business nouns, execution surface, v1 history pattern, or the bounded level of hub-versus-link schema proof for this story.

## Implementation Notes
- Reuse the existing integration-test project and keep the example colocated in tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs unless a later examples ticket explicitly promotes it.
- On the DVault side, use ApplyDataVaultMetadata to project the schema and use IDataVaultSaveService/DataVaultSaveRequest for writes rather than SaveChanges interception or a new write path.
- Keep the generated naming and technical-column expectations aligned with current repo conventions: HubOrder, HubProduct, LinkOrderProduct, SatOrderProductFulfillment, scoped hash-key columns, HashDiff, LoadTimestamp, and RecordSource.
- Use the current repository layering instead of duplicating generic hub proof in this story: NormalEfOrderProductSqliteTests.cs should show HubOrder and HubProduct table presence plus row shape, while ExplicitDataVaultSaveServiceSqliteTests.cs, TechnicalMetadataColumnContractTests.cs, and the naming and MVP documents continue to cover reusable hub technical-metadata defaults.
- No new planning documents, attachments, relation writes, or child tickets were needed because the critic issue was resolved by narrowing the parent contract to the evidence already present in the shared scenario and repository standards.

## Open Questions
- none

## Follow-Up Questions
- Should a later documentation ticket promote this test-backed scenario into a runnable sample under examples/ once the MVP example set stabilizes?
- Should a later documentation or benchmark ticket add a side-by-side narrative that maps conventional OrderLine rows to the DVault LinkOrderProduct plus SatOrderProductFulfillment history?
- If stakeholders later want explicit HubOrder and HubProduct DDL or full column-list assertions inside the scenario itself, should that be handled as a separate follow-up ticket instead of widening this relationship-focused story?

## Risks
- If future edits remove the shared hub metadata coverage in ExplicitDataVaultSaveServiceSqliteTests.cs without replacing it, this story's lighter hub-proof posture will become under-supported.
- If future edits let the conventional and DVault variants drift to different business facts or payloads, the comparison value of the story will erode.
- Because the structure explanation currently lives in integration tests and schema assertions instead of a dedicated tutorial page, readability depends on keeping that evidence compact and intentional.

## Split Recommendations
- The current two-task split remains the bounded v1 plan: 06EXB7SP77MW1HVW7KT4ZFV6G8 covers the normal EF baseline and 06EXB7SY3J6160R9Q35CFN6Q1W covers the DVault link-and-satellite variant plus explicit relationship-table schema visibility.
- No additional split is recommended unless a future ticket explicitly pulls full hub DDL duplication, runnable sample-app packaging, or benchmark harness reuse out of this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Demonstrate link modeling with a common order-product relationship.

## Scope
- Implement normal EF and DVault variants.
- Show how relationship history is represented.

## Acceptance Criteria
- The DVault version includes an order-product link.
- Docs explain the generated structures.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.