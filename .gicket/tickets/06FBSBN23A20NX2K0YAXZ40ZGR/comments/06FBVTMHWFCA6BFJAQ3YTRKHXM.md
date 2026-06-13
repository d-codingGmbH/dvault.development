[gicket-bot] PO-critic review contract

Summary
- Return to PO: the authoritative contract says the v0.36 planning/current-baseline documentation is already aligned, but repository and branch-history evidence show only project/test/package-verifier updates while `docs/plans/shared-implementation-standards.md`, `docs/releases/v0.36.0.md`, and the v0.36 checklist language still reflect the older matrix or former cross-line MySQL exception.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket `06FBSBN23A20NX2K0YAXZ40ZGR` says in Clarifications/Implementation Notes that `docs/plans/shared-implementation-standards.md` 'now codifies the current v0.36 dependency-line policy and version matrix' and `## Open Questions` is `none`.
- `git log --oneline main..HEAD` on `ticket/06FBSBN23A20NX2K0YAXZ40ZGR-story-codify-dependency-line-policy-after-packag` shows only workflow handoff commits plus `c495a26be Updated library versions` and `2ded6dea7 Align net8 dependency line with EF Core 8`.
- `git diff --name-only main..HEAD -- docs/plans/shared-implementation-standards.md docs/releases/v0.36.0.md docs/production-adoption-checklist.md src/DCoding.Data.DVault/DCoding.Data.DVault.csproj tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` lists only the `.csproj`, test, and `PackageVerifier.cs` files; none of the three documentation files appear in the diff.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` currently pins `net8.0` EF/Relational `8.0.28` with `Microsoft.Extensions.DependencyInjection.Abstractions` `8.0.2`, and `net10.0` EF/Relational/DI.Abstractions `10.0.9`.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` and `tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs` currently pin `MySql.EntityFrameworkCore` `8.0.26` for `net8.0` and `10.0.7` for `net10.0`, plus `Sqlite`/`SqlServer` `8.0.28` and `10.0.9` per target.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` maps EF versions to `8.0.28` for `net8.0` and `10.0.9` for `net10.0` (`GetEfCoreVersion` / `GetDependencyInjectionAbstractionsVersion`).
- `docs/plans/shared-implementation-standards.md` still says the required provider evidence is `Sqlite` `8.0.27` / `10.0.8`, `SqlServer` `8.0.27` / `10.0.8`, and that `MySql.EntityFrameworkCore` `10.0.7` is the required exception for both targets.
- `docs/releases/v0.36.0.md` still carries the old compatibility matrix (`net8.0` EF/Relational `8.0.27`, `net10.0` EF/Relational/DI `10.0.8`) and repeats `MySql.EntityFrameworkCore` `10.0.7` as an exception across both targets.
- `docs/production-adoption-checklist.md` still instructs readers not to treat the `MySql.EntityFrameworkCore` `10.0.7` exception as mixed-line permission and treats `docs/releases/v0.36.0.md` as the current public v0.36 baseline.

Blocking findings
- The ticket leaves current-baseline documentation scope unresolved in practice: `docs/releases/v0.36.0.md` and `docs/production-adoption-checklist.md` still carry the old release-line story, while the contract phrases those surfaces as a Follow-Up Question even though Scope In, Acceptance Criteria, and Definition of Done all imply they must be aligned before the story is complete.

Required PO actions
- Amend the delivery contract so the repository-state claims match what is actually on this branch. Either point to landed evidence for the planning-surface update or mark `docs/plans/shared-implementation-standards.md` as still in scope.
- Resolve the current-baseline documentation scope explicitly instead of leaving it as a Follow-Up Question. Name the exact in-scope surfaces for this story, at minimum whether `docs/releases/v0.36.0.md` and `docs/production-adoption-checklist.md` must be updated here.
- Refresh the PO Summary/Implementation Notes so the remaining work statement matches the verified repo state and the developer handoff is unambiguous.

Open issues ledger
- critic-item-1 [required-po-action] Amend the delivery contract so the repository-state claims match what is actually on this branch. Either point to landed evidence for the planning-surface update or mark `docs/plans/shared-implementation-standards.md` as still in scope.
- critic-item-2 [required-po-action] Resolve the current-baseline documentation scope explicitly instead of leaving it as a Follow-Up Question. Name the exact in-scope surfaces for this story, at minimum whether `docs/releases/v0.36.0.md` and `docs/production-adoption-checklist.md` must be updated here.
- critic-item-3 [required-po-action] Refresh the PO Summary/Implementation Notes so the remaining work statement matches the verified repo state and the developer handoff is unambiguous.
- critic-item-4 [blocking-finding] The ticket leaves current-baseline documentation scope unresolved in practice: `docs/releases/v0.36.0.md` and `docs/production-adoption-checklist.md` still carry the old release-line story, while the contract phrases those surfaces as a Follow-Up Question even though Scope In, Acceptance Criteria, and Definition of Done all imply they must be aligned before the story is complete.

Missing examples / edge cases
- The contract does not explicitly distinguish which sections of `docs/plans/shared-implementation-standards.md` are current v0.36 baseline versus intentionally historical audit content, so a developer could over-edit or under-edit that file.
- The contract does not explicitly say whether any other current-baseline guidance surfaces beyond the planning doc, `docs/releases/v0.36.0.md`, and `docs/production-adoption-checklist.md` must be re-verified for exact patch-level alignment.

Risky assumptions
- Assuming the only remaining current-baseline documentation surfaces are `docs/plans/shared-implementation-standards.md`, `docs/releases/v0.36.0.md`, and `docs/production-adoption-checklist.md`; README/manual-publication/local-validation look aligned, but the ticket does not explicitly freeze the surface list.
- Assuming there is no separate landed doc evidence outside this branch, because `git diff main..HEAD` and the branch-local commit history show no changes to the relevant documentation files.

AC / test suggestions
- Name the exact current-baseline documentation surfaces that must align: `docs/plans/shared-implementation-standards.md`, `docs/releases/v0.36.0.md`, and `docs/production-adoption-checklist.md` if those are intended in scope.
- Spell out the exact accepted matrix the docs must reflect: net8 EF/Relational `8.0.28`, DI.Abstractions `8.0.2`, MySQL `8.0.26`, SQLite `8.0.28`, SQL Server `8.0.28`; net10 EF/Relational/DI.Abstractions `10.0.9`, MySQL `10.0.7`, SQLite `10.0.9`, SQL Server `10.0.9`.
- State explicitly that historical release documents or historical contract sections may remain historical and are not blockers as long as current-baseline guidance is consistent.

Implementation watchouts
- The repo already codifies the target-specific matrix in project files, matrix tests, and package verification; documentation updates must mirror those repo-visible values exactly rather than the older v0.35-style matrix text.
- The shared standards file mixes current baseline text with historical compatibility sections, so the ticket needs clear boundaries for what must be updated versus preserved as historical context.
- Because the branch history after `c495a26be` and `2ded6dea7` contains only workflow claim/handoff commits, reviewers should not assume a later documentation sync landed without explicit evidence.

Non-blocking notes
- `README.md`, `docs/manual-nuget-publication.md`, and `docs/local-validation.md` already reflect the dual `8.36.0` / `10.36.0` package-line guidance.
- The persisted contract's `## Open Questions` section is resolved (`none`); the blocker is inaccurate handoff text and unresolved current-baseline surface scoping, not the presence of an open-question marker.
- The story's live done-task `blocks` relations are noted in the contract as historical implementation evidence and are not a PO-critic blocker on their own.

Split recommendations
- No split is needed if PO keeps the current-baseline documentation sync in this story and names the exact surfaces. If PO wants release-note/adoption-checklist alignment to happen elsewhere, create an explicit follow-up ticket instead of leaving that routing decision in `## Follow-Up Questions`.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment