<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- The parent epic now matches its closure-only label baseline, so the clarification is resolved without new child-ticket, relation, attachment, or planning-document writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The live parent ticket label set now matches the closure-only steady-state baseline: area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready.
- 06EXB7F6WNWSJJV14EXTPSFDRG remains the closure/tracking epic over 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8; this pass creates no new child tickets.
- Persisted relation and attachment context remains unchanged: four outgoing parentOf relations, one incoming relates link from 06EXB4MDREV2T51VJNJEP6R0WR, and zero persisted attachments.

### Scope In
- Keep the parent epic as a closure/tracking record for the Entity Framework integration and persistence MVP.
- Use the existing four child stories as the full bounded delivery path with no remaining parent-owned implementation slice.
- Preserve the repository evidence already cited by the contract: UseDataVault and ApplyDataVaultMetadata, DataVaultEfMetadataTranslator, DataVaultSaveService, SQLite integration coverage, and the conditional Postgres opt-in hook.
- Maintain the live parent ticket's closure-only label baseline of area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready.

### Scope Out
- Any new developer-owned or tester-owned implementation on the parent epic.
- Reopening the completed EF model-building, SQLite persistence, explicit save-service, or Postgres-readiness slices already owned by the four child stories.
- First-class Postgres runtime/provider support, SaveChanges interception, and deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- Using this parent epic to absorb future implementation follow-up instead of creating separate downstream tickets.

## Acceptance Criteria
- The parent epic is explicitly treated as a closure/tracking item and not as a developer- or tester-executable ticket.
- The live parent ticket label set matches the closure-only baseline area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready, with no developer/tester blocking labels on the parent epic.
- The parent ticket continues to identify 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8 as the full bounded delivery path, with no remaining implementation slice left on the parent.
- Repository evidence remains aligned with closure of that path: UseDataVault and ApplyDataVaultMetadata plus DataVaultEfMetadataTranslator cover the EF surface, DataVaultSaveService and the explicit save-service architecture note cover the write boundary, SQLite integration tests cover persistence behavior, and the integration test project plus README preserve the conditional Postgres hook.

## Definition of Done
- The contract and the live parent ticket fields no longer contradict each other about the closure-only label baseline.
- The verified outgoing parentOf relations to the four child stories remain the authoritative decomposition of the epic.
- All four child stories remain the complete bounded delivery path, and current repository evidence shows no remaining parent-owned implementation slice.
- No new child tickets, relations, attachments, or planning documents are required for this refinement pass.

## Implementation Notes
- Use src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs as the concrete EF model-building and metadata-translation evidence already cited by the contract.
- Use src/DCoding.Data.DVault/DataVaultSaveService.cs plus docs/architecture/dvault-v1-explicit-save-service.md as evidence that IDataVaultSaveService remains the v1 explicit write boundary instead of SaveChanges interception.
- Use tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs plus tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj and README.md as bounded evidence for SQLite persistence behavior and the opt-in Postgres hook.
- No child-ticket, relation, attachment, or planning-document writes are required in this pass because the split, relation structure, attachment state, and supporting repository evidence were already materialized, and the live parent ticket labels now match the closure-only baseline.

## Open Questions
- none

## Follow-Up Questions
- After this closure epic clears review, should first-class Postgres runtime/provider support be planned as a separate epic instead of extending 06EXB7F6WNWSJJV14EXTPSFDRG?
- Should a later API-convenience story evaluate optional wrappers or interceptors on top of IDataVaultSaveService without changing the v1 explicit-save default?
- Do any downstream tickets still need relation hygiene once this parent epic is fully treated as a closure item rather than an active blocker?
- If workflow governance keeps misrouting closure-only epics, should it add a distinct closure/completion route for tracking epics?

## Risks
- Future provider or convenience work could accidentally reopen this closure ticket instead of being split into a separate follow-up epic or story.
- If downstream relation hygiene is not reviewed after closure-only epics advance, later tickets may continue to point at umbrella records instead of concrete open work.

## Split Recommendations
- No additional split is recommended; the authoritative delivery path is already materialized through child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8.
- If new work is later approved for first-class Postgres runtime support, SaveChanges interception, or deferred Data Vault capabilities, create separate follow-up tickets or epics instead of reopening this parent closure ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Connect the Data Vault modeling core to Entity Framework and implement the first persistence MVP.

## Scope
- Integrate with EF model building and persistence.
- Support Sqlite for examples and tests, with Postgres readiness hooks.

## Acceptance Criteria
- A consuming DbContext can opt into DVault persistence with minimal setup.
- Sqlite integration tests demonstrate hubs, links, and satellites.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.