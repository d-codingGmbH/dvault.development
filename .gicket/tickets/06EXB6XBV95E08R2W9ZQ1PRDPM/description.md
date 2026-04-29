<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the skeleton story against the current repository planning context and ratified the bounded v1 layout defaults for the .NET 10 .slnx foundation work.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- DVault.slnx is the root repository-level .NET entry point for this story.
- Per README.md, DVault.slnx may be intentionally projectless at this stage; adding library or test projects to the solution belongs to later project-creation tickets unless those projects are created as part of this ticket.
- The v1 reserved source path is src/DCoding.Data.DVault/ and the v1 reserved test paths are tests/DCoding.Data.DVault.Tests/ and tests/DCoding.Data.DVault.IntegrationTests/.
- The .NET baseline is net10.0 and the package/root namespace baseline is DCoding.Data.DVault, as evidenced by the current project metadata and repository standards.
- File-scoped namespaces are required for any C# source introduced by this story.

### Scope In
- Ensure the repository has DVault.slnx at the root as the stable solution entry point for dotnet tooling that supports .slnx.
- Ensure the source and test scaffold directories documented in README.md exist with tracked placeholders where no project files are created yet.
- Align README.md or equivalent repository layout documentation with the actual skeleton state if the implementation changes the scaffold.
- Preserve the repository formatting and encoding standards from docs/formatting.md and docs/plans/shared-implementation-standards.md.
- If any C# project is introduced by this ticket, target net10.0 and use DCoding.Data.DVault as the root namespace/package naming baseline.

### Scope Out
- Implementing DVault product APIs, persistence behavior, naming policy logic, hashing, configuration hooks, or provider adapters.
- Creating CI workflows, release packaging automation, migrations, schema generation, or runtime configuration APIs.
- Adding project references to DVault.slnx for projects that are reserved for sibling or downstream tickets but not created by this story.
- Resolving broader repository history cleanup around legacy solution or project names unless they directly prevent the skeleton from opening or building.

## Acceptance Criteria
- The repository contains a root DVault.slnx that opens with dotnet tooling supporting the .slnx format.
- Running dotnet build against the root DVault.slnx succeeds with the supported .NET 10 SDK/tooling available to the developer environment.
- The scaffolded source and test folders match the README.md layout baseline, including reserved DCoding.Data.DVault source and test paths.
- Any C# project created or retained for this story targets net10.0 and uses DCoding.Data.DVault-compatible project naming and root namespace metadata.
- Any C# files introduced by this story use file-scoped namespaces.
- The shared formatting gate bash tools/check-format.sh passes after the skeleton changes.

## Definition of Done
- DVault.slnx, scaffold folders, placeholders, and layout documentation are mutually consistent.
- No product behavior, provider-specific persistence work, or advanced configuration surface is introduced by this foundation skeleton story.
- The repository can be built through the documented root solution entry point using .slnx-capable dotnet tooling.
- Shared implementation standards for formatting, LF line endings, UTF-8 without BOM, final newlines, and same-line braces for brace-based source files are satisfied.

## Implementation Notes
- Use README.md as the source of truth for v1 repository layout and docs/plans/shared-implementation-standards.md for the shared foundation standards.
- Keep DVault.slnx as the only intended root solution entry point for normal build/test automation once the skeleton is completed.
- When placeholder-only directories are needed, use .gitkeep files so clean checkouts retain the layout.
- Do not require project membership in DVault.slnx until the corresponding source or test project files exist.
- Use net10.0, nullable enabled, implicit usings enabled, and DCoding.Data.DVault naming if project files are created or normalized under this story.

## Open Questions
- none

## Follow-Up Questions
- A later cleanup or migration ticket may decide whether legacy root solution/project files such as DVault.sln, DVault.csproj, DVault.Tests.csproj, or older src/DVault and tests/DVault.Tests paths should be removed or retained for compatibility.
- Downstream implementation tickets should decide when to create the first library, unit test, and integration test projects and add them to DVault.slnx.

## Risks
- Developer environments without .NET 10 SDK or .slnx-capable dotnet tooling will not be able to validate the build even if the repository skeleton is correct.
- Existing legacy root/project files could confuse no-argument dotnet commands if not handled consistently with the documented root DVault.slnx entry point.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Set up the repository with the intended solution and project structure.

## Scope
- Create DVault.slnx and source/test folders.
- Target .NET 10 and use file-scoped namespaces.

## Acceptance Criteria
- The solution opens and builds with dotnet tooling that supports .slnx.
- Project names and namespaces match DCoding.Data.DVault.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.