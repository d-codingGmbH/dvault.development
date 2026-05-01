[gicket-bot] PO refinement contract

Summary
- Reconfirmed the existing four-story closure path, four outgoing parentOf relations, one incoming relates link, zero attachments, and the repository evidence already cited by the ticket contract; no new child tickets, relations, attachments, or planning documents were created, but the live parent ticket still carries blocked/dev and blocked/test, so it is not ready to return to PO-critic.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Not yet satisfied. The contract's steady-state parent baseline is the closure-only label set area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready, but the live ticket read still includes blocked/dev and blocked/test, so the field state does not yet match the contract and the ticket should remain with PO until those labels are reconciled.

Clarifications
- 06EXB7F6WNWSJJV14EXTPSFDRG still serves as the closure/tracking epic over 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8; this pass creates no new child tickets.
- gicket-read-ticket-relations revalidated four outgoing parentOf relations from the epic to those child stories and one incoming relates link from 06EXB4MDREV2T51VJNJEP6R0WR; no relation writes were needed.
- gicket-read-ticket-attachments returned zero persisted attachments; no attachment or planning-document writes were needed.

Scope In
- Keep the parent epic as a closure/tracking record for the Entity Framework integration and persistence MVP.
- Use the existing four child stories as the full bounded delivery path with no remaining parent-owned implementation slice.
- Use the repository evidence already cited in the ticket contract: UseDataVault and ApplyDataVaultMetadata, DataVaultEfMetadataTranslator, DataVaultSaveService, SQLite integration coverage, and the conditional Postgres opt-in hook.
- Align the live parent ticket labels to the closure-only steady-state baseline of area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready before another PO-critic pass.

Scope Out
- Any new developer-owned or tester-owned implementation on the parent epic.
- Reopening the completed EF model-building, SQLite persistence, explicit save-service, or Postgres-readiness slices already owned by the four child stories.
- First-class Postgres runtime/provider support, SaveChanges interception, and deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- Using this parent epic to absorb future implementation follow-up instead of creating separate downstream tickets.

Open questions
- none

Follow-up questions
- After this closure epic clears review, should first-class Postgres runtime/provider support be planned as a separate epic instead of extending 06EXB7F6WNWSJJV14EXTPSFDRG?
- Should a later API-convenience story evaluate optional wrappers or interceptors on top of IDataVaultSaveService without changing the v1 explicit-save default?
- Do any downstream tickets still need relation hygiene once this parent epic is fully treated as a closure item rather than an active blocker?
- If workflow governance keeps misrouting closure-only epics, should it add a distinct closure/completion route for tracking epics?

Risks
- Future provider or convenience work could accidentally reopen this closure ticket instead of being split into a separate follow-up epic or story.
- If downstream relation hygiene is not reviewed after closure-only epics advance, later tickets may continue to point at umbrella records instead of concrete open work.

Split recommendations
- No additional split is recommended; the authoritative delivery path is already materialized through child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8.
- If new work is later approved for first-class Postgres runtime support, SaveChanges interception, or deferred Data Vault capabilities, create separate follow-up tickets or epics instead of reopening this parent closure ticket.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment