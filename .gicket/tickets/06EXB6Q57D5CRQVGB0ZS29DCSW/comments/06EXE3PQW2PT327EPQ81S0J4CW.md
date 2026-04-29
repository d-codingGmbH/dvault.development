[gicket-bot] PO refinement contract

Summary
- Resolved the docs-only validation conflict: keep this ticket docs-only, do not add a solution/project/build artifact solely for validation, and treat dotnet build/test as not applicable while the repository has no tracked .NET project or solution.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- For docs-only tickets in this repository, should the workflow waive or replace dotnet build --nologo and dotnet test --nologo when no project/solution exists, or should the delivery contract explicitly allow a minimal root solution/build artifact despite the tester's out-of-scope finding?: `answered` - Waive or replace the dotnet build --nologo and dotnet test --nologo validation for this docs-only/no-project baseline. The delivery contract should not allow reintroducing DVault.sln, a minimal project, source files, test files, or other build artifacts solely to satisfy dotnet commands. Validation should instead record read-only evidence that no .sln/.csproj/.fsproj/.vbproj/src/test/tests paths are tracked, confirm docs/plans/deferred-data-vault-capabilities.md is the committed deliverable, and inspect that document against the acceptance criteria and Definition of Done.

Clarifications
- The approved deliverable for this ticket remains docs/plans/deferred-data-vault-capabilities.md.
- Do not reintroduce DVault.sln or add any minimal .NET solution, project, placeholder source, placeholder test, or build-only artifact for this ticket.
- For this docs-only/no-project repository state, dotnet build --nologo and dotnet test --nologo are not applicable validation gates; replace them with documentation and repository-surface verification evidence.
- If a future branch adds a real .NET project or solution for product work, normal dotnet validation can apply again; this clarification is for the current docs-only/no-project baseline.
- No child tickets, relations, attachments, or planning documents were created in this clarification pass.

Scope In
- Maintain the deferred-capabilities architecture planning documentation for PIT table generation, bridge table generation, multi-active satellites, and provider-specific optimizations as future capabilities.
- Keep the document at docs/plans/deferred-data-vault-capabilities.md as the committed planning deliverable.
- Validate this ticket through documentation inspection and read-only repository evidence when no tracked .NET project or solution exists.

Scope Out
- Adding DVault.sln, a .NET project, package structure, source files, test files, or placeholder build artifacts solely to satisfy dotnet commands.
- Implementing PIT, bridge, multi-active satellite, or provider-specific generation behavior.
- Changing product code, runtime behavior, or repository-wide automation policy as part of this ticket.
- Creating future capability epics now.

Open questions
- none

Follow-up questions
- Formalize a repository-wide automation policy for docs-only tickets so configured build/test commands are conditionally skipped or replaced when no buildable project or solution exists.
- When implementation planning starts, decide whether PIT, bridge, multi-active satellite, and provider optimization work should become separate epics or smaller capability stories.
- Later provider work should decide which database engines need first-class optimization support beyond the Sqlite-oriented MVP baseline.
- Future Data Vault scope should decide how much automation is expected for each deferred capability versus documentation-only guidance.

Risks
- If the validation runtime ignores this clarification and continues to require dotnet commands unconditionally, this docs-only ticket may keep failing for process reasons despite satisfying the delivery contract.
- If a future real .NET project is added, this docs-only waiver should not be reused to skip meaningful build/test validation.
- If the deferred-capabilities document uses overly specific implementation language, downstream work may treat non-MVP features as committed MVP scope.

Split recommendations
- No split is recommended for this ticket; keep it as a single documentation task.
- A separate workflow or automation-policy ticket may be useful later to persist the docs-only validation rule across the repository, but it is not required to complete this ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment