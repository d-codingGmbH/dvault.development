[gicket-bot] PO refinement contract

Summary
- Refined the smoke-test ticket against the verified empty implementation baseline. No child tickets, relation writes, attachments, or planning documents were created; the contract now treats this as dependent on the convention-first API and repository test scaffold rather than asking this ticket to invent them implicitly.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The repository baseline is now explicit: this branch has no tracked source or test implementation roots. src/DVault and tests/DVault.Tests are absent, and there are no src-roots or test-roots in the branch snapshot.
- critic-item-2: `answered` - This ticket is sequenced after the public convention-first startup API and a runnable DVault test-suite scaffold exist. It should consume the planned AddDVault and UseDataVault public API from ticket 06EXB6ZC4M7Q55PXTFBVWP34S0 and the parent story 06EXB6Z3YMAPSRYRB8NQX3ZST4; it is not scoped to create the production API surface, solution skeleton, library project, or full test infrastructure from scratch.
- critic-item-3: `answered` - Because the branch has no solution or test project yet, there is no existing repository test command to ratify. Before dev handoff for this smoke-test task, the prerequisite scaffold must provide the normal DVault test command. Once present, the expected command is the repository's standard .NET test invocation for the DVault test suite, typically dotnet test targeting the solution or tests/DVault.Tests project. Creating that command/scaffold is prerequisite setup, not hidden scope inside the smoke-test implementation.
- critic-item-4: `answered` - The scope is clarified so the smoke test does not silently become product/API scaffolding work. The ticket remains a bounded test task only after the public startup API and runnable test project are available; otherwise it must wait or be split into a separate foundation/setup task before implementation proceeds.

Clarifications
- Ticket 06EXB6ZMBB97J1Z5TBS29QMGPR remains a child of story 06EXB6Z3YMAPSRYRB8NQX3ZST4 through the existing parentOf relation.
- The current branch baseline has no tracked source root, no tracked test root, no solution file, and no tests/DVault.Tests project. The prior wording that treated tests/DVault.Tests as an intended v1 default is superseded by this explicit empty-baseline clarification.
- This ticket depends on the convention-first public startup API shape from sibling ticket 06EXB6ZC4M7Q55PXTFBVWP34S0: AddDVault for service registration and UseDataVault for EF model configuration in namespace DCoding.Data.DVault.
- This ticket is sequenced after a repository foundation/test-suite scaffold exists. If that scaffold is not present when development resumes, this ticket should not create the full solution, library, and test infrastructure as hidden scope.
- No new child tickets, relation writes, attachments, or planning documents were created in this refinement run.

Scope In
- Add a smoke test that defines a small consuming DbContext and verifies the public minimal startup path succeeds with default DVault configuration.
- Exercise the convention-first public API once it exists, using AddDVault and UseDataVault rather than internal implementation details.
- Keep the test self-contained and free of external database, network, or machine-specific infrastructure.
- Place the smoke test in the repository's normal DVault test suite after the test project/scaffold exists.
- Assert the minimal-startup path remains quiet enough that new unnecessary mandatory DVault setup causes the smoke test to fail.

Scope Out
- Creating the production AddDVault or UseDataVault API implementation from scratch.
- Creating the repository solution skeleton, main library project, package metadata, or general build configuration.
- Creating a full test-suite scaffold or deciding the repository-wide test command unless no separate foundation ticket provides it before dev handoff.
- Integration tests against real external databases or services.
- Provider-matrix coverage or broad configuration regression testing beyond the minimal default startup path.
- Runtime workflow label or status transitions.

Open questions
- none

Follow-up questions
- After the first smoke test lands, decide separately whether to add provider-specific startup coverage or broader configuration regression tests.
- If no existing foundation ticket is already intended to create the solution/library/test scaffold and normal test command, create a separate setup ticket before scheduling this smoke-test implementation.

Risks
- Implementing this ticket before the public API and test scaffold exist would expand it into implicit architecture and repository setup work.
- A smoke test that asserts internal registration or EF convention details instead of observable minimal startup success may become brittle.

Split recommendations
- No split is needed for the smoke-test itself once prerequisites exist.
- If the repository still has no solution, source project, test project, or test command at dev start, split or block on a separate foundation/setup ticket rather than broadening this task.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment