[gicket-bot] PO-critic review contract

Summary
- Return to PO: the metadata scope is bounded, but the persisted contract still blocks developer handoff until the foundation solution/library/test structure exists or a direct enforceable dependency is added.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Current branch is `ticket/06EXB74XQJFKGSKVJ6THQWJY8W-task-define-hub-link-and-satellite-metadata-abst`; `git rev-parse HEAD` returned `523de4dab1cdac986db1453295760cc7e763aee4`.
- `git ls-files -- DVault.slnx '*.sln' '*.slnx' '*.csproj' 'src/**' 'tests/**'` returned no tracked solution, project, src, or tests paths.
- `stat /mnt/c/Projects/DVault/DVault.slnx /mnt/c/Projects/DVault/src/DVault /mnt/c/Projects/DVault/tests/DVault.Tests` failed with `No such file or directory` for all three required foundation paths.
- The persisted contract in `.gicket/tickets/06EXB74XQJFKGSKVJ6THQWJY8W/description.md` says implementation must not start until the foundation solution/library/test structure exists or a direct enforceable dependency is added; `## Open Questions` is `none`.
- Direct relation search for `06EXB74XQJFKGSKVJ6THQWJY8W` returned only `.gicket/relations/SW/8W/06EXB74NRVRX18GD33CH1C12SW--06EXB74XQJFKGSKVJ6THQWJY8W--parentOf.json`; that relation file type is `parentOf`, not `blocks`.
- PO refinement comment `06EXG3M743VC2XNE6V67RB1B20.md` explicitly says to keep this ticket out of developer handoff until `DVault.slnx`, `src/DVault`, and `tests/DVault.Tests` exist or an enforceable direct dependency is persisted.

Blocking findings
- Developer handoff is still blocked by the ticket's own persisted contract because the required foundation paths are absent and no direct `blocks` relation is persisted on this task.
- The latest PO handoff routes to PO-critic, but the substantive contract and comments still say this ticket must not enter development before foundation completion.

Required PO actions
- Keep or return the ticket to PO refinement rather than developer handoff until `DVault.slnx`, `src/DVault`, and `tests/DVault.Tests` exist in repository evidence.
- After foundation completion, refresh the ticket contract with concrete repository evidence for the solution, library project, and test project before sending it back to PO-critic.
- If policy permits relation writes, add a direct persisted `blocks` dependency from the foundation skeleton work to this metadata task.

Open issues ledger
- critic-item-1 [required-po-action] Keep or return the ticket to PO refinement rather than developer handoff until `DVault.slnx`, `src/DVault`, and `tests/DVault.Tests` exist in repository evidence.
- critic-item-2 [required-po-action] After foundation completion, refresh the ticket contract with concrete repository evidence for the solution, library project, and test project before sending it back to PO-critic.
- critic-item-3 [required-po-action] If policy permits relation writes, add a direct persisted `blocks` dependency from the foundation skeleton work to this metadata task.
- critic-item-4 [blocking-finding] Developer handoff is still blocked by the ticket's own persisted contract because the required foundation paths are absent and no direct `blocks` relation is persisted on this task.
- critic-item-5 [blocking-finding] The latest PO handoff routes to PO-critic, but the substantive contract and comments still say this ticket must not enter development before foundation completion.

Missing examples / edge cases
- No blocking metadata example gap once the foundation exists; current ACs cover valid hub/link/satellite creation plus null, empty, whitespace, insufficient link endpoints, and missing satellite parent validation.

Risky assumptions
- Assuming `src/DVault` and `tests/DVault.Tests` are available target projects is currently false; they are future targets in the contract, not present repository structure.
- Without a direct persisted blocker relation, sequencing depends on contract text and blocked labels rather than board-level enforcement.

AC / test suggestions
- After foundation lands, keep tests focused on valid construction and deterministic validation failures for null, empty, whitespace names, fewer than two link endpoints, and missing satellite parent.
- Only assert exact exception types or parameter names if the foundation codebase establishes that convention.

Implementation watchouts
- Do not create `DVault.slnx`, csproj files, `src/DVault`, or `tests/DVault.Tests` in this ticket; those are explicitly out of scope.
- When foundation exists, verify the actual namespace and any public modeling API/type directly in source before depending on them.

Non-blocking notes
- The metadata abstraction scope is otherwise small and suitable for a low-assurance task after the foundation dependency is resolved.
- `## Open Questions` is `none`, so the return decision is based on unresolved sequencing/evidence, not unanswered product questions.

Split recommendations
- No split is needed for the metadata abstraction scope; wait for the existing foundation solution/library/test project work or persist a direct enforceable dependency.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment