[gicket-bot] PO-critic review contract

Summary
- Return to PO: the metadata contract is scoped, but the persisted ticket itself says it is not a developer-start signal while foundation structure is absent, and repository/ticket evidence confirms the required solution, src project, and test project are still missing or unresolved.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Current branch is ticket/06EXB74XQJFKGSKVJ6THQWJY8W-task-define-hub-link-and-satellite-metadata-abst at HEAD 7d943ad2955bd172194ca6d2457470a10f5e0d42.
- .gicket/tickets/06EXB74XQJFKGSKVJ6THQWJY8W/description.md contains PO Handoff decision ready_for_po_critic, but Clarifications say this is not a developer-start signal while foundation structure is absent and the ticket must wait for tracked .NET solution, src/DVault library project, and tests/DVault.Tests test project.
- The same description.md Open Questions section says '- none', so the return is not caused by unresolved product questions.
- Command `rg --files src tests` returned `rg: src: No such file or directory` and `rg: tests: No such file or directory`; command `git ls-files "*.sln" "*.slnx" "*.csproj" "src/**" "tests/**"` returned no tracked solution/project/source/test files.
- .gicket/relations/SW/8W/06EXB74NRVRX18GD33CH1C12SW--06EXB74XQJFKGSKVJ6THQWJY8W--parentOf.json makes this task a child of Story 06EXB74NRVRX18GD33CH1C12SW.
- .gicket/relations/PM/SW/06EXB6XBV95E08R2W9ZQ1PRDPM--06EXB74NRVRX18GD33CH1C12SW--blocks.json says the .NET 10 solution skeleton story blocks the parent modeling story.
- PO refinement comment .gicket/tickets/06EXB74XQJFKGSKVJ6THQWJY8W/comments/06EXD5QQGAYPEQ2Q2DQXJNY5N0.md says the direct blocker relation attempt was not persisted and that this ticket is not ready for developer implementation until the foundation child tasks have landed.
- docs/plans/06EXB6ZC4M7Q55PXTFBVWP34S0-adddvault-usedatavault-extension-shape.md records the v1 namespace baseline DCoding.Data.DVault and notes there is no tracked source project, solution file, src root, or test root for implementation work.

Blocking findings
- The ticket is not ready for developer handoff because its own persisted contract requires waiting for the foundation solution/library/test structure, and direct repository inspection confirms that structure is not present.
- The dependency is not directly persisted on this task: the existing blocks relation targets the parent modeling story, while the PO comment says the attempted direct blocker relation for this task was denied. Approving this role would conflict with the ticket's explicit no developer-start clarification.

Required PO actions
- Keep this ticket out of developer handoff until the foundation tickets that create DVault.slnx, src/DVault, and tests/DVault.Tests are complete and visible in the tracked repository, or add an enforceable direct dependency if trust policy later permits it.
- Resolve the ticket-level routing state so blocked/dev and blocked/test are not paired with a handoff that would route to dev before the foundation work exists.
- After foundation completion, refresh the handoff with concrete repository evidence for the solution, library project, and test project before sending back to PO-critic.

Open issues ledger
- critic-item-1 [required-po-action] Keep this ticket out of developer handoff until the foundation tickets that create DVault.slnx, src/DVault, and tests/DVault.Tests are complete and visible in the tracked repository, or add an enforceable direct dependency if trust policy later permits it.
- critic-item-2 [required-po-action] Resolve the ticket-level routing state so blocked/dev and blocked/test are not paired with a handoff that would route to dev before the foundation work exists.
- critic-item-3 [required-po-action] After foundation completion, refresh the handoff with concrete repository evidence for the solution, library project, and test project before sending back to PO-critic.
- critic-item-4 [blocking-finding] The ticket is not ready for developer handoff because its own persisted contract requires waiting for the foundation solution/library/test structure, and direct repository inspection confirms that structure is not present.
- critic-item-5 [blocking-finding] The dependency is not directly persisted on this task: the existing blocks relation targets the parent modeling story, while the PO comment says the attempted direct blocker relation for this task was denied. Approving this role would conflict with the ticket's explicit no developer-start clarification.

Missing examples / edge cases
- No blocking example gap in the metadata scope itself; once foundation exists, validation examples should include zero/one link endpoints and null satellite parent in addition to null, empty, and whitespace names.

Risky assumptions
- Assuming contract text and labels alone will prevent dev dispatch is risky because this role's success path is dev, while the ticket says it is not a developer-start signal.
- Assuming src/DVault and tests/DVault.Tests are available target projects is currently false; the paths are only future targets in the persisted contract.
- The namespace DCoding.Data.DVault is supported by the planning document, but implementation should still verify the foundation project's actual namespace convention once it exists.

AC / test suggestions
- Current ACs are sufficient for low-assurance v1 after the foundation lands: construct hub/link/satellite metadata and cover null, empty, whitespace, insufficient link endpoints, and missing satellite parent validation.
- Consider asserting deterministic exception types or parameter names only if the foundation codebase establishes that convention.

Implementation watchouts
- Do not create DVault.slnx, csproj files, src/DVault, or tests/DVault.Tests in this ticket; those are explicitly out of scope.
- When foundation exists, verify any existing public API/type directly in source before depending on it.
- Keep the API small, documented with XML comments for public/protected members, and uncoupled from persistence, generation, database providers, or runtime loading.

Non-blocking notes
- The persisted Open Questions section says none.
- The metadata abstraction scope is otherwise clear and bounded for a low-assurance task once the foundation dependency is resolved.
- The current worktree has many modified .gicket files primarily reflecting line-ending churn; this review used current persisted ticket files and read-only git/rg evidence only.

Split recommendations
- No split is needed for the metadata abstraction scope; the blocking issue is sequencing against already-existing foundation tickets.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment