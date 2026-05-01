[gicket-bot] PO refinement contract

Summary
- Closure-only parent epic remains valid; the critic issue is resolved by ratifying that the parent has no remaining developer or tester scope and that the earlier blocked routing labels were stale metadata rather than unfinished work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - The parent epic should continue as a closure/tracking record only. The previously verified blocked/dev and blocked/test labels were stale routing metadata that conflicted with the already-verified contract, relation graph, child-ticket completion, and repository evidence; they do not represent remaining parent-owned developer or tester work and should be cleared by runtime handoff rather than kept as a PO blocker.

Clarifications
- 06EXB7F6WNWSJJV14EXTPSFDRG remains the closure/tracking epic over existing child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8.
- Verified relation state already matches that decomposition: four outgoing parentOf relations from the epic to those child stories and one incoming relates link from 06EXB4MDREV2T51VJNJEP6R0WR.
- All four child stories are done, and the referenced repository files still match the EF model-building, explicit save-service, SQLite persistence, and opt-in Postgres-readiness baseline described in the contract.
- The parent ticket still has zero persisted attachments, and this refinement pass created no new child tickets, relations, attachments, or planning documents.

Scope In
- Keep the parent ticket as the umbrella closure/tracking record for the Entity Framework integration and persistence MVP.
- Use existing child stories 06EXB7FF1J9NR2849WKDR8DKPG, 06EXB7G6YE4X0GA0CT7EPEFMPR, 06EXB7GYQKBZ8FMQN6YDYCKATG, and 06EXB7HYG17X73GH0K535GYJH8 as the complete bounded delivery path.
- Treat the existing repository implementation as the evidence base for closure: EF model configuration surface, provider-neutral metadata translator, explicit save service, SQLite persistence coverage, and the conditional Postgres opt-in hook.
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
- Do any downstream tickets still need relation hygiene once this parent epic is fully treated as a closure item rather than an active blocker?
- If workflow governance keeps misrouting closure-only epics, should it add a distinct closure/completion route for tracking epics?

Risks
- Future provider or convenience work could accidentally reopen this closure ticket instead of being split into a separate follow-up epic or story.
- If downstream relation hygiene is not reviewed after closure-only epics advance, later tickets may continue to point at umbrella records instead of concrete open work.

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