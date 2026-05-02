[gicket-bot] PO refinement contract

Summary
- Refined the ticket around a bounded SQLite-backed order-product scenario that reuses the existing DVault link, satellite, naming, and explicit save-service primitives; no ticket split is needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already has generic v1 link and satellite infrastructure in DataVaultEfMetadataTranslator and IDataVaultSaveService/DataVaultSaveRequest, so this ticket is about applying that baseline to the order-product scenario, not inventing a new persistence model.
- Per the MVP concepts, relationship history belongs in a satellite attached to the order-product link; order and product remain hubs.
- The current bounded example surface is the existing integration-test/documentation area, because examples/ is still empty except for .gitkeep.

Scope In
- Model the order/product scenario as order and product hubs plus an order-product link.
- Add one understandable link-satellite history scenario for relationship context that can change over time.
- Prove the scenario against the current SQLite-focused v1 baseline and generated table conventions.
- Keep the scenario readable enough to serve as documentation through tests and any adjacent governed docs that are actually needed.

Scope Out
- New generic Data Vault API design beyond the existing explicit save-service and metadata-translation surfaces.
- PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- Provider-neutral concurrency or upsert behavior beyond the current SQLite v1 reuse rules.
- A separate runnable example application under examples/.

Open questions
- none

Follow-up questions
- After the MVP example baseline lands, decide whether the same scenario should later be promoted from test-backed documentation into a runnable examples/ project.
- If future documentation needs more advanced relationship-navigation ergonomics, evaluate a separate follow-up ticket for PIT or bridge patterns rather than expanding this MVP story.

Risks
- If the chosen relationship payload does not change in a clear, human-readable way, the example may technically show satellite writes without convincingly showing why link history matters.
- Because the current example surface is test-backed rather than a runnable sample app, readability can regress if the implementation prioritizes infrastructure detail over a compact business story.

Split recommendations
- No split recommended; repository evidence shows the underlying link, satellite, and explicit save-service primitives already exist, so the remaining work is one bounded order-product scenario.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment