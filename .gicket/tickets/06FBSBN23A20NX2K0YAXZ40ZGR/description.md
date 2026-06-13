<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to match the repo-visible v0.36 dependency matrix: code, tests, and package verification already enforce the target-matched EF lines, while docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md still need alignment.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already answers the policy question: each target framework follows the matching EF Core major line, and a resolved target must not mix 8.x and 10.x dependency lines.
- The current visible baseline is net8 EF 8.0.28 / Relational 8.0.28 / DI.Abstractions 8.0.2 with DB2 8.0.0.400, SQLite 8.0.28, MySQL 8.0.26, PostgreSQL 8.0.11, Oracle 8.23.26200, SQL Server 8.0.28; and net10 EF 10.0.9 / Relational 10.0.9 / DI.Abstractions 10.0.9 with DB2 10.0.0.100, SQLite 10.0.9, MySQL 10.0.7, PostgreSQL 10.0.2, Oracle 10.23.26200, SQL Server 10.0.9.
- docs/plans/shared-implementation-standards.md has not yet been updated on this branch, so the contract keeps that planning surface in scope instead of treating it as landed evidence.
- README.md, docs/manual-nuget-publication.md, and docs/local-validation.md already reflect the dual 8.36.0 / 10.36.0 package-line posture and do not need new scope unless contradictory evidence is found.
- No child-ticket split or additional planning artifact is justified from the current branch evidence; the remaining work is a bounded documentation-alignment pass.

### Scope In
- Update docs/plans/shared-implementation-standards.md V0.36 Compatibility Contract to the repo-visible net8/net10 dependency matrix and target-matched major-line rule.
- Update docs/releases/v0.36.0.md so the Compatibility Matrix and explanatory text use the current 8.0.28 / 10.0.9 baselines and net8 MySQL 8.0.26 instead of the carried-forward 8.0.27 / 10.0.8 and cross-target MySQL 10.0.7 story.
- Update docs/production-adoption-checklist.md so its v0.36 baseline bullets describe the same target-specific matrix and do not present MySQL 10.0.7 as general mixed-line permission.
- Use the current project files, matrix tests, and PackageVerifier expectations as the authoritative evidence source for those documentation updates.

### Scope Out
- Re-opening the dependency-line policy itself; the branch already ratifies target-matched EF Core major lines.
- Changing package references, matrix tests, or PackageVerifier to a different version policy unless new contradictory repository evidence is found.
- Rewriting historical v0.33 through v0.35 release notes or other intentionally historical documentation.
- Updating README.md, docs/manual-nuget-publication.md, or docs/local-validation.md absent newly discovered contradictions, because those surfaces already align with the repo-visible package-line policy.

## Acceptance Criteria
- docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md all state that 8.36.0 / net8.0 uses the EF Core 8 line and 10.36.0 / net10.0 uses the EF Core 10 line, with no mixed-line restored target.
- Those three surfaces record the current accepted versions as net8: EF 8.0.28, Relational 8.0.28, DI.Abstractions 8.0.2, DB2 8.0.0.400, SQLite 8.0.28, MySQL 8.0.26, PostgreSQL 8.0.11, Oracle 8.23.26200, SQL Server 8.0.28; and net10: EF 10.0.9, Relational 10.0.9, DI.Abstractions 10.0.9, DB2 10.0.0.100, SQLite 10.0.9, MySQL 10.0.7, PostgreSQL 10.0.2, Oracle 10.23.26200, SQL Server 10.0.9.
- Current-baseline documentation stops describing v0.36 as merely carrying forward the 8.0.27 / 10.0.8 matrix or a cross-target MySQL 10.0.7 exception.
- Documentation wording makes clear that patch movement is allowed only within the selected target major line and that the existing project, test, and verifier matrix is the source of truth.

## Definition of Done
- The three named current-baseline surfaces no longer contradict src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj, tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs, or tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs.
- No current planning, release, or adopter-guidance surface still says net8 carries the old 8.0.27 lane or MySQL 10.0.7 as a standing cross-target exception for both targets.
- Historical release documents remain historical and are not rewritten unless they are explicitly being used as current-baseline guidance.

## Implementation Notes
- Do not spend this story on another dependency-matrix redesign. The repo already landed the policy in src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj, tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs, and tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs.
- The remaining delivery work is documentation alignment only: docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md still describe the older 8.0.27 / 10.0.8 baseline and or the old cross-target MySQL 10.0.7 exception.
- Use README.md, docs/manual-nuget-publication.md, and docs/local-validation.md as corroborating examples of the already-accepted 8.36.0 / 10.36.0 package-line posture; they are not update targets for this story unless a contradiction is discovered during delivery.
- If relation metadata still shows done implementation tickets as blocks, treat that as closure housekeeping rather than dependency-line policy scope.

## Open Questions
- none

## Follow-Up Questions
- none

## Risks
- Until the three named documentation surfaces are aligned, release operators and consumers will continue to see guidance that contradicts the already-landed project, test, and package-verifier baseline.
- If historical done-task blocks relations remain in live ticket metadata, automation or humans may misread delivery state even after the documentation work is complete.

## Split Recommendations
- No split recommended. The remaining work is a bounded documentation-alignment pass across docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Decide and document whether each DVault package major line tracks the EF Core major line or the newest package versions compatible with the target framework. Acceptance: package references, package verifier expectations, README/manual-publication guidance, and release notes can all be made consistent from this decision. Explicitly address the current net8.0 references to EF Core 9.x and DI 9.x, and the net10.0 move from 10.0.8 to 10.0.9.