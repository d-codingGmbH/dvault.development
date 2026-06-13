[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FBSBWPN112S4CGP0239K0ZT8\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FBSBWPN112S4CGP0239K0ZT8-task-document-v0-37-dependency-and-analyzer-comp\u0027 and commit \u0027b848d2354b7e\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FBSBWPN112S4CGP0239K0ZT8-task-document-v0-37-dependency-and-analyzer-comp\u0027 from source \u0027b848d2354b7e\u0027.",
    "Prompt-injection writeback protection rejected tester interactive assessment writeback on attempt 1/2; retrying tester output generation before creating a durable stop.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FBSBWPN112S4CGP0239K0ZT8-task-document-v0-37-dependency-and-analyzer-comp\u0027.",
    "Evidence: \u0060git diff --stat develop...b848d2354b7e\u0060 shows \u0060README.md\u0060, \u0060CHANGELOG.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, new \u0060docs/releases/v0.37.0.md\u0060, and a BOM-only change to \u0060docs/plans/shared-implementation-standards.md\u0060 in the claimed branch state.",
    "Evidence: \u0060git ls-tree -r --name-only b848d2354b7e -- docs/releases/v0.37.0.md README.md CHANGELOG.md docs/manual-nuget-publication.md\u0060 lists all four expected documentation surfaces at the claimed commit.",
    "Evidence: \u0060README.md:124-137\u0060, \u0060CHANGELOG.md:5-20\u0060, and \u0060docs/releases/v0.37.0.md:23-70\u0060 all document the \u00608.36.0\u0060/\u0060net8.0\u0060 and \u006010.36.0\u0060/\u0060net10.0\u0060 baseline, the exact dependency matrix, the analyzer \u0060.NET 10 SDK\u0060 host boundary, and the carried-forward validation commands.",
    "Evidence: \u0060tools/pack-release-packages.sh:57-58\u0060 still packs \u00608.36.0\u0060 for \u0060net8.0\u0060 and \u006010.36.0\u0060 for \u0060net10.0\u0060; \u0060src/DCoding.Data.DVault/DCoding.Data.DVault.csproj\u0060, \u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs\u0060, and \u0060tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0060 match the documented version matrix.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj:3\u0060 targets \u0060net10.0\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:46\u0060 references the analyzer with \u0060PrivateAssets=all\u0060 and \u0060SetTargetFramework=TargetFramework=net10.0\u0060.",
    "Evidence: \u0060git diff --check develop...b848d2354b7e -- README.md CHANGELOG.md docs/manual-nuget-publication.md docs/releases/v0.37.0.md docs/plans/shared-implementation-standards.md\u0060 returned no output.",
    "Evidence: \u0060docs/manual-nuget-publication.md:95\u0060 says \u0060v0.37.0\u0060 is the release-note/planning Git tag.",
    "Evidence: \u0060git tag --list \u0027v0.37.0\u0027\u0060 returned no matches in the local repository.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/documentation, area/ef-core, area/packaging, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06FBSBWPN112S4CGP0239K0ZT8-task-document-v0-37-dependency-and-analyzer-comp\u0027.",
    "Evidence: Ticket history references implementation commit \u0027b848d2354b7e\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: The v0.37 guidance records the exact current accepted dependency matrix from repo-visible evidence: \u0060net8.0\u0060 uses EF/Relational \u00608.0.28\u0060, DI.Abstractions \u00608.0.2\u0060, DB2 \u00608.0.0.400\u0060, SQLite \u00608.0.28\u0060, MySQL \u00608.0.26\u0060, PostgreSQL \u00608.0.11\u0060, Oracle \u00608.23.26200\u0060, SQL Server \u00608.0.28\u0060; \u0060net10.0\u0060 uses EF/Relational/DI.Abstractions \u006010.0.9\u0060, DB2 \u006010.0.0.100\u0060, SQLite \u006010.0.9\u0060, MySQL \u006010.0.7\u0060, PostgreSQL \u006010.0.2\u0060, Oracle \u006010.23.26200\u0060, SQL Server \u006010.0.9\u0060. (The delivered docs use the exact \u0060net8.0\u0060 and \u0060net10.0\u0060 dependency matrix shown by \u0060src/DCoding.Data.DVault/DCoding.Data.DVault.csproj\u0060, \u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs\u0060, and \u0060tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0060.).",
    "AC check passed: The v0.37 guidance explicitly carries forward the accepted analyzer compatibility boundary: \u0060DCoding.Data.DVault.Analyzers\u0060 ships one \u0060net10.0\u0060 analyzer asset, analyzer references stay local with \u0060PrivateAssets=all\u0060, and supported analyzer consumption for both coordinated consumer lines uses a \u0060.NET 10 SDK\u0060 build host without claiming validated pure \u0060.NET 8 SDK\u0060 analyzer consumption. (\u0060README.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, \u0060CHANGELOG.md\u0060, and \u0060docs/releases/v0.37.0.md\u0060 all carry forward the one-asset \u0060net10.0\u0060 analyzer boundary, local \u0060PrivateAssets=all\u0060 guidance, and the \u0060.NET 10 SDK\u0060 build-host requirement without claiming validated pure \u0060.NET 8 SDK\u0060 analyzer consumption.).",
    "AC check passed: The v0.37 release record and manual publication guidance point to the current validation evidence surfaces and commands already used in-repo: \u0060dotnet build DVault.slnx --nologo\u0060, \u0060dotnet test DVault.slnx --nologo\u0060, \u0060bash tools/pack-release-packages.sh\u0060, \u0060bash tools/verify-packages.sh\u0060, and \u0060bash tools/check-format.sh\u0060, plus the analyzer audit and matrix/verifier evidence paths. (\u0060docs/releases/v0.37.0.md\u0060 records the required validation commands and points to the matrix, verifier, and analyzer-audit evidence paths, while \u0060docs/manual-nuget-publication.md\u0060 repeats the same command baseline for manual publication.).",
    "AC check passed: No in-scope current-baseline surface leaves a stale \u0060v0.36.0\u0060 dependency/analyzer baseline where the new \u0060v0.37.0\u0060 record should be authoritative, while historical \u0060v0.36.0\u0060 material may remain only as carried-forward background or release history. (The in-scope docs move the authoritative current baseline to v0.37.0; remaining \u0060v0.36.0\u0060 mentions are historical carry-forward references in release history or prior hash-key guidance, not competing current dependency/analyzer baseline text.).",
    "DoD check passed: \u0060docs/releases/v0.37.0.md\u0060 exists and is the current release record linked by the updated README and changelog surfaces for dependency-line/analyzer baseline guidance. (\u0060docs/releases/v0.37.0.md\u0060 exists in the claimed branch state and is linked from \u0060README.md\u0060 and \u0060CHANGELOG.md\u0060 as the current release record.).",
    "DoD check passed: The refined ticket leaves no PO-level ambiguity about consumer package versions: the visible baseline stays \u00608.36.0\u0060 / \u006010.36.0\u0060 unless a separate packaging change lands, and the ticket does not imply an unproved \u00608.37.0\u0060 / \u006010.37.0\u0060 line. (The delivered docs consistently keep \u00608.36.0\u0060 and \u006010.36.0\u0060 as the visible consumer package lines and explicitly reject \u00600.37.0\u0060, \u00608.37.0\u0060, and \u006010.37.0\u0060 consumer package versions.).",
    "DoD check passed: Downstream checklist work in \u006006FBSBWW414TE19KZT14CB7Y3R\u0060 can consume the v0.37 baseline without reopening dependency policy or analyzer compatibility decisions. (The v0.37 baseline documents the settled dependency-line and analyzer compatibility decisions clearly enough for downstream checklist work; the blocking defect is the unsupported Git tag claim, not reopened dependency or analyzer policy.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: \u0060README.md\u0060, \u0060CHANGELOG.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, and new \u0060docs/releases/v0.37.0.md\u0060 all present one consistent current-baseline story: planning label \u0060v0.37.0\u0060, consumer package lines \u00608.36.0\u0060 for \u0060net8.0\u0060 / EF Core 8 and \u006010.36.0\u0060 for \u0060net10.0\u0060 / EF Core 10, and no mixed-line consumer install or approval guidance. (\u0060README.md\u0060, \u0060CHANGELOG.md\u0060, and \u0060docs/releases/v0.37.0.md\u0060 frame \u0060v0.37.0\u0060 as the planning/current-baseline label, but \u0060docs/manual-nuget-publication.md:95\u0060 additionally says \u0060v0.37.0\u0060 is a Git tag; \u0060git tag --list \u0027v0.37.0\u0027\u0060 returned no match, so the four docs are not fully consistent on the current-baseline story.).",
    "DoD check failed: All four in-scope docs agree on the same package lines, target-specific dependency matrix, analyzer build-host boundary, manual-publication posture, and validation evidence with no contradictory wording. (The four docs do not fully agree on manual-publication posture because \u0060docs/manual-nuget-publication.md:95\u0060 asserts a \u0060v0.37.0\u0060 Git tag exists, while the other docs use planning-label wording and the local repository has no \u0060v0.37.0\u0060 tag.).",
    "\u0060docs/manual-nuget-publication.md:95\u0060 makes an unsupported repository claim by stating that \u0060v0.37.0\u0060 exists as a Git tag. The other updated docs describe \u0060v0.37.0\u0060 as the planning/current-baseline label, and \u0060git tag --list \u0027v0.37.0\u0027\u0060 returned no tag. This leaves the manual publication guidance inconsistent with the observed repository state."
  ],
  "evidence": [
    "\u0060git diff --stat develop...b848d2354b7e\u0060 shows \u0060README.md\u0060, \u0060CHANGELOG.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, new \u0060docs/releases/v0.37.0.md\u0060, and a BOM-only change to \u0060docs/plans/shared-implementation-standards.md\u0060 in the claimed branch state.",
    "\u0060git ls-tree -r --name-only b848d2354b7e -- docs/releases/v0.37.0.md README.md CHANGELOG.md docs/manual-nuget-publication.md\u0060 lists all four expected documentation surfaces at the claimed commit.",
    "\u0060README.md:124-137\u0060, \u0060CHANGELOG.md:5-20\u0060, and \u0060docs/releases/v0.37.0.md:23-70\u0060 all document the \u00608.36.0\u0060/\u0060net8.0\u0060 and \u006010.36.0\u0060/\u0060net10.0\u0060 baseline, the exact dependency matrix, the analyzer \u0060.NET 10 SDK\u0060 host boundary, and the carried-forward validation commands.",
    "\u0060tools/pack-release-packages.sh:57-58\u0060 still packs \u00608.36.0\u0060 for \u0060net8.0\u0060 and \u006010.36.0\u0060 for \u0060net10.0\u0060; \u0060src/DCoding.Data.DVault/DCoding.Data.DVault.csproj\u0060, \u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs\u0060, and \u0060tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0060 match the documented version matrix.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj:3\u0060 targets \u0060net10.0\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:46\u0060 references the analyzer with \u0060PrivateAssets=all\u0060 and \u0060SetTargetFramework=TargetFramework=net10.0\u0060.",
    "\u0060git diff --check develop...b848d2354b7e -- README.md CHANGELOG.md docs/manual-nuget-publication.md docs/releases/v0.37.0.md docs/plans/shared-implementation-standards.md\u0060 returned no output.",
    "\u0060docs/manual-nuget-publication.md:95\u0060 says \u0060v0.37.0\u0060 is the release-note/planning Git tag.",
    "\u0060git tag --list \u0027v0.37.0\u0027\u0060 returned no matches in the local repository.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/documentation, area/ef-core, area/packaging, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FBSBWPN112S4CGP0239K0ZT8-task-document-v0-37-dependency-and-analyzer-comp\u0027.",
    "Ticket history references implementation commit \u0027b848d2354b7e\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Replace the \u0060v0.37.0\u0060 Git tag wording in \u0060docs/manual-nuget-publication.md\u0060 with planning-label or release-note wording that matches \u0060README.md\u0060 and \u0060docs/releases/v0.37.0.md\u0060, unless a real \u0060v0.37.0\u0060 tag is intentionally added as separate repository evidence.",
    "After that wording is corrected, rerun tester review on the updated branch state."
  ],
  "branchName": "ticket/06FBSBWPN112S4CGP0239K0ZT8-task-document-v0-37-dependency-and-analyzer-comp",
  "commitSha": "b848d2354b7e"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FBSBWPN112S4CGP0239K0ZT8`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FBSBWPN112S4CGP0239K0ZT8-task-document-v0-37-dependency-and-analyzer-comp`