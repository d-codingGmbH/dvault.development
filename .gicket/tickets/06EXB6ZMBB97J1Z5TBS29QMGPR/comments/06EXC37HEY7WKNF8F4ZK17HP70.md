[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket asks for a smoke test against a public minimal startup path, but the current branch does not contain tracked product/test source evidence for that API or test suite.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted delivery contract for 06EXB6ZMBB97J1Z5TBS29QMGPR has Open Questions: none, but Scope In requires a smoke test around a consuming DbContext and default library setup, and Implementation Notes require exercising the public minimal startup path.
- gicket-read-ticket-comments returned 9 comments; they are bot claim/lease/refinement/handoff metadata, with no human comment adding API or test-suite detail.
- git log develop..HEAD shows only ticket workflow commits b3235fc, efc797a, d1f8819, and 13a4c5d for PO/PO-critic claim and handoff work.
- git diff --name-status develop..HEAD lists only .gicket ticket/comment/event metadata and .gicket/.ticket.write.lock changes; it lists no src/DVault or tests/DVault.Tests files.
- git ls-tree -r --name-only HEAD src tests produced no tracked files, and find src/DVault tests/DVault.Tests -maxdepth 5 -print showed only the two empty directories.
- Parent relation .gicket/relations/T4/PR/06EXB6Z3YMAPSRYRB8NQX3ZST4--06EXB6ZMBB97J1Z5TBS29QMGPR--parentOf.json confirms parentOf from story 06EXB6Z3YMAPSRYRB8NQX3ZST4 to this task.

Blocking findings
- The ticket currently asks a developer to implement a test against undefined repository structure and API surface. That risks turning a testing task into implicit product/API scaffolding work without PO-level dependency or scope clarity.

Required PO actions
- Update the ticket to reflect the verified current repository state: src/DVault and tests/DVault.Tests have no tracked files on this branch.
- Add an explicit dependency/blocking relation or sequencing note to the ticket that provides the public convention-first startup API and test-suite scaffold, or re-scope this ticket as a broader setup task if that work is intentionally included.
- Clarify the expected repository test command or state that establishing the DVault test project/command is in scope before dev handoff.

Open issues ledger
- critic-item-1 [required-po-action] Update the ticket to reflect the verified current repository state: src/DVault and tests/DVault.Tests have no tracked files on this branch.
- critic-item-2 [required-po-action] Add an explicit dependency/blocking relation or sequencing note to the ticket that provides the public convention-first startup API and test-suite scaffold, or re-scope this ticket as a broader setup task if that work is intentionally included.
- critic-item-3 [required-po-action] Clarify the expected repository test command or state that establishing the DVault test project/command is in scope before dev handoff.
- critic-item-4 [blocking-finding] The ticket currently asks a developer to implement a test against undefined repository structure and API surface. That risks turning a testing task into implicit product/API scaffolding work without PO-level dependency or scope clarity.

Missing examples / edge cases
- No concrete example identifies which public call or observable behavior proves minimal startup succeeds.
- The contract does not define how to distinguish quiet default startup from accidentally requiring extra mandatory setup beyond absence of external services.

Risky assumptions
- Assumes prose from the parent story is enough to target an existing public API despite no tracked source evidence.
- Assumes creating or aligning tests/DVault.Tests is small, even though there is no tracked test project or normal test command visible.
- Assumes the smoke test can avoid product behavior changes while the product/API surface needed for the test is not present.

AC / test suggestions
- Add an acceptance criterion tying the smoke test to a named public minimal startup entry point once that API exists.
- Add an acceptance criterion that the test is run by the repository's explicit standard command after the test project or solution exists.
- Add a negative/regression expectation for extra mandatory configuration in observable terms rather than only saying setup should remain quiet.

Implementation watchouts
- Do not let the dev infer or invent the production API solely from this test ticket; source evidence or an upstream ticket should define it first.
- If the test project is created under this ticket, treat that as a scope decision rather than incidental test placement.
- Keep the smoke test focused on observable startup success and no external dependencies, not internal implementation details.

Non-blocking notes
- The Open Questions section says none, so the return is not due to unresolved open questions; it is due to missing direct API/test-suite evidence required for handoff.
- The child relation to 06EXB6Z3YMAPSRYRB8NQX3ZST4 is present and correctly recorded.
- No split is needed if the prerequisite API/test scaffolding already exists elsewhere before dev starts.

Split recommendations
- If no prerequisite ticket owns the public entry point and DVault test-suite scaffold, split that setup/API work from this smoke-test-only task.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment