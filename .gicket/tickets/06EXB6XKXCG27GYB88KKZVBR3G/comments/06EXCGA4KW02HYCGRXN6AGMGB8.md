[gicket-bot] PO refinement contract

Summary
- Refined the scaffold ticket as a bounded foundation task: create the root DVault.slnx, establish the initial repository folders, and document the layout without taking over sibling project-creation work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence shows no src, tests, docs, examples, benchmarks, README.md, or DVault.slnx yet; the repo root currently contains only ticket/config surfaces.
- The parent ticket 06EXB6XBV95E08R2W9ZQ1PRDPM sets .NET 10, .slnx, file-scoped namespaces, and DCoding.Data.DVault naming as the v1 baseline.
- The charter epic attachment dvault-library-guidelines.md establishes DCoding.Data.DVault, .slnx, net10.0, LF/UTF-8, 2-space indentation, file-scoped namespaces, English documentation, and Sqlite-default test examples as shared standards.
- Existing sibling tickets already cover creating the main class library project and unit/integration test projects, so this ticket should not absorb those implementation tasks.

Scope In
- Add DVault.slnx at the repository root using the new .slnx solution format.
- Create the initial top-level repository layout: src, tests, examples, benchmarks, and docs where the implementation needs those folders to exist now.
- Add README.md if absent and briefly document the folder layout in English.
- Reserve naming/path conventions for the intended first projects: main library under src using DCoding.Data.DVault, and unit/integration tests under tests.
- Keep the solution free of missing or stale project references; include project references only for project files that exist after this ticket's changes.

Scope Out
- Creating the main DCoding.Data.DVault class library project, which is covered by sibling ticket 06EXB6XVWBWZGN6MA3SFWGWKM4.
- Creating unit/integration test projects or Sqlite test helpers, which is covered by sibling ticket 06EXB6Y3WRJYKKHFM46R6Q2QMC.
- Implementing DVault APIs, Data Vault metadata abstractions, EF integration, examples, benchmarks, package metadata, or CI automation.

Open questions
- none

Follow-up questions
- When the sibling project tickets land, update DVault.slnx to include the concrete library, unit test, and integration test project files if they were not created in this scaffold ticket.
- Future tickets can decide whether examples and benchmarks remain empty folders initially or gain project files once there is a useful API surface to demonstrate or measure.

Risks
- .slnx support depends on sufficiently recent .NET tooling; verification should use the repository's intended .NET 10-capable SDK/toolchain.
- Empty directories may require placeholder files to be tracked depending on repository policy; keep placeholders minimal and documented if used.

Split recommendations
- No new child split is needed. The existing parent relation already places this ticket alongside separate child tickets for the main library project and test infrastructure, so this ticket should remain the bounded solution/folder/README scaffold.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment