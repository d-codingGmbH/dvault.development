[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket contract is well scoped for the metadata model itself, but it assumes an existing compilable DVault project/test structure that is not present yet and is covered by unresolved foundation tickets/relations.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Current branch is ticket/06EXB74XQJFKGSKVJ6THQWJY8W-task-define-hub-link-and-satellite-metadata-abst at HEAD d61d1ac; git log shows d61d1ac as the PO-critic lease claim and 2e7bbab as the PO-to-PO-critic handoff.
- .gicket/tickets/06EXB74XQJFKGSKVJ6THQWJY8W/description.md contains the persisted Delivery Contract with Open Questions: none.
- The Delivery Contract says production code belongs under src/DVault, tests under tests/DVault.Tests, and DoD requires the implementation to compile in the existing DVault solution or project structure.
- Local repository inspection found src/DVault and tests/DVault.Tests directories, but find/git ls-files found no files under src or tests and no .slnx, .sln, .csproj, .props, or .targets files.
- .gicket/attachments/blobs/3689523bd181e246bc2d24e33351a37684aec40d2aacb4cb13c61e73fea438de defines project-wide requirements: default namespace DCoding.Data.DVault, solution format .slnx, main target .NET 10, public/protected APIs documented, and file-scoped namespaces.
- Relation .gicket/relations/SW/8W/06EXB74NRVRX18GD33CH1C12SW--06EXB74XQJFKGSKVJ6THQWJY8W--parentOf.json makes this ticket a child of Story 06EXB74NRVRX18GD33CH1C12SW.
- Relation .gicket/relations/PM/SW/06EXB6XBV95E08R2W9ZQ1PRDPM--06EXB74NRVRX18GD33CH1C12SW--blocks.json says the .NET 10 solution skeleton story blocks the modeling story.

Blocking findings
- The ticket is not ready for developer handoff because it asks the developer to compile and test within an existing DVault solution/project structure, but the repository currently has no tracked solution, project, production source, or test project files.
- The ticket's parent modeling story is explicitly blocked by the unresolved .NET 10 solution skeleton story, and the foundation child tasks that would create DVault.slnx, the library project, and test projects are still todo/needs-po.

Required PO actions
- Return the ticket to PO refinement and make sequencing explicit: either wait for the foundation tickets that create the solution/library/test projects, or add a direct blocker/dependency so this task cannot enter dev before that structure exists.
- Keep the metadata scope as-is, but revise the handoff language so it no longer implies an existing compilable solution until the foundation work is complete.

Open issues ledger
- critic-item-1 [required-po-action] Return the ticket to PO refinement and make sequencing explicit: either wait for the foundation tickets that create the solution/library/test projects, or add a direct blocker/dependency so this task cannot enter dev before that structure exists.
- critic-item-2 [required-po-action] Keep the metadata scope as-is, but revise the handoff language so it no longer implies an existing compilable solution until the foundation work is complete.
- critic-item-3 [blocking-finding] The ticket is not ready for developer handoff because it asks the developer to compile and test within an existing DVault solution/project structure, but the repository currently has no tracked solution, project, production source, or test project files.
- critic-item-4 [blocking-finding] The ticket's parent modeling story is explicitly blocked by the unresolved .NET 10 solution skeleton story, and the foundation child tasks that would create DVault.slnx, the library project, and test projects are still todo/needs-po.

Missing examples / edge cases
- No blocking metadata examples are missing once the project-structure dependency is resolved; the contract already covers valid hub/link/satellite construction, invalid names, insufficient link endpoints, and missing satellite parent relationships.

Risky assumptions
- The current contract assumes src/DVault and tests/DVault.Tests are usable owning projects, but direct repository inspection shows they are empty directories with no tracked project files.
- The contract tells developers to apply charter standards already present in the repository; those standards exist in the attachment, but the ticket should not rely on a non-existent project scaffold for enforcing them.

AC / test suggestions
- After the foundation blocker is resolved, the existing ACs are adequate for a low-assurance v1 handoff: add construction tests for hub/link/satellite metadata and validation tests for null, empty, whitespace, missing endpoints, and missing parent cases.

Implementation watchouts
- When this ticket is later approved, developers should align public namespace, target framework, file-scoped namespaces, XML documentation, and one-public/protected-member-per-file guidance with the charter attachment.
- Do not let implementation drift into persistence, SQL generation, configuration loading, or Data Vault variants that the contract explicitly scopes out.

Non-blocking notes
- The persisted Open Questions section says none, so the return is based on dependency/readiness evidence rather than unresolved product questions.
- The worktree has local line-ending-only modifications in several .gicket files, but the reviewed ticket.json and description content were readable and the relevant ticket facts were observable.

Split recommendations
- No split needed for the metadata model ticket itself; the necessary split already exists as separate foundation tickets that must precede it.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment