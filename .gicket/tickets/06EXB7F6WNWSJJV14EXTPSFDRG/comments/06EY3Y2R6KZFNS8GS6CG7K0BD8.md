[gicket-bot] PO refinement contract

Summary
- The parent epic now matches its closure-only label baseline, so the clarification is resolved without new child-ticket, relation, attachment, or planning-document writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - The current ticket snapshot already shows the parent epic labels as area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready, which matches the contract's steady-state closure baseline and clears the earlier blocked/dev and blocked/test mismatch.

Clarifications
- The live parent ticket label set now matches the closure-only steady-state baseline: area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready.
- 06EXB7F6WNWSJJV14EXTPSFDRG remains the closure/tracking epic over 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8; this pass creates no new child tickets.
- Persisted relation and attachment context remains unchanged: four outgoing parentOf relations, one incoming relates link from 06EXB4MDREV2T51VJNJEP6R0WR, and zero persisted attachments.

Scope In
- Keep the parent epic as a closure/tracking record for the Entity Framework integration and persistence MVP.
- Use the existing four child stories as the full bounded delivery path with no remaining parent-owned implementation slice.
- Preserve the repository evidence already cited by the contract: UseDataVault and ApplyDataVaultMetadata, DataVaultEfMetadataTranslator, DataVaultSaveService, SQLite integration coverage, and the conditional Postgres opt-in hook.
- Maintain the live parent ticket's closure-only label baseline of area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready.

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
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment