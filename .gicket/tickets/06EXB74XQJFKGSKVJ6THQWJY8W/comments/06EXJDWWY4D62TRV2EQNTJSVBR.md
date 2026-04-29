[gicket-bot] PO refinement contract

Summary
- PO refinement remains blocked on missing foundation repository structure. The relation-write path was attempted for the identified foundation skeleton ticket but trust policy denied the blocks relation, so the only remaining blocking open question is the absent DVault solution/library/test evidence.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

Clarification resolution
- resolution-decision: `partially_resolved`
- critic-item-1: `answered` - Keep this ticket in PO refinement and do not hand it to development while DVault.slnx, src/DVault, and tests/DVault.Tests are absent from repository evidence.
- critic-item-2: `answered` - After foundation completion, the next PO refinement pass must refresh the contract with concrete evidence for DVault.slnx, src/DVault, and tests/DVault.Tests before any PO-critic handoff.
- critic-item-3: `cannot_answer` - A direct persisted blocks relation cannot be added in this run because the attempted gicket-add-relation call from foundation skeleton ticket 06EXB6XBV95E08R2W9ZQ1PRDPM to this metadata task was denied by trust policy. This is no longer an unanswered product decision; it is a runtime permission limitation.
- critic-item-4: `answered` - Developer handoff remains blocked. The contract should continue to require either tracked foundation evidence or a successfully persisted enforceable blocks relation before implementation starts.
- critic-item-5: `answered` - This refinement outcome should not route the task onward as ready for development. The substantive contract remains a PO-held blocker until foundation evidence appears; workflow labels and handoff metadata are runtime-managed and are not part of the product acceptance contract.

Clarifications
- This ticket stays in PO refinement and is not ready for developer handoff because the foundation solution, production project, and test project are still absent from repository evidence.
- The attempted direct blocks relation from foundation skeleton ticket 06EXB6XBV95E08R2W9ZQ1PRDPM to this metadata task failed under trust policy, so sequencing cannot currently be enforced through a persisted ticket relation.
- The metadata abstraction scope remains valid for v1 once the foundation structure exists; no product-scope split is needed now.
- The current v1 owning targets remain src/DVault for production code and tests/DVault.Tests for unit tests once those projects are created by foundation work.

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

Follow-up questions
- After foundation completion, rerun PO refinement with concrete evidence for DVault.slnx, src/DVault, and tests/DVault.Tests before returning to PO-critic.
- If ticket-relation trust policy later permits it, retry adding a direct persisted blocks relation from foundation skeleton ticket 06EXB6XBV95E08R2W9ZQ1PRDPM to this metadata task for board-level enforcement.
- Later tickets can decide whether to add specialized Data Vault constructs such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault metadata.
- Later tickets can define serialization/configuration formats and stricter naming convention enforcement beyond basic missing-input validation.

Risks
- Without a direct persisted blocker relation, sequencing depends on the ticket contract and runtime routing rather than an enforceable task-level dependency.
- Sending this ticket back to PO-critic before foundation completion would repeat the same blocking finding because current repository evidence still lacks the required structure.
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.

Split recommendations
- No split is needed for the metadata abstraction scope; keep this task blocked until the foundation solution/library/test project work is complete or directly linked as an enforceable dependency.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 6
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment