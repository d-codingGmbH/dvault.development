[gicket-bot] PO refinement contract

Summary
- Reframed 06EXB7F6WNWSJJV14EXTPSFDRG as a closure/tracking epic over four done child stories; repository and ticket evidence already show the EF surface, explicit save pipeline, SQLite coverage, and Postgres opt-in readiness hook, so no developer-owned parent slice remains.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - 06EXB7F6WNWSJJV14EXTPSFDRG is now treated as a tracking/closure epic only. The parent contract should explicitly state that implementation was fully decomposed into the four existing child stories and that the parent itself must not be sent to development.
- critic-item-2: `answered` - No further implementation is intended on the parent epic. All four existing child stories are already done and repository evidence already covers the shipped EF/persistence surface, so no new child ticket was created in this pass.
- critic-item-3: `answered` - The handoff is aligned by changing this contract to a PO-critic closure path instead of a developer handoff. Current ticket state still carries needs-po and blocked/dev metadata from the return, but runtime-managed status and label updates should keep the parent off the dev path once this tracking/closure contract is accepted.
- critic-item-4: `answered` - Confirmed. The parent epic does not own a live developer slice; its only delivery path is the four linked child stories and each child story is already done.
- critic-item-5: `answered` - Confirmed. The repository already contains the EF model-building entry points, provider-neutral EF translator, explicit IDataVaultSaveService write boundary, SQLite persistence integration tests, and the Postgres opt-in test hook. That makes this a workflow/closure decision on the parent epic, not a developer implementation handoff.

Clarifications
- 06EXB7F6WNWSJJV14EXTPSFDRG is a tracking/closure epic over existing child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8.
- There is no remaining developer-owned slice on the parent epic; direct ticket reads show all four child stories are already done.
- Repository evidence already covers the parent epic's intended outcomes through DataVaultModelBuilderExtensions, DataVaultEfMetadataTranslator, DataVaultSaveService, ExplicitDataVaultSaveServiceSqliteTests, and the conditional Postgres integration-test surface.
- No child tickets, relations, attachments, or planning documents were created in this refinement pass because the existing decomposition and repository evidence already close the implementation path.

Scope In
- Keep the parent ticket as the umbrella/tracking and closure record for the Entity Framework integration and persistence MVP.
- Use the existing parentOf children 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8 as the authoritative completed delivery path.
- Record that the repository already contains the completed EF metadata projection, explicit save-service path, SQLite validation surface, and Postgres opt-in readiness hook represented by those child stories.
- Route the parent epic toward closure review instead of a fresh developer handoff.

Scope Out
- Any new developer-owned implementation on the parent epic.
- Reopening the completed EF model-building, SQLite schema, explicit save-pipeline, or Postgres-readiness slices already owned by the four done child stories.
- Full Postgres runtime/provider support, SaveChanges interception, and deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimization.
- New migration/tooling promises or unrelated CI/workflow expansion on this parent closure ticket.

Open questions
- none

Follow-up questions
- After this closure epic finishes review, should first-class Postgres runtime/provider support be planned as a separate epic instead of extending 06EXB7F6WNWSJJV14EXTPSFDRG?
- Should a later API-convenience story evaluate optional wrappers or interceptors on top of IDataVaultSaveService without changing the v1 explicit-save default?
- Do any downstream tickets still need relation hygiene now that this parent epic is a closure item rather than an active developer blocker?

Risks
- If the parent epic wording drifts back toward executable implementation scope, automation can route already-completed work back to development.
- Stale blocked/dev or blocked/test workflow metadata on the parent could misroute automation until runtime applies the updated closure-oriented handoff.
- Future provider work could accidentally reopen this closure ticket instead of being split into a new provider-specific epic.

Split recommendations
- No additional split is recommended for this parent epic; the authoritative delivery path is already materialized and complete through child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8.
- If new work is later approved for first-class Postgres runtime support, SaveChanges interception, or deferred Data Vault capabilities, create a separate follow-up ticket or epic instead of reopening this closure ticket.

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