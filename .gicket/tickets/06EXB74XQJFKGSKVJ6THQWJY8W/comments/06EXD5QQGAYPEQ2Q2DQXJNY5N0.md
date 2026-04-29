[gicket-bot] PO refinement contract

Summary
- Refined the contract to keep the metadata abstraction scope while making foundation sequencing explicit. No child tickets, relations, or planning documents were created; the attempted direct foundation blocker relation was denied by local trust policy, so the contract uses the wait-for-foundation path.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Sequencing is now explicit: this metadata task must wait for the foundation work that creates the .NET solution, src/DVault library project, and tests/DVault.Tests test project before product-code development starts. A direct blocker relation was attempted but was not persisted because the local trust policy denied the relation write, so the refined contract carries the wait-for-foundation requirement instead.
- critic-item-2: `answered` - The metadata scope remains unchanged, but the handoff language no longer implies an existing compilable DVault solution. References to src/DVault and tests/DVault.Tests are target locations supplied by the foundation work, not current repository structure.
- critic-item-3: `answered` - The blocker is acknowledged and reflected in the contract: the repository currently has no tracked solution, project, production source, or test project files, so compile/test expectations apply only after the foundation structure exists.
- critic-item-4: `answered` - The parent modeling story remains downstream of the unresolved .NET 10 solution skeleton work. This ticket is not ready for developer implementation until the foundation child tasks that create DVault.slnx, the library project, and test projects have landed.

Clarifications
- This PO handoff is only for PO-critic review; it is not a developer-start signal while the foundation structure is absent.
- This ticket must wait for the foundation work that creates the tracked .NET solution, src/DVault library project, and tests/DVault.Tests test project before implementation begins.
- References to src/DVault and tests/DVault.Tests are target project locations expected after foundation completion, not evidence that those projects currently exist.
- This ticket covers the in-memory metadata object model only; persistence, code generation, database connectivity, runtime loading, and project scaffolding are out of scope.
- Hub, link, and satellite metadata should expose a small documented public API sufficient for construction, validation, and inspection in tests.

Scope In
- Define metadata abstractions for hubs, links, and satellites in the DVault library after the foundation library project exists.
- Provide enough public or protected documented members for tests to create and inspect hub, link, and satellite metadata.
- Represent the minimum required relationships: a hub has identifying metadata, a link connects two or more hub-like endpoints, and a satellite is associated with a parent structure and contains descriptive metadata.
- Add focused unit tests under the foundation-provided tests/DVault.Tests project proving valid hub, link, and satellite metadata can be created.
- Add focused validation tests for missing or invalid required inputs.

Scope Out
- Creating DVault.slnx, csproj files, src/DVault, tests/DVault.Tests, or other foundation scaffolding.
- Database schema generation, migrations, SQL rendering, or physical Data Vault deployment behavior.
- Persistence, serialization formats, configuration loading, or discovery from external files.
- Advanced Data Vault modeling variants such as effectivity satellites, multi-active satellites, bridge tables, PIT tables, or business vault constructs.
- Large framework-level abstractions beyond what is needed to model hub, link, and satellite metadata for this ticket.

Open questions
- none

Follow-up questions
- Should later tickets add specialized Data Vault constructs such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault metadata?
- Should later tickets define serialization or configuration formats for metadata definitions?
- Should later tickets define naming convention enforcement beyond basic missing-input validation?
- If ticket-relation writes are later permitted by trust policy, should a direct persisted blocker relation be added from the foundation skeleton ticket to this metadata task for board-level enforcement?

Risks
- Because the direct blocker relation write was denied by local trust policy, sequencing is expressed in the refined contract rather than enforced by a newly persisted relation from this run.
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.
- The referenced charter standards are not expanded in the provided ticket context; developers should apply any standards already present in the repository when implementing after the foundation projects exist.

Split recommendations
- No split is needed for the metadata abstraction scope; the required sequencing is handled by waiting for the existing foundation solution/library/test project work rather than creating another child ticket from this task.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 6
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment