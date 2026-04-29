[gicket-bot] PO refinement contract

Summary
- PO refinement holds the ticket in PO clarification because the foundation solution, library project, and test project are still absent, and no ticket-relation write capability is available to add an enforceable foundation dependency.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket must remain in PO refinement and must not proceed to developer handoff until DVault.slnx, src/DVault, and tests/DVault.Tests exist in repository evidence.
- critic-item-2: `answered` - The foundation has not completed yet, so the contract cannot be refreshed with concrete solution/library/test evidence in this pass. The next PO refinement after foundation completion must verify DVault.slnx, src/DVault, and tests/DVault.Tests before PO-critic handoff.
- critic-item-3: `cannot_answer` - A direct persisted blocks dependency cannot be added in this run because the execution boundary exposes no ticket-relation write surface or declared relation-write tool. The contract keeps this as a non-implementation blocker until policy permits the relation or foundation evidence exists.
- critic-item-4: `answered` - Developer handoff remains blocked by current repository evidence: foundation paths are absent and no direct blocks relation is persisted or writable in this role boundary.
- critic-item-5: `answered` - The handoff decision is corrected to needs_po_clarification so the ticket does not route to PO-critic or development while its substantive contract blocks implementation before foundation completion.

Clarifications
- This ticket is not ready for PO-critic or developer handoff in the current repository state.
- Implementation remains blocked until DVault.slnx, src/DVault, and tests/DVault.Tests exist in tracked repository evidence or until an allowed ticket-relation write adds an enforceable foundation dependency.
- The metadata abstraction scope remains valid once the foundation project structure exists.
- No relation write was performed because the current execution boundary provides no ticket-relation write capability.

Scope In
- Define metadata abstractions for hubs, links, and satellites in the DVault library after the foundation library project exists.
- Provide enough documented public or protected members for tests to create and inspect hub, link, and satellite metadata.
- Represent minimum required relationships: hubs have identifying metadata, links connect two or more hub-like endpoints, and satellites are associated with a parent hub or link and descriptive metadata.
- Add focused unit tests under the foundation-provided tests/DVault.Tests project for valid construction and obvious invalid inputs.

Scope Out
- Creating DVault.slnx, csproj files, src/DVault, tests/DVault.Tests, or other foundation scaffolding.
- Database schema generation, migrations, SQL rendering, physical Data Vault deployment behavior, persistence, serialization, configuration loading, or runtime discovery.
- Advanced Data Vault variants such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault constructs.
- Developer implementation before foundation structure is present or before a direct enforceable dependency is persisted.

Open questions
- Foundation repository evidence is still missing: DVault.slnx is not listed, src-roots is empty, and tests/DVault.Tests is missing.
- A direct persisted blocks relation from the foundation skeleton work to this metadata task is still absent and cannot be added within the current role boundary.

Follow-up questions
- After foundation completion, rerun PO refinement with concrete evidence for DVault.slnx, src/DVault, and tests/DVault.Tests before returning to PO-critic.
- If ticket-relation writes become permitted by trust policy, add a direct persisted blocks relation from the foundation skeleton work to this metadata task for board-level enforcement.
- Later tickets can decide whether to add specialized Data Vault constructs such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault metadata.
- Later tickets can define serialization/configuration formats and stricter naming convention enforcement beyond basic missing-input validation.

Risks
- Without a direct persisted blocker relation, sequencing depends on the ticket contract and runtime routing rather than an enforceable task-level dependency.
- Sending this ticket back to PO-critic before foundation completion would repeat the same blocking finding because current repository evidence still lacks the required structure.
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.

Split recommendations
- No split is needed for the metadata abstraction scope; keep this task blocked until the existing foundation solution/library/test project work is complete or directly linked as an enforceable dependency.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 6
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment