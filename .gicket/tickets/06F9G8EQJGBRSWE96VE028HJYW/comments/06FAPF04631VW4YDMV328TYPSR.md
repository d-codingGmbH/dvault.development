[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 7/7 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F9G8EQJGBRSWE96VE028HJYW-story-define-net8-0-and-net10-0-compatibility-co\u0027 at commit \u002790a2ece2157c\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F9G8EQJGBRSWE96VE028HJYW-story-define-net8-0-and-net10-0-compatibility-co",
    "commitSha": "90a2ece2157c",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "The refined contract states that the coordinated DVault family remains the same seven package IDs across all lines, with no line-specific package-ID split or duplicate artifact naming scheme.",
      "satisfied": true,
      "reason": "The new \u0060V0.33 Compatibility Contract\u0060 section lists the unchanged seven DVault package IDs and explicitly rejects line-specific package-ID splits or duplicate artifact naming."
    },
    {
      "expectation": "The refined contract states that planning release v0.33.0 produces two aligned consumer package lines only: 8.33.0 for the net8.0 and EF Core 8 compatibility line and 10.33.0 for the net10.0 and EF Core 10 compatibility line. It explicitly rejects any consumer-facing 0.33.0 package version and any mixed-line artifact family.",
      "satisfied": true,
      "reason": "The updated document maps planning release \u0060v0.33.0\u0060 only to package lines \u00608.33.0\u0060 for \u0060net8.0\u0060 and EF Core 8 and \u006010.33.0\u0060 for \u0060net10.0\u0060 and EF Core 10, and it forbids any consumer-facing \u00600.33.0\u0060 package version or mixed-line artifact family."
    },
    {
      "expectation": "The net8.0 compatibility line pins the required provider package evidence to Microsoft.EntityFrameworkCore.Sqlite 8.0.27, MySql.EntityFrameworkCore 10.0.7, Npgsql.EntityFrameworkCore.PostgreSQL 8.0.11, Oracle.EntityFrameworkCore 8.23.26200, and Microsoft.EntityFrameworkCore.SqlServer 8.0.27.",
      "satisfied": true,
      "reason": "The provider matrix in \u0060docs/plans/shared-implementation-standards.md\u0060 pins the net8.0 line to \u0060Microsoft.EntityFrameworkCore.Sqlite\u0060 \u00608.0.27\u0060, \u0060MySql.EntityFrameworkCore\u0060 \u006010.0.7\u0060, \u0060Npgsql.EntityFrameworkCore.PostgreSQL\u0060 \u00608.0.11\u0060, \u0060Oracle.EntityFrameworkCore\u0060 \u00608.23.26200\u0060, and \u0060Microsoft.EntityFrameworkCore.SqlServer\u0060 \u00608.0.27\u0060."
    },
    {
      "expectation": "The net10.0 compatibility line pins the required provider package evidence to Microsoft.EntityFrameworkCore.Sqlite 10.0.8, MySql.EntityFrameworkCore 10.0.7, Npgsql.EntityFrameworkCore.PostgreSQL 10.0.2, Oracle.EntityFrameworkCore 10.23.26200, and Microsoft.EntityFrameworkCore.SqlServer 10.0.8.",
      "satisfied": true,
      "reason": "The same provider matrix pins the net10.0 line to \u0060Microsoft.EntityFrameworkCore.Sqlite\u0060 \u006010.0.8\u0060, \u0060MySql.EntityFrameworkCore\u0060 \u006010.0.7\u0060, \u0060Npgsql.EntityFrameworkCore.PostgreSQL\u0060 \u006010.0.2\u0060, \u0060Oracle.EntityFrameworkCore\u0060 \u006010.23.26200\u0060, and \u0060Microsoft.EntityFrameworkCore.SqlServer\u0060 \u006010.0.8\u0060."
    },
    {
      "expectation": "Allowed conditional PackageReference logic is limited to target-framework selection and the existing opt-in external-provider test switches. Every resolved target and every published artifact family must contain exactly one compatible EF and provider dependency line and must not resolve both 8.x and 10.x packages together.",
      "satisfied": true,
      "reason": "The contract limits conditional \u0060PackageReference\u0060 usage to target-framework selection and the existing opt-in external-provider test switches, and it explicitly forbids resolving both the 8.x and 10.x dependency lines in one target."
    },
    {
      "expectation": "The optional analyzer package keeps the current analyzer and source-generator asset boundary: coordinated family membership, PrivateAssets=all guidance, analyzer assets present in package verification, and no new runtime or transitive dependency behavior.",
      "satisfied": true,
      "reason": "The contract keeps \u0060DCoding.Data.DVault.Analyzers\u0060 as coordinated-family tooling, requires local analyzer/source-generator references with \u0060PrivateAssets=all\u0060, requires analyzer asset verification, and rejects new runtime or transitive analyzer dependency behavior."
    },
    {
      "expectation": "Downstream package verification, matrix tests, and documentation must fail or be treated as incomplete when they blur planning release v0.33.0 with package versions 8.33.0 and 10.33.0, miss one of the required provider pins, or allow mixed-line dependency resolution.",
      "satisfied": true,
      "reason": "The updated document says downstream package verification, matrix tests, release notes, README guidance, and CI documentation are incomplete if they blur planning release \u0060v0.33.0\u0060 with package versions \u00608.33.0\u0060 and \u006010.33.0\u0060, omit required provider pins, or allow mixed-line dependency resolution."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The ticket carries an authoritative PO contract for target-framework support, package-line mapping, provider-version pinning, analyzer handling, and conditional-reference boundaries, with no blocking PO questions left open.",
      "satisfied": true,
      "reason": "The ticket description remains the authoritative PO contract, the shared standards document now mirrors that contract, and the ticket\u0027s \u0060Open Questions\u0060 section is \u0060none\u0060."
    },
    {
      "expectation": "The contract stays consistent with current repository evidence: the existing seven package IDs, the current net10.0-only project baseline, the analyzer package\u0027s local-asset posture, the opt-in external-provider test pattern, and the already-completed v0.33 version-line policy ticket.",
      "satisfied": true,
      "reason": "Repository evidence still shows the seven package IDs and a net10.0-only baseline across src/tests, the integration test project still uses opt-in provider conditions and analyzer \u0060PrivateAssets=all\u0060, and upstream ticket \u006006F9GF2Z4Y7A91ZHG4NW1YTNMC\u0060 is \u0060done\u0060."
    },
    {
      "expectation": "Sibling tickets for multitargeting, provider matrix tests, verifier guidance, and v0.33 documentation can implement against this ticket without reopening package IDs, provider-version selections, analyzer export behavior, or consumer version-line wording.",
      "satisfied": true,
      "reason": "The shared standards document now gives downstream multitargeting, provider-matrix, verifier, and documentation tickets a single source for package-line mapping, provider pins, analyzer boundaries, conditional-reference limits, and deferred implementation scope."
    },
    {
      "expectation": "The ticket does not imply new runtime behavior, automatic publication, provider provisioning, or other out-of-scope platform changes.",
      "satisfied": true,
      "reason": "The updated document explicitly says the v0.33 contract does not itself edit project files, add provider behavior, provision databases, publish packages, or create release automation, and it defers those items to sibling tickets."
    }
  ],
  "evidence": [
    "\u0060git -C /mnt/c/Projects/DVault diff --name-only develop...90a2ece2157c\u0060 showed only ticket metadata plus \u0060docs/plans/shared-implementation-standards.md\u0060; no \u0060src/\u0060 or \u0060tests/\u0060 implementation files changed on this branch.",
    "\u0060docs/plans/shared-implementation-standards.md\u0060 now adds a \u0060V0.33 Compatibility Contract\u0060 section covering the seven package IDs, the \u00608.33.0\u0060/\u006010.33.0\u0060 mapping, the exact net8.0 and net10.0 provider version table, the conditional \u0060PackageReference\u0060 boundary, and analyzer runtime-boundary guidance.",
    "The same document now marks \u0060.NET Project Baseline\u0060 as the current pre-v0.33 net10.0 baseline, updates \u0060V1 Defaults\u0060 with the dual package-line contract, and defers multitargeting, matrix tests, verifier guidance, README work, release notes, and publication automation to sibling tickets.",
    "\u0060rg -n --glob \u0027*.csproj\u0027 \u0027\u003CPackageId\u003E|\u003CTargetFramework\u003E|\u003CTargetFrameworks\u003E|\u003CSuppressDependenciesWhenPacking\u003E\u0027 /mnt/c/Projects/DVault/src /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests -g \u0027*.csproj\u0027\u0060 showed the seven coordinated DVault package IDs and \u0060TargetFramework\u003Enet10.0\u0060 across the current src/test baseline.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0060 still uses opt-in provider conditions for MySQL/PostgreSQL/Oracle/SQL Server and references \u0060src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj\u0060 with \u0060OutputItemType=\u0022Analyzer\u0022\u0060, \u0060ReferenceOutputAssembly=\u0022false\u0022\u0060, and \u0060PrivateAssets=\u0022all\u0022\u0060.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj\u0060 still sets \u0060SuppressDependenciesWhenPacking=true\u0060 and packs analyzer assets under \u0060analyzers/dotnet/cs/\u0060, matching the local analyzer/source-generator asset boundary.",
    "\u0060.gicket/tickets/06F9GF2Z4Y7A91ZHG4NW1YTNMC/ticket.json\u0060 has \u0060\u0022status\u0022: \u0022done\u0022\u0060, matching the ticket\u0027s dependency on the completed v0.33 version-line policy task.",
    "\u0060git -C /mnt/c/Projects/DVault diff --check develop...90a2ece2157c -- docs/plans/shared-implementation-standards.md\u0060 returned no output.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/architecture, area/ef-core, area/packaging, area/provider-support, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F9G8EQJGBRSWE96VE028HJYW-story-define-net8-0-and-net10-0-compatibility-co\u0027.",
    "Ticket history references implementation commit \u002790a2ece2157c\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Proceed to the integrator gate; no tester rework is required for ticket \u006006F9G8EQJGBRSWE96VE028HJYW\u0060."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F9G8EQJGBRSWE96VE028HJYW`
- target-role: `integrator`
- verification-summary: Tester verified 7/7 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F9G8EQJGBRSWE96VE028HJYW-story-define-net8-0-and-net10-0-compatibility-co' at commit '90a2ece2157c'.
- acceptance-criteria: `7/7` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F9G8EQJGBRSWE96VE028HJYW-story-define-net8-0-and-net10-0-compatibility-co`
- implementation-commit: `90a2ece2157c`
- implementation-pr: `<none>`
- implementation-change: `<none>`