[gicket-bot] PO refinement contract

Summary
- Adjusted the story contract so current HubOrder/HubProduct table-name and row-shape proof is sufficient, while explicit schema assertions stay scoped to LinkOrderProduct and SatOrderProductFulfillment; the existing two-child split remains valid and no new planning artifacts were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The story does not require separate full HubOrder/HubProduct DDL-style schema assertions. For this relationship-focused story, current hub table-name coverage plus hub row-shape assertions are sufficient, and reusable hub technical-metadata expectations remain covered by shared repository tests and architecture documents.
- critic-item-2: `answered` - Because explicit HubOrder/HubProduct schema proof is not required after refinement, no new ownership split is needed. The parent contract is revised to keep explicit schema/table assertions owned only for LinkOrderProduct and SatOrderProductFulfillment, so the existing two-child split remains sufficient.
- critic-item-3: `answered` - The parent acceptance criteria are narrowed to match visible repository evidence: all four generated tables must be named explicitly in the scenario, HubOrder and HubProduct must be shown through table-name and row-shape assertions, and full schema/table assertions with technical metadata remain required only for LinkOrderProduct and SatOrderProductFulfillment.
- critic-item-4: `answered` - The parent definition of done and implementation notes are revised to align with child ownership. Ticket 06EXB7SY3J6160R9Q35CFN6Q1W owns the DVault scenario and explicit LinkOrderProduct/SatOrderProductFulfillment schema visibility, while the parent story now treats HubOrder/HubProduct proof as bounded shared scenario evidence rather than unassigned extra child scope.

Clarifications
- Persisted split remains unchanged: story 06EXB7SEAWB2KSBQSHQB2MVV38 still has child task 06EXB7SP77MW1HVW7KT4ZFV6G8 for the conventional EF baseline and child task 06EXB7SY3J6160R9Q35CFN6Q1W for the DVault variant; no new child tickets, relations, attachments, or planning documents were created in this PO pass.
- The v1 example surface remains tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs rather than examples/ or a standalone sample app.
- Story-level DVault proof is intentionally layered: the order-product scenario shows HubOrder and HubProduct table presence plus row shape, while shared repository tests and documents already cover the reusable hub technical-metadata contract and default naming behavior.
- The bounded comparison model remains conventional EF Order/Product/OrderLine versus DVault HubOrder/HubProduct with LinkOrderProduct and SatOrderProductFulfillment.
- Ticket comments still introduce no human scope changes, and the ticket has no persisted attachments relevant to this refinement decision.

Scope In
- Deliver one small SQLite-backed order/product comparison scenario that keeps the normal EF and DVault variants aligned to the same business narrative.
- Keep the conventional side as ordinary EF Core Order, Product, and OrderLine entity mapping in the existing integration-test surface.
- Keep the DVault side as Order and Product hubs, one OrderProduct link, and a link satellite that records relationship-context history over time.
- Make generated structures visible by asserting all four generated table names, by asserting hub row shape for HubOrder and HubProduct, and by keeping explicit schema/table assertions for LinkOrderProduct and SatOrderProductFulfillment.

Scope Out
- Do not introduce a separate runnable sample application under examples/ for this story.
- Do not widen the story into new generic persistence APIs beyond the existing ApplyDataVaultMetadata, AddDVault, and IDataVaultSaveService surfaces.
- Do not add PIT tables, bridge tables, multi-active satellites, provider-specific optimizations, or non-SQLite baseline requirements.
- Do not duplicate standalone hub DDL or full hub column-list assertions in this story when the reusable hub technical-metadata baseline is already covered by shared save-service and contract tests.

Open questions
- none

Follow-up questions
- Should a later documentation ticket promote this test-backed scenario into a runnable sample under examples/ once the MVP example set stabilizes?
- Should a later documentation or benchmark ticket add a side-by-side narrative that maps conventional OrderLine rows to the DVault LinkOrderProduct plus SatOrderProductFulfillment history?
- If stakeholders later want explicit HubOrder and HubProduct DDL or full column-list assertions inside the scenario itself, should that be handled as a separate follow-up ticket instead of widening this relationship-focused story?

Risks
- If future edits remove the shared hub metadata coverage in ExplicitDataVaultSaveServiceSqliteTests.cs without replacing it, this story's lighter hub-proof posture will become under-supported.
- If future edits let the conventional and DVault variants drift to different business facts or payloads, the comparison value of the story will erode.
- Because the structure explanation currently lives in integration tests and schema assertions instead of a dedicated tutorial page, readability depends on keeping that evidence compact and intentional.

Split recommendations
- The current two-task split remains the bounded v1 plan: 06EXB7SP77MW1HVW7KT4ZFV6G8 covers the normal EF baseline and 06EXB7SY3J6160R9Q35CFN6Q1W covers the DVault link-and-satellite variant plus explicit relationship-table schema visibility.
- No additional split is recommended unless a future ticket explicitly pulls full hub DDL duplication, runnable sample-app packaging, or benchmark harness reuse out of this story.

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