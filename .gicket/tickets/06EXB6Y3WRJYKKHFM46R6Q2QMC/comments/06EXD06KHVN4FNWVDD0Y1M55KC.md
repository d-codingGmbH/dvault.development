[gicket-bot] PO refinement contract

Summary
- Revised the PO contract to match the actual branch: there is no existing src/DVault, tests/DVault.Tests, solution, or project file baseline. This ticket now owns bootstrapping the test area and the smallest dotnet test entry-point wiring needed for test discovery, while production source project creation and source project references remain out of scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is revised to match the actual repository state instead of depending on nonexistent current scaffolding. The target branch has no src root, no tests root, and no .NET solution/project files; this ticket is therefore defined as a test-infrastructure bootstrap that creates tests/DVault.Tests and minimal dotnet test entry-point wiring. No prerequisite branch is required for the test infrastructure itself; production source-project integration is deferred until a foundation source project exists.
- critic-item-2: `answered` - Decision: this ticket does not depend on existing .NET scaffolding. Scope In explicitly includes creating the smallest conventional .NET test entry point needed for dotnet test discovery, plus the unit/integration test projects and shared Sqlite utilities. Scope Out explicitly excludes creating src/DVault or a production DVault library project.
- critic-item-3: `answered` - The incorrect baseline ratification is removed. tests/DVault.Tests is now the desired v1 test area to create under this ticket, not an existing repository tree. src/DVault is not treated as existing on this branch and is not in scope for this ticket.
- critic-item-4: `answered` - The acceptance criteria no longer require compiling against a DVault source project, because no DVault source project is visible on the reviewed branch. The test projects must compile and run independently with smoke/helper validation now; adding project references to a future DVault source project is deferred to downstream source-foundation or source-integration work.

Clarifications
- The actual target-branch baseline is repository metadata and planning material only; there is no existing src/DVault, tests/DVault.Tests, solution, or .NET project file to reference.
- tests/DVault.Tests is the v1 test area this ticket should create, not an already existing test tree.
- This ticket is infrastructure-only and includes the smallest .NET test-entry-point wiring needed so dotnet test can discover the new test projects from the repository after implementation.
- This ticket does not create the DVault production source project and does not require a ProjectReference to a DVault source project while none exists.
- Sqlite-backed test helpers must run locally using in-memory or temporary-file Sqlite and must not depend on external database services.

Scope In
- Create the tests/DVault.Tests test area from the current empty implementation baseline.
- Create unit and integration test projects under tests/DVault.Tests using a dotnet test-compatible framework and the v1 .NET 10 target platform established by planning context.
- Add the smallest conventional repository-level test entry point needed for dotnet test discovery, such as a solution/test solution that includes the new test projects.
- Add shared test utilities for Sqlite-backed tests, including setup and cleanup support for isolated in-memory or temporary-file databases.
- Add minimal smoke/sample tests that prove test discovery, project compilation, and Sqlite helper usability.

Scope Out
- Creating or implementing src/DVault, the DVault production library project, public API behavior, EF model conventions, or DI extension methods.
- Requiring the new test projects to compile against a DVault source project before such a project exists on the branch.
- Implementing feature-specific unit or integration coverage beyond minimal infrastructure validation.
- Adding dependencies on external database servers, containers, cloud services, or developer-machine-specific services.
- Changing CI pipeline behavior, runtime workflow labels, ticket status, or bot handoff metadata as part of implementation.

Open questions
- none

Follow-up questions
- After the DVault source project exists, decide which ticket should add ProjectReference wiring from these test projects to the source project and introduce first source-backed tests.
- Decide later whether CI should run unit and integration tests as separate stages or a single dotnet test command once pipeline work is in scope.
- Decide later whether longer-running integration tests need category filters or naming conventions once real integration coverage is added.

Risks
- The implementation may need to choose initial package versions because no central package management files are visible on the current branch.
- If the .NET 10 SDK is not available in a developer environment, local verification may require installing the target SDK before dotnet test can run.
- Future source-project creation may require adding project references or adjusting namespaces, but that is downstream integration work rather than a blocker for this test-infrastructure bootstrap.

Split recommendations
- No child tickets or relations were created in this run. No split is required for the revised scope because the ticket is bounded to test infrastructure plus minimal test-entry-point wiring; production source-project creation remains separate downstream foundation work.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment