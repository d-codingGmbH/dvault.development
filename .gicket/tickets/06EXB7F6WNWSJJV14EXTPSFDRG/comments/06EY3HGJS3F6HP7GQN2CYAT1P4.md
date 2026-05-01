[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The parent status already matches the ready-state policy because both PO and PO-critic use status todo; the conflict is the live label set, not the status value. This contract now makes the closure-only intent explicit: the parent owns no remaining developer or tester work, and runtime-managed handoff metadata must clear the stale blocked/dev, blocked/test, and PO-routing labels before the next PO-critic review.
- critic-item-2: `answered` - The contract now explicitly separates live metadata drift from product scope: the description, relation graph, child-ticket status, and repository evidence already show that no developer or tester execution remains on the parent. Re-handoff therefore depends only on runtime applying the corresponding live-field label cleanup, not on reopening scope or creating new implementation tickets.
- critic-item-3: `answered` - The contradiction is real and is now explicitly resolved in the contract: blocked/dev and blocked/test are stale routing metadata, not evidence of remaining scope. The parent epic remains a closure/tracking umbrella only, so developer- or tester-blocking labels must not survive the runtime-managed PO-to-PO-critic handoff.

Clarifications
- 06EXB7F6WNWSJJV14EXTPSFDRG remains a closure/tracking epic over existing parentOf children 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8.
- Relation state already matches the intended decomposition: the parent has four outgoing parentOf relations to those child stories and one incoming relates link from 06EXB4MDREV2T51VJNJEP6R0WR.
- All four child stories are already done, so the parent carries no remaining developer-owned or tester-owned implementation slice.
- The parent still has zero persisted attachments, and this refinement pass created no new child tickets, relations, attachments, or planning documents.

Scope In
- Keep the parent ticket as the umbrella closure/tracking record for the Entity Framework integration and persistence MVP.
- Use existing child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8 as the complete bounded delivery path.
- Treat the existing repository implementation as the evidence base for closure: EF model configuration surface, provider-neutral metadata translator, explicit save service, SQLite persistence coverage, and conditional Postgres opt-in hook.
- Keep the parent on a closure-review path rather than reopening developer or tester execution.

Scope Out
- Any new developer-owned or tester-owned implementation on the parent epic.
- Reopening the completed EF model-building, SQLite persistence, explicit save-service, or Postgres-readiness slices already owned by the four child stories.
- First-class Postgres runtime/provider support, SaveChanges interception, and deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- New migration, tooling, or CI expansion promises on this closure ticket.

Open questions
- none

Follow-up questions
- After this closure epic clears review, should first-class Postgres runtime/provider support be planned as a separate epic instead of extending 06EXB7F6WNWSJJV14EXTPSFDRG?
- Should a later API-convenience story evaluate optional wrappers or interceptors on top of IDataVaultSaveService without changing the v1 explicit-save default?
- Do any downstream tickets still need relation hygiene now that this parent epic is a closure item rather than an active developer blocker?
- If workflow governance keeps misrouting closure-only epics, should it add a distinct closure/completion route for tracking epics?

Risks
- If the parent epic wording or routing hygiene drifts back toward executable implementation scope, automation could redispatch already completed work.
- Future provider or convenience work could accidentally reopen this closure ticket instead of being split into a new follow-up epic or story.
- If workflow governance keeps closure-only tracking epics on developer-oriented paths, similar umbrella tickets may continue to bounce even when implementation is already complete.

Split recommendations
- No additional split is recommended; the authoritative delivery path is already materialized through child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8, and all four are done.
- If new work is later approved for first-class Postgres runtime support, SaveChanges interception, or deferred Data Vault capabilities, create a separate follow-up ticket or epic instead of reopening this parent closure ticket.

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