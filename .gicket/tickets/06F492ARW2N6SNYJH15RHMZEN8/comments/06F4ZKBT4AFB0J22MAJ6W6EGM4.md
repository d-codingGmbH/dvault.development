[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F492ARW2N6SNYJH15RHMZEN8\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in\u0027 and commit \u002755a5782224e3\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in\u0027 from source \u002755a5782224e3\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in\u0027.",
    "Evidence: git rev-parse --verify 55a5782224e3 resolved the claimed commit to 55a5782224e38720fbe278f0c84d4df36928eeb5.",
    "Evidence: git diff --name-status develop...55a5782224e3 shows the change set adds src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs, src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs, and updates src/DCoding.Data.DVault.Analyzers/README.md.",
    "Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:47-81 and 112-124 report DMV1910/DMV1911 based only on DbContext members or mutating invocations whose type is DbSet\u003CDictionary\u003Cstring, object\u003E\u003E; there is no additional check for DVault metadata, produced name, or table kind.",
    "Evidence: src/DCoding.Data.DVault.Analyzers/README.md:35-39 describes the analyzer scope as generated DVault hub/link/satellite boundaries and states that UseDataVaultSaveChangesMetadataInterceptor(...) remains an opt-in metadata filler for tracked generated rows.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveChangesMetadataInterceptorSqliteTests.cs:24-39 and 74-107 exercise direct Add(...) calls against generated shared-type tables after configuring UseDataVaultSaveChangesMetadataInterceptor(...).",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceRowGenerationTests.cs:59-76 performs direct Add(...) calls on SatCustomerProfile, SatCustomerStatu, and PitCustomerProfileStatus shared-type tables.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:116-125 only proves that IDataVaultSaveService.SaveAsync(...) and interceptor registration themselves are non-findings; it does not cover writes in the interceptor-backed lane or other shared-type tables.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in\u0027.",
    "Evidence: Ticket history references implementation commit \u002755a5782224e3\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Diagnostics do not fire on documented safe read/query usage of generated shared-type tables, including \u0060AsNoTracking()\u0060 and compiled-query read patterns over \u0060Set\u003CDictionary\u003Cstring, object\u003E\u003E(...)\u0060. (The implementation only reports exposed DbSet members and explicit mutating methods, and the added analyzer tests cover AsNoTracking() and compiled-query reads as non-findings.).",
    "AC check passed: Each new diagnostic ships with clear message, description, and remediation text; code fixes are included only where the fix is mechanical and safe. (EfCoreMisuseDiagnosticCatalog.cs defines ids, titles, messages, explanations, and remediation text for DMV1910 and DMV1911, and no unsafe code fix was added.).",
    "DoD check passed: New EF misuse diagnostics are implemented in \u0060src/DCoding.Data.DVault.Analyzers\u0060 and follow the existing DMV catalog conventions used by the package. (New EF misuse diagnostics were implemented in src/DCoding.Data.DVault.Analyzers and use the existing diagnostic metadata/catalog pattern already used by the package.).",
    "DoD check passed: The refined implementation keeps the analyzer package as optional developer tooling rather than turning it into a complete DVault model validator. (The implementation remains a bounded compile-time analyzer slice and does not attempt whole-application DI or full model validation.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: The analyzer package adds one or more new stable DMV diagnostics for EF Core misuse patterns that are statically obvious and specific to documented DVault invariants. (Stable DMV1910 and DMV1911 ids were added, but the analyzer identifies targets solely by DbSet\u003CDictionary\u003Cstring, object\u003E\u003E shape, so the diagnostics are not specific to documented DVault invariants.).",
    "AC check failed: The initial rule set includes at least unsupported/generated-table \u0060DbSet\u0060 exposure and obviously unsafe direct generated-table write patterns; any missing-registration or technical-metadata rules are limited to cases that are unambiguous from source. (The rule set includes DbSet exposure and direct-write checks, but the direct-write rule is not limited to unsupported generated-table patterns; it reports any mutating call on any DbSet\u003CDictionary\u003Cstring, object\u003E\u003E receiver.).",
    "AC check failed: Analyzer tests cover positive findings, non-findings for supported patterns, and regression cases around the explicit \u0060IDataVaultSaveService\u0060 boundary and opt-in \u0060UseDataVaultSaveChangesMetadataInterceptor(...)\u0060 lane. (The new analyzer tests cover positive findings, safe reads, IDataVaultSaveService, and interceptor registration, but they do not prove non-findings for supported writes in the opt-in interceptor lane or for other non-hub/link/satellite shared-type tables.).",
    "DoD check failed: Repository analyzer tests prove the intended trigger and non-trigger boundaries for every added diagnostic. (Repository analyzer tests were added, but they do not prove the intended non-trigger boundaries for every added diagnostic because supported interceptor-backed writes and other shared-type tables are not covered.).",
    "DoD check failed: Downstream documentation work can consume the final diagnostic ids and remediation text without reopening the analyzer scope. (Final diagnostic ids and remediation text exist, but the analyzer scope is still unsettled because current behavior conflicts with supported shared-type write patterns already exercised in the repository.).",
    "src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs is over-broad: DMV1910 and DMV1911 trigger on any DbSet\u003CDictionary\u003Cstring, object\u003E\u003E shape, so they can fire on non-DVault or non-hub/link/satellite shared-type entities instead of only on statically obvious DVault misuse.",
    "The implementation does not distinguish the documented opt-in UseDataVaultSaveChangesMetadataInterceptor(...) lane. Repository integration tests show supported direct Add(...) flows under that interceptor, but the new analyzer tests do not protect that boundary."
  ],
  "evidence": [
    "git rev-parse --verify 55a5782224e3 resolved the claimed commit to 55a5782224e38720fbe278f0c84d4df36928eeb5.",
    "git diff --name-status develop...55a5782224e3 shows the change set adds src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs, src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs, and updates src/DCoding.Data.DVault.Analyzers/README.md.",
    "src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:47-81 and 112-124 report DMV1910/DMV1911 based only on DbContext members or mutating invocations whose type is DbSet\u003CDictionary\u003Cstring, object\u003E\u003E; there is no additional check for DVault metadata, produced name, or table kind.",
    "src/DCoding.Data.DVault.Analyzers/README.md:35-39 describes the analyzer scope as generated DVault hub/link/satellite boundaries and states that UseDataVaultSaveChangesMetadataInterceptor(...) remains an opt-in metadata filler for tracked generated rows.",
    "tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveChangesMetadataInterceptorSqliteTests.cs:24-39 and 74-107 exercise direct Add(...) calls against generated shared-type tables after configuring UseDataVaultSaveChangesMetadataInterceptor(...).",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceRowGenerationTests.cs:59-76 performs direct Add(...) calls on SatCustomerProfile, SatCustomerStatu, and PitCustomerProfileStatus shared-type tables.",
    "tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:116-125 only proves that IDataVaultSaveService.SaveAsync(...) and interceptor registration themselves are non-findings; it does not cover writes in the interceptor-backed lane or other shared-type tables.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in\u0027.",
    "Ticket history references implementation commit \u002755a5782224e3\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Narrow DMV1910 and DMV1911 so they require DVault-specific evidence of generated hub/link/satellite tables, or explicitly exempt supported shared-type flows such as the interceptor-backed lane and PIT maintenance patterns.",
    "Add analyzer regression tests for supported interceptor-backed direct writes and for non-hub/link/satellite shared-type entities so the intended non-trigger boundaries are proved in the analyzer test project.",
    "After the rule scope is corrected, rerun analyzer and repository verification, including dotnet test tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj --nologo, dotnet test DVault.slnx --nologo, and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in",
  "commitSha": "55a5782224e3"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F492ARW2N6SNYJH15RHMZEN8`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in`