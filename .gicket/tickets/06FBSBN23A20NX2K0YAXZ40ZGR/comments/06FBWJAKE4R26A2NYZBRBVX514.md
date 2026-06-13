[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the ticket now scopes a bounded docs-only alignment pass against repo-visible source/test/verifier evidence, and the persisted contract has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSBN23A20NX2K0YAXZ40ZGR/description.md says the remaining work is updating docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md; its ## Open Questions and ## Follow-Up Questions are both none.
- Comment .gicket/tickets/06FBSBN23A20NX2K0YAXZ40ZGR/comments/06FBVWXS99WCBN8XT5GDZXG0MG.md explicitly answers the prior PO-critic blockers and marks the ticket ready_for_po_critic with those three docs in scope.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj pins net8.0 EF/Relational 8.0.28 plus DI.Abstractions 8.0.2, and net10.0 EF/Relational/DI.Abstractions 10.0.9.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj and tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs pin MySql.EntityFrameworkCore 8.0.26 on net8.0 and 10.0.7 on net10.0, plus target-matched Sqlite and SqlServer versions.
- tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs enforces GetEfCoreVersion(net8.0)=8.0.28 and GetEfCoreVersion(net10.0)=10.0.9, matching the contract's authoritative repo baseline.
- docs/plans/shared-implementation-standards.md and docs/releases/v0.36.0.md still show stale 8.0.27 / 10.0.8 rows and cross-target MySQL 10.0.7 wording, while docs/production-adoption-checklist.md still routes v0.36 baseline readers through that release note and repeats the 10.0.7 evidence-exception language; this matches the ticket's stated remaining documentation work.
- git diff --name-only 83f046acb...e9b455775 returns only .gicket/tickets/06FBSBN23A20NX2K0YAXZ40ZGR/... metadata files, so the branch is still at pre-development handoff state rather than carrying half-implemented repo changes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not provide sample replacement prose for the MySQL exception wording, so review should key off exact version and mixed-line semantics rather than wording similarity.

Risky assumptions
- Assumes the three named docs are the only remaining current-baseline update surfaces, because the contract explicitly scopes out README.md, docs/manual-nuget-publication.md, and docs/local-validation.md unless contradictory evidence appears.
- Assumes historical sections such as the v0.33 compatibility block in docs/plans/shared-implementation-standards.md remain audit context and are not to be rewritten during this story.

AC / test suggestions
- Reviewer acceptance should compare the three target docs against src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs, and tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs for exact patch-level values.
- Acceptance should explicitly confirm that a resolved target cannot mix 8.x and 10.x dependency lines and that net8 MySQL is 8.0.26 while net10 MySQL remains 10.0.7.

Implementation watchouts
- docs/production-adoption-checklist.md treats docs/releases/v0.36.0.md as the current v0.36 baseline, so those two docs must be updated coherently.
- docs/plans/shared-implementation-standards.md mixes current and historical compatibility sections; update the v0.36 current-baseline content without rewriting historical audit sections.
- The branch currently differs from develop only in .gicket metadata, so all repository documentation edits for this story still need to be authored.

Non-blocking notes
- README.md, docs/manual-nuget-publication.md, and docs/local-validation.md already reflect the dual 8.36.0 / 10.36.0 package-line posture, consistent with the contract's scoped-out corroborating surfaces.

Split recommendations
- No split recommended; the contract already bounds the work to three documentation surfaces and exact repo-visible version values.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment