<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story into a manual coordinated NuGet release gate for the six-package DVault family, with explicit pre-publish evidence, approval controls, and source-based pre-publication guidance.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 release gate covers exactly six packable packages: DCoding.Data.DVault plus MySql, Oracle, Postgres, Sqlite, and SqlServer; the non-packable src/DCoding.Data project is explicitly out of publication scope.
- Manual publication remains the only supported path in this story; no package push may occur until final publish approval is recorded.
- The current documentation and example baseline is the repository's source-consumption guidance and README quickstart content; live NuGet install commands and versioned package examples remain post-publication follow-up work.
- Local pre-publish evidence is bounded to the repo-root validation flow already defined in the repository: build, test, release pack, package verification, and formatting verification against the same checkout and intended release version.

### Scope In
- Define the coordinated release gate for the six-package DVault NuGet family as one synchronized publication unit.
- Document the required pre-publish evidence, including build, test, release pack, package verification, formatting verification, and auditable release-note review.
- Define package validation expectations for each packable package, including aligned versions, dependency alignment, readme presence, XML docs, symbols, and exclusion of unintended test/helper/benchmark packages.
- Document manual release steps, publish-order and stop-condition expectations, and the approval boundary before the first package push.
- Preserve and reference the current source/project-reference consumer guidance as the pre-publication baseline.

### Scope Out
- Adding CI/CD publish automation, release credentials, secret handling, or package push tooling.
- Changing product code, provider implementations, or NuGet metadata beyond what is needed to describe the release gate.
- Publishing only a subset of the six-package family or redefining the coordinated release as provider-by-provider.
- Introducing live NuGet installation instructions or versioned dotnet add package examples before the packages are publicly published.
- Treating the non-packable src/DCoding.Data anchor project as a publication artifact.

## Acceptance Criteria
- The ticket defines a manual release gate that blocks publication unless the full six-package DVault family is validated and approved as one synchronized release.
- The required pre-publish evidence explicitly includes successful repo-root build, test, release pack, package verification, and formatting verification against the same checkout and intended release version.
- Package validation for every packable package explicitly checks aligned package versions, correct provider-to-core dependency alignment, readme inclusion, XML documentation, symbols, and absence of unintended test/helper/benchmark publication artifacts.
- The release guidance records that release notes or equivalent auditable release evidence must be prepared and reviewed before final publish approval, and that approval must be recorded before the first package push.
- The release documentation clearly distinguishes current source-based developer and consumer guidance from future post-publication NuGet-first guidance and does not present live NuGet install commands as current usage.

## Definition of Done
- Repository release guidance documents the six-package publication scope, required evidence, approval gate, and manual release boundaries in a way that matches the current DVault package-family baseline.
- The local validation path for release readiness is documented from the repository root and includes the existing solution, package-verification, and formatting gates.
- The documented package checklist states how maintainers verify version alignment, dependency alignment, package contents, and coordinated publish readiness for every packable package.
- The guidance explicitly excludes automatic publish, subset releases, and pre-publication NuGet-consumer instructions from the current release path.

## Implementation Notes
- Use docs/manual-nuget-publication.md as the primary release-gate artifact and keep it aligned with README.md source-consumption guidance.
- Ratify the visible v1 package matrix from the repository and treat the release as coordinated across DCoding.Data.DVault, DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer only.
- Use the existing repo-root validation baseline as the local pack verification path: dotnet build DVault.slnx --nologo, dotnet test DVault.slnx --nologo, dotnet pack DVault.slnx --configuration Release --nologo, bash tools/verify-packages.sh, and bash tools/check-format.sh.
- Keep the story at release-governance level; do not expand it into CI automation, credential management, package push scripting, or provider-specific implementation work.
- Treat README quickstart and source-project-reference documentation as the bounded v1 docs and examples baseline because the current branch snapshot does not expose a separate sample application surface.

## Open Questions
- none

## Follow-Up Questions
- After the packages are publicly available, what NuGet-first installation guidance and versioned examples should replace or supplement the current source-based README instructions?
- Should a later story automate the same validated manual release gate in CI while preserving the explicit human approval boundary before package push?
- Does the team want a separate post-MVP artifact for public-facing release notes or changelog publication beyond the auditable internal release evidence required here?

## Risks
- Because publication remains manual across six coordinated packages, a missed checklist step or partial-family push would create version and dependency drift unless the documented gate is followed strictly.
- If package verification does not actually inspect all required artifacts for every package, the release gate could appear complete while still shipping incomplete or unintended package contents.
- Documentation drift between the manual publication guide and README consumer guidance could confuse maintainers about whether source-based setup or NuGet-based setup is currently supported.

## Split Recommendations
- If release credential handling, package push tooling, or CI-driven publication is needed, schedule that as a separate follow-on story after the manual release gate is accepted.
- If public NuGet consumer documentation is needed immediately after first publication, schedule a separate documentation story for post-publication installation guidance and examples.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Define evidence required before publishing the package family to NuGet.

## Current Baseline
- The release candidate is now a package family: `DCoding.Data.DVault` plus SQLite, PostgreSQL, SQL Server, Oracle, and MySQL provider extension packages.
- Package validation must check aligned versions, package dependencies, readme files, XML docs, symbols, and absence of unintended test/helper/benchmark packages.

## Scope
- Create package validation checklist for the full package matrix.
- Add local pack verification.
- Document manual release steps and approval boundaries.

## Acceptance Criteria
- Publication is explicitly gated by tests, docs, examples, and package validation for every packable package.
- No automatic publish occurs before approval.
- The gate distinguishes source/development guidance from future NuGet-first consumer guidance.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.