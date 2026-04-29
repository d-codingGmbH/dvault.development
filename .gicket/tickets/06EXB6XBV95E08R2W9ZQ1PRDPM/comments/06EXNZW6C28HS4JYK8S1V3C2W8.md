[gicket-bot] PO refinement contract

Summary
- Refined the skeleton story against the current repository planning context and ratified the bounded v1 layout defaults for the .NET 10 .slnx foundation work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- DVault.slnx is the root repository-level .NET entry point for this story.
- Per README.md, DVault.slnx may be intentionally projectless at this stage; adding library or test projects to the solution belongs to later project-creation tickets unless those projects are created as part of this ticket.
- The v1 reserved source path is src/DCoding.Data.DVault/ and the v1 reserved test paths are tests/DCoding.Data.DVault.Tests/ and tests/DCoding.Data.DVault.IntegrationTests/.
- The .NET baseline is net10.0 and the package/root namespace baseline is DCoding.Data.DVault, as evidenced by the current project metadata and repository standards.
- File-scoped namespaces are required for any C# source introduced by this story.

Scope In
- Ensure the repository has DVault.slnx at the root as the stable solution entry point for dotnet tooling that supports .slnx.
- Ensure the source and test scaffold directories documented in README.md exist with tracked placeholders where no project files are created yet.
- Align README.md or equivalent repository layout documentation with the actual skeleton state if the implementation changes the scaffold.
- Preserve the repository formatting and encoding standards from docs/formatting.md and docs/plans/shared-implementation-standards.md.
- If any C# project is introduced by this ticket, target net10.0 and use DCoding.Data.DVault as the root namespace/package naming baseline.

Scope Out
- Implementing DVault product APIs, persistence behavior, naming policy logic, hashing, configuration hooks, or provider adapters.
- Creating CI workflows, release packaging automation, migrations, schema generation, or runtime configuration APIs.
- Adding project references to DVault.slnx for projects that are reserved for sibling or downstream tickets but not created by this story.
- Resolving broader repository history cleanup around legacy solution or project names unless they directly prevent the skeleton from opening or building.

Open questions
- none

Follow-up questions
- A later cleanup or migration ticket may decide whether legacy root solution/project files such as DVault.sln, DVault.csproj, DVault.Tests.csproj, or older src/DVault and tests/DVault.Tests paths should be removed or retained for compatibility.
- Downstream implementation tickets should decide when to create the first library, unit test, and integration test projects and add them to DVault.slnx.

Risks
- Developer environments without .NET 10 SDK or .slnx-capable dotnet tooling will not be able to validate the build even if the repository skeleton is correct.
- Existing legacy root/project files could confuse no-argument dotnet commands if not handled consistently with the documented root DVault.slnx entry point.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment