[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 4/4 acceptance criteria and 3/3 definition-of-done expectations on branch \u0027ticket/06FBSBN23A20NX2K0YAXZ40ZGR-story-codify-dependency-line-policy-after-packag\u0027 at commit \u00273790fc8c0117\u0027.",
  "implementationReference": {
    "branchName": "ticket/06FBSBN23A20NX2K0YAXZ40ZGR-story-codify-dependency-line-policy-after-packag",
    "commitSha": "3790fc8c0117",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md all state that 8.36.0 / net8.0 uses the EF Core 8 line and 10.36.0 / net10.0 uses the EF Core 10 line, with no mixed-line restored target.",
      "satisfied": true,
      "reason": "docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md now state that 8.36.0/net8.0 uses the EF Core 8 line, 10.36.0/net10.0 uses the EF Core 10 line, and a single restored target must not mix 8.x and 10.x dependency lines."
    },
    {
      "expectation": "Those three surfaces record the current accepted versions as net8: EF 8.0.28, Relational 8.0.28, DI.Abstractions 8.0.2, DB2 8.0.0.400, SQLite 8.0.28, MySQL 8.0.26, PostgreSQL 8.0.11, Oracle 8.23.26200, SQL Server 8.0.28; and net10: EF 10.0.9, Relational 10.0.9, DI.Abstractions 10.0.9, DB2 10.0.0.100, SQLite 10.0.9, MySQL 10.0.7, PostgreSQL 10.0.2, Oracle 10.23.26200, SQL Server 10.0.9.",
      "satisfied": true,
      "reason": "Those three surfaces now record the current accepted target-specific versions: net8 EF 8.0.28, Relational 8.0.28, DI.Abstractions 8.0.2, DB2 8.0.0.400, SQLite 8.0.28, MySQL 8.0.26, PostgreSQL 8.0.11, Oracle 8.23.26200, SQL Server 8.0.28; and net10 EF 10.0.9, Relational 10.0.9, DI.Abstractions 10.0.9, DB2 10.0.0.100, SQLite 10.0.9, MySQL 10.0.7, PostgreSQL 10.0.2, Oracle 10.23.26200, SQL Server 10.0.9."
    },
    {
      "expectation": "Current-baseline documentation stops describing v0.36 as merely carrying forward the 8.0.27 / 10.0.8 matrix or a cross-target MySQL 10.0.7 exception.",
      "satisfied": true,
      "reason": "The current v0.36 planning, release, and adopter-guidance sections no longer describe v0.36 as carrying forward the 8.0.27/10.0.8 matrix or a cross-target MySQL 10.0.7 exception; the only remaining stale values are confined to the explicitly historical v0.33 section."
    },
    {
      "expectation": "Documentation wording makes clear that patch movement is allowed only within the selected target major line and that the existing project, test, and verifier matrix is the source of truth.",
      "satisfied": true,
      "reason": "The updated wording now makes patch movement target-major-line-only and ties the documentation baseline back to the project files, matrix tests, and package verifier expectations rather than treating mixed-line restoration as acceptable."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The three named current-baseline surfaces no longer contradict src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj, tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs, or tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs.",
      "satisfied": true,
      "reason": "The three scoped current-baseline docs now match the directly inspected source/test/verifier evidence in src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj, tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs, and tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs."
    },
    {
      "expectation": "No current planning, release, or adopter-guidance surface still says net8 carries the old 8.0.27 lane or MySQL 10.0.7 as a standing cross-target exception for both targets.",
      "satisfied": true,
      "reason": "No current v0.36 planning, release, or adopter-guidance surface still says net8 carries the old 8.0.27 lane or that MySQL 10.0.7 is a standing cross-target exception for both targets; current guidance is now target-specific."
    },
    {
      "expectation": "Historical release documents remain historical and are not rewritten unless they are explicitly being used as current-baseline guidance.",
      "satisfied": true,
      "reason": "The branch diff against develop changes only the three scoped current-baseline docs plus ticket metadata, so historical release documents were not rewritten."
    }
  ],
  "evidence": [
    "git diff --name-only develop...3790fc8c0117 shows repository content changes only in docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md; the other diff entries are under .gicket/ ticket metadata.",
    "git diff --stat develop...3790fc8c0117 for the scoped documentation paths reports 3 files changed, 17 insertions, and 15 deletions.",
    "src/DCoding.Data.DVault/DCoding.Data.DVault.csproj at 3790fc8c0117 pins net8 Microsoft.EntityFrameworkCore and Microsoft.EntityFrameworkCore.Relational at 8.0.28 with Microsoft.Extensions.DependencyInjection.Abstractions 8.0.2, and pins all three at 10.0.9 for net10.",
    "tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj at 3790fc8c0117 pins MySql.EntityFrameworkCore 8.0.26 for net8 and 10.0.7 for net10, alongside SQLite 8.0.28/10.0.9, DB2 8.0.0.400/10.0.0.100, PostgreSQL 8.0.11/10.0.2, Oracle 8.23.26200/10.23.26200, and SQL Server 8.0.28/10.0.9.",
    "tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs at 3790fc8c0117 asserts the same per-target package matrix and checks the target-specific package references explicitly.",
    "tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs at 3790fc8c0117 defines expected package lines 8.36.0/net8.0/EF Core 8 and 10.36.0/net10.0/EF Core 10, with GetEfCoreVersion returning 8.0.28 for net8 and 10.0.9 for net10 and the corresponding DI.Abstractions and DB2 versions.",
    "README.md, docs/manual-nuget-publication.md, and docs/local-validation.md at 3790fc8c0117 remain aligned with the dual 8.36.0 and 10.36.0 package-line posture and are unchanged by the branch diff.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/api-compatibility, area/architecture, area/ef-core, area/packaging, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027develop\u0027.",
    "Ticket history references implementation commit \u00273790fc8c0117\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06FBSBN23A20NX2K0YAXZ40ZGR`
- target-role: `integrator`
- verification-summary: Tester verified 4/4 acceptance criteria and 3/3 definition-of-done expectations on branch 'ticket/06FBSBN23A20NX2K0YAXZ40ZGR-story-codify-dependency-line-policy-after-packag' at commit '3790fc8c0117'.
- acceptance-criteria: `4/4` satisfied
- definition-of-done: `3/3` satisfied
- implementation-branch: `ticket/06FBSBN23A20NX2K0YAXZ40ZGR-story-codify-dependency-line-policy-after-packag`
- implementation-commit: `3790fc8c0117`
- implementation-pr: `<none>`
- implementation-change: `<none>`