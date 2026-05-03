<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around the existing six-package DVault pack matrix, a local CLI verification flow, and artifact-level package checks needed before any publication decision.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 package matrix is the six packable library projects already present in `DVault.slnx`: `DCoding.Data.DVault`, `DCoding.Data.DVault.MySql`, `DCoding.Data.DVault.Oracle`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.Sqlite`, and `DCoding.Data.DVault.SqlServer`.
- The current shared package output baseline is `bin/packages/`, which is already declared by the packable project files.
- The package content baseline already visible in the packable project files is a packaged root `README.md`, generated XML documentation, `.snupkg` symbols, and nuspec metadata for authors, description, tags, Apache-2.0 license, and Git repository information.

### Scope In
- Add a repo-local CLI verification entry point for package artifacts produced from `dotnet pack DVault.slnx`.
- Verify the expected `.nupkg` and `.snupkg` artifacts in `bin/packages/` for the six packable packages and fail on missing or unexpected artifacts.
- Inspect packaged README, generated XML documentation, symbols output, and nuspec metadata such as package id, title, authors, description, tags, license, and repository fields.
- Verify provider package dependency metadata resolves to `DCoding.Data.DVault` with the same packed version as the core package.

### Scope Out
- Publishing packages to NuGet or any other feed.
- Changing DVault runtime behavior or public API surface except for packaging metadata adjustments needed to satisfy verification.
- Making `src/DCoding.Data`, benchmark projects, or any test project packable.
- Adding post-publication install guidance beyond the current pre-publication README stance.

## Acceptance Criteria
- A documented repo-local command can be run from the repository root to verify package artifacts produced from `DVault.slnx`.
- The verification expects exactly the six packable packages and corresponding `.snupkg` files in `bin/packages/`, and it fails when any expected artifact is missing or when any unexpected or non-packable package artifact is present.
- For each expected package, the verification checks packaged README presence, generated XML documentation availability, symbols package presence, and the nuspec metadata baseline already declared in the project files, and it reports actionable failure messages that identify the offending package and condition.
- The verification confirms every provider package depends on `DCoding.Data.DVault` using the same version as the packed core package.

## Definition of Done
- The local package verification flow and its automated tests are added to the repository and satisfy the acceptance criteria.
- Automated tests cover the passing package matrix and representative failure cases for missing artifacts, unexpected artifacts, missing README or XML docs or symbols, incorrect metadata, and mismatched provider-to-core dependency versions.
- Developer-facing guidance states how to run the local verification flow from the repository root.
- Any added scripts, tests, or docs follow the shared formatting and implementation standards already attached to the ticket.

## Implementation Notes
- Use `DVault.slnx` as the repository pack baseline and `bin/packages/` as the default artifact location.
- Treat the visible six-package matrix as the v1 source of truth; `src/DCoding.Data` is already non-packable via `<IsPackable>false</IsPackable>`, and benchmark and test projects must continue producing no package artifacts.
- All provider extension projects already project-reference `../DCoding.Data.DVault/DCoding.Data.DVault.csproj`; verification should assert the packed nuspec output resolves that relationship to a `DCoding.Data.DVault` package dependency with aligned version metadata.
- Assert package facts from the built archives themselves, not only from project properties, so failures reflect the shipped artifacts.
- Keep README assertions focused on packaging the shared root `README.md` and the current pre-publication guidance, avoiding brittle full-content snapshots that would churn on ordinary documentation edits.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket wire the same local package verification command into CI or release gating once publication automation exists?

## Risks
- Future additions or renames in the packable package matrix will require the verification baseline and tests to be updated in lockstep.
- Overly strict archive-structure assertions can create noisy failures; checks should target semantic package facts rather than incidental ZIP ordering, timestamps, or other non-semantic packaging details.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Verify package contents locally before any publication decision.

## Current Baseline
- `dotnet pack DVault.slnx` now produces the core package and provider extension packages.
- Test/shared, benchmark, and umbrella projects must remain non-packable.

## Scope
- Check package metadata, docs, symbols, readme, dependency metadata, and package-to-package references.
- Verify all expected `.nupkg` and `.snupkg` files for the package matrix and fail on unexpected packages.

## Acceptance Criteria
- Package verification can run from command line.
- Failures describe missing or incorrect package content.
- Verification confirms aligned versions and correct dependencies from provider packages to the core package.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.