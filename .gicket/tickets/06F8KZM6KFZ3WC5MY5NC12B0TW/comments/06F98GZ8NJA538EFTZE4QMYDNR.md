[gicket-bot] PO refinement contract

Summary
- Confirmed that 06F8KZM6KFZ3WC5MY5NC12B0TW is a closure-only epic over four completed child tickets already landed on develop; the remaining action is closure-path cleanup, not a dev handoff.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This refinement moves the epic onto the closure/completion path by treating it as a closure-only roll-up with no residual developer slice and by keeping it out of the normal po-critic -> dev route. Because the allowed decision enum has no explicit closure outcome, `needs_po_clarification` is used as the schema-compatible return-to-PO signal for runtime closure/status cleanup.
- critic-item-2: `answered` - Future provider-expansion work and any consumer-facing physical-naming-override API remain new scope and must be tracked on follow-up tickets or epics, not by reopening this parent epic.
- critic-item-3: `answered` - Confirmed. This epic owns no residual developer slice. The correct next step is closure-path/status cleanup rather than a dev handoff, so this run intentionally does not mark the ticket ready for PO-critic and instead keeps it on a PO-side clarification/closure path.

Clarifications
- This epic is a closure-only traceability parent over four completed child tickets already integrated into develop; it is not a remaining dev execution ticket.
- The authoritative contract for this run keeps the epic off the po-critic -> dev path and instead requests closure/completion routing cleanup under PO because no residual developer-owned work remains.
- Future provider-expansion work, consumer physical-naming override APIs, and post-v1 schema expansion stay out of this parent and belong on new follow-up tickets or epics.
- No child tickets, relation writes, description writes, attachments, or planning documents were materialized in this run. Live gicket ticket/comment/relation reads were trust-blocked earlier in the session, so this refinement relies on the supplied ticket snapshot and repository evidence already present in context.

Scope In
- Ratify that 06F8KZMRXRHRKHV56Y96M4S90G, 06F8KZN2BBPB3XFFXEXGX4N4RG, 06F8KZNBGB8FPW6TK5A8SAJMVC, and 06F8KZNNS76TD9Z7ESB173FZ68 collectively satisfy this epic.
- Preserve epic-level traceability across the provider identifier contract, provider identifier preflight implementation/tests, migration DDL guardrail diagnostics/reporting, and v0.29.0 documentation baseline already landed on develop.
- Document that the only remaining work is closure/completion routing cleanup for this epic, not new implementation.

Scope Out
- Any dev handoff for new implementation under this epic.
- Creating replacement child tickets for work already delivered by the four done children.
- Reopening provider expansion beyond SQLite, Oracle, PostgreSQL, SQL Server, and MySQL under this parent.
- Adding a consumer-facing physical naming override API or other post-v1 schema-governance expansion under this parent.

Open questions
- Which runtime closure/completion path should this closure-only epic use so it can be finished without re-entering the normal po-critic -> dev route?

Follow-up questions
- If DVault later adds provider-specific DDL safety beyond SQLite, Oracle, PostgreSQL, SQL Server, and MySQL, should that be tracked as a new epic rather than appended to this closed parent?
- If a later release wants a consumer-facing physical naming override API, should that be opened as a separate follow-up ticket or epic instead of reopening this parent?

Risks
- If runtime workflow cleanup does not move this ticket onto a closure/completion path, automation or humans could still misroute it toward dev despite no remaining implementation scope.
- If future provider-expansion or physical-naming-override requests are attached to this parent epic, the repository could reopen already-completed scope and blur release traceability.

Split recommendations
- No new split is recommended; the existing four child tickets already cover and complete the epic scope on develop.
- Any future provider-expansion or physical-naming-override work should be created as new follow-up tickets or epics rather than as children under this closure-only roll-up.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment