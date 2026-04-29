[gicket-bot] PO refinement contract

Summary
- Refined the minimal-startup smoke-test task using the current ticket state and available repository evidence. The ticket is small enough to remain a single task and is ready for PO critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ticket 06EXB6ZMBB97J1Z5TBS29QMGPR is a child of 06EXB6Z3YMAPSRYRB8NQX3ZST4 through an existing parentOf relation.
- No human comments or ticket attachments add further scope beyond the ticket description; the only comments are bot claim and lease metadata.
- The repository snapshot names src/DVault as the product source root and tests/DVault.Tests as the intended test root for this ticket, but the bounded directory reads reported those directories missing in the current scratch worktree. Treat the snapshot as the planning baseline and let development create or align the missing test project if it is still absent on the implementation branch.
- The smoke-test target is the minimal configuration experience: a consuming DbContext should start with library defaults without requiring noisy setup or external infrastructure.

Scope In
- Add a smoke test that defines a small consuming DbContext and verifies minimal startup succeeds with default configuration.
- Keep the test self-contained and free of external database dependencies.
- Place the test under the repository's visible test layout convention, using tests/DVault.Tests as the v1 default when available or creating/aliging that test project if it is absent.
- Assert that required setup remains quiet enough that unnecessary new mandatory configuration causes the smoke test to fail.

Scope Out
- Integration tests against real external databases or services.
- Broad provider-matrix coverage beyond the minimal default startup path.
- Large test infrastructure rewrites unrelated to enabling this smoke test.
- Runtime workflow label or status changes; those are handled by orchestration after this PO handoff.

Open questions
- none

Follow-up questions
- After the initial smoke test lands, decide separately whether to add provider-specific startup coverage or broader configuration regression tests.
- If the source/test directories are still absent when development starts, consider a separate project-structure cleanup ticket only if creating the expected test project is larger than this task.

Risks
- The scratch repository directory reads did not find src/DVault or tests/DVault.Tests even though the branch snapshot lists them; development should verify the actual checked-out branch layout before implementing.
- A smoke test that asserts too much internal behavior could become brittle, so it should focus on observable minimal startup success and absence of external dependencies.

Split recommendations
- No split recommended; the ticket is a bounded testing task with one clear smoke-test scenario.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment