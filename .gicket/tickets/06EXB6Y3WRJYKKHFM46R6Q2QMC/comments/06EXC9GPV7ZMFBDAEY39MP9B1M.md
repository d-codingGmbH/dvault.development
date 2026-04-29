[gicket-bot] PO refinement contract

Summary
- Refined the testing infrastructure task using the current repository baseline. The ticket is ready for PO critic review with no blocking product questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository baseline is a .NET project rooted at src/DVault with the visible test tree under tests/DVault.Tests; this is ratified as the v1 layout convention for this ticket.
- Sqlite-backed test helpers must run locally using in-memory or temporary-file Sqlite and must not depend on external database services.
- The ticket is infrastructure-only: it establishes discoverable test projects and reusable helpers, not broad application behavior coverage.

Scope In
- Create or complete unit and integration test projects under the existing tests/DVault.Tests test area, following the repository's current .NET naming and layout conventions.
- Ensure the new test projects reference the DVault source project as needed and are discoverable by dotnet test from the repository's normal test entry point.
- Add shared test utilities for Sqlite-backed tests, including setup and cleanup support suitable for repeatable local runs.
- Include minimal smoke/sample tests where useful to prove project discovery and utility usability.

Scope Out
- Implementing feature-specific unit or integration coverage beyond minimal infrastructure validation.
- Adding dependencies on external database servers, containers, cloud services, or developer-machine-specific services.
- Redesigning the repository source layout outside src/DVault and tests/DVault.Tests.
- Changing runtime workflow labels, ticket status, or bot handoff metadata as part of implementation.

Open questions
- none

Follow-up questions
- Decide later whether CI should run unit and integration tests as separate stages or a single dotnet test command once pipeline work is in scope.
- Decide later whether longer-running integration tests need category filters or naming conventions once real integration coverage is added.

Risks
- If the repository does not yet have a solution or central test entry point, the developer may need to add the smallest conventional .NET wiring needed for dotnet test discovery.
- Package versions for the test framework and Sqlite provider should be kept consistent with any central package management conventions discovered during implementation.

Split recommendations
- none

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