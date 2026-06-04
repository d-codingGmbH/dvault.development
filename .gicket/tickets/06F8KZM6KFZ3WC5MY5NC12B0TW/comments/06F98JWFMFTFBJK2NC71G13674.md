[gicket-bot] PO refinement contract

Summary
- Clarification resolved: this epic is a closure-only roll-up over four completed child tickets, and runtime should keep it on the PO-owned closure/completion path (return_to_po semantics) instead of routing it toward dev.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - The correct path is PO-owned closure/completion cleanup. In the current workflow semantics that maps to return_to_po, not the normal po-critic -> dev route. Runtime-managed routing and status cleanup should happen after this contract is accepted, and no new developer slice is created.
- critic-item-2: `answered` - Future provider-expansion work, consumer physical-naming override APIs, and post-v1 schema expansion stay out of this parent and must be tracked on new follow-up tickets or epics rather than reopening this epic.
- critic-item-3: `answered` - Confirmed. This epic owns no residual developer slice because the four named child tickets already cover the provider identifier contract, provider identifier preflight implementation and tests, migration DDL guardrail diagnostics and reporting, and the v0.29.0 documentation baseline already landed on develop. The correct next step is PO-owned closure cleanup, which under the current decision semantics maps to return_to_po.

Clarifications
- This epic remains a traceability parent over completed child tickets 06F8KZMRXRHRKHV56Y96M4S90G, 06F8KZN2BBPB3XFFXEXGX4N4RG, 06F8KZNBGB8FPW6TK5A8SAJMVC, and 06F8KZNNS76TD9Z7ESB173FZ68; it does not reopen implementation scope.
- No child-ticket creation, relation writes, description writes, attachments, or planning-document writes were materialized in this clarification pass; the contract relies on the supplied ticket snapshot, referenced repository documents, and bounded repository evidence already in context.

Scope In
- Ratify that 06F8KZMRXRHRKHV56Y96M4S90G, 06F8KZN2BBPB3XFFXEXGX4N4RG, 06F8KZNBGB8FPW6TK5A8SAJMVC, and 06F8KZNNS76TD9Z7ESB173FZ68 collectively satisfy this epic.
- Preserve epic-level traceability across the provider identifier contract, provider identifier preflight implementation and tests, migration DDL guardrail diagnostics and reporting, and the v0.29.0 documentation baseline already landed on develop.
- Limit remaining PO work to closure and completion cleanup for this parent epic; no new product implementation is in scope.

Scope Out
- Any dev handoff or new implementation under this epic.
- Creating replacement child tickets for work already delivered by the four completed children.
- Reopening provider expansion beyond SQLite, Oracle, PostgreSQL, SQL Server, and MySQL under this parent.
- Adding a consumer-facing physical naming override API or other post-v1 schema-governance expansion under this parent.

Open questions
- none

Follow-up questions
- If DVault later adds provider-specific DDL safety beyond SQLite, Oracle, PostgreSQL, SQL Server, and MySQL, should that be tracked as a new epic rather than appended to this closed parent?
- If a later release wants a consumer-facing physical naming override API, should that be opened as a separate follow-up ticket or epic instead of reopening this parent?

Risks
- If future provider-expansion or physical-naming-override work is attached to this parent epic instead of new follow-up tickets, release traceability will blur and completed scope could be reopened unintentionally.

Split recommendations
- No new split is recommended; the existing four completed child tickets already cover the epic scope.
- Any future provider-expansion or physical-naming-override work should be created as new follow-up tickets or epics rather than as children under this closure-only parent.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment