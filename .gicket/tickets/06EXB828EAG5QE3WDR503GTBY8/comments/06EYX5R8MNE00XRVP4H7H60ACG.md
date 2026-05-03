[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff; the persisted contract is concrete, has no open questions, and matches the current six-package packaging baseline in the repository.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB828EAG5QE3WDR503GTBY8/description.md` records PO handoff `ready_for_po_critic`, and `## Open Questions` contains only `- none`.
- `DVault.slnx` lists exactly six packable library projects under `/src/`: `DCoding.Data.DVault`, `DCoding.Data.DVault.MySql`, `DCoding.Data.DVault.Oracle`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.Sqlite`, and `DCoding.Data.DVault.SqlServer`.
- Repository inspection shows the non-packable baseline is explicit: `src/DCoding.Data/DCoding.Data.csproj` contains `<IsPackable>false</IsPackable>`, and `benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj` plus the `tests/DCoding.Data.DVault.Tests/{Unit,Integration,Shared}` csproj files also set `<IsPackable>false</IsPackable>`.
- `rg -n` over `src/DCoding.Data*/*.csproj` shows all six package projects emit to `../../bin/packages/`, pack `../../README.md`, publish Apache-2.0 and Git repository metadata, and each provider project references `../DCoding.Data.DVault/DCoding.Data.DVault.csproj`.
- `ls -1 bin/packages` returns exactly 12 current artifacts: six `.nupkg` and six `.snupkg` files for the expected package ids, with no extra package ids in the observed baseline.
- Direct archive inspection of `bin/packages/*.nupkg` shows `README.md` plus `lib/net10.0/*.xml` in every package, and the provider nuspecs in `DCoding.Data.DVault.MySql`, `.Oracle`, `.Postgres`, `.Sqlite`, and `.SqlServer` each declare a `DCoding.Data.DVault` dependency at version `1.0.0`.
- `README.md` `## Local Validation` still documents `dotnet pack src/DCoding.Data.DVault/DCoding.Data.DVault.csproj --configuration Release --nologo`, so the ticket's solution-level verification guidance is a real repo delta rather than duplicate scope.
- `git log --all --grep='06EXB828EAG5QE3WDR503GTBY8' -n 20` shows workflow-only branch-history entries `017ca84e`, `c221ca7b`, `2770f07c`, and `ea5fba59`; no later branch churn re-opened the contract after PO handoff.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Include an unexpected-artifact example that covers stale prior-version package files left in `bin/packages/`, not only obviously foreign files.
- Include a repository-metadata example that makes clear whether verification checks only stable `repository url/type` fields or also generated nuspec `branch`/`commit` attributes.

Risky assumptions
- Assuming `bin/packages/` will always be clean before verification; the acceptance criteria require failure on unexpected artifacts, so leftover versioned packages from earlier runs can matter.
- Assuming generated nuspec repository `branch` and `commit` values are appropriate assertion targets; the stable baseline directly declared in project files is `RepositoryUrl`/`RepositoryType`, not an exact source commit hash.
- Assuming symbol validation means only `.snupkg` presence; current `.snupkg` archives also contain `lib/net10.0/*.pdb`, and the implementation should document how deep the symbol check goes.

AC / test suggestions
- Lock down one explicit failure test for extra versioned DVault package artifacts in `bin/packages/` so `unexpected artifact` behavior is unambiguous.
- Have the dependency-alignment test derive the core version from the packed `DCoding.Data.DVault` nuspec and compare provider dependencies against that discovered version, rather than against a hard-coded literal.
- Keep README assertions semantic by checking that the packaged root `README.md` preserves the current pre-publication installation stance, not a full-file snapshot.

Implementation watchouts
- The developer-facing command will need to reconcile the current README local-validation example, which still packs only `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`, with the ticket's `dotnet pack DVault.slnx` baseline.
- Archive checks should stay semantic: expected artifacts, README presence, XML documentation presence, symbols package presence, expected nuspec metadata, and provider-to-core dependency alignment; avoid ZIP ordering, timestamps, or other incidental archive details already called out in the contract risks.

Non-blocking notes
- Comment history under `.gicket/tickets/06EXB828EAG5QE3WDR503GTBY8/comments/` is automation-only; I did not find unresolved human discussion that conflicts with the persisted contract.
- The ticket is well bounded: publication is out of scope, runtime/API changes are out of scope except packaging metadata adjustments, and the six-package matrix is already concrete in both `DVault.slnx` and `bin/packages/`.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment