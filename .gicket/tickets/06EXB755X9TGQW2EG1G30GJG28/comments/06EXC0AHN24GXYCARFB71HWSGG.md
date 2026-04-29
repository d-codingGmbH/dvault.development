[gicket-bot] PO-critic review contract

Summary
- Return to PO: the refined contract has no open questions, but it depends on a source/test project layout and concrete default naming that are not currently established in the repository or ticket dependencies.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB755X9TGQW2EG1G30GJG28/description.md contains ## Open Questions with '- none' and PO Handoff decision ready_for_po_critic.
- The delivery contract says V1 should cover src/DVault and tests/DVault.Tests, requires focused tests in tests/DVault.Tests, and says implementation follows the existing layout under src/DVault and tests/DVault.Tests.
- git ls-tree -r --name-only HEAD -- src tests produced no tracked src or tests files; rg --files --hidden src tests also produced no files.
- find src/DVault tests/DVault.Tests -maxdepth 4 listed only the two empty directories: src/DVault and tests/DVault.Tests.
- .gicket/relations/SW/28/06EXB74NRVRX18GD33CH1C12SW--06EXB755X9TGQW2EG1G30GJG28--parentOf.json only shows this ticket as a child of the modeling story; observed relation files did not show this ticket blocked by the pending foundation/test setup tickets.

Blocking findings
- The contract sends development to src/DVault and tests/DVault.Tests as if an existing source/test layout is ready, but direct branch inspection shows no tracked source or test project files there. This creates an implicit setup dependency or hidden scaffolding scope that the ticket does not declare.
- The contract requires testable default column names but only says they should be conventional Data Vault terminology; with the convention-policy/naming tickets still todo/needs-po, the expected v1 default names are not concrete enough for objective acceptance.

Required PO actions
- Revise the ticket to either declare dependency/order against the source/test scaffolding and project setup tickets, or explicitly include the minimum project/test scaffolding needed for this work.
- Replace the vague default-name language with explicit v1 default effective column names for hash key, hash diff, load timestamp, and record source, or explicitly state that the developer owns those defaults and update acceptance criteria accordingly.
- Update the PO handoff/comment so developer routing is based on the corrected scope, dependencies, and acceptance checks.

Open issues ledger
- critic-item-1 [required-po-action] Revise the ticket to either declare dependency/order against the source/test scaffolding and project setup tickets, or explicitly include the minimum project/test scaffolding needed for this work.
- critic-item-2 [required-po-action] Replace the vague default-name language with explicit v1 default effective column names for hash key, hash diff, load timestamp, and record source, or explicitly state that the developer owns those defaults and update acceptance criteria accordingly.
- critic-item-3 [required-po-action] Update the PO handoff/comment so developer routing is based on the corrected scope, dependencies, and acceptance checks.
- critic-item-4 [blocking-finding] The contract sends development to src/DVault and tests/DVault.Tests as if an existing source/test layout is ready, but direct branch inspection shows no tracked source or test project files there. This creates an implicit setup dependency or hidden scaffolding scope that the ticket does not declare.
- critic-item-5 [blocking-finding] The contract requires testable default column names but only says they should be conventional Data Vault terminology; with the convention-policy/naming tickets still todo/needs-po, the expected v1 default names are not concrete enough for objective acceptance.

Missing examples / edge cases
- One example contract for each v1 role showing role, default name, effective name, semantic purpose, requiredness expectation, and overrideability.
- An override example for each role showing that only the effective column name changes while role identity and default name remain stable.
- Clarification of whether requiredness expectations are global per role or differ by hub, link, and satellite usage.

Risky assumptions
- Assuming empty local src/DVault and tests/DVault.Tests directories are sufficient developer-ready project layout.
- Assuming pending convention-policy or deterministic-naming tickets will define the default metadata names before this ticket is implemented.
- Assuming downstream hub/link/satellite tickets will adapt to this contract without an explicit compatibility or dependency note.

AC / test suggestions
- Acceptance criteria should name the exact four default column names that tests must assert.
- Tests should assert the complete default set contains exactly the four v1 roles and that each role has distinct role/default/effective-name metadata.
- Override tests should cover all four roles and assert the role and default name are preserved while effective name changes.

Implementation watchouts
- Do not let this ticket silently absorb unrelated solution, library, or test-project setup unless PO intentionally adds that scope.
- Do not expand into DDL, hub/link/satellite modeling, PIT/bridge behavior, or broad naming-policy work beyond the four-role contract.
- Any compatibility with an existing public API/type must be based on direct source evidence; no such public API was observed under tracked src/tests files in this branch.

Non-blocking notes
- The persisted delivery contract has ## Open Questions set to '- none', so the explicit open-question gate is satisfied.
- Recent comments show PO refinement and handoff to po-critic, including comment 06EXBY9KJAF88GB7VK170BAKJM and run report 06EXBYADSA8NTVBPPKVBDNHRJ4.

Split recommendations
- Keep this as the metadata contract ticket after the source/test project setup dependency is resolved; route scaffolding to the existing foundation/test tickets if those remain separate.
- If exact physical default names are meant to be decided by convention policy rather than this ticket, split or block this ticket on the existing convention/naming policy work instead of leaving the default-name expectation implicit.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment