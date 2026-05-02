[gicket-bot] PO refinement contract

Summary
- Refined the story around the already-materialized two-task split: 06EXB7SP77MW1HVW7KT4ZFV6G8 covers the normal EF baseline, 06EXB7SY3J6160R9Q35CFN6Q1W covers the DVault link-and-satellite variant, and repository evidence fixes the execution surface at tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs; no new child tickets, relations, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Persisted relation state already matches the intended decomposition: epic 06EXB7QPAXMRV0AVQGSXQT13MC is parentOf this story, this story is parentOf task 06EXB7SP77MW1HVW7KT4ZFV6G8 for the conventional EF baseline and task 06EXB7SY3J6160R9Q35CFN6Q1W for the DVault variant, and story 06EXB7G6YE4X0GA0CT7EPEFMPR still has a blocks relation into this story even though that source story is already done.
- Repository evidence fixes the v1 example surface at tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs rather than examples/ or a standalone sample app.
- The bounded comparison model is already clear in repo/test evidence: conventional EF uses Order, Product, and OrderLine, while the DVault side uses Order and Product hubs, an OrderProduct link, and a Fulfillment satellite attached to that link.
- Ticket comments contain only runtime relation-follow-up and claim/lease notes, with no human scope changes to incorporate, and the ticket currently has no persisted attachments.

Scope In
- Deliver one small SQLite-backed order/product comparison scenario that keeps the normal EF and DVault variants aligned to the same business narrative.
- Keep the conventional side as ordinary EF Core Order, Product, and OrderLine entity mapping in the existing integration-test surface.
- Keep the DVault side as Order and Product hubs, one OrderProduct link, and a link satellite that records relationship-context history over time.
- Show the generated table/column shape and persisted history clearly enough that the scenario doubles as documentation-through-tests.

Scope Out
- Do not introduce a separate runnable sample application under examples/ for this story.
- Do not widen the story into new generic persistence APIs beyond the existing ApplyDataVaultMetadata, AddDVault, and IDataVaultSaveService surfaces.
- Do not add PIT tables, bridge tables, multi-active satellites, provider-specific optimizations, or non-SQLite baseline requirements.
- Do not turn this story into benchmark harness work or broader documentation packaging that belongs to later epic follow-up.

Open questions
- none

Follow-up questions
- Should a later documentation ticket promote this test-backed scenario into a runnable sample under examples/ once the MVP example set stabilizes?
- Should a later documentation or benchmark ticket add a side-by-side narrative that maps conventional OrderLine rows to the DVault OrderProduct link plus Fulfillment satellite history?
- If more advanced relationship-navigation examples are needed later, should they be handled as separate PIT or bridge follow-up tickets rather than expanding this story?

Risks
- If future edits let the conventional and DVault variants drift to different business facts or payloads, the comparison value of the story will erode.
- Because the structure explanation currently lives in integration tests and schema assertions instead of a dedicated tutorial page, readability depends on keeping that evidence compact and intentional.
- A reader may expect a standalone sample application even though the bounded v1 delivery surface is test-backed documentation rather than a public example app.

Split recommendations
- The story is already split appropriately: 06EXB7SP77MW1HVW7KT4ZFV6G8 covers the normal EF baseline and 06EXB7SY3J6160R9Q35CFN6Q1W covers the DVault link-and-satellite variant.
- No further split is recommended unless a future ticket explicitly separates standalone sample-app packaging or benchmark harness reuse from the current test-backed example surface.

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