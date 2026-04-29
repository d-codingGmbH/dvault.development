[gicket-bot] PO refinement contract

Summary
- Refined the ticket into a bounded v1 modeling task for Data Vault metadata abstractions using the visible DVault source and test roots as the default layout.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the existing repository layout as the v1 baseline: production code belongs under src/DVault and tests under tests/DVault.Tests.
- This ticket covers the in-memory metadata object model only; persistence, code generation, database connectivity, and runtime loading are out of scope.
- Hub, link, and satellite metadata should expose a small documented public API sufficient for construction, validation, and inspection in tests.
- Validation should cover obvious missing or invalid required inputs such as null, empty, or whitespace names and missing required relationships.
- Implementation details such as exact helper names, constructor overloads, or file names may be chosen by the developer if they follow existing project conventions.

Scope In
- Define metadata abstractions for hubs, links, and satellites in the DVault library.
- Provide enough public or protected documented members for tests to create and inspect hub, link, and satellite metadata.
- Represent the minimum required relationships: a hub has identifying metadata, a link connects two or more hub-like endpoints, and a satellite is associated with a parent structure and contains descriptive metadata.
- Add focused unit tests in tests/DVault.Tests proving valid hub, link, and satellite metadata can be created.
- Add focused validation tests for missing or invalid required inputs.

Scope Out
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

Risks
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.
- The referenced charter standards are not expanded in the provided ticket context; developers should apply any standards already present in the repository when implementing.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment