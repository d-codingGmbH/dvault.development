[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 3/3 definition-of-done expectations on branch \u0027ticket/06FBSBWW414TE19KZT14CB7Y3R-task-prepare-v0-37-release-checklist-and-validat\u0027 at commit \u00271f0fcad911f7\u0027.",
  "implementationReference": {
    "branchName": "ticket/06FBSBWW414TE19KZT14CB7Y3R-task-prepare-v0-37-release-checklist-and-validat",
    "commitSha": "1f0fcad911f7",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "The release-note material states that \u0060v0.37.0\u0060 is a planning/release-note label, not a consumer NuGet version, and explicitly forbids consumer-facing \u00600.37.0\u0060, \u00608.37.0\u0060, or \u006010.37.0\u0060 versions for this baseline.",
      "satisfied": true,
      "reason": "\u0060docs/releases/v0.37.0.md\u0060, \u0060README.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, and \u0060CHANGELOG.md\u0060 all state that \u0060v0.37.0\u0060 is a planning/release-note label, keep the visible consumer lines at \u00608.36.0\u0060/\u0060net8.0\u0060 and \u006010.36.0\u0060/\u0060net10.0\u0060, and forbid consumer-facing \u00600.37.0\u0060, \u00608.37.0\u0060, and \u006010.37.0\u0060."
    },
    {
      "expectation": "The release-note material records the exact dual package lines and target-specific dependency matrix already enforced by repository tests and package verification.",
      "satisfied": true,
      "reason": "The release-note material records the exact \u0060net8.0\u0060 and \u0060net10.0\u0060 dependency matrix, and those versions match the pack script, the version-matrix test, and the package-verifier enforcement surfaces."
    },
    {
      "expectation": "The release-note material states that \u0060DCoding.Data.DVault.Analyzers\u0060 remains one \u0060net10.0\u0060 analyzer asset for both package lines, requires \u0060PrivateAssets=\u0022all\u0022\u0060, and is supported on a \u0060.NET 10 SDK\u0060 build host for both lines.",
      "satisfied": true,
      "reason": "The release-note, README, and manual publication checklist state that \u0060DCoding.Data.DVault.Analyzers\u0060 stays one \u0060net10.0\u0060 analyzer asset, analyzer references stay local with \u0060PrivateAssets=\u0022all\u0022\u0060, and both package lines use a \u0060.NET 10 SDK\u0060 build host; the analyzer project targets \u0060net10.0\u0060 and packs under \u0060analyzers/dotnet/cs/\u0060."
    },
    {
      "expectation": "The release closure guidance includes the five required validation commands: \u0060dotnet build DVault.slnx --nologo\u0060, \u0060dotnet test DVault.slnx --nologo\u0060, \u0060bash tools/pack-release-packages.sh\u0060, \u0060bash tools/verify-packages.sh\u0060, and \u0060bash tools/check-format.sh\u0060.",
      "satisfied": true,
      "reason": "\u0060docs/releases/v0.37.0.md\u0060, \u0060docs/local-validation.md\u0060, \u0060README.md\u0060, and \u0060docs/manual-nuget-publication.md\u0060 all list the same five required validation commands: build, test, pack, verify-packages, and check-format."
    },
    {
      "expectation": "The release closure guidance states the known limits: no mixed package lines in one consumer example or approval, no pure \u0060.NET 8 SDK\u0060 analyzer compatibility claim, and no package publication evidence inside the release-note artifact itself.",
      "satisfied": true,
      "reason": "The release-note material states the known limits: no mixed \u00608.36.0\u0060/\u006010.36.0\u0060 line in one consumer example or approval, no pure \u0060.NET 8 SDK\u0060 analyzer compatibility claim, and no package publication evidence inside the \u0060v0.37.0\u0060 release-note artifact."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The current repository release surfaces (\u0060docs/releases/v0.37.0.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, \u0060docs/local-validation.md\u0060, \u0060README.md\u0060, and \u0060CHANGELOG.md\u0060) tell one consistent v0.37 story for package scope, version lines, analyzer boundary, validation, and non-goals.",
      "satisfied": true,
      "reason": "The named release surfaces already tell one consistent v0.37 story for eight-package scope, dual package lines, analyzer boundary, validation lane, and non-goals; no required output path diverges from that baseline."
    },
    {
      "expectation": "The exact dependency matrix is ratified against existing repository enforcement, including \u0060tools/pack-release-packages.sh\u0060, \u0060src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs\u0060.",
      "satisfied": true,
      "reason": "The documented dependency matrix is ratified by \u0060tools/pack-release-packages.sh\u0060, \u0060src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs\u0060, and matching package-verifier checks."
    },
    {
      "expectation": "A release operator can use the documented checklist and validation commands for later closure without reopening package-line, analyzer-host, or limitation decisions in this ticket.",
      "satisfied": true,
      "reason": "A release operator can follow the documented checklist and validation commands in the release note, README, local validation note, and manual publication checklist without reopening package-line, analyzer-host, or limitation decisions in this ticket."
    }
  ],
  "evidence": [
    "\u0060git diff --name-only develop...1f0fcad911f7\u0060 only listed \u0060.gicket/tickets/06FBSBWW414TE19KZT14CB7Y3R/**\u0060; the same diff limited to \u0060docs/manual-nuget-publication.md\u0060, \u0060docs/local-validation.md\u0060, \u0060README.md\u0060, \u0060CHANGELOG.md\u0060, \u0060tools/pack-release-packages.sh\u0060, \u0060src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs\u0060, and \u0060docs/releases/v0.37.0.md\u0060 returned no paths.",
    "\u0060git diff --name-only 1f0fcad911f7..HEAD\u0060 on those repository surfaces returned no paths, and \u0060git ls-tree -r --name-only 1f0fcad911f7\u0060 includes all required outputs plus \u0060DVault.slnx\u0060, \u0060tools/check-format.sh\u0060, and \u0060tools/verify-packages.sh\u0060.",
    "\u0060docs/releases/v0.37.0.md:23-30\u0060, \u0060README.md:18\u0060, \u0060docs/manual-nuget-publication.md:22-29\u0060, and \u0060CHANGELOG.md:7-8\u0060 all state the \u0060v0.37.0\u0060 planning-label rule, the \u00608.36.0\u0060/\u0060net8.0\u0060 and \u006010.36.0\u0060/\u0060net10.0\u0060 consumer lines, and the forbidden \u00600.37.0\u0060, \u00608.37.0\u0060, and \u006010.37.0\u0060 package versions.",
    "\u0060docs/releases/v0.37.0.md:38-49\u0060, \u0060README.md:132-135\u0060, and \u0060docs/manual-nuget-publication.md:88-91\u0060 align on the exact dependency matrix and single \u0060net10.0\u0060 analyzer asset with local \u0060PrivateAssets=\u0022all\u0022\u0060 references and a \u0060.NET 10 SDK\u0060 build host; \u0060src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj:3,45-46\u0060 targets \u0060net10.0\u0060 and packs analyzer assets under \u0060analyzers/dotnet/cs/\u0060.",
    "\u0060docs/releases/v0.37.0.md:56-63\u0060, \u0060docs/local-validation.md:6-21\u0060, \u0060README.md:185-192\u0060, and \u0060docs/manual-nuget-publication.md:73-77\u0060 and \u0060110-117\u0060 list the five validation commands and describe their role in later release closure.",
    "\u0060tools/pack-release-packages.sh:57-58\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs:16-55\u0060, and \u0060tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:28-29\u0060 and \u0060738-755\u0060 enforce the same \u00608.36.0\u0060/\u006010.36.0\u0060 package lines and target-specific dependency versions named in the documentation.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/documentation, area/packaging, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FBSBWW414TE19KZT14CB7Y3R-task-prepare-v0-37-release-checklist-and-validat\u0027.",
    "Ticket history references implementation commit \u00271f0fcad911f7\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Latest developer delivery outcome declares \u0027already_satisfied_on_branch\u0027.",
    "Developer delivery outcome reason: The current branch already contains the required documentation/checklist baseline at explicit repository-relative paths. The ticket contract is satisfied by existing release documentation, README/changelog/local-validation alignment, analyzer package metadata, pack-script package lines, and dependency-matrix enforcement; adding another repository artifact would duplicate existing authoritative surfaces..",
    "Developer delivery outcome targets role \u0027test\u0027.",
    "Observed behavior: developer verified that the checked-out branch already satisfied the required repository state without creating a new implementation commit.",
    "Developer delivery evidence: docs/releases/v0.37.0.md:23-30 states v0.37.0 is not a consumer NuGet version, lists 8.36.0/net8.0 and 10.36.0/net10.0, and forbids 0.37.0, 8.37.0, and 10.37.0 consumer package versions.",
    "Developer delivery evidence: README.md:18-44, docs/manual-nuget-publication.md:22-35, and CHANGELOG.md:7-18 carry the same package-line separation, forbidden-version guidance, analyzer PrivateAssets guidance, and validation story.",
    "Developer delivery evidence: docs/releases/v0.37.0.md:38-39, README.md:132-133, docs/manual-nuget-publication.md:88-89, and CHANGELOG.md:13-14 contain the exact EF/provider dependency matrix for net8.0 and net10.0.",
    "Developer delivery evidence: docs/releases/v0.37.0.md:45-49 and README.md:44 document one net10.0 analyzer asset, local PrivateAssets=\u0022all\u0022 references, .NET 10 SDK host support for both package lines, and no pure .NET 8 SDK analyzer claim.",
    "Developer delivery evidence: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj:3 targets net10.0 and lines 45-46 package analyzer assets under analyzers/dotnet/cs/.",
    "Developer delivery evidence: docs/local-validation.md:6-10, docs/manual-nuget-publication.md:73-77, README.md:185-189, CHANGELOG.md:17, and docs/releases/v0.37.0.md:56-60 list the five required validation commands.",
    "Developer delivery evidence: tools/pack-release-packages.sh:57-58 packs 8.36.0 for net8.0 and 10.36.0 for net10.0.",
    "Developer delivery evidence: tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs:16-55 and tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:738-755 enforce the dependency versions named in the release checklist.",
    "Developer verification hint: Run git grep for v0.37.0, 8.36.0, 10.36.0, 0.37.0, 8.37.0, and 10.37.0 across docs/releases/v0.37.0.md, docs/manual-nuget-publication.md, README.md, CHANGELOG.md, and docs/local-validation.md to confirm the package-line story remains aligned.",
    "Developer verification hint: Run git grep for PrivateAssets, net10.0, .NET 10 SDK, and analyzers/dotnet/cs across the release docs, README, analyzer README, and src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj.",
    "Developer verification hint: Run git grep for pack_line \u00228.36.0\u0022 \u0022net8.0\u0022 and pack_line \u002210.36.0\u0022 \u0022net10.0\u0022 in tools/pack-release-packages.sh.",
    "Developer verification hint: For full release-lane validation, run dotnet build DVault.slnx --nologo, dotnet test DVault.slnx --nologo, bash tools/pack-release-packages.sh, bash tools/verify-packages.sh, and bash tools/check-format.sh from the repository root."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator.",
    "During actual release closure, run the documented five-command validation lane and record the final approval record before any package push."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06FBSBWW414TE19KZT14CB7Y3R`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 3/3 definition-of-done expectations on branch 'ticket/06FBSBWW414TE19KZT14CB7Y3R-task-prepare-v0-37-release-checklist-and-validat' at commit '1f0fcad911f7'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `3/3` satisfied
- implementation-branch: `ticket/06FBSBWW414TE19KZT14CB7Y3R-task-prepare-v0-37-release-checklist-and-validat`
- implementation-commit: `1f0fcad911f7`
- implementation-pr: `<none>`
- implementation-change: `<none>`