[gicket-bot] PO refinement contract

Summary
- Ratified the existing four-story delivery path and explicitly set the parent epic's closure-compatible end state to a closure/tracking record with no blocked developer/tester routing labels; no new child tickets, relations, attachments, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The parent epic's closure-compatible end state is a closure/tracking item that stays at status todo while under PO and PO-critic review and carries only the steady-state labels area/ef-integration, backlog/initial-dvault, type/epic, and automation/bot-ready; blocked/dev and blocked/test are explicitly not part of the parent epic's persisted state.
- critic-item-2: `answered` - This refinement pass no longer defers routing cleanup to a downstream developer-oriented handoff. The contract itself now ratifies the closure-only label baseline and makes removal of blocked/dev and blocked/test part of the parent epic's persisted end state.

Clarifications
- 06EXB7F6WNWSJJV14EXTPSFDRG remains the closure/tracking epic over child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8, and all four were re-verified as done.
- Verified relation state still matches the intended decomposition: four outgoing parentOf relations from the epic to those child stories and one incoming relates link from 06EXB4MDREV2T51VJNJEP6R0WR.
- The parent ticket still has zero persisted attachments, and this refinement pass creates no new child tickets, relations, attachments, or planning documents.

Scope In
- Keep the parent ticket as the umbrella closure/tracking record for the Entity Framework integration and persistence MVP.
- Use existing child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8 as the complete bounded delivery path.
- Treat the existing repository implementation as the evidence base for closure: UseDataVault and ApplyDataVaultMetadata, DataVaultEfMetadataTranslator, the explicit save service, SQLite persistence coverage, and the conditional Postgres opt-in hook.
- Persist the parent epic as a closure-only ticket interpretation that excludes developer/tester routing labels from its steady-state label baseline.

Scope Out
- Any new developer-owned or tester-owned implementation on the parent epic.
- Reopening the completed EF model-building, SQLite persistence, explicit save-service, or Postgres-readiness slices already owned by the four child stories.
- First-class Postgres runtime/provider support, SaveChanges interception, and deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- New migration, tooling, CI expansion, or workflow redesign promises on this closure ticket.

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
- If automation reintroduces blocked/dev or blocked/test on closure-only epics, routing can again misclassify completed umbrella work as active execution scope.

Split recommendations
- No additional split is recommended; the authoritative delivery path is already materialized through child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8, and all four are done.
- If new work is later approved for first-class Postgres runtime support, SaveChanges interception, or deferred Data Vault capabilities, create a separate follow-up ticket or epic instead of reopening this parent closure ticket.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment