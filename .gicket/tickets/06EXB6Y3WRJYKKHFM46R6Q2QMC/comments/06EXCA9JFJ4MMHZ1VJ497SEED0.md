[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket contract is internally ready, but it relies on a repository baseline that direct branch inspection does not support.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned 9 comments, including the PO refinement contract and handoff comment for branch ticket/06EXB6Y3WRJYKKHFM46R6Q2QMC-task-add-test-projects-and-shared-test-utilities at commit 8f44575d646e.
- git rev-parse HEAD at /mnt/c/Projects/DVault returned f24b03557fea2a47f9eb4e28929961ad64aad00a, matching the scratch-source-ref.
- git log --oneline --decorate --max-count=8 shows HEAD on ticket/06EXB6Y3WRJYKKHFM46R6Q2QMC-task-add-test-projects-and-shared-test-utilities, with commits f24b035, 0529de7, 8f44575, 8698a52, then develop at 6e94918.
- repository-list-directory for . returned only .gicket and .gicket-bot; repository-list-directory for src/DVault and tests/DVault.Tests each returned REPOSITORY-DIRECTORY-NOT-FOUND.
- git ls-tree --name-only HEAD returned only .gicket-bot and .gicket, and git ls-files src tests *.sln *.csproj Directory.Build.props Directory.Packages.props global.json returned no output.
- The delivery contract states the repository baseline is a .NET project rooted at src/DVault with visible test tree tests/DVault.Tests, and AC require unit/integration projects to compile against the DVault source project.

Blocking findings
- The persisted contract ratifies src/DVault and tests/DVault.Tests as the current v1 baseline, but direct repository evidence on the target branch shows neither path nor any .NET project/solution files exist. That makes the handoff contract materially inaccurate for development.
- Acceptance criteria require test projects to compile against the DVault source project, but there is no direct source evidence for a DVault source project or public project API on the reviewed branch.

Required PO actions
- Revise the contract to match the actual repository state, or identify the prerequisite ticket/branch that introduces src/DVault, tests/DVault.Tests, and the normal dotnet test entry point before handing this ticket to dev.
- Clarify whether this ticket depends on existing .NET scaffolding or is intended to include the smallest test-entry-point wiring; keep that decision explicit in Scope In/Out and AC.

Open issues ledger
- critic-item-1 [required-po-action] Revise the contract to match the actual repository state, or identify the prerequisite ticket/branch that introduces src/DVault, tests/DVault.Tests, and the normal dotnet test entry point before handing this ticket to dev.
- critic-item-2 [required-po-action] Clarify whether this ticket depends on existing .NET scaffolding or is intended to include the smallest test-entry-point wiring; keep that decision explicit in Scope In/Out and AC.
- critic-item-3 [blocking-finding] The persisted contract ratifies src/DVault and tests/DVault.Tests as the current v1 baseline, but direct repository evidence on the target branch shows neither path nor any .NET project/solution files exist. That makes the handoff contract materially inaccurate for development.
- critic-item-4 [blocking-finding] Acceptance criteria require test projects to compile against the DVault source project, but there is no direct source evidence for a DVault source project or public project API on the reviewed branch.

Missing examples / edge cases
- No concrete expected test entry point is named, and the repository currently has no observable .sln, .csproj, or test root to anchor it.

Risky assumptions
- The contract assumes an existing DVault .NET source project and test tree, but branch evidence shows only Gicket metadata at HEAD.
- The PO run report comment says the interactive PO tool loop hit tool_call_limit_reached and fell back to legacy planning, which increases the risk that the baseline claim was copied from seed context rather than verified against the final branch state.

AC / test suggestions
- Add an acceptance criterion that names the repository-level command or file that is expected to make dotnet test discover the new unit and integration projects once the baseline exists.
- Keep the Sqlite helper validation criterion explicit about in-memory or temporary-file databases and cleanup after disposal/run completion.

Implementation watchouts
- Do not let dev infer package choices or project references from prose alone; they need direct project-file evidence once the .NET baseline exists.
- If a prerequisite source-layout ticket is merged first, re-check src/DVault, tests/DVault.Tests, and central package/version conventions before implementation.

Non-blocking notes
- Open Questions is explicitly none, so the return is based on repository-contract mismatch rather than unresolved PO questions.

Split recommendations
- No split is needed if the missing .NET baseline is a prerequisite handled elsewhere; otherwise separate baseline .NET scaffolding from test infrastructure to avoid combining source-project creation with test utilities.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment