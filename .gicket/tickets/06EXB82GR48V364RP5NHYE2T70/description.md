<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket against the current six-package DVault family, current source-based installation docs, and the existing build/test/pack/package-verification baseline; no split or planning artifact was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence fixes the v1 release family as six packable packages: DCoding.Data.DVault plus DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer; src/DCoding.Data is non-packable and out of publication scope.
- README already establishes source/project-reference consumption as the current baseline and defers live NuGet install commands until packages are published, so this ticket should document that distinction rather than rewrite installation guidance.
- The current validation baseline is already visible in repo docs: dotnet build DVault.slnx --nologo, dotnet test DVault.slnx --nologo, dotnet pack DVault.slnx --configuration Release --nologo, bash tools/verify-packages.sh, and bash tools/check-format.sh.
- Existing relations already place this task under story 06EXB8202A88KJJP7WEGBESBYM and show done upstream blocker tickets 06EXB82RW6PV2NFG088G6BPFHC and 06EXB7TP9PF2XFRQ9MG7CJQR10.

### Scope In
- Document one manual release checklist for the coordinated DVault NuGet family rather than per-package publication instructions.
- Document required pre-publish quality evidence, package creation and validation, version alignment, publish ordering, stop conditions, and final publish approval.
- Document how maintainers confirm provider packages reference the aligned core package version before any push.
- Document the current source-consumption guidance versus future post-publication NuGet-first consumer guidance.

### Scope Out
- Automating publishing, adding CI/CD release credentials, or introducing package push tooling.
- Changing product code, package metadata, or provider implementation behavior.
- Publishing only a subset of the package family for the planned coordinated release.
- Replacing current README source-installation guidance with live NuGet commands before packages exist.

## Acceptance Criteria
- The release document names the full coordinated package family and states that manual publication must not proceed for only a subset of that family.
- The document lists the current minimum pre-publish quality evidence using the visible repository baseline: dotnet build DVault.slnx --nologo, dotnet test DVault.slnx --nologo, dotnet pack DVault.slnx --configuration Release --nologo, bash tools/verify-packages.sh, and bash tools/check-format.sh.
- The document requires one aligned release version across all six packages and verification that each provider package depends on the packed DCoding.Data.DVault version before publish approval.
- The document includes a manual step to prepare and review release notes or changelog content for the coordinated release before final publish approval, without assuming a pre-existing changelog automation system.
- The document defines an anti-partial-publication flow that validates all packages before any push, publishes the core package first, then publishes provider extension packages in the documented order MySql, Oracle, Postgres, Sqlite, and SqlServer, and stops the release immediately if any validation or push step fails.
- The document states that current developer/consumer setup remains source/project-reference based until public packages exist, and that live dotnet add package commands or version examples belong only to later post-publication guidance.

## Definition of Done
- A repository documentation page or checklist is added or updated with the manual release criteria, sequence, and guardrails defined by this ticket.
- The documented flow uses the current repository validation and package-verification commands instead of inventing a new automation path.
- The checklist makes the release-note or changelog recording step explicit, even if the implementation chooses the recording location inside the new documentation rather than introducing separate tooling.
- The documentation clearly distinguishes packable release artifacts from the non-packable src/DCoding.Data anchor and preserves the current source-based installation baseline.
- Documentation changes satisfy shared repository documentation standards and the ticket acceptance criteria.

## Implementation Notes
- Use README.md as the baseline for current installation and validation behavior; it already documents source/project-reference consumption, solution-level build/test/pack commands, and bash tools/verify-packages.sh package verification.
- Use the existing project files as the package identity source of truth; provider projects reference ../DCoding.Data.DVault/DCoding.Data.DVault.csproj, which is the basis for requiring core-package publication before provider-package pushes.
- Treat verify-packages output as the manual dependency/alignment gate because it already checks for exactly the six DVault packages, matching symbol packages, README/XML metadata, and provider dependency alignment on DCoding.Data.DVault.
- No child tickets, relation writes, or planning documents were materialized during this refinement run.

## Open Questions
- none

## Follow-Up Questions
- After the first public publication, should a separate ticket switch the README installation section from source/project references to NuGet-first examples while preserving pre-release contributor guidance elsewhere?
- Should a later release-management ticket introduce automation or a machine-readable release checklist once the manual publication flow has stabilized?
- Should the project later standardize a dedicated changelog or release-notes file, or keep release-note recording inside release documentation and ticket artifacts?

## Risks
- If the documentation leaves the release-note or changelog location implicit, manual releases may still diverge even though the rest of the checklist is explicit.
- Because publishing remains manual, any checklist that does not force full-family validation before the first push still leaves room for accidental partial publication.
- Future provider-specific release needs could pressure the coordinated family-release rule, so the documentation should state that the current v1 baseline is synchronized publication across all six packages.

## Split Recommendations
- No split recommended; the work is a single bounded documentation task for the current manual six-package NuGet release process.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Write the manual release checklist for future NuGet publishing.

## Current Baseline
- The release checklist must treat core plus provider extension packages as one aligned package family.
- Publishing remains manual and should avoid accidental partial publication of only one package.

## Scope
- Document required quality evidence, versioning, changelog, package validation, package publish order, and publish approval.
- Document the distinction between current source/development setup and future NuGet-first consumer installation guidance.

## Acceptance Criteria
- Docs state that publishing waits for sufficient quality and feature coverage.
- Manual steps avoid accidental publication.
- Checklist covers aligned versions and dependencies for `DCoding.Data.DVault` and all provider extension packages.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.