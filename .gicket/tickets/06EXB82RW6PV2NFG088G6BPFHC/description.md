<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratified the first CI workflow around the existing root solution, shared formatting gate, package verification command, and SQLite-default test baseline, with no blocking PO questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository currently has no tracked `.github` workflow surface, so this ticket is the bounded first CI workflow definition rather than an update to an existing pipeline.
- `DVault.slnx` is the repository build, test, and pack entry point, and the visible v1 pack matrix is the six packable packages `DCoding.Data.DVault`, `DCoding.Data.DVault.MySql`, `DCoding.Data.DVault.Oracle`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.Sqlite`, and `DCoding.Data.DVault.SqlServer`.
- The shared non-mutating validation gate is `bash tools/check-format.sh`; repository evidence shows it already validates governed documentation and configuration text, runs `dotnet format DVault.slnx --verify-no-changes --no-restore`, and enforces one-member-per-file through `tools/check-one-member-per-file.sh`.
- The existing package-validation baseline is `bash tools/verify-packages.sh` after `dotnet pack DVault.slnx --configuration Release --nologo`; the verifier inspects `bin/packages/` for the six `.nupkg` and six `.snupkg` artifacts plus packaged README, XML docs, nuspec metadata, and provider-to-core dependency version alignment.
- Existing test-baseline evidence keeps SQLite as the required default local integration path and leaves PostgreSQL, SQL Server, Oracle, and MySQL live-database checks opt-in unless explicitly configured.

### Scope In
- Add the repository's first CI validation workflow that runs the bounded local commands needed to trust a candidate build from the repository root.
- Run default build and test validation through `DVault.slnx`, including required SQLite integration coverage and default-run provider smoke coverage.
- Run the shared formatting and documentation gate through `bash tools/check-format.sh` as a blocking step.
- Pack the six visible packable packages and run `bash tools/verify-packages.sh` to validate artifacts, symbols, README and docs content, and nuspec metadata.
- Make workflow failures map cleanly to reproducible repository-local commands.

### Scope Out
- Provisioning PostgreSQL, SQL Server, Oracle, or MySQL services, secrets, or default-on external database jobs.
- NuGet publication, release orchestration, version stamping, or deployment automation.
- Expanding the visible six-package matrix or making non-packable anchor, benchmark, or test projects produce packages.
- Broader test-architecture refactors beyond the filter and category adjustments needed so default CI follows the existing SQLite-required and external-provider-opt-in contract.

## Acceptance Criteria
- A CI workflow runs the repository-local validation flow from the repository root using the current baseline commands: `bash tools/check-format.sh`, `dotnet build DVault.slnx`, `dotnet test DVault.slnx` with the default provider boundary, `dotnet pack DVault.slnx --configuration Release --nologo`, and `bash tools/verify-packages.sh`.
- The default workflow completes without external database services or secrets while still running required SQLite integration coverage and default-run provider smoke coverage; external-provider live-database tests run only when explicitly configured or enabled.
- The workflow blocks on `bash tools/check-format.sh`, so governed documentation and configuration text, `dotnet format` verification, and one-member-per-file enforcement are automated rather than manual review steps.
- The package-validation step fails on missing or unexpected package artifacts, missing symbols or generated XML docs, missing packaged README content, incorrect nuspec metadata, or provider-to-core dependency version drift across the six-packable-package matrix.
- Failure output identifies the concrete repository-local command or step developers can rerun to reproduce the problem outside CI.

## Definition of Done
- The workflow file and any supporting filters, scripts, or docs updates needed for the CI flow are added consistently with the shared formatting and implementation standards.
- Repository automation exercises distinct blocking stages for formatting and docs validation, build, tests, pack, and package verification.
- Default CI behavior stays within the existing SQLite-required and external-provider-opt-in test contract and does not require live external database infrastructure.
- Any optional environment switches or configuration needed to enable external-provider jobs are documented where developers or maintainers will discover them.

## Implementation Notes
- Use `DVault.slnx` as the single repository entry point for build, test, and pack automation so CI matches the documented local baseline.
- Treat `bash tools/check-format.sh` as the canonical formatting and docs-validation gate because repository policy already states the first CI workflow must call it as a blocking step.
- Reuse the existing local package-verification contract by running `dotnet pack DVault.slnx --configuration Release --nologo` and then `bash tools/verify-packages.sh` against the default `bin/packages/` output.
- Keep test selection aligned with the refined provider-category baseline from ticket `06EXB80QQHAYH61RY4X3T1E8S0`: SQLite integration remains required by default, while live-database PostgreSQL, SQL Server, Oracle, and MySQL checks stay opt-in unless configured.
- Reuse the existing six-package matrix already encoded in the repository and in the package verifier rather than inventing a broader matrix in this ticket.
- Current repository evidence already covers the adjacent baselines from tickets `06EXB80QQHAYH61RY4X3T1E8S0` and `06EXB828EAG5QE3WDR503GTBY8`; this ticket should integrate those validated local commands into CI rather than redefine them.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add scheduled or secret-backed CI jobs for configured PostgreSQL, SQL Server, Oracle, or MySQL environments once that infrastructure exists?
- Should release or publication automation later reuse this validation workflow directly or layer separate release-specific gates on top of it?

## Risks
- If workflow test filters drift from the provider-category contract, default CI could either miss required SQLite coverage or accidentally execute unconfigured external-provider tests.
- Any future change to the packable package matrix or package metadata baseline will require the CI package-verification step to be updated in lockstep.
- CI runner images must continue providing the expected .NET SDK and shell support for the repository scripts; otherwise failures will present as environment drift rather than product regressions.

## Split Recommendations
- No new split is recommended; current repository and ticket evidence keep this work bounded to wiring the existing validation commands into the first CI workflow.
- If configured external-provider jobs or release automation are needed later, capture them as separate follow-up tickets instead of expanding this ticket beyond the default validation workflow.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Automate the checks needed to trust the package candidate.

## Current Baseline
- CI must validate the full solution and every packable package in the current package matrix.
- SQLite remains the required local database integration path; PostgreSQL, SQL Server, Oracle, and MySQL external checks remain opt-in.

## Scope
- Run build, tests, formatting checks, documentation checks, and package verification.
- Run local SQLite/provider-registration checks by default and skip external-provider checks unless configured.

## Acceptance Criteria
- CI does not require external database services by default.
- Failures point to reproducible local commands.
- Pack verification covers core and provider packages, including symbols and readme/docs content.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.