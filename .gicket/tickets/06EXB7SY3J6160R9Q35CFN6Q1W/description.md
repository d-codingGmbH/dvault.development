<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around a bounded SQLite-backed order-product scenario that reuses the existing DVault link, satellite, naming, and explicit save-service primitives; no ticket split is needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already has generic v1 link and satellite infrastructure in DataVaultEfMetadataTranslator and IDataVaultSaveService/DataVaultSaveRequest, so this ticket is about applying that baseline to the order-product scenario, not inventing a new persistence model.
- Per the MVP concepts, relationship history belongs in a satellite attached to the order-product link; order and product remain hubs.
- The current bounded example surface is the existing integration-test/documentation area, because examples/ is still empty except for .gitkeep.

### Scope In
- Model the order/product scenario as order and product hubs plus an order-product link.
- Add one understandable link-satellite history scenario for relationship context that can change over time.
- Prove the scenario against the current SQLite-focused v1 baseline and generated table conventions.
- Keep the scenario readable enough to serve as documentation through tests and any adjacent governed docs that are actually needed.

### Scope Out
- New generic Data Vault API design beyond the existing explicit save-service and metadata-translation surfaces.
- PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- Provider-neutral concurrency or upsert behavior beyond the current SQLite v1 reuse rules.
- A separate runnable example application under examples/.

## Acceptance Criteria
- The order/product example uses two hubs, one order-product link, and a satellite attached to that link for relationship context/history.
- Integration coverage persists at least two distinct historical versions for the same order-product relationship and shows that both versions remain queryable or visible through the generated SQLite tables.
- The generated schema or table assertions visibly include the relationship link table and its satellite table with the expected technical metadata shape for the current naming and persistence conventions.
- The scenario stays documentation-friendly by using a small, easily explained business narrative instead of an overly abstract or provider-specific example.

## Definition of Done
- Repository changes satisfy the acceptance criteria using the current DCoding.Data.DVault solution/test layout and shared implementation standards.
- Relevant automated tests are added or updated under the existing test surface and pass with dotnet test.
- Formatting and governed text checks continue to pass with bash tools/check-format.sh.
- Any supporting documentation added for readability stays aligned with the MVP concepts, default naming policy, stable hashing contract, and v1 persistence-convention references instead of redefining them locally.

## Implementation Notes
- Use the existing explicit IDataVaultSaveService boundary rather than introducing SaveChanges interception or another write path.
- Keep the provider baseline at DataVaultProviderCapabilityProfiles.Sqlite; timestamps should remain UTC-normalized and SQLite-friendly.
- The scenario should demonstrate link-history behavior through a changed satellite payload/hash diff for the same link parent, because the current save service suppresses only unchanged latest satellite payloads.
- Prefer extending the existing order/product integration scenario and nearby snapshot/assertion coverage instead of spreading the example across new repository surfaces.
- Reuse the established generated naming and technical-column conventions already enforced by the translator and related tests.

## Open Questions
- none

## Follow-Up Questions
- After the MVP example baseline lands, decide whether the same scenario should later be promoted from test-backed documentation into a runnable examples/ project.
- If future documentation needs more advanced relationship-navigation ergonomics, evaluate a separate follow-up ticket for PIT or bridge patterns rather than expanding this MVP story.

## Risks
- If the chosen relationship payload does not change in a clear, human-readable way, the example may technically show satellite writes without convincingly showing why link history matters.
- Because the current example surface is test-backed rather than a runnable sample app, readability can regress if the implementation prioritizes infrastructure detail over a compact business story.

## Split Recommendations
- No split recommended; repository evidence shows the underlying link, satellite, and explicit save-service primitives already exist, so the remaining work is one bounded order-product scenario.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Create the DVault-backed relationship scenario.

## Scope
- Use a link for order-product relationships and satellites where useful.

## Acceptance Criteria
- Relationship history is visible in generated tables.
- The example remains understandable for documentation.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.