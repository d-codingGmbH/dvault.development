[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the existing six-package DVault pack matrix, a local CLI verification flow, and artifact-level package checks needed before any publication decision.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 package matrix is the six packable library projects already present in `DVault.slnx`: `DCoding.Data.DVault`, `DCoding.Data.DVault.MySql`, `DCoding.Data.DVault.Oracle`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.Sqlite`, and `DCoding.Data.DVault.SqlServer`.
- The current shared package output baseline is `bin/packages/`, which is already declared by the packable project files.
- The package content baseline already visible in the packable project files is a packaged root `README.md`, generated XML documentation, `.snupkg` symbols, and nuspec metadata for authors, description, tags, Apache-2.0 license, and Git repository information.

Scope In
- Add a repo-local CLI verification entry point for package artifacts produced from `dotnet pack DVault.slnx`.
- Verify the expected `.nupkg` and `.snupkg` artifacts in `bin/packages/` for the six packable packages and fail on missing or unexpected artifacts.
- Inspect packaged README, generated XML documentation, symbols output, and nuspec metadata such as package id, title, authors, description, tags, license, and repository fields.
- Verify provider package dependency metadata resolves to `DCoding.Data.DVault` with the same packed version as the core package.

Scope Out
- Publishing packages to NuGet or any other feed.
- Changing DVault runtime behavior or public API surface except for packaging metadata adjustments needed to satisfy verification.
- Making `src/DCoding.Data`, benchmark projects, or any test project packable.
- Adding post-publication install guidance beyond the current pre-publication README stance.

Open questions
- none

Follow-up questions
- Should a later ticket wire the same local package verification command into CI or release gating once publication automation exists?

Risks
- Future additions or renames in the packable package matrix will require the verification baseline and tests to be updated in lockstep.
- Overly strict archive-structure assertions can create noisy failures; checks should target semantic package facts rather than incidental ZIP ordering, timestamps, or other non-semantic packaging details.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment