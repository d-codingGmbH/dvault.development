[gicket-bot] PO refinement contract

Summary
- PO refinement remains blocked: bounded reads confirm this task still has no direct persisted foundation blocker relation and the repository branch still lacks src/tests foundation structure, so this run keeps the ticket out of PO-critic/developer handoff instead of re-approving it.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket is kept out of developer handoff in this refinement by returning needs_po_clarification. Current evidence does not show the required foundation structure, and no enforceable direct dependency is persisted on this task, so it must remain blocked until DVault.slnx, src/DVault, and tests/DVault.Tests are visible or a direct blocker relation is allowed and added.
- critic-item-2: `answered` - Routing is resolved for this run by not returning ready_for_po_critic. The current ticket labels include blocked/dev, blocked/test, and needs-po, and this handoff decision is needs_po_clarification, so the contract does not pair those blocked labels with a route toward developer handoff before the foundation exists.
- critic-item-3: `cannot_answer` - The requested post-foundation refresh cannot be completed now because the foundation is not present in the tracked repository evidence. This ticket needs a later PO refresh after DVault.slnx, src/DVault, and tests/DVault.Tests exist, with concrete repository evidence captured at that time.
- critic-item-4: `answered` - The blocking finding is accepted. The persisted contract already says implementation must wait for the foundation solution/library/test structure, and current repository inspection confirms that structure is absent, so this ticket is not ready for developer handoff.
- critic-item-5: `answered` - The blocking finding is accepted. The only persisted relation found on this task is the parentOf relation from the modeling story, with no outgoing direct blocker relation; the prior PO contract/comment says the direct blocker write was denied. This run therefore does not approve the ticket for PO-critic or developer handoff.

Clarifications
- This ticket is not ready to return to PO-critic while the foundation solution, library project, and test project remain absent from tracked repository evidence.
- This refinement supersedes the earlier ready_for_po_critic handoff language for current routing purposes; the current PO decision is needs_po_clarification.
- The metadata abstraction scope remains valid but must wait behind the foundation work or a direct persisted blocker relation that can be enforced by the ticket system.

Scope In
- Define metadata abstractions for hubs, links, and satellites in the DVault library after the foundation library project exists.
- Provide enough documented public or protected members for tests to create and inspect hub, link, and satellite metadata.
- Represent the minimum required relationships: hubs have identifying metadata, links connect two or more hub-like endpoints, and satellites are associated with a parent hub or link and descriptive metadata.
- Add focused unit tests under the foundation-provided tests/DVault.Tests project for valid construction and obvious invalid inputs.

Scope Out
- Creating DVault.slnx, csproj files, src/DVault, tests/DVault.Tests, or other foundation scaffolding.
- Database schema generation, migrations, SQL rendering, physical Data Vault deployment behavior, persistence, serialization, configuration loading, or runtime discovery.
- Advanced Data Vault variants such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault constructs.
- Developer implementation before foundation structure is present or before a direct enforceable dependency is persisted.

Open questions
- Foundation repository evidence is still missing: DVault.slnx, src/DVault, and tests/DVault.Tests are not visible in the current tracked branch evidence.
- No enforceable direct blocker relation is currently persisted from the foundation work to this task; the only visible relation is the parentOf relation from the modeling story.

Follow-up questions
- After foundation completion, rerun PO refinement with concrete evidence for DVault.slnx, src/DVault, and tests/DVault.Tests before returning to PO-critic.
- If ticket-relation writes become permitted by trust policy, add a direct persisted blocks relation from the foundation skeleton work to this metadata task for board-level enforcement.
- Later tickets can decide whether to add specialized Data Vault constructs such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault metadata.
- Later tickets can define serialization/configuration formats and stricter naming convention enforcement beyond basic missing-input validation.

Risks
- Without a direct persisted blocker relation, sequencing depends on the ticket contract and blocked routing labels rather than an enforceable task-level dependency.
- Sending this ticket back to PO-critic before foundation completion would repeat the same blocking finding because current repository evidence still lacks the required structure.
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.

Split recommendations
- No split is needed for the metadata abstraction scope; keep this task blocked until the existing foundation solution/library/test project work is complete or directly linked as an enforceable dependency.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment