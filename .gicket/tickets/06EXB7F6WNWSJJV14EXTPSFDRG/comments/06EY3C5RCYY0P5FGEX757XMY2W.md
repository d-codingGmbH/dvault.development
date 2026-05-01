[gicket-bot] PO refinement contract

Summary
- Confirmed 06EXB7F6WNWSJJV14EXTPSFDRG remains a closure/tracking epic over four existing child stories, with explicit ticket-level handoff back to PO-critic and no new split, relation, attachment, or planning document needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The parent contract already reframes 06EXB7F6WNWSJJV14EXTPSFDRG as a closure/tracking epic with no remaining developer-owned or tester-owned slice. The only conflicting signals in the live snapshot are stale workflow labels from the return loop (`blocked/dev`, `blocked/test`, `needs-po`), so the accepted closure handoff should clear those runtime signals instead of sending the parent back to development or test.
- critic-item-2: `answered` - The next automation path is explicit at the ticket level: the delivery contract handoff is `ready_for_po_critic`, Scope In routes the parent toward closure review instead of a fresh developer handoff, and the Acceptance Criteria plus Definition of Done say no developer-executable parent slice remains.
- critic-item-3: `answered` - The current supported route for this parent is closure-oriented PO-critic review, not dev. Ticket-level evidence shows no remaining implementation slice, and the prior PO handoff already targeted `po-critic`; if workflow governance later needs a distinct closure-only route for tracking epics, that is separate follow-up work rather than a blocker for this parent contract.

Clarifications
- 06EXB7F6WNWSJJV14EXTPSFDRG remains a closure/tracking epic over existing `parentOf` children 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8.
- Verified relation state already matches the intended decomposition: the parent has four outgoing `parentOf` relations to those child stories and one incoming `relates` link from 06EXB4MDREV2T51VJNJEP6R0WR.
- The live parent snapshot still carries returned-workflow labels, but the ticket contract itself contains no remaining developer or tester-owned slice.
- No new child tickets, relations, attachments, or planning documents were created in this pass; the parent also still has zero persisted attachments.

Scope In
- Keep the parent ticket as the umbrella closure/tracking record for the Entity Framework integration and persistence MVP.
- Use the verified existing `parentOf` children 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8 as the complete bounded delivery path.
- Treat the existing repository implementation as the evidence base for closure: EF model configuration surface, provider-neutral metadata translator, explicit save service, SQLite persistence coverage, and conditional Postgres opt-in hook.
- Make the parent ticket-level handoff explicit as closure review rather than developer execution.

Scope Out
- Any new developer-owned implementation on the parent epic.
- Reopening the completed EF model-building, SQLite persistence, explicit save-service, or Postgres-readiness slices already owned by the four child stories.
- First-class Postgres runtime/provider support, SaveChanges interception, and deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- New migration, tooling, or CI expansion promises on this closure ticket.

Open questions
- none

Follow-up questions
- After this closure epic clears review, should first-class Postgres runtime/provider support be planned as a separate epic instead of extending 06EXB7F6WNWSJJV14EXTPSFDRG?
- Should a later API-convenience story evaluate optional wrappers or interceptors on top of `IDataVaultSaveService` without changing the v1 explicit-save default?
- Do any downstream tickets still need relation hygiene now that this parent epic is a closure item rather than an active developer blocker?
- If automation still struggles to keep closure-only epics off the dev path after this contract is accepted, should workflow governance add a distinct closure/completion route for tracking epics?

Risks
- If the parent epic wording drifts back toward executable implementation scope, automation could redispatch already completed work.
- Future provider or convenience work could accidentally reopen this closure ticket instead of being split into a new follow-up epic or story.

Split recommendations
- No additional split is recommended; the authoritative delivery path is already materialized through child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8.
- If new work is later approved for first-class Postgres runtime support, SaveChanges interception, or deferred Data Vault capabilities, create a separate follow-up ticket or epic instead of reopening this parent closure ticket.

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